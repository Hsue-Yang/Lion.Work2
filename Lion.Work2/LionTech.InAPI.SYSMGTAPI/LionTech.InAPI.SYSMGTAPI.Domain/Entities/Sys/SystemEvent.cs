using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemEvent
    {
        public string SysNM { get; set; }
        public string EventGroupID { get; set; }
        public string EventGroupNM { get; set; }
        public string EventID { get; set; }
        public string EventNM { get; set; }
        public string IsDisable { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}