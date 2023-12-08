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
        public async Task<ActionResult> SystemPurviewDetail(SystemPurviewDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add && await model.GetSystemPurviewDetail(AuthState.SessionData.UserID))
                {
                    SetSystemAlertMessage(SysSystemPurviewDetail.SystemMsg_IsExistSystemPurviewDetail);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (await model.EditSystemPurviewDetail(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSystemPurviewDetail.EditSystemPurviewDetailResult_Failure);
                        result = false;
                    }
                    else if (GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemPurviewEdit();
                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemPurview, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemPurviewDetail.EditSystemPurviewDetailResult_Failure);
                            result = false;
                        }
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemPurviewDetailResult = await model.DeleteSystemPurviewDetail(AuthState.SessionData.UserID);
                    if (deleteSystemPurviewDetailResult == SystemPurviewDetailModel.EnumDeleteSystemPurviewDetailResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemPurviewDetail.DeleteSystemPurviewDetailResult_Failure);
                    }
                    else if (deleteSystemPurviewDetailResult == SystemPurviewDetailModel.EnumDeleteSystemPurviewDetailResult.DataExist)
                    {
                        string message = string.Format(SysSystemPurviewDetail.DeleteSystemPurviewDetailResult_DataExist, SysResource.TabText_SystemFun);
                        SetSystemAlertMessage(message);
                    }
                    else if(deleteSystemPurviewDetailResult == SystemPurviewDetailModel.EnumDeleteSystemPurviewDetailResult.Success && GetEDIServiceDistributor())
                    {
                        string eventParaJsonString = model.GetEventParaSysSystemPurviewDelete();
                        if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemPurview, EnumEDIServiceEventID.Delete, eventParaJsonString) == null)
                        {
                            SetSystemErrorMessage(SysSystemPurviewDetail.DeleteSystemPurviewDetailResult_Failure);
                        }
                    }
                }
               
                return RedirectToAction("SystemPurview", "Sys");
            }

            model.FormReset();

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemPurviewDetail.SystemMsg_UnGetUserSystemByIdList);
            }

            if (model.ExecAction == EnumActionType.Update && await model.GetSystemPurviewDetail(AuthState.SessionData.UserID) == false)
            {   
                SetSystemErrorMessage(SysSystemPurviewDetail.SystemMsg_UnGetSystemPurviewDetail);
            }

            return View(model);
        }
    }
}