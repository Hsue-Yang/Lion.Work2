using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using LionTech.Utility;

namespace LionTech.Entity.ERP
{
    public class Entity_Base : DBEntity
    {
#if !NET461
        public Entity_Base(string connectionName)
            : base(connectionName)
        {
        }
#endif
        public Entity_Base(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class RestrictType
        {
            public bool IsRestrict;
            public bool IsPowerUser;
        }

        public class UserMainPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public class UserMain
        {
            public enum DataField
            {
                PWD_VALID_DATE
            }

            public DBDateTime PWDValidDate;
        }

        public UserMain SelectUserMain(UserMainPara userMainPara)
        {
            string commandText = string.Concat(new object[] 
            {
                "SELECT PWD_VALID_DATE ", Environment.NewLine,
                "FROM SYS_USER_MAIN ", Environment.NewLine,
                "WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "  AND IS_DISABLE='N'; ", Environment.NewLine
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_ID.ToString(), Value = userMainPara.UserID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                UserMain userMain = new UserMain()
                {
                    PWDValidDate = new DBDateTime(dataTable.Rows[0][UserMain.DataField.PWD_VALID_DATE.ToString()])
                };

                return userMain;
            }
            return null;
        }

        public enum EnumSelectUserRestrictTypeResult
        {
            [Description("Normal")]
            N,
            [Description("Restricted")]
            R,
            [Description("OnlyInternal")]
            I,
            [Description("Unrestricted")]
            U
        }

        public EnumSelectUserRestrictTypeResult SelectUserRestrictType(UserMainPara userMainPara)
        {
            string commandText = string.Concat(new object[] 
            {
                "SELECT RESTRICT_TYPE ", Environment.NewLine,
                "FROM SYS_USER_MAIN ", Environment.NewLine,
                "WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "  AND IS_DISABLE='N'; ", Environment.NewLine
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserMainPara.ParaField.USER_ID.ToString(), Value = userMainPara.UserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            if (result.IsNull() == false)
            {
                if (Enum.IsDefined(typeof(EnumSelectUserRestrictTypeResult), result.GetValue()))
                {
                    return (EnumSelectUserRestrictTypeResult)Enum.Parse(typeof(EnumSelectUserRestrictTypeResult), result.GetValue());
                }
            }
            return EnumSelectUserRestrictTypeResult.R;
        }

        public class TrustIPPara
        {
            public enum ParaField
            {
                IP_ADDRESS
            }

            public DBVarChar IpAddress;
        }

        public enum EnumValidateIPAddressIsTrustResult
        {
            Trust, UnTrust, Reject
        }

        public EnumValidateIPAddressIsTrustResult ValidateIPAddressIsTrust(TrustIPPara trustIPPara)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @TRUST_IP_CNT INT; ", Environment.NewLine,
                "DECLARE @RESULT CHAR; ", Environment.NewLine,
                
                "SET @RESULT='U'; ", Environment.NewLine,
                
                "SELECT @TRUST_IP_CNT=COUNT(1) ", Environment.NewLine,
                "FROM SYS_TRUST_IP ", Environment.NewLine,
                "WHERE dbo.FN_GET_TRSUT_BYIP(IP_BEGIN, IP_END, {IP_ADDRESS})='Y'; ", Environment.NewLine,
                
                "IF @TRUST_IP_CNT > 0 ", Environment.NewLine,
                "BEGIN ", Environment.NewLine,
                "    SELECT @RESULT=(CASE WHEN TRUST_STATUS IS NOT NULL THEN TRUST_STATUS ELSE 'N' END) ", Environment.NewLine,
                "    FROM SYS_TRUST_IP ", Environment.NewLine,
                "    WHERE dbo.FN_GET_TRSUT_BYIP(IP_BEGIN, IP_END, {IP_ADDRESS})='Y'; ", Environment.NewLine,
                "END; ", Environment.NewLine,
                
                "SELECT @RESULT; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = TrustIPPara.ParaField.IP_ADDRESS.ToString(), Value = trustIPPara.IpAddress });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            if (result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumValidateIPAddressIsTrustResult.Trust;
            }
            else if (result.GetValue() == EnumYN.N.ToString())
            {
                return EnumValidateIPAddressIsTrustResult.Reject;
            }
            else
            {
                return EnumValidateIPAddressIsTrustResult.UnTrust;
            }
        }

        public enum EnumValidateIPAddressIsInternalResult
        {
            Success, Failure
        }

        public EnumValidateIPAddressIsInternalResult ValidateIPAddressIsInternal(TrustIPPara trustIPPara)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @IP_CNT INT; ", Environment.NewLine,

                "SELECT @IP_CNT=COUNT(1) ", Environment.NewLine,
                "FROM SYS_TRUST_IP ", Environment.NewLine,
                "WHERE TRUST_STATUS='Y' AND TRUST_TYPE='I' ", Environment.NewLine,
                "  AND dbo.FN_GET_TRSUT_BYIP(IP_BEGIN, IP_END, {IP_ADDRESS})='Y'; ", Environment.NewLine,

                "SELECT @IP_CNT; ", Environment.NewLine
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = TrustIPPara.ParaField.IP_ADDRESS.ToString(), Value = trustIPPara.IpAddress });

            DBInt result = new DBInt(base.ExecuteScalar(commandText, dbParameters));
            if (result.GetValue() > 0)
            {
                return EnumValidateIPAddressIsInternalResult.Success;
            }
            return EnumValidateIPAddressIsInternalResult.Failure;
        }

        public class UserSystemRoleFunPara
        {
            public enum ParaField
            {
                USER_ID, SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME, SESSION_ID, IP_ADDRESS, IS_OUTSIDE
            }

            public DBVarChar UserID;
            public DBVarChar SystemID;
            public DBVarChar ControllerID;
            public DBVarChar ActionName;
            public DBChar SessionID;
            public DBVarChar IPAddress;
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
                "DECLARE @IN_BUILDING_CN INT = 0; ", Environment.NewLine,
                "DECLARE @SESSION_ID CHAR(24); ", Environment.NewLine,
                "DECLARE @FUN_CN INT = 0; ", Environment.NewLine,

                "SELECT @IN_BUILDING_CN=COUNT(1) ", Environment.NewLine,
                "FROM SYS_TRUST_IP ", Environment.NewLine,
                "WHERE dbo.FN_GET_TRSUT_BYIP(IP_BEGIN, IP_END, {IP_ADDRESS})='Y' ", Environment.NewLine,
                "  AND SOURCE_TYPE='C'; ", Environment.NewLine,

                "SELECT TOP 1 @SESSION_ID=C.SESSION_ID ", Environment.NewLine,
                "FROM SYS_USER_CONNECT C ", Environment.NewLine,
                "JOIN SYS_USER_MAIN U ON C.USER_ID=U.USER_ID ", Environment.NewLine,
                "JOIN RAW_CM_USER R ON U.USER_ID=R.USER_ID ", Environment.NewLine,
                "WHERE C.USER_ID={USER_ID} AND C.SESSION_ID={SESSION_ID} AND C.CUST_LOGOUT='N' ", Environment.NewLine,
                "  AND C.IP_ADDRESS=(CASE WHEN @IN_BUILDING_CN>0 THEN C.IP_ADDRESS ELSE {IP_ADDRESS} END) ", Environment.NewLine,
                "  AND U.IS_DISABLE='N' AND R.IS_LEFT='N' ", Environment.NewLine,
                "ORDER BY C.LAST_CONNECT_DT DESC; ", Environment.NewLine,

                "IF @SESSION_ID IS NOT NULL ", Environment.NewLine,
                "BEGIN ", Environment.NewLine,
                "    UPDATE SYS_USER_CONNECT SET ", Environment.NewLine,
                "        LAST_CONNECT_DT=GETDATE(), IP_ADDRESS={IP_ADDRESS} ", Environment.NewLine,
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
            dbParameters.Add(new DBParameter { Name = UserSystemRoleFunPara.ParaField.IP_ADDRESS.ToString(), Value = userSystemRoleFunPara.IPAddress });
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

        public class RAWUserPara
        {
            public enum ParaField
            {
                CONDITION, CONDITION_LENGTH
            }
            public DBObject Condition;
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
                return Security.Encrypt(this.UserID.StringValue(), key);
            }

            public string PictureUrl()
            {
                return string.Empty;
            }

            public string GroupBy()
            {
                return this.UserID.GetValue();
            }
        }

