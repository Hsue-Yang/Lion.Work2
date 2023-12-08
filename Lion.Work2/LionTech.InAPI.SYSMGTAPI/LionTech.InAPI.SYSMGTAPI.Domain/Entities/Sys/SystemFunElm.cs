using MongoDB.Bson.Serialization.Attributes;
using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemFunElm
    {
        [BsonElement("ELM_ID")]
        public string ElmID { get; set; }
        [BsonElement("SYS_ID")]
        public string SysID { get; set; }
        [BsonElement("SYS_NM")]
        public string SysNM { get; set; }
        [BsonElement("CONTROLLER_NAME")]
        public string FunControllerID { get; set; }
        [BsonElement("ACTION_NAME")]
        public string FunActionNM { get; set; }
        public string ElmNM { get; set; }
        public string ElmNMID { get; set; }
        public string FnNMID { get; set; }
        public string FnGroupNMID { get; set; }
        public int DefaultDisplaySts { get; set; }
        [BsonElement("DEFAULT_DISPLAY_STS")]
        public string DefaultDisplay { get; set; }
        [BsonElement("IS_DISABLE")]
        public string IsDisable { get; set; }
        public string UpdUserIDNM { get; set; }
        [BsonElement("ELM_NM_ZH_TW")]
        public string ElmNMZHTW { get; set; }
        [BsonElement("ELM_NM_ZH_CN")]
        public string ElmNMZHCN { get; set; }
        [BsonElement("ELM_NM_EN_US")]
        public string ElmNMENUS { get; set; }
        [BsonElement("ELM_NM_TH_TH")]
        public string ElmNMTHTH { get; set; }
        [BsonElement("ELM_NM_JA_JP")]
        public string ElmNMJAJP { get; set; }
        [BsonElement("ELM_NM_KO_KR")]
        public string ElmNMKOKR { get; set; }
        public string UpdUserID { get; set; }
        public DateTime UpdDT { get; set; }
    }

    public class ElmRoleInfoValue
    {
        public string SYS_ID { get; set; }
        public string ROLE_ID { get; set; }
        public string FUN_CONTROLLER_ID { get; set; }
        public string FUN_ACTION_NAME { get; set; }
        public string ELM_ID { get; set; }
        public int DISPLAY_STS { get; set; }
        public string UPD_USER_ID { get; set; }
    }

    public class SystemFunController
    {
        public string SysID { get; set; }
        public string FunControllerID { get; set; }
        public string FunControllerNM { get; set; }
    }
}