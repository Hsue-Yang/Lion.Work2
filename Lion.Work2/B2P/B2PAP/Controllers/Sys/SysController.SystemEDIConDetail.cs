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
        public ActionResult SystemEDIConDetail(SystemEDIConDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && model.ExecAction == EnumActionType.Add && model.GetSystemEDIConDetail() == true)
                {
                    SetSystemAlertMessage(SysSystemEDIConDetail.SystemMsg_AddSystemEDIConDetailExist);
                    result = false;
                }

                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update) &&
                    model.GetEditSystemEDIConDetailResult(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysSystemEDIConDetail.EditSystemEDIConDetailResult_Failure);
                    result = false;
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteSystemEDIConDetailResult = model.GetDeleteSystemEDIConDetailResult();
                    if (deleteSystemEDIConDetailResult == EntitySystemEDIConDetail.EnumDeleteSystemEDIConDetailResult.Failure)
                    {
                        SetSystemAlertMessage(SysSystemEDIConDetail.DeleteSystemEDIConDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteSystemEDIConDetailResult == EntitySystemEDIConDetail.EnumDeleteSystemEDIConDetailResult.DataExist)
                    {
                        SetSystemAlertMessage(SysSystemEDIConDetail.DeleteSystemEDIConDetailResult_DataExist);
                        result = false;
                    }

                }

                if (result)
                {
                    return RedirectToAction("SystemEDICon", "Sys");
                }
            }

            if (result)
            {
                model.FormReset();
            }

            if (model.GetSysSystemSysIDList(true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIConDetail.SystemMsg_GetSystemSysIDList);
            }

            if (model.GetSysUserSystemSysIDList(AuthState.SessionData.UserID, true, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIConDetail.SystemMsg_GetSysUserSystemSysIDList);
            }

            else
            {
                if (!model.SetHasSysID())
                {
                    SetSystemErrorMessage(SysSystemEDIConDetail.SystemMsg_SetHasSysID);
                    return RedirectToAction("SystemEDICon", "Sys");
                }
            }

            if (model.GetSysSystemEDIFlowList(model.SysID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemEDIConDetail.SystemMsg_GetSysSystemEDIFlowList);
            }
            if (model.ExecAction == EnumActionType.Add)
            {
                model.GetSortOrder();

            }
            if (model.ExecAction == EnumActionType.Update)
            {
                if (model.GetSystemEDIConDetail() == false)
                {
                    SetSystemErrorMessage(SysSystemEDIConDetail.SystemMsg_GetSystemEDIConDetail);
                }
                else
                {
                    model.EDIConID = model.EntitySystemEDIConDetail.EDIConID.GetValue();
                    model.EDIConZHTW = model.EntitySystemEDIConDetail.EDIConZHTW.GetValue();
                    model.EDIConZHCN = model.EntitySystemEDIConDetail.EDIConZHCN.GetValue();
                    model.EDIConENUS = model.EntitySystemEDIConDetail.EDIConENUS.GetValue();
                    model.EDIConTHTH = model.EntitySystemEDIConDetail.EDIConTHTH.GetValue();
                    model.EDIConJAJP = model.EntitySystemEDIConDetail.EDIConJAJP.GetValue();
                    model.ProviderName = model.EntitySystemEDIConDetail.ProviderName.GetValue();
                    model.ConValue = model.EntitySystemEDIConDetail.ConValue.GetValue();
                    model.SortOrder = model.EntitySystemEDIConDetail.SortOrder.GetValue();
                }
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemEDIConDetail")]
        public ActionResult SystemEDIFlowList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemEDIFlowModel model = new SystemEDIFlowModel();

            if (model.GetSysSystemEDIFlowList(sysID, base.CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.EntitySysSystemEDIFlowList, true));
            }

            return Json(null);
        }
    }
}