using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemLineReceiver
    {
        public string SysID { get; set; }
        public string LineID { get; set; }
        public string LineNMID { get; set; }
        public string LineReceiverID { get; set; }
        public string LineReceiverNM { get; set; }
        public string LineReceiverNMZHTW { get; set; }
        public string LineReceiverNMZHCN { get; set; }
        public string LineReceiverNMENUS { get; set; }
        public string LineReceiverNMTHTH { get; set; }
        public string LineReceiverNMJAJP { get; set; }
        public string LineReceiverNMKOKR { get; set; }
        public string SourceType { get; set; }
        public string IsDisable { get; set; }
        public string ReceiverID { get; set; }
        public string UpdUserNM { get; set; }
        public string UpdUserID { get; set; }
        public DateTime UpdDT { get; set; }
    }
}