using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemEDIFlowDetail(SystemEDIFlowDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;
            bool result = true;

            if (base.IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add &&
                    await model.GetSystemEDIFlowDetail(AuthState.SessionData.UserID))
                {
                    SetSystemAlertMessage(SysSystemEDIFlowDetail.SystemMsg_AddSystemEDIFlowDetailExist);
                    result = false;
                }

                if (result &&
                    (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (TryValidatableObject(model))
                    {
                        if (await model.GetEditSystemEDIFlowDetailResult(AuthState.SessionData.UserID) == false)
                        {
                            SetSystemErrorMessage(SysSystemEDIFlowDetail.EditSystemEDIFlowDetailResult_Failure);
                            result = false;
                        }
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemEDIFlowDetailResult = await model.GetDeleteSystemEDIFlowDetailResult(AuthState.SessionData.UserID);
                    if (deleteSystemEDIFlowDetailResult == SystemEDIFlowDetailModel.EnumDeleteSystemEDIFlowDetailResult.Failure)
                    {
                        SetSystemAlertMessage(SysSystemEDIFlowDetail.DeleteSystemEDIFlowDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemEDIFlowDetailResult == SystemEDIFlowDetailModel.EnumDeleteSystemEDIFlowDetailResult.DataExist)
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

            if (await model.GetSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowDetail.SystemMsg_GetSystemSysIDList);
            }

            if (await model.GetSystemEDIFlowIDList(model.SysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIJob.SystemMsg_GetSysSystemEDIFlowList);
            }

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowDetail.SystemMsg_GetSysUserSystemSysIDList);
            }
            else
            {
                if (model.IsHasSysAuth() == false)
                {
                    SetSystemErrorMessage(SysSystemEDIFlowDetail.SystemMsg_SetHasSysID);
                    return RedirectToAction("SystemEDIFlow", "Sys");
                }
            }

            if (await model.GetCMCodeTypeList(AuthState.SessionData.UserID, "0005", base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIFlowDetail.SystemMsg_GetSCHFrequencyList);
            }

            if (model.ExecAction == EnumActionType.Add)
            {
                await model.GetSortOrder(AuthState.SessionData.UserID);
            }

            if (model.ExecAction == EnumActionType.Update || model.ExecAction == EnumActionType.Copy)
            {
                if (await model.GetSystemEDIFlowDetail(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemEDIFlowDetail.SystemMsg_GetSystemEDIFlowDetail);
                }
            }
            return View(model);
        }
    }
}