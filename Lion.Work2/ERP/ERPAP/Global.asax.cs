using LionTech.Consul;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ERPAP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            base.Init();

            ApplicationSessionStateStore.MakeRequestString(this.Modules);
            RedisConnection.Init(Security.Decrypt(ConfigurationManager.ConnectionStrings["redis9"].ConnectionString));
        }

        protected void Application_Start()
        {
            ApplicationSessionStateStore.RegisterAppDomain();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new CSharpViewEngine());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            if (Common.IsInKubernetes())
            {
                ConsulKeyVaultInject.Install();
            }
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            if (Common.IsInKubernetes() == false)
            {
                if (Common.GetEnumDesc(LionTech.Entity.ERP.EnumSystemID.Domain) == "liontravel.com" && !Context.Request.IsSecureConnection)
                {
                    Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"));
                }
            }

            PropertyInfo webPageHttpModuleAppStartExecuteCompleted =
                typeof(System.Web.WebPages.Scope.AspNetRequestScopeStorageProvider).Assembly
                .GetType("System.Web.WebPages.WebPageHttpModule")
                .GetProperty("AppStartExecuteCompleted", BindingFlags.NonPublic | BindingFlags.Static);
            webPageHttpModuleAppStartExecuteCompleted.SetValue(null, true, null);

            LionTech.Entity.ERP.EnumCultureID finalCultureID = LionTech.Entity.ERP.EnumCultureID.zh_TW;

            HttpCookie httpCookie = Request.Cookies[EnumCookieName.CultureID.ToString()];
            if (httpCookie != null)
            {
                finalCultureID = LionTech.Entity.ERP.Utility.GetCultureID(httpCookie.Value);
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo(Common.GetEnumDesc(finalCultureID));
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Common.GetEnumDesc(finalCultureID));
        }
    }
}
