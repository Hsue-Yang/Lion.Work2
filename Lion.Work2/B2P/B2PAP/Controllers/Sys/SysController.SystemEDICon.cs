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
        public ActionResult SystemEDICon()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemEDIConModel model = new SystemEDIConModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIConModel.Field.QuerySysID.ToString());
                model.QueryEDIFlowID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIConModel.Field.QueryEDIFlowID.ToString());
            }
            #endregion

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDICon); //TabText

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDICon.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemEDIFlowList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDICon.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (model.GetSystemEDIConList( base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDICon.SystemMsg_GetSystemEDIConList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemEDICon(SystemEDIConModel model, List<EntitySystemEDICon.EDIConValue> EDIConValueList)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (base.IsPostBack && model.ExecAction == EnumActionType.Update)
            {
                if (model.GetEDIConSettingResult(AuthState.SessionData.UserID, base.CultureID, EDIConValueList) == false)
                {
                    SetSystemErrorMessage(SysSystemEDICon.SystemMsg_SaveEDIConSortOrderError);
                }
            }

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDICon);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDICon.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemEDIFlowList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDICon.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (model.GetSystemEDIConList( base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDICon.SystemMsg_GetSystemEDIConList);
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemEDIConModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemEDIConModel.Field.QueryEDIFlowID.ToString(), model.QueryEDIFlowID);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Query == true)
            {
                return RedirectToAction("SystemEDIFlow", "Sys");
            }

            return View(model);
        }
    }
}

