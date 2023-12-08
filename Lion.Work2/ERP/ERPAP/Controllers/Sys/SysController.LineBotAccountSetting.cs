using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> LineBotAccountSetting()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            var model = new LineBotAccountSettingModel();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.SysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, nameof(model.SysID));
                model.LineID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, nameof(model.LineID));
            }
            #endregion

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, false, CultureID) == false)
            {
                SetSystemErrorMessage(SysLineBotAccountSetting.SystemMsg_GetSysUserSystemSysIDList_Failure);
            }

            if (model.GetLineBotIDList(AuthState.SessionData.UserID, model.SysID, CultureID) == false)
            {
                SetSystemErrorMessage(SysLineBotAccountSetting.SystemMsg_GetLineBotIDList_Failure);
            }

            if (model.CheckIsITManager(AuthState.SessionData.UserID, model.SysID) == false)
            {
                SetSystemErrorMessage(SysResource.SystemMsg_CheckIsITManager_Failure);
            }
            else if (model.IsITManager == false)
            {
                model.SysID = string.Empty;
            }

            if (await model.GetLineBotAccountSettingsList(PageSize, AuthState.SessionData.UserID, model.LineID, CultureID) == false)
            {
                SetSystemErrorMessage(SysLineBotAccountSetting.SystemMsg_GetLineBotAccountSettingList_Failure);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> LineBotAccountSetting(LineBotAccountSettingModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            #region - Set Cookie -
            Dictionary<string, string> paraDict =
                new Dictionary<string, string>
                {
                    { nameof(model.SysID), model.SysID },
                    { nameof(model.LineID), model.LineID }
                };

            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, false, CultureID) == false)
            {
                SetSystemErrorMessage(SysLineBotAccountSetting.SystemMsg_GetSysUserSystemSysIDList_Failure);
            }

            if (model.GetLineBotIDList(AuthState.SessionData.UserID, model.SysID, CultureID) == false)
            {
                SetSystemErrorMessage(SysLineBotAccountSetting.SystemMsg_GetLineBotIDList_Failure);
            }

            if (model.CheckIsITManager(AuthState.SessionData.UserID, model.SysID) == false)
            {
                SetSystemErrorMessage(SysResource.SystemMsg_CheckIsITManager_Failure);
            }
            else if (model.IsITManager == false)
            {
                model.SysID = string.Empty;
            }

            if (await model.GetLineBotAccountSettingsList(PageSize, AuthState.SessionData.UserID, model.LineID, CultureID) == false)
            {
                SetSystemErrorMessage(SysLineBotAccountSetting.SystemMsg_GetLineBotAccountSettingList_Failure);
            }

            return View(model);
        }
    }
}