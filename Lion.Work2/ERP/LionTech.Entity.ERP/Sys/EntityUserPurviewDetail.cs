using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityUserPurviewDetail : EntitySys
    {
        public EntityUserPurviewDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 取得資料權限名稱 -
        public class PurviewNamePara : DBCulture
        {
            public PurviewNamePara(string cultureID) 
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

        public class PurviewName : DBTableRow
        {
            public DBVarChar PurviewID;
            public DBNVarChar PurviewNM;
        }

        public List<PurviewName> SelectPurviewNameList(PurviewNamePara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,new object[]
            {
                "SELECT PURVIEW_ID AS PurviewID",
                "     , {PURVIEW_NM} AS PurviewNM",
                "  FROM SYS_SYSTEM_PURVIEW",
                " WHERE SYS_ID = {SYS_ID}"
            }));

            dbParameters.Add(new DBParameter { Name = PurviewNamePara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = PurviewNamePara.ParaField.PURVIEW_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(PurviewNamePara.ParaField.PURVIEW_NM.ToString())) });

            return GetEntityList<PurviewName>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 取得使用者資料權限明細 -
        public class UserPurviewDetailPara : DBCulture
        {
            public UserPurviewDetailPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                USER_ID,
                SYS_NM,
                PURVIEW_NM
            }

            public DBVarChar SysID;
            public DBVarChar UserID;
            public DBVarChar PurviewID;
            public DBVarChar CodeType;
            public DBVarChar CodeID;
            public DBChar PurviewOP;
        }

        public class UserPurviewDetail : DBTableRow
        {
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBNVarChar PurviewNM;
            public DBVarChar PurviewID;
            public DBVarChar CodeType;
            public DBVarChar CodeID;
            public DBChar PurviewOP;
            public DBBit HasDataPur;
        }

        public List<UserPurviewDetail> SelectUserPurviewDetailList(UserPurviewDetailPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT P.SYS_ID AS SysID",
                "     , M.{SYS_NM} AS SysNM",
                "     , P.PURVIEW_ID AS PurviewID",
                "     , P.{PURVIEW_NM} AS PurviewNM",
                "	  , U.CODE_TYPE AS CodeType",
                "	  , U.CODE_ID AS CodeID",
                "	  , U.PURVIEW_OP AS PurviewOP",
                "	  , (CASE WHEN U.CODE_TYPE IS NULL THEN 0 ELSE 1 END) AS HasDataPur",
                "  FROM SYS_SYSTEM_PURVIEW P",
                "  JOIN SYS_SYSTEM_MAIN M",
                "    ON P.SYS_ID = M.SYS_ID",
                "  LEFT JOIN SYS_USER_PURVIEW U",
                "    ON U.SYS_ID = P.SYS_ID",
                "   AND U.USER_ID = {USER_ID}",
                "   AND P.PURVIEW_ID = U.PURVIEW_ID",
                " WHERE P.SYS_ID = {SYS_ID}"
            }));

            dbParameters.Add(new DBParameter { Name = UserPurviewDetailPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserPurviewDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = UserPurviewDetailPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserPurviewDetailPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = UserPurviewDetailPara.ParaField.PURVIEW_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserPurviewDetailPara.ParaField.PURVIEW_NM.ToString())) });

            return GetEntityList<UserPurviewDetail>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 更新使用者資料權限 -
        public class UserPurviewPara
        {
            public enum ParaField
            {
                SYS_ID,
                USER_ID,
                UPD_USER_ID,
                PURVIEW_ID,
                CODE_TYPE,
                CODE_ID,
                PURVIEW_OP
            }

            public DBVarChar SysID;
            public DBVarChar UserID;
            public DBVarChar UpdUserID;

            public List<UserPurviewDetail> UserPurviewList;
        }

        public enum EnumEditSysUserPurviewDetailResult
        {
            Success,
            Failure
        }

        public EnumEditSysUserPurviewDetailResult EditSysUserPurviewDetail(UserPurviewPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();

            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    " DECLARE @RESULT CHAR(1) = 'N';",
                    " DECLARE @ERROR_LINE INT;",
                    " DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                    " BEGIN TRANSACTION",
                    "     BEGIN TRY",
                    "         DELETE FROM SYS_USER_PURVIEW ",
                    "          WHERE USER_ID = {USER_ID}",
                    "            AND SYS_ID = {SYS_ID};",
                    Environment.NewLine
                }));

            foreach (var purview in para.UserPurviewList)
            {
                commandText.AppendLine(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                    "        INSERT INTO SYS_USER_PURVIEW (",
                    "               USER_ID",
                    "             , SYS_ID",
                    "             , PURVIEW_ID",
                    "             , CODE_TYPE",
                    "             , CODE_ID",
                    "             , PURVIEW_OP",
                    "             , UPD_USER_ID",
                    "             , UPD_DT",
                    "        ) VALUES ( ",
                    "               {USER_ID}",
                    "             , {SYS_ID}",
                    "             , {PURVIEW_ID}",
                    "             , {CODE_TYPE}",
                    "             , {CODE_ID}",
                    "             , {PURVIEW_OP}",
                    "             , {UPD_USER_ID}",
                    "             , GETDATE()",
                    "             );"
                    ),
                    new List<DBParameter>
                    {
                        new DBParameter { Name = UserPurviewPara.ParaField.USER_ID, Value = para.UserID },
                        new DBParameter { Name = UserPurviewPara.ParaField.SYS_ID, Value = para.SysID },
                        new DBParameter { Name = UserPurviewPara.ParaField.PURVIEW_ID, Value = purview.PurviewID },
                        new DBParameter { Name = UserPurviewPara.ParaField.CODE_TYPE, Value = purview.CodeType },
                        new DBParameter { Name = UserPurviewPara.ParaField.CODE_ID, Value = purview.CodeID },
                        new DBParameter { Name = UserPurviewPara.ParaField.PURVIEW_OP, Value = purview.PurviewOP },
                        new DBParameter { Name = UserPurviewPara.ParaField.UPD_USER_ID, Value = para.UpdUserID },
                    }));
            }

            commandText.AppendLine(string.Join(Environment.NewLine,
                new object[]
                {
                    "       SET @RESULT = 'Y';",
                    "       COMMIT;",
                    "    END TRY",
                    "    BEGIN CATCH",
                    "       SET @RESULT = 'N';",
                    "       SET @ERROR_LINE = ERROR_LINE();",
                    "       SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                    "       ROLLBACK TRANSACTION;",
                    "    END CATCH;",
                    "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                }));

            dbParameters.Add(new DBParameter { Name = UserPurviewPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = UserPurviewPara.ParaField.USER_ID, Value = para.UserID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditSysUserPurviewDetailResult.Success;
            }
            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion
    }
}