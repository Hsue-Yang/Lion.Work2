using System.Web.Mvc;
using B2PAP.Models.Home;

namespace B2PAP.Controllers
{
    public partial class HomeController
    {
        [HttpGet]
        public ActionResult Logout()
        {
            LogoutModel model = new LogoutModel();
            model.ExecUserConnectCustLogout(AuthState.SessionData.UserID, AuthState.SessionData.SessionID);

            AuthState.SessionData.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
