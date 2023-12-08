using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Subscriber
{
    public class EntitySubscriber : DBEntity
    {
#if !NET461
        public EntitySubscriber(string connectionName)
            : base(connectionName)
        {
        }
#endif
        public EntitySubscriber(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public DBVarChar UpdUserID = new DBVarChar("APIService.ERP.Subscriber");

        public class CMCodePara
        {
            public enum ParaField
            {
                CODE_KIND,
                CODE_KIND_NM,
                CODE_ID,
                IS_DISABLE,
                CODE_NM,
                SORT_ORDER,
                UPD_USER_ID,
                UPD_EDI_EVENT_NO
            }

            public DBVarChar CodeKind;
            public DBNVarChar CodeKindNM;
            public DBVarChar CodeID;
            public DBChar IsDisable;
            public DBNVarChar CodeNM;
            public DBVarChar SortOrder;
            public DBVarChar UpdUserID;
            public DBChar UpdEDIEventNo;

        }

        public enum EnumEditCMCodeResult
        {
            Success,
            Failure
        }

        public EnumEditCMCodeResult EditCMCode(CMCodePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            StringBuilder strbCommand = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT VARCHAR(50) = '" + EnumEditCMCodeResult.Failure + "'; ",
                "DECLARE @ERROR_LINE INT;",
                "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                "DECLARE @CODE_KIND VARCHAR(20); ",

                "BEGIN TRANSACTION ",
                "    BEGIN TRY ",
                Environment.NewLine
            }));

            if (para.CodeKind != null &&
                para.CodeKind.IsNull() == false)
            {
                strbCommand.AppendLine(string.Join(Environment.NewLine, new object[]
                {
                    "        SET @CODE_KIND = {CODE_KIND} "
                }));
                dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.CODE_KIND.ToString(), Value = para.CodeKind });
            }
            else
            {
                strbCommand.AppendLine(string.Join(Environment.NewLine, new object[]
                {
                    "        SELECT @CODE_KIND = CODE_KIND ",
                    "        FROM CM_CODEH ",
                    "        WHERE UPPER(CODE_KIND_NM_EN_US) = {CODE_KIND_NM} "
                }));
                dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.CODE_KIND_NM.ToString(), Value = para.CodeKindNM });
            }

            strbCommand.AppendLine(string.Join(Environment.NewLine, new object[]
            {
                "        IF @CODE_KIND IS NOT NULL ",
                "        BEGIN ",
                "            DELETE FROM CM_CODE WHERE CODE_ID = {CODE_ID} AND CODE_KIND = @CODE_KIND; ",

                "            INSERT INTO dbo.CM_CODE (",
                "                   CODE_KIND",
                "                 , CODE_ID",
                "                 , CODE_PARENT",
                "                 , CODE_NM_ZH_TW",
                "                 , CODE_NM_ZH_CN",
                "                 , CODE_NM_EN_US",
                "                 , CODE_NM_TH_TH",
                "                 , CODE_NM_JA_JP",
                "                 , CODE_NM_KO_KR",
                "                 , IS_DISABLE",
                "                 , SORT_ORDER",
                "                 , UPD_USER_ID",
                "                 , UPD_DT",
                "                 , UPD_EDI_EVENT_NO",
                "            ) VALUES (",
                "                   @CODE_KIND",
                "                 , {CODE_ID}",
                "                 , NULL",
                "                 , {CODE_NM}",
                "                 , {CODE_NM}",
                "                 , {CODE_NM}",
                "                 , {CODE_NM}",
                "                 , {CODE_NM}",
                "                 , {CODE_NM}",
                "                 , (CASE WHEN {IS_DISABLE} = '0' THEN 'N' ELSE 'Y' END)",
                "                 , RIGHT('000000' + {CODE_ID}, 6)",
                "                 , {UPD_USER_ID}",
                "                 , GETDATE()",
                "                 , {UPD_EDI_EVENT_NO}",
                "            )",

                "        END; ",

                "        SET @RESULT = '" + EnumEditCMCodeResult.Success + "'; ",
                "        COMMIT; ",
                "    END TRY ",
                "    BEGIN CATCH ",
                "        SET @RESULT = '" + EnumEditCMCodeResult.Failure + "'; ",
                "        SET @ERROR_LINE = ERROR_LINE();",
                "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                "        ROLLBACK TRANSACTION; ",
                "    END CATCH; ",
                "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage; ", Environment.NewLine
            }));

            dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.CODE_ID.ToString(), Value = para.CodeID });
            dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.IS_DISABLE.ToString(), Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.CODE_NM.ToString(), Value = para.CodeNM });
            dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.UPD_USER_ID.ToString(), Value = UpdUserID });
            dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.UPD_EDI_EVENT_NO.ToString(), Value = para.UpdEDIEventNo });

            var result = GetEntityList<ExecuteResult>(strbCommand.ToString(), dbParameters).SingleOrDefault();

            var enumResult = ((EnumEditCMCodeResult)Enum.Parse(typeof(EnumEditCMCodeResult), result.Result.GetValue()));

            if (enumResult == EnumEditCMCodeResult.Failure)
            {
                throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
            }

            return enumResult;
        }

        public enum EnumDeleteCMCodeResult
        {
            Success,
            Failure
        }

        public EnumDeleteCMCodeResult DeleteCMCode(CMCodePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            StringBuilder strbCommand = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT VARCHAR(50) = '" + EnumDeleteCMCodeResult.Failure + "'; ",
                "DECLARE @ERROR_LINE INT;",
                "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                "DECLARE @CODE_KIND VARCHAR(20); ",

                "BEGIN TRANSACTION ",
                "    BEGIN TRY ",
                Environment.NewLine
            }));

            if (para.CodeKind != null &&
                para.CodeKind.IsNull() == false)
            {
                strbCommand.AppendLine(string.Join(Environment.NewLine, new object[]
                {
                    "        SET @CODE_KIND = {CODE_KIND} "
                }));
                dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.CODE_KIND.ToString(), Value = para.CodeKind });
            }
            else
            {
                strbCommand.AppendLine(string.Join(Environment.NewLine, new object[]
                {
                    "        SELECT @CODE_KIND = CODE_KIND ",
                    "        FROM CM_CODEH ",
                    "        WHERE UPPER(CODE_KIND_NM_EN_US) = {CODE_KIND_NM} "
                }));
                dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.CODE_KIND_NM.ToString(), Value = para.CodeKindNM });
            }
            
            strbCommand.AppendLine(string.Join(Environment.NewLine, new object[]
            {
                "       IF @CODE_KIND IS NOT NULL ",
                "       BEGIN ",
                "           DELETE FROM CM_CODE WHERE CODE_ID = {CODE_ID} AND CODE_KIND = @CODE_KIND; ",
                "       END; ",

                "       SET @RESULT = '" + EnumDeleteCMCodeResult.Success + "'; ",
                "       COMMIT; ",
                "    END TRY ",
                "    BEGIN CATCH ",
                "        SET @RESULT = '" + EnumDeleteCMCodeResult.Failure + "'; ",
                "        SET @ERROR_LINE = ERROR_LINE();",
                "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                "        ROLLBACK TRANSACTION; ",
                "    END CATCH; ",
                "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage; ", Environment.NewLine
            }));

            dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.CODE_KIND_NM.ToString(), Value = para.CodeKindNM });
            dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.CODE_ID.ToString(), Value = para.CodeID });

            var result = GetEntityList<ExecuteResult>(strbCommand.ToString(), dbParameters).SingleOrDefault();

            var enumResult = ((EnumDeleteCMCodeResult)Enum.Parse(typeof(EnumDeleteCMCodeResult), result.Result.GetValue()));

            if (enumResult == EnumDeleteCMCodeResult.Failure)
            {
                throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
            }

            return enumResult;
        }
    }
}