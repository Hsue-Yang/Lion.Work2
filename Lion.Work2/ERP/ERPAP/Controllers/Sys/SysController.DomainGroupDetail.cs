using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult DomainGroupDetail(DomainGroupDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;
            model.GetBaseDomainNameList(base.CultureID);

            bool result = true;

            if (base.IsPostBack)
            {
                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (model.EditDomainGroupDetail(base.CultureID, AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysDomainGroupDetail.AddDomainGroupDetailResult_Failure);
                        result = false;
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    var deleteDomainGroupDetailResult = model.GetDeleteDomainGroupDetailResult(base.CultureID);
                    if (deleteDomainGroupDetailResult == EntityDomainGroupDetail.EnumDeleteDomainGroupDetailListResult.Failure)
                    {
                        SetSystemErrorMessage(SysDomainGroupDetail.DeleteDomainGroupDetailResult_Failure);
                        result = false;
                    }
                    else if (deleteDomainGroupDetailResult == EntityDomainGroupDetail.EnumDeleteDomainGroupDetailListResult.DataExist)
                    {
                        string message = string.Format(SysDomainGroupDetail.DeleteDomainGroupDetailResult_DataExist, SysResource.TabText_DomainGroup);
                        SetSystemAlertMessage(message);
                        result = false;
                    }

                }
                if (result)
                {
                    return RedirectToAction("DomainGroup", "Sys");
                }
            }

            if (model.ExecAction == EnumActionType.Update)
            {
                if (model.GetDomainGroupDetail(base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysDomainGroupDetail.SystemMsg_GetDomainGroupDetail);
                }
                else
                {
                    model.DomainGroupID = model.EntityDomainGroupDetail.DomainGroupID.GetValue();
                    model.DomainGroupNMZHTW = model.EntityDomainGroupDetail.DomainGroupNMZHTW.GetValue();
                    model.DomainGroupNMZHCN = model.EntityDomainGroupDetail.DomainGroupNMZHCN.GetValue();
                    model.DomainGroupNMENUS = model.EntityDomainGroupDetail.DomainGroupNMENUS.GetValue();
                    model.DomainGroupNMTHTH = model.EntityDomainGroupDetail.DomainGroupNMTHTH.GetValue();
                    model.DomainGroupNMJAJP = model.EntityDomainGroupDetail.DomainGroupNMJAJP.GetValue();
                }
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("DomainGroupDetail")]
        public ActionResult SelectDomainGroupID(string domainName, string domainGroupID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;
            DomainGroupDetailModel model = new DomainGroupDetailModel();

            string domainGroupValue = model.SelectDomainGroupID(base.CultureID, domainName, domainGroupID);

            return Json(domainGroupValue);
        }
    }
}
