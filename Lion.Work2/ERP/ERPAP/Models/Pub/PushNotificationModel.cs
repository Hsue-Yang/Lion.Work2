// 新增日期：2017-03-17
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LionTech.APIService.LionGroupApp;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Pub;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Pub
{
    public class PushNotificationModel : PubModel, IValidatableObject
    {
        #region - Definitions -
        public enum EnumPushDateRange
        {
            Range = 31
        }

        public new enum EnumCookieKey
        {
            UserID,
            AppFunID,
            Title,
            Body,
            StartPushDT,
            EndPushDT,
            PushStatus,
            IncludeUnSent
        }

        public enum EnumAppServicePushStatus
        {
            [Description("Cancel")]
            C,
            [Description("NotSend")]
            N,
            [Description("Send")]
            Y,
            [Description("Error")]
            E
        }
        #endregion

        #region - Constructor -
        public PushNotificationModel()
        {
            _entity = new EntityPushNotification(ConnectionStringSERP, ProviderNameSERP);
            _mongoEntity = new MongoPushNotification(ConnectionStringMSERP, ProviderNameMSERP);
            _entityBaseAP = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string MessageID { get; set; }
        public string ErrorMsg { get; set; }
        public string AppUUID { get; set; }
        public string UserID { get; set; }
        public string AppFunID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxDatePicker)]
        public string StartPushDT { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxDatePicker)]
        public string EndPushDT { get; set; }

        public string CancelPush { get; set; }
        public string PushStatus { get; set; }
        public string IncludeUnSent { get; set; }
        public List<string> CancelPushMsgList { get; set; }
        public List<MongoPushNotification.PushNotification> PushNotificationList { get; private set; }

        private Dictionary<string, string> _appFunTypeDictionary;

        public Dictionary<string, string> AppFunTypeDictionary => _appFunTypeDictionary ?? (_appFunTypeDictionary = new Dictionary<string, string>
        {
            { string.Empty, string.Empty },
            { EnumLionGroupFunType.Other.ToString(), PubPushNotification.Label_AppFunTypeOther },
            { EnumLionGroupFunType.Metting.ToString(), PubPushNotification.Label_AppFunTypeMeeting },
            { EnumLionGroupFunType.LeaveApply.ToString(), PubPushNotification.Label_AppFunTypeLeaveApply },
            { EnumLionGroupFunType.Message.ToString(), PubPushNotification.Label_AppFunTypeMessage },
            { EnumLionGroupFunType.LeaveSupplement.ToString(), PubPushNotification.Label_AppFunTypeLeaveSupplement }
        });

        private Dictionary<string, string> _pushStatusDictionary;

        public Dictionary<string, string> PushStatusDictionary => _pushStatusDictionary ?? (_pushStatusDictionary = new Dictionary<string, string>
        {
            { string.Empty, string.Empty },
            { EnumAppServicePushStatus.E.ToString(), PubPushNotification.Lable_ErrorStatues },
            { EnumAppServicePushStatus.Y.ToString(), PubPushNotification.Lable_SendStatues },
            { EnumAppServicePushStatus.N.ToString(), PubPushNotification.Lable_NotSendStatues },
            { EnumAppServicePushStatus.C.ToString(), PubPushNotification.Lable_CancelStatues }
        });
        #endregion

        #region - Private -
        private readonly EntityPushNotification _entity;
        private readonly MongoPushNotification _mongoEntity;
        private readonly Entity_BaseAP _entityBaseAP;
        #endregion

        #region - Validation -
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsValid == false)
            {
                yield break;
            }

            if (ExecAction == EnumActionType.Select)
            {
                #region - 搜尋日期資料驗證 -
                if (Common.GetFormatDate(StartPushDT) > Common.GetFormatDate(EndPushDT))
                {
                    yield return new ValidationResult(PubPushNotification.SystemMsg_PushDate_Error);
                }
                #endregion

                #region - 搜尋日期範圍驗證 -
                TimeSpan ts = Common.GetFormatDate(EndPushDT) - Common.GetFormatDate(StartPushDT);

                if (ts.Days > (int)EnumPushDateRange.Range)
                {
                    yield return new ValidationResult(PubPushNotification.SystemMsg_PushDateRange_Error);
                }
                #endregion
            }
        }
        #endregion
        
        #region - 取得推播紀錄清單 -
        /// <summary>
        /// 取得推播紀錄清單
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public bool GetPushNotificationList(int pageSize, EnumCultureID cultureID)
        {
            try
            {
                MongoPushNotification.PushNotificationPara para = new MongoPushNotification.PushNotificationPara
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(UserID) ? null : UserID),
                    AppFunID = new DBVarChar(string.IsNullOrWhiteSpace(AppFunID) ? null : AppFunID),
                    Title = new DBNVarChar(string.IsNullOrWhiteSpace(Title) ? null : Title),
                    Body = new DBNVarChar(string.IsNullOrWhiteSpace(Body) ? null : Body),
                    PushSts = new DBChar(string.IsNullOrWhiteSpace(PushStatus) ? null : PushStatus),
                    StartPushDateTime = new DBDateTime(string.IsNullOrWhiteSpace(StartPushDT) ? new DateTime?(): Common.GetFormatDate(StartPushDT)),
                    EndPushDateTime = new DBDateTime(string.IsNullOrWhiteSpace(EndPushDT) ? new DateTime?(): Common.GetFormatDate(EndPushDT).AddDays(1).AddSeconds(-1))
                };

                var pushNotificationList = _mongoEntity.SelectPushNotificationList(para);

                if (IncludeUnSent == EnumYN.Y.ToString())
                {
                    var appUserPushList = _mongoEntity.SelectAppUserPushList(para);

                    if (appUserPushList != null &&
                        appUserPushList.Any())
                    {
                        pushNotificationList.InsertRange(0, appUserPushList);
                    }
                }

                PushNotificationList = GetEntitysByPage(pushNotificationList, pageSize);

                List<DBVarChar> userIDList = Utility.ToDBTypeList<DBVarChar>(PushNotificationList.Select(s => s.UserID.GetValue()).Distinct());
                var userList = _entityBaseAP.SelectRawCMUserList(new Entity_BaseAP.RawCMUserPara { UserIDList = userIDList });

                PushNotificationList.ForEach(row =>
                {
                    var rawCmUser = userList.Find(f => f.UserID.GetValue() == row.UserID.GetValue());
                    if (rawCmUser != null)
                    {
                        row.UserIDNM = rawCmUser.UserIDNM;
                    }
                });

                List<DBVarChar> appUUIDList = Utility.ToDBTypeList<DBVarChar>(PushNotificationList.Select(s => s.AppUUID.GetValue()).Distinct());
                var mobileInfoList = _entity.SelectMobileInfoList(new EntityPushNotification.MobileInfoPara(cultureID.ToString()) { AppUUIDList = appUUIDList });
                PushNotificationList.ForEach(row =>
                {
                    var mobileInfo = mobileInfoList.Find(f => f.AppUUID.GetValue() == row.AppUUID.GetValue());
                    if (mobileInfo != null)
                    {
                        row.MobileType = mobileInfo.CodeNM.IsNull() ? mobileInfo.MobileType : new DBVarChar(mobileInfo.CodeNM.GetValue());
                    }
                    else
                    {
                        row.MobileType = new DBVarChar(null);
                    }
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

        #region - 取消推播 -
        /// <summary>
        /// 取消推播
        /// </summary>
        /// <returns></returns>
        public bool CancelPushMsg(string userID)
        {
            try
            {
                if (CancelPushMsgList != null &&
                    CancelPushMsgList.Any())
                {
                    LionGroupAppClient lionGroupAppClient = LionGroupAppClient.Create();
                    lionGroupAppClient.ClientSysID = EnumSystemID.ERPAP.ToString();
                    lionGroupAppClient.ClientUserID = userID;

                    foreach (var row in (from s in (from s in CancelPushMsgList
                                                    where s.Split('|').Length == 2
                                                    select new
                                                    {
                                                        messageID = s.Split('|')[0],
                                                        userID = s.Split('|')[1]
                                                    })
                                         group s by s.messageID
                                         into g
                                         select new
                                         {
                                             messageID = g.Key,
                                             userIDList = g.Select(s => s.userID)
                                         }))
                    {
                        lionGroupAppClient.CancelPushMessage(row.messageID, row.userIDList);
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

        #region - 取得推播錯誤訊息 -
        /// <summary>
        /// 取得推播錯誤訊息
        /// </summary>
        /// <returns></returns>
        public bool GetPushNotificationErrorMsg()
        {
            try
            {
                MongoPushNotification.PushNotificationErrorMsgPara para = new MongoPushNotification.PushNotificationErrorMsgPara
                {
                    MessageID = new DBVarChar(string.IsNullOrWhiteSpace(MessageID) ? null : MessageID),
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(UserID) ? null : UserID),
                    AppUUID = new DBVarChar(string.IsNullOrWhiteSpace(AppUUID) ? null : AppUUID)
                };

                ErrorMsg = _mongoEntity.SelectPushNotificationErrorMsg(para)?.ErrorMessage.GetValue();
                
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