// 新增日期：2017-02-17
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemLoginEventSettingDetail : EntitySys
    {
        public EntitySystemLoginEventSettingDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 查詢是否有LoginEventID -
        public DBChar SelectHasLoginEventID(LoginEventSettingDetailPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "IF EXISTS (SELECT *",
                    "             FROM SYS_SYSTEM_LOGIN_EVENT",
                    "            WHERE SYS_ID = {SYS_ID}",
                    "              AND LOGIN_EVENT_ID = {LOGIN_EVENT_ID})",
                    "BEGIN",
                    "    SET @RESULT = 'Y';",
                    "END",
                    "SELECT @RESULT;"
                }));

            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.LOGIN_EVENT_ID, Value = para.LoginEventID });

            return new DBChar(ExecuteScalar(commandText.ToString(), dbParameters));
        }
        #endregion

        #region - 取得登入事件設定明細主檔 -
        public class LoginEventSettingDetailPara : DBCulture
        {
            public LoginEventSettingDetailPara(string cultureID) : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                LOGIN_EVENT_ID,
                LOGIN_EVENT_NM_ZH_CN,
                LOGIN_EVENT_NM_ZH_TW,
                LOGIN_EVENT_NM_EN_US,
                LOGIN_EVENT_NM_TH_TH,
                LOGIN_EVENT_NM_JA_JP,
                START_DT,
                END_DT,
                FREQUENCY,
                START_EXEC_TIME,
                END_EXEC_TIME,
                TARGET_PATH,
                VALID_PATH,
                SUB_SYS_ID,
                IS_DISABLE,
                SORT_ORDER,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar LoginEventID;
            public DBNVarChar LoginEventNMZHCN;
            public DBNVarChar LoginEventNMZHTW;
            public DBNVarChar LoginEventNMENUS;
            public DBNVarChar LoginEventNMTHTH;
            public DBNVarChar LoginEventNMJAJP;
            public DBDateTime StartDT;
            public DBDateTime EndDT;
            public DBInt Frequency;
            public DBChar StartExecTime;
            public DBChar EndExecTime;
            public DBNVarChar TargetPath;
            public DBNVarChar ValidPath;
            public DBVarChar SubSysID;
            public DBChar IsDisable;
            public DBVarChar SortOrder;
            public DBVarChar UpdUserID;
        }

        public class LoginEventSettingDetail : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar LoginEventID;
            public DBNVarChar LoginEventNMZHCN;
            public DBNVarChar LoginEventNMZHTW;
            public DBNVarChar LoginEventNMENUS;
            public DBNVarChar LoginEventNMTHTH;
            public DBNVarChar LoginEventNMJAJP;
            public DBDateTime StartDT;
            public DBDateTime EndDT;
            public DBInt Frequency;
            public DBChar StartExecTime;
            public DBChar EndExecTime;
            public DBNVarChar TargetPath;
            public DBNVarChar ValidPath;
            public DBVarChar SubSysID;
            public DBChar IsDisable;
            public DBVarChar SortOrder;
        }

        public LoginEventSettingDetail SelectLoginEventSettingDetail(LoginEventSettingDetailPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT SYS_ID AS SysID",
                "     , LOGIN_EVENT_ID AS LoginEventID",
                "     , LOGIN_EVENT_NM_ZH_CN AS LoginEventNMZHCN",
                "     , LOGIN_EVENT_NM_ZH_TW AS LoginEventNMZHTW",
                "     , LOGIN_EVENT_NM_EN_US AS LoginEventNMENUS",
                "     , LOGIN_EVENT_NM_TH_TH AS LoginEventNMTHTH",
                "     , LOGIN_EVENT_NM_JA_JP AS LoginEventNMJAJP",
                "     , START_DT AS StartDT",
                "     , END_DT AS EndDT",
                "     , FREQUENCY AS Frequency",
                "     , LEFT(START_EXEC_TIME, 5) AS StartExecTime",
                "     , LEFT(END_EXEC_TIME, 5) AS EndExecTime",
                "     , TARGET_PATH AS TargetPath",
                "     , VALID_PATH AS ValidPath",
                "     , SUB_SYS_ID AS SubSysID",
                "     , IS_DISABLE AS IsDisable",
                "     , SORT_ORDER AS SortOrder",
                "  FROM SYS_SYSTEM_LOGIN_EVENT",
                " WHERE SYS_ID = {SYS_ID}",
                "   AND LOGIN_EVENT_ID = {LOGIN_EVENT_ID}"
            }));

            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.LOGIN_EVENT_ID, Value = para.LoginEventID });

            return GetEntityList<LoginEventSettingDetail>(commandText.ToString(), dbParameters).SingleOrDefault();
        }
        #endregion

        #region - 編輯登入事件設定明細 -
        public enum EnumEditLoginEventSettingDetailResult
        {
            Success,
            Failure
        }

        public EnumEditLoginEventSettingDetailResult EditLoginEventSettingDetail(LoginEventSettingDetailPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1) = 'N';",
                "DECLARE @ERROR_LINE INT;",
                "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                "BEGIN TRANSACTION",
                "    BEGIN TRY",
                "        DELETE FROM SYS_SYSTEM_LOGIN_EVENT",
                "         WHERE SYS_ID = {SYS_ID}",
                "           AND LOGIN_EVENT_ID = {LOGIN_EVENT_ID};",
                "        INSERT INTO SYS_SYSTEM_LOGIN_EVENT",
                "             ( SYS_ID",
                "             , LOGIN_EVENT_ID",
                "             , LOGIN_EVENT_NM_ZH_CN",
                "             , LOGIN_EVENT_NM_ZH_TW",
                "             , LOGIN_EVENT_NM_EN_US",
                "             , LOGIN_EVENT_NM_TH_TH",
                "             , LOGIN_EVENT_NM_JA_JP",
                "             , START_DT",
                "             , END_DT",
                "             , FREQUENCY",
                "             , START_EXEC_TIME",
                "             , END_EXEC_TIME",
                "             , TARGET_PATH",
                "             , VALID_PATH",
                "             , SUB_SYS_ID",
                "             , IS_DISABLE",
                "             , SORT_ORDER",
                "             , UPD_USER_ID",
                "             , UPD_DT",
                "             )",
                "        SELECT {SYS_ID}",
                "             , {LOGIN_EVENT_ID}",
                "             , {LOGIN_EVENT_NM_ZH_CN}",
                "             , {LOGIN_EVENT_NM_ZH_TW}",
                "             , {LOGIN_EVENT_NM_EN_US}",
                "             , {LOGIN_EVENT_NM_TH_TH}",
                "             , {LOGIN_EVENT_NM_JA_JP}",
                "             , {START_DT}",
                "             , {END_DT}",
                "             , {FREQUENCY}",
                "             , {START_EXEC_TIME}",
                "             , {END_EXEC_TIME}",
                "             , {TARGET_PATH}",
                "             , {VALID_PATH}",
                "             , {SUB_SYS_ID}",
                "             , {IS_DISABLE}",
                "             , {SORT_ORDER}",
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
            }));

            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.LOGIN_EVENT_ID, Value = para.LoginEventID });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.LOGIN_EVENT_NM_ZH_CN, Value = para.LoginEventNMZHCN });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.LOGIN_EVENT_NM_ZH_TW, Value = para.LoginEventNMZHTW });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.LOGIN_EVENT_NM_EN_US, Value = para.LoginEventNMENUS });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.LOGIN_EVENT_NM_TH_TH, Value = para.LoginEventNMTHTH });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.LOGIN_EVENT_NM_JA_JP, Value = para.LoginEventNMJAJP });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.START_DT, Value = para.StartDT });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.END_DT, Value = para.EndDT });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.FREQUENCY, Value = para.Frequency });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.START_EXEC_TIME, Value = para.StartExecTime });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.END_EXEC_TIME, Value = para.EndExecTime });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.TARGET_PATH, Value = para.TargetPath });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.VALID_PATH, Value = para.ValidPath });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.SUB_SYS_ID, Value = para.SubSysID });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.IS_DISABLE, Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = LoginEventSettingDetailPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditLoginEventSettingDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion

        #region - 刪除登入事件設定明細 -
        public class DeleteLoginEventSettingDetailPara
        {
            public enum ParaField
            {
                SYS_ID,
                LOGIN_EVENT_ID
            }

            public DBVarChar SysID;
            public DBVarChar LoginEventID;
        }

        public enum EnumDeleteLoginEventSettingDetailResult
        {
            Success,
            Failure
        }

        public EnumDeleteLoginEventSettingDetailResult DeleteLoginEventSettingDetail(DeleteLoginEventSettingDetailPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1) = 'N';",
                "DECLARE @ERROR_LINE INT;",
                "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                "  BEGIN TRANSACTION",
                "      BEGIN TRY",
                "          DELETE FROM SYS_SYSTEM_LOGIN_EVENT",
                "           WHERE SYS_ID = {SYS_ID}",
                "             AND LOGIN_EVENT_ID = {LOGIN_EVENT_ID}",
                "             SET @RESULT = 'Y';",
                "             COMMIT;",
                "      END TRY",
                "      BEGIN CATCH",
                "          SET @RESULT = 'N';",
                "          SET @ERROR_LINE = ERROR_LINE();",
                "          SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                "          ROLLBACK TRANSACTION;",
                "      END CATCH;",
                "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
            }));

            dbParameters.Add(new DBParameter { Name = DeleteLoginEventSettingDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = DeleteLoginEventSettingDetailPara.ParaField.LOGIN_EVENT_ID, Value = para.LoginEventID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumDeleteLoginEventSettingDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion
    }
}