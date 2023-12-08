using System;
using LionTech.Entity;
using LionTech.Entity.B2P.Home;

namespace B2PAP.Models.Home
{
    public class LogoutModel : _BaseAPModel
    {
        public LogoutModel()
        {
        }

        public void ExecUserConnectCustLogout(string userID, string sessionID)
        {
            try
            {
                EntityLogout.UserConnectCustLogoutPara userConnectCustLogoutPara = new EntityLogout.UserConnectCustLogoutPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                    SessionID = new DBChar((string.IsNullOrWhiteSpace(sessionID) ? null : sessionID))
                };

                new EntityLogout(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .ExecUserConnectCustLogout(userConnectCustLogoutPara);
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
        }
    }
}