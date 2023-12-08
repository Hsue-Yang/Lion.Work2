using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Home
{
    public class EntityIndex : DBEntity
    {
        public EntityIndex(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
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

        public class UserAuthorization : DBTableRow
        {
            public enum DataField
            {
            }
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
                "WHERE U.USER_ID={USER_ID} AND U.IS_DISABLE='N' AND R.IS_LEFT='N' ", Environment.NewLine,
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
            if (returnObject != null && returnObject.GetType() != typeof(System.DBNull))
            {
                return EnumValidateUserAuthorizationResult.Success;
            }
            return EnumValidateUserAuthorizationResult.Failure;
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
            public enum DataField
            {
                USER_ID, USER_NM, USER_COM_ID, USER_UNIT_ID, USER_UNIT_NM,
                DEFAULT_SYS_ID, DEFAULT_PATH
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UserComID;
            public DBVarChar UserUnitID;
            public DBNVarChar UserUnitNM;
            public DBVarChar DefaultSysID;
            public DBNVarChar DefaultPath;
        }

        public UserMain SelectUserMain(UserMainPara userMainPara)
        {
            string commandText = string.Concat(new object[] 
            { 
                "SELECT S.USER_ID, U.USER_NM, U.USER_COM_ID, O.UNIT_ID AS USER_UNIT_ID, O.UNIT_NM AS USER_UNIT_NM ", Environment.NewLine,
                "     , D.DEFAULT_SYS_ID, D.DEFAULT_PATH ", Environment.NewLine,
                "FROM SYS_USER_MAIN S ", Environment.NewLine,
                "JOIN SYS_USER_DETAIL D ON S.USER_ID=D.USER_ID ", Environment.NewLine,
                "JOIN RAW_CM_USER U ON S.USER_ID=U.USER_ID ", Environment.NewLine,
                "JOIN RAW_CM_ORG_UNIT O ON U.USER_UNIT_ID=O.UNIT_ID ", Environment.NewLine,
                "WHERE S.USER_ID={USER_ID} AND S.IS_DISABLE='N' AND U.IS_LEFT='N'; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_ID.ToString(), Value = userMainPara.UserID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                UserMain userMain = new UserMain()
                {
                    UserID = new DBVarChar(dataTable.Rows[0][UserMain.DataField.USER_ID.ToString()]),
                    UserNM = new DBNVarChar(dataTable.Rows[0][UserMain.DataField.USER_NM.ToString()]),
                    UserComID = new DBVarChar(dataTable.Rows[0][UserMain.DataField.USER_COM_ID.ToString()]),
                    UserUnitID = new DBVarChar(dataTable.Rows[0][UserMain.DataField.USER_UNIT_ID.ToString()]),
                    UserUnitNM = new DBNVarChar(dataTable.Rows[0][UserMain.DataField.USER_UNIT_NM.ToString()]),
                    DefaultSysID = new DBVarChar(dataTable.Rows[0][UserMain.DataField.DEFAULT_SYS_ID.ToString()]),
                    DefaultPath = new DBNVarChar(dataTable.Rows[0][UserMain.DataField.DEFAULT_PATH.ToString()])
                };
                return userMain;
            }
            return null;
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
            public enum DataField
            {
                USER_ID, SYS_ID
            }

            public DBVarChar UserID;
            public DBVarChar SystemID;
        }

        public List<UserSystem> SelectUserSystemList(UserSystemPara userSystemPara)
        {
            string commandText = string.Concat(new object[] 
            { 
                "SELECT U.USER_ID, U.SYS_ID ", Environment.NewLine,
                "FROM SYS_USER_SYSTEM U ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN S ON U.SYS_ID=S.SYS_ID ", Environment.NewLine,
                "WHERE U.USER_ID={USER_ID} AND S.IS_DISABLE='N' AND S.IS_OUTSOURCING='N' ", Environment.NewLine,
                "ORDER BY U.SYS_ID; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSystemPara.ParaField.USER_ID.ToString(), Value = userSystemPara.UserID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserSystem> userSystemList = new List<UserSystem>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserSystem userSystem = new UserSystem()
                    {
                        UserID = new DBVarChar(dataRow[UserSystem.DataField.USER_ID.ToString()]),
                        SystemID = new DBVarChar(dataRow[UserSystem.DataField.SYS_ID.ToString()])
                    };
                    userSystemList.Add(userSystem);
                }
                return userSystemList;
            }
            return null;
        }

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
            public enum DataField
            {
                USER_ID, SYS_ID, ROLE_ID
            }

            public DBVarChar UserID;
            public DBVarChar SystemID;
            public DBVarChar RoleID;
        }

        public List<UserSystemRole> SelectUserSystemRoleList(UserSystemRolePara userSystemRolePara)
        {
            string commandText = string.Concat(new object[] 
            { 
                "SELECT R.USER_ID, R.SYS_ID, R.ROLE_ID ", Environment.NewLine,
                "FROM SYS_USER_SYSTEM_ROLE R ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON R.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "WHERE R.USER_ID={USER_ID} AND M.IS_DISABLE='N' ", Environment.NewLine,
                "ORDER BY R.USER_ID, R.SYS_ID, R.ROLE_ID; ",  Environment.NewLine
            });
            
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID.ToString(), Value = userSystemRolePara.UserID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserSystemRole> userSystemRoleList = new List<UserSystemRole>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserSystemRole userSystemRole = new UserSystemRole()
                    {
                        UserID = new DBVarChar(dataRow[UserSystemRole.DataField.USER_ID.ToString()]),
                        SystemID = new DBVarChar(dataRow[UserSystemRole.DataField.SYS_ID.ToString()]),
                        RoleID = new DBVarChar(dataRow[UserSystemRole.DataField.ROLE_ID.ToString()])
                    };
                    userSystemRoleList.Add(userSystemRole);
                }
                return userSystemRoleList;
            }
            return null;
        }

        public class UserInforPara
        {
            public enum ParaField
            {
                USER_ID, USER_PWD
            }

            public DBVarChar UserID;
            public DBVarChar UserPWD;
        }

        public bool ValidateUserInfor(UserInforPara userInforPara)
        {
            string commandText = string.Concat(new object[] 
            { 
                "DECLARE @RESULT INT; ", Environment.NewLine,
                "SELECT @RESULT=COUNT(1) ", Environment.NewLine,
                "FROM SYS_USER_MAIN M ", Environment.NewLine,
                "JOIN SYS_USER_DETAIL D ON M.USER_ID = D.USER_ID ", Environment.NewLine,
                "WHERE M.IS_DISABLE='N' ", Environment.NewLine,
                "  AND M.USER_ID={USER_ID} AND D.USER_PWD={USER_PWD}; ", Environment.NewLine,
                "SELECT @RESULT; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserInforPara.ParaField.USER_ID.ToString(), Value = userInforPara.UserID });
            dbParameters.Add(new DBParameter { Name = UserInforPara.ParaField.USER_PWD.ToString(), Value = userInforPara.UserPWD });

            DBInt result = new DBInt(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == 1) ? true : false;
        }

        public class SCMAPB2PSettingSupB2PUserBulletin : DBTableRow
        {
            public DBVarChar UserID;
        }
    }
}