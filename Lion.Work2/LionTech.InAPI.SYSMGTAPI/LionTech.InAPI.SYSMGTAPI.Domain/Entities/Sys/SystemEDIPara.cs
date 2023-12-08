using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemEDIPara
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string EDIFlowID { get; set; }
        public string EDIFlowNM { get; set; }
        public string EDIJobID { get; set; }
        public string EDIJobNM { get; set; }
        public string EDIJobParaID { get; set; }
        public string EDIJobParaType { get; set; }
        public string EDIJobParaValue { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDt { get; set; }
    }
}
