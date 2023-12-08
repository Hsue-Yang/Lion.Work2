using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class HomeController
    {
        [HttpGet]
        public ActionResult Rejective()
        {
            AuthState.SessionData.Clear();

            return View();
        }
    }
}