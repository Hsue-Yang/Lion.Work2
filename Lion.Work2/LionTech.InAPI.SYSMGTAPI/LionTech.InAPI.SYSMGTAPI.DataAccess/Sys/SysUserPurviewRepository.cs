using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class SysUserPurviewRepository : ISysUserPurviewRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysUserPurviewRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<SysUserPurview>> GetSysUserPurviews(string userID, string updUserID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SysUserPurview>("EXEC dbo.sp_GetSystemUserPurviews @userID, @updUserID, @cultureID ", new { userID, updUserID, cultureID });
            }
        }

        public async Task<IEnumerable<PurviewName>> GetPurviewNames(string sysID, string cultureID)
        {
            using (var conn=new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<PurviewName>("EXEC dbo.sp_GetSystemPurviewByIds @sysID, @cultureID", new { sysID, cultureID });
            }
        }

        public async Task<IEnumerable<SysUserPurviewDetails>> GetSysUserPurviewDetails(string sysID, string userID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SysUserPurviewDetails>("EXEC dbo.sp_GetSystemUserPurviewDetails @sysID, @userID, @cultureID ", new { sysID, userID, cultureID });
            }
        }

        public async Task EditSysUserPurviewDetail(UserPurviewPara userPurviewPara)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemUserPurviewDetail @sysID, @userID, @updUserID, @SystemUserPurview";
                await conn.QuerySingleOrDefaultAsync(commandText, new
                {
                    sysID= userPurviewPara.SysID,
                    userID= userPurviewPara.UserID,
                    updUserID= userPurviewPara.UserID,
                    SystemUserPurview = new TableValuedParameter(TableValuedParameter.GetDataTable(userPurviewPara.SysUserPurviewDetailList, "type_SystemUserPurview"))
                });
            }
        }
    }
}
