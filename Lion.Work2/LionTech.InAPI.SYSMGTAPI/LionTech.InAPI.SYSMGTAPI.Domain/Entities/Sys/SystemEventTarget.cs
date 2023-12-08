using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemEventTarget
    {
        public string SysID { get; set; }
        public string EventGroupID { get; set; }
        public string EventID { get; set; }

        public string TargetSysID { get; set; }
        public string TargetSysNM { get; set; }
        public string SubSysID { get; set; }
        public string SubSysNM { get; set; }

        public string TargetPath { get; set; }
        public bool HasITRole { get; set; }

        public string UpdUserID { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}
