﻿// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月30日 11:27
// 
// 修改于：2013年02月05日 17:28
// 文件名：DbContextManager.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.Data
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    #endregion

    public class DbContextManager
    {
        #region Static Fields

        /// <summary>
        ///     The default connection string name used if only one database is being communicated with.
        /// </summary>
        public static readonly string DefaultConnectionStringName = "DefaultConnection";

        /// <summary>
        ///     Maintains a dictionary of db context builders, one per database.  The key is a
        ///     connection string name used to look up the associated database, and used to decorate respective
        ///     repositories. If only one database is being used, this dictionary contains a single
        ///     factory with a key of <see cref="DefaultConnectionStringName" />.
        /// </summary>
        //private static readonly Dictionary<string, IDbContextBuilder<DbContext>> _dbContextBuilders =
        //    new Dictionary<string, IDbContextBuilder<DbContext>>();
        private static readonly IDictionary<string, Func<DbContext>> _DbContexts =
            new Dictionary<string, Func<DbContext>>();

        private static readonly object _syncLock = new object();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Used to get the current db context session if you're communicating with a single database.
        ///     When communicating with multiple databases, invoke <see cref="CurrentFor()" /> instead.
        /// </summary>
        public static DbContext Current
        {
            get
            {
                return CurrentFor(DefaultConnectionStringName);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     An application-specific implementation of IDbContextStorage must be setup either thru
        ///     <see cref="InitStorage" /> or one of the <see cref="Init" /> overloads.
        /// </summary>
        private static IDbContextStorage _storage { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     This method is used by application-specific db context storage implementations
        ///     and unit tests. Its job is to walk thru existing cached object context(s) and Close() each one.
        /// </summary>
        public static void CloseAllDbContexts()
        {
            foreach (DbContext ctx in _storage.GetAllDbContexts())
            {
                if (((IObjectContextAdapter)ctx).ObjectContext.Connection.State == ConnectionState.Open)
                {
                    ((IObjectContextAdapter)ctx).ObjectContext.Connection.Close();
                }
            }
        }

        /// <summary>
        ///     Used to get the current DbContext associated with a key; i.e., the key
        ///     associated with an object context for a specific database.
        ///     If you're only communicating with one database, you should call <see cref="Current" /> instead,
        ///     although you're certainly welcome to call this if you have the key available.
        /// </summary>
        public static DbContext CurrentFor(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            if (_storage == null)
            {
                throw new ApplicationException("An IDbContextStorage has not been initialized");
            }

            DbContext context = null;
            lock (_syncLock)
            {
                if (!_DbContexts.ContainsKey(key))
                {
                    throw new ApplicationException("An DbContextBuilder does not exist with a key of " + key);
                }

                context = _storage.GetDbContextForKey(key);

                if (context == null)
                {
                    context = _DbContexts[key]();
                    _storage.SetDbContextForKey(key, context);
                }
            }

            return context;
        }

        public static void Init(
            string[] mappingAssemblies, bool recreateDatabaseIfExist = false, bool lazyLoadingEnabled = true)
        {
            Init(DefaultConnectionStringName, mappingAssemblies, recreateDatabaseIfExist, lazyLoadingEnabled);
        }

        public static void Init(
            string connectionStringName,
            string[] mappingAssemblies,
            bool recreateDatabaseIfExist = false,
            bool lazyLoadingEnabled = true)
        {
            AddConfiguration(connectionStringName, mappingAssemblies, recreateDatabaseIfExist, lazyLoadingEnabled);
        }

        public static void Init<T>(string connectionStringName) where T : DbContext
        {
            if (string.IsNullOrEmpty(connectionStringName))
            {
                throw new ArgumentNullException("connectionStringName");
            }

            _DbContexts.Add(
                connectionStringName,
                () =>
                {
                    //new DbContext(connectionStringName);//
                    var context = (T)Activator.CreateInstance(typeof(T), connectionStringName);


                    //context.Database.Connection.ConnectionString =
                    //    ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;



                    return context;
                });

            //_dbContextBuilders.Add(connectionStringName, new CommDbContextBuilder<T>(connectionStringName));
        }

        /// <summary>
        ///     初始化存储
        /// </summary>
        /// <param name="storage"></param>
        public static void InitStorage(IDbContextStorage storage)
        {
            if (storage == null)
            {
                throw new ArgumentNullException("storage");
            }
            if ((_storage != null) && (_storage != storage))
            {
                throw new ApplicationException("A storage mechanism has already been configured for this application");
            }
            _storage = storage;
        }

        #endregion

        #region Methods

        private static void AddConfiguration(
            string connectionStringName,
            string[] mappingAssemblies,
            bool recreateDatabaseIfExists = false,
            bool lazyLoadingEnabled = true)
        {
            if (string.IsNullOrEmpty(connectionStringName))
            {
                throw new ArgumentNullException("connectionStringName");
            }

            if (mappingAssemblies == null)
            {
                throw new ArgumentNullException("mappingAssemblies");
            }

            lock (_syncLock)
            {
                //_dbContextBuilders.Add(
                //    connectionStringName,
                //    new DbContextBuilder<DbContext>(
                //        connectionStringName, mappingAssemblies, recreateDatabaseIfExists, lazyLoadingEnabled));

                _DbContexts.Add(
                    connectionStringName,
                    () =>
                    {
                        var builder = new DbContextBuilder<DbContext>(
                            connectionStringName, mappingAssemblies, recreateDatabaseIfExists, lazyLoadingEnabled);
                        return builder.BuildDbContext();
                    });
            }
        }

        #endregion
    }
}