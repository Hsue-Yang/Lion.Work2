using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class PubController : _BaseAPController
    {
        [HttpGet]
        public ActionResult Empty()
        {
            return new EmptyResult();
        }
    }
}