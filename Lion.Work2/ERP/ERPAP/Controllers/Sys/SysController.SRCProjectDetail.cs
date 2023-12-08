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
        public ActionResult SRCProjectDetail(SRCProjectDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;
            model.GetBaseDomainNameList(base.CultureID);
            model.GetProjectParentList(base.CultureID);

             bool result = true;

            if (base.IsPostBack)
            {
                if (result && (model.ExecAction == EnumActionType.Add || model.ExecAction == EnumActionType.Update))
                {
                    if (model.EditSRCProjectDetail(base.CultureID, AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSRCProjectDetail.AddSRCProjectDetailResult_Failure);
                        result = false;
                    }
                }

                if (result && model.ExecAction == EnumActionType.Delete)
                {
                    if (model.DeleteSRCProjectDetail(base.CultureID, AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(SysSRCProjectDetail.DeleteSRCProjectDetailResult_Failure);
                        result = false;
                    }
                }
                if (result)
                {
                    return RedirectToAction("SRCProject", "Sys");
                }
            }
            model.GetDomainGroupList(base.CultureID);

            if (model.ExecAction == EnumActionType.Update)
            {
                if (model.GetSRCProjectDetailList(base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysDomainGroupDetail.SystemMsg_GetDomainGroupDetail);
                }
                else
                {
                    model.ProjectID = model.EntitySRCProjectDetail.ProjectID.GetValue();
                    model.ProjectNMZHTW = model.EntitySRCProjectDetail.ProjectNMZHTW.GetValue();
                    model.ProjectNMZHCN = model.EntitySRCProjectDetail.ProjectNMZHCN.GetValue();
                    model.ProjectNMENUS = model.EntitySRCProjectDetail.ProjectNMENUS.GetValue();
                    model.ProjectNMTHTH = model.EntitySRCProjectDetail.ProjectNMTHTH.GetValue();
                    model.ProjectNMJAJP = model.EntitySRCProjectDetail.ProjectNMJAJP.GetValue();
                    model.ProjectNMKOKR = model.EntitySRCProjectDetail.ProjectNMKOKR.GetValue();
                    model.ProjectParent = model.EntitySRCProjectDetail.ProjectParent.GetValue();
                    model.Remark = model.EntitySRCProjectDetail.Remark.GetValue();
                }
            }


            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("SRCProjectDetail")]
        public ActionResult SelectProjectID(string projectID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;
            SRCProjectDetailModel model = new SRCProjectDetailModel();

            string projectValue = model.SelectProjectID(base.CultureID, projectID);

            return Json(projectValue);
        }
    }
}
