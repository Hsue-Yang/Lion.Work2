using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemEDICon
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string EDIFlowID { get; set; }
        public string EDIFlowNM { get; set; }
        public string EDIConID { get; set; }
        public string EDIConNM { get; set; }
        public string ProviderName { get; set; }
        public string ConValue { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }
        public DateTime UpdDt { get; set; }
    }

    public class SystemEDIConValue
    {
        public string SysID { get; set; }
        public string EDIFlowID { get; set; }
        public string EDIConID { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }
    }

    public class SystemEDIConDetail
    {
        public string SysID { get; set; }
        public string EDIFlowID { get; set; }
        public string EDIConID { get; set; }
        public string EDIConZHTW { get; set; }
        public string EDIConZHCN { get; set; }
        public string EDIConENUS { get; set; }
        public string EDIConTHTH { get; set; }
        public string EDIConJAJP { get; set; }
        public string EDIConKOKR { get; set; }
        public string ProviderName { get; set; }
        public string ConValue { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }
    }

}
