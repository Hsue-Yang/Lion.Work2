using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SysLoginEventSetting
    {
        public string SysID { get; set; }
        public string SysNMID { get; set; }
        public string SubSysNMID { get; set; }
        public string LoginEventID { get; set; }
        public string LoginEventNMID { get; set; }
        public string SubSysID { get; set; }
        public DateTime StartDT { get; set; }
        public DateTime EndDT { get; set; }
        public string IsDisable { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}
