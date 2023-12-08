using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysEDIRepository
    {
        public async Task<(int rowCount, IEnumerable<SystemEDIFlowLog> systemEDIFlowLogList)> GetSystemEDIFlowLogs(string sysID, string ediNO, string ediFlowID, string ediDate, string dataDate, string resultID, string statusID, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemEDIFlowLogs @sysID, @ediNO, @ediFlowID, @ediDate, @dataDate, @resultID, @statusID, @cultureID, @pageIndex, @pageSize;"
                                                         , new { sysID, ediNO, ediFlowID, ediDate, dataDate, resultID, statusID, cultureID, pageIndex, pageSize });
                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemEDIFlowLogList = multi.Read<SystemEDIFlowLog>();

                return (rowCount, systemEDIFlowLogList);
            }
        }

        public async Task EditEDIFlowLogs(SystemEDIFlowLogUpdateWaitStatus systemEDIFlowLogUpdateWaitStatus)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync(@"EXEC dbo.sp_EditSystemEDIFlowLog @sysID, @dataDate, @ediFlowID, @updUserID", new
                {
                    sysID = systemEDIFlowLogUpdateWaitStatus.SysID,
                    dataDate = systemEDIFlowLogUpdateWaitStatus.DataDate,
                    ediFlowID = systemEDIFlowLogUpdateWaitStatus.EDIFlowID,
                    updUserID = systemEDIFlowLogUpdateWaitStatus.UpdUserID
                });
            }
        }
    }
}

