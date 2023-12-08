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
        public async Task<IEnumerable<SystemEDIJob>> GetSystemEDIJobs(string sysID, string ediFlowID, string ediJobType, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemEDIJob>("EXEC dbo.sp_GetSystemEDIJobs @sysID, @ediFlowID, @ediJobType, @cultureID; ", new { sysID, ediFlowID, ediJobType, cultureID });
            }
        }

        public async Task<SystemEDIJob> GetSystemEDIJobDetail(string sysID, string ediFlowID, string ediJobID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemEDIJob>("EXEC dbo.sp_GetSystemEDIJobDetail @sysID, @ediflowID, @ediJobID; ", new { sysID, ediFlowID, ediJobID });
            }
        }

        public async Task<EnumDeleteResult> DeleteSystemEDIJobDetail(string sysID, string ediflowID, string ediJobID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                var result = await conn.ExecuteScalarAsync<string>("EXEC dbo.sp_DeleteSystemEDIJobDetail @sysID, @ediflowID, @ediJobID;", new { sysID, ediflowID, ediJobID });
                if (result == EnumYN.N.ToString()) return EnumDeleteResult.DataExist;

                return result == EnumYN.Y.ToString()
                    ? EnumDeleteResult.Success
                    : EnumDeleteResult.Failure;
            }
        }

        public async Task<IEnumerable<SystemDepEDIJobID>> GetSystemEDIJobByIds(string sysID, string ediFlowID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemDepEDIJobID>("EXEC dbo.sp_GetSystemEDIJobByIds @sysID, @ediflowID, @cultureID; ", new { sysID, ediFlowID, cultureID });
            }
        }

        public async Task<string> GetJobMaxSortOrder(string sysID, string ediFlowID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<string>("EXEC dbo.sp_GetJobMaxSortOrder @sysID, @ediFlowID;", new { sysID, ediFlowID });
            }
        }

        public async Task<IEnumerable<SystemEDIPara>> GetSystemEDIParas(string sysID, string ediFlowID, string ediJobID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemEDIPara>("EXEC dbo.sp_GetSystemEDIParas @sysID, @ediFlowID, @ediJobID, @cultureID; ", new { sysID, ediFlowID, ediJobID, cultureID });
            }
        }

        public async Task DeleteSystemEDIPara(string sysID, string ediFlowID, string ediJobID, string ediJobParaID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_DeleteSystemEDIPara @sysID, @ediFlowID, @ediJobID, @ediJobParaID;", new { sysID, ediFlowID, ediJobID, ediJobParaID });
            }
        }

        public async Task EditSystemEDIJobSortOrder(List<EditEDIJobValue> editEDIJobValues)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_EditSystemEDIJobSortOrder @editSystemEDIJob;"
                        , new { editSystemEDIJob = new TableValuedParameter(GetEDIJobDataTable(editEDIJobValues, "type_SystemEDIJob")) });
            }
        }

        public async Task EditSystemEDIJobDetail(EditEDIJobDetail systemEDIJobDetail)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemEDIJobDetail @SystemEDIJobDetail";
                await conn.ExecuteAsync(commandText, new
                {
                    SystemEDIJobDetail = new TableValuedParameter(GetEDIJobDetailDataTable(systemEDIJobDetail, "type_SystemEDIJobDetail")),
                });
            }
        }

        public async Task EditSystemEDIJobImport(EdiJobSettingPara ediJobSettingPara)
        {

            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemEDIJobImport @SystemEDIJobDetails, @SystemEDIParaDetails";
                await conn.ExecuteAsync(commandText, new
                {
                    SystemEDIJobDetails = new TableValuedParameter(GetEDIJobDetailDataTable(ediJobSettingPara.EDIJobDetailEditList, "type_SystemEDIJobDetail")),
                    SystemEDIParaDetails = new TableValuedParameter(GetEDIJobParaDataTable(ediJobSettingPara.EDIJobParaEditList, "type_SystemEDIParaDetail"))
                });
            }
        }

        public async Task EditSystemEDIParaSortOrder(List<EditEDIJobPara> editEDIJobParas)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_EditSystemEDIParaSortOrder @editSystemEDIPara;"
                        , new { editSystemEDIPara = new TableValuedParameter(GetEDIJobParaDataTable(editEDIJobParas, "type_SystemEDIParaDetail")) });
            }
        }

        public async Task EditSystemEDIPara(EditEDIJobPara editEDIJobParas)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemEDIPara @SystemEDIParaDetail";
                await conn.ExecuteAsync(commandText, new
                {
                    SystemEDIParaDetail = new TableValuedParameter(GetEDIJobParaDataTable(editEDIJobParas, "type_SystemEDIParaDetail")),
                });
            }
        }
        public async Task<IEnumerable<SystemEDICon>> GetSystemEDIConByIdsProviderCons(string sysID, string ediflowID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemEDICon>("EXEC dbo.sp_GetSystemEDIConByIdsProviderCons @sysID, @ediflowID; ", new { sysID, ediflowID });
            }
        }

        public async Task<IEnumerable<SystemEDICon>> GetSystemEDIConByIds(string sysID, string ediflowID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QueryAsync<SystemEDICon>("EXEC sp_GetSystemEDIConByIds @sysID, @ediflowID, @cultureID ;", new { sysID, ediflowID, cultureID });
            }
        }

        private DataTable GetEDIJobDataTable<T>(List<T> list, string tableName)
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
                dtable.Columns.Add("EDIJobID");
                dtable.Columns.Add("EDIFlowID");
            }

            return dtable;
        }

        private DataTable GetEDIJobDetailDataTable<T>(T obj, string tableName)
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
                dtable.Columns.Add("EDIJobID");
                dtable.Columns.Add("EDIJobZHTW");
                dtable.Columns.Add("EDIJobZHCN");
                dtable.Columns.Add("EDIJobENUS");
                dtable.Columns.Add("EDIJobTHTH");
                dtable.Columns.Add("EDIJobJAJP");
                dtable.Columns.Add("EDIJobKOKR");
                dtable.Columns.Add("EDIJobType");
                dtable.Columns.Add("EDIConID");
                dtable.Columns.Add("ObjectName");
                dtable.Columns.Add("DepEDIJobID");
                dtable.Columns.Add("IsUseRes");
                dtable.Columns.Add("IgnoreWarning");
                dtable.Columns.Add("IsDisable");
                dtable.Columns.Add("FileSource");
                dtable.Columns.Add("FileEncoding");
                dtable.Columns.Add("URLPath");
                dtable.Columns.Add("SortOrder");
                dtable.Columns.Add("UpdUserID");
            }
            return dtable;
        }

        private DataTable GetEDIJobDetailDataTable<T>(List<T> list, string tableName)
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
                dtable.Columns.Add("EDIJobID");
                dtable.Columns.Add("EDIJobZHTW");
                dtable.Columns.Add("EDIJobZHCN");
                dtable.Columns.Add("EDIJobENUS");
                dtable.Columns.Add("EDIJobTHTH");
                dtable.Columns.Add("EDIJobJAJP");
                dtable.Columns.Add("EDIJobKOKR");
                dtable.Columns.Add("EDIJobType");
                dtable.Columns.Add("EDIConID");
                dtable.Columns.Add("ObjectName");
                dtable.Columns.Add("DepEDIJobID");
                dtable.Columns.Add("IsUseRes");
                dtable.Columns.Add("IgnoreWarning");
                dtable.Columns.Add("IsDisable");
                dtable.Columns.Add("FileSource");
                dtable.Columns.Add("FileEncoding");
                dtable.Columns.Add("URLPath");
                dtable.Columns.Add("SortOrder");
                dtable.Columns.Add("UpdUserID");
            }

            return dtable;
        }

        private DataTable GetEDIJobParaDataTable<T>(List<T> list, string tableName)
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
                dtable.Columns.Add("EDIJobID");
                dtable.Columns.Add("EDIJobParaID");
                dtable.Columns.Add("EDIJobParaType");
                dtable.Columns.Add("EDIJobParaValue");
            }

            return dtable;
        }

        private DataTable GetEDIJobParaDataTable<T>(T obj, string tableName)
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
                dtable.Columns.Add("SortOrder");
                dtable.Columns.Add("UpdUserID");
                dtable.Columns.Add("EDIFlowID");
                dtable.Columns.Add("EDIJobID");
                dtable.Columns.Add("EDIJobParaID");
                dtable.Columns.Add("EDIJobParaType");
                dtable.Columns.Add("EDIJobParaValue");
            }

            return dtable;
        }
    }
}
