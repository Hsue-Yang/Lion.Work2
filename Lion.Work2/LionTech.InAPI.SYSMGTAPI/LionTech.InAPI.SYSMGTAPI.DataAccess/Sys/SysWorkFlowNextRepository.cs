using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class SysWorkFlowNextRepository : ISysWorkFlowNextRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysWorkFlowNextRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }
        public async Task<IEnumerable<SystemWorkFlowNext>> GetSystemWorkFlowNext(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemWorkFlowNext>("EXEC sp_GetSystemWorkFlowNext @sysID,  @wfFlowID,  @wfFlowVer,  @wfNodeID,  @cultureID; ", new { sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID });
            }
        }

        public async Task<IEnumerable<SystemWFNodeList>> GetSystemWorkFlowNextNodes(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string nodeType, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemWFNodeList>("EXEC sp_GetSystemWorkFlowNextNodes @sysID,  @wfFlowID,  @wfFlowVer,  @wfNodeID, @nodeType, @cultureID; ", new { sysID, wfFlowID, wfFlowVer, wfNodeID, nodeType, cultureID });
            }
        }

        public async Task<SystemWFNext> GetSystemWorkFlowNextDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string nextWFNodeID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemWFNext>("EXEC sp_GetSystemWorkFlowNextDetail @sysID,  @wfFlowID,  @wfFlowVer,  @wfNodeID, @nextWFNodeID, @cultureID; ",
                new { sysID, wfFlowID, wfFlowVer, wfNodeID, nextWFNodeID, cultureID });
            }
        }

        public async Task EditSystemWorkFlowNext(EditSystemWFNext editSystemWFNext)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {

                string commandText = @"EXEC dbo.sp_EditSystemWorkFlowNext
                                        @SysID,
                                        @WFFlowID,
                                        @WFFlowVer,
                                        @WFNodeID,
                                        @NextWFNodeID,
                                        @NextResultValue,
                                        @SortOrder,
                                        @Remark,
                                        @UpdUserID; ";

                await conn.ExecuteAsync(commandText, editSystemWFNext);
            }
        }

        public async Task DeleteSystemWorkFlowNext(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string nextWFNodeID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_DeleteSystemWorkFlowNext @sysID, @wfFlowID, @wfFlowVer, @wfNodeID, @nextWFNodeID; ", new { sysID, wfFlowID, wfFlowVer, wfNodeID, nextWFNodeID });
            }
        }
    }
}
