using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityUserRoleFunDetail : EntitySys
    {
        public EntityUserRoleFunDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserRawPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public bool SelectRawUserIsDisable(UserRawPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT IS_LEFT ", Environment.NewLine,
                "FROM RAW_CM_USER ", Environment.NewLine,
                "WHERE USER_ID={USER_ID}; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserRawPara.ParaField.USER_ID.ToString(), Value = para.UserID });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? true : false;
        }

        public class UserMainPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public class UserMain : DBTableRow
        {
            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar RoleGroupID;
        }

        public UserMain SelectUserMainInfo(UserMainPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT USER_ID AS UserID",
                "     , dbo.FN_GET_USER_NM(USER_ID) AS UserNM ",
                "     , ROLE_GROUP_ID AS RoleGroupID ",
                "  FROM SYS_USER_MAIN ",
                " WHERE USER_ID = {USER_ID}; "
            });

            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            return GetEntityList<UserMain>(commandText, dbParameters).SingleOrDefault();
        }

        public bool SelectUserMainIsDisable(UserMainPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT IS_DISABLE ", Environment.NewLine,
                "FROM SYS_USER_MAIN ", Environment.NewLine,
                "WHERE USER_ID={USER_ID}; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_ID.ToString(), Value = para.UserID });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? true : false;
        }

        public class SysSystemRoleGroupCollectPara
        {
            public enum ParaField
            {
                ROLE_GROUP_ID
            }

            public DBVarChar RoleGroupID;
        }

        public class SysSystemRoleGroupCollect : DBTableRow, ISelectItem
        {
            public DBVarChar SysID;
            public DBVarChar RoleID;
            public DBNVarChar Remark;

            public string ItemText()
            {
                return Remark.StringValue();
            }

            public string ItemValue()
            {
                return string.Format("{0}|{1}", SysID.StringValue(), RoleID.StringValue());
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

        public List<SysSystemRoleGroupCollect> SelectSysSystemRoleGroupCollectList(SysSystemRoleGroupCollectPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT C.SYS_ID AS SysID ",
                "     , C.ROLE_ID AS RoleID ",
                "     , G.REMARK AS Remark ",
                "  FROM SYS_SYSTEM_ROLE_GROUP_COLLECT C ",
                "  JOIN SYS_SYSTEM_ROLE_GROUP G ON C.ROLE_GROUP_ID=G.ROLE_GROUP_ID ",
                " WHERE C.ROLE_GROUP_ID = {ROLE_GROUP_ID} ",
                " ORDER BY C.SYS_ID, C.ROLE_ID; "
            });

            dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupCollectPara.ParaField.ROLE_GROUP_ID.ToString(), Value = para.RoleGroupID });
            return GetEntityList<SysSystemRoleGroupCollect>(commandText, dbParameters);
        }

        public class UserSystemRolePara : DBCulture
        {
            public UserSystemRolePara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                USER_ID,
                ROLE_GROUP_ID,
                IS_DISABLE,
                ERP_WFNO,
                MEMO,
                SYS_ID,
                SYS_NM,
                ROLE_ID,
                ROLE_NM,
                API_NO,
                EXEC_SYS_ID,
                IP_ADDRESS,
                UPD_USER_ID,
                UPD_DT
            }

            public DBVarChar UserID;
            public DBVarChar IpAddress;
            public DBVarChar ExecSysID;
            public DBVarChar ErpWFNO;
            public DBNVarChar Memo;
            public DBVarChar RoleGroupID;
            public DBChar ApiNO;
            public DBChar IsDisable;
            public DBVarChar SysID;
            public DBVarChar RoleID;
            public DBVarChar UpdUserID;
            public DBDateTime UpdDT;
        }

        public class UserSystemRole : DBTableRow
        {
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBNVarChar SysNMID;
            public DBVarChar RoleID;
            public DBNVarChar RoleNMID;
            public DBNVarChar RoleNM;
            public DBChar HasRole;
            public DBBit HasAuth;
            public DBVarChar UpdUserID;
            public DBDateTime UpdDT;
        }

        public List<UserSystemRole> SelectUserSystemRoleList(UserSystemRolePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @USER_ID VARCHAR(20) = {USER_ID};",
                "DECLARE @UPD_USER_ID VARCHAR(20) = {UPD_USER_ID};",
                "DECLARE @IS_ERPIT BIT;",

                " SELECT @IS_ERPIT = CAST(1 AS BIT)",
                "   FROM SYS_USER_SYSTEM_ROLE",
                "  WHERE USER_ID = @UPD_USER_ID",
                "    AND SYS_ID = '" + EnumSystemID.ERPAP + "'",
                "    AND ROLE_ID = '" + EnumSystemRoleID.IT + "'",

                ";WITH USER_SYSTEM_ROLE AS (",
                "    SELECT SYS_ID",
                "      FROM SYS_USER_SYSTEM_ROLE",
                "     WHERE USER_ID = @UPD_USER_ID",
                "       AND ROLE_ID = '" + EnumSystemRoleID.IT + "'",
                ")",
                " SELECT @USER_ID AS USER_ID",
                "      , R.SYS_ID AS SysID",
                "      , dbo.FN_GET_NMID(R.SYS_ID, S.{SYS_NM}) AS SysNMID",
                "      , S.{SYS_NM} AS SysNM",
                "      , R.ROLE_ID AS RoleID",
                "      , dbo.FN_GET_NMID(R.ROLE_ID, R.{ROLE_NM}) AS RoleNMID",
                "      , R.{ROLE_NM} AS RoleNM",
                "      , (CASE WHEN U.USER_ID IS NOT NULL THEN 'Y' ELSE 'N' END) AS HasRole",
                "      , (CASE WHEN @IS_ERPIT = 1",
                "                OR EXISTS(SELECT USER_ID FROM USER_SYSTEM_ROLE G WHERE R.SYS_ID = G.SYS_ID) ",
                "                OR S.SYS_MAN_USER_ID = @UPD_USER_ID ",
                "              THEN 1 ",
                "              ELSE 0 ",
                "        END) AS HasAuth",
                "      , U.UPD_USER_ID AS UpdUserID",
                "      , U.UPD_DT AS UpdDT",
                "   FROM SYS_SYSTEM_ROLE R",
                "   JOIN SYS_SYSTEM_MAIN S",
                "     ON R.SYS_ID = S.SYS_ID",
                "   LEFT JOIN (SELECT USER_ID",
                "                   , SYS_ID",
                "                   , ROLE_ID ",
                "                   , UPD_USER_ID ",
                "                   , UPD_DT ",
                "                FROM SYS_USER_SYSTEM_ROLE",
                "               WHERE USER_ID = @USER_ID) U",
                "     ON R.SYS_ID = U.SYS_ID",
                "    AND R.ROLE_ID = U.ROLE_ID",
                "  ORDER BY S.SORT_ORDER, R.SYS_ID, R.ROLE_ID"
            }));

            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.UPD_USER_ID.ToString(), Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserSystemRolePara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserSystemRolePara.ParaField.ROLE_NM.ToString())) });

            return GetEntityList<UserSystemRole>(commandText.ToString(), dbParameters);
        }

        public enum EnumEditUserSystemRoleResult
        {
            Success,
            Failure
        }

        public EnumEditUserSystemRoleResult EditUserSystemRole(UserSystemRolePara para, List<UserSystemRolePara> paraList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            if (paraList == null ||
                paraList.Count == 0)
            {
                string deleteCommand = string.Concat(new object[]
                {
                    "DELETE FROM SYS_USER_SYSTEM_ROLE WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                    "DELETE FROM SYS_USER_SYSTEM WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                    "DELETE FROM SYS_USER_FUN WHERE USER_ID={USER_ID} AND IS_ASSIGN='N'; ", Environment.NewLine,
                    "DELETE FROM SYS_USER_FUN_MENU WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                    "UPDATE SYS_USER_MAIN SET ROLE_GROUP_ID=NULL, IS_DISABLE='Y', UPD_USER_ID={UPD_USER_ID}, UPD_DT=GETDATE() WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                });

                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID, Value = para.UserID });
                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                commandTextStringBuilder.Append(GetCommandText(ProviderName, deleteCommand, dbParameters));
                dbParameters.Clear();
            }
            else
            {
                string deleteCommand = string.Concat(new object[]
                {
                    "DELETE FROM SYS_USER_SYSTEM_ROLE WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                    "DELETE FROM SYS_USER_SYSTEM WHERE USER_ID={USER_ID} AND SYS_ID NOT IN (SELECT SYS_ID FROM SYS_SYSTEM_MAIN WHERE IS_OUTSOURCING='Y'); ", Environment.NewLine,
                    "DELETE FROM SYS_USER_FUN WHERE USER_ID={USER_ID} AND IS_ASSIGN='N'; ", Environment.NewLine,
                    "UPDATE SYS_USER_MAIN SET ROLE_GROUP_ID={ROLE_GROUP_ID}, IS_DISABLE={IS_DISABLE}, UPD_USER_ID={UPD_USER_ID}, UPD_DT=GETDATE() WHERE USER_ID={USER_ID}; ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID, Value = para.UserID });
                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.ROLE_GROUP_ID, Value = para.RoleGroupID });
                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.IS_DISABLE, Value = para.IsDisable });
                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                commandTextStringBuilder.Append(GetCommandText(ProviderName, deleteCommand, dbParameters));
                dbParameters.Clear();

                foreach (UserSystemRolePara userSystemRolePara in paraList)
                {
                    string insertCommand = string.Concat(new object[]
                    {
                        "IF EXISTS(SELECT * FROM SYS_SYSTEM_ROLE WHERE SYS_ID = {SYS_ID} AND ROLE_ID = {ROLE_ID}) ",
                        "BEGIN ",
                        "INSERT INTO SYS_USER_SYSTEM_ROLE VALUES ({USER_ID}, {SYS_ID}, {ROLE_ID}, {UPD_USER_ID}, {UPD_DT}); ",
                        "END "
                    });

                    dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID, Value = userSystemRolePara.UserID });
                    dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.SYS_ID, Value = userSystemRolePara.SysID });
                    dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.ROLE_ID, Value = userSystemRolePara.RoleID });
                    dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.UPD_USER_ID, Value = userSystemRolePara.UpdUserID });
                    dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.UPD_DT, Value = userSystemRolePara.UpdDT });

                    commandTextStringBuilder.Append(GetCommandText(ProviderName, insertCommand, dbParameters));
                    dbParameters.Clear();
                }

                string systemCommand = string.Concat(new object[]
                {
                    "INSERT INTO SYS_USER_SYSTEM ", Environment.NewLine,
                    "SELECT USER_ID, SYS_ID, {USER_ID}, GETDATE() ", Environment.NewLine,
                    "FROM SYS_USER_SYSTEM_ROLE ", Environment.NewLine,
                    "WHERE USER_ID={USER_ID} ", Environment.NewLine,
                    "GROUP BY USER_ID, SYS_ID; ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID, Value = para.UserID });
                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.IS_DISABLE, Value = para.IsDisable });

                commandTextStringBuilder.Append(GetCommandText(ProviderName, systemCommand, dbParameters));
                dbParameters.Clear();

                string functionCommand = string.Concat(new object[]
                {
                    "INSERT INTO SYS_USER_FUN ", Environment.NewLine,
                    "SELECT U.USER_ID ", Environment.NewLine,
                    "     , F.SYS_ID, F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME ", Environment.NewLine,
                    "     , 'N' AS IS_ASSIGN ", Environment.NewLine,
                    "     , {USER_ID}, GETDATE() ", Environment.NewLine,
                    "FROM SYS_USER_SYSTEM_ROLE U ", Environment.NewLine,
                    "JOIN SYS_SYSTEM_MAIN S ON U.SYS_ID=S.SYS_ID ", Environment.NewLine,
                    "JOIN SYS_SYSTEM_ROLE_FUN R ON U.SYS_ID=R.SYS_ID AND U.ROLE_ID=R.ROLE_ID ", Environment.NewLine,
                    "JOIN SYS_SYSTEM_FUN F ON R.SYS_ID=F.SYS_ID AND R.FUN_CONTROLLER_ID=F.FUN_CONTROLLER_ID AND R.FUN_ACTION_NAME=F.FUN_ACTION_NAME ", Environment.NewLine,
                    "LEFT JOIN ( ", Environment.NewLine,
                    "    SELECT N.SYS_ID, N.FUN_MENU, N.FUN_CONTROLLER_ID, N.FUN_ACTION_NAME, M.DEFAULT_MENU_ID ", Environment.NewLine,
                    "    FROM SYS_SYSTEM_MENU_FUN N ", Environment.NewLine,
                    "    JOIN SYS_SYSTEM_FUN_MENU M ON N.FUN_MENU_SYS_ID=M.SYS_ID AND N.FUN_MENU=M.FUN_MENU ", Environment.NewLine,
                    ") Z ON F.SYS_ID=Z.SYS_ID AND F.FUN_CONTROLLER_ID=Z.FUN_CONTROLLER_ID AND F.FUN_ACTION_NAME=Z.FUN_ACTION_NAME ", Environment.NewLine,
                    "LEFT JOIN SYS_USER_FUN O ON U.USER_ID=O.USER_ID ", Environment.NewLine,
                    "                        AND F.SYS_ID=O.SYS_ID ", Environment.NewLine,
                    "                        AND F.FUN_CONTROLLER_ID=O.FUN_CONTROLLER_ID ", Environment.NewLine,
                    "                        AND F.FUN_ACTION_NAME=O.FUN_ACTION_NAME ", Environment.NewLine,
                    "WHERE U.USER_ID={USER_ID} AND S.IS_DISABLE='N' AND F.IS_DISABLE='N' AND Z.FUN_MENU IS NOT NULL ", Environment.NewLine,
                    "  AND O.IS_ASSIGN IS NULL ", Environment.NewLine,
                    "GROUP BY U.USER_ID, F.SYS_ID, F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME ", Environment.NewLine,
                    "ORDER BY F.SYS_ID, F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME; ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID, Value = para.UserID });

                commandTextStringBuilder.Append(GetCommandText(ProviderName, functionCommand, dbParameters));
                dbParameters.Clear();

                string menuCommand = string.Concat(new object[]
                {
                    "INSERT INTO SYS_USER_FUN_MENU ", Environment.NewLine,
                    "SELECT DISTINCT U.USER_ID, Z.FUN_MENU_SYS_ID ", Environment.NewLine,
                    "     , Z.FUN_MENU, Z.DEFAULT_MENU_ID, Z.SORT_ORDER ", Environment.NewLine,
                    "     , {USER_ID}, GETDATE() ", Environment.NewLine,
                    "FROM SYS_USER_FUN U ", Environment.NewLine,
                    "JOIN ( ", Environment.NewLine,
                    "    SELECT N.SYS_ID ", Environment.NewLine,
                    "         , N.FUN_MENU_SYS_ID, N.FUN_MENU ", Environment.NewLine,
                    "         , N.FUN_CONTROLLER_ID, N.FUN_ACTION_NAME ", Environment.NewLine,
                    "         , M.DEFAULT_MENU_ID, M.SORT_ORDER ", Environment.NewLine,
                    "    FROM SYS_SYSTEM_MENU_FUN N ", Environment.NewLine,
                    "    JOIN SYS_SYSTEM_FUN_MENU M ON N.FUN_MENU_SYS_ID=M.SYS_ID AND N.FUN_MENU=M.FUN_MENU ", Environment.NewLine,
                    ") Z ON U.SYS_ID=Z.SYS_ID AND U.FUN_CONTROLLER_ID=Z.FUN_CONTROLLER_ID AND U.FUN_ACTION_NAME=Z.FUN_ACTION_NAME ", Environment.NewLine,
                    "LEFT JOIN SYS_USER_FUN_MENU M ON U.USER_ID=M.USER_ID AND Z.FUN_MENU_SYS_ID=M.SYS_ID AND Z.FUN_MENU=M.FUN_MENU ", Environment.NewLine,
                    "WHERE U.USER_ID={USER_ID} AND M.USER_ID IS NULL AND M.SYS_ID IS NULL AND M.FUN_MENU IS NULL; ", Environment.NewLine,

                    "DELETE SYS_USER_FUN_MENU ", Environment.NewLine,
                    "FROM SYS_USER_FUN_MENU U ", Environment.NewLine,
                    "LEFT JOIN (SELECT DISTINCT N.FUN_MENU_SYS_ID, N.FUN_MENU ", Environment.NewLine,
                    "           FROM SYS_USER_FUN F ", Environment.NewLine,
                    "           JOIN SYS_SYSTEM_MENU_FUN N ON F.SYS_ID=N.SYS_ID AND F.FUN_CONTROLLER_ID=N.FUN_CONTROLLER_ID AND F.FUN_ACTION_NAME=N.FUN_ACTION_NAME ", Environment.NewLine,
                    "           WHERE F.USER_ID={USER_ID}) M ", Environment.NewLine,
                    "ON U.SYS_ID=M.FUN_MENU_SYS_ID AND U.FUN_MENU=M.FUN_MENU ", Environment.NewLine,
                    "WHERE U.USER_ID={USER_ID} AND M.FUN_MENU_SYS_ID IS NULL AND M.FUN_MENU IS NULL; ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID, Value = para.UserID });

                commandTextStringBuilder.Append(GetCommandText(ProviderName, menuCommand, dbParameters));
                dbParameters.Clear();
            }

            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,
                commandTextStringBuilder.ToString(), Environment.NewLine,
                "        EXECUTE dbo.SP_LOG_USER_SYSTEM_ROLE {USER_ID} ,{ERP_WFNO} ,{MEMO} ,{API_NO} ,'"+ Mongo_BaseAP.EnumModifyType.U +"', {EXEC_SYS_ID} ,{IP_ADDRESS} ,{UPD_USER_ID};",
                "        SET @RESULT = 'Y'; ", Environment.NewLine,
                "        COMMIT; ", Environment.NewLine,
                "    END TRY ", Environment.NewLine,
                "    BEGIN CATCH ", Environment.NewLine,
                "        SET @RESULT = 'N'; ", Environment.NewLine,
                "        ROLLBACK TRANSACTION; ", Environment.NewLine,
                "    END CATCH ", Environment.NewLine,
                "; ", Environment.NewLine,
                "SELECT @RESULT; ", Environment.NewLine
            });

            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.ERP_WFNO, Value = para.ErpWFNO });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.MEMO, Value = para.Memo });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.API_NO, Value = para.ApiNO });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.EXEC_SYS_ID, Value = para.ExecSysID });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.IP_ADDRESS, Value = para.IpAddress });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditUserSystemRoleResult.Success : EnumEditUserSystemRoleResult.Failure;
        }
    }
}