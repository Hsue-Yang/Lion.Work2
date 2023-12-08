using System.Web.Http;

namespace ERPAPI.Controllers
{
    public partial class RootController
    {
        [HttpGet]
        public IHttpActionResult GenericError()
        {
            return Ok();
        }
    }
}