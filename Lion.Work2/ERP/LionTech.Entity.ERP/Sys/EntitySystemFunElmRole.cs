// 新增日期：2018-01-12
// 新增人員：廖先駿
// 新增內容：元素權限角色設定
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemFunElmRole : EntitySys
    {
        public EntitySystemFunElmRole(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 取得元素權限角色明細 -
        public class SystemFunElmRolePara : DBCulture
        {
            public SystemFunElmRolePara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                FUN_CONTROLLER_ID,
                FUN_ACTION_NAME,
                ELM_ID,
                ROLE_ID,
                ROLE_NM,
                DISPLAY_STS,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionNM;
            public DBVarChar ElmID;
            public DBVarChar UpdUserID;
            public List<ElmRoleInfo> ElmRoleInfoList;
        }

        public class SystemFunElmRole : DBTableRow
        {
            public DBVarChar RoleID;
            public DBNVarChar RoleNMID;
            public DBTinyInt DisplaySts;
        }

        public List<SystemFunElmRole> SelectSystemFunElmRoleList(SystemFunElmRolePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT F.ROLE_ID AS RoleID",
                "     , dbo.FN_GET_NMID(F.ROLE_ID, R.{ROLE_NM}) AS RoleNMID",
                "     , DISPLAY_STS AS DisplaySts",
                "  FROM SYS_SYSTEM_ROLE_FUN_ELM F",
                "  JOIN SYS_SYSTEM_ROLE R",
                "    ON R.SYS_ID = F.SYS_ID",
                "   AND R.ROLE_ID = F.ROLE_ID",
                " WHERE F.SYS_ID = {SYS_ID}",
                "   AND F.FUN_CONTROLLER_ID = {FUN_CONTROLLER_ID}",
                "   AND F.FUN_ACTION_NAME = {FUN_ACTION_NAME}",
                "   AND F.ELM_ID = {ELM_ID}"
            });

            dbParameters.Add(new DBParameter { Name = SystemFunElmRolePara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmRolePara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmRolePara.ParaField.FUN_ACTION_NAME, Value = para.FunActionNM });
            dbParameters.Add(new DBParameter { Name = SystemFunElmRolePara.ParaField.ELM_ID, Value = para.ElmID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmRolePara.ParaField.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunElmRolePara.ParaField.ROLE_NM.ToString())) });

            return GetEntityList<SystemFunElmRole>(commandText, dbParameters);
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
        
        #region - 編輯元素權限角色 -
        public enum EnumEditSystemFunElmRoleResult
        {
            Success,
            Failure
        }

        public class ElmRoleInfo
        {
            public DBVarChar RoleID;
            public DBTinyInt DispalySts;
        }

        public EnumEditSystemFunElmRoleResult EditSystemFunElmRole(SystemFunElmRolePara para)
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
                    "        DELETE SYS_SYSTEM_ROLE_FUN_ELM",
                    "         WHERE SYS_ID = {SYS_ID}",
                    "           AND FUN_CONTROLLER_ID = {FUN_CONTROLLER_ID}",
                    "           AND FUN_ACTION_NAME = {FUN_ACTION_NAME}",
                    "           AND ELM_ID = {ELM_ID};"
                }));

            foreach (var roleInfo in para.ElmRoleInfoList)
            {
                commandText.AppendLine(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                    " INSERT INTO SYS_SYSTEM_ROLE_FUN_ELM",
                    "      ( SYS_ID",
                    "      , ROLE_ID",
                    "      , FUN_CONTROLLER_ID",
                    "      , FUN_ACTION_NAME",
                    "      , ELM_ID",
                    "      , DISPLAY_STS",
                    "      , UPD_USER_ID",
                    "      , UPD_DT",
                    "      ) ",
                    " VALUES",
                    "      ( {SYS_ID}",
                    "      , {ROLE_ID}",
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
                        new DBParameter { Name = SystemFunElmRolePara.ParaField.SYS_ID, Value = para.SysID },
                        new DBParameter { Name = SystemFunElmRolePara.ParaField.ROLE_ID, Value = roleInfo.RoleID },
                        new DBParameter { Name = SystemFunElmRolePara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID },
                        new DBParameter { Name = SystemFunElmRolePara.ParaField.FUN_ACTION_NAME, Value = para.FunActionNM },
                        new DBParameter { Name = SystemFunElmRolePara.ParaField.ELM_ID, Value = para.ElmID },
                        new DBParameter { Name = SystemFunElmRolePara.ParaField.DISPLAY_STS, Value = roleInfo.DispalySts },
                        new DBParameter { Name = SystemFunElmRolePara.ParaField.UPD_USER_ID, Value = para.UpdUserID }
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

            dbParameters.Add(new DBParameter { Name = SystemFunElmRolePara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmRolePara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmRolePara.ParaField.FUN_ACTION_NAME, Value = para.FunActionNM });
            dbParameters.Add(new DBParameter { Name = SystemFunElmRolePara.ParaField.ELM_ID, Value = para.ElmID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditSystemFunElmRoleResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion
    }

    public class MongoSystemFunElmRole : Mongo_BaseAP
    {
        public MongoSystemFunElmRole(string connectionString, string providerName)
            : base(connectionString, providerName)
        {

        }

        #region - 紀錄元素權限角色 -
        public enum EnumRecordLogSystemRoleFunElmResult
        {
            Success,
            Failure
        }

        public class SystemRoleFunElmPara : MongoElement
        {
            public enum ParaField
            {
                SYS_ID,
                SYS_NM,
                ELM_ID,
                CONTROLLER_NAME,
                ACTION_NAME,
                ELM_ROLE_LIST,
                MODIFY_TYPE,
                MODIFY_TYPE_NM,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT
            }

            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;

            [DBTypeProperty("SYS_NM")]
            public DBNVarChar SysNM;

            [DBTypeProperty("ELM_ID")]
            public DBVarChar ElmID;

            [DBTypeProperty("CONTROLLER_NAME")]
            public DBVarChar ControllerName;

            [DBTypeProperty("ACTION_NAME")]
            public DBVarChar ActionName;

            [DBTypeProperty("ELM_ROLE_LIST")]
            public List<ElmRole> ElmRoleList;

            [DBTypeProperty("MODIFY_TYPE")]
            public DBChar ModifyType;

            [DBTypeProperty("MODIFY_TYPE_NM")]
            public DBNVarChar ModifyTypeNM;

            [DBTypeProperty("UPD_USER_ID")]
            public DBVarChar UpdUserID;

            [DBTypeProperty("UPD_USER_NM")]
            public DBNVarChar UpdUserNM;

            [DBTypeProperty("UPD_DT")]
            public DBDateTime UpdDT;
        }

        public class ElmRole : MongoDocument
        {
            [DBTypeProperty("DISPLAY_STS")]
            public DBTinyInt DisplaySts;

            [DBTypeProperty("DISPLAY_NM")]
            public DBNVarChar DisplayNM;

            [DBTypeProperty("ROLE_ID_LIST")]
            public List<DBVarChar> RoleIDList;
        }

        public EnumRecordLogSystemRoleFunElmResult RecordLogSystemRoleFunElm(SystemRoleFunElmPara para)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.LOG_SYS_SYSTEM_ROLE_FUN_ELM.ToString());

            DBParameters dbParameters = new DBParameters();
            dbParameters.Add(new DBParameter { Name = SystemRoleFunElmPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunElmPara.ParaField.SYS_NM, Value = para.SysNM });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunElmPara.ParaField.ELM_ID, Value = para.ElmID });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunElmPara.ParaField.CONTROLLER_NAME, Value = para.ControllerName });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunElmPara.ParaField.ACTION_NAME, Value = para.ActionName });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunElmPara.ParaField.ELM_ROLE_LIST, Value = para.ElmRoleList });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunElmPara.ParaField.MODIFY_TYPE, Value = para.ModifyType });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunElmPara.ParaField.MODIFY_TYPE_NM, Value = para.ModifyTypeNM });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunElmPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunElmPara.ParaField.UPD_USER_NM, Value = para.UpdUserNM });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunElmPara.ParaField.UPD_DT, Value = para.UpdDT });

            bool result = Insert(command, dbParameters);
            return (result) ? EnumRecordLogSystemRoleFunElmResult.Success : EnumRecordLogSystemRoleFunElmResult.Failure;
        }

        #endregion
    }
}