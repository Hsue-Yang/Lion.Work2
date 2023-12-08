// 新增日期：2017-01-06
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System.Web.Mvc;
using ERPAP.Models.Pub;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class PubController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult AppUserMobile()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            AppUserMobileModel model = new AppUserMobileModel();

            if (model.GetAppUserMobileList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(PubAppUserMobile.SystemMsg_GetAppUserMobileList_Failure);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult AppUserMobile(AppUserMobileModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Update)
                {
                    if (model.UpdateAppUserDevice(AuthState.SessionData.UserID) == false)
                    {
                        SetSystemErrorMessage(PubAppUserMobile.SystemMsg_UpdateAppUserDevice_Failure);
                    }
                }
            }

            if (model.GetAppUserMobileList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(PubAppUserMobile.SystemMsg_GetAppUserMobileList_Failure);
            }

            return View(model);
        }
    }
}