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
        public async Task<ActionResult> SystemWorkFlow()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            SystemWorkFlowModel model = new SystemWorkFlowModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.SysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowModel.EnumCookieKey.SysID.ToString());
                model.WFFlowGroupID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowModel.EnumCookieKey.WFFlowGroupID.ToString());
            }
            #endregion

            model.GetSysWFTabList(_BaseAPModel.EnumTabAction.SystemWorkFlow);

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlow.SystemMsg_GetSysUserSystemSysIDList);
            }

            await model.GetSystemWorkFlowGroupIDList(model.SysID, CultureID);

            if (await model.GetSystemWorkFlowList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlow.SystemMsg_GetSystemWorkFlowList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemWorkFlow(SystemWorkFlowModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.GetSysWFTabList(_BaseAPModel.EnumTabAction.SystemWorkFlow);

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlow.SystemMsg_GetSysUserSystemSysIDList);
            }

            await model.GetSystemWorkFlowGroupIDList(model.SysID, CultureID);

            if (await model.GetSystemWorkFlowList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlow.SystemMsg_GetSystemWorkFlowList);
            }

            if (IsPostBack && model.ExecAction == EnumActionType.Select)
            {
                #region - Set Cookie -
                Dictionary<string, string> paraDict = new Dictionary<string, string>();
                paraDict.Add(SystemWorkFlowModel.EnumCookieKey.SysID.ToString(), model.SysID);
                paraDict.Add(SystemWorkFlowModel.EnumCookieKey.WFFlowGroupID.ToString(), model.WFFlowGroupID);
                AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
                #endregion
            }

            return View(model);
        }
    }
}