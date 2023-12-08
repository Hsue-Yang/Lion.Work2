using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemEDIEvent
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }

        public string EventGroupID { get; set; }
        public string EventGroupNM { get; set; }

        public string EventID { get; set; }
        public string EventNM { get; set; }

        public string EDIEventNo { get; set; }

        public string StatusID { get; set; }
        public string StatusNM { get; set; }
        public string ResultID { get; set; }
        public string ResultNM { get; set; }

        public string InsertEDINo { get; set; }
        public string InsertEDIDate { get; set; }
        public string InsertEDITime { get; set; }
        public string ExecEDIEventNo { get; set; }

        public string TargetSysID { get; set; }
        public string TargetSysNM { get; set; }

        public string TargetStatusID { get; set; }
        public string TargetStatusNM { get; set; }
        public string TargetResultID { get; set; }
        public string TargetResultNM { get; set; }
        public string TargetReturnAPINo { get; set; }

        public DateTime? TargetDTBegin { get; set; }
        public DateTime? TargetDTEnd { get; set; }

        public string UpdUserID { get; set; }
    }
}