using System;
using System.Collections.Generic;

namespace LionTech.Entity.B2P.Home
{
    public class EntityLogout : DBEntity
    {
        public EntityLogout(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserConnectCustLogoutPara
        {
            public enum ParaField
            {
                USER_ID, SESSION_ID
            }

            public DBVarChar UserID;
            public DBChar SessionID;
        }

        public class UserConnectCustLogout : DBTableRow
        {
        }

        public void ExecUserConnectCustLogout(UserConnectCustLogoutPara userConnectCustLogoutPara)
        {
            string commandText = string.Concat(new object[] 
            { 
                "UPDATE SYS_USER_CONNECT SET CUST_LOGOUT='Y' WHERE USER_ID={USER_ID} AND SESSION_ID={SESSION_ID}; ", Environment.NewLine,
            });
            
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserConnectCustLogoutPara.ParaField.USER_ID, Value = userConnectCustLogoutPara.UserID });
            dbParameters.Add(new DBParameter { Name = UserConnectCustLogoutPara.ParaField.SESSION_ID, Value = userConnectCustLogoutPara.SessionID });

            base.ExecuteScalar(commandText, dbParameters);
        }
    }
}
