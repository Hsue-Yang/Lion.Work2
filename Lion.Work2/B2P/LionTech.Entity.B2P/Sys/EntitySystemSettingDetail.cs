using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemSettingDetail : EntitySys
    {
        public EntitySystemSettingDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemSettingDetailPara
        {
            public enum ParaField
            {
                USER_ID,

                ROLE_ID, 
                ROLE_NM_ZH_TW, ROLE_NM_ZH_CN, ROLE_NM_EN_US, ROLE_NM_TH_TH, ROLE_NM_JA_JP,

                SYS_ID, SYS_MAN_USER_ID,
                SYS_NM_ZH_TW, SYS_NM_ZH_CN, SYS_NM_EN_US, SYS_NM_TH_TH, SYS_NM_JA_JP,
                SYS_INDEX_PATH, SYS_ICON_PATH,
                SYS_KEY, EN_SYS_ID,
                IS_OUTSOURCING, IS_DISABLE, SORT_ORDER,

                UPD_USER_ID
            }

            public DBChar FirstEdit;
            public DBVarChar UserID;

            public DBVarChar RoleID;
            public DBNVarChar RoleNMZHTW;
            public DBNVarChar RoleNMZHCN;
            public DBNVarChar RoleNMENUS;
            public DBNVarChar RoleNMTHTH;
            public DBNVarChar RoleNMJAJP;

            public DBVarChar SysID;
            public DBVarChar SysMANUserID;
            public DBNVarChar SysNMZHTW;
            public DBNVarChar SysNMZHCN;
            public DBNVarChar SysNMENUS;
            public DBNVarChar SysNMTHTH;
            public DBNVarChar SysNMJAJP;
            public DBNVarChar SysIndexPath;
            public DBNVarChar SysIconPath;
            public DBChar SysKey;
            public DBChar ENSysID;
            public DBChar IsOutsourcing;
            public DBChar IsDisable;
            public DBVarChar SortOrder;

            public DBVarChar UpdUserID;
        }

        public class SystemSettingDetail : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_MAN_USER_ID,
                SYS_NM_ZH_TW, SYS_NM_ZH_CN, SYS_NM_EN_US, SYS_NM_TH_TH, SYS_NM_JA_JP,
                SYS_INDEX_PATH, SYS_ICON_PATH,
                SYS_KEY, EN_SYS_ID,
                IS_OUTSOURCING, IS_DISABLE, SORT_ORDER
            }

            public DBVarChar SysID;
            public DBVarChar SysMANUserID;
            public DBNVarChar SysNMZHTW;
            public DBNVarChar SysNMZHCN;
            public DBNVarChar SysNMENUS;
            public DBNVarChar SysNMTHTH;
            public DBNVarChar SysNMJAJP;
            public DBNVarChar SysIndexPath;
            public DBNVarChar SysIconPath;
            public DBChar SysKey;
            public DBChar ENSysID;
            public DBChar IsOutsourcing;
            public DBChar IsDisable;
            public DBVarChar SortOrder;
        }

        public SystemSettingDetail SelectSystemSettingDetail(SystemSettingDetailPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID, SYS_MAN_USER_ID ", Environment.NewLine,
                "     , SYS_NM_ZH_TW, SYS_NM_ZH_CN, SYS_NM_EN_US, SYS_NM_TH_TH, SYS_NM_JA_JP ", Environment.NewLine,
                "     , SYS_INDEX_PATH, SYS_ICON_PATH ", Environment.NewLine,
                "     , SYS_KEY, EN_SYS_ID ", Environment.NewLine,
                "     , IS_OUTSOURCING, IS_DISABLE, SORT_ORDER ", Environment.NewLine,
                "FROM SYS_SYSTEM_MAIN ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.SYS_ID, Value = para.SysID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                SystemSettingDetail systemSettingDetail = new SystemSettingDetail()
                {
                    SysID = new DBVarChar(dataRow[SystemSettingDetail.DataField.SYS_ID.ToString()]),
                    SysMANUserID = new DBVarChar(dataRow[SystemSettingDetail.DataField.SYS_MAN_USER_ID.ToString()]),
                    SysNMZHTW = new DBNVarChar(dataRow[SystemSettingDetail.DataField.SYS_NM_ZH_TW.ToString()]),
                    SysNMZHCN = new DBNVarChar(dataRow[SystemSettingDetail.DataField.SYS_NM_ZH_CN.ToString()]),
                    SysNMENUS = new DBNVarChar(dataRow[SystemSettingDetail.DataField.SYS_NM_EN_US.ToString()]),
                    SysNMTHTH = new DBNVarChar(dataRow[SystemSettingDetail.DataField.SYS_NM_TH_TH.ToString()]),
                    SysNMJAJP = new DBNVarChar(dataRow[SystemSettingDetail.DataField.SYS_NM_JA_JP.ToString()]),
                    SysIndexPath = new DBNVarChar(dataRow[SystemSettingDetail.DataField.SYS_INDEX_PATH.ToString()]),
                    SysIconPath = new DBNVarChar(dataRow[SystemSettingDetail.DataField.SYS_ICON_PATH.ToString()]),
                    SysKey = new DBChar(dataRow[SystemSettingDetail.DataField.SYS_KEY.ToString()]),
                    ENSysID = new DBChar(dataRow[SystemSettingDetail.DataField.EN_SYS_ID.ToString()]),
                    IsOutsourcing = new DBChar(dataRow[SystemSettingDetail.DataField.IS_OUTSOURCING.ToString()]),
                    IsDisable = new DBChar(dataRow[SystemSettingDetail.DataField.IS_DISABLE.ToString()]),
                    SortOrder = new DBVarChar(dataRow[SystemSettingDetail.DataField.SORT_ORDER.ToString()])
                };
                return systemSettingDetail;
            }
            return null;
        }

        public enum EnumSelectUserSystemRoleResult
        {
            Success, Failure
        }

        public class UserSystemRolePara
        {
            public enum ParaField
            {
                SYS_ID, USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar UserID;
        }

        public EnumSelectUserSystemRoleResult SelectUserSystemRole(UserSystemRolePara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @ROLE_CN INT = 0; ", Environment.NewLine,
                "IF (SELECT IS_OUTSOURCING FROM SYS_SYSTEM_MAIN WHERE IS_DISABLE='N' AND SYS_ID={SYS_ID}) = 'Y' ", Environment.NewLine,
                "BEGIN ", Environment.NewLine,
                "    SET @ROLE_CN = 1; ", Environment.NewLine,
                "END; ", Environment.NewLine,
                "ELSE ", Environment.NewLine,
                "BEGIN ", Environment.NewLine,
                "    SELECT @ROLE_CN=COUNT(R.ROLE_ID) ", Environment.NewLine,
                "    FROM SYS_USER_SYSTEM_ROLE R ", Environment.NewLine,
                "    JOIN SYS_SYSTEM_MAIN M ON R.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "    WHERE R.SYS_ID={SYS_ID} AND M.IS_DISABLE='N' ", Environment.NewLine,
                "      AND R.USER_ID={USER_ID} AND R.ROLE_ID<>'USER'; ", Environment.NewLine,
                "END; ", Environment.NewLine,
                "SELECT @ROLE_CN; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID, Value = para.UserID });

            DBInt result = new DBInt(base.ExecuteScalar(commandText, dbParameters));
            if (result.GetValue() > 0)
            {
                return EnumSelectUserSystemRoleResult.Success;
            }
            return EnumSelectUserSystemRoleResult.Failure;
        }

        public enum EnumEditSystemSettingDetailResult
        {
            Success, Failure
        }

        public EnumEditSystemSettingDetailResult EditSystemSettingDetail(SystemSettingDetailPara para)
        {
            string commandText = string.Empty;

            if (para.FirstEdit.GetValue() == EnumYN.Y.ToString())
            {
                commandText = string.Concat(new object[] 
                {
                    "INSERT INTO SYS_SYSTEM_MAIN VALUES ( ", Environment.NewLine,
                    "    {SYS_ID}, {SYS_MAN_USER_ID} ", Environment.NewLine,
                    "  , {SYS_NM_ZH_TW}, {SYS_NM_ZH_CN}, {SYS_NM_EN_US}, {SYS_NM_TH_TH}, {SYS_NM_JA_JP} ", Environment.NewLine,
                    "  , {SYS_INDEX_PATH}, {SYS_ICON_PATH} ", Environment.NewLine,
                    "  , {SYS_KEY}, {EN_SYS_ID} ", Environment.NewLine,
                    "  , {IS_OUTSOURCING}, {IS_DISABLE}, {SORT_ORDER} ", Environment.NewLine,
                    "  , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                    "); ", Environment.NewLine,

                    "INSERT INTO SYS_USER_SYSTEM VALUES ( ", Environment.NewLine,
                    "    {USER_ID}, {SYS_ID} ", Environment.NewLine,
                    "  , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                    "); ", Environment.NewLine
                });

                if (para.IsOutsourcing.GetValue() == EnumYN.N.ToString())
                {
                    commandText = string.Concat(new object[] 
                    {
                        commandText,

                        "INSERT INTO SYS_SYSTEM_ROLE VALUES ( ", Environment.NewLine,
                        "    {SYS_ID}, {ROLE_ID} ", Environment.NewLine,
                        "  , {ROLE_NM_ZH_TW}, {ROLE_NM_ZH_CN}, {ROLE_NM_EN_US}, {ROLE_NM_TH_TH}, {ROLE_NM_JA_JP} ", Environment.NewLine,
                        "  , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                        "); ", Environment.NewLine,

                        "INSERT INTO SYS_USER_SYSTEM_ROLE VALUES ( ", Environment.NewLine,
                        "    {USER_ID}, {SYS_ID}, {ROLE_ID} ", Environment.NewLine,
                        "  , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                        "); ", Environment.NewLine
                    });
                }
            }
            else
            {
                commandText = string.Concat(new object[] 
                {
                    "DECLARE @SYS_KEY CHAR(64); ", Environment.NewLine,
                    "SELECT @SYS_KEY=SYS_KEY FROM SYS_SYSTEM_MAIN WHERE SYS_ID={SYS_ID}; ", Environment.NewLine,

                    "DELETE FROM SYS_SYSTEM_MAIN WHERE SYS_ID={SYS_ID}; ", Environment.NewLine,

                    "INSERT INTO SYS_SYSTEM_MAIN VALUES ( ", Environment.NewLine,
                    "    {SYS_ID}, {SYS_MAN_USER_ID} ", Environment.NewLine,
                    "  , {SYS_NM_ZH_TW}, {SYS_NM_ZH_CN}, {SYS_NM_EN_US}, {SYS_NM_TH_TH}, {SYS_NM_JA_JP} ", Environment.NewLine,
                    "  , {SYS_INDEX_PATH}, {SYS_ICON_PATH} ", Environment.NewLine,
                    "  , @SYS_KEY, {EN_SYS_ID} ", Environment.NewLine,
                    "  , {IS_OUTSOURCING}, {IS_DISABLE}, {SORT_ORDER} ", Environment.NewLine,
                    "  , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                    "); "
                });
            }

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.USER_ID, Value = para.UserID });

            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.ROLE_ID, Value = para.RoleID });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.ROLE_NM_ZH_TW, Value = para.RoleNMZHTW });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.ROLE_NM_ZH_CN, Value = para.RoleNMZHCN });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.ROLE_NM_EN_US, Value = para.RoleNMENUS });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.ROLE_NM_TH_TH, Value = para.RoleNMTHTH });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.ROLE_NM_JA_JP, Value = para.RoleNMJAJP });

            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.SYS_MAN_USER_ID, Value = para.SysMANUserID });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.SYS_NM_ZH_TW, Value = para.SysNMZHTW });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.SYS_NM_ZH_CN, Value = para.SysNMZHCN });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.SYS_NM_EN_US, Value = para.SysNMENUS });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.SYS_NM_TH_TH, Value = para.SysNMTHTH });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.SYS_NM_JA_JP, Value = para.SysNMJAJP });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.SYS_INDEX_PATH, Value = para.SysIndexPath });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.SYS_ICON_PATH, Value = para.SysIconPath });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.SYS_KEY, Value = para.SysKey });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.EN_SYS_ID, Value = para.ENSysID });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.IS_OUTSOURCING, Value = para.IsOutsourcing });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.IS_DISABLE, Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.SORT_ORDER, Value = para.SortOrder });

            dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,
                commandText, Environment.NewLine,
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
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditSystemSettingDetailResult.Success : EnumEditSystemSettingDetailResult.Failure;
        }

        public enum EnumDeleteSystemSettingDetailResult
        {
            Success, Failure, DataExist
        }

        public EnumDeleteSystemSettingDetailResult DeleteSystemSettingDetail(SystemSettingDetailPara para)
        {
            try
            {
                string commandText = string.Concat(new object[] 
                {
                    "DECLARE @RESULT CHAR(1) = NULL; ", Environment.NewLine,
                    "IF NOT EXISTS (SELECT * FROM SYS_SYSTEM_EVENT_GROUP WHERE SYS_ID={SYS_ID}) AND ", Environment.NewLine,
                    "   NOT EXISTS (SELECT * FROM SYS_SYSTEM_API_GROUP WHERE SYS_ID={SYS_ID}) AND ", Environment.NewLine,
                    "   NOT EXISTS (SELECT * FROM SYS_SYSTEM_FUN_GROUP WHERE SYS_ID={SYS_ID}) AND ", Environment.NewLine,
                    "   NOT EXISTS (SELECT * FROM SYS_SYSTEM_FUN_MENU WHERE SYS_ID={SYS_ID}) AND ", Environment.NewLine,
                    "   NOT EXISTS (SELECT * FROM SYS_SYSTEM_ROLE WHERE SYS_ID={SYS_ID}) ", Environment.NewLine,
                    "BEGIN ", Environment.NewLine,
                    "    BEGIN TRANSACTION ", Environment.NewLine,
                    "        BEGIN TRY ", Environment.NewLine,
                    "            DELETE FROM SYS_SYSTEM_MAIN WHERE SYS_ID={SYS_ID}; ", Environment.NewLine,
                    "            DELETE FROM SYS_SYSTEM_IP WHERE SYS_ID={SYS_ID}; ", Environment.NewLine,
                    "            DELETE FROM SYS_USER_SYSTEM WHERE SYS_ID={SYS_ID}; ", Environment.NewLine,
                    "            SET @RESULT = 'Y'; ", Environment.NewLine,
                    "            COMMIT; ", Environment.NewLine,
                    "        END TRY ", Environment.NewLine,
                    "        BEGIN CATCH ", Environment.NewLine,
                    "            SET @RESULT = 'N'; ", Environment.NewLine,
                    "            ROLLBACK TRANSACTION; ", Environment.NewLine,
                    "        END CATCH ", Environment.NewLine,
                    "    ; ", Environment.NewLine,
                    "END; ", Environment.NewLine,
                    "SELECT @RESULT; "
                });

                List<DBParameter> dbParameters = new List<DBParameter>();
                dbParameters.Add(new DBParameter { Name = SystemSettingDetailPara.ParaField.SYS_ID, Value = para.SysID });

                DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
                if (result.IsNull())
                {
                    return EnumDeleteSystemSettingDetailResult.DataExist;
                }
                else if (result.GetValue() == EnumYN.Y.ToString())
                {
                    return EnumDeleteSystemSettingDetailResult.Success;
                }
                else
                {
                    return EnumDeleteSystemSettingDetailResult.Failure;
                }
            }
            catch
            {
                return EnumDeleteSystemSettingDetailResult.Failure;
            }
        }
    }
}