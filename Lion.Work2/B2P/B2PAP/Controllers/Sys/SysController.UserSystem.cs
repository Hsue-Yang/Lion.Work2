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
        public ActionResult UserSystem()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            UserSystemModel model = new UserSystemModel();

            model.FormReset(AuthState.SessionData.UserID);

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QueryUserID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, UserSystemModel.Field.QueryUserID.ToString());
                model.QueryUserNM = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, UserSystemModel.Field.QueryUserNM.ToString());
            }
            #endregion

            model.GetSysUserSystemTabList(_BaseAPModel.EnumTabAction.SysUserSystem);

            if (model.GetUserSystemList(base.PageSize) == false)
            {
                SetSystemErrorMessage(SysUserSystem.SystemMsg_GetUserSystemList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult UserSystem(UserSystemModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysUserSystemTabList(_BaseAPModel.EnumTabAction.SysUserSystem);

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(UserSystemModel.Field.QueryUserID.ToString(), model.QueryUserID);
                paraDict.Add(UserSystemModel.Field.QueryUserNM.ToString(), model.QueryUserNM);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion

                if (model.GetUserSystemList(base.PageSize) == false)
                {
                    SetSystemErrorMessage(SysUserSystem.SystemMsg_GetUserSystemList);
                }
            }

            return View(model);
        }
    }
}