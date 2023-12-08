using System.Collections.Generic;
using System.Web.Mvc;
using B2PAP.Models;
using B2PAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult SystemRole()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemRoleModel model = new SystemRoleModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemRoleModel.Field.QuerySysID.ToString());
                model.QueryRoleID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemRoleModel.Field.QueryRoleID.ToString());
            }
            #endregion

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemRole);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRole.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemRoleIDList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRole.SystemMsg_GetSysSystemRoleIDList);
            }

            if (model.GetSystemRoleList(base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRole.SystemMsg_GetSystemRoleList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemRole(SystemRoleModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemRole);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRole.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemRoleIDList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRole.SystemMsg_GetSysSystemRoleIDList);
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemRoleModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemRoleModel.Field.QueryRoleID.ToString(), model.QueryRoleID);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion

                if (model.GetSystemRoleList(base.PageSize, base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemRole.SystemMsg_GetSystemRoleList);
                }
            }

            return View(model);
        }
    }
}