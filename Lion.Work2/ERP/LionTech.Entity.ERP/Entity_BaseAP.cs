using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using LionTech.Utility;

namespace LionTech.Entity.ERP
{
    public class Entity_BaseAP : DBEntity
    {   
#if !NET461
        public Entity_BaseAP(string connectionName)
            : base(connectionName)
        {
        }
#endif
        
        public Entity_BaseAP(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 取得自動完成使用者清單 -
        public class AutoUserInfoPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public class AutoUserInfo : DBTableRow
        {
            public DBVarChar UserID;
            public DBNVarChar UserNM;
        }

        public List<AutoUserInfo> SelectAutoUserInfoList(AutoUserInfoPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT TOP 20 USER_ID AS UserID",
                "     , USER_NM AS UserNM ",
                "  FROM SYS_USER_MAIN ",
                " WHERE IS_LEFT = '"+ EnumYN.N +"'",
                "   AND (USER_ID LIKE '%' + {USER_ID} + '%'",
                "    OR USER_NM LIKE '%' + {USER_ID} + '%')"
            });

            dbParameters.Add(new DBParameter { Name = AutoUserInfoPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            return GetEntityList<AutoUserInfo>(commandText, dbParameters);
        }
        #endregion

        #region - EDIXMLRule -
        public class SystemEDIFlowDetail : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, EDI_FLOW_ID,
                EDI_FLOW_NM, SCH_FREQUENCY, SCH_START_DATE, SCH_START_TIME, SCH_INTERVAL_NUM, SCH_WEEKS, SCH_DAYS_STR, SCH_DATA_DELAY, SCH_KEEP_LOG_DAY,
                PATHS_CMD, PATHS_DAT, PATHS_SRC, PATHS_RES, PATHS_BAD, PATHS_LOG,
                PATHS_FLOW_XML, PATHS_FLOW_CMD, PATHS_ZIP_DAT, PATHS_EXCEPTION, PATHS_SUMMARY, SORT_ORDER
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
            public DBNVarChar EDIFlowNM;
            public DBVarChar SCHFrequency;
            public DBChar SCHStartDate;
            public DBChar SCHStartTime;
            public DBInt SCHIntervalNum;
            public DBInt SCHWeeks;
            public DBVarChar SCHDaysStr;
            public DBInt SCHDataDelay;
            public DBInt SCHKeepLogDay;
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
                IS_USE_RES, FILE_SOURCE, FILE_ENCODING, URL_PATH, IGNORE_WARNING, IS_DISABLE, SORT_ORDER
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
            public DBChar IgnoreWarning;
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
            public DBXmlNode numDelay;
            public DBXmlNode keepLogDay;
            public List<fixedTime> fixedTimes;
            public List<fixedWeekly> fixedWeeklys;
            public List<fixedDay> fixedDays;
            public DBXmlNode dataDelay;
        }

        public class fixedCycle : DBTableRow
        {
            public enum Field
            {
                value
            }

            public DBXmlNode Attribute;
        }

        public class fixedTime : fixedCycle
        {
        }

        public class fixedWeekly : fixedCycle
        {
        }

        public class fixedDay : fixedCycle
        {
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
                ignoreWarning,
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

            DataTable dataTable = GetDataTable(commandText, dbParameters);
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

        public enum EnumCMCodeItemTextType
        {
            CodeNM,
            CodeNMID
        }

        public enum EnumCMCodeKind
        {
            [Description("0000")]
            Culture,

            [Description("0001")]
            SystemService,

            [Description("0002")]
            EDIStatusCode,

            [Description("0003")]
            EDIResultStatusCode,

            [Description("0009")]
            DomainName,

            [Description("0011")]
            SystemFunType,

            [Description("0013")]
            APIReturnType,

            [Description("0015")]
            OrgArea,

            [Description("0016")]
            OrgGroup,

            [Description("0017")]
            OrgPlace,

            [Description("0018")]
            OrgDept,

            [Description("0019")]
            OrgTeam,

            [Description("0020")]
            OrgPTitle,

            [Description("0021")]
            OrgPTitle2,

            [Description("0022")]
            OrgLevel,

            [Description("0023")]
            OrgTitle,

            [Description("0024")]
            OrgProperty,

            [Description("0026")]
            WorkFlowType,

            [Description("0028")]
            WorkFlowResultType,

            [Description("0029")]
            SignatureResultType,

            [Description("0030")]
            SignatureUserType,

            [Description("0031")]
            IPTrustType,

            [Description("0032")]
            IPSourceType,

            [Description("0033")]
            RestrictType,

            [Description("0034")]
            ModifyType,

            [Description("0035")]
            RecordType,

            [Description("0036")]
            UserLocation,

            [Description("0037")]
            SignatureID,

            [Description("0038")]
            Work,

            [Description("0039")]
            Title,

            [Description("0040")]
            LionCountryCode,

            [Description("0041")]
            PurviewType,

            [Description("0042")]
            PurviewOperationType,

            [Description("0043")]
            LineReceiverSourceType,

            [Description("0044")]
            IOSType,

            [Description("0045")]
            ElmDisplayType
        }

        public enum EnumPurviewCodeType
        {
            COMPANY,
            COUNTRY,
            UNIT
        }

        public enum EnumLineSourceType
        {
            USER,
            ROOM,
            GROUP
        }

        public class CMCodePara
        {
            public enum ParaField
            {
                CODE_KIND, CODE_PARENT,
                CULTURE_ID,
                CODE_ID
            }

            public EnumCMCodeItemTextType ItemTextType;
            public EnumCMCodeKind CodeKind;
            public DBVarChar CodeParent;
            public DBVarChar CultureID;
            public List<DBVarChar> CodeIDs;
        }

        public class CMCode : DBTableRow, IExtendedSelectItem
        {
            public CMCode()
            {
                _itemTextType = EnumCMCodeItemTextType.CodeNM;
            }

            public CMCode(EnumCMCodeItemTextType itemTextType)
            {
                // 動態注入
                _itemTextType = itemTextType;
            }

            private readonly EnumCMCodeItemTextType _itemTextType;

            public DBVarChar CodeKind;
            public DBVarChar CodeID;
            public DBNVarChar CodeNM;
            public DBVarChar CodeParent;
            public DBChar IsDisable;
            public DBNVarChar SortOrder;

            public string ItemText()
            {
                string itemText;

                switch (_itemTextType)
                {
                    case EnumCMCodeItemTextType.CodeNM:
                        itemText = CodeNM.StringValue();
                        break;
                    case EnumCMCodeItemTextType.CodeNMID:
                        itemText = string.Format("{0} ({1})", CodeNM.StringValue(), CodeID.StringValue());
                        break;
                    default:
                        itemText = string.Format("{0} {1}", CodeID.StringValue(), CodeNM.StringValue());
                        break;
                }

                return itemText;
            }

            public string ItemValue()
            {
                return CodeID.GetValue();
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

            public string Background()
            {
                throw new NotImplementedException();
            }
        }

        public List<CMCode> SelectCMCodeList(CMCodePara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            List<string> commandWhereList = new List<string>();

            if (para.CodeParent != null &&
                para.CodeParent.IsNull() == false)
            {
                commandWhereList.Add(" CODE_PARENT = {CODE_PARENT} ");
                dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.CODE_PARENT.ToString(), Value = para.CodeParent });
            }

