// 新增日期：2017-03-17
// 新增人員：廖先駿
// 新增內容：推播通知
// ---------------------------------------------------

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
        public ActionResult PushNotification()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            PushNotificationModel model = new PushNotificationModel();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.UserID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, PushNotificationModel.EnumCookieKey.UserID.ToString());
                model.AppFunID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, PushNotificationModel.EnumCookieKey.AppFunID.ToString());
                model.Title = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, PushNotificationModel.EnumCookieKey.Title.ToString());
                model.Body = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, PushNotificationModel.EnumCookieKey.Body.ToString());
                model.StartPushDT = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, PushNotificationModel.EnumCookieKey.StartPushDT.ToString());
                model.EndPushDT = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, PushNotificationModel.EnumCookieKey.EndPushDT.ToString());
                model.PushStatus = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, PushNotificationModel.EnumCookieKey.PushStatus.ToString());
                model.IncludeUnSent = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, PushNotificationModel.EnumCookieKey.IncludeUnSent.ToString());
            }
            #endregion

            if (string.IsNullOrWhiteSpace(model.StartPushDT) &&
                string.IsNullOrWhiteSpace(model.EndPushDT))
            {
                model.StartPushDT = Common.GetDateTimeFormattedText(DateTime.Now.AddDays(1 - DateTime.Now.Day), Common.EnumDateTimeFormatted.ShortDateNumber);
                model.EndPushDT = Common.GetDateTimeFormattedText(DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(1).AddDays(-1), Common.EnumDateTimeFormatted.ShortDateNumber);
            }

            if (model.GetPushNotificationList(PageSize, CultureID) == false)
            {
                SetSystemErrorMessage(PubPushNotification.SystemMsg_GetPushNotificationList_Failure);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult PushNotification(PushNotificationModel model)
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
                            { PushNotificationModel.EnumCookieKey.UserID.ToString(), model.UserID },
                            { PushNotificationModel.EnumCookieKey.AppFunID.ToString(), model.AppFunID },
                            { PushNotificationModel.EnumCookieKey.Title.ToString(), model.Title },
                            { PushNotificationModel.EnumCookieKey.Body.ToString(), model.Body },
                            { PushNotificationModel.EnumCookieKey.StartPushDT.ToString(), model.StartPushDT },
                            { PushNotificationModel.EnumCookieKey.EndPushDT.ToString(), model.EndPushDT },
                            { PushNotificationModel.EnumCookieKey.PushStatus.ToString(), model.PushStatus },
                            { PushNotificationModel.EnumCookieKey.IncludeUnSent.ToString(), model.IncludeUnSent }
                        };

                    AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                    #endregion
                }
                else
                {
                    return View(model);
                }
            }
            else if (model.ExecAction == EnumActionType.Delete)
            {
                if (model.CancelPushMsg(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(PubPushNotification.SystemMsg_CancelPushMsg_Failure);
                }
            }

            if (model.GetPushNotificationList(PageSize, CultureID) == false)
            {
                SetSystemErrorMessage(PubPushNotification.SystemMsg_GetPushNotificationList_Failure);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("PushNotification")]
        public ActionResult PushNotificationErrorMsg(PushNotificationModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (model.GetPushNotificationErrorMsg() == false)
            {
                SetSystemErrorMessage(PubPushNotification.SystemMsg_GetPushNotificationErrorMsg_Failure);
            }

            return View(model);
        }
    }
}