using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysSettingRepository
    {
        public async Task<IEnumerable<SystemSub>> GetUserSystemSubList(string userID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemSub>("EXEC dbo.sp_GetUserSystemSubs @userID, @cultureID;", new { userID, cultureID });
            }
        }

        public async Task<IEnumerable<SystemSetting.SystemSub>> GetSystemSubByIdList(string sysID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                string commandText = @"EXEC dbo.sp_GetSystemSubByIds @sysID, @cultureID;";

                return await conn.QueryAsync<SystemSetting.SystemSub>(commandText, new { sysID, cultureID });
            }
        }

        public async Task<IEnumerable<SystemSub>> GetSystemSubList(string sysID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemSub>("EXEC dbo.sp_GetSystemSubs @sysID;", new { sysID });
            }
        }

        public async Task EditSystemSubDetail(SystemSub systemSub)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText =
                   @"EXEC dbo.sp_EditSystemSub
                      @SubSysID, @SysID
                    , @SysMANUserID, @SysNMZHTW, @SysNMZHCN, @SysNMENUS, @SysNMTHTH, @SysNMJAJP, @SysNMKOKR
                    , @SortOrder, @UpdUserID;";

                await conn.ExecuteAsync(commandText, systemSub);
            }
        }

        public async Task DeleteSystemSubDetail(string sysID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_DeleteSystemSub @sysID;", new { sysID });
            }
        }
    }
}