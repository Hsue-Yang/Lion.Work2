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
        public ActionResult SystemRoleGroupDetail(SystemRoleGroupDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add && model.GetSystemRoleGroupDetail() == true)
                {
                    SetSystemAlertMessage(SysSystemRoleGroupDetail.SystemMsg_AddSystemRoleGroupDetailExist);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (model.GetEditSystemRoleGroupDetailResult(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSystemRoleGroupDetail.EditSystemRoleGroupDetailResult_Failure);
                        result = false;
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemRoleGroupDetailResult = model.GetDeleteSystemRoleGroupDetailResult();
                    if (deleteSystemRoleGroupDetailResult == EntitySystemRoleGroupDetail.EnumDeleteSystemRoleGroupDetailResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemRoleGroupDetail.DeleteSystemRoleGroupDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemRoleGroupDetailResult == EntitySystemRoleGroupDetail.EnumDeleteSystemRoleGroupDetailResult.DataExist)
                    {
                        string message = string.Format(SysSystemRoleGroupDetail.DeleteSystemRoleGroupDetailResult_DataExist, SysSystemRoleGroupDetail.Text_UserRoleFunDetail);
                        SetSystemAlertMessage(message);
                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemRoleGroup", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.ExecAction == EnumActionType.Update)
            {
                if (model.GetSystemRoleGroupDetail() == false)
                {
                    SetSystemErrorMessage(SysSystemRoleGroupDetail.SystemMsg_GetSystemRoleGroupDetail); //無法取得EDIJob清單明細列表
                }
                else
                {
                    model.RoleGroupNMZhTW = model.EntitySystemRoleGroupDetail.RoleGroupNMZhTW.GetValue();
                    model.RoleGroupNMZhCN = model.EntitySystemRoleGroupDetail.RoleGroupNMZhCN.GetValue();
                    model.RoleGroupNMEnUS = model.EntitySystemRoleGroupDetail.RoleGroupNMEnUS.GetValue();
                    model.RoleGroupNMThTH = model.EntitySystemRoleGroupDetail.RoleGroupNMThTH.GetValue();
                    model.RoleGroupNMJaJP = model.EntitySystemRoleGroupDetail.RoleGroupNMJaJP.GetValue();
                    model.SortOrder = model.EntitySystemRoleGroupDetail.SortOrder.GetValue();
                    model.Remark = model.EntitySystemRoleGroupDetail.Remark.GetValue();
                }
            }

            return View(model);
        }
    }
}