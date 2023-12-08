using Dapper;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.Data.SqlClient;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys
{
    public class SysRecordRepository : ISysRecordRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IMongoConnectionProvider _mongoConnectionProvider;
        public SysRecordRepository(IConnectionStringProvider connectionStringProvider, IMongoConnectionProvider mongoConnectionProvider)
        {
            _connectionStringProvider = connectionStringProvider;
            _mongoConnectionProvider = mongoConnectionProvider;
        }

        public async Task<IEnumerable<LogUserSystemRole>> GetLogUserSystemRoles(LogPara logPara)
        {
            using (var conn = new SqlConnection(_connectionStringProvider.ConnStringSerpReadOnly))
            {
                string commandText = @"EXEC dbo.sp_GetLogUserSystemRoles @userID, @logNo, @sUpdDT, @eUpdDT, @sysID";
                return await conn.QueryAsync<LogUserSystemRole>(commandText, logPara);
            }
        }

        public async Task<IEnumerable<SysRecord>> GetSysRecordMongoDB(LogPara logPara)
        {
            var dbFilter = Builders<SysRecord>.Filter.Empty;

            if (logPara.UserID != null)
            {
                dbFilter &= Builders<SysRecord>.Filter.In(x => x.UserID, logPara.UserIDListStr.Split(','));
            }
            if (logPara.WFNo != null)
            {
                dbFilter &= Builders<SysRecord>.Filter.Eq(x => x.WFNo, logPara.WFNo);
            }
            if (logPara.SysID != null)
            {
                dbFilter &= Builders<SysRecord>.Filter.Eq(x => x.SysID, logPara.SysID);
            }
            if (logPara.RoleConditionID != null)
            {
                dbFilter &= Builders<SysRecord>.Filter.Eq(x => x.RoleConditionID, logPara.RoleConditionID);
            }
            if (logPara.LineID != null)
            {
                dbFilter &= Builders<SysRecord>.Filter.Eq(x => x.LineID, logPara.LineID);
            }
            if (logPara.FunControllerID != null)
            {
                dbFilter &= Builders<SysRecord>.Filter.Eq(x => x.FunControllerID, logPara.FunControllerID);
            }
            if (logPara.FunActionName != null)
            {
                dbFilter &= Builders<SysRecord>.Filter.Eq(x => x.FunActionName, logPara.FunActionName);
            }
            if (logPara.SUpdDT != null && logPara.EUpdDT != null)
            {
                dbFilter &= Builders<SysRecord>.Filter.Gt(x => x.UpdDT, logPara.SUpdDT)
                  & Builders<SysRecord>.Filter.Lt(y => y.UpdDT, logPara.EUpdDT);
            }

            IMongoCollection<SysRecord> col = _mongoConnectionProvider.LionGroupSERP.GetCollection<SysRecord>(logPara.CollectionNM);
            var returnData = await col.Find(dbFilter).ToListAsync();
            return returnData;
        }
    }
}
