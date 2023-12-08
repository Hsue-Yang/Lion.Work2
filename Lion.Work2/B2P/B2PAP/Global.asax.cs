using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LionTech.Utility;
using LionTech.Utility.B2P;

namespace B2PAP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            base.Init();

            ApplicationSessionStateStore.MakeRequestString(this.Modules);
        }

        protected void Application_Start()
        {
            ApplicationSessionStateStore.RegisterAppDomain(LionTech.Entity.B2P.EnumSystemID.B2PAP.ToString());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            if (Common.GetEnumDesc(LionTech.Entity.B2P.EnumSystemID.Domain) == "liontravel.com" && !Context.Request.IsSecureConnection)
            {
                Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"));
            }

            PropertyInfo webPageHttpModuleAppStartExecuteCompleted =
                typeof(System.Web.WebPages.Scope.AspNetRequestScopeStorageProvider).Assembly
                .GetType("System.Web.WebPages.WebPageHttpModule")
                .GetProperty("AppStartExecuteCompleted", BindingFlags.NonPublic | BindingFlags.Static);
            webPageHttpModuleAppStartExecuteCompleted.SetValue(null, true, null);

            LionTech.Entity.B2P.EnumCultureID finalCultureID = LionTech.Entity.B2P.EnumCultureID.zh_TW;

            HttpCookie httpCookie = Request.Cookies[EnumCookieName.CultureID.ToString()];
            if (httpCookie != null)
            {
                finalCultureID = LionTech.Entity.B2P.Utility.GetCultureID(httpCookie.Value);
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo(Common.GetEnumDesc(finalCultureID));
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Common.GetEnumDesc(finalCultureID));
        }
    }
}
