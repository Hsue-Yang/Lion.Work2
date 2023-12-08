using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemEDIJob : EntitySys
    {
        public EntitySystemEDIJob(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemEDIJobPara : DBCulture
        {
            public SystemEDIJobPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, SYS_NM, EDI_FLOW_ID, EDI_FLOW, EDI_JOB, SORT_ORDER, UPD_USER_ID
            }

            public DBVarChar UpdUserID;
            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
        }

        public class EDIJobValue : ValueListRow
        {
            public enum ValueField
            {
                EDI_JOB_ID, SORT_ORDER
            }

            public string SysID { get; set; }
            public string EDIJobID { get; set; }
            public string BeforeSortOrder { get; set; }
            public string AfterSortOrder { get; set; }

            public DBVarChar GetSysID()
            {
                return new DBVarChar(SysID);
            }
            public DBVarChar GetEDIJobID()
            {
                return new DBVarChar(EDIJobID);
            }
            public DBVarChar GetBeforeSortOrder()
            {
                return new DBVarChar(BeforeSortOrder);
            }
            public DBVarChar GetAfterSortOrder()
            {
                return new DBVarChar(AfterSortOrder);
            }
        }

        public class SystemEDIJob : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM, EDI_FLOW_ID, EDI_FLOW_NM,
                EDI_JOB_ID, EDI_JOB_NM, OBJECT_NAME, DEP_EDI_JOB_ID, IS_USE_RES, IS_DISABLE,
                SORT_ORDER, UPD_USER_NM, UPD_DT
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar EDIFlowID;
            public DBNVarChar EDIFlowNM;
            public DBVarChar EDIJobID;
            public DBNVarChar EDIJobNM;
            public DBVarChar ObjectName;
            public DBVarChar DepEDIJobID;
            public DBChar IsUseRes;
            public DBChar IsDisable;
            public DBVarChar SortOrder;
            public DBVarChar UpdUserID;
            public DBDateTime UpdDt;
        }

        public enum EnumEDIJobSettingResult
        {
            Success, Failure
        }

        public EnumEDIJobSettingResult EditEDIJobSetting(SystemEDIJobPara para,List<EDIJobValue> EDIJobValueList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();
            foreach (EDIJobValue EDIJobValue in EDIJobValueList)
            {
                //判斷SORT_ORDER有變才更新
                if (EDIJobValue.AfterSortOrder != EDIJobValue.BeforeSortOrder)
                {
                    string insertCommand = string.Concat(new object[]
                    {
                        "UPDATE SYS_SYSTEM_EDI_JOB SET ", Environment.NewLine,
                        "SORT_ORDER={SORT_ORDER},UPD_USER_ID={UPD_USER_ID},UPD_DT=GETDATE() ", Environment.NewLine,
                        "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} AND EDI_JOB_ID={EDI_JOB_ID}; ", Environment.NewLine,
                    });

                    dbParameters.Add(new DBParameter { Name = EDIJobValue.ValueField.SORT_ORDER, Value = EDIJobValue.GetAfterSortOrder() });
                    dbParameters.Add(new DBParameter { Name = EDIJobValue.ValueField.EDI_JOB_ID, Value = EDIJobValue.GetEDIJobID() });
                    dbParameters.Add(new DBParameter { Name = SystemEDIJobPara.ParaField.SYS_ID, Value = para.SysID});
                    dbParameters.Add(new DBParameter { Name = SystemEDIJobPara.ParaField.EDI_FLOW_ID, Value = para.EDIFlowID });
                    dbParameters.Add(new DBParameter { Name = SystemEDIJobPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                    commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, insertCommand, dbParameters));
                    dbParameters.Clear();
                }
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

        public List<SystemEDIJob> SelectSystemEDIJobList(SystemEDIJobPara para)
        {
            string commandWhere = string.Empty;

            if (para.EDIFlowID != null && !string.IsNullOrWhiteSpace(para.EDIFlowID.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                {
                    "  AND E.EDI_FLOW_ID={EDI_FLOW_ID} ", Environment.NewLine
                });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT M.SYS_ID, dbo.FN_GET_NMID(M.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , E.EDI_FLOW_ID, dbo.FN_GET_NMID(E.EDI_FLOW_ID, E.{EDI_FLOW}) AS EDI_FLOW_NM ", Environment.NewLine,
                "     , J.EDI_JOB_ID, dbo.FN_GET_NMID(J.EDI_JOB_ID, J.{EDI_JOB}) AS EDI_JOB_NM ", Environment.NewLine,
                "     , J.OBJECT_NAME, J.DEP_EDI_JOB_ID, J.IS_USE_RES, J.IS_DISABLE ", Environment.NewLine,
                "     , J.SORT_ORDER, dbo.FN_GET_USER_NM(J.UPD_USER_ID) AS UPD_USER_NM, J.UPD_DT ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_JOB J ", Environment.NewLine,
                "JOIN SYS_SYSTEM_EDI_FLOW E ON J.SYS_ID=E.SYS_ID AND J.EDI_FLOW_ID=E.EDI_FLOW_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON J.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "WHERE E.SYS_ID={SYS_ID} ", Environment.NewLine,
                commandWhere, Environment.NewLine,
                "ORDER BY J.SYS_ID, J.EDI_FLOW_ID, J.SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIJobPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobPara.ParaField.EDI_FLOW_ID.ToString(), Value = para.EDIFlowID });

            dbParameters.Add(new DBParameter { Name = SystemEDIJobPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIJobPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobPara.ParaField.EDI_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIJobPara.ParaField.EDI_FLOW.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEDIJobPara.ParaField.EDI_JOB.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIJobPara.ParaField.EDI_JOB.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemEDIJob> systemEDIJobList = new List<SystemEDIJob>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemEDIJob systemEDIJob = new SystemEDIJob()
                    {
                        SysID = new DBVarChar(dataRow[SystemEDIJob.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemEDIJob.DataField.SYS_NM.ToString()]),
                        EDIFlowID = new DBVarChar(dataRow[SystemEDIJob.DataField.EDI_FLOW_ID.ToString()]),
                        EDIFlowNM = new DBNVarChar(dataRow[SystemEDIJob.DataField.EDI_FLOW_NM.ToString()]),
                        EDIJobID = new DBVarChar(dataRow[SystemEDIJob.DataField.EDI_JOB_ID.ToString()]),
                        EDIJobNM = new DBNVarChar(dataRow[SystemEDIJob.DataField.EDI_JOB_NM.ToString()]),
                        ObjectName = new DBVarChar(dataRow[SystemEDIJob.DataField.OBJECT_NAME.ToString()]),
                        DepEDIJobID = new DBVarChar(dataRow[SystemEDIJob.DataField.DEP_EDI_JOB_ID.ToString()]),
                        IsUseRes = new DBChar(dataRow[SystemEDIJob.DataField.IS_USE_RES.ToString()]),
                        IsDisable = new DBChar(dataRow[SystemEDIJob.DataField.IS_DISABLE.ToString()]),
                        SortOrder = new DBVarChar(dataRow[SystemEDIJob.DataField.SORT_ORDER.ToString()]),
                        UpdUserID = new DBVarChar(dataRow[SystemEDIJob.DataField.UPD_USER_NM.ToString()]),
                        UpdDt = new DBDateTime(dataRow[SystemEDIJob.DataField.UPD_DT.ToString()])
                    };
                    systemEDIJobList.Add(systemEDIJob);
                }
                return systemEDIJobList;
            }
            return null;
        }
    }
}