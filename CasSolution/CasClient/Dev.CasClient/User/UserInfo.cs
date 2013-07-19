using System;
using System.Collections.Generic;

namespace Dev.CasClient.User
{
    /// <summary>
    /// </summary>
    public class UserInfo
    {
        #region Class Methods

        /// <summary>
        ///   ����ǳ��Ƿ����
        /// </summary>
        /// <param name="nickname"> </param>
        /// <returns> </returns>
        public static bool CheckNick(string nickname)
        {
            var url = Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl
                      + "/api/User/CheckNick?nickname=";

            url += nickname;

            var result = Dev.Comm.Net.Http.GetUrl(url);

            return Dev.Comm.JsonConvert.ToJsonObject<bool>(result);
        }

        /// <summary>
        ///   ȡ���û���ע��ʱ�䷽����null��ʾ�û�������
        /// </summary>
        /// <param name="uid"> </param>
        /// <returns> </returns>
        public static DateTime? GetRegDateTime(decimal uid)
        {
            var url = Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl
                      + "/api/User/GetRegDateTime?uid=";

            url += uid;

            var result = Dev.Comm.Net.Http.GetUrl(url);
            var time = Dev.Comm.JsonConvert.ToJsonObject<DateTime?>(result);

            return time;
        }

        /// <summary>
        ///   �����û�UIDȡ���û���Ϣ
        /// </summary>
        /// <param name="uid"> </param>
        /// <returns> </returns>
        public static UserProfileModel GetUserInfo(decimal uid)
        {
            var url = Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl
                      + "/api/User/GetUserInfo?uid=";

            url += uid;

            var result = Dev.Comm.Net.Http.GetUrl(url);

            return Dev.Comm.JsonConvert.ToJsonObject<UserProfileModel>(result);
        }


        /// <summary>
        ///   �����û��ǳ�ȡ���û���Ϣ
        /// </summary>
        /// <param name="nickname"> </param>
        /// <returns> </returns>
        public static UserProfileModel GetUserInfoByNickname(string nickname)
        {
            var url = Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl
                      + "/api/User/GetUserInfoByNickname?nickname=" + nickname;

            var result = Dev.Comm.Net.Http.GetUrl(url);

            return Dev.Comm.JsonConvert.ToJsonObject<UserProfileModel>(result);
        }


        /// <summary>
        ///   �����û���ȡ���û���Ϣ
        /// </summary>
        /// <param name="username"> </param>
        /// <returns> </returns>
        public static UserProfileModel GetUserInfoByUserName(string username)
        {
            var url = Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl
                      + "/api/User/GetUserInfoByUserName?username=";

            url += username;

            var result = Dev.Comm.Net.Http.GetUrl(url);

            return Dev.Comm.JsonConvert.ToJsonObject<UserProfileModel>(result);
        }

        /// <summary>
        ///   �����û�UIDȡ���û���Ϣ
        /// </summary>
        /// <param name="uids"> </param>
        /// <returns> </returns>
        public static List<UserProfileModel> GetUserInfoList(decimal[] uids)
        {
            var url = Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl
                      + "/api/User/GetUserInfoList?uids=";

            url += string.Join("&uids=", uids);

            var result = Dev.Comm.Net.Http.GetUrl(url);

            return Dev.Comm.JsonConvert.ToJsonObject<List<UserProfileModel>>(result);
        }

        /// <summary>
        ///   �����û��ǳ�����ȡ���û���Ϣ
        /// </summary>
        /// <param name="nicknames"> </param>
        /// <returns> </returns>
        public static List<UserProfileModel> GetUserInfoListByNickNames(string[] nicknames)
        {
            var url = Dev.CasClient.Configuration.CasClientConfiguration.Config.CasServerUrl
                      + "/api/User/GetUserInfoListByNickNames?nicknames=";

            url += string.Join("&nicknames=", nicknames);

            var result = Dev.Comm.Net.Http.GetUrl(url);

            return Dev.Comm.JsonConvert.ToJsonObject<List<UserProfileModel>>(result);
        }

        #endregion
    }
}