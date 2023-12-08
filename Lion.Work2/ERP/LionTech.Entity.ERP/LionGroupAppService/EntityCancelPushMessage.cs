// 新增日期：2016-12-28
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace LionTech.Entity.ERP.LionGroupAppService
{
    public class EntityCancelPushMessage : EntityLionGroupAppService
    {
        public EntityCancelPushMessage(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }
    }

    public class MongoCancelPushMessage : Mongo_BaseAP
    {
        public MongoCancelPushMessage(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 取得取消推播排程清單 -
        public class AppUserPushPara
        {
            public enum ParaField
            {
                MESSAGE_ID,
                USER_ID
            }

            public DBVarChar MessageID;
            public List<DBVarChar> UserIDList;
        }

        public class AppUserPush : MongoDocument
        {
            public enum DataField
            {
                MESSAGE_ID,
                APP_ID,
                USER_ID,
                APP_UUID,
                APP_FUN_ID,
                APP_ROLE_ID,
                TITLE,
                BODY,
                PUSH_DT,
                DATA
            }

            public DBVarChar MessageID;
            public DBVarChar AppID;
            public DBVarChar AppUUID;
            public DBVarChar UserID;
            public DBVarChar AppFunID;
            public DBVarChar AppRoleID;
            public DBNVarChar Title;
            public DBNVarChar Body;
            public DBDateTime PushDT;
            public PushMsgData Data;
        }
        
        public List<AppUserPush> SelectAppUserPushList(AppUserPushPara para)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.APP_USER_PUSH.ToString());

            command.AddFields(EnumSpecifiedFieldType.Select, AppUserPush.DataField.MESSAGE_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, AppUserPush.DataField.USER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, AppUserPush.DataField.APP_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, AppUserPush.DataField.APP_UUID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, AppUserPush.DataField.APP_FUN_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, AppUserPush.DataField.APP_ROLE_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, AppUserPush.DataField.TITLE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, AppUserPush.DataField.BODY.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, AppUserPush.DataField.PUSH_DT.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, AppUserPush.DataField.DATA.ToString());

            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, AppUserPushPara.ParaField.MESSAGE_ID.ToString(), para.MessageID);

            if (para.UserIDList != null &&
                para.UserIDList.Any())
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, AppUserPushPara.ParaField.USER_ID.ToString(), Utility.GetUserIDList(para.UserIDList));
            }
            
            return Select<AppUserPush>(command);
        }
        #endregion

        #region - 刪除推播排程 -
        public void DeleteAppUserPushList(IEnumerable<MongoDocument> dataList)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.APP_USER_PUSH.ToString());

            foreach (var row in dataList)
            {
                Delete(command, row);
            }
        }
        #endregion
    }
}