// 新增日期：2017-02-13
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using LionTech.Entity;
using LionTech.Entity.ERP.LionGroupAppService;
using LionTech.Utility;

namespace ERPAPI.Models.LionGroupAppService
{
    public class LogPushMessageModel : LionGroupAppServiceModel
    {
        #region - Constructor -
        public LogPushMessageModel()
        {
            _mongoentity = new MongoLogPushMessage(ConnectionStringMSERP, ProviderNameMSERP);
        }
        #endregion

        #region - Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public string UserID { get; set; }
        public string UUID { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public List<MongoLogPushMessage.LogPushMessage> LogPushMessageList { get; private set; }
        #endregion

        #region - Private -
        private readonly MongoLogPushMessage _mongoentity;
        #endregion

        #region - 取得推播歷史紀錄清單 -
        /// <summary>
        /// 取得推播歷史紀錄清單
        /// </summary>
        /// <returns></returns>
        public bool GetLogPushMessageList()
        {
            try
            {
                MongoLogPushMessage.LogPushMessagePara para = new MongoLogPushMessage.LogPushMessagePara
                {
                    UserID = new DBVarChar(UserID),
                    UUID = new DBVarChar(UUID.ToLower()),
                    SourceType = new DBVarChar(LionTech.APIService.LionGroupApp.EnumLionGroupFunType.Other.ToString()),
                    StartDateTime = new DBDateTime(Common.GetDateTime(Common.FormatDateTimeString(StartDateTime))),
                    EndDateTime = new DBDateTime(string.IsNullOrWhiteSpace(EndDateTime) ? new DateTime?() : Common.GetDateTime(Common.FormatDateTimeString(EndDateTime)))
                };

                LogPushMessageList = _mongoentity.SelectLogPushMessageList(para);

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