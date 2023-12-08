using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TRAININGAP.Models;

namespace TRAININGAP.Controllers
{
    public class _BaseAPController : _BaseController
    {
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
                Entity_BaseAP.UserMessageAPIPara entityUserMessageAPIPara = new Entity_BaseAP.UserMessageAPIPara()
                {
                    UserID = new DBVarChar(AuthState.SessionData.UserID)
                };

                string apiParaJsonString = entityUserMessageAPIPara.SerializeToJson();
                string apiReturnJsonString = this.ExecAPIService(EnumAppSettingAPIKey.APIERPAPERPPubDataUserMessageSelectEventURL, apiParaJsonString);

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
        }

        public enum EnumEDIServiceEventGroupID
        {

        }

        public enum EnumEDIServiceEventID
        {

        }

        protected string ExecEDIServiceDistributor(EnumEDIServiceEventGroupID eventGroup, EnumEDIServiceEventID eventID, string eventParaJsonString)
        {
            int ediServiceDistributorTimeOut = base.GetEDIServiceDistributorTimeOut();

            string ediServiceDistributorPath = base.GetEDIServiceDistributorPath(
                EnumSystemID.TRAININGAP.ToString(), eventGroup.ToString(), eventID.ToString(),
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

            APIERPAPERPPubDataUserMessageSelectEventURL

        }

        protected string ExecAPIService(EnumAppSettingAPIKey enumAppSettingAPIKey, string apiParaJsonString)
        {
            int apiTimeOut = int.Parse(ConfigurationManager.AppSettings[EnumAppSettingAPIKey.APITimeOut.ToString()]);

            string apiURL = string.Empty;

            switch (enumAppSettingAPIKey)
            {
                case EnumAppSettingAPIKey.APIERPAPERPPubDataUserMessageSelectEventURL:
                    apiURL = string.Format(
                        ConfigurationManager.AppSettings[enumAppSettingAPIKey.ToString()],
                        new string[] { Common.GetEnumDesc(EnumAPISystemID.ERPAP), EnumSystemID.TRAININGAP.ToString(), AuthState.SessionData.UserID, HttpUtility.UrlEncode(apiParaJsonString) });
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

    }
}