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
        public ActionResult SystemEDIFlowLog()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemEDIFlowLogModel model = new SystemEDIFlowLogModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIFlowLogModel.Field.QuerySysID.ToString());
                model.QueryEDIFlowID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIFlowLogModel.Field.QueryEDIFlowID.ToString());
                model.DataDate = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIFlowLogModel.Field.DataDate.ToString());
                model.EDIDate = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIFlowLogModel.Field.EDIDate.ToString());
                model.EDINO = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIFlowLogModel.Field.EDINO.ToString());
            }
            #endregion

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDIFlowLog);

            model.GetStatusIDList(base.CultureID);

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowLog.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSystemSysIDList);
            }
            if (string.IsNullOrWhiteSpace(model.QuerySysID)) model.QuerySysID = model.EntitySysSystemSysIDList[0].SysID.StringValue();
            if (model.GetSysSystemEDIFlowList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (model.GetSystemEDIFlowLogList(base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowLog.SystemMsg_GetSystemEDIFlowLogList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemEDIFlowLog(SystemEDIFlowLogModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDIFlowLog);

            model.GetStatusIDList(base.CultureID);

            if (model.ExecAction != EnumActionType.Select)
            {
                model.FormReset();
            }

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowLog.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.GetSysSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJobDetail.SystemMsg_GetSystemSysIDList);
            }

            if (model.GetSysSystemEDIFlowList(model.QuerySysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (model.GetSystemEDIFlowLogList(base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowLog.SystemMsg_GetSystemEDIFlowLogList);
            }

            if (base.IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Select)
                {
                    #region - Set Cookie -
                    Dictionary<string, string> paraDict = new Dictionary<string, string>();
                    paraDict.Add(SystemEDIFlowLogModel.Field.QuerySysID.ToString(), model.QuerySysID);
                    paraDict.Add(SystemEDIFlowLogModel.Field.QueryEDIFlowID.ToString(), model.QueryEDIFlowID);
                    paraDict.Add(SystemEDIFlowLogModel.Field.EDIDate.ToString(), model.EDIDate);
                    paraDict.Add(SystemEDIFlowLogModel.Field.EDINO.ToString(), model.EDINO);
                    paraDict.Add(SystemEDIFlowLogModel.Field.DataDate.ToString(), model.DataDate);
                    AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                    #endregion
                }

                if (model.ExecAction == EnumActionType.Query == true)
                {
                    return RedirectToAction("SystemEDIFlow", "Sys");
                }
            }

            return View(model);
        }
    }
}