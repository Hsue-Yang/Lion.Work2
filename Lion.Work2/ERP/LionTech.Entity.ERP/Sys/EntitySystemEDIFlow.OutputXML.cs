using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemEDIFlowOutputXML : EntitySys
    {
        public EntitySystemEDIFlowOutputXML(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        
        }

        public enum EnumEDIXMLPathFile
        {
            [Description(@"FileData")]
            FileData,
            [Description(@"LionTech.EDIService.")]
            EDIService,
            [Description(@".exe.xml")]
            xml
        }

        public class SystemEDIXMLPara : DBCulture
        {
            public SystemEDIXMLPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, SYS_NM, EDI_FLOW, EDI_JOB, EDI_FLOW_ID
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
        }

        //取得con列表
        public List<Entity_BaseAP.SystemEDIConnectionDetail> SysEDIConnectionList(SystemEDIXMLPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID,EDI_FLOW_ID,EDI_CON_ID,PROVIDER_NAME,CON_VALUE,SORT_ORDER ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_CON ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SYS_ID, SORT_ORDER ,EDI_FLOW_ID "
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            //帶入參數
            dbParameters.Add(new DBParameter { Name = SystemEDIXMLPara.ParaField.SYS_ID.ToString(), Value = para.SysID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<Entity_BaseAP.SystemEDIConnectionDetail> systemEDIConDetailList = new List<Entity_BaseAP.SystemEDIConnectionDetail>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    Entity_BaseAP.SystemEDIConnectionDetail systemEDIConDetail = new Entity_BaseAP.SystemEDIConnectionDetail()
                    {
                        SysID = new DBVarChar(dataRow[Entity_BaseAP.SystemEDIConnectionDetail.DataField.SYS_ID.ToString()]),
                        EDIFlowID = new DBVarChar(dataRow[Entity_BaseAP.SystemEDIConnectionDetail.DataField.EDI_FLOW_ID.ToString()]),
                        EDIConID = new DBVarChar(dataRow[Entity_BaseAP.SystemEDIConnectionDetail.DataField.EDI_CON_ID.ToString()]),
                        ProviderName = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIConnectionDetail.DataField.PROVIDER_NAME.ToString()]),
                        ConValue = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIConnectionDetail.DataField.CON_VALUE.ToString()]),
                        SortOrder = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIConnectionDetail.DataField.SORT_ORDER.ToString()]),
                    };
                    systemEDIConDetailList.Add(systemEDIConDetail);
                }
                return systemEDIConDetailList;
            }
            return null;
        }

        //取得job列表
        public List<Entity_BaseAP.SystemEDIJobDetail> SysEDIJobList(SystemEDIXMLPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID,EDI_FLOW_ID,EDI_JOB_ID,EDI_JOB_TYPE,EDI_CON_ID", Environment.NewLine,
                "     , OBJECT_NAME,DEP_EDI_JOB_ID,{EDI_JOB} As EDI_JOB_NM", Environment.NewLine,
                "     , IS_USE_RES,FILE_SOURCE,FILE_ENCODING, URL_PATH, IGNORE_WARNING, IS_DISABLE,SORT_ORDER ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_JOB ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SYS_ID, SORT_ORDER, EDI_FLOW_ID "
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIXMLPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIXMLPara.ParaField.EDI_JOB.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIXMLPara.ParaField.EDI_JOB.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<Entity_BaseAP.SystemEDIJobDetail> systemEDIJobDetailList = new List<Entity_BaseAP.SystemEDIJobDetail>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    Entity_BaseAP.SystemEDIJobDetail systemEDIJobDetail = new Entity_BaseAP.SystemEDIJobDetail()
                    {
                        SysID = new DBVarChar(dataRow[Entity_BaseAP.SystemEDIJobDetail.DataField.SYS_ID.ToString()]),
                        EDIFlowID = new DBVarChar(dataRow[Entity_BaseAP.SystemEDIJobDetail.DataField.EDI_FLOW_ID.ToString()]),
                        EDIJobID = new DBVarChar(dataRow[Entity_BaseAP.SystemEDIJobDetail.DataField.EDI_JOB_ID.ToString()]),
                        EDIJobNM = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIJobDetail.DataField.EDI_JOB_NM.ToString()]),
                        EDIJobType = new DBVarChar(dataRow[Entity_BaseAP.SystemEDIJobDetail.DataField.EDI_JOB_TYPE.ToString()]),
                        EDIConID = new DBVarChar(dataRow[Entity_BaseAP.SystemEDIJobDetail.DataField.EDI_CON_ID.ToString()]),
                        ObjectName = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIJobDetail.DataField.OBJECT_NAME.ToString()]),
                        DepEDIJobID = new DBVarChar(dataRow[Entity_BaseAP.SystemEDIJobDetail.DataField.DEP_EDI_JOB_ID.ToString()]),
                        FileSource = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIJobDetail.DataField.FILE_SOURCE.ToString()]),
                        FileEncoding = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIJobDetail.DataField.FILE_ENCODING.ToString()]),
                        URLPath = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIJobDetail.DataField.URL_PATH.ToString()]),
                        IsUseRes = new DBChar(dataRow[Entity_BaseAP.SystemEDIJobDetail.DataField.IS_USE_RES.ToString()]),
						IgnoreWarning = new DBChar(dataRow[Entity_BaseAP.SystemEDIJobDetail.DataField.IGNORE_WARNING.ToString()]),
                        IsDisable = new DBChar(dataRow[Entity_BaseAP.SystemEDIJobDetail.DataField.IS_DISABLE.ToString()]),
                        SortOrder = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIJobDetail.DataField.SORT_ORDER.ToString()])
                    };
                    systemEDIJobDetailList.Add(systemEDIJobDetail);
                }
                return systemEDIJobDetailList;
            }
            return null;
        }

        //取得flow列表
        public List<Entity_BaseAP.SystemEDIFlowDetail> SysEDIFlowList(SystemEDIXMLPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID,EDI_FLOW_ID,SCH_FREQUENCY,dbo.FN_GET_SYSDATE_SLASH(SCH_START_DATE) AS SCH_START_DATE ", Environment.NewLine,
                "     , {EDI_FLOW} AS EDI_FLOW_NM,SCH_START_TIME,SCH_INTERVAL_NUM,SCH_KEEP_LOG_DAY ", Environment.NewLine,
                "     , SCH_WEEKS,SCH_DAYS_STR,SCH_DATA_DELAY,PATHS_CMD,PATHS_DAT,PATHS_SRC,PATHS_RES ", Environment.NewLine,
                "     , PATHS_BAD,PATHS_LOG,PATHS_FLOW_XML,PATHS_FLOW_CMD,PATHS_ZIP_DAT,PATHS_EXCEPTION ", Environment.NewLine,
                "     , PATHS_SUMMARY,SORT_ORDER ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_FLOW ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SYS_ID,SORT_ORDER "
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIXMLPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIXMLPara.ParaField.EDI_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIXMLPara.ParaField.EDI_FLOW.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<Entity_BaseAP.SystemEDIFlowDetail> systemEDIFlowDetailList = new List<Entity_BaseAP.SystemEDIFlowDetail>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string startTimeString = dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.SCH_START_TIME.ToString()].ToString();
                    startTimeString = startTimeString.Substring(0, 2) + ":" + startTimeString.Substring(2, 2) + ":" + startTimeString.Substring(4, 2) + "." + startTimeString.Substring(6);

                    Entity_BaseAP.SystemEDIFlowDetail systemEDIFlowDetail = new Entity_BaseAP.SystemEDIFlowDetail()
                    {
                        SysID = new DBVarChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.SYS_ID.ToString()]),
                        EDIFlowID = new DBVarChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.EDI_FLOW_ID.ToString()]),
                        EDIFlowNM = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.EDI_FLOW_NM.ToString()]),
                        SCHFrequency = new DBVarChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.SCH_FREQUENCY.ToString()]),
                        SCHStartDate = new DBChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.SCH_START_DATE.ToString()]),
                        SCHStartTime = new DBChar(startTimeString),
                        SCHIntervalNum = new DBInt(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.SCH_INTERVAL_NUM.ToString()]),
                        SCHWeeks = new DBInt(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.SCH_WEEKS.ToString()]),
                        SCHDaysStr = new DBVarChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.SCH_DAYS_STR.ToString()]),
                        SCHDataDelay = new DBInt(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.SCH_DATA_DELAY.ToString()]),
                        SCHKeepLogDay = new DBInt(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.SCH_KEEP_LOG_DAY.ToString()]),
                        PATHSCmd = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.PATHS_CMD.ToString()]),
                        PATHSDat = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.PATHS_DAT.ToString()]),
                        PATHSSrc = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.PATHS_SRC.ToString()]),
                        PATHSRes = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.PATHS_RES.ToString()]),
                        PATHSBad = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.PATHS_BAD.ToString()]),
                        PATHSLog = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.PATHS_LOG.ToString()]),
                        PATHSFlowXml = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.PATHS_FLOW_XML.ToString()]),
                        PATHSFlowCmd = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.PATHS_FLOW_CMD.ToString()]),
                        PATHSZipDat = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.PATHS_ZIP_DAT.ToString()]),
                        PATHSException = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.PATHS_EXCEPTION.ToString()]),
                        PATHSSummary = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIFlowDetail.DataField.PATHS_SUMMARY.ToString()])
                    };
                    systemEDIFlowDetailList.Add(systemEDIFlowDetail);
                }
                return systemEDIFlowDetailList;
            }
            return null;
        }

        //取得Para列表
        public List<Entity_BaseAP.SystemEDIParaDetail> SysEDIParaList(SystemEDIXMLPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID,EDI_FLOW_ID,EDI_JOB_ID ", Environment.NewLine,
                "     , EDI_JOB_PARA_ID,EDI_JOB_PARA_TYPE,EDI_JOB_PARA_VALUE ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_PARA ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SYS_ID,SORT_ORDER,EDI_FLOW_ID,EDI_JOB_ID "
            });
            List<DBParameter> DbParameters = new List<DBParameter>();
            //帶入參數
            DbParameters.Add(new DBParameter { Name = SystemEDIXMLPara.ParaField.SYS_ID.ToString(), Value = para.SysID });

            DataTable dataTable = base.GetDataTable(commandText, DbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<Entity_BaseAP.SystemEDIParaDetail> systemEDIParaDetailList = new List<Entity_BaseAP.SystemEDIParaDetail>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    Entity_BaseAP.SystemEDIParaDetail systemEDIParaDetail = new Entity_BaseAP.SystemEDIParaDetail()
                    {
                        SysID = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIParaDetail.DataField.SYS_ID.ToString()]),
                        EDIFlowID = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIParaDetail.DataField.EDI_FLOW_ID.ToString()]),
                        EDIJobID = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIParaDetail.DataField.EDI_JOB_ID.ToString()]),
                        EDIJobParaID = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIParaDetail.DataField.EDI_JOB_PARA_ID.ToString()]),
                        EDIJobParaType = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIParaDetail.DataField.EDI_JOB_PARA_TYPE.ToString()]),
                        EDIJobParaValue = new DBNVarChar(dataRow[Entity_BaseAP.SystemEDIParaDetail.DataField.EDI_JOB_PARA_VALUE.ToString()]),
                    };
                    systemEDIParaDetailList.Add(systemEDIParaDetail);
                }
                return systemEDIParaDetailList;
            }
            return null;
        }

        //取得FixedTime列表
        public List<Entity_BaseAP.SystemEDIFlowExecuteTimeDetail> SysEDIFlowFixedTimeList(SystemEDIXMLPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID,EDI_FLOW_ID,EXECUTE_TIME", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_FIXEDTIME ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID}", Environment.NewLine,
                "ORDER BY SYS_ID,EDI_FLOW_ID"
            });
            List<DBParameter> DbParameters = new List<DBParameter>();
            //帶入參數
            DbParameters.Add(new DBParameter { Name = SystemEDIXMLPara.ParaField.SYS_ID.ToString(), Value = para.SysID });

            DataTable dataTable = base.GetDataTable(commandText, DbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<Entity_BaseAP.SystemEDIFlowExecuteTimeDetail> systemEDIFlowFixedTimeList = new List<Entity_BaseAP.SystemEDIFlowExecuteTimeDetail>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string executeTimeString = dataRow[Entity_BaseAP.SystemEDIFlowExecuteTimeDetail.DataField.EXECUTE_TIME.ToString()].ToString();
                    executeTimeString = executeTimeString.Substring(0, 2) + ":" + executeTimeString.Substring(2, 2) + ":" + executeTimeString.Substring(4, 2) + "." + executeTimeString.Substring(6);

                    Entity_BaseAP.SystemEDIFlowExecuteTimeDetail systemEDIFlowExecuteTimeDetail = new Entity_BaseAP.SystemEDIFlowExecuteTimeDetail()
                    {
                        SysID = new DBVarChar(dataRow[Entity_BaseAP.SystemEDIParaDetail.DataField.SYS_ID.ToString()]),
                        EDIFlowID = new DBVarChar(dataRow[Entity_BaseAP.SystemEDIParaDetail.DataField.EDI_FLOW_ID.ToString()]),
                        ExecuteTime = new DBChar(executeTimeString),
                    };
                    systemEDIFlowFixedTimeList.Add(systemEDIFlowExecuteTimeDetail);
                }
                return systemEDIFlowFixedTimeList;
            }
            return null;
        }
    }
}