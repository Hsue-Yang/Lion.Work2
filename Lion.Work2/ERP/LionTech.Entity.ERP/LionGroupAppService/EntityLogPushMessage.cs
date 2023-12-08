// 新增日期：2017-02-13
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using LionTech.Utility;

namespace LionTech.Entity.ERP.LionGroupAppService
{
    public class MongoLogPushMessage : Mongo_BaseAP
    {
        public MongoLogPushMessage(string connectionString, string providerName)
            : base(connectionString, providerName)
        {

        }

        #region - 取得推播歷史紀錄 -
        public class LogPushMessagePara
        {
            public enum ParaField
            {
                USER_ID,
                PUSH_STS,
                APP_UUID,
                UPD_DT,
                [Description("DATA.SourceType")]
                SourceType
            }

            public DBVarChar UserID;
            public DBVarChar SourceType;
            public DBVarChar UUID;
            public DBDateTime StartDateTime;
            public DBDateTime EndDateTime;
        }

        public class LogPushMessage : MongoDocument
        {
            public enum DataField
            {
                MESSAGE_ID,
                TITLE,
                BODY,
                DATA,
                UPD_DT
            }

            public DBVarChar MessageID;
            public DBNVarChar Title;
            public DBNVarChar Body;
            public PushMsgData Data;
            public DBDateTime UpdDT;
        }

        public class PushMsgData : MongoDocument
        {
            public DBVarChar SourceType;
            public DBNVarChar SourceID;
        }

        public List<LogPushMessage> SelectLogPushMessageList(LogPushMessagePara para)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.LOG_APP_USER_PUSH.ToString());

            command.AddFields(EnumSpecifiedFieldType.Select, LogPushMessage.DataField.MESSAGE_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogPushMessage.DataField.TITLE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogPushMessage.DataField.BODY.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogPushMessage.DataField.DATA.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogPushMessage.DataField.UPD_DT.ToString());

            command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, LogPushMessagePara.ParaField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogPushMessagePara.ParaField.APP_UUID.ToString(), para.UUID);
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, Common.GetEnumDesc(LogPushMessagePara.ParaField.SourceType), para.SourceType);
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogPushMessagePara.ParaField.PUSH_STS.ToString(), new DBChar(EnumYN.Y.ToString()));

            if (para.EndDateTime.IsNull())
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.GreaterThan, LogPushMessagePara.ParaField.UPD_DT.ToString(), para.StartDateTime);
            }
            else
            {
                command.AddQueryBetween(EnumConditionType.AND, LogPushMessagePara.ParaField.UPD_DT.ToString(), para.StartDateTime, para.EndDateTime);
            }

            command.AddSortBy(EnumSortType.DESC, LogPushMessagePara.ParaField.UPD_DT.ToString());

            return Select<LogPushMessage>(command);
        }
        #endregion
    }
}