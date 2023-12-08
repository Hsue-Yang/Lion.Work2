using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Raw.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Raw;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Raw
{
    public class RawRepository : IRawRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public RawRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task<IEnumerable<RawCMOrgCom>> GetRawCMOrgComs()
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<RawCMOrgCom>("EXEC dbo.sp_GetRawCMOrgComs;");
            }
        }

        public async Task<IEnumerable<RawCMOrgUnit>> GetRawCMOrgUnits()
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<RawCMOrgUnit>(@"EXEC dbo.sp_GetRawCMOrgUnits;");
            }
        }

        public async Task<IEnumerable<RawCMBusinessUnit>> GetRawCMBusinessUnits(string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<RawCMBusinessUnit>(@"EXEC dbo.sp_GetRawCMBusinessUnits @cultureID;", new { cultureID });
            }
        }

        public async Task<IEnumerable<RawCMCountry>> GetRawCMCountries(string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<RawCMCountry>(@"EXEC dbo.sp_GetRawCMCountries @cultureID;", new { cultureID });
            }
        }

        public async Task<IEnumerable<RawUser>> GetRawUsers(string condition, int limit)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<RawUser>("EXEC dbo.sp_GetRawUsers @condition, @limit", new { condition, limit });
            }
        }
    }
}