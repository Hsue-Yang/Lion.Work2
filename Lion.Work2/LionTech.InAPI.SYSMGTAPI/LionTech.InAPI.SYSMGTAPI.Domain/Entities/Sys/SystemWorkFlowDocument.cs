using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemWorkFlowDocument
    {
        public string WFDocSeq { get; set; }
        public string WFDocNM { get; set; }
        public string IsReq { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDt { get; set; }
    }
    public class SystemWorkFlowDocumentDetail
    {
        public string SysID { get; set; }
        public string WFFlowID { get; set; }
        public string WFFlowVer { get; set; }
        public string WFNodeID { get; set; }
        public string WFDocSeq { get; set; }
        public string WFDocZHTW { get; set; }
        public string WFDocZHCN { get; set; }
        public string WFDocENUS { get; set; }
        public string WFDocTHTH { get; set; }
        public string WFDocJAJP { get; set; }
        public string WFDocKOKR { get; set; }
        public string IsReq { get; set; }
        public string Remark { get; set; }
        public string UpdUserID { get; set; }
    }
}