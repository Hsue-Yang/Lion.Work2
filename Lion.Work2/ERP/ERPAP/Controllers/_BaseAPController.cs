using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ERPAP.Models;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Log;
using LionTech.Utility;
using LionTech.Utility.ERP;

namespace ERPAP.Controllers
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

        protected override void OnSetERPUserMessage()
        {
            if (base.ClientIPAddress() != Common.GetEnumDesc(EnumSystemID.Domain))
            {
                string apiParaJsonString = string.Empty;
                string apiReturnJsonString = string.Empty;
                try
                {
                    Entity_BaseAP.UserMessageAPIPara entityUserMessageAPIPara = new Entity_BaseAP.UserMessageAPIPara()
                    {
                        UserID = new DBVarChar(AuthState.SessionData.UserID)
                    };

                    apiParaJsonString = entityUserMessageAPIPara.SerializeToJson();
                    apiReturnJsonString = this.ExecAPIService(EnumAppSettingAPIKey.APIERPAPERPPubDataUserMessageSelectEventURL, apiParaJsonString);
                    
                    if (string.IsNullOrWhiteSpace(apiReturnJsonString))
                    {
                        TempData["ERPUserMessageCount"] = 0;
                    }
                    else
                    {
                        JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                        Entity_BaseAP.UserMessageCollection userMessageCollection = jsonConvert.Deserialize<Entity_BaseAP.UserMessageCollection>(HttpUtility.UrlDecode(apiReturnJsonString));

                        if (userMessageCollection != null && userMessageCollection.UserMessages != null)
                        {
                            int i = 0;
                            foreach (Entity_BaseAP.UserMessageItem userMessageItem in userMessageCollection.UserMessages)
                            {
                                TempData["ERPUserMessage" + i.ToString()] = userMessageItem.UserMessage;
                                i += 1;
                            }
                            TempData["ERPUserMessageCount"] = i;
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileLog.Write(this.GetRootPathFilePath(EnumRootPathFile.Exception), $"{nameof(OnSetERPUserMessage)}, {nameof(apiParaJsonString)} = {apiParaJsonString}, {nameof(apiReturnJsonString)} = {apiReturnJsonString}");
                    FileLog.Write(this.GetRootPathFilePath(EnumRootPathFile.Exception), ex);
                }
            }
        }

        protected override void OnSetUserSystemNotifications()
        {
            try
            {
                Entity_BaseAP.UserSystemNotificationAPIPara entityUserSystemNotificationAPIParaPara =
                    new Entity_BaseAP.UserSystemNotificationAPIPara()
                    {
                        UserID = new DBVarChar(AuthState.SessionData.UserID)
                    };

                string apiParaJsonString = entityUserSystemNotificationAPIParaPara.SerializeToJson();
                string apiReturnJsonString = this.ExecAPIService(EnumAppSettingAPIKey.APIERPAPPubDataUserSystemNotificationsUnReadCountSelectEventURL, apiParaJsonString);

                if (string.IsNullOrWhiteSpace(apiReturnJsonString) == false)
                {
                    long unRead = 0;
                    if (long.TryParse(apiReturnJsonString, out unRead))
                    {
                        TempData["UserSystemNotificationUnReadCount"] = unRead;
                    }
                }
            }
            catch (Exception ex)
            {
                FileLog.Write(this.GetRootPathFilePath(EnumRootPathFile.Exception), ex);
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            try
            {
                var serpMailMessages =
                    new SERPMailMessages
                    {
                        MialAddress = ConfigurationManager.AppSettings[EnumAppSettingKey.SysSDMail.ToString()],
                        SmtpClientIPAddress = ConfigurationManager.AppSettings[EnumAppSettingKey.SmtpClientIPAddress.ToString()],
                        AppName = Common.GetEnumDesc(EnumSystemID.ERPAP),
                        UserID = AuthState.SessionData.UserID,
                        UserName = AuthState.SessionData.UserNM,
                        Ex = filterContext.Exception
                    };

                PublicFun.SendErrorMailForSERP(serpMailMessages);

                string lineID = ConfigurationManager.AppSettings[EnumAppSettingKey.LineID.ToString()];
                if (string.IsNullOrWhiteSpace(lineID) == false)
                {
                    var to = ConfigurationManager.AppSettings[EnumAppSettingKey.LineTo.ToString()].Split(';');
                    PublicFun.SendErrorLineForSERP(Common.GetEnumDesc(EnumSystemID.ERPAP), lineID, to, filterContext.Exception);
                }

                string teamsID = ConfigurationManager.AppSettings[EnumAppSettingKey.TeamsTo.ToString()];
                if (string.IsNullOrWhiteSpace(teamsID) == false)
                {
                    PublicFun.SendErrorTeamsForSERP(teamsID, Common.GetEnumDesc(EnumSystemID.ERPAP), AuthState.SessionData.UserID, AuthState.SessionData.UserNM, filterContext.Exception); ;
                }
            }
            catch (Exception ex)
            {
                FileLog.Write(GetRootPathFilePath(EnumRootPathFile.Exception), ex);
            }
        }

        public enum EnumEDIServiceEventGroupID
        {
            SysSystemMenu, SysSystemFunMenu,
            SysSystemFunGroup, SysSystemFun, SysSystemFunAssign,
            SysSystemAPIGroup, SysSystemAPI,
            SysSystemRoleCateg, SysSystemRole,
            SysSystemPurview,
            SysRoleGroup, SysRoleGroupCollect,
            SysUserSystemRole,
            SysUserFunction,
            SysSystemWFGroup,
            SysSystemWF,
            SysSystemWFNode,
            SysSystemWFSignature,
            SysSystemWFDocument,
            SysUserPurview
        }

        public enum EnumEDIServiceEventID
        {
            Edit, Delete
        }

        protected string ExecEDIServiceDistributor(EnumEDIServiceEventGroupID eventGroup, EnumEDIServiceEventID eventID, string eventParaJsonString)
        {
            int ediServiceDistributorTimeOut = base.GetEDIServiceDistributorTimeOut();

            string ediServiceDistributorPath = base.GetEDIServiceDistributorPath(
                EnumSystemID.ERPAP.ToString(), eventGroup.ToString(), eventID.ToString(),
                AuthState.SessionData.UserID, HttpUtility.UrlEncode(eventParaJsonString));

            string responseString = Common.HttpWebRequestGetResponseString(ediServiceDistributorPath, ediServiceDistributorTimeOut);
            if (string.IsNullOrWhiteSpace(responseString) == false)
            {
                return responseString;
            }

            return null;
        }

        public enum EnumAppSettingAPIKey
        {
            APITimeOut,

            APITKNAPTokenServiceGeneratorURL,
            APITKNAPTokenServiceLoginURL,
            APITKNAPTokenServiceValidationURL,
            APITKNAPTokenServiceDelayValidationURL,

            APIERPAPAuthorizationERPUserValidateEventURL,
            APIERPAPAuthorizationERPRoleUserEditEventURL,
            APIERPAPAuthorizationERPGenerateUserMenuEventURL,

            APIERPAPSystemSettingSystemIconUploadEventURL,
            APIERPAPSystemSettingSystemIconDownloadEventURL,

            APIERPAPERPPubDataUserMessageSelectEventURL,
            APIERPAPPubDataUserSystemNotificationsSelectEventURL,
            APIERPAPPubDataUserSystemNotificationsUnReadCountSelectEventURL,
            IsWriteLog
        }

        protected string ExecAPIService(EnumAppSettingAPIKey enumAppSettingAPIKey, string apiParaJsonString)
        {
            int apiTimeOut = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingAPIKey.APITimeOut.ToString()]);

            string apiURL = string.Empty;

            switch (enumAppSettingAPIKey)
            {
                case EnumAppSettingAPIKey.APIERPAPERPPubDataUserMessageSelectEventURL:
                case EnumAppSettingAPIKey.APIERPAPPubDataUserSystemNotificationsSelectEventURL:
                case EnumAppSettingAPIKey.APIERPAPPubDataUserSystemNotificationsUnReadCountSelectEventURL:
                    apiURL = string.Format(
                        ConfigurationManager.AppSettings[enumAppSettingAPIKey.ToString()],
                        new string[] { Common.GetEnumDesc(EnumAPISystemID.ERPAP), EnumSystemID.ERPAP.ToString(), AuthState.SessionData.UserID, HttpUtility.UrlEncode(apiParaJsonString) });
                    break;
                default:
                    apiURL = string.Empty;
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
                case EnumAppSettingAPIKey.APIERPAPSystemSettingSystemIconUploadEventURL:
                    apiURL = string.Format(
                        ConfigurationManager.AppSettings[enumAppSettingAPIKey.ToString()],
                        new string[] { Common.GetEnumDesc(EnumAPISystemID.ERPAP), EnumSystemID.ERPAP.ToString(), AuthState.SessionData.UserID, HttpUtility.UrlEncode(apiParaJsonString) });
                    break;
                case EnumAppSettingAPIKey.APIERPAPAuthorizationERPRoleUserEditEventURL:
                    apiURL = string.Format(
                        ConfigurationManager.AppSettings[enumAppSettingAPIKey.ToString()],
                        new string[] { Common.GetEnumDesc(EnumAPISystemID.ERPAP), EnumSystemID.ERPAP.ToString(), AuthState.SessionData.UserID });
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
                case EnumAppSettingAPIKey.APIERPAPSystemSettingSystemIconDownloadEventURL:

                case EnumAppSettingAPIKey.APIERPAPAuthorizationERPGenerateUserMenuEventURL:
                    apiURL = string.Format(
                        ConfigurationManager.AppSettings[enumAppSettingAPIKey.ToString()],
                        new string[] { Common.GetEnumDesc(EnumAPISystemID.ERPAP), EnumSystemID.ERPAP.ToString(), AuthState.SessionData.UserID, HttpUtility.UrlEncode(apiParaJsonString) });
                    break;
            }

            byte[] responseByte = Common.HttpWebRequestGetResponseStream(apiURL, apiTimeOut, bufferLength);
            if (responseByte != null)
            {
                return responseByte;
            }

            return null;
        }

        #region - UserSystemNotification -
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult GetUserSystemNotificationList(int dataIndex)
        {
            if (AuthState.IsAuthorized == false)
                return AuthState.UnAuthorizedActionResult;

            Entity_BaseAP.UserSystemNotificationAPIPara entityUserSystemNotificationAPIParaPara =
                new Entity_BaseAP.UserSystemNotificationAPIPara
                {
                    UserID = new DBVarChar(AuthState.SessionData.UserID),
                    DataIndex = new DBInt(dataIndex)
                };

            string apiParaJsonString = entityUserSystemNotificationAPIParaPara.SerializeToJson();
            string apiReturnJsonString = this.ExecAPIService(EnumAppSettingAPIKey.APIERPAPPubDataUserSystemNotificationsSelectEventURL, apiParaJsonString);

            if (string.IsNullOrWhiteSpace(apiReturnJsonString) == false)
            {
                return new ContentResult { Content = apiReturnJsonString };
            }

            return Json(null);
        }
        #endregion

        #region - FunTool -

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult GetUpdateSysFunToolResult(string funControllerID, string funActionName, string toolNo)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            _BaseModel model = new _BaseModel();

            if (model.GetUpdateSysFunTool(AuthState.SessionData.UserID, AuthState.SystemID.ToString(), funControllerID, funActionName, toolNo))
            {
                return Json(toolNo);
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult GetCopySysFunToolResult(string funControllerID, string funActionName, string toolNo, string copyUserID, string isUseDefault)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            _BaseModel model = new _BaseModel();

            string defaultToolNo = string.Empty;
            if (isUseDefault == EnumYN.Y.ToString())
            {
                defaultToolNo = Common.GetEnumDesc(_BaseAPModel.EnumSysFunToolNo.DefaultNo);
            }

            if (model.GetCopySysFunTool(AuthState.SessionData.UserID, AuthState.SystemID.ToString(), funControllerID, funActionName,
                                        defaultToolNo, toolNo, copyUserID))
            {
                return Json(toolNo);
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult GetDeleteSysFunToolResult(string funControllerID, string funActionName, string toolNo)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            _BaseModel model = new _BaseModel();

            if (model.GetDeleteSysFunTool(AuthState.SessionData.UserID, AuthState.SystemID.ToString(), funControllerID, funActionName, toolNo))
            {
                return Json(toolNo);
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult GetBaseRAWUserList(string condition)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            _BaseModel model = new _BaseModel();

            model.GetBaseRAWUserList(condition);
            if (model.EntityBaseRAWUserList != null)
            {
                var userList = model.EntityBaseRAWUserList.Take(20);

                var result = from user in userList
                             select new
                             {
                                 Text = user.UserNM.GetValue(),
                                 Value = user.UserID.GetValue(),
                             };

                return Json(result);
            }
            return null;
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult GetUpdateSysFunToolNameResult(string funToolControllerID, string funToolActionName, string toolNo, string toolNM)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            _BaseModel model = new _BaseModel();

            if (model.GetUpdateSysFunToolName(AuthState.SessionData.UserID, AuthState.SystemID.ToString(), funToolControllerID, funToolActionName, toolNo, ref toolNM))
            {
                return Json(toolNM);
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult GetSelectSysFunToolNameList(string funToolControllerID, string funToolActionName, string toolNo)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            _BaseModel model = new _BaseModel();

            string toolNM = string.Empty;

            if (model.GetSelectSysFunToolName(AuthState.SessionData.UserID, AuthState.SystemID.ToString(), funToolControllerID, funToolActionName, toolNo, ref toolNM))
            {
                return Json(toolNM);
            }
            return null;
        }
        #endregion

        public enum EnumExecTokenDelayResult
        {
            Success, Failure, NotExist
        }

        public EnumExecTokenDelayResult ExecTokenDelay(string paraT, string paraK, string paraC, string paraU)
        {
            if (string.IsNullOrWhiteSpace(paraT) == false)
            {
                int apiTimeOut = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingAPIKey.APITimeOut.ToString()]);

                string apiURL = string.Format(
                    ConfigurationManager.AppSettings[EnumAppSettingAPIKey.APITKNAPTokenServiceDelayValidationURL.ToString()],
                    new string[] { Common.GetEnumDesc(EnumAPISystemID.TKNAP), paraT, paraK, paraC, paraU }
                );

                string isValid = Common.HttpWebRequestGetResponseString(apiURL, apiTimeOut);

                if (string.IsNullOrWhiteSpace(isValid) == false && isValid == bool.TrueString)
                {
                    return EnumExecTokenDelayResult.Success;
                }

                return EnumExecTokenDelayResult.Failure;
            }

            return EnumExecTokenDelayResult.NotExist;
        }

        public void ExecUserFunLogWrite(string userID, string erpWFNo, string memo)
        {
            _BaseAPModel model = new _BaseAPModel();
            model.GetRecordUserFunctionResult(userID, erpWFNo, memo, AuthState.SessionData.UserID, base.ClientIPAddress(), base.CultureID);
        }
    }
}