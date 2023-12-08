using MongoDB.Bson.Serialization.Attributes;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemFunMain
    {
        [BsonElement("SYS_ID")]
        public string SysID { get; set; }
        [BsonElement("SYS_NM")]
        public string SysNM { get; set; }
        [BsonElement("SUB_SYS_ID")]
        public string SubSysID { get; set; }
        [BsonElement("SUB_SYS_NM")]
        public string SubSysNM { get; set; }
        [BsonElement("PURVIEW_ID")]
        public string PurviewID { get; set; }
        [BsonElement("PURVIEW_NM")]
        public string PurviewNM { get; set; }
        [BsonElement("FUN_CONTROLLER_ID")]
        public string FunControllerID { get; set; }
        [BsonElement("FUN_GROUP_NM")]
        public string FunGroupNM { get; set; }
        [BsonElement("FUN_ACTION_NAME")]
        public string FunActionName { get; set; }
        [BsonElement("FUN_NM_ZH_TW")]
        public string FunNMZHTW { get; set; }
        [BsonElement("FUN_NM_ZH_CN")]
        public string FunNMZHCN { get; set; }
        [BsonElement("FUN_NM_EN_US")]
        public string FunNMENUS { get; set; }
        [BsonElement("FUN_NM_TH_TH")]
        public string FunNMTHTH { get; set; }
        [BsonElement("FUN_NM_JA_JP")]
        public string FunNMJAJP { get; set; }
        [BsonElement("FUN_NM_KO_KR")]
        public string FunNMKOKR { get; set; }
        [BsonElement("FUN_TYPE")]
        public string FunType { get; set; }
        [BsonElement("FUN_TYPE_NM")]
        public string FunTypeNM { get; set; }
        [BsonElement("IS_DISABLE")]
        public string IsDisable { get; set; }
        [BsonElement("IS_OUTSIDE")]
        public string IsOutside { get; set; }
        [BsonElement("SORT_ORDER")]
        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }
    }
}
