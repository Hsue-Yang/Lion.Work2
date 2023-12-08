// 新增日期：2017-03-17
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
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
    public class PushNotificationDetailModel : PubModel, IValidatableObject
    {
        #region - Definitions -
        public enum EnumPushRoleID
        {
            User
        }

        public class PushMsgUserInfo
        {
            public string UserID { get; set; }
            public string UserNM { get; set; }
        }
        #endregion

        #region - Constructor -
        public PushNotificationDetailModel()
        {
            _entity = new EntityPushNotificationDetail(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

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
            ErrorMessageResourceType = typeof(PubPushNotificationDetail),
            ErrorMessageResourceName = nameof(PubPushNotificationDetail.SystemMsg_PushTimeFormate_Error))]
        [StringLength(5, MinimumLength = 4)]
        [InputType(EnumInputType.Other)]
        public string PushTime { get; set; }

        [Required]
        public string ImmediatelyPush { get; set; }

        public List<PushMsgUserInfo> PushMsgUserInfoList { get; set; }

        private Dictionary<string, string> _pushTimeDictionary;

        public Dictionary<string, string> PushTimeDictionary => _pushTimeDictionary ?? (_pushTimeDictionary = new Dictionary<string, string>
        {
            { EnumYN.Y.ToString(), PubPushNotificationDetail.Label_ImmediatePush },
            { EnumYN.N.ToString(), PubPushNotificationDetail.Label_LaterPush }
        });
        #endregion

        #region - Private -
        private readonly EntityPushNotificationDetail _entity;
        #endregion

        #region - Validation -
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsValid == false)
            {
                yield break;
            }

            #region - 驗證使用者必填 -
            if (PushMsgUserInfoList == null ||
                PushMsgUserInfoList.Any() == false)
            {
                yield return new ValidationResult(PubPushNotificationDetail.SystemMsg_UserID_Required);
            }
            #endregion

            if (ExecAction == EnumActionType.Add &&
                ImmediatelyPush == EnumYN.N.ToString())
            {
                #region - 驗證排程推播日期時間必填 -
                if (string.IsNullOrWhiteSpace(PushDate))
                {
                    yield return new ValidationResult(PubPushNotificationDetail.SystemMsg_PushMessageDate_Required);
                }
                #endregion

                #region - 驗證推播時間格式 -
                if (string.IsNullOrWhiteSpace(PushTime))
                {
                    yield return new ValidationResult(PubPushNotificationDetail.SystemMsg_PushMessageTime_Required);
                }
                else
                {
                    if (DateTime.Now > Common.GetFormatDate(PushDate).Add(TimeSpan.Parse(PushTime)))
                    {
                        yield return new ValidationResult(PubPushNotificationDetail.SystemMsg_PushTime_Error);
                    }
                }
                #endregion
            }
        }
        #endregion

        #region - 推播訊息 -
        /// <summary>
        /// 推播訊息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool PushMessgae(string userID)
        {
            try
            {
                LionGroupAppMessage message = new LionGroupAppMessage();
                message.FunID = EnumLionGroupFunType.Other;
                message.Title = Title;
                message.Body = Body;
                message.UserList = (from user in PushMsgUserInfoList
                                    select new LionGroupAppUserFunRole
                                    {
                                        UserID = user.UserID,
                                        RoleID = EnumPushRoleID.User.ToString()
                                    }).ToList();

                if (string.IsNullOrWhiteSpace(PushDate) == false &&
                    string.IsNullOrWhiteSpace(PushTime) == false)
                {
                    message.AddPushDateTime(Common.GetFormatDate(PushDate).Add(TimeSpan.Parse(PushTime)));
                }
                
                LionGroupAppClient lionGroupAppClient = LionGroupAppClient.Create();
                lionGroupAppClient.ClientSysID = EnumSystemID.ERPAP.ToString();
                lionGroupAppClient.ClientUserID = userID;
                lionGroupAppClient.PushMessage(message);

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