// 新增日期：2018-01-09
// 新增人員：廖先駿
// 新增內容：元素權限明細
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemFunElmDetail : EntitySys
    {
        public EntitySystemFunElmDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 取得元素權限明細 -
        public class SystemFunElmDetailPara
        {
            public enum ParaField
            {
                ELM_ID,
                SYS_ID,
                FUN_CONTROLLER_ID,
                FUN_ACTION_NAME,
                IS_DISABLE,
                DEFAULT_DISPLAY_STS,
                ELM_NM_ZH_TW,
                ELM_NM_ZH_CN,
                ELM_NM_EN_US,
                ELM_NM_TH_TH,
                ELM_NM_JA_JP,
                UPD_USER_ID
            }

            public DBVarChar ElmID;
            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionNM;
            public DBChar IsDisable;
            public DBTinyInt DefaultDisplaySts;
            public DBNVarChar ElmNMZHTW;
            public DBNVarChar ElmNMZHCN;
            public DBNVarChar ElmNMENUS;
            public DBNVarChar ElmNMTHTH;
            public DBNVarChar ElmNMJAJP;
            public DBVarChar UpdUserID;
        }

        public class SystemFunElmDetail : DBTableRow
        {
            public DBChar IsDisable;
            public DBTinyInt DefaultDisplaySts;
            public DBNVarChar ElmNMZHTW;
            public DBNVarChar ElmNMZHCN;
            public DBNVarChar ElmNMENUS;
            public DBNVarChar ElmNMTHTH;
            public DBNVarChar ElmNMJAJP;
        }

        public SystemFunElmDetail SelectSystemFunElmDetail(SystemFunElmDetailPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();

            var commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT IS_DISABLE AS IsDisable",
                "     , DEFAULT_DISPLAY_STS AS DefaultDisplaySts",
                "     , ELM_NM_ZH_TW AS ElmNMZHTW",
                "     , ELM_NM_ZH_CN AS ElmNMZHCN",
                "     , ELM_NM_EN_US AS ElmNMENUS",
                "     , ELM_NM_TH_TH AS ElmNMTHTH",
                "     , ELM_NM_JA_JP AS ElmNMJAJP",
                "  FROM SYS_SYSTEM_FUN_ELM",
                " WHERE ELM_ID = {ELM_ID}",
                "   AND SYS_ID = {SYS_ID}",
                "   AND FUN_CONTROLLER_ID = {FUN_CONTROLLER_ID}",
                "   AND FUN_ACTION_NAME = {FUN_ACTION_NAME}"
            });

            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.ELM_ID, Value = para.ElmID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionNM });

            return GetEntityList<SystemFunElmDetail>(commandText, dbParameters).SingleOrDefault();
        }
        #endregion

        #region - 編輯元素權限明細 -
        public enum EnumEditSystemFunElmDetailResult
        {
            Success,
            Failure
        }

        public EnumEditSystemFunElmDetailResult EditSystemFunElmDetail(SystemFunElmDetailPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1) = 'N';",
                "DECLARE @ERROR_LINE INT;",
                "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                "BEGIN TRANSACTION",
                "    BEGIN TRY",
                "        DELETE FROM SYS_SYSTEM_FUN_ELM",
                "         WHERE ELM_ID = {ELM_ID}",
                "           AND SYS_ID = {SYS_ID}",
                "           AND FUN_CONTROLLER_ID = {FUN_CONTROLLER_ID}",
                "           AND FUN_ACTION_NAME = {FUN_ACTION_NAME}",

                "        INSERT INTO SYS_SYSTEM_FUN_ELM",
                "             ( ELM_ID",
                "             , SYS_ID",
                "             , FUN_CONTROLLER_ID",
                "             , FUN_ACTION_NAME",
                "             , IS_DISABLE",
                "             , DEFAULT_DISPLAY_STS",
                "             , ELM_NM_ZH_TW",
                "             , ELM_NM_ZH_CN",
                "             , ELM_NM_EN_US",
                "             , ELM_NM_TH_TH",
                "             , ELM_NM_JA_JP",
                "             , UPD_USER_ID",
                "             , UPD_DT",
                "             )",
                "        SELECT {ELM_ID}",
                "             , {SYS_ID}",
                "             , {FUN_CONTROLLER_ID}",
                "             , {FUN_ACTION_NAME}",
                "             , {IS_DISABLE}",
                "             , {DEFAULT_DISPLAY_STS}",
                "             , {ELM_NM_ZH_TW}",
                "             , {ELM_NM_ZH_CN}",
                "             , {ELM_NM_EN_US}",
                "             , {ELM_NM_TH_TH}",
                "             , {ELM_NM_JA_JP}",
                "             , {UPD_USER_ID}",
                "             , GETDATE();",
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

            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.ELM_ID, Value = para.ElmID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionNM });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.IS_DISABLE, Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.DEFAULT_DISPLAY_STS, Value = para.DefaultDisplaySts });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.ELM_NM_ZH_TW, Value = para.ElmNMZHTW });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.ELM_NM_ZH_CN, Value = para.ElmNMZHCN });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.ELM_NM_EN_US, Value = para.ElmNMENUS });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.ELM_NM_TH_TH, Value = para.ElmNMTHTH });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.ELM_NM_JA_JP, Value = para.ElmNMJAJP });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditSystemFunElmDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion

        #region - 刪除元素權限明細 -
        public enum EnumDeleteSystemFunElmDetailResult
        {
            Success,
            Failure
        }

        public EnumDeleteSystemFunElmDetailResult DeleteSystemFunElmDetail(SystemFunElmDetailPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1) = 'N';",
                "DECLARE @ERROR_LINE INT;",
                "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                "BEGIN TRANSACTION",
                "    BEGIN TRY",
                "        DELETE FROM SYS_SYSTEM_FUN_ELM",
                "         WHERE ELM_ID = {ELM_ID}",
                "           AND SYS_ID = {SYS_ID}",
                "           AND FUN_CONTROLLER_ID = {FUN_CONTROLLER_ID}",
                "           AND FUN_ACTION_NAME = {FUN_ACTION_NAME};",
                "        DELETE FROM SYS_SYSTEM_ROLE_FUN_ELM",
                "         WHERE ELM_ID = {ELM_ID}",
                "           AND SYS_ID = {SYS_ID}",
                "           AND FUN_CONTROLLER_ID = {FUN_CONTROLLER_ID}",
                "           AND FUN_ACTION_NAME = {FUN_ACTION_NAME};",
                "        DELETE FROM SYS_USER_FUN_ELM",
                "         WHERE ELM_ID = {ELM_ID}",
                "           AND SYS_ID = {SYS_ID}",
                "           AND FUN_CONTROLLER_ID = {FUN_CONTROLLER_ID}",
                "           AND FUN_ACTION_NAME = {FUN_ACTION_NAME};",
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

            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.ELM_ID, Value = para.ElmID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionNM });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumDeleteSystemFunElmDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion

        #region - 查詢是否存在功能元素代碼 -
        /// <summary>
        /// 查詢是否存在功能元素代碼
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DBChar SelectHasElmID(SystemFunElmDetailPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "IF EXISTS (SELECT *",
                    "             FROM SYS_SYSTEM_FUN_ELM",
                    "            WHERE SYS_ID = {SYS_ID}",
                    "              AND FUN_CONTROLLER_ID = {FUN_CONTROLLER_ID}",
                    "              AND FUN_ACTION_NAME = {FUN_ACTION_NAME}",
                    "              AND ELM_ID = {ELM_ID})",
                    "BEGIN",
                    "    SET @RESULT = 'Y';",
                    "END",
                    "SELECT @RESULT;"
                });

            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionNM });
            dbParameters.Add(new DBParameter { Name = SystemFunElmDetailPara.ParaField.ELM_ID, Value = para.ElmID });

            return new DBChar(ExecuteScalar(commandText, dbParameters));
        }
        #endregion
    }

    public class MongoSystemFunElmDetail : Mongo_BaseAP
    {
        public MongoSystemFunElmDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 紀錄元素權限 -
        public enum EnumRecordLogSysSystemFunElmResult
        {
            Success,
            Failure
        }

        public class SysSystemFunElmPara : MongoElement
        {
            public enum ParaField
            {
                SYS_ID,
                SYS_NM,
                ELM_ID,
                CONTROLLER_NAME,
                ACTION_NAME,
                IS_DISABLE,
                DEFAULT_DISPLAY_STS,
                ELM_NM_ZH_TW,
                ELM_NM_ZH_CN,
                ELM_NM_EN_US,
                ELM_NM_TH_TH,
                ELM_NM_JA_JP,
                ELM_NM_KO_KR,
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

            [DBTypeProperty("IS_DISABLE")]
            public DBChar IsDisable;

            [DBTypeProperty("DEFAULT_DISPLAY_STS")]
            public DBNVarChar DefaultDisplaySts;

            [DBTypeProperty("ELM_NM_ZH_TW")]
            public DBNVarChar ElmNMZHTW;

            [DBTypeProperty("ELM_NM_ZH_CN")]
            public DBNVarChar ElmNMZHCN;

            [DBTypeProperty("ELM_NM_EN_US")]
            public DBNVarChar ElmNMENUS;

            [DBTypeProperty("ELM_NM_TH_TH")]
            public DBNVarChar ElmNMTHTH;

            [DBTypeProperty("ELM_NM_JA_JP")]
            public DBNVarChar ElmNMJAJP;

            [DBTypeProperty("ELM_NM_KO_KR")]
            public DBNVarChar ElmNMKOKR;

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

        public EnumRecordLogSysSystemFunElmResult RecordLogSysSystemFunElm(SysSystemFunElmPara para)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.LOG_SYS_SYSTEM_FUN_ELM.ToString());

            DBParameters dbParameters = new DBParameters();
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.SYS_NM, Value = para.SysNM });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.ELM_ID, Value = para.ElmID });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.CONTROLLER_NAME, Value = para.ControllerName });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.ACTION_NAME, Value = para.ActionName });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.IS_DISABLE, Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.DEFAULT_DISPLAY_STS, Value = para.DefaultDisplaySts });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.ELM_NM_ZH_TW, Value = para.ElmNMZHTW });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.ELM_NM_ZH_CN, Value = para.ElmNMZHCN });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.ELM_NM_EN_US, Value = para.ElmNMENUS });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.ELM_NM_TH_TH, Value = para.ElmNMTHTH });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.ELM_NM_JA_JP, Value = para.ElmNMJAJP });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.ELM_NM_KO_KR, Value = para.ElmNMKOKR });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.MODIFY_TYPE, Value = para.ModifyType });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.MODIFY_TYPE_NM, Value = para.ModifyTypeNM });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.UPD_USER_NM, Value = para.UpdUserNM });
            dbParameters.Add(new DBParameter { Name = SysSystemFunElmPara.ParaField.UPD_DT, Value = para.UpdDT });

            bool result = Insert(command, dbParameters);
            return (result) ? EnumRecordLogSysSystemFunElmResult.Success : EnumRecordLogSysSystemFunElmResult.Failure;
        }
        #endregion
    }
}