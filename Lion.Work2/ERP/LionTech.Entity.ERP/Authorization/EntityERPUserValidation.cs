using System;
using System.Collections.Generic;
using System.Text;

namespace LionTech.Entity.ERP.Authorization
{
    public class EntityERPUserValidation : EntityAuthorization
    {
        public EntityERPUserValidation(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }
    }
}
