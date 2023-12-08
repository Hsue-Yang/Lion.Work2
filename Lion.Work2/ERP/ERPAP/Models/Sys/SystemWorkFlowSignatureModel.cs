using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;

namespace ERPAP.Models.Sys
{
    public class SystemWorkFlowSignatureModel : SysModel
    {
        public new enum EnumCookieKey
        {
            SysID,
            WFFlowGroupID,
            WFCombineKey,
            WFFlowID,
            WFFlowVer,
            WFNodeID
        }

        public class SystemWFSig
        {
            public int SigStep { get; set; }
            public string WFSigSeq { get; set; }
            public string WFSigNM { get; set; }
            public string SigTypeNM { get; set; }
            public string APISysNM { get; set; }
            public string APIControllerNM { get; set; }
            public string APIActionNameNM { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDt { get; set; }
        }

        public List<SystemWFSig> SystemWFSigs { get; set; }

        public class SystemWFNode
        {
            public string SysID { get; set; }
            public string WFFlowID { get; set; }
            public string WFFlowVer { get; set; }
            public string WFNodeID { get; set; }
            public string WFSigMemoZHTW { get; set; }
            public string WFSigMemoZHCN { get; set; }
            public string WFSigMemoENUS { get; set; }
            public string WFSigMemoTHTH { get; set; }
            public string WFSigMemoJAJP { get; set; }
            public string WFSigMemoKOKR { get; set; }
            public string SigAPISysID { get; set; }
            public string SigAPIControllerID { get; set; }
            public string SigAPIActionName { get; set; }
            public string ChkAPISysID { get; set; }
            public string ChkAPIControllerID { get; set; }
            public string ChkAPIActionName { get; set; }
            public string IsSigNextNode { get; set; }
            public string IsSigBackNode { get; set; }
            public string UpdUserID { get; set; }
        }

        public class ReturnSystemWFNode
        {
            public string SysID { get; set; }
            public string WFFlowID { get; set; }
            public string WFFlowVer { get; set; }
            public string WFNodeID { get; set; }
            public string WFNodeZHTW { get; set; }
            public string WFNodeZHCN { get; set; }
            public string WFNodeENUS { get; set; }
            public string WFNodeTHTH { get; set; }
            public string WFNodeJAJP { get; set; }
            public string WFNodeKOKR { get; set; }
            public string NodeType { get; set; }
            public string NodeSeqX { get; set; }
            public string NodeSeqY { get; set; }
            public string NodePosBeginX { get; set; }
            public string NodePosBeginY { get; set; }
            public string NodePosEndX { get; set; }
            public string NodePosEndY { get; set; }
            public string IsFirst { get; set; }
            public string IsFinally { get; set; }
            public string BackWFNodeID { get; set; }
            public string WFSigMemoZHTW { get; set; }
            public string WFSigMemoZHCN { get; set; }
            public string WFSigMemoENUS { get; set; }
            public string WFSigMemoTHTH { get; set; }
            public string WFSigMemoJAJP { get; set; }
            public string WFSigMemoKOKR { get; set; }
            public string FunSysID { get; set; }
            public string FunControllerID { get; set; }
            public string FunActionName { get; set; }
            public string SigApiSysID { get; set; }
            public string SigApiControllerID { get; set; }
            public string SigApiActionName { get; set; }
            public string ChkApiSysID { get; set; }
            public string ChkApiControllerID { get; set; }
            public string ChkApiActionName { get; set; }
            public string AssgAPISysID { get; set; }
            public string AssgAPIControllerID { get; set; }
            public string AssgAPIActionName { get; set; }
            public string IsSigNextNode { get; set; }
            public string IsSigBackNode { get; set; }
            public string IsAssgNextNode { get; set; }
            public string SortOrder { get; set; }
            public string Remark { get; set; }         
        }

        public SystemWorkFlowSignatureModel()
        {
            _entity = new EntitySystemWorkFlowSignature(ConnectionStringSERP, ProviderNameSERP);
        }

        #region - Property -
        [Required]
        public string SysID { get; set; }

        [Required]
        public string WFFlowGroupID { get; set; }

        [Required]
        public string WFCombineKey { get; set; }

        public string WFFlowID { get; set; }

        public string WFFlowVer { get; set; }

        public string WFNodeID { get; set; }

        public string SigAPISysID { get; set; }

        public string SigAPIControllerID { get; set; }

        public string SigAPIActionName { get; set; }

        public string ValidAPISysID { get; set; }

        public string ValidAPIControllerID { get; set; }

        public string ValidAPIActionName { get; set; }

        public string IsSigNextNode { get; set; }

        public string IsSigBackNode { get; set; }

        [AllowHtml]
        [StringLength(4000)]
        [InputType(EnumInputType.TextBoxHidden)]
        public string WFSigMemoZHTW { get; set; }

        [AllowHtml]
        [StringLength(4000)]
        [InputType(EnumInputType.TextBoxHidden)]
        public string WFSigMemoZHCN { get; set; }

