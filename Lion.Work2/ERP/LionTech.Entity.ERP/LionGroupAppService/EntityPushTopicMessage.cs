namespace LionTech.Entity.ERP.LionGroupAppService
{
    public class EntityPushTopicMessage : EntityLionGroupAppService
    {
        public EntityPushTopicMessage(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

    }

    public class MongoTopicPush : Mongo_BaseAP
    {
        public MongoTopicPush(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 推播排程 -
        public class AppTopicPushPara : MongoDocument
        {
            [DBTypeProperty("MESSAGE_ID")]
            public DBVarChar MessageID;

            [DBTypeProperty("Topic_ID")]
            public DBVarChar TopicID;

            [DBTypeProperty("APP_ID")]
            public DBVarChar AppID;

            [DBTypeProperty("DEVICE_TOKEN_TYPE")]
            public DBVarChar DeviceTokenType;

            [DBTypeProperty("PUSH_DT")]
            public DBDateTime PushDT;

            [DBTypeProperty("TITLE")]
            public DBNVarChar Title;

            [DBTypeProperty("BODY")]
            public DBNVarChar Body;

            [DBTypeProperty("API_NO")]
            public DBChar APINo;

            [DBTypeProperty("UPD_USER_ID")]
            public DBVarChar UpdUserID;

            [DBTypeProperty("UPD_USER_NM")]
            public DBNVarChar UpdUserNM;

            [DBTypeProperty("UPD_DT")]
            public DBDateTime UpdDT;

            [DBTypeProperty("EXEC_SYS_ID")]
            public DBVarChar ExecSysID;

            [DBTypeProperty("EXEC_SYS_NM")]
            public DBNVarChar ExecSysNM;

            [DBTypeProperty("EXEC_IP_ADDRESS")]
            public DBVarChar ExecIPAddress;
        }
        #endregion
    }
}