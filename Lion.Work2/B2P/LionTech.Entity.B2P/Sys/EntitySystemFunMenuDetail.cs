using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemFunMenuDetail : EntitySys
    {
        public EntitySystemFunMenuDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public enum EnumDeleteSystemFunMenuDetailResult
        {
            Success, Failure, DataExist
        }

        public class SystemFunMenuDetailPara
        {
            public enum Field
            {
                SYS_ID, FUN_MENU,
                FUN_MENU_NM_ZH_TW, FUN_MENU_NM_ZH_CN, FUN_MENU_NM_EN_US,
                FUN_MENU_NM_TH_TH, FUN_MENU_NM_JA_JP,
                DEFAULT_MENU_ID, IS_DISABLE, SORT_ORDER,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar FunMenu;

            public DBNVarChar FunMenuNMZHTW;
            public DBNVarChar FunMenuNMZHCN;
            public DBNVarChar FunMenuNMENUS;
            public DBNVarChar FunMenuNMTHTH;
            public DBNVarChar FunMenuNMJAJP;

            public DBVarChar DefaultMenuID;
            public DBChar IsDisable;
            public DBVarChar SortOrder;

            public DBVarChar UpdUserID;
        }

        public class SystemFunMenuDetail : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, FUN_MENU,
                FUN_MENU_NM_ZH_TW, FUN_MENU_NM_ZH_CN, FUN_MENU_NM_EN_US,
                FUN_MENU_NM_TH_TH, FUN_MENU_NM_JA_JP,
                DEFAULT_MENU_ID, IS_DISABLE, SORT_ORDER
            }

            public DBVarChar SysID;
            public DBVarChar FunMenu;

            public DBNVarChar FunMenuNMZHTW;
            public DBNVarChar FunMenuNMZHCN;
            public DBNVarChar FunMenuNMENUS;
            public DBNVarChar FunMenuNMTHTH;
            public DBNVarChar FunMenuNMJAJP;

            public DBVarChar DefaultMenuID;
            public DBChar IsDisable;
            public DBVarChar SortOrder;
        }

        public SystemFunMenuDetail SelectSystemFunMenuDetail(SystemFunMenuDetailPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID, FUN_MENU ", Environment.NewLine,
                "     , FUN_MENU_NM_ZH_TW, FUN_MENU_NM_ZH_CN, FUN_MENU_NM_EN_US ", Environment.NewLine,
                "     , FUN_MENU_NM_TH_TH, FUN_MENU_NM_JA_JP ", Environment.NewLine,
                "     , DEFAULT_MENU_ID, IS_DISABLE, SORT_ORDER ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN_MENU ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND FUN_MENU={FUN_MENU}; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemFunMenuDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunMenuDetailPara.Field.FUN_MENU, Value = para.FunMenu });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                SystemFunMenuDetail systemFunMenuDetail = new SystemFunMenuDetail()
                {
                    SysID = new DBVarChar(dataRow[SystemFunMenuDetail.DataField.SYS_ID.ToString()]),
                    FunMenu = new DBVarChar(dataRow[SystemFunMenuDetail.DataField.FUN_MENU.ToString()]),
                    FunMenuNMZHTW = new DBNVarChar(dataRow[SystemFunMenuDetail.DataField.FUN_MENU_NM_ZH_TW.ToString()]),
                    FunMenuNMZHCN = new DBNVarChar(dataRow[SystemFunMenuDetail.DataField.FUN_MENU_NM_ZH_CN.ToString()]),
                    FunMenuNMENUS = new DBNVarChar(dataRow[SystemFunMenuDetail.DataField.FUN_MENU_NM_EN_US.ToString()]),
                    FunMenuNMTHTH = new DBNVarChar(dataRow[SystemFunMenuDetail.DataField.FUN_MENU_NM_TH_TH.ToString()]),
                    FunMenuNMJAJP = new DBNVarChar(dataRow[SystemFunMenuDetail.DataField.FUN_MENU_NM_JA_JP.ToString()]),
                    DefaultMenuID = new DBVarChar(dataRow[SystemFunMenuDetail.DataField.DEFAULT_MENU_ID.ToString()]),
                    IsDisable = new DBChar(dataRow[SystemFunMenuDetail.DataField.IS_DISABLE.ToString()]),
                    SortOrder = new DBVarChar(dataRow[SystemFunMenuDetail.DataField.SORT_ORDER.ToString()])
                };
                return systemFunMenuDetail;
            }
            return null;
        }

        public enum EnumEditSystemFunMenuDetailResult
        {
            Success, Failure
        }

        public EnumEditSystemFunMenuDetailResult EditSystemFunMenuDetail(SystemFunMenuDetailPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE FROM SYS_SYSTEM_FUN_MENU ", Environment.NewLine,
                "        WHERE SYS_ID={SYS_ID} AND FUN_MENU={FUN_MENU}; ", Environment.NewLine,

                "        INSERT INTO SYS_SYSTEM_FUN_MENU VALUES ( ", Environment.NewLine,
                "            {SYS_ID}, {FUN_MENU} ", Environment.NewLine,
                "          , {FUN_MENU_NM_ZH_TW}, {FUN_MENU_NM_ZH_CN}, {FUN_MENU_NM_EN_US}, {FUN_MENU_NM_TH_TH}, {FUN_MENU_NM_JA_JP} ", Environment.NewLine,
                "          , {DEFAULT_MENU_ID}, {IS_DISABLE}, {SORT_ORDER} ", Environment.NewLine,
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
            dbParameters.Add(new DBParameter { Name = SystemFunMenuDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunMenuDetailPara.Field.FUN_MENU, Value = para.FunMenu });
            dbParameters.Add(new DBParameter { Name = SystemFunMenuDetailPara.Field.FUN_MENU_NM_ZH_TW, Value = para.FunMenuNMZHTW });
            dbParameters.Add(new DBParameter { Name = SystemFunMenuDetailPara.Field.FUN_MENU_NM_ZH_CN, Value = para.FunMenuNMZHCN });
            dbParameters.Add(new DBParameter { Name = SystemFunMenuDetailPara.Field.FUN_MENU_NM_EN_US, Value = para.FunMenuNMENUS });
            dbParameters.Add(new DBParameter { Name = SystemFunMenuDetailPara.Field.FUN_MENU_NM_TH_TH, Value = para.FunMenuNMTHTH });
            dbParameters.Add(new DBParameter { Name = SystemFunMenuDetailPara.Field.FUN_MENU_NM_JA_JP, Value = para.FunMenuNMJAJP });
            dbParameters.Add(new DBParameter { Name = SystemFunMenuDetailPara.Field.DEFAULT_MENU_ID, Value = para.DefaultMenuID });
            dbParameters.Add(new DBParameter { Name = SystemFunMenuDetailPara.Field.IS_DISABLE, Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = SystemFunMenuDetailPara.Field.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = SystemFunMenuDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditSystemFunMenuDetailResult.Success : EnumEditSystemFunMenuDetailResult.Failure;
        }

        public EnumDeleteSystemFunMenuDetailResult DeleteSystemFunMenuDetail(SystemFunMenuDetailPara para)
        {
            try
            {
                string commandText = string.Concat(new object[] 
                {
                    "DECLARE @RESULT CHAR(1) = 'N'; ", Environment.NewLine,
                    "IF NOT EXISTS (SELECT * FROM SYS_SYSTEM_FUN WHERE SYS_ID={SYS_ID} AND FUN_MENU={FUN_MENU}) ", Environment.NewLine,
                    "BEGIN ", Environment.NewLine,
                    "    DELETE FROM SYS_SYSTEM_FUN_MENU ", Environment.NewLine,
                    "    WHERE SYS_ID={SYS_ID} AND FUN_MENU={FUN_MENU}; ", Environment.NewLine,
                    "    SET @RESULT = 'Y'; ", Environment.NewLine,
                    "END; ", Environment.NewLine,
                    "SELECT @RESULT; "
                });

                List<DBParameter> dbParameters = new List<DBParameter>();
                dbParameters.Add(new DBParameter { Name = SystemFunMenuDetailPara.Field.SYS_ID, Value = para.SysID });
                dbParameters.Add(new DBParameter { Name = SystemFunMenuDetailPara.Field.FUN_MENU, Value = para.FunMenu });

                DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
                if (result.GetValue() == EnumYN.Y.ToString())
                {
                    return EnumDeleteSystemFunMenuDetailResult.Success;
                }
                else
                {
                    return EnumDeleteSystemFunMenuDetailResult.DataExist;
                }
            }
            catch
            {
                return EnumDeleteSystemFunMenuDetailResult.Failure;
            }
        }
    }
}