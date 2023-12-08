using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Configuration
{
    public interface IMongoConnectionProvider
    {
        IMongoDatabase LionGroupSERP { get; }
    }

    public class MongoConnectionProvider : IMongoConnectionProvider
    {
        public IMongoDatabase LionGroupSERP { get; }

        public MongoConnectionProvider(IConfiguration configuration)
        {
            var connectionStrings = configuration.GetSection("ConnectionStrings").Get<Dictionary<string, string>>();
            if (connectionStrings.TryGetValue("LionGroupMSERP", out _connStringSerp))
            {
                _connStringSerp = LionTech.AspNetCore.Utility.Security.Decrypt(_connStringSerp);
                IMongoClient mongoClient = new MongoClient(_connStringSerp);
                LionGroupSERP = mongoClient.GetDatabase("LionGroupSERP");
            }
        }

        private static string _connStringSerp;
    }
}
