using System;
using System.Collections.Generic;
using System.Linq;

namespace LionTech.Entity.ERP.Subscriber
{
    public class EntityERPAPIscpm00 : EntitySubscriber
    {
        public EntityERPAPIscpm00(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class RawCMOrgPara
        {
            public enum ParaField
            {
                COM_ID, COM_NM, COM_BU, IS_CEASED, SORT_ORDER,
                UPD_USER_ID, UPD_EDI_EVENT_NO,
                IS_SALARY_COM, COM_COUNTRY
            }

            public DBVarChar ComID;
            public DBNVarChar ComNM;
            public DBChar ComBu;
            public DBChar IsCeased;
            public DBVarChar SortOrder;
            public DBChar UpdEDIEventNo;
            public DBChar IsSalaryCom;
            public DBChar Country;
        }

        public enum EnumEditRawCMOrgResult
        {
            Success, Failure
        }

        public EnumEditRawCMOrgResult EditRawCMOrg(RawCMOrgPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE FROM RAW_CM_ORG_COM WHERE COM_ID={COM_ID}; ", Environment.NewLine,
                "        INSERT INTO RAW_CM_ORG_COM VALUES ( ", Environment.NewLine,
                "            {COM_ID}, {COM_NM}, {COM_BU}, {COM_COUNTRY}, {IS_SALARY_COM}, {IS_CEASED}, {SORT_ORDER}, {UPD_USER_ID}, GETDATE(), {UPD_EDI_EVENT_NO} ", Environment.NewLine,
                "        ); ", Environment.NewLine,

                "        SET @RESULT = 'Y'; ", Environment.NewLine,
                "        COMMIT; ", Environment.NewLine,
                "    END TRY ", Environment.NewLine,
                "    BEGIN CATCH ", Environment.NewLine,
                "        SET @RESULT = 'N'; ", Environment.NewLine,
                "        ROLLBACK TRANSACTION; ", Environment.NewLine,
                "    END CATCH ", Environment.NewLine,
                "; ", Environment.NewLine,
                "SELECT @RESULT; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RawCMOrgPara.ParaField.COM_ID.ToString(), Value = para.ComID });
            dbParameters.Add(new DBParameter { Name = RawCMOrgPara.ParaField.COM_NM.ToString(), Value = para.ComNM });
            dbParameters.Add(new DBParameter { Name = RawCMOrgPara.ParaField.COM_BU.ToString(), Value = para.ComBu });
            dbParameters.Add(new DBParameter { Name = RawCMOrgPara.ParaField.IS_CEASED.ToString(), Value = para.IsCeased });
            dbParameters.Add(new DBParameter { Name = RawCMOrgPara.ParaField.SORT_ORDER.ToString(), Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = RawCMOrgPara.ParaField.IS_SALARY_COM.ToString(), Value = para.IsSalaryCom });
            dbParameters.Add(new DBParameter { Name = RawCMOrgPara.ParaField.COM_COUNTRY.ToString(), Value = para.Country });
            dbParameters.Add(new DBParameter { Name = RawCMOrgPara.ParaField.UPD_USER_ID.ToString(), Value = UpdUserID });
            dbParameters.Add(new DBParameter { Name = RawCMOrgPara.ParaField.UPD_EDI_EVENT_NO.ToString(), Value = para.UpdEDIEventNo });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditRawCMOrgResult.Success : EnumEditRawCMOrgResult.Failure;
        }

        public enum EnumDeleteRawCMOrgResult
        {
            Success, Failure
        }

        public EnumDeleteRawCMOrgResult DeleteRawCMOrg(RawCMOrgPara para)
        {
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                " DECLARE @RESULT CHAR(1) = 'N';",
                " DECLARE @ERROR_LINE INT;",
                " DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                " BEGIN TRANSACTION",
                " BEGIN TRY",
                "     DELETE FROM RAW_CM_ORG_COM ",
                "      WHERE COM_ID={COM_ID}; ",
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

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RawCMOrgPara.ParaField.COM_ID.ToString(), Value = para.ComID });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumDeleteRawCMOrgResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
    }
}