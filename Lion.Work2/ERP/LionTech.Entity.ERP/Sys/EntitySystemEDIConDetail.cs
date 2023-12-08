using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using LionTech.Utility;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemEDIConDetail : EntitySys
    {
        public EntitySystemEDIConDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public enum SortorderField
        {
            [Description("00")]
            Right,
            [Description("000")]
            Left,
            [Description("000001")]
            First
        }

        public class SystemEDIConDetailPara
        {
            public enum Field
            {
                SYS_ID, EDI_FLOW_ID, EDI_CON_ID,
                EDI_CON_ZH_TW, EDI_CON_ZH_CN, EDI_CON_EN_US, EDI_CON_TH_TH, EDI_CON_JA_JP,
                PROVIDER_NAME, CON_VALUE,
                SORT_ORDER, UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBVarChar EDIConID;
            public DBNVarChar EDIConZHTW;
            public DBNVarChar EDIConZHCN;
            public DBNVarChar EDIConENUS;
            public DBNVarChar EDIConTHTH;
            public DBNVarChar EDIConJAJP;
            public DBVarChar ProviderName;
            public DBVarChar ConValue;
            public DBVarChar SortOrder;
            public DBVarChar UpdUserID;
        }

        public class SystemEDIConDetail : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, EDI_FLOW_ID, EDI_CON_ID,
                EDI_CON_ZH_TW, EDI_CON_ZH_CN, EDI_CON_EN_US, EDI_CON_TH_TH, EDI_CON_JA_JP,
                PROVIDER_NAME, CON_VALUE,
                SORT_ORDER, NEW_SORT_ORDER
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBVarChar EDIConID;
            public DBNVarChar EDIConZHTW;
            public DBNVarChar EDIConZHCN;
            public DBNVarChar EDIConENUS;
            public DBNVarChar EDIConTHTH;
            public DBNVarChar EDIConJAJP;
            public DBVarChar ProviderName;
            public DBVarChar ConValue;
            public DBVarChar SortOrder;
        }

        public SystemEDIConDetail SelectSystemEDIConDetail(SystemEDIConDetailPara para) //明細
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID, EDI_FLOW_ID, EDI_CON_ID ", Environment.NewLine, 
                "     , EDI_CON_ZH_TW, EDI_CON_ZH_CN, EDI_CON_EN_US, EDI_CON_TH_TH, EDI_CON_JA_JP ", Environment.NewLine,
                "     , PROVIDER_NAME, CON_VALUE ", Environment.NewLine,
                "     , SORT_ORDER ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_CON ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} AND EDI_CON_ID={EDI_CON_ID}; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.EDI_CON_ID, Value = para.EDIConID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                SystemEDIConDetail systemEDICon = new SystemEDIConDetail()
                {
                    SysID = new DBVarChar(dataRow[SystemEDIConDetail.DataField.SYS_ID.ToString()]),
                    EDIConID = new DBVarChar(dataRow[SystemEDIConDetail.DataField.EDI_CON_ID.ToString()]),
                    EDIConZHTW = new DBNVarChar(dataRow[SystemEDIConDetail.DataField.EDI_CON_ZH_TW.ToString()]),
                    EDIConZHCN = new DBNVarChar(dataRow[SystemEDIConDetail.DataField.EDI_CON_ZH_CN.ToString()]),
                    EDIConENUS = new DBNVarChar(dataRow[SystemEDIConDetail.DataField.EDI_CON_EN_US.ToString()]),
                    EDIConTHTH = new DBNVarChar(dataRow[SystemEDIConDetail.DataField.EDI_CON_TH_TH.ToString()]),
                    EDIConJAJP = new DBNVarChar(dataRow[SystemEDIConDetail.DataField.EDI_CON_JA_JP.ToString()]),
                    ProviderName = new DBVarChar(dataRow[SystemEDIConDetail.DataField.PROVIDER_NAME.ToString()]),
                    ConValue = new DBVarChar(dataRow[SystemEDIConDetail.DataField.CON_VALUE.ToString()]),
                    SortOrder = new DBVarChar(dataRow[SystemEDIConDetail.DataField.SORT_ORDER.ToString()])
                };
                return systemEDICon;
            }
            return null;
        }

        public enum EnumEditSystemEDIConDetailResult
        {
            Success, Failure
        }

        public string GetConNewSortOrder(SystemEDIConDetailPara para)
        {
            string newSortOrder = Common.GetEnumDesc(SortorderField.First);
            string commandText = string.Concat(new object[]
            {
                "SELECT TOP 1 SORT_ORDER AS NEW_SORT_ORDER ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_CON ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} ", Environment.NewLine,
                "ORDER BY SORT_ORDER DESC ; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                newSortOrder = dataRow[SystemEDIConDetail.DataField.NEW_SORT_ORDER.ToString()].ToString().Replace("0", null);
                if (!string.IsNullOrWhiteSpace(newSortOrder))
                {
                    newSortOrder = Convert.ToString(Convert.ToInt32(newSortOrder) + 1);
                    newSortOrder = Common.GetEnumDesc(SortorderField.Left) + newSortOrder + Common.GetEnumDesc(SortorderField.Right);
                    newSortOrder = newSortOrder.Substring(newSortOrder.Length - 6, 6);
                }
            }
            return newSortOrder;
        }

        public EnumEditSystemEDIConDetailResult EditSystemEDIConDetail(SystemEDIConDetailPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE FROM SYS_SYSTEM_EDI_CON ", Environment.NewLine,
                "        WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} AND EDI_CON_ID={EDI_CON_ID}; ", Environment.NewLine,

                "        INSERT INTO SYS_SYSTEM_EDI_CON VALUES ( ", Environment.NewLine,
                "            {SYS_ID} ", Environment.NewLine,
                "          , {EDI_FLOW_ID} ", Environment.NewLine,
                "          , {EDI_CON_ID} ", Environment.NewLine,
                "          , {EDI_CON_ZH_TW}, {EDI_CON_ZH_CN}, {EDI_CON_EN_US}, {EDI_CON_TH_TH}, {EDI_CON_JA_JP} ", Environment.NewLine,
                "          , {PROVIDER_NAME}, {CON_VALUE} ", Environment.NewLine,
                "          , {SORT_ORDER} ", Environment.NewLine,
                "          , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "        ); ", Environment.NewLine,

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

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.EDI_CON_ID, Value = para.EDIConID });
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.EDI_CON_ZH_TW, Value = para.EDIConZHTW });
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.EDI_CON_ZH_CN, Value = para.EDIConZHCN });
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.EDI_CON_EN_US, Value = para.EDIConENUS });
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.EDI_CON_TH_TH, Value = para.EDIConTHTH });
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.EDI_CON_JA_JP, Value = para.EDIConJAJP });
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.PROVIDER_NAME, Value = para.ProviderName });
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.CON_VALUE, Value = para.ConValue });
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditSystemEDIConDetailResult.Success : EnumEditSystemEDIConDetailResult.Failure;
        }

        public enum EnumDeleteSystemEDIConDetailResult
        {
            Success, Failure, DataExist
        }

        public EnumDeleteSystemEDIConDetailResult DeleteSystemEDIConDetail(SystemEDIConDetailPara para)
        {
            try
            {
                string commandText = string.Concat(new object[] 
                {
                    "DECLARE @RESULT CHAR(1) = 'N'; ", Environment.NewLine,
                    "IF NOT EXISTS (SELECT * FROM SYS_SYSTEM_EDI_JOB WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} AND EDI_CON_ID={EDI_CON_ID}) ", Environment.NewLine,
                    "BEGIN ", Environment.NewLine,
                    "            DELETE FROM SYS_SYSTEM_EDI_CON ", Environment.NewLine,
                    "                WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} AND EDI_CON_ID={EDI_CON_ID}; ", Environment.NewLine,
                    "            SET @RESULT = 'Y'; ", Environment.NewLine,
                    "END; ", Environment.NewLine,
                    "SELECT @RESULT; ", Environment.NewLine
                });

                List<DBParameter> dbParameters = new List<DBParameter>();
                dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.SYS_ID, Value = para.SysID });
                dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.EDI_FLOW_ID, Value = para.EDIFlowID });
                dbParameters.Add(new DBParameter { Name = SystemEDIConDetailPara.Field.EDI_CON_ID, Value = para.EDIConID });

                DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
                if (result.GetValue() == EnumYN.Y.ToString())
                {
                    return EnumDeleteSystemEDIConDetailResult.Success;
                }
                else
                {
                    return EnumDeleteSystemEDIConDetailResult.DataExist;
                }
            }
            catch
            {
                return EnumDeleteSystemEDIConDetailResult.Failure;
            }
        }
    }
}