using LionTech.Entity.ERP;
using LionTech.Log;
using LionTech.Utility;
using System.Configuration;
using System;
using System.Web;
using LionTech.Utility.ERP;
using System.ComponentModel;  

namespace TRAININGAPI.Controllers
{
    public class _BaseAPController : _BaseController
    {
        protected override void OnActionExecutingSetAuthState()
        {

        }

        public enum EnumEDIServiceEventGroupID
        {

        }

        public enum EnumEDIServiceEventID
        {

        }

        protected string ExecEDIServiceDistributor(EnumEDIServiceEventGroupID eventGroup, EnumEDIServiceEventID eventID, string eventParaJsonString, string serviceUserID)
        {
            int ediServiceDistributorTimeOut = base.GetEDIServiceDistributorTimeOut();

            string ediServiceDistributorPath = base.GetEDIServiceDistributorPath(
                EnumSystemID.TRAININGAP.ToString(), eventGroup.ToString(), eventID.ToString(),
                serviceUserID, HttpUtility.UrlEncode(eventParaJsonString));

            string responseString = Common.HttpWebRequestGetResponseString(ediServiceDistributorPath, ediServiceDistributorTimeOut);
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
    }
}