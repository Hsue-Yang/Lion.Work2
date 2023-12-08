// 新增日期：2017-02-17
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------
namespace LionTech.Entity.ERP.LoginEventService
{
    public class EntityLoginEventService : DBEntity
    {
#if !NET461
        public EntityLoginEventService(string connectionName)
            : base(connectionName)
        {
        }
#endif
        public EntityLoginEventService(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }
    }
}