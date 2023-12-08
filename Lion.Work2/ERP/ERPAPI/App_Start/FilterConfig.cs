using System.Web.Http.Filters;
using LionTech.Entity.ERP;
using LionTech.Utility;

namespace ERPAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(HttpFilterCollection filters)
        {
            if (LionTechAppSettings.ServerEnvironment != EnumServerEnvironment.Developing)
            {
                filters.Add(new LionTech.AspNet.WebApi.Filters.TraceLogAttribute(EnumAPISystemID.ERPAP));
            }
        }
    }
}