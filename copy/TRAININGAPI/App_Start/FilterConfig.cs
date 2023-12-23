using LionTech.Entity.ERP;
using LionTech.Utility;
using System.Web.Http.Filters;

namespace TRAININGAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(HttpFilterCollection filters)
        {
            if (LionTechAppSettings.ServerEnvironment != EnumServerEnvironment.Developing)
            {
                filters.Add(new ApiTraceActionLogAttribute(EnumAPISystemID.TRAININGAP));
            }
        }
    }
}
