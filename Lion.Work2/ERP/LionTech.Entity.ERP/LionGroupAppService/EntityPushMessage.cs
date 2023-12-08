// 新增日期：2016-12-23
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace LionTech.Entity.ERP.LionGroupAppService
{
    public class EntityPushMessage : EntityLionGroupAppService
    {
        public EntityPushMessage(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class AppUserDeviceTokenPara
        {
            public enum ParaField
            {
                UPD_USER_ID,
                DEVICE_TOKEN_ID
            }

            public DBVarChar UpdUserID;
            public List<DBVarChar> DeviceTokenList;
        }

        public void UpdateAppUserDeviceTokenList(AppUserDeviceTokenPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "UPDATE APP_USER_DEVICE_TOKEN SET IS_MASTER = 'N', UPD_USER_ID = {UPD_USER_ID}, UPD_DT = GETDATE() WHERE DEVICE_TOKEN_ID IN ({DEVICE_TOKEN_ID})"
            }));
            dbParameters.Add(new DBParameter { Name = AppUserDeviceTokenPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = AppUserDeviceTokenPara.ParaField.DEVICE_TOKEN_ID, Value = para.DeviceTokenList });
            ExecuteNonQuery(commandText.ToString(), dbParameters);
        }
    }

    public class MongoPushMessage: Mongo_BaseAP
    {
        public MongoPushMessage(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 推播排程 -
        public class AppUserPushPara : MongoDocument
        {
            [DBTypeProperty("MESSAGE_ID")]
            public DBVarChar MessageID;

            [DBTypeProperty("APP_ID")]
            public DBVarChar AppID;

            [DBTypeProperty("USER_ID")]
            public DBVarChar UserID;

            [DBTypeProperty("APP_UUID")]
            public DBVarChar AppUUID;

            [DBTypeProperty("APP_FUN_ID")]
            public DBVarChar AppFunID;

            [DBTypeProperty("APP_ROLE_ID")]
            public DBVarChar AppRoleID;

            [DBTypeProperty("TITLE")]
            public DBNVarChar Title;

            [DBTypeProperty("BODY")]
            public DBNVarChar Body;

            [DBTypeProperty("PUSH_DT")]
            public DBDateTime PushDT;

            [DBTypeProperty("DATA")]
            public PushMsgData Data;

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