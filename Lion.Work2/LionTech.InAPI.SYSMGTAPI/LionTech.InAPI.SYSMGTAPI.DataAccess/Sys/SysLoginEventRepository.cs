using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class SysLoginEventRepository : ISysLoginEventRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysLoginEventRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<SysLoginEventID>> GetSysLoginEventListById(string sysID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SysLoginEventID>("EXEC dbo.sp_GetSysLoginEventByIds @sysID, @cultureID;", new { sysID, cultureID });
            }
        }

        public async Task<(int rowCount, IEnumerable<SysLoginEventSetting>)> GetSysLoginEventSettingList(string sysID, string cultureID, string logineventID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSysLoginEventSettings @sysID, @cultureID, @logineventID, @pageIndex, @pageSize;", new { sysID, cultureID, logineventID, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var sysLoginEventSettingList = multi.Read<SysLoginEventSetting>();

                return (rowCount, sysLoginEventSettingList);
            }
        }

        public async Task<LoginEventSettingDetail> GetLoginEventSettingDetail(string sysID, string logineventID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<LoginEventSettingDetail>("EXEC dbo.sp_GetSysLoginEventSettingDetail @sysID, @logineventID;", new { sysID, logineventID });
            }
        }

        public async Task EditSysLoginEventSettingSort(List<LoginEventSettingValue> loginEventSettingValue)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_EditSystemLoginEventSortOrder @SystemLoginEvent"
                        , new { SystemLoginEvent = new TableValuedParameter(GetDataTable(loginEventSettingValue, "type_SystemLoginEvent")) });
            }
        }

        public async Task EditLoginEventSettingDetail(LoginEventSettingDetail loginEventSettingDetail)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText =
                    @"EXEC dbo.sp_EditLoginEventSettingDetail
                           @SysID
                         , @LoginEventID
                         , @LoginEventNMZHCN
                         , @LoginEventNMZHTW
                         , @LoginEventNMENUS
                         , @LoginEventNMTHTH
                         , @LoginEventNMJAJP
                         , @LoginEventNMKOKR
                         , @StartDT
                         , @EndDT
                         , @Frequency
                         , @StartExecTime
                         , @EndExecTime
                         , @TargetPath
                         , @ValidPath
                         , @SubSysID
                         , @IsDisable
                         , @SortOrder
                         , @UpdUserID";
                await conn.ExecuteAsync(commandText, loginEventSettingDetail);
            }
        }

        public async Task DeleteLoginEventSettingDetail(string sysID, string logineventID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteScalarAsync<string>("EXEC dbo.sp_DeleteLoginEventSettingDetail @sysID, @logineventID;", new { sysID, logineventID });
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

            return dtable;
        }

        public async Task<bool> CheckSysLoginEventIdIsExists(string sysID, string logineventID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.ExecuteScalarAsync<bool>(@"DECLARE @RESULT BIT = 0;
                                                                   EXEC dbo.sp_GetLoginEventIDIsExists @sysID, @logineventID, @RESULT OUT;
                                                                   SELECT @RESULT AS IsExists"
                                                                 , new { sysID, logineventID });
            }
        }
    }
}
