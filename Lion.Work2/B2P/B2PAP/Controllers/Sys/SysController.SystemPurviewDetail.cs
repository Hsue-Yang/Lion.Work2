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
        public ActionResult SystemPurviewDetail(SystemPurviewDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add && model.GetSystemPurviewDetail() == true)
                {
                    SetSystemAlertMessage(SysSystemPurviewDetail.SystemMsg_AddSystemPurviewDetailExist);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (model.GetEditSystemPurviewDetailResult(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSystemPurviewDetail.EditSystemPurviewDetailResult_Failure);
                        result = false;
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemPurviewDetailResult = model.GetDeleteSystemPurviewDetailResult();
                    if (deleteSystemPurviewDetailResult == EntitySystemPurviewDetail.EnumDeleteSystemPurviewDetailResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemPurviewDetail.DeleteSystemPurviewDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemPurviewDetailResult == EntitySystemPurviewDetail.EnumDeleteSystemPurviewDetailResult.DataExist)
                    {
                        string message = string.Format(SysSystemPurviewDetail.DeleteSystemPurviewDetailResult_DataExist, SysResource.TabText_SystemFun);
                        SetSystemAlertMessage(message);
                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("SystemPurview", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemPurviewDetail.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (model.ExecAction == EnumActionType.Update)
            {
                if (model.GetSystemPurviewDetail() == false)
                {
                    SetSystemErrorMessage(SysSystemPurviewDetail.SystemMsg_GetSystemPurviewDetail);
                }
                else
                {
                    model.PurviewNMZHTW = model.EntitySystemPurviewDetail.PurviewNMZHTW.GetValue();
                    model.PurviewNMZHCN = model.EntitySystemPurviewDetail.PurviewNMZHCN.GetValue();
                    model.PurviewNMENUS = model.EntitySystemPurviewDetail.PurviewNMENUS.GetValue();
                    model.PurviewNMTHTH = model.EntitySystemPurviewDetail.PurviewNMTHTH.GetValue();
                    model.PurviewNMJAJP = model.EntitySystemPurviewDetail.PurviewNMJAJP.GetValue();
                    model.SortOrder = model.EntitySystemPurviewDetail.SortOrder.GetValue();
                    model.Remark = model.EntitySystemPurviewDetail.Remark.GetValue();
                }
            }

            return View(model);
        }
    }
}