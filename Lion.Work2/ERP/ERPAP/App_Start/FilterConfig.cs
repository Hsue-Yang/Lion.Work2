using System;
using System.Configuration;
using System.Web.Mvc;
using LionTech.AspNet.Authentication.Jwt;
using LionTech.Entity.ERP;
using LionTech.Utility;

namespace ERPAP
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (LionTechAppSettings.ServerEnvironment != EnumServerEnvironment.Developing)
            {
                filters.Add(new UserTraceActionLogAttribute(EnumSystemID.ERPAP));
            }

            if (Convert.ToBoolean(ConfigurationManager.AppSettings[EnumAppSettingKey.JwtEnable.ToString()]))
            {
                filters.Add(new JwtAuthorizeAttribute());
            }
        }
    }
}