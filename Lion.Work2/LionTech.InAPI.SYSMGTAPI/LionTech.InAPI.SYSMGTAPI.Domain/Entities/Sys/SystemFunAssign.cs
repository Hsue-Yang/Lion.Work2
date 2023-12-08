using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemFunAssign
    {
        public string UserID { get; set; }
        public string UserNM { get; set; }
    }

    public class SystemFunAssignPara
    {
        public string SysID { get; set; }
        public string FunControllerID { get; set; }
        public string FunActionName { get; set; }
        public List<string> UserIDList { get; set; }
        public string UpdUserID { get; set; }
        public string ErpWFNO { get; set; }
        public string Memo { get; set; }
    }

    public class SystemFunAssignUser
    {
        public string UserID { get; set; }
    }

    public class SysFunRawData
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string FunControllerID { get; set; }
        public string FunControllerNMID { get; set; }
        public string FunControllerNM { get; set; }
        public string FunActionID { get; set; }
        public string FunActionNM { get; set; }
        public string FunActionNMID { get; set; }
    }
}