using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysRoleRepository
    {
        public async Task<(int rowCount, IEnumerable<SystemRoleElms> systemRoleElmList)> GetSystemRoleElmsList(string sysID, string roleID, string cultureID, string funControllerId, string funactionNM, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemRoleElms @sysID, @roleID, @cultureID, @funControllerId, @funactionNM, @pageIndex, @pageSize;"
                                                   , new { sysID, roleID, cultureID, funControllerId, funactionNM, pageIndex, pageSize });
                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemRoleElmList = multi.Read<SystemRoleElms>();

                return (rowCount, systemRoleElmList);
            }
        }

        public async Task EditSystemRoleElmsList(SystemRoleElmEditLists systemRoleElmEditLists)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemRoleElms @SystemRoleFunElmAdd, @SystemRoleFunElmDelete";
                await conn.ExecuteAsync(commandText
                  , new { SystemRoleFunElmAdd = new TableValuedParameter(GetElmDataTable(systemRoleElmEditLists.SystemRoleElmAddList, "type_SystemRoleFunElm"))
                       ,  SystemRoleFunElmDelete = new TableValuedParameter(GetElmDataTable(systemRoleElmEditLists.SystemRoleElmDeleteList, "type_SystemRoleFunElm")) });
            }
        }

        public async Task<IEnumerable<SysElmName>> GetSystemFunElmByIdList(string sysID, string cultureID, string funControllerId, string funactionNM)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SysElmName>("EXEC dbo.sp_GetSystemFunElmByIds @sysID, @cultureID, @funControllerId, @funactionNM;"
                    , new { sysID, cultureID, funControllerId, funactionNM });
            }
        }

        private DataTable GetElmDataTable<T>(List<T> list, string tableName)
        {
            DataTable dtable = new DataTable(tableName);

            if (list.Any() && list != null)
            {
                Type type = list.First().GetType();

                foreach (PropertyInfo pi in type.GetProperties())
                {
                    dtable.Columns.Add(new DataColumn(pi.Name));
                }

                foreach (var item in list)
                {
                    DataRow dr = dtable.NewRow();
                    foreach (DataColumn dc in dtable.Columns)
                    {
                        var data = item.GetType().GetProperty(dc.ColumnName).GetValue(item, null);
                        dr[dc.ColumnName] = data;
                    }
                    dtable.Rows.Add(dr);
                }
            }
            else
            {
                dtable.Columns.Add("SYS_ID");
                dtable.Columns.Add("ROLE_ID");
                dtable.Columns.Add("FUN_CONTROLLER_ID");
                dtable.Columns.Add("FUN_ACTION_NAME");
                dtable.Columns.Add("ELM_ID");
                dtable.Columns.Add("DISPLAY_STS");
                dtable.Columns.Add("UPD_USER_ID");
            }

            return dtable;
        }
    }
}