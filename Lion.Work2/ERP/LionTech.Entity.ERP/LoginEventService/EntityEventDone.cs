// 新增日期：2017-02-17
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.LoginEventService
{
    public class EntityEventDone : EntityLoginEventService
    {
        public EntityEventDone(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 編輯使用者登入事件 -
        public class EventDonePara
        {
            public enum ParaField
            {
                USER_ID,
                SYS_ID,
                LOGIN_EVENT_ID,
                DONE_DT,
                UPD_USER_ID
            }

            public DBVarChar UserID;
            public DBVarChar SysID;
            public DBVarChar LoginEventID;
            public DBDateTime DoneDT;
            public DBVarChar UpdUserID;
        }

        public enum EnumEditEventDoneResult
        {
            Success,
            Failure
        }

        public EnumEditEventDoneResult EditEventDone(EventDonePara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                " DECLARE @RESULT CHAR(1) = 'N';",
                " DECLARE @ERROR_LINE INT;",
                " DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                " DECLARE @USER_ID VARCHAR(20) = {USER_ID};",
                "     BEGIN TRANSACTION",
                "         BEGIN TRY",
                "             DELETE FROM SYS_USER_LOGIN_EVENT ",
                "              WHERE USER_ID = {USER_ID} ",
                "                AND SYS_ID = {SYS_ID} ",
                "                AND LOGIN_EVENT_ID = {DONE_DT} ",
                "                AND SYS_ID = {DONE_DT};",

                "             INSERT INTO SYS_USER_LOGIN_EVENT",
                "                  ( USER_ID",
                "                  , SYS_ID",
                "                  , LOGIN_EVENT_ID",
                "                  , DONE_DT",
                "                  , UPD_USER_ID",
                "                  , UPD_DT",
                "                  )",
                "             VALUES",
                "                  ( {USER_ID}",
                "                  , {SYS_ID}",
                "                  , {LOGIN_EVENT_ID}",
                "                  , {DONE_DT}",
                "                  , {UPD_USER_ID}",
                "                  , GETDATE()",
                "                  );",
                "           SET @RESULT = 'Y';",
                "           COMMIT;",
                "       END TRY",
                "       BEGIN CATCH",
                "           SET @RESULT = 'N';",
                "           SET @ERROR_LINE = ERROR_LINE();",
                "           SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                "           ROLLBACK TRANSACTION;",
                "       END CATCH;",
                " SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
            }));

            dbParameters.Add(new DBParameter {Name = EventDonePara.ParaField.USER_ID,Value = para.UserID});
            dbParameters.Add(new DBParameter { Name = EventDonePara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = EventDonePara.ParaField.LOGIN_EVENT_ID, Value = para.LoginEventID });
            dbParameters.Add(new DBParameter { Name = EventDonePara.ParaField.DONE_DT, Value = para.DoneDT });
            dbParameters.Add(new DBParameter { Name = EventDonePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditEventDoneResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion
    }
}