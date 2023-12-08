using System.Collections.Generic;
using System.Web.Mvc;
using ERPAP.Models.Pub;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Pub;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class PubController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult MenuSetting()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            MenuSettingModel model = new MenuSettingModel();

            if (model.GetMenuSettingList(AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(PubMenuSetting.SystemMsg_GetMenuSettingList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult MenuSetting(MenuSettingModel model, List<EntityMenuSetting.MenuSettingValue> menuSettingValueList)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (base.IsPostBack && model.ExecAction == EnumActionType.Update)
            {
                if (model.GetEditMenuSettingResult(AuthState.SessionData.UserID, base.CultureID, menuSettingValueList) == false)
                {
                    SetSystemErrorMessage(PubMenuSetting.EditMenuSettingResult_Failure);
                }
                else
                {
                    var zh_TW = model.GenerateUserMenuXML(AuthState.SessionData.UserID, EnumCultureID.zh_TW);
                    var zh_CN = model.GenerateUserMenuXML(AuthState.SessionData.UserID, EnumCultureID.zh_CN);
                    var en_US = model.GenerateUserMenuXML(AuthState.SessionData.UserID, EnumCultureID.en_US);
                    var th_TH = model.GenerateUserMenuXML(AuthState.SessionData.UserID, EnumCultureID.th_TH);
                    var ja_JP = model.GenerateUserMenuXML(AuthState.SessionData.UserID, EnumCultureID.ja_JP);
                    var ko_KR = model.GenerateUserMenuXML(AuthState.SessionData.UserID, EnumCultureID.ko_KR);

                    GenerateUserMenu(AuthState.SessionData.UserID, zh_TW, zh_CN, en_US, th_TH, ja_JP, ko_KR);
                }
            }

            return RedirectToAction("MenuSetting", "Pub");
        }
    }
}