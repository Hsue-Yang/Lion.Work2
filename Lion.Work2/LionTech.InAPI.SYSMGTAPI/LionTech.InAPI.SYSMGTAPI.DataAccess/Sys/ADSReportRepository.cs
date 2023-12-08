using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class ADSReportRepository : IADSReportRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public ADSReportRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<ADSReport.SysRoleToFunction>> GetSysRoleToFunction(string sysID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QueryAsync<ADSReport.SysRoleToFunction>("EXEC dbo.sp_GetSysRoleToFunNames @SysID;", new {sysID});
            }
        }

        public async Task<IEnumerable<ADSReport.SysUserToFunction>> GetSysUserToFunction(string sysID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QueryAsync<ADSReport.SysUserToFunction>("EXEC dbo.sp_GetSysUserToFunction @SysID;", new {sysID});
            }
        }

        public async Task<IEnumerable<ADSReport.SysSingleFunctionAwarded>> GetSysSingleFunctionAwarded(string sysID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QueryAsync<ADSReport.SysSingleFunctionAwarded>("EXEC dbo.sp_GetSysSingleFunctionAwarded @SysID;", new {sysID});
            }
        }

        public async Task<IEnumerable<ADSReport.SysUserLoginLastTime>> GetSysUserLoginLastTime()
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QueryAsync<ADSReport.SysUserLoginLastTime>("EXEC dbo.sp_GetSysUserLoginLastTime;");
            }
        }

        public async Task<IEnumerable<ADSReport.SysReportToPermissions>> GetSysReportToPermissions(string sysID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                var resourceList = GetColumnInfos();

                var permissionsList = await conn.QueryAsync<ADSReport.SysReportToPermissions>("EXEC dbo.sp_GetSysReportToPermissions @SysID;", new {sysID});
                foreach (var row in permissionsList)
                {
                    if (row.RoleConditionSyntax != null)
                    {
                        foreach (var subStr in resourceList)
                        {
                            for (var i = 0; i < Regex.Matches(row.RoleConditionSyntax, subStr.Key.ColumnSyntax).Count; i++)
                            {
                                int startIndex = row.RoleConditionSyntax.IndexOf(subStr.Key.ColumnSyntax, StringComparison.Ordinal);
                                int endIndex = startIndex + subStr.Key.ColumnSyntax.Length + 1;
                                var oper = row.RoleConditionSyntax.Substring(endIndex).Split(' ')[0];
                                
                                if (oper == "=" || oper == "<>" || oper == "<" || oper == ">" || oper == "<=" || oper == ">=")
                                {
                                    string value = row.RoleConditionSyntax.Substring(endIndex).Split(' ')[1];
                                    value = value.Substring(0, value.Length - 1).Replace("N'", "").Replace("'", "");

                                    var nm = subStr.Value[value.Trim()];
                                    row.RoleConditionSyntax = row.RoleConditionSyntax.Replace($"{subStr.Key.ColumnSyntax} {oper} N'{value}'", $"{subStr.Key.ColumnName} {oper} {nm}");
                                }
                                else
                                {
                                    row.RoleConditionSyntax = row.RoleConditionSyntax.Replace(subStr.Key.ColumnSyntax, subStr.Key.ColumnName);
                                }
                            }
                        }
                    }
                }

                return permissionsList;
            }
        }

        private struct ColunmInfo
        {
            public string ColumnSyntax { get; set; }
            public string ColumnName { get; set; }
        }

        private Dictionary<ColunmInfo, Dictionary<string, string>> GetColumnInfos()
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string KeySelector(dynamic row) => (string) row.ID;
                string ValueSelector(dynamic row) => (string) row.NM;
                ColunmInfo GetColunmInfo(string syntax, string name) => new ColunmInfo {ColumnSyntax = syntax, ColumnName = name};

                Dictionary<string, string> GetCmCode(string kind) =>
                    conn.Query($"SELECT CODE_ID AS ID, CODE_NM_ZH_TW AS NM FROM [dbo].[CM_CODE] WHERE CODE_KIND = '{kind}'", null)
                        .ToDictionary(KeySelector, ValueSelector);

                var comp = conn.Query("SELECT COM_ID AS ID, COM_NM AS NM FROM RAW_CM_ORG_COM ORDER BY SORT_ORDER", null)
                    .ToDictionary(KeySelector, ValueSelector);
                var compCountry = conn.Query("SELECT COUNTRY_ID AS ID, COUNTRY_NM_ZH_TW AS NM FROM RAW_CM_COUNTRY", null)
                    .ToDictionary(KeySelector, ValueSelector);
                var unit = conn.Query("SELECT UNIT_ID AS ID, UNIT_NM AS NM FROM RAW_CM_ORG_UNIT", null)
                    .ToDictionary(KeySelector, ValueSelector);
                var bu = conn.Query("SELECT BU_ID AS ID, BU_NM_ZH_TW AS NM FROM RAW_CM_BUSINESS_UNIT ORDER BY SORT_ORDER", null)
                    .ToDictionary(KeySelector, ValueSelector);

                var result = new Dictionary<ColunmInfo, Dictionary<string, string>>();
                result.Add(GetColunmInfo("U.USER_COM_ID", "ERP-公司"), comp);
                result.Add(GetColunmInfo("O.USER_COM_ID", "組織-上班公司"), comp);
                result.Add(GetColunmInfo("U.USER_SALARY_COM_ID", "薪資-公司"), comp);
                result.Add(GetColunmInfo("S.COM_COUNTRY", "薪資-國家"), compCountry);
                result.Add(GetColunmInfo("U.USER_UNIT_ID", "ERP-單位"), unit);
                result.Add(GetColunmInfo("C.COM_BU", "ERP-集團"), bu);
                result.Add(GetColunmInfo("U.USER_WORK_ID", "ERP-工作"), GetCmCode("0038"));
                result.Add(GetColunmInfo("U.USER_TITLE_ID", "ERP-職稱"), GetCmCode("0039"));
                result.Add(GetColunmInfo("O.USER_AREA", "組織-區域"), GetCmCode("0015"));
                result.Add(GetColunmInfo("O.USER_GROUP", "組織-事業群"), GetCmCode("0016"));
                result.Add(GetColunmInfo("O.USER_PLACE", "組織-事業處"), GetCmCode("0017"));
                result.Add(GetColunmInfo("O.USER_DEPT", "組織-部門"), GetCmCode("0018"));
                result.Add(GetColunmInfo("O.USER_TEAM", "組織-組別"), GetCmCode("0019"));
                result.Add(GetColunmInfo("O.USER_JOB_TITLE", "組織-職稱"), GetCmCode("0020"));
                result.Add(GetColunmInfo("O.USER_BIZ_TITLE", "組織-業務職稱"), GetCmCode("0021"));
                result.Add(GetColunmInfo("O.USER_LEVEL", "組織-職等"), GetCmCode("0022"));
                result.Add(GetColunmInfo("O.USER_TITLE", "組織-職級"), GetCmCode("0023"));

                return result;
            }
        }
    }
}
