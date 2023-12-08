using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemEDIJob()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemEDIJobModel model = new SystemEDIJobModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIJobModel.Field.QuerySysID.ToString());
                model.QueryEDIFlowID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIJobModel.Field.QueryEDIFlowID.ToString());
                model.QueryEDIJobType = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEDIJobModel.Field.QueryEDIJobType.ToString());
            }
            #endregion

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDIJob);


            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (string.IsNullOrWhiteSpace(model.QuerySysID))
            {
                var sysUserSystemSysId = model.UserSystemByIdList.FirstOrDefault();
                if (sysUserSystemSysId != null)
                {
                    model.QuerySysID = sysUserSystemSysId.SysID;
                }
            }

            if (await model.GetSystemEDIFlowIDList(model.QuerySysID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (string.IsNullOrWhiteSpace(model.QueryEDIFlowID))
            {
                var sysSystemEDIFlow = model.EntitySysSystemEDIFlowList.FirstOrDefault();

                if (sysSystemEDIFlow != null)
                {
                    model.QueryEDIFlowID = sysSystemEDIFlow.EDIFlowID;
                }
            }

            if (await model.GetSystemEDIJobList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSystemEDIJobList);
            }

            if (await model.GetCMCodeTypeList(AuthState.SessionData.UserID, "0006", CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetEDIJobTypeList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemEDIJob(SystemEDIJobModel model, List<SystemEDIJobModel.EDIJobValue> ediJobSortValueList)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (base.IsPostBack && model.ExecAction == EnumActionType.Update)
            {
                if (await model.GetEDIJobSettingResult(AuthState.SessionData.UserID, ediJobSortValueList) == false)
                {
                    SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_SaveEDIJobSortOrderError);
                }
            }

            model.GetSysEDITabList(_BaseAPModel.EnumTabAction.SysSystemEDIJob);

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (await model.GetSystemEDIFlowIDList(model.QuerySysID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (await model.GetCMCodeTypeList(AuthState.SessionData.UserID, "0006", CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetEDIJobTypeList);
            }

            if (await model.GetSystemEDIJobList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSystemEDIJobList);
            }

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemEDIJobModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemEDIJobModel.Field.QueryEDIFlowID.ToString(), model.QueryEDIFlowID);
                paraDict.Add(SystemEDIJobModel.Field.QueryEDIJobType.ToString(), model.QueryEDIJobType);
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