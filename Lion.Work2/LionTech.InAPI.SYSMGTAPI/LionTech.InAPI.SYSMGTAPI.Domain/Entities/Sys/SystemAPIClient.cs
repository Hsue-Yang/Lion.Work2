using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemAPIClient
    {
        public string APINo { get; set; }
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string APIGroupID { get; set; }
        public string APIGroupNM { get; set; }
        public string APIFunID { get; set; }
        public string APIFunNM { get; set; }

        public string ClientSysID { get; set; }
        public string ClientSysNM { get; set; }
        public string ClientUserNM { get; set; }
        public DateTime ClientDTBegin { get; set; }
        public DateTime? ClientDTEnd { get; set; }

        public string IPAddress { get; set; }
        public string REQHeaders { get; set; }
        public string REQUrl { get; set; }
        public string REQReturn { get; set; }
    }
}
