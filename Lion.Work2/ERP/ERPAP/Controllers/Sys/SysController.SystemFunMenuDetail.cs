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
        public async Task<ActionResult> SystemFunMenuDetail(SystemFunMenuDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add && await model.GetSystemFunMenuDetail(AuthState.SessionData.UserID))
                {
                    SetSystemAlertMessage(SysSystemFunMenuDetail.SystemMsg_IsExistSystemFunMenuDetail);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (await model.EditSystemFunMenuDetail(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSystemFunMenuDetail.EditSystemFunMenuDetailResult_Failure);
                        result = false;
                    }
                    else if (GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemFunMenuEdit();
                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemMenu, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemFunMenuDetail.EditSystemFunMenuDetailResult_Failure);
                            result = false;
                        }
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemFunMenuDetailResult = await model.GetDeleteSystemFunMenuDetailResult(AuthState.SessionData.UserID);
                    if (deleteSystemFunMenuDetailResult == SystemFunMenuDetailModel.EnumDeleteSystemFunMenuDetailResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemFunMenuDetail.DeleteSystemFunMenuDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemFunMenuDetailResult == SystemFunMenuDetailModel.EnumDeleteSystemFunMenuDetailResult.DataExist)
                    {
                        string message = string.Format(SysSystemFunMenuDetail.DeleteSystemFunMenuDetailResult_DataExist, SysResource.TabText_SystemFun);
                        SetSystemAlertMessage(message);
                        result = false;
                    }
                    else if (deleteSystemFunMenuDetailResult == SystemFunMenuDetailModel.EnumDeleteSystemFunMenuDetailResult.Success && GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemFunMenuDelete();
                        if (base.ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemMenu, EnumEDIServiceEventID.Delete, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemFunMenuDetail.DeleteSystemFunMenuDetailResult_Failure);
                            result = false;
                        }
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemFunMenu", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunMenuDetail.SystemMsg_UnGetUserSystemByIdList);
            }

            if (model.ExecAction == EnumActionType.Update && await model.GetSystemFunMenuDetail(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(SysSystemFunMenuDetail.SystemMsg_UnGetSystemFunMenuDetail);
            }

            return View(model);
        }
    }
}