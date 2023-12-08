// 新增日期：2016-12-22
// 新增人員：王汶智
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.LineBotService;

namespace ERPAPI.Models.LineBotService
{
    public class WebhooksModel : LineBotServiceModel
    {
        #region - Constructor -
        public WebhooksModel()
        {
            _entity = new EntityWebhooks(ConnectionStringSERP, ProviderNameSERP);
            _mongo = new MongoWebhooks(ConnectionStringMSERP, ProviderNameMSERP);
        }
        #endregion

        #region - Property -
        public List<EntityWebhooks.SystemLine> SystemLineList { get; private set; }
        #endregion

        #region - Private -
        private readonly EntityWebhooks _entity;
        private MongoWebhooks _mongo;
        #endregion

        #region - 取得應用系統Line清單 -
        /// <summary>
        /// 取得應用系統Line清單
        /// </summary>
        /// <returns></returns>
        public bool GetSystemLineList()
        {
            try
            {
                SystemLineList = _entity.SelectSystemLineList(new EntityWebhooks.SystemLinePara(EnumCultureID.zh_TW.ToString()));
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 編輯好友清單 -
        /// <summary>
        /// 編輯好友清單
        /// </summary>
        /// <param name="systemLine"></param>
        /// <param name="receiverID"></param>
        /// <param name="source"></param>
        /// <param name="isLeave"></param>
        /// <param name="updUserID"></param>
        /// <returns></returns>
        public bool EditSystemLineReceiver(EntityWebhooks.SystemLine systemLine, string receiverID, string source, bool isLeave, string updUserID)
        {
            try
            {
                _entity.EditSystemLineReceiver(new EntityWebhooks.SystemLineReceiverPara
                {
                    SysID = systemLine.SysID,
                    LineID = systemLine.LineID,
                    IsDisable = new DBChar(isLeave ? EnumYN.Y : EnumYN.N),
                    ReceiverID = new DBVarChar(receiverID),
                    SourceType = new DBVarChar(source),
                    UpdUserID = new DBVarChar(updUserID)
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

        #region - 紀錄Line訊息 -
        /// <summary>
        /// 紀錄Line訊息
        /// </summary>
        /// <param name="systemLine"></param>
        /// <param name="jsonData"></param>
        /// <param name="ipaddress"></param>
        public void GetRecordLineWebhooks(EntityWebhooks.SystemLine systemLine, string jsonData, string ipaddress)
        {
            var log = new Dictionary<string, object>();

            if (systemLine != null)
            {
                log.Add("SYS_ID", systemLine.SysID.GetValue());
                log.Add("SYS_NM", systemLine.SysNM.GetValue());
                log.Add("LINE_ID", systemLine.LineID.GetValue());
                log.Add("LINE_NM", systemLine.LineNM.GetValue());
                log.Add("CHANNEL_ACCESS_TOKEN", systemLine.ChannelAccessToken.GetValue());
                log.Add("CHANNEL_SECRET", systemLine.ChannelSecret.GetValue());
            }

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            var data = javaScriptSerializer.Deserialize<Dictionary<string, object>>(jsonData);
            log.Add("DATA", data);
            log.Add("UPD_TP", (DateTime.Now.AddHours(-8) - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);
            log.Add("EXEC_IP_ADDRESS", ipaddress);
            
            _mongo.InsertDoc(Mongo_BaseAP.EnumLogDocName.LOG_LINE_WEBHOOKS.ToString(), javaScriptSerializer.Serialize(log));
        }
        #endregion
    }
}