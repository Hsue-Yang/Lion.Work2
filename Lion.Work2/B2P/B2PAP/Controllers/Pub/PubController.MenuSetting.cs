using System.Collections.Generic;
using System.Web.Mvc;
using B2PAP.Models.Pub;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Pub;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
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
                    string filePath = base.GetUserMenuFilePath(AuthState.SessionData.UserID, EnumCultureID.zh_TW);
                    bool generateUserMenuXMLResult = model.GenerateUserMenuXML(AuthState.SessionData.UserID, filePath, EnumCultureID.zh_TW);

                    if (generateUserMenuXMLResult)
                    {
                        filePath = base.GetUserMenuFilePath(AuthState.SessionData.UserID, EnumCultureID.zh_CN);
                        generateUserMenuXMLResult = model.GenerateUserMenuXML(AuthState.SessionData.UserID, filePath, EnumCultureID.zh_CN);
                    }
                    if (generateUserMenuXMLResult)
                    {
                        filePath = base.GetUserMenuFilePath(AuthState.SessionData.UserID, EnumCultureID.en_US);
                        generateUserMenuXMLResult = model.GenerateUserMenuXML(AuthState.SessionData.UserID, filePath, EnumCultureID.en_US);
                    }
                    if (generateUserMenuXMLResult)
                    {
                        filePath = base.GetUserMenuFilePath(AuthState.SessionData.UserID, EnumCultureID.th_TH);
                        generateUserMenuXMLResult = model.GenerateUserMenuXML(AuthState.SessionData.UserID, filePath, EnumCultureID.th_TH);
                    }
                    if (generateUserMenuXMLResult)
                    {
                        filePath = base.GetUserMenuFilePath(AuthState.SessionData.UserID, EnumCultureID.ja_JP);
                        generateUserMenuXMLResult = model.GenerateUserMenuXML(AuthState.SessionData.UserID, filePath, EnumCultureID.ja_JP);
                    }

                    if (!generateUserMenuXMLResult)
                    {
                        base.SetSystemAlertMessage(PubMenuSetting.SystemMsg_GenerateFailure);
                    }
                }
            }

            return RedirectToAction("MenuSetting", "Pub");
        }
    }
}