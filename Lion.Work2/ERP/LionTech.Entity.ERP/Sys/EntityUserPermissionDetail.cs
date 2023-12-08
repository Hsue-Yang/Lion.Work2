using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityUserPermissionDetail : EntitySys
    {
        public EntityUserPermissionDetail(string connectionString, string providerName)
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
                RESTRICT_TYPE,
                ERROR_TIMES, IS_LOCK
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBChar RestrictType;
            public DBInt ErrorTimes;
            public DBChar IsLock;
        }

        public UserRawData SelectUserRawData(UserRawDataPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT USER_ID, dbo.FN_GET_USER_NM(USER_ID) AS USER_NM ", Environment.NewLine,
                "     , RESTRICT_TYPE ", Environment.NewLine,
                "     , ERROR_TIMES, IS_LOCK ", Environment.NewLine,
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
                    RestrictType = new DBChar(dataTable.Rows[0][UserRawData.DataField.RESTRICT_TYPE.ToString()]),
                    ErrorTimes = new DBInt(dataTable.Rows[0][UserRawData.DataField.ERROR_TIMES.ToString()]),
                    IsLock = new DBChar(dataTable.Rows[0][UserRawData.DataField.IS_LOCK.ToString()])
                };
                return userRawData;
            }
            return null;
        }

        public class UserPermissionPara
        {
            public enum ParaField
            {
                USER_ID,
                RESTRICT_TYPE, IS_LOCK,
                UPD_USER_ID
            }

            public DBVarChar UserID;
            public DBChar RestrictType;
            public DBChar IsLock;
            public DBVarChar UpdUserID;
        }

        public enum EnumEditUserPermissionResult
        {
            Success, Failure
        }

        public EnumEditUserPermissionResult EditUserPermission(UserPermissionPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        UPDATE SYS_USER_MAIN SET ", Environment.NewLine,
                "            RESTRICT_TYPE={RESTRICT_TYPE} ", Environment.NewLine,
                "          , ERROR_TIMES=(CASE WHEN {IS_LOCK}='N' THEN 0 ELSE ERROR_TIMES END) ", Environment.NewLine,
                "          , IS_LOCK={IS_LOCK} ", Environment.NewLine,
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
            dbParameters.Add(new DBParameter { Name = UserPermissionPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserPermissionPara.ParaField.RESTRICT_TYPE, Value = para.RestrictType });
            dbParameters.Add(new DBParameter { Name = UserPermissionPara.ParaField.IS_LOCK, Value = para.IsLock });
            dbParameters.Add(new DBParameter { Name = UserPermissionPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditUserPermissionResult.Success : EnumEditUserPermissionResult.Failure;
        }

        public enum EnumSyncEditOpagm20Result
        {
            Success, Failure
        }

        public EnumSyncEditOpagm20Result SyncEditOpagm20(UserPermissionPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        UPDATE opagm20 SET ", Environment.NewLine,
                "            stfn_check_ip=(CASE WHEN {RESTRICT_TYPE}='N' THEN '1' ", Environment.NewLine,
                "                                WHEN {RESTRICT_TYPE}='R' THEN '' ", Environment.NewLine,
                "                                WHEN {RESTRICT_TYPE}='U' THEN '0' ", Environment.NewLine,
                "                                ELSE '1' END) ", Environment.NewLine,
                "          , stfn_pswd_err=(CASE WHEN {IS_LOCK}='N' THEN 0 ELSE stfn_pswd_err END) ", Environment.NewLine,
                "          , stfn_pswd_errtime=(CASE WHEN {IS_LOCK}='N' THEN NULL ELSE stfn_pswd_errtime END) ", Environment.NewLine,
                "        WHERE stfn_stfn={USER_ID}; ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = UserPermissionPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserPermissionPara.ParaField.RESTRICT_TYPE, Value = para.RestrictType });
            dbParameters.Add(new DBParameter { Name = UserPermissionPara.ParaField.IS_LOCK, Value = para.IsLock });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumSyncEditOpagm20Result.Success : EnumSyncEditOpagm20Result.Failure;
        }
    }
}