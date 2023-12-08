using System;
using LionTech.Entity;
using LionTech.Entity.ERP.Subscriber;

namespace ERPAPI.Models.Subscriber
{
    public class ERPAPPsppm00Model : SubscriberModel
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
            public string pp00_stfn { get; set; }
            public string pp00_idno { get; set; }
            public string pp00_bdate { get; set; }
            public string pp00_scmp { get; set; }
        }

        public EventParaData EventData { get; set; }

        public ERPAPPsppm00Model()
        {

        }

        public bool EditRawCMUserDetail()
        {
            try
            {
                EntityERPAPPsppm00.RawCMUserDetailPara para = new EntityERPAPPsppm00.RawCMUserDetailPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(EventData.pp00_stfn) ? null : EventData.pp00_stfn)),
                    UserIDNO = new DBVarChar((string.IsNullOrWhiteSpace(EventData.pp00_idno) ? null : EventData.pp00_idno)),
                    UserBirthday = new DBChar((string.IsNullOrWhiteSpace(EventData.pp00_bdate) ? null : EventData.pp00_bdate)),
                    UserSalaryComID = new DBChar(string.IsNullOrWhiteSpace(EventData.pp00_scmp) ? null : EventData.pp00_scmp),
                    UpdEDIEventNo = new DBChar((string.IsNullOrWhiteSpace(EDIEventNo) ? null : EDIEventNo))
                };

                if (new EntityERPAPPsppm00(ConnectionStringSERP, ProviderNameSERP)
                    .EditRawCMUserDetail(para) == EntityERPAPPsppm00.EnumEditeRawCMUserDetailResult.Success)
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

        public bool DeleteRawCMUserDetail()
        {
            try
            {
                EntityERPAPPsppm00.RawCMUserDetailPara para = new EntityERPAPPsppm00.RawCMUserDetailPara
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(EventData.pp00_stfn) ? null : EventData.pp00_stfn)
                };

                if (new EntityERPAPPsppm00(ConnectionStringSERP, ProviderNameSERP)
                    .DeleteRawCMUserDetail(para) == EntityERPAPPsppm00.EnumDeleteRawCMUserDetailResult.Success)
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