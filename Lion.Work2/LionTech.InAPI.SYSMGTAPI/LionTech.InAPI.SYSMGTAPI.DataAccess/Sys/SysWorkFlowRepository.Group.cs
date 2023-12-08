using Dapper;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysWorkFlowRepository
    {
        public async Task<IEnumerable<SystemWorkFlowGroup>> GetSystemWorkFlowGroups(string sysID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemWorkFlowGroup>("EXEC dbo.sp_GetSystemWorkFlowGroups @sysID, @cultureID; ", new { sysID, cultureID });
            }
        }

        public async Task<SystemWorkFlowGroupDetail> GetSystemWorkFlowGroupDetail(string sysID, string wfFlowGroupID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleAsync<SystemWorkFlowGroupDetail>("EXEC dbo.sp_GetSystemWorkFlowGroupDetail @sysID, @wfFlowGroupID", new { sysID, wfFlowGroupID });
            }
        }

        public async Task<bool> EditSystemWorkFlowGroupDetail(SystemWorkFlowGroupDetail systemWorkFlowGroupDetail)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemWorkFlowGroupDetail 
                                            @sysID
                                          , @wfFlowGroupID
                                          , @wfFlowGroupZHTW
                                          , @wfFlowGroupZHCN
                                          , @wfFlowGroupENUS
                                          , @wfFlowGroupTHTH
                                          , @wfFlowGroupJAJP
                                          , @wfFlowGroupKOKR
                                          , @sortOrder
                                          , @updUserID";

                return await conn.ExecuteScalarAsync<bool>(commandText, new
                {
                    systemWorkFlowGroupDetail.SysID,
                    systemWorkFlowGroupDetail.WFFlowGroupID,
                    systemWorkFlowGroupDetail.WFFlowGroupZHTW,
                    systemWorkFlowGroupDetail.WFFlowGroupZHCN,
                    systemWorkFlowGroupDetail.WFFlowGroupENUS,
                    systemWorkFlowGroupDetail.WFFlowGroupTHTH,
                    systemWorkFlowGroupDetail.WFFlowGroupJAJP,
                    systemWorkFlowGroupDetail.WFFlowGroupKOKR,
                    systemWorkFlowGroupDetail.SortOrder,
                    systemWorkFlowGroupDetail.UpdUserID
                });
            }
        }

        public async Task<bool> CheckSystemWorkFlowGroupExists(string sysID, string wfFlowGroupID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.ExecuteScalarAsync<bool>("EXEC dbo.sp_GetSystemWorkFlowGroupIsExists @sysID, @wfFlowGroupID", new { sysID, wfFlowGroupID });
            }
        }

        public async Task<bool> CheckSystemWorkFlowExists(string sysID, string wfFlowGroupID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.ExecuteScalarAsync<bool>("EXEC dbo.sp_GetSystemWorkFlowIsExists @sysID, @wfFlowGroupID", new { sysID, wfFlowGroupID });
            }
        }

        public async Task<bool> DeleteSystemWorkFlowGroupDetail(string sysID, string wfFlowGroupID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.ExecuteScalarAsync<bool>("EXEC dbo.sp_DeleteSystemWorkFlowGroupDetail @sysID, @wfFlowGroupID", new { sysID, wfFlowGroupID });
            }
        }

        public async Task<IEnumerable<SystemWorkFlowGroupIDs>> GetSystemWorkFlowGroupIDs(string sysID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemWorkFlowGroupIDs>("EXEC dbo.sp_GetSystemWorkFlowGroupIDs @sysID, @cultureID;",
                                                                     new { sysID, cultureID });
            }
        }
    }
}
