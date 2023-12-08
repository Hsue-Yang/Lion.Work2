// 新增日期：2017-03-17
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Pub
{
    public class EntityPushNotification : EntityPub
    {
        public EntityPushNotification(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 取得裝置資訊清單 -
        public class MobileInfoPara : DBCulture
        {
            public MobileInfoPara(string cultureID) : base(cultureID)
            {
                
            }

            public enum ParaField
            {
                CODE_NM,
                APP_UUID
            }

            public List<DBVarChar> AppUUIDList;
        }

        public class MobileInfo : DBTableRow
        {
            public DBNVarChar CodeNM;
            public DBVarChar AppUUID;
            public DBVarChar MobileType;
        }

        public List<MobileInfo> SelectMobileInfoList(MobileInfoPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT C.{CODE_NM} AS CodeNM",
                "     , M.APP_UUID AS AppUUID",
                "     , M.MOBILE_TYPE AS MobileType",
                "  FROM APP_USER_MOBILE M",
                "  LEFT JOIN CM_CODE C",
                "    ON M.MOBILE_TYPE = C.CODE_ID",
                " WHERE M.APP_UUID IN ({APP_UUID})"
            }));

            dbParameters.Add(new DBParameter { Name = MobileInfoPara.ParaField.APP_UUID, Value = para.AppUUIDList });
            dbParameters.Add(new DBParameter { Name = MobileInfoPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(MobileInfoPara.ParaField.CODE_NM.ToString())) });

            return GetEntityList<MobileInfo>(commandText.ToString(), dbParameters);
        }
        #endregion
    }

    public class MongoPushNotification : Mongo_BaseAP
    {
        public MongoPushNotification(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 取得推播紀錄清單 -
        public class PushNotificationPara
        {
            public enum ParaField
            {
                USER_ID,
                APP_FUN_ID,
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

        public class PushNotification : MongoDocument
        {
            public enum DataField
            {
                MESSAGE_ID,
                USER_ID,
                APP_UUID,
                APP_FUN_ID,
                PUSH_STS,
                PUSH_DT,
                TITLE,
                BODY,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                IS_OPEN_PUSH,
                UPD_DT
            }

            public DBVarChar MessageID;
            public DBVarChar UserID;
            public DBNVarChar UserIDNM;
            public DBVarChar MobileType;
            public DBVarChar AppUUID;
            public DBNVarChar AppFunID;
            public DBChar PushSts;
            public DBDateTime PushDT;
            public DBNVarChar Title;
            public DBNVarChar Body;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBChar IsOpenPush;
            public DBDateTime UpdDT;
        }

        public List<PushNotification> SelectPushNotificationList(PushNotificationPara para)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.LOG_APP_USER_PUSH.ToString());

            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.MESSAGE_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.USER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.APP_UUID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.APP_FUN_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.PUSH_STS.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.PUSH_DT.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.TITLE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.BODY.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.IS_OPEN_PUSH.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.UPD_DT.ToString());

            if (para.UserID.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, PushNotificationPara.ParaField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            }

            if (para.AppFunID.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, PushNotificationPara.ParaField.APP_FUN_ID.ToString(), para.AppFunID);
            }

            if (para.Title.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND,EnumOperatorType.Like, PushNotificationPara.ParaField.TITLE.ToString(), para.Title);
            }

            if (para.Body.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Like, PushNotificationPara.ParaField.BODY.ToString(), para.Body);
            }

            if (para.PushSts.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, PushNotificationPara.ParaField.PUSH_STS.ToString(), para.PushSts);
            }

            if (para.StartPushDateTime.IsNull() == false &&
                para.EndPushDateTime.IsNull() == false)
            {
                command.AddQueryBetween(EnumConditionType.AND, PushNotificationPara.ParaField.UPD_DT.ToString(), para.StartPushDateTime, para.EndPushDateTime);
            }
            else
            {
                if (para.StartPushDateTime.IsNull() == false)
                {
                    command.AddQuery(EnumConditionType.AND, EnumOperatorType.GreaterThan, PushNotificationPara.ParaField.UPD_DT.ToString(), para.StartPushDateTime);
                }

                if (para.EndPushDateTime.IsNull() == false)
                {
                    command.AddQuery(EnumConditionType.AND, EnumOperatorType.LessThan, PushNotificationPara.ParaField.UPD_DT.ToString(), para.EndPushDateTime);
                }
            }

            command.AddSortBy(EnumSortType.DESC, PushNotification.DataField.UPD_DT.ToString());

            return Select<PushNotification>(command);
        }
        #endregion

        #region - 取得排程推播清單 -
        public List<PushNotification> SelectAppUserPushList(PushNotificationPara para)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.APP_USER_PUSH.ToString());

            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.MESSAGE_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.USER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.APP_FUN_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.PUSH_DT.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.TITLE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.BODY.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, PushNotification.DataField.UPD_DT.ToString());

            if (para.UserID.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, PushNotificationPara.ParaField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            }

            if (para.AppFunID.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, PushNotificationPara.ParaField.APP_FUN_ID.ToString(), para.AppFunID);
            }

            if (para.Title.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Like, PushNotificationPara.ParaField.TITLE.ToString(), para.Title);
            }

            if (para.Body.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Like, PushNotificationPara.ParaField.BODY.ToString(), para.Body);
            }

            if (para.StartPushDateTime.IsNull() == false &&
                para.EndPushDateTime.IsNull() == false)
            {
                command.AddQueryBetween(EnumConditionType.AND, PushNotificationPara.ParaField.UPD_DT.ToString(), para.StartPushDateTime, para.EndPushDateTime);
            }
            else
            {
                if (para.StartPushDateTime.IsNull() == false)
                {
                    command.AddQuery(EnumConditionType.AND, EnumOperatorType.GreaterThan, PushNotificationPara.ParaField.UPD_DT.ToString(), para.StartPushDateTime);
                }

                if (para.EndPushDateTime.IsNull() == false)
                {
                    command.AddQuery(EnumConditionType.AND, EnumOperatorType.LessThan, PushNotificationPara.ParaField.UPD_DT.ToString(), para.EndPushDateTime);
                }
            }

            command.AddSortBy(EnumSortType.DESC, PushNotification.DataField.UPD_DT.ToString());

            return Select<PushNotification>(command);
        }
        #endregion

        #region - 取得推播錯誤訊息 -
        public class PushNotificationErrorMsgPara
        {
            public enum ParaField
            {
                MESSAGE_ID,
                USER_ID,
                APP_UUID
            }

            public DBVarChar MessageID;
            public DBVarChar UserID;
            public DBVarChar AppUUID;
        }

        public class PushNotificationErrorMsg : MongoDocument
        {
            public enum DataField
            {
                ERROR_MESSAGE
            }

            public DBNVarChar ErrorMessage;
        }

        public PushNotificationErrorMsg SelectPushNotificationErrorMsg(PushNotificationErrorMsgPara para)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.LOG_APP_USER_PUSH.ToString());

            command.AddFields(EnumSpecifiedFieldType.Select, PushNotificationErrorMsg.DataField.ERROR_MESSAGE.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, PushNotificationErrorMsgPara.ParaField.MESSAGE_ID.ToString(), para.MessageID);
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, PushNotificationErrorMsgPara.ParaField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, PushNotificationErrorMsgPara.ParaField.APP_UUID.ToString(), para.AppUUID);

            return Select<PushNotificationErrorMsg>(command).SingleOrDefault();
        }
        #endregion
    }
}