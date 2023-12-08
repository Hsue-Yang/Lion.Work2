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
        public async Task<IEnumerable<SystemEDIFlow>> GetSystemEDIFlows(string sysID, string schFrequency, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemEDIFlow>("EXEC dbo.sp_GetSystemEDIFlows @sysID, @cultureID, @schFrequency; ", new { sysID, cultureID, schFrequency });
            }
        }

        public async Task EditEDIFlowSettingSort(List<SystemEDIFlowSort> SystemEDIFlowSort)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_EditSystemEDIFlowSortOrder @editSystemEDIFlow;"
                        , new { editSystemEDIFlow = new TableValuedParameter(GetEDIFlowDataTable(SystemEDIFlowSort, "type_SystemEDIFlow")) });
            }
        }

        public async Task<string> GetSystemEDIIPAddress(string sysID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<string>("EXEC dbo.sp_GetSystemEDIIPAddress @sysID;", new { sysID });
            }
        }

        public async Task<SystemEDIFlowDetails> GetSystemEDIFlowDetail(string sysID, string ediFlowID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemEDIFlowDetails>("EXEC dbo.sp_GetSystemEDIFlowDetail @sysID, @ediflowID; ", new { sysID, ediFlowID });
            }
        }

        public async Task EditSystemEDIFlowDetail(SystemEDIFlowDetail systemEDIFlowDetail)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemEDIFlowDetail @SystemEDIFixEDTime, @SystemEDIFlowDetails";
                await conn.ExecuteAsync(commandText, new
                {
                    SystemEDIFixEDTime = new TableValuedParameter(GetEDIFixDataTable(systemEDIFlowDetail.SystemEDIFixEDTime, "type_SystemEDIFixEDTime")),
                    SystemEDIFlowDetails = new TableValuedParameter(GetEDIDetailDataTable(systemEDIFlowDetail.SystemEDIFlowDetails, "type_SystemEDIFlowDetail"))
                });
            }
        }

        public async Task<EnumDeleteResult> DeleteSystemEDIFlowDetail(string sysID, string ediflowID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                var result = await conn.ExecuteScalarAsync<string>("EXEC dbo.sp_DeleteSystemEDIFlowDetail @sysID, @ediflowID;", new { sysID, ediflowID });

                if (result == EnumYN.N.ToString())
                {
                    return EnumDeleteResult.DataExist;
                }

                return result == EnumYN.Y.ToString()
                    ? EnumDeleteResult.Success
                    : EnumDeleteResult.Failure;
            }
        }

        public async Task<string> GetFlowNewSortOrder(string sysID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<string>("EXEC dbo.sp_GetFlowNewSortOrder @sysID;", new { sysID });
            }
        }

        private DataTable GetEDIFlowDataTable<T>(List<T> list, string tableName)
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
                dtable.Columns.Add("SortOrder");
                dtable.Columns.Add("UpdUserID");
                dtable.Columns.Add("EDIFlowID");
            }

            return dtable;
        }


        private DataTable GetEDIFixDataTable<T>(List<T> list, string tableName)
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
                dtable.Columns.Add("ExeCuteTIME");
                dtable.Columns.Add("UpdUserID");
            }

            return dtable;
        }

        private DataTable GetEDIDetailDataTable<T>(T obj, string tableName)
        {
            DataTable dtable = new DataTable(tableName);

            if (obj != null)
            {
                Type type = obj.GetType();

                foreach (PropertyInfo pi in type.GetProperties())
                {
                    if (pi.Name != "ExecuteTimeList" && pi.Name != "EDIFlowNM")
                    {
                        dtable.Columns.Add(new DataColumn(pi.Name));
                    }
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
                dtable.Columns.Add("EDIFlowZHTW");
                dtable.Columns.Add("EDIFlowZHCN");
                dtable.Columns.Add("EDIFlowENUS");
                dtable.Columns.Add("EDIFlowTHTH");
                dtable.Columns.Add("EDIFlowJAJP");
                dtable.Columns.Add("EDIFlowKOKR");
                dtable.Columns.Add("SCHFrequency");
                dtable.Columns.Add("SCHStartDate");
                dtable.Columns.Add("SCHStartTime");
                dtable.Columns.Add("SCHIntervalNum");
                dtable.Columns.Add("SCHIntervalTime");
                dtable.Columns.Add("SCHEndTime");
                dtable.Columns.Add("SCHWeeks");
                dtable.Columns.Add("SCHDaysStr");
                dtable.Columns.Add("SCHDataDelay");
                dtable.Columns.Add("SCHKeepLogDay");
                dtable.Columns.Add("PATHSCmd");
                dtable.Columns.Add("PATHSDat");
                dtable.Columns.Add("PATHSSrc");
                dtable.Columns.Add("PATHSRes");
                dtable.Columns.Add("PATHSBad");
                dtable.Columns.Add("PATHSLog");
                dtable.Columns.Add("PATHSFlowXml");
                dtable.Columns.Add("PATHSFlowCmd");
                dtable.Columns.Add("PATHSZipDat");
                dtable.Columns.Add("PATHSException");
                dtable.Columns.Add("PATHSSummary");
                dtable.Columns.Add("SortOrder");
                dtable.Columns.Add("UpdUserID");
                dtable.Columns.Add("ExeCuteTIME");
            }

            return dtable;
        }

        public async Task<IEnumerable<SystemEDIFlowSchedule>> GetSystemEDIFlowScheduleList(string sysID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemEDIFlowSchedule>("EXEC dbo.sp_GetSystemEDIFlowScheduleList @sysID, @cultureID; ", new { sysID, cultureID });
            }
        }

        public async Task<IEnumerable<SystemEDIFlowByIds>> GetSystemEDIFlowByIds(string sysID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemEDIFlowByIds>("EXEC dbo.sp_GetSystemEDIFlowByIds @sysID, @cultureID; ", new { sysID, cultureID });
            }
        }

        public async Task<SystemEDIXML> GetSystemEDIFlowXMLDetail(string sysID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemEDIFlowXMLDetail @sysID, @cultureID;", new { sysID, cultureID });    
                return new SystemEDIXML
                {
                    SystemEDIFlowDetails = multi.Read<SystemEDIFlowDetails>().ToList(),
                    SystemEDIJobDetails = multi.Read<SystemEDIJob>().ToList(),
                    SystemEDIConDetails = multi.Read<SystemEDICon>().ToList(),
                    SystemEDIParDetails = multi.Read<SystemEDIPara>().ToList(),
                    SystemEDIFlowExecuteTimeDetails = multi.Read<SystemEDIFlowExecuteTime>().ToList()
                };
            }
        }

    }
}
