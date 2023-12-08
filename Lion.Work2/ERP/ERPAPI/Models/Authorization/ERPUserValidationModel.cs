using System;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;

namespace ERPAPI.Models.Authorization
{
    public class ERPUserValidationModel : AuthorizationModel
    {
        #region - API Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public string APIPara { get; set; }
        public APIParaData APIData { get; set; }
        #endregion

        public class APIParaData
        {
            public string UserID { get; set; }
            public string UserPWD { get; set; }
        }
        
        public void Format()
        {
            APIData.UserID = APIData.UserID?.ToUpper();
        }

        public bool GetValidUserAccountResult()
        {
            try
            {
                string userPWD = Security.Decrypt(APIData.UserPWD, Security.EnumEncodeType.UTF8);

                Entity_BaseAP.UserAccountPara para = new Entity_BaseAP.UserAccountPara
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(APIData.UserID) ? null : APIData.UserID)),
                    UserPWD = new DBVarChar((string.IsNullOrWhiteSpace(userPWD) ? null : userPWD))
                };

                return new Entity_BaseAP(ConnectionStringERP, ProviderNameERP).ValidUserAccount(para);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
    }
}