using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class UserConnectRepository : IUserConnectRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public UserConnectRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<(int rowCount, IEnumerable<UserConnect>)> GetUserConnectList(string connectDTBegin, string connectDTEnd, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                var multiResult = await conn.QueryMultipleAsync("EXEC dbo.sp_GetUserConnect  @connectDTBegin, @connectDTEnd, @pageIndex, @pageSize;", new { connectDTBegin, connectDTEnd, pageIndex, pageSize });

                var rowCount = multiResult.Read<int>().SingleOrDefault();
                var userConnectList = multiResult.Read<UserConnect>();

                return (rowCount, userConnectList);
            }
        }
    }
}
