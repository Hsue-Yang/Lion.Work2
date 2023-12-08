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
        public async Task<(int rowCount, IEnumerable<SystemIP> systemIPList)> GetSystemIPList(string sysID, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemIPs @sysID, @cultureID, @pageIndex, @pageSize;", new { sysID, cultureID, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemIPList = multi.Read<SystemIP>();

                return (rowCount, systemIPList);
            }
        }

        public async Task EditSystemIPDetail(SystemIP systemIP)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText =
                    @"EXEC dbo.sp_EditSystemIp
                      @SysID, @SubSysID
                    , @IPAddress, @ServerNM
                    , @IsAPServer, @IsAPIServer, @IsDBServer, @IsFileServer, @IsOutsourcing
                    , @FolderPath, @Remark, @UpdUserID;";

                await conn.ExecuteAsync(commandText, systemIP);
            }
        }

        public async Task DeleteSystemIPDetail(string sysID, string subSysID, string ipAddress)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_DeleteSystemIp @sysID, @subSysID, @ipAddress;", new { sysID, subSysID, ipAddress });
            }
        }
    }
}