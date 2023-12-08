using ERPAPI.Models;
using LionTech.Entity.ERP;
using LionTech.Log;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;

namespace ERPAPI.Controllers
{
    public class _BaseAPController : _BaseController
    {
        protected override void OnActionExecutingSetAuthState()
        {
            
        }

        internal enum EnumEDIServiceEventGroupID
        {
            SysUserSystemRole
        }

        internal enum EnumEDIServiceEventID
        {
            Edit
        }

        [NonAction]
        internal virtual string ExecEDIServiceDistributor(EnumEDIServiceEventGroupID eventGroup, EnumEDIServiceEventID eventID, string eventParaJsonString, string serviceUserID)
        {
            int ediServiceDistributorTimeOut = base.GetEDIServiceDistributorTimeOut();

            string ediServiceDistributorPath = base.GetEDIServiceDistributorPath(
                LionTech.Entity.ERP.EnumSystemID.ERPAP.ToString(), eventGroup.ToString(), eventID.ToString(),
                serviceUserID, HttpUtility.UrlEncode(eventParaJsonString));

            string responseString = Common.HttpWebRequestGetResponseString(ediServiceDistributorPath, ediServiceDistributorTimeOut);
            if (string.IsNullOrWhiteSpace(responseString) == false)
            {
                return responseString;
            }

            return null;
        }

        protected string GetB2PAPUserMenuFilePath(string userID, LionTech.Entity.B2P.EnumCultureID cultureID)
        {
            return Path.Combine(
                new string[]
                    {
                        string.Format(
                            ConfigurationManager.AppSettings[EnumAppSettingKey.FilePathB2PAPUserMenu.ToString()],
                            userID,
                            cultureID == LionTech.Entity.B2P.EnumCultureID.zh_TW ? string.Empty : "." + Common.GetEnumDesc(cultureID)
                        )
                    });
        }

        public enum EnumLogWriter
        {
            [Description("APIService.ERP.LineBotService")]
            ERPLineBotService,
            [Description("APIService.ERP.Authorization")]
            ERPAuthorization,
            [Description("APIService.ERP.Subscriber")]
            ERPSubscriber,
            [Description("APIService.B2P.Authorization")]
            B2PUpdUserID
        }

        internal virtual string ExecAPIService(EnumAppSettingKey enumAppSettingAPIKey, string apiParaJsonString, string requestMethod = WebRequestMethods.Http.Get)
        {
            int apiTimeOut = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingKey.APITimeOut.ToString()]);
            
            string apiURL = string.Empty;

            switch (enumAppSettingAPIKey)
            {
                case EnumAppSettingKey.ERPAPHomeQRCodeAuthorization:
                    apiURL = string.Format(
                        ConfigurationManager.AppSettings[enumAppSettingAPIKey.ToString()], Common.GetEnumDesc(EnumSystemID.ERPAP));
                    break;
                default:
                    apiURL = string.Empty;
                    break;
            }

            string responseString;
            if (requestMethod == WebRequestMethods.Http.Get)
            {
                responseString = Common.HttpWebRequestGetResponseString(apiURL, apiTimeOut);
            }
            else
            {
                responseString = Common.HttpWebRequestGetResponseString(apiURL, apiTimeOut, Encoding.UTF8.GetBytes(apiParaJsonString));
            }

            if (string.IsNullOrWhiteSpace(responseString) == false)
            {
                return responseString;
            }

            return null;
        }
        
        internal virtual void OnException(Exception exception)
        {
            FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), exception);

            try
            {
                var serpMailMessages =
                    new SERPMailMessages
                    {
                        MialAddress = ConfigurationManager.AppSettings[EnumAppSettingKey.SysSDMail.ToString()],
                        SmtpClientIPAddress = ConfigurationManager.AppSettings[EnumAppSettingKey.SmtpClientIPAddress.ToString()],
                        AppName = Common.GetEnumDesc(EnumAPISystemID.ERPAP),
                        Ex = exception
                    };

                PublicFun.SendErrorMailForSERP(serpMailMessages);

                string lineID = ConfigurationManager.AppSettings[EnumAppSettingKey.LineID.ToString()];
                if (string.IsNullOrWhiteSpace(lineID) == false)
                {
                    var to = ConfigurationManager.AppSettings[EnumAppSettingKey.LineTo.ToString()].Split(';');
                    PublicFun.SendErrorLineForSERP(Common.GetEnumDesc(EnumAPISystemID.ERPAP), lineID, to, exception);
                }
            }
            catch (Exception ex)
            {
                FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), ex);
            }
        }

        protected string GetModelErrorMessages()
        {
            return
                string.Join(Environment.NewLine,
                    ModelState
                        .Values.Where(w => w.Errors.Count > 0)
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage));
        }

        protected void ExecERPUserAccessLogWrite(string userID, string apiNo, EnumLogWriter updUser, string sysID, string restrictType, string isLock, string isDisable)
        {
            _BaseAPModel model = new _BaseAPModel();
            model.GetERPRecordUserAccessResult(userID, apiNo, Common.GetEnumDesc(updUser), sysID, restrictType, isLock, isDisable, base.ClientIPAddress());
        }
        
        protected void ExecB2PUserRolePLogWrite(string userID, EnumLogWriter updUser, string sysID)
        {
            _BaseAPModel model = new _BaseAPModel();
            model.GetB2PRecordUserSystemRoleResult(userID, Common.GetEnumDesc(updUser), sysID, base.ClientIPAddress());
        }
    }
}