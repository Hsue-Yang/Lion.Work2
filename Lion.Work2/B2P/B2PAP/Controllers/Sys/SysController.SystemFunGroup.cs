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
        public ActionResult SystemFunGroup()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemFunGroupModel model = new SystemFunGroupModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunGroupModel.Field.QuerySysID.ToString());
                model.QueryFunControllerID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunGroupModel.Field.QueryFunControllerID.ToString());
            }
            #endregion

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemFunGroup);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunGroup.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemFunControllerIDList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunGroup.SystemMsg_GetSysSystemFunControllerIDList);
            }

            if (model.GetSystemFunGroupList(base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunGroup.SystemMsg_GetSystemFunGroupList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemFunGroup(SystemFunGroupModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemFunGroup);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunGroup.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemFunControllerIDList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunGroup.SystemMsg_GetSysSystemFunControllerIDList);
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemFunGroupModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemFunGroupModel.Field.QueryFunControllerID.ToString(), model.QueryFunControllerID);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion

                if (model.GetSystemFunGroupList(base.PageSize, base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemFunGroup.SystemMsg_GetSystemFunGroupList);
                }
            }

            return View(model);
        }
    }
}