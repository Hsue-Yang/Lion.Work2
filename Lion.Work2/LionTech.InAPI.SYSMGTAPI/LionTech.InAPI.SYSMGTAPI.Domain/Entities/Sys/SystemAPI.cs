using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemAPI
    {
        public string SysNM { get; set; }
        public string APIGroupID { get; set; }
        public string APIGroupNM { get; set; }

        public string APIFunID { get; set; }
        public string APIFunNM { get; set; }
        public string APIPara { get; set; }
        public string APIReturn { get; set; }
        public string APIParaDesc { get; set; }
        public string APIReturnContent { get; set; }

        public string IsOutside { get; set; }
        public string IsDisable { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}