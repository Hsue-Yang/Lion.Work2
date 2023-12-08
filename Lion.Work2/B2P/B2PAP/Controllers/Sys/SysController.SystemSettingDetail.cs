using System.Web.Mvc;
using B2PAP.Models.Sys;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemSettingDetail(SystemSettingDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add && model.GetSystemSettingDetail() == true)
                {
                    SetSystemAlertMessage(SysSystemSettingDetail.SystemMsg_AddSystemSettingDetailExist);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Update && model.GetUserSystemRole() == false)
                {
                    SetSystemErrorMessage(string.Format(SysSystemSettingDetail.SystemMsg_NoAuthorization, model.SysID));
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (model.GetEditSystemSettingDetailResult(model.ExecAction, AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSystemSettingDetail.EditSystemSettingDetailResult_Failure);
                        result = false;
                    }
                    else
                    {
                        if (model.ExecAction == EnumActionType.Add && string.IsNullOrWhiteSpace(model.IsOutsourcing))
                        {
                            base.ExecUserRoleLogWrite(model.UserID);
                        }
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemSettingDetailResult = model.GetDeleteSystemSettingDetailResult();
                    if (deleteSystemSettingDetailResult == EntitySystemSettingDetail.EnumDeleteSystemSettingDetailResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemSettingDetail.DeleteSystemSettingDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemSettingDetailResult == EntitySystemSettingDetail.EnumDeleteSystemSettingDetailResult.DataExist)
                    {
                        string message = string.Format(
                            SysSystemSettingDetail.DeleteSystemSettingDetailResult_DataExist,
                            string.Concat(new object[] 
                            {
                                SysResource.TabText_SystemEventGroup, ", ",
                                SysResource.TabText_SystemFunGroup, ", ",
                                SysResource.TabText_SystemFunMenu, ", ",
                                SysResource.TabText_SystemRole, ", ",
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
                if (model.GetSystemSettingDetail() == false)
                {
                    SetSystemErrorMessage(SysSystemSettingDetail.SystemMsg_GetSystemSettingDetail);
                }
                else
                {
                    model.UserID = model.EntitySystemSettingDetail.SysMANUserID.GetValue();
                    model.SysNMZHTW = model.EntitySystemSettingDetail.SysNMZHTW.GetValue();
                    model.SysNMZHCN = model.EntitySystemSettingDetail.SysNMZHCN.GetValue();
                    model.SysNMENUS = model.EntitySystemSettingDetail.SysNMENUS.GetValue();
                    model.SysNMTHTH = model.EntitySystemSettingDetail.SysNMTHTH.GetValue();
                    model.SysNMJAJP = model.EntitySystemSettingDetail.SysNMJAJP.GetValue();
                    model.SysIndexPath = model.EntitySystemSettingDetail.SysIndexPath.GetValue();
                    model.SysIconPath = model.EntitySystemSettingDetail.SysIconPath.GetValue();
                    model.SysKey = model.EntitySystemSettingDetail.SysKey.GetValue();
                    model.ENSysID = model.EntitySystemSettingDetail.ENSysID.GetValue();
                    model.IsOutsourcing = model.EntitySystemSettingDetail.IsOutsourcing.GetValue();
                    model.IsDisable = model.EntitySystemSettingDetail.IsDisable.GetValue();
                    model.SortOrder = model.EntitySystemSettingDetail.SortOrder.GetValue();
                }
            }

            return View(model);
        }
    }
}