using System.Web.Mvc;
using B2PAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemRoleDetail(SystemRoleDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add && model.GetSystemRoleDetail() == true)
                {
                    SetSystemAlertMessage(SysSystemRoleDetail.SystemMsg_AddSystemRoleDetailExist);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (model.GetEditSystemRoleDetailResult(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSystemRoleDetail.EditSystemRoleDetailResult_Failure);
                        result = false;
                    }
                    else
                    {
                        string apiParaJsonString = model.GetAPIParaSCMAPB2PSettingB2PSystemRoleEntity().SerializeToJson();
                        string apiReturnString = base.ExecAPIService(EnumAppSettingAPIKey.APISCMAPB2PSettingB2PSystemRoleEditEventURL, apiParaJsonString);
                        if (apiReturnString == null || apiReturnString != bool.TrueString)
                        {
                            SetSystemErrorMessage(SysSystemRoleDetail.EditSystemRoleDetailResult_Failure);
                            result = false;
                        }
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    if (model.GetDeleteSystemRoleDetailResult() == false)
                    {
                        SetSystemErrorMessage(SysSystemRoleDetail.DeleteSystemRoleDetailResult_Failure);
                        result = false;
                    }
                    else
                    {
                        string apiParaJsonString = model.GetAPIParaSCMAPB2PSettingB2PSystemRoleEntity().SerializeToJson();
                        string apiReturnString = base.ExecAPIService(EnumAppSettingAPIKey.APISCMAPB2PSettingB2PSystemRoleDeleteEventURL, apiParaJsonString);
                        if (apiReturnString == null || apiReturnString != bool.TrueString)
                        {
                            SetSystemErrorMessage(SysSystemRoleDetail.DeleteSystemRoleDetailResult_Failure);
                            result = false;
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

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleDetail.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.ExecAction == EnumActionType.Update)
            {
                if (model.GetSystemRoleDetail() == false)
                {
                    SetSystemErrorMessage(SysSystemRoleDetail.SystemMsg_GetSystemRoleDetail);
                }
                else
                {
                    model.RoleNMZHTW = model.EntitySystemRoleDetail.RoleNMZHTW.GetValue();
                    model.RoleNMZHCN = model.EntitySystemRoleDetail.RoleNMZHCN.GetValue();
                    model.RoleNMENUS = model.EntitySystemRoleDetail.RoleNMENUS.GetValue();
                    model.RoleNMTHTH = model.EntitySystemRoleDetail.RoleNMTHTH.GetValue();
                    model.RoleNMJAJP = model.EntitySystemRoleDetail.RoleNMJAJP.GetValue();
                }
            }

            return View(model);
        }
    }
}