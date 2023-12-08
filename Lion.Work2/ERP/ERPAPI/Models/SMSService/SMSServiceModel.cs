using System;
using LionTech.Entity;
using LionTech.Entity.ERP.SMSService;

namespace ERPAPI.Models.SMSService
{
    public class SMSServiceModel : _BaseAPModel
    {
        #region - Constructor -
        public SMSServiceModel()
        {
            _entity = new EntitySMSService(ConnectionStringERP, ProviderNameERP);
        }
        #endregion

        #region - Private -
        private readonly EntitySMSService _entity;
        #endregion

        #region - 驗證寄件者帳號是否有效 -
        /// <summary>
        /// 驗證寄件者帳號是否有效
        /// </summary>
        /// <returns></returns>
        public bool ValidateSendUser(string user)
        {
            try
            {
                var isState =
                    _entity.ValidUserState(new EntitySMSService.UserStatePara
                    {
                        UserID = new DBVarChar(user)
                    });

                if (isState.GetValue() == EnumYN.Y.ToString())
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