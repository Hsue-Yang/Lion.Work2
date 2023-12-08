using System;
using LionTech.Entity;
using LionTech.Entity.ERP.ERPPubData;

namespace ERPAPI.Models.ERPPubData
{
    public class UserFunLogModel : _BaseAPModel
    {
        #region - API Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }

        //[AllowHtml]
        public string APIPara { get; set; }
        #endregion

        public class APIParaData
        {
            public string UserID { get; set; }
            public string AspID { get; set; }
            public string Url { get; set; }
            public string ServerName { get; set; }
        }

        public APIParaData APIData { get; set; }
        
        internal void RecordUserSystemFunLog()
        {
            EntityUserFunLog entity = new EntityUserFunLog(ConnectionStringSERP, ProviderNameSERP);
            EntityUserFunLog.UserFunLogPara para = new EntityUserFunLog.UserFunLogPara();
            string[] urlspilt = APIData.Url.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            para.USER_ID = new DBVarChar(APIData.UserID);
            para.SERVER_NAME = new DBNVarChar(APIData.ServerName);
            para.ASP_ID = new DBVarChar(APIData.AspID);
            para.URL = new DBNVarChar(APIData.Url);
            para.CONTROLLER_ID = new DBNVarChar(urlspilt[0]);
            para.ACTION_NM = new DBNVarChar(urlspilt.Length > 1 ? urlspilt[1] : null);
            para.UPD_DT = new DBDateTime(DateTime.Now);

            entity.EditUserFunLog(para);
        }
    }
}