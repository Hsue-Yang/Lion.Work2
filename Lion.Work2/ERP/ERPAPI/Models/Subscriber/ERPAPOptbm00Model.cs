// 新增日期：2018-10-25
// 新增人員：方道筌
// 新增內容：
// ---------------------------------------------------

using System;
using LionTech.Entity;
using LionTech.Entity.ERP.Subscriber;

namespace ERPAPI.Models.Subscriber
{
    public class ERPAPOptbm00Model : SubscriberModel
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
            public string cnty_country { get; set; }
            public string cnty_sts { get; set; }
            public string cnty_dname { get; set; }
            public string cnty_cname { get; set; }
            public string cnty_ename { get; set; }
        }

        public EventParaData EventData { get; set; }

        public bool EditRawCMCountry()
        {
            try
            {
                EntityERPAPOptbm00.RawCMCountryPara para = new EntityERPAPOptbm00.RawCMCountryPara()
                {
                    CountryID = new DBChar(string.IsNullOrWhiteSpace(EventData.cnty_country) ? null : EventData.cnty_country),
                    IsDisable = new DBChar(EventData.cnty_sts.Trim() == "1" ? EnumYN.Y.ToString() : EnumYN.N.ToString()),
                    CountryNmDisplay = new DBNVarChar(string.IsNullOrWhiteSpace(EventData.cnty_dname) ? null : EventData.cnty_dname),
                    CountryNmZhTw = new DBNVarChar(string.IsNullOrWhiteSpace(EventData.cnty_cname) ? null : EventData.cnty_cname),
                    CountryNmEnUs = new DBVarChar(string.IsNullOrWhiteSpace(EventData.cnty_ename) ? null : EventData.cnty_ename)
                };

                if (new EntityERPAPOptbm00(ConnectionStringSERP, ProviderNameSERP)
                    .EditRawCMCountry(para) == EntityERPAPOptbm00.EnumEditRawCMCountryResult.Success)
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

        public bool DeleteRawCMCountry()
        {
            try
            {
                EntityERPAPOptbm00.RawCMCountryPara para = new EntityERPAPOptbm00.RawCMCountryPara()
                {
                    CountryID = new DBChar(string.IsNullOrWhiteSpace(EventData.cnty_country) ? null : EventData.cnty_country)
                };

                if (new EntityERPAPOptbm00(ConnectionStringSERP, ProviderNameSERP)
                    .DeleteRawCMCountry(para) == EntityERPAPOptbm00.EnumDeleteRawCMCountryResult.Success)
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