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
    public class SysEventGroupRepository : ISysEventGroupRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysEventGroupRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<SystemEventGroup>> GetSystemEventGroupByIdList(string sysID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemEventGroup>("EXEC dbo.sp_GetSystemEventGroupByIds @sysID, @cultureID; ", new { sysID, cultureID });
            }
        }

        public async Task<(int rowCount, IEnumerable<SystemEventGroup> SystemEventGroupList)> GetSystemEventGroupList(string sysID, string eventGroupID, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemEventGroups @sysID, @eventGroupID, @cultureID, @pageIndex, @pageSize;", new { sysID, eventGroupID, cultureID, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemEventGroupList = multi.Read<SystemEventGroup>();

                return (rowCount, systemEventGroupList);
            }
        }

        public async Task<SystemEventGroupMain> GetSystemEventGroupDetail(string sysID, string eventGroupID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemEventGroupMain>("EXEC dbo.sp_GetSystemEventGroup @sysID, @eventGroupID; ", new { sysID, eventGroupID });
            }
        }

        public async Task EditSystemEventGroupDetail(SystemEventGroupMain systemEventGroup)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemEventGroup @SysID, @EventGroupID,
                                        @EventGroupZHTW, @EventGroupZHCN, @EventGroupENUS, @EventGroupTHTH, @EventGroupJAJP, @EventGroupKOKR,
                                        @SortOrder, @UpdUserID;";

                await conn.ExecuteAsync(commandText, systemEventGroup);
            }
        }

        public async Task<EnumDeleteResult> DeleteSystemEventGroupDetail(string sysID, string eventGroupID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                var result = await conn.ExecuteScalarAsync<string>("EXEC dbo.sp_DeleteSystemEventGroup @sysID, @eventGroupID;", new { sysID, eventGroupID });

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
