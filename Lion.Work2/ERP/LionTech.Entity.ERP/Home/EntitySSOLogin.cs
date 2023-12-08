using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Home
{
    public class EntitySSOLogin : EntityHome
    {
        public EntitySSOLogin(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserConnectPara
        {
            public enum ParaField
            {
                SESSION_COUNT, DELAY_SECOND, USER_ID, SYSTEM_ID, SESSION_ID, SSO_VALID
            }

            public DBInt SessionCount;
            public DBInt DelaySecond;
            public DBVarChar UserID;
            public DBVarChar SystemID;
            public DBChar SessionID;
            public DBChar SSOValid;
        }
        
        public class UserConnect : DBTableRow
        {
            public enum DataField
            {
                USER_ID, SYS_ID, SESSION_ID, LAST_CONNECT_DT
            }

            public DBVarChar UserID;
            public DBVarChar SystemID;
            public DBChar SessionID;
            public DBDateTime LastConnectDT;
        }

        public UserConnect SelectUserConnect(UserConnectPara userConnectPara)
        {
            string commandText = string.Concat(new object[] 
            { 
                "SELECT C.USER_ID, S.SYS_ID, C.SESSION_ID, C.LAST_CONNECT_DT ", Environment.NewLine,
                "FROM (SELECT M.USER_ID, M.LAST_CONNECT_DT, M.SESSION_ID ", Environment.NewLine,
                "      FROM (SELECT TOP {SESSION_COUNT} USER_ID, LAST_CONNECT_DT, SESSION_ID, SSO_VALID ", Environment.NewLine,
                "            FROM SYS_USER_CONNECT ", Environment.NewLine,
                "            WHERE CUST_LOGOUT='N' ", Environment.NewLine,
                "            ORDER BY LAST_CONNECT_DT DESC) M ", Environment.NewLine,
                "      WHERE M.USER_ID={USER_ID} AND M.SESSION_ID={SESSION_ID} AND M.SSO_VALID='Y' ", Environment.NewLine,
                "        AND M.LAST_CONNECT_DT>=DATEADD(SECOND, {DELAY_SECOND}, GETDATE())) C ", Environment.NewLine,
                "JOIN SYS_USER_SYSTEM S ON C.USER_ID=S.USER_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON S.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "WHERE S.SYS_ID={SYSTEM_ID} AND M.IS_DISABLE='N'; ",  Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserConnectPara.ParaField.SESSION_COUNT.ToString(), Value = userConnectPara.SessionCount });
            dbParameters.Add(new DBParameter { Name = UserConnectPara.ParaField.DELAY_SECOND.ToString(), Value = userConnectPara.DelaySecond });
            dbParameters.Add(new DBParameter { Name = UserConnectPara.ParaField.USER_ID.ToString(), Value = userConnectPara.UserID });
            dbParameters.Add(new DBParameter { Name = UserConnectPara.ParaField.SYSTEM_ID.ToString(), Value = userConnectPara.SystemID });
            dbParameters.Add(new DBParameter { Name = UserConnectPara.ParaField.SESSION_ID.ToString(), Value = userConnectPara.SessionID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                UserConnect userConnect = new UserConnect()
                {
                    UserID = new DBVarChar(dataTable.Rows[0][UserConnect.DataField.USER_ID.ToString()]),
                    SystemID = new DBVarChar(dataTable.Rows[0][UserConnect.DataField.SYS_ID.ToString()]),
                    SessionID = new DBChar(dataTable.Rows[0][UserConnect.DataField.SESSION_ID.ToString()]),
                    LastConnectDT = new DBDateTime(dataTable.Rows[0][UserConnect.DataField.LAST_CONNECT_DT.ToString()])
                };
                return userConnect;
            }
            return null;
        }

        #region - 查詢使用者資料 -
        public class UserSessionDataPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public class SSOLoginResult : DBTableRow
        {
            public DBChar IsAuthorized;
            public UserSessionData Data;
        }

        public class UserSessionData : DBTableRow
        {
            public DBNVarChar UserNM;
            public DBVarChar UserEMail;
            public DBVarChar UserUnitID;
            public DBVarChar UserTeamID;
        }

        /// <summary>
        /// 查詢使用者資料
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public UserSessionData SelectUserSessionData(UserSessionDataPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    " SELECT USER_NM AS UserNM",
                    "      , USER_EMAIL AS UserEMail",
                    "      , USER_UNIT_ID AS UserUnitID",
                    "      , USER_TEAM_ID AS UserTeamID",
                    " FROM RAW_CM_USER",
                    " WHERE USER_ID = {USER_ID}",
                    Environment.NewLine
                }));

            dbParameters.Add(new DBParameter { Name = UserSessionDataPara.ParaField.USER_ID, Value = para.UserID });

            return GetEntityList<UserSessionData>(commandText.ToString(), dbParameters).SingleOrDefault();
        }
        #endregion
       
        public void ExecUserConnectSSOValid(UserConnectPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "UPDATE SYS_USER_CONNECT SET SSO_VALID={SSO_VALID} WHERE USER_ID={USER_ID} AND SESSION_ID={SESSION_ID}; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserConnectPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserConnectPara.ParaField.SESSION_ID, Value = para.SessionID });
            dbParameters.Add(new DBParameter { Name = UserConnectPara.ParaField.SSO_VALID, Value = para.SSOValid });

            base.ExecuteScalar(commandText, dbParameters);
        }
    }
}