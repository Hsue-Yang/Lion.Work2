using System;
using LionTech.Entity;
using LionTech.Entity.ERP.Subscriber;

namespace ERPAPI.Models.Subscriber
{
    public class ERPAPPstbm20Model : SubscriberModel
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
            public string tb20_type1 { get; set; }
            public string tb20_type2 { get; set; }
            public string tb20_seq { get; set; }
            public string tb20_sts { get; set; }
            public string tb20_name { get; set; }
            public string tb20_ename { get; set; }
            public string tb20_order { get; set; }
        }

        public EventParaData EventData { get; set; }

        public ERPAPPstbm20Model()
        {
            _entity = new EntityERPAPPstbm20(ConnectionStringSERP, ProviderNameSERP);
        }

        private readonly EntityERPAPPstbm20 _entity;

        public bool EditRawCMCode()
        {
            try
            {
                if (EventData.tb20_type1 == "B")
                {
                    EntitySubscriber.CMCodePara para = new EntitySubscriber.CMCodePara
                    {
                        CodeKindNM = new DBNVarChar((string.IsNullOrWhiteSpace(EventData.tb20_type2) ? null : EventData.tb20_type2)),
                        CodeID = new DBVarChar((string.IsNullOrWhiteSpace(EventData.tb20_seq) ? null : EventData.tb20_seq)),
                        IsDisable = new DBChar((string.IsNullOrWhiteSpace(EventData.tb20_sts) ? null : EventData.tb20_sts)),
                        CodeNM = new DBNVarChar((string.IsNullOrWhiteSpace(EventData.tb20_name) ? null : EventData.tb20_name)),
                        SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(EventData.tb20_order) ? null : EventData.tb20_order))
                    };

                    if (_entity.EditCMCode(para) == EntitySubscriber.EnumEditCMCodeResult.Success)
                    {
                        return true;
                    }
                }
                else
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
                    CodeKindNM = new DBNVarChar((string.IsNullOrWhiteSpace(EventData.tb20_type2) ? null : EventData.tb20_type2)),
                    CodeID = new DBVarChar((string.IsNullOrWhiteSpace(EventData.tb20_seq) ? null : EventData.tb20_seq))
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