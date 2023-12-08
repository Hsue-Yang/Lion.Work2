// 新增日期：2017-02-17
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemLoginEventSetting()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            var model = new SystemLoginEventSettingModel();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.SysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, nameof(model.SysID));
                model.LoginEventID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, nameof(model.LoginEventID));
            }
            #endregion

            if (await model.GetSystemSysIDList(false, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemLoginEventSetting.SystemMsg_GetSysSystemSysIDList_Failure);
            }
            
            if (await model.GetSysLoginEventIDList(model.SysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemLoginEventSetting.SystemMsg_GetSysLoginEventIDList_Failure);
            }

            if (await model.GetSysLoginEventSettingList(AuthState.SessionData.UserID, PageSize, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemLoginEventSetting.SystemMsg_GetSysLoginEventSettingList_Failure);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemLoginEventSetting(SystemLoginEventSettingModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (IsPostBack && model.ExecAction == EnumActionType.Update)
            {
                if (await model.UpdateSysLoginEventSettingSortResult(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemLoginEventSetting.SystemMsg_UpdateSysLoginEventSettingSortResult_Failure);
                }
            }

            #region - Set Cookie -
            Dictionary<string, string> paraDict =
                new Dictionary<string, string>
                {
                    { nameof(model.SysID), model.SysID },
                    { nameof(model.LoginEventID), model.LoginEventID }
                };

            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            if (await model.GetSystemSysIDList(false, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemLoginEventSetting.SystemMsg_GetSysSystemSysIDList_Failure);
            }

            if (await model.GetSysLoginEventIDList(model.SysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemLoginEventSetting.SystemMsg_GetSysLoginEventIDList_Failure);
            }

            if (await model.GetSysLoginEventSettingList(AuthState.SessionData.UserID, PageSize, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemLoginEventSetting.SystemMsg_GetSysLoginEventSettingList_Failure);
            }

            return View(model);
        }
    }
}