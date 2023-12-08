using System.Web.Mvc;
using B2PAP.Models;
using B2PAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult SystemSetting()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemSettingModel model = new SystemSettingModel();

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemSetting);

            if (model.GetSystemSettingList(AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemSetting.SystemMsg_GetSystemSettingList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemSetting(SystemSettingModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemSetting);

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                if (model.GetSystemSettingList(AuthState.SessionData.UserID, base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemSetting.SystemMsg_GetSystemSettingList);
                }
            }

            return View(model);
        }
    }
}