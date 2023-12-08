using System.Collections.Generic;
using System.Web.Mvc;
using B2PAP.Models;
using B2PAP.Models.Sys;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult SystemFun()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemFunModel model = new SystemFunModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunModel.Field.QuerySysID.ToString());
                model.QuerySubSysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunModel.Field.QuerySubSysID.ToString());
                model.QueryFunControllerID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunModel.Field.QueryFunControllerID.ToString());
                model.QueryFunControllerNM = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunModel.Field.QueryFunControllerNM.ToString());
                model.QueryFunActionName = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunModel.Field.QueryFunActionName.ToString());
                model.QueryFunName = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunModel.Field.QueryFunName.ToString());
                model.QueryFunMenuSysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunModel.Field.QueryFunMenuSysID.ToString());
                model.QueryFunMenu = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunModel.Field.QueryFunMenu.ToString());
            }
            #endregion

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemFun);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemSubsysIDList(model.QuerySysID, false, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSysSystemSubsysIDList);
            }

            if (model.GetSysSystemFunControllerIDList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSysSystemFunControllerIDList);
            }

            if (model.GetSysSystemFunNameList(model.QuerySysID, model.QueryFunControllerID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSysSystemFunNameList);
            }

            if (model.GetSysSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSysSystemSysIDList);
            }

            if (model.GetSysSystemFunMenuList(model.QueryFunMenuSysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSysSystemFunMenuList);
            }

            if (model.GetSysSystemPurviewIDList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSysSystemPurviewIDList);
            }

            if (model.GetSystemFunList(base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSystemFunList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemFun(SystemFunModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemFun);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemFunControllerIDList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSysSystemFunControllerIDList);
            }

            if (model.GetSysSystemFunNameList(model.QuerySysID, model.QueryFunControllerID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSysSystemFunNameList);
            }

            if (model.GetSysSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSysSystemSysIDList);
            }

            if (model.GetSysSystemFunMenuList(model.QueryFunMenuSysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSysSystemFunMenuList);
            }

            if (model.GetSysSystemPurviewIDList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSysSystemPurviewIDList);
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Update)
            {
                EntitySystemFun.EnumEditSystemFunResult result = model.GetEditSystemFunResult(AuthState.SessionData.UserID, base.CultureID);

                if (result == EntitySystemFun.EnumEditSystemFunResult.Failure)
                {
                    SetSystemErrorMessage(SysSystemFun.EditSystemFunResult_Failure);
                }
                else if (result == EntitySystemFun.EnumEditSystemFunResult.Success)
                {
                    SetSystemAlertMessage(SysSystemFun.EditSystemFunResult_Success);
                }
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemFunModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemFunModel.Field.QuerySubSysID.ToString(), model.QuerySubSysID);
                paraDict.Add(SystemFunModel.Field.QueryFunControllerID.ToString(), model.QueryFunControllerID);
                paraDict.Add(SystemFunModel.Field.QueryFunControllerNM.ToString(), model.QueryFunControllerNM);
                paraDict.Add(SystemFunModel.Field.QueryFunActionName.ToString(), model.QueryFunActionName);
                paraDict.Add(SystemFunModel.Field.QueryFunName.ToString(), model.QueryFunName);
                paraDict.Add(SystemFunModel.Field.QueryFunMenuSysID.ToString(), model.QueryFunMenuSysID);
                paraDict.Add(SystemFunModel.Field.QueryFunMenu.ToString(), model.QueryFunMenu);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion
            }

            if (model.GetSystemFunList(base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSystemFunList);
            }

            return View(model);
        }
    }
}