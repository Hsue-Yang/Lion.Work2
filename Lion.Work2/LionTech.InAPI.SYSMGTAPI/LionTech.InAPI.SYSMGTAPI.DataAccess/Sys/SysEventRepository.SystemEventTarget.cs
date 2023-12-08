using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysEventRepository
    {
        public async Task<IEnumerable<SystemEventTarget>> GetSystemEventTargetList(string sysID, string userID, string eventGroupID, string eventID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemEventTarget>("EXEC dbo.sp_GetSystemEventTargets @sysID, @eventGroupID, @eventID, @userID, @cultureID; ", new { sysID, eventGroupID, eventID, userID, cultureID });
            }
        }

        public async Task EditSystemEventTarget(SystemEventTarget systemEventTarget)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_EditSystemEventTarget @SysID, @EventGroupID, @EventID, @TargetSysID, @TargetPath, @SubSysID, @UpdUserID;", systemEventTarget);
            }
        }

        public async Task DeleteSystemEventTarget(string sysID, string eventGroupID, string eventID, string targetSysID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_DeleteSystemEventTarget @sysID, @eventGroupID, @eventID, @targetSysID", new { sysID, eventGroupID, eventID, targetSysID });
            }
        }

        public async Task<IEnumerable<SystemEventTarget>> GetSystemEventTargetIDs(string sysID, string eventGroupID, string eventID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemEventTarget>("EXEC dbo.sp_GetSystemEventTargetIDs @sysID, @eventGroupID, @eventID; ", new { sysID, eventGroupID, eventID });
            }
        }
    }
}
