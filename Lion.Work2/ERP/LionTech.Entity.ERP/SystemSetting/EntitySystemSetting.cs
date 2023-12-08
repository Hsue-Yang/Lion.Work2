namespace LionTech.Entity.ERP.SystemSetting
{
    public class EntitySystemSetting : DBEntity
    {
#if !NET461
        public EntitySystemSetting(string connectionName)
            : base(connectionName)
        {
        }
#endif
        public EntitySystemSetting(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }
    }
}