            if (para.CodeIDs != null &&
                para.CodeIDs.Any())
            {
                commandWhereList.Add(" CODE_ID IN ({CODE_ID}) ");
                dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.CODE_ID, Value = para.CodeIDs });
            }

            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT CODE_ID AS CodeID ",
                        "     , dbo.FN_GET_CM_NM({CODE_KIND}, CODE_ID, {CULTURE_ID}) AS CodeNM ",
                        "     , CODE_PARENT AS CodeParent ",
                        "     , IS_DISABLE AS IsDisable ",
                        "     , SORT_ORDER AS SortOrder ",
                        "  FROM CM_CODE ",
                        " WHERE CODE_KIND = {CODE_KIND} ",
                        commandWhereList.Any()
                            ? string.Format(" AND {0}", string.Join(" AND ", commandWhereList))
                            : string.Empty,
                        " ORDER BY SORT_ORDER "
                    });

            dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.CODE_KIND.ToString(), Value = new DBChar(Common.GetEnumDesc(para.CodeKind)) });
            dbParameters.Add(new DBParameter { Name = CMCodePara.ParaField.CULTURE_ID, Value = para.CultureID });
            return GetEntityList<CMCode>(commandText, dbParameters, trArgs: new object[] { para.ItemTextType });
        }

        #endregion

        #region - RAW_CM -

        public class RawCMUserPara
        {
            public enum ParaField
            {
                USER_ID
            }
            public List<DBVarChar> UserIDList;
        }

        public class RawCMUser : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                USER_ID, USER_NM
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBNVarChar UserIDNM;

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

        public List<RawCMUser> SelectRawCMUserList(RawCMUserPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT USER_ID AS UserID, USER_NM AS UserNM,dbo.FN_GET_IDNM(USER_ID, USER_NM) AS UserIDNM ", Environment.NewLine,
                "FROM RAW_CM_USER ", Environment.NewLine,
                "WHERE USER_ID IN ({USER_ID}) ", Environment.NewLine,
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = RawCMUserPara.ParaField.USER_ID.ToString(), Value = para.UserIDList });
            return GetEntityList<RawCMUser>(commandText, dbParameters);
        }

        public class RawCMOrgComPara
        {
            public enum ParaField
            {

            }
        }

        public class RawCMOrgCom : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                COM_ID, COM_NM, IS_SALARY_COM
            }

            public DBVarChar ComID;
            public DBNVarChar ComNM;
            public DBChar IsSalaryCOM;

            public string ItemText()
            {
                return string.Format("{0} ({1})", ComNM.StringValue(), ComID.StringValue());
            }

            public string ItemValue()
            {
                return ComID.StringValue();
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

        public List<RawCMOrgCom> SelectRawCMOrgComList(RawCMOrgComPara para)
        {
            string commandText = string.Concat(new object[]{
                "SELECT COM_ID, COM_NM, IS_SALARY_COM", Environment.NewLine,
                "FROM RAW_CM_ORG_COM ", Environment.NewLine,
                "ORDER BY SORT_ORDER "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();

            DataTable dataTable = GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<RawCMOrgCom> rawCMOrgComList = new List<RawCMOrgCom>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    RawCMOrgCom rawCMOrgCom = new RawCMOrgCom()
                    {
                        ComID = new DBVarChar(dataRow[RawCMOrgCom.DataField.COM_ID.ToString()]),
                        ComNM = new DBNVarChar(dataRow[RawCMOrgCom.DataField.COM_NM.ToString()]),
                        IsSalaryCOM = new DBChar(dataRow[RawCMOrgCom.DataField.IS_SALARY_COM.ToString()])
                    };
                    rawCMOrgComList.Add(rawCMOrgCom);
                }
                return rawCMOrgComList;
            }
            return null;
        }

        #region - 查詢組織-單位 -
        public class RawCMOrgUnitPara
        {
        }

        public class RawCMOrgUnit : DBTableRow, ISelectItem
        {
            public DBVarChar UnitID;
            public DBNVarChar UnitNM;

            public string ItemText()
            {
                return string.Format("{0} ({1})", UnitNM.StringValue(), UnitID.StringValue());
            }

            public string ItemValue()
            {
                return UnitID.StringValue();
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

        /// <summary>
        /// 查詢組織-單位
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<RawCMOrgUnit> SelectRawCMOrgUnitList(RawCMOrgUnitPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT UNIT_ID AS UnitID",
                    "     , UNIT_NM AS UnitNM",
                    "  FROM RAW_CM_ORG_UNIT",
                    " WHERE IS_CEASED = '" + EnumYN.N + "';"
                }));

            return GetEntityList<RawCMOrgUnit>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 查詢ERP-集團 -
        public class RawCMBusinessUnitPara : DBCulture
        {
            public RawCMBusinessUnitPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                BU_NM
            }
        }

        public class RawCMBusinessUnit : DBTableRow, ISelectItem
        {
            public DBChar BuID;
            public DBNVarChar BuNM;

            public string ItemText()
            {
                return string.Format("{0} ({1})", BuNM.StringValue(), BuID.StringValue());
            }

            public string ItemValue()
            {
                return BuID.StringValue();
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

        /// <summary>
        /// 查詢ERP-集團
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<RawCMBusinessUnit> SelectRawCMBusinessUnitList(RawCMBusinessUnitPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT BU_ID AS BuID",
                    "     , {BU_NM} AS BuNM",
                    "  FROM RAW_CM_BUSINESS_UNIT",
                    " ORDER BY SORT_ORDER"
                }));

            dbParameters.Add(new DBParameter { Name = RawCMBusinessUnitPara.ParaField.BU_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(RawCMBusinessUnitPara.ParaField.BU_NM.ToString())) });

            return GetEntityList<RawCMBusinessUnit>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 查詢ERP-國家 和 薪資-國家 -
        public class RawCMCountryPara : DBCulture
        {
            public RawCMCountryPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                COUNTRY_NM
            }
        }

        public class RawCMCountry : DBTableRow, ISelectItem
        {
            public DBChar CountryID;
            public DBNVarChar CountryNM;
            public DBChar IsSalaryCOM;

            public string ItemText()
            {
                return string.Format("{0} ({1})", CountryNM.StringValue(), CountryID.StringValue());
            }

            public string ItemValue()
            {
                return CountryID.StringValue();
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

        /// <summary>
        /// 查詢ERP-國家 和 薪資-國家
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<RawCMCountry> SelectRawCMCountryList(RawCMCountryPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT DISTINCT C.COM_COUNTRY AS CountryID",
                    "     , {COUNTRY_NM} AS CountryNM",
                    "     , C.IS_SALARY_COM AS IsSalaryCOM",
                    "  FROM RAW_CM_ORG_COM C",
                    "  JOIN RAW_CM_COUNTRY Y",
                    "    ON C.COM_COUNTRY = Y.COUNTRY_ID",
                    " WHERE C.IS_CEASED = '" + EnumYN.N + "'"
                }));

            dbParameters.Add(new DBParameter { Name = RawCMCountryPara.ParaField.COUNTRY_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(RawCMCountryPara.ParaField.COUNTRY_NM.ToString())) });

            return GetEntityList<RawCMCountry>(commandText.ToString(), dbParameters);
        }
        #endregion

        #endregion

        #region - UserMessage -

        public class UserMessageAPIPara : DBTableRow
        {
            public DBVarChar UserID;
        }

        public class UserMessageItem : DBTableRow
        {
            public string UserMessage { get; set; }
        }

        public class UserMessageCollection : DBTableRow
        {
            public IEnumerable<UserMessageItem> UserMessages { get; set; }
        }

        #endregion

        #region - UserSystemNotifications -

        public class UserSystemNotificationAPIPara : DBTableRow
        {
            public DBVarChar UserID;
            public DBInt DataIndex;
        }

        public class UserSystemNotificationItem : DBTableRow
        {
            public string NoticeDateTime { get; set; }
            public string Content { get; set; }
            public string URL { get; set; }
        }
        #endregion

        #region - Log -

        public class BasicInfoPara : DBCulture
        {
            public BasicInfoPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                USER_ID, UPD_USER_ID, EXEC_SYS_ID,

                SYS_NM
            }

            public DBVarChar UserID;
            public DBVarChar UpdUserID;
            public DBVarChar ExecSysID;
        }

        public class BasicInfo
        {
            public enum DataField
            {
                USER_ID, USER_NM,
                UPD_USER_ID, UPD_USER_NM,
                EXEC_SYS_ID, EXEC_SYS_NM
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
        }

        public BasicInfo SelectBasicInfo(BasicInfoPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @USER_NM NVARCHAR(150); ", Environment.NewLine,
                "SET @USER_NM={USER_ID}; ", Environment.NewLine,
                "IF EXISTS (SELECT USER_NM FROM RAW_CM_USER WHERE USER_ID={USER_ID}) ", Environment.NewLine,
                "BEGIN ", Environment.NewLine,
                "    SELECT @USER_NM=USER_NM ", Environment.NewLine,
                "    FROM RAW_CM_USER ", Environment.NewLine,
                "    WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                "END; ", Environment.NewLine,

                "DECLARE @UPD_USER_NM NVARCHAR(150); ", Environment.NewLine,
                "SET @UPD_USER_NM={UPD_USER_ID}; ", Environment.NewLine,
                "IF EXISTS (SELECT USER_NM FROM RAW_CM_USER WHERE USER_ID={UPD_USER_ID}) ", Environment.NewLine,
                "BEGIN ", Environment.NewLine,
                "    SELECT @UPD_USER_NM=USER_NM ", Environment.NewLine,
                "    FROM RAW_CM_USER ", Environment.NewLine,
                "    WHERE USER_ID={UPD_USER_ID}; ", Environment.NewLine,
                "END; ", Environment.NewLine,

                "DECLARE @EXEC_SYS_NM NVARCHAR(150); ", Environment.NewLine,
                "SET @EXEC_SYS_NM={EXEC_SYS_ID}; ", Environment.NewLine,
                "IF EXISTS (SELECT SYS_NM_ZH_TW FROM SYS_SYSTEM_MAIN WHERE SYS_ID={EXEC_SYS_ID}) ", Environment.NewLine,
                "BEGIN ", Environment.NewLine,
                "    SELECT @EXEC_SYS_NM={SYS_NM} ", Environment.NewLine,
                "    FROM SYS_SYSTEM_MAIN ", Environment.NewLine,
                "    WHERE SYS_ID={EXEC_SYS_ID}; ", Environment.NewLine,
                "END; ", Environment.NewLine,

                "SELECT {USER_ID} AS USER_ID, @USER_NM AS USER_NM ", Environment.NewLine,
                "     , {UPD_USER_ID} AS UPD_USER_ID, @UPD_USER_NM AS UPD_USER_NM ", Environment.NewLine,
                "     , {EXEC_SYS_ID} AS EXEC_SYS_ID, @EXEC_SYS_NM AS EXEC_SYS_NM; ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = BasicInfoPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = BasicInfoPara.ParaField.UPD_USER_ID.ToString(), Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = BasicInfoPara.ParaField.EXEC_SYS_ID.ToString(), Value = para.ExecSysID });
            dbParameters.Add(new DBParameter { Name = BasicInfoPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(BasicInfoPara.ParaField.SYS_NM.ToString())) });

            DataTable dataTable = GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                BasicInfo basicInfo = new BasicInfo()
                {
                    UserID = new DBVarChar(dataTable.Rows[0][BasicInfo.DataField.USER_ID.ToString()]),
                    UserNM = new DBNVarChar(dataTable.Rows[0][BasicInfo.DataField.USER_NM.ToString()]),
                    UpdUserID = new DBVarChar(dataTable.Rows[0][BasicInfo.DataField.UPD_USER_ID.ToString()]),
                    UpdUserNM = new DBNVarChar(dataTable.Rows[0][BasicInfo.DataField.UPD_USER_NM.ToString()]),
                    ExecSysID = new DBVarChar(dataTable.Rows[0][BasicInfo.DataField.EXEC_SYS_ID.ToString()]),
                    ExecSysNM = new DBNVarChar(dataTable.Rows[0][BasicInfo.DataField.EXEC_SYS_NM.ToString()])
                };
                return basicInfo;
            }
            return null;
        }

        /*Sys Log*/
        public class SysSystemRoleGroupCollectPara : DBCulture
        {
            public SysSystemRoleGroupCollectPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                ROLE_GROUP_ID, UPD_USER_ID,

                SYS_NM, ROLE_NM
            }

            public DBVarChar RoleGroupID;
            public DBVarChar UpdUserID;
        }

        public class SysSystemRoleGroupCollect : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM,
                ROLE_ID, ROLE_NM
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
        }

        public List<SysSystemRoleGroupCollect> SelectSysSystemRoleGroupCollectList(SysSystemRoleGroupCollectPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT C.SYS_ID, M.{SYS_NM} AS SYS_NM ", Environment.NewLine,
                "     , C.ROLE_ID, R.{ROLE_NM} AS ROLE_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_ROLE_GROUP_COLLECT C ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_MAIN M ON C.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_ROLE R ON C.SYS_ID=R.SYS_ID AND C.ROLE_ID=R.ROLE_ID ", Environment.NewLine,
                "WHERE C.ROLE_GROUP_ID={ROLE_GROUP_ID} ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupCollectPara.ParaField.ROLE_GROUP_ID.ToString(), Value = para.RoleGroupID });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupCollectPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemRoleGroupCollectPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleGroupCollectPara.ParaField.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemRoleGroupCollectPara.ParaField.ROLE_NM.ToString())) });

            DataTable dataTable = GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemRoleGroupCollect> sysSystemRoleGroupCollectList = new List<SysSystemRoleGroupCollect>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemRoleGroupCollect sysSystemRoleGroupCollect = new SysSystemRoleGroupCollect()
                    {
                        SysID = new DBVarChar(dataRow[SysSystemRoleGroupCollect.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SysSystemRoleGroupCollect.DataField.SYS_NM.ToString()]),
                        RoleID = new DBVarChar(dataRow[SysSystemRoleGroupCollect.DataField.ROLE_ID.ToString()]),
                        RoleNM = new DBNVarChar(dataRow[SysSystemRoleGroupCollect.DataField.ROLE_NM.ToString()])
                    };
                    sysSystemRoleGroupCollectList.Add(sysSystemRoleGroupCollect);
                }
                return sysSystemRoleGroupCollectList;
            }
            return null;
        }


        public class SysSystemRoleFunPara : DBCulture
        {
            public SysSystemRoleFunPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME,

                SYS_NM, ROLE_NM, FUN_GROUP_NM, FUN_GROUP, FUN_NM
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionNM;
        }

        public class SysSystemRoleFun : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM,
                ROLE_ID, ROLE_NM,
                FUN_CONTROLLER_ID, FUN_GROUP_NM,
                FUN_ACTION_NAME, FUN_NM
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
            public DBVarChar FunControllerID;
            public DBNVarChar FunGroupNM;
            public DBVarChar FunActionNM;
            public DBNVarChar FunNM;
        }

        public List<SysSystemRoleFun> SelectSysSystemRoleFunList(SysSystemRoleFunPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT F.SYS_ID, M.{SYS_NM} AS SYS_NM ", Environment.NewLine,
                "     , Z.ROLE_ID, R.{ROLE_NM} AS ROLE_NM ", Environment.NewLine,
                "     , F.FUN_CONTROLLER_ID, G.{FUN_GROUP_NM} AS FUN_GROUP_NM ", Environment.NewLine,
                "     , F.FUN_ACTION_NAME, F.{FUN_NM} AS FUN_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN F ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_MAIN M ON F.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_FUN_GROUP G ON F.SYS_ID=G.SYS_ID AND F.FUN_CONTROLLER_ID=G.FUN_CONTROLLER_ID ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_ROLE_FUN Z ON F.SYS_ID=Z.SYS_ID AND F.FUN_CONTROLLER_ID=Z.FUN_CONTROLLER_ID AND F.FUN_ACTION_NAME=Z.FUN_ACTION_NAME ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_ROLE R ON Z.SYS_ID=R.SYS_ID AND Z.ROLE_ID=R.ROLE_ID ", Environment.NewLine,
                "WHERE F.SYS_ID={SYS_ID} AND F.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND F.FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                "ORDER BY Z.ROLE_ID ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemRoleFunPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleFunPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleFunPara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionNM });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleFunPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemRoleFunPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleFunPara.ParaField.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemRoleFunPara.ParaField.ROLE_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleFunPara.ParaField.FUN_GROUP_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemRoleFunPara.ParaField.FUN_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleFunPara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemRoleFunPara.ParaField.FUN_NM.ToString())) });

            DataTable dataTable = GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemRoleFun> sysSystemRoleFunList = new List<SysSystemRoleFun>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemRoleFun sysSystemRoleFun = new SysSystemRoleFun()
                    {
                        SysID = new DBVarChar(dataRow[SysSystemRoleFun.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SysSystemRoleFun.DataField.SYS_NM.ToString()]),
                        RoleID = new DBVarChar(dataRow[SysSystemRoleFun.DataField.ROLE_ID.ToString()]),
                        RoleNM = new DBNVarChar(dataRow[SysSystemRoleFun.DataField.ROLE_NM.ToString()]),
                        FunControllerID = new DBVarChar(dataRow[SysSystemRoleFun.DataField.FUN_CONTROLLER_ID.ToString()]),
                        FunGroupNM = new DBNVarChar(dataRow[SysSystemRoleFun.DataField.FUN_GROUP_NM.ToString()]),
                        FunActionNM = new DBVarChar(dataRow[SysSystemRoleFun.DataField.FUN_ACTION_NAME.ToString()]),
                        FunNM = new DBNVarChar(dataRow[SysSystemRoleFun.DataField.FUN_NM.ToString()])
                    };
                    sysSystemRoleFunList.Add(sysSystemRoleFun);
                }
                return sysSystemRoleFunList;
            }
            return null;
        }


        /*User Log*/
        public class UserSystemRolePara : DBCulture
        {
            public UserSystemRolePara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                USER_ID,

                ROLE_GROUP_NM, SYS_NM, ROLE_NM
            }

            public DBVarChar UserID;
        }

        public class UserSystemRole : DBTableRow
        {
            public enum DataField
            {
                USER_ID, USER_NM,
                ROLE_GROUP_ID, ROLE_GROUP_NM,
                SYS_ID, SYS_NM,
                ROLE_ID, ROLE_NM
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar RoleGroupID;
            public DBNVarChar RoleGroupNM;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
        }

        public List<UserSystemRole> SelectUserSystemRoleList(UserSystemRolePara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT U.USER_ID, S.USER_NM ", Environment.NewLine,
                "     , U.ROLE_GROUP_ID, G.{ROLE_GROUP_NM} AS ROLE_GROUP_NM ", Environment.NewLine,
                "     , E.SYS_ID, M.{SYS_NM} AS SYS_NM ", Environment.NewLine,
                "     , E.ROLE_ID, R.{ROLE_NM} AS ROLE_NM ", Environment.NewLine,
                "FROM SYS_USER_MAIN U ", Environment.NewLine,
                "LEFT JOIN SYS_USER_SYSTEM_ROLE E ON U.USER_ID=E.USER_ID ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_MAIN M ON E.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_ROLE_GROUP G ON U.ROLE_GROUP_ID=G.ROLE_GROUP_ID ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_ROLE R ON E.SYS_ID=R.SYS_ID AND E.ROLE_ID=R.ROLE_ID ", Environment.NewLine,
                "LEFT JOIN RAW_CM_USER S ON U.USER_ID=S.USER_ID ", Environment.NewLine,
                "WHERE U.USER_ID={USER_ID} ", Environment.NewLine,
                "ORDER BY M.SORT_ORDER, R.ROLE_ID ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.ROLE_GROUP_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserSystemRolePara.ParaField.ROLE_GROUP_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserSystemRolePara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = UserSystemRolePara.ParaField.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserSystemRolePara.ParaField.ROLE_NM.ToString())) });

            DataTable dataTable = GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserSystemRole> userSystemRoleList = new List<UserSystemRole>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserSystemRole userSystemRole = new UserSystemRole()
                    {
                        UserID = new DBVarChar(dataRow[UserSystemRole.DataField.USER_ID.ToString()]),
                        UserNM = new DBNVarChar(dataRow[UserSystemRole.DataField.USER_NM.ToString()]),
                        RoleGroupID = new DBVarChar(dataRow[UserSystemRole.DataField.ROLE_GROUP_ID.ToString()]),
                        RoleGroupNM = new DBNVarChar(dataRow[UserSystemRole.DataField.ROLE_GROUP_NM.ToString()]),
                        SysID = new DBVarChar(dataRow[UserSystemRole.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[UserSystemRole.DataField.SYS_NM.ToString()]),
                        RoleID = new DBVarChar(dataRow[UserSystemRole.DataField.ROLE_ID.ToString()]),
                        RoleNM = new DBNVarChar(dataRow[UserSystemRole.DataField.ROLE_NM.ToString()])
                    };
                    userSystemRoleList.Add(userSystemRole);
                }
                return userSystemRoleList;
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
                USER_ID,

                SYS_NM, FUN_GROUP, FUN_NM
            }

            public DBVarChar UserID;
        }

        public class UserFunction : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM,
                FUN_CONTROLLER_ID, FUN_GROUP_NM,
                FUN_ACTION_NAME, FUN_NM
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar FunControllerID;
            public DBNVarChar FunGroupNM;
            public DBVarChar FunActionNM;
            public DBNVarChar FunNM;
        }

        public List<UserFunction> SelectUserFunctionList(UserFunctionPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT U.SYS_ID, M.{SYS_NM} AS SYS_NM ", Environment.NewLine,
                "     , U.FUN_CONTROLLER_ID, G.{FUN_GROUP} AS FUN_GROUP_NM ", Environment.NewLine,
                "     , U.FUN_ACTION_NAME, F.{FUN_NM} AS FUN_NM ", Environment.NewLine,
                "FROM SYS_USER_FUN U ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_MAIN M ON U.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_FUN_GROUP G ON U.SYS_ID=G.SYS_ID AND U.FUN_CONTROLLER_ID=G.FUN_CONTROLLER_ID ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_FUN F ON U.SYS_ID=F.SYS_ID AND U.FUN_CONTROLLER_ID=F.FUN_CONTROLLER_ID AND U.FUN_ACTION_NAME=F.FUN_ACTION_NAME ", Environment.NewLine,
                "WHERE U.USER_ID={USER_ID} ", Environment.NewLine,
                "ORDER BY M.SORT_ORDER, G.SORT_ORDER, F.SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserFunctionPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserFunctionPara.ParaField.FUN_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = UserFunctionPara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(UserFunctionPara.ParaField.FUN_NM.ToString())) });

            DataTable dataTable = GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserFunction> userFunctionList = new List<UserFunction>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserFunction userFunction = new UserFunction()
                    {
                        SysID = new DBVarChar(dataRow[UserFunction.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[UserFunction.DataField.SYS_NM.ToString()]),
                        FunControllerID = new DBVarChar(dataRow[UserFunction.DataField.FUN_CONTROLLER_ID.ToString()]),
                        FunGroupNM = new DBNVarChar(dataRow[UserFunction.DataField.FUN_GROUP_NM.ToString()]),
                        FunActionNM = new DBVarChar(dataRow[UserFunction.DataField.FUN_ACTION_NAME.ToString()]),
                        FunNM = new DBNVarChar(dataRow[UserFunction.DataField.FUN_NM.ToString()])
                    };
                    userFunctionList.Add(userFunction);
                }
                return userFunctionList;
            }
            return null;
        }

        #endregion

        #region - Valid -

        public class UserAccountPara
        {
            public enum ParaField
            {
                USER_ID, USER_PWD
            }

            public DBVarChar UserID;
            public DBVarChar UserPWD;
        }

        public bool ValidUserAccount(UserAccountPara para)
        {
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT stfn_pswd AS USER_PWD ",
                "FROM opagm20 ",
                "WHERE stfn_stfn={USER_ID} "
            });

            //string commandText = string.Concat(new object[]
            //{
            //    "SELECT (CASE WHEN COUNT(1) > 0 THEN 'Y' ELSE 'N' END) AS USER_CNT ", Environment.NewLine,
            //    "FROM SYS_USER_MAIN ", Environment.NewLine,
            //    "WHERE USER_ID={USER_ID} ", Environment.NewLine,
            //    "  AND USER_PWD={USER_PWD}; "
            //});

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_PWD.ToString(), Value = para.UserPWD });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
            return para.UserPWD.IsNull() == false && result.GetValue() == para.UserPWD.GetValue();
        }
        #endregion

        #region - 查詢工作流程紀錄檔 -
        public class LogWFFlowPara
        {
            public enum ParaField
            {
                WF_NO
            }

            public DBChar WFNo;
        }

        public class LogWFFlow : DBTableRow
        {
            public DBChar LOG_NO;

            public DBChar WF_NO;
            public DBVarChar SYS_ID;
            public DBVarChar WF_FLOW_ID;
            public DBChar WF_FLOW_VER;
            public DBNVarChar WF_SUBJECT;
            public DBNVarChar LOT;
            public DBVarChar NEW_USER_ID;
            public DBVarChar END_USER_ID;
            public DBChar DT_BEGIN;
            public DBChar DT_END;
            public DBVarChar RESULT_ID;
            public DBChar NODE_NO;

            public List<LogWFNode> LOG_WF_NODE_LIST;

            public DBVarChar EXEC_ACTION;
            public DBVarChar FUN_CONTROLLER_ID;
            public DBVarChar FUN_ACTION_NAME;

            public DBChar API_NO;
            public DBVarChar UPD_USER_ID;
            public DBDateTime UPD_DT;

            public DBVarChar CLIENT_SYS_ID;
            public DBVarChar CLIENT_IP_ADDRESS;

            public DBVarChar EXEC_SYS_ID;
            public DBVarChar EXEC_IP_ADDRESS;
        }

        public class LogWFNode : DBTableRow
        {
            public DBChar WF_NO;
            public DBChar NODE_NO;
            public DBVarChar SYS_ID;
            public DBVarChar WF_FLOW_ID;
            public DBChar WF_FLOW_VER;
            public DBVarChar WF_NODE_ID;
            public DBVarChar NEW_USER_ID;
            public DBVarChar NEW_USER_ID_STR;
            public DBNVarChar NEW_USER_NM_STR;
            public DBVarChar END_USER_ID;
            public DBChar DT_BEGIN;
            public DBChar DT_END;
            public DBVarChar RESULT_ID;
            public DBVarChar RESULT_VALUE;
            public DBChar IS_START_SIG;
            public DBInt SIG_STEP;
            public DBVarChar SIG_RESULT_ID;
            public DBVarChar SIG_USER_ID_STR;
            public DBNVarChar SIG_USER_NM_STR;
            public DBVarChar UPD_USER_ID;
            public DBDateTime UPD_DT;

            public List<LogWFSig> LOG_WF_SIG_LIST;
            public List<LogWFDoc> LOG_WF_DOC_LIST;
            public List<LogWFRemark> LOG_WF_REMARK_LIST;
            public List<LogWFNodeNewUser> LOG_WF_NODE_NEW_USER_LIST;
        }

        public class LogWFSig : DBTableRow
        {
            public DBChar WF_NO;
            public DBChar NODE_NO;
            public DBVarChar SYS_ID;
            public DBVarChar WF_FLOW_ID;
            public DBChar WF_FLOW_VER;
            public DBVarChar WF_NODE_ID;
            public DBVarChar SIG_KIND;
            public DBInt SIG_STEP;
            public DBChar WF_SIG_SEQ;
            public DBNVarChar WF_SIG_ZH_TW;
            public DBNVarChar WF_SIG_ZH_CN;
            public DBNVarChar WF_SIG_EN_US;
            public DBNVarChar WF_SIG_TH_TH;
            public DBNVarChar WF_SIG_JA_JP;
            public DBNVarChar WF_SIG_KO_KR;
            public DBVarChar SIG_USER_ID;
            public DBChar SIG_DATE;
            public DBVarChar SIG_RESULT_ID;
            public DBNVarChar SIG_COMMENT;
            public DBVarChar UPD_USER_ID;
            public DBDateTime UPD_DT;
        }

        public class LogWFDoc : DBTableRow
        {
            public DBChar WF_NO;
            public DBChar NODE_NO;
            public DBChar DOC_NO;
            public DBVarChar SYS_ID;
            public DBVarChar WF_FLOW_ID;
            public DBChar WF_FLOW_VER;
            public DBVarChar WF_NODE_ID;
            public DBChar WF_DOC_SEQ;
            public DBNVarChar WF_DOC_ZH_TW;
            public DBNVarChar WF_DOC_ZH_CN;
            public DBNVarChar WF_DOC_EN_US;
            public DBNVarChar WF_DOC_TH_TH;
            public DBNVarChar WF_DOC_JA_JP;
            public DBNVarChar WF_DOC_KO_KR;
            public DBChar IS_REQ;
            public DBVarChar DOC_USER_ID;
            public DBNVarChar DOC_FILE_NAME;
            public DBVarChar DOC_ENCODE_NAME;
            public DBChar DOC_DATE;
            public DBNVarChar DOC_PATH;
            public DBNVarChar DOC_LOCAL_PATH;
            public DBChar IS_DELETE;
            public DBVarChar UPD_USER_ID;
            public DBDateTime UPD_DT;
        }

        public class LogWFRemark : DBTableRow
        {
            public DBChar WF_NO;
            public DBChar NODE_NO;
            public DBChar REMARK_NO;
            public DBVarChar SYS_ID;
            public DBVarChar WF_FLOW_ID;
            public DBChar WF_FLOW_VER;
            public DBVarChar WF_NODE_ID;
            public DBVarChar NODE_RESULT_ID;
            public DBVarChar NODE_NEW_USER_ID;
            public DBVarChar BACK_WF_NODE_ID;
            public DBInt SIG_STEP;
            public DBChar WF_SIG_SEQ;
            public DBChar SIG_DATE;
            public DBVarChar SIG_RESULT_ID;
            public DBChar DOC_NO;
            public DBChar WF_DOC_SEQ;
            public DBChar DOC_DATE;
            public DBChar DOC_IS_DELETE;
            public DBVarChar REMARK_USER_ID;
            public DBChar REMARK_DATE;
            public DBNVarChar REMARK;
            public DBVarChar UPD_USER_ID;
            public DBDateTime UPD_DT;
        }

        public class LogWFNodeNewUser : DBTableRow
        {
            public DBChar WF_NO;
            public DBChar NODE_NO;
            public DBVarChar SYS_ID;
            public DBVarChar WF_FLOW_ID;
            public DBChar WF_FLOW_VER;
            public DBVarChar WF_NODE_ID;
            public DBVarChar NEW_USER_ID;
            public DBVarChar UPD_USER_ID;
            public DBDateTime UPD_DT;
        }

        /// <summary>
        /// 查詢工作流程紀錄檔
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public LogWFFlow SelectLogWFFlow(LogWFFlowPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT WF_NO",
                    "     , SYS_ID",
                    "     , WF_FLOW_ID",
                    "     , WF_FLOW_VER",
                    "     , WF_SUBJECT",
                    "     , LOT",
                    "     , NEW_USER_ID",
                    "     , END_USER_ID",
                    "     , DT_BEGIN",
                    "     , DT_END",
                    "     , RESULT_ID",
                    "     , NODE_NO",
                    "     , UPD_USER_ID",
                    "     , UPD_DT",
                    "  FROM WF_FLOW",
                    " WHERE WF_NO = {WF_NO}"
                }));

            dbParameters.Add(new DBParameter { Name = LogWFFlowPara.ParaField.WF_NO, Value = para.WFNo });
            var logWFFlow = GetEntityList<LogWFFlow>(commandText.ToString(), dbParameters).SingleOrDefault();
            if (logWFFlow != null)
            {
                logWFFlow.LOG_WF_NODE_LIST = SelectLogWFNodeList(para);
            }
            return logWFFlow;
        }

        #region - 查詢工作流程-節點紀錄檔 -
        /// <summary>
        /// 查詢工作流程-節點紀錄檔
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private List<LogWFNode> SelectLogWFNodeList(LogWFFlowPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT WF_NO",
                    "     , NODE_NO",
                    "     , SYS_ID",
                    "     , WF_FLOW_ID",
                    "     , WF_FLOW_VER",
                    "     , WF_NODE_ID",
                    "     , NEW_USER_ID",
                    "     , NEW_USER_ID_STR",
                    "     , NEW_USER_NM_STR",
                    "     , END_USER_ID",
                    "     , DT_BEGIN",
                    "     , DT_END",
                    "     , RESULT_ID",
                    "     , RESULT_VALUE",
                    "     , IS_START_SIG",
                    "     , SIG_STEP",
                    "     , SIG_RESULT_ID",
                    "     , SIG_USER_ID_STR",
                    "     , SIG_USER_NM_STR",
                    "     , UPD_USER_ID",
                    "     , UPD_DT",
                    "  FROM WF_NODE",
                    " WHERE WF_NO = {WF_NO}"
                }));

            dbParameters.Add(new DBParameter { Name = LogWFFlowPara.ParaField.WF_NO, Value = para.WFNo });
            var logWFNodeList = GetEntityList<LogWFNode>(commandText.ToString(), dbParameters);
            var logWFSigList = SelectLogWFSigList(para);
            var logWFDocList = SelectLogWFDocList(para);
            var logWFRemarkList = SelectLogWFRemarkList(para);
            var logWFNodeNewUserList = SelectLogWFNodeNewUserList(para);

            logWFNodeList.ForEach(row =>
            {
                row.LOG_WF_SIG_LIST = logWFSigList.FindAll(f => f.NODE_NO.GetValue() == row.NODE_NO.GetValue());
                row.LOG_WF_DOC_LIST = logWFDocList.FindAll(f => f.NODE_NO.GetValue() == row.NODE_NO.GetValue());
                row.LOG_WF_REMARK_LIST = logWFRemarkList.FindAll(f => f.NODE_NO.GetValue() == row.NODE_NO.GetValue());
                row.LOG_WF_NODE_NEW_USER_LIST = logWFNodeNewUserList.FindAll(f => f.NODE_NO.GetValue() == row.NODE_NO.GetValue());
            });

            return logWFNodeList;
        }
        #endregion

        #region - 查詢工作流程-簽核紀錄檔 -
        /// <summary>
        /// 查詢工作流程-簽核紀錄檔
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private List<LogWFSig> SelectLogWFSigList(LogWFFlowPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT WF_NO",
                    "     , NODE_NO",
                    "     , SYS_ID",
                    "     , WF_FLOW_ID",
                    "     , WF_FLOW_VER",
                    "     , WF_NODE_ID",
                    "     , SIG_KIND",
                    "     , SIG_STEP",
                    "     , WF_SIG_SEQ",
                    "     , WF_SIG_ZH_TW",
                    "     , WF_SIG_ZH_CN",
                    "     , WF_SIG_EN_US",
                    "     , WF_SIG_TH_TH",
                    "     , WF_SIG_JA_JP",
                    "     , WF_SIG_KO_KR",
                    "     , SIG_USER_ID",
                    "     , SIG_DATE",
                    "     , SIG_RESULT_ID",
                    "     , SIG_COMMENT",
                    "     , UPD_USER_ID",
                    "     , UPD_DT",
                    "  FROM WF_SIG",
                    " WHERE WF_NO = {WF_NO}"
                }));

            dbParameters.Add(new DBParameter { Name = LogWFFlowPara.ParaField.WF_NO, Value = para.WFNo });
            return GetEntityList<LogWFSig>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 查詢工作流程-文件紀錄檔 -
        /// <summary>
        /// 查詢工作流程-文件紀錄檔
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private List<LogWFDoc> SelectLogWFDocList(LogWFFlowPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT WF_NO",
                    "     , NODE_NO",
                    "     , DOC_NO",
                    "     , SYS_ID",
                    "     , WF_FLOW_ID",
                    "     , WF_FLOW_VER",
                    "     , WF_NODE_ID",
                    "     , WF_DOC_SEQ",
                    "     , WF_DOC_ZH_TW",
                    "     , WF_DOC_ZH_CN",
                    "     , WF_DOC_EN_US",
                    "     , WF_DOC_TH_TH",
                    "     , WF_DOC_JA_JP",
                    "     , WF_DOC_KO_KR",
                    "     , IS_REQ",
                    "     , DOC_USER_ID",
                    "     , DOC_FILE_NAME",
                    "     , DOC_ENCODE_NAME",
                    "     , DOC_DATE",
                    "     , DOC_PATH",
                    "     , DOC_LOCAL_PATH",
                    "     , IS_DELETE",
                    "     , UPD_USER_ID",
                    "     , UPD_DT",
                    "  FROM WF_DOC",
                    " WHERE WF_NO = {WF_NO}"
                }));

            dbParameters.Add(new DBParameter { Name = LogWFFlowPara.ParaField.WF_NO, Value = para.WFNo });
            return GetEntityList<LogWFDoc>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 查詢工作流程-備註紀錄檔 -
        /// <summary>
        /// 查詢工作流程-備註紀錄檔
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private List<LogWFRemark> SelectLogWFRemarkList(LogWFFlowPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT WF_NO",
                    "     , NODE_NO",
                    "     , REMARK_NO",
                    "     , SYS_ID",
                    "     , WF_FLOW_ID",
                    "     , WF_FLOW_VER",
                    "     , WF_NODE_ID",
                    "     , NODE_RESULT_ID",
                    "     , NODE_NEW_USER_ID",
                    "     , BACK_WF_NODE_ID",
                    "     , SIG_STEP",
                    "     , WF_SIG_SEQ",
                    "     , SIG_DATE",
                    "     , SIG_RESULT_ID",
                    "     , DOC_NO",
                    "     , WF_DOC_SEQ",
                    "     , DOC_DATE",
                    "     , DOC_IS_DELETE",
                    "     , REMARK_USER_ID",
                    "     , REMARK_DATE",
                    "     , REMARK",
                    "     , UPD_USER_ID",
                    "     , UPD_DT",
                    "  FROM WF_REMARK",
                    " WHERE WF_NO = {WF_NO}"
                }));

            dbParameters.Add(new DBParameter { Name = LogWFFlowPara.ParaField.WF_NO, Value = para.WFNo });
            return GetEntityList<LogWFRemark>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 查詢工作流程-候選建立節點使用者紀錄檔 -
        /// <summary>
        /// 查詢工作流程-候選建立節點使用者紀錄檔
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private List<LogWFNodeNewUser> SelectLogWFNodeNewUserList(LogWFFlowPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT WF_NO",
                    "     , NODE_NO",
                    "     , SYS_ID",
                    "     , WF_FLOW_ID",
                    "     , WF_FLOW_VER",
                    "     , WF_NODE_ID",
                    "     , NEW_USER_ID",
                    "     , UPD_USER_ID",
                    "     , UPD_DT",
                    "  FROM WF_NODE_NEW_USER",
                    " WHERE WF_NO = {WF_NO}"
                }));

            dbParameters.Add(new DBParameter { Name = LogWFFlowPara.ParaField.WF_NO, Value = para.WFNo });
            return GetEntityList<LogWFNodeNewUser>(commandText.ToString(), dbParameters);
        }
        #endregion
        #endregion
    }

    public class Mongo_BaseAP : MongoEntity
    {
        public Mongo_BaseAP(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public enum EnumMongoDocName
        {
            SYS_USER_SYSTEM_NOTIFICATIONS, SYS_SYSTEM_ROLE_CONDTION,
            LOG_USER_SYSTEM_ROLE_APPLY, LOG_USER_FUN_APPLY,
            APP_USER_PUSH, LOG_APP_USER_PUSH,
            APP_TOPIC_PUSH, LOG_APP_TOPIC_PUSH,
            LOG_SYS_SYSTEM_FUN_ELM, LOG_SYS_SYSTEM_ROLE_FUN_ELM, LOG_USER_FUN_ELM,
            LOG_USER_TRACE_ACTION
        }

        [AttributeUsage(AttributeTargets.Field)]
        private class PrimarykeyAttribute : Attribute
        {
        }

        [AttributeUsage(AttributeTargets.Field)]
        private class CollectionAttribute : Attribute
        {
            public Type CollectionType { get; set; }

            public CollectionAttribute(Type collectionType)
            {
                CollectionType = collectionType;
            }
        }

        public abstract class RecordLogPara : MongoDocument
        {
            internal enum ParaField
            {
                LOG_NO,
                API_NO,
                MODIFY_TYPE,
                MODIFY_TYPE_NM,

                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT,
                METHOD_FULL_NAME_LIST,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            [DBTypeProperty("LOG_NO")]
            public DBChar LogNo;

            [DBTypeProperty("API_NO")]
            public DBChar APINo;

            [DBTypeProperty("MODIFY_TYPE")]
            public DBChar ModifyType;

            [DBTypeProperty("MODIFY_TYPE_NM")]
            public DBNVarChar ModifyTypeNM;

            [DBTypeProperty("METHOD_FULL_NAME_LIST")]
            public List<DBVarChar> MethodFullNameList;

            [DBTypeProperty("UPD_USER_ID")]
            public DBVarChar UpdUserID;

            [DBTypeProperty("UPD_USER_NM")]
            public DBNVarChar UpdUserNM;

            [DBTypeProperty("UPD_DT")]
            public DBDateTime UpdDT;

            [DBTypeProperty("EXEC_SYS_ID")]
            public DBVarChar ExecSysID;

            [DBTypeProperty("EXEC_SYS_NM")]
            public DBNVarChar ExecSysNM;

            [DBTypeProperty("EXEC_IP_ADDRESS")]
            public DBVarChar ExecIPAddress;

            public RecordLogPara Clone()
            {
                return (RecordLogPara)MemberwiseClone();
            }
        }

        public void Insert(EnumLogDocName docName, ITableRow tableRow)
        {
            MongoCommand command = new MongoCommand(docName.ToString());
            bool result = Insert(command, tableRow);
            if (result == false)
            {
                throw new Exception("Insert Failure");
            }
        }

        public void Insert(EnumLogDocName docName, IEnumerable<ITableRow> tableRowList)
        {
            MongoCommand command = new MongoCommand(docName.ToString());
            bool result = Insert(command, tableRowList);
            if (result == false)
            {
                throw new Exception("Insert Failure");
            }
        }

        public void Insert(EnumMongoDocName docName, ITableRow tableRow)
        {
            MongoCommand command = new MongoCommand(docName.ToString());
            bool result = Insert(command, tableRow);
            if (result == false)
            {
                throw new Exception("Insert Failure");
            }
        }

        public void Insert(EnumMongoDocName docName, IEnumerable<ITableRow> tableRowList)
        {
            MongoCommand command = new MongoCommand(docName.ToString());
            bool result = Insert(command, tableRowList);
            if (result == false)
            {
                throw new Exception("Insert List Failure");
            }
        }

        #region - Log -

        private string InitLogNo = "000001";

        public enum EnumModifyType
        {
            [Description("Insert")]
            I,

            [Description("Update")]
            U,

            [Description("Delete")]
            D
        }

        public enum EnumLogDocName
        {
            LOG_SYS_SYSTEM_ROLE,
            LOG_SYS_SYSTEM_ROLE_GROUP,
            LOG_SYS_SYSTEM_ROLE_GROUP_COLLECT,
            [Collection(typeof(RecordLogSystemRoleCondotionPara))]
            LOG_SYS_SYSTEM_ROLE_CONDTION,
            LOG_SYS_SYSTEM_FUN_GROUP,
            LOG_SYS_SYSTEM_FUN,
            LOG_SYS_SYSTEM_ROLE_FUN,
            LOG_SYS_TRUST_IP,

            [Collection(typeof(RecordUserFunctionPara))]
            LOG_USER_FUN,
            LOG_USER_LOGIN,
            LOG_USER_PWD,
            LOG_USER_ACCESS,
            [Collection(typeof(RecordLogUserAccountPara))]
            LOG_USER_ACCOUNT,

            [Collection(typeof(RecordLogSysSystemWFDocPara))]
            LOG_SYS_SYSTEM_WF_DOC,
            [Collection(typeof(RecordLogSysSystemWFFlowPara))]
            LOG_SYS_SYSTEM_WF_FLOW,
            [Collection(typeof(RecordLogSysSystemWFFlowGroupPara))]
            LOG_SYS_SYSTEM_WF_FLOW_GROUP,
            [Collection(typeof(RecordLogSysSystemWFNextPara))]
            LOG_SYS_SYSTEM_WF_NEXT,
            [Collection(typeof(RecordLogSysSystemWFNodePara))]
            LOG_SYS_SYSTEM_WF_NODE,
            [Collection(typeof(RecordLogSysSystemWFSigPara))]
            LOG_SYS_SYSTEM_WF_SIG,
            [Collection(typeof(RecordLogSysSystemRoleFlowPara))]
            LOG_SYS_SYSTEM_ROLE_FLOW,
            [Collection(typeof(RecordLogSysSystemRoleNodePara))]
            LOG_SYS_SYSTEM_ROLE_NODE,
            [Collection(typeof(RecordLogSysSystemRoleSigPara))]
            LOG_SYS_SYSTEM_ROLE_SIG,

            LOG_ERP_USER_FUN,

            LOG_WF_FLOW,

            [Collection(typeof(RecordLogUserPurviewPara))]
            LOG_USER_PURVIEW,

            [Collection(typeof(RecordLogUserPurviewApplyPara))]
            LOG_USER_PURVIEW_APPLY,

            [Collection(typeof(RecordLogSystemLinePara))]
            LOG_SYS_SYSTEM_LINE,

            [Collection(typeof(RecordLogSystemLineReceiverPara))]
            LOG_SYS_SYSTEM_LINE_RECEIVER,

            [Collection(typeof(RecordLogUserSystemRoleApplyPara))]
            LOG_USER_SYSTEM_ROLE_APPLY,

            [Collection(typeof(RecordLogUserFunApplyPara))]
            LOG_USER_FUN_APPLY,

            LOG_APP_USER_PUSH,

            LOG_APP_USER_FUN,

            LOG_APP_USER_FUN_ROLE,

            LOG_LINE_WEBHOOKS,

            [Collection(typeof(RecordLogUserActionTracePara))]
            LOG_ERP_USER_TRACE_ACTION
        }


        /*Sys Log*/

        public class SysLogNo
        {
            public enum DataField
            {
                SYS_ID,
                FUN_CONTROLLER_ID,
                FUN_ACTION_NAME,
                ROLE_GROUP_ID,
                ROLE_ID,
                IP_BEGIN,
                IP_END,
                LOG_NO,
                WF_FLOW_GROUP_ID
            }

            public DBChar LogNo;
        }

        public class RecordLogUserActionTracePara : MongoDocument
        {
            public class IPInfo : MongoDocument
            {
                [DBTypeProperty("IP_BEGIN")]
                public DBVarChar IPBegin;
                [DBTypeProperty("IP_END")]
                public DBVarChar IPEnd;

                [DBTypeProperty("COM_ID")]
                public DBVarChar ComID;
                [DBTypeProperty("COM_NM")]
                public DBNVarChar ComNM;

                [DBTypeProperty("TRUST_STATUS")]
                public DBChar TrustStatus;
                [DBTypeProperty("TRUST_TYPE")]
                public DBChar TrustType;
                [DBTypeProperty("TRUST_TYPE_NM")]
                public DBNVarChar TrustTypeNM;
                [DBTypeProperty("SOURCE_TYPE")]
                public DBChar SourceType;
                [DBTypeProperty("SOURCE_TYPE_NM")]
                public DBNVarChar SourceTypeNM;

                [DBTypeProperty("REMARK")]
                public DBNVarChar Remark;
            }

            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;
            [DBTypeProperty("CONTROLLER_NAME")]
            public DBVarChar ControllerName;
            [DBTypeProperty("ACTION_NAME")]
            public DBVarChar ActionName;
            [DBTypeProperty("REQUEST_URL")]
            public DBNVarChar RequestUrl;

            [DBTypeProperty("USER_ID")]
            public DBVarChar UserID;
            [DBTypeProperty("USER_IP_ADDRESS")]
            public DBVarChar UserIPAddress;
            [DBTypeProperty("USER_IP_ADDRESS_INFO")]
            public IPInfo UserIPAddressInfo;

            [DBTypeProperty("ACTION_PARAMETERS")]
            public MongoDocument QueryPara;

            [DBTypeProperty("UPD_DT")]
            public DBDateTime UpdDT;
        }

        public class RecordSysSystemRolePara : MongoElement
        {
            public enum ParaField
            {
                LOG_NO,
                MODIFY_TYPE,
                MODIFY_TYPE_NM,
                SYS_ID,
                SYS_NM,
                ROLE_CATEGORY_ID,
                ROLE_CATEGORY_NM,
                ROLE_ID,
                ROLE_NM_ZH_TW,
                ROLE_NM_ZH_CN,
                ROLE_NM_EN_US,
                ROLE_NM_TH_TH,
                ROLE_NM_JA_JP,
                ROLE_NM_KO_KR,
                IS_MASTER,
                API_NO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBChar ModifyType;
            public DBNVarChar ModifyTypeNM;

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleCategoryID;
            public DBNVarChar RoleCategoryNM;
            public DBVarChar RoleID;

            public DBNVarChar RoleNMZHTW;
            public DBNVarChar RoleNMZHCN;
            public DBNVarChar RoleNMENUS;
            public DBNVarChar RoleNMTHTH;
            public DBNVarChar RoleNMJAJP;
            public DBNVarChar RoleNMKOKR;

            public DBChar IsMaster;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public enum EnumRecordSysSystemRoleResult
        {
            Success,
            Failure
        }

        public EnumRecordSysSystemRoleResult RecordSysSystemRole(RecordSysSystemRolePara para)
        {
            MongoCommand command = new MongoCommand(EnumLogDocName.LOG_SYS_SYSTEM_ROLE.ToString());
            DBChar logNo = new DBChar(InitLogNo);

            #region Get Log No
            command.SetRowCount(1);
            command.AddFields(EnumSpecifiedFieldType.Select, SysLogNo.DataField.LOG_NO.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysLogNo.DataField.SYS_ID.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysLogNo.DataField.ROLE_ID.ToString());
            command.AddSortBy(EnumSortType.DESC, SysLogNo.DataField.LOG_NO.ToString());

            DBParameters dbParameters = new DBParameters();
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.ROLE_ID.ToString(), Value = para.RoleID });

            List<SysLogNo> sysLogNoList = Select<SysLogNo>(command, dbParameters);
            if (sysLogNoList != null &&
                sysLogNoList.Count == 1)
            {
                int tempLogNo = 0;
                if (sysLogNoList[0].LogNo != null &&
                    int.TryParse(sysLogNoList[0].LogNo.GetValue(), out tempLogNo))
                {
                    logNo = new DBChar((tempLogNo + 1).ToString().PadLeft(6, '0'));
                }
            }

            command.Clear();
            dbParameters.Clear();
            #endregion

            dbParameters = new DBParameters();
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.LOG_NO, Value = logNo });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.MODIFY_TYPE, Value = para.ModifyType });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.MODIFY_TYPE_NM, Value = para.ModifyTypeNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.SYS_NM, Value = para.SysNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.ROLE_CATEGORY_ID, Value = para.RoleCategoryID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.ROLE_CATEGORY_NM, Value = para.RoleCategoryNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.ROLE_ID, Value = para.RoleID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.ROLE_NM_ZH_TW, Value = para.RoleNMZHTW });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.ROLE_NM_ZH_CN, Value = para.RoleNMZHCN });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.ROLE_NM_EN_US, Value = para.RoleNMENUS });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.ROLE_NM_TH_TH, Value = para.RoleNMTHTH });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.ROLE_NM_JA_JP, Value = para.RoleNMJAJP });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.ROLE_NM_KO_KR, Value = para.RoleNMKOKR });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.IS_MASTER, Value = para.IsMaster });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.API_NO, Value = para.APINo });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.UPD_USER_NM, Value = para.UpdUserNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.UPD_DT, Value = para.UpdDT });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.EXEC_SYS_ID, Value = para.ExecSysID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.EXEC_SYS_NM, Value = para.ExecSysNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRolePara.ParaField.EXEC_IP_ADDRESS, Value = para.ExecIPAddress });

            bool result = Insert(command, dbParameters);
            return (result) ? EnumRecordSysSystemRoleResult.Success : EnumRecordSysSystemRoleResult.Failure;
        }


        public class RecordSysSystemRoleGroupPara : MongoElement
        {
            public enum ParaField
            {
                LOG_NO,
                MODIFY_TYPE,
                MODIFY_TYPE_NM,
                ROLE_GROUP_ID,
                ROLE_GROUP_NM_ZH_TW,
                ROLE_GROUP_NM_ZH_CN,
                ROLE_GROUP_NM_EN_US,
                ROLE_GROUP_NM_TH_TH,
                ROLE_GROUP_NM_JA_JP,
                ROLE_GROUP_NM_KO_KR,
                SORT_ORDER,
                REMARK,
                API_NO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBChar ModifyType;
            public DBNVarChar ModifyTypeNM;

            public DBVarChar RoleGroupID;

            public DBNVarChar RoleGroupNMZHTW;
            public DBNVarChar RoleGroupNMZHCN;
            public DBNVarChar RoleGroupNMENUS;
            public DBNVarChar RoleGroupNMTHTH;
            public DBNVarChar RoleGroupNMJAJP;
            public DBNVarChar RoleGroupNMKOKR;

            public DBVarChar SortOrder;
            public DBNVarChar Remark;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public enum EnumRecordSysSystemRoleGroupResult
        {
            Success,
            Failure
        }

        public EnumRecordSysSystemRoleGroupResult RecordSysSystemRoleGroup(RecordSysSystemRoleGroupPara para)
        {
            MongoCommand command = new MongoCommand(EnumLogDocName.LOG_SYS_SYSTEM_ROLE_GROUP.ToString());
            DBChar logNo = new DBChar(InitLogNo);

            #region Get Log No
            command.SetRowCount(1);
            command.AddFields(EnumSpecifiedFieldType.Select, SysLogNo.DataField.LOG_NO.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysLogNo.DataField.ROLE_GROUP_ID.ToString());
            command.AddSortBy(EnumSortType.DESC, SysLogNo.DataField.LOG_NO.ToString());

            DBParameters dbParameters = new DBParameters();
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.ROLE_GROUP_ID.ToString(), Value = para.RoleGroupID });

            List<SysLogNo> sysLogNoList = Select<SysLogNo>(command, dbParameters);
            if (sysLogNoList != null &&
                sysLogNoList.Count == 1)
            {
                int tempLogNo = 0;
                if (sysLogNoList[0].LogNo != null &&
                    int.TryParse(sysLogNoList[0].LogNo.GetValue(), out tempLogNo))
                {
                    logNo = new DBChar((tempLogNo + 1).ToString().PadLeft(6, '0'));
                }
            }

            command.Clear();
            dbParameters.Clear();
            #endregion

            dbParameters = new DBParameters();
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.LOG_NO, Value = logNo });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.MODIFY_TYPE, Value = para.ModifyType });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.MODIFY_TYPE_NM, Value = para.ModifyTypeNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.ROLE_GROUP_ID, Value = para.RoleGroupID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.ROLE_GROUP_NM_ZH_TW, Value = para.RoleGroupNMZHTW });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.ROLE_GROUP_NM_ZH_CN, Value = para.RoleGroupNMZHCN });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.ROLE_GROUP_NM_EN_US, Value = para.RoleGroupNMENUS });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.ROLE_GROUP_NM_TH_TH, Value = para.RoleGroupNMTHTH });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.ROLE_GROUP_NM_JA_JP, Value = para.RoleGroupNMJAJP });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.ROLE_GROUP_NM_KO_KR, Value = para.RoleGroupNMKOKR });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.API_NO, Value = para.APINo });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.UPD_USER_NM, Value = para.UpdUserNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.UPD_DT, Value = para.UpdDT });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.EXEC_SYS_ID, Value = para.ExecSysID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.EXEC_SYS_NM, Value = para.ExecSysNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupPara.ParaField.EXEC_IP_ADDRESS, Value = para.ExecIPAddress });

            bool result = Insert(command, dbParameters);
            return (result) ? EnumRecordSysSystemRoleGroupResult.Success : EnumRecordSysSystemRoleGroupResult.Failure;
        }


        public class RecordSysSystemRoleGroupCollectPara : MongoElement
        {
            public enum ParaField
            {
                LOG_NO,
                ROLE_GROUP_ID,
                ROLE_GROUP_NM,
                SYS_ID,
                SYS_NM,
                ROLE_ID,
                ROLE_NM,
                API_NO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBVarChar RoleGroupID;
            public DBNVarChar RoleGroupNM;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public enum EnumRecordSysSystemRoleGroupCollectResult
        {
            Success,
            Failure
        }

        public EnumRecordSysSystemRoleGroupCollectResult RecordSysSystemRoleGroupCollect(RecordSysSystemRoleGroupCollectPara para, List<RecordSysSystemRoleGroupCollectPara> paraList)
        {
            if (paraList != null &&
                paraList.Count > 0)
            {
                MongoCommand command = new MongoCommand(EnumLogDocName.LOG_SYS_SYSTEM_ROLE_GROUP_COLLECT.ToString());
                DBChar logNo = new DBChar(InitLogNo);

                #region Get Log No
                command.SetRowCount(1);
                command.AddFields(EnumSpecifiedFieldType.Select, SysLogNo.DataField.LOG_NO.ToString());
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysLogNo.DataField.ROLE_GROUP_ID.ToString());
                command.AddSortBy(EnumSortType.DESC, SysLogNo.DataField.LOG_NO.ToString());

                DBParameters dbParameters = new DBParameters();
                dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupCollectPara.ParaField.ROLE_GROUP_ID.ToString(), Value = para.RoleGroupID });

                List<SysLogNo> sysLogNoList = Select<SysLogNo>(command, dbParameters);
                if (sysLogNoList != null &&
                    sysLogNoList.Count == 1)
                {
                    int tempLogNo = 0;
                    if (sysLogNoList[0].LogNo != null &&
                        int.TryParse(sysLogNoList[0].LogNo.GetValue(), out tempLogNo))
                    {
                        logNo = new DBChar((tempLogNo + 1).ToString().PadLeft(6, '0'));
                    }
                }

                command.Clear();
                dbParameters.Clear();
                #endregion

                List<DBParameters> dbParametersList = new List<DBParameters>();
                foreach (RecordSysSystemRoleGroupCollectPara recordPara in paraList)
                {
                    dbParameters = new DBParameters();
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupCollectPara.ParaField.LOG_NO, Value = logNo });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupCollectPara.ParaField.ROLE_GROUP_ID, Value = recordPara.RoleGroupID });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupCollectPara.ParaField.ROLE_GROUP_NM, Value = recordPara.RoleGroupNM });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupCollectPara.ParaField.SYS_ID, Value = recordPara.SysID });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupCollectPara.ParaField.SYS_NM, Value = recordPara.SysNM });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupCollectPara.ParaField.ROLE_ID, Value = recordPara.RoleID });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupCollectPara.ParaField.ROLE_NM, Value = recordPara.RoleNM });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupCollectPara.ParaField.API_NO, Value = recordPara.APINo });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupCollectPara.ParaField.UPD_USER_ID.ToString(), Value = recordPara.UpdUserID });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupCollectPara.ParaField.UPD_USER_NM.ToString(), Value = recordPara.UpdUserNM });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupCollectPara.ParaField.UPD_DT, Value = recordPara.UpdDT });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupCollectPara.ParaField.EXEC_SYS_ID, Value = recordPara.ExecSysID });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupCollectPara.ParaField.EXEC_SYS_NM, Value = recordPara.ExecSysNM });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleGroupCollectPara.ParaField.EXEC_IP_ADDRESS, Value = recordPara.ExecIPAddress });
                    dbParametersList.Add(dbParameters);
                }

                bool result = Insert(command, dbParametersList);
                return (result) ? EnumRecordSysSystemRoleGroupCollectResult.Success : EnumRecordSysSystemRoleGroupCollectResult.Failure;
            }

            return EnumRecordSysSystemRoleGroupCollectResult.Failure;
        }


        public class RecordSysSystemFunGroupPara : MongoElement
        {
            public enum ParaField
            {
                LOG_NO,
                MODIFY_TYPE,
                MODIFY_TYPE_NM,
                SYS_ID,
                SYS_NM,
                FUN_CONTROLLER_ID,
                FUN_GROUP_ZH_TW,
                FUN_GROUP_ZH_CN,
                FUN_GROUP_EN_US,
                FUN_GROUP_TH_TH,
                FUN_GROUP_JA_JP,
                FUN_GROUP_KO_KR,
                SORT_ORDER,
                API_NO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBChar ModifyType;
            public DBNVarChar ModifyTypeNM;

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar FunControllerID;

            public DBNVarChar FunGroupZHTW;
            public DBNVarChar FunGroupZHCN;
            public DBNVarChar FunGroupENUS;
            public DBNVarChar FunGroupTHTH;
            public DBNVarChar FunGroupJAJP;
            public DBNVarChar FunGroupKOKR;

            public DBVarChar SortOrder;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public enum EnumRecordSysSystemFunGroupResult
        {
            Success,
            Failure
        }

        public EnumRecordSysSystemFunGroupResult RecordSysSystemFunGroup(RecordSysSystemFunGroupPara para)
        {
            MongoCommand command = new MongoCommand(EnumLogDocName.LOG_SYS_SYSTEM_FUN_GROUP.ToString());
            DBChar logNo = new DBChar(InitLogNo);

            #region Get Log No
            command.SetRowCount(1);
            command.AddFields(EnumSpecifiedFieldType.Select, SysLogNo.DataField.LOG_NO.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysLogNo.DataField.SYS_ID.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysLogNo.DataField.FUN_CONTROLLER_ID.ToString());
            command.AddSortBy(EnumSortType.DESC, SysLogNo.DataField.LOG_NO.ToString());

            DBParameters dbParameters = new DBParameters();
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });

            List<SysLogNo> sysLogNoList = Select<SysLogNo>(command, dbParameters);
            if (sysLogNoList != null &&
                sysLogNoList.Count == 1)
            {
                int tempLogNo = 0;
                if (sysLogNoList[0].LogNo != null &&
                    int.TryParse(sysLogNoList[0].LogNo.GetValue(), out tempLogNo))
                {
                    logNo = new DBChar((tempLogNo + 1).ToString().PadLeft(6, '0'));
                }
            }

            command.Clear();
            dbParameters.Clear();
            #endregion

            dbParameters = new DBParameters();
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.LOG_NO, Value = logNo });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.MODIFY_TYPE, Value = para.ModifyType });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.MODIFY_TYPE_NM, Value = para.ModifyTypeNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.SYS_NM, Value = para.SysNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.FUN_GROUP_ZH_TW, Value = para.FunGroupZHTW });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.FUN_GROUP_ZH_CN, Value = para.FunGroupZHCN });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.FUN_GROUP_EN_US, Value = para.FunGroupENUS });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.FUN_GROUP_TH_TH, Value = para.FunGroupTHTH });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.FUN_GROUP_JA_JP, Value = para.FunGroupJAJP });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.FUN_GROUP_KO_KR, Value = para.FunGroupKOKR });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.API_NO, Value = para.APINo });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.UPD_USER_NM, Value = para.UpdUserNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.UPD_DT, Value = para.UpdDT });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.EXEC_SYS_ID, Value = para.ExecSysID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.EXEC_SYS_NM, Value = para.ExecSysNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunGroupPara.ParaField.EXEC_IP_ADDRESS, Value = para.ExecIPAddress });

            bool result = Insert(command, dbParameters);
            return (result) ? EnumRecordSysSystemFunGroupResult.Success : EnumRecordSysSystemFunGroupResult.Failure;
        }


        public class RecordSysSystemFunPara : MongoElement
        {
            public enum ParaField
            {
                LOG_NO,
                MODIFY_TYPE,
                MODIFY_TYPE_NM,
                SYS_ID,
                SYS_NM,
                SUB_SYS_ID,
                SUB_SYS_NM,
                PURVIEW_ID,
                PURVIEW_NM,
                FUN_CONTROLLER_ID,
                FUN_GROUP_NM,
                FUN_ACTION_NAME,
                FUN_NM_ZH_TW,
                FUN_NM_ZH_CN,
                FUN_NM_EN_US,
                FUN_NM_TH_TH,
                FUN_NM_JA_JP,
                FUN_NM_KO_KR,
                FUN_TYPE,
                FUN_TYPE_NM,
                IS_OUTSIDE,
                IS_DISABLE,
                SORT_ORDER,
                API_NO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBChar ModifyType;
            public DBNVarChar ModifyTypeNM;

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar SubSysID;
            public DBNVarChar SubSysNM;
            public DBVarChar PurviewID;
            public DBNVarChar PurviewNM;
            public DBVarChar FunControllerID;
            public DBNVarChar FunGroupNM;
            public DBVarChar FunActionNM;

            public DBNVarChar FunNMZHTW;
            public DBNVarChar FunNMZHCN;
            public DBNVarChar FunNMENUS;
            public DBNVarChar FunNMTHTH;
            public DBNVarChar FunNMJAJP;
            public DBNVarChar FunNMKOKR;

            public DBVarChar FunType;
            public DBNVarChar FunTypeNM;
            public DBChar IsOutside;
            public DBChar IsDisable;
            public DBVarChar SortOrder;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public enum EnumRecordSysSystemFunResult
        {
            Success,
            Failure
        }

        public EnumRecordSysSystemFunResult RecordSysSystemFun(RecordSysSystemFunPara para)
        {
            MongoCommand command = new MongoCommand(EnumLogDocName.LOG_SYS_SYSTEM_FUN.ToString());
            DBChar logNo = new DBChar(InitLogNo);

            #region Get Log No
            command.SetRowCount(1);
            command.AddFields(EnumSpecifiedFieldType.Select, SysLogNo.DataField.LOG_NO.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysLogNo.DataField.SYS_ID.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysLogNo.DataField.FUN_CONTROLLER_ID.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysLogNo.DataField.FUN_ACTION_NAME.ToString());
            command.AddSortBy(EnumSortType.DESC, SysLogNo.DataField.LOG_NO.ToString());

            DBParameters dbParameters = new DBParameters();
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionNM });

            List<SysLogNo> sysLogNoList = Select<SysLogNo>(command, dbParameters);
            if (sysLogNoList != null &&
                sysLogNoList.Count == 1)
            {
                int tempLogNo = 0;
                if (sysLogNoList[0].LogNo != null &&
                    int.TryParse(sysLogNoList[0].LogNo.GetValue(), out tempLogNo))
                {
                    logNo = new DBChar((tempLogNo + 1).ToString().PadLeft(6, '0'));
                }
            }

            command.Clear();
            dbParameters.Clear();
            #endregion

            dbParameters = new DBParameters();
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.LOG_NO, Value = logNo });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.MODIFY_TYPE, Value = para.ModifyType });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.MODIFY_TYPE_NM, Value = para.ModifyTypeNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.SYS_NM, Value = para.SysNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.SUB_SYS_ID, Value = para.SubSysID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.SUB_SYS_NM, Value = para.SubSysNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.PURVIEW_ID, Value = para.PurviewID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.PURVIEW_NM, Value = para.PurviewNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.FUN_GROUP_NM, Value = para.FunGroupNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.FUN_NM_ZH_TW, Value = para.FunNMZHTW });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.FUN_NM_ZH_CN, Value = para.FunNMZHCN });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.FUN_NM_EN_US, Value = para.FunNMENUS });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.FUN_NM_TH_TH, Value = para.FunNMTHTH });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.FUN_NM_JA_JP, Value = para.FunNMJAJP });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.FUN_NM_KO_KR, Value = para.FunNMKOKR });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.FUN_TYPE, Value = para.FunType });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.FUN_TYPE_NM, Value = para.FunTypeNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.IS_OUTSIDE, Value = para.IsOutside });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.IS_DISABLE, Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.API_NO, Value = para.APINo });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.UPD_USER_NM, Value = para.UpdUserNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.UPD_DT, Value = para.UpdDT });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.EXEC_SYS_ID, Value = para.ExecSysID });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.EXEC_SYS_NM, Value = para.ExecSysNM });
            dbParameters.Add(new DBParameter { Name = RecordSysSystemFunPara.ParaField.EXEC_IP_ADDRESS, Value = para.ExecIPAddress });

            bool result = Insert(command, dbParameters);
            return (result) ? EnumRecordSysSystemFunResult.Success : EnumRecordSysSystemFunResult.Failure;
        }


        public class RecordSysSystemRoleFunPara : MongoElement
        {
            public enum ParaField
            {
                LOG_NO,
                SYS_ID,
                SYS_NM,
                ROLE_ID,
                ROLE_NM,
                FUN_CONTROLLER_ID,
                FUN_GROUP_NM,
                FUN_ACTION_NAME,
                FUN_NM,
                API_NO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
            public DBVarChar FunControllerID;
            public DBNVarChar FunGroupNM;
            public DBVarChar FunActionNM;
            public DBNVarChar FunNM;
            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public enum EnumRecordSysSystemRoleFunResult
        {
            Success,
            Failure
        }

        public EnumRecordSysSystemRoleFunResult RecordSysSystemRoleFun(RecordSysSystemRoleFunPara para, List<RecordSysSystemRoleFunPara> paraList)
        {
            if (paraList != null &&
                paraList.Count > 0)
            {
                MongoCommand command = new MongoCommand(EnumLogDocName.LOG_SYS_SYSTEM_ROLE_FUN.ToString());
                DBChar logNo = new DBChar(InitLogNo);

                #region Get Log No
                command.SetRowCount(1);
                command.AddFields(EnumSpecifiedFieldType.Select, SysLogNo.DataField.LOG_NO.ToString());
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysLogNo.DataField.SYS_ID.ToString());
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysLogNo.DataField.FUN_CONTROLLER_ID.ToString());
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysLogNo.DataField.FUN_ACTION_NAME.ToString());
                command.AddSortBy(EnumSortType.DESC, SysLogNo.DataField.LOG_NO.ToString());

                DBParameters dbParameters = new DBParameters();
                dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
                dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
                dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionNM });

                List<SysLogNo> sysLogNoList = Select<SysLogNo>(command, dbParameters);
                if (sysLogNoList != null &&
                    sysLogNoList.Count == 1)
                {
                    int tempLogNo = 0;
                    if (sysLogNoList[0].LogNo != null &&
                        int.TryParse(sysLogNoList[0].LogNo.GetValue(), out tempLogNo))
                    {
                        logNo = new DBChar((tempLogNo + 1).ToString().PadLeft(6, '0'));
                    }
                }

                command.Clear();
                dbParameters.Clear();
                #endregion

                List<DBParameters> dbParametersList = new List<DBParameters>();
                foreach (RecordSysSystemRoleFunPara recordPara in paraList)
                {
                    dbParameters = new DBParameters();
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.LOG_NO, Value = logNo });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.SYS_ID, Value = recordPara.SysID });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.SYS_NM, Value = recordPara.SysNM });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.ROLE_ID, Value = recordPara.RoleID });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.ROLE_NM, Value = recordPara.RoleNM });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.FUN_CONTROLLER_ID, Value = recordPara.FunControllerID });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.FUN_GROUP_NM, Value = recordPara.FunGroupNM });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.FUN_ACTION_NAME, Value = recordPara.FunActionNM });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.FUN_NM, Value = recordPara.FunNM });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.API_NO, Value = recordPara.APINo });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.UPD_USER_ID, Value = recordPara.UpdUserID });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.UPD_USER_NM, Value = recordPara.UpdUserNM });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.UPD_DT, Value = recordPara.UpdDT });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.EXEC_SYS_ID, Value = recordPara.ExecSysID });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.EXEC_SYS_NM, Value = recordPara.ExecSysNM });
                    dbParameters.Add(new DBParameter { Name = RecordSysSystemRoleFunPara.ParaField.EXEC_IP_ADDRESS, Value = recordPara.ExecIPAddress });
                    dbParametersList.Add(dbParameters);
                }

                bool result = Insert(command, dbParametersList);
                return (result) ? EnumRecordSysSystemRoleFunResult.Success : EnumRecordSysSystemRoleFunResult.Failure;
            }

            return EnumRecordSysSystemRoleFunResult.Failure;
        }

        public class RecordSysTrustIPPara : MongoElement
        {
            public enum ParaField
            {
                LOG_NO,
                MODIFY_TYPE,
                MODIFY_TYPE_NM,
                IP_BEGIN,
                IP_END,
                COM_ID,
                COM_NM,
                TRUST_STATUS,
                TRUST_STATUS_NM,
                TRUST_TYPE,
                TRUST_TYPE_NM,
                SOURCE_TYPE,
                SOURCE_TYPE_NM,
                REMARK,
                SORT_ORDER,
                API_NO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBChar ModifyType;
            public DBNVarChar ModifyTypeNM;

            public DBVarChar IPBegin;
            public DBVarChar IPEnd;
            public DBVarChar ComID;
            public DBNVarChar ComNM;

            public DBChar TrustStatus;
            public DBChar TrustType;
            public DBNVarChar TrustTypeNM;
            public DBChar SourceType;
            public DBNVarChar SourceTypeNM;

            public DBNVarChar Remark;
            public DBVarChar SortOrder;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public enum EnumRecordSysTrustIPResult
        {
            Success,
            Failure
        }

        public EnumRecordSysTrustIPResult RecordSysTrustIP(RecordSysTrustIPPara para)
        {
            MongoCommand command = new MongoCommand(EnumLogDocName.LOG_SYS_TRUST_IP.ToString());
            DBChar logNo = new DBChar(InitLogNo);

            #region Get Log No
            command.SetRowCount(1);
            command.AddFields(EnumSpecifiedFieldType.Select, SysLogNo.DataField.LOG_NO.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysLogNo.DataField.IP_BEGIN.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysLogNo.DataField.IP_END.ToString());
            command.AddSortBy(EnumSortType.DESC, SysLogNo.DataField.LOG_NO.ToString());

            DBParameters dbParameters = new DBParameters();
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.IP_BEGIN.ToString(), Value = para.IPBegin });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.IP_END.ToString(), Value = para.IPEnd });

            List<SysLogNo> sysLogNoList = Select<SysLogNo>(command, dbParameters);
            if (sysLogNoList != null &&
                sysLogNoList.Count == 1)
            {
                int tempLogNo = 0;
                if (sysLogNoList[0].LogNo != null &&
                    int.TryParse(sysLogNoList[0].LogNo.GetValue(), out tempLogNo))
                {
                    logNo = new DBChar((tempLogNo + 1).ToString().PadLeft(6, '0'));
                }
            }

            command.Clear();
            dbParameters.Clear();
            #endregion

            dbParameters = new DBParameters();
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.LOG_NO, Value = logNo });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.MODIFY_TYPE, Value = para.ModifyType });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.MODIFY_TYPE_NM, Value = para.ModifyTypeNM });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.IP_BEGIN, Value = para.IPBegin });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.IP_END, Value = para.IPEnd });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.COM_ID, Value = para.ComID });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.COM_NM, Value = para.ComNM });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.TRUST_STATUS, Value = para.TrustStatus });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.TRUST_TYPE, Value = para.TrustType });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.TRUST_TYPE_NM, Value = para.TrustTypeNM });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.SOURCE_TYPE, Value = para.SourceType });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.SOURCE_TYPE_NM, Value = para.SourceTypeNM });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.API_NO, Value = para.APINo });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.UPD_USER_NM, Value = para.UpdUserNM });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.UPD_DT, Value = para.UpdDT });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.EXEC_SYS_ID, Value = para.ExecSysID });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.EXEC_SYS_NM, Value = para.ExecSysNM });
            dbParameters.Add(new DBParameter { Name = RecordSysTrustIPPara.ParaField.EXEC_IP_ADDRESS, Value = para.ExecIPAddress });

            bool result = Insert(command, dbParameters);
            return (result) ? EnumRecordSysTrustIPResult.Success : EnumRecordSysTrustIPResult.Failure;
        }


        /*User Log*/

        public class UserLogNo
        {
            public enum DataField
            {
                USER_ID,
                LOG_NO
            }

            public DBChar LogNo;
        }

        public class RecordUserLoginPara : MongoElement
        {
            public enum ParaField
            {
                USER_ID,
                USER_NM,
                LOG_NO,
                USER_PWD,
                LOCATION,
                LOCATION_NM,
                LOCATION_DESC,
                USER_IDNO,
                USER_BIRTHDAY,
                IP_ADDRESS,
                VALID_RESULT,
                VALID_RESULT_NM,
                PROXY_LOGIN_SYS_ID,
                TARGET_URL,
                EXAPI_ERROR_CODE,
                EXAPI_ERROR_DESC,
                API_NO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UserPWD;
            public DBVarChar LoginType;
            public DBVarChar Location;
            public DBNVarChar LocationNM;
            public DBNVarChar LocationDesc;
            public DBVarChar UserIDNo;
            public DBVarChar UserBirthday;
            public DBVarChar IPAddress;
            public DBVarChar ValidResult;
            public DBNVarChar ValidResultNM;
            public DBVarChar ProxyLoginSystemID;
            public DBNVarChar TargetUrl;

            public DBVarChar ExApiErrorCode;
            public DBNVarChar ExApiErrorDesc;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public enum EnumRecordUserLoginResult
        {
            Success,
            Failure
        }

        public EnumRecordUserLoginResult RecordUserLogin(RecordUserLoginPara para)
        {
            MongoCommand command = new MongoCommand(EnumLogDocName.LOG_USER_LOGIN.ToString());
            DBChar logNo = new DBChar(InitLogNo);
            #region Get Log No
            command.SetRowCount(1);
            command.AddFields(EnumSpecifiedFieldType.Select, UserLogNo.DataField.LOG_NO.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, UserLogNo.DataField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            command.AddSortBy(EnumSortType.DESC, UserLogNo.DataField.LOG_NO.ToString());

            DBParameters dbParameters = new DBParameters();
            List<SysLogNo> sysLogNoList = Select<SysLogNo>(command);
            if (sysLogNoList != null &&
                sysLogNoList.Count == 1)
            {
                int tempLogNo = 0;
                if (sysLogNoList[0].LogNo != null &&
                    int.TryParse(sysLogNoList[0].LogNo.GetValue(), out tempLogNo))
                {
                    logNo = new DBChar((tempLogNo + 1).ToString().PadLeft(6, '0'));
                }
            }

            command.Clear();
            dbParameters.Clear();
            #endregion

            dbParameters = new DBParameters();
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.USER_NM, Value = para.UserNM });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.LOG_NO, Value = logNo });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.USER_PWD, Value = para.UserPWD });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.LOCATION, Value = para.Location });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.LOCATION_NM, Value = para.LocationNM });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.LOCATION_DESC, Value = para.LocationDesc });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.USER_IDNO, Value = para.UserIDNo });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.USER_BIRTHDAY, Value = para.UserBirthday });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.IP_ADDRESS, Value = para.IPAddress });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.VALID_RESULT, Value = para.ValidResult });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.VALID_RESULT_NM, Value = para.ValidResultNM });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.PROXY_LOGIN_SYS_ID, Value = para.ProxyLoginSystemID });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.TARGET_URL, Value = para.TargetUrl });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.EXAPI_ERROR_CODE, Value = para.ExApiErrorCode });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.EXAPI_ERROR_DESC, Value = para.ExApiErrorDesc });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.API_NO, Value = para.APINo });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.UPD_USER_ID.ToString(), Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.UPD_USER_NM, Value = para.UpdUserNM });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.UPD_DT, Value = para.UpdDT });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.EXEC_SYS_ID, Value = para.ExecSysID });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.EXEC_SYS_NM, Value = para.ExecSysNM });
            dbParameters.Add(new DBParameter { Name = RecordUserLoginPara.ParaField.EXEC_IP_ADDRESS, Value = para.ExecIPAddress });

            bool result = Insert(command, dbParameters);
            return (result) ? EnumRecordUserLoginResult.Success : EnumRecordUserLoginResult.Failure;
        }


        public class RecordUserPWDPara : MongoElement
        {
            public enum ParaField
            {
                USER_ID,
                USER_NM,
                LOG_NO,
                USER_PWD,
                MODIFY_DATE,
                IS_RESET,
                API_NO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UserPWD;
            public DBChar ModifyDate;
            public DBChar IsReset;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public enum EnumRecordUserPWDResult
        {
            Success,
            Failure
        }

        public EnumRecordUserPWDResult RecordUserPWD(RecordUserPWDPara para)
        {
            MongoCommand command = new MongoCommand(EnumLogDocName.LOG_USER_PWD.ToString());
            DBChar logNo = new DBChar(InitLogNo);

            #region Get Log No
            command.SetRowCount(1);
            command.AddFields(EnumSpecifiedFieldType.Select, UserLogNo.DataField.LOG_NO.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, UserLogNo.DataField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            command.AddSortBy(EnumSortType.DESC, UserLogNo.DataField.LOG_NO.ToString());

            DBParameters dbParameters = new DBParameters();
            List<SysLogNo> sysLogNoList = Select<SysLogNo>(command);
            if (sysLogNoList != null &&
                sysLogNoList.Count == 1)
            {
                int tempLogNo = 0;
                if (sysLogNoList[0].LogNo != null &&
                    int.TryParse(sysLogNoList[0].LogNo.GetValue(), out tempLogNo))
                {
                    logNo = new DBChar((tempLogNo + 1).ToString().PadLeft(6, '0'));
                }
            }

            command.Clear();
            dbParameters.Clear();
            #endregion

            dbParameters = new DBParameters();
            dbParameters.Add(new DBParameter { Name = RecordUserPWDPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = RecordUserPWDPara.ParaField.USER_NM, Value = para.UserNM });
            dbParameters.Add(new DBParameter { Name = RecordUserPWDPara.ParaField.LOG_NO, Value = logNo });
            dbParameters.Add(new DBParameter { Name = RecordUserPWDPara.ParaField.USER_PWD, Value = para.UserPWD });
            dbParameters.Add(new DBParameter { Name = RecordUserPWDPara.ParaField.MODIFY_DATE, Value = para.ModifyDate });
            dbParameters.Add(new DBParameter { Name = RecordUserPWDPara.ParaField.IS_RESET, Value = para.IsReset });
            dbParameters.Add(new DBParameter { Name = RecordUserPWDPara.ParaField.API_NO, Value = para.APINo });
            dbParameters.Add(new DBParameter { Name = RecordUserPWDPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = RecordUserPWDPara.ParaField.UPD_USER_NM, Value = para.UpdUserNM });
            dbParameters.Add(new DBParameter { Name = RecordUserPWDPara.ParaField.UPD_DT, Value = para.UpdDT });
            dbParameters.Add(new DBParameter { Name = RecordUserPWDPara.ParaField.EXEC_SYS_ID, Value = para.ExecSysID });
            dbParameters.Add(new DBParameter { Name = RecordUserPWDPara.ParaField.EXEC_SYS_NM, Value = para.ExecSysNM });
            dbParameters.Add(new DBParameter { Name = RecordUserPWDPara.ParaField.EXEC_IP_ADDRESS, Value = para.ExecIPAddress });

            bool result = Insert(command, dbParameters);
            return (result) ? EnumRecordUserPWDResult.Success : EnumRecordUserPWDResult.Failure;
        }


        public class RecordUserAccessPara : MongoElement
        {
            public enum ParaField
            {
                USER_ID,
                USER_NM,
                LOG_NO,
                RESTRICT_TYPE,
                RESTRICT_TYPE_NM,
                IS_LOCK,
                IS_DISABLE,
                API_NO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;

            public DBChar RestrictType;
            public DBNVarChar RestrictTypeNM;
            public DBChar IsLock;
            public DBChar IsDisable;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public enum EnumRecordUserAccessResult
        {
            Success,
            Failure
        }

        public EnumRecordUserAccessResult RecordUserAccess(RecordUserAccessPara para)
        {
            MongoCommand command = new MongoCommand(EnumLogDocName.LOG_USER_ACCESS.ToString());
            DBChar logNo = new DBChar(InitLogNo);

            #region Get Log No
            command.SetRowCount(1);
            command.AddFields(EnumSpecifiedFieldType.Select, UserLogNo.DataField.LOG_NO.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, UserLogNo.DataField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            command.AddSortBy(EnumSortType.DESC, UserLogNo.DataField.LOG_NO.ToString());

            DBParameters dbParameters = new DBParameters();
            List<SysLogNo> sysLogNoList = Select<SysLogNo>(command);
            if (sysLogNoList != null &&
                sysLogNoList.Count == 1)
            {
                int tempLogNo = 0;
                if (sysLogNoList[0].LogNo != null &&
                    int.TryParse(sysLogNoList[0].LogNo.GetValue(), out tempLogNo))
                {
                    logNo = new DBChar((tempLogNo + 1).ToString().PadLeft(6, '0'));
                }
            }

            command.Clear();
            dbParameters.Clear();
            #endregion

            dbParameters = new DBParameters();
            dbParameters.Add(new DBParameter { Name = RecordUserAccessPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = RecordUserAccessPara.ParaField.USER_NM, Value = para.UserNM });
            dbParameters.Add(new DBParameter { Name = RecordUserAccessPara.ParaField.LOG_NO, Value = logNo });
            dbParameters.Add(new DBParameter { Name = RecordUserAccessPara.ParaField.RESTRICT_TYPE, Value = para.RestrictType });
            dbParameters.Add(new DBParameter { Name = RecordUserAccessPara.ParaField.RESTRICT_TYPE_NM, Value = para.RestrictTypeNM });
            dbParameters.Add(new DBParameter { Name = RecordUserAccessPara.ParaField.IS_LOCK, Value = para.IsLock });
            dbParameters.Add(new DBParameter { Name = RecordUserAccessPara.ParaField.IS_DISABLE, Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = RecordUserAccessPara.ParaField.API_NO, Value = para.APINo });
            dbParameters.Add(new DBParameter { Name = RecordUserAccessPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = RecordUserAccessPara.ParaField.UPD_USER_NM, Value = para.UpdUserNM });
            dbParameters.Add(new DBParameter { Name = RecordUserAccessPara.ParaField.UPD_DT, Value = para.UpdDT });
            dbParameters.Add(new DBParameter { Name = RecordUserAccessPara.ParaField.EXEC_SYS_ID, Value = para.ExecSysID });
            dbParameters.Add(new DBParameter { Name = RecordUserAccessPara.ParaField.EXEC_SYS_NM, Value = para.ExecSysNM });
            dbParameters.Add(new DBParameter { Name = RecordUserAccessPara.ParaField.EXEC_IP_ADDRESS, Value = para.ExecIPAddress });

            bool result = Insert(command, dbParameters);
            return (result) ? EnumRecordUserAccessResult.Success : EnumRecordUserAccessResult.Failure;
        }


        #region - Record Log Utility -

        #region - LOG_SYS_SYSTEM_LINE_RECEIVER -
        public class RecordLogSystemLineReceiverPara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;

            [DBTypeProperty("SYS_NM")]
            public DBNVarChar SysNM;

            [Primarykey]
            [DBTypeProperty("LINE_ID")]
            public DBVarChar LineID;

            [DBTypeProperty("LINE_NM")]
            public DBNVarChar LineNM;

            [DBTypeProperty("LINE_RECEIVER_ID")]
            public DBVarChar LineReceiverID;

            [DBTypeProperty("LINE_RECEIVER_NM_ZH_TW")]
            public DBNVarChar LineReceiverNMZHTW;

            [DBTypeProperty("LINE_RECEIVER_NM_ZH_CN")]
            public DBNVarChar LineReceiverNMZHCN;

            [DBTypeProperty("LINE_RECEIVER_NM_EN_US")]
            public DBNVarChar LineReceiverNMENUS;

            [DBTypeProperty("LINE_RECEIVER_NM_TH_TH")]
            public DBNVarChar LineReceiverNMTHTH;

            [DBTypeProperty("LINE_RECEIVER_NM_JA_JP")]
            public DBNVarChar LineReceiverNMJAJP;

            [DBTypeProperty("LINE_RECEIVER_NM_KO_KR")]
            public DBNVarChar LineReceiverNMKOKR;

            [DBTypeProperty("IS_DISABLE")]
            public DBChar IsDisable;

            [Primarykey]
            [DBTypeProperty("RECEIVER_ID")]
            public DBVarChar ReceiverID;

            [DBTypeProperty("SOURCE_TYPE")]
            public DBVarChar SourceType;
        }
        #endregion

        #region - LOG_SYS_SYSTEM_LINE -
        public class RecordLogSystemLinePara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;

            [DBTypeProperty("SYS_NM")]
            public DBNVarChar SysNM;

            [Primarykey]
            [DBTypeProperty("LINE_ID")]
            public DBVarChar LineID;

            [DBTypeProperty("LINE_NM_ZH_TW")]
            public DBNVarChar LineNMZHTW;

            [DBTypeProperty("LINE_NM_ZH_CN")]
            public DBNVarChar LineNMZHCN;

            [DBTypeProperty("LINE_NM_EN_US")]
            public DBNVarChar LineNMENUS;

            [DBTypeProperty("LINE_NM_TH_TH")]
            public DBNVarChar LineNMTHTH;

            [DBTypeProperty("LINE_NM_JA_JP")]
            public DBNVarChar LineNMJAJP;

            [DBTypeProperty("LINE_NM_KO_KR")]
            public DBNVarChar LineNMKOKR;

            [DBTypeProperty("CHANNEL_ID")]
            public DBVarChar ChannelID;

            [DBTypeProperty("CHANNEL_SECRET")]
            public DBVarChar ChannelSecret;

            [DBTypeProperty("CHANNEL_ACCESS_TOKEN")]
            public DBVarChar ChannelAccessToken;

            [DBTypeProperty("IS_DISABLE")]
            public DBVarChar IsDisable;

            [DBTypeProperty("SORT_ORDER")]
            public DBVarChar SortOrder;
        }
        #endregion

        #region - LOG_SYS_SYSTEM_WF_FLOW_GROUP -
        public class RecordLogSysSystemWFFlowGroupPara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;
            [Primarykey]
            [DBTypeProperty("WF_FLOW_GROUP_ID")]
            public DBVarChar WFFlowGroupID;

            [DBTypeProperty("WF_FLOW_GROUP_ZH_TW")]
            public DBNVarChar WFFlowGroupZHTW;
            [DBTypeProperty("WF_FLOW_GROUP_ZH_CN")]
            public DBNVarChar WFFlowGroupZHCN;
            [DBTypeProperty("WF_FLOW_GROUP_EN_US")]
            public DBNVarChar WFFlowGroupENUS;
            [DBTypeProperty("WF_FLOW_GROUP_TH_TH")]
            public DBNVarChar WFFlowGroupTHTH;
            [DBTypeProperty("WF_FLOW_GROUP_JA_JP")]
            public DBNVarChar WFFlowGroupJAJP;
            [DBTypeProperty("WF_FLOW_GROUP_KO_KR")]
            public DBNVarChar WFFlowGroupKOKR;
            [DBTypeProperty("SORT_ORDER")]
            public DBVarChar SortOrder;
        }
        #endregion

        #region - LOG_SYS_SYSTEM_WF_FLOW -
        public class RecordLogSysSystemWFFlowPara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;
            [DBTypeProperty("WF_FLOW_GROUP_ID")]
            public DBVarChar WFFlowGroupID;
            [Primarykey]
            [DBTypeProperty("WF_FLOW_ID")]
            public DBVarChar WFFlowID;
            [Primarykey]
            [DBTypeProperty("WF_FLOW_VER")]
            public DBChar WFFlowVer;

            [DBTypeProperty("WF_FLOW_ZH_TW")]
            public DBNVarChar WFFlowZHTW;
            [DBTypeProperty("WF_FLOW_ZH_CN")]
            public DBNVarChar WFFlowZHCN;
            [DBTypeProperty("WF_FLOW_EN_US")]
            public DBNVarChar WFFlowENUS;
            [DBTypeProperty("WF_FLOW_TH_TH")]
            public DBNVarChar WFFlowTHTH;
            [DBTypeProperty("WF_FLOW_JA_JP")]
            public DBNVarChar WFFlowJAJP;
            [DBTypeProperty("WF_FLOW_KO_KR")]
            public DBNVarChar WFFlowKOKR;

            [DBTypeProperty("FLOW_TYPE")]
            public DBVarChar FlowType;
            [DBTypeProperty("FLOW_MAN_USER_ID")]
            public DBVarChar FlowManUserID;
            [DBTypeProperty("ENABLE_DATE")]
            public DBChar EnableDate;
            [DBTypeProperty("DISABLE_DATE")]
            public DBChar DisableDate;
            [DBTypeProperty("IS_START_FUN")]
            public DBChar IsStartFun;

            [DBTypeProperty("SORT_ORDER")]
            public DBVarChar SortOrder;
            [DBTypeProperty("REMARK")]
            public DBNVarChar Remark;
        }
        #endregion

        #region - LOG_SYS_SYSTEM_WF_NODE -
        public class RecordLogSysSystemWFNodePara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;
            [Primarykey]
            [DBTypeProperty("WF_FLOW_ID")]
            public DBVarChar WFFlowID;
            [Primarykey]
            [DBTypeProperty("WF_FLOW_VER")]
            public DBChar WFFlowVer;
            [Primarykey]
            [DBTypeProperty("WF_NODE_ID")]
            public DBVarChar WFNodeID;

            [DBTypeProperty("WF_NODE_ZH_TW")]
            public DBNVarChar WFNodeZHTW;
            [DBTypeProperty("WF_NODE_ZH_CN")]
            public DBNVarChar WFNodeZHCN;
            [DBTypeProperty("WF_NODE_EN_US")]
            public DBNVarChar WFNodeENUS;
            [DBTypeProperty("WF_NODE_TH_TH")]
            public DBNVarChar WFNodeTHTH;
            [DBTypeProperty("WF_NODE_JA_JP")]
            public DBNVarChar WFNodeJAJP;
            [DBTypeProperty("WF_NODE_KO_KR")]
            public DBNVarChar WFNodeKOKR;
            [DBTypeProperty("NODE_TYPE")]
            public DBVarChar NodeType;
            [DBTypeProperty("NODE_SEQ_X")]
            public DBInt NodeSeqX;
            [DBTypeProperty("NODE_SEQ_Y")]
            public DBInt NodeSeqY;
            [DBTypeProperty("NODE_POS_BEGIN_X")]
            public DBInt NodePosBeginX;
            [DBTypeProperty("NODE_POS_BEGIN_Y")]
            public DBInt NodePosBeginY;
            [DBTypeProperty("NODE_POS_END_X")]
            public DBInt NodePosEndX;
            [DBTypeProperty("NODE_POS_END_Y")]
            public DBInt NodePosEndY;
            [DBTypeProperty("IS_FIRST")]
            public DBChar IsFirst;
            [DBTypeProperty("IS_FINALLY")]
            public DBChar IsFinally;
            [DBTypeProperty("BACK_WF_NODE_ID")]
            public DBVarChar BackWFNodeID;
            [DBTypeProperty("WF_SIG_MEMO_ZH_TW")]
            public DBNVarChar WFSigMemoZHTW;
            [DBTypeProperty("WF_SIG_MEMO_ZH_CN")]
            public DBNVarChar WFSigMemoZHCN;
            [DBTypeProperty("WF_SIG_MEMO_EN_US")]
            public DBNVarChar WFSigMemoENUS;
            [DBTypeProperty("WF_SIG_MEMO_TH_TH")]
            public DBNVarChar WFSigMemoTHTH;
            [DBTypeProperty("WF_SIG_MEMO_JA_JP")]
            public DBNVarChar WFSigMemoJAJP;
            [DBTypeProperty("WF_SIG_MEMO_KO_KR")]
            public DBNVarChar WFSigMemoKOKR;
            [DBTypeProperty("FUN_SYS_ID")]
            public DBVarChar FunSysID;
            [DBTypeProperty("FUN_CONTROLLER_ID")]
            public DBVarChar FunControllerID;
            [DBTypeProperty("FUN_ACTION_NAME")]
            public DBVarChar FunActionName;
            [DBTypeProperty("SIG_API_SYS_ID")]
            public DBVarChar SigApiSysID;
            [DBTypeProperty("SIG_API_CONTROLLER_ID")]
            public DBVarChar SigApiControllerID;
            [DBTypeProperty("SIG_API_ACTION_NAME")]
            public DBVarChar SigApiActionName;
            [DBTypeProperty("CHK_API_SYS_ID")]
            public DBVarChar ChkApiSysID;
            [DBTypeProperty("CHK_API_CONTROLLER_ID")]
            public DBVarChar ChkApiControllerID;
            [DBTypeProperty("CHK_API_ACTION_NAME")]
            public DBVarChar ChkApiActionName;
            [DBTypeProperty("ASSG_API_SYS_ID")]
            public DBVarChar AssgAPISysID;
            [DBTypeProperty("ASSG_API_CONTROLLER_ID")]
            public DBVarChar AssgAPIControllerID;
            [DBTypeProperty("ASSG_API_ACTION_NAME")]
            public DBVarChar AssgAPIActionName;
            [DBTypeProperty("IS_SIG_NEXT_NODE")]
            public DBChar IsSigNextNode;
            [DBTypeProperty("IS_SIG_BACK_NODE")]
            public DBChar IsSigBackNode;
            [DBTypeProperty("IS_ASSG_NEXT_NODE")]
            public DBChar IsAssgNextNode;
            [DBTypeProperty("SORT_ORDER")]
            public DBVarChar SortOrder;
            [DBTypeProperty("REMARK")]
            public DBNVarChar Remark;
        }
        #endregion

        #region - LOG_SYS_SYSTEM_WF_NEXT -
        public class RecordLogSysSystemWFNextPara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;
            [Primarykey]
            [DBTypeProperty("WF_FLOW_ID")]
            public DBVarChar WFFlowID;
            [Primarykey]
            [DBTypeProperty("WF_FLOW_VER")]
            public DBChar WFFlowVer;
            [Primarykey]
            [DBTypeProperty("WF_NODE_ID")]
            public DBVarChar WFNodeID;
            [Primarykey]
            [DBTypeProperty("NEXT_WF_NODE_ID")]
            public DBVarChar NextWFNodeID;
            [DBTypeProperty("NEXT_RESULT_VALUE")]
            public DBVarChar NextResultValue;
            [DBTypeProperty("SORT_ORDER")]
            public DBVarChar SortOrder;
            [DBTypeProperty("REMARK")]
            public DBNVarChar Remark;
        }
        #endregion

        #region - LOG_SYS_SYSTEM_ROLE_FLOW -
        public class RecordLogSysSystemRoleFlowPara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;
            [Primarykey]
            [DBTypeProperty("ROLE_ID")]
            public DBVarChar RoleID;
            [Primarykey]
            [DBTypeProperty("WF_FLOW_VER")]
            public DBChar WFFlowVer;
            [DBTypeProperty("WF_FLOW_ID")]
            public DBVarChar WFFlowID;
        }
        #endregion

        #region - LOG_SYS_SYSTEM_ROLE_NODE -
        public class RecordLogSysSystemRoleNodePara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;
            [Primarykey]
            [DBTypeProperty("WF_FLOW_ID")]
            public DBVarChar WFFlowID;
            [Primarykey]
            [DBTypeProperty("WF_FLOW_VER")]
            public DBChar WFFlowVer;
            [Primarykey]
            [DBTypeProperty("WF_NODE_ID")]
            public DBChar WFNodeID;
            [DBTypeProperty("ROLE_ID")]
            public DBVarChar RoleID;
        }
        #endregion

        #region - LOG_SYS_SYSTEM_WF_SIG -
        public class RecordLogSysSystemWFSigPara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;
            [Primarykey]
            [DBTypeProperty("WF_FLOW_ID")]
            public DBVarChar WFFlowID;
            [Primarykey]
            [DBTypeProperty("WF_FLOW_VER")]
            public DBChar WFFlowVer;
            [Primarykey]
            [DBTypeProperty("WF_NODE_ID")]
            public DBVarChar WFNodeID;
            [Primarykey]
            [DBTypeProperty("WF_SIG_SEQ")]
            public DBChar WFSigSeq;

            [DBTypeProperty("SIG_STEP")]
            public DBInt SigStep;
            [DBTypeProperty("WF_SIG_ZH_TW")]
            public DBNVarChar WFSigZHTW;
            [DBTypeProperty("WF_SIG_ZH_CN")]
            public DBNVarChar WFSigZHCN;
            [DBTypeProperty("WF_SIG_EN_US")]
            public DBNVarChar WFSigENUS;
            [DBTypeProperty("WF_SIG_TH_TH")]
            public DBNVarChar WFSigTHTH;
            [DBTypeProperty("WF_SIG_JA_JP")]
            public DBNVarChar WFSigJAJP;
            [DBTypeProperty("WF_SIG_KO_KR")]
            public DBNVarChar WFSigKOKR;
            [DBTypeProperty("SIG_TYPE")]
            public DBVarChar SigType;
            [DBTypeProperty("API_SYS_ID")]
            public DBVarChar APISysID;
            [DBTypeProperty("API_CONTROLLER_ID")]
            public DBVarChar APIControllerID;
            [DBTypeProperty("API_ACTION_NAME")]
            public DBVarChar APIActionName;
            [DBTypeProperty("COMPARE_WF_NODE_ID")]
            public DBVarChar CompareWFNodeID;
            [DBTypeProperty("COMPARE_WF_SIG_SEQ")]
            public DBChar CompareWFSigSeq;
            [DBTypeProperty("CHK_API_SYS_ID")]
            public DBVarChar ChkAPISysID;
            [DBTypeProperty("CHK_API_CONTROLLER_ID")]
            public DBVarChar ChkAPIControllerID;
            [DBTypeProperty("CHK_API_ACTION_NAME")]
            public DBVarChar ChkAPIActionName;
            [DBTypeProperty("REMARK")]
            public DBNVarChar Remark;
        }
        #endregion

        #region - LOG_SYS_SYSTEM_ROLE_SIG -
        public class RecordLogSysSystemRoleSigPara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;
            [Primarykey]
            [DBTypeProperty("WF_FLOW_ID")]
            public DBVarChar WFFlowID;
            [Primarykey]
            [DBTypeProperty("WF_FLOW_VER")]
            public DBChar WFFlowVer;
            [Primarykey]
            [DBTypeProperty("WF_NODE_ID")]
            public DBVarChar WFNodeID;
            [Primarykey]
            [DBTypeProperty("WF_SIG_SEQ")]
            public DBChar WFSigSeq;
            [Primarykey]
            [DBTypeProperty("ROLE_ID")]
            public DBVarChar RoleID;
        }
        #endregion

        #region - LOG_SYS_SYSTEM_WF_DOC -
        public class RecordLogSysSystemWFDocPara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;
            [Primarykey]
            [DBTypeProperty("WF_FLOW_ID")]
            public DBVarChar WFFlowID;
            [Primarykey]
            [DBTypeProperty("WF_FLOW_VER")]
            public DBChar WFFlowVer;
            [Primarykey]
            [DBTypeProperty("WF_NODE_ID")]
            public DBVarChar WFNodeID;
            [Primarykey]
            [DBTypeProperty("WF_DOC_SEQ")]
            public DBChar WFDocSeq;

            [DBTypeProperty("WF_DOC_ZH_TW")]
            public DBNVarChar WFDocZHTW;
            [DBTypeProperty("WF_DOC_ZH_CN")]
            public DBNVarChar WFDocZHCN;
            [DBTypeProperty("WF_DOC_EN_US")]
            public DBNVarChar WFDocENUS;
            [DBTypeProperty("WF_DOC_TH_TH")]
            public DBNVarChar WFDocTHTH;
            [DBTypeProperty("WF_DOC_JA_JP")]
            public DBNVarChar WFDocJAJP;
            [DBTypeProperty("WF_DOC_KO_KR")]
            public DBNVarChar WFDocKOKR;

            [DBTypeProperty("IS_REQ")]
            public DBChar IsReq;
            [DBTypeProperty("REMARK")]
            public DBNVarChar Remark;
        }
        #endregion

        #region - LOG_SYS_SYSTEM_ROLE_CONDTION -
        public class RecordLogSystemRoleCondotionPara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;

            [DBTypeProperty("SYS_NM")]
            public DBNVarChar SysNM;

            [Primarykey]
            [DBTypeProperty("ROLE_CONDITION_ID")]
            public DBVarChar RoleConditionID;

            [DBTypeProperty("ROLES")]
            public List<DBVarChar> Roles;

            [DBTypeProperty("ROLE_CONDITION_NM_ZH_TW")]
            public DBVarChar RoleConditionNMZHTW;

            [DBTypeProperty("ROLE_CONDITION_NM_ZH_CN")]
            public DBVarChar RoleConditionNMZHCN;

            [DBTypeProperty("ROLE_CONDITION_NM_EN_US")]
            public DBVarChar RoleConditionNMENUS;

            [DBTypeProperty("ROLE_CONDITION_NM_TH_TH")]
            public DBVarChar RoleConditionNMTHTH;

            [DBTypeProperty("ROLE_CONDITION_NM_JA_JP")]
            public DBVarChar RoleConditionNMJAJP;

            [DBTypeProperty("ROLE_CONDITION_NM_KO_KR")]
            public DBVarChar RoleConditionNMKOKR;

            [DBTypeProperty("ROLE_CONDITION_SYNTAX")]
            public DBNVarChar RoleConditionSynTax;

            [DBTypeProperty("SORT_ORDER")]
            public DBVarChar SortOrder;

            [DBTypeProperty("REMARK")]
            public DBVarChar Remark;

            [DBTypeProperty("ROLE_CONDITION_RULES")]
            public RecordLogSystemRoleConditionGroupRule RoleConditionRules;
        }

        public class RecordLogSystemRoleConditionGroupRule : MongoDocument
        {
            [DBTypeProperty("CONDITION")]
            public DBVarChar Condition;

            [DBTypeProperty("RULE_LIST")]
            public List<RecordLogSystemRoleConditionRoleRule> RuleList;

            [DBTypeProperty("GROUP_RULE_LIST")]
            public List<RecordLogSystemRoleConditionGroupRule> GroupRuleList;
        }

        public class RecordLogSystemRoleConditionRoleRule : MongoDocument
        {
            public DBVarChar ID;

            [DBTypeProperty("FIELD")]
            public DBVarChar Field;

            [DBTypeProperty("FIELD_TYPE")]
            public DBVarChar FieldType;

            [DBTypeProperty("INPUT")]
            public DBVarChar Input;

            [DBTypeProperty("OPERATOR")]
            public DBVarChar Operator;

            [DBTypeProperty("VALUE")]
            public DBVarChar Value;
        }
        #endregion

        #region - LOG_USER_ACCOUNT -
        public class RecordLogUserAccountPara : RecordLogPara
        {
            [DBTypeProperty("USER_ID")]
            public DBVarChar UserID;
            [DBTypeProperty("USER_NM")]
            public DBNVarChar UserNM;
            [DBTypeProperty("IS_LEFT")]
            public DBChar IsLeft;
            [DBTypeProperty("JOIN_DATE")]
            public DBChar JoinDate;

            [DBTypeProperty("USER_COM_ID")]
            public DBVarChar UserComID;
            [DBTypeProperty("USER_COM_NM")]
            public DBNVarChar UserComNM;
            [DBTypeProperty("USER_SALARY_COM_ID")]
            public DBVarChar UserSalaryComID;
            [DBTypeProperty("USER_SALARY_COM_NM")]
            public DBNVarChar UserSalaryComNM;

            [DBTypeProperty("USER_UNIT_ID")]
            public DBVarChar UserUnitID;
            [DBTypeProperty("USER_UNIT_NM")]
            public DBNVarChar UserUnitNM;

            [DBTypeProperty("USER_TEAM_ID")]
            public DBVarChar UserTeamID;

            [DBTypeProperty("USER_TITLE_ID")]
            public DBVarChar UserTitleID;
            [DBTypeProperty("USER_TITLE_NM")]
            public DBNVarChar UserTitleNM;

            [DBTypeProperty("USER_WORK_ID")]
            public DBVarChar UserWorkID;
            [DBTypeProperty("USER_WORK_NM")]
            public DBNVarChar UserWorkNM;

            [DBTypeProperty("USER_ORG_WORKCOM")]
            public DBVarChar UserOrgWorkCom;
            [DBTypeProperty("USER_ORG_WORKCOM_NM")]
            public DBNVarChar UserOrgWorkComNM;

            [DBTypeProperty("USER_ORG_AREA")]
            public DBVarChar UserOrgArea;
            [DBTypeProperty("USER_ORG_AREA_NM")]
            public DBNVarChar UserOrgAreaNM;

            [DBTypeProperty("USER_ORG_GROUP")]
            public DBVarChar UserOrgGroup;
            [DBTypeProperty("USER_ORG_GROUP_NM")]
            public DBNVarChar UserOrgGroupNM;

            [DBTypeProperty("USER_ORG_PLACE")]
            public DBVarChar UserOrgPlace;
            [DBTypeProperty("USER_ORG_PLACE_NM")]
            public DBNVarChar UserOrgPlaceNM;

            [DBTypeProperty("USER_ORG_DEPT")]
            public DBVarChar UserOrgDept;
            [DBTypeProperty("USER_ORG_DEPT_NM")]
            public DBNVarChar UserOrgDeptNM;

            [DBTypeProperty("USER_ORG_TEAM")]
            public DBVarChar UserOrgTeam;
            [DBTypeProperty("USER_ORG_TEAM_NM")]
            public DBNVarChar UserOrgTeamNM;

            [DBTypeProperty("USER_ORG_JOB_TITLE")]
            public DBVarChar UserOrgJobTitle;
            [DBTypeProperty("USER_ORG_JOB_TITLE_NM")]
            public DBNVarChar UserOrgJobTitleNM;

            [DBTypeProperty("USER_ORG_BIZ_TITLE")]
            public DBVarChar UserOrgBizTitle;
            [DBTypeProperty("USER_ORG_BIZ_TITLE_NM")]
            public DBNVarChar UserOrgBizTitleNM;

            [DBTypeProperty("USER_ORG_TITLE")]
            public DBVarChar UserOrgTitle;
            [DBTypeProperty("USER_ORG_TITLE_NM")]
            public DBNVarChar UserOrgTitleNM;

            [DBTypeProperty("USER_ORG_LEVEL")]
            public DBVarChar UsreOrgLevel;
            [DBTypeProperty("USER_ORG_LEVEL_NM")]
            public DBNVarChar UsreOrgLevelNM;
        }
        #endregion

        #region - LOG_USER_PURVIEW -
        public class RecordLogUserPurviewPara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("PURVIEW_ID")]
            public DBVarChar PurviewID;

            [DBTypeProperty("PURVIEW_NM")]
            public DBNVarChar PurviewNM;

            [Primarykey]
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;

            [DBTypeProperty("SYS_NM")]
            public DBNVarChar SysNM;

            [Primarykey]
            [DBTypeProperty("USER_ID")]
            public DBVarChar UserID;

            [DBTypeProperty("USER_NM")]
            public DBNVarChar UserNM;

            [DBTypeProperty("PURVIEW_COLLECT_LIST")]
            public List<LOG_SYS_USER_PURVIEW_COLLECT> PurviewCollectList;
        }

        public class LOG_SYS_USER_PURVIEW_COLLECT : MongoDocument
        {
            [Primarykey]
            [DBTypeProperty("CODE_ID")]
            public DBVarChar CodeID;

            [DBTypeProperty("CODE_NM")]
            public DBNVarChar CodeNM;

            [Primarykey]
            [DBTypeProperty("CODE_TYPE")]
            public DBVarChar CodeType;

            [DBTypeProperty("CODE_TYPE_NM")]
            public DBNVarChar CodeTypeNM;

            [DBTypeProperty("PURVIEW_OP")]
            public DBChar PurviewOP;

            [DBTypeProperty("PURVIEW_OP_NM")]
            public DBNVarChar PurviewOPNM;
        }
        #endregion

        #region - LOG_USER_SYSTEM_ROLE_APPLY -
        public class RecordLogUserSystemRoleApplyPara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("USER_ID")]
            public DBVarChar UserID;

            [DBTypeProperty("USER_NM")]
            public DBNVarChar UserNM;

            [DBTypeProperty("WFNO")]
            public DBVarChar WFNO;

            [DBTypeProperty("MEMO")]
            public DBVarChar Memo;

            [DBTypeProperty("MODIFY_LIST")]
            public List<UserSystemRoleModify> ModifyList;

            [DBTypeProperty("BaseLine_DT")]
            public DBDateTime BaseLineDT;
        }

        public class UserSystemRoleModify : MongoDocument
        {
            [Primarykey]
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;

            [DBTypeProperty("SYS_NM")]
            public DBNVarChar SysNM;

            [DBTypeProperty("ROLE_ID")]
            public DBVarChar RoleID;

            [DBTypeProperty("ROLE_NM")]
            public DBNVarChar RoleNM;

            [DBTypeProperty("MOTIFY_TYPE")]
            public DBChar ModifyType;

            [DBTypeProperty("MODIFY_TYPE_NM")]
            public DBNVarChar ModifyTypeNM;
        }
        #endregion

        #region - LOG_USER_FUN -
        public class RecordUserFunctionPara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("USER_ID")]
            public DBVarChar UserID;

            [DBTypeProperty("USER_NM")]
            public DBNVarChar UserNM;

            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;

            [DBTypeProperty("SYS_NM")]
            public DBNVarChar SysNM;

            [DBTypeProperty("FUN_CONTROLLER_ID")]
            public DBVarChar FunControllerID;

            [DBTypeProperty("FUN_CONTROLLER_NM")]
            public DBNVarChar FunGroupNM;

            [DBTypeProperty("FUN_ACTION_NAME")]
            public DBVarChar FunActionNM;

            [DBTypeProperty("FUN_NM")]
            public DBNVarChar FunNM;

            [DBTypeProperty("ERP_WFNO")]
            public DBVarChar ErpWFNO;

            [DBTypeProperty("MEMO")]
            public DBNVarChar Memo;
        }
        #endregion

        #region - LOG_USER_FUN_APPLY -
        public class RecordLogUserFunApplyPara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("USER_ID")]
            public DBVarChar UserID;

            [DBTypeProperty("USER_NM")]
            public DBNVarChar UserNM;

            [DBTypeProperty("WFNO")]
            public DBVarChar WFNO;

            [DBTypeProperty("MEMO")]
            public DBVarChar Memo;

            [DBTypeProperty("MODIFY_LIST")]
            public List<UserFunModify> ModifyList;

            [DBTypeProperty("BaseLine_DT")]
            public DBDateTime BaseLineDT;
        }

        public class UserFunModify : MongoDocument
        {
            [Primarykey]
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;

            [DBTypeProperty("SYS_NM")]
            public DBNVarChar SysNM;

            [DBTypeProperty("FUN_CONTROLLER_ID")]
            public DBVarChar FunControllerID;

            [DBTypeProperty("FUN_CONTROLLER_NM")]
            public DBNVarChar FunControllerNM;

            [DBTypeProperty("FUN_ACTION_NAME")]
            public DBVarChar FunActionNM;

            [DBTypeProperty("FUN_NM")]
            public DBNVarChar FunNM;

            [DBTypeProperty("MOTIFY_TYPE")]
            public DBChar ModifyType;

            [DBTypeProperty("MODIFY_TYPE_NM")]
            public DBNVarChar ModifyTypeNM;
        }
        #endregion

        #region - LOG_USER_PURVIEW_APPLY -
        public class RecordLogUserPurviewApplyPara : RecordLogPara
        {
            [Primarykey]
            [DBTypeProperty("USER_ID")]
            public DBVarChar UserID;

            [DBTypeProperty("USER_NM")]
            public DBNVarChar UserNM;

            [DBTypeProperty("WFNO")]
            public DBVarChar WFNO;

            [DBTypeProperty("MEMO")]
            public DBVarChar Memo;

            [DBTypeProperty("MODIFY_LIST")]
            public List<UserPurviewModify> ModifyList;
        }

        public class UserPurviewModify : MongoDocument
        {
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;

            [DBTypeProperty("SYS_NM")]
            public DBNVarChar SysNM;

            [DBTypeProperty("PURVIEW_ID")]
            public DBVarChar PurviewID;

            [DBTypeProperty("PURVIEW_NM")]
            public DBNVarChar PurviewNM;

            [DBTypeProperty("CODE_ID")]
            public DBVarChar CodeID;

            [DBTypeProperty("CODE_NM")]
            public DBNVarChar CodeNM;

            [DBTypeProperty("CODE_TYPE")]
            public DBVarChar CodeType;

            [DBTypeProperty("CODE_TYPE_NM")]
            public DBNVarChar CodeTypeNM;

            [DBTypeProperty("PURVIEW_OP")]
            public DBChar PurviewOP;

            [DBTypeProperty("PURVIEW_OP_NM")]
            public DBNVarChar PurviewOPNM;

            [DBTypeProperty("MOTIFY_TYPE")]
            public DBChar ModifyType;

            [DBTypeProperty("MODIFY_TYPE_NM")]
            public DBNVarChar ModifyTypeNM;
        }
        #endregion

        #region - LOG_APP_USER_PUSH -
        public class RecordLogAppUserPushPara : MongoDocument
        {
            [DBTypeProperty("MESSAGE_ID")]
            public DBVarChar MessageID;

            [DBTypeProperty("APP_ID")]
            public DBVarChar AppID;

            [DBTypeProperty("APP_UUID")]
            public DBVarChar AppUUID;

            [DBTypeProperty("USER_ID")]
            public DBVarChar UserID;

            [DBTypeProperty("APP_FUN_ID")]
            public DBVarChar AppFunID;

            [DBTypeProperty("APP_ROLE_ID")]
            public DBVarChar AppRoleID;

            [DBTypeProperty("APP_ROLE_ID_LIST")]
            public List<DBVarChar> AppRoleIDList;

            [DBTypeProperty("DEVICE_TOKEN_ID")]
            public DBVarChar DeviceTokenID;

            [DBTypeProperty("DEVICE_TOKEN_IS_MASTER")]
            public DBChar DeviceTokenIsMaster;

            [DBTypeProperty("MOBILE_OS")]
            public DBVarChar MobileOS;

            [DBTypeProperty("REMIND_MINUTE")]
            public DBInt RemindMinute;

            [DBTypeProperty("IS_OPEN_PUSH")]
            public DBChar IsOpenPush;

            [DBTypeProperty("PUSH_DT")]
            public DBDateTime PushDT;

            [DBTypeProperty("PUSH_STS")]
            public DBChar PushSts;

            [DBTypeProperty("TITLE")]
            public DBNVarChar Title;

            [DBTypeProperty("BODY")]
            public DBNVarChar Body;

            [DBTypeProperty("DATA")]
            public PushMsgData Data;

            [DBTypeProperty("API_NO")]
            public DBChar APINo;

            [DBTypeProperty("ERROR_MESSAGE")]
            public DBNVarChar ErrorMessage;

            [DBTypeProperty("UPD_USER_ID")]
            public DBVarChar UpdUserID;

            [DBTypeProperty("UPD_USER_NM")]
            public DBNVarChar UpdUserNM;

            [DBTypeProperty("UPD_DT")]
            public DBDateTime UpdDT;

            [DBTypeProperty("EXEC_SYS_ID")]
            public DBVarChar ExecSysID;

            [DBTypeProperty("EXEC_SYS_NM")]
            public DBNVarChar ExecSysNM;

            [DBTypeProperty("EXEC_IP_ADDRESS")]
            public DBVarChar ExecIPAddress;
        }

        public class PushMsgData : MongoDocument
        {
            public DBVarChar SourceType;
            public DBNVarChar SourceID;
        }
        #endregion

        #region - LOG_APP_TOPIC_PUSH -
        public class RecordLogAppTopicPushPara : MongoDocument
        {
            [DBTypeProperty("MESSAGE_ID")]
            public DBVarChar MessageID;

            [DBTypeProperty("Topic_ID")]
            public DBVarChar TopicID;

            [DBTypeProperty("APP_ID")]
            public DBVarChar AppID;

            [DBTypeProperty("DEVICE_TOKEN_TYPE")]
            public DBVarChar DeviceTokenType;

            [DBTypeProperty("PUSH_DT")]
            public DBDateTime PushDT;

            [DBTypeProperty("PUSH_STS")]
            public DBChar PushSts;

            [DBTypeProperty("TITLE")]
            public DBNVarChar Title;

            [DBTypeProperty("BODY")]
            public DBNVarChar Body;

            [DBTypeProperty("API_NO")]
            public DBChar APINo;

            [DBTypeProperty("UPD_USER_ID")]
            public DBVarChar UpdUserID;

            [DBTypeProperty("UPD_USER_NM")]
            public DBNVarChar UpdUserNM;

            [DBTypeProperty("UPD_DT")]
            public DBDateTime UpdDT;

            [DBTypeProperty("EXEC_SYS_ID")]
            public DBVarChar ExecSysID;

            [DBTypeProperty("EXEC_SYS_NM")]
            public DBNVarChar ExecSysNM;

            [DBTypeProperty("EXEC_IP_ADDRESS")]
            public DBVarChar ExecIPAddress;
        }
        #endregion

        #region - LOG_APP_USER_FUN -
        public class RecordLogAppUserFunPara : MongoDocument
        {
            [DBTypeProperty("USER_ID")]
            public DBVarChar UserID;

            [DBTypeProperty("APP_UUID")]
            public DBVarChar AppUUID;

            [DBTypeProperty("APP_FUN_ID")]
            public DBVarChar AppFunID;

            [DBTypeProperty("REMIND_MINUTE")]
            public DBInt RemindMinute;

            [DBTypeProperty("IS_OPEN_PUSH")]
            public DBChar IsOpenPush;

            [DBTypeProperty("UPD_USER_ID")]
            public DBVarChar UpdUserID;

            [DBTypeProperty("UPD_USER_NM")]
            public DBNVarChar UpdUserNM;

            [DBTypeProperty("UPD_DT")]
            public DBDateTime UpdDT;

            [DBTypeProperty("EXEC_IP_ADDRESS")]
            public DBVarChar ExecIPAddress;
        }
        #endregion

        #region - LOG_APP_USER_FUN_ROLE -
        public class RecordLogAppUserFunRolePara : MongoDocument
        {
            [DBTypeProperty("USER_ID")]
            public DBVarChar UserID;

            [DBTypeProperty("APP_UUID")]
            public DBVarChar AppUUID;

            [DBTypeProperty("APP_FUN_ID")]
            public DBVarChar AppFunID;

            [DBTypeProperty("APP_ROLE_ID")]
            public DBVarChar AppRoleID;

            [DBTypeProperty("UPD_USER_ID")]
            public DBVarChar UpdUserID;

            [DBTypeProperty("UPD_USER_NM")]
            public DBNVarChar UpdUserNM;

            [DBTypeProperty("UPD_DT")]
            public DBDateTime UpdDT;

            [DBTypeProperty("EXEC_IP_ADDRESS")]
            public DBVarChar ExecIPAddress;
        }
        #endregion

        public void RecordLogWFFlow(Entity_BaseAP.LogWFFlow source)
        {
            MongoCommand command = new MongoCommand(EnumLogDocName.LOG_WF_FLOW.ToString());

            DBParameters dbParameters = new DBParameters();
            FieldInfo logNofieldInfo = source.GetType().GetFields().SingleOrDefault(s => string.Equals(s.Name.Replace("_", string.Empty), RecordLogPara.ParaField.LOG_NO.ToString().Replace("_", string.Empty), StringComparison.CurrentCultureIgnoreCase));
            logNofieldInfo.SetValue(source, new DBChar(InitLogNo));
            command.SetRowCount(1);
            command.AddFields(EnumSpecifiedFieldType.Select, RecordLogPara.ParaField.LOG_NO.ToString());

            var mongoIndexes = GetCollectionIndexes(command);

            foreach (var mongoIndex in mongoIndexes)
            {
                foreach (var mongoIndexKey in mongoIndex.Keys)
                {
                    FieldInfo fieldInfo = source.GetType().GetFields().SingleOrDefault(s => string.Equals(s.Name, mongoIndexKey.Name.Replace("_", string.Empty), StringComparison.CurrentCultureIgnoreCase));
                    if (mongoIndexKey.Name != "_id" &&
                        mongoIndexKey.Name != RecordLogPara.ParaField.LOG_NO.ToString() &&
                        fieldInfo != null &&
                        fieldInfo.FieldType.GetInterfaces().Any(a => a == typeof(IDBType)))
                    {
                        command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, mongoIndexKey.Name);
                        dbParameters.Add(new DBParameter { Name = mongoIndexKey.Name, Value = fieldInfo.GetValue(source) });
                    }
                }
            }

            command.AddSortBy(EnumSortType.DESC, RecordLogPara.ParaField.LOG_NO.ToString());
            SysLogNo sysLogNo = Select<SysLogNo>(command, dbParameters).SingleOrDefault();

            if (sysLogNo != null)
            {
                int tempLogNo;
                if (sysLogNo.LogNo != null &&
                    int.TryParse(sysLogNo.LogNo.GetValue(), out tempLogNo))
                {
                    logNofieldInfo.SetValue(source, new DBChar((tempLogNo + 1).ToString().PadLeft(6, '0')));
                }
            }

            command.Clear();
            dbParameters.Clear();

            Insert(command, source);
        }

        public void RecordLog<TLogPara>(EnumLogDocName docName, TLogPara recordPara)
            where TLogPara : RecordLogPara
        {
            MongoCommand command = new MongoCommand(docName.ToString());

            _GetLogColectionLogNo(command, recordPara);

            bool result = Insert(command, recordPara);
            if (result == false)
            {
                throw new Exception("RecordLog Insert Failure");
            }
        }

        public void RecordLog<TLogPara, TSource>(EnumLogDocName docName, TLogPara recordPara, TSource source)
            where TLogPara : RecordLogPara
        {
            MongoCommand command = new MongoCommand(docName.ToString());
            DBEntity.DBTypeMemberwiseClone(source, recordPara);

            _GetLogColectionLogNo(command, recordPara);

            bool result = Insert(command, recordPara);
            if (result == false)
            {
                throw new Exception("RecordLog Insert Failure");
            }
        }

        public void RecordLog<TLogPara, TSource>(EnumLogDocName docName, TLogPara recordPara, List<TSource> sourceList)
            where TLogPara : RecordLogPara
        {
            if (sourceList == null ||
                sourceList.Any() == false)
            {
                return;
            }

            MongoCommand command = new MongoCommand(docName.ToString());
            List<TLogPara> recordParaList = new List<TLogPara>();

            foreach (var source in sourceList)
            {
                TLogPara tempLogPara = (TLogPara)recordPara.Clone();
                DBEntity.DBTypeMemberwiseClone(source, tempLogPara);
                recordParaList.Add(tempLogPara);
            }

            var recordParaFirst = recordParaList.FirstOrDefault();
            _GetLogColectionLogNo(command, recordParaFirst);

            recordParaList.ForEach(row =>
            {
                row.LogNo = recordParaFirst.LogNo;
            });

            bool result = Insert(command, recordParaList);
            if (result == false)
            {
                throw new Exception("RecordLog Insert Failure");
            }
        }

        public Type GetRecordLogParaType(EnumLogDocName docName)
        {
            var collectionAttribute =
                (from attribute in typeof(EnumLogDocName).GetField(docName.ToString()).GetCustomAttributes(true)
                 where attribute.GetType() == typeof(CollectionAttribute)
                 select (CollectionAttribute)attribute).SingleOrDefault();

            if (collectionAttribute == null)
            {
                throw new ArgumentOutOfRangeException("docName", docName, null);
            }

            return collectionAttribute.CollectionType;
        }

        private void _GetLogColectionLogNo(MongoCommand command, object para)
        {
            FieldInfo logNofieldInfo = para.GetType().GetFields().SingleOrDefault(s => string.Equals(s.Name.Replace("_", string.Empty), RecordLogPara.ParaField.LOG_NO.ToString().Replace("_", string.Empty), StringComparison.CurrentCultureIgnoreCase));
            logNofieldInfo.SetValue(para, new DBChar(InitLogNo));
            command.SetRowCount(1);
            command.AddFields(EnumSpecifiedFieldType.Select, RecordLogPara.ParaField.LOG_NO.ToString());

            var pkFieldInfos =
                (from field in para.GetType().GetFields()
                 let dbTypeProp = (from attribute in field.GetCustomAttributes(true)
                                   where attribute.GetType() == typeof(PrimarykeyAttribute)
                                   select (PrimarykeyAttribute)attribute).SingleOrDefault()
                 let propName = (from attribute in field.GetCustomAttributes(true)
                                 where attribute.GetType() == typeof(DBTypePropertyAttribute)
                                 select ((DBTypePropertyAttribute)attribute).PropertyName).SingleOrDefault()
                 where dbTypeProp != null && string.IsNullOrWhiteSpace(propName) == false
                 select new { Name = propName, Value = field.GetValue(para) });

            foreach (var fieldInfo in pkFieldInfos)
            {
                if (fieldInfo.Name.Replace("_", string.Empty).ToLower() == "userid")
                {
                    command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, fieldInfo.Name, Utility.GetUserIDList(((DBVarChar)fieldInfo.Value)));
                }
                else
                {
                    command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, fieldInfo.Name, fieldInfo.Value);
                }
            }

            command.AddSortBy(EnumSortType.DESC, RecordLogPara.ParaField.LOG_NO.ToString());
            SysLogNo sysLogNo = Select<SysLogNo>(command).SingleOrDefault();

            if (sysLogNo != null)
            {
                int tempLogNo;
                if (sysLogNo.LogNo != null &&
                    int.TryParse(sysLogNo.LogNo.GetValue(), out tempLogNo))
                {
                    logNofieldInfo.SetValue(para, new DBChar((tempLogNo + 1).ToString().PadLeft(6, '0')));
                }
            }

            command.Clear();
        }
        #endregion

        #endregion
    }
}