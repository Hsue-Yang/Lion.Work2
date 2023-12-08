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
        public ActionResult SystemEDIJobLog()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemEDIJobLogModel model = new SystemEDIJobLogModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIJobLogModel.Field.QuerySysID.ToString());
                model.QueryEDIFlowID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIJobLogModel.Field.QueryEDIFlowID.ToString());
                model.QueryEDIJobID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIJobLogModel.Field.QueryEDIJobID.ToString());
                model.DataDate = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIJobLogModel.Field.DataDate.ToString());
                model.EDIDate = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIJobLogModel.Field.EDIDate.ToString());
                model.EDINO = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIJobLogModel.Field.EDINO.ToString());
            }
            #endregion

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDIJobLog);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobLog.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSystemSysIDList);
            }

            if (string.IsNullOrWhiteSpace(model.QuerySysID))
            {
                model.QuerySysID = model.EntitySysSystemSysIDList[0].SysID.StringValue();
            }

            if (model.GetSysSystemEDIFlowList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (string.IsNullOrWhiteSpace(model.QueryEDIFlowID))
            {
                model.QueryEDIFlowID = model.EntitySysSystemEDIFlowList[0].EDIFlowID.StringValue();
            }

            if (model.GetSysSystemEDIJobList(model.QuerySysID, model.QueryEDIFlowID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (model.GetSystemEDIJobLogList(base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobLog.SystemMsg_GetSystemEDIJobLogList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemEDIJobLog(SystemEDIJobLogModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDIJobLog);

            if (model.ExecAction != EnumActionType.Select)
            {
                model.FormReset();
            }

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobLog.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSystemSysIDList);
            }

            if (model.GetSysSystemEDIFlowList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobLog.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (model.GetSysSystemEDIJobList(model.QuerySysID, model.QueryEDIFlowID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobLog.SystemMsg_GetSysSystemEDIJobList);
            }

            if (model.GetSystemEDIJobLogList(base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobLog.SystemMsg_GetSysSystemEDIJobList);
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemEDIJobLogModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemEDIJobLogModel.Field.QueryEDIFlowID.ToString(), model.QueryEDIFlowID);
                paraDict.Add(SystemEDIJobLogModel.Field.QueryEDIJobID.ToString(), model.QueryEDIJobID);
                paraDict.Add(SystemEDIJobLogModel.Field.DataDate.ToString(), model.DataDate);
                paraDict.Add(SystemEDIJobLogModel.Field.EDIDate.ToString(), model.EDIDate);
                paraDict.Add(SystemEDIJobLogModel.Field.EDINO.ToString(), model.EDINO);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Query == true)
            {
                return RedirectToAction("SystemEDIJob", "Sys");
            }

            return View(model);
        }
    }
}