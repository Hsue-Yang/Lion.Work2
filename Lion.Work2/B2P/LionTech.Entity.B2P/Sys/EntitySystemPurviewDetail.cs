using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemPurviewDetail : EntitySys
    {
        public EntitySystemPurviewDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public enum EnumDeleteSystemPurviewDetailResult
        {
            Success, Failure, DataExist
        }

        public class SystemPurviewDetailPara
        {
            public enum Field
            {
                SYS_ID, PURVIEW_ID,
                PURVIEW_NM_ZH_TW, PURVIEW_NM_ZH_CN, PURVIEW_NM_EN_US, PURVIEW_NM_TH_TH, PURVIEW_NM_JA_JP,
                SORT_ORDER, REMARK,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar PurviewID;

            public DBNVarChar PurviewNMZHTW;
            public DBNVarChar PurviewNMZHCN;
            public DBNVarChar PurviewNMENUS;
            public DBNVarChar PurviewNMTHTH;
            public DBNVarChar PurviewNMJAJP;
            public DBVarChar SortOrder;
            public DBNVarChar Remark;

            public DBVarChar UpdUserID;
        }

        public class SystemPurviewDetail : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, PURVIEW_ID,
                PURVIEW_NM_ZH_TW, PURVIEW_NM_ZH_CN, PURVIEW_NM_EN_US, PURVIEW_NM_TH_TH, PURVIEW_NM_JA_JP,
                SORT_ORDER, REMARK
            }

            public DBVarChar SysID;
            public DBVarChar PurviewID;

            public DBNVarChar PurviewNMZHTW;
            public DBNVarChar PurviewNMZHCN;
            public DBNVarChar PurviewNMENUS;
            public DBNVarChar PurviewNMTHTH;
            public DBNVarChar PurviewNMJAJP;
            public DBVarChar SortOrder;
            public DBNVarChar Remark;
        }

        public SystemPurviewDetail SelectSystemPurviewDetail(SystemPurviewDetailPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID, PURVIEW_ID ", Environment.NewLine,
                "     , PURVIEW_NM_ZH_TW, PURVIEW_NM_ZH_CN, PURVIEW_NM_EN_US, PURVIEW_NM_TH_TH, PURVIEW_NM_JA_JP ", Environment.NewLine,
                "     , SORT_ORDER, REMARK ", Environment.NewLine,
                "FROM SYS_SYSTEM_PURVIEW ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND PURVIEW_ID={PURVIEW_ID}; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemPurviewDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemPurviewDetailPara.Field.PURVIEW_ID, Value = para.PurviewID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                SystemPurviewDetail systemFunGroupDetail = new SystemPurviewDetail()
                {
                    SysID = new DBVarChar(dataRow[SystemPurviewDetail.DataField.SYS_ID.ToString()]),
                    PurviewID = new DBVarChar(dataRow[SystemPurviewDetail.DataField.PURVIEW_ID.ToString()]),
                    PurviewNMZHTW = new DBNVarChar(dataRow[SystemPurviewDetail.DataField.PURVIEW_NM_ZH_TW.ToString()]),
                    PurviewNMZHCN = new DBNVarChar(dataRow[SystemPurviewDetail.DataField.PURVIEW_NM_ZH_CN.ToString()]),
                    PurviewNMENUS = new DBNVarChar(dataRow[SystemPurviewDetail.DataField.PURVIEW_NM_EN_US.ToString()]),
                    PurviewNMTHTH = new DBNVarChar(dataRow[SystemPurviewDetail.DataField.PURVIEW_NM_TH_TH.ToString()]),
                    PurviewNMJAJP = new DBNVarChar(dataRow[SystemPurviewDetail.DataField.PURVIEW_NM_JA_JP.ToString()]),
                    SortOrder = new DBVarChar(dataRow[SystemPurviewDetail.DataField.SORT_ORDER.ToString()]),
                    Remark = new DBNVarChar(dataRow[SystemPurviewDetail.DataField.REMARK.ToString()])
                };
                return systemFunGroupDetail;
            }
            return null;
        }

        public enum EnumEditSystemPurviewDetailResult
        {
            Success, Failure
        }

        public EnumEditSystemPurviewDetailResult EditSystemPurviewDetail(SystemPurviewDetailPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE FROM SYS_SYSTEM_PURVIEW ", Environment.NewLine,
                "        WHERE SYS_ID={SYS_ID} AND PURVIEW_ID={PURVIEW_ID}; ", Environment.NewLine,

                "        INSERT INTO SYS_SYSTEM_PURVIEW VALUES ( ", Environment.NewLine,
                "            {SYS_ID}, {PURVIEW_ID} ", Environment.NewLine,
                "          , {PURVIEW_NM_ZH_TW}, {PURVIEW_NM_ZH_CN}, {PURVIEW_NM_EN_US}, {PURVIEW_NM_TH_TH}, {PURVIEW_NM_JA_JP} ", Environment.NewLine,
                "          , {SORT_ORDER}, {REMARK} ", Environment.NewLine,
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
            dbParameters.Add(new DBParameter { Name = SystemPurviewDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemPurviewDetailPara.Field.PURVIEW_ID, Value = para.PurviewID });
            dbParameters.Add(new DBParameter { Name = SystemPurviewDetailPara.Field.PURVIEW_NM_ZH_TW, Value = para.PurviewNMZHTW });
            dbParameters.Add(new DBParameter { Name = SystemPurviewDetailPara.Field.PURVIEW_NM_ZH_CN, Value = para.PurviewNMZHCN });
            dbParameters.Add(new DBParameter { Name = SystemPurviewDetailPara.Field.PURVIEW_NM_EN_US, Value = para.PurviewNMENUS });
            dbParameters.Add(new DBParameter { Name = SystemPurviewDetailPara.Field.PURVIEW_NM_TH_TH, Value = para.PurviewNMTHTH });
            dbParameters.Add(new DBParameter { Name = SystemPurviewDetailPara.Field.PURVIEW_NM_JA_JP, Value = para.PurviewNMJAJP });
            dbParameters.Add(new DBParameter { Name = SystemPurviewDetailPara.Field.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = SystemPurviewDetailPara.Field.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = SystemPurviewDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditSystemPurviewDetailResult.Success : EnumEditSystemPurviewDetailResult.Failure;
        }

        public EnumDeleteSystemPurviewDetailResult DeleteSystemPurviewDetail(SystemPurviewDetailPara para)
        {
            try
            {
                string commandText = string.Concat(new object[] 
                {
                    "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                    "SET @RESULT = 'N'; ", Environment.NewLine,
                    "BEGIN TRANSACTION ", Environment.NewLine,
                    "    BEGIN TRY ", Environment.NewLine,
                    "        IF NOT EXISTS (SELECT * FROM SYS_SYSTEM_FUN WHERE SYS_ID={SYS_ID} AND PURVIEW_ID={PURVIEW_ID}) ", Environment.NewLine,
                    "        BEGIN ", Environment.NewLine,
                    "            DELETE FROM SYS_SYSTEM_PURVIEW WHERE SYS_ID={SYS_ID} AND PURVIEW_ID={PURVIEW_ID}; ", Environment.NewLine,
                    "            SET @RESULT = 'Y'; ", Environment.NewLine,
                    "        END; ", Environment.NewLine,
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
                dbParameters.Add(new DBParameter { Name = SystemPurviewDetailPara.Field.SYS_ID, Value = para.SysID });
                dbParameters.Add(new DBParameter { Name = SystemPurviewDetailPara.Field.PURVIEW_ID, Value = para.PurviewID });

                DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
                if (result.GetValue() == EnumYN.Y.ToString())
                {
                    return EnumDeleteSystemPurviewDetailResult.Success;
                }
                else
                {
                    return EnumDeleteSystemPurviewDetailResult.DataExist;
                }
            }
            catch
            {
                return EnumDeleteSystemPurviewDetailResult.Failure;
            }
        }
    }
}