using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemPurview
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }

        public string PurviewID { get; set; }
        public string PurviewNM { get; set; }
        public string SortOrder { get; set; }
        public string Remark { get; set; }

        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}
