using Dapper;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysWorkFlowRepository
    {

        public async Task<IEnumerable<SystemWorkFlowNode>> GetSystemWorkFlowNodes(string sysID, string cultureID, string wfFlowID, string wfFlowVer)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemWorkFlowNode>("EXEC dbo.sp_GetSystemWorkFlowNodes @sysID, @cultureID, @wfFlowID, @wfFlowVer; ", new { sysID, cultureID, wfFlowID, wfFlowVer });
            }
        }

        public async Task<SystemWorkFlowNode> GetSystemWorkFlowNode(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemWorkFlowNode>("EXEC dbo.sp_GetSystemWorkFlowNode @sysID, @wfFlowID, @wfFlowVer, @wfNodeID, @cultureID;",
                                                                                    new { sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID });
            }
        }
    }
}
