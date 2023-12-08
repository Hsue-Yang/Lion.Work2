using System;
using LionTech.Entity;
using LionTech.Entity.ERP.Subscriber;

namespace ERPAPI.Models.Subscriber
{
    public class ERPAPPsppm01Model : SubscriberModel
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
            public string pp01_stfn { get; set; }
            public string pp01_workcomp { get; set; }
            public string pp01_area { get; set; }
            public string pp01_group { get; set; }
            public string pp01_place { get; set; }
            public string pp01_dept { get; set; }
            public string pp01_team { get; set; }
            public string pp01_ptitle { get; set; }
            public string pp01_title { get; set; }
            public string pp01_identity { get; set; }
            public string pp01_level { get; set; }
        }

        public EventParaData EventData { get; set; }

        public ERPAPPsppm01Model()
        {

        }

        public bool EditRawCMUserOrg()
        {
            try
            {
                EntityERPAPPsppm01.RawCMUserPara para = new EntityERPAPPsppm01.RawCMUserPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.EventData.pp01_stfn) ? null : this.EventData.pp01_stfn)),
                    UserComID = new DBVarChar((string.IsNullOrWhiteSpace(this.EventData.pp01_workcomp) ? null : this.EventData.pp01_workcomp)),
                    UserArea = new DBVarChar((string.IsNullOrWhiteSpace(this.EventData.pp01_area) ? null : this.EventData.pp01_area)),
                    UserGroup = new DBVarChar((string.IsNullOrWhiteSpace(this.EventData.pp01_group) ? null : this.EventData.pp01_group)),
                    UserPlace = new DBVarChar((string.IsNullOrWhiteSpace(this.EventData.pp01_place) ? null : this.EventData.pp01_place)),
                    UserDept = new DBVarChar((string.IsNullOrWhiteSpace(this.EventData.pp01_dept) ? null : this.EventData.pp01_dept)),
                    Userteam = new DBVarChar((string.IsNullOrWhiteSpace(this.EventData.pp01_team) ? null : this.EventData.pp01_team)),
                    UserTitle = new DBVarChar((string.IsNullOrWhiteSpace(this.EventData.pp01_title) ? null : this.EventData.pp01_title)),
                    UserJobTitle = new DBVarChar((string.IsNullOrWhiteSpace(this.EventData.pp01_ptitle) ? null : this.EventData.pp01_ptitle)),
                    UserIdentity = new DBVarChar((string.IsNullOrWhiteSpace(this.EventData.pp01_identity) ? null : this.EventData.pp01_identity)),
                    UserLevel = new DBVarChar((string.IsNullOrWhiteSpace(this.EventData.pp01_level) ? null : this.EventData.pp01_level)),
                    UpdEDIEventNo = new DBChar((string.IsNullOrWhiteSpace(this.EDIEventNo) ? null : this.EDIEventNo))
                };

                if (new EntityERPAPPsppm01(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .EditRawCMUserOrg(para) == EntityERPAPPsppm01.EnumEditRawCMUserResult.Success)
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

        public bool DeleteRawCMUserOrg()
        {
            try
            {
                EntityERPAPPsppm01.RawCMUserPara para = new EntityERPAPPsppm01.RawCMUserPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.EventData.pp01_stfn) ? null : this.EventData.pp01_stfn))
                };

                if (new EntityERPAPPsppm01(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .DeleteRawCMUserOrg(para) == EntityERPAPPsppm01.EnumDeleteRawCMUserResult.Success)
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