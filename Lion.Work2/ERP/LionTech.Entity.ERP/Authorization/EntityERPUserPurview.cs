// 新增日期：2016-11-08
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Authorization
{
    public class EntityERPUserPurview : EntityAuthorization
    {
        public EntityERPUserPurview(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 更新使用者資料權限 -
        public class Purview
        {
            public DBVarChar PurviewID;
            public DBVarChar CodeType;
            public DBVarChar CodeID;
            public DBChar PurviewOP;
        }

        public class UserPurviewPara
        {
            public enum ParaField
            {
                USER_ID,
                UPD_USER_ID,
                PURvIEW_LIST,

                SYS_ID,
                PURVIEW_ID,
                CODE_TYPE,
                CODE_ID,
                PURVIEW_OP
            }

            public DBVarChar SysID;
            public DBVarChar UserID;
            public DBVarChar UpdUserID;
            public List<Purview> PurviewList;
        }

        public class UserPurview : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar PurviewID;
            public DBVarChar CodeType;
            public DBVarChar CodeID;
            public DBChar PurviewOP;
        }

        public enum EnumUserPurviewResult
        {
            Success,
            Failure,
            NotExist
        }

        public EnumUserPurviewResult EditUserPurview(UserPurviewPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                " DECLARE @ERROR_LINE INT;",
                " DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                " DECLARE @RESULT VARCHAR(50) = '" + EnumUserPurviewResult.Failure + "';",
                " DECLARE @SYS_ID VARCHAR(20) = {SYS_ID};",
                " DECLARE @USER_ID VARCHAR(20) = {USER_ID};",
                " DECLARE @UPD_USER_ID VARCHAR(20) = {UPD_USER_ID};",
                "   BEGIN TRANSACTION",
                "       BEGIN TRY",
                "           DELETE FROM SYS_USER_PURVIEW ",
                "            WHERE SYS_ID = @SYS_ID",
                "              AND USER_ID = @USER_ID",
                "           IF EXISTS(SELECT USER_ID FROM SYS_USER_MAIN WHERE USER_ID = @USER_ID)",
                "               BEGIN"
            }));

            foreach (var purview in para.PurviewList)
            {
                commandText.AppendLine(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                    "               INSERT INTO SYS_USER_PURVIEW",
                    "                    ( USER_ID",
                    "                    , SYS_ID",
                    "                    , PURVIEW_ID",
                    "                    , CODE_TYPE",
                    "                    , CODE_ID",
                    "                    , PURVIEW_OP",
                    "                    , UPD_USER_ID",
                    "                    , UPD_DT",
                    "                    )",
                    "               VALUES",
                    "                    ( @USER_ID",
                    "                    , {SYS_ID}",
                    "                    , {PURVIEW_ID}",
                    "                    , {CODE_TYPE}",
                    "                    , {CODE_ID}",
                    "                    , {PURVIEW_OP}",
                    "                    , @UPD_USER_ID",
                    "                    , GETDATE()",
                    "                    );"
                    ),
                    new List<DBParameter>
                    {
                        new DBParameter { Name = UserPurviewPara.ParaField.PURVIEW_ID, Value = purview.PurviewID },
                        new DBParameter { Name = UserPurviewPara.ParaField.CODE_TYPE, Value = purview.CodeType },
                        new DBParameter { Name = UserPurviewPara.ParaField.CODE_ID, Value = purview.CodeID },
                        new DBParameter { Name = UserPurviewPara.ParaField.PURVIEW_OP, Value = purview.PurviewOP }
                    }));
            }

            commandText.AppendLine(string.Join(Environment.NewLine,
                new object[]
                {
                    "               SET @RESULT = '" + EnumUserPurviewResult.Success + "';",
                    "           END",
                    "       ELSE",
                    "           BEGIN",
                    "               SET @RESULT = '" + EnumUserPurviewResult.NotExist + "'",
                    "           END",
                    "       COMMIT;",
                    "    END TRY",
                    "    BEGIN CATCH",
                    "       SET @RESULT = '" + EnumUserPurviewResult.Failure + "';",
                    "       SET @ERROR_LINE = ERROR_LINE();",
                    "       SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                    "       ROLLBACK TRANSACTION;",
                    "    END CATCH;",
                    "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                }));

            dbParameters.Add(new DBParameter { Name = UserPurviewPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = UserPurviewPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserPurviewPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumUserPurviewResult.Failure.ToString())
            {
                throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
            }

            return (EnumUserPurviewResult)Enum.Parse(typeof(EnumUserPurviewResult), result.Result.GetValue());
        }
        #endregion

        #region - 取得使用者資料權限代碼名稱 -
        public class CodeInfoPara : DBCulture
        {
            public CodeInfoPara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                CODE_ID,
                CODE_KIND,
                CULTURE_ID,
                CODE_TYPE
            }

            public List<Code> Codes;
        }

        public class Code
        {
            public Entity_BaseAP.EnumPurviewCodeType CodeType;
            public DBVarChar CodeID;
            public DBVarChar CodeKind;
        }

        public class CodeInfo : DBTableRow
        {
            public DBVarChar CodeType;
            public DBVarChar CodeID;
            public DBNVarChar CodeNM;
        }

        public List<CodeInfo> SelectCodeInfoList(CodeInfoPara para)
        {
            StringBuilder commandText = new StringBuilder();
            List<string> commandTable = new List<string>();
            List<DBParameter> dbParameters = new List<DBParameter>();

            foreach (var row in para.Codes)
            {
                switch (row.CodeType)
                {
                    case Entity_BaseAP.EnumPurviewCodeType.COUNTRY:
                        commandTable.Add(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                            "   SELECT {CODE_TYPE} AS CodeType",
                            "        , CODE_ID AS CodeID",
                            "        , dbo.FN_GET_CM_NM({CODE_KIND}, CODE_ID, {CULTURE_ID}) AS CodeNM",
                            "     FROM CM_CODE ",
                            "    WHERE CODE_KIND = {CODE_KIND} ",
                            "      AND CODE_ID = {CODE_ID}"
                            ),
                            new List<DBParameter>
                            {
                                new DBParameter { Name = CodeInfoPara.ParaField.CODE_TYPE, Value = new DBVarChar(row.CodeType) },
                                new DBParameter { Name = CodeInfoPara.ParaField.CODE_ID, Value = row.CodeID },
                                new DBParameter { Name = CodeInfoPara.ParaField.CODE_KIND.ToString(), Value = row.CodeKind },
                                new DBParameter { Name = CodeInfoPara.ParaField.CULTURE_ID, Value = new DBVarChar(para.CultureID) }
                            }));
                        break;
                    case Entity_BaseAP.EnumPurviewCodeType.COMPANY:
                        commandTable.Add(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                            "   SELECT {CODE_TYPE} AS CodeType",
                            "        , COM_ID AS CodeID",
                            "        , COM_NM AS CodeNM",
                            "     FROM RAW_CM_ORG_COM",
                            "    WHERE COM_ID = {CODE_ID}"
                            ),
                            new List<DBParameter>
                            {
                                new DBParameter { Name = CodeInfoPara.ParaField.CODE_TYPE, Value = new DBVarChar(row.CodeType) },
                                new DBParameter { Name = CodeInfoPara.ParaField.CODE_ID.ToString(), Value = row.CodeID }
                            }));
                        break;
                    case Entity_BaseAP.EnumPurviewCodeType.UNIT:
                    {
                        commandTable.Add(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                            "   SELECT {CODE_TYPE} AS CodeType",
                            "        , UNIT_ID AS CodeID",
                            "        , UNIT_NM AS CodeNM",
                            "     FROM RAW_CM_ORG_UNIT ",
                            "    WHERE UNIT_ID = {CODE_ID}"
                            ),
                            new List<DBParameter>
                            {
                                new DBParameter { Name = CodeInfoPara.ParaField.CODE_TYPE, Value = new DBVarChar(row.CodeType) },
                                new DBParameter { Name = CodeInfoPara.ParaField.CODE_ID.ToString(), Value = row.CodeID }
                            }));
                        break;
                    }
                }
            }

            if (commandTable.Any())
            {
                commandText.AppendLine(string.Join(" UNION ", commandTable));
            }

            return GetEntityList<CodeInfo>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 取得資料權限名稱 -
        public class SysPurviewPara : DBCulture
        {
            public SysPurviewPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                PURVIEW_NM
            }

            public DBVarChar SysID;
        }

        public class SysPurview : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar PurviewID;
            public DBNVarChar PurviewNM;
        }

        public List<SysPurview> SelectSysPurviewList(SysPurviewPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT SYS_ID AS SysID",
                "     , PURVIEW_ID AS PurviewID",
                "     , {PURVIEW_NM} AS PurviewNM",
                "  FROM SYS_SYSTEM_PURVIEW",
                " WHERE SYS_ID = {SYS_ID}"
            }));

            dbParameters.Add(new DBParameter { Name = SysPurviewPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysPurviewPara.ParaField.PURVIEW_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysPurviewPara.ParaField.PURVIEW_NM.ToString())) });

            return GetEntityList<SysPurview>(commandText.ToString(), dbParameters);
        }
        #endregion
    }
}