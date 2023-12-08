using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemFunAssign : EntitySys
    {
        public EntitySystemFunAssign(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class RAWUserPara
        {
            public enum ParaField
            {
                CONDITION, CONDITION_LENGTH
            }

            public DBNVarChar UserCondition;
        }

        public class RAWUser : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                USER_ID, USER_NM
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;

            public string ItemText()
            {
                return this.UserNM.StringValue();
            }

            public string ItemValue()
            {
                return this.UserID.GetValue();
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

        public List<RAWUser> SelectRAWUserList(RAWUserPara para)
        {
            string commandText = string.Concat(new object[] { 
                "SELECT USER_ID, USER_NM ", Environment.NewLine,
                "FROM RAW_CM_USER ", Environment.NewLine,
                "WHERE SUBSTRING(USER_ID,1,{CONDITION_LENGTH})={CONDITION} ", Environment.NewLine,
                "      OR SUBSTRING(USER_NM,1,{CONDITION_LENGTH})={CONDITION}; ", Environment.NewLine,
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RAWUserPara.ParaField.CONDITION.ToString(), Value = para.UserCondition });
            dbParameters.Add(new DBParameter { Name = RAWUserPara.ParaField.CONDITION_LENGTH.ToString(), Value = new DBObject(para.UserCondition.GetValue().Length.ToString()) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                var rawUserList = new List<RAWUser>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    var rawUser = new RAWUser()
                    {
                        UserID = new DBVarChar(dataRow[RAWUser.DataField.USER_ID.ToString()]),
                        UserNM = new DBNVarChar(dataRow[RAWUser.DataField.USER_NM.ToString()])
                    };
                    rawUserList.Add(rawUser);
                }
                return rawUserList;
            }
            return null;
        }

        public class SystemFunAssignPara : DBCulture
        {
            public SystemFunAssignPara(string cultureID)
                : base(cultureID)
            {

            }

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
            public DBVarChar UpdUserID;
        }

        public class SystemFunAssign : DBTableRow
        {
            public enum DataField
            {
                USER_ID, USER_NM
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
        }

        public List<SystemFunAssign> SelectSystemFunAssignList(SystemFunAssignPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT U.USER_ID AS UserID",
                "     , R.USER_NM AS UserNM",
                "  FROM SYS_USER_FUN U",
                "  JOIN RAW_CM_USER R",
                "    ON U.USER_ID = R.USER_ID",
                " WHERE U.SYS_ID = {SYS_ID}",
                "   AND U.FUN_CONTROLLER_ID = {FUN_CONTROLLER_ID}",
                "   AND U.FUN_ACTION_NAME = {FUN_ACTION_NAME}",
                "   AND U.IS_ASSIGN = '" + EnumYN.Y + "'",
                "ORDER BY U.USER_ID;"
            }));

            dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionName });

            return GetEntityList<SystemFunAssign>(commandText.ToString(), dbParameters);
        }

        public enum EnumEditSystemFunAssignResult
        {
            Success, Failure
        }

        public EnumEditSystemFunAssignResult EditSystemFunAssign(SystemFunAssignPara para)
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

                dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.SYS_ID, Value = para.SysID });
                dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
                dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
                dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.USER_ID_LIST, Value = para.UserIDList });
                dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

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

            dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
            if (para.UserIDList != null && para.UserIDList.Count > 0)
            {
                dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.USER_ID_LIST, Value = para.UserIDList });
            }
            dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

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
            
            dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });

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

                        dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.USER_ID, Value = userID });
                        dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.SYS_ID, Value = para.SysID });
                        dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
                        dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
                        dbParameters.Add(new DBParameter { Name = SystemFunAssignPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

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
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditSystemFunAssignResult.Success : EnumEditSystemFunAssignResult.Failure;
        }

        public class SystemFunAssignValue : ValueListRow
        {
            public string UserID { get; set; }

            public DBVarChar GetUserID()
            {
                return new DBVarChar(this.UserID);
            }
        }
    }
}