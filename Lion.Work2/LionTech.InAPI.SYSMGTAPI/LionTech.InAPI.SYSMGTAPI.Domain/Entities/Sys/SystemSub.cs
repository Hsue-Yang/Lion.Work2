using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemSub
    {
        public string SubSysID { get; set; }
        public string SubSysNM { get; set; }

        public string SysMANUserID { get; set; }
        public string SysMANUserNM { get; set; }

        public string SysID { get; set; }
        public string SysNM { get; set; }
        
        public string SysNMZHTW { get; set; }
        public string SysNMZHCN { get; set; }
        public string SysNMENUS { get; set; }
        public string SysNMTHTH { get; set; }
        public string SysNMJAJP { get; set; }
        public string SysNMKOKR { get; set; }

        public string SortOrder { get; set; }

        public string UpdUserID { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}
