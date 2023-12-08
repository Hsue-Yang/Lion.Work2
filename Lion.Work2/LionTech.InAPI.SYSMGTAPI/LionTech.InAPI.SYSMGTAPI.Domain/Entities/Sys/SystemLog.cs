using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemLog
    {
        public EnumMongoDocName MongoDocName { get; set; }
        public List<BsonDocument> Data { get; set; }
        public BsonDocument Condition { get; set; }
        public EnumSystemLogModify Modify { get; set; }
        public string UpdUserID { get; set; }
        public string UpdUserNM { get; set; }
        public DateTime UpdDT { get; set; }
        public string ExecSysID { get; set; }
        public string ExecSysNM { get; set; }
        public string ExecIPAddress { get; set; }
    }
}
