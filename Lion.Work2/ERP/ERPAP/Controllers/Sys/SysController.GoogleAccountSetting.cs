using System.Collections.Generic;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult GoogleAccountSetting()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            GoogleAccountSettingModel model = new GoogleAccountSettingModel();

            model.FormReset();

            model.GetGoogleAccountSettingTabList(_BaseAPModel.EnumTabAction.GoogleAccountSetting);

            if (model.GetGoogleAccountSettingList(base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysGoogleAccountSetting.SystemMsg_GoogleAccountSettingList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult GoogleAccountSetting(GoogleAccountSettingModel model, List<EntityGoogleAccountSetting.GoogleAccountSettingValue> googleAccountValueList)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetGoogleAccountSettingTabList(_BaseAPModel.EnumTabAction.GoogleAccountSetting);

            if (base.IsPostBack && model.ExecAction == EnumActionType.Update)
            {
                if (model.GetEditGoogleAccountSettingResult(AuthState.SessionData.UserID, base.CultureID, googleAccountValueList) == false)
                {
                    SetSystemErrorMessage(SysGoogleAccountSetting.SystemMsg_GoogleAccountSettingList);
                }
                return RedirectToAction("GoogleAccountSetting", "Sys");
            }
            else
            {
                if (model.GetGoogleAccountSettingList(base.PageSize, base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysGoogleAccountSetting.SystemMsg_GoogleAccountSettingList);
                }
                return View(model);
            }
        }
    }
}