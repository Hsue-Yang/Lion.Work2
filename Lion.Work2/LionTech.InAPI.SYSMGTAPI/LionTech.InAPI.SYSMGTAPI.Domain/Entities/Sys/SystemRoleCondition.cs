using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemRoleCondition
    {
        public string SysID { get; set; }
        public string RoleConditionID { get; set; }
        public string RoleConditionNM { get; set; }
        public string SortOrder { get; set; }
        public string Remark { get; set; }
        public string UpdUserID { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
        public string SysRole { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class SystemRoleConditionDetail
    {
        [BsonElement("SYS_ID")]
        public string SysID { get; set; }

        [BsonElement("ROLE_CONDITION_ID")]
        public string RoleConditionID { get; set; }

        [BsonElement("ROLE_CONDITION_NM_ZH_TW")]
        public string RoleConditionNMZHTW { get; set; }

        [BsonElement("ROLE_CONDITION_NM_ZH_CN")]
        public string RoleConditionNMZHCN { get; set; }

        [BsonElement("ROLE_CONDITION_NM_EN_US")]
        public string RoleConditionNMENUS { get; set; }

        [BsonElement("ROLE_CONDITION_NM_TH_TH")]
        public string RoleConditionNMTHTH { get; set; }

        [BsonElement("ROLE_CONDITION_NM_JA_JP")]
        public string RoleConditionNMJAJP { get; set; }

        [BsonElement("ROLE_CONDITION_NM_KO_KR")]
        public string RoleConditionNMKOKR { get; set; }

        [BsonElement("ROLE_CONDITION_SYNTAX")]
        public string RoleConditionSynTax { get; set; }

        [BsonElement("SORT_ORDER")]
        public string SortOrder { get; set; }

        [BsonElement("REMARK")]
        public string Remark { get; set; }
        [BsonElement("UPD_USER_ID")]
        public string UpdUserID { get; set; }
        [BsonElement("UPD_DT")]
        public DateTime UpdDT { get; set; }

        [BsonElement("ROLES")]
        public string RoleID { get; set; }

        public string SysRole { get; set; }

        [BsonElement("ROLE_CONDITION_RULES")]
        public RecordLogSystemRoleConditionGroupRule RoleConditionRules { get; set; }
    }

    public class SystemRoleConditionDetailPara
    {
        public SysSystemRoleCondition SysSystemRoleCondition { get; set; }

        public List<SystemRoleConditionCollect> SystemRoleConditionCollect { get; set; }
    }

    public class SysSystemRoleCondition
    {
        public string SysID { get; set; }
        public string RoleConditionID { get; set; }
        public string RoleConditionNMZHTW { get; set; }
        public string RoleConditionNMZHCN { get; set; }
        public string RoleConditionNMENUS { get; set; }
        public string RoleConditionNMTHTH { get; set; }
        public string RoleConditionNMJAJP { get; set; }
        public string RoleConditionNMKOKR { get; set; }
        public string RoleConditionSyntax { get; set; }
        public string SortOrder { get; set; }
        public string Remark { get; set; }
        public string UpdUserID { get; set; }
    }

    public class SystemRoleConditionCollect
    {
        public string SysID { get; set; }
        public string RoleConditionID { get; set; }
        public string RoleID { get; set; }
        public string UpdUserID { get; set; }
    }

    public class SysRoleConditionDetailPara
    {
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string RoleConditionID { get; set; }
        public string RoleConditionNMZHTW { get; set; }
        public string RoleConditionNMZHCN { get; set; }
        public string RoleConditionNMENUS { get; set; }
        public string RoleConditionNMTHTH { get; set; }
        public string RoleConditionNMJAJP { get; set; }
        public string RoleConditionNMKOKR { get; set; }
        public string RoleConditionSyntax { get; set; }
        public string SortOrder { get; set; }
        public string Remark { get; set; }
        public string UpdUserID { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
        public List<string> RoleList { get; set; }
        public RecordLogSystemRoleConditionGroupRule RoleConditionRules { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class SystemRoleCondotionMongo
    {
        [BsonElement("SYS_ID")]
        public string SysID { get; set; }

        [BsonElement("ROLE_CONDITION_ID")]
        public string RoleConditionID { get; set; }

        [BsonElement("ROLE_CONDITION_RULES")]
        public RecordLogSystemRoleConditionGroupRule RoleConditionRules { get; set; }
    }

    public class RecordLogSystemRoleConditionGroupRule
    {
        [BsonElement("CONDITION")]
        public string Condition { get; set; }

        [BsonElement("RULE_LIST")]
        public List<RecordLogSystemRoleConditionRoleRule> RuleList { get; set; }

        [BsonElement("GROUP_RULE_LIST")]
        public List<RecordLogSystemRoleConditionGroupRule> GroupRuleList { get; set; }
    }

    public class RecordLogSystemRoleConditionRoleRule
    {
        [BsonElement("ID")]
        public string ID { get; set; }

        [BsonElement("IDNM")]
        public string IDNM { get; set; }

        [BsonElement("FIELD")]
        public string Field { get; set; }

        [BsonElement("FIELD_TYPE")]
        public string FieldType { get; set; }

        [BsonElement("INPUT")]
        public string Input { get; set; }

        [BsonElement("OPERATOR")]
        public string Operator { get; set; }

        [BsonElement("VALUE")]
        public string Value { get; set; }
    }

    public class MongoSystemRoleConditionDetail
    {
        [BsonElement("SYS_ID")]
        public string SysID { get; set; }

        [BsonElement("SYS_NM")]
        public string SysNM { get; set; }

        [BsonElement("ROLE_CONDITION_ID")]
        public string RoleConditionID { get; set; }

        [BsonElement("ROLES")]
        public List<string> Roles { get; set; }

        [BsonElement("ROLE_CONDITION_NM_ZH_TW")]
        public string RoleConditionNMZHTW { get; set; }

        [BsonElement("ROLE_CONDITION_NM_ZH_CN")]
        public string RoleConditionNMZHCN { get; set; }

        [BsonElement("ROLE_CONDITION_NM_EN_US")]
        public string RoleConditionNMENUS { get; set; }

        [BsonElement("ROLE_CONDITION_NM_TH_TH")]
        public string RoleConditionNMTHTH { get; set; }

        [BsonElement("ROLE_CONDITION_NM_JA_JP")]
        public string RoleConditionNMJAJP { get; set; }

        [BsonElement("ROLE_CONDITION_NM_KO_KR")]
        public string RoleConditionNMKOKR { get; set; }

        [BsonElement("ROLE_CONDITION_SYNTAX")]
        public string RoleConditionSynTax { get; set; }

        [BsonElement("SORT_ORDER")]
        public string SortOrder { get; set; }

        [BsonElement("REMARK")]
        public string Remark { get; set; }

        [BsonElement("UPD_USER_ID")]
        public string UpdUserID { get; set; }

        [BsonElement("UPD_USER_NM")]
        public string UpdUserNM { get; set; }

        [BsonElement("UPD_DT")]
        public DateTime UpdDT { get; set; }

        [BsonElement("ROLE_CONDITION_RULES")]
        public RecordLogSystemRoleConditionGroupRule RoleConditionRules { get; set; }
    }
}
