using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysRoleRepository
    {
        public async Task<(int rowCount, IEnumerable<SystemRoleUser> systemRoleUserList)> GetSystemRoleUserList(string sysID, string roleID, string userID, string userNM, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemRoleUsers @sysID, @roleID, @userID, @userNM, @pageIndex, @pageSize;", new { sysID, roleID, userID, userNM, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemRoleUserList = multi.Read<SystemRoleUser>();

                return (rowCount, systemRoleUserList);
            }
        }
    }
}
