using System;
using LionTech.APIService.SMS;
using LionTech.Entity;
using LionTech.Entity.ERP.SMSService;

namespace ERPAPI.Models.SMSService
{
    public class SendModel : SMSServiceModel
    {
        #region - Definitions -
        #endregion

        #region - Constructor -
        public SendModel()
        {
            _entity = new EntitySend(ConnectionStringERP, ProviderNameERP);
        }
        #endregion

        #region - Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public SMSSendResult SendResult { get; private set; }
        #endregion

        #region - Private -
        private readonly EntitySend _entity;
        #endregion
        
        #region - 發送簡訊 -

        public bool SMSSend(SMSMessage message)
        {
            try
            {
                var para = new EntitySend.SMSSendPara
                {
                    SmsDesc = new DBNVarChar(message.Message),
                    Stfn = new DBVarChar(message.SendUser),
                    Project = new DBNVarChar(message.ProjectName),
                    Number = new DBVarChar(message.PhoneNumber),
                    BookingTime = new DBDateTime(message.BookingDateTime),
                    OrdrYear = new DBChar(message.OrderYear),
                    OrdrOrdr = new DBInt(message.OrderNumber)
                };

                var result = _entity.EditSMSSend(para);

                if (result != null)
                {
                    SendResult = new SMSSendResult
                    {
                        SMSYear = result.SMSYear.GetValue(),
                        SMSSeq = result.SMSSeq.GetValue()
                    };
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