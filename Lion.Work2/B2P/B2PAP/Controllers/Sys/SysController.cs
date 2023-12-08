using System.Web.Mvc;
using B2PAP.Models.Sys;

namespace B2PAP.Controllers
{
    public partial class SysController : _BaseAPController
    {
        [HttpPost]
        [AuthorizationActionFilter("SystemFun")]
        public ActionResult GetSystemRoleIDList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (model.GetSysSystemRoleIDList(sysID, base.CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.EntitySysSystemRoleIDList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFun")]
        public ActionResult GetSystemFunControllerIDList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (model.GetSysSystemFunControllerIDList(sysID, base.CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.EntitySysSystemFunControllerIDList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFun")]
        public ActionResult GetSystemFunActionNameList(string sysID, string funControllerID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (model.GetSysSystemFunNameList(sysID, funControllerID, base.CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.EntitySysSystemFunNameList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFun")]
        public ActionResult GetSystemFunMenuList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (model.GetSysSystemFunMenuList(sysID, base.CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.EntitySysSystemFunMenuList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFun")]
        public ActionResult GetSystemSubsysIDList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (model.GetSysSystemSubsysIDList(sysID, false, base.CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.EntitySysSystemSubsysIDList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFun")]
        public ActionResult GetSystemPurviewIDList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SysModel model = new SysModel();

            if (model.GetSysSystemPurviewIDList(sysID, base.CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.EntitySysSystemPurviewIDList, true));
            }

            return Json(null);
        }
    }
}