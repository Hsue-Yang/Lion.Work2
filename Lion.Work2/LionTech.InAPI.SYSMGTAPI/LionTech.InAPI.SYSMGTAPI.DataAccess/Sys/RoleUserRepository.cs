using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class RoleUserRepository : IRoleUserRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public RoleUserRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }
        public async Task<IEnumerable<RoleUser>> GetRoleUsers(string sysID, string roleID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<RoleUser>("EXEC dbo.sp_GetRoleUsers @sysID, @roleID, @cultureID ", new { sysID, roleID, cultureID });
            }
        }
    }
}
