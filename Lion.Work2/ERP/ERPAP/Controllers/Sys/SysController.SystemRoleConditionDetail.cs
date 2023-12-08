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
        public async Task<ActionResult> SystemRoleConditionDetail(SystemRoleConditionDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (await model.GetSysSystemRoleIDList(model.SysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleConditionDetail.SystemMsg_GetSysSystemRoleIDList_Failure);
            }

            if (model.GetSysNM(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleConditionDetail.SystemMsg_GetSysNM_Failure);
            }

            if (IsPostBack == false)
            {
                model.FormReset();

                if (await model.GetSysSystemRoleConditionDetail(CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemRoleConditionDetail.SystemMsg_GetSysSystemRoleConditionDetail_Failure);
                }
                if (model.SetSystemRoleConditionFilterJsonString(CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemRoleConditionDetail.SystemMsg_SetSystemRoleConditionFilterJsonString_Failure);
                }
            }
            else
            {
                bool result = true;

                if (model.ExecAction == EnumActionType.Add ||
                    model.ExecAction == EnumActionType.Update ||
                    model.ExecAction == EnumActionType.Delete)
                {
                    if (TryValidatableObject(model))
                    {
                        if (model.ExecAction == EnumActionType.Add ||
                            model.ExecAction == EnumActionType.Update)
                        {
                            if (await model.EditSysSystemRoleCondition(CultureID, AuthState.SessionData.UserID, AuthState.SessionData.UserNM) == false)
                            {
                                SetSystemErrorMessage(SysSystemRoleConditionDetail.SystemMsg_EditSystemRoleCondition_Failure);
                                result = false;
                            }
                            if (result &&
                                model.ConditionType == SystemRoleConditionDetailModel.EnumConditionType.AccordingCondition.ToString() &&
                                model.RecordLogSysSystemRoleCondition(CultureID, AuthState.SessionData.UserID, ClientIPAddress()) == false)
                            {
                                SetSystemErrorMessage(SysSystemRoleConditionDetail.SystemMsg_AddSystemRoleConditionLogData_Failure);
                                result = false;
                            }
                        }

                        if (model.ExecAction == EnumActionType.Delete)
                        {
                            if (result && await model.GetSysSystemRoleConditionDetail(CultureID) == false)
                            {
                                SetSystemErrorMessage(SysSystemRoleConditionDetail.SystemMsg_GetSysSystemRoleConditionDetail_Failure);
                                result = false;
                            }
                            if (result && model.RecordLogSysSystemRoleCondition(CultureID, AuthState.SessionData.UserID, ClientIPAddress()) == false)
                            {
                                SetSystemErrorMessage(SysSystemRoleConditionDetail.SystemMsg_AddSystemRoleConditionLogData_Failure);
                                result = false;
                            }
                            if (result && await model.DeleteSystemRoleCondition(CultureID, AuthState.SessionData.UserID) == false)
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
                    return RedirectToAction("SystemRoleCondition");
                }
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemRoleConditionDetail")]
        public ActionResult GetRoleConditionSyntax(SystemRoleConditionDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            return Json(model.GetRoleConditionSyntaxFormat(model.SystemRoleConditionGroupRule));
        }
    }
}