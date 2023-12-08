using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemFunGroupDetail : EntitySys
    {
        public EntitySystemFunGroupDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public enum EnumDeleteSystemFunGroupDetailResult
        {
            Success, Failure, DataExist
        }

        public class SystemFunGroupDetailPara
        {
            public enum Field
            {
                SYS_ID, FUN_CONTROLLER_ID,
                FUN_GROUP_ZH_TW, FUN_GROUP_ZH_CN, FUN_GROUP_EN_US,
                FUN_GROUP_TH_TH, FUN_GROUP_JA_JP,
                SORT_ORDER,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;

            public DBNVarChar FunGroupZHTW;
            public DBNVarChar FunGroupZHCN;
            public DBNVarChar FunGroupENUS;
            public DBNVarChar FunGroupTHTH;
            public DBNVarChar FunGroupJAJP;
            public DBVarChar SortOrder;

            public DBVarChar UpdUserID;
        }

        public class SystemFunGroupDetail : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, FUN_CONTROLLER_ID,
                FUN_GROUP_ZH_TW, FUN_GROUP_ZH_CN, FUN_GROUP_EN_US,
                FUN_GROUP_TH_TH, FUN_GROUP_JA_JP,
                SORT_ORDER
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;

            public DBNVarChar FunGroupZHTW;
            public DBNVarChar FunGroupZHCN;
            public DBNVarChar FunGroupENUS;
            public DBNVarChar FunGroupTHTH;
            public DBNVarChar FunGroupJAJP;
            public DBVarChar SortOrder;
        }

        public SystemFunGroupDetail SelectSystemFunGroupDetail(SystemFunGroupDetailPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID, FUN_CONTROLLER_ID ", Environment.NewLine,
                "     , FUN_GROUP_ZH_TW, FUN_GROUP_ZH_CN, FUN_GROUP_EN_US ", Environment.NewLine,
                "     , FUN_GROUP_TH_TH, FUN_GROUP_JA_JP ", Environment.NewLine,
                "     , SORT_ORDER ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN_GROUP ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID}; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemFunGroupDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunGroupDetailPara.Field.FUN_CONTROLLER_ID, Value = para.FunControllerID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                SystemFunGroupDetail systemFunGroupDetail = new SystemFunGroupDetail()
                {
                    SysID = new DBVarChar(dataRow[SystemFunGroupDetail.DataField.SYS_ID.ToString()]),
                    FunControllerID = new DBVarChar(dataRow[SystemFunGroupDetail.DataField.FUN_CONTROLLER_ID.ToString()]),
                    FunGroupZHTW = new DBNVarChar(dataRow[SystemFunGroupDetail.DataField.FUN_GROUP_ZH_TW.ToString()]),
                    FunGroupZHCN = new DBNVarChar(dataRow[SystemFunGroupDetail.DataField.FUN_GROUP_ZH_CN.ToString()]),
                    FunGroupENUS = new DBNVarChar(dataRow[SystemFunGroupDetail.DataField.FUN_GROUP_EN_US.ToString()]),
                    FunGroupTHTH = new DBNVarChar(dataRow[SystemFunGroupDetail.DataField.FUN_GROUP_TH_TH.ToString()]),
                    FunGroupJAJP = new DBNVarChar(dataRow[SystemFunGroupDetail.DataField.FUN_GROUP_JA_JP.ToString()]),
                    SortOrder = new DBVarChar(dataRow[SystemFunGroupDetail.DataField.SORT_ORDER.ToString()])
                };
                return systemFunGroupDetail;
            }
            return null;
        }

        public enum EnumEditSystemFunGroupDetailResult
        {
            Success, Failure
        }

        public EnumEditSystemFunGroupDetailResult EditSystemFunGroupDetail(SystemFunGroupDetailPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE FROM SYS_SYSTEM_FUN_GROUP ", Environment.NewLine,
                "        WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID}; ", Environment.NewLine,

                "        INSERT INTO SYS_SYSTEM_FUN_GROUP VALUES ( ", Environment.NewLine,
                "            {SYS_ID}, {FUN_CONTROLLER_ID} ", Environment.NewLine,
                "          , {FUN_GROUP_ZH_TW}, {FUN_GROUP_ZH_CN}, {FUN_GROUP_EN_US}, {FUN_GROUP_TH_TH}, {FUN_GROUP_JA_JP} ", Environment.NewLine,
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
            dbParameters.Add(new DBParameter { Name = SystemFunGroupDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunGroupDetailPara.Field.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunGroupDetailPara.Field.FUN_GROUP_ZH_TW, Value = para.FunGroupZHTW });
            dbParameters.Add(new DBParameter { Name = SystemFunGroupDetailPara.Field.FUN_GROUP_ZH_CN, Value = para.FunGroupZHCN });
            dbParameters.Add(new DBParameter { Name = SystemFunGroupDetailPara.Field.FUN_GROUP_EN_US, Value = para.FunGroupENUS });
            dbParameters.Add(new DBParameter { Name = SystemFunGroupDetailPara.Field.FUN_GROUP_TH_TH, Value = para.FunGroupTHTH });
            dbParameters.Add(new DBParameter { Name = SystemFunGroupDetailPara.Field.FUN_GROUP_JA_JP, Value = para.FunGroupJAJP });
            dbParameters.Add(new DBParameter { Name = SystemFunGroupDetailPara.Field.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = SystemFunGroupDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditSystemFunGroupDetailResult.Success : EnumEditSystemFunGroupDetailResult.Failure;
        }

        public EnumDeleteSystemFunGroupDetailResult DeleteSystemFunGroupDetail(SystemFunGroupDetailPara para)
        {
            try
            {
                string commandText = string.Concat(new object[] 
                {
                    "DECLARE @RESULT CHAR(1) = 'N'; ", Environment.NewLine,
                    "IF NOT EXISTS (SELECT * FROM SYS_SYSTEM_FUN WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID}) ", Environment.NewLine,
                    "BEGIN ", Environment.NewLine,
                    "    DELETE FROM SYS_SYSTEM_FUN_GROUP ", Environment.NewLine,
                    "    WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID}; ", Environment.NewLine,
                    "    SET @RESULT = 'Y'; ", Environment.NewLine,
                    "END; ", Environment.NewLine,
                    "SELECT @RESULT; "
                });

                List<DBParameter> dbParameters = new List<DBParameter>();
                dbParameters.Add(new DBParameter { Name = SystemFunGroupDetailPara.Field.SYS_ID, Value = para.SysID });
                dbParameters.Add(new DBParameter { Name = SystemFunGroupDetailPara.Field.FUN_CONTROLLER_ID, Value = para.FunControllerID });

                DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
                if (result.GetValue() == EnumYN.Y.ToString())
                {
                    return EnumDeleteSystemFunGroupDetailResult.Success;
                }
                else
                {
                    return EnumDeleteSystemFunGroupDetailResult.DataExist;
                }
            }
            catch
            {
                return EnumDeleteSystemFunGroupDetailResult.Failure;
            }
        }
    }
}