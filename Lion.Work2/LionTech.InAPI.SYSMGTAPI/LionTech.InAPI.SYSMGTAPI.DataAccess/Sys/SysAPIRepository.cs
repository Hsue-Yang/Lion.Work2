using System.Collections.Generic;
using System.Data;
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
    public partial class SysAPIRepository : ISysAPIRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IMongoConnectionProvider _mongoConnectionProvider;

        public SysAPIRepository(IConnectionStringProvider connectionStringProvider, IMongoConnectionProvider mongoConnectionProvider)
        {
            _connectionStringProvider = connectionStringProvider;
            _mongoConnectionProvider = mongoConnectionProvider;
        }

        public async Task<SystemAPI> GetSystemAPIFullName(string sysID, string apiGroupID, string apiFunID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemAPI>("EXEC dbo.sp_GetSystemAPIFullName @sysID, @apiGroupID, @apiFunID, @cultureID;", new { sysID, apiGroupID, apiFunID, cultureID });
            }
        }

        public async Task<IEnumerable<SystemAPI>> GetSystemAPIByIdList(string sysID, string apiGroup, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemAPI>("EXEC dbo.sp_GetSystemAPIByIds @sysID, @apiGroup, @cultureID; ", new { sysID, apiGroup, cultureID });
            }
        }

        public async Task<(int rowCount, IEnumerable<SystemAPI> SystemAPIList)> GetSystemAPIList(string sysID, string apiGroupID, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemAPIs @sysID, @apiGroupID, @cultureID, @pageIndex, @pageSize;", new { sysID, apiGroupID, cultureID, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemAPIList = multi.Read<SystemAPI>();

                return (rowCount, systemAPIList);
            }
        }

        public async Task<SystemAPIMain> GetSystemAPIDetail(string sysID, string apiGroupID, string apiFunID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemAPIMain>("EXEC dbo.sp_GetSystemAPI @sysID, @apiGroupID, @apiFunID;", new { sysID, apiGroupID, apiFunID });
            }
        }

        public async Task<IEnumerable<SystemAPIRole>> GetSystemAPIRoleList(string sysID, string apiGroupID, string apiFunID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemAPIRole>("EXEC dbo.sp_GetSystemAPIRoles @sysID, @apiGroupID, @apiFunID, @cultureID;", new { sysID, apiGroupID, apiFunID, cultureID });
            }
        }

        public async Task EditSystemAPIDetail(SystemAPIMain systemAPI, DataTable systemRoleAPIs)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText =
                    @"EXEC dbo.sp_EditSystemAPI 
                      @SysID, @APIGroupID, @APIFunID 			 
                    , @APINMZHTW, @APINMZHCN, @APINMENUS, @APINMTHTH, @APINMJAJP, @APINMKOKR 			 
                    , @APIPara, @APIReturn, @APIParaDesc, @APIReturnContent	 
                    , @IsOutside, @IsDisable, @SortOrder, @UpdUserID 		
                    , @systemRoleAPIs";

                await conn.ExecuteAsync(commandText, new
                {
                    systemAPI.SysID,
                    systemAPI.APIGroupID,
                    systemAPI.APIFunID,
                    systemAPI.APINMZHTW,
                    systemAPI.APINMZHCN,
                    systemAPI.APINMENUS,
                    systemAPI.APINMTHTH,
                    systemAPI.APINMJAJP,
                    systemAPI.APINMKOKR,
                    systemAPI.APIPara,
                    systemAPI.APIReturn,
                    systemAPI.APIParaDesc,
                    systemAPI.APIReturnContent,
                    systemAPI.IsOutside,
                    systemAPI.IsDisable,
                    systemAPI.SortOrder,
                    systemAPI.UpdUserID,
                    systemRoleAPIs = new TableValuedParameter(systemRoleAPIs)
                });
            }
        }

        public async Task<EnumDeleteSystemAPIDetailResult> DeleteSystemAPIDetail(string sysID, string apiGroupID, string apiFunID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                var result = await conn.ExecuteScalarAsync<string>("EXEC dbo.sp_DeleteSystemAPI @sysID, @apiGroupID, @apiFunID;", new { sysID, apiGroupID, apiFunID });

                if (result == null)
                {
                    return EnumDeleteSystemAPIDetailResult.DataExist;
                }

                return result == EnumYN.Y.ToString()
                    ? EnumDeleteSystemAPIDetailResult.Success
                    : EnumDeleteSystemAPIDetailResult.Failure;
            }
        }

        public async Task<IEnumerable<SystemAPIFuntion>> GetSystemAPIFuntionList(string sysID, string apiControllerID, string cultureID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QueryAsync<SystemAPIFuntion>("EXEC dbo.sp_GetSystemAPIFuntions @sysID, @apiControllerID, @cultureID;", new { sysID, apiControllerID, cultureID });
            }
        }
    }
}
