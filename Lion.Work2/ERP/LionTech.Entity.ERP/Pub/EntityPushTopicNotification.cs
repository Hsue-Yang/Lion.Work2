using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionTech.Entity.ERP.Pub
{
    public class EntityPushTopicNotification : EntityPub
    {
        public EntityPushTopicNotification(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        
    }

    public class MongoPushTopicNotification : Mongo_BaseAP
    {
        public MongoPushTopicNotification(string connectionString, string providerName)
        : base(connectionString, providerName)
        {
        }

        public class PushTopicNotificationPara
        {
            public enum ParaField
            {
                USER_ID,
                TITLE,
                BODY,
                PUSH_STS,
                UPD_DT
            }

            public DBVarChar UserID;
            public DBVarChar AppFunID;
            public DBNVarChar Title;
            public DBNVarChar Body;
            public DBChar PushSts;
            public DBDateTime StartPushDateTime;
            public DBDateTime EndPushDateTime;
        }

        public class PushTopicNotification : MongoDocument
        {
            public enum DataField
            {
                MESSAGE_ID,
                PUSH_STS,
                PUSH_DT,
                TITLE,
                BODY,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                UPD_DT
            }

            public DBVarChar MessageID;
            public DBNVarChar UserIDNM;
            public DBChar PushSts;
            public DBDateTime PushDT;
            public DBNVarChar Title;
            public DBNVarChar Body;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBDateTime UpdDT;
        }

        /// <summary>
        /// 取得主題推播紀錄
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<PushTopicNotification> SelectPushNotificationList(PushTopicNotificationPara para)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.LOG_APP_TOPIC_PUSH.ToString());

            command.AddFields(EnumSpecifiedFieldType.Select, PushTopicNotification.DataField.MESSAGE_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushTopicNotification.DataField.PUSH_STS.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushTopicNotification.DataField.PUSH_DT.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushTopicNotification.DataField.TITLE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushTopicNotification.DataField.BODY.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushTopicNotification.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushTopicNotification.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushTopicNotification.DataField.UPD_DT.ToString());

            if (para.Title.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Like, PushTopicNotificationPara.ParaField.TITLE.ToString(), para.Title);
            }

            if (para.Body.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Like, PushTopicNotificationPara.ParaField.BODY.ToString(), para.Body);
            }

            if (para.PushSts.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, PushTopicNotificationPara.ParaField.PUSH_STS.ToString(), para.PushSts);
            }

            if (para.StartPushDateTime.IsNull() == false &&
                para.EndPushDateTime.IsNull() == false)
            {
                command.AddQueryBetween(EnumConditionType.AND, PushTopicNotificationPara.ParaField.UPD_DT.ToString(), para.StartPushDateTime, para.EndPushDateTime);
            }
            else
            {
                if (para.StartPushDateTime.IsNull() == false)
                {
                    command.AddQuery(EnumConditionType.AND, EnumOperatorType.GreaterThan, PushTopicNotificationPara.ParaField.UPD_DT.ToString(), para.StartPushDateTime);
                }

                if (para.EndPushDateTime.IsNull() == false)
                {
                    command.AddQuery(EnumConditionType.AND, EnumOperatorType.LessThan, PushTopicNotificationPara.ParaField.UPD_DT.ToString(), para.EndPushDateTime);
                }
            }

            command.AddSortBy(EnumSortType.DESC, PushTopicNotification.DataField.UPD_DT.ToString());
            return Select<PushTopicNotification>(command);
        }
    }
}
