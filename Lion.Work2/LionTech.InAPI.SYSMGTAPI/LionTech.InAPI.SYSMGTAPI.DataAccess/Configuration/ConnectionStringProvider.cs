using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration
{
    public interface IConnectionStringProvider
    {
        string ConnStringSerp { get; }
        string ConnStringSerpReadOnly { get; }
    }

    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public ConnectionStringProvider(IConfiguration configuration)
        {
            var connectionStrings = configuration.GetSection("ConnectionStrings").Get<Dictionary<string, string>>();
            if (connectionStrings.TryGetValue("LionGroupSERP", out _connStringSerp))
            {
                _connStringSerp = LionTech.AspNetCore.Utility.Security.Decrypt(_connStringSerp);

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_connStringSerp)
                {
                    ApplicationIntent = ApplicationIntent.ReadOnly
                };
                _connStringSerpReadOnly = builder.ConnectionString;
            }
        }

        private static string _connStringSerp;
        private static string _connStringSerpReadOnly;
        
        public string ConnStringSerp => _connStringSerp;
        public string ConnStringSerpReadOnly => _connStringSerpReadOnly;
    }
}