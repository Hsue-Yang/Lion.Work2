using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class UserPermission
    {
        public string UserID { get; set; }
        public string UserNM { get; set; }
        public string UnitID { get; set; }
        public string UnitNM { get; set; }
        public string RestrictType { get; set; }
        public string RestrictTypeNM { get; set; }
        public string IsLock { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}
