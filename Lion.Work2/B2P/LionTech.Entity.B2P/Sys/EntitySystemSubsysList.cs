using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemSubsysList : EntitySys
    {
        public EntitySystemSubsysList(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemSubsysPara
        {
            public enum ParaField
            {
                SYS_ID, PARENT_SYS_ID,
                SYS_NM_ZH_TW, SYS_NM_ZH_CN, SYS_NM_EN_US, SYS_NM_TH_TH, SYS_NM_JA_JP,
                SORT_ORDER,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar ParentSysID;
            public DBNVarChar SysNMZHTW;
            public DBNVarChar SysNMZHCN;
            public DBNVarChar SysNMENUS;
            public DBNVarChar SysNMTHTH;
            public DBNVarChar SysNMJAJP;
            public DBVarChar SortOrder;
            public DBVarChar UpdUserID;
        }

        public class SystemSubsys : DBTableRow
        {
            public enum DataField
            {
                SYS_ID,
                SYS_NM_ZH_TW, SYS_NM_ZH_CN, SYS_NM_EN_US, SYS_NM_TH_TH, SYS_NM_JA_JP,
                SORT_ORDER,
                UPD_USER_ID, UPD_DT, UPD_USER_NM
            }

            public DBVarChar SubSysID;
            public DBNVarChar SysNMZHTW;
            public DBNVarChar SysNMZHCN;
            public DBNVarChar SysNMENUS;
            public DBNVarChar SysNMTHTH;
            public DBNVarChar SysNMJAJP;
            public DBVarChar SortOrder;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<SystemSubsys> SelectSystemSub(SystemSubsysPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID ", Environment.NewLine,
                "     , SYS_NM_ZH_TW, SYS_NM_ZH_CN, SYS_NM_EN_US, SYS_NM_TH_TH, SYS_NM_JA_JP ", Environment.NewLine,
                "     , SORT_ORDER ", Environment.NewLine,
                "     , UPD_USER_ID, dbo.FN_GET_USER_NM(UPD_USER_ID) AS UPD_USER_NM, UPD_DT ", Environment.NewLine,
                "FROM SYS_SYSTEM_SUB WHERE PARENT_SYS_ID={PARENT_SYS_ID} "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.PARENT_SYS_ID.ToString(), Value = para.ParentSysID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemSubsys> systemSubsysList = new List<SystemSubsys>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemSubsys systemSubsys = new SystemSubsys()
                    {
                        SubSysID = new DBVarChar(dataRow[SystemSubsys.DataField.SYS_ID.ToString()]),
                        SysNMZHTW = new DBNVarChar(dataRow[SystemSubsys.DataField.SYS_NM_ZH_TW.ToString()]),
                        SysNMZHCN = new DBNVarChar(dataRow[SystemSubsys.DataField.SYS_NM_ZH_CN.ToString()]),
                        SysNMENUS = new DBNVarChar(dataRow[SystemSubsys.DataField.SYS_NM_EN_US.ToString()]),
                        SysNMTHTH = new DBNVarChar(dataRow[SystemSubsys.DataField.SYS_NM_TH_TH.ToString()]),
                        SysNMJAJP = new DBNVarChar(dataRow[SystemSubsys.DataField.SYS_NM_JA_JP.ToString()]),
                        SortOrder = new DBVarChar(dataRow[SystemSubsys.DataField.SORT_ORDER.ToString()]),
                        UpdUserID = new DBVarChar(dataRow[SystemSubsys.DataField.UPD_USER_ID.ToString()]),
                        UpdUserNM = new DBNVarChar(dataRow[SystemSubsys.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[SystemSubsys.DataField.UPD_DT.ToString()])
                    };
                    systemSubsysList.Add(systemSubsys);
                }
                return systemSubsysList;
            }
            return null;
        }

        public enum EnumValidationSubsystemExistResult
        {
            Success, Failure
        }

        public EnumValidationSubsystemExistResult ValidationSubsystemExist(SystemSubsysPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                
                "IF NOT EXISTS (SELECT * FROM SYS_SYSTEM_SUB WHERE SYS_ID={SYS_ID}) ", Environment.NewLine,
                "BEGIN ", Environment.NewLine,
                "    SET @RESULT = 'Y'; ", Environment.NewLine,
                "END; ", Environment.NewLine,

                "SELECT @RESULT; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.SYS_ID.ToString(), Value = para.SysID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumValidationSubsystemExistResult.Success : EnumValidationSubsystemExistResult.Failure;
        }

        public enum EnumInsertSystemSubResult
        {
            Success, Failure
        }

        public EnumInsertSystemSubResult InsertSystemSub(SystemSubsysPara para)
        {
            string commandText = string.Concat(new object[] 
            { 
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "       IF NOT EXISTS (SELECT * FROM SYS_SYSTEM_SUB WHERE SYS_ID={SYS_ID}) ", Environment.NewLine,
                "       BEGIN ", Environment.NewLine,
                "           INSERT INTO SYS_SYSTEM_SUB(SYS_ID, PARENT_SYS_ID, SYS_NM_ZH_TW, SYS_NM_ZH_CN, SYS_NM_EN_US, SYS_NM_TH_TH, SYS_NM_JA_JP, SORT_ORDER, UPD_USER_ID, UPD_DT)", Environment.NewLine,
                "           VALUES ({SYS_ID}, {PARENT_SYS_ID}, {SYS_NM_ZH_TW}, {SYS_NM_ZH_CN}, {SYS_NM_EN_US}, {SYS_NM_TH_TH}, {SYS_NM_JA_JP}, {SORT_ORDER}, {UPD_USER_ID}, GETDATE()); ", Environment.NewLine,
                "           SET @RESULT = 'Y'; ", Environment.NewLine,
                "       END; ", Environment.NewLine,

                "       COMMIT; ", Environment.NewLine,
                "    END TRY ", Environment.NewLine,
                "    BEGIN CATCH ", Environment.NewLine,
                "       SET @RESULT = 'N'; ", Environment.NewLine,
                "       ROLLBACK TRANSACTION; ", Environment.NewLine,
                "    END CATCH ", Environment.NewLine,
                "; ", Environment.NewLine,
                "SELECT @RESULT; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.PARENT_SYS_ID, Value = para.ParentSysID });
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.SYS_NM_ZH_TW, Value = para.SysNMZHTW });
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.SYS_NM_ZH_CN, Value = para.SysNMZHCN });
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.SYS_NM_EN_US, Value = para.SysNMENUS });
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.SYS_NM_TH_TH, Value = para.SysNMTHTH });
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.SYS_NM_JA_JP, Value = para.SysNMJAJP });
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumInsertSystemSubResult.Success : EnumInsertSystemSubResult.Failure;
        }

        public enum EnumUpdateSystemSubResult
        {
            Success, Failure
        }

        public EnumUpdateSystemSubResult UpdateSystemSub(SystemSubsysPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        UPDATE SYS_SYSTEM_SUB SET ", Environment.NewLine,
                "               SYS_NM_ZH_TW={SYS_NM_ZH_TW} ", Environment.NewLine,
                "             , SYS_NM_ZH_CN={SYS_NM_ZH_CN} ", Environment.NewLine,
                "             , SYS_NM_EN_US={SYS_NM_EN_US} ", Environment.NewLine,
                "             , SYS_NM_TH_TH={SYS_NM_TH_TH} ", Environment.NewLine,
                "             , SYS_NM_JA_JP={SYS_NM_JA_JP} ", Environment.NewLine,
                "             , SORT_ORDER={SORT_ORDER} ", Environment.NewLine,
                "             , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "             , UPD_DT=GETDATE() ", Environment.NewLine,
                "        WHERE SYS_ID={SYS_ID}; ", Environment.NewLine,

                "        SET @RESULT = 'Y'; ", Environment.NewLine,
                "        COMMIT; ", Environment.NewLine,
                "    END TRY ", Environment.NewLine,
                "    BEGIN CATCH ", Environment.NewLine,
                "        SET @RESULT = 'N'; ", Environment.NewLine,
                "        ROLLBACK TRANSACTION; ", Environment.NewLine,
                "    END CATCH ", Environment.NewLine,
                "; ", Environment.NewLine,
                "SELECT @RESULT; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.SYS_NM_ZH_TW, Value = para.SysNMZHTW });
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.SYS_NM_ZH_CN, Value = para.SysNMZHCN });
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.SYS_NM_EN_US, Value = para.SysNMENUS });
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.SYS_NM_TH_TH, Value = para.SysNMTHTH });
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.SYS_NM_JA_JP, Value = para.SysNMJAJP });
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumUpdateSystemSubResult.Success : EnumUpdateSystemSubResult.Failure;
        }

        public enum EnumDeleteSystemSubResult
        {
            Success, Failure
        }

        public EnumDeleteSystemSubResult DeleteSystemSub(SystemSubsysPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE SYS_SYSTEM_SUB WHERE SYS_ID={SYS_ID}; ", Environment.NewLine,

                "        SET @RESULT = 'Y'; ", Environment.NewLine,
                "        COMMIT; ", Environment.NewLine,
                "    END TRY ", Environment.NewLine,
                "    BEGIN CATCH ", Environment.NewLine,
                "        SET @RESULT = 'N'; ", Environment.NewLine,
                "        ROLLBACK TRANSACTION; ", Environment.NewLine,
                "    END CATCH ", Environment.NewLine,
                "; ", Environment.NewLine,
                "SELECT @RESULT; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemSubsysPara.ParaField.SYS_ID, Value = para.SysID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumDeleteSystemSubResult.Success : EnumDeleteSystemSubResult.Failure;
        }
    }
}