using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.SystemSetting
{
    public class EntitySystemRole : EntitySystemSetting
    {
        public EntitySystemRole(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemRolePara
        {
            public enum ParaField
            {
                SYS_ID, ROLE_ID,
                ROLE_NM_ZH_TW, ROLE_NM_ZH_CN, ROLE_NM_EN_US, ROLE_NM_TH_TH, ROLE_NM_JA_JP, ROLE_NM_KO_KR,
                ROLE_CATEGORY_ID,

                UPD_USER_ID
            }

            public DBVarChar SysID;

            public DBVarChar RoleID;
            public DBNVarChar RoleNMzhTW;
            public DBNVarChar RoleNMzhCN;
            public DBNVarChar RoleNMenUS;
            public DBNVarChar RoleNMthTH;
            public DBNVarChar RoleNMjaJP;
            public DBNVarChar RoleNMkoKR;
            public DBVarChar RoleCategoryID;
            public DBVarChar UpdUserID;
        }

        public class SystemRole : DBTableRow
        {
            public DBVarChar RoleID;
            public DBNVarChar RoleNMzhTW;
            public DBNVarChar RoleNMzhCN;
            public DBNVarChar RoleNMenUS;
            public DBNVarChar RoleNMthTH;
            public DBNVarChar RoleNMjaJP;
            public DBNVarChar RoleNMkoKR;
            public DBVarChar RoleCategoryID;
        }

        public List<SystemRole> SelectSystemRoleList(SystemRolePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();

            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT ROLE_ID AS RoleID",
                "     , ROLE_NM_ZH_TW AS RoleNMzhTW",
                "     , ROLE_NM_ZH_CN AS RoleNMzhCN",
                "     , ROLE_NM_EN_US AS RoleNMenUS",
                "     , ROLE_NM_TH_TH AS RoleNMthTH",
                "     , ROLE_NM_JA_JP AS RoleNMjaJP",
                "     , ROLE_NM_KO_KR AS RoleNMkoKR",
                "     , ROLE_CATEGORY_ID AS RoleCategoryID",
                "  FROM SYS_SYSTEM_ROLE R ",
                " WHERE R.SYS_ID = {SYS_ID}"
            }));

            if (para.RoleCategoryID.IsNull() == false)
            {
                commandText.AppendLine(" AND R.ROLE_CATEGORY_ID = {ROLE_CATEGORY_ID} ");
                dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.ROLE_CATEGORY_ID.ToString(), Value = para.RoleCategoryID });
            }

            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            return GetEntityList<SystemRole>(commandText.ToString(), dbParameters);
        }

        public enum EnumEditSystemRoleDetailResult
        {
            Success, Failure
        }

        public EnumEditSystemRoleDetailResult EditSystemRoleDetail(SystemRolePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "DECLARE @ERROR_LINE INT;",
                    "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",

                    "BEGIN TRANSACTION",
                    "    BEGIN TRY",

                    "        DELETE FROM SYS_SYSTEM_ROLE ", 
                    "        WHERE SYS_ID={SYS_ID} AND ROLE_ID={ROLE_ID}; ", 

                    "        INSERT INTO SYS_SYSTEM_ROLE VALUES ( ",
                    "            {SYS_ID}, {ROLE_CATEGORY_ID}, {ROLE_ID} ",
                    "          , {ROLE_NM_ZH_TW}, {ROLE_NM_ZH_CN}, {ROLE_NM_EN_US}, {ROLE_NM_TH_TH}, {ROLE_NM_JA_JP}, {ROLE_NM_KO_KR} ", 
                    "          , 'N', {UPD_USER_ID}, GETDATE() ", 
                    "        ); ", 

                    "        SET @RESULT = 'Y';",
                    "        COMMIT;",
                    "    END TRY",
                    "    BEGIN CATCH",
                    "        SET @RESULT = 'N';",
                    "        SET @ERROR_LINE = ERROR_LINE();",
                    "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                    "        ROLLBACK TRANSACTION;",
                    "    END CATCH;",
                    "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                });

            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.ROLE_ID, Value = para.RoleID });
            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.ROLE_NM_ZH_TW, Value = para.RoleNMzhTW });
            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.ROLE_NM_ZH_CN, Value = para.RoleNMzhCN });
            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.ROLE_NM_EN_US, Value = para.RoleNMenUS });
            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.ROLE_NM_TH_TH, Value = para.RoleNMthTH });
            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.ROLE_NM_JA_JP, Value = para.RoleNMjaJP });
            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.ROLE_NM_KO_KR, Value = para.RoleNMkoKR });
            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.ROLE_CATEGORY_ID, Value = para.RoleCategoryID });
            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditSystemRoleDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }

        public EnumEditSystemRoleDetailResult DeleteSystemRoleDetail(SystemRolePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "DECLARE @ERROR_LINE INT;",
                    "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",

                    "BEGIN TRANSACTION",
                    "    BEGIN TRY",

                    "        DELETE FROM SYS_SYSTEM_ROLE ",
                    "        WHERE SYS_ID={SYS_ID} AND ROLE_ID={ROLE_ID}; ",

                    "        SET @RESULT = 'Y';",
                    "        COMMIT;",
                    "    END TRY",
                    "    BEGIN CATCH",
                    "        SET @RESULT = 'N';",
                    "        SET @ERROR_LINE = ERROR_LINE();",
                    "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                    "        ROLLBACK TRANSACTION;",
                    "    END CATCH;",
                    "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                });

            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.ROLE_ID, Value = para.RoleID });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditSystemRoleDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
    }
}
