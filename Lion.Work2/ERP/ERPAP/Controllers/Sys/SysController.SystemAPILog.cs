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
        public async Task<ActionResult> SystemAPILog()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemAPILogModel model = new SystemAPILogModel();

            model.FormReset();

            #region - Get Cookie -

            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemAPILogModel.Field.QuerySysID.ToString());
                model.QueryAPIGroupID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemAPILogModel.Field.QueryAPIGroupID.ToString());
                model.QueryAPIFunID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemAPILogModel.Field.QueryAPIFunID.ToString());
                model.QueryClientSysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemAPILogModel.Field.QueryClientSysID.ToString());
                model.APINo = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemAPILogModel.Field.APINo.ToString());
                model.BeginDate = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemAPILogModel.Field.BeginDate.ToString());
                model.EndDate = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemAPILogModel.Field.EndDate.ToString());
                model.BeginTime = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemAPILogModel.Field.BeginTime.ToString());
                model.EndTime = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemAPILogModel.Field.EndTime.ToString());
            }
            #endregion

            model.GetSysAPITabList(_BaseAPModel.EnumTabAction.SysSystemAPILog);

            Task<bool> getUserSystemByIdList = model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID);
            Task<bool> getAllSystemByIdList = model.GetAllSystemByIdList(AuthState.SessionData.UserID, false, CultureID);
            Task<bool> getSysSystemAPIGroupByIdList = model.GetSystemAPIGroupByIdList(model.QuerySysID, AuthState.SessionData.UserID, CultureID);
            Task<bool> getSysSystemAPIByIdList = model.GetSysSystemAPIByIdList(model.QuerySysID, AuthState.SessionData.UserID, model.QueryAPIGroupID, CultureID);

            await Task.WhenAll(getUserSystemByIdList, getAllSystemByIdList, getSysSystemAPIGroupByIdList, getSysSystemAPIByIdList);

            if (getUserSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPILog.SystemMsg_UnGetUserSystemByIdList);
            }

            if (getAllSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPILog.SystemMsg_UnGetSystemByIdList);
            }

            if (getSysSystemAPIGroupByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPILog.SystemMsg_UnGetSysSystemAPIGroupByIdList);
            }

            if (getSysSystemAPIByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemAPILog.SystemMsg_UnGetSysSystemAPIByIdList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemAPILog(SystemAPILogModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysAPITabList(_BaseAPModel.EnumTabAction.SysSystemAPILog);

            if (model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -

                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemAPILogModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemAPILogModel.Field.QueryAPIGroupID.ToString(), model.QueryAPIGroupID);
                paraDict.Add(SystemAPILogModel.Field.QueryAPIFunID.ToString(), model.QueryAPIFunID);
                paraDict.Add(SystemAPILogModel.Field.QueryClientSysID.ToString(), model.QueryClientSysID);
                paraDict.Add(SystemAPILogModel.Field.APINo.ToString(), model.APINo);
                paraDict.Add(SystemAPILogModel.Field.BeginDate.ToString(), model.BeginDate);
                paraDict.Add(SystemAPILogModel.Field.EndDate.ToString(), model.EndDate);
                paraDict.Add(SystemAPILogModel.Field.BeginTime.ToString(), model.BeginTime);
                paraDict.Add(SystemAPILogModel.Field.EndTime.ToString(), model.EndTime);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);

                #endregion

                Task<bool> getUserSystemByIdList = model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID);
                Task<bool> getAllSystemByIdList = model.GetAllSystemByIdList(AuthState.SessionData.UserID, false, CultureID);
                Task<bool> getSysSystemAPIGroupByIdList = model.GetSystemAPIGroupByIdList(model.QuerySysID, AuthState.SessionData.UserID, CultureID);
                Task<bool> getSysSystemAPIByIdList = model.GetSysSystemAPIByIdList(model.QuerySysID, AuthState.SessionData.UserID, model.QueryAPIGroupID, CultureID);
                Task<bool> getSystemAPILogList = model.GetSystemAPILogList(AuthState.SessionData.UserID, PageSize, CultureID);

                await Task.WhenAll(getUserSystemByIdList, getAllSystemByIdList, getSysSystemAPIGroupByIdList, getSysSystemAPIByIdList, getSystemAPILogList);

                if (getUserSystemByIdList.Result == false)
                {
                    SetSystemErrorMessage(SysSystemAPILog.SystemMsg_UnGetUserSystemByIdList);
                }

                if (getAllSystemByIdList.Result == false)
                {
                    SetSystemErrorMessage(SysSystemAPILog.SystemMsg_UnGetSystemByIdList);
                }

                if (getSysSystemAPIGroupByIdList.Result == false)
                {
                    SetSystemErrorMessage(SysSystemAPILog.SystemMsg_UnGetSysSystemAPIGroupByIdList);
                }

                if (getSysSystemAPIByIdList.Result == false)
                {
                    SetSystemErrorMessage(SysSystemAPILog.SystemMsg_UnGetSysSystemAPIByIdList);
                }

                if (getSystemAPILogList.Result == false)
                {
                    SetSystemErrorMessage(SysSystemAPILog.SystemMsg_UnGetSystemAPILogList);
                }
            }

            return View(model);
        }
    }
}