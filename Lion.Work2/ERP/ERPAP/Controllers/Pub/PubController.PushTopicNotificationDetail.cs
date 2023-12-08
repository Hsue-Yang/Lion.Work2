using System.Web.Mvc;
using ERPAP.Models.Pub;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class PubController
    {
        [HttpPost]
        [AuthorizationActionFilter("PushTopicNotification")]
        public ActionResult PushTopicNotificationDetail(PushTopicNotificationDetailModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            bool result = true;
            if (IsPostBack && model.ExecAction == EnumActionType.Add)
            {
                if (TryValidatableObject(model) && model.PushTopicMessgae(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(PubPushTopicNotificationDetail.SystemMsg_PushMessgae_Failure);
                    result = false;
                }
            }
            else
            {
                return View(model);
            }

            if (result)
            {
                SetSystemAlertMessage(PubPushTopicNotificationDetail.SystemMsg_PushMessage_Success);
            }

            return RedirectToAction("PushTopicNotification");
        }
    }
}