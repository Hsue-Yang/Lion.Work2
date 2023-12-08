using System;
using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class UserSystem
    {
        public string UserID { get; set; }
        public string UserNM { get; set; }
        public string UnitID { get; set; }
        public string UnitNM { get; set; }
        public string IsLeft { get; set; }
        public string IsDisable { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
    }

    public class UserSystemRole
    {
        public string DeptID { get; set; }
        public string DeptNM { get; set; }
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string HasSys { get; set; }
    }

    public class UserSystemRolePara
    {
        public string SysID { get; set; }
    }

    public class UserSystemRoleParas
    {
        public string UserID { get; set; }
        public string UpdUserID { get; set; }
        public List<string> UserSystemRoleList { get; set; }
    }
}