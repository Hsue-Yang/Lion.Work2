using System;
using System.Collections.Generic;
using System.Linq;

namespace LionTech.Entity.ERP.Subscriber
{
    public class EntityERPAPPsppm00 : EntitySubscriber
    {
        public EntityERPAPPsppm00(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class RawCMUserDetailPara
        {
            public enum ParaField
            {
                USER_ID, USER_IDNO, USER_BIRTHDAY,
                UPD_USER_ID, UPD_EDI_EVENT_NO,
                USER_SALARY_COM_ID
            }

            public DBVarChar UserID;
            public DBVarChar UserIDNO;
            public DBChar UserBirthday;
            public DBChar UserSalaryComID;
            public DBChar UpdEDIEventNo;
        }

        public enum EnumEditeRawCMUserDetailResult
        {
            Success, Failure
        }

        public EnumEditeRawCMUserDetailResult EditRawCMUserDetail(RawCMUserDetailPara para)
        {
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1); ",
                "SET @RESULT = 'N'; ",
                "BEGIN TRANSACTION ",
                "    BEGIN TRY ",
                "        DELETE FROM RAW_CM_USER_DETAIL ",
                "         WHERE USER_ID={USER_ID}; ",

                "        INSERT RAW_CM_USER_DETAIL ",
                "        VALUES ( ",
                "               {USER_ID}, {USER_IDNO}, {USER_BIRTHDAY}, {USER_SALARY_COM_ID} ",
                "             , {UPD_USER_ID}, GETDATE(), {UPD_EDI_EVENT_NO} ",
                "               ); ",

                "           SET @RESULT = 'Y'; ",
                "        COMMIT; ",
                "    END TRY ",
                "    BEGIN CATCH ",
                "        SET @RESULT = 'N'; ",
                "        ROLLBACK TRANSACTION; ",
                "    END CATCH; ",
                "SELECT @RESULT; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RawCMUserDetailPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = RawCMUserDetailPara.ParaField.USER_IDNO.ToString(), Value = para.UserIDNO });
            dbParameters.Add(new DBParameter { Name = RawCMUserDetailPara.ParaField.USER_BIRTHDAY.ToString(), Value = para.UserBirthday });
            dbParameters.Add(new DBParameter { Name = RawCMUserDetailPara.ParaField.USER_SALARY_COM_ID.ToString(), Value = para.UserSalaryComID });
            dbParameters.Add(new DBParameter { Name = RawCMUserDetailPara.ParaField.UPD_USER_ID.ToString(), Value = UpdUserID });
            dbParameters.Add(new DBParameter { Name = RawCMUserDetailPara.ParaField.UPD_EDI_EVENT_NO.ToString(), Value = para.UpdEDIEventNo });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditeRawCMUserDetailResult.Success : EnumEditeRawCMUserDetailResult.Failure;
        }

        public enum EnumDeleteRawCMUserDetailResult
        {
            Success,
            Failure
        }

        public EnumDeleteRawCMUserDetailResult DeleteRawCMUserDetail(RawCMUserDetailPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();

            string commandText = string.Join(Environment.NewLine, new object[]
            {
                " DECLARE @RESULT CHAR(1) = 'N';",
                " DECLARE @ERROR_LINE INT;",
                " DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                " BEGIN TRANSACTION",
                " BEGIN TRY",
                "     DELETE FROM RAW_CM_USER_DETAIL",
                "      WHERE USER_ID = {USER_ID};",
                "        SET @RESULT = 'Y';",
                "     COMMIT;",
                " END TRY",
                " BEGIN CATCH",
                "        SET @RESULT = 'N';",
                "        SET @ERROR_LINE = ERROR_LINE();",
                "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                "   ROLLBACK TRANSACTION;",
                " END CATCH;",
                " SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
            });

            dbParameters.Add(new DBParameter { Name = RawCMUserDetailPara.ParaField.USER_ID.ToString(), Value = para.UserID });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumDeleteRawCMUserDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
    }
}