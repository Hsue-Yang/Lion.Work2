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
        public async Task<ActionResult> SystemAPI()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemAPIModel model = new SystemAPIModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemAPIModel.Field.QuerySysID.ToString());
                model.QueryAPIGroupID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemAPIModel.Field.QueryAPIGroupID.ToString());
            }
            #endregion

            model.GetSysAPITabList(_BaseAPModel.EnumTabAction.SysSystemAPI);

            Task<bool> getAllSystemByIdList = model.GetAllSystemByIdList(AuthState.SessionData.UserID, true, CultureID);
            Task<bool> getSystemAPIGroupByIdList = model.GetSystemAPIGroupByIdList(model.QuerySysID, AuthState.SessionData.UserID, CultureID);
            Task<bool> getSystemAPIList = model.GetSystemAPIList(AuthState.SessionData.UserID, PageSize, CultureID);

            await Task.WhenAll(getAllSystemByIdList, getSystemAPIGroupByIdList, getSystemAPIList);

            if (getAllSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPI.SystemMsg_UnGetSystemByIdList);
            }

            if (getSystemAPIGroupByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPI.SystemMsg_UnGetSystemAPIGroupByIdList);
            }

            if (getSystemAPIList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPI.SystemMsg_UnGetSystemAPIList);
            }

            if (model.CheckIsITManager(AuthState.SessionData.UserID, model.QuerySysID) == false)
            {
                SetSystemErrorMessage(SysSystemAPI.SystemMsg_CheckIsITManager_Failure);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemAPI(SystemAPIModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            #region - Set Cookie -
            Dictionary<string, string> paraDict = new Dictionary<string, string>();
            paraDict.Add(SystemAPIModel.Field.QuerySysID.ToString(), model.QuerySysID);
            paraDict.Add(SystemAPIModel.Field.QueryAPIGroupID.ToString(), model.QueryAPIGroupID);
            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion
            
            model.GetSysAPITabList(_BaseAPModel.EnumTabAction.SysSystemAPI);

            Task<bool> getAllSystemByIdList = model.GetAllSystemByIdList(AuthState.SessionData.UserID, true, CultureID);
            Task<bool> getSystemAPIGroupByIdList = model.GetSystemAPIGroupByIdList(model.QuerySysID, AuthState.SessionData.UserID, CultureID);
            Task<bool> getSystemAPIList = model.GetSystemAPIList(AuthState.SessionData.UserID, PageSize, CultureID);

            await Task.WhenAll(getAllSystemByIdList, getSystemAPIGroupByIdList, getSystemAPIList);
            
            if (getAllSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPI.SystemMsg_UnGetSystemByIdList);
            }

            if (getSystemAPIGroupByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPI.SystemMsg_UnGetSystemAPIGroupByIdList);
            }
            
            if (getSystemAPIList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPI.SystemMsg_UnGetSystemAPIList);
            }

            if (model.CheckIsITManager(AuthState.SessionData.UserID, model.QuerySysID) == false)
            {
                SetSystemErrorMessage(SysResource.SystemMsg_CheckIsITManager_Failure);
            }

            return View(model);
        }
    }
}