using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysRoleRepository : ISysRoleRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysRoleRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<SystemRole>> GetSystemRoleByIdList(string sysID, string roleCategoryID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemRole>("EXEC dbo.sp_GetSystemRoleByIds @sysID, @roleCategoryID, @cultureID;", new { sysID, @roleCategoryID, cultureID });
            }
        }

        public async Task<(int rowCount, IEnumerable<SystemRole> SystemRoleList)> GetSystemRoleList(string sysID, string roleID, string roleCategoryID, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemRoles @sysID, @roleID, @roleCategoryID, @cultureID, @pageIndex, @pageSize;", new { sysID, roleID, roleCategoryID, cultureID, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var SystemRoleList = multi.Read<SystemRole>();

                return (rowCount, SystemRoleList);
            }
        }

        public async Task<SystemRoleMain> GetSystemRole(string sysID, string roleID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemRoleMain>("EXEC dbo.sp_GetSystemRole @sysID, @roleID;", new { sysID, roleID });
            }
        }

        public async Task EditSystemRoleByCategory(SystemRoleMain systemRole)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @" DECLARE @systemRole AS type_SystemRole;
                                        INSERT INTO @systemRole (SYS_ID,ROLE_CATEGORY_ID,ROLE_ID,UPD_USER_ID)
                                        VALUES(@SysID,@RoleCategoryID,@RoleID,@UpdUserID);                                        
                                        EXEC dbo.sp_EditSystemRoleByCategory @systemRole;";

                await conn.ExecuteAsync(commandText, systemRole);
            }
        }

        public async Task EditSystemRoleDetail(SystemRoleMain systemRole)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @" DECLARE @systemRole AS type_SystemRole;
                                        INSERT INTO @systemRole (SYS_ID,ROLE_CATEGORY_ID,ROLE_ID,ROLE_NM_ZH_TW,ROLE_NM_ZH_CN,ROLE_NM_EN_US,ROLE_NM_TH_TH,ROLE_NM_JA_JP,ROLE_NM_KO_KR,IS_MASTER,UPD_USER_ID)
                                        VALUES(@SysID,@RoleCategoryID,@RoleID,@RoleNMZHTW,@RoleNMZHCN,@RoleNMENUS,@RoleNMTHTH,@RoleNMJAJP,@RoleNMKOKR,@IsMaster,@UpdUserID);
                                        EXEC dbo.sp_EditSystemRole @systemRole;";

                await conn.ExecuteAsync(commandText, systemRole);
            }
        }

        public async Task DeleteSystemRoleDetail(string sysID, string roleID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_DeleteSystemRole @sysID, @roleID;", new { sysID, roleID });
            }
        }
    }
}
