using System.Web.Mvc;
using ERPAP.Models.Dev;

namespace ERPAP.Controllers
{
    public partial class DevController : _BaseAPController
    {
        [HttpPost]
        [AuthorizationActionFilter("SystemFun")]
        public ActionResult GetSystemFunControllerIDList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            DevModel model = new DevModel();

            if (model.GetSysSystemFunControllerIDList(sysID, base.CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.EntitySysSystemFunControllerIDList, true));
            }

            return Json(null);
        }

        [HttpPost]
        [AuthorizationActionFilter("SystemFun")]
        public ActionResult GetSystemFunMenuList(string sysID)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            DevModel model = new DevModel();

            if (model.GetSysSystemFunMenuList(sysID, base.CultureID))
            {
                return Content(model.GetJsonFormSelectItem(model.EntitySysSystemFunMenuList, true));
            }

            return Json(null);
        }
    }
}