using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemRoleCategory
    {
        public string SysID { get; set; }
        public string RoleCategoryID { get; set; }
        public string RoleCategoryNM { get; set; }

        public string SortOrder { get; set; }

        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}
