using System;
using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
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

    public class SystemWFSigSeq
    {
        public string CodeID { get; set; }
        public string CodeNM { get; set; }
    }

    public class SystemWFSignatureDetail
    {
        public SystemWFSigDetail SystemWFSigDetail { get; set; }
        public List<SystemRoleSignatures> SystemRoleSignatures { get; set; }
    }

    public class SystemWorkFlowSignatureDetail
    {
        public string SysID { get; set; }
        public string WFFlowID { get; set; }
        public string WFFlowVer { get; set; }
        public string WFNodeID { get; set; }
        public string WFSigSeq { get; set; }
        public int SigStep { get; set; }
        public string WFSigZHTW { get; set; }
        public string WFSigZHCN { get; set; }
        public string WFSigENUS { get; set; }
        public string WFSigTHTH { get; set; }
        public string WFSigJAJP { get; set; }
        public string WFSigKOKR { get; set; }
        public string SigType { get; set; }
        public string APISysID { get; set; }
        public string APIControllerID { get; set; }
        public string APIActionName { get; set; }
        public string CompareWFNodeID { get; set; }
        public string CompareWFSigSeq { get; set; }
        public string ChkAPISysID { get; set; }
        public string ChkAPIControllerID { get; set; }
        public string ChkAPIActionName { get; set; }
        public string IsReq { get; set; }
        public string Remark { get; set; }
    }

    public class SystemWFSigDetail
    {
        public string SysID { get; set; }
        public string WFFlowID { get; set; }
        public string WFFlowVer { get; set; }
        public string WFNodeID { get; set; }
        public int SigStep { get; set; }
        public string WFSigSeq { get; set; }
        public string WFSigZHTW { get; set; }
        public string WFSigZHCN { get; set; }
        public string WFSigENUS { get; set; }
        public string WFSigTHTH { get; set; }
        public string WFSigJAJP { get; set; }
        public string WFSigKOKR { get; set; }
        public string SigType { get; set; }
        public string APISysID { get; set; }
        public string APIControllerID { get; set; }
        public string APIActionName { get; set; }
        public string CompareWFNodeID { get; set; }
        public string CompareWFSigSeq { get; set; }
        public string ChkAPISysID { get; set; }
        public string ChkAPIControllerID { get; set; }
        public string ChkAPIActionName { get; set; }
        public string IsReq { get; set; }
        public string Remark { get; set; }
        public string UpdUserID { get; set; }
    }
    public class SystemRoleSig
    {
        public string SysID { get; set; }
        public string WFFlowID { get; set; }
        public string WFFlowVer { get; set; }
        public string WFNodeID { get; set; }
        public string WFSigSeq { get; set; }
        public string RoleID { get; set; }
        public string RoleNM { get; set; }
        public string HasRole { get; set; }
    }

    public class SystemRoleSignatures
    {
        public string SysID { get; set; }
        public string RoleID { get; set; }
        public string WFFlowID { get; set; }
        public string WFFlowVer { get; set; }
        public string WFNodeID { get; set; }
        public string WFSigSeq { get; set; }
        public string UpdUserID { get; set; }
    }
}
