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
        public async Task<ActionResult> SystemEventGroup()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemEventGroupModel model = new SystemEventGroupModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEventGroupModel.Field.QuerySysID.ToString());
                model.QueryEventGroupID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEventGroupModel.Field.QueryEventGroupID.ToString());
            }
            #endregion

            model.GetSysEventTabList(_BaseAPModel.EnumTabAction.SysSystemEventGroup);

            Task<bool> getUserSystemByIdList = model.GetUserSystemByIdList(AuthState.SessionData.UserID, false, CultureID);
            Task<bool> getSysSystemEventGroupByIdList = model.GetSysSystemEventGroupByIdList(model.QuerySysID, AuthState.SessionData.UserID, CultureID);
            Task<bool> getSystemEventGroupList = model.GetSystemEventGroupList(AuthState.SessionData.UserID, PageSize, CultureID);

            await Task.WhenAll(getUserSystemByIdList, getSysSystemEventGroupByIdList, getSystemEventGroupList);

            if (getUserSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventGroup.SystemMsg_UnGetUserSystemByIdList);
            }

            if (getSysSystemEventGroupByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventGroup.SystemMsg_UnGetSysSystemEventGroupByIdList);
            }

            if (getSystemEventGroupList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventGroup.SystemMsg_UnGetSystemEventGroupList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemEventGroup(SystemEventGroupModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            #region - Set Cookie -
            Dictionary<string, string> paraDict = new Dictionary<string, string>();
            paraDict.Add(SystemEventGroupModel.Field.QuerySysID.ToString(), model.QuerySysID);
            paraDict.Add(SystemEventGroupModel.Field.QueryEventGroupID.ToString(), model.QueryEventGroupID);
            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion
            
            model.GetSysEventTabList(_BaseAPModel.EnumTabAction.SysSystemEventGroup);

            Task<bool> getUserSystemByIdList = model.GetUserSystemByIdList(AuthState.SessionData.UserID, false, CultureID);
            Task<bool> getSysSystemEventGroupByIdList = model.GetSysSystemEventGroupByIdList(model.QuerySysID, AuthState.SessionData.UserID, CultureID);
            Task<bool> getSystemEventGroupList = model.GetSystemEventGroupList(AuthState.SessionData.UserID, PageSize, CultureID);

            await Task.WhenAll(getUserSystemByIdList, getSysSystemEventGroupByIdList, getSystemEventGroupList);

            if (getUserSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventGroup.SystemMsg_UnGetUserSystemByIdList);
            }

            if (getSysSystemEventGroupByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventGroup.SystemMsg_UnGetSysSystemEventGroupByIdList);
            }

            if (getSystemEventGroupList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventGroup.SystemMsg_UnGetSystemEventGroupList);
            }

            return View(model);
        }
    }
}