namespace LionTech.Entity.ERP.Home
{
    public class EntityToken : DBEntity
    {
#if !NET461
        public EntityToken(string connectionName)
            : base(connectionName)
        {
        }
#endif
        public EntityToken(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }
    }
}