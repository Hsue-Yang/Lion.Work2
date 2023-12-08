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
        public async Task<ActionResult> SystemRoleCategoryDetail(SystemRoleCategoryDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add && await model.GetSystemRoleCategoryDetail(AuthState.SessionData.UserID))
                {
                    SetSystemAlertMessage(SysSystemRoleCategoryDetail.SystemMsg_IsExistSystemRoleCategoryDetail);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (await model.EditSystemRoleCategoryDetail(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSystemRoleCategoryDetail.EditSystemRoleCategoryDetailResult_Failure);
                        result = false;
                    }
                    else if (GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemRoleCategoryEdit();
                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemRoleCateg, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemRoleCategoryDetail.SystemMsg_IsExistSystemRoleCategoryDetail);
                            result = false;
                        }
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemRoleCategoryDetailResult = await model.DeleteSystemRoleCategoryDetail(AuthState.SessionData.UserID);
                    if (deleteSystemRoleCategoryDetailResult == SystemRoleCategoryDetailModel.EnumDeleteSystemRoleCategoryDetailResult.Failure)
                    {
                        SetSystemAlertMessage(SysSystemRoleCategoryDetail.DeleteSystemRoleCategoryDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemRoleCategoryDetailResult == SystemRoleCategoryDetailModel.EnumDeleteSystemRoleCategoryDetailResult.DataExist)
                    {
                        SetSystemAlertMessage(SysSystemRoleCategoryDetail.DeleteSystemRoleCategoryDetailResult_DataExist);
                        result = false;
                    }
                    else if (deleteSystemRoleCategoryDetailResult == SystemRoleCategoryDetailModel.EnumDeleteSystemRoleCategoryDetailResult.Success && GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemRoleCategoryDelete();
                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemRoleCateg, EnumEDIServiceEventID.Delete, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemRoleCategoryDetail.DeleteSystemRoleCategoryDetailResult_Failure);
                            result = false;
                        }
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemRoleCategory", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleCategoryDetail.SystemMsg_UnGetUserSystemByIdList);
            }

            if (model.ExecAction == EnumActionType.Update && await model.GetSystemRoleCategoryDetail(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleCategoryDetail.SystemMsg_UnGetSystemRoleCategoryDetail);
            }

            return View(model);
        }
    }
}