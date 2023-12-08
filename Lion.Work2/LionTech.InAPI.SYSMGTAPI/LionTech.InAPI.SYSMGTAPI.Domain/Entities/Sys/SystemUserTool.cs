using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemUserToolPara
    {
        public string CopyToUserID { get; set; }
        public string UpdUserID { get; set; }
        public List<SystemUserTool> SystemUserToolList { get; set; }
    }

    public class SystemUserTool
    {
        public string UserID { get; set; }
        public string SysID { get; set; }
        public string FunControllerID { get; set; }
        public string FunActionName { get; set; }
        public string ToolNO { get; set; }
        public string ToolNM { get; set; }
        public string AfterSortOrder { get; set; }
        public string BeforeSortOrder { get; set; }
    }
}
