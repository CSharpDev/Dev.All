using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Application.Config
{
    [DataContract]
    public class CommConfiguration
    {
        private static CommConfiguration _config;
        private static IConfigurationStorage _configProvider;

        [XmlIgnore]
        public static IConfigurationStorage ConfigProvider
        {
            get
            {
                if (_configProvider == null)
                    _configProvider = new XmlConfigurationStorage();
                return _configProvider;
            }
            set { _configProvider = value; }
        }

        public static CommConfiguration Config
        {
            get
            {

                if (_config == null)
                    _config = ConfigProvider.Get();
                return _config;
            }
            set
            {
                _config = value;
                ConfigProvider.Save(_config);
            }
        }


        /// <summary>
        /// Ĭ�ϵ���ҳ��
        /// </summary>
        [DataMember]
        public string CurrentUrl { get; set; }
        /// <summary>
        /// ���Žӿڵ�ַ
        /// </summary>
        [DataMember]
        public string SmsApi;
        /// <summary>
        /// ��Ϸ���ṩ��API
        /// </summary>
        [DataMember]
        public string TuanApibase { get; set; }
    }
}