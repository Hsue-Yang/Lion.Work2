using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class SysFunToolSettingRepository : ISysFunToolSettingRepository
    {

        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysFunToolSettingRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<SystemFunToolSetting>> GetSystemFunToolSettings(string userID, string sysID, string funControllerID, string funActionName, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemFunToolSetting>(
                    @"EXEC dbo.sp_GetSystemFunToolSettings 
                        @userID,
                        @sysID,
                        @funControllerID,
                        @funActionName,
                        @cultureID;",
                    new
                    {
                        userID,
                        sysID,
                        funControllerID,
                        funActionName,
                        cultureID
                    });
            }
        }

        public async Task<IEnumerable<SystemFunControllerID>> GetSystemFunToolControllerIDs(string sysID, string condition, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemFunControllerID>(
                    @"EXEC dbo.sp_GetSystemFunToolControllerIDs
                        @sysID,
                        @condition,
                        @cultureID;",
                    new
                    {
                        sysID,
                        condition,
                        cultureID
                    });
            }
        }

        public async Task<IEnumerable<SystemFunToolFunName>> GetSystemFunToolFunNames(string sysID, string funControllerID, string condition, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemFunToolFunName>(
                    @"EXEC dbo.sp_GetSystemFunToolFunNames
                        @sysID,
                        @funControllerID,
                        @condition,
                        @cultureID;",
                    new
                    {
                        sysID,
                        funControllerID,
                        condition,
                        cultureID
                    });
            }
        }

        public async Task EditSystemFunToolSetting(SystemUserToolPara systemUserToolPara)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.QueryAsync(
                    @"EXEC dbo.sp_EditSystemFunToolSetting 
                          @updUserID
                        , @type_SystemUserTool;",
                    new
                    {
                        updUserID = systemUserToolPara.UpdUserID,
                        type_SystemUserTool = new TableValuedParameter(TableValuedParameter.GetDataTable(systemUserToolPara.SystemUserToolList, "type_SystemUserTool"))
                    }
                );
            }
        }

        public async Task DeleteSystemFunToolSetting(string userID, string sysID, string funControllerID, string funActionName, SystemUserToolPara systemUserToolPara)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.QueryAsync(
                    @"EXEC dbo.sp_DeleteSystemFunToolSetting 
                          @userID
                        , @sysID
                        , @funControllerID
                        , @funActionName
                        , @updUserID
                        , @type_SystemUserTool;",
                    new
                    {
                        userID = userID,
                        sysID = sysID,
                        funControllerID = funControllerID,
                        funActionName = funActionName,
                        updUserID = systemUserToolPara.UpdUserID,
                        type_SystemUserTool = new TableValuedParameter(TableValuedParameter.GetDataTable(systemUserToolPara.SystemUserToolList, "type_SystemUserTool"))
                    }
                );
            }
        }

        public async Task CopySystemUserFunTool(string toolNO, SystemUserToolPara systemUserToolPara)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.QueryAsync(
                    @"EXEC dbo.sp_CopySystemUserFunTool
                          @toolNO
                        , @copyToUserID
                        , @updUserID
                        , @type_SystemUserTool;",
                    new
                    {
                        toolNO = toolNO,
                        copyToUserID = systemUserToolPara.CopyToUserID,
                        updUserID = systemUserToolPara.UpdUserID,
                        type_SystemUserTool = new TableValuedParameter(TableValuedParameter.GetDataTable(systemUserToolPara.SystemUserToolList.SingleOrDefault(), "type_SystemUserTool"))
                    }
                );
            }
        }
    }
}