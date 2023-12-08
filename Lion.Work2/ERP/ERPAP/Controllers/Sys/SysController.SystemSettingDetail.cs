using ERPAP.Models.Sys;
using LionTech.Entity.ERP;
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
        public async Task<ActionResult> SystemSettingDetail(SystemSettingDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (model.CheckIsITManager(AuthState.SessionData.UserID, EnumSystemID.ERPAP.ToString()) == false)
            {
                SetSystemErrorMessage(SysResource.SystemMsg_CheckIsITManager_Failure);
            }

            bool result = true;

            if (IsPostBack)
            {
                if ((model.ExecAction == EnumActionType.Add ||
                     model.ExecAction == EnumActionType.Update ||
                     model.ExecAction == EnumActionType.Delete) &&
                    model.IsITManager == false)
                {
                    SetSystemErrorMessage(SysResource.SystemMsg_NoEditPermissions);
                    result = false;
                }

                if (result &&
                    model.ExecAction == EnumActionType.Add &&
                    await model.IsExistSystem(AuthState.SessionData.UserID, model.SysID))
                {
                    SetSystemAlertMessage(SysSystemSettingDetail.SystemMsg_AddSystemSettingDetailExist);
                    result = false;
                }

                if (result &&
                    (model.ExecAction == EnumActionType.Add ||
                     model.ExecAction == EnumActionType.Update))
                {
                    if (await model.EditSystemSetting(AuthState.SessionData.UserID, ClientIPAddress()) == false)
                    {
                        SetSystemErrorMessage(SysSystemSettingDetail.EditSystemSettingDetailResult_Failure);
                        result = false;
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteResult = await model.DeleteSystemSetting(AuthState.SessionData.UserID, model.SysID);
                    if (deleteResult == SystemSettingDetailModel.EnumDeleteSystemSettingDetailResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemSettingDetail.DeleteSystemSettingDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteResult == SystemSettingDetailModel.EnumDeleteSystemSettingDetailResult.DataExist)
                    {
                        string message = string.Format(
                            SysSystemSettingDetail.DeleteSystemSettingDetailResult_DataExist,
                            string.Concat(new object[]
                            {
                                SysResource.TabText_SystemEDIFlow, ", ",
                                SysResource.TabText_SystemWorkFlowGroup, ", ",
                                SysResource.TabText_SystemEventGroup, ", ",
                                SysResource.TabText_SystemAPIGroup, ", ",
                                SysResource.TabText_SystemFunGroup, ", ",
                                SysResource.TabText_SystemFunMenu, ", ",
                                SysResource.TabText_SystemRole, ", ",
                                SysResource.TabText_SystemPurview, ", ",
                                SysResource.TabText_SystemSub, ", ",
                                SysResource.TabText_UserSystem
                            })
                        );
                        SetSystemAlertMessage(message);
                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemSetting", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.ExecAction == EnumActionType.Update)
            {
                if (await model.GetSystemMain(AuthState.SessionData.UserID, model.SysID) == false)
                {
                    SetSystemErrorMessage(SysSystemSettingDetail.SystemMsg_GetSystemSettingDetail);
                }
            }

            return View(model);
        }
    }
}