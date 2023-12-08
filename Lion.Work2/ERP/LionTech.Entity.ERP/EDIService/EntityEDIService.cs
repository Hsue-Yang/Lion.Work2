namespace LionTech.Entity.ERP.EDIService
{
    public class EntityEDIService : DBEntity
    {
#if !NET461
        public EntityEDIService(string connectionName)
            : base(connectionName)
        {
        }
#endif
        public EntityEDIService(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public DBVarChar UpdUserID = new DBVarChar("EDIService.ERP");
    }
}