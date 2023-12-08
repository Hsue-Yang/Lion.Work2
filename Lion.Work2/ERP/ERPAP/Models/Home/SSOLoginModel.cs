using System;
using LionTech.Entity;
using LionTech.Entity.ERP.Home;
using LionTech.Utility;

namespace ERPAP.Models.Home
{
    public class SSOLoginModel : HomeModel
    {
        public string UserID { get; set; }
        
        public string SystemID { get; set; }

        string _sessionID = string.Empty;
        public string SessionID {
            get { return _sessionID; }
            set { _sessionID = Security.Decrypt(value); }
        }

        public SSOLoginModel()
        {
            SSOLoginResult = new EntitySSOLogin.SSOLoginResult();
            SSOLoginResult.IsAuthorized = new DBChar(EnumYN.N);
        }

        public EntitySSOLogin.SSOLoginResult SSOLoginResult { get; }

        public bool GetUserConnect(int sessionCount, int delaySecond)
        {
            try
            {
                EntitySSOLogin.UserConnectPara userConnectPara = new EntitySSOLogin.UserConnectPara()
                {
                    SessionCount = new DBInt(sessionCount), 
                    DelaySecond = new DBInt(delaySecond),
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID)),
                    SystemID = new DBVarChar((string.IsNullOrWhiteSpace(this.SystemID) ? null : this.SystemID)),
                    SessionID = new DBChar((string.IsNullOrWhiteSpace(this.SessionID) ? null : this.SessionID))
                };

                if (new EntitySSOLogin(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectUserConnect(userConnectPara) != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public bool GetUserSessionData()
        {
            try
            {
                EntitySSOLogin.UserSessionDataPara userConnectPara = new EntitySSOLogin.UserSessionDataPara
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                SSOLoginResult.Data = new EntitySSOLogin(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectUserSessionData(userConnectPara);
                SSOLoginResult.IsAuthorized = new DBChar(EnumYN.Y);
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
        
        public void ExecUserConnectSSOValid(string userID, string sessionID, EnumYN ssoValid)
        {
            try
            {
                EntitySSOLogin.UserConnectPara userConnectPara = new EntitySSOLogin.UserConnectPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID)),
                    SessionID = new DBChar((string.IsNullOrWhiteSpace(sessionID) ? null : sessionID)),
                    SSOValid = new DBChar(ssoValid.ToString())
                };

                new EntitySSOLogin(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .ExecUserConnectSSOValid(userConnectPara);
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
        }

        public bool ValidateIP(string ip)
        {
            try
            {
                EntityHome.SystemIPPara para = new EntityHome.SystemIPPara()
                {
                    SystemID = new DBVarChar((string.IsNullOrWhiteSpace(this.SystemID) ? null : this.SystemID)),
                    IPAddress = new DBVarChar(ip)
                };

                DBInt ipAddressCount = new EntitySSOLogin(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectOutSourcingSystemIP(para);

                if (ipAddressCount.GetValue() > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
    }
}