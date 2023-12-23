
namespace LionTech.Entity.TRAINING.Subscriber
{
    public class EntitySubscriber : DBEntity
    {
        public EntitySubscriber(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public DBVarChar UpdUserID = new DBVarChar("APIService.TRAINING.Subscriber");
    }
}