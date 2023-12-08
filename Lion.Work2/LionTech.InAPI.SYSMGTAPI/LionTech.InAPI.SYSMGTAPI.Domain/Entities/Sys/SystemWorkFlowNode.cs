using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemWorkFlowNode
    {
        public string SysID { get; set; }
        public string WFFlowGroupID { get; set; }
        public string WFFlowID { get; set; }
        public string WFFlowVer { get; set; }
        public string WFNodeID { get; set; }
        public string WFNodeNM { get; set; }

        public string NodeTypeNM { get; set; }
        public string NodeSeqX { get; set; }
        public string NodeSeqY { get; set; }
        public string IsFirst { get; set; }
        public string IsFinally { get; set; }

        public string FunSysNM { get; set; }
        public string SubSysNM { get; set; }
        public string FunControllerNM { get; set; }
        public string FunActionNameNM { get; set; }

        public string BackWFNodeNM { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDt { get; set; }

        public string SysNM { get; set; }
        public string WFFlowNM { get; set; }
        public string NodeType { get; set; }
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
        public string AssgAPISysID { get; set; }
        public string AssgAPIControllerID { get; set; }
        public string AssgAPIActionName { get; set; }
        public string IsSigNextNode { get; set; }
        public string IsSigBackNode { get; set; }
        public string IsAssgNextNode { get; set; }
    }

    public class SysUserSystemWorkFlowID
    {
        public string WF_FLOW_ID { get; set; }
        public string WF_FLOW_VER { get; set; }
        public string WF_FLOW_VALUE { get; set; }
        public string WF_FLOW_TEXT { get; set; }
        public string SORT_ORDER { get; set; }
    }

    public class SystemWorkFlowNodeIDs
    {
        public string WFNodeID { get; set; }
        public string SortOrder { get; set; }
        public string WFNodeNM { get; set; }
    }
}