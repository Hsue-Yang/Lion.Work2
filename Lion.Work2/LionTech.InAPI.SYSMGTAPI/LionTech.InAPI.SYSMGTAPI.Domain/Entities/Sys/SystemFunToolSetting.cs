using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemFunToolSetting
    {
        public string UserID { get; set; }
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string FunControllerID { get; set; }
        public string FunGroupNM { get; set; }
        public string FunActionName { get; set; }
        public string FunNM { get; set; }
        public string ToolNo { get; set; }
        public string ToolNM { get; set; }
        public string IsCurrently { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}
