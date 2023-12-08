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
        public async Task<ActionResult> SystemFun()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemFunModel model = new SystemFunModel();

            model.FormReset();
            
            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunModel.Field.QuerySysID.ToString());
                model.QuerySubSysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunModel.Field.QuerySubSysID.ToString());
                model.QueryFunControllerID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunModel.Field.QueryFunControllerID.ToString());
                model.QueryFunGroupNM = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunModel.Field.QueryFunGroupNM.ToString());
                model.QueryFunActionName = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunModel.Field.QueryFunActionName.ToString());
                model.QueryFunNM = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunModel.Field.QueryFunName.ToString());
                model.QueryFunMenuSysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunModel.Field.QueryFunMenuSysID.ToString());
                model.QueryFunMenu = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemFunModel.Field.QueryFunMenu.ToString());
            }
            #endregion

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemFun);

            Task<bool> getSystemInfoList = model.GetSystemInfoList(AuthState.SessionData.UserID, CultureID);
            Task<bool> getAllSystemByIdList = model.GetAllSystemByIdList(AuthState.SessionData.UserID, true, CultureID);
            Task<bool> getSystemFunMenuByIdList = model.GetSystemFunMenuByIdList(model.QueryFunMenuSysID, AuthState.SessionData.UserID, CultureID);
            Task<bool> getSystemPurviewByIdList = model.GetSystemPurviewByIdList(model.QuerySysID, AuthState.SessionData.UserID, CultureID);
            Task<bool> getSystemFunList = model.GetSystemFunList(AuthState.SessionData.UserID, PageSize, CultureID);
            
            await Task.WhenAll(getSystemInfoList, getAllSystemByIdList, getSystemFunMenuByIdList,
                getSystemPurviewByIdList, getSystemFunList);

            if (getSystemInfoList.Result == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSystemInfoList_Failure);
            }

            if (getAllSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_UnGetAllSystemByIdList);
            }

            if (getSystemFunMenuByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_UnGetSystemFunMenuByIdList);
            }

            if (getSystemPurviewByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_UnGetSystemPurviewByIdList);
            }

            if (getSystemFunList.Result == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_UnGetSystemFunList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemFun(SystemFunModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemFun);

            Task<bool> getSystemInfoList = model.GetSystemInfoList(AuthState.SessionData.UserID, CultureID);
            Task<bool> getAllSystemByIdList = model.GetAllSystemByIdList(AuthState.SessionData.UserID, true, CultureID);
            Task<bool> getSystemFunMenuByIdList = model.GetSystemFunMenuByIdList(model.QueryFunMenuSysID, AuthState.SessionData.UserID, CultureID);
            Task<bool> getSystemPurviewByIdList = model.GetSystemPurviewByIdList(model.QuerySysID, AuthState.SessionData.UserID, CultureID);
            
            await Task.WhenAll(getSystemInfoList, getAllSystemByIdList, getSystemFunMenuByIdList,
                getSystemPurviewByIdList);

            if (getSystemInfoList .Result == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_GetSystemInfoList_Failure);
            }

            if (getAllSystemByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_UnGetAllSystemByIdList);
            }

            if (getSystemFunMenuByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_UnGetSystemFunMenuByIdList);
            }

            if (getSystemPurviewByIdList.Result == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_UnGetSystemPurviewByIdList);
            }

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Update)
                {
                    SystemFunModel.EnumEditSystemFunByPurviewResult result = await model.GetEditSystemFunByPurviewResult(AuthState.SessionData.UserID);

                    if (result == SystemFunModel.EnumEditSystemFunByPurviewResult.Failure)
                    {
                        SetSystemErrorMessage(SysSystemFun.EditSystemFunByPurviewResult_Failure);
                    }
                    else if (result == SystemFunModel.EnumEditSystemFunByPurviewResult.Success && GetEDIServiceDistributor())
                    {
                        bool execEDIResult = true;
                        foreach (string pickData in model.PickList)
                        {
                            string eventParaJsonString = await model.GetEventParaSysSystemFunEdit(AuthState.SessionData.UserID, pickData, CultureID);
                            if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemFun, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                            {
                                execEDIResult = false;
                            }
                        }

                        if (execEDIResult)
                        {
                            SetSystemAlertMessage(SysSystemFun.EditSystemFunByPurviewResult_Success);
                        }
                        else
                        {
                            SetSystemErrorMessage(SysSystemFun.EditSystemFunByPurviewResult_Failure);
                        }
                    }
                    else if (result == SystemFunModel.EnumEditSystemFunByPurviewResult.NotExecuted)
                    {
                        SetSystemErrorMessage(SysSystemFun.EditSystemFunByPurviewResult_NotExecuted);
                    }
                }

                if (model.ExecAction == EnumActionType.Select)
                {
                    #region - Set Cookie -
                    Dictionary<string, string> paraDict = new Dictionary<string, string>();
                    paraDict.Add(SystemFunModel.Field.QuerySysID.ToString(), model.QuerySysID);
                    paraDict.Add(SystemFunModel.Field.QuerySubSysID.ToString(), model.QuerySubSysID);
                    paraDict.Add(SystemFunModel.Field.QueryFunMenuSysID.ToString(), model.QueryFunMenuSysID);
                    paraDict.Add(SystemFunModel.Field.QueryFunMenu.ToString(), model.QueryFunMenu);
                    paraDict.Add(SystemFunModel.Field.QueryFunControllerID.ToString(), model.QueryFunControllerID);
                    paraDict.Add(SystemFunModel.Field.QueryFunActionName.ToString(), model.QueryFunActionName);
                    paraDict.Add(SystemFunModel.Field.QueryFunGroupNM.ToString(), model.QueryFunGroupNM);
                    paraDict.Add(SystemFunModel.Field.QueryFunName.ToString(), model.QueryFunNM);

                    AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                    #endregion
                }
            }

            if (await model.GetSystemFunList(AuthState.SessionData.UserID, PageSize, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemFun.SystemMsg_UnGetSystemFunList);
            }

            return View(model);
        }
    }
}