using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using LionTech.EDI;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemEDIJobConvey : EntitySys
    {
        public EntitySystemEDIJobConvey(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemEDIFlowPara : DBCulture
        {
            public SystemEDIFlowPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, EDI_FLOW_ID, UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBVarChar UpdUserID;
        }

        public class SystemEDICon : DBTableRow
        {
            public enum DataField
            {
                EDI_CON_ID, PROVIDER_NAME, CON_VALUE
            }

            public DBVarChar EDIConID;
            public DBVarChar ProviderName;
            public DBVarChar EDIConValue;
        }

        public class SystemEDIJobCount : DBTableRow
        {
            public enum DataField
            {
                MAX_SORT_ORDER
            }
            
            public DBInt JobMaxSortOrder;
        }

        public Flow SelectSystemEDIConData(Flow flow, SystemEDIFlowPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT EDI_CON_ID, PROVIDER_NAME, CON_VALUE ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_CON ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID}; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowPara.ParaField.EDI_FLOW_ID.ToString(), Value = para.EDIFlowID });
            DataTable dataTable = GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    Connection connection = new Connection()
                    {
                        id = dataRow[SystemEDICon.DataField.EDI_CON_ID.ToString()].ToString(),
                        providerName = dataRow[SystemEDICon.DataField.PROVIDER_NAME.ToString()].ToString(),
                        value = dataRow[SystemEDICon.DataField.CON_VALUE.ToString()].ToString()
                    };
                    flow.connections.Add(connection);
                }
                return flow;
            }
            return flow;
        }

        public DBInt SelectJobMaxSortOrder(SystemEDIFlowPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT MAX(SORT_ORDER) AS MAX_SORT_ORDER ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_JOB ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID}; ", Environment.NewLine,
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowPara.ParaField.EDI_FLOW_ID.ToString(), Value = para.EDIFlowID });
            return new DBInt(ExecuteScalar(commandText, dbParameters));
        }

        public class SystemEDIJobDetailPara
        {
            public enum Field
            {
                SYS_ID, EDI_FLOW_ID, EDI_JOB_ID,
                EDI_JOB_ZH_TW, EDI_JOB_ZH_CN, EDI_JOB_EN_US,EDI_JOB_TH_TH,EDI_JOB_JA_JP,
                EDI_JOB_TYPE, EDI_CON_ID, OBJECT_NAME, DEP_EDI_JOB_ID,
                IS_USE_RES, IGNORE_WARNING, IS_DISABLE, FILE_SOURCE, FILE_ENCODING, URL_PATH,
                SORT_ORDER, UPD_USER_ID
            }
        }

        public class SystemEDIParaDetailPara
        {
            public enum Field
            {
                SYS_ID, EDI_FLOW_ID, EDI_JOB_ID,
                EDI_JOB_PARA_ID, EDI_JOB_PARA_TYPE, EDI_JOB_PARA_VALUE, SORT_ORDER,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBVarChar EDIJobID;
            public DBVarChar EDIParaID;
            public DBVarChar EDIParaType;
            public DBNVarChar EDIParaValue;
            public DBVarChar SortOrder;
            public DBVarChar UpdUserID;
        }

        public enum EnumEDIJobSettingResult
        {
            Success, Failure
        }

        public EnumEDIJobSettingResult EdiJobSetting(SystemEDIFlowPara para, Flow flow, int jobSortOrder)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();
            foreach (Job job in flow.jobs)
            {
                int paraCount = 0;
                string commandJobsText =
                    string.Join(Environment.NewLine,
                        new object[]
                        {
                            "DELETE FROM SYS_SYSTEM_EDI_PARA ",
                            "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} AND EDI_JOB_ID={EDI_JOB_ID}; ",

                            "DELETE FROM SYS_SYSTEM_EDI_JOB  ",
                            "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} AND EDI_JOB_ID={EDI_JOB_ID}; ",

                            "INSERT INTO SYS_SYSTEM_EDI_JOB VALUES( ",
                            "     {SYS_ID} ",
                            "   , {EDI_FLOW_ID} ",
                            "   , {EDI_JOB_ID} ",
                            "   , {EDI_JOB_ZH_TW}, {EDI_JOB_ZH_CN}, {EDI_JOB_EN_US}, {EDI_JOB_TH_TH}, {EDI_JOB_JA_JP} ",
                            "   , {EDI_JOB_TYPE}, {EDI_CON_ID}, {OBJECT_NAME}, {DEP_EDI_JOB_ID} ",
                            "   , {IS_USE_RES}, {FILE_SOURCE}, {FILE_ENCODING}, {URL_PATH}, {IGNORE_WARNING}, {IS_DISABLE} ",
                            "   , {SORT_ORDER} ",
                            "   , {UPD_USER_ID}, GETDATE() ",
                            ");"
                        });
                
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.SYS_ID, Value = para.SysID });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_JOB_ID, Value = new DBVarChar(job.id) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_JOB_ZH_TW, Value = new DBNVarChar(job.description) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_JOB_ZH_CN, Value = new DBNVarChar(job.description) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_JOB_EN_US, Value = new DBNVarChar(job.description) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_JOB_TH_TH, Value = new DBNVarChar(job.description) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_JOB_JA_JP, Value = new DBNVarChar(job.description) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_JOB_TYPE, Value = new DBVarChar(job.type) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.EDI_CON_ID, Value = new DBVarChar(job.connectionID) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.OBJECT_NAME, Value = new DBVarChar(job.objectName) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.DEP_EDI_JOB_ID, Value = new DBVarChar(job.dependOnJobID) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.IS_USE_RES, Value = new DBChar(job.useRES) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.FILE_SOURCE, Value = new DBNVarChar(job.fileSource) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.FILE_ENCODING, Value = new DBVarChar(job.fileEncoding) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.URL_PATH, Value = new DBVarChar(job.urlPath) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.IGNORE_WARNING, Value = new DBChar(job.ignoreWarning) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.IS_DISABLE, Value = new DBChar(job.isDisable) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.SORT_ORDER, Value = new DBVarChar((jobSortOrder).ToString().PadLeft(6, '0')) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID });

                commandTextStringBuilder.Append(GetCommandText(ProviderName, commandJobsText, dbParameters));

                foreach (Parameter parameter in job.parameters)
                {
                    dbParameters.Clear();
                    string commandParametersText =
                        string.Join(Environment.NewLine,
                            new object[]
                            {
                                "INSERT INTO SYS_SYSTEM_EDI_PARA VALUES( ",
                                "     {SYS_ID} ",
                                "   , {EDI_FLOW_ID} ",
                                "   , {EDI_JOB_ID} ",
                                "   , {EDI_JOB_PARA_ID} ",
                                "   , {EDI_JOB_PARA_TYPE} ",
                                "   , {EDI_JOB_PARA_VALUE} ",
                                "   , {SORT_ORDER} ",
                                "   , {UPD_USER_ID}, GETDATE() ",
                                ");"
                            });

                    dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.SYS_ID, Value = para.SysID });
                    dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });
                    dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_JOB_ID, Value = new DBVarChar(job.id) });
                    dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_JOB_PARA_ID, Value = new DBVarChar(parameter.id) });
                    dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_JOB_PARA_TYPE, Value = new DBVarChar(parameter.type) });
                    dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_JOB_PARA_VALUE, Value = new DBNVarChar(parameter.value) });
                    dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.SORT_ORDER, Value = new DBVarChar((paraCount*100).ToString().PadLeft(6,'0')) });
                    dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID });

                    commandTextStringBuilder.Append(GetCommandText(ProviderName, commandParametersText, dbParameters));
                    paraCount++;
                }
                jobSortOrder = jobSortOrder +100;
                dbParameters.Clear();
            }

            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT CHAR(1) = 'N';",
                        "DECLARE @ERROR_LINE INT;",
                        "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",

                        "BEGIN TRANSACTION ",
                        "    BEGIN TRY ",
                        commandTextStringBuilder.ToString(),
                        "        SET @RESULT = 'Y'; ",
                        "        COMMIT; ",
                        "    END TRY ",
                        "    BEGIN CATCH ",
                        "        SET @RESULT = 'N';",
                        "        SET @ERROR_LINE = ERROR_LINE();",
                        "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                        "        ROLLBACK TRANSACTION; ",
                        "    END CATCH ",
                        "; ",
                        "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;",
                        Environment.NewLine
                    });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEDIJobSettingResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
    }
}