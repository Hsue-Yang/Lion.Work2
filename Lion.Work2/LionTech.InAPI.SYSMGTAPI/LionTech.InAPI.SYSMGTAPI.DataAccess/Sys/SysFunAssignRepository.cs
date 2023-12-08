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
    public class SysFunAssignRepository : ISysFunAssignRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SysFunAssignRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<SystemFunAssign>> GetSystemFunAssigns(string sysID, string funControllerID, string funActionName)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemFunAssign>("EXEC dbo.sp_GetSystemFunAssigns @sysID, @funControllerID, @funActionName", new { sysID, funControllerID, funActionName });
            }
        }
        
        public async Task EditSystemFunAssign(string sysID, string funControllerID, string funActionName, string updUserID, List<SystemFunAssignUser> userIDList)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemFunAssign @sysID, @funControllerID, @funActionName, @updUserID, @userIDList";
                await conn.ExecuteAsync(commandText, new
                {
                    sysID,
                    funControllerID,
                    funActionName,
                    updUserID,
                    userIDList = new TableValuedParameter(TableValuedParameter.GetDataTable(userIDList, "USER_TYPE"))
                });
            }
        }

        public async Task<IEnumerable<SysFunRawData>> GetFunRawDatas(List<UserFunctionValue> userFunction, string userID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SysFunRawData>("EXEC dbo.sp_GetFunRawDatas @userFunctionValueList, @userID, @cultureID;", new
                {
                    userFunctionValueList = new TableValuedParameter(TableValuedParameter.GetDataTable(userFunction, "type_UserFunction")),
                    userID,
                    cultureID
                });
            }
        }

        public async Task<IEnumerable<UserMain>> GetUserRawDatas(List<UserMain> userIDList)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var list = userIDList.Select(x => new { UserID = x.UserID }).ToList();
                return await conn.QueryAsync<UserMain>("EXEC dbo.sp_GetUserRawDatas @userIDList;", new
                {
                    userIDList = new TableValuedParameter(TableValuedParameter.GetDataTable(list, "USER_TYPE"))
                });
            }
        }
    }
}