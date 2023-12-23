using System.Web.Http;

namespace TRAININGAPI.Controllers
{
    public partial class RootController
    {
        [HttpGet]
        public IHttpActionResult GenericError()
        {
            return Text(string.Empty);
        }
    }
}