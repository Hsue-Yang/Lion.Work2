using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysLineRepository : ISysLineRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysLineRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }
        public async Task<bool> CheckSystemLineBotIdIsExists(string sysID, string lineID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.ExecuteScalarAsync<bool>(@"DECLARE @RESULT BIT = 0;
                                                                   EXEC dbo.sp_GetSystemLineBotIdIsExists @sysID, @lineID, @RESULT OUT;
                                                                   SELECT @RESULT AS IsExists"
                                                                 , new { sysID, lineID });
            }
        }

        public async Task<IEnumerable<SystemLine>> GetSystemLineBotIdList(string sysID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemLine>("EXEC dbo.sp_GetSystemLineBotIds @SYS_ID, @CULTURE_ID;"
                                                   , new { SYS_ID = sysID, CULTURE_ID = cultureID });
            }
        }

        public async Task<(int rowCount, IEnumerable<SystemLine> systemLineAccountList)> GetSystemLineBotAccountList(string sysID, string lineID, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemLineBotAccounts @sysID, @lineID, @cultureID, @pageIndex, @pageSize;"
                                                   , new { sysID, lineID, cultureID, pageIndex, pageSize });
                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemLineAccountList = multi.Read<SystemLine>();

                return (rowCount, systemLineAccountList);
            }
        }

        public async Task<SystemLine> GetSystemLineBotAccountDetail(string sysID, string lineID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemLine>("EXEC dbo.sp_GetSystemLineBotAccountsDetail @SYS_ID, @LINE_ID;"
                                                                       , new { SYS_ID = sysID, LINE_ID = lineID });
            }
        }

        public async Task EditSystemLineBotAccountDetail(SystemLine systemLine)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText =
                    @"EXEC dbo.sp_EditSystemLineBotAccountDetail
                      @SysID,@LINEID, @LineNMZHTW, @LineNMZHCN, @LineNMENUS
                    , @LineNMTHTH, @LineNMJAJP, @LineNMKOKR, @ChannelID, @ChannelSecret
                    , @ChannelAccessToken, @IsDisable, @SortOrder, @UpdUserID;";
                await conn.ExecuteAsync(commandText, systemLine);
            }
        }

        public async Task DeleteSystemLineById(string sysID, string lineID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_DeleteSystemLineByIds @sysID, @lineID;"
                                 , new { sysID, lineID });
            }
        }
    }
}
