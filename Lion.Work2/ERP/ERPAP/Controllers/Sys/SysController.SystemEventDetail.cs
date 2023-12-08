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
        public async Task<ActionResult> SystemEventDetail(SystemEventDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add && await model.GetSystemEventDetail(AuthState.SessionData.UserID) == true)
                {
                    SetSystemAlertMessage(SysSystemEventDetail.SystemMsg_IsExistSystemEventDetail);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update) &&
                    await model.EditSystemEventDetail(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemEventDetail.EditSystemEventDetailResult_Failure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemEventDetailResult = await model.GetDeleteSystemEventDetailResult(AuthState.SessionData.UserID);
                    if (deleteSystemEventDetailResult == SystemEventDetailModel.EnumDeleteResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemEventDetail.DeleteSystemEventDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemEventDetailResult == SystemEventDetailModel.EnumDeleteResult.DataExist)
                    {
                        string message = string.Format(SysSystemEventDetail.DeleteSystemEventDetailResult_DataExist, SysSystemEventDetail.Label_EventTarget);
                        SetSystemAlertMessage(message);
                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemEvent", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, false, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEventDetail.SystemMsg_UnGetUserSystemByIdList);
            }
            else
            {
                if (model.SetHasSysID(model.SysID) == false)
                {
                    SetSystemErrorMessage(SysSystemEventDetail.SystemMsg_HasNotSysID);
                    return RedirectToAction("SystemEvent", "Sys");
                }
            }

            if (await model.GetSysSystemEventGroupByIdList(model.SysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEventDetail.SystemMsg_UnGetSysSystemEventGroupByIdList);
            }

            if (model.ExecAction == EnumActionType.Update && await model.GetSystemEventDetail(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(SysSystemEventDetail.SystemMsg_UnGetSystemEventDetail);
            }

            return View(model);
        }
    }
}