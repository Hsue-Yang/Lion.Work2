using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ERPAP.Models.Pub;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class PubController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult PushTopicNotification()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            PushTopicNotificationModel model = new PushTopicNotificationModel();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.Title = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, PushTopicNotificationModel.EnumCookieKey.Title.ToString());
                model.Body = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, PushTopicNotificationModel.EnumCookieKey.Body.ToString());
                model.StartPushDT = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, PushTopicNotificationModel.EnumCookieKey.StartPushDT.ToString());
                model.EndPushDT = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, PushTopicNotificationModel.EnumCookieKey.EndPushDT.ToString());
                model.PushStatus = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, PushTopicNotificationModel.EnumCookieKey.PushStatus.ToString());
            }
            #endregion

            if (string.IsNullOrWhiteSpace(model.StartPushDT) &&
                string.IsNullOrWhiteSpace(model.EndPushDT))
            {
                model.StartPushDT = Common.GetDateTimeFormattedText(DateTime.Now.AddDays(1 - DateTime.Now.Day), Common.EnumDateTimeFormatted.ShortDateNumber);
                model.EndPushDT = Common.GetDateTimeFormattedText(DateTime.Now.AddDays(1 - DateTime.Now.Day).AddYears(1), Common.EnumDateTimeFormatted.ShortDateNumber);
            }

            if (model.GetPushTopicNotificationList(PageSize) == false)
            {
                SetSystemErrorMessage(PubPushNotification.SystemMsg_GetPushNotificationList_Failure);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult PushTopicNotification(PushTopicNotificationModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (model.ExecAction == EnumActionType.Select)
            {
                if (TryValidatableObject(model))
                {
                    #region - Set Cookie -
                    Dictionary<string, string> paraDict =
                        new Dictionary<string, string>
                        {
                            { PushTopicNotificationModel.EnumCookieKey.Title.ToString(), model.Title },
                            { PushTopicNotificationModel.EnumCookieKey.Body.ToString(), model.Body },
                            { PushTopicNotificationModel.EnumCookieKey.StartPushDT.ToString(), model.StartPushDT },
                            { PushTopicNotificationModel.EnumCookieKey.EndPushDT.ToString(), model.EndPushDT },
                            { PushTopicNotificationModel.EnumCookieKey.PushStatus.ToString(), model.PushStatus },
                        };

                    AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                    #endregion
                }
                else
                {
                    return View(model);
                }
            }

            if (model.GetPushTopicNotificationList(PageSize) == false)
            {
                SetSystemErrorMessage(PubPushNotification.SystemMsg_GetPushNotificationList_Failure);
            }

            return View(model);
        }
    }
}