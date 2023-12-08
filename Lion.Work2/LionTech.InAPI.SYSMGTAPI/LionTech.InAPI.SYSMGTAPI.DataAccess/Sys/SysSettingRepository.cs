using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysSettingRepository : ISysSettingRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysSettingRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<bool> CheckIsITManager(string sysID, string userID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                string commandText = @"DECLARE @IS_ERP_IT BIT = 0; 
                                       EXEC sp_IsITMangaer @sysID, @clientUserID, @IS_ERP_IT OUT; 

                                       SELECT @IS_ERP_IT AS IsERPIT";

                return await conn.ExecuteScalarAsync<bool>(commandText, new { sysID, clientUserID = userID });
            }
        }

        public async Task<IEnumerable<CMCode>> GetCodeManagementList(string codeKind, string cultureID, List<CMCode> codeIDParas, string codeParent)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<CMCode>("EXEC dbo.sp_GetCMCode @codeKind, @cultureID, @codeID, @codeParent;", new
                {
                    codeKind,
                    cultureID,
                    codeID = new TableValuedParameter(TableValuedParameter.GetDataTable(codeIDParas, "type_CMCode", new List<string> { "CodeNM", "CodeParent", "IsDisable", "SortOrder" })),
                    codeParent
                });
            }
        }

        public async Task<string> GetSystemFilePath(string sysID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<string>("EXEC dbo.sp_GetSystemFilePath @sysID;", new { sysID });
            }
        }

        public async Task<IEnumerable<SystemSetting>> GetAllSystemByIdList(string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemSetting>("EXEC dbo.sp_GetSystemMainByIds @cultureID;", new { cultureID });
            }
        }

        public async Task<IEnumerable<SystemSetting>> GetUserSystemByIdList(string userID, bool isExcludeOutsourcing, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemSetting>("EXEC dbo.sp_GetUserSystemByIds @userID, @IsExcludeOutsourcing, @cultureID;", new { userID, isExcludeOutsourcing, cultureID });
            }
        }

        public async Task<List<SystemSetting>> GetSystemSettingList(string userID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetUserSystems @clientUserID, @cultureID;", new { clientUserID = userID, cultureID });

                var systemSettingList = multi.Read<SystemSetting>().ToList();
                var subsystemList = multi.Read<SystemSetting.SystemSub>().ToList();

                systemSettingList.ForEach(row => { row.SubsystemList = subsystemList.FindAll(f => f.ParentSysID == row.SysID); });
                return systemSettingList;
            }
        }

        public async Task<SystemMain> GetSystemMain(string sysID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = "EXEC dbo.sp_GetSystemMain @sysID;";
                return await conn.QuerySingleOrDefaultAsync<SystemMain>(commandText, new { sysID });
            }
        }

        public async Task<bool> EditSystemSettingDetail(SystemMain systemMain, string userID, string clientIPAddress)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText =
                    @"EXEC dbo.sp_EditSystem
                      @clientIPAddress, @SysID
                    , @SysMANUserID, @SysNMZHTW, @SysNMZHCN, @SysNMENUS, @SysNMTHTH, @SysNMJAJP, @SysNMKOKR
                    , @SysIndexPath, @SysIconPath, @SysKey, @ENSysID
                    , @IsOutsourcing, @IsDisable, @SortOrder, @UpdUserID;";

                return await conn.ExecuteScalarAsync<bool>(commandText,
                    new
                    {
                        clientIPAddress,
                        systemMain.SysID,
                        systemMain.SysMANUserID,
                        systemMain.SysNMZHTW,
                        systemMain.SysNMZHCN,
                        systemMain.SysNMENUS,
                        systemMain.SysNMTHTH,
                        systemMain.SysNMJAJP,
                        systemMain.SysNMKOKR,
                        systemMain.SysIndexPath,
                        systemMain.SysIconPath,
                        systemMain.SysKey,
                        systemMain.ENSysID,
                        systemMain.IsOutsourcing,
                        systemMain.IsDisable,
                        systemMain.SortOrder,
                        UpdUserID = userID
                    });
            }
        }

        public async Task<EnumDeleteSystemSettingResult> DeleteSystemSettingDetail(string sysID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = "EXEC dbo.sp_DeleteSystem @sysID;";
                var result = await conn.ExecuteScalarAsync<string>(commandText, new { sysID });

                if (result == null)
                {
                    return EnumDeleteSystemSettingResult.DataExist;
                }

                return result == EnumYN.Y.ToString()
                    ? EnumDeleteSystemSettingResult.Success
                    : EnumDeleteSystemSettingResult.Failure;
            }
        }

        public async Task<IEnumerable<SystemSysID>> GetUserSystemSysIDs(string userID, bool excludeOutsourcing, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemSysID>("EXEC dbo.sp_GetUserSystemSysIDs @userID, @excludeOutsourcing, @cultureID;", new { userID, excludeOutsourcing, cultureID });
            }
        }

        public async Task<IEnumerable<SystemSysID>> GetSystemSysIDs(bool excludeOutsourcing, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemSysID>("EXEC dbo.sp_GetSystemSysIDs @excludeOutsourcing, @cultureID;", new { excludeOutsourcing, cultureID });
            }
        }

        public async Task<IEnumerable<SystemRoleGroup>> GetSystemRoleGroups(string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemRoleGroup>("EXEC dbo.sp_GetSystemRoleGroups @cultureID;", new { cultureID });
            }
        }

        public async Task<IEnumerable<SystemConditionID>> GetSystemConditionIDs(string sysID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemConditionID>("EXEC dbo.sp_GetSystemConditionIDs @sysID, @cultureID;", new { sysID, cultureID });
            }
        }
    }
}