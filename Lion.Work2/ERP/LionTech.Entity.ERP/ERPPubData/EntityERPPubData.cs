namespace LionTech.Entity.ERP.ERPPubData
{
    public class EntityERPPubData : DBEntity
    {
#if !NET461
        public EntityERPPubData(string connectionName)
            : base(connectionName)
        {
        }
#endif
        public EntityERPPubData(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public DBVarChar UpdUserID = new DBVarChar("APIService.ERP.ERPPubData");
    }
}