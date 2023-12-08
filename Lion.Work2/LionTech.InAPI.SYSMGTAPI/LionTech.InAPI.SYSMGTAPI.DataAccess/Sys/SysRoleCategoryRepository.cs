using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysRoleCategoryRepository : ISysRoleCategoryRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysRoleCategoryRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<SystemRoleCategory>> GetSystemRoleCategoryByIdList(string sysID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemRoleCategory>("EXEC dbo.sp_GetSystemRoleCategoryByIds @sysID, @cultureID;", new { sysID, cultureID });
            }
        }

        public async Task<IEnumerable<SystemRoleCategory>> GetSystemRoleCategoryList(string sysID, string roleCategoryNM, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemRoleCategory>("EXEC dbo.sp_GetSystemRoleCategorys @sysID, @roleCategoryNM, @cultureID;", new { sysID, roleCategoryNM, cultureID });
            }
        }

        public async Task<SystemRoleCategoryMain> GetSystemRoleCategoryDetail(string sysID, string roleCategoryID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemRoleCategoryMain>("EXEC dbo.sp_GetSystemRoleCategory @sysID, @roleCategoryID;", new { sysID, roleCategoryID });
            }
        }

        public async Task EditSystemRoleCategoryDetail(SystemRoleCategoryMain systemRoleCategory)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText =
                    @"EXEC dbo.sp_EditSystemRoleCategory
                      @SysID, @RoleCategoryID
                    , @RoleCategoryNMZHTW, @RoleCategoryNMZHCN, @RoleCategoryNMENUS, @RoleCategoryNMTHTH, @RoleCategoryNMJAJP, @RoleCategoryNMKOKR
                    , @SortOrder, @UpdUserID";
                await conn.ExecuteAsync(commandText, systemRoleCategory);
            }
        }

        public async Task<EnumDeleteSystemRoleCategoryDetailResult> DeleteSystemRoleCategoryDetail(string sysID, string roleCategoryID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                var result = await conn.ExecuteScalarAsync<string>("EXEC dbo.sp_DeleteSystemRoleCategory @sysID, @roleCategoryID;", new { sysID, roleCategoryID });

                if (result == null) return EnumDeleteSystemRoleCategoryDetailResult.DataExist;

                return result == EnumYN.Y.ToString()
                    ? EnumDeleteSystemRoleCategoryDetailResult.Success
                    : EnumDeleteSystemRoleCategoryDetailResult.Failure;
            }
        }
    }
}