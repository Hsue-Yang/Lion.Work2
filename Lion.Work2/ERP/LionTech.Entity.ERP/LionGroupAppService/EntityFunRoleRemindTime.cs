// 新增日期：2017-01-05
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.LionGroupAppService
{
    public class EntityFunRoleRemindTime : EntityLionGroupAppService
    {
        public EntityFunRoleRemindTime(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 查詢App功能角色清單 -
        public class AppFunRolePara
        {
            public enum ParaField
            {
                APP_FUN_ID
            }

            public DBVarChar AppFunID;
        }

        public class AppFunRole : DBTableRow
        {
            public DBVarChar AppRoleID;
        }

        /// <summary>
        /// 查詢App功能角色清單
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<AppFunRole> SelectAppFunRoleList(AppFunRolePara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT APP_ROLE_ID AS AppRoleID",
                    "  FROM APP_FUN_ROLE",
                    " WHERE APP_FUN_ID = {APP_FUN_ID}"
                }));

            dbParameters.Add(new DBParameter { Name = AppFunRolePara.ParaField.APP_FUN_ID, Value = para.AppFunID });

            return GetEntityList<AppFunRole>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 編輯使用者功能 -
        public class FunRoleRemindTimePara
        {
            public enum ParaField
            {
                USER_ID,
                APP_UUID,
                APP_FUN_ID,
                APP_ROLE_ID,
                REMIND_MINUTE,
                UPD_USER_ID
            }

            public DBVarChar UserID;
            public DBVarChar AppUUID;
            public DBVarChar AppFunID;
            public DBInt RemindMinute;
            public DBVarChar UpdUserID;
            public List<DBVarChar> AppRoleIDList;
        }

        public enum EnumEditFunRoleRemindTimeResult
        {
            Success,
            Failure
        }

        public EnumEditFunRoleRemindTimeResult EditFunRoleRemindTime(FunRoleRemindTimePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "DECLARE @ERROR_LINE INT;",
                    "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                    "DECLARE @USER_ID VARCHAR(20) = {USER_ID};",
                    "DECLARE @APP_UUID UNIQUEIDENTIFIER = CONVERT(UNIQUEIDENTIFIER, {APP_UUID});",
                    "DECLARE @APP_FUN_ID VARCHAR(20) = {APP_FUN_ID};",
                    "DECLARE @UPD_USER_ID VARCHAR(50) = {UPD_USER_ID};",
                    "BEGIN TRANSACTION",
                    "    BEGIN TRY",
                    "        IF EXISTS(SELECT *",
                    "                    FROM APP_USER_FUN_ROLE",
                    "                   WHERE USER_ID = @USER_ID",
                    "                     AND APP_UUID = @APP_UUID",
                    "                     AND APP_FUN_ID = @APP_FUN_ID",
                    "                 )",
                    "        BEGIN",
                    "            DELETE APP_USER_FUN_ROLE",
                    "             WHERE USER_ID = @USER_ID",
                    "               AND APP_UUID = @APP_UUID",
                    "               AND APP_FUN_ID = @APP_FUN_ID",
                    "        END"
                }));

            foreach (var role in para.AppRoleIDList)
            {
                commandText.AppendLine(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                    "            INSERT INTO APP_USER_FUN_ROLE",
                    "                 ( USER_ID",
                    "                 , APP_UUID",
                    "                 , APP_FUN_ID",
                    "                 , APP_ROLE_ID",
                    "                 , UPD_USER_ID",
                    "                 , UPD_DT",
                    "                 )",
                    "            VALUES",
                    "                 ( @USER_ID",
                    "                 , @APP_UUID",
                    "                 , @APP_FUN_ID",
                    "                 , {APP_ROLE_ID}",
                    "                 , @UPD_USER_ID",
                    "                 , GETDATE()",
                    "                 );"
                    ),
                    new List<DBParameter>
                    {
                        new DBParameter{Name = FunRoleRemindTimePara.ParaField.APP_ROLE_ID, Value = role}
                    }));
            }

            commandText.AppendLine(string.Join(Environment.NewLine,
                new object[]
                {
                    "        UPDATE APP_USER_FUN",
                    "           SET REMIND_MINUTE = {REMIND_MINUTE}",
                    "             , UPD_USER_ID = @UPD_USER_ID",
                    "             , UPD_DT = GETDATE()",
                    "         WHERE USER_ID = @USER_ID",
                    "           AND APP_UUID = @APP_UUID",
                    "           AND APP_FUN_ID = @APP_FUN_ID;",

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

            dbParameters.Add(new DBParameter { Name = FunRoleRemindTimePara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = FunRoleRemindTimePara.ParaField.APP_UUID, Value = para.AppUUID });
            dbParameters.Add(new DBParameter { Name = FunRoleRemindTimePara.ParaField.APP_FUN_ID, Value = para.AppFunID });
            dbParameters.Add(new DBParameter { Name = FunRoleRemindTimePara.ParaField.REMIND_MINUTE, Value = para.RemindMinute });
            dbParameters.Add(new DBParameter { Name = FunRoleRemindTimePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditFunRoleRemindTimeResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion

        #region - 取得使用者功能角色 -
        public class AppUserFunRolePara
        {
            public enum ParaField
            {
                USER_ID,
                APP_UUID,
                APP_FUN_ID
            }

            public DBVarChar UserID;
            public DBVarChar AppUUID;
            public DBVarChar AppFunID;
        }

        public class AppUserFunRole : DBTableRow
        {
            public DBVarChar UserID;
            public DBVarChar AppUUID;
            public DBVarChar AppFunID;
            public DBVarChar AppRoleID;
        }

        public List<AppUserFunRole> SelectAppUserFunRoleList(AppUserFunRolePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT USER_ID AS UserID",
                    "     , APP_UUID AS AppUUID",
                    "     , APP_FUN_ID AS AppFunID",
                    "     , APP_ROLE_ID AS AppRoleID",
                    "  FROM APP_USER_FUN_ROLE",
                    " WHERE USER_ID = {USER_ID}",
                    "   AND APP_UUID = {APP_UUID}",
                    "   AND APP_FUN_ID = {APP_FUN_ID}"
                }));

            dbParameters.Add(new DBParameter { Name = AppUserFunRolePara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = AppUserFunRolePara.ParaField.APP_UUID, Value = para.AppUUID });
            dbParameters.Add(new DBParameter { Name = AppUserFunRolePara.ParaField.APP_FUN_ID, Value = para.AppFunID });

            return GetEntityList<AppUserFunRole>(commandText.ToString(), dbParameters);
        }
        #endregion
    }
}