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
        public ActionResult SystemFunGroupDetail(SystemFunGroupDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add && model.GetSystemFunGroupDetail() == true)
                {
                    SetSystemAlertMessage(SysSystemFunGroupDetail.SystemMsg_AddSystemFunGroupDetailExist);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update) &&
                    model.GetEditSystemFunGroupDetailResult(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemFunGroupDetail.EditSystemFunGroupDetailResult_Failure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemFunGroupDetailResult = model.GetDeleteSystemFunGroupDetailResult();
                    if (deleteSystemFunGroupDetailResult == EntitySystemFunGroupDetail.EnumDeleteSystemFunGroupDetailResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemFunGroupDetail.DeleteSystemFunGroupDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemFunGroupDetailResult == EntitySystemFunGroupDetail.EnumDeleteSystemFunGroupDetailResult.DataExist)
                    {
                        string message = string.Format(SysSystemFunGroupDetail.DeleteSystemFunGroupDetailResult_DataExist, SysResource.TabText_SystemFun);
                        SetSystemAlertMessage(message);
                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemFunGroup", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunGroupDetail.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.ExecAction == EnumActionType.Update)
            {
                if (model.GetSystemFunGroupDetail() == false)
                {
                    SetSystemErrorMessage(SysSystemFunGroupDetail.SystemMsg_GetSystemFunGroupDetail);
                }
                else
                {
                    model.FunGroupZHTW = model.EntitySystemFunGroupDetail.FunGroupZHTW.GetValue();
                    model.FunGroupZHCN = model.EntitySystemFunGroupDetail.FunGroupZHCN.GetValue();
                    model.FunGroupENUS = model.EntitySystemFunGroupDetail.FunGroupENUS.GetValue();
                    model.FunGroupTHTH = model.EntitySystemFunGroupDetail.FunGroupTHTH.GetValue();
                    model.FunGroupJAJP = model.EntitySystemFunGroupDetail.FunGroupJAJP.GetValue();
                    model.SortOrder = model.EntitySystemFunGroupDetail.SortOrder.GetValue();
                }
            }

            return View(model);
        }
    }
}