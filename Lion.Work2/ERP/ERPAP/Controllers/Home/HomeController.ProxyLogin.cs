using System.Web.Mvc;
using ERPAP.Models.Home;

namespace ERPAP.Controllers
{
    public partial class HomeController
    {
        [HttpGet]
        public ActionResult ProxyLogin(string SystemID)
        {
            if (string.IsNullOrWhiteSpace(SystemID) == false)
            {
                TempData["ProxyLoginModelSystemID"] = SystemID;
                IndexModel model = new IndexModel();
                TempData["ProxyLoginToAp"] = model.GetSystemName(SystemID, CultureID);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}