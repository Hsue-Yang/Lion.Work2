using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public partial class SysAPIRepository
    {
        public async Task<IEnumerable<SystemAPIAuthorize>> GetSystemAPIAuthorizeList(string sysID, string apiGroupID, string apiFunID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemAPIAuthorize>("EXEC dbo.sp_GetSystemAPIAuthorizes @sysID, @apiGroupID, @apiFunID, @cultureID;", new { sysID, apiGroupID, apiFunID, cultureID });
            }
        }

        public async Task EditSystemAPIAuthorize(SystemAPIAuthorize systemAPIAuthorize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_EditSystemAPIAuthorize @SysID, @APIGroupID, @APIFunID, @ClientSysID, @UpdUserID;", systemAPIAuthorize);
            }
        }

        public async Task DeleteSystemAPIAuthorize(string sysID, string apiGroupID, string apiFunID, string clientSysID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_DeleteSystemAPIAuthorize @sysID, @apiGroupID, @apiFunID, @clientSysID;", new { sysID, apiGroupID, apiFunID, clientSysID });
            }
        }
    }
}
