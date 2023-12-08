using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemAPIAuthorize
    {
        public string SysID { get; set; }
        public string APIGroupID { get; set; }
        public string APIFunID { get; set; }
        public string ClientSysID { get; set; }
        public string ClientSysNM { get; set; }

        public string UpdUserID { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}
