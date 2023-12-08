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
    public class EntitySystemLoginEventSetting : EntitySys
    {
        public EntitySystemLoginEventSetting(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 更新應用系統登入事件列表排序 -
        public class LoginEventSettingValue : ValueListRow
        {
            public enum ValueField
            {
                LOGIN_EVENT_ID,
                SORT_ORDER
            }

            public string LoginEventID { get; set; }
            public string BeforeSortOrder { get; set; }
            public string AfterSortOrder { get; set; }

            public DBVarChar GetLoginEventID()
            {
                return new DBVarChar(LoginEventID);
            }
            public DBVarChar GetAfterSortOrder()
            {
                return new DBVarChar(AfterSortOrder);
            }
        }

        public class SysLoginEventSettingSortPara : DBCulture
        {
            public SysLoginEventSettingSortPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar UpdUserID;
            public List<LoginEventSettingValue> LoginEventSettingValueList;
        }

        public enum EnumSysLoginEventSettingSortResult
        {
            Success, Failure
        }

        public EnumSysLoginEventSettingSortResult EditSysLoginEventSettingSort(SysLoginEventSettingSortPara para)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            foreach (LoginEventSettingValue loginEventValue in para.LoginEventSettingValueList)
            {
                if (loginEventValue.AfterSortOrder != loginEventValue.BeforeSortOrder)
                {
                    var insertCommand = new StringBuilder(string.Join(Environment.NewLine, new object[]
                    {
                        "UPDATE SYS_SYSTEM_LOGIN_EVENT",
                        "   SET SORT_ORDER = {SORT_ORDER}",
                        "     , UPD_USER_ID = {UPD_USER_ID}",
                        "     , UPD_DT = GETDATE()",
                        " WHERE SYS_ID = {SYS_ID}",
                        "   AND LOGIN_EVENT_ID = {LOGIN_EVENT_ID};"
                    }));

                    dbParameters.Add(new DBParameter { Name = SysLoginEventSettingSortPara.ParaField.SYS_ID, Value = para.SysID });
                    dbParameters.Add(new DBParameter { Name = LoginEventSettingValue.ValueField.LOGIN_EVENT_ID, Value = loginEventValue.GetLoginEventID() });
                    dbParameters.Add(new DBParameter { Name = LoginEventSettingValue.ValueField.SORT_ORDER, Value = loginEventValue.GetAfterSortOrder() });
                    dbParameters.Add(new DBParameter { Name = SysLoginEventSettingSortPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                    commandTextStringBuilder.Append(GetCommandText(ProviderName, insertCommand.ToString(), dbParameters));
                    dbParameters.Clear();
                }
            }

            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1) = 'N';",
                "DECLARE @ERROR_LINE INT;",
                "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                "BEGIN TRANSACTION",
                "    BEGIN TRY",
                commandTextStringBuilder.ToString(),
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

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumSysLoginEventSettingSortResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }

        #endregion

        #region - 取得應用系統登入事件設定清單 -
        public class SysLoginEventSettingPara : DBCulture
        {
            public SysLoginEventSettingPara(string cultureID) : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                SYS_NM,
                LOGIN_EVENT_ID,
                LOGIN_EVENT_NM
            }

            public DBVarChar SysID;
            public DBVarChar LoginEventID;
        }

        public class SysLoginEventSetting : DBTableRow
        {
            public DBVarChar SysID;
            public DBNVarChar SysNMID;
            public DBNVarChar SubSysNMID;
            public DBVarChar LoginEventID;
            public DBNVarChar LoginEventNMID;
            public DBDateTime StartDT;
            public DBDateTime EndDT;
            public DBChar IsDisable;
            public DBVarChar SortOrder;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<SysLoginEventSetting> SelectSysLoginEventSettingList(SysLoginEventSettingPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            List<string> commandWhere = new List<string>();
            
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT E.SYS_ID AS SysID",
                "     , dbo.FN_GET_NMID(E.SYS_ID, M.{SYS_NM}) AS SysNMID",
                "     , (CASE WHEN E.SUB_SYS_ID IS NULL THEN NULL ELSE dbo.FN_GET_NMID(E.SUB_SYS_ID, S.{SYS_NM}) END) AS SubSysNMID",
                "     , E.LOGIN_EVENT_ID AS LoginEventID",
                "     , dbo.FN_GET_NMID(E.LOGIN_EVENT_ID, E.{LOGIN_EVENT_NM}) AS LoginEventNMID",
                "     , E.SUB_SYS_ID AS SubSysID",
                "     , E.START_DT AS StartDT",
                "     , E.END_DT AS EndDT",
                "     , E.IS_DISABLE AS IsDisable",
                "     , E.SORT_ORDER AS SortOrder",
                "     , dbo.FN_GET_USER_NM(E.UPD_USER_ID) AS UpdUserNM",
                "     , E.UPD_DT AS UpdDT",
                "  FROM SYS_SYSTEM_LOGIN_EVENT E",
                "  JOIN SYS_SYSTEM_MAIN M",
                "    ON E.SYS_ID = M.SYS_ID",
                "  LEFT JOIN SYS_SYSTEM_SUB S",
                "   ON E.SYS_ID = S.PARENT_SYS_ID",
                "  AND E.SUB_SYS_ID = S.SYS_ID"
            }));

            if (para.SysID.IsNull() == false)
            {
                commandWhere.Add("E.SYS_ID = {SYS_ID}");
                dbParameters.Add(new DBParameter { Name = SysLoginEventSettingPara.ParaField.SYS_ID, Value = para.SysID });
            }

            if (para.LoginEventID.IsNull() == false)
            {
                commandWhere.Add("E.LOGIN_EVENT_ID = {LOGIN_EVENT_ID}");
                dbParameters.Add(new DBParameter { Name = SysLoginEventSettingPara.ParaField.LOGIN_EVENT_ID, Value = para.LoginEventID });
            }

            if (commandWhere.Any())
            {
                commandText.AppendLine(string.Format(" WHERE {0}", string.Join(" AND ", commandWhere)));
            }

            commandText.AppendLine(" ORDER BY E.SORT_ORDER");

            dbParameters.Add(new DBParameter { Name = SysLoginEventSettingPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysLoginEventSettingPara.ParaField.LOGIN_EVENT_NM, Value = para.GetCultureFieldNM(new DBObject(SysLoginEventSettingPara.ParaField.LOGIN_EVENT_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SysLoginEventSettingPara.ParaField.SYS_NM, Value = para.GetCultureFieldNM(new DBObject(SysLoginEventSettingPara.ParaField.SYS_NM.ToString())) });

            return GetEntityList<SysLoginEventSetting>(commandText.ToString(), dbParameters);
        }
        #endregion
    }
}