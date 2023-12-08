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
        public async Task<ActionResult> SystemRole()
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            SystemRoleModel model = new SystemRoleModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.QuerySysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemRoleModel.Field.QuerySysID.ToString());
                model.QueryRoleCategoryID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemRoleModel.Field.QueryRoleCategoryID.ToString());
            }
            #endregion

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemRole);

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRole.SystemMsg_UnGetUserSystemByIdList);
            }

            if (await model.GetSystemRoleCategoryByIdList(model.QuerySysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRole.SystemMsg_UnGetSystemRoleCategoryByIdList);
            }

            if (await model.GetSystemRoleList(AuthState.SessionData.UserID, PageSize, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRole.SystemMsg_UnGetSystemRoleList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemRole(SystemRoleModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemRole);

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRole.SystemMsg_UnGetUserSystemByIdList);
            }

            if (await model.GetSystemRoleCategoryByIdList(model.QuerySysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRole.SystemMsg_UnGetSystemRoleCategoryByIdList);
            }

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Update)
                {
                    SystemRoleModel.EnumEditSystemRoleByCategoryResult editSystemRoleByCategoryResult = await model.GetEditSystemRoleByCategoryResult(AuthState.SessionData.UserID, CultureID);

                    if (editSystemRoleByCategoryResult == SystemRoleModel.EnumEditSystemRoleByCategoryResult.Success)
                    {
                        model.RoleCategoryID = string.Empty;

                        if (GetEDIServiceDistributor())
                        {
                            bool execEDIResult = true;
                            foreach (string pickData in model.PickList)
                            {
                                string eventParaJsonString = model.GetEventParaSysSystemRoleEdit(pickData, AuthState.SessionData.UserID);
                                if (ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysSystemRole, EnumEDIServiceEventID.Edit, eventParaJsonString) == null)
                                {
                                    execEDIResult = false;
                                }
                            }

                            if (execEDIResult)
                            {
                                SetSystemAlertMessage(SysSystemRole.EditSystemRoleByCategoryResult_Success);
                            }
                            else
                            {
                                SetSystemErrorMessage(SysSystemRole.EditEDISystemRole_Failure);
                            }
                        }
                    }
                    else if (editSystemRoleByCategoryResult == SystemRoleModel.EnumEditSystemRoleByCategoryResult.NotExecuted)
                    {
                        SetSystemErrorMessage(SysSystemRole.EditSystemRoleByCategoryResult_NotExecuted);
                    }
                    else
                    {
                        SetSystemErrorMessage(SysSystemRole.EditSystemRoleByCategoryResult_Failure);
                    }
                }

                if (model.ExecAction == EnumActionType.Select)
                {
                    #region - Set Cookie -
                    Dictionary<string, string> paraDict = new Dictionary<string, string>();
                    paraDict.Add(SystemRoleModel.Field.QuerySysID.ToString(), model.QuerySysID);
                    paraDict.Add(SystemRoleModel.Field.QueryRoleCategoryID.ToString(), model.QueryRoleCategoryID);
                    AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                    #endregion
                }
            }

            if (await model.GetSystemRoleList(AuthState.SessionData.UserID, PageSize, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRole.SystemMsg_UnGetSystemRoleList);
            }

            return View(model);
        }
    }
}