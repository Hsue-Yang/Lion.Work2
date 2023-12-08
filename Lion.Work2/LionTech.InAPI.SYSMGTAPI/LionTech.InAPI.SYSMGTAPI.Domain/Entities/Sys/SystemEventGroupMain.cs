namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemEventGroupMain
    {
        public string SysID { get; set; }
        public string EventGroupID { get; set; }

        public string EventGroupZHTW { get; set; }
        public string EventGroupZHCN { get; set; }
        public string EventGroupENUS { get; set; }
        public string EventGroupTHTH { get; set; }
        public string EventGroupJAJP { get; set; }
        public string EventGroupKOKR { get; set; }

        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }
    }
}
