using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.SystemSetting
{
    public class EntitySystemRoleFun : EntitySystemSetting
    {
        public EntitySystemRoleFun(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemRoleFunPara
        {
            public enum ParaField
            {
                SYS_ID,
                FUN_CONTROLLER_ID,
                FUN_ACTION_NAME,
                ROLE_ID,
                ROLE_ID_LIST,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar RoleID;
            public List<DBVarChar> RoleIDList;
            public List<SystemRoleFun> SystemRoleFunList;
            public DBVarChar UpdUserID;
        }

        public class SystemRoleFun
        {
            public DBVarChar RoleID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionNM;
        }

        public enum EnumEditSystemRoleFunResult
        {
            Success,
            Failure
        }

        public EnumEditSystemRoleFunResult EditSystemRoleFun(SystemRoleFunPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "DECLARE @ERROR_LINE INT;",
                    "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",

                    "DECLARE @SYSID VARCHAR(12) = {SYS_ID};",
                    "DECLARE @UPDUSERID VARCHAR(50) = {UPD_USER_ID};",

                    "DECLARE @SystemRoleFun TABLE (",
                    "     ROLE_ID VARCHAR(20)",
                    "   , FUN_CONTROLLER_ID VARCHAR(20)",
                    "   , FUN_ACTION_NAME VARCHAR(50));",

                    "BEGIN TRANSACTION",
                    "    BEGIN TRY"
                }));

            foreach (var roleInfo in para.SystemRoleFunList)
            {
                commandText.AppendLine(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                        " INSERT INTO @SystemRoleFun VALUES ({ROLE_ID}, {FUN_CONTROLLER_ID}, {FUN_ACTION_NAME});"
                    ),
                    new List<DBParameter>
                    {
                        new DBParameter { Name = SystemRoleFunPara.ParaField.ROLE_ID, Value = roleInfo.RoleID },
                        new DBParameter { Name = SystemRoleFunPara.ParaField.FUN_CONTROLLER_ID, Value = roleInfo.FunControllerID },
                        new DBParameter { Name = SystemRoleFunPara.ParaField.FUN_ACTION_NAME, Value = roleInfo.FunActionNM }
                    }));
            }

            commandText.AppendLine(string.Join(Environment.NewLine,
                new object[]
                {
                    "        DELETE SYS_SYSTEM_ROLE_FUN",
                    "         WHERE SYS_ID = @SYSID ",
                    "           AND ROLE_ID IN ({ROLE_ID_LIST});",

                    "        INSERT INTO SYS_SYSTEM_ROLE_FUN ",
                    "        SELECT DISTINCT @SYSID, ROLE_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME, @UPDUSERID, GETDATE() ",
                    "          FROM @SystemRoleFun",

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
                }));

            dbParameters.Add(new DBParameter { Name = SystemRoleFunPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunPara.ParaField.ROLE_ID_LIST, Value = para.RoleIDList });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditSystemRoleFunResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }

        public class SystemFun : DBTableRow
        {
            public DBVarChar ControllerID;
            public DBVarChar ActionName;

            public DBNVarChar ActionNMzhTW;
            public DBNVarChar ActionNMzhCN;
            public DBNVarChar ActionNMenUS;
            public DBNVarChar ActionNMthTH;
            public DBNVarChar ActionNMjaJP;
            public DBNVarChar ActionNMkoKR;
        }

        public List<SystemFun> SelectSystemRoleFunList(SystemRoleFunPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();

            var commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT F.FUN_CONTROLLER_ID AS ControllerID",
                "     , F.FUN_ACTION_NAME AS ActionName",
                "     , F.FUN_NM_ZH_TW AS ActionNMzhTW",
                "     , F.FUN_NM_ZH_CN AS ActionNMzhCN",
                "     , F.FUN_NM_EN_US AS ActionNMenUS",
                "     , F.FUN_NM_TH_TH AS ActionNMthTH",
                "     , F.FUN_NM_JA_JP AS ActionNMjaJP",
                "     , F.FUN_NM_KO_KR AS ActionNMkoKR",
                "  FROM SYS_SYSTEM_ROLE_FUN R",
                "  JOIN SYS_SYSTEM_FUN F",
                "    ON R.SYS_ID = F.SYS_ID",
                "   AND R.FUN_CONTROLLER_ID = F.FUN_CONTROLLER_ID",
                "   AND R.FUN_ACTION_NAME = F.FUN_ACTION_NAME",
                " WHERE R.SYS_ID = {SYS_ID}",
                "   AND R.ROLE_ID = {ROLE_ID} "
            });

            dbParameters.Add(new DBParameter { Name = SystemRoleFunPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunPara.ParaField.ROLE_ID.ToString(), Value = para.RoleID });
            return GetEntityList<SystemFun>(commandText, dbParameters);
        }
    }
}
