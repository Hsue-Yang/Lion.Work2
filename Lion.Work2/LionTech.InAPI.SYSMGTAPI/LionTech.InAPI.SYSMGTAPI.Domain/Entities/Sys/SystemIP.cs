using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemIP
    {
        public string SysID { get; set; }

        public string SubSysID { get; set; }
        public string SubSysNM { get; set; }

        public string IPAddress { get; set; }
        public string ServerNM { get; set; }

        public string IsAPServer { get; set; }
        public string IsAPIServer { get; set; }
        public string IsDBServer { get; set; }
        public string IsFileServer { get; set; }
        public string IsOutsourcing { get; set; }

        public string FolderPath { get; set; }
        public string Remark { get; set; }

        public string UpdUserID { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}
