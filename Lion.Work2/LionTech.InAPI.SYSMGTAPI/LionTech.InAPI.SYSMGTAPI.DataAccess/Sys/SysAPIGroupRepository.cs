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
    public class SysAPIGroupRepository : ISysAPIGroupRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysAPIGroupRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<SystemAPIGroup>> GetSystemAPIGroupByIdList(string sysID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemAPIGroup>("EXEC dbo.sp_GetSystemAPIGroupByIds @sysID, @cultureID; ", new { sysID, cultureID });
            }
        }

        public async Task<(int rowCount, IEnumerable<SystemAPIGroup> SystemAPIGroupList)> GetSystemAPIGroupList(string sysID, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemAPIGroups @sysID, @cultureID, @pageIndex, @pageSize;", new { sysID, cultureID, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemAPIGroupList = multi.Read<SystemAPIGroup>();

                return (rowCount, systemAPIGroupList);
            }
        }

        public async Task<SystemAPIGroupMain> GetSystemAPIGroupDetail(string sysID, string apiGroupID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemAPIGroupMain>("EXEC dbo.sp_GetSystemAPIGroup @sysID, @apiGroupID;", new { sysID, apiGroupID });
            }
        }

        public async Task EditSystemAPIGroupDetail(SystemAPIGroupMain systemAPIGroup)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText =
                    @"EXEC dbo.sp_EditSystemAPIGroup
                      @SysID, @APIGroupID
                    , @APIGroupZHTW,@APIGroupZHCN,@APIGroupENUS,@APIGroupTHTH,@APIGroupJAJP,@APIGroupKOKR
                    , @SortOrder, @UpdUserID";

                await conn.ExecuteAsync(commandText, systemAPIGroup);
            }
        }

        public async Task<EnumDeleteSystemAPIGroupResult> DeleteSystemAPIGroupDetail(string sysID, string apiGroupID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = "EXEC sp_DeleteSystemAPIGroup @sysID, @apiGroupID;";
                var result = await conn.ExecuteScalarAsync<string>(commandText, new { sysID, apiGroupID });

                if (result == null)
                {
                    return EnumDeleteSystemAPIGroupResult.DataExist;
                }

                return result == EnumYN.Y.ToString()
                    ? EnumDeleteSystemAPIGroupResult.Success
                    : EnumDeleteSystemAPIGroupResult.Failure;
            }
        }
    }
}
