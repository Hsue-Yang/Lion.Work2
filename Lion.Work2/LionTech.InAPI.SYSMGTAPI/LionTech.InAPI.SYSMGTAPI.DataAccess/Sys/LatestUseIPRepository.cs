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
    public class LatestUseIPRepository : ILatestUseIPRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public LatestUseIPRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<(int rowCount, IEnumerable<LatestUseIPInfo>)> SelectLatestUseIPInfoList(string ipAddress, string codeNM, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                var multiResult = await conn.QueryMultipleAsync("EXEC dbo.sp_GetLatestUseIP  @ipAddress, @codeNM, @pageIndex, @pageSize;", new { ipAddress, codeNM, pageIndex, pageSize });

                var rowCount = multiResult.Read<int>().SingleOrDefault();
                var latestUseIPInfolist = multiResult.Read<LatestUseIPInfo>();

                return (rowCount, latestUseIPInfolist);
            }
        }
    }
}
