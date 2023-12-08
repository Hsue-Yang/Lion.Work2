using System.Web.Mvc;
using B2PAP.Models.Sys;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemEDIFlowDetail(SystemEDIFlowDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add && model.GetSystemEDIFlowDetail() == true)
                {
                    SetSystemAlertMessage(SysSystemEDIFlowDetail.SystemMsg_AddSystemEDIFlowDetailExist);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update) &&
                    model.GetEditSystemEDIFlowDetailResult(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemEDIFlowDetail.EditSystemEDIFlowDetailResult_Failure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemEDIFlowDetailResult = model.GetDeleteSystemEDIFlowDetailResult();
                    if (deleteSystemEDIFlowDetailResult == EntitySystemEDIFlowDetail.EnumDeleteSystemEDIFlowDetailResult.Failure)
                    {
                        SetSystemAlertMessage(SysSystemEDIFlowDetail.DeleteSystemEDIFlowDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemEDIFlowDetailResult == EntitySystemEDIFlowDetail.EnumDeleteSystemEDIFlowDetailResult.DataExist)
                    {
                        SetSystemAlertMessage(SysSystemEDIFlowDetail.DeleteSystemEDIFlowDetailResult_DataExist);
                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemEDIFlow", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetSysSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowDetail.SystemMsg_GetSystemSysIDList);
            }

            if (model.GetSysSystemEDIFlowList(model.SysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowDetail.SystemMsg_GetSysUserSystemSysIDList);
            }
            else
            {
                if (!model.SetHasSysID())
                {
                    SetSystemErrorMessage(SysSystemEDIFlowDetail.SystemMsg_SetHasSysID);
                    return RedirectToAction("SystemEDIFlow", "Sys");
                }
            }

            if (model.GetSCHFrequecnyList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowDetail.SystemMsg_GetSCHFrequecnyList);
            }

            if (model.GetSCHIntervalTimeList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowDetail.SystemMsg_GetSCHIntervalTimeList);
            }

            if (model.ExecAction == EnumActionType.Add)
            {
                model.GetSortOrder();

            }
            if (model.ExecAction == EnumActionType.Update)
            {
                if (model.GetSystemEDIFlowDetail() == false)
                {
                    SetSystemErrorMessage(SysSystemEDIFlowDetail.SystemMsg_GetSystemEDIFlowDetail);
                }
                else
                {
                    model.EDIFlowZHTW = model.EntitySystemEDIFlowDetail.EDIFlowZHTW.GetValue();
                    model.EDIFlowZHCN = model.EntitySystemEDIFlowDetail.EDIFlowZHCN.GetValue();
                    model.EDIFlowENUS = model.EntitySystemEDIFlowDetail.EDIFlowENUS.GetValue();
                    model.EDIFlowTHTH = model.EntitySystemEDIFlowDetail.EDIFlowTHTH.GetValue();
                    model.EDIFlowJAJP = model.EntitySystemEDIFlowDetail.EDIFlowJAJP.GetValue();
                    model.SCHFrequency = model.EntitySystemEDIFlowDetail.SCHFrequency.GetValue();
                    model.SCHStartDate = model.EntitySystemEDIFlowDetail.SCHStartDate.GetValue();
                    model.SCHStartTime = model.EntitySystemEDIFlowDetail.SCHStartTime.GetValue();
                    model.SCHIntervalTime = model.EntitySystemEDIFlowDetail.SCHIntervalTime.GetValue().ToString();
                    model.SCHEndTime = model.EntitySystemEDIFlowDetail.SCHEndTime.GetValue();
                    model.SCHDataDelay = model.EntitySystemEDIFlowDetail.SCHDataDelay.GetValue().ToString();
                    model.PATHSCmd = model.EntitySystemEDIFlowDetail.PATHSCmd.GetValue();
                    model.PATHSDat = model.EntitySystemEDIFlowDetail.PATHSDat.GetValue();
                    model.PATHSSrc = model.EntitySystemEDIFlowDetail.PATHSSrc.GetValue();
                    model.PATHSRes = model.EntitySystemEDIFlowDetail.PATHSRes.GetValue();
                    model.PATHSBad = model.EntitySystemEDIFlowDetail.PATHSBad.GetValue();
                    model.PATHSLog = model.EntitySystemEDIFlowDetail.PATHSLog.GetValue();
                    model.PATHSFlowXml = model.EntitySystemEDIFlowDetail.PATHSFlowXml.GetValue();
                    model.PATHSFlowCmd = model.EntitySystemEDIFlowDetail.PATHSFlowCmd.GetValue();
                    model.PATHSZipDat = model.EntitySystemEDIFlowDetail.PATHSZipDat.GetValue();
                    model.PATHSException = model.EntitySystemEDIFlowDetail.PATHSException.GetValue();
                    model.PATHSSummary = model.EntitySystemEDIFlowDetail.PATHSSummary.GetValue();
                    model.SortOrder = model.EntitySystemEDIFlowDetail.SortOrder.GetValue();
                }
            }

            return View(model);
        }
    }
}