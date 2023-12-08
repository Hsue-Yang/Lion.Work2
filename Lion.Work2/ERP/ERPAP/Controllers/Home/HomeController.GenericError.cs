using System.Web.Mvc;

namespace ERPAP.Controllers
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