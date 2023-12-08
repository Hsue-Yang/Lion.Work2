using LionTech.Entity.ERP;
using LionTech.Utility;
using System.Web.Mvc;

namespace TRAININGAP
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (LionTechAppSettings.ServerEnvironment != EnumServerEnvironment.Developing)
            {
                filters.Add(new UserTraceActionLogAttribute(EnumSystemID.TRAININGAP));
            }
        }
    }
}
