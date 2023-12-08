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
        public async Task<ActionResult> UserRoleFun()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            UserRoleFunModel model = new UserRoleFunModel();

            model.FormReset(AuthState.SessionData.UserID);

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QueryUserID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, UserRoleFunModel.Field.QueryUserID.ToString());
                model.QueryUserNM = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, UserRoleFunModel.Field.QueryUserNM.ToString());
            }
            #endregion

            model.GetSysUserSystemTabList(_BaseAPModel.EnumTabAction.SysUserRoleFun);

            if (await model.GetUserRoleFunList(base.PageSize) == false)
            {
                SetSystemErrorMessage(SysUserRoleFun.SystemMsg_GetUserRoleFunList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> UserRoleFun(UserRoleFunModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysUserSystemTabList(_BaseAPModel.EnumTabAction.SysUserRoleFun);

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(UserRoleFunModel.Field.QueryUserID.ToString(), model.QueryUserID);
                paraDict.Add(UserRoleFunModel.Field.QueryUserNM.ToString(), model.QueryUserNM);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion              

                if (await model.GetUserRoleFunList(base.PageSize) == false)
                {
                    SetSystemErrorMessage(SysUserRoleFun.SystemMsg_GetUserRoleFunList);
                }
            }

            return View(model);
        }
    }
}