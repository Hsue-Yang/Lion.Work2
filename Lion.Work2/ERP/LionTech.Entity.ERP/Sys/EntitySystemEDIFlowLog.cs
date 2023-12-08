using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemEDIFlowLog : EntitySys
    {
        public EntitySystemEDIFlowLog(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class StatusIDPara : DBCulture
        {
            public StatusIDPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                CODE_NM
            }
        }

        public class StatusID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                CODE_ID, CODE_NM
            }

            public DBVarChar CodeID;
            public DBNVarChar CodeNM;

            public string ItemText()
            {
                return this.CodeNM.StringValue();
            }

            public string ItemValue()
            {
                return this.CodeID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<StatusID> SelectStatusIDList(StatusIDPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT CODE_ID, dbo.FN_GET_NMID(CODE_ID, {CODE_NM}) AS CODE_NM ", Environment.NewLine,
                "FROM CM_CODE ", Environment.NewLine,
                "WHERE CODE_KIND='0002' ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = StatusIDPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(StatusIDPara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<StatusID> statusIDList = new List<StatusID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    StatusID statusID = new StatusID()
                    {
                        CodeID = new DBVarChar(dataRow[StatusID.DataField.CODE_ID.ToString()]),
                        CodeNM = new DBNVarChar(dataRow[StatusID.DataField.CODE_NM.ToString()])
                    };
                    statusIDList.Add(statusID);
                }
                return statusIDList;
            }
            return null;
        }

        public class SystemEDIFlowLogPara : DBCulture
        {
            public SystemEDIFlowLogPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, SYS_NM, EDI_FLOW_ID, EDI_FLOW, EDI_DATE, EDI_NO,
                DATA_DATE, STATUS_ID, RESULT_ID, IS_AUTOMATIC, IS_DELETED, UPD_USER_ID, CULTURE_ID
            }

            public enum ParaNull
            {
                NULL
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBChar EDIDate;
            public DBChar EDINO;
            public DBChar DataDate;
            public DBVarChar StatusID;
            public DBVarChar ResultID;
            public DBChar IsAutomatic;
            public DBChar IsDeleted;
            public DBVarChar UpdUserID;
        }

        public class SystemEDIFlowLog : DBTableRow
        {
            public enum DataField
            {
                EDI_NO, SYS_ID, SYS_NM, EDI_FLOW_ID, EDI_FLOW_NM,
                STATUS_ID, RESULT_ID, RESULT_CODE, EDIFLOW_STATUS_NM, EDIFLOW_RESULT_NM,
                EDI_DATE, EDI_TIME, DATA_DATE, DT_BEGIN, DT_END, IS_AUTOMATIC,
                AUTO_SCHEDULE, AUTO_EDI_NO, AUTO_FLOW_ID,
                IS_DELETED, UPD_USER_NM, UPD_DT
            }

            public DBVarChar EDINO;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar EDIFlowID;
            public DBNVarChar EDIFlowNM;
            public DBChar EDIDate;
            public DBChar EDITime;
            public DBChar DataDate;
            public DBVarChar StatusID;
            public DBVarChar ResultID;
            public DBVarChar ResultCode;
            public DBNVarChar EDIFlowStatusNM;
            public DBNVarChar EDIFlowResultNM;
            public DBDateTime DTBegin;
            public DBDateTime DTEnd;
            public DBChar IsAutomatic;
            public DBDateTime AutoSchedule;
            public DBChar AutoEDINO;
            public DBChar AutoFlowID;
            public DBChar IsDeleted;
            
            public DBVarChar UpdUserID;
            public DBDateTime UpdDt;
        }

        public List<SystemEDIFlowLog> SelectSystemEDIFlowLogList(SystemEDIFlowLogPara para)
        {
            string commandWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(para.ResultID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, " AND F.RESULT_ID={RESULT_ID}", Environment.NewLine });
            }
            if (!string.IsNullOrWhiteSpace(para.StatusID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, " AND F.STATUS_ID={STATUS_ID}", Environment.NewLine });
            }
            if (!string.IsNullOrWhiteSpace(para.EDINO.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND F.EDI_NO={EDI_NO} ", Environment.NewLine });
            }
            if (!string.IsNullOrWhiteSpace(para.DataDate.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND F.DATA_DATE={DATA_DATE} ", Environment.NewLine });
            }
            if (!string.IsNullOrWhiteSpace(para.EDIDate.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND F.EDI_DATE={EDI_DATE} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.EDIFlowID.GetValue()) && para.EDIFlowID.GetValue() != SystemEDIFlowLogPara.ParaNull.NULL.ToString())
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND F.EDI_FLOW_ID={EDI_FLOW_ID} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.EDIFlowID.GetValue()) && para.EDIFlowID.GetValue() == SystemEDIFlowLogPara.ParaNull.NULL.ToString())
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND E.EDI_FLOW_ID IS NULL ", Environment.NewLine });
            }
            else
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND E.EDI_FLOW_ID IS NOT NULL ", Environment.NewLine });
            }
            
            string commandText = string.Concat(new object[]
            {
                "SELECT M.SYS_ID, dbo.FN_GET_NMID(M.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , F.EDI_FLOW_ID, dbo.FN_GET_NMID(F.EDI_FLOW_ID, E.{EDI_FLOW}) AS EDI_FLOW_NM ", Environment.NewLine,
                "     , F.EDI_NO, F.EDI_DATE, F.EDI_TIME, F.DATA_DATE, F.DT_BEGIN, F.DT_END ", Environment.NewLine,
                "     , F.IS_AUTOMATIC, F.AUTO_EDI_NO, F.AUTO_FLOW_ID, F.IS_DELETED ", Environment.NewLine,
                "     , F.STATUS_ID, F.RESULT_ID, F.RESULT_CODE ", Environment.NewLine,
                "     , (CASE WHEN F.STATUS_ID IS NULL THEN NULL ELSE dbo.FN_GET_NMID(F.STATUS_ID, dbo.FN_GET_CM_NM('0002', F.STATUS_ID, {CULTURE_ID})) END) AS EDIFLOW_STATUS_NM ", Environment.NewLine,
                "     , (CASE WHEN F.RESULT_ID IS NULL THEN NULL ELSE dbo.FN_GET_NMID(F.RESULT_ID, dbo.FN_GET_CM_NM('0003', F.RESULT_ID, {CULTURE_ID})) END) AS EDIFLOW_RESULT_NM ", Environment.NewLine,
                "     , F.AUTO_SCHEDULE, F.AUTO_EDI_NO, F.AUTO_FLOW_ID, F.IS_DELETED ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(F.UPD_USER_ID) AS UPD_USER_NM, F.UPD_DT ", Environment.NewLine,
                "FROM EDI_FLOW F ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON F.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_EDI_FLOW E ON F.SYS_ID=E.SYS_ID AND F.EDI_FLOW_ID=E.EDI_FLOW_ID ", Environment.NewLine,
                "WHERE F.SYS_ID={SYS_ID} ", Environment.NewLine,
                commandWhere,
                "ORDER BY (CASE WHEN F.DT_BEGIN IS NULL THEN F.UPD_DT ELSE F.DT_BEGIN END) DESC ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowLogPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowLogPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIFlowLogPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowLogPara.ParaField.EDI_FLOW_ID.ToString(), Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowLogPara.ParaField.EDI_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIFlowLogPara.ParaField.EDI_FLOW.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowLogPara.ParaField.EDI_NO.ToString(), Value = para.EDINO });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowLogPara.ParaField.EDI_DATE.ToString(), Value = para.EDIDate });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowLogPara.ParaField.DATA_DATE.ToString(), Value = para.DataDate });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowLogPara.ParaField.RESULT_ID.ToString(), Value = para.ResultID });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowLogPara.ParaField.STATUS_ID.ToString(), Value = para.StatusID });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowLogPara.ParaField.CULTURE_ID.ToString(), Value = new DBVarChar(para.CultureID) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemEDIFlowLog> systemEDIFlowLogList = new List<SystemEDIFlowLog>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemEDIFlowLog systemEDIFlowLog = new SystemEDIFlowLog()
                    {
                        EDINO = new DBVarChar(dataRow[SystemEDIFlowLog.DataField.EDI_NO.ToString()]),
                        SysID = new DBVarChar(dataRow[SystemEDIFlowLog.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemEDIFlowLog.DataField.SYS_NM.ToString()]),
                        EDIFlowID = new DBVarChar(dataRow[SystemEDIFlowLog.DataField.EDI_FLOW_ID.ToString()]),
                        EDIFlowNM = new DBNVarChar(dataRow[SystemEDIFlowLog.DataField.EDI_FLOW_NM.ToString()]),

                        EDIDate = new DBChar(dataRow[SystemEDIFlowLog.DataField.EDI_DATE.ToString()]),
                        EDITime = new DBChar(dataRow[SystemEDIFlowLog.DataField.EDI_TIME.ToString()]),
                        DataDate = new DBChar(dataRow[SystemEDIFlowLog.DataField.DATA_DATE.ToString()]),

                        StatusID = new DBVarChar(dataRow[SystemEDIFlowLog.DataField.STATUS_ID.ToString()]),
                        ResultID = new DBVarChar(dataRow[SystemEDIFlowLog.DataField.RESULT_ID.ToString()]),
                        ResultCode = new DBVarChar(dataRow[SystemEDIFlowLog.DataField.RESULT_CODE.ToString()]),
                        EDIFlowStatusNM = new DBNVarChar(dataRow[SystemEDIFlowLog.DataField.EDIFLOW_STATUS_NM.ToString()]),
                        EDIFlowResultNM = new DBNVarChar(dataRow[SystemEDIFlowLog.DataField.EDIFLOW_RESULT_NM.ToString()]),

                        DTBegin = new DBDateTime(dataRow[SystemEDIFlowLog.DataField.DT_BEGIN.ToString()]),
                        DTEnd = new DBDateTime(dataRow[SystemEDIFlowLog.DataField.DT_END.ToString()]),
                        IsAutomatic = new DBChar(dataRow[SystemEDIFlowLog.DataField.IS_AUTOMATIC.ToString()]),
                        AutoSchedule = new DBDateTime(dataRow[SystemEDIFlowLog.DataField.AUTO_SCHEDULE.ToString()]),
                        AutoEDINO = new DBChar(dataRow[SystemEDIFlowLog.DataField.AUTO_EDI_NO.ToString()]),
                        AutoFlowID = new DBChar(dataRow[SystemEDIFlowLog.DataField.AUTO_FLOW_ID.ToString()]),
                        IsDeleted = new DBChar(dataRow[SystemEDIFlowLog.DataField.IS_DELETED.ToString()]),

                        UpdUserID = new DBVarChar(dataRow[SystemEDIFlowLog.DataField.UPD_USER_NM.ToString()]),
                        UpdDt = new DBDateTime(dataRow[SystemEDIFlowLog.DataField.UPD_DT.ToString()])
                    };
                    systemEDIFlowLogList.Add(systemEDIFlowLog);
                }
                return systemEDIFlowLogList;
            }
            return null;
        }
        public enum EnumUpdateWaitStatusLogResult
        {
            Success,
            Failure
        }

        public EnumUpdateWaitStatusLogResult UpdateWaitStatusLog(SystemEDIFlowLogPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1) = 'N';",
                "DECLARE @ERROR_LINE INT;",
                "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                "BEGIN TRANSACTION",
                "    BEGIN TRY",

                "        UPDATE EDI_FLOW",
                "           SET STATUS_ID = 'F'",
                "             , RESULT_ID = 'F'",
                "             , UPD_USER_ID = {UPD_USER_ID}",
                "             , UPD_DT = GETDATE()",
                "         WHERE SYS_ID = {SYS_ID}",
                "           AND STATUS_ID = 'W'",
                para.DataDate.IsNull() == false
                    ? "           AND DATA_DATE = {DATA_DATE}"
                    : string.Empty,
                para.EDIFlowID.IsNull() == false
                    ? "           AND EDI_FLOW_ID = {EDI_FLOW_ID}"
                    : string.Empty,

                "        SET @RESULT = 'Y';",
                "        COMMIT;",
                "    END TRY",
                "    BEGIN CATCH",
                "        SET @RESULT = 'N';",
                "        SET @ERROR_LINE = ERROR_LINE();",
                "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                "        ROLLBACK TRANSACTION;",
                "    END CATCH;",
                "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
            }));

            dbParameters.Add(new DBParameter { Name = SystemEDIFlowLogPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowLogPara.ParaField.EDI_FLOW_ID.ToString(), Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowLogPara.ParaField.DATA_DATE.ToString(), Value = para.DataDate });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowLogPara.ParaField.UPD_USER_ID.ToString(), Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumUpdateWaitStatusLogResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
    }
}