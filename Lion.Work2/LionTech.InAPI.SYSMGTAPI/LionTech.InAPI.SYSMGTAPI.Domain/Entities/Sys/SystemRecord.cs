using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class LogUserSystemRole
    {
        public string UserID { get; set; }
        public string UserNM { get; set; }
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string RoleID { get; set; }
        public string RoleNM { get; set; }
        public string ErpWFNo { get; set; }
        public string LogNo { get; set; }
        public string APINo { get; set; }
        public string UpdUserID { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
        public string ExecSysID { get; set; }
        public string ExecSysNM { get; set; }
        public string ExecIPAddress { get; set; }
        public string RoleConditionRules { get; set; }
    }

    public class LogPara
    {
        public string CollectionNM { get; set; }
        public string UserID { get; set; }
        public string SysID { get; set; }
        public string UserIDListStr { get; set; }
        public string RoleConditionID { get; set; }
        public string LineID { get; set; }
        public string FunControllerID { get; set; }
        public string FunActionName { get; set; }
        public string WFNo { get; set; }
        public string LogNo { get; set; }
        public DateTime? SUpdDT { get; set; }
        public DateTime? EUpdDT { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class SysRecord
    {
        [BsonElement("LOG_NO")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LogNo { get; set; }

        [BsonElement("API_NO")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string APINo { get; set; }

        [BsonElement("UPD_USER_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UpdUserID { get; set; }

        [BsonElement("UPD_USER_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UpdUserNM { get; set; }

        [BsonElement("UPD_DT")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime UpdDT { get; set; }

        [BsonElement("EXEC_SYS_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ExecSysID { get; set; }

        [BsonElement("EXEC_SYS_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ExecSysNM { get; set; }

        [BsonElement("EXEC_IP_ADDRESS")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ExecIPAddress { get; set; }

        [BsonElement("USER_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserID { get; set; }

        [BsonElement("USER_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserNM { get; set; }

        [BsonElement("USER_PWD")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserPWD { get; set; }

        [BsonElement("LOCATION")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Location { get; set; }

        [BsonElement("LOCATION_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LocationNM { get; set; }

        [BsonElement("LOCATION_DESC")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LocationDesc { get; set; }

        [BsonElement("USER_IDNO")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserIDNo { get; set; }

        [BsonElement("USER_BIRTHDAY")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserBirthday { get; set; }

        [BsonElement("IP_ADDRESS")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string IPAddress { get; set; }

        [BsonElement("VALID_RESULT")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ValidResult { get; set; }

        [BsonElement("VALID_RESULT_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ValidResultNM { get; set; }

        [BsonElement("MODIFY_TYPE")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ModifyType { get; set; }

        [BsonElement("MODIFY_TYPE_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ModifyTypeNM { get; set; }

        [BsonElement("IS_LEFT")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string IsLeft { get; set; }

        [BsonElement("USER_COM_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserComID { get; set; }

        [BsonElement("USER_COM_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserComNM { get; set; }

        [BsonElement("USER_UNIT_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserUnitID { get; set; }

        [BsonElement("USER_UNIT_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserUnitNM { get; set; }

        [BsonElement("USER_TEAM_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserTeamID { get; set; }

        [BsonElement("USER_TITLE_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserTitleID { get; set; }

        [BsonElement("USER_TITLE_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserTitleNM { get; set; }

        [BsonElement("USER_WORK_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserWorkID { get; set; }

        [BsonElement("USER_WORK_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserWorkNM { get; set; }

        [BsonElement("USER_ORG_WORKCOM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgWorkCom { get; set; }

        [BsonElement("USER_ORG_WORKCOM_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgWorkComNM { get; set; }

        [BsonElement("USER_ORG_AREA")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgArea { get; set; }

        [BsonElement("USER_ORG_AREA_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgAreaNM { get; set; }

        [BsonElement("USER_ORG_GROUP")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgGroup { get; set; }

        [BsonElement("USER_ORG_GROUP_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgGroupNM { get; set; }

        [BsonElement("USER_ORG_PLACE")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgPlace { get; set; }

        [BsonElement("USER_ORG_PLACE_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgPlaceNM { get; set; }

        [BsonElement("USER_ORG_DEPT")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgDept { get; set; }

        [BsonElement("USER_ORG_DEPT_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgDeptNM { get; set; }

        [BsonElement("USER_ORG_TEAM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgTeam { get; set; }

        [BsonElement("USER_ORG_TEAM_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgTeamNM { get; set; }

        [BsonElement("USER_ORG_JOB_TITLE")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgJobTitle { get; set; }

        [BsonElement("USER_ORG_JOB_TITLE_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgJobTitleNM { get; set; }

        [BsonElement("USER_ORG_BIZ_TITLE")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgBizTitle { get; set; }

        [BsonElement("USER_ORG_BIZ_TITLE_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgBizTitleNM { get; set; }

        [BsonElement("USER_ORG_TITLE")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgTitle { get; set; }

        [BsonElement("USER_ORG_TITLE_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgTitleNM { get; set; }

        [BsonElement("USER_ORG_LEVEL")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgLevel { get; set; }

        [BsonElement("USER_ORG_LEVEL_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserOrgLevelNM { get; set; }

        [BsonElement("RESTRICT_TYPE")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RestrictType { get; set; }

        [BsonElement("RESTRICT_TYPE_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RestrictTypeNM { get; set; }

        [BsonElement("IS_LOCK")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string IsLock { get; set; }

        [BsonElement("IS_DISABLE")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string IsDisable { get; set; }

        [BsonElement("MODIFY_DATE")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ModifyDate { get; set; }

        [BsonElement("IS_RESET")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string IsReset { get; set; }

        [BsonElement("SYS_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SysID { get; set; }

        [BsonElement("SYS_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SysNM { get; set; }

        [BsonElement("FUN_CONTROLLER_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunControllerID { get; set; }

        [BsonElement("FUN_CONTROLLER_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunControllerNM { get; set; }

        [BsonElement("FUN_ACTION_NAME")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunActionName { get; set; }

        [BsonElement("FUN_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunNM { get; set; }

        [BsonElement("ERP_WFNO")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ErpWFNo { get; set; }

        [BsonElement("PURVIEW_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string PurviewID { get; set; }

        [BsonElement("PURVIEW_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string PurviewNM { get; set; }

        [BsonElement("CODE_TYPE")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string CodeType { get; set; }

        [BsonElement("PURVIEW_COLLECT_LIST")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<LogUserPurviewCollect> PurviewCollectList { get; set; }

        [BsonElement("ROLE_CATEGORY_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleCategoryID { get; set; }

        [BsonElement("ROLE_CATEGORY_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleCategoryNM { get; set; }

        [BsonElement("ROLE_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleID { get; set; }

        [BsonElement("ROLE_NM_ZH_TW")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleNMZHTW { get; set; }

        [BsonElement("ROLE_NM_ZH_CN")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleNMZHCN { get; set; }

        [BsonElement("ROLE_NM_EN_US")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleNMENUS { get; set; }

        [BsonElement("ROLE_NM_TH_TH")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleNMTHTH { get; set; }

        [BsonElement("ROLE_NM_JA_JP")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleNMJAJP { get; set; }

        [BsonElement("ROLE_NM_KO_KR")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleNMKOKR { get; set; }

        [BsonElement("IS_MASTER")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string IsMaster { get; set; }

        [BsonElement("ROLE_CONDITION_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleConditionID { get; set; }

        [BsonElement("ROLES")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Roles { get; set; }

        [BsonElement("ROLE_CONDITION_RULES")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RecordLogSysRoleConditionGroupRule RoleConditionRules { get; set; }

        [BsonElement("LINE_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LineID { get; set; }

        [BsonElement("LINE_NM_ZH_TW")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LineNMZHTW { get; set; }

        [BsonElement("LINE_NM_ZH_CN")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LineNMZHCN { get; set; }

        [BsonElement("LINE_NM_EN_US")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LineNMENUS { get; set; }

        [BsonElement("LINE_NM_TH_TH")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LineNMTHTH { get; set; }

        [BsonElement("LINE_NM_JA_JP")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LineNMJAJP { get; set; }

        [BsonElement("LINE_NM_KO_KR")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LineNMKOKR { get; set; }

        [BsonElement("SORT_ORDER")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SortOrder { get; set; }

        [BsonElement("LINE_RECEIVER_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LineReceiverID { get; set; }

        [BsonElement("LINE_RECEIVER_NM_ZH_TW")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LineReceiverNMZHTW { get; set; }

        [BsonElement("LINE_RECEIVER_NM_ZH_CN")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LineReceiverNMZHCN { get; set; }

        [BsonElement("LINE_RECEIVER_NM_EN_US")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LineReceiverNMENUS { get; set; }

        [BsonElement("LINE_RECEIVER_NM_TH_TH")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LineReceiverNMTHTH { get; set; }

        [BsonElement("LINE_RECEIVER_NM_JA_JP")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LineReceiverNMJAJP { get; set; }

        [BsonElement("LINE_RECEIVER_NM_KO_KR")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LineReceiverNMKOKR { get; set; }

        [BsonElement("SOURCE_TYPE")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SourceType { get; set; }

        [BsonElement("FUN_GROUP_ZH_TW")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunGroupZHTW { get; set; }

        [BsonElement("FUN_GROUP_ZH_CN")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunGroupZHCN { get; set; }

        [BsonElement("FUN_GROUP_EN_US")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunGroupENUS { get; set; }

        [BsonElement("FUN_GROUP_TH_TH")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunGroupTHTH { get; set; }

        [BsonElement("FUN_GROUP_JA_JP")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunGroupJAJP { get; set; }

        [BsonElement("FUN_GROUP_KO_KR")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunGroupKOKR { get; set; }

        [BsonElement("SUB_SYS_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SubSysID { get; set; }

        [BsonElement("SUB_SYS_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SubSysNM { get; set; }

        [BsonElement("FUN_GROUP_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunGroupNM { get; set; }

        [BsonElement("FUN_NM_ZH_TW")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunNMZHTW { get; set; }

        [BsonElement("FUN_NM_ZH_CN")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunNMZHCN { get; set; }

        [BsonElement("FUN_NM_EN_US")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunNMENUS { get; set; }

        [BsonElement("FUN_NM_TH_TH")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunNMTHTH { get; set; }

        [BsonElement("FUN_NM_JA_JP")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunNMJAJP { get; set; }

        [BsonElement("FUN_NM_KO_KR")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunNMKOKR { get; set; }

        [BsonElement("FUN_TYPE")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunType { get; set; }

        [BsonElement("FUN_TYPE_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FunTypeNM { get; set; }

        [BsonElement("IS_OUTSIDE")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string IsOutside { get; set; }

        [BsonElement("ROLE_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleNM { get; set; }

        [BsonElement("ROLE_GROUP_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleGroupID { get; set; }

        [BsonElement("ROLE_GROUP_NM_ZH_TW")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleGroupNMZHTW { get; set; }

        [BsonElement("ROLE_GROUP_NM_ZH_CN")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleGroupNMZHCN { get; set; }

        [BsonElement("ROLE_GROUP_NM_EN_US")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleGroupNMENUS { get; set; }

        [BsonElement("ROLE_GROUP_NM_TH_TH")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleGroupNMTHTH { get; set; }

        [BsonElement("ROLE_GROUP_NM_JA_JP")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleGroupNMJAJP { get; set; }

        [BsonElement("ROLE_GROUP_NM_KO_KR")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RoleGroupNMKOKR { get; set; }

        [BsonElement("REMARK")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Remark { get; set; }

        [BsonElement("IP_BEGIN")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string IPBegin { get; set; }

        [BsonElement("IP_END")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string IPEnd { get; set; }

        [BsonElement("COM_ID")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ComID { get; set; }

        [BsonElement("COM_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ComNM { get; set; }

        [BsonElement("TRUST_STATUS")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string TrustStatus { get; set; }

        [BsonElement("TRUST_TYPE")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string TrustType { get; set; }

        [BsonElement("TRUST_TYPE_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string TrustTypeNM { get; set; }

        [BsonElement("SOURCE_TYPE_NM")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SourceTypeNM { get; set; }

        [BsonElement("MEMO")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Memo { get; set; }

        [BsonElement("WFNO")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string WFNo { get; set; }
    }

    public class RecordLogSysRoleConditionGroupRule
    {
        [BsonElement("CONDITION")]
        public string Condition { get; set; }

        [BsonElement("RULE_LIST")]
        public List<RecordLogSystemRoleConditionRoleRule> RuleList { get; set; }

        [BsonElement("GROUP_RULE_LIST")]
        public List<RecordLogSystemRoleConditionGroupRule> GroupRuleList { get; set; }
    }

    public class LogUserPurviewCollect
    {
        [BsonElement("CODE_ID")]
        public string CodeID { get; set; }

        [BsonElement("CODE_NM")]
        public string CodeNM { get; set; }

        [BsonElement("CODE_TYPE")]
        public string CodeType { get; set; }

        [BsonElement("CODE_TYPE_NM")]
        public string CodeTypeNM { get; set; }

        [BsonElement("PURVIEW_OP")]
        public string PurviewOP { get; set; }

        [BsonElement("PURVIEW_OP_NM")]
        public string PurviewOPNM { get; set; }
    }
}