        [AllowHtml]
        [StringLength(4000)]
        [InputType(EnumInputType.TextBoxHidden)]
        public string WFSigMemoENUS { get; set; }

        [AllowHtml]
        [StringLength(4000)]
        [InputType(EnumInputType.TextBoxHidden)]
        public string WFSigMemoTHTH { get; set; }

        [AllowHtml]
        [StringLength(4000)]
        [InputType(EnumInputType.TextBoxHidden)]
        public string WFSigMemoJAJP { get; set; }

        [AllowHtml]
        [StringLength(4000)]
        [InputType(EnumInputType.TextBoxHidden)]
        public string WFSigMemoKOKR { get; set; }

        public List<UserSystemSysID> EntityChkSysUserSystemSysIDList { get; set; }
        public List<SysModel.SysSystemAPIGroup> EntityChkSysSystemAPIGroupList { get; set; }
        public List<SysModel.SystemAPIFuntions> EntityChkSysSystemAPIFuntionList { get; set; }
        public List<EntitySystemWorkFlowSignature.SystemWFSig> EntitySystemWFSigList { get; private set; }

        #endregion

        #region - Private -
        private readonly EntitySystemWorkFlowSignature _entity;
        #endregion

        public async Task<bool> GetSystemWFSigList(EnumCultureID cultureID)
        {
            try
            {
                SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID;
                WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer;
                WFNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID;

                string apiUrl = API.SystemWorkFlowSignature.QuerySystemWorkFlowSignatures(SysID, WFFlowID, WFFlowVer, WFNodeID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SystemWFSigList = (List<SystemWFSig>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                SystemWFSigs = responseObj.SystemWFSigList;

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetUpdateSystemWFNode(string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                string sigAPISysID = (string.IsNullOrWhiteSpace(SigAPISysID) ? null : SigAPISysID);
                string sigAPIControllerID = (string.IsNullOrWhiteSpace(SigAPIControllerID) ? null : SigAPIControllerID);
                string sigAPIActionName = (string.IsNullOrWhiteSpace(SigAPIActionName) ? null : SigAPIActionName);

                string chkAPISysID = (string.IsNullOrWhiteSpace(ValidAPISysID) ? null : ValidAPISysID);
                string chkAPIControllerID = (string.IsNullOrWhiteSpace(ValidAPIControllerID) ? null : ValidAPIControllerID);
                string chkAPIActionName = (string.IsNullOrWhiteSpace(ValidAPIActionName) ? null : ValidAPIActionName);

                if (sigAPISysID == null ||
                    sigAPIControllerID == null ||
                    sigAPIActionName == null)
                {
                    sigAPISysID = null;
                    sigAPIControllerID = null;
                    sigAPIActionName = null;
                }

                if (chkAPISysID == null ||
                    chkAPIControllerID == null ||
                    chkAPIActionName == null)
                {
                    chkAPISysID = null;
                    chkAPIControllerID = null;
                    chkAPIActionName = null;
                }

                SystemWFNode para = new SystemWFNode()
                {
                    SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                    WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID,
                    WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer,
                    WFNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID,
                    WFSigMemoZHTW = string.IsNullOrWhiteSpace(WFSigMemoZHTW) ? null : WFSigMemoZHTW,
                    WFSigMemoZHCN = string.IsNullOrWhiteSpace(WFSigMemoZHCN) ? null : WFSigMemoZHCN,
                    WFSigMemoENUS = string.IsNullOrWhiteSpace(WFSigMemoENUS) ? null : WFSigMemoENUS,
                    WFSigMemoTHTH = string.IsNullOrWhiteSpace(WFSigMemoTHTH) ? null : WFSigMemoTHTH,
                    WFSigMemoJAJP = string.IsNullOrWhiteSpace(WFSigMemoJAJP) ? null : WFSigMemoJAJP,
                    WFSigMemoKOKR = string.IsNullOrWhiteSpace(WFSigMemoKOKR) ? null : WFSigMemoKOKR,
                    SigAPISysID = sigAPISysID,
                    SigAPIControllerID = sigAPIControllerID,
                    SigAPIActionName = sigAPIActionName,
                    ChkAPISysID = chkAPISysID,
                    ChkAPIControllerID = chkAPIControllerID,
                    ChkAPIActionName = chkAPIActionName,
                    IsSigNextNode = string.IsNullOrWhiteSpace(IsSigNextNode) ? EnumYN.N.ToString() : EnumYN.Y.ToString(),
                    IsSigBackNode = string.IsNullOrWhiteSpace(IsSigBackNode) ? EnumYN.N.ToString() : EnumYN.Y.ToString(),
                    UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID
                };

                var paraJsonStr = Common.GetJsonSerializeObject(para);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.SystemWorkFlowSignature.EditSystemWorkFlowNode(userID);
                string response = await PublicFun.HttpPutWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes);

                var responseObj = new
                {
                    returnSystemWFNode = (ReturnSystemWFNode)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_WF_NODE, Mongo_BaseAP.EnumModifyType.U, userID, ipAddress, cultureID, response);
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
    }
}