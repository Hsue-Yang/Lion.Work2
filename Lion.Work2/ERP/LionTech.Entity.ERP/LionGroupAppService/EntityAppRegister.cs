// 新增日期：2017-01-03
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.LionGroupAppService
{
    public class EntityAppRegister : EntityLionGroupAppService
    {
        public EntityAppRegister(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 編輯使用者註冊資料 -
        public class AppRegisterUserInfoPara
        {
            public enum ParaField
            {
                USER_ID,
                APP_UUID,
                APP_ID,
                MOBILE_OS,
                MOBILE_TYPE,
                UPD_USER_ID,

                DEVICE_TOKEN_TYPE,
                DEVICE_TOKEN_ID
            }

            public DBVarChar UserID;
            public DBVarChar AppUUID;
            public DBVarChar AppID;
            public DBVarChar MobileOS;
            public DBVarChar MobileType;
            public DBVarChar UpdUserID;

            public DBVarChar DeviceTokenType;
            public DBVarChar DeviceTokenID;
        }

        public enum EnumEditAppRegisterResult
        {
            Success,
            Failure
        }

        public EnumEditAppRegisterResult EditAppRegister(AppRegisterUserInfoPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "DECLARE @ERROR_LINE INT;",
                    "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                    "DECLARE @USER_ID VARCHAR(20) = {USER_ID};",
                    "DECLARE @APP_UUID UNIQUEIDENTIFIER  = CONVERT(UNIQUEIDENTIFIER, {APP_UUID});",
                    "DECLARE @DEVICE_TOKEN_TYPE VARCHAR(20) = {DEVICE_TOKEN_TYPE};",
                    "DECLARE @DEVICE_TOKEN_ID VARCHAR(300) = {DEVICE_TOKEN_ID};",
                    "DECLARE @UPD_USER_ID VARCHAR(50) = {UPD_USER_ID};",

                    "BEGIN TRANSACTION",
                    "    BEGIN TRY",
                    "        UPDATE APP_USER_DEVICE_TOKEN",
                    "           SET IS_MASTER = '" + EnumYN.N + "'",
                    "             , UPD_USER_ID = @UPD_USER_ID",
                    "             , UPD_DT = GETDATE()",
                    "         WHERE DEVICE_TOKEN_ID = @DEVICE_TOKEN_ID",
                    "           AND DEVICE_TOKEN_TYPE = @DEVICE_TOKEN_TYPE;",

                    "        IF NOT EXISTS(SELECT *",
                    "                        FROM APP_USER_DEVICE_TOKEN T",
                    "                       WHERE T.DEVICE_TOKEN_ID = @DEVICE_TOKEN_ID",
                    "                         AND T.USER_ID = @USER_ID",
                    "                         AND T.APP_UUID = @APP_UUID",
                    "                     )",
                    "        BEGIN",
                    "            INSERT INTO APP_USER_DEVICE_TOKEN",
                    "                 ( USER_ID",
                    "                 , APP_UUID",
                    "                 , DEVICE_TOKEN_TYPE",
                    "                 , DEVICE_TOKEN_ID",
                    "                 , IS_MASTER",
                    "                 , UPD_USER_ID",
                    "                 , UPD_DT",
                    "                 )",
                    "            VALUES",
                    "                 ( @USER_ID",
                    "                 , @APP_UUID",
                    "                 , @DEVICE_TOKEN_TYPE",
                    "                 , @DEVICE_TOKEN_ID",
                    "                 , '" + EnumYN.Y + "'",
                    "                 , @UPD_USER_ID",
                    "                 , GETDATE()",
                    "                 );",
                    "        END",
                    "        ELSE",
                    "        BEGIN",
                    "            UPDATE APP_USER_DEVICE_TOKEN",
                    "               SET IS_MASTER = '" + EnumYN.Y + "'",
                    "                 , UPD_USER_ID = @UPD_USER_ID",
                    "                 , UPD_DT = GETDATE()",
                    "             WHERE DEVICE_TOKEN_ID = @DEVICE_TOKEN_ID",
                    "               AND USER_ID = @USER_ID",
                    "               AND APP_UUID = @APP_UUID",
                    "        END",

                    "        UPDATE APP_USER_MOBILE",
                    "           SET IS_DISABLE = '" + EnumYN.Y + "'",
                    "             , UPD_USER_ID = @UPD_USER_ID",
                    "             , UPD_DT = GETDATE()",
                    "         WHERE APP_UUID = @APP_UUID;",

                    "        IF NOT EXISTS(SELECT *",
                    "                        FROM APP_USER_MOBILE M",
                    "                       WHERE M.USER_ID = @USER_ID",
                    "                         AND M.APP_UUID = @APP_UUID",
                    "                     )",
                    "        BEGIN",
                    "            INSERT INTO APP_USER_MOBILE",
                    "                 ( USER_ID",
                    "                 , APP_UUID",
                    "                 , APP_ID",
                    "                 , MOBILE_OS",
                    "                 , MOBILE_TYPE",
                    "                 , IS_DISABLE",
                    "                 , UPD_USER_ID",
                    "                 , UPD_DT",
                    "                 )",
                    "            VALUES",
                    "                 ( @USER_ID",
                    "                 , @APP_UUID",
                    "                 , {APP_ID}",
                    "                 , {MOBILE_OS}",
                    "                 , {MOBILE_TYPE}",
                    "                 , '" + EnumYN.N + "'",
                    "                 , @UPD_USER_ID",
                    "                 , GETDATE()",
                    "                 );",
                    "        END",
                    "        ELSE",
                    "        BEGIN",
                    "            UPDATE APP_USER_MOBILE",
                    "               SET MOBILE_OS = {MOBILE_OS}",
                    "                 , APP_ID = {APP_ID}",
                    "                 , MOBILE_TYPE = {MOBILE_TYPE}",
                    "                 , IS_DISABLE = '" + EnumYN.N + "'",
                    "                 , UPD_USER_ID = @UPD_USER_ID",
                    "                 , UPD_DT = GETDATE()",
                    "             WHERE USER_ID = @USER_ID",
                    "               AND APP_UUID = @APP_UUID;",
                    "        END",

                    "       SET @RESULT = 'Y';",
                    "       COMMIT;",
                    "    END TRY",
                    "    BEGIN CATCH",
                    "       SET @RESULT = 'N';",
                    "       SET @ERROR_LINE = ERROR_LINE();",
                    "       SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                    "       ROLLBACK TRANSACTION;",
                    "    END CATCH;",
                    "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                }));

            dbParameters.Add(new DBParameter { Name = AppRegisterUserInfoPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = AppRegisterUserInfoPara.ParaField.APP_UUID, Value = para.AppUUID });
            dbParameters.Add(new DBParameter { Name = AppRegisterUserInfoPara.ParaField.APP_ID, Value = para.AppID });
            dbParameters.Add(new DBParameter { Name = AppRegisterUserInfoPara.ParaField.MOBILE_OS, Value = para.MobileOS });
            dbParameters.Add(new DBParameter { Name = AppRegisterUserInfoPara.ParaField.MOBILE_TYPE, Value = para.MobileType });
            dbParameters.Add(new DBParameter { Name = AppRegisterUserInfoPara.ParaField.MOBILE_OS, Value = para.MobileOS });
            dbParameters.Add(new DBParameter { Name = AppRegisterUserInfoPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = AppRegisterUserInfoPara.ParaField.DEVICE_TOKEN_TYPE, Value = para.DeviceTokenType });
            dbParameters.Add(new DBParameter { Name = AppRegisterUserInfoPara.ParaField.DEVICE_TOKEN_ID, Value = para.DeviceTokenID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditAppRegisterResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion
    }
}