using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemEDIFlow : EntitySys
    {
        public EntitySystemEDIFlow(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class EDIFlowValue : ValueListRow
        {
            public enum ValueField
            {
                EDI_FLOW_ID, SORT_ORDER
            }

            public string SysID { get; set; }
            public string EDIFlowID { get; set; }
            public string BeforeSortOrder { get; set; }
            public string AfterSortOrder { get; set; }

            public DBVarChar GetSysID()
            {
                return new DBVarChar(SysID);
            }
            public DBVarChar GetEDIFlowID()
            {
                return new DBVarChar(EDIFlowID);
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

        public class SystemEDIFlowPara : DBCulture
        {
            public SystemEDIFlowPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, SYS_NM, EDI_FLOW, UPD_USER_ID 
            }

            public DBVarChar SysID;
            public DBVarChar UpdUserID;
        }

        public class SystemEDIFlow : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM, EDI_FLOW_ID, EDI_FLOW_NM, SORT_ORDER, UPD_USER_NM, UPD_DT
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar EDIFlowID;
            public DBNVarChar EDIFlowNM;
            public DBVarChar SortOrder;
            public DBVarChar UpdUserNM;
            public DBDateTime UpdDt;
        }

        public enum EnumEDIFlowSettingResult
        {
            Success, Failure
        }

        public EnumEDIFlowSettingResult EditEDIFlowSetting(SystemEDIFlowPara para, List<EDIFlowValue> EDIFlowValueList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            foreach (EDIFlowValue EDIFlowValue in EDIFlowValueList)
            {
                //判斷SORT_ORDER有變才更新
                if (EDIFlowValue.AfterSortOrder != EDIFlowValue.BeforeSortOrder)
                {
                    string insertCommand = string.Concat(new object[]
                    {
                        " UPDATE SYS_SYSTEM_EDI_FLOW SET  ", Environment.NewLine,
                        " SORT_ORDER={SORT_ORDER},UPD_USER_ID={UPD_USER_ID},UPD_DT=GETDATE()", Environment.NewLine,
                        " WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID};", Environment.NewLine,
                    });

                    dbParameters.Add(new DBParameter { Name = EDIFlowValue.ValueField.SORT_ORDER, Value = EDIFlowValue.GetAfterSortOrder() });
                    dbParameters.Add(new DBParameter { Name = EDIFlowValue.ValueField.EDI_FLOW_ID, Value = EDIFlowValue.GetEDIFlowID() });
                    dbParameters.Add(new DBParameter { Name = SystemEDIFlowPara.ParaField.SYS_ID, Value = para.SysID });
                    dbParameters.Add(new DBParameter { Name = SystemEDIFlowPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

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
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEDIFlowSettingResult.Success : EnumEDIFlowSettingResult.Failure;
        }

        public List<SystemEDIFlow> SelectSystemEDIFlowList(SystemEDIFlowPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT M.SYS_ID, dbo.FN_GET_NMID(M.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , E.EDI_FLOW_ID, dbo.FN_GET_NMID(E.EDI_FLOW_ID, E.{EDI_FLOW}) AS EDI_FLOW_NM ", Environment.NewLine,
                "     , E.SORT_ORDER, dbo.FN_GET_USER_NM(E.UPD_USER_ID) AS UPD_USER_NM, E.UPD_DT ", Environment.NewLine,
                "FROM SYS_SYSTEM_MAIN M ", Environment.NewLine,
                "JOIN SYS_SYSTEM_EDI_FLOW E ON M.SYS_ID=E.SYS_ID ", Environment.NewLine,
                "WHERE M.SYS_ID={SYS_ID}", Environment.NewLine,
                "ORDER BY M.SYS_ID,E.SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIFlowPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEDIFlowPara.ParaField.EDI_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIFlowPara.ParaField.EDI_FLOW.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemEDIFlow> SystemEDIFlowList = new List<SystemEDIFlow>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemEDIFlow systemEDIFlow = new SystemEDIFlow()
                    {
                        SysID = new DBVarChar(dataRow[SystemEDIFlow.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemEDIFlow.DataField.SYS_NM.ToString()]),
                        EDIFlowID = new DBVarChar(dataRow[SystemEDIFlow.DataField.EDI_FLOW_ID.ToString()]),
                        EDIFlowNM = new DBNVarChar(dataRow[SystemEDIFlow.DataField.EDI_FLOW_NM.ToString()]),
                        SortOrder = new DBVarChar(dataRow[SystemEDIFlow.DataField.SORT_ORDER.ToString()]),
                        UpdUserNM = new DBVarChar(dataRow[SystemEDIFlow.DataField.UPD_USER_NM.ToString()]),
                        UpdDt = new DBDateTime(dataRow[SystemEDIFlow.DataField.UPD_DT.ToString()])
                    };
                    SystemEDIFlowList.Add(systemEDIFlow);
                }
                return SystemEDIFlowList;
            }
            return null;
        }
    }
}