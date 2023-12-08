using System.Collections.Generic;
using System.Web.Mvc;
using B2PAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public ActionResult UserSetting()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            UserSettingModel model = new UserSettingModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QueryUserID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, UserSettingModel.Field.QueryUserID.ToString());
                model.QueryUserNM = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, UserSettingModel.Field.QueryUserNM.ToString());
            }
            #endregion

            if (model.GetSystemUserDetail(AuthState.SessionData.UserID) == false)
            {
                SetSystemErrorMessage(SysUserSetting.SystemMsg_GetSystemUserDetail);
            }

            if (model.GetUserSettingList(AuthState.SessionData.UserID, base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysUserSetting.SystemMsg_GetUserSettingList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult UserSetting(UserSettingModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(UserSettingModel.Field.QueryUserID.ToString(), model.QueryUserID);
                paraDict.Add(UserSettingModel.Field.QueryUserNM.ToString(), model.QueryUserNM);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion

                if (model.GetSystemUserDetail(AuthState.SessionData.UserID) == false)
                {
                    SetSystemErrorMessage(SysUserSetting.SystemMsg_GetSystemUserDetail);
                }

                if (model.GetUserSettingList(AuthState.SessionData.UserID, base.PageSize, base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysUserSetting.SystemMsg_GetUserSettingList);
                }
            }

            return View(model);
        }
    }
}