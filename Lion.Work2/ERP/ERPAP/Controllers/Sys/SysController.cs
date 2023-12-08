using ERPAP.Models.Sys;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController : _BaseAPController
    {
        #region - AP -
        [HttpPost]
        [AuthorizationActionFilter("SystemFun")]
        public async Task<ActionResult> GetSystemSubsysIDList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSystemSubByIds(sysID, AuthState.SessionData.UserID, CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.SysSystemSubByIdList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFun")]
        public async Task<ActionResult> GetSystemRoleCategoryIDList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSysSystemRoleCategoryIDList(sysID, AuthState.SessionData.UserID, base.CultureID))
            {
                return Content(model.GetJsonToSelectItem(model.EntitySysSystemRoleCategoryIDList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFun")]
        public async Task<ActionResult> GetSystemRoleIDList(string sysID, string roleCategoryID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSysSystemRoleIDList(sysID, AuthState.SessionData.UserID, roleCategoryID, CultureID))
            {
                return Content(model.GetJsonToSelectItem(model.EntitySysSystemRoleIDList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemRoleCondition")]
        public async Task<ActionResult> GetSystemConditionIDList(string sysID)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            SysModel model = new SysModel();

            if (await model.GetSysSystemConditionIDList(sysID, CultureID))
            {
                return Content(model.GetJsonToSelectItem(model.EntitySysSystemConditionIDList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("LineBotAccountSetting")]
        public ActionResult GetLineBotIDList(string sysID)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            SysModel model = new SysModel();

            if (model.GetLineBotIDList(AuthState.SessionData.UserID, sysID, CultureID))
            {
                return Content(model.GetListToJsonFormSelectItem(model.LineBotIDList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFun")]
        public async Task<ActionResult> GetSystemFunControllerIDList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSysSystemFunControllerIDList(sysID, AuthState.SessionData.UserID, base.CultureID))
            {
                return Content(model.GetListToJsonFormSelectItem(model.EntitySysSystemFunControllerIDList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFun")]
        public async Task<ActionResult> GetSystemFunActionNameList(string sysID, string funControllerID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSystemFunNameList(sysID, funControllerID, base.CultureID))
            {
                return Content(model.GetListToJsonFormSelectItem(model.EntitySysSystemFunNameList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFun")]
        public async Task<ActionResult> GetSystemFunNameList(string sysID, string funControllerID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSystemFunNameList(sysID, funControllerID, base.CultureID))
            {
                return Content(model.GetListToJsonFormSelectItem(model.EntitySysSystemFunNameList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFun")]
        public async Task<ActionResult> GetSystemPurviewIDList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSystemPurviewByIdList(sysID, AuthState.SessionData.UserID, CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.SystemPurviewByIdList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFun")]
        public async Task<ActionResult> GetSystemFunMenuList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSystemFunMenuByIdList(sysID, AuthState.SessionData.UserID, CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.SystemFunMenuByIdList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SRCProject")]
        public ActionResult GetDomainGroupMenuList(string domainName)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (model.GetDomainGroupMenuList(base.CultureID, domainName))
            {
                return Content(model.GetJsonFormSelectItem(model.EntitySysDomainGroupMenuList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemLoginEventSetting")]
        public async Task<ActionResult> GetSysLoginEventIDList(string sysID)
        {
            SysModel model = new SysModel();

            if (await model.GetSysLoginEventIDList(sysID, AuthState.SessionData.UserID, CultureID))
            {
                return Json(model.SysLoginEventIDList.ToDictionary(p => p.LoginEventID, p => p.LoginEventNMID).ToList());
            }

            return Json(null);
        }
        #endregion

        #region - API -
        [HttpPost]
        [AuthorizationActionFilter("SystemAPI")]
        public async Task<ActionResult> GetSysSystemAPIGroupList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSystemAPIGroupByIdList(sysID, AuthState.SessionData.UserID, base.CultureID))
            {
                return Content(model.GetListToJsonFormSelectItem(model.SystemAPIGroupByIdList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemAPI")]
        public async Task<ActionResult> GetSysSystemAPIFunList(string sysID, string apiGroup)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSystemAPIFuntionList(sysID, apiGroup, base.CultureID))
            {
                return Content(model.GetListToJsonFormSelectItem(model.EntitySystemAPIFuntionList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemAPILog")]
        public async Task<ActionResult> GetSysSystemAPIByIdList(string sysID, string apiGroup)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSysSystemAPIByIdList(sysID, AuthState.SessionData.UserID, apiGroup, CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.SystemAPIByIdList, true));
            }

            return Json(null);
        }
        #endregion

        #region - Event -
        [HttpPost]
        [AuthorizationActionFilter("SystemEventTarget")]
        public async Task<ActionResult> GetSysSystemSubByIdList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSystemSubByIds(sysID, AuthState.SessionData.UserID, CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.SysSystemSubByIdList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemEvent")]
        public async Task<ActionResult> GetSysSystemEventGroupByIdList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSysSystemEventGroupByIdList(sysID, AuthState.SessionData.UserID, CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.SysSystemEventGroupByIdList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemEventEDI")]
        public async Task<ActionResult> GetSysSystemEventByIdList(string sysID, string eventGroupID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSysSystemEventByIdList(sysID, AuthState.SessionData.UserID, eventGroupID, CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.SysSystemEventByIdList, true));
            }

            return Json(null);
        }
        #endregion

        #region - EDI -
        [HttpPost]
        [AuthorizationActionFilter("SystemEDIFlow")]
        public async Task<ActionResult> GetSysSystemEDIFlowList(string sysID, bool useNullFlow = false)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSystemEDIFlowIDList(sysID, base.CultureID, useNullFlow: useNullFlow))
            {
                return Content(model.GetJsonToSelectItem(model.EntitySysSystemEDIFlowList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemEDIPara")]
        public async Task<ActionResult> GetSysSystemEDIJobList(string sysID, string eDIFlowID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSystemEDIJobIDList(AuthState.SessionData.UserID, sysID, eDIFlowID, base.CultureID))
            {
                return Content(model.GetJsonToSelectItem(model.EntitySysSystemEDIJobIDList, true));
            }

            return Json(null);
        }
        #endregion

        #region - WorkFlow -
        [HttpPost]
        [AuthorizationActionFilter("SystemWorkFlowGroup")]
        public async Task<ActionResult> GetSysSystemWorkFlowGroupIDList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetSystemWorkFlowGroupIDList(sysID, base.CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.EntitySystemWorkFlowGroupIDList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemWorkFlow")]
        public async Task<ActionResult> GetSysUserSystemWorkFlowIDList(string sysID, string groupID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (await model.GetUserSystemWorkFlowIDList(AuthState.SessionData.UserID, sysID, groupID, base.CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.EntityUserSystemWorkFlowIDList, true));
            }

            return Json(null);
        }
        #endregion
    }
}