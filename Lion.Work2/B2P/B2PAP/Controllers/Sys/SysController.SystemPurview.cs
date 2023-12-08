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
        public ActionResult SystemPurview()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemPurviewModel model = new SystemPurviewModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemPurviewModel.Field.QuerySysID.ToString());
                model.QueryPurviewID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemPurviewModel.Field.QueryPurviewID.ToString());
            }
            #endregion

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemPurview);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemPurview.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemPurviewIDList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemPurview.SystemMsg_GetSysSystemPurviewIDList);
            }

            if (model.GetSystemPurviewList(base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemPurview.SystemMsg_GetSystemPurviewList); ;
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemPurview(SystemPurviewModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemPurview);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemPurview.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemPurviewIDList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemPurview.SystemMsg_GetSysSystemPurviewIDList);
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemPurviewModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemPurviewModel.Field.QueryPurviewID.ToString(), model.QueryPurviewID);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion

                if (model.GetSystemPurviewList(base.PageSize, base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemPurview.SystemMsg_GetSystemPurviewList);
                }
            }

            return View(model);
        }
    }
}