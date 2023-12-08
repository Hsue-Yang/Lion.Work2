using System;
using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class UserMain
    {
        public string UserID { get; set; }
        public string UserNM { get; set; }
        public string RoleGroupID { get; set; }
        public string IsDisable { get; set; }
    }

    public class SystemRoleGroupCollect
    {
        public string SysID { get; set; }
        public string RoleID { get; set; }
        public string Remark { get; set; }
    }

    public class UserSystemRoleData
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string SysNMID { get; set; }
        public string RoleID { get; set; }
        public string RoleNMID { get; set; }
        public string RoleNM { get; set; }
        public string ModifyType { get; set; }
        public string ModifyTypeNM { get; set; }
        public string HasRole { get; set; }
        public int HasAuth { get; set; }
        public string UpdUserID { get; set; }
        public DateTime UpdDT { get; set; }
    }

    public class UserRoleFunDetailParaList
    {
        public UserRoleFunDetailPara userRoleFunDetailPara { get; set; }
        public List<SystemRoleMain> userRoleFunDetailParaList { get; set; }
    }

    public class UserRoleFunDetailPara
    {
        public string UserID { get; set; }
        public string UserNM { get; set; }
        public string IpAddress { get; set; }
        public string ExecSysID { get; set; }
        public string ErpWFNO { get; set; }
        public string Memo { get; set; }
        public string RoleGroupID { get; set; }
        public string ApiNO { get; set; }
        public string IsDisable { get; set; }
        public string SysID { get; set; }
        public string RoleID { get; set; }
        public string UpdUserID { get; set; }
        public string UpdDT { get; set; }
    }
}
