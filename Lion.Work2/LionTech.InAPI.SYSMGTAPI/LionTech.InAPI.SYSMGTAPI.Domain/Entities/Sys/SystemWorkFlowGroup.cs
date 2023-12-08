using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemWorkFlowGroup
    {
        public string SysID { get; set; }
        public string SysNm { get; set; }
        public string WFFlowGroupID { get; set; }
        public string WFFlowGroupNM { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDt { get; set; }
    }
    public class SystemWorkFlowGroupDetail
    {
        public string SysID { get; set; }
        public string WFFlowGroupID { get; set; }
        public string WFFlowGroupZHTW { get; set; }
        public string WFFlowGroupZHCN { get; set; }
        public string WFFlowGroupENUS { get; set; }
        public string WFFlowGroupTHTH { get; set; }
        public string WFFlowGroupJAJP { get; set; }
        public string WFFlowGroupKOKR { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }
    }
    public class SystemWorkFlowGroupIDs
    {
        public string WFFlowGroupID { get; set; }
        public string WFFlowGroupNM { get; set; }
    }
}