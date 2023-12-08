using System;
using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemFun
    {
        public string SysID { get; set; }
        public string SubSysID { get; set; }
        public string SubSysNM { get; set; }

        public string FunControllerID { get; set; }
        public string FunGroupNM { get; set; }

        public string FunActionName { get; set; }
        public string FunNM { get; set; }

        public string FunMenuSysID { get; set; }
        public string FunMenu { get; set; }

        public string IsDisable { get; set; }
        public string IsOutSide { get; set; }
        public string SortOrder { get; set; }

        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }

        public List<SysMenuFun> MenuFunList { get; set; }
    }
}
