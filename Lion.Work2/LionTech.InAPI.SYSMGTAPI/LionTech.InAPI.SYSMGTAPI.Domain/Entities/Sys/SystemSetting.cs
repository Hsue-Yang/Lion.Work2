using System;
using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemSetting
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }

        public string SysMANUserID { get; set; }
        public string SysMANUserNM { get; set; }

        public string IsAP { get; set; }
        public string IsAPI { get; set; }
        public string IsEDI { get; set; }
        public string IsEvent { get; set; }

        public string SysIndexPath { get; set; }
        public string SysIconPath { get; set; }
        public string IsOutsourcing { get; set; }
        public string IsDisable { get; set; }
        public string SortOrder { get; set; }

        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }

        public List<SystemSub> SubsystemList { get; set; }

        public class SystemSub
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }
            
            public string AP { get; set; }
            public string API { get; set; }
            public string EDI { get; set; }
            public string Event { get; set; }

            public string SysMANUserNM { get; set; }

            public string ParentSysID { get; set; }
            public string SortOrder { get; set; }

            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }
    }

    public class SystemSysID
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }
    }

    public class SystemRoleGroup
    {
        public string ROLE_GROUP_ID { get; set; }
        public string ROLE_GROUP_NM { get; set; }
    }

    public class SystemConditionID
    {
        public string RoleConditionID { get; set; }
        public string RoleConditionNM { get; set; }
    }
}
