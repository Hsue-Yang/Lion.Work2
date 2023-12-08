using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemCulture
    {
        public string CultureID { get; set; }
        public string CultureNM { get; set; }
        public string DisplayNM { get; set; }
        public string IsSerpUse { get; set; }
        public string IsDisable { get; set; }
        public string UpdUserID { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}