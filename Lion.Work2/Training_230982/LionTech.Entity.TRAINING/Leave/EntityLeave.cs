using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LionTech.Entity.TRAINING.Leave
{
    public class EntityLeave : DBEntity
    {
        public EntityLeave(string connectionString, string providerName) : base(connectionString, providerName)
        {
        }
    }
}
