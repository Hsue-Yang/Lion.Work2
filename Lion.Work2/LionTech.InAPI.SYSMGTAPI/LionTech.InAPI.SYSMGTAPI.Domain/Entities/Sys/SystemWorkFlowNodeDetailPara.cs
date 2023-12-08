using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemWorkFlowNodeDetailExecuteResult
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
        public int NodeSeqX { get; set; }
        public int NodeSeqY { get; set; }

        public int NodePosBeginX { get; set; }
        public int NodePosBeginY { get; set; }
        public int NodePosEndX { get; set; }
        public int NodePosEndY { get; set; }

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

    public class SystemWorkFlowNodePara
    {
        public SystemWorkFlowNodeDetailPara SystemWorkFlowNodeDetailPara { get; set; }
        public List<SystemWorkFlowNodeRolePara> SystemWorkFlowNodeRoleParas { get; set; }
    }

    public class SystemWorkFlowNodeDetailPara
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
        public int NodeSeqX { get; set; }
        public int NodeSeqY { get; set; }

        public int NodePosBeginX { get; set; }
        public int NodePosBeginY { get; set; }
        public int NodePosEndX { get; set; }
        public int NodePosEndY { get; set; }

        public string IsFirst { get; set; }
        public string IsFinally { get; set; }
        public string BackWFNodeID { get; set; }

        public string FunSysID { get; set; }
        public string FunControllerID { get; set; }
        public string FunActionName { get; set; }
        public string AssgAPISysID { get; set; }
        public string AssgAPIControllerID { get; set; }
        public string AssgAPIActionName { get; set; }

        public string IsAssgNextNode { get; set; }

        public string SortOrder { get; set; }
        public string Remark { get; set; }
        public string UpdUserID { get; set; }
    }

    public class SystemWorkFlowNodeRole
    {
        public string SysID { get; set; }
        public string RoleID { get; set; }
        public string RoleNM { get; set; }
        public string HasRole { get; set; }
    }

    public class SystemWorkFlowNodeRolePara
    {
        public string SysID { get; set; }
        public string RoleID { get; set; }
        public string WFFlowID { get; set; }
        public string WFFlowVer { get; set; }

        public string WFNodeID { get; set; }
        public string UpdUserID { get; set; }
    }
}
