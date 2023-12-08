using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using FirebaseAdmin.Messaging;
using LionTech.APIService.LionGroupApp;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.LionGroupAppService;
using LionTech.Utility;
using Newtonsoft.Json;

namespace ERPAPI.Models.LionGroupAppService
{
    public class PushTopicMessageModel : LionGroupAppServiceModel
    {
        #region - Definitions -
        private enum EnumTokenType 
        {
            Firebase
        }
        #endregion

        #region - Constructor -
        public PushTopicMessageModel()
        {
            _mongoEntity = new MongoTopicPush(ConnectionStringMSERP, ProviderNameMSERP);
        }
        #endregion

        #region - Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public LionGroupAppTopicMessage TopicMessage { get; set; }
        #endregion

        #region - Private -
        private readonly MongoTopicPush _mongoEntity;
        private readonly string _sound = "mysound";
        private readonly string _appID = "LionGroupApp";
        #endregion

        #region - Public Methods -
        /// <summary>
        /// 主題推播
        /// </summary>
        /// <returns></returns>
        public bool AddPushTopicMessage()
        {
            try
            {
                var message = new Message()
                {
                    Topic = TopicMessage.Topic,
                    Notification = new Notification()
                    {
                        Title = string.Empty,
                        Body = string.Join(Environment.NewLine, TopicMessage.Title, TopicMessage.Body),
                    },
                    Android = new AndroidConfig() { Notification = new AndroidNotification() { Sound = _sound } },
                    Apns = new ApnsConfig() { Aps = new Aps() { Sound = _sound } }
                };

                var respones = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
                return false;
            }
        }

        /// <summary>
        /// 寫入主題推播排程
        /// </summary>
        /// <param name="updUserID"></param>
        /// <param name="messageID"></param>
        /// <param name="apiNo"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool AddPushTopicSchedule(string updUserID, string messageID, string apiNo, string ipAddress)
        {
            try
            {
                var execSysNM = _GetSysNM(EnumSystemID.ERPAP.ToString());

                List<MongoTopicPush.AppTopicPushPara> paraList = new List<MongoTopicPush.AppTopicPushPara>();

                paraList.Add(new MongoTopicPush.AppTopicPushPara()
                {
                    MessageID = new DBVarChar(messageID),
                    TopicID = new DBVarChar(TopicMessage.Topic),
                    AppID = new DBVarChar(_appID),
                    APINo = apiNo,
                    Title = new DBNVarChar(TopicMessage.Title),
                    Body = new DBNVarChar(TopicMessage.Body),
                    DeviceTokenType = new DBVarChar(EnumTokenType.Firebase.ToString()),
                    PushDT = new DBDateTime(Common.GetDateTime(Common.FormatDateTimeString(TopicMessage.PushDateTime))),
                    UpdUserID = new DBVarChar(updUserID),
                    UpdUserNM = new DBNVarChar(updUserID),
                    UpdDT = new DBDateTime(DateTime.Now),
                    ExecSysID = new DBVarChar(EnumSystemID.ERPAP.ToString()),
                    ExecSysNM = execSysNM,
                    ExecIPAddress = new DBVarChar(ipAddress)
                });

                if (paraList.Any())
                {
                    _mongoEntity.Insert(Mongo_BaseAP.EnumMongoDocName.APP_TOPIC_PUSH, paraList);
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        /// <summary>
        /// 紀錄主題推播
        /// </summary>
        /// <param name="updUserID"></param>
        /// <param name="messageID"></param>
        /// <param name="apiNo"></param>
        /// <param name="ipAddress"></param>
        /// <param name="isPush"></param>
        public void RecordingTopicPushToLogAppTopicPush(string updUserID, string messageID, string apiNo, string ipAddress, bool isPush)
        {
            try
            {
                var execSysNM = _GetSysNM(EnumSystemID.ERPAP.ToString());

                List<Mongo_BaseAP.RecordLogAppTopicPushPara> paraList = new List<Mongo_BaseAP.RecordLogAppTopicPushPara>();

                paraList.Add(new Mongo_BaseAP.RecordLogAppTopicPushPara()
                {
                    MessageID = new DBVarChar(messageID),
                    TopicID = new DBVarChar(TopicMessage.Topic),
                    AppID = new DBVarChar(_appID),
                    APINo = apiNo,
                    Title = new DBNVarChar(TopicMessage.Title),
                    Body = new DBNVarChar(TopicMessage.Body),
                    DeviceTokenType = new DBVarChar(EnumTokenType.Firebase.ToString()),
                    PushDT = new DBDateTime(null),
                    PushSts = new DBChar(isPush ? EnumYN.Y.ToString() : EnumYN.N.ToString()),
                    UpdUserID = new DBVarChar(updUserID),
                    UpdUserNM = new DBNVarChar(updUserID),
                    UpdDT = new DBDateTime(DateTime.Now),
                    ExecSysID = new DBVarChar(EnumSystemID.ERPAP.ToString()),
                    ExecSysNM = execSysNM,
                    ExecIPAddress = new DBVarChar(ipAddress)
                });

                if (paraList.Any())
                {
                    _mongoEntity.Insert(Mongo_BaseAP.EnumMongoDocName.LOG_APP_TOPIC_PUSH, paraList);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }
        #endregion

        #region - Private Methods -
        /// <summary>
        /// 取得系統名稱
        /// </summary>
        /// <param name="SysID"></param>
        /// <returns></returns>
        private DBNVarChar _GetSysNM(string SysID)
        {
            Entity_BaseAP.BasicInfoPara entityBasicInfoPara =
                new Entity_BaseAP.BasicInfoPara(EnumCultureID.zh_TW.ToString())
                {
                    ExecSysID = new DBVarChar(SysID)
                };
            Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                .SelectBasicInfo(entityBasicInfoPara);

            return entityBasicInfo.ExecSysNM;
        }
        #endregion
    }
}