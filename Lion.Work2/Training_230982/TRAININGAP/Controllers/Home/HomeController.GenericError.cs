using System.Web.Mvc;

namespace TRAININGAP.Controllers
{
    public partial class HomeController
    {
        [HttpGet]
        public ActionResult GenericError()
        {
            return View();
        }
    }
}