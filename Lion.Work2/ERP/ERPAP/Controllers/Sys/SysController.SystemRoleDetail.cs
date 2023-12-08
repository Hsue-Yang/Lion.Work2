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
        public async Task<ActionResult> SystemRoleDetail(SystemRoleDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add && await model.GetSystemRoleDetail(AuthState.SessionData.UserID))
                {
                    SetSystemAlertMessage(SysSystemRoleDetail.SystemMsg_IsExistSystemRole);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (await model.EditSystemRole(model.ExecAction, AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                    {
                        SetSystemErrorMessage(SysSystemRoleDetail.EditSystemRoleDetailResult_Failure);
                        result = false;
                    }
                    else if (GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemRoleEdit();
                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemRole, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemRoleDetail.EditEDISystemRole_Failure);
                            result = false;
                        }
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    if (await model.DeleteSystemRole(AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                    {
                        SetSystemErrorMessage(SysSystemRoleDetail.DeleteSystemRoleDetailResult_Failure);
                        result = false;
                    }
                    else
                    {
                        if (GetEDIServiceDistributor())
                        {
                            string eventParaJsonString = model.GetEventParaSysSystemRoleDelete();
                            if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemRole, EnumEDIServiceEventID.Delete, eventParaJsonString) == null)
                            {
                                SetSystemErrorMessage(SysSystemRoleDetail.DeleteEDISystemRole_Failure);
                                result = false;
                            }
                        }
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemRole", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (await model.GetSystemRoleCategoryByIdList(model.SysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleDetail.SystemMsg_UnGetSystemRoleCategoryByIdList);
            }

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleDetail.SystemMsg_UnGetUserSystemByIdList);
            }

            if (model.ExecAction == EnumActionType.Update && await model.GetSystemRoleDetail(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleDetail.SystemMsg_UnGetSystemRoleDetail);
            }

            return View(model);
        }
    }
}