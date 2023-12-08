using System.Collections.Generic;
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
    public partial class SysFunMenuRepository : ISysFunMenuRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysFunMenuRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<SystemFunMenu>> GetSystemFunMenuByIdList(string sysID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemFunMenu>("EXEC dbo.sp_GetSystemFunMenuByIds @sysID, @cultureID;", new { sysID, cultureID });
            }
        }

        public async Task<(int rowCount, IEnumerable<SystemFunMenu> SystemFunMenuList)> GetSystemFunMenuList(string sysID, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemFunMenus @sysID, @cultureID, @pageIndex, @pageSize;", new { sysID, cultureID, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var SystemFunMenuList = multi.Read<SystemFunMenu>();

                return (rowCount, SystemFunMenuList);
            }
        }

        public async Task<SystemFunMenuMain> GetSystemFunMenuDetail(string sysID, string funMenu)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemFunMenuMain>("EXEC dbo.sp_GetSystemFunMenu @sysID, @funMenu;", new { sysID, funMenu });
            }
        }

        public async Task EditSystemFunMenuDetail(SystemFunMenuMain systemFunMenu)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText =
                    @"EXEC dbo.sp_EditSystemFunMenu
                      @SysID, @FunMenu
                    , @FunMenuNMZHTW,@FunMenuNMZHCN,@FunMenuNMENUS,@FunMenuNMTHTH,@FunMenuNMJAJP,@FunMenuNMKOKR
                    , @DefaultMenuID, @IsDisable, @SortOrder, @UpdUserID";

                await conn.ExecuteAsync(commandText, systemFunMenu);
            }
        }

        public async Task<EnumDeleteSystemFunMenuResult> DeleteSystemFunMenuDetail(string sysID, string funMenu)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = "EXEC sp_DeleteSystemFunMenu @sysID, @funMenu;";
                var result = await conn.ExecuteScalarAsync<string>(commandText, new { sysID, funMenu });

                if (result == null)
                {
                    return EnumDeleteSystemFunMenuResult.DataExist;
                }

                return result == EnumYN.Y.ToString()
                    ? EnumDeleteSystemFunMenuResult.Success
                    : EnumDeleteSystemFunMenuResult.Failure;
            }
        }
    }
}