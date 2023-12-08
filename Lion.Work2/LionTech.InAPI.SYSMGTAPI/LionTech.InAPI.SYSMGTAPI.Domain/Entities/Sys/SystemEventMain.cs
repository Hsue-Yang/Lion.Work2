namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemEventMain
    {
        public string SysID { get; set; }
        public string EventGroupID { get; set; }
        public string EventID { get; set; }

        public string EventNMZHTW { get; set; }
        public string EventNMZHCN { get; set; }
        public string EventNMENUS { get; set; }
        public string EventNMTHTH { get; set; }
        public string EventNMJAJP { get; set; }
        public string EventNMKOKR { get; set; }

        public string EventPara { get; set; }
        public string IsDisable { get; set; }
        public string SortOrder { get; set; }
        public string Remark { get; set; }
        public string UpdUserID { get; set; }
    }
}
