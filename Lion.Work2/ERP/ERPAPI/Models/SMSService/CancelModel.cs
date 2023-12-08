// 新增日期：2017-07-21
// 新增人員：王汶智
// 新增內容：
// ---------------------------------------------------

using System;
using LionTech.APIService.SMS;
using LionTech.Entity;
using LionTech.Entity.ERP.SMSService;

namespace ERPAPI.Models.SMSService
{
    public class CancelModel : SMSServiceModel
    {
        #region - Constructor -
        public CancelModel()
        {
            _entity = new EntityCancel(ConnectionStringERP, ProviderNameERP);
        }
        #endregion

        #region - Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        #endregion

        #region - Private -
        private readonly EntityCancel _entity;
        #endregion

        #region - 取得狀態 -
        public EntitySMSService.SMSDetail GetState(string smsYear, int smsSeq, string phoneNumber)
        {
            try
            {
                var sms = _entity.SelectSMSDetail(new EntitySMSService.SMSDetailPara
                {
                    SMSYear = new DBChar(smsYear),
                    SMSSeq = new DBInt(smsSeq),
                    PhoneNumber = new DBChar(phoneNumber)
                });

                if (sms != null)
                {
                    return sms;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return null;
        }
        #endregion

        public bool Cancel(SMSCancel cancel)
        {
            try
            {
                var result =
                    _entity.EditSMSCancel(new EntityCancel.SMSCancelPara
                    {
                        SMSYear = new DBChar(cancel.SMSYear),
                        SMSSeq = new DBInt(cancel.SMSSeq),
                        UserID = new DBVarChar(ClientUserID)
                    });

                return result == EntityCancel.EnumSMSCancelResult.Success;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
    }
}