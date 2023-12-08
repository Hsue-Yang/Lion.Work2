using System.Web.Mvc;
using ERPAP.Models.Pub;
using Resources;

namespace ERPAP.Controllers
{
    public partial class PubController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult ExternalSystem()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            ExternalSystemModel model = new ExternalSystemModel();

            if (model.GetExternalSystemList(AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(PubExternalSystem.SystemMsg_GetExternalSystemList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult ExternalSystem(ExternalSystemModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            return RedirectToAction("SSOLogin", "Home", new { systemID = model.SystemID });
        }
    }
}
