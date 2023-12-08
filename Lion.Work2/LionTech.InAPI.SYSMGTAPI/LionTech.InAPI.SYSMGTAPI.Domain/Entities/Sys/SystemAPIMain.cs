using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemAPIMain
    {
        public string SysID { get; set; }
        public string APIGroupID { get; set; }
        public string APIFunID { get; set; }

        public string APINMZHTW { get; set; }
        public string APINMZHCN { get; set; }
        public string APINMENUS { get; set; }
        public string APINMTHTH { get; set; }
        public string APINMJAJP { get; set; }
        public string APINMKOKR { get; set; }

        public string APIPara { get; set; }
        public string APIReturn { get; set; }
        public string APIParaDesc { get; set; }
        public string APIReturnContent { get; set; }

        public string IsOutside { get; set; }
        public string IsDisable { get; set; }
        public string SortOrder { get; set; }

        public string UpdUserID { get; set; }
        public IEnumerable<string> RoleIDs { get; set; }
    }
}
