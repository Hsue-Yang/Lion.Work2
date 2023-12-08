using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysSettingRepository
    {
        public async Task<(int rowCount, IEnumerable<SystemUser> systemUserList)> GetSystemUserList(string sysID, string userID, string userNM, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemUsers @sysID, @userID, @userNM, @pageIndex, @pageSize;", new { sysID, userID, userNM, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemUserList = multi.Read<SystemUser>();

                return (rowCount, systemUserList);
            }
        }
    }
}