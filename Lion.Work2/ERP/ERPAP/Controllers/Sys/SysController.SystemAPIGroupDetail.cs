using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemAPIGroupDetail(SystemAPIGroupDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add &&
                    await model.GetSystemAPIGroupDetail(AuthState.SessionData.UserID) == true)
                {
                    SetSystemAlertMessage(SysSystemAPIGroupDetail.SystemMsg_IsExistSystemAPIGroupDetail);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (await model.EditSystemAPIGroupDetail(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSystemAPIGroupDetail.EditSystemAPIGroupDetailResult_Failure);
                        result = false;
                    }
                    else if (GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemAPIGroupEdit();
                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemAPIGroup, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemAPIGroupDetail.EditSystemAPIGroupDetailResult_Failure);
                            result = false;
                        }
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemAPIGroupDetailResult = await model.GetDeleteSystemAPIGroupDetailResult(AuthState.SessionData.UserID);
                    if (deleteSystemAPIGroupDetailResult == SystemAPIGroupDetailModel.EnumDeleteSystemAPIGroupDetailResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemAPIGroupDetail.DeleteSystemAPIGroupDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemAPIGroupDetailResult == SystemAPIGroupDetailModel.EnumDeleteSystemAPIGroupDetailResult.DataExist)
                    {
                        string message = string.Format(SysSystemAPIGroupDetail.DeleteSystemAPIGroupDetailResult_DataExist, SysResource.TabText_SystemAPI);
                        SetSystemAlertMessage(message);
                        result = false;
                    }
                    else if (deleteSystemAPIGroupDetailResult == SystemAPIGroupDetailModel.EnumDeleteSystemAPIGroupDetailResult.Success && GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemAPIGroupDelete();
                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemAPIGroup, EnumEDIServiceEventID.Delete, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemAPIGroupDetail.DeleteSystemAPIGroupDetailResult_Failure);
                            result = false;
                        }
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemAPIGroup", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemAPIGroupDetail.SystemMsg_UnGetUserSystemByIdList);
            }

            if (model.ExecAction == EnumActionType.Update &&
                await model.GetSystemAPIGroupDetail(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(SysSystemAPIGroupDetail.SystemMsg_UnGetSystemAPIGroupDetail);
            }

            return View(model);
        }
    }
}