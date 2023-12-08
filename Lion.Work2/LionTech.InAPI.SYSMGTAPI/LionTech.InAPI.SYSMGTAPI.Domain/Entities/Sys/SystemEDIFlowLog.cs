using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities
{
    public class SystemEDIFlowLog
    {
        public string EDINO { get; set; }
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string EDIFlowID { get; set; }
        public string EDIFlowNM { get; set; }
        public string EDIDate { get; set; }
        public string EDITime { get; set; }
        public string DataDate { get; set; }
        public string StatusID { get; set; }
        public string ResultID { get; set; }
        public string ResultCode { get; set; }
        public string EDIFlowStatusNM { get; set; }
        public string EDIFlowResultNM { get; set; }
        public DateTime DTBegin { get; set; }
        public DateTime DTEnd { get; set; }
        public string IsAutomatic { get; set; }
        public DateTime AutoSchedule { get; set; }
        public string AutoEDINO { get; set; }
        public string AutoFlowID { get; set; }
        public string IsDeleted { get; set; }
        public string UpdUserID { get; set; }
        public DateTime UpdDt { get; set; }
    }

    public class SystemEDIFlowLogUpdateWaitStatus
    {
        public string SysID { get; set; }
        public string EDIFlowID { get; set; }
        public string DataDate { get; set; }
        public string UpdUserID { get; set; }
    }
}
