using System.Collections.Generic;
using System.Web.Mvc;
using B2PAP.Models;
using LionTech.Entity.B2P.Sys;
using B2PAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult SystemEDIJob()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemEDIJobModel model = new SystemEDIJobModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIJobModel.Field.QuerySysID.ToString());
                model.QueryEDIFlowID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIJobModel.Field.QueryEDIFlowID.ToString());
            }
            #endregion

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDIJob);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemEDIFlowList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (model.GetSystemEDIJobList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSystemEDIJobList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemEDIJob(SystemEDIJobModel model, List<EntitySystemEDIJob.EDIJobValue> EDIJobValueList)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (base.IsPostBack && model.ExecAction == EnumActionType.Update)
            {
                if (model.GetEDIJobSettingResult(AuthState.SessionData.UserID, base.CultureID, EDIJobValueList) == false)
                {
                    SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_SaveEDIJobSortOrderError);
                }
            }

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDIJob);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemEDIFlowList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (model.GetSystemEDIJobList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSystemEDIJobList);
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemEDIJobModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemEDIJobModel.Field.QueryEDIFlowID.ToString(), model.QueryEDIFlowID);
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