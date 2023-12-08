using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysEventRepository
    {
        public async Task<(int rowCount, IEnumerable<SystemEDIEvent> SystemEventEDIList)> GetSystemEventEDIList(string sysID, string userID, string targetSysID, string eventGroupID, string eventID, string dtBegin, string dtEnd, string isOnlyFail, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemEventEDIs @dtBegin, @dtEnd, @sysID, @eventGroupID, @eventID, @targetSysID, @isOnlyFail, @cultureID, @pageIndex, @pageSize;", new { dtBegin, dtEnd, sysID, eventGroupID, eventID, targetSysID, isOnlyFail, cultureID, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemEventEDIList = multi.Read<SystemEDIEvent>();

                return (rowCount, systemEventEDIList);
            }
        }

        public async Task<(int rowCount, IEnumerable<SystemEDIEvent> SystemEventTargetEDIList)> GetSystemEventTargetEDIList(string sysID, string userID, string targetSysID, string eventGroupID, string eventID, string dtBegin, string dtEnd, string isOnlyFail, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemEventTargetEDIs @sysID, @eventGroupID, @eventID, @dtBegin, @dtEnd, @targetSysID, @isOnlyFail, @userID, @cultureID, @pageIndex, @pageSize;", new { sysID, eventGroupID, eventID, dtBegin, dtEnd, targetSysID, isOnlyFail, userID, cultureID, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemEventTargetEDIList = multi.Read<SystemEDIEvent>();

                return (rowCount, systemEventTargetEDIList);
            }
        }

        public async Task<string> ExcuteSubscription(SystemEDIEvent systemEDIEvent)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.ExecuteScalarAsync<string>("EXEC dbo.sp_ExecSubscription @SysID, @EventGroupID, @EventID, @ExecEDIEventNo, @UpdUserID;", systemEDIEvent);
            }
        }
    }
}
