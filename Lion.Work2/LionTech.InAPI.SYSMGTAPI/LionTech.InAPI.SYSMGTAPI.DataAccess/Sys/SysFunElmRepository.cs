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
    public class SysFunElmRepository : ISysFunElmRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysFunElmRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<(int rowCount, IEnumerable<SystemFunElm> systemFunElms)> GetSysFunElmList(string sysID, string isDisable, string elmID, string elmName, string funControllerID, string funActionName, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync(@"EXEC dbo.sp_GetSystemFunElms @sysID, @isDisable, @elmID, @elmName
                                                             ,@funControllerID, @funActionName, @cultureID, @pageIndex, @pageSize;"
                                                   , new { sysID, isDisable, elmID, elmName, funControllerID, funActionName, cultureID, pageIndex, pageSize });
                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemFunElms = multi.Read<SystemFunElm>();

                return (rowCount, systemFunElms);
            }
        }

        public async Task<bool> CheckSystemFunElmIdIsExists(string sysID, string elmID, string funControllerID, string funActionName)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.ExecuteScalarAsync<bool>(@"DECLARE @RESULT BIT = 0
                                                             EXEC dbo.sp_GetSystemFunElmIdIsExists
                                                             @sysID, @elmID, @funControllerID, @funActionName, @RESULT OUT;
                                                             SELECT @RESULT AS IsExists"
                                                           , new { sysID, elmID, funControllerID, funActionName });
            }
        }

        public async Task<SystemFunElm> GetSystemFunElmDetail(string sysID, string elmID, string funControllerID, string funActionName)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemFunElm>("EXEC dbo.sp_GetSystemFunElmDetail @sysID, @elmID, @funControllerID, @funActionName;"
                                                                       , new { sysID, elmID, funControllerID, funActionName });
            }
        }

        public async Task<IEnumerable<SystemRoleFunElm>> GetSystemFunElmRoleList(string sysID, string elmID, string funControllerID, string funActionName, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemRoleFunElm>("EXEC dbo.sp_GetSystemFunElmRoles @cultureID, @elmID, @sysID, @funControllerID, @funActionName;"
                                                                       , new { cultureID, elmID, sysID, funControllerID, funActionName });
            }
        }

        public async Task<SystemFunElm> GetSystemFunElmInfo(string sysID, string elmID, string funControllerID, string funActionName, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemFunElm>("EXEC dbo.sp_GetSystemFunElmInfo @sysID, @elmID, @funControllerID, @funActionName, @cultureID;"
                                                                       , new { sysID, elmID, funControllerID, funActionName, cultureID });
            }
        }

        public async Task EditSystemFunElmDetail(SystemFunElm systemFunElm)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText =
                    @"EXEC dbo.sp_EditSystemFunElmDetail
                      @ElmID,@SysID, @FunControllerID, @FunActionNM, @IsDisable
                    , @DefaultDisplaySts, @ElmNMZHTW, @ElmNMZHCN, @ElmNMENUS
                    , @ElmNMTHTH, @ElmNMJAJP, @ElmNMKOKR, @UpdUserID;";
                await conn.ExecuteAsync(commandText, systemFunElm);
            }
        }

        public async Task EditSystemFunElmRole(string sysID, string elmID, string funControllerID, string funActionName, List<ElmRoleInfoValue> elmRoleInfoValue)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_EditSystemFunElmRole @elmID, @sysID, @funControllerID, @funActionName, @SystemRoleFunElm"
                        , new { elmID, sysID, funControllerID, funActionName, SystemRoleFunElm = new TableValuedParameter(GetDataTable(elmRoleInfoValue, "type_SystemRoleFunElm")) });
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