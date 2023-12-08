using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class UserFunction
    {
        public string HasAuth { get; set; }
        [BsonElement("USER_ID")]
        public string UserID { get; set; }
        [BsonElement("USER_NM")]
        public string UserNM { get; set; }
        [BsonElement("SYS_ID")]
        public string SysID { get; set; }
        [BsonElement("SYS_NM")]
        public string SysNM { get; set; }
        public string SysNMID { get; set; }
        [BsonElement("FUN_CONTROLLER_ID")]
        public string FunControllerID { get; set; }
        public string FunControllerNM { get; set; }
        [BsonElement("FUN_CONTROLLER_NM")]
        public string FunGroupNM { get; set; }
        public string FunGroupNMID { get; set; }
        [BsonElement("FUN_ACTION_NAME")]
        public string FunActionName { get; set; }
        [BsonElement("FUN_NM")]
        public string FunNM { get; set; }
        public string FunNMID { get; set; }
        public string UpdUserID { get; set; }
        public string UpdUserNM { get; set; }
        public string ModifyType { get; set; }
        public string ModifyTypeNM { get; set; }
        public DateTime UpdDT { get; set; }
    }

    public class RecordUserFunApplyModifyItem
    {
        public string HasAuth { get; set; }
        public string UserID { get; set; }
        public string UserNM { get; set; }
        [BsonElement("SYS_ID")]
        public string SysID { get; set; }
        [BsonElement("SYS_NM")]
        public string SysNM { get; set; }
        public string SysNMID { get; set; }
        [BsonElement("FUN_CONTROLLER_ID")]
        public string FunControllerID { get; set; }
        public string FunControllerNM { get; set; }
        [BsonElement("FUN_CONTROLLER_NM")]
        public string FunGroupNM { get; set; }
        public string FunGroupNMID { get; set; }
        [BsonElement("FUN_ACTION_NAME")]
        public string FunActionName { get; set; }
        [BsonElement("FUN_NM")]
        public string FunNM { get; set; }
        public string FunNMID { get; set; }
        public string UpdUserID { get; set; }
        public string UpdUserNM { get; set; }
        [BsonElement("MODIFY_TYPE")]
        public string ModifyType { get; set; }
        [BsonElement("MODIFY_TYPE_NM")]
        public string ModifyTypeNM { get; set; }
        public DateTime UpdDT { get; set; }
    }

    public class UserFunctionDetail
    {
        [BsonElement("USER_ID")]
        public string UserID { get; set; }
        [BsonElement("USER_NM")]
        public string UserNM { get; set; }
        public string IsDisable { get; set; }
        public string UpdUserID { get; set; }
        public List<UserFunctionValue> FunctionList { get; set; }
        [BsonElement("MODIFY_LIST")]
        public List<RecordUserFunApplyModifyItem> ModifyList { get; set; }
        [BsonElement("BaseLine_DT")]
        public DateTime? BaseLineDT { get; set; }
        public string ErpWFNO { get; set; }
        public string Memo { get; set; }
    }

    public class UserFunctionValue
    {
        public string SysID { get; set; }
        public string FunControllerID { get; set; }
        public string FunActionName { get; set; }
    }
}