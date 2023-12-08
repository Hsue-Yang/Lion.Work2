using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemRoleFun
    {
        [BsonElement("SYS_ID")]
        public string SysID { get; set; }
        [BsonElement("SYS_NM")]
        public string SysNM { get; set; }
        [BsonElement("FUN_CONTROLLER_ID")]
        public string FunControllerID { get; set; }
        [BsonElement("FUN_GROUP_NM")]
        public string FunGroupNM { get; set; }
        [BsonElement("FUN_ACTION_NAME")]
        public string FunActionName { get; set; }
        [BsonElement("FUN_NM")]
        public string FunNM { get; set; }
        [BsonElement("ROLE_ID")]
        public string RoleID { get; set; }
        [BsonElement("ROLE_NM")]
        public string RoleNM { get; set; }

        public string HasRole { get; set; }
        public string UpdUserID { get; set; }
    }

    public class SystemRoleFunEdit
    {
        public string SysID { get; set; }
        public string RoleID { get; set; }
        public string FunControllerID { get; set; }
        public string FunActionName { get; set; }
        public string UpdUserID { get; set; }
    }

    public class SystemRoleFunEditLists
    {
        public List<SystemRoleFunEdit> SystemRoleFunsAddList { get; set; }
        public List<SystemRoleFunEdit> SystemRoleFunsDeleteList { get; set; }
    }

    public class SystemRoleFunList
    {
        public string SubSysNM { get; set; }
        public string SysID { get; set; }
        public string FunGroupNM { get; set; }
        public string FunActionNMID { get; set; }
        public string FunControllerID { get; set; }
        public string FunActionName { get; set; }
        public string UpdUserNM { get; set; }
        public string UpdUserID { get; set; }
        public DateTime UpdDT { get; set; }
    }

}
