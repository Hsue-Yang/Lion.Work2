using System.Web.Mvc;

namespace B2PAP.Controllers
{
    public partial class HomeController
    {
        [HttpGet]
        public ActionResult B2PLogin(string UserID, string TargetUrl)
        {
            if (base.AuthState.IsLogined)
            {
                return Redirect(TargetUrl);
            }
            else
            {
                TempData["B2PLoginModelUserID"] = UserID;
                TempData["B2PLoginModelTargetUrl"] = TargetUrl; //System.Web.HttpUtility.UrlEncode

                return RedirectToAction("Index", "Home");
            }
        }
    }
}