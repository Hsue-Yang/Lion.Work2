// 新增日期：2018-01-12
// 新增人員：廖先駿
// 新增內容：元素權限使用者設定
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemFunElmUser : EntitySys
    {
        public EntitySystemFunElmUser(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 取得元素權限使用者明細 -
        public class ElmAuthUserPara
        {
            public enum ParaField
            {
                SYS_ID,
                FUN_CONTROLLER_ID,
                FUN_ACTION_NAME,
                USER_ID,
                ELM_ID,
                DISPLAY_STS,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionNM;
            public DBVarChar ElmID;
            public DBVarChar UpdUserID;
            public List<ElmUserInfo> ElmUserInfoList;
        }

        public class ElmAuthUser : DBTableRow
        {
            public DBVarChar UserID;
            public DBNVarChar UserNMID;
            public DBTinyInt DisplaySts;
        }

        public List<ElmAuthUser> SelectElmAuthUserList(ElmAuthUserPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT USER_ID AS UserID",
                "     , dbo.FN_GET_USER_NM(USER_ID) AS UserNMID",
                "     , DISPLAY_STS AS DisplaySts",
                "  FROM SYS_USER_FUN_ELM",
                " WHERE SYS_ID = {SYS_ID}",
                "   AND FUN_CONTROLLER_ID = {FUN_CONTROLLER_ID}",
                "   AND FUN_ACTION_NAME = {FUN_ACTION_NAME}",
                "   AND ELM_ID = {ELM_ID}"
            });

            dbParameters.Add(new DBParameter { Name = ElmAuthUserPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = ElmAuthUserPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = ElmAuthUserPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionNM });
            dbParameters.Add(new DBParameter { Name = ElmAuthUserPara.ParaField.ELM_ID, Value = para.ElmID });

            return GetEntityList<ElmAuthUser>(commandText, dbParameters);
        }
        #endregion

        #region - 編輯元素權限使用者 -
        public enum EnumEditElmAuthUserResult
        {
            Success,
            Failure
        }

        public class ElmUserInfo
        {
            public DBVarChar User;
            public DBTinyInt DispalySts;
        }

        public EnumEditElmAuthUserResult EditElmAuthUser(ElmAuthUserPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "DECLARE @ERROR_LINE INT;",
                    "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                    "BEGIN TRANSACTION",
                    "    BEGIN TRY",
                    "        DELETE SYS_USER_FUN_ELM",
                    "         WHERE SYS_ID = {SYS_ID}",
                    "           AND FUN_CONTROLLER_ID = {FUN_CONTROLLER_ID}",
                    "           AND FUN_ACTION_NAME = {FUN_ACTION_NAME}",
                    "           AND ELM_ID = {ELM_ID};"
                }));

            foreach (var roleInfo in para.ElmUserInfoList)
            {
                commandText.AppendLine(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                    " INSERT INTO SYS_USER_FUN_ELM",
                    "      ( SYS_ID",
                    "      , USER_ID",
                    "      , FUN_CONTROLLER_ID",
                    "      , FUN_ACTION_NAME",
                    "      , ELM_ID",
                    "      , DISPLAY_STS",
                    "      , UPD_USER_ID",
                    "      , UPD_DT",
                    "      ) ",
                    " VALUES",
                    "      ( {SYS_ID}",
                    "      , {USER_ID}",
                    "      , {FUN_CONTROLLER_ID}",
                    "      , {FUN_ACTION_NAME}",
                    "      , {ELM_ID}",
                    "      , {DISPLAY_STS}",
                    "      , {UPD_USER_ID}",
                    "      , GETDATE()",
                    "      );"
                    ),
                    new List<DBParameter>
                    {
                        new DBParameter { Name = ElmAuthUserPara.ParaField.SYS_ID, Value = para.SysID },
                        new DBParameter { Name = ElmAuthUserPara.ParaField.USER_ID, Value = roleInfo.User },
                        new DBParameter { Name = ElmAuthUserPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID },
                        new DBParameter { Name = ElmAuthUserPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionNM },
                        new DBParameter { Name = ElmAuthUserPara.ParaField.ELM_ID, Value = para.ElmID },
                        new DBParameter { Name = ElmAuthUserPara.ParaField.DISPLAY_STS, Value = roleInfo.DispalySts },
                        new DBParameter { Name = ElmAuthUserPara.ParaField.UPD_USER_ID, Value = para.UpdUserID }
                    }));
            }

            commandText.AppendLine(string.Join(Environment.NewLine,
                new object[]
                {
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

            dbParameters.Add(new DBParameter { Name = ElmAuthUserPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = ElmAuthUserPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = ElmAuthUserPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionNM });
            dbParameters.Add(new DBParameter { Name = ElmAuthUserPara.ParaField.ELM_ID, Value = para.ElmID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditElmAuthUserResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion

        public class SystemFunElmInfoPara : DBCulture
        {
            public SystemFunElmInfoPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                FUN_CONTROLLER_ID,
                FUN_ACTION_NAME,
                ELM_ID,
                FUN_NM,
                FUN_GROUP,
                ELM_NM,
                CODE_NM
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionNM;
            public DBVarChar ElmID;
        }

        public class SystemFunElmInfo : DBTableRow
        {
            public DBNVarChar ElmNMID;
            public DBNVarChar FnNMID;
            public DBNVarChar FnGroupNMID;
            public DBNVarChar DefaultDisplay;
        }

        public SystemFunElmInfo SelectSystemFunElmInfo(SystemFunElmInfoPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT C.{CODE_NM} AS DefaultDisplay",
                "     , dbo.FN_GET_NMID(E.ELM_ID, E.{ELM_NM}) AS ElmNMID",
                "     , dbo.FN_GET_NMID(F.FUN_ACTION_NAME, F.{FUN_NM}) AS FnNMID",
                "     , dbo.FN_GET_NMID(G.FUN_CONTROLLER_ID, G.{FUN_GROUP}) AS FnGroupNMID",
                "  FROM SYS_SYSTEM_FUN_ELM E",
                "  JOIN SYS_SYSTEM_FUN F",
                "  ON E.SYS_ID = F.SYS_ID",
                "  AND E.FUN_CONTROLLER_ID = F.FUN_CONTROLLER_ID",
                "  AND E.FUN_ACTION_NAME = F.FUN_ACTION_NAME",
                "  JOIN SYS_SYSTEM_FUN_GROUP G",
                "  ON E.SYS_ID = G.SYS_ID",
                "  AND E.FUN_CONTROLLER_ID = G.FUN_CONTROLLER_ID",
                "  JOIN CM_CODE C",
                "  ON E.DEFAULT_DISPLAY_STS = C.CODE_ID",
                "  AND C.CODE_KIND = '0045'",
                " WHERE E.SYS_ID = {SYS_ID}",
                "   AND E.FUN_CONTROLLER_ID = {FUN_CONTROLLER_ID}",
                "   AND E.FUN_ACTION_NAME = {FUN_ACTION_NAME}",
                "   AND E.ELM_ID = {ELM_ID}"
            });

            dbParameters.Add(new DBParameter { Name = SystemFunElmInfoPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmInfoPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmInfoPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionNM });
            dbParameters.Add(new DBParameter { Name = SystemFunElmInfoPara.ParaField.ELM_ID, Value = para.ElmID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmInfoPara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunElmInfoPara.ParaField.FUN_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunElmInfoPara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunElmInfoPara.ParaField.FUN_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunElmInfoPara.ParaField.ELM_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunElmInfoPara.ParaField.ELM_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunElmInfoPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunElmInfoPara.ParaField.CODE_NM.ToString())) });

            return GetEntityList<SystemFunElmInfo>(commandText, dbParameters).SingleOrDefault();
        }
    }

    public class MongoSystemFunElmUser : Mongo_BaseAP
    {
        public MongoSystemFunElmUser(string connectionString, string providerName)
            : base(connectionString, providerName)
        {

        }

        public class UserFunElmPara : MongoElement
        {
            public enum ParaField
            {
                SYS_ID,
                SYS_NM,
                CONTROLLER_NAME,
                ACTION_NAME,
                ELM_ID,
                USER_ID,
                USER_NM,
                DISPLAY_STS,
                DISPLAY_NM,
                MODIFY_TYPE,
                MODIFY_TYPE_NM,
                WFNO,
                MEMO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT
            }

            public List<UserFunElmInfo> UserInfoList;
        }

        public class UserFunElmInfo
        {
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;

            [DBTypeProperty("SYS_NM")]
            public DBNVarChar SysNM;

            [DBTypeProperty("CONTROLLER_NAME")]
            public DBVarChar ControllerName;

            [DBTypeProperty("ACTION_NAME")]
            public DBVarChar ActionName;

            [DBTypeProperty("ELM_ID")]
            public DBVarChar ElmID;

            [DBTypeProperty("USER_ID")]
            public DBVarChar UserID;

            [DBTypeProperty("USER_NM")]
            public DBVarChar UserNM;

            [DBTypeProperty("DISPLAY_STS")]
            public DBTinyInt DisplaySts;

            [DBTypeProperty("DISPLAY_NM")]
            public DBNVarChar DisplayNM;

            [DBTypeProperty("MODIFY_TYPE")]
            public DBChar ModifyType;

            [DBTypeProperty("MODIFY_TYPE_NM")]
            public DBNVarChar ModifyTypeNM;

            [DBTypeProperty("WFNO")]
            public DBVarChar WFNO;

            [DBTypeProperty("MEMO")]
            public DBVarChar Memo;

            [DBTypeProperty("UPD_USER_ID")]
            public DBVarChar UpdUserID;

            [DBTypeProperty("UPD_USER_NM")]
            public DBNVarChar UpdUserNM;

            [DBTypeProperty("UPD_DT")]
            public DBDateTime UpdDT;
        }

        #region - 紀錄元素權限角色 -
        public enum EnumRecordLogSystemUserFunElmResult
        {
            Success,
            Failure
        }

        public EnumRecordLogSystemUserFunElmResult RecordLogSystemUserFunElm(UserFunElmPara para)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.LOG_USER_FUN_ELM.ToString());

            List<DBParameters> dbParameters = new List<DBParameters>();

            dbParameters = para.UserInfoList.Select(user => new DBParameters
            {
                new DBParameter { Name = UserFunElmPara.ParaField.SYS_ID, Value = user.SysID },
                new DBParameter { Name = UserFunElmPara.ParaField.SYS_NM, Value = user.SysNM },
                new DBParameter { Name = UserFunElmPara.ParaField.CONTROLLER_NAME, Value = user.ControllerName },
                new DBParameter { Name = UserFunElmPara.ParaField.ACTION_NAME, Value = user.ActionName },
                new DBParameter { Name = UserFunElmPara.ParaField.ELM_ID, Value = user.ElmID },
                new DBParameter { Name = UserFunElmPara.ParaField.USER_ID, Value = user.UserID },
                new DBParameter { Name = UserFunElmPara.ParaField.USER_NM, Value = user.UserNM },
                new DBParameter { Name = UserFunElmPara.ParaField.DISPLAY_STS, Value = user.DisplaySts },
                new DBParameter { Name = UserFunElmPara.ParaField.DISPLAY_NM, Value = user.DisplayNM },
                new DBParameter { Name = UserFunElmPara.ParaField.MODIFY_TYPE, Value = user.ModifyType },
                new DBParameter { Name = UserFunElmPara.ParaField.MODIFY_TYPE_NM, Value = user.ModifyTypeNM },
                new DBParameter { Name = UserFunElmPara.ParaField.WFNO, Value = user.WFNO },
                new DBParameter { Name = UserFunElmPara.ParaField.MEMO, Value = user.Memo },
                new DBParameter { Name = UserFunElmPara.ParaField.UPD_USER_ID, Value = user.UpdUserID },
                new DBParameter { Name = UserFunElmPara.ParaField.UPD_USER_NM, Value = user.UpdUserNM },
                new DBParameter { Name = UserFunElmPara.ParaField.UPD_DT, Value = user.UpdDT }
            }).ToList();

            bool result = Insert(command, dbParameters);
            return (result) ? EnumRecordLogSystemUserFunElmResult.Success : EnumRecordLogSystemUserFunElmResult.Failure;
        }
        #endregion
    }
}