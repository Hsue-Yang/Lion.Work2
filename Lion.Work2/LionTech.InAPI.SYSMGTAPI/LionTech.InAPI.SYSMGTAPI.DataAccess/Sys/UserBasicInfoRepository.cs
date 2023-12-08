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
    public class UserBasicInfoRepository : IUserBasicInfoRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public UserBasicInfoRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }
        public async Task<(int rowCount, IEnumerable<UserBasicInfo> userBasicInfoList)> GetUserBasicInfotList(string userID, string userNM, string isDisable, string isLeft, string connectDTBegin, string connectDTEnd, string cultureID, int pageIndex, int pageSize)
        {
            isDisable = isDisable != "Y" ? null : "Y"; 
            isLeft = isLeft != "Y" ? null : "Y"; 
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync(@"EXEC dbo.sp_GetUserBasicInfo  @cultureID, @userID, @userNM, @isDisable, @isLeft,
                                                             @connectDTBegin,@connectDTEnd, @pageIndex, @pageSize;"
                                                   , new { cultureID, userID, userNM, isDisable, isLeft, connectDTBegin, connectDTEnd, pageIndex, pageSize });
                var rowCount = multi.Read<int>().SingleOrDefault();
                var userBasicInfoList = multi.Read<UserBasicInfo>();

                return (rowCount, userBasicInfoList);
            }
        }

        public async Task<UserBasicInfo> GetUserBasicInfoDetail(string userID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var result = await conn.QueryFirstOrDefaultAsync<UserBasicInfo>(@"EXEC dbo.sp_GetUserBasicInfoDetail  @cultureID, @userID;"
                                                   , new { cultureID, userID });

                return result;
            }
        }
    }
}
