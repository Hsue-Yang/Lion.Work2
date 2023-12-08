using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class UserSystemRepository : IUserSystemRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public UserSystemRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<(int rowCount, IEnumerable<UserSystem> UserSystemList)> GetUserStatusList(string userID, string userNM, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetUserStatusList @userID, @userNM, @pageIndex, @pageSize ", new { userID, userNM, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var userSystemList = multi.Read<UserSystem>();

                return (rowCount, userSystemList);
            }
        }

        public async Task<UserSystem> GetUserRawData(string userID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<UserSystem>("EXEC dbo.sp_GetUserRawData @userID ", new { userID });
            }
        }
        
        public async Task<IEnumerable<UserSystemRole>> GetUserOutSourcingSystemRoles(string userID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<UserSystemRole>("EXEC dbo.sp_GetUserOutSourcingSystemRoles @userID, @cultureID ", new { userID, cultureID });
            }
        }


        public async Task EditUserOutSourcingSystemRoles(string userID, string updUserID, List<UserSystemRolePara> sysIDList)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_EditUserOutSourcingSystemRoles @userID, @updUserID, @sysIDList ", new
                {
                    userID,
                    updUserID,
                    sysIDList = new TableValuedParameter(TableValuedParameter.GetDataTable(sysIDList, "type_UserSystemRole"))
                });
            }
        }
    }
}