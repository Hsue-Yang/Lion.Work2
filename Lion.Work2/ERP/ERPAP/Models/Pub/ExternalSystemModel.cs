using System;
using System.Collections.Generic;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Pub;

namespace ERPAP.Models.Pub
{
    public class ExternalSystemModel : PubModel
    {
        public string SystemID { get; set; }
        
        public List<EntityExternalSystem.ExternalSystem> EntityExternalSystemList { get; private set; }

        public bool GetExternalSystemList(string userID, EnumCultureID cultureID)
        {
            try
            {
                EntityExternalSystem.ExternalSystemPara para = new EntityExternalSystem.ExternalSystemPara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                EntityExternalSystemList = new EntityExternalSystem(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectExternalSystemList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
    }
}