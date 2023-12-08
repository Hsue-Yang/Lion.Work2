using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysFunGroupRepository : ISysFunGroupRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysFunGroupRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<SystemFunGroup>> GetUserSystemFunGroupList(string userID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemFunGroup>("EXEC dbo.sp_GetUserSystemFunGroups @userID, @cultureID;", new { userID, cultureID });
            }
        }

        public async Task<IEnumerable<SystemFunGroup>> GetSystemFunGroupByIdList(string sysID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemFunGroup>("EXEC dbo.sp_GetSystemFunGroupByIds @sysID, @cultureID; ", new { sysID, cultureID });
            }
        }

        public async Task<(int rowCount, IEnumerable<SystemFunGroup> SystemFunGroupList)> GetSystemFunGroupList(string sysID, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemFunGroups @sysID, @cultureID, @pageIndex, @pageSize;", new { sysID, cultureID, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemFunGroupList = multi.Read<SystemFunGroup>();

                return (rowCount, systemFunGroupList);
            }
        }

        public async Task<SystemFunGroupMain> GetSystemFunGroupDetail(string sysID, string funControllerID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemFunGroupMain>("EXEC dbo.sp_GetSystemFunGroup @sysID, @funControllerID;", new { sysID, funControllerID });
            }
        }

        public async Task EditSystemFunGroupDetail(SystemFunGroupMain systemFunGroup)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemFunGroup @SysID, @FunControllerID,
                                        @FunGroupZHTW, @FunGroupZHCN, @FunGroupENUS, @FunGroupTHTH, @FunGroupJAJP, @FunGroupKOKR,
                                        @SortOrder, @UpdUserID, @ExecSysID, @ExecIpAddress;";

                await conn.ExecuteAsync(commandText, systemFunGroup);
            }
        }

        public async Task<EnumDeleteSystemFunGroupDetailResult> DeleteSystemFunGroupDetail(string sysID, string funControllerID, string userID, string execSysID, string execIpAddress)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                var result = await conn.ExecuteScalarAsync<string>("EXEC dbo.sp_DeleteSystemFunGroup @sysID, @funControllerID, @userID, @execSysID, @execIpAddress;", new { sysID, funControllerID, userID, execSysID, execIpAddress });

                if (result == null) return EnumDeleteSystemFunGroupDetailResult.DataExist;

                return result == EnumYN.Y.ToString()
                    ? EnumDeleteSystemFunGroupDetailResult.Success
                    : EnumDeleteSystemFunGroupDetailResult.Failure;
            }
        }
    }
}