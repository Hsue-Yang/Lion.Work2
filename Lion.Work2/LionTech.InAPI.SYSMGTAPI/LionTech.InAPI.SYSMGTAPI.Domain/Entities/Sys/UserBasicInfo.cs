using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
   public class UserBasicInfo
    {
        public string UserID { get; set; }
        public string UserNM { get; set; }
        public string ComID { get; set; }
        public string ComNM { get; set; }
        public string UnitID { get; set; }
        public string UnitNM { get; set; }
        public string RestrictType { get; set; }
        public string RestrictTypeNM { get; set; }
        public int ErrorTimes { get; set; }
        public string IsLock { get; set; }
        public string IsDisable { get; set; }
        public string IsLeft { get; set; }
        public DateTime? LastConnectDT { get; set; }
    }
}
