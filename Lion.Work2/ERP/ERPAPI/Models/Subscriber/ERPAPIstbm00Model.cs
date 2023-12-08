using System;
using LionTech.Entity;
using LionTech.Entity.ERP.Subscriber;

namespace ERPAPI.Models.Subscriber
{
    public class ERPAPIstbm00Model : SubscriberModel
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
            public string tabl_type { get; set; }
            public string tabl_code { get; set; }
            public string tabl_sts { get; set; }
            public string tabl_sts1 { get; set; }
            public string tabl_cname { get; set; }
            public string tabl_dname { get; set; }
            public string tabl_ename { get; set; }
            public string tabl_order { get; set; }
        }

        public EventParaData EventData { get; set; }

        public ERPAPIstbm00Model()
        {
            _entity = new EntityERPAPIstbm00(ConnectionStringSERP, ProviderNameSERP);
        }

        private readonly EntityERPAPIstbm00 _entity;

        public bool EditRawCMCode()
        {
            try
            {
                EntitySubscriber.CMCodePara para = new EntitySubscriber.CMCodePara
                {
                    CodeKindNM = new DBNVarChar((string.IsNullOrWhiteSpace(EventData.tabl_type) ? null : EventData.tabl_type)),
                    CodeID = new DBVarChar((string.IsNullOrWhiteSpace(EventData.tabl_code) ? null : EventData.tabl_code)),
                    IsDisable = new DBChar((string.IsNullOrWhiteSpace(EventData.tabl_sts) ? null : EventData.tabl_sts)),
                    CodeNM = new DBNVarChar((string.IsNullOrWhiteSpace(EventData.tabl_cname) ? null : EventData.tabl_cname)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(EventData.tabl_order) ? null : EventData.tabl_order))
                };

                if (_entity.EditCMCode(para) == EntitySubscriber.EnumEditCMCodeResult.Success)
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

        public bool DeleteRawCMCode()
        {
            try
            {
                EntitySubscriber.CMCodePara para = new EntitySubscriber.CMCodePara
                {
                    CodeKindNM = new DBNVarChar((string.IsNullOrWhiteSpace(EventData.tabl_type) ? null : EventData.tabl_type)),
                    CodeID = new DBVarChar((string.IsNullOrWhiteSpace(EventData.tabl_code) ? null : EventData.tabl_code))
                };

                if (_entity.DeleteCMCode(para) == EntitySubscriber.EnumDeleteCMCodeResult.Success)
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