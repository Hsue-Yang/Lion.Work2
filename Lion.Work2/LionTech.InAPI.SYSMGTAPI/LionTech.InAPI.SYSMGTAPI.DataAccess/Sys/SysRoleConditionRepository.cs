using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using Microsoft.Data.SqlClient;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class SysRoleConditionRepository : ISysRoleConditionRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IMongoConnectionProvider _mongoConnectionProvider;

        public SysRoleConditionRepository(IConnectionStringProvider connectionStringProvider, IMongoConnectionProvider mongoConnectionProvider)
        {
            _connectionStringProvider = connectionStringProvider;
            _mongoConnectionProvider = mongoConnectionProvider;
        }

        public async Task<(int rowCount, IEnumerable<SystemRoleCondition>)> GetSystemRoleConditions(string roleConditionID, string roleID, string sysID, string cultureID, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                var multi = await conn.QueryMultipleAsync("EXEC dbo.sp_GetSystemRoleConditions @roleConditionID, @roleID, @sysID, @cultureID, @pageIndex, @pageSize;", new { roleConditionID, roleID, sysID, cultureID, pageIndex, pageSize });

                var rowCount = multi.Read<int>().SingleOrDefault();
                var systemRoleConditions = multi.Read<SystemRoleCondition>();

                return (rowCount, systemRoleConditions);
            }
        }

        public async Task<SystemRoleConditionDetail> GetSystemRoleConditionDetail(string sysID, string roleConditionID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                return await conn.QuerySingleOrDefaultAsync<SystemRoleConditionDetail>("EXEC dbo.sp_GetSystemRoleConditionDetail @sysID, @roleConditionID;", new { sysID, roleConditionID });
            }
        }

        public async Task<SystemRoleCondotionMongo> GetSystemRoleConditionDetailMongoDB(string sysID, string roleConditionID)
        {
            IMongoCollection<SystemRoleCondotionMongo> col = _mongoConnectionProvider.LionGroupSERP.GetCollection<SystemRoleCondotionMongo>(EnumMongoDocName.SYS_SYSTEM_ROLE_CONDTION.ToString());
            var builder = Builders<SystemRoleCondotionMongo>.Filter;
            var dbFilter = builder.Eq(x => x.SysID, sysID)
                           & builder.Eq(x => x.RoleConditionID, roleConditionID);
            var returnData = (await col.FindAsync(dbFilter)).ToList().SingleOrDefault();
            return returnData;
        }

        public async Task EditSystemRoleConditionDetail(SystemRoleConditionDetailPara systemRoleConditionDetailPara)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                string commandText = @"EXEC dbo.sp_EditSystemRoleConditionDetail @SystemRoleCondition, @SystemRoleConditionCollect";
                await conn.ExecuteAsync(commandText, new
                {
                    SystemRoleCondition = new TableValuedParameter(TableValuedParameter.GetDataTable(systemRoleConditionDetailPara.SysSystemRoleCondition, "type_SystemRoleCondition")),
                    SystemRoleConditionCollect = new TableValuedParameter(TableValuedParameter.GetDataTable(systemRoleConditionDetailPara.SystemRoleConditionCollect, "type_SystemRoleConditionCollect"))
                });
            }
        }

        public async Task DeleteSystemRoleConditionDetail(string sysID, string roleConditionID)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerp))
            {
                await conn.ExecuteAsync("EXEC dbo.sp_DeleteSystemRoleConditionDetail @sysID, @roleConditionID;", new { sysID, roleConditionID });
            }
        }

        public void DeleteSystemRoleCondotionDetailMongoDB(string sysID, string roleConditionID)
        {
            IMongoCollection<SystemRoleCondotionMongo> col = _mongoConnectionProvider.LionGroupSERP.GetCollection<SystemRoleCondotionMongo>(EnumMongoDocName.SYS_SYSTEM_ROLE_CONDTION.ToString());
            var builder = Builders<SystemRoleCondotionMongo>.Filter;
            var dbFilter = builder.Eq(x => x.SysID, sysID)
                          & builder.Eq(x => x.RoleConditionID, roleConditionID);
            col.DeleteOne(dbFilter);
        }

        public void InsertSystemRoleCondotionDetailMongoDB(MongoSystemRoleConditionDetail mongoSystemRoleConditionDetail)
        {
            IMongoCollection<MongoSystemRoleConditionDetail> col = _mongoConnectionProvider.LionGroupSERP.GetCollection<MongoSystemRoleConditionDetail>(EnumMongoDocName.SYS_SYSTEM_ROLE_CONDTION.ToString());
            col.InsertOne(mongoSystemRoleConditionDetail);
        }
    }
}
