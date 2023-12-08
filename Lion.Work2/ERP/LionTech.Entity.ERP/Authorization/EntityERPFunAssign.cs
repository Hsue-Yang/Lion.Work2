using System;
using System.Collections.Generic;
using System.Text;

namespace LionTech.Entity.ERP.Authorization
{
    public class EntityERPFunAssign : EntityAuthorization
    {
        public EntityERPFunAssign(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserFunPara
        {
            public enum ParaField
            {
                USER_ID,
                SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME,
                USER_ID_LIST,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
            public List<DBVarChar> UserIDList;
        }

        public enum EnumEditUserFunResult
        {
            Success, Failure
        }

        public EnumEditUserFunResult EditUserFun(UserFunPara para)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            #region - 預設處理將指派的資料更新成Y -
            string updateCommandText = string.Empty;
            if (para.UserIDList != null && para.UserIDList.Count > 0)
            {
                updateCommandText = string.Concat(new object[] 
                {
                    "UPDATE SYS_USER_FUN SET IS_ASSIGN='Y' ", Environment.NewLine,
                    "                      , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                    "                      , UPD_DT=GETDATE() ", Environment.NewLine,
                    "WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                    "  AND USER_ID IN ({USER_ID_LIST}); ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.SYS_ID, Value = para.SysID });
                dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
                dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
                dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.USER_ID_LIST, Value = para.UserIDList });
                dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.UPD_USER_ID, Value = base.UpdUserID });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, updateCommandText, dbParameters));
                dbParameters.Clear();
            }
            #endregion

            #region - 確保在移除指派人員時將原有權限寫回 -
            string whereCommandText = string.Empty;
            if (para.UserIDList != null && para.UserIDList.Count > 0)
            {
                whereCommandText = string.Concat(new object[] 
                {
                    "   AND U.USER_ID NOT IN ({USER_ID_LIST}) ", Environment.NewLine
                });
            }

            updateCommandText = string.Concat(new object[] 
            {
                "UPDATE SYS_USER_FUN SET SYS_USER_FUN.IS_ASSIGN='N' ", Environment.NewLine,
                "                      , SYS_USER_FUN.UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "                      , SYS_USER_FUN.UPD_DT=GETDATE() ", Environment.NewLine,
                "FROM SYS_USER_FUN U ", Environment.NewLine,
                "JOIN ( ", Environment.NewLine,
                "    SELECT R.USER_ID, F.SYS_ID, F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME ", Environment.NewLine,
                "    FROM SYS_SYSTEM_FUN F ", Environment.NewLine,
                "    JOIN SYS_SYSTEM_ROLE_FUN N ON F.SYS_ID=N.SYS_ID AND F.FUN_CONTROLLER_ID=N.FUN_CONTROLLER_ID AND F.FUN_ACTION_NAME=N.FUN_ACTION_NAME ", Environment.NewLine,
                "    JOIN SYS_USER_SYSTEM_ROLE R ON N.SYS_ID=R.SYS_ID AND N.ROLE_ID=R.ROLE_ID ", Environment.NewLine,
                "    WHERE F.SYS_ID={SYS_ID} AND F.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND F.FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                ") Z ON U.USER_ID=Z.USER_ID ", Environment.NewLine,
                "   AND U.SYS_ID=Z.SYS_ID AND U.FUN_CONTROLLER_ID=Z.FUN_CONTROLLER_ID AND U.FUN_ACTION_NAME=Z.FUN_ACTION_NAME ", Environment.NewLine,
                whereCommandText,
                "; ", Environment.NewLine
            });

            dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
            if (para.UserIDList != null && para.UserIDList.Count > 0)
            {
                dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.USER_ID_LIST, Value = para.UserIDList });
            }
            dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.UPD_USER_ID, Value = base.UpdUserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, updateCommandText, dbParameters));
            dbParameters.Clear();
            #endregion

            #region - Delete -
            string deleteCommandText = string.Concat(new object[] 
            {
                "DELETE FROM SYS_USER_FUN ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                "  AND IS_ASSIGN='Y'; ", Environment.NewLine
            });

            dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, deleteCommandText, dbParameters));
            dbParameters.Clear();
            #endregion

            #region - Insert -
            if (para.UserIDList != null && para.UserIDList.Count > 0)
            {
                foreach (DBVarChar userID in para.UserIDList)
                {
                    if (userID.IsNull() == false)
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

                        dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.USER_ID, Value = userID });
                        dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.SYS_ID, Value = para.SysID });
                        dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
                        dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
                        dbParameters.Add(new DBParameter { Name = UserFunPara.ParaField.UPD_USER_ID, Value = base.UpdUserID });

                        commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, insertCommand, dbParameters));
                        dbParameters.Clear();
                    }
                }
            }
            #endregion

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
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditUserFunResult.Success : EnumEditUserFunResult.Failure;
        }
    }
}