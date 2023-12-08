using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemLine
    {
        public string SysID { get; set; }
        public string LineID { get; set; }
        public string LineNM { get; set; }
        public string LineNMID { get; set; }
        public string LineNMZHTW { get; set; }
        public string LineNMZHCN { get; set; }
        public string LineNMENUS { get; set; }
        public string LineNMTHTH { get; set; }
        public string LineNMJAJP { get; set; }
        public string LineNMKOKR { get; set; }
        public string ChannelID { get; set; }
        public string ChannelSecret { get; set; }
        public string ChannelAccessToken { get; set; }
        public string IsDisable { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserNM { get; set; }
        public string UpdUserID { get; set; }
        public DateTime UpdDT { get; set; }
    }
}