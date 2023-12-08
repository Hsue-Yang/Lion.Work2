using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemRoleCondition()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            var model = new SystemRoleConditionModel();

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemRoleCondition);

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.SysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemRoleConditionModel.EnumCookieKey.SysID.ToString());
                model.ConditionID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemRoleConditionModel.EnumCookieKey.ConditionID.ToString());
                model.IncludeRoleID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemRoleConditionModel.EnumCookieKey.IncludeRoleID.ToString());
            }
            #endregion

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleCondition.SystemMsg_GetSysUserSystemSysIDList_Failure);
            }

            if (string.IsNullOrWhiteSpace(model.SysID))
            {
                var sysUserSystemSysId = model.EntityUserSystemSysIDList.FirstOrDefault();
                if (sysUserSystemSysId != null)
                {
                    model.SysID = sysUserSystemSysId.SysID;
                }
            }

            if (await model.GetSysSystemRoleIDList(model.SysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRole.SystemMsg_UnGetSystemRoleByIdList);
            }
            if (await model.GetSysSystemConditionIDList(model.SysID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleCondition.SystemMsg_GetSysSystemConditionIDList_Failure);
            }
            if (await model.GetSysSystemRoleConditionList(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleCondition.SystemMsg_GetSysSystemRoleConditionList_Failure);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemRoleCondition(SystemRoleConditionModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.GetSysSystemTabList(_BaseAPModel.EnumTabAction.SysSystemRoleCondition);

            #region - Set Cookie -
            Dictionary<string, string> paraDict =
                new Dictionary<string, string>
                {
                    { SystemRoleConditionModel.EnumCookieKey.SysID.ToString(), model.SysID },
                    { SystemRoleConditionModel.EnumCookieKey.ConditionID.ToString(), model.ConditionID },
                    { SystemRoleConditionModel.EnumCookieKey.IncludeRoleID.ToString(), model.IncludeRoleID }
                };

            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleCondition.SystemMsg_GetSysUserSystemSysIDList_Failure);
            }
            if (await model.GetSysSystemRoleIDList(model.SysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRole.SystemMsg_UnGetSystemRoleByIdList);
            }
            if (await model.GetSysSystemConditionIDList(model.SysID,CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleCondition.SystemMsg_GetSysSystemConditionIDList_Failure);
            }
            if (await model.GetSysSystemRoleConditionList(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemRoleCondition.SystemMsg_GetSysSystemRoleConditionList_Failure);
            }

            return View(model);
        }
    }
}