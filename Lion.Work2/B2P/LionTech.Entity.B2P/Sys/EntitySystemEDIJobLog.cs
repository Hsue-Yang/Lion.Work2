using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
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
                SYS_ID, SYS_NM, EDI_FLOW_ID, EDI_FLOW, EDI_JOB_ID, EDI_JOB, EDI_NO, EDI_DATE, CULTURE_ID
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBVarChar EDIJobID;
            public DBChar EDINO;
            public DBChar EDIDate;
        }

        public class SystemEDIJobLog : DBTableRow
        {
            public enum DataField
            {
                EDI_NO, SYS_ID, SYS_NM, EDI_FLOW_ID, EDI_FLOW_NM, EDI_JOB_ID, EDI_JOB_NM,
                STATUS_ID, RESULT_ID, RESULT_CODE,EDIJOB_STATUS_NM,EDIJOB_RESULT_NM,
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
            public DBVarChar ResultID;
            public DBVarChar ResultCode;
            public DBNVarChar EDIJobStatusNM;
            public DBNVarChar EDIJobResultNM;
            public DBDateTime DTBegin;
            public DBDateTime DTEnd;
            public DBInt CountRow;

            public DBVarChar UpdUserID;
            public DBDateTime UpdDt;
        }

        public List<SystemEDIJobLog> SelectSystemEDIJobLogList(SystemEDIJobLogPara para)
        {
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.EDIJobID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND B.EDI_JOB_ID={EDI_JOB_ID} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.EDINO.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND B.EDI_NO={EDI_NO} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.EDIDate.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND (W.EDI_DATE={EDI_DATE} OR W.EDI_DATE IS NULL)  ", Environment.NewLine });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT M.SYS_ID, dbo.FN_GET_NMID(M.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , E.EDI_FLOW_ID, dbo.FN_GET_NMID(E.EDI_FLOW_ID, E.{EDI_FLOW}) AS EDI_FLOW_NM ", Environment.NewLine,
                "     , J.EDI_JOB_ID, dbo.FN_GET_NMID(B.EDI_JOB_ID, J.{EDI_JOB}) AS EDI_JOB_NM ", Environment.NewLine,
                "     , B.EDI_NO, B.STATUS_ID, B.RESULT_ID ", Environment.NewLine,
                "     , (CASE WHEN B.STATUS_ID IS NULL THEN NULL ELSE dbo.FN_GET_NMID(B.STATUS_ID, dbo.FN_GET_CM_NM('0002', B.STATUS_ID, {CULTURE_ID})) END) AS EDIJOB_STATUS_NM ", Environment.NewLine,
                "     , (CASE WHEN B.RESULT_ID IS NULL THEN NULL ELSE dbo.FN_GET_NMID(B.RESULT_ID, dbo.FN_GET_CM_NM('0003', B.RESULT_ID, {CULTURE_ID})) END) AS EDIJOB_RESULT_NM ", Environment.NewLine,
                "     , B.RESULT_CODE ", Environment.NewLine,
                "     , B.DT_BEGIN, B.DT_END, B.COUNT_ROW ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(B.UPD_USER_ID) AS UPD_USER_NM, B.UPD_DT ", Environment.NewLine,
                "FROM EDI_JOB B ", Environment.NewLine,
                "JOIN EDI_FLOW W ON B.EDI_NO=W.EDI_NO ", Environment.NewLine,
                "JOIN SYS_SYSTEM_EDI_JOB J ON B.EDI_JOB_ID=J.EDI_JOB_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_EDI_FLOW E ON J.SYS_ID=E.SYS_ID AND J.EDI_FLOW_ID=E.EDI_FLOW_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON E.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "WHERE E.SYS_ID={SYS_ID} AND E.EDI_FLOW_ID={EDI_FLOW_ID}", Environment.NewLine,
                commandWhere,
                "ORDER BY UPD_DT DESC", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIJobLogPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.EDI_FLOW_ID.ToString(), Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.EDI_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIJobLogPara.ParaField.EDI_FLOW.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.EDI_JOB_ID.ToString(), Value = para.EDIJobID });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.EDI_JOB.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIJobLogPara.ParaField.EDI_JOB.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.EDI_NO.ToString(), Value = para.EDINO });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.EDI_DATE.ToString(), Value = para.EDIDate });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobLogPara.ParaField.CULTURE_ID.ToString(), Value = new DBVarChar(para.CultureID) });

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
                        ResultID = new DBVarChar(dataRow[SystemEDIJobLog.DataField.RESULT_ID.ToString()]),
                        ResultCode = new DBVarChar(dataRow[SystemEDIJobLog.DataField.RESULT_CODE.ToString()]),
                        EDIJobStatusNM = new DBNVarChar(dataRow[SystemEDIJobLog.DataField.EDIJOB_STATUS_NM.ToString()]),
                        EDIJobResultNM = new DBNVarChar(dataRow[SystemEDIJobLog.DataField.EDIJOB_RESULT_NM.ToString()]),

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