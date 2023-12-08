using System.Collections.Generic;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult SRCProject(string domainGroupID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SRCProjectModel model = new SRCProjectModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.ProjectID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SRCProjectModel.Field.ProjectID.ToString());
                model.DomainName = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SRCProjectModel.Field.DomainName.ToString());
                model.DomainGroupID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SRCProjectModel.Field.DomainGroupID.ToString());
            }
            #endregion

            model.GetSysDomainTabList(_BaseAPModel.EnumTabAction.SysSRCProject);
            model.GetBaseDomainNameList(base.CultureID);

            if (model.GetDomainGroupMenuList(base.CultureID, model.DomainName) == false)
            {
                SetSystemErrorMessage(SysSRCProject.SystemMsg_GetSysSRCProjectList);
            }

            if (model.GetProjectMenuList(base.CultureID, domainGroupID) == false)
            {
                SetSystemErrorMessage(SysSRCProject.SystemMsg_GetSysSRCProjectList);
            }

            if (model.GetProjectList(base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSRCProject.SystemMsg_GetSysSRCProjectList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SRCProject(SRCProjectModel model, string domainGroupID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;
            model.GetSysDomainTabList(_BaseAPModel.EnumTabAction.SysSRCProject);
            model.GetBaseDomainNameList(base.CultureID);

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SRCProjectModel.Field.ProjectID.ToString(), model.ProjectID);
                paraDict.Add(SRCProjectModel.Field.DomainName.ToString(), model.DomainName);
                paraDict.Add(SRCProjectModel.Field.DomainGroupID.ToString(), model.DomainGroupID);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion

                if (model.GetDomainGroupMenuList(base.CultureID, model.DomainName) == false)
                {
                    SetSystemErrorMessage(SysSRCProject.SystemMsg_GetSysSRCProjectList);
                }

                if (model.GetProjectMenuList(base.CultureID, domainGroupID) == false)
                {
                    SetSystemErrorMessage(SysSRCProject.SystemMsg_GetSysSRCProjectList);
                }

                if (model.GetProjectList(base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysSRCProject.SystemMsg_GetSysSRCProjectList);
                }
            }
            

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("SRCProject")]
        public ActionResult GetProjectID(string domainGroupID, string domainName)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;
            SRCProjectModel model = new SRCProjectModel();

            if (!string.IsNullOrWhiteSpace(domainName))
            {
                model.DomainName = domainName;
            }

            if (model.GetProjectMenuList(base.CultureID, domainGroupID) == true)
            {
                return Content(model.GetJsonFormSelectItem(model.EntitySRCProjectMenuList, true));
            }
            return Json(null);
        }
    }
}
