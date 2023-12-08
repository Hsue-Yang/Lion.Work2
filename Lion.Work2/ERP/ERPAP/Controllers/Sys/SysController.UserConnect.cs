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
        public async Task<ActionResult> UserConnect()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            UserConnectModel model = new UserConnectModel();

            model.GetConnectLogTabList(_BaseAPModel.EnumTabAction.SysUserConnect);

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.DateBegin = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, UserConnectModel.Field.DateBegin.ToString());
                model.TimeBegin = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, UserConnectModel.Field.TimeBegin.ToString());
                model.DateEnd = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, UserConnectModel.Field.DateEnd.ToString());
                model.TimeEnd = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, UserConnectModel.Field.TimeEnd.ToString());
            }
            #endregion

            if (await model.GetUserConnectList(AuthState.SessionData.UserID, base.PageSize) == false)
            {
                SetSystemErrorMessage(SysUserConnect.SystemMsg_GetUserConnectList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> UserConnect(UserConnectModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetConnectLogTabList(_BaseAPModel.EnumTabAction.SysUserConnect);

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(UserConnectModel.Field.DateBegin.ToString(), model.DateBegin);
                paraDict.Add(UserConnectModel.Field.DateEnd.ToString(), model.DateEnd);
                paraDict.Add(UserConnectModel.Field.TimeBegin.ToString(), model.TimeBegin);
                paraDict.Add(UserConnectModel.Field.TimeEnd.ToString(), model.TimeEnd);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion

                if (await model.GetUserConnectList(AuthState.SessionData.UserID, base.PageSize) == false)
                {
                    SetSystemErrorMessage(SysUserConnect.SystemMsg_GetUserConnectList);
                }
            }

            return View(model);
        }
    }
}