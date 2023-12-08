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
        public ActionResult SystemFunMenuDetail(SystemFunMenuDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add && model.GetSystemFunMenuDetail() == true)
                {
                    SetSystemAlertMessage(SysSystemFunMenuDetail.SystemMsg_AddSystemFunMenuDetailExist);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update) &&
                    model.GetEditSystemFunMenuDetailResult(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemFunMenuDetail.EditSystemFunMenuDetailResult_Failure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemFunMenuDetailResult = model.GetDeleteSystemFunMenuDetailResult();
                    if (deleteSystemFunMenuDetailResult == EntitySystemFunMenuDetail.EnumDeleteSystemFunMenuDetailResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemFunMenuDetail.DeleteSystemFunMenuDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemFunMenuDetailResult == EntitySystemFunMenuDetail.EnumDeleteSystemFunMenuDetailResult.DataExist)
                    {
                        string message = string.Format(SysSystemFunMenuDetail.DeleteSystemFunMenuDetailResult_DataExist, SysResource.TabText_SystemFun);
                        SetSystemAlertMessage(message);
                        result = false;
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

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunMenuDetail.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.ExecAction == EnumActionType.Update)
            {
                if (model.GetSystemFunMenuDetail() == false)
                {
                    SetSystemErrorMessage(SysSystemFunMenuDetail.SystemMsg_GetSystemFunMenuDetail);
                }
                else
                {
                    model.FunMenuNMZHTW = model.EntitySystemFunMenuDetail.FunMenuNMZHTW.GetValue();
                    model.FunMenuNMZHCN = model.EntitySystemFunMenuDetail.FunMenuNMZHCN.GetValue();
                    model.FunMenuNMENUS = model.EntitySystemFunMenuDetail.FunMenuNMENUS.GetValue();
                    model.FunMenuNMTHTH = model.EntitySystemFunMenuDetail.FunMenuNMTHTH.GetValue();
                    model.FunMenuNMJAJP = model.EntitySystemFunMenuDetail.FunMenuNMJAJP.GetValue();
                    model.DefaultMenuID = model.EntitySystemFunMenuDetail.DefaultMenuID.GetValue();
                    model.IsDisable = model.EntitySystemFunMenuDetail.IsDisable.GetValue();
                    model.SortOrder = model.EntitySystemFunMenuDetail.SortOrder.GetValue();
                }
            }

            return View(model);
        }
    }
}