using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityUserSystemRole : EntitySys
    {
        public EntityUserSystemRole(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
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
                USER_ID, USER_NM, ROLE_GROUP_ID
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar RoleGroupID;
        }

        public UserMain SelectUserMainInfo(UserMainPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT M.USER_ID, R.USER_NM AS USER_NM ", Environment.NewLine,
                "     , M.ROLE_GROUP_ID ", Environment.NewLine,
                "FROM SYS_USER_MAIN M ", Environment.NewLine,
                "INNER JOIN RAW_CM_USER R ON M.USER_ID=R.USER_ID ", Environment.NewLine,
                "WHERE M.USER_ID={USER_ID}; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_ID.ToString(), Value = para.UserID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                UserMain userMain = new UserMain()
                {
                    UserID = new DBVarChar(dataTable.Rows[0][UserMain.DataField.USER_ID.ToString()]),
                    UserNM = new DBNVarChar(dataTable.Rows[0][UserMain.DataField.USER_NM.ToString()]),
                    RoleGroupID = new DBVarChar(dataTable.Rows[0][UserMain.DataField.ROLE_GROUP_ID.ToString()])
                };
                return userMain;
            }
            return null;
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
                ROLE_GROUP_ID, IS_DISABLE,
                SYS_ID, SYS_NM, ROLE_ID, ROLE_NM, UPD_USER_ID, EXEC_IP_ADDRESS
            }

            public DBVarChar UserID;
            public DBVarChar RoleGroupID;
            public DBChar IsDisable;
            public DBVarChar SysID;
            public DBVarChar RoleID;
            public DBVarChar UpdUserID;
            public DBVarChar ExecIPAddress;
        }

        public class UserSystemRole : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM, ROLE_ID, ROLE_NM, HAS_ROLE
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
            public DBChar HasRole;
        }

        public List<UserSystemRole> SelectUserSystemRoleList(UserSystemRolePara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT {USER_ID} AS USER_ID ", Environment.NewLine,
                "     , R.SYS_ID, dbo.FN_GET_NMID(R.SYS_ID, S.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , R.ROLE_ID, dbo.FN_GET_NMID(R.ROLE_ID, R.{ROLE_NM}) AS ROLE_NM ", Environment.NewLine,
                "     , (CASE WHEN U.USER_ID IS NOT NULL THEN 'Y' ELSE 'N' END) AS HAS_ROLE ", Environment.NewLine,
                "FROM SYS_SYSTEM_ROLE R ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN S ON R.SYS_ID=S.SYS_ID ", Environment.NewLine,
                "LEFT OUTER JOIN (SELECT USER_ID, SYS_ID, ROLE_ID ", Environment.NewLine,
                "                 FROM SYS_USER_SYSTEM_ROLE ", Environment.NewLine,
                "                 WHERE USER_ID={USER_ID}) U ", Environment.NewLine,
                "ON R.SYS_ID=U.SYS_ID AND R.ROLE_ID=U.ROLE_ID ", Environment.NewLine,
                "ORDER BY S.SORT_ORDER, R.SYS_ID, R.ROLE_ID ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserSystemRolePara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserSystemRolePara.ParaField.ROLE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserSystemRole> userSystemRoleList = new List<UserSystemRole>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserSystemRole userRole = new UserSystemRole()
                    {
                        SysID = new DBVarChar(dataRow[UserSystemRole.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[UserSystemRole.DataField.SYS_NM.ToString()]),
                        RoleID = new DBVarChar(dataRow[UserSystemRole.DataField.ROLE_ID.ToString()]),
                        RoleNM = new DBNVarChar(dataRow[UserSystemRole.DataField.ROLE_NM.ToString()]),
                        HasRole = new DBChar(dataRow[UserSystemRole.DataField.HAS_ROLE.ToString()])
                    };
                    userSystemRoleList.Add(userRole);
                }
                return userSystemRoleList;
            }
            return null;
        }
    }
}