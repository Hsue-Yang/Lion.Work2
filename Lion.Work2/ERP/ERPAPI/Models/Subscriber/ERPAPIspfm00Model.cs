using System;
using LionTech.Entity;
using LionTech.Entity.ERP.Subscriber;

namespace ERPAPI.Models.Subscriber
{
    public class ERPAPIspfm00Model : SubscriberModel
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
            public string prof_prof { get; set; }
            public string prof_sts { get; set; }
            public string prof_dname { get; set; }
        }

        public EventParaData EventData { get; set; }

        public ERPAPIspfm00Model()
        {

        }

        public bool EditRawCMOrgUnit()
        {
            try
            {
                EntityERPAPIspfm00.RawCMOrgUnitPara para = new EntityERPAPIspfm00.RawCMOrgUnitPara()
                {
                    UnitID = new DBVarChar((string.IsNullOrWhiteSpace(this.EventData.prof_prof) ? null : this.EventData.prof_prof)),
                    UnitNM = new DBNVarChar((string.IsNullOrWhiteSpace(this.EventData.prof_dname) ? null : this.EventData.prof_dname)),
                    IsCeased = new DBChar(this.EventData.prof_sts.Trim() == "1" ? EnumYN.Y.ToString() : EnumYN.N.ToString()),
                    UpdEDIEventNo = new DBChar((string.IsNullOrWhiteSpace(this.EDIEventNo) ? null : this.EDIEventNo))
                };

                if (new EntityERPAPIspfm00(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .EditRawCMOrgUnit(para) == EntityERPAPIspfm00.EnumEditRawCMOrgUnitResult.Success)
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

        public bool DeleteRawCMOrgUnit()
        {
            try
            {
                EntityERPAPIspfm00.RawCMOrgUnitPara para = new EntityERPAPIspfm00.RawCMOrgUnitPara()
                {
                    UnitID = new DBVarChar((string.IsNullOrWhiteSpace(this.EventData.prof_prof) ? null : this.EventData.prof_prof))
                };

                new EntityERPAPIspfm00(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .DeleteRawCMOrgUnit(para);

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