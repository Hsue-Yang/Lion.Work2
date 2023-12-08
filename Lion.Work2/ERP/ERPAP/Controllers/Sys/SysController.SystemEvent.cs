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
        public async Task<ActionResult> SystemEvent()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemEventModel model = new SystemEventModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEventModel.Field.QuerySysID.ToString());
                model.QueryEventGroupID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEventModel.Field.QueryEventGroupID.ToString());
            }
            #endregion

            model.GetSysEventTabList(_BaseAPModel.EnumTabAction.SysSystemEvent);

            Task<bool> getAllSystemByIdList = model.GetAllSystemByIdList(AuthState.SessionData.UserID, false, CultureID);
            Task<bool> getSysSystemEventGroupByIdList = model.GetSysSystemEventGroupByIdList(model.QuerySysID, AuthState.SessionData.UserID, CultureID);
            Task<bool> getSystemEventList = model.GetSystemEventList(AuthState.SessionData.UserID, PageSize, CultureID);

            await Task.WhenAll(getAllSystemByIdList, getSysSystemEventGroupByIdList, getSystemEventList);

            if (getAllSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEvent.SystemMsg_UnGetSystemByIdList);
            }

            if (getSysSystemEventGroupByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEvent.SystemMsg_UnGetSysSystemEventGroupByIdList);
            }

            if (model.CheckIsITManager(AuthState.SessionData.UserID, model.QuerySysID) == false)
            {
                SetSystemErrorMessage(SysResource.SystemMsg_CheckIsITManager_Failure);
            }

            if (getSystemEventList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEvent.SystemMsg_UnGetSystemEventList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemEvent(SystemEventModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            #region - Set Cookie -
            Dictionary<string, string> paraDict = new Dictionary<string, string>();
            paraDict.Add(SystemEventModel.Field.QuerySysID.ToString(), model.QuerySysID);
            paraDict.Add(SystemEventModel.Field.QueryEventGroupID.ToString(), model.QueryEventGroupID);
            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            model.GetSysEventTabList(_BaseAPModel.EnumTabAction.SysSystemEvent);

            Task<bool> getAllSystemByIdList = model.GetAllSystemByIdList(AuthState.SessionData.UserID, false, CultureID);
            Task<bool> getSysSystemEventGroupByIdList = model.GetSysSystemEventGroupByIdList(model.QuerySysID, AuthState.SessionData.UserID, CultureID);
            Task<bool> getSystemEventList = model.GetSystemEventList(AuthState.SessionData.UserID, PageSize, CultureID);

            await Task.WhenAll(getAllSystemByIdList, getSysSystemEventGroupByIdList, getSystemEventList);

            if (getAllSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEvent.SystemMsg_UnGetSystemByIdList);
            }

            if (getSysSystemEventGroupByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEvent.SystemMsg_UnGetSysSystemEventGroupByIdList);
            }

            if (model.CheckIsITManager(AuthState.SessionData.UserID, model.QuerySysID) == false)
            {
                SetSystemErrorMessage(SysResource.SystemMsg_CheckIsITManager_Failure);
            }

            if (getSystemEventList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEvent.SystemMsg_UnGetSystemEventList);
            }

            return View(model);
        }
    }
}