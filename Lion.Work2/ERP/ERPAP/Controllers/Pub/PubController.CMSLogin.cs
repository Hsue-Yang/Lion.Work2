using System;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class PubController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult CMSLogin()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            var randomDateTimeTicks = DateTime.Now.Ticks.ToString(); // 使用時間戳作為隨機參數
            return RedirectToAction("SSOLogin", "Home", new { systemID = "CMSAP", randomDateTimeTicks });
        }
    }
}