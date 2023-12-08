using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemWorkFlowGroup()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            SystemWorkFlowGroupModel model = new SystemWorkFlowGroupModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.SysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowGroupModel.EnumCookieKey.SysID.ToString());
            }
            #endregion

            model.GetSysWFTabList(_BaseAPModel.EnumTabAction.SystemWorkFlowGroup);

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowGroup.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (await model.GetSystemWorkFlowGroupList(AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowGroup.SystemMsg_GetSystemWorkFlowGroupList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemWorkFlowGroup(SystemWorkFlowGroupModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.GetSysWFTabList(_BaseAPModel.EnumTabAction.SystemWorkFlowGroup);

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowGroup.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (await model.GetSystemWorkFlowGroupList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowGroup.SystemMsg_GetSystemWorkFlowGroupList);
            }

            if (IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict =
                    new Dictionary<string, string>
                    {
                        { SystemWorkFlowGroupModel.EnumCookieKey.SysID.ToString(), model.SysID }
                    };
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion
            }

            return View(model);
        }
    }
}