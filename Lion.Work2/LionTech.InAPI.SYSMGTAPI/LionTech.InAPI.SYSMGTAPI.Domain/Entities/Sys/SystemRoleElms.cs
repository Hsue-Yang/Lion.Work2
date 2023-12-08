using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemRoleElms
    {
        public string FunControllerID { get; set; }
        public string FunActionNM { get; set; }
        public string ElmID { get; set; }
        public string ElmNM { get; set; }
        public string IsDisable { get; set; }
        public string DisplaySts { get; set; }
        public string UpdUserIDNM { get; set; }
        public DateTime UpdDT { get; set; }
    }

    public class SystemRoleElmEdit
    {
        [BsonElement("SYS_ID")]
        public string SYS_ID { get; set; }
        [BsonElement("ROLE_ID")]
        public string ROLE_ID { get; set; }
        [BsonElement("CONTROLLER_ID")]
        public string FUN_CONTROLLER_ID { get; set; }
        [BsonElement("ACTION_NAME")]
        public string FUN_ACTION_NAME { get; set; }
        [BsonElement("ELM_ID")]
        public string ELM_ID { get; set; }
        [BsonElement("DISPLAY_STS")]
        public int DISPLAY_STS { get; set; }
        public string UPD_USER_ID { get; set; }
    }

    public class SystemRoleElmEditLists
    {
        public List<SystemRoleElmEdit> SystemRoleElmAddList { get; set; }
        public List<SystemRoleElmEdit> SystemRoleElmDeleteList { get; set; }
    }

    public class SysElmName
    {
        public string ElmNM { get; set; }
        public string ElmID { get; set; }
    }
}