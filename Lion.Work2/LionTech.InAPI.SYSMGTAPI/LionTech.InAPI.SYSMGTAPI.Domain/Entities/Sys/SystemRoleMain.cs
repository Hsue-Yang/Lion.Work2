using MongoDB.Bson.Serialization.Attributes;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemRoleMain
    {
        [BsonElement("SYS_ID")]
        public string SysID { get; set; }
        [BsonElement("ROLE_CATEGORY_ID")]
        public string RoleCategoryID { get; set; }
        [BsonElement("ROLE_ID")]
        public string RoleID { get; set; }
        [BsonElement("ROLE_NM_ZH_TW")]
        public string RoleNMZHTW { get; set; }
        [BsonElement("ROLE_NM_ZH_CN")]
        public string RoleNMZHCN { get; set; }
        [BsonElement("ROLE_NM_EN_US")]
        public string RoleNMENUS { get; set; }
        [BsonElement("ROLE_NM_TH_TH")]
        public string RoleNMTHTH { get; set; }
        [BsonElement("ROLE_NM_JA_JP")]
        public string RoleNMJAJP { get; set; }
        [BsonElement("ROLE_NM_KO_KR")]
        public string RoleNMKOKR { get; set; }
        [BsonElement("IS_MASTER")]
        public string IsMaster { get; set; }
        public string UpdUserID { get; set; }
    }
}
