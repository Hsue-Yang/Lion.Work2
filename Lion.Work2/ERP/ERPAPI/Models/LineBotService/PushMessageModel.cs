using System;
using System.Collections.Generic;
using System.Linq;
using LionTech.Entity;
using LionTech.Entity.ERP.LineBotService;

namespace ERPAPI.Models.LineBotService
{
    public class PushMessageModel : LineBotServiceModel
    {
        #region - Constructor -
        public PushMessageModel()
        {
            _entity = new EntityPushMessage(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string SysID { get; set; }
        public string LineID { get; set; }
        public List<string> To { get; set; }
        public List<EntityPushMessage.LineReceiver> LineReceiverList { get; private set; }
        #endregion

        #region - Private -
        private readonly EntityPushMessage _entity;
        #endregion
        
        #region - 取得接收者清單 -
        /// <summary>
        /// 取得接收者清單
        /// </summary>
        /// <returns></returns>
        public bool GetLineReceiverList()
        {
            try
            {
                LineReceiverList =
                    _entity.SelectLineReceiverList(
                        new EntityPushMessage.LineReceiverPara
                        {
                            SysID = new DBVarChar(ClientSysID),
                            LineID = new DBVarChar(LineID),
                            LineReceiverIDList = To != null ? To.Select(s => new DBVarChar(s)).ToList() : new List<DBVarChar>()
                        });

                return true;
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