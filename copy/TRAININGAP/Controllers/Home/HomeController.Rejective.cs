using System.Web.Mvc;

namespace TRAININGAP.Controllers
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