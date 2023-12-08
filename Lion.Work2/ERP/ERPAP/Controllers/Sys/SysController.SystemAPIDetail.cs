using ERPAP.Models.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;
using System.Threading.Tasks;
using System.Web.Mvc;
using static ERPAP.Models._BaseAPModel;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemAPIDetail(SystemAPIDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add && await model.GetSystemAPIDetail(AuthState.SessionData.UserID) == true)
                {
                    SetSystemAlertMessage(SysSystemAPIDetail.SystemMsg_IsExistSystemAPIDetail);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (await model.EditSystemAPIDetail(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSystemAPIDetail.EditSystemAPIDetailResult_Failure);
                        result = false;
                    }
                    else if (GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemAPIEdit();
                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemAPI, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemAPIDetail.EditSystemAPIDetailResult_Failure);
                            result = false;
                        }
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemAPIDetailResult = await model.GetDeleteSystemAPIDetailResult(AuthState.SessionData.UserID);
                    if (deleteSystemAPIDetailResult == SystemAPIDetailModel.EnumDeleteSystemAPIDetailResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemAPIDetail.DeleteSystemAPIDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemAPIDetailResult == SystemAPIDetailModel.EnumDeleteSystemAPIDetailResult.DataExist)
                    {
                        string message = SysSystemAPIDetail.DeleteSystemAPIDetailResult_DataExist;
                        SetSystemAlertMessage(message);
                        result = false;
                    }
                    else if (deleteSystemAPIDetailResult == SystemAPIDetailModel.EnumDeleteSystemAPIDetailResult.Success && GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemAPIDelete();
                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemAPI, EnumEDIServiceEventID.Delete, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemAPIDetail.DeleteSystemAPIDetailResult_Failure);
                            result = false;
                        }
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemAPI", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemAPIDetail.SystemMsg_UnGetUserSystemByIdList);
            }
            else
            {
                if (model.SetHasSysID(model.SysID) == false)
                {
                    SetSystemErrorMessage(SysSystemAPIDetail.SystemMsg_HasNotSysID);
                    return RedirectToAction("SystemAPI", "Sys");
                }
            }

            Task<bool> getSystemAPIGroupByIdList = model.GetSystemAPIGroupByIdList(model.SysID, AuthState.SessionData.UserID, CultureID);
            Task<bool> getAPIReturnTypeList = model.GetCMCodeTypeList(AuthState.SessionData.UserID, Common.GetEnumDesc(EnumCMCodeKind.APIReturnType), CultureID);
            Task<bool> getSystemAPIRoleList = model.GetSystemAPIRoleList(AuthState.SessionData.UserID, CultureID);

            await Task.WhenAll(getSystemAPIGroupByIdList, getAPIReturnTypeList, getSystemAPIRoleList);

            if (getSystemAPIGroupByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPIDetail.SystemMsg_UnGetSystemAPIGroupByIdList);
            }

            if (getAPIReturnTypeList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPIDetail.SystemMsg_UnGetAPIReturnTypeList);
            }

            if (getSystemAPIRoleList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPIDetail.SystemMsg_UnGetSystemAPIRoleList);
            }

            if (model.ExecAction == EnumActionType.Update &&
                await model.GetSystemAPIDetail(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(SysSystemAPIDetail.SystemMsg_UnGetSystemAPIDetail);
            }

            return View(model);
        }
    }
}