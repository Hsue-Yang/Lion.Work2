using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class UserFunctionRepository : IUserFunctionRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public UserFunctionRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<UserFunction>> GetUserFunctions(string userID, string updUserID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<UserFunction>("EXEC dbo.sp_GetUserFunctions @userID, @updUserID, @cultureID ; ", new { userID, updUserID, cultureID });
            }
        }

        public async Task<IEnumerable<UserFunction>> GetAllSystemUserFunctions(string userID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<UserFunction>("EXEC dbo.sp_GetAllUserFunctions @userID, @cultureID ; ", new { userID, cultureID });
            }
        }

        public async Task EditUserFunction(string userID, string isDisable, string updUserID, List<UserFunctionValue> userFunctionValueList)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_EditUserFunction @userID, @isDisable, @updUserID, @userFunctionValueList ; ", new
                {
                    userID,
                    isDisable,
                    updUserID,
                    userFunctionValueList = new TableValuedParameter(TableValuedParameter.GetDataTable(userFunctionValueList, "type_UserFunction"))
                });
            }
        }
    }
}