using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;
using static ERPAP.Models._BaseAPModel;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> UserPermission()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            UserPermissionModel model = new UserPermissionModel();

            model.FormReset(AuthState.SessionData.UserID);

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QueryUserID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, UserPermissionModel.Field.QueryUserID.ToString());
                model.QueryUserNM = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, UserPermissionModel.Field.QueryUserNM.ToString());
                model.QueryRestrictType = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, UserPermissionModel.Field.QueryRestrictType.ToString());
            }
            #endregion

            if (await model.GetCMCodeTypeList(AuthState.SessionData.UserID, Common.GetEnumDesc(EnumCMCodeKind.RestrictType), base.CultureID) == false)
            {
                SetSystemErrorMessage(SysUserPermission.SystemMsg_GetBaseRestrictTypeList);
            }

            if (await model.GetUserPermissionList(base.PageSize, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysUserPermission.SystemMsg_GetUserPermissionList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> UserPermission(UserPermissionModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (base.IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(UserPermissionModel.Field.QueryUserID.ToString(), model.QueryUserID);
                paraDict.Add(UserPermissionModel.Field.QueryUserNM.ToString(), model.QueryUserNM);
                paraDict.Add(UserPermissionModel.Field.QueryRestrictType.ToString(), model.QueryRestrictType);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion

                if (await model.GetCMCodeTypeList(AuthState.SessionData.UserID, Common.GetEnumDesc(EnumCMCodeKind.RestrictType), base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysUserPermission.SystemMsg_GetBaseRestrictTypeList);
                }

                if (await model.GetUserPermissionList(base.PageSize, base.CultureID) == false)
                {
                    SetSystemErrorMessage(SysUserPermission.SystemMsg_GetUserPermissionList);
                }
            }

            return View(model);
        }
    }
}