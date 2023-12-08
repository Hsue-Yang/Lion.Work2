using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter("LineBotAccountSetting")]
        public async Task<ActionResult> LineBotReceiver(LineBotReceiverModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (model.CheckIsITManager(AuthState.SessionData.UserID, model.SysID) == false || model.IsITManager == false)
            {
                SetSystemErrorMessage(SysResource.SystemMsg_CheckIsITManager_Failure);
                return RedirectToAction("LineBotAccountSetting");
            }

            model.GetCMCodeDictionary(CultureID, Entity_BaseAP.EnumCMCodeItemTextType.CodeNMID, Entity_BaseAP.EnumCMCodeKind.LineReceiverSourceType);

            if (IsPostBack == false)
            {
                #region - Get Cookie -
                if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
                {
                    model.LineReceiverNM = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, nameof(model.LineReceiverNM));
                }
                #endregion
            }
            else
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict =
                    new Dictionary<string, string>
                    {
                        { nameof(model.LineReceiverNM), model.LineReceiverNM }
                    };

                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion
            }
            
            if (await model.FormInitialize(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysLineBotReceiver.SystemMsg_FormInitialize_Failure);
            }

            if (await model.GetLineBotReceiverList(PageSize, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysLineBotReceiver.SystemMsg_GetLineBotReceiverList_Failure);
            }

            return View(model);
        }
    }
}