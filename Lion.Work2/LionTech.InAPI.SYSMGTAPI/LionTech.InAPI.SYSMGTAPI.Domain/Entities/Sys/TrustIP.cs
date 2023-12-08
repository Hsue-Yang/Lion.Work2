using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class TrustIPPara
    {
        public string IPBegin { get; set; }
        public string IPEnd { get; set; }
        public string ComID { get; set; }
        public string TrustStatus { get; set; }
        public string TrustType { get; set; }
        public string SourceType { get; set; }
        public string Remark { get; set; }
        public string CultureID { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class TrustIP
    {
        public string IPBeginOriginal { get; set; }
        public string IPEndOriginal { get; set; }
        public string IPBegin { get; set; }
        public string IPEnd { get; set; }
        public string ComID { get; set; }
        public string ComNM { get; set; }
        public string TrustStatus { get; set; }
        public string TrustType { get; set; }
        public string TrustTypeNM { get; set; }
        public string SourceType { get; set; }
        public string SourceTypeNM { get; set; }
        public string Remark { get; set; }
        public string SortOrder { get; set; }
        public string UpdUserNM { get; set; }
        public string UpdUserID { get; set; }
        public DateTime UpdDT { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class RecordSysTrustIP
    {
        [BsonElement("_id")]
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("LOG_NO")]
        public string LogNo { get; set; }
        [BsonElement("MODIFY_TYPE")]
        public string ModifyType { get; set; }
        [BsonElement("MODIFY_TYPE_NM")]
        public string ModifyTypeNM { get; set; }
        [BsonElement("IP_BEGIN")]
        public string IPBegin { get; set; }
        [BsonElement("IP_END")]
        public string IPEnd { get; set; }
        [BsonElement("COM_ID")]
        public string ComID { get; set; }
        [BsonElement("COM_NM")]
        public string ComNM { get; set; }
        [BsonElement("TRUST_STATUS")]
        public string TrustStatus { get; set; }
        [BsonElement("TRUST_TYPE")]
        public string TrustType { get; set; }
        [BsonElement("TRUST_TYPE_NM")]
        public string TrustTypeNM { get; set; }
        [BsonElement("SOURCE_TYPE")]
        public string SourceType { get; set; }
        [BsonElement("SOURCE_TYPE_NM")]
        public string SourceTypeNM { get; set; }
        [BsonElement("REMARK")]
        public string Remark { get; set; }
        [BsonElement("SORT_ORDER")]
        public string SortOrder { get; set; }
        [BsonElement("API_NO")]
        public string APINo { get; set; }
        [BsonElement("UPD_USER_ID")]
        public string UpdUserID { get; set; }
        [BsonElement("UPD_USER_NM")]
        public string UpdUserNM { get; set; }
        [BsonElement("UPD_DT")]
        public DateTime UpdDT { get; set; }
        [BsonElement("EXEC_SYS_ID")]
        public string ExecSysID { get; set; }
        [BsonElement("EXEC_SYS_NM")]
        public string ExecSysNM { get; set; }
        [BsonElement("EXEC_IP_ADDRESS")]
        public string ExecIPAddress { get; set; }
    }
}