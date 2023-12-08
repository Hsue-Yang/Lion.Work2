using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysEventRepository : ISysEventRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysEventRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<SystemEvent> GetSystemEventFullName(string sysID, string eventGroupID, string eventID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemEvent>("EXEC dbo.sp_GetSystemEventFullName @sysID, @eventGroupID, @eventID, @cultureID;", new { sysID, eventGroupID, eventID, cultureID });
            }
        }

        public async Task<IEnumerable<SystemEvent>> GetSystemEventByIdList(string sysID, string eventGroupID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemEvent>("EXEC dbo.sp_GetSystemEventByIds @sysID, @eventGroupID, @cultureID; ", new { sysID, eventGroupID, cultureID });
            }
        }

        public async Task<(int rowCount, IEnumerable<SystemEvent> SystemEventList)> GetSystemEventList(string sysID, string eventGroupID, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemEvents @sysID, @eventGroupID, @cultureID, @pageIndex, @pageSize;", new { sysID, eventGroupID, cultureID, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemEventList = multi.Read<SystemEvent>();

                return (rowCount, systemEventList);
            }
        }

        public async Task<SystemEventMain> GetSystemEventDetail(string sysID, string eventGroupID, string eventID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemEventMain>("EXEC dbo.sp_GetSystemEvent @sysID, @eventGroupID, @eventID; ", new { sysID, eventGroupID, eventID });
            }
        }

        public async Task EditSystemEventDetail(SystemEventMain systemEvent)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemEvent @SysID, @EventGroupID, @EventID,
                                        @EventNMZHTW, @EventNMZHCN, @EventNMENUS, @EventNMTHTH, @EventNMJAJP, @EventNMKOKR,
                                        @EventPara, @IsDisable, @SortOrder, @Remark, @UpdUserID;";

                await conn.ExecuteAsync(commandText, systemEvent);
            }
        }

        public async Task<EnumDeleteResult> DeleteSystemEventDetail(string sysID, string eventGroupID, string eventID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                var result = await conn.ExecuteScalarAsync<string>("EXEC dbo.sp_DeleteSystemEvent @sysID, @eventGroupID, @eventID;", new { sysID, eventGroupID, eventID });

                if (result == null)
                {
                    return EnumDeleteResult.DataExist;
                }

                return result == EnumYN.Y.ToString()
                    ? EnumDeleteResult.Success
                    : EnumDeleteResult.Failure;
            }
        }
    }
}
