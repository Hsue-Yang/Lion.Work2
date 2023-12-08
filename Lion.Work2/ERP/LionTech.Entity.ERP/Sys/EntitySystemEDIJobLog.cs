using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemEDIJobLog : EntitySys
    {
        public EntitySystemEDIJobLog(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemEDIJobLogPara : DBCulture
        {
            public SystemEDIJobLogPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, SYS_NM, EDI_FLOW_ID, EDI_FLOW, EDI_JOB_ID, EDI_JOB, EDI_NO, EDI_DATE, CULTURE_ID,
                EDI_FLOW_ID_SEARCH, EDI_JOB_ID_SEARCH
            }

            public enum ParaNull
            {
                NULL
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBVarChar EDIJobID;
            public DBChar EDINO;
            public DBChar EDIDate;
            public DBObject EDIFlowIDSearch;
            public DBObject EDIJobIDSearch;
        }

        public class SystemEDIJobLog : DBTableRow
        {
            public enum DataField
            {
                EDI_NO, SYS_ID, SYS_NM, EDI_FLOW_ID, EDI_FLOW_NM, EDI_JOB_ID, EDI_JOB_NM,
                STATUS_ID, STATUS_NM, RESULT_ID, RESULT_NM,
                RESULT_CODE,
                DT_BEGIN, DT_END, COUNT_ROW,
                UPD_USER_NM, UPD_DT
            }

            public DBVarChar EDINO;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar EDIFlowID;
            public DBNVarChar EDIFlowNM;
            public DBVarChar EDIJobID;
            public DBNVarChar EDIJobNM;

            public DBVarChar StatusID;
            public DBNVarChar EDIJobStatusNM;
            public DBVarChar ResultID;
            public DBNVarChar EDIJobResultNM;
            public DBVarChar ResultCode;
            
            public DBDateTime DTBegin;
            public DBDateTime DTEnd;
            public DBInt CountRow;

            public DBVarChar UpdUserID;
            public DBDateTime UpdDt;
        }

        public List<SystemEDIJobLog> SelectSystemEDIJobLogList(SystemEDIJobLogPara para)
        {
            string commandWhere = string.Empty;
            string commandOutWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.EDIFlowID.GetValue()) && para.EDIFlowID.GetValue() != SystemEDIJobLogPara.ParaNull.NULL.ToString())
            {
                commandWhere = string.Concat(new object[] { commandWhere, "      AND W.EDI_FLOW_ID={EDI_FLOW_ID} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.EDIJobID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "      AND B.EDI_JOB_ID={EDI_JOB_ID} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.EDINO.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "      AND W.EDI_NO={EDI_NO} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.EDIDate.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "      AND W.EDI_DATE={EDI_DATE} AND W.EDI_DATE IS NOT NULL ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.EDIFlowID.GetValue()) && para.EDIFlowID.GetValue() == SystemEDIJobLogPara.ParaNull.NULL.ToString())
            {
                commandOutWhere = string.Concat(new object[] { "WHERE F.EDI_FLOW_ID IS NULL ", Environment.NewLine });
            }
            else
            {
                commandOutWhere = string.Concat(new object[] { "WHERE F.EDI_FLOW_ID IS NOT NULL ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.EDIFlowIDSearch.GetValue()))
            {
                commandOutWhere = string.Concat(new object[] { commandOutWhere, "  AND Z.EDI_FLOW_ID LIKE '%{EDI_FLOW_ID_SEARCH}%' OR F.{EDI_FLOW} LIKE '%{EDI_FLOW_ID_SEARCH}%' ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.EDIJobIDSearch.GetValue()))
            {
                commandOutWhere = string.Concat(new object[] { commandOutWhere, "  AND Z.EDI_JOB_ID LIKE '%{EDI_JOB_ID_SEARCH}%' OR J.{EDI_JOB} LIKE '%{EDI_JOB_ID_SEARCH}%' ", Environment.NewLine });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT Z.SYS_ID, dbo.FN_GET_NMID(Z.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , Z.EDI_FLOW_ID, dbo.FN_GET_NMID(Z.EDI_FLOW_ID, F.{EDI_FLOW}) AS EDI_FLOW_NM ", Environment.NewLine,
                "     , Z.EDI_JOB_ID, dbo.FN_GET_NMID(Z.EDI_JOB_ID, J.{EDI_JOB}) AS EDI_JOB_NM ", Environment.NewLine,
                "     , Z.EDI_NO ", Environment.NewLine,
                "     , Z.STATUS_ID, Z.STATUS_NM ", Environment.NewLine,
                "     , Z.RESULT_ID, Z.RESULT_NM ", Environment.NewLine,
                "     , Z.RESULT_CODE ",Environment.NewLine,
                "     , Z.DT_BEGIN, Z.DT_END, Z.COUNT_ROW ", Environment.NewLine,
                "     , Z.UPD_USER_NM, Z.UPD_DT ", Environment.NewLine,
                "FROM ( ", Environment.NewLine,
                "    SELECT W.SYS_ID, W.EDI_FLOW_ID, B.EDI_JOB_ID ", Environment.NewLine,
                "         , B.EDI_NO ", Environment.NewLine,
                "         , B.STATUS_ID, (CASE WHEN B.STATUS_ID IS NULL THEN NULL ELSE dbo.FN_GET_NMID(B.STATUS_ID, dbo.FN_GET_CM_NM('0002', B.STATUS_ID, {CULTURE_ID})) END) AS STATUS_NM ", Environment.NewLine,
                "         , B.RESULT_ID, (CASE WHEN B.RESULT_ID IS NULL THEN NULL ELSE dbo.FN_GET_NMID(B.RESULT_ID, dbo.FN_GET_CM_NM('0003', B.RESULT_ID, {CULTURE_ID})) END) AS RESULT_NM ", Environment.NewLine,
                "         , B.RESULT_CODE ", Environment.NewLine,
                "         , W.DT_BEGIN AS FLOW_DT_BEGIN, W.UPD_DT AS FLOW_UPD_DT ", Environment.NewLine,
                "         , B.DT_BEGIN, B.DT_END, B.COUNT_ROW ", Environment.NewLine,
                "         , dbo.FN_GET_USER_NM(B.UPD_USER_ID) AS UPD_USER_NM, B.UPD_DT ", Environment.NewLine,
                "    FROM EDI_FLOW W ", Environment.NewLine,
                "    LEFT JOIN EDI_JOB B ON W.EDI_NO=B.EDI_NO ", Environment.NewLine,
                "    WHERE W.SYS_ID={SYS_ID} ", Environment.NewLine,
                commandWhere,
                ") Z ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_EDI_FLOW F ON Z.SYS_ID=F.SYS_ID AND Z.EDI_FLOW_ID=F.EDI_FLOW_ID ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_EDI_JOB J ON F.SYS_ID=J.SYS_ID AND F.EDI_FLOW_ID=J.EDI_FLOW_ID AND Z.EDI_JOB_ID=J.EDI_JOB_ID ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_MAIN M ON Z.SYS_ID=M.SYS_ID ", Environment.NewLine,
                commandOutWhere,
                "ORDER BY Z.SYS_ID ", Environment.NewLine,
                "      , (CASE WHEN Z.FLOW_DT_BEGIN IS NULL THEN Z.FLOW_UPD_DT ELSE Z.FLOW_DT_BEGIN END) DESC ", Environment.NewLine,
                "      , (CASE WHEN Z.DT_BEGIN IS NULL THEN Z.UPD_DT ELSE Z.DT_BEGIN END) DESC ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.EDI_FLOW_ID.ToString(), Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.EDI_JOB_ID.ToString(), Value = para.EDIJobID });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.EDI_NO.ToString(), Value = para.EDINO });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.EDI_DATE.ToString(), Value = para.EDIDate });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.CULTURE_ID.ToString(), Value = new DBVarChar(para.CultureID) });

            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIJobLogPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.EDI_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIJobLogPara.ParaField.EDI_FLOW.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.EDI_JOB.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIJobLogPara.ParaField.EDI_JOB.ToString())) });

            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.EDI_FLOW_ID_SEARCH.ToString(), Value = para.EDIFlowIDSearch });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.EDI_JOB_ID_SEARCH.ToString(), Value = para.EDIJobIDSearch });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemEDIJobLog> systemEDIJobLogList = new List<SystemEDIJobLog>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemEDIJobLog systemEDIJobLog = new SystemEDIJobLog()
                    {
                        EDINO = new DBVarChar(dataRow[SystemEDIJobLog.DataField.EDI_NO.ToString()]),
                        
                        SysID = new DBVarChar(dataRow[SystemEDIJobLog.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemEDIJobLog.DataField.SYS_NM.ToString()]),
                        EDIFlowID = new DBVarChar(dataRow[SystemEDIJobLog.DataField.EDI_FLOW_ID.ToString()]),
                        EDIFlowNM = new DBNVarChar(dataRow[SystemEDIJobLog.DataField.EDI_FLOW_NM.ToString()]),
                        EDIJobID = new DBVarChar(dataRow[SystemEDIJobLog.DataField.EDI_JOB_ID.ToString()]),
                        EDIJobNM = new DBNVarChar(dataRow[SystemEDIJobLog.DataField.EDI_JOB_NM.ToString()]),

                        StatusID = new DBVarChar(dataRow[SystemEDIJobLog.DataField.STATUS_ID.ToString()]),
                        EDIJobStatusNM = new DBNVarChar(dataRow[SystemEDIJobLog.DataField.STATUS_NM.ToString()]),
                        ResultID = new DBVarChar(dataRow[SystemEDIJobLog.DataField.RESULT_ID.ToString()]),
                        EDIJobResultNM = new DBNVarChar(dataRow[SystemEDIJobLog.DataField.RESULT_NM.ToString()]),
                        ResultCode = new DBVarChar(dataRow[SystemEDIJobLog.DataField.RESULT_CODE.ToString()]),

                        DTBegin = new DBDateTime(dataRow[SystemEDIJobLog.DataField.DT_BEGIN.ToString()]),
                        DTEnd = new DBDateTime(dataRow[SystemEDIJobLog.DataField.DT_END.ToString()]),
                        CountRow = new DBInt(dataRow[SystemEDIJobLog.DataField.COUNT_ROW.ToString()]),

                        UpdUserID = new DBVarChar(dataRow[SystemEDIJobLog.DataField.UPD_USER_NM.ToString()]),
                        UpdDt = new DBDateTime(dataRow[SystemEDIJobLog.DataField.UPD_DT.ToString()])
                    };
                    systemEDIJobLogList.Add(systemEDIJobLog);
                }
                return systemEDIJobLogList;
            }
            return null;
        }
    }
}