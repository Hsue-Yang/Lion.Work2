using System;
using LionTech.Entity;
using LionTech.Entity.ERP.Subscriber;

namespace ERPAPI.Models.Subscriber
{
    public class ERPAPIscpm00Model : SubscriberModel
    {
        #region Event Property
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }

        public string EDIEventNo { get; set; }

        //[AllowHtml]
        public string EventPara { get; set; }
        #endregion

        public class EventParaData
        {
            public string comp_comp { get; set; }
            public string comp_sts { get; set; }
            public string comp_bu { get; set; }
            public string comp_dname { get; set; }
            public string comp_order { get; set; }
            public string comp_country { get; set; }
            public string comp_ps { get; set; }
        }

        public EventParaData EventData { get; set; }

        public ERPAPIscpm00Model()
        {

        }

        public bool EditRawCMOrg()
        {
            try
            {
                EntityERPAPIscpm00.RawCMOrgPara para = new EntityERPAPIscpm00.RawCMOrgPara()
                {
                    ComID = new DBVarChar((string.IsNullOrWhiteSpace(EventData.comp_comp) ? null : EventData.comp_comp)),
                    ComNM = new DBNVarChar((string.IsNullOrWhiteSpace(EventData.comp_dname) ? null : EventData.comp_dname)),
                    ComBu = new DBChar(string.IsNullOrWhiteSpace(EventData.comp_bu) ? null : EventData.comp_bu),
                    IsCeased = new DBChar(EventData.comp_sts.Trim() == "1" ? EnumYN.Y.ToString() : EnumYN.N.ToString()),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(EventData.comp_order) ? null : EventData.comp_order)),
                    Country = new DBChar(string.IsNullOrWhiteSpace(EventData.comp_country) ? null : EventData.comp_country),
                    IsSalaryCom = new DBChar(EventData.comp_ps.Trim() == "1" ? EnumYN.Y.ToString() : EnumYN.N.ToString()),
                    UpdEDIEventNo = new DBChar((string.IsNullOrWhiteSpace(EDIEventNo) ? null : EDIEventNo))
                };

                if (new EntityERPAPIscpm00(ConnectionStringSERP, ProviderNameSERP)
                    .EditRawCMOrg(para) == EntityERPAPIscpm00.EnumEditRawCMOrgResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public bool DeleteRawCMOrg()
        {
            try
            {
                EntityERPAPIscpm00.RawCMOrgPara para = new EntityERPAPIscpm00.RawCMOrgPara()
                {
                    ComID = new DBVarChar((string.IsNullOrWhiteSpace(EventData.comp_comp) ? null : EventData.comp_comp))
                };

                if (new EntityERPAPIscpm00(ConnectionStringSERP, ProviderNameSERP)
                    .DeleteRawCMOrg(para) == EntityERPAPIscpm00.EnumDeleteRawCMOrgResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
    }
}