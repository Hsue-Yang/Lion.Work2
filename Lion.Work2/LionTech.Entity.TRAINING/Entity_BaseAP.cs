using System.Collections.Generic;

namespace LionTech.Entity.TRAINING
{
    public class Entity_BaseAP : DBEntity
    {
        public Entity_BaseAP(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserMessageAPIPara : DBTableRow
        {
            public DBVarChar UserID;
        }

        public class UserMessageItem : DBTableRow
        {
            public string UserMessage { get; set; }
        }

        public class UserMessageCollection : DBTableRow
        {
            public IEnumerable<UserMessageItem> UserMessages { get; set; }
        }
    }
}