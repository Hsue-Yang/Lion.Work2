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
        public async Task<ActionResult> SystemEventEDI()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemEventEDIModel model = new SystemEventEDIModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEventEDIModel.Field.QuerySysID.ToString());
                model.QueryTargetSysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEventEDIModel.Field.QueryTargetSysID.ToString());
                model.QueryEventGroupID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEventEDIModel.Field.QueryEventGroupID.ToString());
                model.QueryEventID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEventEDIModel.Field.QueryEventID.ToString());
                model.DTBegin = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEventEDIModel.Field.DTBegin.ToString());
                model.DTEnd = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEventEDIModel.Field.DTEnd.ToString());
                model.IsOnlyFail = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemEventEDIModel.Field.IsOnlyFail.ToString());
            }
            #endregion

            model.GetSysEventTabList(_BaseAPModel.EnumTabAction.SysSystemEventEDI);

            Task<bool> getAllSystemByIdList = model.GetAllSystemByIdList(AuthState.SessionData.UserID, false, CultureID);
            Task<bool> getSysSystemEventGroupByIdList = model.GetSysSystemEventGroupByIdList(model.QuerySysID, AuthState.SessionData.UserID, CultureID);
            Task<bool> getSysSystemEventByIdList = model.GetSysSystemEventByIdList(model.QuerySysID, AuthState.SessionData.UserID, model.QueryEventGroupID, CultureID);
            Task<bool> getSystemEventEDIList = model.GetSystemEventEDIList(AuthState.SessionData.UserID, PageSize, CultureID);

            await Task.WhenAll(getAllSystemByIdList, getSysSystemEventGroupByIdList, getSysSystemEventByIdList, getSystemEventEDIList);

            if (getAllSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEvent.SystemMsg_UnGetSystemByIdList);
            }

            if (getSysSystemEventGroupByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEvent.SystemMsg_UnGetSysSystemEventGroupByIdList);
            }

            if (getSysSystemEventByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventEDI.SystemMsg_UnGetSysSystemEventByIdList);
            }

            if (getSystemEventEDIList.Result == false)
            {
                SetSystemErrorMessage(SysSystemEventEDI.SystemMsg_UnGetSystemEDIEventList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemEventEDI(SystemEventEDIModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysEventTabList(_BaseAPModel.EnumTabAction.SysSystemEventEDI);

            if (IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemEventEDIModel.Field.QuerySysID.ToString(), model.QuerySysID);
                paraDict.Add(SystemEventEDIModel.Field.QueryTargetSysID.ToString(), model.QueryTargetSysID);
                paraDict.Add(SystemEventEDIModel.Field.QueryEventGroupID.ToString(), model.QueryEventGroupID);
                paraDict.Add(SystemEventEDIModel.Field.QueryEventID.ToString(), model.QueryEventID);
                paraDict.Add(SystemEventEDIModel.Field.DTBegin.ToString(), model.DTBegin);
                paraDict.Add(SystemEventEDIModel.Field.DTEnd.ToString(), model.DTEnd);
                paraDict.Add(SystemEventEDIModel.Field.IsOnlyFail.ToString(), model.IsOnlyFail);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion

                Task<bool> getAllSystemByIdList = model.GetAllSystemByIdList(AuthState.SessionData.UserID, false, CultureID);
                Task<bool> getSysSystemEventGroupByIdList = model.GetSysSystemEventGroupByIdList(model.QuerySysID, AuthState.SessionData.UserID, CultureID);
                Task<bool> getSysSystemEventByIdList = model.GetSysSystemEventByIdList(model.QuerySysID, AuthState.SessionData.UserID, model.QueryEventGroupID, CultureID);
                Task<bool> getSystemEventEDIList = model.GetSystemEventEDIList(AuthState.SessionData.UserID, PageSize, CultureID);

                await Task.WhenAll(getAllSystemByIdList, getSysSystemEventGroupByIdList, getSysSystemEventByIdList, getSystemEventEDIList);

                if (getAllSystemByIdList.Result == false)
                {
                    SetSystemErrorMessage(SysSystemEvent.SystemMsg_UnGetSystemByIdList);
                }

                if (getSysSystemEventGroupByIdList.Result == false)
                {
                    SetSystemErrorMessage(SysSystemEvent.SystemMsg_UnGetSysSystemEventGroupByIdList);
                }

                if (getSysSystemEventByIdList.Result == false)
                {
                    SetSystemErrorMessage(SysSystemEventEDI.SystemMsg_UnGetSysSystemEventByIdList);
                }

                if (getSystemEventEDIList.Result == false)
                {
                    SetSystemErrorMessage(SysSystemEventEDI.SystemMsg_UnGetSystemEDIEventList);
                }
            }
            return View(model);
        }
    }
}