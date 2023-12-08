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
        public async Task<ActionResult> SystemEventGroupDetail(SystemEventGroupDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add && await model.GetSystemEventGroupDetail(AuthState.SessionData.UserID) == true)
                {
                    SetSystemAlertMessage(SysSystemEventGroupDetail.SystemMsg_IsExistSystemEventGroupDetail);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update) && await model.EditSystemEventGroupDetail(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemEventGroupDetail.EditSystemEventGroupDetailResult_Failure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemEventGroupDetailResult = await model.GetDeleteSystemEventGroupDetailResult(AuthState.SessionData.UserID);
                    if (deleteSystemEventGroupDetailResult == SystemEventGroupDetailModel.EnumDeleteSystemEventGroupDetailResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemEventGroupDetail.DeleteSystemEventGroupDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemEventGroupDetailResult == SystemEventGroupDetailModel.EnumDeleteSystemEventGroupDetailResult.DataExist)
                    {
                        string message = string.Format(SysSystemEventGroupDetail.DeleteSystemEventGroupDetailResult_DataExist, SysResource.TabText_SystemEvent);
                        SetSystemAlertMessage(message);
                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemEventGroup", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, false, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEventGroupDetail.SystemMsg_UnGetUserSystemByIdList);
            }

            if (model.ExecAction == EnumActionType.Update && await model.GetSystemEventGroupDetail(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(SysSystemEventGroupDetail.SystemMsg_UnGetSystemEventGroupDetail);
            }

            return View(model);
        }
    }
}