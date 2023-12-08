namespace LionTech.Entity.B2P.Authorization
{
    public class EntityAuthorization : DBEntity
    {
        public EntityAuthorization(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public DBVarChar UpdUserID = new DBVarChar("APIService.B2P.Authorization");
    }
}