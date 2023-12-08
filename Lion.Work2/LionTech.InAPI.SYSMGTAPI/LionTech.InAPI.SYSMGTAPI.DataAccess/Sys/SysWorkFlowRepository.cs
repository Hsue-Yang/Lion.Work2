using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysWorkFlowRepository : ISysWorkFlowRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysWorkFlowRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<SystemWorkFlow>> GetSystemWorkFlows(string sysID, string wfFlowGroupID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemWorkFlow>("EXEC dbo.sp_GetSystemWorkFlows @sysID, @wfFlowGroupID, @cultureID", new { sysID, wfFlowGroupID, cultureID });
            }
        }

        public async Task<SystemWorkFlowDetail> GetSystemWorkFlowDetail(string sysID, string wfFlowID, string wfFlowVer)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemWorkFlowDetail>("EXEC dbo.sp_GetSystemWorkFlowDetail @sysID, @wfFlowID, @wfFlowVer", new { sysID, wfFlowID, wfFlowVer });
            }
        }

        public async Task<bool> CheckWorkFlowIsExists(string sysID, string wfFlowID, string wfFlowVer)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.ExecuteScalarAsync<bool>("EXEC dbo.sp_GetWorkFlowIsExists @sysID, @wfFlowID, @wfFlowVer", new { sysID, wfFlowID, wfFlowVer });
            }
        }

        public async Task<IEnumerable<FlowRole>> GetSystemWorkFlowRoles(string sysID, string wfFlowID, string wfFlowVer, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<FlowRole>("EXEC dbo.sp_GetSystemWorkFlowRoles @sysID, @wfFlowID, @wfFlowVer, @cultureID", new { sysID, wfFlowID, wfFlowVer, cultureID });
            }
        }

        public async Task<bool> EditSystemWorkFlowDetail(SystemWorkFlowDetails systemWorkFlowDetails)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.ExecuteScalarAsync<bool>("EXEC dbo.sp_EditSystemWorkFlowDetail @systemWorkFlowRole, @systemWorkFlowDetail", new
                {
                    SystemWorkFlowRole = new TableValuedParameter(TableValuedParameter.GetDataTable(systemWorkFlowDetails.SystemWorkFlowRoles, "type_SystemWorkFlowRole")),
                    SystemWorkFlowDetail = new TableValuedParameter(TableValuedParameter.GetDataTable(systemWorkFlowDetails.SystemWorkFlowDetail, "type_SystemWorkFlowDetail"))
                });
            }
        }

        public async Task<bool> DeleteSystemWorkFlowDetail(string sysID, string wfFlowID, string wfFlowVer)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.ExecuteScalarAsync<bool>("EXEC dbo.sp_DeleteSystemWorkFlowDetail @sysID, @wfFlowID, @wfFlowVer", new { sysID, wfFlowID, wfFlowVer });
            }
        }

        public async Task<IEnumerable<SysUserSystemWorkFlowID>> GetSysUserSystemWorkFlowIDs(string sysID, string userID, string cultureID, string wfFlowGroupID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SysUserSystemWorkFlowID>("EXEC dbo.sp_GetSysUserSystemWorkFlowByIds @sysID, @wfFlowGroupID, @cultureID, @userID; ", new { sysID, wfFlowGroupID, cultureID, userID });
            }
        }
    }
}
