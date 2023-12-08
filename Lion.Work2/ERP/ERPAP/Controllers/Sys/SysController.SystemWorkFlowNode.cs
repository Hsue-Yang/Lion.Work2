using System.Collections.Generic;
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
        public async Task<ActionResult> SystemWorkFlowNode()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            SystemWorkFlowNodeModel model = new SystemWorkFlowNodeModel();

            model.FormReset();

            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.SysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowNodeModel.EnumCookieKey.SysID.ToString());
                model.WFFlowGroupID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowNodeModel.EnumCookieKey.WFFlowGroupID.ToString());
                model.WFFlowID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowNodeModel.EnumCookieKey.WFFlowID.ToString());
                model.WFFlowVer = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowNodeModel.EnumCookieKey.WFFlowVer.ToString());
                model.WFCombineKey = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowNodeModel.EnumCookieKey.WFCombineKey.ToString());
            }
            #endregion

            model.GetSysWFTabList(_BaseAPModel.EnumTabAction.SystemWorkFlowNode);

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNode.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (await model.GetSystemWorkFlowGroupList(AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowGroup.SystemMsg_GetSystemWorkFlowGroupList);
            }

            if (await model.GetSysUserSystemWorkFlowIDs(AuthState.SessionData.UserID, model.SysID, base.CultureID, model.WFFlowGroupID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowGroup.SystemMsg_GetSystemWorkFlowGroupList);
            }

            model.SetSysParameter(AuthState.SessionData.UserID, CultureID);

            if (await model.GetSystemWorkFlowNodeList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNode.SystemMsg_GetSystemWorkFlowNodeList);
            }

            #region - Set Cookie -
            Dictionary<string, string> paraDict = new Dictionary<string, string>();
            paraDict.Add(SystemWorkFlowNodeModel.EnumCookieKey.SysID.ToString(), model.SysID);
            paraDict.Add(SystemWorkFlowNodeModel.EnumCookieKey.WFFlowGroupID.ToString(), model.WFFlowGroupID);
            paraDict.Add(SystemWorkFlowNodeModel.EnumCookieKey.WFFlowID.ToString(), model.WFFlowID);
            paraDict.Add(SystemWorkFlowNodeModel.EnumCookieKey.WFFlowVer.ToString(), model.WFFlowVer);
            paraDict.Add(SystemWorkFlowNodeModel.EnumCookieKey.WFCombineKey.ToString(), model.WFCombineKey);
            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemWorkFlowNode(SystemWorkFlowNodeModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            model.GetSysWFTabList(_BaseAPModel.EnumTabAction.SystemWorkFlowNode);

            if (await model.GetUserSystemByIdList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNode.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (await model.GetSystemWorkFlowGroupList(AuthState.SessionData.UserID, base.CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowGroup.SystemMsg_GetSystemWorkFlowGroupList);
            }

            if (await model.GetSysUserSystemWorkFlowIDs(AuthState.SessionData.UserID, model.SysID, base.CultureID, model.WFFlowGroupID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowGroup.SystemMsg_GetSystemWorkFlowGroupList);
            }

            model.SetSysParameter(AuthState.SessionData.UserID, CultureID);

            if (await model.GetSystemWorkFlowNodeList(AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNode.SystemMsg_GetSystemWorkFlowNodeList);
            }

            #region - Set Cookie -
            Dictionary<string, string> paraDict = new Dictionary<string, string>();
            paraDict.Add(SystemWorkFlowNodeModel.EnumCookieKey.SysID.ToString(), model.SysID);
            paraDict.Add(SystemWorkFlowNodeModel.EnumCookieKey.WFFlowGroupID.ToString(), model.WFFlowGroupID);
            paraDict.Add(SystemWorkFlowNodeModel.EnumCookieKey.WFFlowID.ToString(), model.WFFlowID);
            paraDict.Add(SystemWorkFlowNodeModel.EnumCookieKey.WFFlowVer.ToString(), model.WFFlowVer);
            paraDict.Add(SystemWorkFlowNodeModel.EnumCookieKey.WFCombineKey.ToString(), model.WFCombineKey);
            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            return View(model);
        }
    }
}
