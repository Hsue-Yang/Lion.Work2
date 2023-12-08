// 新增日期：2017-01-04
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.LionGroupAppService
{
    public class EntityOpenPush : EntityLionGroupAppService
    {
        public EntityOpenPush(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 編輯是否開啟推播 -
        public class OpenPushPara
        {
            public enum ParaField
            {
                USER_ID,
                APP_UUID,
                APP_FUN_ID,
                REMIND_MINUTE,
                IS_OPEN_PUSH,
                UPD_USER_ID
            }

            public DBVarChar UserID;
            public DBVarChar AppUUID;
            public DBVarChar AppFunID;
            public DBInt RemindMinute;
            public DBChar IsOpenPush;
            public DBVarChar UpdUserID;
        }

        public enum EnumEditOpenPushResult
        {
            Success,
            Failure
        }

        public enum EnumAppPushRemindMinute
        {
            [Description("5")]
            Five
        }

        public EnumEditOpenPushResult EditOpenPush(OpenPushPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "DECLARE @ERROR_LINE INT;",
                    "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                    "DECLARE @USER_ID VARCHAR(20) = {USER_ID}",
                    "DECLARE @APP_UUID UNIQUEIDENTIFIER  = CONVERT(UNIQUEIDENTIFIER, {APP_UUID})",
                    "DECLARE @APP_FUN_ID VARCHAR(20) = {APP_FUN_ID}",
                    "BEGIN TRANSACTION",
                    "    BEGIN TRY",
                    "        IF NOT EXISTS(SELECT *",
                    "                        FROM APP_USER_FUN",
                    "                       WHERE USER_ID = @USER_ID",
                    "                         AND APP_UUID = @APP_UUID",
                    "                         AND APP_FUN_ID = @APP_FUN_ID",
                    "                     )",
                    "        BEGIN",
                    "            INSERT INTO APP_USER_FUN",
                    "                 ( USER_ID",
                    "                 , APP_UUID",
                    "                 , APP_FUN_ID",
                    "                 , REMIND_MINUTE",
                    "                 , IS_OPEN_PUSH",
                    "                 , UPD_USER_ID",
                    "                 , UPD_DT",
                    "                 )",
                    "            VALUES",
                    "                 ( @USER_ID",
                    "                 , @APP_UUID",
                    "                 , @APP_FUN_ID",
                    "                 , {REMIND_MINUTE}",
                    "                 , {IS_OPEN_PUSH}",
                    "                 , {UPD_USER_ID}",
                    "                 , GETDATE()",
                    "                 );",
                    "        END",
                    "        ELSE",
                    "        BEGIN",
                    "            UPDATE APP_USER_FUN",
                    "               SET IS_OPEN_PUSH = {IS_OPEN_PUSH}",
                    "                 , UPD_USER_ID = {UPD_USER_ID}",
                    "                 , UPD_DT = GETDATE()",
                    "             WHERE USER_ID = @USER_ID",
                    "               AND APP_UUID = @APP_UUID",
                    "               AND APP_FUN_ID = @APP_FUN_ID;",
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

            dbParameters.Add(new DBParameter { Name = OpenPushPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = OpenPushPara.ParaField.APP_UUID, Value = para.AppUUID });
            dbParameters.Add(new DBParameter { Name = OpenPushPara.ParaField.APP_FUN_ID, Value = para.AppFunID });
            dbParameters.Add(new DBParameter { Name = OpenPushPara.ParaField.IS_OPEN_PUSH, Value = para.IsOpenPush });
            dbParameters.Add(new DBParameter { Name = OpenPushPara.ParaField.REMIND_MINUTE, Value = para.RemindMinute });
            dbParameters.Add(new DBParameter { Name = OpenPushPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditOpenPushResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion
    }
}