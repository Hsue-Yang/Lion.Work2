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

        public async Task<(int rowCount, IEnumerable<SystemRoleFunList> SystemRoleFunList)> GetSystemRoleFunList(string sysID, string roleID, string funControllerId, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemRoleFuns @sysID, @roleID, @funControllerId, @cultureID, @pageIndex, @pageSize;"
                                                   , new { sysID, roleID, funControllerId, cultureID, pageIndex, pageSize });
                var rowCount = multi.Read<int>().SingleOrDefault();
                var SystemRoleFunList = multi.Read<SystemRoleFunList>();

                return (rowCount, SystemRoleFunList);
            }
        }

        public async Task EditSystemRoleFunList(SystemRoleFunEditLists systemRoleFunEditLists)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemRoleFuns @SystemRoleFunsAddList, @SystemRoleFunsDeleteList";
                await conn.ExecuteAsync(commandText
                  , new { SystemRoleFunsAddList = new TableValuedParameter(GetDataTable(systemRoleFunEditLists.SystemRoleFunsAddList, "type_SystemRoleFun"))
                        , SystemRoleFunsDeleteList = new TableValuedParameter(GetDataTable(systemRoleFunEditLists.SystemRoleFunsDeleteList, "type_SystemRoleFun")) });
            }
        }

        private DataTable GetDataTable<T>(List<T> list, string tableName)
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
                dtable.Columns.Add("SysID");
                dtable.Columns.Add("RoleID");
                dtable.Columns.Add("FunControllerID");
                dtable.Columns.Add("FunActionName");
                dtable.Columns.Add("UpdUserID");
            }

            return dtable;
        }
    }
}
