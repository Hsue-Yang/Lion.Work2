using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Home
{
    public class EntityHome : DBEntity
    {
#if !NET461
        public EntityHome(string connectionName)
            : base(connectionName)
        {
        }
#endif
        public EntityHome(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemMainPara
        {
            public enum ParaField
            {
                SYS_ID
            }

            public DBVarChar SysID;
        }

        public class SystemMain : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_INDEX_PATH,
                SYS_KEY, EN_SYS_ID,
                IS_OUTSOURCING
            }

            public DBVarChar SysID;
            public DBNVarChar SysIndexPath;
            public DBChar SysKey;
            public DBChar ENSysID;
            public DBChar IsOutsourcing;
        }

        public SystemMain SelectSystemMain(SystemMainPara systemMainPara)
        {
            string commandText = string.Concat(new object[] 
            { 
                "SELECT SYS_ID, SYS_INDEX_PATH ", Environment.NewLine,
                "     , SYS_KEY, EN_SYS_ID ", Environment.NewLine,
                "     , IS_OUTSOURCING ", Environment.NewLine,
                "FROM SYS_SYSTEM_MAIN ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND IS_DISABLE='N'; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemMainPara.ParaField.SYS_ID.ToString(), Value = systemMainPara.SysID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                SystemMain systemMain = new SystemMain()
                {
                    SysID = new DBVarChar(dataTable.Rows[0][SystemMain.DataField.SYS_ID.ToString()]),
                    SysIndexPath = new DBNVarChar(dataTable.Rows[0][SystemMain.DataField.SYS_INDEX_PATH.ToString()]),
                    SysKey = new DBChar(dataTable.Rows[0][SystemMain.DataField.SYS_KEY.ToString()]),
                    ENSysID = new DBChar(dataTable.Rows[0][SystemMain.DataField.EN_SYS_ID.ToString()]),
                    IsOutsourcing = new DBChar(dataTable.Rows[0][SystemMain.DataField.IS_OUTSOURCING.ToString()])
                };
                return systemMain;
            }
            return null;
        }

        public class SystemIPPara
        {
            public enum ParaField
            {
                SYS_ID, IP_ADDRESS
            }

            public DBVarChar SystemID;
            public DBVarChar IPAddress;
        }

        public DBInt SelectOutSourcingSystemIP(SystemIPPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT COUNT(1) AS COUNT_CN ", Environment.NewLine,
                "FROM SYS_SYSTEM_MAIN S JOIN SYS_SYSTEM_IP P ON S.SYS_ID=P.SYS_ID ", Environment.NewLine,
                "WHERE S.IS_OUTSOURCING='Y' AND S.SYS_ID={SYS_ID} AND P.IP_ADDRESS={IP_ADDRESS} ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemIPPara.ParaField.SYS_ID, Value = para.SystemID });
            dbParameters.Add(new DBParameter { Name = SystemIPPara.ParaField.IP_ADDRESS, Value = para.IPAddress });

            return new DBInt(base.ExecuteScalar(commandText, dbParameters));
        }

        public DBInt SelectSystemIP(SystemIPPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT COUNT(1) AS COUNT_CN ", Environment.NewLine,
                "FROM SYS_SYSTEM_MAIN S JOIN SYS_SYSTEM_IP P ON S.SYS_ID=P.SYS_ID ", Environment.NewLine,
                "WHERE S.IS_OUTSOURCING='N' AND S.SYS_ID={SYS_ID} AND P.IP_ADDRESS={IP_ADDRESS} ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemIPPara.ParaField.SYS_ID, Value = para.SystemID });
            dbParameters.Add(new DBParameter { Name = SystemIPPara.ParaField.IP_ADDRESS, Value = para.IPAddress });

            return new DBInt(base.ExecuteScalar(commandText, dbParameters));
        }

        public class UserSystemPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public class UserSystem : DBTableRow
        {
            public DBVarChar UserID;
            public DBVarChar SystemID;
        }

        public List<UserSystem> SelectUserSystemList(UserSystemPara userSystemPara)
        {
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT U.USER_ID AS UserID, U.SYS_ID AS SystemID",
                "FROM SYS_USER_SYSTEM U ",
                "JOIN SYS_SYSTEM_MAIN S ON U.SYS_ID=S.SYS_ID ",
                "WHERE U.USER_ID={USER_ID} AND S.IS_DISABLE='N' AND S.IS_OUTSOURCING='N' ",
                "ORDER BY U.SYS_ID; ",
                Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSystemPara.ParaField.USER_ID.ToString(), Value = userSystemPara.UserID });
            return GetEntityList<UserSystem>(commandText, dbParameters);
        }

        #region - 查詢使用者工作相差時數 -
        public class UserWorkDiffHourPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }
        
        /// <summary>
        /// 查詢使用者工作相差時數
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DBInt SelectUserWorkDiffHour(UserWorkDiffHourPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @DATENOW CHAR(8) = dbo.FN_GET_SYSDATE(GETDATE());",
                    "SELECT W.DIFF_HOUR",
                    "  FROM RAW_CM_WORKDATE W",
                    "  JOIN RAW_CM_USER_ORG O",
                    "    ON W.COMP_ID = O.USER_COM_ID",
                    "   AND W.WORK_DATE = @DATENOW",
                    "   AND O.USER_ID = {USER_ID}",
                }));

            dbParameters.Add(new DBParameter { Name = UserWorkDiffHourPara.ParaField.USER_ID, Value = para.UserID });
            return new DBInt(ExecuteScalar(commandText.ToString(), dbParameters));
        }
        #endregion

        public class UserSystemRolePara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public class UserSystemRole : DBTableRow
        {
            public DBVarChar UserID;
            public DBVarChar SystemID;
            public DBVarChar RoleID;
        }

        public List<UserSystemRole> SelectUserSystemRoleList(UserSystemRolePara userSystemRolePara)
        {
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT R.USER_ID AS UserID, R.SYS_ID AS SystemID, R.ROLE_ID AS RoleID ",
                "FROM SYS_USER_SYSTEM_ROLE R ",
                "JOIN SYS_SYSTEM_MAIN M ON R.SYS_ID=M.SYS_ID ",
                "WHERE R.USER_ID={USER_ID} AND M.IS_DISABLE='N' ",
                "ORDER BY R.USER_ID, R.SYS_ID, R.ROLE_ID; ",
                Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID.ToString(), Value = userSystemRolePara.UserID });
            return GetEntityList<UserSystemRole>(commandText, dbParameters);
        }
        
        public class UserAuthorizationPara
        {
            public enum ParaField
            {
                USER_ID, SYS_ID, SESSION_ID, IP_ADDRESS
            }

            public DBVarChar UserID;
            public DBVarChar SystemID;
            public DBChar SessionID;
            public DBVarChar IPAddress;
        }

        public enum EnumValidateUserAuthorizationResult
        {
            Success, Failure
        }

        public EnumValidateUserAuthorizationResult ValidateUserAuthorization(UserAuthorizationPara userAuthorizationPara)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @USER_ID VARCHAR(6) = NULL; ", Environment.NewLine,
                "SELECT @USER_ID=U.USER_ID ", Environment.NewLine,
                "FROM SYS_USER_MAIN U ", Environment.NewLine,
                "JOIN RAW_CM_USER R ON U.USER_ID=R.USER_ID ", Environment.NewLine,
                "JOIN SYS_USER_SYSTEM S ON U.USER_ID=S.USER_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON S.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "WHERE U.USER_ID={USER_ID} AND R.IS_LEFT='N' ", Environment.NewLine,
                "  AND S.SYS_ID={SYS_ID} AND M.IS_DISABLE='N'; ", Environment.NewLine,
                "IF @USER_ID IS NOT NULL ", Environment.NewLine,
                "BEGIN ", Environment.NewLine,
                "    IF EXISTS (SELECT USER_ID FROM SYS_USER_CONNECT WHERE USER_ID={USER_ID} AND SESSION_ID={SESSION_ID}) ", Environment.NewLine,
                "        UPDATE SYS_USER_CONNECT SET ", Environment.NewLine,
                "            LAST_CONNECT_DT=GETDATE(), CUST_LOGOUT='N', SSO_VALID='N', IP_ADDRESS={IP_ADDRESS} ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID} AND SESSION_ID={SESSION_ID}; ", Environment.NewLine,
                "    ELSE ", Environment.NewLine,
                "        INSERT INTO SYS_USER_CONNECT VALUES ({USER_ID}, GETDATE(), {SESSION_ID}, 'N', 'N', {IP_ADDRESS}); ", Environment.NewLine,
                "    ; ", Environment.NewLine,
                "END; ", Environment.NewLine,
                "SELECT @USER_ID; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserAuthorizationPara.ParaField.USER_ID, Value = userAuthorizationPara.UserID });
            dbParameters.Add(new DBParameter { Name = UserAuthorizationPara.ParaField.SYS_ID, Value = userAuthorizationPara.SystemID });
            dbParameters.Add(new DBParameter { Name = UserAuthorizationPara.ParaField.SESSION_ID, Value = userAuthorizationPara.SessionID });
            dbParameters.Add(new DBParameter { Name = UserAuthorizationPara.ParaField.IP_ADDRESS, Value = userAuthorizationPara.IPAddress });

            object returnObject = base.ExecuteScalar(commandText, dbParameters);
            if (returnObject != null && returnObject.GetType() != typeof(DBNull))
            {
                return EnumValidateUserAuthorizationResult.Success;
            }
            return EnumValidateUserAuthorizationResult.Failure;
        }

        public class UserMainPara
        {
            public enum ParaField
            {
                USER_ID,
                LAST_LOGIN_DATE,
                UPD_USER_ID
            }

            public DBVarChar UserID;
            public DBChar LastLoginDate;
            public DBVarChar UpdUserID;
        }

        public class UserMain : DBTableRow
        {
            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UserComID;
            public DBVarChar UserUnitID;
            public DBNVarChar UserUnitNM;
            public DBNVarChar UserIdentity;
        }

        public UserMain SelectUserMain(UserMainPara userMainPara)
        {
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT S.USER_ID AS UserID, U.USER_NM AS UserNM ",
                "     , U.USER_COM_ID AS UserComID, O.UNIT_ID AS UserUnitID, O.UNIT_NM AS UserUnitNM, UO.USER_IDENTITY AS UserIdentity ",
                "FROM SYS_USER_MAIN S ",
                "JOIN RAW_CM_USER U ON S.USER_ID = U.USER_ID ",
                "JOIN RAW_CM_ORG_UNIT O ON U.USER_UNIT_ID = O.UNIT_ID ",
                "JOIN RAW_CM_USER_ORG UO ON S.USER_ID = UO.USER_ID",
                "WHERE S.USER_ID={USER_ID} AND U.IS_LEFT='N'; ",
                Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_ID.ToString(), Value = userMainPara.UserID });
            return GetEntityList<UserMain>(commandText, dbParameters).SingleOrDefault();
        }

        public bool EditUserMain(UserMainPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DECLARE @IS_DAILY_FIRST CHAR(1)='N'; ", Environment.NewLine,
                "        IF NOT EXISTS (SELECT USER_ID FROM SYS_USER_MAIN WHERE USER_ID={USER_ID} AND LAST_LOGIN_DATE={LAST_LOGIN_DATE}) ", Environment.NewLine,
                "        BEGIN ", Environment.NewLine,
                "            SET @IS_DAILY_FIRST='Y'; ", Environment.NewLine,
                "        END ", Environment.NewLine,

                "        UPDATE SYS_USER_MAIN SET ", Environment.NewLine,
                "            IS_DAILY_FIRST=@IS_DAILY_FIRST ", Environment.NewLine,
                "          , LAST_LOGIN_DATE={LAST_LOGIN_DATE} ", Environment.NewLine,
                "          , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "          , UPD_DT=GETDATE() ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID}; ", Environment.NewLine,

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

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.LAST_LOGIN_DATE.ToString(), Value = para.LastLoginDate });
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.UPD_USER_ID.ToString(), Value = para.UpdUserID });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
            return result.GetValue() == EnumYN.Y.ToString();
        }

        #region - 取得使用者登入事件 -
        public class UserLoginEventPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public class UserLoginEvent : DBTableRow
        {
            public DBNVarChar TargetPath;
            public DBNVarChar ValidPath;
            public DBVarChar SysID;
            public DBBit IsDirect;
            public DBChar IsOutSourcing;
        }

        public UserLoginEvent SelectUserLoginEvent(UserLoginEventPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @DATETIMENOW DATETIME = GETDATE()",
                ";WITH USERLOGINEVENT AS(",
                "    SELECT USER_ID",
                "         , SYS_ID",
                "         , LOGIN_EVENT_ID",
                "         , MAX(DONE_DT) AS DONE_DT",
                "      FROM SYS_USER_LOGIN_EVENT",
                "     WHERE USER_ID = {USER_ID}",
                "     GROUP BY USER_ID, SYS_ID, LOGIN_EVENT_ID",
                ")",
                "SELECT TOP 1 ",
                "       TargetPath",
                "     , ValidPath",
                "     , SysID",
                "     , IsOutSourcing",
                "     , IsDirect",
                "     , SortOrder",
                "  FROM (",
                "       SELECT S.TARGET_PATH AS TargetPath",
                "            , S.VALID_PATH AS ValidPath",
                "            , S.SYS_ID AS SysID",
                "            , M.IS_OUTSOURCING AS IsOutSourcing",
                "            , (CASE WHEN U.DONE_DT IS NULL",
                "                      OR (CAST(U.DONE_DT AS DATE) != CAST(@DATETIMENOW AS DATE)",
                "                     AND  DATEDIFF(DAY, U.DONE_DT, @DATETIMENOW) >= ISNULL(S.FREQUENCY, 0))",
                "                    THEN 1",
                "                    ELSE 0",
                "               END) AS IsDirect",
                "            , S.SORT_ORDER AS SortOrder",
                "         FROM SYS_SYSTEM_LOGIN_EVENT S",
                "         JOIN SYS_SYSTEM_MAIN M",
                "           ON M.SYS_ID = S.SYS_ID",
                "         LEFT JOIN USERLOGINEVENT U",
                "           ON S.SYS_ID = U.SYS_ID",
                "          AND S.LOGIN_EVENT_ID = U.LOGIN_EVENT_ID",
                "        WHERE S.IS_DISABLE = '" + EnumYN.N + "'",
                "          AND @DATETIMENOW <= S.END_DT",
                "          AND @DATETIMENOW >= S.START_DT",
                "          AND CAST(@DATETIMENOW AS TIME(3)) BETWEEN S.START_EXEC_TIME AND S.END_EXEC_TIME",
                "       ) S",
                " WHERE IsDirect = 1",
                " ORDER BY SortOrder ASC"
            }));

            dbParameters.Add(new DBParameter { Name = UserLoginEventPara.ParaField.USER_ID, Value = para.UserID });
            return GetEntityList<UserLoginEvent>(commandText.ToString(), dbParameters).SingleOrDefault();
        }
        #endregion
    }
}