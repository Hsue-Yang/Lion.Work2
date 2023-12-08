using System;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Subscriber;
using LionTech.Utility;

namespace ERPAPI.Models.Subscriber
{
    public class ERPAPIscpm20Model : SubscriberModel
    {
        #region - Definitions -
        public class EventParaData
        {
            public string cp20_country { get; set; }
            public string cp20_sts { get; set; }
            public string cp20_order { get; set; }
        }
        #endregion

        #region - Constructor -
        public ERPAPIscpm20Model()
        {
            _entity = new EntityERPAPIscpm20(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Event Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }

        public string EDIEventNo { get; set; }

        //[AllowHtml]
        public string EventPara { get; set; }

        public EventParaData EventData { get; set; }
        #endregion

        #region - Private -
        private readonly EntityERPAPIscpm20 _entity;
        #endregion

        #region - 編輯代碼檔 -
        /// <summary>
        /// 編輯代碼檔
        /// </summary>
        /// <returns></returns>
        public bool EditRawCMCode()
        {
            try
            {
                EntitySubscriber.CMCodePara para = new EntitySubscriber.CMCodePara
                {
                    CodeKind = new DBVarChar(Common.GetEnumDesc(Entity_BaseAP.EnumCMCodeKind.LionCountryCode)),
                    CodeID = new DBVarChar((string.IsNullOrWhiteSpace(EventData.cp20_country) ? null : EventData.cp20_country)),
                    IsDisable = new DBChar((string.IsNullOrWhiteSpace(EventData.cp20_sts) ? null : EventData.cp20_sts)),
                    CodeNM = new DBNVarChar((string.IsNullOrWhiteSpace(EventData.cp20_country) ? null : EventData.cp20_country)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(EventData.cp20_order) ? null : EventData.cp20_order))
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
        #endregion

        #region - 刪除代碼檔 -
        /// <summary>
        /// 刪除代碼檔
        /// </summary>
        /// <returns></returns>
        public bool DeleteRawCMCode()
        {
            try
            {
                EntitySubscriber.CMCodePara para = new EntitySubscriber.CMCodePara
                {
                    CodeKind = new DBVarChar(Common.GetEnumDesc(Entity_BaseAP.EnumCMCodeKind.LionCountryCode)),
                    CodeID = new DBVarChar((string.IsNullOrWhiteSpace(EventData.cp20_country) ? null : EventData.cp20_country))
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
        #endregion
    }
}