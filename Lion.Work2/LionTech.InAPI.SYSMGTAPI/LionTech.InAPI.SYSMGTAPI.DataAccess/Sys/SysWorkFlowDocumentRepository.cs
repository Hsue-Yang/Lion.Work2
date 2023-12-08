using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class SysWorkFlowDocumentRepository : ISysWorkFlowDocumentRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysWorkFlowDocumentRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }
        public async Task<IEnumerable<SystemWorkFlowDocument>> GetSystemWorkFlowDocuments(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemWorkFlowDocument>("EXEC sp_GetSystemWorkFlowDocuments @sysID,  @wfFlowID,  @wfFlowVer,  @wfNodeID,  @cultureID; ", new { sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID });
            }
        }

        public async Task<SystemWorkFlowDocumentDetail> GetSystemWorkFlowDocumentDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfDocSeq)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemWorkFlowDocumentDetail>("EXEC sp_GetSystemWorkFlowDocumentDetail @sysID,  @wfFlowID,  @wfFlowVer,  @wfNodeID,  @wfDocSeq; ", new { sysID, wfFlowID, wfFlowVer, wfNodeID, wfDocSeq });
            }
        }

        public async Task InsertSystemWorkFlowDocument(SystemWorkFlowDocumentDetail systemWorkFlowDocumentDetail)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_InsertSystemWorkFlowDocument
                                        @SysID,
                                        @WFFlowID,
                                        @WFFlowVer,
                                        @WFNodeID,
                                        @WFDocZHTW,
                                        @WFDocZHCN,
                                        @WFDocENUS,
                                        @WFDocTHTH,
                                        @WFDocJAJP,
                                        @WFDocKOKR,
                                        @IsReq,
                                        @Remark,
                                        @UpdUserID ";

                await conn.ExecuteAsync(commandText, systemWorkFlowDocumentDetail);
            }
        }

        public async Task EditSystemWorkFlowDocument(SystemWorkFlowDocumentDetail systemWorkFlowDocumentDetail)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemWorkFlowDocument
                                        @SysID,
                                        @WFFlowID,
                                        @WFFlowVer,
                                        @WFNodeID,
                                        @WFDocSeq,
                                        @WFDocZHTW,
                                        @WFDocZHCN,
                                        @WFDocENUS,
                                        @WFDocTHTH,
                                        @WFDocJAJP,
                                        @WFDocKOKR,
                                        @IsReq,
                                        @Remark,
                                        @UpdUserID ";

                await conn.ExecuteAsync(commandText, systemWorkFlowDocumentDetail);
            }
        }

        public async Task<string> DeleteSystemWorkFlowDocument(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfDocSeq)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleAsync<string>("EXEC sp_DeleteSystemWorkFlowDocument @sysID,  @wfFlowID,  @wfFlowVer,  @wfNodeID, @wfDocSeq",
                    new { sysID, wfFlowID, wfFlowVer, wfNodeID, wfDocSeq});

            }
        }
    }
}
