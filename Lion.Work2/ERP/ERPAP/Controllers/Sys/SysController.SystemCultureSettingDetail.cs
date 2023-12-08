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
        public async Task<ActionResult> SystemCultureSettingDetail(SystemCultureSettingDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Add ||
                    model.ExecAction == EnumActionType.Update ||
                    model.ExecAction == EnumActionType.Delete)
                {
                    if (TryValidatableObject(model))
                    {
                        if (model.ExecAction == EnumActionType.Add && await model.GetSystemCultureDetail(AuthState.SessionData.UserID) == true)
                        {
                            SetSystemAlertMessage(SysSystemCultureSettingDetail.SystemMsg_IsExistSystemCultureDetail);
                            result = false;
                        }

                        if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                        {
                            if (await model.EditSystemCultureDetail(AuthState.SessionData.UserID) == false)
                            {
                                SetSystemErrorMessage(SysSystemCultureSettingDetail.EditSystemCultureDetail_Failure);
                                result = false;
                            }
                        }

                        if (result && model.ExecAction == EnumActionType.Delete)
                        {
                            if (await model.DeleteSystemCultureDetail(AuthState.SessionData.UserID) == false)
                            {
                                SetSystemErrorMessage(SysSystemRoleConditionDetail.SystemMsg_DeleteSystemRoleCondition_Failure);
                                result = false;
                            }
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemCultureSetting", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.ExecAction == EnumActionType.Update && await model.GetSystemCultureDetail(AuthState.SessionData.UserID) == false)
            {
                SetSystemAlertMessage(SysSystemCultureSettingDetail.SystemMsg_UnGetSystemCultureDetail);
            }

            return View(model);
        }
    }
}