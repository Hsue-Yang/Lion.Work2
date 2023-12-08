using LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Log.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Log
{
    public class SystemLogRepository : ISystemLogRepository
    {
        private readonly IMongoConnectionProvider _mongoConnectionProvider;

        public SystemLogRepository(IMongoConnectionProvider mongoConnectionProvider)
        {
            _mongoConnectionProvider = mongoConnectionProvider;
        }

        public async Task RecordLog(string collectionName, List<BsonDocument> bsonDocs, BsonDocument condition)
        {
            var para = new BsonArray
                { new BsonDocument { { "_id", "addLogSystem" } }, collectionName, new BsonArray(bsonDocs) };
            
            if (condition == null)
            {
                para.Add(BsonNull.Value);
            }
            else
            {
                para.Add(condition);
            }

            var command = 
                new BsonDocumentCommand<BsonDocument>(
                    new BsonDocument{
                        { "eval", "function() { return db.system.js.findOne(arguments[0]).value(arguments[1], arguments[2], arguments[3]); }" },
                        { "args", para }
                    });
            await _mongoConnectionProvider.LionGroupSERP.RunCommandAsync(command);
        }
    }
}
