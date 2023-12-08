using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models;
using ERPAP.Models.Sys;
using LionTech.Entity;
using LionTech.Entity.ERP.Sys;
using LionTech.Web.ERPHelper;
using LionTech.WorkFlow;
using Resources;
using static ERPAP.Models.Sys.SysModel;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemWorkFlowSignature()
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            SystemWorkFlowSignatureModel model = new SystemWorkFlowSignatureModel();
            
            #region - Get Cookie -
            if (AuthState.CookieData.HasSystemFunKey(AuthState.SystemFunKey))
            {
                model.SysID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowSignatureModel.EnumCookieKey.SysID.ToString());
                model.WFFlowGroupID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowSignatureModel.EnumCookieKey.WFFlowGroupID.ToString());
                model.WFCombineKey = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowSignatureModel.EnumCookieKey.WFCombineKey.ToString());
                model.WFFlowID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowSignatureModel.EnumCookieKey.WFFlowID.ToString());
                model.WFFlowVer = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowSignatureModel.EnumCookieKey.WFFlowVer.ToString());
                model.WFNodeID = AuthState.CookieData.CookieKeys(AuthState.SystemFunKey, SystemWorkFlowSignatureModel.EnumCookieKey.WFNodeID.ToString());
            }
            #endregion

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
                SetSystemErrorMessage(SysSystemWorkFlowSignature.SystemMsg_GetSysSystemWFNode);
            }
            else
            {
                EnumNodeType nodeType;
                if (Enum.TryParse(model.EntitySystemWorkFlowNode.NodeType, out nodeType) == false)
                {
                    SetSystemErrorMessage(SysSystemWorkFlowSignature.SystemMsg_GetSysSystemWFNode);
                }
                else
                {
                    model.GetWFNodeTabList(_BaseAPModel.EnumTabAction.SystemWorkFlowSignature, nodeType);
                }
            }

            if (await model.GetSystemWFSigList(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignature.SystemMsg_GetSystemWorkFlowSignatureList);
            }

            if (await model.GetSystemWorkFlowNode(model.SysID, model.WFFlowID, model.WFFlowVer, model.WFNodeID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignature.SystemMsg_GetSysSystemWFNode);
            }
            else
            {
                model.SigAPISysID = model.EntitySystemWorkFlowNode.SigAPISysID;
                model.SigAPIControllerID = model.EntitySystemWorkFlowNode.SigAPIControllerID;
                model.SigAPIActionName = model.EntitySystemWorkFlowNode.SigAPIActionName;
                model.ValidAPISysID = model.EntitySystemWorkFlowNode.ChkAPISysID;
                model.ValidAPIControllerID = model.EntitySystemWorkFlowNode.ChkAPIControllerID;
                model.ValidAPIActionName = model.EntitySystemWorkFlowNode.ChkAPIActionName;
                model.IsSigNextNode = (model.EntitySystemWorkFlowNode.IsSigNextNode == EnumYN.Y.ToString()) ? EnumYN.Y.ToString() : string.Empty;
                model.IsSigBackNode = (model.EntitySystemWorkFlowNode.IsSigBackNode == EnumYN.Y.ToString()) ? EnumYN.Y.ToString() : string.Empty;
                model.WFSigMemoZHTW = model.EntitySystemWorkFlowNode.WFSigMemoZHTW;
                model.WFSigMemoZHCN = model.EntitySystemWorkFlowNode.WFSigMemoZHCN;
                model.WFSigMemoENUS = model.EntitySystemWorkFlowNode.WFSigMemoENUS;
                model.WFSigMemoTHTH = model.EntitySystemWorkFlowNode.WFSigMemoTHTH;
                model.WFSigMemoJAJP = model.EntitySystemWorkFlowNode.WFSigMemoJAJP;
                model.WFSigMemoKOKR = model.EntitySystemWorkFlowNode.WFSigMemoKOKR;
            }

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignature.SystemMsg_GetSysUserSystemSysIDList);
            }
            else if (model.EntityUserSystemSysIDList != null)
            {
                model.EntityChkSysUserSystemSysIDList = new List<UserSystemSysID>(model.EntityUserSystemSysIDList);
            }

            if (await model.GetSystemAPIGroupByIdList(model.ValidAPISysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignature.SystemMsg_GetSysSystemAPIGroupList);
            }
            else if (model.SystemAPIGroupByIdList != null)
            {
                model.EntityChkSysSystemAPIGroupList = new List<SysModel.SysSystemAPIGroup>(model.SystemAPIGroupByIdList);
            }

            if (await model.GetSystemAPIFuntionList(model.ValidAPISysID, model.ValidAPIControllerID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignature.SystemMsg_GetSysSystemAPIFuntionList);
            }
            else if (model.EntitySystemAPIFuntionList != null)
            {
                model.EntityChkSysSystemAPIFuntionList = new List<SysModel.SystemAPIFuntions>(model.EntitySystemAPIFuntionList);
            }

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignature.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (await model.GetSystemAPIGroupByIdList(model.SigAPISysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.SystemMsg_GetSysSystemAPIGroupList);
            }

            if (await model.GetSystemAPIFuntionList(model.SigAPISysID, model.SigAPIControllerID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.SystemMsg_GetSysSystemAPIFuntionList);
            }

            return View(model);
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemWorkFlowSignature(SystemWorkFlowSignatureModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return AuthState.UnAuthorizedActionResult;
            }

            if (IsPostBack)
            {
                if (model.ExecAction == EnumActionType.Update)
                {
                    if (await model.GetUpdateSystemWFNode(AuthState.SessionData.UserID, ClientIPAddress(), CultureID) == false)
                    {
                        SetSystemErrorMessage(SysSystemWorkFlowSignature.EditWFNodeDetail_Failure);
                    }
                    else
                    {
                        SetSystemAlertMessage(SysSystemWorkFlowSignature.EditWFNodeDetail_Success);
                    }
                }
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
                SetSystemErrorMessage(SysSystemWorkFlowSignature.SystemMsg_GetSysSystemWFNode);
            }
            else
            {
                EnumNodeType nodeType;
                if (Enum.TryParse(model.EntitySystemWorkFlowNode.NodeType, out nodeType) == false)
                {
                    SetSystemErrorMessage(SysSystemWorkFlowSignature.SystemMsg_GetSysSystemWFNode);
                }
                else
                {
                    model.GetWFNodeTabList(_BaseAPModel.EnumTabAction.SystemWorkFlowSignature, nodeType);
                }
            }

            if (await model.GetSystemWFSigList(CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignature.SystemMsg_GetSystemWorkFlowSignatureList);
            }

            if (await model.GetSystemWorkFlowNode(model.SysID, model.WFFlowID, model.WFFlowVer, model.WFNodeID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignature.SystemMsg_GetSysSystemWFNode);
            }
            else
            {
                model.SigAPISysID = model.EntitySystemWorkFlowNode.SigAPISysID;
                model.SigAPIControllerID = model.EntitySystemWorkFlowNode.SigAPIControllerID;
                model.SigAPIActionName = model.EntitySystemWorkFlowNode.SigAPIActionName;
                model.ValidAPISysID = model.EntitySystemWorkFlowNode.ChkAPISysID;
                model.ValidAPIControllerID = model.EntitySystemWorkFlowNode.ChkAPIControllerID;
                model.ValidAPIActionName = model.EntitySystemWorkFlowNode.ChkAPIActionName;
                model.IsSigNextNode = (model.EntitySystemWorkFlowNode.IsSigNextNode == EnumYN.Y.ToString()) ? EnumYN.Y.ToString() : string.Empty;
                model.IsSigBackNode = (model.EntitySystemWorkFlowNode.IsSigBackNode == EnumYN.Y.ToString()) ? EnumYN.Y.ToString() : string.Empty;
                model.WFSigMemoZHTW = model.EntitySystemWorkFlowNode.WFSigMemoZHTW;
                model.WFSigMemoZHCN = model.EntitySystemWorkFlowNode.WFSigMemoZHCN;
                model.WFSigMemoENUS = model.EntitySystemWorkFlowNode.WFSigMemoENUS;
                model.WFSigMemoTHTH = model.EntitySystemWorkFlowNode.WFSigMemoTHTH;
                model.WFSigMemoJAJP = model.EntitySystemWorkFlowNode.WFSigMemoJAJP;
                model.WFSigMemoKOKR = model.EntitySystemWorkFlowNode.WFSigMemoKOKR;
            }

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignature.SystemMsg_GetSysUserSystemSysIDList);
            }
            else if (model.EntityUserSystemSysIDList != null)
            {
                model.EntityChkSysUserSystemSysIDList = new List<UserSystemSysID>(model.EntityUserSystemSysIDList);
            }

            if (await model.GetSystemAPIGroupByIdList(model.ValidAPISysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignature.SystemMsg_GetSysSystemAPIGroupList);
            }
            else if (model.SystemAPIGroupByIdList != null)
            {
                model.EntityChkSysSystemAPIGroupList = new List<SysModel.SysSystemAPIGroup>(model.SystemAPIGroupByIdList);
            }

            if (await model.GetSystemAPIFuntionList(model.ValidAPISysID, model.ValidAPIControllerID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignature.SystemMsg_GetSysSystemAPIFuntionList);
            }
            else if (model.EntitySystemAPIFuntionList != null)
            {
                model.EntityChkSysSystemAPIFuntionList = new List<SysModel.SystemAPIFuntions>(model.EntitySystemAPIFuntionList);
            }

            if (await model.GetUserSystemSysIDList(AuthState.SessionData.UserID, true, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignature.SystemMsg_GetSysUserSystemSysIDList);
            }

            if (await model.GetSystemAPIGroupByIdList(model.SigAPISysID, AuthState.SessionData.UserID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.SystemMsg_GetSysSystemAPIGroupList);
            }

            if (await model.GetSystemAPIFuntionList(model.SigAPISysID, model.SigAPIControllerID, CultureID) == false)
            {
                SetSystemErrorMessage(SysSystemWorkFlowSignatureDetail.SystemMsg_GetSysSystemAPIFuntionList);
            }

            #region - Set Cookie -
            Dictionary<string, string> paraDict = new Dictionary<string, string>();
            paraDict.Add(SystemWorkFlowSignatureModel.EnumCookieKey.SysID.ToString(), model.SysID);
            paraDict.Add(SystemWorkFlowSignatureModel.EnumCookieKey.WFFlowGroupID.ToString(), model.WFFlowGroupID);
            paraDict.Add(SystemWorkFlowSignatureModel.EnumCookieKey.WFCombineKey.ToString(), model.WFCombineKey);
            paraDict.Add(SystemWorkFlowSignatureModel.EnumCookieKey.WFFlowID.ToString(), model.WFFlowID);
            paraDict.Add(SystemWorkFlowSignatureModel.EnumCookieKey.WFFlowVer.ToString(), model.WFFlowVer);
            paraDict.Add(SystemWorkFlowSignatureModel.EnumCookieKey.WFNodeID.ToString(), model.WFNodeID);
            AuthState.CookieData.SetCookieKeys(AuthState.SystemFunKey, paraDict);
            #endregion

            return View(model);
        }
    }
}