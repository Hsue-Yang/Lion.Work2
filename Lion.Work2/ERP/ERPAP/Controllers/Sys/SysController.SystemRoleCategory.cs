using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemRoleCategory()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemRoleCategoryModel model = new SystemRoleCategoryModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.SysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemRoleCategoryModel.Field.SysID.ToString());
                model.RoleCategoryNM = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemRoleCategoryModel.Field.RoleCategoryNM.ToString());
            }
            #endregion

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleCategory.SystemMsg_UnGetUserSystemByIdList);
            }

            if (await model.GetSystemRoleCategoryList(AuthState.SessionData.UserID, CultureID) == false)            
            {
                SetSystemErrorMessage(SysSystemRoleCategory.SystemMsg_UnGetSystemRoleCategoryList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemRoleCategory(SystemRoleCategoryModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleCategory.SystemMsg_UnGetUserSystemByIdList);
            }

            if (await model.GetSystemRoleCategoryList(AuthState.SessionData.UserID, CultureID) == false)     
            {
                SetSystemErrorMessage(SysSystemRoleCategory.SystemMsg_UnGetSystemRoleCategoryList);
            }

            #region - Set Cookie -
            Dictionary<string, string> paraDict = new Dictionary<string, string>();
            paraDict.Add(SystemRoleCategoryModel.Field.SysID.ToString(), model.SysID);
            paraDict.Add(SystemRoleCategoryModel.Field.RoleCategoryNM.ToString(), model.RoleCategoryNM);
            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            return View(model);
        }
    }
}