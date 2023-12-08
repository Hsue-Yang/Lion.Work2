using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public AuthorizationRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<UserSysFun>> GetAllUserAssignSystemFuns()
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                string commmandText = @"EXEC dbo.sp_GetAllUserAssignSystemFuns";
                return await conn.QueryAsync<UserSysFun>(commmandText);
            }
        }

        public async Task<IEnumerable<UserSysRole>> GetAllUserSystemRoles()
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                string commmandText = @"EXEC dbo.sp_GetAllUserSystemRoles";
                return await conn.QueryAsync<UserSysRole>(commmandText);
            }
        }
    }
}