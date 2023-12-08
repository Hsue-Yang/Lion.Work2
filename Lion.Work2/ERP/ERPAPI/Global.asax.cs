using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using LionTech.Utility;
using System;
using System.Configuration;
using System.IO;
using System.Web.Http;

namespace ERPAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalConfiguration.Configuration.Filters);
            RedisConnection.Init(Security.Decrypt(ConfigurationManager.ConnectionStrings["redis9"].ConnectionString));
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(ConfigurationManager.AppSettings["FirebaseCredentialsPath"])
            });
        }
    }
}
