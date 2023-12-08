using MongoDB.Bson.Serialization.Attributes;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemFunGroupMain
    {
        [BsonElement("SYS_ID")]
        public string SysID { get; set; }
        [BsonElement("SYS_NM")]
        public string SysNM { get; set; }
        [BsonElement("FUN_CONTROLLER_ID")]
        public string FunControllerID { get; set; }

        [BsonElement("FUN_GROUP_ZH_TW")]
        public string FunGroupZHTW { get; set; }
        [BsonElement("FUN_GROUP_ZH_CN")]
        public string FunGroupZHCN { get; set; }
        [BsonElement("FUN_GROUP_EN_US")]
        public string FunGroupENUS { get; set; }
        [BsonElement("FUN_GROUP_TH_TH")]
        public string FunGroupTHTH { get; set; }
        [BsonElement("FUN_GROUP_JA_JP")]
        public string FunGroupJAJP { get; set; }
        [BsonElement("FUN_GROUP_KO_KR")]
        public string FunGroupKOKR { get; set; }

        [BsonElement("SORT_ORDER")]
        public string SortOrder { get; set; }
        public string UpdUserID { get; set; }

        public string ExecSysID { get; set; }
        public string ExecIpAddress { get; set; }
    }
}
