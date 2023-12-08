using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP.Sys;
using LionTech.WorkFlow;
using Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemWorkFlowNext()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            SystemWorkFlowNextModel model = new SystemWorkFlowNextModel();
            
            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.SysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowNextModel.EnumCookieKey.SysID.ToString());
                model.WFFlowGroupID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowNextModel.EnumCookieKey.WFFlowGroupID.ToString());
                model.WFCombineKey = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowNextModel.EnumCookieKey.WFCombineKey.ToString());
                model.WFFlowID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowNextModel.EnumCookieKey.WFFlowID.ToString());
                model.WFFlowVer = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowNextModel.EnumCookieKey.WFFlowVer.ToString());
                model.WFNodeID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowNextModel.EnumCookieKey.WFNodeID.ToString());
            }
            #endregion

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNext.SystemMsg_GetSysUserSystemSysIDList);
            }

            await model.GetSystemWorkFlowGroupIDList(model.SysID, CultureID);

            await model.GetUserSystemWorkFlowIDList(AuthState.SessionData.UserID, model.SysID, model.WFFlowGroupID, CultureID);

            if (model.EntityUserSystemWorkFlowIDList != null)
            {
                foreach (SysModel.UserSystemWorkFlowIDs row in model.EntityUserSystemWorkFlowIDList)
                {
                    if (row.WF_FLOW_ID == model.WFFlowID &&
                        row.WF_FLOW_VER == model.WFFlowVer)
                    {
                        model.WFCombineKey = row.ItemValue();
                    }
                }
            }

            await model.GetSystemWorkFlowNodeIDList(model.SysID, model.WFFlowID, model.WFFlowVer, CultureID);

            if (await model.GetSystemWorkFlowNode(model.SysID, model.WFFlowID, model.WFFlowVer, model.WFNodeID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNext.SystemMsg_GetSysSystemWFNode);
            }
            else
            {
                EnumNodeType nodeType;
                if (Enum.TryParse(model.EntitySystemWorkFlowNode.NodeType, out nodeType) == false)
                {
                    SetSystemErrorMessage(SysSystemWorkFlowNext.SystemMsg_GetSysSystemWFNode);
                }
                else
                {
                    model.GetWFNodeTabList(_BaseAPModel.EnumTabAction.SystemWorkFlowNext, nodeType);
                }
            }

            if ((await model.GetSystemWFNextList(CultureID)) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNext.SystemMsg_GetSystemWFNextList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemWorkFlowNext(SystemWorkFlowNextModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNext.SystemMsg_GetSysUserSystemSysIDList);
            }

            await model.GetSystemWorkFlowGroupIDList(model.SysID, CultureID);

            await model.GetUserSystemWorkFlowIDList(AuthState.SessionData.UserID, model.SysID, model.WFFlowGroupID, CultureID);

            if (model.EntityUserSystemWorkFlowIDList != null)
            {
                foreach (SysModel.UserSystemWorkFlowIDs row in model.EntityUserSystemWorkFlowIDList)
                {
                    if (row.WF_FLOW_ID == model.WFFlowID &&
                        row.WF_FLOW_VER == model.WFFlowVer)
                    {
                        model.WFCombineKey = row.ItemValue();
                    }
                }
            }

            await model.GetSystemWorkFlowNodeIDList(model.SysID, model.WFFlowID, model.WFFlowVer, CultureID);

            if (await model.GetSystemWorkFlowNode(model.SysID, model.WFFlowID, model.WFFlowVer, model.WFNodeID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNext.SystemMsg_GetSysSystemWFNode);
            }
            else
            {
                EnumNodeType nodeType;
                if (Enum.TryParse(model.EntitySystemWorkFlowNode.NodeType, out nodeType) == false)
                {
                    SetSystemErrorMessage(SysSystemWorkFlowNext.SystemMsg_GetSysSystemWFNode);
                }
                else
                {
                    model.GetWFNodeTabList(_BaseAPModel.EnumTabAction.SystemWorkFlowNext, nodeType);
                }
            }

            if ((await model.GetSystemWFNextList(CultureID)) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowNext.SystemMsg_GetSystemWFNextList);
            }

            #region - Set Cookie -
            Dictionary<string, string> paraDict = new Dictionary<string, string>();
            paraDict.Add(SystemWorkFlowNextModel.EnumCookieKey.SysID.ToString(), model.SysID);
            paraDict.Add(SystemWorkFlowNextModel.EnumCookieKey.WFFlowGroupID.ToString(), model.WFFlowGroupID);
            paraDict.Add(SystemWorkFlowNextModel.EnumCookieKey.WFCombineKey.ToString(), model.WFCombineKey);
            paraDict.Add(SystemWorkFlowNextModel.EnumCookieKey.WFFlowID.ToString(), model.WFFlowID);
            paraDict.Add(SystemWorkFlowNextModel.EnumCookieKey.WFFlowVer.ToString(), model.WFFlowVer);
            paraDict.Add(SystemWorkFlowNextModel.EnumCookieKey.WFNodeID.ToString(), model.WFNodeID);
            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            return View(model);
        }
    }
}