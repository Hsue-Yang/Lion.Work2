using System.Web.Http;

namespace TRAININGAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalConfiguration.Configuration.Filters);
        }
    }
}
