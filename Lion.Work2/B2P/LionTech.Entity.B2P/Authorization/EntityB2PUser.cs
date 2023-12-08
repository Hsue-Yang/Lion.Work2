using System;
using System.Collections.Generic;
using System.Text;

namespace LionTech.Entity.B2P.Authorization
{
    public class EntityB2PUser : EntityAuthorization
    {
        public EntityB2PUser(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class B2PUserAccountPara
        {
            public enum ParaField
            {
                USER_ID, USER_NM, USER_PWD,
                COM_ID, UNIT_ID, UNIT_NM,
                USER_GENDER, USER_TITLE, USER_TEL1, USER_TEL2, USER_EMAIL, REMARK, 
                DEFAULT_SYS_ID, DEFAULT_PATH, IS_GRANTOR, IS_DISABLE,
                UPD_USER_ID
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UserPWD;

            public DBVarChar ComID;
            public DBVarChar UnitID;
            public DBNVarChar UnitNM;

            public DBChar UserGender;
            public DBNVarChar UserTitle;
            public DBVarChar UserTel1;
            public DBVarChar UserTel2;
            public DBVarChar UserEmail;
            public DBNVarChar Remark;
            public DBVarChar DefaultSysID;
            public DBNVarChar DefaultPath;
            public DBChar IsGrantor;
            public DBChar IsDisable;

            public DBVarChar UpdUserID;
        }

        public class SystemRolePara
        {
            public enum ParaField
            {
                USER_ID, SYS_ID, ROLE_ID,
                UPD_USER_ID
            }

            public DBVarChar UserID;
            public DBVarChar SysID;
            public DBVarChar RoleID;
            public DBVarChar UpdUserID;
        }

        public enum EnumCreateAccountResult
        {
            Success, Failure
        }

        public EnumCreateAccountResult CreateAccount(B2PUserAccountPara para, List<SystemRolePara> paraList)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            StringBuilder commandTextStringBuilder = new StringBuilder();
            string insertCommand;

            foreach (SystemRolePara systemRolePara in paraList)
            {
                insertCommand = string.Concat(new object[]
                {
                    "        INSERT INTO SYS_USER_SYSTEM_ROLE VALUES ({USER_ID}, {SYS_ID}, {ROLE_ID}, {UPD_USER_ID}, GETDATE()); ", Environment.NewLine
                });
                dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.USER_ID, Value = systemRolePara.UserID });
                dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.SYS_ID, Value = systemRolePara.SysID });
                dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.ROLE_ID, Value = systemRolePara.RoleID });
                dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, insertCommand, dbParameters));
                dbParameters.Clear();
            }

            string systemRoleCommand = string.Concat(new object[]
            {
                "        INSERT INTO SYS_USER_SYSTEM ", Environment.NewLine,
                "        SELECT DISTINCT USER_ID, SYS_ID, {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "        FROM SYS_USER_SYSTEM_ROLE ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID}; ", Environment.NewLine
            });

            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, systemRoleCommand, dbParameters));
            dbParameters.Clear();

            string functionCommand = string.Concat(new object[]
            {
                "INSERT INTO SYS_USER_FUN ", Environment.NewLine,
                "SELECT U.USER_ID ", Environment.NewLine,
                "     , F.SYS_ID, F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME ", Environment.NewLine,
                "     , 'N' AS IS_ASSIGN ", Environment.NewLine,
                "     , {USER_ID}, GETDATE() ", Environment.NewLine,
                "FROM SYS_USER_SYSTEM_ROLE U ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN S ON U.SYS_ID=S.SYS_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_ROLE_FUN R ON U.SYS_ID=R.SYS_ID AND U.ROLE_ID=R.ROLE_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_FUN F ON R.SYS_ID=F.SYS_ID AND R.FUN_CONTROLLER_ID=F.FUN_CONTROLLER_ID AND R.FUN_ACTION_NAME=F.FUN_ACTION_NAME ", Environment.NewLine,
                "LEFT JOIN ( ", Environment.NewLine,
                "    SELECT N.SYS_ID, N.FUN_MENU, N.FUN_CONTROLLER_ID, N.FUN_ACTION_NAME, M.DEFAULT_MENU_ID ", Environment.NewLine,
                "    FROM SYS_SYSTEM_MENU_FUN N ", Environment.NewLine,
                "    JOIN SYS_SYSTEM_FUN_MENU M ON N.FUN_MENU_SYS_ID=M.SYS_ID AND N.FUN_MENU=M.FUN_MENU ", Environment.NewLine,
                ") Z ON F.SYS_ID=Z.SYS_ID AND F.FUN_CONTROLLER_ID=Z.FUN_CONTROLLER_ID AND F.FUN_ACTION_NAME=Z.FUN_ACTION_NAME ", Environment.NewLine,
                "LEFT JOIN SYS_USER_FUN O ON U.USER_ID=O.USER_ID ", Environment.NewLine,
                "                        AND F.SYS_ID=O.SYS_ID ", Environment.NewLine,
                "                        AND F.FUN_CONTROLLER_ID=O.FUN_CONTROLLER_ID ", Environment.NewLine,
                "                        AND F.FUN_ACTION_NAME=O.FUN_ACTION_NAME ", Environment.NewLine,
                "WHERE U.USER_ID={USER_ID} AND S.IS_DISABLE='N' AND F.IS_DISABLE='N' AND Z.FUN_MENU IS NOT NULL ", Environment.NewLine,
                "  AND O.IS_ASSIGN IS NULL ", Environment.NewLine,
                "GROUP BY U.USER_ID, F.SYS_ID, F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME ", Environment.NewLine,
                "ORDER BY F.SYS_ID, F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME; ", Environment.NewLine
            });

            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.USER_ID, Value = para.UserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, functionCommand, dbParameters));
            dbParameters.Clear();

            string menuCommand = string.Concat(new object[]
            {
                "INSERT INTO SYS_USER_FUN_MENU ", Environment.NewLine,
                "SELECT DISTINCT U.USER_ID, Z.FUN_MENU_SYS_ID ", Environment.NewLine,
                "     , Z.FUN_MENU, Z.DEFAULT_MENU_ID, Z.SORT_ORDER ", Environment.NewLine,
                "     , {USER_ID}, GETDATE() ", Environment.NewLine,
                "FROM SYS_USER_FUN U ", Environment.NewLine,
                "JOIN ( ", Environment.NewLine,
                "    SELECT N.SYS_ID ", Environment.NewLine,
                "         , N.FUN_MENU_SYS_ID, N.FUN_MENU ", Environment.NewLine,
                "         , N.FUN_CONTROLLER_ID, N.FUN_ACTION_NAME ", Environment.NewLine,
                "         , M.DEFAULT_MENU_ID, M.SORT_ORDER ", Environment.NewLine,
                "    FROM SYS_SYSTEM_MENU_FUN N ", Environment.NewLine,
                "    JOIN SYS_SYSTEM_FUN_MENU M ON N.FUN_MENU_SYS_ID=M.SYS_ID AND N.FUN_MENU=M.FUN_MENU ", Environment.NewLine,
                ") Z ON U.SYS_ID=Z.SYS_ID AND U.FUN_CONTROLLER_ID=Z.FUN_CONTROLLER_ID AND U.FUN_ACTION_NAME=Z.FUN_ACTION_NAME ", Environment.NewLine,
                "LEFT JOIN SYS_USER_FUN_MENU M ON U.USER_ID=M.USER_ID AND Z.FUN_MENU_SYS_ID=M.SYS_ID AND Z.FUN_MENU=M.FUN_MENU ", Environment.NewLine,
                "WHERE U.USER_ID={USER_ID} AND M.USER_ID IS NULL AND M.SYS_ID IS NULL AND M.FUN_MENU IS NULL; ", Environment.NewLine,

                "DELETE SYS_USER_FUN_MENU ", Environment.NewLine,
                "FROM SYS_USER_FUN_MENU U ", Environment.NewLine,
                "LEFT JOIN (SELECT DISTINCT N.FUN_MENU_SYS_ID, N.FUN_MENU ", Environment.NewLine,
                "           FROM SYS_USER_FUN F ", Environment.NewLine,
                "           JOIN SYS_SYSTEM_MENU_FUN N ON F.SYS_ID=N.SYS_ID AND F.FUN_CONTROLLER_ID=N.FUN_CONTROLLER_ID AND F.FUN_ACTION_NAME=N.FUN_ACTION_NAME ", Environment.NewLine,
                "           WHERE F.USER_ID={USER_ID}) M ", Environment.NewLine,
                "ON U.SYS_ID=M.FUN_MENU_SYS_ID AND U.FUN_MENU=M.FUN_MENU ", Environment.NewLine,
                "WHERE U.USER_ID={USER_ID} AND M.FUN_MENU_SYS_ID IS NULL AND M.FUN_MENU IS NULL; ", Environment.NewLine
            });

            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.USER_ID, Value = para.UserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, menuCommand, dbParameters));
            dbParameters.Clear();

            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE FROM RAW_CM_USER WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                "        DELETE FROM RAW_CM_ORG_UNIT WHERE UNIT_ID={UNIT_ID}; ", Environment.NewLine,
                "        DELETE FROM SYS_USER_MAIN WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                "        DELETE FROM SYS_USER_DETAIL WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                "        DELETE FROM SYS_USER_SYSTEM WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                "        DELETE FROM SYS_USER_SYSTEM_ROLE WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                "        DELETE FROM SYS_USER_FUN WHERE USER_ID={USER_ID}; ", Environment.NewLine,

                "        INSERT INTO RAW_CM_USER VALUES ( ", Environment.NewLine,
                "            {USER_ID}, {USER_NM}, {COM_ID}, {UNIT_ID}, 'N', {UPD_USER_ID}, GETDATE(), NULL ", Environment.NewLine,
                "        ); ", Environment.NewLine,
                "        INSERT INTO RAW_CM_ORG_UNIT VALUES ( ", Environment.NewLine,
                "            {UNIT_ID}, {UNIT_NM}, 'N', {UPD_USER_ID}, GETDATE(), NULL ", Environment.NewLine,
                "        ); ", Environment.NewLine,
                "        INSERT INTO SYS_USER_MAIN VALUES ( ", Environment.NewLine,
                "            {USER_ID}, NULL, {IS_DISABLE}, {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "        ); ", Environment.NewLine,
                "        INSERT INTO SYS_USER_DETAIL VALUES ( ", Environment.NewLine,
                "            {USER_ID}, {USER_NM}, {USER_PWD}, {USER_GENDER} ", Environment.NewLine,
                "          , {USER_TITLE}, {USER_TEL1}, {USER_TEL2}, {USER_EMAIL}, {REMARK} ", Environment.NewLine,
                "          , {DEFAULT_SYS_ID}, {DEFAULT_PATH} ", Environment.NewLine,
                "          , {IS_GRANTOR}, NULL ", Environment.NewLine,
                "          , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "        ); ", Environment.NewLine,

                commandTextStringBuilder.ToString(),

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
            dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.USER_NM, Value = para.UserNM });
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.USER_PWD, Value = para.UserPWD });

            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.COM_ID, Value = para.ComID });
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.UNIT_ID, Value = para.UnitID });
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.UNIT_NM, Value = para.UnitNM });

            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.USER_GENDER, Value = para.UserGender });
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.USER_TITLE, Value = para.UserTitle });
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.USER_TEL1, Value = para.UserTel1 });
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.USER_TEL2, Value = para.UserTel2 });
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.USER_EMAIL, Value = para.UserEmail });
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.DEFAULT_SYS_ID, Value = para.DefaultSysID });
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.DEFAULT_PATH, Value = para.DefaultPath });
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.IS_GRANTOR, Value = para.IsGrantor });
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.IS_DISABLE, Value = para.IsDisable });

            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumCreateAccountResult.Success : EnumCreateAccountResult.Failure;
        }

        public enum EnumDeleteAccountResult
        {
            Success, Failure
        }

        public EnumDeleteAccountResult DeleteAccount(B2PUserAccountPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        UPDATE SYS_USER_MAIN SET ", Environment.NewLine,
                "            ROLE_GROUP_ID=NULL, IS_DISABLE='Y', UPD_USER_ID={UPD_USER_ID}, UPD_DT=GETDATE() ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID}; ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.UPD_USER_ID.ToString(), Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumDeleteAccountResult.Success : EnumDeleteAccountResult.Failure;
        }

        public enum EnumCheckAccountResult
        {
            Success, Failure
        }

        public EnumCheckAccountResult CheckAccount(B2PUserAccountPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "IF EXISTS (SELECT USER_ID FROM RAW_CM_USER WHERE USER_ID={USER_ID}) ", Environment.NewLine,
                "    SET @RESULT = 'Y'; ", Environment.NewLine,
                "ELSE ", Environment.NewLine,
                "    SET @RESULT = 'N'; ", Environment.NewLine,
                "; ", Environment.NewLine,
                "SELECT @RESULT; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = B2PUserAccountPara.ParaField.USER_ID, Value = para.UserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumCheckAccountResult.Success : EnumCheckAccountResult.Failure;
        }
    }
}