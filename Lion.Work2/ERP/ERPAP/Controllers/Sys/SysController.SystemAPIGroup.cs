using ERPAP.Models;
using ERPAP.Models.Sys;
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
        public async Task<ActionResult> SystemAPIGroup()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemAPIGroupModel model = new SystemAPIGroupModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemAPIGroupModel.Field.QuerySysID.ToString());
            }
            #endregion

            model.GetSysAPITabList(_BaseAPModel.EnumTabAction.SysSystemAPIGroup);

            Task<bool> getUserSystemByIdList = model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID);
            Task<bool> getSystemAPIGroupList = model.GetSystemAPIGroupList(AuthState.SessionData.UserID, PageSize, CultureID);

            await Task.WhenAll(getUserSystemByIdList, getSystemAPIGroupList);

            if (getUserSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPIGroup.SystemMsg_UnGetUserSystemByIdList);
            }

            if (getSystemAPIGroupList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPIGroup.SystemMsg_UnGetSystemAPIGroupList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemAPIGroup(SystemAPIGroupModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysAPITabList(_BaseAPModel.EnumTabAction.SysSystemAPIGroup);

            #region - Set Cookie -
            Dictionary<string, string> paraDict = new Dictionary<string, string>();
            paraDict.Add(SystemAPIGroupModel.Field.QuerySysID.ToString(), model.QuerySysID);
            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            Task<bool> getUserSystemByIdList = model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID);
            Task<bool> getSystemAPIGroupList = model.GetSystemAPIGroupList(AuthState.SessionData.UserID, PageSize, CultureID);

            await Task.WhenAll(getUserSystemByIdList, getSystemAPIGroupList);

            if (getUserSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPIGroup.SystemMsg_UnGetUserSystemByIdList);
            }
            
            if (getSystemAPIGroupList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPIGroup.SystemMsg_UnGetSystemAPIGroupList);
            }

            return View(model);
        }
    }
}