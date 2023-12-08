using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemFunMenu
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }

        public string FunMenu { get; set; }
        public string FunMenuNM { get; set; }

        public string DefaultMenuID { get; set; }
        public string IsDisable { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}
