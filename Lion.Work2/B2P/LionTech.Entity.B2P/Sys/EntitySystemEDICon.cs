using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemEDICon : EntitySys
    {
        public EntitySystemEDICon(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemEDIConPara : DBCulture
        {
            public SystemEDIConPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, SYS_NM, EDI_FLOW_ID, EDI_FLOW, EDI_CON, SORT_ORDER, UPD_USER_ID
            }
            public DBVarChar UpdUserID;
            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
        }

        public class SystemEDICon : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM, EDI_FLOW_ID, EDI_FLOW_NM,
                EDI_CON_ID, EDI_CON_NM, PROVIDER_NAME,
                SORT_ORDER, UPD_USER_NM, UPD_DT
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar EDIFlowID;
            public DBNVarChar EDIFlowNM;
            public DBVarChar EDIConID;
            public DBNVarChar EDIConNM;
            public DBVarChar ProviderName;
            public DBVarChar SortOrder;
            public DBVarChar UpdUserID;
            public DBDateTime UpdDt;
        }

        public List<SystemEDICon> SelectSystemEDIConList(SystemEDIConPara para)
        {
            string commandWhere = string.Empty;

            if (para.EDIFlowID != null && !string.IsNullOrWhiteSpace(para.EDIFlowID.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                {
                    " AND E.EDI_FLOW_ID={EDI_FLOW_ID} ", Environment.NewLine
                });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT M.SYS_ID, dbo.FN_GET_NMID(M.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "         , E.EDI_FLOW_ID, dbo.FN_GET_NMID(E.EDI_FLOW_ID, E.{EDI_FLOW}) AS EDI_FLOW_NM ", Environment.NewLine,
                "         , C.EDI_CON_ID, dbo.FN_GET_NMID(C.EDI_CON_ID, C.{EDI_CON}) AS EDI_CON_NM ", Environment.NewLine,
                "         , C.PROVIDER_NAME ", Environment.NewLine,
                "         , C.SORT_ORDER, dbo.FN_GET_USER_NM(E.UPD_USER_ID) AS UPD_USER_NM, C.UPD_DT ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_CON C INNER JOIN SYS_SYSTEM_EDI_FLOW E ON C.EDI_FLOW_ID=E.EDI_FLOW_ID AND E.SYS_ID=C.SYS_ID", Environment.NewLine,
                "        JOIN SYS_SYSTEM_MAIN M ON E.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "WHERE M.SYS_ID={SYS_ID} ", Environment.NewLine,
                commandWhere, Environment.NewLine,
                "ORDER BY C.SYS_ID,C.EDI_FLOW_ID ,C.SORT_ORDER  ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIConPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIConPara.ParaField.EDI_FLOW_ID.ToString(), Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SystemEDIConPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIConPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEDIConPara.ParaField.EDI_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIConPara.ParaField.EDI_FLOW.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEDIConPara.ParaField.EDI_CON.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEDIConPara.ParaField.EDI_CON.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemEDICon> systemEDIConList = new List<SystemEDICon>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemEDICon systemEDICon = new SystemEDICon()
                    {
                        SysID = new DBVarChar(dataRow[SystemEDICon.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemEDICon.DataField.SYS_NM.ToString()]),
                        EDIFlowID = new DBVarChar(dataRow[SystemEDICon.DataField.EDI_FLOW_ID.ToString()]),
                        EDIFlowNM = new DBNVarChar(dataRow[SystemEDICon.DataField.EDI_FLOW_NM.ToString()]),
                        EDIConID = new DBVarChar(dataRow[SystemEDICon.DataField.EDI_CON_ID.ToString()]),
                        EDIConNM = new DBNVarChar(dataRow[SystemEDICon.DataField.EDI_CON_NM.ToString()]),
                        ProviderName = new DBVarChar(dataRow[SystemEDICon.DataField.PROVIDER_NAME.ToString()]),
                        SortOrder = new DBVarChar(dataRow[SystemEDICon.DataField.SORT_ORDER.ToString()]),
                        UpdUserID = new DBVarChar(dataRow[SystemEDICon.DataField.UPD_USER_NM.ToString()]),
                        UpdDt = new DBDateTime(dataRow[SystemEDICon.DataField.UPD_DT.ToString()])
                    };
                    systemEDIConList.Add(systemEDICon);
                }
                return systemEDIConList;
            }
            return null;
        }

        public class EDIConValue : ValueListRow
        {
            public enum ValueField
            {
                EDI_FLOW_ID, EDI_CON_ID, SORT_ORDER
            }

            public string SysID { get; set; }
            public string EDIFlowID { get; set; }
            public string EDIConID { get; set; }
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
            public DBVarChar GetEDIConID()
            {
                return new DBVarChar(EDIConID);
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

        public enum EnumEDIConSettingResult
        {
            Success, Failure
        }

        public EnumEDIConSettingResult EditEDIConSetting(SystemEDIConPara para, List<EDIConValue> EDIConValueList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();
            foreach (EDIConValue EDIConValue in EDIConValueList)
            {
                //判斷SORT_ORDER有變才更新
                if (EDIConValue.AfterSortOrder != EDIConValue.BeforeSortOrder)
                {
                    string insertCommand = string.Concat(new object[]
                    {
                        " UPDATE  SYS_SYSTEM_EDI_CON SET  ", Environment.NewLine,
                        " SORT_ORDER={SORT_ORDER},UPD_USER_ID={UPD_USER_ID},UPD_DT=GETDATE()", Environment.NewLine,
                        " WHERE EDI_FLOW_ID = {EDI_FLOW_ID} AND EDI_CON_ID={EDI_CON_ID};", Environment.NewLine,
                    });
                    
                    dbParameters.Add(new DBParameter { Name = EDIConValue.ValueField.SORT_ORDER, Value = EDIConValue.GetAfterSortOrder() });
                    dbParameters.Add(new DBParameter { Name = EDIConValue.ValueField.EDI_FLOW_ID, Value = EDIConValue.GetEDIFlowID() });
                    dbParameters.Add(new DBParameter { Name = EDIConValue.ValueField.EDI_CON_ID, Value = EDIConValue.GetEDIConID() });
                    dbParameters.Add(new DBParameter { Name = SystemEDIConPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

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
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEDIConSettingResult.Success : EnumEDIConSettingResult.Failure;
        }
    }
}