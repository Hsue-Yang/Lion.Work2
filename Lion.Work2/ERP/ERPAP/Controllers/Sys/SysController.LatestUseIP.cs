// 新增日期：2018-01-23
// 新增人員：廖先駿
// 新增內容：IP最後使用
// ---------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> LatestUseIP()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            var model = new LatestUseIPModel();

            model.GetConnectLogTabList(_BaseAPModel.EnumTabAction.SysLatestUseIP);

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.IPAddresss = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, LatestUseIPModel.EnumCookieKey.IPAddresss.ToString());
            }
            #endregion

            if(await model.GetLatestUseIPInfoList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysLatestUseIP.SystemMsg_GetLatestUseIPInfo_Failure);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> LatestUseIP(LatestUseIPModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.GetConnectLogTabList(_BaseAPModel.EnumTabAction.SysLatestUseIP);

            if (model.ExecAction == EnumActionType.Select)
            {
                if (TryValidatableObject(model))
                {
                    #region - Set Cookie -
                    Dictionary<string, string> paraDict =
                        new Dictionary<string, string>
                        {
                            { LatestUseIPModel.EnumCookieKey.IPAddresss.ToString(), model.IPAddresss }
                        };

                    AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                    #endregion
                }
                else
                {
                    return View(model);
                }
            }

            if (await model.GetLatestUseIPInfoList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysLatestUseIP.SystemMsg_GetLatestUseIPInfo_Failure);
            }

            return View(model);
        }
    }
}