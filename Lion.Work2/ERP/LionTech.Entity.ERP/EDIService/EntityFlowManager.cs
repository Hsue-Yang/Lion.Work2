using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace LionTech.Entity.ERP.EDIService
{
    public class EntityFlowManager : EntityEDIService
    {
        public EntityFlowManager(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class EDIFlowPara
        {
            public enum ParaField
            {
                SYS_ID, EDI_FLOW_ID, UPD_USER_ID, DATA_DATE, EDI_NO
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBChar DataDate;
            public DBChar EDINo;
        }

        public class EDIFlow : DBTableRow
        {
            public enum DataField
            {
                EDI_NO, SYS_ID,
                EDI_FLOW_ID,
                DATA_DATE, 
                DT_BEGIN, DT_END,
                STATUS_ID, RESULT_ID
            }

            public DBChar EDINo;
            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBChar DataDate;
            public DBVarChar StatusID;
            public DBVarChar ResultID;
            public DBDateTime DTBegin;
            public DBDateTime DTEnd;
        }

        private class InsertNewEDIExecuteResult : ExecuteResult
        {
            public DBEDINO EDI_NO;
        }

        public string ExecuteEDIFlow(EDIFlowPara para, DBVarChar updUserID)
        {
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1) = 'Y';",
                "DECLARE @ERROR_LINE INT;",
                "DECLARE @ERROR_NUMBER INT;",
                "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                "DECLARE @TODAY_YMD CHAR(8); ",
                "DECLARE @EDI_NO CHAR(14); ",

                "SET @TODAY_YMD = dbo.FN_GET_SYSDATE(NULL); ",

                "SELECT TOP 1 @EDI_NO=EDI_NO ",
                "FROM EDI_FLOW ",
                "WHERE STATUS_ID='W' AND SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID}",
                "ORDER BY EDI_NO DESC; ",

                "IF @EDI_NO IS NULL ",
                "BEGIN ",
                "    BEGIN TRANSACTION ",
                "        BEGIN TRY ",
                "            SELECT @EDI_NO=dbo.FN_GENERATE_EDI_NO(); ",
                             
                "            INSERT INTO EDI_FLOW VALUES ( ",
                "                @EDI_NO, {SYS_ID}, {EDI_FLOW_ID}, NULL, NULL, {DATA_DATE}, 'W', NULL, NULL ",
                             
                "              , NULL, NULL, 'N', NULL, NULL, NULL, 'N' ",
                "              , {UPD_USER_ID}, GETDATE() ",
                "            ); ",

                "            SET @RESULT = 'Y'; ",
                "            COMMIT; ",
                "        END TRY ",
                "        BEGIN CATCH ",
                "            SET @RESULT = 'N';",
                "            SET @ERROR_LINE = ERROR_LINE();",
                "            SET @ERROR_NUMBER = ERROR_NUMBER();",
                "            SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                "            ROLLBACK TRANSACTION; ",
                "        END CATCH;",
                "END; ",

                "SELECT @EDI_NO AS EDI_NO, @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage, @ERROR_NUMBER AS ErrorNumber;"
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = EDIFlowPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = EDIFlowPara.ParaField.EDI_FLOW_ID, Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = EDIFlowPara.ParaField.DATA_DATE, Value = para.DataDate });
            dbParameters.Add(new DBParameter { Name = EDIFlowPara.ParaField.UPD_USER_ID, Value = ((updUserID == null) ? this.UpdUserID : updUserID) });

            int execTimes = 0;
            Exception ex = new Exception($"{nameof(ExecuteEDIFlow)} failure");
            while (execTimes < 10)
            {
                execTimes++;
                try
                {
                    var result = GetEntityList<InsertNewEDIExecuteResult>(commandText, dbParameters).SingleOrDefault();

                    if (result != null &&
                        result.Result.GetValue() == EnumYN.Y.ToString())
                    {
                        return result.EDI_NO.GetValue();
                    }

                    Thread.Sleep(500);

                    ex = new EntityExecuteResultException(result);
                    throw ex;
                }
                catch (EntityExecuteResultException exr)
                {
                    ex = exr;
                    if (exr.EntityExceptionType == EnumEntityExceptionMessage.CannotInsertDuplicatekey)
                    {
                        continue;
                    }

                    throw;
                }
            }

            throw ex;
        }

        public EDIFlow SelectEDIFlow(EDIFlowPara para)
        {
            string commandText = string.Concat(new object[]
                {
                    "SELECT EDI_NO ", Environment.NewLine,
                    "	, SYS_ID ", Environment.NewLine,
                    "	, EDI_FLOW_ID ", Environment.NewLine,
                    "	, STATUS_ID ", Environment.NewLine,
                    "	, RESULT_ID ", Environment.NewLine,
                    "	, DATA_DATE ", Environment.NewLine,
                    "	, DT_BEGIN ", Environment.NewLine,
                    "	, DT_END ", Environment.NewLine,
                    "FROM EDI_FLOW ", Environment.NewLine,
                    "WHERE EDI_NO = {EDI_NO} AND EDI_FLOW_ID = {EDI_FLOW_ID} ", Environment.NewLine,
                });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = EDIFlowPara.ParaField.EDI_NO, Value = para.EDINo });
            dbParameters.Add(new DBParameter { Name = EDIFlowPara.ParaField.EDI_FLOW_ID, Value = para.EDIFlowID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                EDIFlow ediFlow = new EDIFlow()
                    {
                        EDINo = new DBChar(dataTable.Rows[0][EDIFlow.DataField.EDI_NO.ToString()]),
                        SysID = new DBVarChar(dataTable.Rows[0][EDIFlow.DataField.SYS_ID.ToString()]),
                        EDIFlowID = new DBVarChar(dataTable.Rows[0][EDIFlow.DataField.EDI_FLOW_ID.ToString()]),
                        DataDate = new DBChar(dataTable.Rows[0][EDIFlow.DataField.DATA_DATE.ToString()]),
                        StatusID = new DBVarChar(dataTable.Rows[0][EDIFlow.DataField.STATUS_ID.ToString()]),
                        ResultID = new DBVarChar(dataTable.Rows[0][EDIFlow.DataField.RESULT_ID.ToString()]),
                        DTBegin = new DBDateTime(dataTable.Rows[0][EDIFlow.DataField.DT_BEGIN.ToString()]),
                        DTEnd = new DBDateTime(dataTable.Rows[0][EDIFlow.DataField.DT_END.ToString()]),
                    };
                return ediFlow;
            }
            return null;
        }
    }
}
