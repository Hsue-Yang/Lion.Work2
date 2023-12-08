using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Authorization
{
    public class EntityERPGenerateUserMenu : EntityAuthorization
    {
        public EntityERPGenerateUserMenu(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class EditUserFunInfoPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public enum EnumEditUserFunInfoResult
        {
            Success,
            Failure
        }

        public EnumEditUserFunInfoResult EditUserFunInfo(EditUserFunInfoPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1) = 'N';",
                "DECLARE @ERROR_LINE INT;",
                "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                "BEGIN TRANSACTION",
                "    BEGIN TRY",
                "        DELETE FROM SYS_USER_FUN",
                "         WHERE USER_ID = {USER_ID}",
                "           AND IS_ASSIGN = 'N';",
                         
                "        INSERT INTO SYS_USER_FUN",
                "        SELECT U.USER_ID",
                "             , F.SYS_ID",
                "             , F.FUN_CONTROLLER_ID",
                "             , F.FUN_ACTION_NAME",
                "             , 'N' AS IS_ASSIGN",
                "             , {USER_ID}",
                "             , GETDATE()",
                "          FROM SYS_USER_SYSTEM_ROLE U",
                "          JOIN SYS_SYSTEM_MAIN S",
                "            ON U.SYS_ID = S.SYS_ID",
                "          JOIN SYS_SYSTEM_ROLE_FUN R",
                "            ON U.SYS_ID = R.SYS_ID",
                "           AND U.ROLE_ID = R.ROLE_ID",
                "          JOIN SYS_SYSTEM_FUN F",
                "            ON R.SYS_ID = F.SYS_ID",
                "           AND R.FUN_CONTROLLER_ID = F.FUN_CONTROLLER_ID",
                "           AND R.FUN_ACTION_NAME = F.FUN_ACTION_NAME",
                "          LEFT JOIN (",
                "                        SELECT N.SYS_ID",
                "                             , N.FUN_MENU",
                "                             , N.FUN_CONTROLLER_ID",
                "                             , N.FUN_ACTION_NAME",
                "                             , M.DEFAULT_MENU_ID",
                "                          FROM SYS_SYSTEM_MENU_FUN N",
                "                          JOIN SYS_SYSTEM_FUN_MENU M",
                "                            ON N.FUN_MENU_SYS_ID = M.SYS_ID",
                "                           AND N.FUN_MENU = M.FUN_MENU",
                "                    ) Z",
                "                 ON F.SYS_ID = Z.SYS_ID",
                "                AND F.FUN_CONTROLLER_ID = Z.FUN_CONTROLLER_ID",
                "                AND F.FUN_ACTION_NAME = Z.FUN_ACTION_NAME",
                "          LEFT JOIN SYS_USER_FUN O",
                "            ON U.USER_ID = O.USER_ID",
                "           AND F.SYS_ID = O.SYS_ID",
                "           AND F.FUN_CONTROLLER_ID = O.FUN_CONTROLLER_ID",
                "           AND F.FUN_ACTION_NAME = O.FUN_ACTION_NAME",
                "         WHERE S.IS_DISABLE='N'",
                "           AND F.IS_DISABLE = 'N'",
                "           AND Z.FUN_MENU IS NOT NULL",
                "           AND O.IS_ASSIGN IS NULL",
                "         GROUP BY U.USER_ID, F.SYS_ID, F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME",
                "         ORDER BY U.USER_ID, F.SYS_ID, F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME;",

                "        INSERT INTO SYS_USER_FUN_MENU",
                "        SELECT DISTINCT U.USER_ID",
                "             , Z.FUN_MENU_SYS_ID",
                "             , Z.FUN_MENU",
                "             , Z.DEFAULT_MENU_ID",
                "             , Z.SORT_ORDER",
                "             , {USER_ID}",
                "             , GETDATE()",
                "          FROM SYS_USER_FUN U",
                "          JOIN (",
                "                   SELECT N.SYS_ID ",
                "                        , N.FUN_MENU_SYS_ID",
                "                        , N.FUN_MENU",
                "                        , N.FUN_CONTROLLER_ID",
                "                        , N.FUN_ACTION_NAME",
                "                        , M.DEFAULT_MENU_ID",
                "                        , M.SORT_ORDER",
                "                     FROM SYS_SYSTEM_MENU_FUN N",
                "                     JOIN SYS_SYSTEM_FUN_MENU M",
                "                       ON N.FUN_MENU_SYS_ID = M.SYS_ID",
                "                      AND N.FUN_MENU = M.FUN_MENU",
                "               ) Z",
                "            ON U.SYS_ID = Z.SYS_ID",
                "           AND U.FUN_CONTROLLER_ID = Z.FUN_CONTROLLER_ID",
                "           AND U.FUN_ACTION_NAME = Z.FUN_ACTION_NAME",
                "          LEFT JOIN SYS_USER_FUN_MENU M",
                "            ON U.USER_ID = M.USER_ID",
                "           AND Z.FUN_MENU_SYS_ID = M.SYS_ID",
                "           AND Z.FUN_MENU=M.FUN_MENU",
                "         WHERE U.USER_ID = {USER_ID}",
                "           AND M.USER_ID IS NULL",
                "           AND M.SYS_ID IS NULL",
                "           AND M.FUN_MENU IS NULL;",

                "        DELETE SYS_USER_FUN_MENU",
                "          FROM SYS_USER_FUN_MENU U",
                "          LEFT JOIN (",
                "                        SELECT DISTINCT N.FUN_MENU_SYS_ID",
                "                             , N.FUN_MENU",
                "                          FROM SYS_USER_FUN F",
                "                          JOIN SYS_SYSTEM_MENU_FUN N",
                "                            ON F.SYS_ID = N.SYS_ID",
                "                           AND F.FUN_CONTROLLER_ID = N.FUN_CONTROLLER_ID",
                "                           AND F.FUN_ACTION_NAME = N.FUN_ACTION_NAME",
                "                         WHERE F.USER_ID = {USER_ID}) M",
                "                 ON U.SYS_ID = M.FUN_MENU_SYS_ID",
                "                AND U.FUN_MENU = M.FUN_MENU",
                "         WHERE U.USER_ID = {USER_ID}",
                "           AND M.FUN_MENU_SYS_ID IS NULL",
                "           AND M.FUN_MENU IS NULL;",
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

            dbParameters.Add(new DBParameter { Name = EditUserFunInfoPara.ParaField.USER_ID, Value = para.UserID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditUserFunInfoResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
    }
}
