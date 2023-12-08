using System;
using System.Collections.Generic;

namespace LionTech.Entity.B2P
{
    public class Entity_Base : DBEntity
    {
        public Entity_Base(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserSystemRoleFunPara
        {

            public enum ParaField
            {
                USER_ID, SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME, SESSION_ID, IS_OUTSIDE
            }

            public DBVarChar UserID;
            public DBVarChar SystemID;
            public DBVarChar ControllerID;
            public DBVarChar ActionName;
            public DBChar SessionID;
            public DBChar IsOutside;
        }

        public class UserSystemRoleFun
        {
        }

        public enum EnumValidateUserSystemRoleFunResult
        {
            Success, Failure
        }

        public EnumValidateUserSystemRoleFunResult ValidateUserSystemRoleFun(UserSystemRoleFunPara userSystemRoleFunPara)
        {
            string whereCommandText = string.Empty;
            if (userSystemRoleFunPara.IsOutside.GetValue() == EnumYN.Y.ToString())
            {
                whereCommandText = string.Concat(new object[] 
                {
                    "      AND U.IS_OUTSIDE={IS_OUTSIDE} ", Environment.NewLine
                });
            }

            string commandText = string.Concat(new object[] 
            { 
                "DECLARE @FUN_CN INT = 0; ", Environment.NewLine,
                "DECLARE @SESSION_ID CHAR(24); ", Environment.NewLine,

                "SELECT TOP 1 @SESSION_ID=C.SESSION_ID ", Environment.NewLine,
                "FROM SYS_USER_CONNECT C ", Environment.NewLine,
                "JOIN SYS_USER_MAIN U ON C.USER_ID=U.USER_ID ", Environment.NewLine,
                "JOIN RAW_CM_USER R ON U.USER_ID=R.USER_ID ", Environment.NewLine,
                "WHERE C.USER_ID={USER_ID} AND C.SESSION_ID={SESSION_ID} AND C.CUST_LOGOUT='N' ", Environment.NewLine,
                "  AND U.IS_DISABLE='N' AND R.IS_LEFT='N' ", Environment.NewLine,
                "ORDER BY C.LAST_CONNECT_DT DESC; ", Environment.NewLine,

                "IF @SESSION_ID IS NOT NULL ", Environment.NewLine,
                "BEGIN ", Environment.NewLine,
                "    UPDATE SYS_USER_CONNECT SET ", Environment.NewLine,
                "        LAST_CONNECT_DT=GETDATE() ", Environment.NewLine,
                "    WHERE USER_ID={USER_ID} AND SESSION_ID={SESSION_ID}; ", Environment.NewLine,

                "    SELECT @FUN_CN=COUNT(F.FUN_ACTION_NAME) ", Environment.NewLine,
                "    FROM SYS_USER_SYSTEM_ROLE R ", Environment.NewLine,
                "    JOIN SYS_SYSTEM_ROLE_FUN F ON R.SYS_ID=F.SYS_ID AND R.ROLE_ID=F.ROLE_ID ", Environment.NewLine,
                "    JOIN SYS_SYSTEM_FUN U ON F.SYS_ID=U.SYS_ID AND F.FUN_CONTROLLER_ID=U.FUN_CONTROLLER_ID AND F.FUN_ACTION_NAME=U.FUN_ACTION_NAME ", Environment.NewLine,
                "    JOIN SYS_SYSTEM_MAIN M ON U.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "    WHERE R.USER_ID={USER_ID} ", Environment.NewLine,
                whereCommandText,
                "      AND F.SYS_ID={SYS_ID} AND F.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND F.FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                "      AND U.IS_DISABLE='N' AND M.IS_DISABLE='N'; ", Environment.NewLine,

                "    IF @FUN_CN=0 ", Environment.NewLine,
                "    BEGIN ", Environment.NewLine,
                "        SELECT @FUN_CN=COUNT(U.FUN_ACTION_NAME) ", Environment.NewLine,
                "        FROM SYS_USER_FUN F ", Environment.NewLine,
                "        JOIN SYS_SYSTEM_FUN U ON F.SYS_ID=U.SYS_ID AND F.FUN_CONTROLLER_ID=U.FUN_CONTROLLER_ID AND F.FUN_ACTION_NAME=U.FUN_ACTION_NAME ", Environment.NewLine,
                "        JOIN SYS_SYSTEM_MAIN M ON U.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "        WHERE F.USER_ID={USER_ID} AND F.IS_ASSIGN='Y' ", Environment.NewLine,
                whereCommandText,
                "          AND F.SYS_ID={SYS_ID} AND F.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND F.FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                "          AND U.IS_DISABLE='N' AND M.IS_DISABLE='N'; ", Environment.NewLine,
                "    END; ", Environment.NewLine,
                "END; ", Environment.NewLine,
                "SELECT @FUN_CN; ", Environment.NewLine
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSystemRoleFunPara.ParaField.USER_ID.ToString(), Value = userSystemRoleFunPara.UserID });
            dbParameters.Add(new DBParameter { Name = UserSystemRoleFunPara.ParaField.SESSION_ID.ToString(), Value = userSystemRoleFunPara.SessionID });
            dbParameters.Add(new DBParameter { Name = UserSystemRoleFunPara.ParaField.SYS_ID.ToString(), Value = userSystemRoleFunPara.SystemID });
            dbParameters.Add(new DBParameter { Name = UserSystemRoleFunPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = userSystemRoleFunPara.ControllerID });
            dbParameters.Add(new DBParameter { Name = UserSystemRoleFunPara.ParaField.FUN_ACTION_NAME.ToString(), Value = userSystemRoleFunPara.ActionName });
            dbParameters.Add(new DBParameter { Name = UserSystemRoleFunPara.ParaField.IS_OUTSIDE.ToString(), Value = userSystemRoleFunPara.IsOutside });

            DBInt result = new DBInt(base.ExecuteScalar(commandText, dbParameters));
            if (result.GetValue() > 0)
            {
                return EnumValidateUserSystemRoleFunResult.Success;
            }
            return EnumValidateUserSystemRoleFunResult.Failure;
        }
    }
}