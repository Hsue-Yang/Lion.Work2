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
        public ActionResult SystemFunMenu()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemFunMenuModel model = new SystemFunMenuModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunMenuModel.Field.QuerySysID.ToString());
                model.QueryFunMenu = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunMenuModel.Field.QueryFunMenu.ToString());
            }
            #endregion

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemFunMenu);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunMenu.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemFunMenuList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunMenu.SystemMsg_GetSysSystemFunMenuList);
            }

            if (model.GetSystemFunMenuList(base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunMenu.SystemMsg_GetSystemFunMenuList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemFunMenu(SystemFunMenuModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemFunMenu);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunMenu.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemFunMenuList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunMenu.SystemMsg_GetSysSystemFunMenuList);
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemFunMenuModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemFunMenuModel.Field.QueryFunMenu.ToString(), model.QueryFunMenu);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion

                if (model.GetSystemFunMenuList(base.PageSize, base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemFunMenu.SystemMsg_GetSystemFunMenuList);
                }
            }

            return View(model);
        }
    }
}