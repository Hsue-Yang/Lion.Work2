using System.ComponentModel;
using System.Configuration;
using System.Web;
using B2PAP.Models;
using LionTech.Entity.B2P;
using LionTech.Utility;

namespace B2PAP.Controllers
{
    public class _BaseAPController : _BaseController
    {
        public enum EnumEDIServicePathFile
        {
            [Description(@"{0}\{1}\{2}\Exception.err")]
            EDIServiceSubscriberLog
        }

        public enum EnumAPSessionKey
        {
            
        }

        protected override void OnActionExecutingSetAuthState()
        {
            
        }

        public enum EnumAppSettingAPIKey
        {
            APITimeOut,
            APISCMAPB2PSettingSupB2PUserEditEventURL,
            APISCMAPB2PSettingB2PSystemRoleEditEventURL, APISCMAPB2PSettingB2PSystemRoleDeleteEventURL,
            APISCMAPB2PSettingSupB2PUserBulletinCheckEventURL
        }

        protected string ExecAPIService(EnumAppSettingAPIKey enumAppSettingAPIKey, string apiParaJsonString)
        {
            int apiTimeOut = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingAPIKey.APITimeOut.ToString()]);

            string apiURL = string.Empty;

            switch (enumAppSettingAPIKey)
            {
                case EnumAppSettingAPIKey.APISCMAPB2PSettingSupB2PUserEditEventURL:
                    apiURL = string.Format(
                        ConfigurationManager.AppSettings[enumAppSettingAPIKey.ToString()],
                        new string[] { Common.GetEnumDesc(EnumAPISystemID.B2PAP), EnumSystemID.B2PAP.ToString(), AuthState.SessionData.UserID, HttpUtility.UrlEncode(apiParaJsonString) });
                    break;
                case EnumAppSettingAPIKey.APISCMAPB2PSettingSupB2PUserBulletinCheckEventURL:
                    apiURL = string.Format(
                        ConfigurationManager.AppSettings[enumAppSettingAPIKey.ToString()],
                        new string[] { Common.GetEnumDesc(EnumAPISystemID.B2PAP), EnumSystemID.B2PAP.ToString(), AuthState.SessionData.UserID, HttpUtility.UrlEncode(apiParaJsonString) });
                    break;
                case EnumAppSettingAPIKey.APISCMAPB2PSettingB2PSystemRoleEditEventURL:
                    apiURL = string.Format(
                        ConfigurationManager.AppSettings[enumAppSettingAPIKey.ToString()],
                        new string[] { Common.GetEnumDesc(EnumAPISystemID.B2PAP), EnumSystemID.B2PAP.ToString(), AuthState.SessionData.UserID, HttpUtility.UrlEncode(apiParaJsonString) });
                    break;
                case EnumAppSettingAPIKey.APISCMAPB2PSettingB2PSystemRoleDeleteEventURL:
                    apiURL = string.Format(
                        ConfigurationManager.AppSettings[enumAppSettingAPIKey.ToString()],
                        new string[] { Common.GetEnumDesc(EnumAPISystemID.B2PAP), EnumSystemID.B2PAP.ToString(), AuthState.SessionData.UserID, HttpUtility.UrlEncode(apiParaJsonString) });
                    break;
            }

            string responseString = Common.HttpWebRequestGetResponseString(apiURL, apiTimeOut);
            if (string.IsNullOrWhiteSpace(responseString) == false)
            {
                return responseString;
            }

            return null;
        }

        protected string ExecAPIService(EnumAppSettingAPIKey enumAppSettingAPIKey, string apiParaJsonString, byte[] byteArray)
        {
            int apiTimeOut = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingAPIKey.APITimeOut.ToString()]);

            string apiURL = string.Empty;

            switch (enumAppSettingAPIKey)
            {
                default:
                    apiURL = string.Empty;
                    break;
            }

            string responseString = Common.HttpWebRequestGetResponseString(apiURL, apiTimeOut, byteArray);
            if (string.IsNullOrWhiteSpace(responseString) == false)
            {
                return responseString;
            }

            return null;
        }

        protected byte[] ExecAPIService(EnumAppSettingAPIKey enumAppSettingAPIKey, string apiParaJsonString, int bufferLength)
        {
            int apiTimeOut = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingAPIKey.APITimeOut.ToString()]);

            string apiURL = string.Empty;

            switch (enumAppSettingAPIKey)
            {
                default:
                    apiURL = string.Empty;
                    break;
            }

            byte[] responseByte = Common.HttpWebRequestGetResponseStream(apiURL, apiTimeOut, bufferLength);
            if (responseByte != null)
            {
                return responseByte;
            }

            return null;
        }

        public void ExecUserRoleLogWrite(string userID)
        {
            _BaseAPModel model = new _BaseAPModel();
            model.GetRecordUserSystemRoleResult(userID, AuthState.SessionData.UserID, AuthState.SystemID.ToString(), base.ClientIPAddress());
        }

        public void ExecUserFunLogWrite(string userID)
        {
            _BaseAPModel model = new _BaseAPModel();
            model.GetRecordUserFunctionResult(userID, AuthState.SessionData.UserID, AuthState.SystemID.ToString(), base.ClientIPAddress());
        }
    }
}