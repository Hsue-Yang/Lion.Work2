using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemRole
    {
        public string RoleID { get; set; }
        public string RoleNM { get; set; }

        public string RoleCategoryID { get; set; }
        public string RoleCategoryNM { get; set; }

        public string IsMaster { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}
