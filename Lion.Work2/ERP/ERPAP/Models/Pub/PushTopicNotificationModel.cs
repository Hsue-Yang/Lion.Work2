using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LionTech.Utility;
using LionTech.Entity.ERP.Pub;
using LionTech.Web.ERPHelper;
using Resources;
using LionTech.Entity;

namespace ERPAP.Models.Pub
{
    public class PushTopicNotificationModel : PubModel, IValidatableObject
    {
        #region - Definitions -
        public enum EnumPushDateRange
        {
            Range = 365
        }

        public new enum EnumCookieKey
        {
            Title,
            Body,
            StartPushDT,
            EndPushDT,
            PushStatus,
        }

        public enum EnumAppServicePushStatus
        {
            [Description("Send")]
            Y,
            [Description("NotSend")]
            N
        }
        #endregion

        #region - Constructor -
        public PushTopicNotificationModel()
        {
            _entity = new EntityPushTopicNotification(ConnectionStringSERP, ProviderNameSERP);
            _mongoEntity = new MongoPushTopicNotification(ConnectionStringMSERP, ProviderNameMSERP);
        }
        #endregion

        #region - Property -
        public string MessageID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxDatePicker)]
        public string StartPushDT { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxDatePicker)]
        public string EndPushDT { get; set; }

        public string PushStatus { get; set; }
        public List<MongoPushTopicNotification.PushTopicNotification> PushTopicNotificationList { get; private set; }

        public Dictionary<string, string> PushStatusDictionary => _pushStatusDictionary ?? (_pushStatusDictionary = new Dictionary<string, string>
        {
            { string.Empty, string.Empty },
            { EnumAppServicePushStatus.Y.ToString(), PubPushTopicNotification.Lable_SendStatues },
            { EnumAppServicePushStatus.N.ToString(), PubPushTopicNotification.Lable_NotSendStatues }
        });
        #endregion

        #region - Private -
        private Dictionary<string, string> _pushStatusDictionary;
        private readonly EntityPushTopicNotification _entity;
        private readonly MongoPushTopicNotification _mongoEntity;
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
                    yield return new ValidationResult(PubPushTopicNotification.SystemMsg_PushDate_Error);
                }
                #endregion

                #region - 搜尋日期範圍驗證(一年內) -
                TimeSpan ts = Common.GetFormatDate(EndPushDT) - Common.GetFormatDate(StartPushDT);

                if (ts.Days > (int)EnumPushDateRange.Range)
                {
                    yield return new ValidationResult(PubPushTopicNotification.SystemMsg_PushDateRange_Error);
                }
                #endregion
            }
        }
        #endregion

        #region - Methods -
        /// <summary>
        /// 搜尋主題推播紀錄
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public bool GetPushTopicNotificationList(int pageSize)
        {
            try
            {
                MongoPushTopicNotification.PushTopicNotificationPara para = new MongoPushTopicNotification.PushTopicNotificationPara
                {
                    Title = new DBNVarChar(string.IsNullOrWhiteSpace(Title) ? null : Title),
                    Body = new DBNVarChar(string.IsNullOrWhiteSpace(Body) ? null : Body),
                    PushSts = new DBChar(string.IsNullOrWhiteSpace(PushStatus) ? null : PushStatus),
                    StartPushDateTime = new DBDateTime(string.IsNullOrWhiteSpace(StartPushDT) ? new DateTime?() : Common.GetFormatDate(StartPushDT)),
                    EndPushDateTime = new DBDateTime(string.IsNullOrWhiteSpace(EndPushDT) ? new DateTime?() : Common.GetFormatDate(EndPushDT).AddDays(1).AddSeconds(-1))
                };

                PushTopicNotificationList = _mongoEntity.SelectPushNotificationList(para);
                PushTopicNotificationList = GetEntitysByPage(PushTopicNotificationList, pageSize);
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