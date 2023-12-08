using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemFunTool
    {
        public string UserID { get; set; }
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string FunControllerID { get; set; }
        public string FunGroupNM { get; set; }
        public string FunActionName { get; set; }
        public string FunNM { get; set; }
        public string ToolNo { get; set; }
        public string ToolNM { get; set; }
        public string ParaID { get; set; }
        public string ParaValue { get; set; }
        public string IsCurrently { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserNM { get; set; }
        public string UpdDt { get; set; }
    }
}
