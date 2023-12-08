using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP.Sys;
using LionTech.WorkFlow;
using Resources;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemWorkFlowDocument()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            SystemWorkFlowDocumentModel model = new SystemWorkFlowDocumentModel();
            
            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.SysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowDocumentModel.EnumCookieKey.SysID.ToString());
                model.WFFlowGroupID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowDocumentModel.EnumCookieKey.WFFlowGroupID.ToString());
                model.WFCombineKey = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowDocumentModel.EnumCookieKey.WFCombineKey.ToString());
                model.WFFlowID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowDocumentModel.EnumCookieKey.WFFlowID.ToString());
                model.WFFlowVer = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowDocumentModel.EnumCookieKey.WFFlowVer.ToString());
                model.WFNodeID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowDocumentModel.EnumCookieKey.WFNodeID.ToString());
            }
            #endregion

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowDocument.SystemMsg_GetSysUserSystemSysIDList);
            }

            await model.GetSystemWorkFlowGroupIDList(model.SysID, CultureID);

            await model.GetUserSystemWorkFlowIDList(AuthState.SessionData.UserID, model.SysID, model.WFFlowGroupID, CultureID);

            if (model.EntityUserSystemWorkFlowIDList != null)
            {
                foreach (SysModel.UserSystemWorkFlowIDs entityUserSystemWorkFlowID in model.EntityUserSystemWorkFlowIDList)
                {
                    if (entityUserSystemWorkFlowID.WF_FLOW_ID == model.WFFlowID &&
                        entityUserSystemWorkFlowID.WF_FLOW_VER == model.WFFlowVer)
                    {
                        model.WFCombineKey = entityUserSystemWorkFlowID.ItemValue();
                    }
                }
            }

            await model.GetSystemWorkFlowNodeIDList(model.SysID, model.WFFlowID, model.WFFlowVer, CultureID);

            if (await model.GetSystemWorkFlowNode(model.SysID, model.WFFlowID, model.WFFlowVer, model.WFNodeID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowDocument.SystemMsg_GetSysSystemWFNode);
            }
            else
            {
                EnumNodeType nodeType;
                if (Enum.TryParse(model.EntitySystemWorkFlowNode.NodeType, out nodeType) == false)
                {
                    SetSystemErrorMessage(SysSystemWorkFlowDocument.SystemMsg_GetSysSystemWFNode);
                }
                else
                {
                    model.GetWFNodeTabList(_BaseAPModel.EnumTabAction.SystemWorkFlowDocument, nodeType);
                }
            }

            if (await model.GetSystemWFDocList(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowDocument.SystemMsg_GetSystemWorkFlowDocumentList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemWorkFlowDocument(SystemWorkFlowDocumentModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowDocument.SystemMsg_GetSysUserSystemSysIDList);
            }

            await model.GetSystemWorkFlowGroupIDList(model.SysID, CultureID);

            await model.GetUserSystemWorkFlowIDList(AuthState.SessionData.UserID, model.SysID, model.WFFlowGroupID, CultureID);

            if (model.EntityUserSystemWorkFlowIDList != null)
            {
                foreach (SysModel.UserSystemWorkFlowIDs entityUserSystemWorkFlowID in model.EntityUserSystemWorkFlowIDList)
                {
                    if (entityUserSystemWorkFlowID.WF_FLOW_ID == model.WFFlowID &&
                        entityUserSystemWorkFlowID.WF_FLOW_VER == model.WFFlowVer)
                    {
                        model.WFCombineKey = entityUserSystemWorkFlowID.ItemValue();
                    }
                }
            }

            await model.GetSystemWorkFlowNodeIDList(model.SysID, model.WFFlowID, model.WFFlowVer, CultureID);

            if (await model.GetSystemWorkFlowNode(model.SysID, model.WFFlowID, model.WFFlowVer, model.WFNodeID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowDocument.SystemMsg_GetSysSystemWFNode);
            }
            else
            {
                EnumNodeType nodeType;
                if (Enum.TryParse(model.EntitySystemWorkFlowNode.NodeType, out nodeType) == false)
                {
                    SetSystemErrorMessage(SysSystemWorkFlowDocument.SystemMsg_GetSysSystemWFNode);
                }
                else
                {
                    model.GetWFNodeTabList(_BaseAPModel.EnumTabAction.SystemWorkFlowDocument, nodeType);
                }
            }

            if (await model.GetSystemWFDocList(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowDocument.SystemMsg_GetSystemWorkFlowDocumentList);
            }

            #region - Set Cookie -
            Dictionary<string, string> paraDict = new Dictionary<string, string>();
            paraDict.Add(SystemWorkFlowDocumentModel.EnumCookieKey.SysID.ToString(), model.SysID);
            paraDict.Add(SystemWorkFlowDocumentModel.EnumCookieKey.WFFlowGroupID.ToString(), model.WFFlowGroupID);
            paraDict.Add(SystemWorkFlowDocumentModel.EnumCookieKey.WFCombineKey.ToString(), model.WFCombineKey);
            paraDict.Add(SystemWorkFlowDocumentModel.EnumCookieKey.WFFlowID.ToString(), model.WFFlowID);
            paraDict.Add(SystemWorkFlowDocumentModel.EnumCookieKey.WFFlowVer.ToString(), model.WFFlowVer);
            paraDict.Add(SystemWorkFlowDocumentModel.EnumCookieKey.WFNodeID.ToString(), model.WFNodeID);
            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            return View(model);
        }
    }
}