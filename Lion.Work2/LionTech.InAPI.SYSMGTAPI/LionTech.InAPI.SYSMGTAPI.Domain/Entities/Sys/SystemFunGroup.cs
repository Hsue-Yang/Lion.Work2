using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemFunGroup
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }

        public string FunControllerID { get; set; }
        public string FunGroupNM { get; set; }
        public string SortOrder { get; set; }

        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}
