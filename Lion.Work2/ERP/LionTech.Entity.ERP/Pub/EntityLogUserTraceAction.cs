using System.Collections.Generic;
namespace LionTech.Entity.ERP.Pub
{
    public class EntityLogUserTraceAction : EntityPub
    {
        public EntityLogUserTraceAction(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }
    }
    public class MongoLogUserTraceAction : Mongo_BaseAP
    {
        public MongoLogUserTraceAction(string connectionString, string providerName)
        : base(connectionString, providerName)
        {
        }
        public class LogUserTraceActionPara
        {
            public enum ParaField
            {
                USER_ID,
                SYS_ID,
                CONTROLLER_NAME,
                ACTION_NAME,
                SESSION_ID,
                REQUEST_SESSION_ID,
                UPD_DT
            }
            public DBVarChar UserID;
            public DBVarChar SysID;
            public DBVarChar ControllerName;
            public DBVarChar ActionName;
            public DBChar SessionID;
            public DBChar RequestSessionID;
            public DBDateTime StartTraceDateTime;
            public DBDateTime EndTraceDateTime;
        }
        public class LogUserTraceAction : MongoDocument
        {
            public enum DataField
            {
                USER_ID,
                USER_NM,
                SYS_ID,
                CONTROLLER_NAME,
                ACTION_NAME,
                SESSION_ID,
                REQUEST_URL,
                USER_IP_ADDRESS,
                UPD_DT
            }
            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar SysID;
            public DBVarChar ControllerName;
            public DBVarChar ActionName;
            public DBChar SessionID;
            public DBVarChar RequestURL;
            public DBVarChar UserIPAddress;
            public DBDateTime UPDDT;
        }
        /// <summary>
        /// 取得使用者追蹤紀錄
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<LogUserTraceAction> SelectLogUserTraceList(LogUserTraceActionPara para)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.LOG_USER_TRACE_ACTION.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserTraceAction.DataField.USER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserTraceAction.DataField.USER_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserTraceAction.DataField.SYS_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserTraceAction.DataField.CONTROLLER_NAME.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserTraceAction.DataField.ACTION_NAME.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserTraceAction.DataField.SESSION_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserTraceAction.DataField.REQUEST_URL.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserTraceAction.DataField.USER_IP_ADDRESS.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserTraceAction.DataField.UPD_DT.ToString());
            if (para.UserID.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogUserTraceActionPara.ParaField.USER_ID.ToString(), para.UserID);
            }
            if (para.SysID.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogUserTraceActionPara.ParaField.SYS_ID.ToString(), para.SysID);
            }
            if (para.ControllerName.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogUserTraceActionPara.ParaField.CONTROLLER_NAME.ToString(), para.ControllerName);
            }
            if (para.ActionName.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogUserTraceActionPara.ParaField.ACTION_NAME.ToString(), para.ActionName);
            }
            if (para.SessionID.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogUserTraceActionPara.ParaField.SESSION_ID.ToString(), para.SessionID);
            }
            if (para.SessionID.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogUserTraceActionPara.ParaField.REQUEST_SESSION_ID.ToString(), para.RequestSessionID);
            }

            command.AddQueryBetween(EnumConditionType.AND, LogUserTraceActionPara.ParaField.UPD_DT.ToString(), para.StartTraceDateTime, para.EndTraceDateTime);

            return Select<LogUserTraceAction>(command);
        }
    }
}