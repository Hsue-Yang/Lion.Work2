namespace LionTech.Entity.ERP.Authorization
{
    public class EntityAuthorization : DBEntity
    {
#if !NET461
        public EntityAuthorization(string connectionName)
            : base(connectionName)
        {
        }
#endif
        public EntityAuthorization(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public DBVarChar UpdUserID = new DBVarChar("APIService.ERP.Authorization");
    }

    public class MongoAuthorization : MongoEntity
    {
        public MongoAuthorization(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public DBVarChar UpdUserID = new DBVarChar("APIService.ERP.Authorization");
    }
}