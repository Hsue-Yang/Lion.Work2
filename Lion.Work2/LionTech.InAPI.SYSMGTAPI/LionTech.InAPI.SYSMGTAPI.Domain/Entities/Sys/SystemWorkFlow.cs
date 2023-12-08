using System;
using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemWorkFlow
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string WFFlowGroupID { get; set; }
        public string WFFlowGroupNM { get; set; }
        public string WFFlowID { get; set; }
        public string WFFlowNM { get; set; }
        public string WFFlowVer { get; set; }
        public string FlowTypeNM { get; set; }
        public string FlowManUserNM { get; set; }
        public string EnableDate { get; set; }
        public string DisableDate { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDt { get; set; }
    }

    public class FlowRole
    {
        public string SysID { get; set; }
        public string RoleID { get; set; }
        public string RoleNM { get; set; }
        public bool HasRole { get; set; }
    }

    public class SystemWorkFlowDetails
    {
        public SystemWorkFlowDetail SystemWorkFlowDetail { get; set; }
        public List<SystemWorkFlowRole> SystemWorkFlowRoles { get; set; }
    }

    public class SystemWorkFlowDetail
    {
        public string SysID { get; set; }
        public string WFFlowGroupID { get; set; }
        public string WFFlowID { get; set; }
        public string WFFlowZHTW { get; set; }
        public string WFFlowZHCN { get; set; }
        public string WFFlowENUS { get; set; }
        public string WFFlowTHTH { get; set; }
        public string WFFlowJAJP { get; set; }
        public string WFFlowKOKR { get; set; }
        public string WFFlowVer { get; set; }
        public string FlowType { get; set; }
        public string FlowManUserID { get; set; }
        public string EnableDate { get; set; }
        public string DisableDate { get; set; }
        public string IsStartFun { get; set; }
        public string SortOrder { get; set; }
        public string MsgSysID { get; set; }
        public string MsgControllerID { get; set; }
        public string MsgActionName { get; set; }
        public string Remark { get; set; }
        public string UpdUserID { get; set; }
    }

    public class SystemWorkFlowRole
    {
        public string SysID { get; set; }
        public string RoleID { get; set; }
        public string WFFlowID { get; set; }
        public string WFFlowVer { get; set; }
        public string UpdUserID { get; set; }
    }
}
