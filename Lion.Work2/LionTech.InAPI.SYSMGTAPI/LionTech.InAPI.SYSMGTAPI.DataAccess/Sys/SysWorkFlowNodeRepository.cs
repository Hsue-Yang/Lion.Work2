using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class SysWorkFlowNodeRepository : ISysWorkFlowNodeRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysWorkFlowNodeRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<SystemWorkFlow> GetSystemWorkFlowName(string sysID, string wfFlowID, string wfFlowVer, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleAsync<SystemWorkFlow>(
                    @"EXEC dbo.sp_GetSystemWorkFlowName
                          @sysID
                        , @wfFlowID 
                        , @wfFlowVer 
                        , @cultureID;",
                    new
                    {
                        sysID,
                        wfFlowID,
                        wfFlowVer,
                        cultureID
                    }
                    );
            }
        }

        public async Task<IEnumerable<SystemWorkFlowNode>> GetBackSystemWorkFlowNodes(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var returnData = await conn.QueryAsync<SystemWorkFlowNode>(
                    @"EXEC dbo.sp_GetBackSystemWorkFlowNodes
                          @sysID
                        , @wfFlowID
                        , @wfFlowVer
                        , @wfNodeID
                        , @cultureID;",
                    new
                    {
                        sysID,
                        wfFlowID,
                        wfFlowVer,
                        wfNodeID,
                        cultureID
                    }
                );
                return returnData;
            }
        }

        public async Task<IEnumerable<SystemWorkFlowNodeRole>> GetSystemWorkFlowNodeRoles(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemWorkFlowNodeRole>(
                    @"EXEC dbo.sp_GetSystemWorkFlowNodeRoles 
                        @sysID
                        , @wfFlowID
                        , @wfFlowVer
                        , @wfNodeID
                        , @cultureID;",
                    new
                    {
                        sysID,
                        wfFlowID,
                        wfFlowVer,
                        wfNodeID,
                        cultureID
                    }
                );
            }
        }

        public async Task<SystemWorkFlowNodeDetailExecuteResult> GetSystemWorkFlowNodeDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleAsync<SystemWorkFlowNodeDetailExecuteResult>(
                    @"EXEC dbo.sp_GetSystemWorkFlowNodeDetail 
                          @sysID
                        , @wfFlowID
                        , @wfFlowVer
                        , @wfNodeID;",
                    new
                    {
                        sysID,
                        wfFlowID,
                        wfFlowVer,
                        wfNodeID
                    }
                );
            }
        }

        public async Task<IEnumerable<SystemWorkFlowNodeDetailExecuteResult>> GetSystemWorkFlowNodeDetails(string sysID, string wfFlowID, string wfFlowVer)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemWorkFlowNodeDetailExecuteResult>(
                    @"EXEC dbo.sp_GetSystemWorkFlowNodeDetails 
                          @sysID
                        , @wfFlowID
                        , @wfFlowVer;",
                    new
                    {
                        sysID,
                        wfFlowID,
                        wfFlowVer
                    }
                );
            }
        }

        public async Task<SystemWorkFlowNodeDetailExecuteResult> EditSystemWorkFlowNodeDetail(SystemWorkFlowNodePara para)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemWorkFlowNodeDetailExecuteResult>(
                    @"EXEC dbo.sp_EditSystemWorkFlowNodeDetail 
                          @type_SystemWorkFlowNodeDetail
                        , @type_SystemWorkFlowNodeRole;",
                    new
                    {
                        type_SystemWorkFlowNodeDetail = new TableValuedParameter(TableValuedParameter.GetDataTable(para.SystemWorkFlowNodeDetailPara, "type_SystemWorkFlowNodeDetail")),
                        type_SystemWorkFlowNodeRole = new TableValuedParameter(TableValuedParameter.GetDataTable(para.SystemWorkFlowNodeRoleParas, "type_SystemWorkFlowNodeRole"))
                    }
                );
            }
        }

        public async Task<bool> IsWorkFlowChildsExist(string sysID, string wfFlowID, string wfFlowVer, string wfnodeid)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleAsync<bool>(
                    @"EXEC dbo.sp_IsWorkFlowChildsExist 
                          @sysID
                        , @wfFlowID
                        , @wfFlowVer
                        , @wfnodeid; ",
                    new
                    {
                        sysID,
                        wfFlowID,
                        wfFlowVer,
                        wfnodeid
                    }
                );
            }
        }

        public async Task DeleteSystemWorkFlowNodeDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.QueryAsync(
                    @"EXEC dbo.sp_DeleteSystemWorkFlowNodeDetail 
                          @sysID
                        , @wfFlowID
                        , @wfFlowVer
                        , @wfNodeID;",
                    new
                    {
                        sysID,
                        wfFlowID,
                        wfFlowVer,
                        wfNodeID
                    }
                );
            }
        }

        public async Task<bool> IsWorkFlowHasRunTime(string sysID, string wfFlowID, string wfFlowVer)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.ExecuteScalarAsync<bool>("EXEC dbo.sp_IsSystemWorkFlowHasRunTime @sysID, @wfFlowID, @wfFlowVer", new { sysID, wfFlowID, wfFlowVer });
            }
        }

        public async Task<IEnumerable<SystemWorkFlowNodeIDs>> GetSystemWorkFlowNodeIDs(string sysID, string wfFlowID, string wfFlowVer, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemWorkFlowNodeIDs>("EXEC dbo.sp_GetSystemWorkFlowNodeIDs @sysID, @wfFlowID, @wfFlowVer, @cultureID;",
                                                                     new { sysID, wfFlowID, wfFlowVer, cultureID });
            }
        }
    }
}
