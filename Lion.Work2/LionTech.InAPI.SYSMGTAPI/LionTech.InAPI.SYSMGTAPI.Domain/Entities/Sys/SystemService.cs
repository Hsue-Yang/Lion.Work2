using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemService
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string SubSysID { get; set; }
        public string SubSysNM { get; set; }

        public string ServiceID { get; set; }
        public string ServiceNM { get; set; }

        public string Remark { get; set; }
        public string UpdUserID { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}
