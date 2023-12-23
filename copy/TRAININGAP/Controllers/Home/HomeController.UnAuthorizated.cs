using System.Web.Mvc;
using TRAININGAP.Models.Home;

namespace TRAININGAP.Controllers
{
    public partial class HomeController
    {
        [HttpGet]
        public ActionResult UnAuthorizated()
        {
            UnAuthorizatedModel model = new UnAuthorizatedModel();

            if (!string.IsNullOrWhiteSpace(AuthState.SessionData.UserID))
            {
                model.IsTimeout = false;
            }
            else
            {
                AuthState.SessionData.Clear();
                model.IsTimeout = true;
            }

            return View(model);
        }
    }
}