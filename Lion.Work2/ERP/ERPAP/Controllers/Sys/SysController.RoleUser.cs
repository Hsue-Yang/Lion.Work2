using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> RoleUser()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            RoleUserModel model = new RoleUserModel();

            model.GetSysUserSystemTabList(_BaseAPModel.EnumTabAction.SysRoleUser);

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.SysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, RoleUserModel.EnumCookieKey.SysID.ToString());
                model.RoleCategoryID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, RoleUserModel.EnumCookieKey.RoleCategoryID.ToString());
                model.RoleID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, RoleUserModel.EnumCookieKey.RoleID.ToString());
            }
            #endregion

            model.FormReset();

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, false, CultureID) == false)
            {
                SetSystemErrorMessage(SysRoleUser.SystemMsg_GetSysUserSystemSysIDList_Failure);
            }

            if (await model.GetSysSystemRoleCategoryIDList(model.SysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysRoleUser.SystemMsg_GetSysSystemRoleCategoryIDList_Failure);
            }

            if (await model.GetSysSystemRoleIDList(model.SysID, AuthState.SessionData.UserID, model.RoleCategoryID, CultureID) == false)
            {
                SetSystemErrorMessage(SysRoleUser.SystemMsg_GetSysSystemRoleIDList_Failure);
            }

            if (await model.GetRoleUserList(CultureID) == false)
            {
                SetSystemErrorMessage(SysRoleUser.SystemMsg_GetRoleUserList_Failure);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> RoleUser(RoleUserModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (model.ExecAction == EnumActionType.Update)
            {
                var apiResult =
                    ExecAPIService(
                        EnumAppSettingAPIKey.APIERPAPAuthorizationERPRoleUserEditEventURL,
                        null,
                        Encoding.UTF8.GetBytes(model.GetAuthRoleUserParaJsonString()));

                if (string.IsNullOrWhiteSpace(apiResult) ||
                    apiResult.AsBool() == false)
                {
                    SetSystemErrorMessage(SysRoleUser.SystemMsg_RoleUserEdit_Failure);
                }
            }

            model.GetSysUserSystemTabList(_BaseAPModel.EnumTabAction.SysRoleUser);

            #region - Set Cookie -
            Dictionary<string, string> paraDict =
                new Dictionary<string, string>
                {
                    { RoleUserModel.EnumCookieKey.SysID.ToString(), model.SysID },
                    { RoleUserModel.EnumCookieKey.RoleCategoryID.ToString(), model.RoleCategoryID },
                    { RoleUserModel.EnumCookieKey.RoleID.ToString(), model.RoleID }
                };

            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, false, CultureID) == false)
            {
                SetSystemErrorMessage(SysRoleUser.SystemMsg_GetSysUserSystemSysIDList_Failure);
            }

            if (await model.GetSysSystemRoleCategoryIDList(model.SysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysRoleUser.SystemMsg_GetSysSystemRoleCategoryIDList_Failure);
            }

            if (await model.GetSysSystemRoleIDList(model.SysID, AuthState.SessionData.UserID, model.RoleCategoryID, CultureID) == false)
            {
                SetSystemErrorMessage(SysRoleUser.SystemMsg_GetSysSystemRoleIDList_Failure);
            }

            if (await model.GetRoleUserList(CultureID) == false)
            {
                SetSystemErrorMessage(SysRoleUser.SystemMsg_GetRoleUserList_Failure);
            }

            return View(model);
        }
    }
}