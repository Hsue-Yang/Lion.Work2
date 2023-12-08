using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Log.Interfaces
{
    public interface ISystemLogRepository
    {
        Task RecordLog(string MongoDocName, List<BsonDocument> LogModel, BsonDocument QueryLogNoCondition);
    }
}
