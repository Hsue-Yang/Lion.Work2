using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysEDIRepository
    {
        public async Task<(int rowCount, IEnumerable<SystemEDIJobLog> systemEDIJobLogList)> GetSystemEDIJobLogs(string sysID, string ediNO, string ediFlowID, string ediJobID, string ediFlowIDSearch, string ediJobIDSearch, string edidate, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync(@"EXEC dbo.sp_GetSystemEDIJobLogs @sysID, @ediNO, @ediFlowID, @ediJobID, @ediFlowIDSearch
                                                            , @ediJobIDSearch, @edidate, @cultureID, @pageIndex, @pageSize"
                                                         , new { sysID, ediNO, ediFlowID, ediJobID, ediFlowIDSearch,
                                                             ediJobIDSearch, edidate, cultureID, pageIndex, pageSize});

                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemEDIJobLogList = multi.Read<SystemEDIJobLog>();

                return (rowCount, systemEDIJobLogList);
            }
        }
    }
}
