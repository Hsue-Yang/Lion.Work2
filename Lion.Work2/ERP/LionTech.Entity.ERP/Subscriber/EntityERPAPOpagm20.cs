using System;
using System.Collections.Generic;

namespace LionTech.Entity.ERP.Subscriber
{
    public class EntityERPAPOpagm20 : EntitySubscriber
    {
        public EntityERPAPOpagm20(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class RawCMUserPara
        {
            public enum ParaField
            {
                USER_ID,
                USER_NM,
                USER_COM_ID,
                USER_UNIT_ID,
                IS_LEFT,
                API_NO,
                IP_ADDRESS,
                EXEC_SYS_ID,
                UPD_USER_ID,
                UPD_EDI_EVENT_NO,
                USER_EMAIL,
                USER_TITLE_ID,
                USER_WORK_ID,
                USER_TEAM_ID
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UserEMail;
            public DBVarChar UserComID;
            public DBVarChar UserUnitID;
            public DBChar IsLeft;
            public DBChar ApiNO;
            public DBVarChar IPAddress;
            public DBVarChar ExecSysID;
            public DBChar UpdEDIEventNo;
            public DBVarChar UserTitleID;
            public DBVarChar UserWorkID;
            public DBVarChar UserTeamID;
        }

        public enum EnumEditRawCMUserResult
        {
            Success,
            Failure
        }

        public EnumEditRawCMUserResult EditRawCMUser(RawCMUserPara para)
        {
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1); ",
                "SET @RESULT = 'N'; ",
                "BEGIN TRANSACTION ",
                "    BEGIN TRY ",

                "        DECLARE @ROLE_GROUP_ID VARCHAR(20); ",
                "        SELECT @ROLE_GROUP_ID=ROLE_GROUP_ID FROM SYS_USER_MAIN WHERE USER_ID={USER_ID}; ",
                "        DECLARE @USER_SALARY_COM_ID VARCHAR(20); ",
                "        SELECT @USER_SALARY_COM_ID=USER_SALARY_COM_ID FROM RAW_CM_USER WHERE USER_ID={USER_ID}; ",

                "        DELETE FROM RAW_CM_USER WHERE USER_ID={USER_ID}; ",
                "        INSERT INTO dbo.RAW_CM_USER (",
                "               USER_ID",
                "             , USER_NM",
                "             , USER_EMAIL",
                "             , USER_COM_ID",
                "             , USER_SALARY_COM_ID",
                "             , USER_UNIT_ID",
                "             , USER_TEAM_ID",
                "             , USER_TITLE_ID",
                "             , USER_WORK_ID",
                "             , IS_LEFT",
                "             , UPD_USER_ID",
                "             , UPD_DT",
                "             , UPD_EDI_EVENT_NO",
                "        ) VALUES (",
                "               {USER_ID}",
                "             , {USER_NM}",
                "             , {USER_EMAIL}",
                "             , {USER_COM_ID}",
                "             , @USER_SALARY_COM_ID",
                "             , {USER_UNIT_ID}",
                "             , {USER_TEAM_ID}",
                "             , {USER_TITLE_ID}",
                "             , {USER_WORK_ID}",
                "             , {IS_LEFT}",
                "             , {UPD_USER_ID}",
                "             , GETDATE()",
                "             , {UPD_EDI_EVENT_NO}",
                "        )",

                "        IF NOT EXISTS(SELECT USER_ID FROM SYS_USER_MAIN WHERE USER_ID={USER_ID}) ",
                "        BEGIN ",
                "            INSERT INTO dbo.SYS_USER_MAIN (",
                "                   USER_ID",
                "                 , ROLE_GROUP_ID",
                "                 , RESTRICT_TYPE",
                "                 , USER_NM",
                "                 , USER_PWD",
                "                 , PWD_VALID_DATE",
                "                 , ERROR_TIMES",
                "                 , IS_LOCK",
                "                 , LOCK_DT",
                "                 , IS_DISABLE",
                "                 , IS_LEFT",
                "                 , LEFT_DATE",
                "                 , IS_DAILY_FIRST",
                "                 , LAST_LOGIN_DATE",
                "                 , UPD_USER_ID",
                "                 , UPD_DT",
                "            ) VALUES (",
                "                   {USER_ID}",
                "                 , @ROLE_GROUP_ID",
                "                 , 'N'",
                "                 , {USER_NM}",
                "                 , NULL",
                "                 , NULL",
                "                 , NULL",
                "                 , 'N'",
                "                 , NULL",
                "                 , {IS_LEFT}",
                "                 , {IS_LEFT}",
                "                 , NULL",
                "                 , 'N'",
                "                 , NULL",
                "                 , {UPD_USER_ID}",
                "                 , GETDATE()",
                "            )",
                "        END; ",
                "        ELSE ",
                "        BEGIN ",
                "            UPDATE SYS_USER_MAIN ",
                "               SET IS_DISABLE={IS_LEFT} ",
                "                 , IS_LEFT={IS_LEFT} ",
                "                 , UPD_USER_ID={UPD_USER_ID} ",
                "                 , UPD_DT=GETDATE() ",
                "             WHERE USER_ID={USER_ID}; ",
                "        END; ",

                "        IF {IS_LEFT}='N' AND NOT EXISTS(SELECT USER_ID FROM SYS_USER_SYSTEM_ROLE WHERE USER_ID={USER_ID} AND SYS_ID='ERPAP' AND ROLE_ID='USER') ",
                "        BEGIN",
                "            INSERT INTO SYS_USER_SYSTEM_ROLE VALUES ({USER_ID},'ERPAP','USER',{UPD_USER_ID},GETDATE());",
                "            EXECUTE dbo.SP_LOG_USER_SYSTEM_ROLE {USER_ID} ,NULL ,NULL ,{API_NO} ,'"+ Mongo_BaseAP.EnumModifyType.U +"', {EXEC_SYS_ID} ,{IP_ADDRESS} ,{UPD_USER_ID};",
                "        END;",

                "        SET @RESULT = 'Y'; ",
                "        COMMIT; ",
                "    END TRY ",
                "    BEGIN CATCH ",
                "        SET @RESULT = 'N'; ",
                "        ROLLBACK TRANSACTION; ",
                "    END CATCH ",
                "; ",
                "SELECT @RESULT; ",
                Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_NM.ToString(), Value = para.UserNM });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_EMAIL.ToString(), Value = para.UserEMail });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_COM_ID.ToString(), Value = para.UserComID });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_UNIT_ID.ToString(), Value = para.UserUnitID });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_TEAM_ID.ToString(), Value = para.UserTeamID });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_TITLE_ID.ToString(), Value = para.UserTitleID });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_WORK_ID.ToString(), Value = para.UserWorkID });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.IS_LEFT.ToString(), Value = para.IsLeft });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.API_NO.ToString(), Value = para.ApiNO });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.IP_ADDRESS.ToString(), Value = para.IPAddress });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.EXEC_SYS_ID.ToString(), Value = para.ExecSysID });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.UPD_USER_ID.ToString(), Value = UpdUserID });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.UPD_EDI_EVENT_NO.ToString(), Value = para.UpdEDIEventNo });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditRawCMUserResult.Success : EnumEditRawCMUserResult.Failure;
        }

        public enum EnumDeleteRawCMUserResult
        {
            Success,
            Failure
        }

        public EnumDeleteRawCMUserResult DeleteRawCMUser(RawCMUserPara para)
        {
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1); ",
                "SET @RESULT = 'N'; ",
                "BEGIN TRANSACTION ",
                "    BEGIN TRY ",

                "        DELETE FROM RAW_CM_USER WHERE USER_ID={USER_ID}; ",

                "        UPDATE SYS_USER_MAIN ",
                "           SET IS_DISABLE='Y', UPD_USER_ID={UPD_USER_ID}, UPD_DT=GETDATE() ",
                "         WHERE USER_ID={USER_ID}; ",

                "        SET @RESULT = 'Y'; ",
                "        COMMIT; ",
                "    END TRY ",
                "    BEGIN CATCH ",
                "        SET @RESULT = 'N'; ",
                "        ROLLBACK TRANSACTION; ",
                "    END CATCH ",
                "; ",
                "SELECT @RESULT; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.UPD_USER_ID.ToString(), Value = UpdUserID });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumDeleteRawCMUserResult.Success : EnumDeleteRawCMUserResult.Failure;
        }
    }
}