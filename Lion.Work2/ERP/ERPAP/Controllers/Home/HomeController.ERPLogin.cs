using System.Web.Mvc;
using ERPAP.Models.Home;

namespace ERPAP.Controllers
{
    public partial class HomeController
    {
        [HttpGet]
        public ActionResult ERPLogin(string UserID, string TargetUrl)
        {
            if (base.AuthState.IsLogined &&
                string.IsNullOrWhiteSpace(TargetUrl) == false)
            {
                return Redirect(TargetUrl);
            }
            else
            {
                TempData["ERPLoginModelUserID"] = UserID;
                TempData["ERPLoginModelTargetUrl"] = TargetUrl; //System.Web.HttpUtility.UrlEncode

                return RedirectToAction("Index", "Home");
            }
        }
    }
}