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
    public class SysPurviewRepository : ISysPurviewRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysPurviewRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<SystemPurview>> GetSystemPurviewByIdList(string sysID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemPurview>("EXEC dbo.sp_GetSystemPurviewByIds @sysID, @cultureID;", new { sysID, cultureID });
            }
        }

        public async Task<(int rowCount, IEnumerable<SystemPurview> SystemPurviewList)> GetSystemPurviewList(string sysID, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemPurviews @sysID, @cultureID, @pageIndex, @pageSize;", new { sysID, cultureID, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var SystemPurviewList = multi.Read<SystemPurview>();

                return (rowCount, SystemPurviewList);
            }
        }

        public async Task<SystemPurviewMain> GetSystemPurviewDetail(string sysID, string purviewID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemPurviewMain>("EXEC dbo.sp_GetSystemPurview @sysID, @purviewID;", new { sysID, purviewID });
            }
        }

        public async Task EditSystemPurviewDetail(SystemPurviewMain systemPurview)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText =
                    @"EXEC dbo.sp_EditSystemPurview
                      @SysID, @PurviewID
                    , @PurviewNMZHTW,@PurviewNMZHCN,@PurviewNMENUS,@PurviewNMTHTH,@PurviewNMJAJP,@PurviewNMKOKR
                    , @Remark, @SortOrder, @UpdUserID";

                await conn.ExecuteAsync(commandText, systemPurview);
            }
        }

        public async Task<EnumDeleteSystemPurviewResult> DeleteSystemPurviewDetail(string sysID, string purviewID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = "EXEC sp_DeleteSystemPurview @sysID, @purviewID;";
                var result = await conn.ExecuteScalarAsync<string>(commandText, new { sysID, purviewID });

                if (result == null)
                {
                    return EnumDeleteSystemPurviewResult.DataExist;
                }

                return result == EnumYN.Y.ToString()
                    ? EnumDeleteSystemPurviewResult.Success
                    : EnumDeleteSystemPurviewResult.Failure;
            }
        }
    }
}