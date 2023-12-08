using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.B2P.Sys
{
    public class EntityUserFunction : EntitySys
    {
        public EntityUserFunction(string connectionString, string providerName)
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
                USER_ID, USER_NM,
                IS_DISABLE
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBChar IsDisable;
        }

        public UserRawData SelectUserRawData(UserRawDataPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT USER_ID, dbo.FN_GET_USER_NM(USER_ID) AS USER_NM ", Environment.NewLine,
                "     , IS_DISABLE ", Environment.NewLine,
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
                    IsDisable = new DBChar(dataTable.Rows[0][UserRawData.DataField.IS_DISABLE.ToString()])
                };
                return userRawData;
            }
            return null;
        }

        public class UserFunctionPara : DBCulture
        {
            public UserFunctionPara(string cultureID)
                : base(cultureID)
            {
            
            }

            public enum ParaField
            {
                USER_ID, IS_DISABLE,
                SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME,
                FUNCTION_LIST,
                UPD_USER_ID,

                SYS_NM, FUN_GROUP, FUN_NM
            }

            public DBVarChar UserID;
            public DBChar IsDisable;
            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
            public List<DBVarChar> FunctionList;
            public DBVarChar UpdUserID;
        }

        public class UserFunction : DBTableRow
        {
            public enum DataField
            {
                HAS_AUTH,
                SYS_ID, SYS_NM, FUN_CONTROLLER_ID, FUN_GROUP, FUN_ACTION_NAME, FUN_NM,
                UPD_USER_ID, UPD_USER_NM, UPD_DT
            }

            public DBChar HasAuth;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar FunControllID;
            public DBNVarChar FunGroup;
            public DBVarChar FunActionName;
            public DBNVarChar FunNM;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<UserFunction> SelectUserFunctionList(UserFunctionPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT (CASE WHEN Z.SYS_ID IS NULL THEN 'N' WHEN U.SYS_ID=Z.SYS_ID THEN 'Y' ELSE 'N' END) AS HAS_AUTH ", Environment.NewLine,
                "     , U.SYS_ID, dbo.FN_GET_NMID(U.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , U.FUN_CONTROLLER_ID, dbo.FN_GET_NMID(U.FUN_CONTROLLER_ID, G.{FUN_GROUP}) AS FUN_GROUP ", Environment.NewLine,
                "     , U.FUN_ACTION_NAME, dbo.FN_GET_NMID(U.FUN_ACTION_NAME, F.{FUN_NM}) AS FUN_NM ", Environment.NewLine,
                "     , U.UPD_USER_ID, dbo.FN_GET_USER_NM(U.UPD_USER_ID) AS UPD_USER_NM ", Environment.NewLine,
                "     , U.UPD_DT ", Environment.NewLine,
                "FROM SYS_USER_FUN U ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON U.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_FUN_GROUP G ON U.SYS_ID=G.SYS_ID AND U.FUN_CONTROLLER_ID=G.FUN_CONTROLLER_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_FUN F ON U.SYS_ID=F.SYS_ID AND U.FUN_CONTROLLER_ID=F.FUN_CONTROLLER_ID AND U.FUN_ACTION_NAME=F.FUN_ACTION_NAME ", Environment.NewLine,
                "LEFT JOIN ( ", Environment.NewLine,
                "    SELECT SYS_ID ", Environment.NewLine,
                "    FROM SYS_USER_SYSTEM_ROLE ", Environment.NewLine,
                "    WHERE USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "    GROUP BY SYS_ID ", Environment.NewLine,
                ") Z ON U.SYS_ID=Z.SYS_ID ", Environment.NewLine,
                "WHERE U.USER_ID={USER_ID} AND U.IS_ASSIGN='Y'; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.UPD_USER_ID.ToString(), Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserFunctionPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserFunctionPara.ParaField.FUN_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserFunctionPara.ParaField.FUN_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserFunction> userFunctionList = new List<UserFunction>();

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserFunction userFunction = new UserFunction()
                    {
                        HasAuth = new DBChar(dataRow[UserFunction.DataField.HAS_AUTH.ToString()]),
                        SysID = new DBVarChar(dataRow[UserFunction.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[UserFunction.DataField.SYS_NM.ToString()]),
                        FunControllID = new DBVarChar(dataRow[UserFunction.DataField.FUN_CONTROLLER_ID.ToString()]),
                        FunGroup = new DBNVarChar(dataRow[UserFunction.DataField.FUN_GROUP.ToString()]),
                        FunActionName = new DBVarChar(dataRow[UserFunction.DataField.FUN_ACTION_NAME.ToString()]),
                        FunNM = new DBNVarChar(dataRow[UserFunction.DataField.FUN_NM.ToString()]),
                        UpdUserID = new DBVarChar(dataRow[UserFunction.DataField.UPD_USER_ID.ToString()]),
                        UpdUserNM = new DBNVarChar(dataRow[UserFunction.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[UserFunction.DataField.UPD_DT.ToString()]),

                    };
                    userFunctionList.Add(userFunction);
                }

                return userFunctionList;
            }
            return null;
        }

        public enum EnumEditUserFunctionResult
        {
            Success, Failure
        }

        public EnumEditUserFunctionResult EditUserFunctionResult(UserFunctionPara para)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            string deleteCommand = string.Concat(new object[]
            {
                "DELETE FROM SYS_USER_FUN WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                "UPDATE SYS_USER_MAIN SET IS_DISABLE={IS_DISABLE}, UPD_USER_ID={UPD_USER_ID}, UPD_DT=GETDATE() WHERE USER_ID={USER_ID}; ", Environment.NewLine
            });

            dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.IS_DISABLE, Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, deleteCommand, dbParameters));
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

            dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.USER_ID, Value = para.UserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, functionCommand, dbParameters));
            dbParameters.Clear();

            if (para.FunctionList != null && para.FunctionList.Count > 0)
            {
                string deleteCommandText = string.Concat(new object[] 
                {
                    "DELETE FROM SYS_USER_FUN ", Environment.NewLine,
                    "WHERE USER_ID={USER_ID} ", Environment.NewLine,
                    "  AND SYS_ID+'|'+FUN_CONTROLLER_ID+'|'+FUN_ACTION_NAME IN ({FUNCTION_LIST}); ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.USER_ID, Value = para.UserID });
                dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.FUNCTION_LIST, Value = para.FunctionList });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, deleteCommandText, dbParameters));
                dbParameters.Clear();

                foreach (DBVarChar function in para.FunctionList)
                {
                    if (function.IsNull() == false)
                    {
                        string insertCommand = string.Concat(new object[]
                        {
                            "INSERT INTO SYS_USER_FUN VALUES ( ", Environment.NewLine,
                            "    {USER_ID} ", Environment.NewLine,
                            "  , {SYS_ID}, {FUN_CONTROLLER_ID}, {FUN_ACTION_NAME} ", Environment.NewLine,
                            "  , 'Y' ", Environment.NewLine,
                            "  , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                            "); ", Environment.NewLine
                        });

                        dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.USER_ID, Value = para.UserID });
                        dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.SYS_ID, Value = new DBVarChar(function.GetValue().Split('|')[0]) });
                        dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.FUN_CONTROLLER_ID, Value = new DBVarChar(function.GetValue().Split('|')[1]) });
                        dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.FUN_ACTION_NAME, Value = new DBVarChar(function.GetValue().Split('|')[2]) });
                        dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                        commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, insertCommand, dbParameters));
                        dbParameters.Clear();
                    }
                }
            }

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

            dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.USER_ID, Value = para.UserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, menuCommand, dbParameters));
            dbParameters.Clear();

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
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditUserFunctionResult.Success : EnumEditUserFunctionResult.Failure;
        }

        public class SystemUserFunctionValue : ValueListRow
        {
            public enum ValueField
            {
                SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME
            }

            public string SysID { get; set; }
            public string FunControllerID { get; set; }
            public string FunActionName { get; set; }

            public DBVarChar GetSysID()
            {
                return new DBVarChar(SysID);
            }

            public DBVarChar GetFunControllerID()
            {
                return new DBVarChar(FunControllerID);
            }

            public DBVarChar GetFunActionName()
            {
                return new DBVarChar(FunActionName);
            }
        }
    }
}