using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using LionTech.EDI;

namespace LionTech.Entity.B2P.Sys
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
                "SELECT EDI_CON_ID, PROVIDER_NAME, CON_VALUE", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_CON ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID}; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowPara.ParaField.EDI_FLOW_ID.ToString(), Value = para.EDIFlowID });
            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemEDICon> SystemEDIConList = new List<SystemEDICon>();
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

        public int GetJobMaxSortOrder(SystemEDIFlowPara para)
        {
            int jobMaxSortOrder = 0;
            string commandText = string.Concat(new object[]
            {
                "SELECT MAX(SORT_ORDER) AS MAX_SORT_ORDER ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_JOB ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID}; ", Environment.NewLine,
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowPara.ParaField.EDI_FLOW_ID.ToString(), Value = para.EDIFlowID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {   
                DataRow dataRow = dataTable.Rows[0];
                jobMaxSortOrder = Convert.ToInt32(dataRow[SystemEDIJobCount.DataField.MAX_SORT_ORDER.ToString()]);
            }

            return jobMaxSortOrder;
        }

        public class SystemEDIJobDetailPara
        {
            public enum Field
            {
                SYS_ID, EDI_FLOW_ID, EDI_JOB_ID,
                EDI_JOB_ZH_TW, EDI_JOB_ZH_CN, EDI_JOB_EN_US,EDI_JOB_TH_TH,EDI_JOB_JA_JP,
                EDI_JOB_TYPE, EDI_CON_ID, OBJECT_NAME, DEP_EDI_JOB_ID,
                IS_USE_RES, IS_DISABLE, FILE_SOURCE, FILE_ENCODING,
                SORT_ORDER, UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBVarChar EDIJobID;
            public DBNVarChar EDIJobZHTW;
            public DBNVarChar EDIJobZHCN;
            public DBNVarChar EDIJobENUS;
            public DBNVarChar EDIJobTHTH;
            public DBNVarChar EDIJobJAJP;
            public DBVarChar EDIJobType;
            public DBVarChar EDIConID;
            public DBVarChar ObjectName;
            public DBVarChar DepEDIJobID;
            public DBChar IsUseRes;
            public DBChar IsDisable;
            public DBNVarChar FileSource;
            public DBVarChar EDIFileEncoding;
            public DBVarChar SortOrder;
            public DBVarChar UpdUserID;
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
                string commandJobsText = string.Concat(new object[]
                {
                    "DELETE FROM SYS_SYSTEM_EDI_PARA", Environment.NewLine,
                    "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} AND EDI_JOB_ID={EDI_JOB_ID};", Environment.NewLine,
                    
                    "DELETE FROM SYS_SYSTEM_EDI_JOB  ", Environment.NewLine,
                    "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} AND EDI_JOB_ID={EDI_JOB_ID};", Environment.NewLine,
                    
                    "INSERT INTO SYS_SYSTEM_EDI_JOB VALUES(", Environment.NewLine,
                    "     {SYS_ID}", Environment.NewLine,
                    "   , {EDI_FLOW_ID}", Environment.NewLine,
                    "   , {EDI_JOB_ID}", Environment.NewLine,
                    "   , {EDI_JOB_ZH_TW}, {EDI_JOB_ZH_CN}, {EDI_JOB_EN_US}, {EDI_JOB_TH_TH}, {EDI_JOB_JA_JP}", Environment.NewLine,
                    "   , {EDI_JOB_TYPE}, {EDI_CON_ID}, {OBJECT_NAME}, {DEP_EDI_JOB_ID}", Environment.NewLine,
                    "   , {IS_USE_RES}, {FILE_SOURCE}, {FILE_ENCODING},{IS_DISABLE}", Environment.NewLine,
                    "   , {SORT_ORDER}", Environment.NewLine,
                    "   , {UPD_USER_ID}, GETDATE()", Environment.NewLine,
                    ");", Environment.NewLine,
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
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.IS_DISABLE, Value = new DBChar(job.isDisable) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.SORT_ORDER, Value = new DBVarChar((jobSortOrder).ToString().PadLeft(6, '0')) });
                dbParameters.Add(new DBParameter { Name = SystemEDIJobDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, commandJobsText, dbParameters));

                foreach (Parameter parameter in job.parameters)
                {
                    dbParameters.Clear();
                    string commandParametersText = string.Concat(new object[]
                    {
                        "INSERT INTO SYS_SYSTEM_EDI_PARA VALUES(", Environment.NewLine,
                        "     {SYS_ID}", Environment.NewLine,
                        "   , {EDI_FLOW_ID}", Environment.NewLine,
                        "   , {EDI_JOB_ID}", Environment.NewLine,
                        "   , {EDI_JOB_PARA_ID}", Environment.NewLine,
                        "   , {EDI_JOB_PARA_TYPE}", Environment.NewLine,
                        "   , {EDI_JOB_PARA_VALUE}", Environment.NewLine,
                        "   , {SORT_ORDER}", Environment.NewLine,
                        "   , {UPD_USER_ID}, GETDATE()", Environment.NewLine,
                        ");", Environment.NewLine,
                    });

                    dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.SYS_ID, Value = para.SysID });
                    dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });
                    dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_JOB_ID, Value = new DBVarChar(job.id) });
                    dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_JOB_PARA_ID, Value = new DBVarChar(parameter.id) });
                    dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_JOB_PARA_TYPE, Value = new DBVarChar(parameter.type) });
                    dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.EDI_JOB_PARA_VALUE, Value = new DBNVarChar(parameter.value) });
                    dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.SORT_ORDER, Value = new DBVarChar((paraCount*100).ToString().PadLeft(6,'0')) });
                    dbParameters.Add(new DBParameter { Name = SystemEDIParaDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID });

                    commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, commandParametersText, dbParameters));
                    paraCount++;
                }
                jobSortOrder = jobSortOrder +100;
                dbParameters.Clear();
            }

            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,
                commandTextStringBuilder.ToString(), Environment.NewLine,
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

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEDIJobSettingResult.Success : EnumEDIJobSettingResult.Failure; 
        }
    }
}