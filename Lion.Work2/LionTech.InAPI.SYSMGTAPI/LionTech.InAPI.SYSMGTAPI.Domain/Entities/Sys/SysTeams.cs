using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SysTeams
    {
        public string SysID { get; set; }
        public string TeamsChannelID { get; set; }
        public string TeamsChannelNM { get; set; }
        public string TeamsChannelNMZHTW { get; set; }
        public string TeamsChannelNMZHCN { get; set; }
        public string TeamsChannelNMENUS { get; set; }
        public string TeamsChannelNMTHTH { get; set; }
        public string TeamsChannelNMJAJP { get; set; }
        public string TeamsChannelNMKOKR { get; set; }
        public string TeamsChannelUrl { get; set; }
        public string IsDisable { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}
