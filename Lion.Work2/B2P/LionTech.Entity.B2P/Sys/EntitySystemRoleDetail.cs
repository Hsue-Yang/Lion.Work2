using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemRoleDetail : EntitySys
    {
        public EntitySystemRoleDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemRoleDetailPara
        {
            public enum Field
            {
                SYS_ID, ROLE_ID,
                ROLE_NM_ZH_TW, ROLE_NM_ZH_CN, ROLE_NM_EN_US, ROLE_NM_TH_TH, ROLE_NM_JA_JP,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar RoleID;

            public DBNVarChar RoleNMZHTW;
            public DBNVarChar RoleNMZHCN;
            public DBNVarChar RoleNMENUS;
            public DBNVarChar RoleNMTHTH;
            public DBNVarChar RoleNMJAJP;

            public DBVarChar UpdUserID;
        }

        public class SystemRoleDetail : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, ROLE_ID,
                ROLE_NM_ZH_TW, ROLE_NM_ZH_CN, ROLE_NM_EN_US, ROLE_NM_TH_TH, ROLE_NM_JA_JP,
            }

            public DBVarChar SysID;
            public DBVarChar RoleID;

            public DBNVarChar RoleNMZHTW;
            public DBNVarChar RoleNMZHCN;
            public DBNVarChar RoleNMENUS;
            public DBNVarChar RoleNMTHTH;
            public DBNVarChar RoleNMJAJP;
        }

        public SystemRoleDetail SelectSystemRoleDetail(SystemRoleDetailPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID, ROLE_ID ", Environment.NewLine,
                "     , ROLE_NM_ZH_TW, ROLE_NM_ZH_CN, ROLE_NM_EN_US,  ROLE_NM_TH_TH, ROLE_NM_JA_JP", Environment.NewLine,
                "FROM SYS_SYSTEM_ROLE ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND ROLE_ID={ROLE_ID}; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemRoleDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemRoleDetailPara.Field.ROLE_ID, Value = para.RoleID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                SystemRoleDetail systemRoleDetail = new SystemRoleDetail()
                {
                    SysID = new DBVarChar(dataRow[SystemRoleDetail.DataField.SYS_ID.ToString()]),
                    RoleID = new DBVarChar(dataRow[SystemRoleDetail.DataField.ROLE_ID.ToString()]),
                    RoleNMZHTW = new DBNVarChar(dataRow[SystemRoleDetail.DataField.ROLE_NM_ZH_TW.ToString()]),
                    RoleNMZHCN = new DBNVarChar(dataRow[SystemRoleDetail.DataField.ROLE_NM_ZH_CN.ToString()]),
                    RoleNMENUS = new DBNVarChar(dataRow[SystemRoleDetail.DataField.ROLE_NM_EN_US.ToString()]),
                    RoleNMTHTH = new DBNVarChar(dataRow[SystemRoleDetail.DataField.ROLE_NM_TH_TH.ToString()]),
                    RoleNMJAJP = new DBNVarChar(dataRow[SystemRoleDetail.DataField.ROLE_NM_JA_JP.ToString()])
                };
                return systemRoleDetail;
            }
            return null;
        }

        public enum EnumEditSystemRoleDetailResult
        {
            Success, Failure
        }

        public EnumEditSystemRoleDetailResult EditSystemRoleDetail(SystemRoleDetailPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE FROM SYS_SYSTEM_ROLE ", Environment.NewLine,
                "        WHERE SYS_ID={SYS_ID} AND ROLE_ID={ROLE_ID}; ", Environment.NewLine,

                "        INSERT INTO SYS_SYSTEM_ROLE VALUES ( ", Environment.NewLine,
                "            {SYS_ID}, {ROLE_ID} ", Environment.NewLine,
                "          , {ROLE_NM_ZH_TW}, {ROLE_NM_ZH_CN}, {ROLE_NM_EN_US}, {ROLE_NM_TH_TH}, {ROLE_NM_JA_JP} ", Environment.NewLine,
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
            dbParameters.Add(new DBParameter { Name = SystemRoleDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemRoleDetailPara.Field.ROLE_ID, Value = para.RoleID });
            dbParameters.Add(new DBParameter { Name = SystemRoleDetailPara.Field.ROLE_NM_ZH_TW, Value = para.RoleNMZHTW });
            dbParameters.Add(new DBParameter { Name = SystemRoleDetailPara.Field.ROLE_NM_ZH_CN, Value = para.RoleNMZHCN });
            dbParameters.Add(new DBParameter { Name = SystemRoleDetailPara.Field.ROLE_NM_EN_US, Value = para.RoleNMENUS });
            dbParameters.Add(new DBParameter { Name = SystemRoleDetailPara.Field.ROLE_NM_TH_TH, Value = para.RoleNMTHTH });
            dbParameters.Add(new DBParameter { Name = SystemRoleDetailPara.Field.ROLE_NM_JA_JP, Value = para.RoleNMJAJP });
            dbParameters.Add(new DBParameter { Name = SystemRoleDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditSystemRoleDetailResult.Success : EnumEditSystemRoleDetailResult.Failure;
        }

        public enum EnumDeleteSystemRoleDetailResult
        {
            Success, Failure
        }

        public EnumDeleteSystemRoleDetailResult DeleteSystemRoleDetail(SystemRoleDetailPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE FROM SYS_SYSTEM_ROLE ", Environment.NewLine,
                "        WHERE SYS_ID={SYS_ID} AND ROLE_ID={ROLE_ID}; ", Environment.NewLine,

                "        DELETE FROM SYS_SYSTEM_ROLE_FUN ", Environment.NewLine,
                "        WHERE SYS_ID={SYS_ID} AND ROLE_ID={ROLE_ID}; ", Environment.NewLine,

                "        DELETE FROM SYS_USER_SYSTEM_ROLE ", Environment.NewLine,
                "        WHERE SYS_ID={SYS_ID} AND ROLE_ID={ROLE_ID}; ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = SystemRoleDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemRoleDetailPara.Field.ROLE_ID, Value = para.RoleID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumDeleteSystemRoleDetailResult.Success : EnumDeleteSystemRoleDetailResult.Failure;
        }
    }
}