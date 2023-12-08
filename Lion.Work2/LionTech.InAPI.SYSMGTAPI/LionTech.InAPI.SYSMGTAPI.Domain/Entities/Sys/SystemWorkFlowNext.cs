using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemWorkFlowNext
    {
        public string NextWFNodeID { get; set; }
        public string NextWFNodeNM { get; set; }
        public string NextNodeTypeNM { get; set; }
        public string NextResultValue { get; set; }
        public string FunSysNM { get; set; }
        public string SubSysNM { get; set; }
        public string FunControllerNM { get; set; }
        public string FunActionNameNM { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDt { get; set; }
    }

    public class SystemWFNodeList
    {
        public string WFNodeID { get; set; }
        public string WFNodeNM { get; set; }
    }

    public class SystemWFNext
    {
        public string SysID { get; set; }
        public string WFFlowID { get; set; }
        public string WFFlowVer { get; set; }
        public string WFNodeID { get; set; }
        public string NextWFNodeID { get; set; }
        public string NextWFNodeNM { get; set; }
        public string NextResultValue { get; set; }
        public string SortOrder { get; set; }
        public string Remark { get; set; }
    }

    public class EditSystemWFNext
    {
        public string SysID { get; set; }
        public string WFFlowID { get; set; }
        public string WFFlowVer { get; set; }
        public string WFNodeID { get; set; }
        public string NextWFNodeID { get; set; }
        public string NextResultValue { get; set; }
        public string SortOrder { get; set; }
        public string Remark { get; set; }
        public string UpdUserID { get; set; }
    }
}