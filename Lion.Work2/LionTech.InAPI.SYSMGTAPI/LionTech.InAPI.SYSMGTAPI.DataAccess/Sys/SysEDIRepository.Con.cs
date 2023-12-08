using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysEDIRepository
    {

        public async Task<IEnumerable<SystemEDICon>> GetSystemEDICons(string sysID, string cultureID, string ediflowID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemEDICon>("EXEC dbo.sp_GetSystemEDICons @sysID, @cultureID, @ediflowID; ", new { sysID, cultureID, ediflowID });
            }
        }

        public async Task EditEDIConSort(List<SystemEDICon> systemEDICon)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                List<SystemEDIConValue> ediConValueList = new List<SystemEDIConValue>();
                foreach (var ediConValue in systemEDICon)
                {
                    ediConValueList.Add(new SystemEDIConValue
                    {
                        SysID = string.IsNullOrWhiteSpace(ediConValue.SysID) ? null : ediConValue.SysID,
                        SortOrder = string.IsNullOrWhiteSpace(ediConValue.SortOrder) ? null : ediConValue.SortOrder,
                        UpdUserID = string.IsNullOrWhiteSpace(ediConValue.UpdUserID) ? null : ediConValue.UpdUserID,
                        EDIFlowID = string.IsNullOrWhiteSpace(ediConValue.EDIFlowID) ? null : ediConValue.EDIFlowID,
                        EDIConID = string.IsNullOrWhiteSpace(ediConValue.EDIConID) ? null : ediConValue.EDIConID
                    });
                }
                await conn.ExecuteAsync("EXEC dbo.sp_EditSystemEDIConSortOrder @SystemEDICon"
                        , new { SystemEDICon = new TableValuedParameter(GetEDIConDataTable(ediConValueList, "type_SystemEDICon")) });
            }
        }

        public async Task<SystemEDIConDetail> GetSystemEDIConDetail(string sysID, string ediflowID, string ediconID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemEDIConDetail>("EXEC dbo.sp_GetSystemEDIConDetail @sysID, @ediflowID, @ediconID;", new { sysID, ediflowID, ediconID });
            }
        }

        public async Task EditSystemEDIConDetail(SystemEDIConDetail systemEDIConDetail)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemEDIConDetail @systemEDIConDetail";
                await conn.ExecuteAsync(commandText, new
                {
                    systemEDIConDetail = new TableValuedParameter(GetEDIConDataTable(systemEDIConDetail, "type_SystemEDIConDetail")),
                });
            }
        }

        public async Task<EnumDeleteResult> DeleteSystemEDIConDetail(string sysID, string ediflowID, string ediconID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                var result = await conn.ExecuteScalarAsync<string>("EXEC dbo.sp_DeleteSystemEDIConDetail @sysID, @ediflowID, @ediconID ;", new { sysID, ediflowID, ediconID });

                if (result == EnumYN.N.ToString()) return EnumDeleteResult.DataExist;

                return result == EnumYN.Y.ToString()
                    ? EnumDeleteResult.Success
                    : EnumDeleteResult.Failure;
            }
        }

        public async Task<string> GetConNewSortOrder(string sysID, string ediflowID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<string>("EXEC dbo.sp_GetSystemEDIConSort @sysID, @ediflowID ;", new { sysID, ediflowID });
            }
        }
        private DataTable GetEDIConDataTable<T>(List<T> list, string tableName)
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
                dtable.Columns.Add("EDIFlowID");
                dtable.Columns.Add("EDIConID");
                dtable.Columns.Add("SortOrder");
                dtable.Columns.Add("UpdUserID");
            }

            return dtable;
        }

        private DataTable GetEDIConDataTable<T>(T obj, string tableName)
        {
            DataTable dtable = new DataTable(tableName);

            if (obj != null)
            {
                Type type = obj.GetType();

                foreach (PropertyInfo pi in type.GetProperties())
                {
                    dtable.Columns.Add(new DataColumn(pi.Name));
                }
                DataRow dr = dtable.NewRow();
                foreach (DataColumn dc in dtable.Columns)
                {
                    var data = obj.GetType().GetProperty(dc.ColumnName).GetValue(obj, null);
                    dr[dc.ColumnName] = data;
                }
                dtable.Rows.Add(dr);
            }
            else
            {
                dtable.Columns.Add("SysID");
                dtable.Columns.Add("EDIFlowID");
                dtable.Columns.Add("EDIConID");
                dtable.Columns.Add("EDIConZHTW");
                dtable.Columns.Add("EDIConZHCN");
                dtable.Columns.Add("EDIConENUS");
                dtable.Columns.Add("EDIConTHTH");
                dtable.Columns.Add("EDIConJAJP");
                dtable.Columns.Add("EDIConKOKR");
                dtable.Columns.Add("ProviderName");
                dtable.Columns.Add("ConValue");
                dtable.Columns.Add("SortOrder");
                dtable.Columns.Add("UpdUserID");
            }

            return dtable;
        }
    }
}
