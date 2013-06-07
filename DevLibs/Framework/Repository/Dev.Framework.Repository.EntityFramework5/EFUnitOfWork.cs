﻿// ***********************************************************************************
// Created by zbw911 
// 创建于：2012年12月21日 16:28
// 
// 修改于：2013年02月18日 18:24
// 文件名：EFUnitOfWork.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************
using System;
using System.Collections.Generic;
using Kt.Framework.Repository.Extensions;

namespace Kt.Framework.Repository.Data.EntityFramework5
{
    /// <summary>
    ///     Implements the <see cref="IUnitOfWork" /> interface to provide an implementation
    ///     of a IUnitOfWork that uses Entity Framework to query and update the underlying store.
    /// </summary>
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly IEFSessionResolver _resolver;
        private bool _disposed;

        private IDictionary<Guid, IEFSession> _openSessions = new Dictionary<Guid, IEFSession>();

        /// <summary>
        ///     Default Constructor.
        ///     Creates a new instance of the <see cref="EFUnitOfWork" /> class that uses the specified object context.
        /// </summary>
        /// <param name="resolver">
        ///     An instance of <see cref="EFUnitOfWorkSettings" /> that contains settings for
        ///     Entity Framework unit of work instances.
        /// </param>
        public EFUnitOfWork(IEFSessionResolver resolver)
        {
            Guard.Against<ArgumentNullException>(resolver == null,
                                                 "Expected a non-null EFUnitOfWorkSettings instance.");
            _resolver = resolver;
        }

        /// <summary>
        ///     Flushes the changes made in the unit of work to the data store.
        /// </summary>
        public void Flush()
        {
            Guard.Against<ObjectDisposedException>(_disposed,
                                                   "The current EFUnitOfWork instance has been disposed. " +
                                                   "Cannot get sessions from a disposed UnitOfWork instance.");

            _openSessions.ForEach(session => session.Value.SaveChanges());
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Gets a <see cref="IEFSession" /> that can be used to query and update the specified type.
        /// </summary>
        /// <typeparam name="T">
        ///     The type for which an <see cref="IEFSession" /> should be returned.
        /// </typeparam>
        /// <returns>
        ///     An <see cref="IEFSession" /> that can be used to query and update the specified type.
        /// </returns>
        public IEFSession GetSession<T>()
        {
            Guard.Against<ObjectDisposedException>(_disposed,
                                                   "The current EFUnitOfWork instance has been disposed. " +
                                                   "Cannot get sessions from a disposed UnitOfWork instance.");

            Guid sessionKey = _resolver.GetSessionKeyFor<T>();
            if (_openSessions.ContainsKey(sessionKey))
                return _openSessions[sessionKey];

            //Opening a new session...
            IEFSession session = _resolver.OpenSessionFor<T>();
            _openSessions.Add(sessionKey, session);
            return session;
        }

        /// <summary>
        ///     Disposes off the managed and unmanaged resources used.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                if (_openSessions != null && _openSessions.Count > 0)
                {
                    _openSessions.ForEach(session => session.Value.Dispose());
                    _openSessions.Clear();
                }
            }
            _openSessions = null;
            _disposed = true;
        }
    }
}