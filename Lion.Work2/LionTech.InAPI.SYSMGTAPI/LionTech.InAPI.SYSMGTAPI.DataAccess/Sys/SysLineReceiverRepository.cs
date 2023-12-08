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
    public partial class SysLineReceiverRepository : ISysLineReceiverRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysLineReceiverRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<bool> CheckSystemLineBotReceiverIdIsExists(string sysID, string lineID, string lineReceiverID, string receiverID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.ExecuteScalarAsync<bool>(@"DECLARE @RESULT BIT = 0
                                                             EXEC dbo.sp_GetSystemLineBotReceiverIdIsExists
                                                             @sysID, @lineID, @lineReceiverID, @receiverID, @RESULT OUT;
                                                             SELECT @RESULT AS IsExists"
                                                           , new { sysID, lineID, lineReceiverID, receiverID });
            }
        }

        public async Task<(int rowCount, IEnumerable<SystemLineReceiver> systemLineReceiverList)> GetSystemLineBotReceiver(string sysID, string lineID, string cultureID, string queryReceiverNM, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemLineBotReceivers @sysID, @lineID, @cultureID, @queryReceiverNM, @pageIndex, @pageSize;"
                                                   , new { sysID, lineID, cultureID, queryReceiverNM, pageIndex, pageSize });
                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemLineReceiverList = multi.Read<SystemLineReceiver>();

                return (rowCount, systemLineReceiverList);
            }
        }

        public async Task<SystemLineReceiver> GetSystemLineBotReceiverDetail(string sysID, string receiverID, string lineID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemLineReceiver>("EXEC dbo.sp_GetSystemLineBotReceiverDetail @receiverID, @sysID, @lineID, @cultureID;"
                                                    , new { receiverID, sysID, lineID, cultureID });
            }
        }

        public async Task EditSystemLineBotReceiverDetail(SystemLineReceiver systemLineReceiver)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText =
                   @"EXEC dbo.sp_EditSystemLineBotReceiverDetail
                          @SysID, @LineID, @LineReceiverNMZHTW, @LineReceiverNMZHCN, @LineReceiverNMENUS
                        , @LineReceiverNMTHTH, @LineReceiverNMJAJP, @LineReceiverNMKOKR, @LineReceiverID, @ReceiverID, @UpdUserID;";

                await conn.ExecuteAsync(commandText, systemLineReceiver);
            }
        }

    }
}
