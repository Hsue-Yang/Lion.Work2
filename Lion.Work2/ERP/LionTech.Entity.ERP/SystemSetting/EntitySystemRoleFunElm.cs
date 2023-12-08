using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.SystemSetting
{
    public class EntitySystemRoleFunElm : EntitySystemSetting
    {
        public EntitySystemRoleFunElm(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class EditSystemRoleElmListPara
        {
            public enum ParaField
            {
                SYS_ID,
                ROLE_ID,
                CONTROLLER_ID,
                ACTION_NAME,
                ELM_ID,
                DISPLAY_STS,
                UPD_USER,
                UPD_DATE
            }

            public DBVarChar SysID;
            public DBVarChar RoleID;
            public List<SystemRoleElm> SystemRoleElmList;
            public DBVarChar UpdUserID;
        }

        public class SystemRoleElm
        {
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
            public DBVarChar ElmID;
            public DBTinyInt DisplaySts;
        }
        public enum EnumEditSystemRoleElmListResult
        {
            Success,
            Failure
        }
        public EnumEditSystemRoleElmListResult EditSystemRoleFunList(EditSystemRoleElmListPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            List<string> insertList = new List<string>();

            foreach (SystemRoleElm elm in para.SystemRoleElmList)
            {
                insertList.Add(GetCommandText(ProviderName, string.Join(Environment.NewLine, new object[]
                {
                    "INSERT INTO SYS_SYSTEM_ROLE_FUN_ELM",
                    "     ( SYS_ID",
                    "     , ROLE_ID",
                    "     , FUN_CONTROLLER_ID",
                    "     , FUN_ACTION_NAME",
                    "     , ELM_ID",
                    "     , DISPLAY_STS",
                    "     , UPD_USER_ID",
                    "     , UPD_DT",
                    "     )",
                    "VALUES",
                    "     ( @SYS_ID",
                    "     , @ROLE_ID",
                    "     , {CONTROLLER_ID}",
                    "     , {ACTION_NAME}",
                    "     , {ELM_ID}",
                    "     , {DISPLAY_STS}",
                    "     , {UPD_USER}",
                    "     , GETDATE()",
                    "     );",
                }), new List<DBParameter>
                {
                    new DBParameter { Name = EditSystemRoleElmListPara.ParaField.ACTION_NAME, Value = elm.FunActionName },
                    new DBParameter { Name = EditSystemRoleElmListPara.ParaField.CONTROLLER_ID, Value = elm.FunControllerID },
                    new DBParameter { Name = EditSystemRoleElmListPara.ParaField.ELM_ID, Value = elm.ElmID },
                    new DBParameter { Name = EditSystemRoleElmListPara.ParaField.DISPLAY_STS, Value = elm.DisplaySts },
                    new DBParameter { Name = EditSystemRoleElmListPara.ParaField.UPD_USER, Value = para.UpdUserID }
                }));
            }

            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1) = 'N';",
                "DECLARE @ERROR_LINE INT;",
                "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                "DECLARE @SYS_ID VARCHAR(12) = {SYS_ID};",
                "DECLARE @ROLE_ID VARCHAR(20) = {ROLE_ID};",
                "BEGIN TRANSACTION",
                "    BEGIN TRY",
                "        DELETE SYS_SYSTEM_ROLE_FUN_ELM ",
                "         WHERE SYS_ID = @SYS_ID",
                "           AND ROLE_ID = @ROLE_ID",

                string.Join(Environment.NewLine, insertList),

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

            dbParameters.Add(new DBParameter { Name = EditSystemRoleElmListPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = EditSystemRoleElmListPara.ParaField.ROLE_ID, Value = para.RoleID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditSystemRoleElmListResult.Success;
            }
            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
    }
}
