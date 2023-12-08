// 新增日期：2016-12-23
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using LionTech.APIService.Firebase;
using LionTech.APIService.LionGroupApp;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.LionGroupAppService;
using LionTech.Utility;

namespace ERPAPI.Models.LionGroupAppService
{
    public class PushMessageModel : LionGroupAppServiceModel
    {
        #region - Constructor -
        public PushMessageModel()
        {
            _mongoEntity = new MongoPushMessage(ConnectionStringMSERP, ProviderNameMSERP);
            _pushMessageUserInfoListed = new List<EntityLionGroupAppService.PushMessageUserInfo>();
            _unPushMessageUserInfoListed = new List<EntityLionGroupAppService.PushMessageUserInfo>();
            _entity = new EntityLionGroupAppService(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public LionGroupAppMessage LionGroupAppMessage;
        #endregion

        #region - Private -
        private List<EntityLionGroupAppService.PushMessageUserInfo> _pushMessageUserInfoList;
        private readonly List<EntityLionGroupAppService.PushMessageUserInfo> _pushMessageUserInfoListed;
        private readonly List<EntityLionGroupAppService.PushMessageUserInfo> _unPushMessageUserInfoListed;
        private readonly MongoPushMessage _mongoEntity;
        private readonly EntityLionGroupAppService _entity;
        private const int MaxSendRegistrationCount = 1000;
        #endregion

        #region - 取得推播訊息發送對象資訊 -
        /// <summary>
        /// 取得推播訊息發送對象資訊
        /// </summary>
        /// <returns></returns>
        public bool GetPushMessageUserInfoList()
        {
            try
            {
                EntityLionGroupAppService.PushMessageUserInfoPara para = new EntityLionGroupAppService.PushMessageUserInfoPara
                {
                    FunID = new DBVarChar(LionGroupAppMessage.FunID),
                    UserIDs = LionGroupAppMessage.UserList.Select(u => new DBVarChar(u.UserID)).ToList()
                };

                _pushMessageUserInfoList = _entity.SelectPushMessageUserInfoList(para);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 雄獅APP訊息推播 -
        /// <summary>
        /// 雄獅APP訊息推播
        /// </summary>
        /// <param name="updUserID"></param>
        /// <returns></returns>
        public bool LionGroupAppPushMessage(string updUserID)
        {
            try
            {
                if (_pushMessageUserInfoList != null &&
                    _pushMessageUserInfoList.Any())
                {
                    var groupPushMessageList =
                        (from msg in _pushMessageUserInfoList
                         group msg by msg.Os.GetValue()
                         into pushMsgList
                         let match =
                             pushMsgList
                                 .Where(msg => LionGroupAppMessage
                                     .UserList
                                     .Any(w => w.UserID == msg.UserID.GetValue() &&
                                               w.RoleID == msg.AppRoleID.GetValue() &&
                                               msg.HasRole.GetValue() == EnumYN.Y.ToString() &&
                                               msg.IsOpenPush.GetValue() == EnumYN.Y.ToString()))
                         select new
                         {
                             os = pushMsgList.Key,
                             match,
                             unMatch =
                                 pushMsgList
                                     .Where(msg =>
                                         match.Any(w => w.UserID.GetValue() == msg.UserID.GetValue()) == false)
                         }).ToList();

                    List<FirebaseService.PushDataFailureResult> pushDataFailureResults = new List<FirebaseService.PushDataFailureResult>();
                    FirebaseService firebaseService = FirebaseService.Create();

                    foreach (var groupPushMessage in groupPushMessageList)
                    {
                        FirebaseMessage message = new FirebaseMessage();
                        message.Title = LionGroupAppMessage.Title;
                        message.Body = LionGroupAppMessage.Body;
                        message.AppOSType = (EnumAppOSType)Enum.Parse(typeof(EnumAppOSType), groupPushMessage.os);
                        message.Customized = new
                        {
                            SourceType = LionGroupAppMessage.FunID.ToString(),
                            LionGroupAppMessage.Data.SourceID
                        };
                        
                        if (message.AppOSType == EnumAppOSType.iOS)
                        {
                            RouteValueDictionary valueDictionary = new RouteValueDictionary(new
                            {
                                SourceType = LionGroupAppMessage.FunID.ToString(),
                                LionGroupAppMessage.Data.SourceID
                            });

                            if (valueDictionary.ContainsKey("Title") == false)
                            {
                                valueDictionary.Add("Title", LionGroupAppMessage.Title);
                                message.Customized = valueDictionary;
                            }
                        }

                        string[] deviceTokenIDs = groupPushMessage.match.Select(u => u.RegistrationID.GetValue()).Distinct().ToArray();

                        for (int index = 0; index < (int)Math.Ceiling((deviceTokenIDs.Length / (decimal)MaxSendRegistrationCount)); index++)
                        {
                            message.RegistrationIDs = deviceTokenIDs;
                            
                            var pushDataResult = firebaseService.PushMessage(message);

                            if (pushDataResult.Failure > 0 &&
                                pushDataResult.Results != null)
                            {
                                pushDataFailureResults.AddRange(pushDataResult.Results);
                            }

                            _pushMessageUserInfoListed.AddRange(groupPushMessage.match.Where(w => pushDataFailureResults.Exists(e => e.Value == w.RegistrationID.GetValue()) == false));
                            _unPushMessageUserInfoListed.AddRange(groupPushMessage.match.Where(w => pushDataFailureResults.Exists(e => e.Value == w.RegistrationID.GetValue())));
                        }
                    }

                    _unPushMessageUserInfoListed.AddRange(groupPushMessageList.Select(s => s.unMatch).SelectMany(sm => sm.ToList()));

                    var deviceTokenList = (from s in pushDataFailureResults
                                           where s.Error_Msg == "NotRegistered"
                                           select new DBVarChar(s.Value)).ToList();

                    if (deviceTokenList.Any())
                    {
                        new EntityPushMessage(ConnectionStringSERP, ProviderNameSERP)
                            .UpdateAppUserDeviceTokenList(new EntityPushMessage.AppUserDeviceTokenPara
                            {
                                DeviceTokenList = deviceTokenList,
                                UpdUserID = new DBVarChar(updUserID)
                            });
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 紀錄推播訊息 -
        /// <summary>
        /// 紀錄推播訊息
        /// </summary>
        /// <param name="updUserID"></param>
        /// <param name="execSysID"></param>
        /// <param name="apiNo"></param>
        /// <param name="ipAddress"></param>
        /// <param name="messageID"></param>
        /// <returns></returns>
        public bool GetERPRecordAppUserPushResult(string updUserID, string execSysID, string apiNo, string ipAddress, string messageID)
        {
            try
            {
                var execSysNM = _GetSysNM(execSysID);

                List<Mongo_BaseAP.RecordLogAppUserPushPara> paraList = new List<Mongo_BaseAP.RecordLogAppUserPushPara>();

                paraList =
                    (from push in
                        (from s in _pushMessageUserInfoListed
                         group s by new
                         {
                             AppID = s.AppID.GetValue(),
                             UserID = s.UserID.GetValue(),
                             AppUUID = s.AppUUID.GetValue(),
                             AppFunID = s.AppFunID.GetValue(),
                             RegistrationID = s.RegistrationID.GetValue(),
                             Os = s.Os.GetValue(),
                             RemindMinute = s.RemindMinute.GetValue(),
                             IsOpenPush = s.IsOpenPush.GetValue()
                         }
                         into g
                         select new
                         {
                             g.Key.AppID,
                             g.Key.UserID,
                             g.Key.AppUUID,
                             g.Key.AppFunID,
                             DeviceTokenID = g.Key.RegistrationID,
                             MobileOS = g.Key.Os,
                             g.Key.RemindMinute,
                             g.Key.IsOpenPush,
                             AppRoleID = LionGroupAppMessage.UserList.Where(f => f.UserID == g.Key.UserID && LionGroupAppMessage.FunID.ToString() == g.Key.AppFunID).Select(s => s.RoleID).SingleOrDefault(),
                             AppRoleIDList = g.Where(w => w.HasRole.GetValue() == EnumYN.Y.ToString()).Select(s => s.AppRoleID).ToList()
                         })
                     select new Mongo_BaseAP.RecordLogAppUserPushPara
                     {
                         MessageID = new DBVarChar(messageID),
                         AppID = new DBVarChar(push.AppID),
                         UserID = new DBVarChar(push.UserID),
                         AppUUID = new DBVarChar(push.AppUUID),
                         AppFunID = new DBVarChar(push.AppFunID),
                         AppRoleID = new DBVarChar(push.AppRoleID),
                         AppRoleIDList = push.AppRoleIDList,
                         DeviceTokenID = new DBVarChar(push.DeviceTokenID),
                         MobileOS = new DBVarChar(push.MobileOS),
                         RemindMinute = new DBInt(push.RemindMinute),
                         IsOpenPush = new DBChar(push.IsOpenPush),
                         PushDT = new DBDateTime(null),
                         PushSts = new DBChar(EnumYN.Y.ToString()),
                         Title = new DBNVarChar(LionGroupAppMessage.Title),
                         Body = new DBNVarChar(LionGroupAppMessage.Body),
                         Data = new Mongo_BaseAP.PushMsgData
                         {
                             SourceID = new DBNVarChar(LionGroupAppMessage.Data.SourceID),
                             SourceType = new DBVarChar(LionGroupAppMessage.FunID.ToString())
                         },
                         APINo = new DBChar(apiNo),
                         UpdUserID = new DBVarChar(updUserID),
                         UpdUserNM = new DBNVarChar(updUserID),
                         UpdDT = new DBDateTime(DateTime.Now),
                         ExecSysID = new DBVarChar(execSysID),
                         ExecSysNM = execSysNM,
                         ExecIPAddress = new DBVarChar(ipAddress)
                     }).ToList();

                if (paraList.Any())
                {
                    _mongoEntity.Insert(Mongo_BaseAP.EnumLogDocName.LOG_APP_USER_PUSH, paraList);
                }

                paraList =
                    (from push in
                        (from s in _unPushMessageUserInfoListed
                         group s by new
                         {
                             AppID = s.AppID.GetValue(),
                             UserID = s.UserID.GetValue(),
                             AppUUID = s.AppUUID.GetValue(),
                             AppFunID = s.AppFunID.GetValue(),
                             RegistrationID = s.RegistrationID.GetValue(),
                             Os = s.Os.GetValue(),
                             RemindMinute = s.RemindMinute.GetValue(),
                             IsOpenPush = s.IsOpenPush.GetValue()
                         }
                         into g
                         select new
                         {
                             g.Key.AppID,
                             g.Key.UserID,
                             g.Key.AppUUID,
                             g.Key.AppFunID,
                             DeviceTokenID = g.Key.RegistrationID,
                             MobileOS = g.Key.Os,
                             g.Key.RemindMinute,
                             g.Key.IsOpenPush,
                             AppRoleID = LionGroupAppMessage.UserList.Where(f => f.UserID == g.Key.UserID && LionGroupAppMessage.FunID.ToString() == g.Key.AppFunID).Select(s => s.RoleID).SingleOrDefault(),
                             AppRoleIDList = g.Where(w => w.HasRole.GetValue() == EnumYN.Y.ToString()).Select(s => s.AppRoleID).ToList()
                         })
                     select new Mongo_BaseAP.RecordLogAppUserPushPara
                     {
                         MessageID = new DBVarChar(messageID),
                         AppID = new DBVarChar(push.AppID),
                         UserID = new DBVarChar(push.UserID),
                         AppUUID = new DBVarChar(push.AppUUID),
                         AppFunID = new DBVarChar(push.AppFunID),
                         AppRoleID = new DBVarChar(push.AppRoleID),
                         AppRoleIDList = push.AppRoleIDList,
                         DeviceTokenID = new DBVarChar(push.DeviceTokenID),
                         MobileOS = new DBVarChar(push.MobileOS),
                         RemindMinute = new DBInt(push.RemindMinute),
                         IsOpenPush = new DBChar(push.IsOpenPush),
                         PushDT = new DBDateTime(null),
                         PushSts = new DBChar(EntityLionGroupAppService.EnumAppServicePushStatus.N),
                         Title = new DBNVarChar(LionGroupAppMessage.Title),
                         Body = new DBNVarChar(LionGroupAppMessage.Body),
                         Data = new Mongo_BaseAP.PushMsgData
                         {
                             SourceID = new DBNVarChar(LionGroupAppMessage.Data.SourceID),
                             SourceType = new DBVarChar(LionGroupAppMessage.FunID.ToString())
                         },
                         APINo = new DBChar(apiNo),
                         UpdUserID = new DBVarChar(updUserID),
                         UpdUserNM = new DBNVarChar(updUserID),
                         UpdDT = new DBDateTime(DateTime.Now),
                         ExecSysID = new DBVarChar(execSysID),
                         ExecSysNM = execSysNM,
                         ExecIPAddress = new DBVarChar(ipAddress)
                     }).ToList();

                if (paraList.Any())
                {
                    _mongoEntity.Insert(Mongo_BaseAP.EnumLogDocName.LOG_APP_USER_PUSH, paraList);
                }

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 寫入推播排程 -
        /// <summary>
        /// 寫入推播排程
        /// </summary>
        /// <param name="updUserID"></param>
        /// <param name="messageID"></param>
        /// <param name="execSysID"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool AddUserPushSchedule(string updUserID, string messageID, string execSysID, string apiNo, string ipAddress)
        {
            try
            {
                var execSysNM = _GetSysNM(execSysID);

                List<MongoPushMessage.AppUserPushPara> paraList = new List<MongoPushMessage.AppUserPushPara>();

                paraList = (from schedule in _pushMessageUserInfoList
                            where LionGroupAppMessage
                                .UserList
                                .Any(w => w.UserID == schedule.UserID.GetValue() &&
                                          w.RoleID == schedule.AppRoleID.GetValue())
                            select new MongoPushMessage.AppUserPushPara
                            {
                                MessageID = new DBVarChar(messageID),
                                AppID = schedule.AppID,
                                UserID = schedule.UserID,
                                AppUUID = schedule.AppUUID,
                                AppFunID = schedule.AppFunID,
                                AppRoleID = schedule.AppRoleID,
                                Title = new DBNVarChar(LionGroupAppMessage.Title),
                                Body = new DBNVarChar(LionGroupAppMessage.Body),
                                PushDT = new DBDateTime(Common.GetDateTime(Common.FormatDateTimeString(LionGroupAppMessage.PushDateTime))),
                                Data = new Mongo_BaseAP.PushMsgData
                                {
                                    SourceID = new DBNVarChar(LionGroupAppMessage.Data.SourceID),
                                    SourceType = new DBVarChar(LionGroupAppMessage.FunID.ToString())
                                },
                                APINo = new DBChar(apiNo),
                                UpdUserID = new DBVarChar(updUserID),
                                UpdUserNM = new DBNVarChar(updUserID),
                                UpdDT = new DBDateTime(DateTime.Now),
                                ExecSysID = new DBVarChar(execSysID),
                                ExecSysNM = execSysNM,
                                ExecIPAddress = new DBVarChar(ipAddress)
                            }).ToList();

                if (paraList.Any())
                {
                    _mongoEntity.Insert(Mongo_BaseAP.EnumMongoDocName.APP_USER_PUSH, paraList);
                }
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 取得系統名稱 -
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