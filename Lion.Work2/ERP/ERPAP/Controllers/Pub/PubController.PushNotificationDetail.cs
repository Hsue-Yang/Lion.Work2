// 新增日期：2017-03-17
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System.Linq;
using System.Web.Mvc;
using ERPAP.Models.Pub;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class PubController
    {
        [HttpPost]
        [AuthorizationActionFilter("PushNotification")]
        public ActionResult PushNotificationDetail(PushNotificationDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (IsPostBack)
            {
                bool result = true;

                if (model.ExecAction == EnumActionType.Add)
                {
                    if (TryValidatableObject(model))
                    {
                        if (model.PushMessgae(AuthState.SessionData.UserID) == false)
                        {
                            SetSystemErrorMessage(PubPushNotificationDetail.SystemMsg_PushMessgae_Failure);
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }

                if (result)
                {
                    return RedirectToAction("PushNotification");
                }
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter("PushNotification")]
        public ActionResult GetPushMsgUserInfo(string condition)
        {
            PushNotificationDetailModel model = new PushNotificationDetailModel();

            var autoUserInfoList = model.GetAutoUserInfoList(condition);

            if (autoUserInfoList != null &&
                autoUserInfoList.Any())
            {
                return Json((from s in autoUserInfoList
                             select new
                             {
                                 UserID = s.UserID.GetValue(),
                                 UserNM = s.UserNM.GetValue()
                             }));
            }

            return Json(null);
        }
    }
}