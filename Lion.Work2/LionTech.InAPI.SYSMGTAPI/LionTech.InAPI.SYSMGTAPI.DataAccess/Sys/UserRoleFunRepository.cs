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
    public class UserRoleFunRepository : IUserRoleFunRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public UserRoleFunRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<(int rowCount, IEnumerable<UserRoleFun> userRoleFunList)> GetUserRoleFuns(string userID, string userNM, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemUserRoleFuns @userID, @userNM, @pageIndex, @pageSize;", new { userID, userNM, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var userRoleFunList = multi.Read<UserRoleFun>();

                return (rowCount, userRoleFunList);
            }
        }

        public async Task<UserMain> GetUserMainInfo(string userID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<UserMain>(
                    @"EXEC dbo.sp_GetUserMainInfo 
                        @userID",
                    new
                    {
                        userID
                    }
                );
            }
        }

        public async Task<IEnumerable<SystemRoleGroupCollect>> GetSystemRoleGroupCollects(string roleGroupID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemRoleGroupCollect>(
                    @"EXEC dbo.sp_GetSystemRoleGroupCollects
                        @roleGroupID",
                    new
                    {
                        roleGroupID
                    }
                );
            }
        }

        public async Task<IEnumerable<UserSystemRoleData>> GetUserSystemRoles(string userID, string updUserID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<UserSystemRoleData>(
                    @"EXEC dbo.sp_GetUserSystemRoles
                        @userID,
                        @updUserID, 
                        @cultureID",
                    new
                    {
                        userID,
                        updUserID,
                        cultureID
                    }
                );
            }
        }

        public async Task EditUserSystemRole(UserRoleFunDetailPara userRoleFunDetailPara, List<SystemRoleMain> systemRoleList)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.QueryAsync<UserRoleFunDetailPara>(
                @"EXEC dbo.sp_EditUserSystemRole 
                    @userID, 
                    @roleGroupID,
                    @isDisable,
                    @erpWFNO,
                    @memo,
                    @sysID,
                    @apiNO,
                    @execSysID,
                    @ipAddress,
                    @updUserID,
                    @UserSystemRolePara",
                new
                {
                    userID = userRoleFunDetailPara.UserID,
                    roleGroupID = userRoleFunDetailPara.RoleGroupID,
                    isDisable = userRoleFunDetailPara.IsDisable,
                    erpWFNO = userRoleFunDetailPara.ErpWFNO,
                    memo = userRoleFunDetailPara.Memo,
                    sysID = userRoleFunDetailPara.SysID,
                    apiNO = userRoleFunDetailPara.ApiNO,
                    execSysID = userRoleFunDetailPara.ExecSysID,
                    ipAddress = userRoleFunDetailPara.IpAddress,
                    updUserID = userRoleFunDetailPara.UpdUserID,
                    UserSystemRolePara = new TableValuedParameter(TableValuedParameter.GetDataTable(systemRoleList, "type_SystemRole")),
                });
            }
        }

        public async Task<IEnumerable<UserMenuFun>> GetUserMenuFuns(string userID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<UserMenuFun>(@"EXEC dbo.sp_GetUserMenuFuns @userID, @cultureID", new { userID, cultureID });
            }
        }
    }
}