        public List<RAWUser> SelectRAWUserList(RAWUserPara para)
        {
            string commandText = string.Concat(new object[] 
            { 
                "SELECT USER_ID, USER_NM ", Environment.NewLine,
                "FROM RAW_CM_USER ", Environment.NewLine,
                "WHERE USER_ID LIKE '%{CONDITION}%' ", Environment.NewLine,
                "   OR USER_NM LIKE N'%{CONDITION}%' ", Environment.NewLine,
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            if (!string.IsNullOrWhiteSpace(para.Condition.GetValue()))
            {
                dbParameters.Add(new DBParameter { Name = RAWUserPara.ParaField.CONDITION.ToString(), Value = para.Condition });
            }

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

        #region - FunTool -
        public class UserSysFunToolPara
        {
            public enum ParaField
            {
                USER_ID,
                SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME,
                TOOL_NO, TOOL_NM,
                PARA_ID, PARA_VALUE,
                COPY_TOOL_NO, COPY_USER_ID,
                UPD_USER_ID
            }
            
            public DBVarChar UserID;
            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
            public DBChar ToolNo;
            public DBNVarChar ToolNM;

            public DBVarChar ParaID;
            public DBNVarChar ParaValue;

            public DBChar CopyToolNo;
            public DBVarChar CopyUserID;

            public DBVarChar UpdUserID;
        }

        public class UserSysFunTool : DBTableRow
        {
            public enum DataField
            {
                TOOL_NO, TOOL_NM,
                IS_CURRENTLY,
                PARA_ID, PARA_VALUE
            }

            public DBChar ToolNo;
            public DBNVarChar ToolNM;

            public DBChar IsCurrently;

            public DBVarChar ParaID;
            public DBNVarChar ParaValue;
        }

        public List<UserSysFunTool> SelectUserSysFunToolList(UserSysFunToolPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT T.TOOL_NO, T.TOOL_NM, T.IS_CURRENTLY ", Environment.NewLine,
                "     , P.PARA_ID, P.PARA_VALUE ", Environment.NewLine,
                "FROM SYS_USER_TOOL T ", Environment.NewLine,
                "JOIN SYS_USER_TOOL_PARA P ON T.USER_ID=P.USER_ID ", Environment.NewLine,
                "                         AND T.SYS_ID=P.SYS_ID AND T.FUN_CONTROLLER_ID=P.FUN_CONTROLLER_ID AND T.FUN_ACTION_NAME=P.FUN_ACTION_NAME ", Environment.NewLine,
                "                         AND T.TOOL_NO=P.TOOL_NO ", Environment.NewLine,
                "WHERE T.USER_ID={USER_ID} ", Environment.NewLine,
                "  AND T.SYS_ID={SYS_ID} AND T.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND T.FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                "ORDER BY T.SORT_ORDER  ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionName });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserSysFunTool> userSysFunToolList = new List<UserSysFunTool>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserSysFunTool userSysFunTool = new UserSysFunTool()
                    {
                        ToolNo = new DBChar(dataRow[UserSysFunTool.DataField.TOOL_NO.ToString()]),
                        ToolNM = new DBNVarChar(dataRow[UserSysFunTool.DataField.TOOL_NM.ToString()]),
                        IsCurrently = new DBChar(dataRow[UserSysFunTool.DataField.IS_CURRENTLY.ToString()]),
                        ParaID = new DBVarChar(dataRow[UserSysFunTool.DataField.PARA_ID.ToString()]),
                        ParaValue = new DBNVarChar(dataRow[UserSysFunTool.DataField.PARA_VALUE.ToString()]),
                    };
                    userSysFunToolList.Add(userSysFunTool);
                }
                return userSysFunToolList;
            }
            return null;
        }

        public enum EnumCreateUserSysFunToolResult
        {
            Success, Failure
        }

        public EnumCreateUserSysFunToolResult CreateUserSysFunTool(UserSysFunToolPara para, List<UserSysFunToolPara> paraList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();

            string mainCommandText = string.Concat(new object[] 
            { 
                "        DECLARE @TOOL_NO CHAR(6); ", Environment.NewLine,
                "        DECLARE @SORT_ORDER VARCHAR(6); ", Environment.NewLine,

                "        SELECT @TOOL_NO=RIGHT('00000'+CAST(ISNULL(CAST(MAX(TOOL_NO) AS INT),0)+1 AS VARCHAR),6) ", Environment.NewLine,
                "             , @SORT_ORDER=RIGHT('00000'+CAST(ISNULL(CAST(MAX(SORT_ORDER) AS INT),0)+1 AS VARCHAR),6) ", Environment.NewLine,
                "        FROM SYS_USER_TOOL ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID} AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,

                "        UPDATE SYS_USER_TOOL ", Environment.NewLine,
                "        SET IS_CURRENTLY='N' ", Environment.NewLine,
                "          , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "          , UPD_DT=GETDATE() ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "          AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,

                "        INSERT INTO SYS_USER_TOOL VALUES ( ", Environment.NewLine,
                "            {USER_ID}, {SYS_ID}, {FUN_CONTROLLER_ID}, {FUN_ACTION_NAME}, @TOOL_NO, {TOOL_NM}, 'Y', @SORT_ORDER, {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "        ); ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.TOOL_NM, Value = para.ToolNM });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, mainCommandText, dbParameters));
            dbParameters.Clear();

            foreach (UserSysFunToolPara userSysFunToolPara in paraList)
            {
                string insertCommand = string.Concat(new object[]
                {
                    "        INSERT INTO SYS_USER_TOOL_PARA VALUES ({USER_ID}, {SYS_ID}, {FUN_CONTROLLER_ID}, {FUN_ACTION_NAME}, @TOOL_NO, {PARA_ID}, {PARA_VALUE}, {UPD_USER_ID}, GETDATE()); ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.USER_ID, Value = para.UserID });
                dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.SYS_ID, Value = para.SysID });
                dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
                dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
                dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.PARA_ID, Value = userSysFunToolPara.ParaID });
                dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.PARA_VALUE, Value = userSysFunToolPara.ParaValue });
                dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, insertCommand, dbParameters));
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
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumCreateUserSysFunToolResult.Success : EnumCreateUserSysFunToolResult.Failure;
        }

        public enum EnumRebuildUserSysFunToolResult
        {
            Success, Failure
        }

        public EnumRebuildUserSysFunToolResult RebuildUserSysFunTool(UserSysFunToolPara para, List<UserSysFunToolPara> paraList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();

            string mainCommandText = string.Concat(new object[] 
            {
                "        UPDATE SYS_USER_TOOL ", Environment.NewLine,
                "        SET IS_CURRENTLY='N' ", Environment.NewLine,
                "          , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "          , UPD_DT=GETDATE() ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "          AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,

                "        IF EXISTS(SELECT USER_ID FROM SYS_USER_TOOL WHERE USER_ID={USER_ID} AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} AND TOOL_NO={TOOL_NO}) ", Environment.NewLine,
                "        BEGIN ", Environment.NewLine,
                "            UPDATE SYS_USER_TOOL ", Environment.NewLine,
                "            SET IS_CURRENTLY='Y' ", Environment.NewLine,
                "              , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "              , UPD_DT=GETDATE() ", Environment.NewLine,
                "            WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "              AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                "              AND TOOL_NO={TOOL_NO}; ", Environment.NewLine,
                "        END; ", Environment.NewLine,
                "        ELSE ", Environment.NewLine,
                "        BEGIN ", Environment.NewLine,
                "            SELECT @SORT_ORDER=RIGHT('00000'+CAST(ISNULL(CAST(MAX(SORT_ORDER) AS INT),0)+1 AS VARCHAR),6) ", Environment.NewLine,
                "            FROM SYS_USER_TOOL ", Environment.NewLine,
                "            WHERE USER_ID={USER_ID} AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,

                "            INSERT INTO SYS_USER_TOOL VALUES ( ", Environment.NewLine,
                "                {USER_ID}, {SYS_ID}, {FUN_CONTROLLER_ID}, {FUN_ACTION_NAME}, {TOOL_NO}, {TOOL_NM}, 'Y', @SORT_ORDER, {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "            ); ", Environment.NewLine,
                "        END; ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.TOOL_NO, Value = para.ToolNo });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.TOOL_NM, Value = para.ToolNM });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, mainCommandText, dbParameters));
            dbParameters.Clear();

            string deleteCommand = string.Concat(new object[]
            {
                "        DELETE FROM SYS_USER_TOOL_PARA ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "          AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                "          AND TOOL_NO={TOOL_NO}; ", Environment.NewLine
            });

            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.TOOL_NO, Value = para.ToolNo });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, deleteCommand, dbParameters));
            dbParameters.Clear();

            foreach (UserSysFunToolPara userSysFunToolPara in paraList)
            {
                string insertCommand = string.Concat(new object[]
                {
                    "        INSERT INTO SYS_USER_TOOL_PARA VALUES ({USER_ID}, {SYS_ID}, {FUN_CONTROLLER_ID}, {FUN_ACTION_NAME}, {TOOL_NO}, {PARA_ID}, {PARA_VALUE}, {UPD_USER_ID}, GETDATE()); ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.USER_ID, Value = para.UserID });
                dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.SYS_ID, Value = para.SysID });
                dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
                dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
                dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.TOOL_NO, Value = para.ToolNo });
                dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.PARA_ID, Value = userSysFunToolPara.ParaID });
                dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.PARA_VALUE, Value = userSysFunToolPara.ParaValue });
                dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, insertCommand, dbParameters));
                dbParameters.Clear();
            }

            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "DECLARE @SORT_ORDER VARCHAR(6); ", Environment.NewLine,
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
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumRebuildUserSysFunToolResult.Success : EnumRebuildUserSysFunToolResult.Failure;
        }

        public enum EnumUpdateUserSysFunToolResult
        {
            Success, Failure
        }

        public EnumUpdateUserSysFunToolResult UpdateUserSysFunTool(UserSysFunToolPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        UPDATE SYS_USER_TOOL ", Environment.NewLine,
                "        SET IS_CURRENTLY='N' ", Environment.NewLine,
                "          , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "          , UPD_DT=GETDATE() ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "          AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,

                "        UPDATE SYS_USER_TOOL ", Environment.NewLine,
                "        SET IS_CURRENTLY='Y' ", Environment.NewLine,
                "          , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "          , UPD_DT=GETDATE() ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "          AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                "          AND TOOL_NO={TOOL_NO}; ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.TOOL_NO, Value = para.ToolNo });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumUpdateUserSysFunToolResult.Success : EnumUpdateUserSysFunToolResult.Failure;
        }

        public enum EnumCopyUserSysFunToolResult
        {
            Success, Failure
        }

        public EnumCopyUserSysFunToolResult CopyUserSysFunTool(UserSysFunToolPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DECLARE @USER_NM NVARCHAR(150); ", Environment.NewLine,
                "        DECLARE @TOOL_NO CHAR(6); ", Environment.NewLine,
                "        DECLARE @SORT_ORDER VARCHAR(6); ", Environment.NewLine,

                "        SELECT @USER_NM=USER_NM FROM RAW_CM_USER WHERE USER_ID={USER_ID}; ", Environment.NewLine,

                "        SELECT @TOOL_NO=RIGHT('00000'+CAST(ISNULL(CAST(MAX(TOOL_NO) AS INT),0)+1 AS VARCHAR),6) ", Environment.NewLine,
                "             , @SORT_ORDER=RIGHT('00000'+CAST(ISNULL(CAST(MAX(SORT_ORDER) AS INT),0)+1 AS VARCHAR),6) ", Environment.NewLine,
                "        FROM SYS_USER_TOOL ", Environment.NewLine,
                "        WHERE USER_ID={COPY_USER_ID} ", Environment.NewLine,
                "          AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,

                "        IF (LEN(ISNULL({TOOL_NO},''))=6 AND {TOOL_NO}=@TOOL_NO) ", Environment.NewLine,
                "        BEGIN ", Environment.NewLine,
                "            SET @TOOL_NO=RIGHT('00000'+CAST(ISNULL(CAST(@TOOL_NO AS INT),0)+1 AS VARCHAR),6); ", Environment.NewLine,
                "        END; ", Environment.NewLine,

                "        UPDATE SYS_USER_TOOL ", Environment.NewLine,
                "        SET IS_CURRENTLY='N' ", Environment.NewLine,
                "          , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "          , UPD_DT=GETDATE() ", Environment.NewLine,
                "        WHERE USER_ID={COPY_USER_ID} ", Environment.NewLine,
                "          AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,

                "        INSERT INTO SYS_USER_TOOL ", Environment.NewLine,
                "        SELECT {COPY_USER_ID}, SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME, @TOOL_NO, (TOOL_NM + '-' + {USER_ID} + @USER_NM) AS TOOL_NM ", Environment.NewLine,
                "             , 'Y', @SORT_ORDER, {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "        FROM SYS_USER_TOOL ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "          AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} AND TOOL_NO={COPY_TOOL_NO}; ", Environment.NewLine,
                
                "        INSERT INTO SYS_USER_TOOL_PARA ", Environment.NewLine,
                "        SELECT {COPY_USER_ID}, SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME, @TOOL_NO, PARA_ID, PARA_VALUE, {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "        FROM SYS_USER_TOOL_PARA ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "          AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} AND TOOL_NO={COPY_TOOL_NO}; ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.TOOL_NO, Value = para.ToolNo });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.COPY_TOOL_NO, Value = para.CopyToolNo });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.COPY_USER_ID, Value = para.CopyUserID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumCopyUserSysFunToolResult.Success : EnumCopyUserSysFunToolResult.Failure;
        }

        public enum EnumDeleteUserSysFunToolResult
        {
            Success, Failure
        }

        public EnumDeleteUserSysFunToolResult DeleteUserSysFunTool(UserSysFunToolPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE FROM SYS_USER_TOOL ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "          AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                "          AND TOOL_NO={TOOL_NO}; ", Environment.NewLine,

                "        DELETE FROM SYS_USER_TOOL_PARA ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "          AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                "          AND TOOL_NO={TOOL_NO}; ", Environment.NewLine,

                "        DECLARE @USER_ID VARCHAR(20); ", Environment.NewLine,
                "        DECLARE @TOOL_NO CHAR(6); ", Environment.NewLine,

                "        SELECT @USER_ID=USER_ID ", Environment.NewLine,
                "        FROM SYS_USER_TOOL ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "          AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} AND IS_CURRENTLY='Y'; ", Environment.NewLine,

                "        IF (LEN(ISNULL(@USER_ID,''))=0) ", Environment.NewLine,
                "        BEGIN ", Environment.NewLine,
                "            SELECT @TOOL_NO=MAX(TOOL_NO) ", Environment.NewLine,
                "            FROM SYS_USER_TOOL ", Environment.NewLine,
                "            WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "              AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,

                "            UPDATE SYS_USER_TOOL ", Environment.NewLine,
                "            SET IS_CURRENTLY='N' ", Environment.NewLine,
                "              , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "              , UPD_DT=GETDATE() ", Environment.NewLine,
                "            WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "              AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,

                "            UPDATE SYS_USER_TOOL ", Environment.NewLine,
                "            SET IS_CURRENTLY='Y' ", Environment.NewLine,
                "              , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "              , UPD_DT=GETDATE() ", Environment.NewLine,
                "            WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "              AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                "              AND TOOL_NO=@TOOL_NO; ", Environment.NewLine,
                "        END; ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.TOOL_NO, Value = para.ToolNo });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumDeleteUserSysFunToolResult.Success : EnumDeleteUserSysFunToolResult.Failure;
        }

        public enum EnumUpdateNameUserSysFunToolResult
        {
            Success, Failure
        }

        public EnumUpdateNameUserSysFunToolResult UpdateUserSysFunToolName(UserSysFunToolPara para, ref string toolNM)
        {
            string commandText = string.Concat(new object[] 
            { 
                " DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                " SET @RESULT = 'N'; ", Environment.NewLine,
                " BEGIN TRANSACTION ", Environment.NewLine,
                "     BEGIN TRY ", Environment.NewLine,
                  
                " DECLARE @TOOL_NM NVARCHAR(150); ", Environment.NewLine,
                  
                " UPDATE SYS_USER_TOOL ", Environment.NewLine,
                " SET TOOL_NM={TOOL_NM} ", Environment.NewLine,
                "   , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "   , UPD_DT=GETDATE() ", Environment.NewLine,
                " WHERE USER_ID={USER_ID} AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} AND TOOL_NO={TOOL_NO} ", Environment.NewLine,
                
                " SELECT @TOOL_NM=dbo.FN_GET_NMID(T.TOOL_NO, T.TOOL_NM) ", Environment.NewLine,
                " FROM SYS_USER_TOOL T ", Environment.NewLine,
                " WHERE T.USER_ID={USER_ID} AND T.SYS_ID={SYS_ID} AND T.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} ", Environment.NewLine,
                "   AND T.FUN_ACTION_NAME={FUN_ACTION_NAME} AND T.TOOL_NO={TOOL_NO} ", Environment.NewLine,

                "         SET @RESULT = 'Y'; ", Environment.NewLine,
                  
                "         COMMIT; ", Environment.NewLine,
                "     END TRY ", Environment.NewLine,
                "     BEGIN CATCH ", Environment.NewLine,
                "         SET @RESULT = 'N'; ", Environment.NewLine,
                "         ROLLBACK TRANSACTION; ", Environment.NewLine,
                "     END CATCH ", Environment.NewLine,
                " ; ", Environment.NewLine,
                " SELECT (CASE WHEN @RESULT='Y' THEN @TOOL_NM ELSE NULL END) AS TOOL_NM ; ", Environment.NewLine
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.TOOL_NO, Value = para.ToolNo });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.TOOL_NM, Value = para.ToolNM });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                UserSysFunTool userSysFunTool = new UserSysFunTool()
                {
                    ToolNM = new DBNVarChar(dataRow[UserSysFunTool.DataField.TOOL_NM.ToString()])
                };
                if (!userSysFunTool.ToolNM.IsNull())
                {
                    toolNM = userSysFunTool.ToolNM.GetValue();
                    return EnumUpdateNameUserSysFunToolResult.Success;
                }
            }
            return EnumUpdateNameUserSysFunToolResult.Failure;
        }

        public UserSysFunTool SelectUserSysFunToolNameList(UserSysFunToolPara para)
        {
            string commandText = string.Concat(new object[] 
            { 
               " SELECT T.TOOL_NM ", Environment.NewLine,
               " FROM SYS_USER_TOOL T ", Environment.NewLine,
               " WHERE T.USER_ID={USER_ID} AND T.SYS_ID={SYS_ID} AND T.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} ", Environment.NewLine,
               "   AND T.FUN_ACTION_NAME={FUN_ACTION_NAME} AND T.TOOL_NO={TOOL_NO} ", Environment.NewLine,
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = UserSysFunToolPara.ParaField.TOOL_NO.ToString(), Value = para.ToolNo });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                UserSysFunTool userSysFunTool = new UserSysFunTool()
                {
                    ToolNM = new DBNVarChar(dataRow[UserSysFunTool.DataField.TOOL_NM.ToString()])
                };
                return userSysFunTool;
            }
            return null;
        }
        #endregion

        #region - WorkFlow -

        #region - Definitions -

        public class RunTimeWFFlowPara : DBCulture
        {
            public RunTimeWFFlowPara(string culture)
                : base(culture)
            {
            }

            public enum ParaField
            {
                WF_NO, USER_ID, SIG_USER_ID_STR, NEW_USER_ID_STR,
                WF_FLOW, WF_NODE, WF_SIG,
                CULTURE_ID
            }

            public DBChar WFNo;
            public DBVarChar UserID;
            public DBObject SigUserIDStr;
            public DBObject NewUserIDStr;
        }

        public class RunTimeWFFlow : DBTableRow
        {
            public DBChar WFNo;
            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;

            public DBVarChar WFFlowType;
            public DBVarChar WFFlowManUserID;
            public DBChar WFEnableDate;
            public DBChar WFDisableDate;

            public DBNVarChar WFSubject;
            public DBVarChar NewUserID;
            public DBVarChar EndUserID;

            public DBChar DTBegin;
            public DBChar DTEnd;
            public DBVarChar ResultID;
            public DBChar NodeNo;

            public DBVarChar UpdUserID;
            public DBDateTime UpdDT;

            public DBChar FormatWFNo;
            public DBNVarChar WFFlowNM;
            public DBNVarChar WFFlowTypeNM;
            public DBNVarChar WFFlowManUserNM;
            public DBNVarChar NewUserNM;
            public DBNVarChar EndUserNM;
            public DBNVarChar ResultNM;
            public DBNVarChar UpdUserNM;

            public RunTimeWFFlowRoles RoleList;

            public RunTimeWFNodes WFNodeList;

            public RunTimeWFNode WFNodeCurrent
            {
                get { return WFNodeList.WFNodeCurrent; }
            }

            public EnumWFStsType GetWFStsCurrent(string nodeNo)
            {
                var wfNodeCurrent = WFNodeList.SingleOrDefault(n => n.NodeNo.GetValue() == nodeNo);
                var wfSts = EnumWFStsType.NONE;

                if (wfNodeCurrent != null)
                {
                    var isApplying = GetIsApplying(wfNodeCurrent);
                    var isSigStarting = GetIsSigStarting(wfNodeCurrent);
                    var isSignature = GetIsSignature(wfNodeCurrent);
                    var isSigAllAccept = GetIsSigAllAccept(wfNodeCurrent);
                    var isSigReject = GetIsSigReject(wfNodeCurrent);
                    var isCancelUrSelf = GetIsCancelUrSelf(wfNodeCurrent);

                    wfSts = isCancelUrSelf ? EnumWFStsType.IS_CANCEL_URSELF
                        : isApplying ? EnumWFStsType.IS_APPLYING
                        : isSigAllAccept ? EnumWFStsType.IS_SIG_ALL_ACCEPT
                        : isSigReject ? EnumWFStsType.IS_SIG_REJECT
                        : isSignature ? EnumWFStsType.IS_SIGNATUE
                        : isSigStarting ? EnumWFStsType.IS_SIG_STARTING : EnumWFStsType.NONE;
                }

                return wfSts;
            }

            public bool GetIsApplying(RunTimeWFNode wfNodeCurrent)
            {
                return wfNodeCurrent.WFSigIsStarting.GetValue() == EnumYN.N.ToString();
            }

            public bool GetIsSigStarting(RunTimeWFNode wfNodeCurrent)
            {
                return wfNodeCurrent.WFSigIsStarting == EnumYN.Y.ToString() && WFNodeCurrent.WFSigList.Any();
            }

            public bool GetIsSignature(RunTimeWFNode wfNodeCurrent)
            {
                return wfNodeCurrent.WFSigList != null &&
                       wfNodeCurrent.WFSigList.Count > 0 &&
                       wfNodeCurrent.WFSigList.Any(w => w.SigResultID.GetValue() != EnumWFNodeSignatureResultID.R.ToString()) &&
                       wfNodeCurrent.WFSigList.Any(w => string.IsNullOrWhiteSpace(w.SigDate.GetValue()) == false) &&
                       wfNodeCurrent.WFSigList.Count != WFNodeCurrent.WFSigList.Count(w => w.SigResultID.GetValue() == EnumWFNodeSignatureResultID.A.ToString());
            }

            public bool GetIsSigAllAccept(RunTimeWFNode wfNodeCurrent)
            {
                return wfNodeCurrent.WFSigList != null &&
                       wfNodeCurrent.WFSigList.Count > 0 &&
                       wfNodeCurrent.WFSigList.Count == wfNodeCurrent.WFSigList.Count(w => w.SigResultID.GetValue() == EnumWFNodeSignatureResultID.A.ToString());
            }
            public bool GetIsSigReject(RunTimeWFNode wfNodeCurrent)
            {
                return wfNodeCurrent.WFSigList != null &&
                       wfNodeCurrent.WFSigList.Any(r => r.SigResultID.GetValue() == EnumWFNodeSignatureResultID.R.ToString());
            }

            public bool GetIsCancelUrSelf(RunTimeWFNode wfNodeCurrent)
            {
                return wfNodeCurrent.ResultID.GetValue() == EnumWFNodeResultID.C.ToString();
            }

            public EnumSystemID GetSystemID()
            {
                return Utility.GetSystemID(SysID.GetValue());
            }

            public EnumWFResultID GetWFResultID()
            {
                return (EnumWFResultID)Enum.Parse(typeof(EnumWFResultID), ResultID.GetValue());
            }
        }

        public class RunTimeWFFlowRole : DBTableRow
        {
            public DBVarChar RoleID;
        }

        public class RunTimeWFFlowRoles : List<RunTimeWFFlowRole>
        {
        }

        public class RunTimeWFNode : DBTableRow
        {
            public DBChar WFNo;
            public DBChar NodeNo;
            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;

            public DBVarChar WFNodeType;
            public DBChar WFNodeIsFirst;
            public DBChar WFNodeIsFinally;
            public DBVarChar WFBackNodeID;
            public DBVarChar WFFunSysID;
            public DBVarChar WFFunControllerID;
            public DBVarChar WFFunActionName;
            public DBVarChar WFSigAPISysID;
            public DBVarChar WFSigAPIControllerID;
            public DBVarChar WFSigAPIActionName;
            public DBVarChar WFAssgAPISysID;
            public DBVarChar WFAssgAPIControllerID;
            public DBVarChar WFAssgAPIActionName;

            public DBChar IsAssgNextNode;

            public DBVarChar NewUserID;
            public DBVarChar NewUserIDStr;
            public DBVarChar EndUserID;

            public DBChar DTBegin;
            public DBChar DTEnd;
            public DBVarChar ResultID;
            public DBVarChar ResultValue;

            public DBChar WFSigIsStarting;
            public DBInt WFSigStep;
            public DBVarChar WFSigResultID;
            public DBVarChar WFSigUserIDStr;

            public DBVarChar UpdUserID;
            public DBDateTime UpdDT;

            public DBNVarChar WFNodeNM;
            public DBNVarChar WFNodeZHTW;
            public DBNVarChar NewUserNM;
            public DBNVarChar NewUserNMStr;
            public DBNVarChar EndUserNM;
            public DBNVarChar ResultNM;
            public DBNVarChar WFSigCurrentResultNM;
            public DBNVarChar UpdUserNM;

            public RunTimeWFNodeRoles RoleList;
            public RunTimeWFNodeRoles NextNodeRoleList;
            public RunTimeWFNodeNewUsers NewUserList;

            public RunTimeWFSigs WFSigList;

            public RunTimeWFDocs WFDocList;

            public EnumSystemID GetSystemID()
            {
                return Utility.GetSystemID(this.SysID.GetValue());
            }

            public EnumSystemID GetWFFunSystemID()
            {
                return Utility.GetSystemID(this.WFFunSysID.GetValue());
            }

            public EnumWFNodeResultID GetWFNodeResultID()
            {
                return (EnumWFNodeResultID)Enum.Parse(typeof(EnumWFNodeResultID), this.ResultID.GetValue());
            }

            public RunTimeWFSigs WFSigCurrentList
            {
                get
                {
                    RunTimeWFSigs runTimeWfSigs = new RunTimeWFSigs();

                    if (WFSigList != null &&
                        WFSigList.Any())
                    {
                        var filterWFSigList = WFSigList.Where(w => w.SigDate.IsNull() || w.SigResultID.GetValue() == EnumWFNodeSignatureResultID.P.ToString()).ToArray();
                        var currentSigStep =
                            filterWFSigList.Any()
                                ? filterWFSigList.Min(m => m.SigStep.GetValue())
                                : WFSigList.Max(m => m.SigStep.GetValue());
                        runTimeWfSigs.AddRange(WFSigList.FindAll(s => s.SigStep.GetValue() == currentSigStep));
                    }

                    return runTimeWfSigs;
                }
            }
        }

        public class RunTimeWFNodes : List<RunTimeWFNode>
        {
            public RunTimeWFNode WFNodeCurrent { get; private set; }

            public RunTimeWFNodes(IEnumerable<RunTimeWFNode> runTimeWFNodes)
            {
                AddRange(runTimeWFNodes);
                WFNodeCurrent = this[Count - 1];
            }
        }

        public class RunTimeWFNodeRole : DBTableRow
        {
            public DBChar NodeNo;
            public DBVarChar RoleID;
        }

        public class RunTimeWFNodeRoles : List<RunTimeWFNodeRole>
        {
        }

        public class RunTimeWFNodeNewUser : DBTableRow
        {
            public DBChar NodeNo;
            public DBVarChar NewUserID;
            public DBVarChar NewUserNM;
        }

        public class RunTimeWFNodeNewUsers : List<RunTimeWFNodeNewUser>
        {
        }

        public class RunTimeWFSig : DBTableRow
        {
            public DBChar NodeNo;
            public DBVarChar SysID;

            public DBVarChar SigKind;
            public DBInt SigStep;
            public DBChar WFSigSeq;
            public DBNVarChar WFSigZHTW;
            public DBNVarChar WFSigZHCN;
            public DBNVarChar WFSigENUS;
            public DBNVarChar WFSigTHTH;
            public DBNVarChar WFSigJAJP;
            public DBNVarChar WFSigKOKR;
            public DBNVarChar WFSigNM;
            
            public DBVarChar WFSigType;
            public DBVarChar WFAPISysID;
            public DBVarChar WFAPIControllerID;
            public DBVarChar WFAPIActionName;
            public DBVarChar WFCompareNodeID;
            public DBChar WFCompareSigSeq;
            public DBVarChar WFChkAPISysID;
            public DBVarChar WFChkAPIControllerID;
            public DBVarChar WFChkAPIActionName;


            public DBVarChar SigUserID;
            public DBNVarChar SigUserNM;
            public DBChar SigDate;
            public DBVarChar SigResultID;

            public DBVarChar UpdUserID;
            public DBDateTime UpdDT;

            public RunTimeWFSigRoles RoleList;

            public EnumSystemID GetSystemID()
            {
                return Utility.GetSystemID(SysID.GetValue());
            }

            public EnumSystemID GetWFAPISystemID()
            {
                return Utility.GetSystemID(WFAPISysID.GetValue());
            }

            public EnumSystemID GetWFChkAPISystemID()
            {
                return Utility.GetSystemID(WFChkAPISysID.GetValue());
            }

            public EnumWFNodeSignatureResultID GetWFNodeSignatureResultID()
            {
                return (EnumWFNodeSignatureResultID)Enum.Parse(typeof(EnumWFNodeSignatureResultID), SigResultID.GetValue());
            }
        }

        public class RunTimeWFSigs : List<RunTimeWFSig>
        {
        }

        public class RunTimeWFSigRole : DBTableRow
        {
            public DBChar NodeNo;

            public DBInt SigStep;
            public DBChar WFSigSeq;

            public DBVarChar RoleID;
        }

        public class RunTimeWFSigRoles : List<RunTimeWFSigRole>
        {
        }

        public class RunTimeWFDoc : DBTableRow
        {
            public DBChar NodeNo;
            public DBChar DocNo;
            public DBVarChar SysID;

            public DBChar WFDocSeq;

            public DBNVarChar WFDocZHTW;
            public DBNVarChar WFDocZHCN;
            public DBNVarChar WFDocENUS;
            public DBNVarChar WFDocTHTH;
            public DBNVarChar WFDocJAJP;
            public DBNVarChar WFDocKOKR;

            public DBChar IsReq;
            public DBVarChar DocUserID;
            public DBNVarChar DocFileName;
            public DBVarChar DocEncodeName;
            public DBChar DocDate;

            public DBNVarChar DocPath;
            public DBNVarChar DocLocalPath;
            public DBChar IsDelete;

            public DBVarChar UpdUserID;
            public DBDateTime UpdDT;

            public EnumSystemID GetSystemID()
            {
                return Utility.GetSystemID(SysID.GetValue());
            }
        }

        public class RunTimeWFDocs : List<RunTimeWFDoc>
        {
        }
        #endregion
        
        public enum EnumCheckRunTimeWFFlowResult
        {
            Success, Failure
        }

        public EnumCheckRunTimeWFFlowResult CheckRunTimeWFFlow(RunTimeWFFlowPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT CHAR(1) = 'N';",
                        "IF EXISTS (",
                        "       SELECT F.WF_NO ",
                        "         FROM WF_FLOW F ",
                        "         LEFT JOIN WF_NODE N ",
                        "           ON F.WF_NO = N.WF_NO ",
                        "        WHERE F.WF_NO = {WF_NO} ",
                        "          AND (F.NEW_USER_ID = {USER_ID} ",
                        "           OR  N.NEW_USER_ID = {USER_ID} ",
                        "           OR  (N.IS_START_SIG = 'Y' AND N.SIG_USER_ID_STR LIKE '%{SIG_USER_ID_STR},%') ",
                        "           OR  N.NEW_USER_ID_STR LIKE '%{NEW_USER_ID_STR},%' ",
                        "           OR  F.RESULT_ID IN ('F', 'C'))) ",
                        "BEGIN",
                        "    SET @RESULT = 'Y';",
                        "END",
                        "ELSE",
                        "BEGIN",
                        "    IF EXISTS (",
                        "           SELECT F.WF_NO ",
                        "             FROM WF_FLOW F ",
                        "             JOIN WF_NODE N ",
                        "               ON F.WF_NO = N.WF_NO ",
                        "              AND F.NODE_NO = N.NODE_NO ",
                        "             JOIN SYS_SYSTEM_ROLE_NODE Y ",
                        "               ON N.SYS_ID = Y.SYS_ID ",
                        "              AND N.WF_FLOW_ID = Y.WF_FLOW_ID ",
                        "              AND N.WF_FLOW_VER = Y.WF_FLOW_VER ",
                        "              AND N.WF_NODE_ID = Y.WF_NODE_ID ",
                        "             JOIN SYS_USER_SYSTEM_ROLE R ",
                        "               ON R.USER_ID = {USER_ID} ",
                        "              AND Y.SYS_ID = R.SYS_ID ",
                        "              AND Y.ROLE_ID = R.ROLE_ID ",
                        "            WHERE F.WF_NO = {WF_NO})",
                        "    BEGIN",
                        "        SET @RESULT = 'Y';",
                        "    END",
                        "    ELSE",
                        "    BEGIN",
                        "        IF EXISTS (",
                        "               SELECT F.WF_NO ",
                        "                 FROM WF_FLOW F ",
                        "                 JOIN WF_NODE N ",
                        "                   ON F.WF_NO = N.WF_NO ",
                        "                  AND F.NODE_NO = N.NODE_NO ",
                        "                 JOIN WF_SIG S ",
                        "                   ON F.WF_NO = S.WF_NO ",
                        "                  AND F.SYS_ID = S.SYS_ID ",
                        "                  AND F.WF_FLOW_ID = S.WF_FLOW_ID ",
                        "                  AND F.WF_FLOW_VER = S.WF_FLOW_VER ",
                        "                  AND N.WF_NODE_ID = S.WF_NODE_ID ",
                        "                  AND N.NODE_NO = S.NODE_NO ",
                        "                  AND S.SIG_DATE IS NOT NULL ",
                        "                  AND S.SIG_USER_ID = {USER_ID} ",
                        "                WHERE F.WF_NO = {WF_NO})",
                        "        BEGIN",
                        "            SET @RESULT = 'Y';",
                        "        END",
                        "    END",
                        "END",
                        "SELECT @RESULT;",
                        Environment.NewLine
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.SIG_USER_ID_STR.ToString(), Value = para.SigUserIDStr });
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.NEW_USER_ID_STR.ToString(), Value = para.NewUserIDStr });
            if (new DBChar(ExecuteScalar(commandText, dbParameters)).GetValue() == EnumYN.Y.ToString())
            {
                return EnumCheckRunTimeWFFlowResult.Success;
            }
            return EnumCheckRunTimeWFFlowResult.Failure;
        }

        public RunTimeWFFlow SelectRunTimeWFFlow(RunTimeWFFlowPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT W.WF_NO AS WFNo",
                        "     , W.SYS_ID AS SysID",
                        "     , W.WF_FLOW_ID AS WFFlowID",
                        "     , W.WF_FLOW_VER AS WFFlowVer",
                        "     , W.WF_SUBJECT AS WFSubject",
                        "     , W.NEW_USER_ID AS NewUserID",
                        "     , W.END_USER_ID AS EndUserID",
                        "     , W.DT_BEGIN AS DTBegin",
                        "     , W.DT_END AS DTEnd",
                        "     , W.RESULT_ID AS ResultID",
                        "     , W.NODE_NO AS NodeNo",
                        "     , W.UPD_USER_ID AS UpdUserID",
                        "     , W.UPD_DT AS UpdDT",
                        "     , F.FLOW_TYPE AS WFFlowType",
                        "     , F.FLOW_MAN_USER_ID AS WFFlowManUserID",
                        "     , F.ENABLE_DATE AS WFEnableDate",
                        "     , F.DISABLE_DATE AS WFDisableDate",
                        "     , dbo.FN_GET_WF_NO(W.WF_NO) AS FormatWFNo ",
                        "     , dbo.FN_GET_NMID(W.SYS_ID + '/' + W.WF_FLOW_ID + '-' + W.WF_FLOW_VER, F.{WF_FLOW}) AS WFFlowNM ",
                        "     , dbo.FN_GET_NMID(F.FLOW_TYPE, dbo.FN_GET_CM_NM('0026', F.FLOW_TYPE, {CULTURE_ID})) AS WFFlowTypeNM ",
                        "     , dbo.FN_GET_USER_NM(F.FLOW_MAN_USER_ID) AS WFFlowManUserNM ",
                        "     , dbo.FN_GET_USER_NM(W.NEW_USER_ID) AS NewUserNM ",
                        "     , dbo.FN_GET_USER_NM(W.END_USER_ID) AS EndUserNM ",
                        "     , dbo.FN_GET_NMID(W.RESULT_ID, dbo.FN_GET_CM_NM('0028', W.RESULT_ID, {CULTURE_ID})) AS ResultNM ",
                        "     , dbo.FN_GET_USER_NM(W.UPD_USER_ID) AS UpdUserNM ",
                        "  FROM WF_FLOW W ",
                        "  JOIN SYS_SYSTEM_WF_FLOW F ",
                        "    ON W.SYS_ID = F.SYS_ID ",
                        "   AND W.WF_FLOW_ID = F.WF_FLOW_ID ",
                        "   AND W.WF_FLOW_VER = F.WF_FLOW_VER ",
                        " WHERE W.WF_NO = {WF_NO} ",
                        Environment.NewLine
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.WF_NO.ToString(), Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.WF_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(RunTimeWFFlowPara.ParaField.WF_FLOW.ToString())) });
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.CULTURE_ID, Value = new DBVarChar(para.CultureID) });
            RunTimeWFFlow runTimeWfFlow = GetEntityList<RunTimeWFFlow>(commandText, dbParameters).SingleOrDefault();

            if (runTimeWfFlow != null)
            {
                SelectRunTimeWFNode(para, runTimeWfFlow);
                SelectRunTimeWFFlowRole(para, runTimeWfFlow);
            }

            return runTimeWfFlow;
        }

        private void SelectRunTimeWFNode(RunTimeWFFlowPara para, RunTimeWFFlow runTimeWFFlow)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT W.RESULT_ID AS ResultID",
                        "     , W.NODE_NO AS NodeNo",
                        "     , W.WF_NO AS WFNo",
                        "     , W.SYS_ID AS SysID",
                        "     , W.WF_FLOW_ID AS WFFlowID",
                        "     , W.WF_FLOW_VER AS WFFlowVer",
                        "     , W.WF_NODE_ID AS WFNodeID",
                        "     , W.NEW_USER_ID AS NewUserID ",
                        "     , W.NEW_USER_ID_STR AS NewUserIDStr ",
                        "     , W.SIG_USER_ID_STR AS WFSigUserIDStr ",
                        "     , W.IS_START_SIG AS WFSigIsStarting ",
                        "     , W.SIG_STEP AS WFSigStep ",
                        "     , N.NODE_TYPE AS WFNodeType",
                        "     , N.IS_FIRST AS WFNodeIsFirst",
                        "     , N.IS_FINALLY AS WFNodeIsFinally",
                        "     , N.BACK_WF_NODE_ID AS WFBackNodeID",
                        "     , N.FUN_SYS_ID AS WFFunSysID",
                        "     , N.FUN_CONTROLLER_ID AS WFFunControllerID",
                        "     , N.FUN_ACTION_NAME AS WFFunActionName",
                        "     , N.SIG_API_SYS_ID AS WFSigAPISysID",
                        "     , N.SIG_API_CONTROLLER_ID AS WFSigAPIControllerID",
                        "     , N.SIG_API_ACTION_NAME AS WFSigAPIActionName",
                        "     , N.ASSG_API_SYS_ID AS WFAssgAPISysID",
                        "     , N.ASSG_API_CONTROLLER_ID AS WFAssgAPIControllerID",
                        "     , N.ASSG_API_ACTION_NAME AS WFAssgAPIActionName",
                        "     , N.IS_ASSG_NEXT_NODE AS IsAssgNextNode",
                        "     , dbo.FN_GET_NMID(W.NODE_NO + '-' + W.WF_NODE_ID, N.{WF_NODE}) AS WFNodeNM ",
                        "     , N.WF_NODE_ZH_TW AS WFNodeZHTW ",
                        "     , dbo.FN_GET_USER_NM(W.NEW_USER_ID) AS NewUserNM ",
                        "     , W.NEW_USER_NM_STR AS NewUserNMStr ",
                        "     , dbo.FN_GET_USER_NM(W.END_USER_ID) AS EndUserNM ",
                        "     , dbo.FN_GET_NMID(W.RESULT_ID, dbo.FN_GET_CM_NM('0028', W.RESULT_ID, {CULTURE_ID})) AS ResultNM ",
                        "     , (CASE WHEN W.SIG_RESULT_ID IS NULL THEN NULL ELSE dbo.FN_GET_NMID(W.SIG_RESULT_ID, dbo.FN_GET_CM_NM('0029', W.SIG_RESULT_ID, {CULTURE_ID})) END) AS WFSigResultID ",
                        "     , dbo.FN_GET_USER_NM(W.UPD_USER_ID) AS UpdUserNM ",
                        "  FROM WF_NODE W ",
                        "  JOIN SYS_SYSTEM_WF_NODE N ",
                        "    ON W.SYS_ID = N.SYS_ID ",
                        "   AND W.WF_FLOW_ID = N.WF_FLOW_ID ",
                        "   AND W.WF_FLOW_VER = N.WF_FLOW_VER ",
                        "   AND W.WF_NODE_ID = N.WF_NODE_ID ",
                        " WHERE W.WF_NO = {WF_NO} ",
                        " ORDER BY W.NODE_NO ",
                        Environment.NewLine
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.WF_NO.ToString(), Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.WF_NODE.ToString(), Value = para.GetCultureFieldNM(new DBObject(RunTimeWFFlowPara.ParaField.WF_NODE.ToString())) });
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.CULTURE_ID, Value = new DBVarChar(para.CultureID) });

            runTimeWFFlow.WFNodeList = new RunTimeWFNodes(GetEntityList<RunTimeWFNode>(commandText, dbParameters));
            SelectRunTimeWFNodeRole(para, runTimeWFFlow);
            SelectRunTimeWFNextNodeRole(para, runTimeWFFlow);
            SelectRunTimeWFNodeNewUser(para, runTimeWFFlow);
            SelectRunTimeWFSig(para, runTimeWFFlow);
            SelectRunTimeWFDoc(para, runTimeWFFlow);
        }

        private void SelectRunTimeWFNodeRole(RunTimeWFFlowPara para, RunTimeWFFlow runTimeWFFlow)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT W.NODE_NO AS NodeNo",
                        "     , S.ROLE_ID AS RoleID",
                        "  FROM WF_NODE W",
                        "  JOIN SYS_SYSTEM_ROLE_NODE S ",
                        "    ON W.SYS_ID = S.SYS_ID ",
                        "   AND W.WF_FLOW_ID = S.WF_FLOW_ID ",
                        "   AND W.WF_FLOW_VER = S.WF_FLOW_VER ",
                        "   AND W.WF_NODE_ID = S.WF_NODE_ID ",
                        " WHERE W.WF_NO = {WF_NO} ",
                        " ORDER BY W.NODE_NO ",
                        Environment.NewLine
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.WF_NO.ToString(), Value = para.WFNo });

            var runTimeWFNodeRoleList = GetEntityList<RunTimeWFNodeRole>(commandText, dbParameters);

            if (runTimeWFNodeRoleList.Any())
            {
                runTimeWFFlow.WFNodeList.ForEach(row =>
                {
                    row.RoleList = new RunTimeWFNodeRoles();
                    row.RoleList.AddRange(runTimeWFNodeRoleList.FindAll(f => f.NodeNo.GetValue() == row.NodeNo.GetValue()));
                });
            }
        }

        private void SelectRunTimeWFNextNodeRole(RunTimeWFFlowPara para, RunTimeWFFlow runTimeWFFlow)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
            {
                "SELECT WN.NODE_NO AS NodeNo", 
                "     , SR.ROLE_ID AS RoleID", 
                "  FROM WF_NODE WN ", 
                "  JOIN SYS_SYSTEM_WF_NEXT SN ", 
                "    ON WN.SYS_ID = SN.SYS_ID ", 
                "   AND WN.WF_FLOW_ID = SN.WF_FLOW_ID ", 
                "   AND WN.WF_FLOW_VER = SN.WF_FLOW_VER ", 
                "   AND WN.WF_NODE_ID = SN.WF_NODE_ID ",
                "  JOIN SYS_SYSTEM_ROLE_NODE SR ", 
                "    ON SN.SYS_ID = SR.SYS_ID ", 
                "   AND SN.WF_FLOW_ID = SR.WF_FLOW_ID ", 
                "   AND SN.WF_FLOW_VER = SR.WF_FLOW_VER ", 
                "   AND SN.NEXT_WF_NODE_ID = SR.WF_NODE_ID ", 
                " WHERE WN.WF_NO = {WF_NO} ", 
                " ORDER BY WN.NODE_NO ", 
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.WF_NO.ToString(), Value = para.WFNo });

            var runTimeWFNextNodeRoleList = GetEntityList<RunTimeWFNodeRole>(commandText, dbParameters);

            if (runTimeWFNextNodeRoleList.Any())
            {
                runTimeWFFlow.WFNodeList.ForEach(row =>
                {
                    row.NextNodeRoleList = new RunTimeWFNodeRoles();
                    row.NextNodeRoleList.AddRange(runTimeWFNextNodeRoleList.FindAll(f => f.NodeNo.GetValue() == row.NodeNo.GetValue()));
                });
            }
        }

        private void SelectRunTimeWFNodeNewUser(RunTimeWFFlowPara para, RunTimeWFFlow runTimeWFFlow)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT W.NODE_NO AS NodeNo",
                        "     , W.NEW_USER_ID AS NewUserID ",
                        "     , dbo.FN_GET_USER_NM(W.NEW_USER_ID) AS NewUserNM ",
                        "  FROM WF_NODE_NEW_USER W ",
                        " WHERE W.WF_NO = {WF_NO} ",
                        " ORDER BY W.NODE_NO ",
                        Environment.NewLine
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.WF_NO.ToString(), Value = para.WFNo });

            var runTimeWFNodeNewUser = GetEntityList<RunTimeWFNodeNewUser>(commandText, dbParameters);

            if (runTimeWFNodeNewUser.Any())
            {
                runTimeWFFlow.WFNodeList.ForEach(row =>
                {
                    row.NewUserList = new RunTimeWFNodeNewUsers();
                    row.NewUserList.AddRange(runTimeWFNodeNewUser.FindAll(f => f.NodeNo.GetValue() == row.NodeNo.GetValue()));
                });
            }
        }

        private void SelectRunTimeWFSig(RunTimeWFFlowPara para, RunTimeWFFlow runTimeWFFlow)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT W.NODE_NO AS NodeNo",
                        "     , W.SYS_ID AS SysID",
                        "     , W.SIG_KIND AS SigKind",
                        "     , W.SIG_STEP AS SigStep",
                        "     , W.WF_SIG_SEQ AS WFSigSeq",
                        "     , W.WF_SIG_ZH_TW AS WFSigZHTW",
                        "     , W.WF_SIG_ZH_CN AS WFSigZHCN",
                        "     , W.WF_SIG_EN_US AS WFSigENUS",
                        "     , W.WF_SIG_TH_TH AS WFSigTHTH",
                        "     , W.WF_SIG_JA_JP AS WFSigJAJP",
                        "     , W.WF_SIG_KO_KR AS WFSigKOKR",
                        "     , W.{WF_SIG} AS WFSigNM",
                        "     , W.SIG_USER_ID AS SigUserID",
                        "     , W.SIG_DATE AS SigDate",
                        "     , W.SIG_RESULT_ID AS SigResultID",
                        "     , W.UPD_USER_ID AS UpdUserID",
                        "     , W.UPD_DT AS UpdDT",
                        "     , dbo.FN_GET_USER_NM(W.SIG_USER_ID) AS SigUserNM ",
                        "     , S.SIG_TYPE AS WFSigType",
                        "     , S.API_SYS_ID AS WFAPISysID",
                        "     , S.API_CONTROLLER_ID AS WFAPIControllerID",
                        "     , S.API_ACTION_NAME AS WFAPIActionName",
                        "     , S.COMPARE_WF_NODE_ID AS WFCompareNodeID",
                        "     , S.COMPARE_WF_SIG_SEQ AS WFCompareSigSeq",
                        "     , S.CHK_API_SYS_ID AS WFChkAPISysID",
                        "     , S.CHK_API_CONTROLLER_ID AS WFChkAPIControllerID",
                        "     , S.CHK_API_ACTION_NAME AS WFChkAPIActionName",
                        "  FROM WF_SIG W ",
                        "  LEFT JOIN SYS_SYSTEM_WF_SIG S ",
                        "    ON W.SYS_ID = S.SYS_ID ",
                        "   AND W.WF_FLOW_ID = S.WF_FLOW_ID ",
                        "   AND W.WF_FLOW_VER = S.WF_FLOW_VER ",
                        "   AND W.WF_NODE_ID = S.WF_NODE_ID ",
                        "   AND W.WF_SIG_SEQ = S.WF_SIG_SEQ ",
                        " WHERE W.WF_NO = {WF_NO} ",
                        " ORDER BY W.NODE_NO, W.SIG_STEP, W.WF_SIG_SEQ ",
                        Environment.NewLine
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.WF_NO.ToString(), Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.WF_SIG.ToString(), Value = para.GetCultureFieldNM(new DBObject(RunTimeWFFlowPara.ParaField.WF_SIG.ToString())) });

            var runTimeWFSigList = GetEntityList<RunTimeWFSig>(commandText, dbParameters);

            if (runTimeWFSigList.Any())
            {
                runTimeWFFlow.WFNodeList.ForEach(row =>
                {
                    row.WFSigList = new RunTimeWFSigs();
                    row.WFSigList.AddRange(runTimeWFSigList.FindAll(f => f.NodeNo.GetValue() == row.NodeNo.GetValue()));
                });

                SelectRunTimeWFSigRole(para, runTimeWFFlow);
            }
        }

        private void SelectRunTimeWFSigRole(RunTimeWFFlowPara para, RunTimeWFFlow runTimeWFFlow)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT W.NODE_NO AS NodeNo",
                        "     , W.SIG_STEP AS SigStep",
                        "     , W.WF_SIG_SEQ AS WFSigSeq",
                        "     , S.ROLE_ID AS RoleID",
                        "  FROM WF_SIG W ",
                        "  JOIN SYS_SYSTEM_ROLE_SIG S ",
                        "    ON W.SYS_ID = S.SYS_ID ",
                        "   AND W.WF_FLOW_ID = S.WF_FLOW_ID ",
                        "   AND W.WF_FLOW_VER = S.WF_FLOW_VER ",
                        "   AND W.WF_NODE_ID = S.WF_NODE_ID ",
                        "   AND W.WF_SIG_SEQ = S.WF_SIG_SEQ ",
                        " WHERE W.WF_NO = {WF_NO} ",
                        " ORDER BY W.NODE_NO, W.SIG_STEP, W.WF_SIG_SEQ ",
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.WF_NO.ToString(), Value = para.WFNo });

            var runTimeWFSigRoleList = GetEntityList<RunTimeWFSigRole>(commandText, dbParameters);

            if (runTimeWFSigRoleList.Any())
            {
                runTimeWFFlow.WFNodeList.ForEach(row =>
                {
                    row.WFSigList.ForEach(rowSig =>
                    {
                        rowSig.RoleList = new RunTimeWFSigRoles();
                        rowSig.RoleList.AddRange(runTimeWFSigRoleList.FindAll(f => f.NodeNo.GetValue() == row.NodeNo.GetValue()));
                    });
                });
            }
        }

        private void SelectRunTimeWFDoc(RunTimeWFFlowPara para, RunTimeWFFlow runTimeWFFlow)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT NODE_NO AS NodeNo",
                        "     , DOC_NO AS DocNo",
                        "     , SYS_ID AS SysID",
                        "     , WF_DOC_SEQ AS WFDocSeq",
                        "     , WF_DOC_ZH_TW AS WFDocZHTW",
                        "     , WF_DOC_ZH_CN AS WFDocZHCN",
                        "     , WF_DOC_EN_US AS WFDocENUS",
                        "     , WF_DOC_TH_TH AS WFDocTHTH",
                        "     , WF_DOC_JA_JP AS WFDocJAJP",
                        "     , WF_DOC_KO_KR AS WFDocKOKR",
                        "     , IS_REQ AS IsReq",
                        "     , DOC_USER_ID AS DocUserID",
                        "     , DOC_FILE_NAME AS DocFileName",
                        "     , DOC_ENCODE_NAME AS DocEncodeName",
                        "     , DOC_DATE AS DocDate",
                        "     , DOC_PATH AS DocPath",
                        "     , DOC_LOCAL_PATH AS DocLocalPath",
                        "     , IS_DELETE AS IsDelete",
                        "     , UPD_USER_ID AS UpdUserID",
                        "     , UPD_DT AS UpdDT",
                        "  FROM WF_DOC ",
                        " WHERE WF_NO = {WF_NO} ",
                        " ORDER BY NODE_NO, DOC_NO ",
                        Environment.NewLine
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.WF_NO.ToString(), Value = para.WFNo });

            var runTimeWFDocList = GetEntityList<RunTimeWFDoc>(commandText, dbParameters);

            if (runTimeWFDocList.Any())
            {
                runTimeWFFlow.WFNodeList.ForEach(row =>
                {
                    row.WFDocList = new RunTimeWFDocs();
                    row.WFDocList.AddRange(runTimeWFDocList.FindAll(f => f.NodeNo.GetValue() == row.NodeNo.GetValue()));
                });
            }
        }

        private void SelectRunTimeWFFlowRole(RunTimeWFFlowPara para, RunTimeWFFlow runTimeWFFlow)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT S.ROLE_ID AS RoleID", 
                        "  FROM WF_FLOW W ", 
                        "  JOIN SYS_SYSTEM_ROLE_FLOW S ", 
                        "    ON W.SYS_ID = S.SYS_ID ", 
                        "   AND W.WF_FLOW_ID = S.WF_FLOW_ID ", 
                        "   AND W.WF_FLOW_VER = S.WF_FLOW_VER ", 
                        " WHERE W.WF_NO = {WF_NO} ", 
                        Environment.NewLine
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RunTimeWFFlowPara.ParaField.WF_NO.ToString(), Value = para.WFNo });
            runTimeWFFlow.RoleList = new RunTimeWFFlowRoles();
            runTimeWFFlow.RoleList.AddRange(GetEntityList<RunTimeWFFlowRole>(commandText, dbParameters));
        }

        #endregion

        #region API
        public class SystemAPIRolePara
        {
            public enum ParaField
            {
                SYS_ID, API_CONTROLLER_ID, API_ACTION_NAME, IS_OUTSIDE,
                USER_ID
            }

            public DBVarChar SystemID;
            public DBVarChar ControllerID;
            public DBVarChar ActionName;
            public DBChar IsOutside;
            public DBVarChar UserID;
        }

        public class SystemAPIRole
        {
        }

        public enum EnumValidateSystemAPIRoleResult
        {
            Success, Failure
        }

        public EnumValidateSystemAPIRoleResult ValidateSystemAPIRole(SystemAPIRolePara systemAPIRolePara)
        {
            string whereCommandText = string.Empty;
            if (systemAPIRolePara.IsOutside.GetValue() == EnumYN.Y.ToString())
            {
                whereCommandText = string.Concat(new object[] 
                {
                    "      AND A.IS_OUTSIDE={IS_OUTSIDE} ", Environment.NewLine
                });
            }

            string commandText = string.Concat(new object[] 
            { 
                "DECLARE @API_CN INT = 0; ", Environment.NewLine,
                "DECLARE @ROLE_CN INT = 0; ", Environment.NewLine,

                "SELECT @ROLE_CN=COUNT(ROLE_ID) ", Environment.NewLine,
                "FROM SYS_SYSTEM_ROLE_API ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND API_CONTROLLER_ID={API_CONTROLLER_ID} AND API_ACTION_NAME={API_ACTION_NAME}; ", Environment.NewLine,
                
                "IF @ROLE_CN>0 ", Environment.NewLine,
                "BEGIN ", Environment.NewLine,
                "    SELECT @API_CN=COUNT(I.API_ACTION_NAME) ", Environment.NewLine,
                "    FROM SYS_USER_SYSTEM_ROLE R ", Environment.NewLine,
                "    JOIN SYS_SYSTEM_ROLE_API I ON R.SYS_ID=I.SYS_ID AND R.ROLE_ID=I.ROLE_ID ", Environment.NewLine,
                "    JOIN SYS_SYSTEM_API A ON I.SYS_ID=A.SYS_ID AND I.API_CONTROLLER_ID=A.API_CONTROLLER_ID AND I.API_ACTION_NAME=A.API_ACTION_NAME ", Environment.NewLine,
                "    JOIN SYS_SYSTEM_MAIN M ON A.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "    WHERE R.USER_ID={USER_ID} ", Environment.NewLine,
                "      AND I.SYS_ID={SYS_ID} AND I.API_CONTROLLER_ID={API_CONTROLLER_ID} AND I.API_ACTION_NAME={API_ACTION_NAME} ", Environment.NewLine,
                "      AND A.IS_DISABLE='N' AND M.IS_DISABLE='N'; ", Environment.NewLine,
                whereCommandText,
                "END; ", Environment.NewLine,
                
                "SELECT @API_CN; "
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemAPIRolePara.ParaField.SYS_ID.ToString(), Value = systemAPIRolePara.SystemID });
            dbParameters.Add(new DBParameter { Name = SystemAPIRolePara.ParaField.API_CONTROLLER_ID.ToString(), Value = systemAPIRolePara.ControllerID });
            dbParameters.Add(new DBParameter { Name = SystemAPIRolePara.ParaField.API_ACTION_NAME.ToString(), Value = systemAPIRolePara.ActionName });
            dbParameters.Add(new DBParameter { Name = SystemAPIRolePara.ParaField.IS_OUTSIDE.ToString(), Value = systemAPIRolePara.IsOutside });
            dbParameters.Add(new DBParameter { Name = SystemAPIRolePara.ParaField.USER_ID.ToString(), Value = systemAPIRolePara.UserID });

            DBInt result = new DBInt(base.ExecuteScalar(commandText, dbParameters));
            if (result.GetValue() > 0)
            {
                return EnumValidateSystemAPIRoleResult.Success;
            }
            return EnumValidateSystemAPIRoleResult.Failure;
        }


        public class SystemAPIFunPara
        {
            public enum ParaField
            {
                SYS_ID, API_CONTROLLER_ID, API_ACTION_NAME, CLIENT_SYS_ID, IP_ADDRESS, IS_OUTSIDE
            }

            public DBVarChar SystemID;
            public DBVarChar ControllerID;
            public DBVarChar ActionName;
            public DBVarChar ClientSysID;
            public DBVarChar IPAddress;
            public DBChar IsOutside;
        }

        public class SystemAPIFun
        {
        }

        public enum EnumValidateSystemAPIFunResult
        {
            Success, Failure
        }

        public EnumValidateSystemAPIFunResult ValidateSystemAPIFun(SystemAPIFunPara systemAPIFunPara)
        {
            string whereCommandText = string.Empty;
            if (systemAPIFunPara.IsOutside.GetValue() == EnumYN.Y.ToString())
            {
                whereCommandText = string.Concat(new object[] 
                {
                    "  AND A.IS_OUTSIDE={IS_OUTSIDE} ", Environment.NewLine
                });
            }

            string commandText = string.Concat(new object[] 
            { 
                "DECLARE @API_CN INT = 0; ", Environment.NewLine,
                "DECLARE @CN_CLIENT_SYS_ID INT = 0; ", Environment.NewLine,
                "DECLARE @CN_CLIENT_SYS_IP_ADDRESS INT = 0; ", Environment.NewLine,

                "SELECT @CN_CLIENT_SYS_ID=COUNT(CLIENT_SYS_ID) ", Environment.NewLine,
                "FROM SYS_SYSTEM_API_CLIENT ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND API_CONTROLLER_ID={API_CONTROLLER_ID} AND API_ACTION_NAME={API_ACTION_NAME}; ", Environment.NewLine,
                "IF @CN_CLIENT_SYS_ID>0 ", Environment.NewLine,
                "BEGIN ", Environment.NewLine,
                "    SELECT @CN_CLIENT_SYS_IP_ADDRESS=COUNT(P.IP_ADDRESS) ", Environment.NewLine,
                "    FROM SYS_SYSTEM_API_CLIENT C ", Environment.NewLine,
                "    JOIN SYS_SYSTEM_IP P ON C.CLIENT_SYS_ID=P.SYS_ID ", Environment.NewLine,
                "    WHERE C.SYS_ID={SYS_ID} AND C.API_CONTROLLER_ID={API_CONTROLLER_ID} AND C.API_ACTION_NAME={API_ACTION_NAME} ", Environment.NewLine,
                "      AND C.CLIENT_SYS_ID={CLIENT_SYS_ID} AND P.IP_ADDRESS={IP_ADDRESS}; ", Environment.NewLine,
                "END; ", Environment.NewLine,

                "IF (@CN_CLIENT_SYS_ID>0 AND @CN_CLIENT_SYS_IP_ADDRESS>0) ", Environment.NewLine,
                "BEGIN ", Environment.NewLine,
                "    SELECT @API_CN=COUNT(A.API_ACTION_NAME) ", Environment.NewLine,
                "    FROM SYS_SYSTEM_API A ", Environment.NewLine,
                "    JOIN SYS_SYSTEM_MAIN M ON A.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "    WHERE A.SYS_ID={SYS_ID} AND A.API_CONTROLLER_ID={API_CONTROLLER_ID} AND A.API_ACTION_NAME={API_ACTION_NAME} ", Environment.NewLine,
                "      AND A.IS_DISABLE='N' AND M.IS_DISABLE='N' ", Environment.NewLine,
                whereCommandText,
                "END; ", Environment.NewLine,

                "SELECT @API_CN; ", Environment.NewLine,
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemAPIFunPara.ParaField.SYS_ID.ToString(), Value = systemAPIFunPara.SystemID });
            dbParameters.Add(new DBParameter { Name = SystemAPIFunPara.ParaField.API_CONTROLLER_ID.ToString(), Value = systemAPIFunPara.ControllerID });
            dbParameters.Add(new DBParameter { Name = SystemAPIFunPara.ParaField.API_ACTION_NAME.ToString(), Value = systemAPIFunPara.ActionName });
            dbParameters.Add(new DBParameter { Name = SystemAPIFunPara.ParaField.CLIENT_SYS_ID.ToString(), Value = systemAPIFunPara.ClientSysID });
            dbParameters.Add(new DBParameter { Name = SystemAPIFunPara.ParaField.IP_ADDRESS.ToString(), Value = systemAPIFunPara.IPAddress });
            dbParameters.Add(new DBParameter { Name = SystemAPIFunPara.ParaField.IS_OUTSIDE.ToString(), Value = systemAPIFunPara.IsOutside });

            DBInt result = new DBInt(base.ExecuteScalar(commandText, dbParameters));
            if (result.GetValue() > 0)
            {
                return EnumValidateSystemAPIFunResult.Success;
            }
            return EnumValidateSystemAPIFunResult.Failure;
        }


        public class APIClientPara
        {
            public enum ParaField
            {
                API_NO, SYS_ID, API_CONTROLLER_ID, API_ACTION_NAME, CLIENT_SYS_ID, CLIENT_USER_ID,
                API_DATE, API_TIME, DT_BEGIN, DT_END, REQ_URL, REQ_RETURN, IP_ADDRESS,
                UPD_USER_ID
            }

            public DBChar APINo;
            public DBVarChar SysID;
            public DBVarChar APIControllerID;
            public DBVarChar APIActionName;
            public DBVarChar ClientSysID;
            public DBVarChar ClientUserID;
            public DBChar APIDate;
            public DBChar APITime;
            public DBDateTime DTBegin;
            public DBDateTime DTEnd;
            public DBNVarChar ReqURL;
            public DBNVarChar ReqReturn;
            public DBVarChar IPAddress;
            public DBVarChar UpdUserID;
        }

        public class APIClient : DBTableRow
        {
        }

        public string ExecAPIClientBegin(APIClientPara apiClientPara)
        {
            //string commandText = string.Concat(new object[] 
            //{ 
            //    "DECLARE @TODAY_YMD CHAR(8); ", Environment.NewLine,
            //    "DECLARE @API_NO CHAR(16); ", Environment.NewLine,

            //    "SET @TODAY_YMD = dbo.FN_GET_SYSDATE(NULL); ", Environment.NewLine,

            //    "SELECT @API_NO=@TODAY_YMD+RIGHT('0000000'+CAST(ISNULL(CAST(SUBSTRING(MAX(API_NO),9,8) AS INT),0)+1 AS VARCHAR),8) ", Environment.NewLine,
            //    "FROM API_CLIENT ", Environment.NewLine,
            //    "WHERE API_NO>@TODAY_YMD + '00000000'; ", Environment.NewLine,

            //    "INSERT INTO API_CLIENT VALUES ( ", Environment.NewLine,
            //    "    @API_NO, {SYS_ID}, {API_CONTROLLER_ID}, {API_ACTION_NAME}, {CLIENT_SYS_ID}, {CLIENT_USER_ID} ", Environment.NewLine,
            //    "  , {API_DATE}, {API_TIME}, {DT_BEGIN}, NULL, LEFT({REQ_URL}, 4000), NULL, NULL, NULL, NULL, NULL, {IP_ADDRESS} ", Environment.NewLine,
            //    "  , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
            //    "); ", Environment.NewLine,

            //    "SELECT @API_NO; ", Environment.NewLine,
            //});

            //List<DBParameter> dbParameters = new List<DBParameter>();
            //dbParameters.Add(new DBParameter { Name = APIClientPara.ParaField.SYS_ID, Value = apiClientPara.SysID });
            //dbParameters.Add(new DBParameter { Name = APIClientPara.ParaField.API_CONTROLLER_ID, Value = apiClientPara.APIControllerID });
            //dbParameters.Add(new DBParameter { Name = APIClientPara.ParaField.API_ACTION_NAME, Value = apiClientPara.APIActionName });
            //dbParameters.Add(new DBParameter { Name = APIClientPara.ParaField.CLIENT_SYS_ID, Value = apiClientPara.ClientSysID });
            //dbParameters.Add(new DBParameter { Name = APIClientPara.ParaField.CLIENT_USER_ID, Value = apiClientPara.ClientUserID });
            //dbParameters.Add(new DBParameter { Name = APIClientPara.ParaField.API_DATE, Value = apiClientPara.APIDate });
            //dbParameters.Add(new DBParameter { Name = APIClientPara.ParaField.API_TIME, Value = apiClientPara.APITime });
            //dbParameters.Add(new DBParameter { Name = APIClientPara.ParaField.DT_BEGIN, Value = apiClientPara.DTBegin });
            //dbParameters.Add(new DBParameter { Name = APIClientPara.ParaField.REQ_URL, Value = apiClientPara.ReqURL });
            //dbParameters.Add(new DBParameter { Name = APIClientPara.ParaField.IP_ADDRESS, Value = apiClientPara.IPAddress });
            //dbParameters.Add(new DBParameter { Name = APIClientPara.ParaField.UPD_USER_ID, Value = apiClientPara.UpdUserID });

            //DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            //if (result.IsNull() == false)
            //{
            //    return result.GetValue();
            //}
            return null;
        }

        public void ExecAPIClientEnd(APIClientPara apiClientPara)
        {
            //string commandText = string.Concat(new object[] 
            //{ 
            //    "UPDATE API_CLIENT SET ", Environment.NewLine,
            //    "    DT_END={DT_END}, REQ_RETURN = SUBSTRING({REQ_RETURN}, 1, 500) " , Environment.NewLine,
            //    "  , UPD_USER_ID={UPD_USER_ID}, UPD_DT=GETDATE() " , Environment.NewLine,
            //    "WHERE API_NO={API_NO}; ", Environment.NewLine
            //});

            //List<DBParameter> dbParameters = new List<DBParameter>();
            //dbParameters.Add(new DBParameter { Name = APIClientPara.ParaField.API_NO, Value = apiClientPara.APINo });
            //dbParameters.Add(new DBParameter { Name = APIClientPara.ParaField.DT_END, Value = apiClientPara.DTEnd });
            //dbParameters.Add(new DBParameter { Name = APIClientPara.ParaField.REQ_RETURN, Value = apiClientPara.ReqReturn });
            //dbParameters.Add(new DBParameter { Name = APIClientPara.ParaField.UPD_USER_ID, Value = apiClientPara.UpdUserID });

            //base.ExecuteScalar(commandText, dbParameters);
        }
        #endregion
    }

    public class Mongo_Base : MongoEntity
    {
        public Mongo_Base(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class APIClientPara : MongoDocument
        {
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;

            [DBTypeProperty("API_CONTROLLER_ID")]
            public DBVarChar APIControllerID;

            [DBTypeProperty("API_ACTION_NAME")]
            public DBVarChar APIActionName;

            [DBTypeProperty("API_DATE")]
            public DBChar APIDate;

            [DBTypeProperty("API_TIME")]
            public DBChar APITime;

            [DBTypeProperty("DT_BEGIN")]
            public DBDateTime DTBegin;

            [DBTypeProperty("DT_END")]
            public DBDateTime DTEnd;

            [DBTypeProperty("REQ_URL")]
            public DBNVarChar ReqURL;

            [DBTypeProperty("API_PARA")]
            public DBNVarChar APIPara;

            [DBTypeProperty("REQ_RETURN")]
            public DBNVarChar ReqReturn;

            [DBTypeProperty("IP_ADDRESS")]
            public DBVarChar IPAddress;

            [DBTypeProperty("UPD_USER_ID")]
            public DBVarChar UpdUserID;
        }

    }
}