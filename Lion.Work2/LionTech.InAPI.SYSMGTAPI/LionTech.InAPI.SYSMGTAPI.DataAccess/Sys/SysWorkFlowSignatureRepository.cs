using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysWorkFlowSignatureRepository : ISysWorkFlowSignatureRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysWorkFlowSignatureRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<SystemWFSig>> GetSystemWorkFlowSignatures(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemWFSig>("EXEC dbo.sp_GetSystemWorkFlowSignatures @sysID, @wfFlowID, @wfFlowVer, @wfNodeID, @cultureID ", new { sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID });
            }
        }

        public async Task<ReturnSystemWFNode> EditSystemWorkFlowNode(SystemWFNode systemWFNode)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemWorkFlowNode
                                         @sysID,
                                         @wfFlowID,
                                         @wfFlowVer,
                                         @wfNodeID,
                                         @wfSigMemoZHTW,
                                         @wfSigMemoZHCN,                                        
                                         @wfSigMemoENUS,
                                         @wfSigMemoTHTH,
                                         @wfSigMemoJAJP,
                                         @wfSigMemoKOKR,
                                         @sigAPISysID,
                                         @sigAPIControllerID,
                                         @sigAPIActionName,
                                         @chkAPISysID,
                                         @chkAPIControllerID,
                                         @chkAPIActionName,
                                         @isSigNextNode,
                                         @isSigBackNode,
                                         @updUserID";
                return await conn.QuerySingleAsync<ReturnSystemWFNode>(commandText, systemWFNode);
            }
        }

        public async Task<IEnumerable<SystemWFSigSeq>> GetSystemWorkFlowSignatureSeqs(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemWFSigSeq>("EXEC dbo.sp_GetSystemWorkFlowSignatureSeqs @sysID, @wfFlowID, @wfFlowVer, @wfNodeID, @cultureID", new { sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID });
            }
        }

        public async Task<SystemWorkFlowSignatureDetail> GetSystemWorkFlowSignatureDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfSigSeq)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemWorkFlowSignatureDetail>("EXEC dbo.sp_GetSystemWorkFlowSignatureDetail @sysID, @wfFlowID, @wfFlowVer, @wfNodeID, @wfSigSeq ", new { sysID, wfFlowID, wfFlowVer, wfNodeID, wfSigSeq });
            }
        }

        public async Task<IEnumerable<SystemRoleSig>> GetSystemRoleSignatures(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfSigSeq, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemRoleSig>("EXEC dbo.sp_GetSystemRoleSignatures @sysID, @wfFlowID, @wfFlowVer, @wfNodeID, @wfSigSeq, @cultureID ", new { sysID, wfFlowID, wfFlowVer, wfNodeID, wfSigSeq, cultureID });
            }
        }

        public async Task InsertSystemWorkFlowSignatureDetail(SystemWFSignatureDetail systemWFSignatureDetail)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_InsertSystemWorkFlowSignatureDetail @SystemRoleSig, @SystemWorkFlowSignatureDetail";
                await conn.QuerySingleOrDefaultAsync(commandText, new
                {
                    SystemRoleSig = new TableValuedParameter(TableValuedParameter.GetDataTable(systemWFSignatureDetail.SystemRoleSignatures, "type_SystemRoleSignature")),
                    SystemWorkFlowSignatureDetail = new TableValuedParameter(TableValuedParameter.GetDataTable(systemWFSignatureDetail.SystemWFSigDetail, "type_SystemWorkFlowSignatureDetail"))
                });
            }
        }

        public async Task EditSystemWorkFlowSignatureDetail(SystemWFSignatureDetail systemWFSignatureDetail)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemWorkFlowSignatureDetail @SystemRoleSig, @SystemWorkFlowSignatureDetail";
                await conn.QuerySingleOrDefaultAsync(commandText, new
                {
                    SystemRoleSig = new TableValuedParameter(TableValuedParameter.GetDataTable(systemWFSignatureDetail.SystemRoleSignatures, "type_SystemRoleSignature")),
                    SystemWorkFlowSignatureDetail = new TableValuedParameter(TableValuedParameter.GetDataTable(systemWFSignatureDetail.SystemWFSigDetail, "type_SystemWorkFlowSignatureDetail"))
                });
            }
        }

        public async Task DeleteSystemWorkFlowSignatureDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfSigSeq)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.QuerySingleOrDefaultAsync("EXEC dbo.sp_DeleteSystemWorkFlowSignatureDetail @sysID, @wfFlowID, @wfFlowVer, @wfNodeID, @wfSigSeq", new { sysID, wfFlowID, wfFlowVer, wfNodeID, wfSigSeq });
            }
        } 
    }
}
