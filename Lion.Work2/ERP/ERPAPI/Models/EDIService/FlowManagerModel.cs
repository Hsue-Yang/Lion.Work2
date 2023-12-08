using System;
using LionTech.Entity;
using LionTech.Entity.ERP.EDIService;

namespace ERPAPI.Models.EDIService
{
    public class FlowManagerModel : EDIServiceModel
    {
        #region Flow Property
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }

        public string UserID { get; set; }

        //[AllowHtml]
        public string FlowPara { get; set; }
        #endregion

        public class FlowParaData
        {
            public string EDINo { get; set; }
            public string SysID { get; set; }
            public string EDIFlowID { get; set; }
            public string DataDate { get; set; }
        }

        public FlowParaData FlowData { get; set; }

        public FlowManagerModel()
        {

        }

        public string ExecuteEDIFlow()
        {
            try
            {
                EntityFlowManager.EDIFlowPara para = new EntityFlowManager.EDIFlowPara()
                    {
                        SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.FlowData.SysID) ? null : this.FlowData.SysID)),
                        EDIFlowID = new DBVarChar((string.IsNullOrWhiteSpace(this.FlowData.EDIFlowID) ? null : this.FlowData.EDIFlowID)),
                        DataDate = new DBChar((string.IsNullOrWhiteSpace(this.FlowData.DataDate) ? null : this.FlowData.DataDate)),
                    };

                if (this.ClientSysID == this.FlowData.SysID)
                {
                    return new EntityFlowManager(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .ExecuteEDIFlow(para, null);    
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
        }

        public EntityFlowManager.EDIFlow SelectEDIFlow()
        {
            try
            {
                EntityFlowManager.EDIFlowPara para = new EntityFlowManager.EDIFlowPara()
                    {
                        EDINo = new DBChar(string.IsNullOrWhiteSpace(this.FlowData.EDINo) ? null : this.FlowData.EDINo),
                        EDIFlowID = new DBVarChar(string.IsNullOrWhiteSpace(this.FlowData.EDIFlowID) ? null : this.FlowData.EDIFlowID),
                    };

                return new EntityFlowManager(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectEDIFlow(para);
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return null;
        }

    }
}
