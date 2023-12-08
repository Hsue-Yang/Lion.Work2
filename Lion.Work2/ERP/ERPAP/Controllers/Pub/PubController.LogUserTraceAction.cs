using ERPAP.Models.Pub;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
namespace ERPAP.Controllers
{
    public partial class PubController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult LogUserTraceAction()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }
            LogUserTraceActionModel model = new LogUserTraceActionModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.SearchType = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, LogUserTraceActionModel.EnumCookieKey.SearchType.ToString());
                model.UserID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, LogUserTraceActionModel.EnumCookieKey.UserID.ToString());
                model.SysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, LogUserTraceActionModel.EnumCookieKey.SysID.ToString());
                model.ControllerName = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, LogUserTraceActionModel.EnumCookieKey.ControllerName.ToString());
                model.ActionName = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, LogUserTraceActionModel.EnumCookieKey.ActionName.ToString());
                model.SessionID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, LogUserTraceActionModel.EnumCookieKey.SessionID.ToString());
                model.RequestSessionID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, LogUserTraceActionModel.EnumCookieKey.RequestSessionID.ToString());
                model.StartTraceDate = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, LogUserTraceActionModel.EnumCookieKey.StartTraceDate.ToString());
                model.EndTraceDate = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, LogUserTraceActionModel.EnumCookieKey.EndTraceDate.ToString());
                model.StartTraceTime = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, LogUserTraceActionModel.EnumCookieKey.StartTraceTime.ToString());
                model.EndTraceTime = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, LogUserTraceActionModel.EnumCookieKey.EndTraceTime.ToString());
            }
            #endregion

            if (string.IsNullOrWhiteSpace(model.StartTraceDate) &&
               string.IsNullOrWhiteSpace(model.EndTraceDate) &&
               string.IsNullOrWhiteSpace(model.StartTraceTime) &&
               string.IsNullOrWhiteSpace(model.EndTraceTime))
            {
                model.StartTraceDate = Common.GetDateTimeFormattedText(DateTime.Now.AddHours(-6), Common.EnumDateTimeFormatted.ShortDateNumber);
                model.EndTraceDate = Common.GetDateTimeFormattedText(DateTime.Now, Common.EnumDateTimeFormatted.ShortDateNumber);
            }
            if (model.GetLogUserTraceList(PageSize) == false)
            {
                SetSystemErrorMessage(PubLogUserTraceAction.SystemMsg_GetLogUserTraceActionList_Failure);
            }

            return View(model);
        }
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult LogUserTraceAction(LogUserTraceActionModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }
            if (model.ExecAction == EnumActionType.Select)
            {
                if (TryValidatableObject(model))
                {
                    #region - Set Cookie -
                    Dictionary<string, string> paraDict =
                        new Dictionary<string, string>
                        {
                            { LogUserTraceActionModel.EnumCookieKey.SearchType.ToString(), model.SearchType },
                            { LogUserTraceActionModel.EnumCookieKey.UserID.ToString(), model.UserID },
                            { LogUserTraceActionModel.EnumCookieKey.SysID.ToString(), model.SysID },
                            { LogUserTraceActionModel.EnumCookieKey.ControllerName.ToString(), model.ControllerName },
                            { LogUserTraceActionModel.EnumCookieKey.ActionName.ToString(), model.ActionName },
                            { LogUserTraceActionModel.EnumCookieKey.SessionID.ToString(), model.SessionID },
                            { LogUserTraceActionModel.EnumCookieKey.RequestSessionID.ToString(), model.RequestSessionID },
                            { LogUserTraceActionModel.EnumCookieKey.StartTraceDate.ToString(), model.StartTraceDate },
                            { LogUserTraceActionModel.EnumCookieKey.EndTraceDate.ToString(), model.EndTraceDate },
                            { LogUserTraceActionModel.EnumCookieKey.StartTraceTime.ToString(), model.StartTraceTime },
                            { LogUserTraceActionModel.EnumCookieKey.EndTraceTime.ToString(), model.EndTraceTime }
                        };
                    AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                    #endregion
                }
                else
                {
                    return View(model);
                }
            }
            if (model.GetLogUserTraceList(PageSize) == false)
            {
                SetSystemErrorMessage(PubLogUserTraceAction.SystemMsg_GetLogUserTraceActionList_Failure);
            }
            return View(model);
        }
    }
}