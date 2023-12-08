using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemFunMenu()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemFunMenuModel model = new SystemFunMenuModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunMenuModel.Field.QuerySysID.ToString());
            }
            #endregion

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemFunMenu);

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunMenu.SystemMsg_UnGetUserSystemByIdList);
            }

            if (await model.GetSystemFunMenuList(AuthState.SessionData.UserID, PageSize, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunMenu.SystemMsg_UnGetSystemFunMenuList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemFunMenu(SystemFunMenuModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemFunMenu);

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFunMenu.SystemMsg_UnGetUserSystemByIdList);
            }

            if (IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemFunMenuModel.Field.QuerySysID.ToString(), model.QuerySysID);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion

                if (await model.GetSystemFunMenuList(AuthState.SessionData.UserID, PageSize, CultureID) == false)
                {
                    SetSystemErrorMessage(SysSystemFunMenu.SystemMsg_UnGetSystemFunMenuList);
                }
            }

            return View(model);
        }
    }
}