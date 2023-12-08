using System.Web.Mvc;
using TRAININGAP.Models.Demo;

namespace TRAININGAP.Controllers
{
    public partial class DemoController
    {
        [HttpGet]
        //[AuthorizationActionFilter]
        public ActionResult TestAction()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            TestActionModel model = new TestActionModel();
            model.DemoString = string.Format("{0}/{1}/{2}", AuthState.SystemID, AuthState.ControllerName, AuthState.ActionName);

            return View(model);
        }
    }
}