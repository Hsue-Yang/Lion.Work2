using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.APIService.LionGroupApp;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Pub
{
    public class PushTopicNotificationDetailModel : PubModel
    {
        #region - Property -
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Body { get; set; }

        [InputType(EnumInputType.TextBoxDatePicker)]
        public string PushDate { get; set; }

        [RegularExpression(@"^(([1-9]{1})|([0-1][0-9])|([1-2][0-3])):([0-5][0-9])$",
            ErrorMessageResourceType = typeof(PubPushTopicNotificationDetail),
            ErrorMessageResourceName = nameof(PubPushTopicNotificationDetail.SystemMsg_PushTimeFormate_Error))]
        [StringLength(5, MinimumLength = 4)]
        [InputType(EnumInputType.Other)]
        public string PushTime { get; set; }

        [Required]
        public string ImmediatelyPush { get; set; }

        private Dictionary<string, string> _pushTimeDictionary;

        public Dictionary<string, string> PushTimeDictionary => _pushTimeDictionary ?? (_pushTimeDictionary = new Dictionary<string, string>
        {
            { EnumYN.Y.ToString(), PubPushNotificationDetail.Label_ImmediatePush },
            { EnumYN.N.ToString(), PubPushNotificationDetail.Label_LaterPush }
        });
        #endregion

        #region - Method -
        /// <summary>
        /// 推播訊息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool PushTopicMessgae(string userID)
        {
            try
            {
                LionGroupAppTopicMessage message = new LionGroupAppTopicMessage();
                message.Title = Title;
                message.Body = Body;

                if (string.IsNullOrWhiteSpace(PushDate) == false &&
                    string.IsNullOrWhiteSpace(PushTime) == false)
                {
                    message.AddPushDateTime(Common.GetFormatDate(PushDate).Add(TimeSpan.Parse(PushTime)));
                }

                LionGroupAppClient lionGroupAppClient = LionGroupAppClient.Create();
                lionGroupAppClient.ClientSysID = EnumSystemID.ERPAP.ToString();
                lionGroupAppClient.ClientUserID = userID;
                lionGroupAppClient.PushMessageByAll(message);

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