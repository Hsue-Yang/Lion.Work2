using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemAPIGroup
    {
        public string APIGroupID { get; set; }
        public string APIGroupNM { get; set; }

        public string SortOrder { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }
}
