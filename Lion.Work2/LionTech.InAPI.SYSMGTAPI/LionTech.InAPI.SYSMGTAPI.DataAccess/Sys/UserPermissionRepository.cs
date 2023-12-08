using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class UserPermissionRepository : IUserPermissionRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public UserPermissionRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<(int rowCount, IEnumerable<UserPermission>)> GetUserPermissionList(string userID, string userNM, string restrictType, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                var multiResult = await conn.QueryMultipleAsync("EXEC dbo.sp_GetUserPermission  @userID, @userNM, @restrictType, @cultureID, @pageIndex, @pageSize;", new { userID, userNM, restrictType, cultureID, pageIndex, pageSize });

                var rowCount = multiResult.Read<int>().SingleOrDefault();
                var userpermissionList = multiResult.Read<UserPermission>();

                return (rowCount, userpermissionList);
            }
        }

        public async Task<UserPermissionDetail> GetUserPermissionDetail(string userID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                var multiResult = await conn.QueryMultipleAsync("EXEC dbo.sp_GetUserPermissionDetail @userID", new { userID });

                var userrawData = multiResult.Read<UserPermissionDetail>().FirstOrDefault();

                return userrawData;
            }
        }
    }
}
