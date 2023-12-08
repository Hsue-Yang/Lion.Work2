using System;
using System.Collections.Generic;
using LionTech.Entity;
using LionTech.Entity.ERP.Authorization;

namespace ERPAPI.Models.Authorization
{
    public class ERPFunAssignModel : AuthorizationModel
    {
        #region API Property
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }

        //[AllowHtml]
        public string APIPara { get; set; }
        #endregion

        public class APIParaData
        {
            public string SysID { get; set; }
            public string FunControllerID { get; set; }
            public string FunActionName { get; set; }
            public List<string> UserIDList { get; set; }
        }

        public APIParaData APIData { get; set; }

        public ERPFunAssignModel()
        {

        }

        public bool EditUserFun()
        {
            try
            {
                EntityERPFunAssign entityERPFunAssign = new EntityERPFunAssign(this.ConnectionStringSERP, this.ProviderNameSERP);

                EntityERPFunAssign.UserFunPara para = new EntityERPFunAssign.UserFunPara()
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.SysID) ? null : this.APIData.SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.FunControllerID) ? null : this.APIData.FunControllerID)),
                    FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.FunActionName) ? null : this.APIData.FunActionName)),
                    UserIDList = new List<DBVarChar>()
                };

                if (this.APIData.UserIDList != null && this.APIData.UserIDList.Count > 0)
                {
                    foreach (string userID in this.APIData.UserIDList)
                    {
                        para.UserIDList.Add(new DBVarChar(userID));
                    }
                }

                if (entityERPFunAssign.EditUserFun(para) == EntityERPFunAssign.EnumEditUserFunResult.Success)
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