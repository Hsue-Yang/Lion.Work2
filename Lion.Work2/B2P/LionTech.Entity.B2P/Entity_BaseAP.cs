using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P
{
    public class Entity_BaseAP : DBEntity
    {
        public Entity_BaseAP(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemIPPara
        {
            public enum ParaField
            {
                IP_ADDRESS
            }

            public DBVarChar IPAddress;
        }

        public DBInt SelectSystemIP(SystemIPPara para)
        {
            string commandText = string.Concat(new object[] 
            { 
                "SELECT COUNT(1) AS COUNT_CN ", Environment.NewLine,
                "FROM SYS_SYSTEM_IP ", Environment.NewLine,
                "WHERE IP_ADDRESS={IP_ADDRESS} ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemIPPara.ParaField.IP_ADDRESS, Value = para.IPAddress });

            return new DBInt(base.ExecuteScalar(commandText, dbParameters));
        }

        #region - EDIXMLRule -
        public class SystemEDIFlowDetail : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, EDI_FLOW_ID,
                EDI_FLOW_NM, SCH_FREQUENCY, SCH_START_DATE, SCH_START_TIME, SCH_DATA_DELAY,
                PATHS_CMD, PATHS_DAT, PATHS_SRC, PATHS_RES, PATHS_BAD, PATHS_LOG,
                PATHS_FLOW_XML, PATHS_FLOW_CMD, PATHS_ZIP_DAT, PATHS_EXCEPTION, PATHS_SUMMARY, SORT_ORDER
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBNVarChar EDIFlowNM;
            public DBVarChar SCHFrequency;
            public DBChar SCHStartDate;
            public DBChar SCHStartTime;
            public DBInt SCHDataDelay;
            public DBNVarChar PATHSCmd;
            public DBNVarChar PATHSDat;
            public DBNVarChar PATHSSrc;
            public DBNVarChar PATHSRes;
            public DBNVarChar PATHSBad;
            public DBNVarChar PATHSLog;
            public DBNVarChar PATHSFlowXml;
            public DBNVarChar PATHSFlowCmd;
            public DBNVarChar PATHSZipDat;
            public DBNVarChar PATHSException;
            public DBNVarChar PATHSSummary;
            public DBVarChar SortOrder;
        }

        public class SystemEDIJobDetail : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, EDI_FLOW_ID,
                EDI_JOB_ID, EDI_JOB_NM,
                EDI_JOB_TYPE, EDI_CON_ID, OBJECT_NAME, DEP_EDI_JOB_ID,
                IS_USE_RES, FILE_SOURCE, FILE_ENCODING, URL_PATH, IS_DISABLE, SORT_ORDER
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBVarChar EDIJobID;
            public DBNVarChar EDIJobNM;
            public DBVarChar EDIJobType;
            public DBVarChar EDIConID;
            public DBNVarChar ObjectName;
            public DBVarChar DepEDIJobID;
            public DBNVarChar FileSource;
            public DBNVarChar FileEncoding;
            public DBNVarChar URLPath;
            public DBChar IsUseRes;
            public DBChar IsDisable;
            public DBNVarChar SortOrder;
        }

        public class SystemEDIConnectionDetail : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, EDI_FLOW_ID,
                EDI_CON_ID, EDI_CON_NM,
                PROVIDER_NAME, CON_VALUE, SORT_ORDER
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBVarChar EDIConID;
            public DBNVarChar EDIConNM;
            public DBNVarChar ProviderName;
            public DBNVarChar ConValue;
            public DBNVarChar SortOrder;
        }

        public class SystemEDIParaDetail : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, EDI_FLOW_ID,
                EDI_JOB_ID, EDI_JOB_PARA_ID,
                EDI_JOB_PARA_TYPE, EDI_JOB_PARA_VALUE
            }

            public DBNVarChar SysID;
            public DBNVarChar EDIFlowID;
            public DBNVarChar EDIJobID;
            public DBNVarChar EDIJobParaID;
            public DBNVarChar EDIJobParaType;
            public DBNVarChar EDIJobParaValue;
        }

        public class SystemEDIFlowExecuteTimeDetail : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, EDI_FLOW_ID, EXECUTE_TIME,
            }
            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBChar ExecuteTime;
        }

        public class PathType
        {
            public enum Id
            {
                CMD,
                DAT,
                SRC,
                RES,
                BAD,
                LOG,
                FlowXML,
                FlowCMD,
                ZipDAT,
                Exception,
                Summary
            }

            public enum Extension
            {
                sql,
                dat,
                bad,
                log,
                bak,
                zip,
                err
            }
        }

        public class root : DBTableRow
        {
            public DBXmlNode Attribute;
            public List<flow> flows;
        }

        public class flow : DBTableRow
        {
            public enum Field
            {
                description, id
            }

            public DBXmlNode Attribute;
            public schedule schedule;
            public List<path> paths;
            public List<connection> connections;
            public List<job> jobs;
        }

        public class schedule : DBTableRow
        {
            public DBXmlNode frequency;
            public DBXmlNode startDate;
            public DBXmlNode startTime;
            public List<fixedTime> fixedTimes;
            public DBXmlNode dataDelay;
        }

        public class fixedTime : DBTableRow
        {
            public enum Field
            {
                value
            }

            public DBXmlNode Attribute;
        }

        public class path : DBTableRow
        {
            public enum Field
            {
                id,
                value,
                exName
            }

            public DBXmlNode Attribute;
        }

        public class connection : DBTableRow
        {
            public enum Field
            {
                id,
                providerName,
                value
            }

            public DBXmlNode Attribute;
        }

        public class job : DBTableRow
        {
            public enum Field
            {
                id,
                description,
                fileSource,
                fileEncoding,
                urlPath,
                useRES,
                isDisable
            }

            public DBXmlNode Attribute;
            public DBXmlNode type;
            public DBXmlNode connectionID;
            public DBXmlNode objectName;
            public DBXmlNode dependOnJobID;
            public List<parameter> parameters;
        }

        public class parameter : DBTableRow
        {
            public enum Field
            {
                id,
                type,
                value
            }

            public DBXmlNode Attribute;
        }

        public enum EnumGenerateEDIXMLResult
        {
            Success, Failure,
            EDIIsEmpty
        }
        #endregion

        #region - UserMenu -

        public class UserMenuFunPara : DBCulture
        {
            public UserMenuFunPara(string cultureID)
                : base(cultureID)
            {

            }
            public enum ParaField
            {
                USER_ID, FUN_NM, FUN_MENU_NM
            }

            public DBVarChar UserID;
        }

        public class UserMenuFun : DBTableRow
        {
            public enum DataField
            {
                USER_ID,
                MENU_ID, FUN_MENU, FUN_MENU_NM,
                SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME, FUN_NM,
                FUN_MENU_XAXIS
            }

            public DBVarChar UserID;

            public DBVarChar MenuID;
            public DBVarChar FunMenu;
            public DBNVarChar FunMenuNM;

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
            public DBNVarChar FunNM;
            public DBVarChar FunMenuXAxis;
        }

        public List<UserMenuFun> SelectUserMenuFunList(UserMenuFunPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT M.USER_ID, D.MENU_ID ", Environment.NewLine,
                "     , Z.FUN_MENU, Z.FUN_MENU_NM ", Environment.NewLine,
                "     , F.SUB_SYS_ID AS SYS_ID, M.FUN_CONTROLLER_ID, M.FUN_ACTION_NAME, F.{FUN_NM} AS FUN_NM ", Environment.NewLine,
                "     , Z.FUN_MENU_XAXIS ", Environment.NewLine,
                "FROM SYS_USER_FUN M ", Environment.NewLine,
                "JOIN SYS_SYSTEM_FUN F ON M.SYS_ID=F.SYS_ID AND M.FUN_CONTROLLER_ID=F.FUN_CONTROLLER_ID AND M.FUN_ACTION_NAME=F.FUN_ACTION_NAME ", Environment.NewLine,
                "LEFT JOIN ( ", Environment.NewLine,
                "    SELECT N.SYS_ID, N.FUN_CONTROLLER_ID, N.FUN_ACTION_NAME ", Environment.NewLine,
                "         , N.FUN_MENU_SYS_ID, N.FUN_MENU, M.{FUN_MENU_NM} AS FUN_MENU_NM ", Environment.NewLine,
                "         , N.FUN_MENU_XAXIS, N.FUN_MENU_YAXIS ", Environment.NewLine,
                "    FROM SYS_SYSTEM_MENU_FUN N ", Environment.NewLine,
                "    JOIN SYS_SYSTEM_FUN_MENU M ON N.FUN_MENU_SYS_ID=M.SYS_ID AND N.FUN_MENU=M.FUN_MENU ", Environment.NewLine,
                ") Z ON M.SYS_ID=Z.SYS_ID AND M.FUN_CONTROLLER_ID=Z.FUN_CONTROLLER_ID AND M.FUN_ACTION_NAME=Z.FUN_ACTION_NAME ", Environment.NewLine,
                "JOIN SYS_USER_FUN_MENU D ON M.USER_ID=D.USER_ID AND Z.FUN_MENU_SYS_ID=D.SYS_ID AND Z.FUN_MENU=D.FUN_MENU ", Environment.NewLine,
                "WHERE M.USER_ID={USER_ID} ", Environment.NewLine,
                "ORDER BY D.MENU_ID, D.SORT_ORDER, Z.FUN_MENU, Z.FUN_MENU_XAXIS, Z.FUN_MENU_YAXIS, F.SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserMenuFunPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserMenuFunPara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserMenuFunPara.ParaField.FUN_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = UserMenuFunPara.ParaField.FUN_MENU_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserMenuFunPara.ParaField.FUN_MENU_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserMenuFun> userMenuFunList = new List<UserMenuFun>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserMenuFun userMenuFun = new UserMenuFun()
                    {
                        UserID = new DBVarChar(dataRow[UserMenuFun.DataField.USER_ID.ToString()]),
                        MenuID = new DBVarChar(dataRow[UserMenuFun.DataField.MENU_ID.ToString()]),
                        FunMenu = new DBVarChar(dataRow[UserMenuFun.DataField.FUN_MENU.ToString()]),
                        FunMenuNM = new DBNVarChar(dataRow[UserMenuFun.DataField.FUN_MENU_NM.ToString()]),
                        SysID = new DBVarChar(dataRow[UserMenuFun.DataField.SYS_ID.ToString()]),
                        FunControllerID = new DBVarChar(dataRow[UserMenuFun.DataField.FUN_CONTROLLER_ID.ToString()]),
                        FunActionName = new DBVarChar(dataRow[UserMenuFun.DataField.FUN_ACTION_NAME.ToString()]),
                        FunNM = new DBNVarChar(dataRow[UserMenuFun.DataField.FUN_NM.ToString()]),
                        FunMenuXAxis = new DBVarChar(dataRow[UserMenuFun.DataField.FUN_MENU_XAXIS.ToString()]),
                    };
                    userMenuFunList.Add(userMenuFun);
                }
                return userMenuFunList;
            }
            return null;
        }

        public class UserMenu : DBTableRow
        {
            public enum Field
            {
                MenuUserID
            }
            public DBXmlNode Attribute;
            public List<MenuData> MenuDatas;
        }

        public class MenuData : DBTableRow
        {
            public enum Field
            {
                MenuID
            }
            public DBXmlNode Attribute;
            public List<MenuContent> MenuContents;
        }

        public class MenuContent : DBTableRow
        {
            public DBXmlNode MenuItemHeader;
            public List<MenuItem> MenuItems;
        }

        public class MenuItem : DBTableRow
        {
            public enum Field
            {
                xAxis, href
            }
            public DBXmlNode Attribute;
        }

        public enum EnumGenerateUserMenuXMLResult
        {
            Success, Failure,
            MeunIsEmpty
        }

        #endregion

        #region - CM_CODE -

        public class CMCodePara
        {
            public enum ParaField
            {
                CODE_NM, CODE_KIND, CODE_PARENT,
                CULTURE_ID
            }
            public DBVarChar CodeKind;
            public DBVarChar CodeParent;
            public DBVarChar CultureID;
            public DBChar HasCodeID;
        }

        public class CMCode : DBTableRow, ISelectItem
        {
            public CMCode()
            {

            }

            public CMCode(DBChar hasCodeID)
            {
                HasCodeID = hasCodeID;
            }

            public enum DataField
            {
                CODE_KIND, CODE_ID, CODE_NM, CODE_PARENT, IS_DISABLE, SORT_ORDER
            }

            public DBChar HasCodeID;
            public DBVarChar CodeKind;
            public DBVarChar CodeID;
            public DBNVarChar CodeNM;
            public DBVarChar CodeParent;
            public DBChar IsDisable;
            public DBNVarChar SortOrder;


            public string ItemText()
            {
                string itemText = string.Format("{0} {1}", this.CodeID.StringValue(), this.CodeNM.StringValue());
                if (HasCodeID != null && HasCodeID.GetValue() == EnumYN.N.ToString())
                {
                    itemText = this.CodeNM.StringValue();
                }
                return itemText;
            }

            public string ItemValue()
            {
                return this.CodeID.StringValue();
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

        public List<CMCode> SelectCMCodeList(CMCodePara para)
        {
            string commandWhere = string.Empty;
            if (string.IsNullOrWhiteSpace(para.CodeParent.GetValue()) == false)
            {
                commandWhere = "  AND CODE_PARENT={CODE_PARENT} " + Environment.NewLine;
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT CODE_KIND, CODE_ID ", Environment.NewLine,
                "     , dbo.FN_GET_CM_NM({CODE_KIND}, CODE_ID, {CULTURE_ID}) AS CODE_NM ", Environment.NewLine,
                "     , CODE_PARENT, IS_DISABLE, SORT_ORDER ", Environment.NewLine,
                "FROM CM_CODE ", Environment.NewLine,
                "WHERE CODE_KIND={CODE_KIND} ", Environment.NewLine,
                commandWhere,
                "ORDER BY SORT_ORDER "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.CODE_KIND.ToString(), Value = para.CodeKind });
            dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.CODE_PARENT.ToString(), Value = para.CodeParent });
            dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.CULTURE_ID, Value = para.CultureID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<CMCode> cmCodeList = new List<CMCode>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CMCode cmCode = new CMCode(para.HasCodeID)
                    {
                        CodeID = new DBVarChar(dataRow[CMCode.DataField.CODE_ID.ToString()]),
                        CodeNM = new DBNVarChar(dataRow[CMCode.DataField.CODE_NM.ToString()]),
                        CodeParent = new DBVarChar(dataRow[CMCode.DataField.CODE_PARENT.ToString()]),
                        IsDisable = new DBChar(dataRow[CMCode.DataField.IS_DISABLE.ToString()]),
                        SortOrder = new DBNVarChar(dataRow[CMCode.DataField.SORT_ORDER.ToString()])
                    };
                    cmCodeList.Add(cmCode);
                }
                return cmCodeList;
            }
            return null;
        }

        #endregion

        #region - Log -

        public class RecordUserSystemRolePara
        {
            public enum ParaField
            {
                USER_ID, UPD_USER_ID, EXEC_SYS_ID, EXEC_IP_ADDRESS
            }

            public DBVarChar UserID;
            public DBVarChar UpdUserID;
            public DBVarChar ExecSysID;
            public DBVarChar ExecIPAddress;
        }

        public enum EnumRecordUserSystemRoleResult
        {
            Success, Failure
        }

        public EnumRecordUserSystemRoleResult RecordUserSystemRole(RecordUserSystemRolePara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DECLARE @LOG_NO CHAR(6); ", Environment.NewLine,
                "        SELECT @LOG_NO=RIGHT('00000'+CAST(ISNULL(CAST(MAX(LOG_NO) AS INT),0)+1 AS VARCHAR),6) ", Environment.NewLine,
                "        FROM LOG_USER_SYSTEM_ROLE ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID}; ", Environment.NewLine,

                "        IF NOT EXISTS (SELECT * FROM SYS_USER_SYSTEM_ROLE WHERE USER_ID={USER_ID}) ", Environment.NewLine,
                "        BEGIN ", Environment.NewLine,
                "            INSERT INTO LOG_USER_SYSTEM_ROLE VALUES ( ", Environment.NewLine,
                "                {USER_ID}, @LOG_NO, 'NULL', 'NULL', {UPD_USER_ID}, GETDATE(), {EXEC_SYS_ID}, {EXEC_IP_ADDRESS} ", Environment.NewLine,
                "            ); ", Environment.NewLine,
                "        END; ", Environment.NewLine,
                "        ELSE ", Environment.NewLine,
                "        BEGIN ", Environment.NewLine,
                "            INSERT INTO LOG_USER_SYSTEM_ROLE ", Environment.NewLine,
                "            SELECT USER_ID, @LOG_NO, SYS_ID, ROLE_ID, {UPD_USER_ID}, GETDATE(), {EXEC_SYS_ID}, {EXEC_IP_ADDRESS} ", Environment.NewLine,
                "            FROM SYS_USER_SYSTEM_ROLE ", Environment.NewLine,
                "            WHERE USER_ID={USER_ID}; ", Environment.NewLine,
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
            dbParameters.Add(new DBParameter { Name = RecordUserSystemRolePara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = RecordUserSystemRolePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = RecordUserSystemRolePara.ParaField.EXEC_SYS_ID, Value = para.ExecSysID });
            dbParameters.Add(new DBParameter { Name = RecordUserSystemRolePara.ParaField.EXEC_IP_ADDRESS, Value = para.ExecIPAddress });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumRecordUserSystemRoleResult.Success : EnumRecordUserSystemRoleResult.Failure;
        }


        public class RecordUserFunctionPara
        {
            public enum ParaField
            {
                USER_ID, UPD_USER_ID, EXEC_SYS_ID, EXEC_IP_ADDRESS
            }

            public DBVarChar UserID;
            public DBVarChar UpdUserID;
            public DBVarChar ExecSysID;
            public DBVarChar ExecIPAddress;
        }

        public enum EnumRecordUserFunctionResult
        {
            Success, Failure
        }

        public EnumRecordUserFunctionResult RecordUserFunction(RecordUserFunctionPara para)
        {
            string commandText = string.Concat(new object[] 
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DECLARE @LOG_NO CHAR(6); ", Environment.NewLine,
                "        SELECT @LOG_NO=RIGHT('00000'+CAST(ISNULL(CAST(MAX(LOG_NO) AS INT),0)+1 AS VARCHAR),6) ", Environment.NewLine,
                "        FROM LOG_USER_FUN ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID}; ", Environment.NewLine,

                "        IF NOT EXISTS (SELECT * FROM SYS_USER_FUN WHERE USER_ID={USER_ID}) ", Environment.NewLine,
                "        BEGIN ", Environment.NewLine,
                "            INSERT INTO LOG_USER_FUN VALUES ( ", Environment.NewLine,
                "                {USER_ID}, @LOG_NO, 'NULL', 'NULL', 'NULL', {UPD_USER_ID}, GETDATE(), {EXEC_SYS_ID}, {EXEC_IP_ADDRESS} ", Environment.NewLine,
                "            ); ", Environment.NewLine,
                "        END; ", Environment.NewLine,
                "        ELSE ", Environment.NewLine,
                "        BEGIN ", Environment.NewLine,
                "            INSERT INTO LOG_USER_FUN ", Environment.NewLine,
                "            SELECT USER_ID, @LOG_NO, SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME, {UPD_USER_ID}, GETDATE(), {EXEC_SYS_ID}, {EXEC_IP_ADDRESS} ", Environment.NewLine,
                "            FROM SYS_USER_FUN ", Environment.NewLine,
                "            WHERE USER_ID={USER_ID}; ", Environment.NewLine,
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
            dbParameters.Add(new DBParameter { Name = RecordUserFunctionPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = RecordUserFunctionPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = RecordUserFunctionPara.ParaField.EXEC_SYS_ID, Value = para.ExecSysID });
            dbParameters.Add(new DBParameter { Name = RecordUserFunctionPara.ParaField.EXEC_IP_ADDRESS, Value = para.ExecIPAddress });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumRecordUserFunctionResult.Success : EnumRecordUserFunctionResult.Failure;
        }

        #endregion
    }
}