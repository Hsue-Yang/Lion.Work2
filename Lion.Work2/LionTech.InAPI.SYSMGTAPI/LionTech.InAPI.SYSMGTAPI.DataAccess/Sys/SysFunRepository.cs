using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysFunRepository : ISysFunRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysFunRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<SystemFun>> GetUserSystemFunList(string userID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemFun>("EXEC dbo.sp_GetUserSystemFuns @userID, @cultureID;", new { userID, cultureID });
            }
        }

        public async Task<(int rowCount, IEnumerable<SystemFun> SystemFunList)> GetSystemFunList(string sysID, string subSysID, string funControllerID, string funActionName, string funGroupNM, string funNM, string funMenuSysID, string funMenu, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                string commandText =
                    @"EXEC dbo.sp_GetSystemFuns
                      @sysID, @subSysID, @funControllerID, @funActionName
                    , @funGroupNM, @funNM, @funMenuSysID, @funMenu
                    , @cultureID, @pageIndex, @pageSize;";

                var multi = await conn.QueryMultipleAsync(commandText, new { sysID, subSysID, funControllerID, funActionName, funGroupNM, funNM, funMenuSysID, funMenu, cultureID, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemFunList = multi.Read<SystemFun>().ToList();
                var systemMenuFunList = multi.Read<SysMenuFun>().ToList();

                systemFunList.ForEach(row => { row.MenuFunList = systemMenuFunList.Where(m => m.FunControllerID == row.FunControllerID && m.FunActionName == row.FunActionName).ToList(); });

                return (rowCount, systemFunList);
            }
        }

        public async Task<SystemFunMain> GetSystemFunDetail(string sysID, string funControllerID, string funActionName, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemFunMain>("EXEC dbo.sp_GetSystemFun @sysID, @funControllerID, @funActionName, @cultureID;", new { sysID, funControllerID, funActionName, cultureID });
            }
        }

        public async Task<IEnumerable<SystemRoleFun>> GetSystemFunRoleList(string sysID, string funControllerID, string funActionName, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemRoleFun>("EXEC dbo.sp_GetSystemFunRoles @sysID, @funControllerID, @funActionName, @cultureID;", new { sysID, funControllerID, funActionName, cultureID });
            }
        }

        public async Task EditSystemFunByPurview(SystemFunMain systemFun)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_EditSystemFunByPurview @SysID, @FunControllerID, @FunActionName, @PurviewID, @UpdUserID", systemFun);
            }
        }

        public async Task EditSystemFunDetail(DataTable systemFuns, DataTable systemRoleFuns, DataTable systemMenuFuns)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemFun @systemFuns, @systemRoleFuns, @systemMenuFuns;";

                await conn.ExecuteAsync(commandText, new
                {
                    systemFuns = new TableValuedParameter(systemFuns),
                    systemRoleFuns = new TableValuedParameter(systemRoleFuns),
                    systemMenuFuns = new TableValuedParameter(systemMenuFuns)
                });
            }
        }

        public async Task<EnumDeleteSystemFunDetailResult> DeleteSystemFunDetail(string sysID, string funControllerID, string funActionName)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                var result = await conn.ExecuteScalarAsync<string>("EXEC dbo.sp_DeleteSystemFun @sysID, @funControllerID, @funActionName;", new { sysID, funControllerID, funActionName });

                if (result == null)
                {
                    return EnumDeleteSystemFunDetailResult.DataExist;
                }

                return result == EnumYN.Y.ToString()
                    ? EnumDeleteSystemFunDetailResult.Success
                    : EnumDeleteSystemFunDetailResult.Failure;
            }
        }

        public async Task<IEnumerable<SystemMenuFun>> GetSystemMenuFunList(string sysID, string funControllerID, string funActionName)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemMenuFun>("EXEC dbo.sp_GetSystemMenuFuns @sysID, @funControllerID, @funActionName;", new { sysID, funControllerID, funActionName });
            }
        }

        public async Task<IEnumerable<SystemFunToolFunName>> GetSystemFunNameList(string sysID, string funControllerID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemFunToolFunName>("EXEC dbo.sp_GetSystemFunNames @sysID, @funControllerID, @cultureID;", new { sysID, funControllerID, cultureID });
            }
        }

        public async Task<IEnumerable<SystemFunAction>> GetSystemFunActionList(string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemFunAction>("EXEC dbo.sp_GetSystemFunActions @cultureID;", new { cultureID });
            }
        }
    }
}