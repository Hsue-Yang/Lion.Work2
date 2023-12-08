using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration
{
    public interface IMisConnectionStringProvider
    {
        string ConnStringErp { get; }
        string ConnStringErpReadOnly { get; }
    }

    public class MisConnectionStringProvider : IMisConnectionStringProvider
    {
        public MisConnectionStringProvider(IConfiguration configuration)
        {
            var connectionStrings = configuration.GetSection("ConnectionStrings").Get<Dictionary<string, string>>();
            if (connectionStrings.TryGetValue("LionGroupERP", out _connStringErp))
            {
                _connStringErp = LionTech.AspNetCore.Utility.Security.Decrypt(_connStringErp);

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_connStringErp)
                {
                    ApplicationIntent = ApplicationIntent.ReadOnly
                };
                _connStringErpReadOnly = builder.ConnectionString;
            }
        }

        private static string _connStringErp;
        private static string _connStringErpReadOnly;

        public string ConnStringErp => _connStringErp;
        public string ConnStringErpReadOnly => _connStringErpReadOnly;
    }
}
