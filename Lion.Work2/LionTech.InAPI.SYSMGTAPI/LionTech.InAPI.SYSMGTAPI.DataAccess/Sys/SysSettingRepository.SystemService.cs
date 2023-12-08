using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysSettingRepository
    {
        public async Task<IEnumerable<SystemService>> GetSystemServiceList(string sysID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemService>("EXEC dbo.sp_GetSystemServices @sysID, @cultureID;", new { sysID, cultureID });
            }
        }

        public async Task EditSystemServiceDetail(SystemService systemService)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText =
                    @"EXEC dbo.sp_EditSystemService
                      @SysID, @SubSysID
                    , @ServiceID, @Remark, @UpdUserID;";

                await conn.ExecuteAsync(commandText, systemService);
            }
        }

        public async Task DeleteSystemServiceDetail(string sysID, string subSysID, string serviceID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_DeleteSystemService @sysID, @subSysID, @serviceID;", new { sysID, subSysID, serviceID });
            }
        }
    }
}