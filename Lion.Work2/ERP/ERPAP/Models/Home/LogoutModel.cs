using System;
using LionTech.Entity;
using LionTech.Entity.ERP.Home;

namespace ERPAP.Models.Home
{
    public class LogoutModel : HomeModel
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

                new EntityLogout(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .ExecUserConnectCustLogout(userConnectCustLogoutPara);
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
        }
    }
}