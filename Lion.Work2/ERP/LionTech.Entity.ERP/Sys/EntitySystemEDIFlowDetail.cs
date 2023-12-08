using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using LionTech.Utility;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemEDIFlowDetail : EntitySys
    {
        public EntitySystemEDIFlowDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public enum SortorderField
        {
            [Description("00")]
            Right,
            [Description("000")]
            Left,
            [Description("00001")]
            First
        }

        public class SCHIntervalTimePara : DBCulture
        {
            public SCHIntervalTimePara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                CODE_NM, CODE_ID
            }
        }

        public class SCHIntervalTime : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                CODE_ID, CODE_NM
            }

            public DBVarChar CodeID;
            public DBNVarChar CodeNM;

            public string ItemText()
            {
                return CodeNM.StringValue();
            }

            public string ItemValue()
            {
                return CodeID.StringValue();
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

        public List<SCHIntervalTime> SelectSCHIntervalTimeList(SCHIntervalTimePara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT  CODE_ID, dbo.FN_GET_NMID(CODE_ID, {CODE_NM}) AS CODE_NM ", Environment.NewLine,
                "FROM CM_CODE ", Environment.NewLine,
                "WHERE CODE_KIND='0025' ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SCHIntervalTimePara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SCHIntervalTimePara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SCHIntervalTime> ediFlowIntervalTimeList = new List<SCHIntervalTime>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SCHIntervalTime ediFlowInterval = new SCHIntervalTime()
                    {
                        CodeID = new DBVarChar(dataRow[SCHIntervalTime.DataField.CODE_ID.ToString()]),
                        CodeNM = new DBNVarChar(dataRow[SCHIntervalTime.DataField.CODE_NM.ToString()])
                    };
                    ediFlowIntervalTimeList.Add(ediFlowInterval);
                }
                return ediFlowIntervalTimeList;
            }
            return null;
        }

        public class EDIFlowReturnPara : DBCulture
        {
            public EDIFlowReturnPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                CODE_NM
            }
        }

        public class EDIFlowReturn : DBTableRow
        {
            public enum DataField
            {
                CODE_ID, CODE_NM
            }

            public DBVarChar CodeID;
            public DBNVarChar CodeNM;
        }

        public class EDIFlowExecuteTimePara
        {
            public enum Field
            {
                SYS_ID, EDI_FLOW_ID, EXECUTE_TIME, UPD_USER_ID
            }
            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBChar SCHExecuteTime;
            public DBVarChar UpdUserID;
        }

        public class SystemEDIFlowDetailPara
        {
            public enum Field
            {
                SYS_ID, EDI_FLOW_ID,
                EDI_FLOW_ZH_TW, EDI_FLOW_ZH_CN, EDI_FLOW_EN_US, EDI_FLOW_TH_TH, EDI_FLOW_JA_JP,
                SCH_FREQUENCY, SCH_START_DATE, SCH_START_TIME, SCH_INTERVAL_NUM, SCH_INTERVAL_TIME, SCH_END_TIME, SCH_WEEKS, SCH_DAYS_STR, SCH_KEEP_LOG_DAY,
                SCH_DATA_DELAY, PATHS_CMD, PATHS_DAT, PATHS_SRC, PATHS_RES, PATHS_BAD, PATHS_LOG,
                PATHS_FLOW_XML, PATHS_FLOW_CMD, PATHS_ZIP_DAT, PATHS_EXCEPTION, PATHS_SUMMARY,
                SORT_ORDER, UPD_USER_ID, EXECUTE_TIME
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBNVarChar EDIFlowZHTW;
            public DBNVarChar EDIFlowZHCN;
            public DBNVarChar EDIFlowENUS;
            public DBNVarChar EDIFlowTHTH;
            public DBNVarChar EDIFlowJAJP;
            public DBVarChar SCHFrequency;
            public DBChar SCHStartDate;
            public DBChar SCHStartTime;
            public DBInt SCHIntervalNum;
            public DBInt SCHIntervalTime;
            public DBChar SCHEndTime;
            public DBInt SCHWeeks;
            public DBVarChar SCHDaysStr;
            public DBInt SCHDataDelay;
            public DBInt SCHKeepLogDay;
            public DBNVarChar PATHSCmd;
            public DBNVarChar PATHSDat;
            public DBNVarChar PATHSSrc;
            public DBNVarChar PATHSRes;
            public DBNVarChar PATHSBad;
            public DBNVarChar PATHSLog;
            public DBNVarChar PATHSFlowXml;
            public DBNVarChar PATHSFlowCmd;
            public DBNVarChar PATHSZipDat;
            public DBNVarChar PATHSException;
            public DBNVarChar PATHSSummary;
            public DBVarChar SortOrder;
            public DBVarChar UpdUserID;
            public DBVarChar MULTI_EXECUTE_TIME;
            public List<DBChar> ExecuteTimeList;
        }

        public class SystemEDIFlowDetail : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, EDI_FLOW_ID,
                EDI_FLOW_ZH_TW, EDI_FLOW_ZH_CN, EDI_FLOW_EN_US, EDI_FLOW_TH_TH, EDI_FLOW_JA_JP, SORT_ORDER,
                SCH_FREQUENCY, SCH_START_DATE, SCH_START_TIME, SCH_INTERVAL_NUM, SCH_INTERVAL_TIME, SCH_END_TIME, SCH_WEEKS, SCH_DAYS_STR, SCH_DATA_DELAY, SCH_KEEP_LOG_DAY,
                PATHS_CMD, PATHS_DAT, PATHS_SRC, PATHS_RES, PATHS_BAD, PATHS_LOG,
                PATHS_FLOW_XML, PATHS_FLOW_CMD, PATHS_ZIP_DAT, PATHS_EXCEPTION, PATHS_SUMMARY, NEW_SORT_ORDER, EXECUTE_TIME
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBNVarChar EDIFlowZHTW;
            public DBNVarChar EDIFlowZHCN;
            public DBNVarChar EDIFlowENUS;
            public DBNVarChar EDIFlowTHTH;
            public DBNVarChar EDIFlowJAJP;
            public DBVarChar SCHFrequency;
            public DBChar SCHStartDate;
            public DBChar SCHStartTime;
            public DBInt SCHIntervalNum;
            public DBInt SCHIntervalTime;
            public DBChar SCHEndTime;
            public DBInt SCHWeeks;
            public DBVarChar SCHDaysStr;
            public DBInt SCHDataDelay;
            public DBInt SCHKeepLogDay;
            public DBNVarChar PATHSCmd;
            public DBNVarChar PATHSDat;
            public DBNVarChar PATHSSrc;
            public DBNVarChar PATHSRes;
            public DBNVarChar PATHSBad;
            public DBNVarChar PATHSLog;
            public DBNVarChar PATHSFlowXml;
            public DBNVarChar PATHSFlowCmd;
            public DBNVarChar PATHSZipDat;
            public DBNVarChar PATHSException;
            public DBNVarChar PATHSSummary;
            public DBVarChar SortOrder;
            public DBVarChar EXECUTE_TIME;
        }

        public SystemEDIFlowDetail SelectSystemEDIFlowDetail(SystemEDIFlowDetailPara para) //明細
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID, EDI_FLOW_ID ", Environment.NewLine, 
                "     , EDI_FLOW_ZH_TW, EDI_FLOW_ZH_CN, EDI_FLOW_EN_US, EDI_FLOW_TH_TH, EDI_FLOW_JA_JP ", Environment.NewLine,
                "     , SCH_FREQUENCY, SCH_START_DATE, SCH_START_TIME, SCH_INTERVAL_NUM, SCH_INTERVAL_TIME, SCH_END_TIME, SCH_WEEKS, SCH_DAYS_STR, SCH_DATA_DELAY, SCH_KEEP_LOG_DAY ", Environment.NewLine,
                "     , PATHS_CMD, PATHS_DAT, PATHS_SRC, PATHS_RES, PATHS_BAD, PATHS_LOG ", Environment.NewLine,
                "     , PATHS_FLOW_XML, PATHS_FLOW_CMD, PATHS_ZIP_DAT, PATHS_EXCEPTION, PATHS_SUMMARY ", Environment.NewLine,
                "     , SORT_ORDER ", Environment.NewLine,
                "     , ( STUFF((", Environment.NewLine,
	            "SELECT ','+ EXECUTE_TIME ", Environment.NewLine,
		        "  FROM SYS_SYSTEM_EDI_FIXEDTIME AS FIX ", Environment.NewLine,
		        " WHERE  FIX.SYS_ID ={SYS_ID}", Environment.NewLine,
		        "   AND FIX.EDI_FLOW_ID = {EDI_FLOW_ID} FOR XML PATH('')),1,1,'')) AS EXECUTE_TIME", Environment.NewLine,
                "  FROM SYS_SYSTEM_EDI_FLOW ", Environment.NewLine,
                " WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID}; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });

            DataTable dataTable = GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                SystemEDIFlowDetail systemEDIFlow = new SystemEDIFlowDetail()
                {
                    SysID = new DBVarChar(dataRow[SystemEDIFlowDetail.DataField.SYS_ID.ToString()]),
                    EDIFlowZHTW = new DBNVarChar(dataRow[SystemEDIFlowDetail.DataField.EDI_FLOW_ZH_TW.ToString()]),
                    EDIFlowZHCN = new DBNVarChar(dataRow[SystemEDIFlowDetail.DataField.EDI_FLOW_ZH_CN.ToString()]),
                    EDIFlowENUS = new DBNVarChar(dataRow[SystemEDIFlowDetail.DataField.EDI_FLOW_EN_US.ToString()]),
                    EDIFlowTHTH = new DBNVarChar(dataRow[SystemEDIFlowDetail.DataField.EDI_FLOW_TH_TH.ToString()]),
                    EDIFlowJAJP = new DBNVarChar(dataRow[SystemEDIFlowDetail.DataField.EDI_FLOW_JA_JP.ToString()]),
                    SCHFrequency = new DBVarChar(dataRow[SystemEDIFlowDetail.DataField.SCH_FREQUENCY.ToString()]),
                    SCHStartDate = new DBChar(dataRow[SystemEDIFlowDetail.DataField.SCH_START_DATE.ToString()]),
                    SCHStartTime = new DBChar(dataRow[SystemEDIFlowDetail.DataField.SCH_START_TIME.ToString()]),
                    SCHIntervalNum = new DBInt(dataRow[SystemEDIFlowDetail.DataField.SCH_INTERVAL_NUM.ToString()]),
                    SCHIntervalTime = new DBInt(dataRow[SystemEDIFlowDetail.DataField.SCH_INTERVAL_TIME.ToString()]),
                    SCHEndTime = new DBChar(dataRow[SystemEDIFlowDetail.DataField.SCH_END_TIME.ToString()]),
                    SCHWeeks = new DBInt(dataRow[SystemEDIFlowDetail.DataField.SCH_WEEKS.ToString()]),
                    SCHDaysStr = new DBVarChar(dataRow[SystemEDIFlowDetail.DataField.SCH_DAYS_STR.ToString()]),
                    SCHDataDelay = new DBInt(dataRow[SystemEDIFlowDetail.DataField.SCH_DATA_DELAY.ToString()]),
                    SCHKeepLogDay = new DBInt(dataRow[SystemEDIFlowDetail.DataField.SCH_KEEP_LOG_DAY.ToString()]),
                    PATHSCmd = new DBNVarChar(dataRow[SystemEDIFlowDetail.DataField.PATHS_CMD.ToString()]),
                    PATHSDat = new DBNVarChar(dataRow[SystemEDIFlowDetail.DataField.PATHS_DAT.ToString()]),
                    PATHSSrc = new DBNVarChar(dataRow[SystemEDIFlowDetail.DataField.PATHS_SRC.ToString()]),
                    PATHSRes = new DBNVarChar(dataRow[SystemEDIFlowDetail.DataField.PATHS_RES.ToString()]),
                    PATHSBad = new DBNVarChar(dataRow[SystemEDIFlowDetail.DataField.PATHS_BAD.ToString()]),
                    PATHSLog = new DBNVarChar(dataRow[SystemEDIFlowDetail.DataField.PATHS_LOG.ToString()]),
                    PATHSFlowXml = new DBNVarChar(dataRow[SystemEDIFlowDetail.DataField.PATHS_FLOW_XML.ToString()]),
                    PATHSFlowCmd = new DBNVarChar(dataRow[SystemEDIFlowDetail.DataField.PATHS_FLOW_CMD.ToString()]),
                    PATHSZipDat = new DBNVarChar(dataRow[SystemEDIFlowDetail.DataField.PATHS_ZIP_DAT.ToString()]),
                    PATHSException = new DBNVarChar(dataRow[SystemEDIFlowDetail.DataField.PATHS_EXCEPTION.ToString()]),
                    PATHSSummary = new DBNVarChar(dataRow[SystemEDIFlowDetail.DataField.PATHS_SUMMARY.ToString()]),
                    SortOrder = new DBVarChar(dataRow[SystemEDIFlowDetail.DataField.SORT_ORDER.ToString()]),
                    EXECUTE_TIME = new DBVarChar(dataRow[SystemEDIFlowDetail.DataField.EXECUTE_TIME.ToString()])
                };
                return systemEDIFlow;
            }
            return null;
        }

        public enum EnumEditSystemEDIFlowDetailResult
        {
            Success, Failure
        }

        public EnumEditSystemEDIFlowDetailResult EditSystemEDIFlowDetail(SystemEDIFlowDetailPara para)
        {
            StringBuilder strbFixedTime = new StringBuilder();

            if (para.ExecuteTimeList != null)
            {
                foreach (var row in para.ExecuteTimeList)
                {
                    strbFixedTime.AppendLine(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                        " INSERT INTO dbo.SYS_SYSTEM_EDI_FIXEDTIME (",
                        "        SYS_ID, EDI_FLOW_ID, EXECUTE_TIME, UPD_USER_ID, UPD_DT",
                        " ) VALUES (",
                        "        {SYS_ID}, {EDI_FLOW_ID}, {EXECUTE_TIME}, {UPD_USER_ID}, GETDATE()",
                        " )"
                        ),
                        new List<DBParameter>
                        {
                            new DBParameter { Name = SystemEDIFlowDetailPara.Field.SYS_ID, Value = para.SysID },
                            new DBParameter { Name = SystemEDIFlowDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID },
                            new DBParameter { Name = SystemEDIFlowDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID },
                            new DBParameter { Name = SystemEDIFlowDetailPara.Field.SCH_START_TIME, Value = para.SCHStartTime },
                            new DBParameter { Name = SystemEDIFlowDetailPara.Field.SCH_END_TIME, Value = para.SCHEndTime },
                            new DBParameter { Name = SystemEDIFlowDetailPara.Field.EXECUTE_TIME, Value = row },
                        }));
                }
            }

            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1); ",
                "SET @RESULT = 'N'; ",
                "BEGIN TRANSACTION ",
                "    BEGIN TRY ",

                "        DELETE FROM SYS_SYSTEM_EDI_FLOW ",
                "        WHERE SYS_ID = {SYS_ID} AND EDI_FLOW_ID = {EDI_FLOW_ID}; ",

                "        INSERT INTO dbo.SYS_SYSTEM_EDI_FLOW (",
                "               SYS_ID",
                "             , EDI_FLOW_ID",
                "             , EDI_FLOW_ZH_TW",
                "             , EDI_FLOW_ZH_CN",
                "             , EDI_FLOW_EN_US",
                "             , EDI_FLOW_TH_TH",
                "             , EDI_FLOW_JA_JP",
                "             , SCH_FREQUENCY",
                "             , SCH_START_DATE",
                "             , SCH_START_TIME",
                "             , SCH_INTERVAL_NUM",
                "             , SCH_INTERVAL_TIME",
                "             , SCH_END_TIME",
                "             , SCH_WEEKS",
                "             , SCH_DAYS_STR",
                "             , SCH_DATA_DELAY",
                "             , SCH_KEEP_LOG_DAY",
                "             , PATHS_CMD",
                "             , PATHS_DAT",
                "             , PATHS_SRC",
                "             , PATHS_RES",
                "             , PATHS_BAD",
                "             , PATHS_LOG",
                "             , PATHS_FLOW_XML",
                "             , PATHS_FLOW_CMD",
                "             , PATHS_ZIP_DAT",
                "             , PATHS_EXCEPTION",
                "             , PATHS_SUMMARY",
                "             , SORT_ORDER",
                "             , UPD_USER_ID",
                "             , UPD_DT",
                "        ) VALUES (",
                "               {SYS_ID}",
                "             , {EDI_FLOW_ID}",
                "             , {EDI_FLOW_ZH_TW}",
                "             , {EDI_FLOW_ZH_CN}",
                "             , {EDI_FLOW_EN_US}",
                "             , {EDI_FLOW_TH_TH}",
                "             , {EDI_FLOW_JA_JP}",
                "             , {SCH_FREQUENCY}",
                "             , {SCH_START_DATE}",
                "             , {SCH_START_TIME}",
                "             , {SCH_INTERVAL_NUM}",
                "             , {SCH_INTERVAL_TIME}",
                "             , {SCH_END_TIME}",
                "             , {SCH_WEEKS}",
                "             , {SCH_DAYS_STR}",
                "             , {SCH_DATA_DELAY}",
                "             , {SCH_KEEP_LOG_DAY}",
                "             , {PATHS_CMD}",
                "             , {PATHS_DAT}",
                "             , {PATHS_SRC}",
                "             , {PATHS_RES}",
                "             , {PATHS_BAD}",
                "             , {PATHS_LOG}",
                "             , {PATHS_FLOW_XML}",
                "             , {PATHS_FLOW_CMD}",
                "             , {PATHS_ZIP_DAT}",
                "             , {PATHS_EXCEPTION}",
                "             , {PATHS_SUMMARY}",
                "             , {SORT_ORDER}",
                "             , {UPD_USER_ID}",
                "             , GETDATE()",
                "        )",

                "        DELETE FROM SYS_SYSTEM_EDI_FIXEDTIME ",
                "        WHERE SYS_ID = {SYS_ID} AND EDI_FLOW_ID = {EDI_FLOW_ID}; ",

                strbFixedTime.ToString(),

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
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.EDI_FLOW_ZH_TW, Value = para.EDIFlowZHTW });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.EDI_FLOW_ZH_CN, Value = para.EDIFlowZHCN });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.EDI_FLOW_EN_US, Value = para.EDIFlowENUS });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.EDI_FLOW_TH_TH, Value = para.EDIFlowTHTH });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.EDI_FLOW_JA_JP, Value = para.EDIFlowJAJP });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.SCH_FREQUENCY, Value = para.SCHFrequency });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.SCH_START_DATE, Value = para.SCHStartDate });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.SCH_START_TIME, Value = para.SCHStartTime });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.SCH_INTERVAL_NUM, Value = para.SCHIntervalNum });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.SCH_INTERVAL_TIME, Value = para.SCHIntervalTime });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.SCH_END_TIME, Value = para.SCHEndTime });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.SCH_WEEKS, Value = para.SCHWeeks });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.SCH_DAYS_STR, Value = para.SCHDaysStr });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.SCH_DATA_DELAY, Value = para.SCHDataDelay });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.SCH_KEEP_LOG_DAY, Value = para.SCHKeepLogDay.IsNull() ? null : para.SCHKeepLogDay });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.PATHS_CMD, Value = para.PATHSCmd });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.PATHS_DAT, Value = para.PATHSDat });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.PATHS_SRC, Value = para.PATHSSrc });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.PATHS_RES, Value = para.PATHSRes });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.PATHS_BAD, Value = para.PATHSBad });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.PATHS_LOG, Value = para.PATHSLog });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.PATHS_FLOW_XML, Value = para.PATHSFlowXml });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.PATHS_FLOW_CMD, Value = para.PATHSFlowCmd });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.PATHS_ZIP_DAT, Value = para.PATHSZipDat });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.PATHS_EXCEPTION, Value = para.PATHSException });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.PATHS_SUMMARY, Value = para.PATHSSummary });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditSystemEDIFlowDetailResult.Success : EnumEditSystemEDIFlowDetailResult.Failure;
        }

        public enum EnumDeleteSystemEDIFlowDetailResult
        {
            Success, Failure, DataExist
        }

        public EnumDeleteSystemEDIFlowDetailResult DeleteSystemEDIFlowDetail(SystemEDIFlowDetailPara para)
        {
            try
            {
                string commandText = string.Concat(new object[] 
                {
                    "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                    "SET @RESULT = 'N'; ", Environment.NewLine,
                    "BEGIN TRANSACTION ", Environment.NewLine,
                    "    BEGIN TRY ", Environment.NewLine,

                    "        IF NOT EXISTS (SELECT * FROM SYS_SYSTEM_EDI_JOB WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID}) ", Environment.NewLine,
                    "        BEGIN ", Environment.NewLine,
                    "            DELETE FROM SYS_SYSTEM_EDI_CON WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID}; ", Environment.NewLine,
                    "            DELETE FROM SYS_SYSTEM_EDI_FIXEDTIME WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID}; ", Environment.NewLine,
                    "            DELETE FROM SYS_SYSTEM_EDI_FLOW WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID}; ", Environment.NewLine,
                    "            SET @RESULT = 'Y'; ", Environment.NewLine,
                    "        END; ", Environment.NewLine,

                    "        COMMIT; ", Environment.NewLine,
                    "    END TRY ", Environment.NewLine,
                    "    BEGIN CATCH ", Environment.NewLine,
                    "        SET @RESULT = 'E'; ", Environment.NewLine,
                    "        ROLLBACK TRANSACTION; ", Environment.NewLine,
                    "    END CATCH ", Environment.NewLine,
                    "; ", Environment.NewLine,
                    "SELECT @RESULT; "
                });

                List<DBParameter> dbParameters = new List<DBParameter>();
                dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.SYS_ID, Value = para.SysID });
                dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });

                DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
                if (result.GetValue() == EnumYN.Y.ToString())
                {
                    return EnumDeleteSystemEDIFlowDetailResult.Success;
                }
                else if (result.GetValue() == EnumYN.N.ToString())
                {
                    return EnumDeleteSystemEDIFlowDetailResult.DataExist;
                }
                else
                {
                    return EnumDeleteSystemEDIFlowDetailResult.Failure;
                }
            }
            catch
            {
                return EnumDeleteSystemEDIFlowDetailResult.Failure;
            }
        }

        public string GetFlowNewSortOrder(SystemEDIFlowDetailPara para)
        {
            string newSortOrder = Common.GetEnumDesc(SortorderField.First);
            string commandText = string.Concat(new object[]
            {
                "SELECT TOP 1 SORT_ORDER AS NEW_SORT_ORDER ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_FLOW ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SORT_ORDER DESC; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowDetailPara.Field.SYS_ID, Value = para.SysID });

            DataTable dataTable = GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                newSortOrder = dataRow[SystemEDIFlowDetail.DataField.NEW_SORT_ORDER.ToString()].ToString().Replace("0", null);
                if (!string.IsNullOrWhiteSpace(newSortOrder))
                {
                    newSortOrder = Convert.ToString(Convert.ToInt32(newSortOrder) + 1);
                    newSortOrder = Common.GetEnumDesc(SortorderField.Left) + newSortOrder + Common.GetEnumDesc(SortorderField.Right);
                    newSortOrder = newSortOrder.Substring(newSortOrder.Length - 6, 6);
                }
            }
            return newSortOrder;
        }
    }
}