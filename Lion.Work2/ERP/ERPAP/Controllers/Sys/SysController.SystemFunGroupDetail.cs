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
        public async Task<ActionResult> SystemFunGroupDetail(SystemFunGroupDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunGroupDetail.SystemMsg_UnGetUserSystemByIdList);
            }

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add && await model.GetSystemFunGroupDetail(AuthState.SessionData.UserID))
                {
                    SetSystemAlertMessage(SysSystemFunGroupDetail.SystemMsg_IsExistSystemFunGroupDetail);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (await model.EditSystemFunGroupDetail(model.ExecAction, AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                    {
                        SetSystemErrorMessage(SysSystemFunGroupDetail.EditSystemFunGroupDetailResult_Failure);
                        result = false;
                    }
                    else if (GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemFunGroupEdit();
                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemFunGroup, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemFunGroupDetail.EditSystemFunGroupDetailResult_Failure);
                            result = false;
                        }
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemFunGroupDetailResult = await model.DeleteSystemFunGroupDetail(AuthState.SessionData.UserID, ClientIPAddress(), CultureID);
                    if (deleteSystemFunGroupDetailResult == SystemFunGroupDetailModel.EnumDeleteSystemFunGroupDetailResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemFunGroupDetail.DeleteSystemFunGroupDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemFunGroupDetailResult == SystemFunGroupDetailModel.EnumDeleteSystemFunGroupDetailResult.DataExist)
                    {
                        string message = string.Format(SysSystemFunGroupDetail.DeleteSystemFunGroupDetailResult_DataExist, SysResource.TabText_SystemFun);
                        SetSystemAlertMessage(message);
                        result = false;
                    }
                    else if (deleteSystemFunGroupDetailResult == SystemFunGroupDetailModel.EnumDeleteSystemFunGroupDetailResult.Success && GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemFunGroupDelete();
                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemFunGroup, EnumEDIServiceEventID.Delete, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemFunGroupDetail.DeleteSystemFunGroupDetailResult_Failure);
                            result = false;
                        }
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemFunGroup", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }         

            if (model.ExecAction == EnumActionType.Update && await model.GetSystemFunGroupDetail(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(SysSystemFunGroupDetail.SystemMsg_UnGetSystemFunGroupDetail);
            }

            return View(model);
        }
    }
}