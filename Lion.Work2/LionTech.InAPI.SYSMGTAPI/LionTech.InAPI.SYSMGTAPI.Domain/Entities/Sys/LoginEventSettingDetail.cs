using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class LoginEventSettingDetail
    {
        public string SysID { get; set; }
        public string LoginEventID { get; set; }
        public string LoginEventNMZHCN { get; set; }
        public string LoginEventNMZHTW { get; set; }
        public string LoginEventNMENUS { get; set; }
        public string LoginEventNMTHTH { get; set; }
        public string LoginEventNMJAJP { get; set; }
        public string LoginEventNMKOKR { get; set; }
        public DateTime StartDT { get; set; }
        public DateTime EndDT { get; set; }
        public string Frequency { get; set; }
        public string StartExecTime { get; set; }
        public string EndExecTime { get; set; }
        public string TargetPath { get; set; }
        public string ValidPath { get; set; }
        public string SubSysID { get; set; }
        public string IsDisable { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }
    }

    public class LoginEventSettingValue
    {
        public string SYS_ID { get; set; }
        public string SORT_ORDER { get; set; }
        public string UPD_USER_ID { get; set; }
        public string LOGIN_EVENT_ID { get; set; }
    }
}
