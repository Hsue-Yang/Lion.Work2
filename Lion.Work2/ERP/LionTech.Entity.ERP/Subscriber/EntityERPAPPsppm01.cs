using System;
using System.Collections.Generic;

namespace LionTech.Entity.ERP.Subscriber
{
    public class EntityERPAPPsppm01 : EntitySubscriber
    {
        public EntityERPAPPsppm01(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class RawCMUserPara
        {
            public enum ParaField
            {
                USER_ID,
                USER_COM_ID,
                USER_AREA,
                USER_GROUP,
                USER_PLACE,
                USER_DEPT,
                USER_TEAM,
                USER_TITLE,
                USER_IDENTITY,
                USER_LEVEL,
                USER_JOB_TITLE,
                UPD_USER_ID,
                UPD_EDI_EVENT_NO
            }

            public DBVarChar UserID;
            public DBVarChar UserComID;
            public DBVarChar UserArea;
            public DBVarChar UserGroup;
            public DBVarChar UserPlace;
            public DBVarChar UserDept;
            public DBVarChar Userteam;
            public DBVarChar UserJobTitle;
            public DBVarChar UserTitle;
            public DBVarChar UserIdentity;
            public DBVarChar UserLevel;
            public DBVarChar UpdUserID;
            public DBChar UpdEDIEventNo;
        }

        public enum EnumEditRawCMUserResult
        {
            Success,
            Failure
        }

        public EnumEditRawCMUserResult EditRawCMUserOrg(RawCMUserPara para)
        {
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1); ",
                "SET @RESULT = 'N'; ",
                "BEGIN TRANSACTION ",
                "    BEGIN TRY ",

                "        DELETE FROM RAW_CM_USER_ORG WHERE USER_ID={USER_ID}; ",

                "        INSERT INTO dbo.RAW_CM_USER_ORG (",
                "               USER_ID",
                "             , USER_COM_ID",
                "             , USER_AREA",
                "             , USER_GROUP",
                "             , USER_PLACE",
                "             , USER_DEPT",
                "             , USER_TEAM",
                "             , USER_JOB_TITLE",
                "             , USER_TITLE",
                "             , USER_IDENTITY",
                "             , USER_LEVEL",
                "             , UPD_USER_ID",
                "             , UPD_DT",
                "             , UPD_EDI_EVENT_NO",
                "        ) VALUES (",
                "               {USER_ID}",
                "             , {USER_COM_ID}",
                "             , {USER_AREA}",
                "             , {USER_GROUP}",
                "             , {USER_PLACE}",
                "             , {USER_DEPT}",
                "             , {USER_TEAM}",
                "             , {USER_JOB_TITLE}",
                "             , {USER_TITLE}",
                "             , {USER_IDENTITY}",
                "             , {USER_LEVEL}",
                "             , {UPD_USER_ID}",
                "             , GETDATE()",
                "             , {UPD_EDI_EVENT_NO}",
                "        )",

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
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_COM_ID.ToString(), Value = para.UserComID });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_AREA.ToString(), Value = para.UserArea });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_GROUP.ToString(), Value = para.UserGroup });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_PLACE.ToString(), Value = para.UserPlace });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_DEPT.ToString(), Value = para.UserDept });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_TEAM.ToString(), Value = para.Userteam });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_JOB_TITLE.ToString(), Value = para.UserJobTitle });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_TITLE.ToString(), Value = para.UserTitle });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_IDENTITY.ToString(), Value = para.UserIdentity });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_LEVEL.ToString(), Value = para.UserLevel });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.UPD_USER_ID.ToString(), Value = base.UpdUserID });
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.UPD_EDI_EVENT_NO.ToString(), Value = para.UpdEDIEventNo });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditRawCMUserResult.Success : EnumEditRawCMUserResult.Failure;
        }

        public enum EnumDeleteRawCMUserResult
        {
            Success,
            Failure
        }

        public EnumDeleteRawCMUserResult DeleteRawCMUserOrg(RawCMUserPara para)
        {
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1); ",
                "SET @RESULT = 'N'; ",
                "BEGIN TRANSACTION ",
                "    BEGIN TRY ",

                "        DELETE FROM RAW_CM_USER_ORG WHERE USER_ID={USER_ID}; ",

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

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumDeleteRawCMUserResult.Success : EnumDeleteRawCMUserResult.Failure;
        }
    }
}