using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.B2P.Sys
{
    public class EntityUserRoleFunDetail : EntitySys
    {
        public EntityUserRoleFunDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserRawDataPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public class UserRawData : DBTableRow
        {
            public enum DataField
            {
                USER_ID, USER_NM, ROLE_GROUP_ID
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar RoleGroupID;
        }

        public UserRawData SelectUserRawData(UserRawDataPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT USER_ID, dbo.FN_GET_USER_NM(USER_ID) AS USER_NM ", Environment.NewLine,
                "     , ROLE_GROUP_ID ", Environment.NewLine,
                "FROM SYS_USER_MAIN ", Environment.NewLine,
                "WHERE USER_ID={USER_ID}; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserRawDataPara.ParaField.USER_ID.ToString(), Value = para.UserID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                UserRawData userRawData = new UserRawData()
                {
                    UserID = new DBVarChar(dataTable.Rows[0][UserRawData.DataField.USER_ID.ToString()]),
                    UserNM = new DBNVarChar(dataTable.Rows[0][UserRawData.DataField.USER_NM.ToString()]),
                    RoleGroupID = new DBVarChar(dataTable.Rows[0][UserRawData.DataField.ROLE_GROUP_ID.ToString()])
                };
                return userRawData;
            }
            return null;
        }

        public bool SelectUserIsDisable(UserRawDataPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT IS_LEFT ", Environment.NewLine,
                "FROM RAW_CM_USER ", Environment.NewLine,
                "WHERE USER_ID={USER_ID}; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserRawDataPara.ParaField.USER_ID.ToString(), Value = para.UserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? true : false;
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
            public enum DataField
            {
                SYS_ID, ROLE_ID, REMARK
            }

            public DBVarChar SysID;
            public DBVarChar RoleID;
            public DBNVarChar Remark;

            public string ItemText()
            {
                return this.Remark.StringValue();
            }

            public string ItemValue()
            {
                return string.Format("{0}|{1}", this.SysID.StringValue(), this.RoleID.StringValue());
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
            string commandText = string.Concat(new object[]
            {
                "SELECT C.SYS_ID, C.ROLE_ID, G.REMARK ", Environment.NewLine,
                "FROM SYS_SYSTEM_ROLE_GROUP_COLLECT C ", Environment.NewLine,
                "JOIN SYS_SYSTEM_ROLE_GROUP G ON C.ROLE_GROUP_ID=G.ROLE_GROUP_ID ", Environment.NewLine,
                "WHERE C.ROLE_GROUP_ID={ROLE_GROUP_ID} ", Environment.NewLine,
                "ORDER BY C.SYS_ID, C.ROLE_ID; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupCollectPara.ParaField.ROLE_GROUP_ID.ToString(), Value = para.RoleGroupID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemRoleGroupCollect> sysSystemRoleGroupCollectList = new List<SysSystemRoleGroupCollect>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemRoleGroupCollect sysSystemRoleGroupCollect = new SysSystemRoleGroupCollect()
                    {
                        SysID = new DBVarChar(dataRow[SysSystemRoleGroupCollect.DataField.SYS_ID.ToString()]),
                        RoleID = new DBVarChar(dataRow[SysSystemRoleGroupCollect.DataField.ROLE_ID.ToString()]),
                        Remark = new DBNVarChar(dataRow[SysSystemRoleGroupCollect.DataField.REMARK.ToString()])
                    };
                    sysSystemRoleGroupCollectList.Add(sysSystemRoleGroupCollect);
                }
                return sysSystemRoleGroupCollectList;
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
                        HasRole = new DBChar(dataRow[UserSystemRole.DataField.HAS_ROLE.ToString()]),
                    };
                    userSystemRoleList.Add(userRole);
                }
                return userSystemRoleList;
            }
            return null;
        }

        public enum EnumEditUserSystemRoleResult
        {
            Success, Failure
        }

        public EnumEditUserSystemRoleResult EditUserSystemRole(UserSystemRolePara para, List<UserSystemRolePara> paraList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            if (paraList == null || paraList.Count == 0)
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
                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.EXEC_IP_ADDRESS, Value = para.ExecIPAddress });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, deleteCommand, dbParameters));
                dbParameters.Clear();
            }
            else
            {
                string deleteCommand = string.Concat(new object[]
                {
                    "DELETE FROM SYS_USER_SYSTEM_ROLE WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                    "DELETE FROM SYS_USER_SYSTEM WHERE USER_ID={USER_ID} AND SYS_ID NOT IN (SELECT SYS_ID FROM SYS_SYSTEM_MAIN WHERE IS_OUTSOURCING='Y'); ", Environment.NewLine,
                    "DELETE FROM SYS_USER_FUN WHERE USER_ID={USER_ID} AND IS_ASSIGN='N'; ", Environment.NewLine,
                    "UPDATE SYS_USER_MAIN SET ROLE_GROUP_ID=NULL, IS_DISABLE={IS_DISABLE}, UPD_USER_ID={UPD_USER_ID}, UPD_DT=GETDATE() WHERE USER_ID={USER_ID}; ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID, Value = para.UserID });
                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.ROLE_GROUP_ID, Value = para.RoleGroupID });
                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.IS_DISABLE, Value = para.IsDisable });
                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, deleteCommand, dbParameters));
                dbParameters.Clear();

                foreach (UserSystemRolePara userSystemRolePara in paraList)
                {
                    string insertCommand = string.Concat(new object[]
                    {
                        "INSERT INTO SYS_USER_SYSTEM_ROLE VALUES ({USER_ID}, {SYS_ID}, {ROLE_ID}, {USER_ID}, GETDATE()); ", Environment.NewLine
                    });

                    dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID, Value = userSystemRolePara.UserID });
                    dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.SYS_ID, Value = userSystemRolePara.SysID });
                    dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.ROLE_ID, Value = userSystemRolePara.RoleID });

                    commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, insertCommand, dbParameters));
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

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, systemCommand, dbParameters));
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

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, functionCommand, dbParameters));
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

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, menuCommand, dbParameters));
                dbParameters.Clear();
            }

            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,
                commandTextStringBuilder.ToString(), Environment.NewLine,
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

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditUserSystemRoleResult.Success : EnumEditUserSystemRoleResult.Failure;
        }
    }
}