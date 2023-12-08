using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LionTech.Utility;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySys : DBEntity
    {
#if !NET461
        public EntitySys(string connectionName)
            : base(connectionName)
        {
        }
#endif
        public EntitySys(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public enum EnumSystemRoleID
        {
            IT, USER
        }

        #region - AP -
        #region - 取得使用者Mail -
        public class UserEMailPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public DBVarChar SelectUserEMail(UserEMailPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT USER_EMAIL",
                    "  FROM RAW_CM_USER",
                    " WHERE USER_ID = {USER_ID}",
                    "   AND IS_LEFT = '" + EnumYN.N + "'"
                }));

            dbParameters.Add(new DBParameter { Name = UserEMailPara.ParaField.USER_ID, Value = para.UserID });
            return new DBVarChar(ExecuteScalar(commandText.ToString(), dbParameters));
        }
        #endregion

        #region - 取得登入事件代碼清單 -
        public class SysLoginEventIDPara : DBCulture
        {
            public SysLoginEventIDPara(string cultureID) : base(cultureID)
            {

            }
            public enum ParaField
            {
                SYS_ID,
                LOGIN_EVENT_NM
            }

            public DBVarChar SysID;
        }

        public class SysLoginEventID : DBTableRow, ISelectItem
        {
            public DBVarChar LoginEventID;
            public DBNVarChar LoginEventNM;
            public DBNVarChar LoginEventNMID;

            public string GroupBy()
            {
                return LoginEventID.StringValue();
            }

            public string ItemText()
            {
                return LoginEventNMID.StringValue();
            }

            public string ItemValue()
            {
                return LoginEventID.StringValue();
            }

            public string ItemValue(string key)
            {
                return LoginEventID.StringValue();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysLoginEventID> SelectSysLoginEventIDList(SysLoginEventIDPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT LOGIN_EVENT_ID AS LoginEventID",
                "     , {LOGIN_EVENT_NM} AS LoginEventNM",
                "     , dbo.FN_GET_NMID(LOGIN_EVENT_ID, {LOGIN_EVENT_NM}) AS LoginEventNMID",
                "  FROM SYS_SYSTEM_LOGIN_EVENT",
                " WHERE SYS_ID = {SYS_ID}"
            }));

            dbParameters.Add(new DBParameter { Name = SysLoginEventIDPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysLoginEventIDPara.ParaField.LOGIN_EVENT_NM, Value = para.GetCultureFieldNM(new DBObject(SysLoginEventIDPara.ParaField.LOGIN_EVENT_NM.ToString())) });

            return GetEntityList<SysLoginEventID>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 取得系統功能資訊 -
        public class SysFunRawDataPara : DBCulture
        {
            public SysFunRawDataPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                SYS_NM,
                FUN_CONTROLLER_ID,
                FUN_GROUP,
                FUN_ACTION_NAME,
                FUN_NM
            }

            public List<SysFunGroup> SysFunGroupList;
        }

        public class SysFunGroup
        {
            public DBVarChar SysID;
            public DBVarChar ControllerID;
            public DBVarChar ActionID;
        }

        public class SysFunRawData : DBTableRow
        {
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar FunControllerID;
            public DBNVarChar FunControllerNMID;
            public DBNVarChar FunControllerNM;
            public DBVarChar FunActionID;
            public DBNVarChar FunActionNM;
            public DBNVarChar FunActionNMID;
        }

        public List<SysFunRawData> SelectSysFunRawDataList(SysFunRawDataPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            List<string> commandWhere = new List<string>();

            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT DISTINCT U.SYS_ID AS SysID",
                "     , M.{SYS_NM} AS SysNM",
                "	  , G.FUN_CONTROLLER_ID AS FunControllerID",
                "	  , dbo.FN_GET_NMID(G.FUN_CONTROLLER_ID, G.{FUN_GROUP}) AS FunControllerNMID",
                "	  , G.{FUN_GROUP} AS FunControllerNM",
                "	  , F.FUN_ACTION_NAME AS FunActionID",
                "	  , F.{FUN_NM} AS FunActionNM",
                "	  , dbo.FN_GET_NMID(F.FUN_ACTION_NAME, F.{FUN_NM}) AS FunActionNMID",
                "  FROM SYS_USER_SYSTEM U",
                "  JOIN SYS_SYSTEM_MAIN M",
                "    ON U.SYS_ID = M.SYS_ID",
                "  JOIN SYS_SYSTEM_FUN_GROUP G",
                "    ON U.SYS_ID = G.SYS_ID",
                "  JOIN SYS_SYSTEM_FUN F",
                "    ON U.SYS_ID = F.SYS_ID",
                " WHERE M.IS_OUTSOURCING = '" + EnumYN.N + "'",
                "   AND G.FUN_CONTROLLER_ID = F.FUN_CONTROLLER_ID"
            }));

            foreach (var fun in para.SysFunGroupList)
            {
                commandWhere.Add(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                    "(U.SYS_ID = {SYS_ID} AND G.FUN_CONTROLLER_ID = {FUN_CONTROLLER_ID} AND F.FUN_ACTION_NAME = {FUN_ACTION_NAME})"
                    ),
                    new List<DBParameter>
                    {
                        new DBParameter { Name = SysFunRawDataPara.ParaField.SYS_ID, Value = fun.SysID },
                        new DBParameter { Name = SysFunRawDataPara.ParaField.FUN_CONTROLLER_ID, Value = fun.ControllerID },
                        new DBParameter { Name = SysFunRawDataPara.ParaField.FUN_ACTION_NAME, Value = fun.ActionID },
                    }));
            }

            dbParameters.Add(new DBParameter { Name = SysFunRawDataPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysFunRawDataPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SysFunRawDataPara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysFunRawDataPara.ParaField.FUN_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SysFunRawDataPara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysFunRawDataPara.ParaField.FUN_NM.ToString())) });

            if (commandWhere.Any())
            {
                commandText.AppendLine(string.Format(" AND ({0})", string.Join(" OR ", commandWhere)));
            }
            return GetEntityList<SysFunRawData>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 取得Line代碼-
        public class LineBotIDPara : DBCulture
        {
            public LineBotIDPara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                SYS_ID,
                LINE_NM
            }

            public DBVarChar SysID;
        }

        public class LineBotID : DBTableRow, ISelectItem
        {
            public DBVarChar LineID;
            public DBNVarChar LineNM;
            public DBNVarChar LineNMID;

            public string GroupBy()
            {
                return LineID.StringValue();
            }

            public string ItemText()
            {
                return LineNMID.StringValue();
            }

            public string ItemValue(string key)
            {
                return LineID.StringValue();
            }

            public string ItemValue()
            {
                return LineID.StringValue();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }
        }

        public List<LineBotID> SelectLineBotIDList(LineBotIDPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT LINE_ID AS LineID",
                    "     , {LINE_NM} AS LineNM",
                    "     , dbo.FN_GET_NMID(LINE_ID, {LINE_NM}) AS LineNMID",
                    "  FROM SYS_SYSTEM_LINE",
                    " WHERE SYS_ID = {SYS_ID}"
                }));

            dbParameters.Add(new DBParameter { Name = LineBotIDPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LineBotIDPara.ParaField.LINE_NM, Value = para.GetCultureFieldNM(new DBObject(LineBotIDPara.ParaField.LINE_NM)) });
            return GetEntityList<LineBotID>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 取得使用者姓名 -
        public class UserRawDataPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
            public List<DBVarChar> UserIDList;
        }

        public class UserRawData : DBTableRow
        {
            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar RoleGroupID;
            public DBChar IsDisable;
        }

        public UserRawData SelectUserRawData(UserRawDataPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT USER_ID AS UserID",
                "     , dbo.FN_GET_USER_NM(USER_ID) AS UserNM",
                "     , IS_DISABLE AS IsDisable",
                "     , ROLE_GROUP_ID AS RoleGroupID",
                "  FROM SYS_USER_MAIN",
                " WHERE USER_ID={USER_ID}"
            }));

            dbParameters.Add(new DBParameter { Name = UserRawDataPara.ParaField.USER_ID, Value = para.UserID });
            return GetEntityList<UserRawData>(commandText.ToString(), dbParameters).SingleOrDefault();
        }

        public List<UserRawData> SelectUserRawDataList(UserRawDataPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT USER_ID AS UserID",
                "     , dbo.FN_GET_USER_NM(USER_ID) AS UserNM",
                "     , IS_DISABLE AS IsDisable",
                "     , ROLE_GROUP_ID AS RoleGroupID",
                "  FROM SYS_USER_MAIN",
                " WHERE USER_ID IN ({USER_ID})"
            }));

            dbParameters.Add(new DBParameter { Name = UserRawDataPara.ParaField.USER_ID, Value = para.UserIDList });
            return GetEntityList<UserRawData>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 查詢是否Serp IT管理者 -
        public class IsITManagerPara
        {
            public enum ParaField
            {
                USER_ID,
                SYS_ID
            }
            public DBVarChar UserID;
            public DBVarChar SysID;
        }

        public DBChar SelectIsITManager(IsITManagerPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT AS CHAR(1) = 'N'",
                    "IF EXISTS(",
                    "    SELECT 'Y'",
                    "      FROM SYS_SYSTEM_MAIN",
                    "     WHERE SYS_ID = {SYS_ID}",
                    "       AND SYS_MAN_USER_ID = {USER_ID}",
                    "     UNION ",
                    "    SELECT 'Y'",
                    "      FROM SYS_USER_SYSTEM_ROLE",
                    "     WHERE USER_ID = {USER_ID}",
                    "       AND SYS_ID = {SYS_ID}",
                    "       AND ROLE_ID = '" + EnumSystemRoleID.IT + "')",
                    "BEGIN",
                    "    SET @RESULT = 'Y';",
                    "END",
                    "SELECT @RESULT;"
                }));

            dbParameters.Add(new DBParameter { Name = IsITManagerPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = IsITManagerPara.ParaField.USER_ID, Value = para.UserID });

            return new DBChar(ExecuteScalar(commandText.ToString(), dbParameters));
        }
        #endregion

        #region - 取得系統名稱 -
        public class SysSystemMainPara : DBCulture
        {
            public SysSystemMainPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                SYS_NM
            }

            public DBVarChar SysID;
        }

        public class SysSystemMain : DBTableRow
        {
            public DBNVarChar SysNM;
            public DBNVarChar SysNMID;
        }

        public SysSystemMain SelectSysSystemMain(SysSystemMainPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT {SYS_NM} AS SysNM",
                "     , dbo.FN_GET_NMID(SYS_ID, {SYS_NM}) AS SysNMID",
                "  FROM SYS_SYSTEM_MAIN",
                " WHERE SYS_ID = {SYS_ID}"
            }));

            dbParameters.Add(new DBParameter { Name = SysSystemMainPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemMainPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemMainPara.ParaField.SYS_NM.ToString())) });

            return GetEntityList<SysSystemMain>(commandText.ToString(), dbParameters).SingleOrDefault();
        }
        #endregion

        public class SysUserSystemSysIDPara : DBCulture
        {
            public SysUserSystemSysIDPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                USER_ID, SYS_NM
            }

            public DBVarChar UserID;
        }

        public class SysUserSystemSysID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                SYS_ID, SYS_NM
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public string ItemText()
            {
                return this.SysNM.StringValue();
            }

            public string ItemValue()
            {
                return this.SysID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysUserSystemSysID> SelectSysUserSystemSysIDList(SysUserSystemSysIDPara para, bool excludeOutsourcing)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT DISTINCT M.SYS_ID AS SysID",
                    "     , (CASE WHEN M.IS_OUTSOURCING='N' THEN '' ELSE '*' END)+dbo.FN_GET_NMID(M.SYS_ID, M.{SYS_NM}) AS SysNM",
                    "     , M.SORT_ORDER ",
                    "  FROM SYS_USER_SYSTEM S",
                    "  JOIN SYS_SYSTEM_MAIN M",
                    "    ON S.SYS_ID=M.SYS_ID",
                    "  LEFT JOIN SYS_USER_SYSTEM_ROLE R",
                    "    ON S.USER_ID=R.USER_ID",
                    "   AND S.SYS_ID=R.SYS_ID",
                    " WHERE S.USER_ID={USER_ID}",
                    "   AND ((M.IS_OUTSOURCING='Y' AND M.SYS_MAN_USER_ID={USER_ID}) OR (M.IS_OUTSOURCING='N' AND R.ROLE_ID = '"+ EnumSystemRoleID.IT +"'))"
                }));

            if (excludeOutsourcing)
            {
                commandText.AppendLine(string.Join(Environment.NewLine,
                    new object[]
                    {
                        " AND M.IS_OUTSOURCING='N' "
                    }));
            }

            commandText.AppendLine("ORDER BY M.SORT_ORDER");

            dbParameters.Add(new DBParameter { Name = SysUserSystemSysIDPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = SysUserSystemSysIDPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysUserSystemSysIDPara.ParaField.SYS_NM.ToString())) });

            return GetEntityList<SysUserSystemSysID>(commandText.ToString(), dbParameters);
        }

        public class SystemSysIDPara : DBCulture
        {
            public SystemSysIDPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_NM
            }
        }

        public class SystemSysID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                SYS_ID, SYS_NM
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public string ItemText()
            {
                return this.SysNM.StringValue();
            }

            public string ItemValue()
            {
                return this.SysID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SystemSysID> SelectSystemSysIDList(SystemSysIDPara para, bool excludeOutsourcing)
        {
            string commandWhere = string.Empty;

            if (excludeOutsourcing)
            {
                commandWhere = string.Concat(new object[]
                {
                    "WHERE IS_OUTSOURCING='N' ", Environment.NewLine
                });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID ", Environment.NewLine,
                "     , (CASE WHEN IS_OUTSOURCING='N' THEN '' ELSE '*' END)+dbo.FN_GET_NMID(SYS_ID, {SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_MAIN ", Environment.NewLine,
                commandWhere, Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemSysIDPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemSysIDPara.ParaField.SYS_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemSysID> systemSysIDList = new List<SystemSysID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemSysID systemSysID = new SystemSysID()
                    {
                        SysID = new DBVarChar(dataRow[SystemSysID.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemSysID.DataField.SYS_NM.ToString()])
                    };
                    systemSysIDList.Add(systemSysID);
                }
                return systemSysIDList;
            }
            return null;
        }

        public class SystemSubsysIDPara : DBCulture
        {
            public SystemSubsysIDPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_NM, PARENT_SYS_ID
            }

            public DBVarChar ParentSysID;
        }

        public class SystemSubsysID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                SYS_ID, SYS_NM
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public string ItemText()
            {
                return this.SysNM.StringValue();
            }

            public string ItemValue()
            {
                return this.SysID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SystemSubsysID> SelectSystemSubsysIDList(SystemSubsysIDPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID ", Environment.NewLine,
                "     , dbo.FN_GET_NMID(SYS_ID, {SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_SUB ", Environment.NewLine,
                "WHERE PARENT_SYS_ID={PARENT_SYS_ID} ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemSubsysIDPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemSubsysIDPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemSubsysIDPara.ParaField.PARENT_SYS_ID.ToString(), Value = para.ParentSysID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemSubsysID> systemSubsysIDList = new List<SystemSubsysID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemSubsysID systemSubsysID = new SystemSubsysID()
                    {
                        SysID = new DBVarChar(dataRow[SystemSubsysID.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemSubsysID.DataField.SYS_NM.ToString()])
                    };
                    systemSubsysIDList.Add(systemSubsysID);
                }
                return systemSubsysIDList;
            }
            return null;
        }

        public class SysSystemRoleGroupPara : DBCulture
        {
            public SysSystemRoleGroupPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                ROLE_GROUP_NM
            }
        }

        public class SysSystemRoleGroup : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                ROLE_GROUP_ID, ROLE_GROUP_NM
            }

            public DBVarChar RoleGroupID;
            public DBNVarChar RoleGroupNM;

            public string ItemText()
            {
                return this.RoleGroupNM.StringValue();
            }

            public string ItemValue()
            {
                return this.RoleGroupID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysSystemRoleGroup> SelectSysSystemRoleGroupList(SysSystemRoleGroupPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT ROLE_GROUP_ID, dbo.FN_GET_NMID(ROLE_GROUP_ID, ROLE_GROUP_NM_ZH_TW) AS ROLE_GROUP_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_ROLE_GROUP ", Environment.NewLine,
                "ORDER BY SORT_ORDER; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupPara.ParaField.ROLE_GROUP_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemRoleGroupPara.ParaField.ROLE_GROUP_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemRoleGroup> sysSystemRoleGroupList = new List<SysSystemRoleGroup>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemRoleGroup sysSystemRoleGroup = new SysSystemRoleGroup()
                    {
                        RoleGroupID = new DBVarChar(dataRow[SysSystemRoleGroup.DataField.ROLE_GROUP_ID.ToString()]),
                        RoleGroupNM = new DBNVarChar(dataRow[SysSystemRoleGroup.DataField.ROLE_GROUP_NM.ToString()])
                    };
                    sysSystemRoleGroupList.Add(sysSystemRoleGroup);
                }
                return sysSystemRoleGroupList;
            }
            return null;
        }

        public class SysSystemRoleCategoryIDPara : DBCulture
        {
            public SysSystemRoleCategoryIDPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, ROLE_CATEGORY_NM
            }

            public DBVarChar SysID;
        }

        public class SysSystemRoleCategoryID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                ROLE_CATEGORY_ID, ROLE_CATEGORY_NM
            }

            public DBVarChar RoleCategoryID;
            public DBNVarChar RoleCategoryNM;

            public string ItemText()
            {
                return this.RoleCategoryNM.StringValue();
            }

            public string ItemValue()
            {
                return this.RoleCategoryID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysSystemRoleCategoryID> SelectSysSystemRoleCategoryIDList(SysSystemRoleCategoryIDPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT ROLE_CATEGORY_ID ", Environment.NewLine,
                "     , dbo.FN_GET_NMID(ROLE_CATEGORY_ID, {ROLE_CATEGORY_NM}) AS ROLE_CATEGORY_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_ROLE_CATEGORY ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemRoleCategoryIDPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleCategoryIDPara.ParaField.ROLE_CATEGORY_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemRoleCategoryIDPara.ParaField.ROLE_CATEGORY_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemRoleCategoryID> systemRoleCategoryIDList = new List<SysSystemRoleCategoryID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemRoleCategoryID systemRoleCategoryID = new SysSystemRoleCategoryID()
                    {
                        RoleCategoryID = new DBVarChar(dataRow[SysSystemRoleCategoryID.DataField.ROLE_CATEGORY_ID.ToString()]),
                        RoleCategoryNM = new DBNVarChar(dataRow[SysSystemRoleCategoryID.DataField.ROLE_CATEGORY_NM.ToString()])
                    };
                    systemRoleCategoryIDList.Add(systemRoleCategoryID);
                }
                return systemRoleCategoryIDList;
            }
            return null;
        }

        public class SysSystemRoleIDPara : DBCulture
        {
            public SysSystemRoleIDPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                ROLE_NM,
                SYS_ID,
                ROLE_CATEGORY_ID
            }

            public DBVarChar SysID;
            public DBVarChar RoleCategoryID;
        }

        public class SysSystemRoleID : DBTableRow, ISelectItem
        {
            public DBVarChar RoleID;
            public DBNVarChar RoleNMID;

            public string ItemText()
            {
                return RoleNMID.StringValue();
            }

            public string ItemValue()
            {
                return RoleID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysSystemRoleID> SelectSysSystemRoleIDList(SysSystemRoleIDPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();

            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT R.ROLE_ID AS RoleID",
                "     , dbo.FN_GET_NMID(R.ROLE_ID, R.{ROLE_NM}) AS RoleNMID",
                "  FROM SYS_SYSTEM_ROLE R",
                "  LEFT JOIN SYS_SYSTEM_ROLE_CATEGORY C",
                "    ON C.SYS_ID = R.SYS_ID",
                "   AND C.ROLE_CATEGORY_ID = R.ROLE_CATEGORY_ID",
                "WHERE R.SYS_ID = {SYS_ID}"
            }));

            if (para.RoleCategoryID.IsNull() == false)
            {
                commandText.AppendLine("AND R.ROLE_CATEGORY_ID = {ROLE_CATEGORY_ID}");
                dbParameters.Add(new DBParameter { Name = SysSystemRoleIDPara.ParaField.ROLE_CATEGORY_ID.ToString(), Value = para.RoleCategoryID });
            }

            dbParameters.Add(new DBParameter { Name = SysSystemRoleIDPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleIDPara.ParaField.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemRoleIDPara.ParaField.ROLE_NM.ToString())) });

            return GetEntityList<SysSystemRoleID>(commandText.ToString(), dbParameters);
        }

        #region - 取得系統角色預設權限條件名稱 -
        public class SysSystemRoleConditionIDPara : DBCulture
        {
            public SysSystemRoleConditionIDPara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                SYS_ID,
                ROLE_CONDITION_NM
            }

            public DBVarChar SysID;
        }

        public class SysSystemRoleConditionID : DBTableRow, ISelectItem
        {
            public DBVarChar RoleConditionID;
            public DBVarChar RoleConditionNM;

            public string GroupBy()
            {
                return RoleConditionID.StringValue();
            }

            public string ItemText()
            {
                return string.Format("{0} ({1})", RoleConditionNM.StringValue(), RoleConditionID.StringValue());
            }

            public string ItemValue(string key)
            {
                return RoleConditionID.StringValue();
            }

            public string ItemValue()
            {
                return RoleConditionID.StringValue();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 取得系統角色條件代碼
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<SysSystemRoleConditionID> SelectSysSystemConditionIDList(SysSystemRoleConditionIDPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT ROLE_CONDITION_ID AS RoleConditionID",
                    "     , {ROLE_CONDITION_NM} AS RoleConditionNM",
                    "  FROM SYS_SYSTEM_ROLE_CONDITION",
                    " WHERE SYS_ID = {SYS_ID}"
                }));

            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionIDPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionIDPara.ParaField.ROLE_CONDITION_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemRoleConditionIDPara.ParaField.ROLE_CONDITION_NM.ToString())) });

            return GetEntityList<SysSystemRoleConditionID>(commandText.ToString(), dbParameters);
        }
        #endregion

        public class SysSystemFunControllerIDPara : DBCulture
        {
            public SysSystemFunControllerIDPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, FUN_GROUP
            }

            public DBVarChar SysID;
        }

        public class SysSystemFunControllerID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                FUN_CONTROLLER_ID, FUN_GROUP
            }

            public DBVarChar FunControllerID;
            public DBNVarChar FunGroup;

            public string ItemText()
            {
                return this.FunGroup.StringValue();
            }

            public string ItemValue()
            {
                return this.FunControllerID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysSystemFunControllerID> SelectSysSystemFunControllerIDList(SysSystemFunControllerIDPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT FUN_CONTROLLER_ID, dbo.FN_GET_NMID(FUN_CONTROLLER_ID, {FUN_GROUP}) AS FUN_GROUP ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN_GROUP ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SYS_ID, SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemFunControllerIDPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemFunControllerIDPara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemFunControllerIDPara.ParaField.FUN_GROUP.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemFunControllerID> sysSystemFunControllerIDList = new List<SysSystemFunControllerID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemFunControllerID sysSystemFunControllerID = new SysSystemFunControllerID()
                    {
                        FunControllerID = new DBVarChar(dataRow[SysSystemFunControllerID.DataField.FUN_CONTROLLER_ID.ToString()]),
                        FunGroup = new DBNVarChar(dataRow[SysSystemFunControllerID.DataField.FUN_GROUP.ToString()])
                    };
                    sysSystemFunControllerIDList.Add(sysSystemFunControllerID);
                }
                return sysSystemFunControllerIDList;
            }
            return null;
        }

        #region - 取得功能群組清單 -
        public class SystemInfoPara : DBCulture
        {
            public SystemInfoPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                FUN_GROUP,
                FUN_NM,
                SYS_NM
            }
        }

        public class SystemFunController : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBNVarChar FunControllerNM;
        }

        public List<SystemFunController> SelectSystemFunControllerList(SystemInfoPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT G.FUN_CONTROLLER_ID AS FunControllerID",
                "     , dbo.FN_GET_NMID(G.FUN_CONTROLLER_ID, {FUN_GROUP}) AS FunControllerNM ",
                "     , M.SYS_ID AS SysID ",
                "  FROM SYS_SYSTEM_FUN_GROUP G ",
                "  JOIN SYS_SYSTEM_MAIN M ON G.SYS_ID=M.SYS_ID ",
                " ORDER BY G.SYS_ID, G.SORT_ORDER "
            });

            dbParameters.Add(new DBParameter { Name = SysSystemFunControllerIDPara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemFunControllerIDPara.ParaField.FUN_GROUP.ToString())) });
            return GetEntityList<SystemFunController>(commandText, dbParameters);
        }
        #endregion

        #region - 取得功能代碼清單 -
        public class SystemFunAction : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunAction;
            public DBNVarChar FunActionNM;
        }

        public List<SystemFunAction> SelectSystemFunActionList(SystemInfoPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT FUN_ACTION_NAME AS FunAction ",
                "    ,  dbo.FN_GET_NMID(FUN_ACTION_NAME, {FUN_NM}) AS FunActionNM ",
                "    ,  SYS_ID AS SysID ",
                "    ,  FUN_CONTROLLER_ID AS FunControllerID ",
                "FROM SYS_SYSTEM_FUN ",
                "ORDER BY SYS_ID, SORT_ORDER "
            });

            dbParameters.Add(new DBParameter { Name = SystemInfoPara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemInfoPara.ParaField.FUN_NM.ToString())) });
            return GetEntityList<SystemFunAction>(commandText, dbParameters);
        }
        #endregion

        #region - 取得子系統清單 -
        public class SubSys : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar SubSysID;
            public DBNVarChar SubSysNM;
        }

        public List<SubSys> SelectSubSysList(SystemInfoPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT S.SYS_ID AS SubSysID ",
                "     , dbo.FN_GET_NMID(S.SYS_ID, S.{SYS_NM}) AS SubSysNM ",
                "     , M.SYS_ID AS SysID ",
                "  FROM SYS_SYSTEM_SUB S ",
                "  JOIN SYS_SYSTEM_MAIN M ON S.PARENT_SYS_ID=M.SYS_ID ",
                " ORDER BY S.SORT_ORDER "
            });

            dbParameters.Add(new DBParameter { Name = SystemInfoPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemInfoPara.ParaField.SYS_NM.ToString())) });
            return GetEntityList<SubSys>(commandText, dbParameters);
        }
        #endregion

        public class SysSystemFunNamePara : DBCulture
        {
            public SysSystemFunNamePara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, FUN_NM, FUN_CONTROLLER_ID
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
        }

        public class SysSystemFunName : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                FUN_ACTION_NAME, FUN_NM
            }


            public DBVarChar FunActionName;
            public DBVarChar FunName;

            public string ItemText()
            {
                return this.FunName.StringValue();
            }

            public string ItemValue()
            {
                return this.FunActionName.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysSystemFunName> SelectSysSystemFunNameList(SysSystemFunNamePara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT FUN_ACTION_NAME, dbo.FN_GET_NMID(FUN_ACTION_NAME, {FUN_NM}) AS FUN_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} ", Environment.NewLine,
                "ORDER BY SYS_ID, SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemFunNamePara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemFunNamePara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SysSystemFunNamePara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemFunNamePara.ParaField.FUN_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemFunName> sysSystemFunNameList = new List<SysSystemFunName>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemFunName sysSystemFunName = new SysSystemFunName()
                    {
                        FunActionName = new DBVarChar(dataRow[SysSystemFunName.DataField.FUN_ACTION_NAME.ToString()]),
                        FunName = new DBVarChar(dataRow[SysSystemFunName.DataField.FUN_NM.ToString()])

                    };
                    sysSystemFunNameList.Add(sysSystemFunName);
                }
                return sysSystemFunNameList;
            }
            return null;
        }

        public class SysSystemPurviewIDPara : DBCulture
        {
            public SysSystemPurviewIDPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, PURVIEW_NM
            }

            public DBVarChar SysID;
        }

        public class SysSystemPurviewID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                PURVIEW_ID, PURVIEW_NM
            }

            public DBVarChar PurviewID;
            public DBNVarChar PurviewNM;

            public string ItemText()
            {
                return this.PurviewNM.StringValue();
            }

            public string ItemValue()
            {
                return this.PurviewID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysSystemPurviewID> SelectSysSystemPurviewIDList(SysSystemPurviewIDPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT PURVIEW_ID, dbo.FN_GET_NMID(PURVIEW_ID, {PURVIEW_NM}) AS PURVIEW_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_PURVIEW ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SYS_ID, SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemPurviewIDPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemPurviewIDPara.ParaField.PURVIEW_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemPurviewIDPara.ParaField.PURVIEW_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemPurviewID> sysSystemPurviewIDList = new List<SysSystemPurviewID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemPurviewID sysSystemPurviewID = new SysSystemPurviewID()
                    {
                        PurviewID = new DBVarChar(dataRow[SysSystemPurviewID.DataField.PURVIEW_ID.ToString()]),
                        PurviewNM = new DBNVarChar(dataRow[SysSystemPurviewID.DataField.PURVIEW_NM.ToString()])
                    };
                    sysSystemPurviewIDList.Add(sysSystemPurviewID);
                }
                return sysSystemPurviewIDList;
            }
            return null;
        }

        public class SysSystemFunMenuPara : DBCulture
        {
            public SysSystemFunMenuPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, FUN_MENU_NM
            }

            public DBVarChar SysID;
        }

        public class SysSystemFunMenu : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                FUN_MENU, FUN_MENU_NM
            }

            public DBVarChar FunMenu;
            public DBNVarChar FunMenuNM;

            public string ItemText()
            {
                return this.FunMenuNM.StringValue();
            }

            public string ItemValue()
            {
                return this.FunMenu.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysSystemFunMenu> SelectSysSystemFunMenuList(SysSystemFunMenuPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT FUN_MENU, dbo.FN_GET_NMID(FUN_MENU, {FUN_MENU_NM}) AS FUN_MENU_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN_MENU ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SYS_ID, SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemFunMenuPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemFunMenuPara.ParaField.FUN_MENU_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemFunMenuPara.ParaField.FUN_MENU_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemFunMenu> sysSystemFunMenuList = new List<SysSystemFunMenu>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemFunMenu sysSystemFunMenu = new SysSystemFunMenu()
                    {
                        FunMenu = new DBVarChar(dataRow[SysSystemFunMenu.DataField.FUN_MENU.ToString()]),
                        FunMenuNM = new DBNVarChar(dataRow[SysSystemFunMenu.DataField.FUN_MENU_NM.ToString()])
                    };
                    sysSystemFunMenuList.Add(sysSystemFunMenu);
                }
                return sysSystemFunMenuList;
            }
            return null;
        }

        public class SystemMenuFunPara : DBCulture
        {
            public SystemMenuFunPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                FUN_CONTROLLER_ID, FUN_ACTION_NAME,
                FUN_MENU_SYS_ID, FUN_MENU,

                FUN_MENU_NM
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
            public DBVarChar FunMenuSysID;
            public DBVarChar FunMenu;
        }

        public class SystemMenuFun : DBTableRow
        {
            public enum DataField
            {
                SYS_ID,
                FUN_MENU_SYS_ID,
                FUN_MENU, FUN_MENU_NM,
                FUN_CONTROLLER_ID, FUN_ACTION_NAME,
                FUN_MENU_XAXIS, FUN_MENU_YAXIS,
                FUN_MENU_IS_DISABLE
            }

            public DBVarChar SysID;

            public DBVarChar FunMenuSysID;

            public DBVarChar FunMenu;
            public DBNVarChar FunMenuNM;

            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;

            public DBVarChar FunMenuXAxis;
            public DBVarChar FunMenuYAxis;

            public DBChar FunMenuIsDisable;
        }

        public List<SystemMenuFun> SelectSystemMenuFunList(SystemMenuFunPara para)
        {
            #region - commandWhere -
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.FunControllerID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND F.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.FunActionName.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND F.FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.FunMenuSysID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND F.FUN_MENU_SYS_ID={FUN_MENU_SYS_ID} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.FunMenu.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND F.FUN_MENU={FUN_MENU} ", Environment.NewLine });
            }
            #endregion

            string commandText = string.Concat(new object[]
            {
                "SELECT F.SYS_ID ", Environment.NewLine,
                "     , F.FUN_MENU_SYS_ID ", Environment.NewLine,
                "     , F.FUN_MENU, dbo.FN_GET_NMID(F.FUN_MENU, M.{FUN_MENU_NM}) AS FUN_MENU_NM ", Environment.NewLine,
                "     , F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME ", Environment.NewLine,
                "     , F.FUN_MENU_XAXIS, F.FUN_MENU_YAXIS ", Environment.NewLine,
                "     , M.IS_DISABLE AS FUN_MENU_IS_DISABLE ", Environment.NewLine,
                "FROM SYS_SYSTEM_MENU_FUN F ", Environment.NewLine,
                "JOIN SYS_SYSTEM_FUN_MENU M ON F.FUN_MENU_SYS_ID=M.SYS_ID AND F.FUN_MENU=M.FUN_MENU ", Environment.NewLine,
                "WHERE F.SYS_ID={SYS_ID} ", Environment.NewLine,
                commandWhere,
                "ORDER BY F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemMenuFunPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemMenuFunPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemMenuFunPara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = SystemMenuFunPara.ParaField.FUN_MENU_SYS_ID.ToString(), Value = para.FunMenuSysID });
            dbParameters.Add(new DBParameter { Name = SystemMenuFunPara.ParaField.FUN_MENU.ToString(), Value = para.FunMenu });
            dbParameters.Add(new DBParameter { Name = SystemMenuFunPara.ParaField.FUN_MENU_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemMenuFunPara.ParaField.FUN_MENU_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemMenuFun> systemMenuFunList = new List<SystemMenuFun>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemMenuFun systemMenuFun = new SystemMenuFun()
                    {
                        SysID = new DBVarChar(dataRow[SystemMenuFun.DataField.SYS_ID.ToString()]),

                        FunMenuSysID = new DBVarChar(dataRow[SystemMenuFun.DataField.FUN_MENU_SYS_ID.ToString()]),

                        FunMenu = new DBVarChar(dataRow[SystemMenuFun.DataField.FUN_MENU.ToString()]),
                        FunMenuNM = new DBNVarChar(dataRow[SystemMenuFun.DataField.FUN_MENU_NM.ToString()]),

                        FunControllerID = new DBVarChar(dataRow[SystemMenuFun.DataField.FUN_CONTROLLER_ID.ToString()]),
                        FunActionName = new DBVarChar(dataRow[SystemMenuFun.DataField.FUN_ACTION_NAME.ToString()]),

                        FunMenuXAxis = new DBVarChar(dataRow[SystemMenuFun.DataField.FUN_MENU_XAXIS.ToString()]),
                        FunMenuYAxis = new DBVarChar(dataRow[SystemMenuFun.DataField.FUN_MENU_YAXIS.ToString()]),

                        FunMenuIsDisable = new DBChar(dataRow[SystemMenuFun.DataField.FUN_MENU_IS_DISABLE.ToString()])
                    };
                    systemMenuFunList.Add(systemMenuFun);
                }
                return systemMenuFunList;
            }
            return null;
        }

        public class SysSystemFunTypePara : DBCulture
        {
            public SysSystemFunTypePara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                CODE_NM
            }
        }

        public class SysSystemFunType : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                CODE_ID, CODE_NM
            }

            public DBVarChar CodeID;
            public DBNVarChar CodeNM;

            public string ItemText()
            {
                return this.CodeNM.StringValue();
            }

            public string ItemValue()
            {
                return this.CodeID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysSystemFunType> SelectSysSystemFunTypeList(SysSystemFunTypePara para)
        {
            string commandText = string.Concat(new object[]{
                "SELECT CODE_ID, dbo.FN_GET_NMID(CODE_ID, {CODE_NM}) AS CODE_NM ", Environment.NewLine,
                "FROM CM_CODE ", Environment.NewLine,
                "WHERE CODE_KIND='0011' ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemFunTypePara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemFunTypePara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemFunType> sysSystemFunTypeList = new List<SysSystemFunType>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemFunType sysSystemFunType = new SysSystemFunType()
                    {
                        CodeID = new DBVarChar(dataRow[SysSystemFunType.DataField.CODE_ID.ToString()]),
                        CodeNM = new DBNVarChar(dataRow[SysSystemFunType.DataField.CODE_NM.ToString()])
                    };
                    sysSystemFunTypeList.Add(sysSystemFunType);
                }
                return sysSystemFunTypeList;
            }
            return null;
        }

        public class DomainGroupMenuPara : DBCulture
        {
            public DomainGroupMenuPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                DOMAIN_NAME,
                DOMAIN_GROUP_NM,
            }
            public DBVarChar DomainName;
        }

        public class DomainGroupMenu : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                DOMAIN_GROUP_ID,
                DOMAIN_GROUP_NM
            }

            public DBVarChar DomainGroupID;
            public DBNVarChar DomainGroupNM;

            public string ItemText()
            {
                return this.DomainGroupNM.StringValue();
            }

            public string ItemValue()
            {
                return this.DomainGroupID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<DomainGroupMenu> SelectDomainGroupMenuList(DomainGroupMenuPara para)
        {
            string commandWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(para.DomainName.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                    {
                        commandWhere, "WHERE DOMAIN_NAME ={DOMAIN_NAME} ", Environment.NewLine
                    });
            }

            string commandText = string.Concat(new object[]
                {
                    "SELECT DOMAIN_GROUP_ID", Environment.NewLine,
                    "	   , dbo.FN_GET_NMID(DOMAIN_GROUP_ID, {DOMAIN_GROUP_NM}) AS DOMAIN_GROUP_NM", Environment.NewLine,
                    "FROM SYS_DOMAIN_GROUP", Environment.NewLine,
                    "WHERE DOMAIN_NAME ={DOMAIN_NAME} ", Environment.NewLine,
                });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = DomainGroupMenuPara.ParaField.DOMAIN_GROUP_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(DomainGroupMenuPara.ParaField.DOMAIN_GROUP_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = DomainGroupMenuPara.ParaField.DOMAIN_NAME.ToString(), Value = para.DomainName });




            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<DomainGroupMenu> domainGroupMenuList = new List<DomainGroupMenu>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DomainGroupMenu domainGroupMenu = new DomainGroupMenu()
                    {
                        DomainGroupID = new DBVarChar(dataRow[DomainGroupMenu.DataField.DOMAIN_GROUP_ID.ToString()]),
                        DomainGroupNM = new DBNVarChar(dataRow[DomainGroupMenu.DataField.DOMAIN_GROUP_NM.ToString()]),
                    };
                    domainGroupMenuList.Add(domainGroupMenu);
                }
                return domainGroupMenuList;
            }
            return null;
        }
        #endregion

        #region - API -
        public class SysSystemAPIGroupPara : DBCulture
        {
            public SysSystemAPIGroupPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, API_GROUP
            }

            public DBVarChar SysID;
        }

        public class SysSystemAPIGroup : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                API_CONTROLLER_ID, API_GROUP_NM
            }

            public DBVarChar APIControllerID;
            public DBNVarChar APIGroupNM;

            public string ItemText()
            {
                return this.APIGroupNM.StringValue();
            }

            public string ItemValue()
            {
                return this.APIControllerID.GetValue();
            }

            public string ItemValue(string key)
            {
                throw new System.NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new System.NotImplementedException();
            }

            public string GroupBy()
            {
                throw new System.NotImplementedException();
            }
        }

        public List<SysSystemAPIGroup> SelectSystemAPIGroupList(SysSystemAPIGroupPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT API_CONTROLLER_ID, dbo.FN_GET_NMID(API_CONTROLLER_ID, {API_GROUP}) AS API_GROUP_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_API_GROUP ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemAPIGroupPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemAPIGroupPara.ParaField.API_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemAPIGroupPara.ParaField.API_GROUP.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemAPIGroup> systemAPIGroupList = new List<SysSystemAPIGroup>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemAPIGroup systemAPIGroup = new SysSystemAPIGroup()
                    {
                        APIControllerID = new DBVarChar(dataRow[SysSystemAPIGroup.DataField.API_CONTROLLER_ID.ToString()]),
                        APIGroupNM = new DBNVarChar(dataRow[SysSystemAPIGroup.DataField.API_GROUP_NM.ToString()])
                    };
                    systemAPIGroupList.Add(systemAPIGroup);
                }
                return systemAPIGroupList;
            }
            return null;
        }

        public class SysSystemAPIFuntionPara : DBCulture
        {
            public SysSystemAPIFuntionPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, API_CONTROLLER_ID,
                API_NM
            }

            public DBVarChar SysID;
            public DBVarChar APIGroup;
        }

        public class SysSystemAPIFuntion : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                API_FUN, API_FUN_NM
            }

            public DBVarChar APIFun;
            public DBNVarChar APIFunNM;

            public string ItemValue()
            {
                return this.APIFun.GetValue();
            }

            public string ItemText()
            {
                return this.APIFunNM.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new System.NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new System.NotImplementedException();
            }

            public string GroupBy()
            {
                throw new System.NotImplementedException();
            }
        }

        public List<SysSystemAPIFuntion> SelectSystemAPIFuntionList(SysSystemAPIFuntionPara para)
        {
            string commandText = String.Concat(new object[]
            {
                "SELECT API_ACTION_NAME AS API_FUN ", Environment.NewLine,
                "     , dbo.FN_GET_NMID(API_ACTION_NAME, {API_NM}) AS API_FUN_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_API ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "AND API_CONTROLLER_ID={API_CONTROLLER_ID} ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemAPIFuntionPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemAPIFuntionPara.ParaField.API_CONTROLLER_ID.ToString(), Value = para.APIGroup });
            dbParameters.Add(new DBParameter { Name = SysSystemAPIFuntionPara.ParaField.API_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemAPIPara.ParaField.API_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemAPIFuntion> apiFuntionList = new List<SysSystemAPIFuntion>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemAPIFuntion apiFuntion = new SysSystemAPIFuntion()
                    {
                        APIFun = new DBVarChar(dataRow[SysSystemAPIFuntion.DataField.API_FUN.ToString()]),
                        APIFunNM = new DBNVarChar(dataRow[SysSystemAPIFuntion.DataField.API_FUN_NM.ToString()])
                    };
                    apiFuntionList.Add(apiFuntion);
                }
                return apiFuntionList;
            }
            return null;
        }

        public class SystemAPIPara : DBCulture
        {
            public SystemAPIPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, API_CONTROLLER_ID, API_ACTION_NAME,
                SYS_NM, API_GROUP, API_NM
            }

            public DBVarChar SysID;
            public DBVarChar APIGroup;
            public DBVarChar APIFun;
        }

        public class SystemAPI : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM,
                API_GROUP, API_GROUP_NM,
                API_FUN, API_FUN_NM,
                API_PARA
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public DBVarChar APIGroup;
            public DBNVarChar APIGroupNM;

            public DBVarChar APIFun;
            public DBNVarChar APIFunNM;

            public DBVarChar APIPara;
        }

        public SystemAPI SelectSystemAPI(SystemAPIPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT A.SYS_ID, dbo.FN_GET_NMID(A.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , A.API_CONTROLLER_ID AS API_GROUP, dbo.FN_GET_NMID(A.API_CONTROLLER_ID, G.{API_GROUP}) AS API_GROUP_NM ", Environment.NewLine,
                "     , A.API_ACTION_NAME AS API_FUN, dbo.FN_GET_NMID(A.API_ACTION_NAME, A.{API_NM}) AS API_FUN_NM ", Environment.NewLine,
                "     , A.API_PARA ", Environment.NewLine,
                "FROM SYS_SYSTEM_API A ", Environment.NewLine,
                "JOIN SYS_SYSTEM_API_GROUP G ON A.SYS_ID=G.SYS_ID AND A.API_CONTROLLER_ID=G.API_CONTROLLER_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON A.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "WHERE A.SYS_ID={SYS_ID} AND A.API_CONTROLLER_ID={API_CONTROLLER_ID} AND A.API_ACTION_NAME={API_ACTION_NAME} "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemAPIPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemAPIPara.ParaField.API_CONTROLLER_ID.ToString(), Value = para.APIGroup });
            dbParameters.Add(new DBParameter { Name = SystemAPIPara.ParaField.API_ACTION_NAME.ToString(), Value = para.APIFun });
            dbParameters.Add(new DBParameter { Name = SystemAPIPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemAPIPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemAPIPara.ParaField.API_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemAPIPara.ParaField.API_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemAPIPara.ParaField.API_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemAPIPara.ParaField.API_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                SystemAPI systemAPI = new SystemAPI()
                {
                    SysID = new DBVarChar(dataTable.Rows[0][SystemAPI.DataField.SYS_ID.ToString()]),
                    SysNM = new DBNVarChar(dataTable.Rows[0][SystemAPI.DataField.SYS_NM.ToString()]),
                    APIGroup = new DBVarChar(dataTable.Rows[0][SystemAPI.DataField.API_GROUP.ToString()]),
                    APIGroupNM = new DBNVarChar(dataTable.Rows[0][SystemAPI.DataField.API_GROUP_NM.ToString()]),
                    APIFun = new DBVarChar(dataTable.Rows[0][SystemAPI.DataField.API_FUN.ToString()]),
                    APIFunNM = new DBNVarChar(dataTable.Rows[0][SystemAPI.DataField.API_FUN_NM.ToString()]),
                    APIPara = new DBVarChar(dataTable.Rows[0][SystemAPI.DataField.API_PARA.ToString()])
                };
                return systemAPI;
            }
            return null;
        }
        #endregion

        #region - Event -
        public class SystemEventTargetPara
        {
            public enum ParaField
            {
                SYS_ID, EVENT_GROUP_ID, EVENT_ID
            }

            public DBVarChar SysID;
            public DBVarChar EventGroupID;
            public DBVarChar EventID;
        }

        public class SystemEventTarget : DBTableRow
        {
            public enum DataField
            {
                SYS_ID
            }

            public DBVarChar SysID;
        }

        public List<SystemEventTarget> SelectSystemEventTargetList(SystemEventTargetPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT M.SYS_ID ", Environment.NewLine,
                "FROM SYS_SYSTEM_MAIN M ", Environment.NewLine,
                "JOIN ( ", Environment.NewLine,
                "    SELECT TARGET_SYS_ID ", Environment.NewLine,
                "    FROM SYS_SYSTEM_EVENT_TARGET ", Environment.NewLine,
                "    WHERE SYS_ID={SYS_ID} AND EVENT_GROUP_ID={EVENT_GROUP_ID} AND EVENT_ID={EVENT_ID} ", Environment.NewLine,
                ") T ON M.SYS_ID=T.TARGET_SYS_ID ", Environment.NewLine,
                "WHERE M.IS_DISABLE='N' ", Environment.NewLine,
                "ORDER BY M.SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEventTargetPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEventTargetPara.ParaField.EVENT_GROUP_ID.ToString(), Value = para.EventGroupID });
            dbParameters.Add(new DBParameter { Name = SystemEventTargetPara.ParaField.EVENT_ID.ToString(), Value = para.EventID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemEventTarget> targetList = new List<SystemEventTarget>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemEventTarget target = new SystemEventTarget()
                    {
                        SysID = new DBVarChar(dataRow[SystemEventTarget.DataField.SYS_ID.ToString()]),
                    };
                    targetList.Add(target);
                }
                return targetList;
            }
            return null;
        }

        public class SysSystemEventGroupPara : DBCulture
        {
            public SysSystemEventGroupPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, EVENT_GROUP
            }

            public DBVarChar SysID;
        }

        public class SysSystemEventGroup : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                EVENT_GROUP_ID, EVENT_GROUP
            }

            public DBVarChar EventGroupID;
            public DBNVarChar EventGroup;

            public string ItemText()
            {
                return this.EventGroup.StringValue();
            }

            public string ItemValue()
            {
                return this.EventGroupID.GetValue();
            }

            public string ItemValue(string key)
            {
                throw new System.NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new System.NotImplementedException();
            }

            public string GroupBy()
            {
                throw new System.NotImplementedException();
            }
        }

        public List<SysSystemEventGroup> SelectSystemEventGroupList(SysSystemEventGroupPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT EVENT_GROUP_ID, dbo.FN_GET_NMID(EVENT_GROUP_ID, {EVENT_GROUP}) AS EVENT_GROUP ", Environment.NewLine,
                "FROM SYS_SYSTEM_EVENT_GROUP ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemEventGroupPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemEventGroupPara.ParaField.EVENT_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemEventGroupPara.ParaField.EVENT_GROUP.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemEventGroup> systemEventGroupList = new List<SysSystemEventGroup>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemEventGroup systemEventGroup = new SysSystemEventGroup()
                    {
                        EventGroupID = new DBVarChar(dataRow[SysSystemEventGroup.DataField.EVENT_GROUP_ID.ToString()]),
                        EventGroup = new DBNVarChar(dataRow[SysSystemEventGroup.DataField.EVENT_GROUP.ToString()])
                    };
                    systemEventGroupList.Add(systemEventGroup);
                }
                return systemEventGroupList;
            }
            return null;
        }

        public class SystemEventPara : DBCulture
        {
            public SystemEventPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, EVENT_GROUP_ID, EVENT_ID,
                SYS_NM, EVENT_GROUP, EVENT_NM
            }

            public DBVarChar SysID;
            public DBVarChar EventGroupID;
            public DBVarChar EventID;
        }

        public class SystemEvent : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM,
                EVENT_GROUP_ID, EVENT_GROUP_NM,
                EVENT_ID, EVENT_NM,
                EVENT_PARA,
                REMARK
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public DBVarChar EventGroupID;
            public DBNVarChar EventGroupNM;

            public DBVarChar EventID;
            public DBNVarChar EventNM;

            public DBVarChar EventPara;
            public DBNVarChar Remark;
        }

        public SystemEvent SelectSystemEvent(SystemEventPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT E.SYS_ID, dbo.FN_GET_NMID(E.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , E.EVENT_GROUP_ID, dbo.FN_GET_NMID(E.EVENT_GROUP_ID, G.{EVENT_GROUP}) AS EVENT_GROUP_NM ", Environment.NewLine,
                "     , E.EVENT_ID, dbo.FN_GET_NMID(E.EVENT_ID, E.{EVENT_NM}) AS EVENT_NM ", Environment.NewLine,
                "     , E.EVENT_PARA ", Environment.NewLine,
                "     , E.REMARK ", Environment.NewLine,
                "FROM SYS_SYSTEM_EVENT E ", Environment.NewLine,
                "JOIN SYS_SYSTEM_EVENT_GROUP G ON E.SYS_ID=G.SYS_ID AND E.EVENT_GROUP_ID=G.EVENT_GROUP_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON E.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "WHERE E.SYS_ID={SYS_ID} AND E.EVENT_GROUP_ID={EVENT_GROUP_ID} AND E.EVENT_ID={EVENT_ID} ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEventPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEventPara.ParaField.EVENT_GROUP_ID.ToString(), Value = para.EventGroupID });
            dbParameters.Add(new DBParameter { Name = SystemEventPara.ParaField.EVENT_ID.ToString(), Value = para.EventID });
            dbParameters.Add(new DBParameter { Name = SystemEventPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEventPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEventPara.ParaField.EVENT_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEventPara.ParaField.EVENT_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEventPara.ParaField.EVENT_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEventPara.ParaField.EVENT_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                SystemEvent systemEvent = new SystemEvent()
                {
                    SysID = new DBVarChar(dataTable.Rows[0][SystemEvent.DataField.SYS_ID.ToString()]),
                    SysNM = new DBNVarChar(dataTable.Rows[0][SystemEvent.DataField.SYS_NM.ToString()]),
                    EventGroupID = new DBVarChar(dataTable.Rows[0][SystemEvent.DataField.EVENT_GROUP_ID.ToString()]),
                    EventGroupNM = new DBNVarChar(dataTable.Rows[0][SystemEvent.DataField.EVENT_GROUP_NM.ToString()]),
                    EventID = new DBVarChar(dataTable.Rows[0][SystemEvent.DataField.EVENT_ID.ToString()]),
                    EventNM = new DBNVarChar(dataTable.Rows[0][SystemEvent.DataField.EVENT_NM.ToString()]),
                    EventPara = new DBVarChar(dataTable.Rows[0][SystemEvent.DataField.EVENT_PARA.ToString()]),
                    Remark = new DBNVarChar(dataTable.Rows[0][SystemEvent.DataField.REMARK.ToString()])
                };
                return systemEvent;
            }
            return null;
        }
        #endregion

        #region - EDI -
        public class SysSystemEDIFlowPara : DBCulture
        {
            public SysSystemEDIFlowPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, EDI_FLOW
            }

            public DBVarChar SysID;
        }

        public class SysSystemEDIFlow : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                EDI_FLOW_ID, EDI_FLOW_NM
            }

            public DBVarChar EDIFlowID;
            public DBNVarChar EDIFlowNM;

            public string ItemText()
            {
                return this.EDIFlowNM.StringValue();
            }

            public string ItemValue()
            {
                return this.EDIFlowID.GetValue();
            }

            public string ItemValue(string key)
            {
                throw new System.NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new System.NotImplementedException();
            }

            public string GroupBy()
            {
                throw new System.NotImplementedException();
            }
        }

        public List<SysSystemEDIFlow> SelectSystemEDIFlowList(SysSystemEDIFlowPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT EDI_FLOW_ID AS EDIFlowID ",
                "     , dbo.FN_GET_NMID(EDI_FLOW_ID, {EDI_FLOW}) AS EDIFlowNM ",
                "  FROM SYS_SYSTEM_EDI_FLOW ",
                " WHERE SYS_ID = {SYS_ID} ",
                " ORDER BY SORT_ORDER "
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemEDIFlowPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemEDIFlowPara.ParaField.EDI_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemEDIFlowPara.ParaField.EDI_FLOW.ToString())) });
            return GetEntityList<SysSystemEDIFlow>(commandText, dbParameters);
        }

        public class SysSystemEDIJobPara : DBCulture
        {
            public SysSystemEDIJobPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, EDI_FLOW_ID, EDI_JOB
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
        }

        public class SysSystemEDIJob : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                EDI_JOB_ID, EDI_JOB_NM
            }

            public DBVarChar EDIJobID;
            public DBNVarChar EDIJobNM;

            public string ItemText()
            {
                return this.EDIJobNM.StringValue();
            }

            public string ItemValue()
            {
                return this.EDIJobID.GetValue();
            }

            public string ItemValue(string key)
            {
                throw new System.NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new System.NotImplementedException();
            }

            public string GroupBy()
            {
                throw new System.NotImplementedException();
            }
        }

        public List<SysSystemEDIJob> SelectSystemEDIJobList(SysSystemEDIJobPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT EDI_JOB_ID, dbo.FN_GET_NMID(EDI_JOB_ID, {EDI_JOB}) AS EDI_JOB_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_JOB ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemEDIJobPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemEDIJobPara.ParaField.EDI_FLOW_ID.ToString(), Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SysSystemEDIJobPara.ParaField.EDI_JOB.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemEDIJobPara.ParaField.EDI_JOB.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemEDIJob> systemEDIJobList = new List<SysSystemEDIJob>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemEDIJob systemEDIJob = new SysSystemEDIJob()
                    {
                        EDIJobID = new DBVarChar(dataRow[SysSystemEDIJob.DataField.EDI_JOB_ID.ToString()]),
                        EDIJobNM = new DBNVarChar(dataRow[SysSystemEDIJob.DataField.EDI_JOB_NM.ToString()])
                    };
                    systemEDIJobList.Add(systemEDIJob);
                }
                return systemEDIJobList;
            }
            return null;
        }

        public enum SCHFrequencyField
        {
            Continuity, Daily, FixedTime, Monthly, Weekly
        }

        public class SCHFrequencyPara : DBCulture
        {
            public SCHFrequencyPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                CODE_NM
            }
        }

        public class SCHFrequency : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                CODE_ID, CODE_NM
            }

            public DBVarChar CodeID;
            public DBNVarChar CodeNM;

            public string ItemText()
            {
                return this.CodeNM.StringValue();
            }

            public string ItemValue()
            {
                return this.CodeID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SCHFrequency> SelectSCHFrequencyList(SCHFrequencyPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT CODE_ID, dbo.FN_GET_NMID(CODE_ID, {CODE_NM}) AS CODE_NM ", Environment.NewLine,
                "FROM CM_CODE ", Environment.NewLine,
                "WHERE CODE_KIND='0005' ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SCHFrequencyPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SCHFrequencyPara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SCHFrequency> ediJobTypeList = new List<SCHFrequency>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SCHFrequency ediJobType = new SCHFrequency()
                    {
                        CodeID = new DBVarChar(dataRow[SCHFrequency.DataField.CODE_ID.ToString()]),
                        CodeNM = new DBNVarChar(dataRow[SCHFrequency.DataField.CODE_NM.ToString()])
                    };
                    ediJobTypeList.Add(ediJobType);
                }
                return ediJobTypeList;
            }
            return null;
        }

        public class EDIJobTypePara : DBCulture
        {
            public EDIJobTypePara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                CODE_NM
            }
        }

        public class EDIJobType : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                CODE_ID, CODE_NM
            }

            public DBVarChar CodeID;
            public DBNVarChar CodeNM;

            public string ItemText()
            {
                return this.CodeNM.StringValue();
            }

            public string ItemValue()
            {
                return this.CodeID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<EDIJobType> SelectEDIJobTypeList(EDIJobTypePara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT CODE_ID, dbo.FN_GET_NMID(CODE_ID, {CODE_NM}) AS CODE_NM ", Environment.NewLine,
                "FROM CM_CODE ", Environment.NewLine,
                "WHERE CODE_KIND='0006' ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = EDIJobTypePara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(EDIJobTypePara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<EDIJobType> ediJobTypeList = new List<EDIJobType>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    EDIJobType ediJobType = new EDIJobType()
                    {
                        CodeID = new DBVarChar(dataRow[EDIJobType.DataField.CODE_ID.ToString()]),
                        CodeNM = new DBNVarChar(dataRow[EDIJobType.DataField.CODE_NM.ToString()])
                    };
                    ediJobTypeList.Add(ediJobType);
                }
                return ediJobTypeList;
            }
            return null;
        }
        #endregion

        #region - WorkFlow -

        #region - 查詢工作流程是否正在執行 -
        public class WorkFlowHasRunTimePara
        {
            public enum ParaField
            {
                SYS_ID, WF_FLOW_ID, WF_FLOW_VER
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
        }

        /// <summary>
        /// 查詢工作流程是否正在執行
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DBChar SelectWorkFlowHasRunTime(WorkFlowHasRunTimePara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "IF EXISTS (SELECT *",
                    "             FROM WF_FLOW",
                    "            WHERE SYS_ID = {SYS_ID}",
                    "              AND WF_FLOW_ID = {WF_FLOW_ID}",
                    "              AND WF_FLOW_VER = {WF_FLOW_VER})",
                    "BEGIN ",
                    "    SET @RESULT = 'Y'; ",
                    "END; ",
                    "SELECT @RESULT;"
                }));

            dbParameters.Add(new DBParameter { Name = WorkFlowHasRunTimePara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = WorkFlowHasRunTimePara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = WorkFlowHasRunTimePara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            return new DBChar(ExecuteScalar(commandText.ToString(), dbParameters));
        }
        #endregion

        //工作流程
        public class SysUserSystemWorkFlowIDPara : DBCulture
        {
            public SysUserSystemWorkFlowIDPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, USER_ID, WF_FLOW_GROUP_ID, 
                WF_FLOW
            }

            public DBVarChar SysID;
            public DBVarChar UserID;
            public DBVarChar WFFlowGroupID;
        }

        public class SysUserSystemWorkFlowID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                WF_FLOW_ID, WF_FLOW_VER,
                WF_FLOW_VALUE, WF_FLOW_TEXT
            }

            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;

            public DBVarChar WFFlowValue;
            public DBNVarChar WFFlowText;

            public string ItemText()
            {
                return this.WFFlowText.StringValue();
            }

            public string ItemValue()
            {
                return this.WFFlowValue.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysUserSystemWorkFlowID> SelectSysUserSystemWorkFlowIDList(SysUserSystemWorkFlowIDPara para)
        {
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.WFFlowGroupID.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                {
                    commandWhere, " AND S.WF_FLOW_GROUP_ID={WF_FLOW_GROUP_ID} ", Environment.NewLine
                });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT DISTINCT S.WF_FLOW_ID, S.WF_FLOW_VER ", Environment.NewLine,
                "     , (S.WF_FLOW_ID+'|'+S.WF_FLOW_VER) AS WF_FLOW_VALUE ", Environment.NewLine,
                "     , dbo.FN_GET_NMID(S.WF_FLOW_ID, S.{WF_FLOW})+'-'+S.WF_FLOW_VER AS WF_FLOW_TEXT ", Environment.NewLine,
                "     , S.SORT_ORDER ", Environment.NewLine,
                "FROM SYS_SYSTEM_WF_FLOW S ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON S.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "LEFT JOIN SYS_USER_SYSTEM_ROLE R ON R.USER_ID={USER_ID} AND S.SYS_ID=R.SYS_ID ", Environment.NewLine,
                "WHERE M.IS_OUTSOURCING='N' AND R.ROLE_ID<>'USER' ", Environment.NewLine,
                "  AND S.SYS_ID={SYS_ID} ", Environment.NewLine,
                commandWhere, Environment.NewLine, 
                "ORDER BY S.SORT_ORDER, S.WF_FLOW_ID, S.WF_FLOW_VER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysUserSystemWorkFlowIDPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysUserSystemWorkFlowIDPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = SysUserSystemWorkFlowIDPara.ParaField.WF_FLOW_GROUP_ID.ToString(), Value = para.WFFlowGroupID });
            dbParameters.Add(new DBParameter { Name = SysUserSystemWorkFlowIDPara.ParaField.WF_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysUserSystemWorkFlowIDPara.ParaField.WF_FLOW.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysUserSystemWorkFlowID> sysUserSystemWorkFlowIDList = new List<SysUserSystemWorkFlowID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysUserSystemWorkFlowID userSystemSysID = new SysUserSystemWorkFlowID()
                    {
                        WFFlowID = new DBVarChar(dataRow[SysUserSystemWorkFlowID.DataField.WF_FLOW_ID.ToString()]),
                        WFFlowVer = new DBChar(dataRow[SysUserSystemWorkFlowID.DataField.WF_FLOW_VER.ToString()]),
                        WFFlowValue = new DBVarChar(dataRow[SysUserSystemWorkFlowID.DataField.WF_FLOW_VALUE.ToString()]),
                        WFFlowText = new DBNVarChar(dataRow[SysUserSystemWorkFlowID.DataField.WF_FLOW_TEXT.ToString()])
                    };
                    sysUserSystemWorkFlowIDList.Add(userSystemSysID);
                }
                return sysUserSystemWorkFlowIDList;
            }
            return null;
        }

        //工作流程群組
        public class SysSystemWorkFlowGroupPara : DBCulture
        {
            public SysSystemWorkFlowGroupPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, WF_FLOW_GROUP
            }

            public DBVarChar SysID;
        }

        public class SysSystemWorkFlowGroup : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                WF_FLOW_GROUP_ID, WF_FLOW_GROUP_NM
            }

            public DBVarChar WFFlowGroupID;
            public DBNVarChar WFFlowGroupNM;

            public string ItemText()
            {
                return this.WFFlowGroupNM.StringValue();
            }

            public string ItemValue()
            {
                return this.WFFlowGroupID.GetValue();
            }

            public string ItemValue(string key)
            {
                throw new System.NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new System.NotImplementedException();
            }

            public string GroupBy()
            {
                throw new System.NotImplementedException();
            }
        }

        public List<SysSystemWorkFlowGroup> SelectSystemWorkFlowGroupList(SysSystemWorkFlowGroupPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT WF_FLOW_GROUP_ID, dbo.FN_GET_NMID(WF_FLOW_GROUP_ID, {WF_FLOW_GROUP}) AS WF_FLOW_GROUP_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_WF_FLOW_GROUP ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemWorkFlowGroupPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemWorkFlowGroupPara.ParaField.WF_FLOW_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemWorkFlowGroupPara.ParaField.WF_FLOW_GROUP.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemWorkFlowGroup> systemWorkFlowGroupList = new List<SysSystemWorkFlowGroup>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemWorkFlowGroup sysSystemWorkFlowGroup = new SysSystemWorkFlowGroup()
                    {
                        WFFlowGroupID = new DBVarChar(dataRow[SysSystemWorkFlowGroup.DataField.WF_FLOW_GROUP_ID.ToString()]),
                        WFFlowGroupNM = new DBNVarChar(dataRow[SysSystemWorkFlowGroup.DataField.WF_FLOW_GROUP_NM.ToString()])
                    };
                    systemWorkFlowGroupList.Add(sysSystemWorkFlowGroup);
                }
                return systemWorkFlowGroupList;
            }
            return null;
        }

        //工作流程節點
        public class SysSystemWorkFlowNodeIDPara : DBCulture
        {
            public SysSystemWorkFlowNodeIDPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, WF_FLOW_ID, WF_FLOW_VER, WF_NODE
            }

            public DBVarChar SysID;
            public DBVarChar FlowID;
            public DBChar FlowVer;
        }

        public class SysSystemWorkFlowNodeID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                WF_NODE_ID, WF_NODE_NM
            }

            public DBVarChar WFNodeID;
            public DBNVarChar WFNodeNM;

            public string ItemText()
            {
                return this.WFNodeNM.StringValue();
            }

            public string ItemValue()
            {
                return this.WFNodeID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysSystemWorkFlowNodeID> SelectSysSystemWorkFlowNodeIDList(SysSystemWorkFlowNodeIDPara para)
        {
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.FlowID.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                {
                    commandWhere, "  AND WF_FLOW_ID={WF_FLOW_ID} ", Environment.NewLine
                });
            }

            if (!string.IsNullOrWhiteSpace(para.FlowVer.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                {
                    commandWhere, "  AND WF_FLOW_VER={WF_FLOW_VER} ", Environment.NewLine
                });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT DISTINCT WF_NODE_ID ", Environment.NewLine,
                "     , dbo.FN_GET_NMID(WF_NODE_ID, {WF_NODE}) AS WF_NODE_NM ", Environment.NewLine,
                "     , SORT_ORDER ", Environment.NewLine,
                "FROM SYS_SYSTEM_WF_NODE ", Environment.NewLine,                
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                commandWhere, Environment.NewLine, 
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemWorkFlowNodeIDPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemWorkFlowNodeIDPara.ParaField.WF_FLOW_ID.ToString(), Value = para.FlowID });
            dbParameters.Add(new DBParameter { Name = SysSystemWorkFlowNodeIDPara.ParaField.WF_FLOW_VER.ToString(), Value = para.FlowVer });
            dbParameters.Add(new DBParameter { Name = SysSystemWorkFlowNodeIDPara.ParaField.WF_NODE.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemWorkFlowNodeIDPara.ParaField.WF_NODE.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemWorkFlowNodeID> sysSystemWorkFlowNodeIDList = new List<SysSystemWorkFlowNodeID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemWorkFlowNodeID userSystemSysID = new SysSystemWorkFlowNodeID()
                    {
                        WFNodeID = new DBVarChar(dataRow[SysSystemWorkFlowNodeID.DataField.WF_NODE_ID.ToString()]),
                        WFNodeNM = new DBNVarChar(dataRow[SysSystemWorkFlowNodeID.DataField.WF_NODE_NM.ToString()])
                    };
                    sysSystemWorkFlowNodeIDList.Add(userSystemSysID);
                }
                return sysSystemWorkFlowNodeIDList;
            }
            return null;
        }


        public class SysSystemWFNodePara : DBCulture
        {
            public SysSystemWFNodePara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                SYS_ID, WF_FLOW_ID, WF_FLOW_VER, WF_NODE_ID,
                SYS_NM, WF_FLOW, WF_NODE
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBChar WFNodeID;
        }

        public class SysSystemWFNode : DBTableRow
        {
            public DBNVarChar SysNM;
            public DBNVarChar WFFlowNM;
            public DBChar WFFlowVer;
            public DBNVarChar WFNodeNM;

            public DBVarChar NodeType;

            public DBNVarChar WFSigMemoZHTW;
            public DBNVarChar WFSigMemoZHCN;
            public DBNVarChar WFSigMemoENUS;
            public DBNVarChar WFSigMemoTHTH;
            public DBNVarChar WFSigMemoJAJP;

            public DBVarChar SigAPISysID;
            public DBVarChar SigAPIControllerID;
            public DBVarChar SigAPIActionName;

            public DBVarChar ChkAPISysID;
            public DBVarChar ChkAPIControllerID;
            public DBVarChar ChkAPIActionName;

            public DBVarChar AssgAPISysID;
            public DBVarChar AssgAPIControllerID;
            public DBVarChar AssgAPIActionName;

            public DBChar IsSigNextNode;
            public DBChar IsSigBackNode;
            public DBChar IsAssgNextNode;
        }

        public SysSystemWFNode SelectSysSystemWFNode(SysSystemWFNodePara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT dbo.FN_GET_NMID(N.SYS_ID, M.{SYS_NM}) AS SysNM ",
                        "     , dbo.FN_GET_NMID(N.WF_FLOW_ID, F.{WF_FLOW}) AS WFFlowNM ",
                        "     , N.WF_FLOW_VER AS WFFlowVer ",
                        "     , dbo.FN_GET_NMID(N.WF_NODE_ID, N.{WF_NODE}) AS WFNodeNM ",
                        "     , N.NODE_TYPE AS NodeType ",
                        "     , N.WF_SIG_MEMO_ZH_TW AS WFSigMemoZHTW",
                        "     , N.WF_SIG_MEMO_ZH_CN AS WFSigMemoZHCN",
                        "     , N.WF_SIG_MEMO_EN_US AS WFSigMemoENUS",
                        "     , N.WF_SIG_MEMO_TH_TH AS WFSigMemoTHTH",
                        "     , N.WF_SIG_MEMO_JA_JP AS WFSigMemoJAJP",
                        "     , N.SIG_API_SYS_ID AS SigAPISysID ",
                        "     , N.SIG_API_CONTROLLER_ID AS SigAPIControllerID ",
                        "     , N.SIG_API_ACTION_NAME AS SigAPIActionName ",
                        "     , N.CHK_API_SYS_ID AS ChkAPISysID ",
                        "     , N.CHK_API_CONTROLLER_ID AS ChkAPIControllerID ",
                        "     , N.CHK_API_ACTION_NAME AS ChkAPIActionName ",
                        "     , N.ASSG_API_SYS_ID AS AssgAPISysID ",
                        "     , N.ASSG_API_CONTROLLER_ID AS AssgAPIControllerID ",
                        "     , N.ASSG_API_ACTION_NAME AS AssgAPIActionName ",
                        "     , N.IS_SIG_NEXT_NODE AS IsSigNextNode ",
                        "     , N.IS_SIG_BACK_NODE AS IsSigBackNode ",
                        "     , N.IS_ASSG_NEXT_NODE AS IsAssgNextNode ",
                        "FROM SYS_SYSTEM_WF_NODE N ",
                        "JOIN SYS_SYSTEM_WF_FLOW F ON N.SYS_ID=F.SYS_ID AND N.WF_FLOW_ID=F.WF_FLOW_ID AND N.WF_FLOW_VER=F.WF_FLOW_VER ",
                        "JOIN SYS_SYSTEM_MAIN M ON N.SYS_ID=M.SYS_ID ",
                        "WHERE N.SYS_ID={SYS_ID} AND N.WF_FLOW_ID={WF_FLOW_ID} AND N.WF_FLOW_VER={WF_FLOW_VER} ",
                        "  AND N.WF_NODE_ID={WF_NODE_ID} ",
                        Environment.NewLine
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemWFNodePara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemWFNodePara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SysSystemWFNodePara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SysSystemWFNodePara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SysSystemWFNodePara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemWFNodePara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SysSystemWFNodePara.ParaField.WF_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemWFNodePara.ParaField.WF_FLOW.ToString())) });
            dbParameters.Add(new DBParameter { Name = SysSystemWFNodePara.ParaField.WF_NODE.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemWFNodePara.ParaField.WF_NODE.ToString())) });
            return GetEntityList<SysSystemWFNode>(commandText, dbParameters).SingleOrDefault();
        }

        public class SystemWFNodeDetailExecuteResult : ExecuteResult
        {
            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;

            public DBVarChar WFNodeID;
            public DBNVarChar WFNodeZHTW;
            public DBNVarChar WFNodeZHCN;
            public DBNVarChar WFNodeENUS;
            public DBNVarChar WFNodeTHTH;
            public DBNVarChar WFNodeJAJP;

            public DBVarChar NodeType;
            public DBInt NodeSeqX;
            public DBInt NodeSeqY;

            public DBInt NodePosBeginX;
            public DBInt NodePosBeginY;
            public DBInt NodePosEndX;
            public DBInt NodePosEndY;

            public DBChar IsFirst;
            public DBChar IsFinally;
            public DBVarChar BackWFNodeID;

            public DBNVarChar WFSigMemoZHTW;
            public DBNVarChar WFSigMemoZHCN;
            public DBNVarChar WFSigMemoENUS;
            public DBNVarChar WFSigMemoTHTH;
            public DBNVarChar WFSigMemoJAJP;

            public DBVarChar FunSysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;

            public DBVarChar SigApiSysID;
            public DBVarChar SigApiControllerID;
            public DBVarChar SigApiActionName;
            public DBVarChar ChkApiSysID;
            public DBVarChar ChkApiControllerID;
            public DBVarChar ChkApiActionName;

            public DBVarChar AssgAPISysID;
            public DBVarChar AssgAPIControllerID;
            public DBVarChar AssgAPIActionName;

            public DBChar IsSigNextNode;
            public DBChar IsSigBackNode;

            public DBChar IsAssgNextNode;

            public DBVarChar SortOrder;
            public DBNVarChar Remark;
        }
        #endregion

        public class SysSystemSysIDPara : DBCulture
        {
            public SysSystemSysIDPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
            }

            public DBVarChar SysID;
        }

        public string GetFileDataPath(SysSystemSysIDPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT FOLDER_PATH ", Environment.NewLine,
                "FROM SYS_SYSTEM_IP ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND SUB_SYS_ID={SYS_ID} AND IS_FILE_SERVER='Y' ", Environment.NewLine,
                "ORDER BY SYS_ID "
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemSysIDPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            
            string result = (string)ExecuteScalar(commandText, dbParameters);
            if (Common.IsInKubernetes())
            {
                result = result.Replace(@"\\", @"C:\");
                return result;
            }
            return result;
        }
    }

    public class MongoSys : MongoEntity
    {
        public MongoSys(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }
    }
}