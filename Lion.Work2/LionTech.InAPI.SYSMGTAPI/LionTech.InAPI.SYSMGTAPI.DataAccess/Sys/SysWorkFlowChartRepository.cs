using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class SysWorkFlowChartRepository: ISysWorkFlowChartRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysWorkFlowChartRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }
        public async Task<IEnumerable<SystemWorkFlowNodePosition>> GetSystemWorkFlowNodePositions(string sysID, string wfFlowID, string wfFlowVer)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemWorkFlowNodePosition>("EXEC sp_GetSystemWorkFlowNodePositions @sysID, @wfFlowID, @wfFlowVer; ", new { sysID, wfFlowID, wfFlowVer });
            }
        }

        public async Task<IEnumerable<SystemWorkFlowArrowPosition>> GetSystemWorkFlowArrowPositions(string sysID, string wfFlowID, string wfFlowVer)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemWorkFlowArrowPosition>("EXEC sp_GetSystemWorkFlowArrowPositions @sysID,  @wfFlowID,  @wfFlowVer; ", new { sysID, wfFlowID, wfFlowVer });
            }
        }
    }
}