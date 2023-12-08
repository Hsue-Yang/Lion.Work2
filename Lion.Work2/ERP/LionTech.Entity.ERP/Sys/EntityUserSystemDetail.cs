using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityUserSystemDetail : EntitySys
    {
        public EntityUserSystemDetail(string connectionString, string providerName)
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
                USER_ID, USER_NM
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
        }

        public UserRawData SelectUserRawData(UserRawDataPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT {USER_ID} AS USER_ID, dbo.FN_GET_USER_NM({USER_ID}) AS USER_NM ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserRawDataPara.ParaField.USER_ID.ToString(), Value = para.UserID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                UserRawData userRawData = new UserRawData()
                {
                    UserID = new DBVarChar(dataTable.Rows[0][UserRawData.DataField.USER_ID.ToString()]),
                    UserNM = new DBNVarChar(dataTable.Rows[0][UserRawData.DataField.USER_NM.ToString()])
                };
                return userRawData;
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
                USER_ID, CODE_NM,
                SYS_ID, SYS_NM,
                UPD_USER_ID
            }

            public DBVarChar UserID;
            public DBVarChar SysID;
            public DBVarChar UpdUserID;
        }

        public class UserSystemRole : DBTableRow
        {
            public enum DataField
            {
                DEPT_ID, DEPT_NM,
                SYS_ID, SYS_NM,
                HAS_SYS
            }

            public DBVarChar DeptID;
            public DBNVarChar DeptNM;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBChar HasSys;
        }

        public List<UserSystemRole> SelectUserSystemRoleList(UserSystemRolePara para)
        {
            string commandText = string.Concat(new object[]
            {
               " SELECT C.CODE_ID AS DEPT_ID ", Environment.NewLine,
	           "      , dbo.FN_GET_NMID(C.CODE_ID, C.{CODE_NM}) AS DEPT_NM ", Environment.NewLine,
	           "      , S.SYS_ID, dbo.FN_GET_NMID(S.SYS_ID, S.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
               "      , (CASE WHEN U.USER_ID IS NOT NULL THEN 'Y' ELSE 'N' END) AS HAS_SYS ", Environment.NewLine,
               " FROM SYS_SYSTEM_MAIN S ", Environment.NewLine,
               " LEFT OUTER JOIN ( ", Environment.NewLine,
               "     SELECT USER_ID, SYS_ID ", Environment.NewLine,
               "     FROM SYS_USER_SYSTEM ", Environment.NewLine,
               "     WHERE USER_ID={USER_ID} ", Environment.NewLine,
               " ) U ON S.SYS_ID=U.SYS_ID ", Environment.NewLine,
               " LEFT JOIN RAW_CM_USER_ORG O ON S.SYS_MAN_USER_ID=O.USER_ID ", Environment.NewLine,
               " LEFT JOIN CM_CODE C ON O.USER_DEPT=C.CODE_ID AND C.CODE_KIND='0018' ", Environment.NewLine,
               " WHERE S.IS_OUTSOURCING = 'Y' ", Environment.NewLine,
               " ORDER BY C.CODE_ID DESC ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserSystemRolePara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserSystemRolePara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserSystemRole> userSystemRoleList = new List<UserSystemRole>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserSystemRole userRole = new UserSystemRole()
                    {
                        DeptID = new DBVarChar(dataRow[UserSystemRole.DataField.DEPT_ID.ToString()]),
                        DeptNM = new DBNVarChar(dataRow[UserSystemRole.DataField.DEPT_NM.ToString()]),
                        SysID = new DBVarChar(dataRow[UserSystemRole.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[UserSystemRole.DataField.SYS_NM.ToString()]),
                        HasSys = new DBChar(dataRow[UserSystemRole.DataField.HAS_SYS.ToString()])
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
                    "DELETE FROM SYS_USER_SYSTEM ", Environment.NewLine,
                    "WHERE USER_ID={USER_ID} ", Environment.NewLine,
                    "  AND SYS_ID IN (SELECT SYS_ID FROM SYS_SYSTEM_MAIN WHERE IS_OUTSOURCING='Y'); ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID, Value = para.UserID });
                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, deleteCommand, dbParameters));
                dbParameters.Clear();
            }
            else
            {
                string deleteCommand = string.Concat(new object[]
                {
                    "DELETE FROM SYS_USER_SYSTEM ", Environment.NewLine,
                    "WHERE USER_ID={USER_ID} ", Environment.NewLine,
                    "  AND SYS_ID IN (SELECT SYS_ID FROM SYS_SYSTEM_MAIN WHERE IS_OUTSOURCING='Y'); ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID, Value = para.UserID });
                dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, deleteCommand, dbParameters));
                dbParameters.Clear();

                foreach (UserSystemRolePara userSystemRolePara in paraList)
                {
                    string insertCommand = string.Concat(new object[]
                    {
                        "INSERT INTO SYS_USER_SYSTEM VALUES ({USER_ID}, {SYS_ID}, {USER_ID}, GETDATE()); ", Environment.NewLine
                    });

                    dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID, Value = userSystemRolePara.UserID });
                    dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.SYS_ID, Value = userSystemRolePara.SysID });

                    commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, insertCommand, dbParameters));
                    dbParameters.Clear();
                }
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