using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysEDIRepository
    {
        public async Task InsertSystemEDIFlowLog(SystemEDILogSetting systemEDILogSetting)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync(@"EXEC dbo.sp_InsertSystemEDIFlowLog @sysID, @dataDate, @ediFlowID, @statusID, @isAutomatic, @isDeleted, @updUserID", new
                {
                    sysID =  systemEDILogSetting.SysID,
                    dataDate =  systemEDILogSetting.DataDate,
                    ediFlowID =  systemEDILogSetting.EDIFlowID,
                    statusID =  systemEDILogSetting.StatusID,
                    isAutomatic = systemEDILogSetting.IsAutomatic,
                    isDeleted = systemEDILogSetting.IsDeleted,
                    updUserID = systemEDILogSetting.UpdUserID
                });
            }
        }
    }
}

