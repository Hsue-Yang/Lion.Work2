using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Newtonsoft.Json;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace ERPAP.Models.Sys
{
    public class SystemRecordModel : SysModel
    {
        public class SystemRecord
        {
            public string LogNo { get; set; }
            public DateTime UpdDT { get; set; }
            public List<string> TitleList { get; set; }
            public Dictionary<string, string> ContentDict { get; set; }
        }

        public List<string> titleList { get; set; }
        public string collectionNM { get; set; }
        public List<SystemRecord> systemRecords { get; set; } = new List<SystemRecord>();

        public class LogPara
        {
            public string CollectionNM { get; set; }
            public string UserID { get; set; }
            public string SysID { get; set; }
            public List<DateTime> UpdDT { get; set; }
            public List<string> UserIDList { get; set; }
            public string RoleConditionID { get; set; }
            public string LineID { get; set; }
            public string FunControllerID { get; set; }
            public string FunActionName { get; set; }
            public string WFNo { get; set; }
            public string LogNo { get; set; }
            public string SUpdDT { get; set; }
            public string EUpdDT { get; set; }
        }

        public class LogUserApply
        {
            public string Memo { get; set; }
            public string UpdUserID { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }

        #region Enum

        public enum Field
        {
            RecordType, IsOnlyDiffData,
            DateBegin, DateEnd, TimeBegin, TimeEnd,
            UserID,
            SysID, FunControllerID, FunActionName,

            LogNo, ErpSign, APINo, Condition, PurviewID, Detail,
            LineID
        }

        public enum EnumUserLogin
        {
            UserID, LogNo,
            Location, LocationDesc,
            IPAddress,
            ValidResult,
            APINo, UpdUserID, UpdDT,
            ExecSysID, ExecIPAddress
        }

        public enum EnumUserAccount
        {
            LogNo,
            ModifyType,
            ModifyTypeNM,
            UserID,
            UserNM,
            IsLeft,
            UserCompNM,
            UserUnitNM,
            UserTeamNM,
            UserTitleNM,
            UserWorkNM,
            UserOrgWorkCompNM,
            UserOrgAreaNM,
            UserOrgGroupNM,
            UserOrgPlaceNM,
            UserOrgDeptNM,
            UserOrgTeamNM,
            UserOrgPTitleNM,
            UserOrgPTitle2NM,
            UserOrgTitleNM,
            UserOrgLevelNM,
            APINo,
            UpdUserID,
            UpdUserNM,
            UpdDT,
            ExecSysID,
            ExecSysNM,
            ExecIPAddress
        }

        public enum EnumUserAccess
        {
            UserID, LogNo,
            RestrictType,
            IsLock, IsDisable,
            APINo, UpdUserID, UpdDT,
            ExecSysID, ExecIPAddress
        }

        public enum EnumUserSystemRole
        {
            UserID, LogNo, Condition,
            SysID, RoleID, ErpWFNo,
            APINo, UpdUserID, UpdDT,
            ExecSysID, ExecIPAddress
        }

        public enum EnumUserFun
        {
            UserID, LogNo, RoleGroupID,
            SysID, FunControllerID, FunActionName, ErpSign,
            APINo, UpdUserID, UpdDT,
            ExecSysID, ExecIPAddress
        }

        public enum EnumUserPurview
        {
            UserID,
            LogNo,
            ModifyType,
            SysID,
            PurviewID,
            Detail,
            APINo,
            UpdUserID,
            UpdDT,
            ExecSysID,
            ExecIPAddress
        }

        public enum EnumSysRoleCondition
        {
            LogNo,
            ModifyType,
            Condition,
            ConditionID,
            SysID,
            RoleList,
            APINo,
            UpdUserID,
            UpdDT,
            ExecSysID,
            ExecIPAddress
        }

        public enum EnumSysLine
        {
            LogNo,
            SysID,
            LineID,
            LineNMZHTW,
            LineNMZHCN,
            LineNMENUS,
            LineNMTHTH,
            LineNMJAJP,
            LineNMKOKR,
            IsDisable,
            SortOrder,
            APINo,
            UpdUserID,
            UpdDT,
            ExecSysID,
            ExecIPAddress
        }

        public enum EnumSysLineReceiver
        {
            LogNo,
            SysID,
            LineID,
            LineReceiverID,
            LineReceiverNMZHTW,
            LineReceiverNMZHCN,
            LineReceiverNMENUS,
            LineReceiverNMTHTH,
            LineReceiverNMJAJP,
            LineReceiverNMKOKR,
            IsDisable,
            SourceType,
            APINo,
            UpdUserID,
            UpdDT,
            ExecSysID,
            ExecIPAddress
        }

        public enum EnumSysRole
        {
            LogNo,
            ModifyType,
            SysID,
            RoleCategoryID, RoleID,
            RoleNMZHTW, RoleNMZHCN, RoleNMENUS, RoleNMTHTH, RoleNMJAJP, RoleNMKOKR,
            IsMaster,
            APINo, UpdUserID, UpdDT,
            ExecSysID, ExecIPAddress
        }

        public enum EnumSysFunGroup
        {
            LogNo,
            ModifyType,
            SysID,
            FunControllerID,
            FunGroupZHTW, FunGroupZHCN, FunGroupENUS, FunGroupTHTH, FunGroupJAJP, FunGroupKOKR,
            SortOrder,
            APINo, UpdUserID, UpdDT,
            ExecSysID, ExecIPAddress,
        }

        public enum EnumSysFun
        {
            LogNo,
            ModifyType,
            SysID, SubSysID,
            PurviewID,
            FunControllerID, FunActionName,
            FunNMZHTW, FunNMZHCN, FunNMENUS, FunNMTHTH, FunNMJAJP, FunNMKOKR,
            FunType,
            IsOutside, IsDisable, SortOrder,
            APINo, UpdUserID, UpdDT,
            ExecSysID, ExecIPAddress,
        }

        public enum EnumSysRoleFun
        {
            LogNo,
            ModifyType,
            SysID,
            RoleID,
            FunControllerID, FunActionName,
            APINo, UpdUserID, UpdDT,
            ExecSysID, ExecIPAddress,
        }

        public enum EnumSysRoleGroup
        {
            LogNo,
            ModifyType,
            RoleGroupID,
            RoleGroupNMZHTW, RoleGroupNMZHCN, RoleGroupNMENUS, RoleGroupNMTHTH, RoleGroupNMJAJP, RoleGroupNMKOKR,
            SortOrder, Remark,
            APINo, UpdUserID, UpdDT,
            ExecSysID, ExecIPAddress
        }

        public enum EnumSysTrustIP
        {
            LogNo,
            ModifyType,
            IPBegin, IPEnd,
            ComID,
            TrustStatus, TrustType, SourceType,
            Remark, SortOrder,
            APINo, UpdUserID, UpdDT,
            ExecSysID, ExecIPAddress,
        }

        public enum EnumRecordType
        {
            [Description("LOG_USER_LOGIN")]
            UserLogin,
            [Description("LOG_USER_ACCOUNT")]
            UserAccount,
            [Description("LOG_USER_ACCESS")]
            UserAccess,
            [Description("LOG_USER_PWD")]
            UserPassword,
            [Description("LOG_USER_SYSTEM_ROLE")]
            UserSystemRole,
            [Description("LOG_USER_FUN")]
            UserFun,
            [Description("LOG_USER_PURVIEW")]
            UserPurview,

            [Description("LOG_SYS_SYSTEM_ROLE")]
            SysRole,
            [Description("LOG_SYS_SYSTEM_FUN_GROUP")]
            SysFunGroup,
            [Description("LOG_SYS_SYSTEM_FUN")]
            SysFun,
            [Description("LOG_SYS_SYSTEM_ROLE_FUN")]
            SysRoleFun,

            [Description("LOG_SYS_SYSTEM_ROLE_GROUP")]
            SysRoleGroup,
            [Description("LOG_SYS_SYSTEM_ROLE_GROUP_COLLECT")]
            SysRoleGroupCollect,
            [Description("LOG_SYS_SYSTEM_ROLE_CONDTION")]
            SysRoleCondition,
            [Description("LOG_SYS_SYSTEM_LINE")]
            SysLine,
            [Description("LOG_SYS_SYSTEM_LINE_RECEIVER")]
            SysLineReceiver,
            [Description("LOG_SYS_TRUST_IP")]
            SysTrustIP
        }

        #endregion

        [Required]
        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string DateBegin { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string DateEnd { get; set; }

        [Required]
        public string TimeBegin { get; set; }

        [Required]
        public string TimeEnd { get; set; }

        [Required]
        public string RecordType { get; set; }

        private string _UserID;

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string UserID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_UserID))
                {
                    return _UserID;
                }
                return _UserID.ToUpper();
            }
            set
            {
                _UserID = value;
            }
        }

        public string SysID { get; set; }
        public string FunControllerID { get; set; }
        public string ConditionID { get; set; }
        public string LineID { get; set; }
        public string IncludeRoleID { get; set; }
        public string FunActionName { get; set; }
        public string RoleGroupID { get; set; }
        public string IsOnlyDiffData { get; set; }
        public string Roles { get; set; }
        public string FilterJsonString { get; set; }
        public string RulesJsonString { get; set; }
        public string PurviewID { get; set; }
        public string PurviewCollectList { get; set; }
        public string UnitCode { get; set; }
        public string CompCode { get; set; }
        public string CountryCode { get; set; }
        public string ErpWFNo { get; set; }

        #region Object Binding

        public Dictionary<string, string> BeginTimeList = new Dictionary<string, string>()
        {
            {Common.GetEnumDesc(EnumBeginTimeValue.AM00), Common.GetEnumDesc(EnumBeginTimeText.AM00)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM01), Common.GetEnumDesc(EnumBeginTimeText.AM01)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM02), Common.GetEnumDesc(EnumBeginTimeText.AM02)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM03), Common.GetEnumDesc(EnumBeginTimeText.AM03)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM04), Common.GetEnumDesc(EnumBeginTimeText.AM04)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM05), Common.GetEnumDesc(EnumBeginTimeText.AM05)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM06), Common.GetEnumDesc(EnumBeginTimeText.AM06)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM07), Common.GetEnumDesc(EnumBeginTimeText.AM07)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM08), Common.GetEnumDesc(EnumBeginTimeText.AM08)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM09), Common.GetEnumDesc(EnumBeginTimeText.AM09)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM10), Common.GetEnumDesc(EnumBeginTimeText.AM10)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM11), Common.GetEnumDesc(EnumBeginTimeText.AM11)},
            {Common.GetEnumDesc(EnumBeginTimeValue.AM12), Common.GetEnumDesc(EnumBeginTimeText.AM12)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM01), Common.GetEnumDesc(EnumBeginTimeText.PM01)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM02), Common.GetEnumDesc(EnumBeginTimeText.PM02)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM03), Common.GetEnumDesc(EnumBeginTimeText.PM03)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM04), Common.GetEnumDesc(EnumBeginTimeText.PM04)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM05), Common.GetEnumDesc(EnumBeginTimeText.PM05)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM06), Common.GetEnumDesc(EnumBeginTimeText.PM06)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM07), Common.GetEnumDesc(EnumBeginTimeText.PM07)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM08), Common.GetEnumDesc(EnumBeginTimeText.PM08)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM09), Common.GetEnumDesc(EnumBeginTimeText.PM09)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM10), Common.GetEnumDesc(EnumBeginTimeText.PM10)},
            {Common.GetEnumDesc(EnumBeginTimeValue.PM11), Common.GetEnumDesc(EnumBeginTimeText.PM11)}
        };

        public Dictionary<string, string> EndTimeList = new Dictionary<string, string>()
        {
            {Common.GetEnumDesc(EnumEndTimeValue.AM00), Common.GetEnumDesc(EnumEndTimeText.AM00)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM01), Common.GetEnumDesc(EnumEndTimeText.AM01)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM02), Common.GetEnumDesc(EnumEndTimeText.AM02)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM03), Common.GetEnumDesc(EnumEndTimeText.AM03)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM04), Common.GetEnumDesc(EnumEndTimeText.AM04)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM05), Common.GetEnumDesc(EnumEndTimeText.AM05)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM06), Common.GetEnumDesc(EnumEndTimeText.AM06)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM07), Common.GetEnumDesc(EnumEndTimeText.AM07)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM08), Common.GetEnumDesc(EnumEndTimeText.AM08)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM09), Common.GetEnumDesc(EnumEndTimeText.AM09)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM10), Common.GetEnumDesc(EnumEndTimeText.AM10)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM11), Common.GetEnumDesc(EnumEndTimeText.AM11)},
            {Common.GetEnumDesc(EnumEndTimeValue.AM12), Common.GetEnumDesc(EnumEndTimeText.AM12)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM01), Common.GetEnumDesc(EnumEndTimeText.PM01)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM02), Common.GetEnumDesc(EnumEndTimeText.PM02)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM03), Common.GetEnumDesc(EnumEndTimeText.PM03)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM04), Common.GetEnumDesc(EnumEndTimeText.PM04)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM05), Common.GetEnumDesc(EnumEndTimeText.PM05)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM06), Common.GetEnumDesc(EnumEndTimeText.PM06)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM07), Common.GetEnumDesc(EnumEndTimeText.PM07)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM08), Common.GetEnumDesc(EnumEndTimeText.PM08)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM09), Common.GetEnumDesc(EnumEndTimeText.PM09)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM10), Common.GetEnumDesc(EnumEndTimeText.PM10)},
            {Common.GetEnumDesc(EnumEndTimeValue.PM11), Common.GetEnumDesc(EnumEndTimeText.PM11)}
        };

        #endregion

        public SystemRecordModel()
        {
        }

        public void FormReset()
        {
            this.DateBegin = Common.GetDateString();
            this.DateEnd = Common.GetDateString();
            this.TimeEnd = Common.GetEnumDesc(EnumEndTimeValue.PM11);
        }

        List<Entity_BaseAP.CMCode> _modifyTypeList = new List<Entity_BaseAP.CMCode>();
        List<EntitySystemRecord.SystemRecord> _entitySystemRecordList;
        public List<EntitySystemRecord.SystemRecord> EntitySystemRecordList { get { return _entitySystemRecordList; } }
        public List<LogUserApply> _LogUserApplyList { get; set; } = new List<LogUserApply>();


        #region - 取得使用者異動紀錄檔清單 -
        public async Task<bool> GetLogUserApplyList()
        {
            try
            {
                LogPara para = new LogPara
                {
                    CollectionNM = RecordType == EnumRecordType.UserFun.ToString() ? "LOG_USER_FUN_APPLY" : "LOG_USER_SYSTEM_ROLE_APPLY",
                    UserID = string.IsNullOrWhiteSpace(UserID) ? null : UserID,
                    SysID = SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                    UserIDList = Utility.GetUserIDList(UserID),
                    RoleConditionID = string.IsNullOrWhiteSpace(ConditionID) ? null : ConditionID,
                    LineID = string.IsNullOrWhiteSpace(LineID) ? null : LineID,
                    FunControllerID = string.IsNullOrWhiteSpace(FunControllerID) ? null : FunControllerID,
                    FunActionName = string.IsNullOrWhiteSpace(FunActionName) ? null : FunActionName,
                    WFNo = string.IsNullOrWhiteSpace(ErpWFNo) ? null : ErpWFNo,
                    LogNo = null,
                    SUpdDT = ErpWFNo != null ? null : Common.FormatDateTimeString($"{DateBegin}{TimeBegin}"),
                    EUpdDT = ErpWFNo != null ? null : Common.FormatDateTimeString($"{DateEnd}{TimeEnd}")
                };

                var userIDListStr = string.Join(",", para.UserIDList);
                string apiUrl = API.SystemRecord.QuerySystemRecord(para.CollectionNM, para.UserID, para.SysID, userIDListStr, para.RoleConditionID, para.LineID, para.FunControllerID, para.FunActionName, para.WFNo, para.LogNo, para.SUpdDT, para.EUpdDT);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                _LogUserApplyList = Common.GetJsonDeserializeObject<List<LogUserApply>>(response);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 設定使用者權限明細資料 -
        public bool SetSysUserPurviewDetailData(EnumCultureID cultureId)
        {
            try
            {
                var collectList = _GetPurviewCollectList();

                if (collectList.Any())
                {
                    UnitCode = string.Join("、", collectList.Where(u => u.CodeType.GetValue() == Entity_BaseAP.EnumPurviewCodeType.UNIT.ToString()).Select(c => string.Format("{0}({1})-{2}", c.CodeNM.GetValue(), c.CodeID.GetValue(), c.PurviewOPNM.GetValue())));
                    CompCode = string.Join("、", collectList.Where(u => u.CodeType.GetValue() == Entity_BaseAP.EnumPurviewCodeType.COMPANY.ToString()).Select(c => string.Format("{0}({1})-{2}", c.CodeNM.GetValue(), c.CodeID.GetValue(), c.PurviewOPNM.GetValue())));
                    CountryCode = string.Join("、", collectList.Where(u => u.CodeType.GetValue() == Entity_BaseAP.EnumPurviewCodeType.COUNTRY.ToString()).Select(c => string.Format("{0}({1})-{2}", c.CodeNM.GetValue(), c.CodeID.GetValue(), c.PurviewOPNM.GetValue())));
                }
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 設定系統角色預設條件明細資料 -
        /// <summary>
        /// 設定系統角色預設條件明細資料
        /// </summary>
        /// <param name="cultureId"></param>
        /// <returns></returns>
        public bool SetSysSystemRoleConditionDetailData(EnumCultureID cultureId)
        {
            try
            {
                FilterJsonString = GetSystemRoleConditionFilterJsonString(cultureId);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        private object GetValue(Dictionary<string, object> queryWhere, string key)
        {
            object returnValue = null;
            if (!queryWhere.TryGetValue(key, out returnValue))
            {
                returnValue = null;
            }
            return returnValue;
        }

        public Dictionary<string, List<string>> CollectionColumns { get; set; }

        public async Task<bool> GetSystemRecordList(int pageSize, EnumCultureID cultureID)
        {
            CollectionColumns = new Dictionary<string, List<string>>();
            CollectionColumns.Add("LOG_USER_LOGIN", new List<string> { "UserID", "LogNo", "Location", "LocationDesc", "IPAddress", "ValidResult", "APINo", "UpdUserID", "UpdDT", "ExecSysID", "ExecIPAddress" });
            CollectionColumns.Add("LOG_USER_ACCOUNT", new List<string> { "UserID", "LogNo", "ModifyType", "IsLeft", "UserOrgWorkCompNM", "UserOrgAreaNM", "UserOrgGroupNM", "UserOrgPlaceNM", "UserOrgDeptNM", "UserOrgTeamNM", "UserOrgPTitleNM", "UserOrgPTitle2NM", "UserOrgLevelNM", "UserOrgTitleNM", "UserCompNM", "UserUnitNM", "UserTeamNM", "UserTitleNM", "UserWorkNM", "APINo", "UpdUserID", "UpdDT", "ExecSysID", "ExecIPAddress" });
            CollectionColumns.Add("LOG_USER_ACCESS", new List<string> { "UserID", "LogNo", "RestrictType", "IsLock", "IsDisable", "APINo", "UpdUserID", "UpdDT", "ExecSysID", "ExecIPAddress" });
            CollectionColumns.Add("LOG_USER_SYSTEM_ROLE", new List<string> { "UserID", "LogNo", "ModifyType", "Condition", "SysID", "RoleID", "ErpSign", "APINo", "UpdUserID", "UpdDT", "ExecSysID", "ExecIPAddress" });
            CollectionColumns.Add("LOG_SYS_SYSTEM_ROLE", new List<string> { "LogNo", "ModifyType", "SysID", "RoleCategoryID", "RoleID", "RoleNMZHTW", "RoleNMZHCN", "RoleNMENUS", "RoleNMTHTH", "RoleNMJAJP", "RoleNMKOKR", "IsMaster", "APINo", "UpdUserID", "UpdDT", "ExecSysID", "ExecIPAddress" });
            CollectionColumns.Add("LOG_USER_FUN", new List<string> { "UserID", "LogNo", "ModifyType", "SysID", "FunControllerID", "FunActionName", "ErpSign", "APINo", "UpdUserID", "UpdDT", "ExecSysID", "ExecIPAddress" });
            CollectionColumns.Add("LOG_SYS_SYSTEM_FUN_GROUP", new List<string> { "LogNo", "ModifyType", "SysID", "FunControllerID", "FunGroupZHTW", "FunGroupZHCN", "FunGroupENUS", "FunGroupTHTH", "FunGroupJAJP", "FunGroupKOKR", "SortOrder", "APINo", "UpdUserID", "UpdDT", "ExecSysID", "ExecIPAddress" });
            CollectionColumns.Add("LOG_SYS_SYSTEM_FUN", new List<string> { "LogNo", "ModifyType", "SysID", "SubSysID", "PurviewID", "FunControllerID", "FunActionName", "FunNMZHTW", "FunNMZHCN", "FunNMENUS", "FunNMTHTH", "FunNMJAJP", "FunNMKOKR", "FunType", "IsOutside", "IsDisable", "SortOrder", "APINo", "UpdUserID", "UpdDT", "ExecSysID", "ExecIPAddress" });
            CollectionColumns.Add("LOG_SYS_SYSTEM_ROLE_FUN", new List<string> { "LogNo", "ModifyType", "SysID", "RoleID", "FunControllerID", "FunActionName", "APINo", "UpdUserID", "UpdDT", "ExecSysID", "ExecIPAddress" });
            CollectionColumns.Add("LOG_SYS_SYSTEM_LINE", new List<string> { "LogNo", "SysID", "LineID", "LineNMZHTW", "LineNMZHCN", "LineNMENUS", "LineNMTHTH", "LineNMJAJP", "LineNMKOKR", "IsDisable", "SortOrder", "APINo", "UpdUserID", "UpdDT", "ExecSysID", "ExecIPAddress" });
            CollectionColumns.Add("LOG_SYS_SYSTEM_LINE_RECEIVER", new List<string> { "LogNo", "SysID", "LineID", "LineReceiverID", "LineReceiverNMZHTW", "LineReceiverNMZHCN", "LineReceiverNMENUS", "LineReceiverNMTHTH", "LineReceiverNMJAJP", "LineReceiverNMKOKR", "IsDisable", "SourceType", "APINo", "UpdUserID", "UpdDT", "ExecSysID", "ExecIPAddress" });
            CollectionColumns.Add("LOG_SYS_SYSTEM_ROLE_GROUP", new List<string> { "LogNo", "ModifyType", "RoleGroupID", "RoleGroupNMZHTW", "RoleGroupNMZHCN", "RoleGroupNMENUS", "RoleGroupNMTHTH", "RoleGroupNMJAJP", "RoleGroupNMKOKR", "SortOrder", "Remark", "APINo", "UpdUserID", "UpdDT", "ExecSysID", "ExecIPAddress" });
            CollectionColumns.Add("LOG_SYS_TRUST_IP", new List<string> { "LogNo", "ModifyType", "IPBegin", "IPEnd", "ComID", "TrustStatus", "TrustType", "SourceType", "Remark", "SortOrder", "APINo", "UpdUserID", "UpdDT", "ExecSysID", "ExecIPAddress" });
            CollectionColumns.Add("LOG_SYS_SYSTEM_ROLE_CONDTION", new List<string> { "LogNo", "ModifyType", "Condition", "ConditionID", "SysID", "RoleList", "APINo", "UpdUserID", "UpdDT", "ExecSysID", "ExecIPAddress" });

            var collectionName = new List<string> { "LOG_USER_FUN", "LOG_SYS_SYSTEM_ROLE_FUN", "LOG_SYS_SYSTEM_ROLE_CONDTION", "LOG_USER_SYSTEM_ROLE" };

            try
            {
                Entity_BaseAP.CMCodePara codePara = new Entity_BaseAP.CMCodePara
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNM,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.ModifyType,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID.ToString())
                };

                _modifyTypeList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectCMCodeList(codePara);

                _entitySystemRecordList = new List<EntitySystemRecord.SystemRecord>();

                var recordType = (EnumRecordType)Enum.Parse(typeof(EnumRecordType), RecordType);

                collectionNM = Common.GetEnumDesc(recordType);

                foreach (var collection in collectionName)
                {
                    if (collectionNM == collection && IsOnlyDiffData != EnumYN.Y.ToString())
                    {
                        CollectionColumns[collection].Remove("ModifyType");
                    }
                }

                #region mongoDB
                LogPara para = new LogPara()
                {
                    CollectionNM = string.IsNullOrWhiteSpace(collectionNM) ? null : collectionNM,
                    UserID = string.IsNullOrWhiteSpace(UserID) ? null : UserID,
                    SysID = SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                    UserIDList = Utility.GetUserIDList(UserID),
                    RoleConditionID = string.IsNullOrWhiteSpace(ConditionID) ? null : ConditionID,
                    LineID = string.IsNullOrWhiteSpace(LineID) ? null : LineID,
                    FunControllerID = string.IsNullOrWhiteSpace(FunControllerID) ? null : FunControllerID,
                    FunActionName = string.IsNullOrWhiteSpace(FunActionName) ? null : FunActionName,
                    WFNo = string.IsNullOrWhiteSpace(ErpWFNo) ? null : ErpWFNo,
                    LogNo = null,
                    SUpdDT = Common.FormatDateTimeString($"{DateBegin}{TimeBegin}"),
                    EUpdDT = Common.FormatDateTimeString($"{DateEnd}{TimeEnd}")
                };

                var userIDListStr = string.Join(",", para.UserIDList);
                string apiUrl = API.SystemRecord.QuerySystemRecord(para.CollectionNM, para.UserID, para.SysID, userIDListStr, para.RoleConditionID, para.LineID, para.FunControllerID, para.FunActionName, para.WFNo, para.LogNo, para.SUpdDT, para.EUpdDT);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                var _logList = Common.GetJsonDeserializeObject<List<Dictionary<string, object>>>(response);

                if (_logList != null)
                {
                    #region 只看差異
                    if (collectionNM != "LOG_SYS_SYSTEM_ROLE_CONDTION")
                    {
                        if (this.IsOnlyDiffData == EnumYN.Y.ToString())
                        {
                            List<string> logNoList = (from entity in _logList
                                                      select (string)GetValue(entity, "LogNo")).Distinct().ToList();

                            string compareLogNo = string.Empty;
                            List<Dictionary<string, object>> compareList = new List<Dictionary<string, object>>();
                            var _logs = new List<Dictionary<string, object>>();

                            if (collectionNM == "LOG_USER_SYSTEM_ROLE")
                            {
                                var logNum = (from entity in _logList
                                              select (string)GetValue(entity, "LogNo")).Distinct().ToList().Count;

                                if (logNum == 1)
                                {
                                    para.LogNo = (int.Parse(_logList[0]["LogNo"].ToString()) - 1).ToString().PadLeft(6, '0');
                                    apiUrl = API.SystemRecord.QuerySystemRecord(para.CollectionNM, para.UserID, para.SysID, userIDListStr, para.RoleConditionID, para.LineID, para.FunControllerID, para.FunActionName, para.WFNo, para.LogNo, para.SUpdDT, para.EUpdDT);
                                    response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                                    var logList = Common.GetJsonDeserializeObject<List<Dictionary<string, object>>>(response);
                                    _logList.AddRange(logList);
                                }

                                logNoList = (from entity in _logList
                                             select (string)GetValue(entity, "LogNo")).Distinct().ToList();
                            }

                            if (collectionNM == "LOG_SYS_SYSTEM_ROLE_FUN")
                            {
                                logNoList.Reverse();
                            }

                            foreach (string logNo in logNoList)
                            {
                                var currentList = _logList.Where(d => (string)d["LogNo"] == logNo).ToList();
                                var tempCurrentList = new List<string>();
                                var tempCompareList = new List<string>();

                                string systemID = string.Empty;
                                string funControllerID = string.Empty;
                                string funActionName = string.Empty;


                                if (compareList.Count > 0)
                                {
                                    if (collectionNM == "LOG_USER_FUN")
                                    {
                                        currentList = currentList.Where(d => d.ContainsKey("SysID") && d.ContainsKey("FunControllerID") && d.ContainsKey("FunActionName")).ToList();
                                        tempCurrentList = currentList.Select(d => (string)d["SysID"] + "|" + (string)d["FunControllerID"] + "|" + (string)d["FunActionName"]).ToList();

                                        compareList = compareList.Where(d => d.ContainsKey("SysID") && d.ContainsKey("FunControllerID") && d.ContainsKey("FunActionName")).ToList();
                                        tempCompareList = compareList.Select(d => (string)d["SysID"] + "|" + (string)d["FunControllerID"] + "|" + (string)d["FunActionName"]).ToList();

                                        if (currentList.Count > 0 && compareList.Count > 0)
                                        {
                                            foreach (string systemFun in (tempCurrentList.Except(tempCompareList)).Union(tempCompareList.Except(tempCurrentList)))
                                            {
                                                systemID = systemFun.Split('|')[0];
                                                funControllerID = systemFun.Split('|')[1];
                                                funActionName = systemFun.Split('|')[2];

                                                if (currentList.Exists(e => (string)e["SysID"] == systemID && (string)e["FunControllerID"] == funControllerID && (string)e["FunActionName"] == funActionName))
                                                {
                                                    var current = currentList.Where(e => (string)e["SysID"] == systemID && (string)e["FunControllerID"] == funControllerID && (string)e["FunActionName"] == funActionName).First();
                                                    Dictionary<string, object> currentDic = new Dictionary<string, object>(current);

                                                    currentDic["ModifyType"] = Mongo_BaseAP.EnumModifyType.D;
                                                    currentDic["UpdDT"] = DateTime.Parse(compareList[0]["UpdDT"].ToString()).ToLocalTime();
                                                    currentDic["UpdUserID"] = (string)compareList[0]["UpdUserID"];
                                                    currentDic["UpdUserNM"] = (string)compareList[0]["UpdUserNM"];
                                                    if (currentDic.ContainsKey("APINo"))
                                                    {
                                                        currentDic["APINo"] = (string)GetValue(compareList[0], "APINo");
                                                    }
                                                    currentDic["ExecSysNM"] = (string)compareList[0]["ExecSysNM"];
                                                    currentDic["ExecSysID"] = (string)compareList[0]["ExecSysID"];
                                                    currentDic["ExecIPAddress"] = (string)compareList[0]["ExecIPAddress"];

                                                    _logs.Add(currentDic);
                                                }

                                                if (compareList.Exists(e => (string)e["SysID"] == systemID && (string)e["FunControllerID"] == funControllerID && (string)e["FunActionName"] == funActionName))
                                                {
                                                    Dictionary<string, object> compare = compareList.Where(e => (string)e["SysID"] == systemID && (string)e["FunControllerID"] == funControllerID && (string)e["FunActionName"] == funActionName).First();
                                                    Dictionary<string, object> compareDic = new Dictionary<string, object>(compare);

                                                    compareDic["ModifyType"] = Mongo_BaseAP.EnumModifyType.I;
                                                    compareDic["UpdDT"] = DateTime.Parse(compareList[0]["UpdDT"].ToString()).ToLocalTime();
                                                    compareDic["UpdUserID"] = (string)compareList[0]["UpdUserID"];
                                                    compareDic["UpdUserNM"] = (string)compareList[0]["UpdUserNM"];
                                                    if (compareDic.ContainsKey("APINo"))
                                                    {
                                                        compareDic["APINo"] = (string)GetValue(compareList[0], "APINo");
                                                    }
                                                    compareDic["ExecSysNM"] = (string)compareList[0]["ExecSysNM"];
                                                    compareDic["ExecSysID"] = (string)compareList[0]["ExecSysID"];
                                                    compareDic["ExecIPAddress"] = (string)compareList[0]["ExecIPAddress"];
                                                    _logs.Add(compareDic);
                                                }
                                            }
                                        }
                                    }
                                    if (collectionNM == "LOG_SYS_SYSTEM_ROLE_FUN")
                                    {
                                        currentList = currentList.Where(d => d.ContainsKey("RoleID")).ToList();
                                        tempCurrentList = currentList.Select(d => (string)d["RoleID"]).ToList();

                                        compareList = compareList.Where(d => d.ContainsKey("RoleID")).ToList();
                                        tempCompareList = compareList.Select(d => (string)d["RoleID"]).ToList();

                                        if (currentList.Count > 0 && compareList.Count > 0)
                                        {
                                            foreach (string roleID in (tempCurrentList.Except(tempCompareList)).Union(tempCompareList.Except(tempCurrentList)))
                                            {
                                                if (currentList.Exists(e => (string)e["RoleID"] == roleID))
                                                {
                                                    var current = currentList.Where(e => (string)e["RoleID"] == roleID).First();
                                                    Dictionary<string, object> currentDic = new Dictionary<string, object>(current);
                                                    currentDic["ModifyType"] = Mongo_BaseAP.EnumModifyType.D;
                                                    currentDic["UpdUserID"] = (string)compareList[0]["UpdUserID"];
                                                    currentDic["UpdUserNM"] = (string)compareList[0]["UpdUserNM"];
                                                    currentDic["UpdDT"] = DateTime.Parse(compareList[0]["UpdDT"].ToString()).ToLocalTime();
                                                    currentDic["ExecSysNM"] = (string)compareList[0]["ExecSysNM"];
                                                    currentDic["ExecSysID"] = (string)compareList[0]["ExecSysID"];
                                                    _logs.Add(currentDic);

                                                }

                                                if (compareList.Exists(e => (string)e["RoleID"] == roleID))
                                                {
                                                    var compare = compareList.Where(e => (string)e["RoleID"] == roleID).First();
                                                    Dictionary<string, object> compareDic = new Dictionary<string, object>(compare);
                                                    compareDic["ModifyType"] = Mongo_BaseAP.EnumModifyType.I;
                                                    compareDic["UpdDT"] = DateTime.Parse(compareList[0]["UpdDT"].ToString()).ToLocalTime();
                                                    compareDic["UpdUserID"] = (string)compareList[0]["UpdUserID"];
                                                    compareDic["UpdUserNM"] = (string)compareList[0]["UpdUserNM"];
                                                    compareDic["ExecSysNM"] = (string)compareList[0]["ExecSysNM"];
                                                    compareDic["ExecSysID"] = (string)compareList[0]["ExecSysID"];
                                                    _logs.Add(compareDic);
                                                }
                                            }
                                        }
                                    }
                                    if (collectionNM == "LOG_USER_SYSTEM_ROLE")
                                    {
                                        string roleID = string.Empty;
                                        tempCurrentList = currentList.Select(e => (string)e["SysID"] + "|" + (string)e["RoleID"]).ToList();
                                        tempCompareList = compareList.Select(e => (string)e["SysID"] + "|" + (string)e["RoleID"]).ToList();

                                        foreach (string systemRole in (tempCurrentList.Except(tempCompareList)).Union(tempCompareList.Except(tempCurrentList)))
                                        {
                                            systemID = systemRole.Split('|')[0];
                                            roleID = systemRole.Split('|')[1];

                                            if (currentList.Exists(e => (string)e["SysID"] == systemID && (string)e["RoleID"] == roleID))
                                            {
                                                var current = currentList.Where(e => (string)e["SysID"] == systemID && (string)e["RoleID"] == roleID).First();
                                                Dictionary<string, object> currentDic = new Dictionary<string, object>(current);

                                                currentDic["ModifyType"] = Mongo_BaseAP.EnumModifyType.D;
                                                currentDic["UpdDT"] = DateTime.Parse(compareList[0]["UpdDT"].ToString());
                                                currentDic["UpdUserID"] = (string)compareList[0]["UpdUserID"];
                                                currentDic["UpdUserNM"] = (string)compareList[0]["UpdUserNM"];
                                                if (currentDic.ContainsKey("APINo"))
                                                {
                                                    currentDic["APINo"] = (string)GetValue(compareList[0], "APINo");
                                                }
                                                currentDic["ExecSysNM"] = (string)compareList[0]["ExecSysNM"];
                                                currentDic["ExecSysID"] = (string)compareList[0]["ExecSysID"];
                                                currentDic["ExecIPAddress"] = (string)compareList[0]["ExecIPAddress"];

                                                _logs.Add(currentDic);
                                            }

                                            if (compareList.Exists(e => (string)e["SysID"] == systemID && (string)e["RoleID"] == roleID))
                                            {
                                                Dictionary<string, object> compare = compareList.Where(e => (string)e["SysID"] == systemID && (string)e["RoleID"] == roleID).First();
                                                Dictionary<string, object> compareDic = new Dictionary<string, object>(compare);
                                                compareDic["ModifyType"] = Mongo_BaseAP.EnumModifyType.I;
                                                compareDic["UpdDT"] = DateTime.Parse(compareList[0]["UpdDT"].ToString());
                                                compareDic["UpdUserID"] = (string)compareList[0]["UpdUserID"];
                                                compareDic["UpdUserNM"] = (string)compareList[0]["UpdUserNM"];
                                                if (compareDic.ContainsKey("APINo"))
                                                {
                                                    compareDic["APINo"] = (string)GetValue(compareList[0], "APINo");
                                                }
                                                compareDic["ExecSysNM"] = (string)compareList[0]["ExecSysNM"];
                                                compareDic["ExecSysID"] = (string)compareList[0]["ExecSysID"];
                                                compareDic["ExecIPAddress"] = (string)compareList[0]["ExecIPAddress"];

                                                _logs.Add(compareDic);
                                            }
                                        }
                                    }
                                }
                                compareLogNo = logNo;
                                compareList = currentList;
                                if (collectionNM == "LOG_USER_FUN" || collectionNM == "LOG_SYS_SYSTEM_ROLE_FUN")
                                {
                                    if (logNoList.IndexOf(logNo) + 1 == logNoList.Count)
                                    {
                                        foreach (Dictionary<string, object> entity in currentList)
                                        {
                                            entity["ModifyType"] = Mongo_BaseAP.EnumModifyType.U;
                                            _logs.Add(entity);
                                        }
                                    }
                                }
                            }
                            _logList = _logs;
                        }
                    }
                    #endregion
                    var columns = CollectionColumns[collectionNM];
                    Type type = typeof(SysSystemRecord);
                    var properties = type.GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                    titleList = new List<string>();

                    foreach (var item in columns)
                    {
                        var title = properties.Where(w => w.Name == $"Table_{item}").SingleOrDefault();
                        var titleValue = title.GetValue(null, null).ToString();
                        titleList.Add(titleValue);
                    }

                    foreach (Dictionary<string, object> entity in _logList)
                    {
                        SystemRecord systemRecord = new SystemRecord()
                        {
                            LogNo = (string)GetValue(entity, "LogNo"),
                            ContentDict = new Dictionary<string, string>()
                        };

                        string updUser = (string)GetValue(entity, "UpdUserNM");
                        if ((string)GetValue(entity, "UpdUserID") != null && (string)GetValue(entity, "UpdUserID") != (string)GetValue(entity, "UpdUserNM"))
                        {
                            updUser = (string.Format(Common.GetEnumDesc(EnumTextFormat.User), ((string)GetValue(entity, "UpdUserID")).returnHtml(), ((string)GetValue(entity, "UpdUserNM")).returnHtml()));
                        }

                        if ((string)GetValue(entity, "ValidResultNM") != null && (((string)GetValue(entity, "ValidResultNM")).Contains("\n")))
                        {
                            entity["ValidResultNM"] = ((string)GetValue(entity, "ValidResultNM")).Replace("\n", "<br/>");
                        }

                        var updDT = RecordType == EnumRecordType.UserSystemRole.ToString() ? ((DateTime)GetValue(entity, "UpdDT")).ToString() : ((DateTime)GetValue(entity, "UpdDT")).ToLocalTime().ToString();
                        systemRecord.ContentDict.Add(EnumUserLogin.UpdDT.ToString(), updDT);

                        if (!entity.ContainsKey("ModifyType"))
                        {
                            CollectionColumns.Remove("ModifyType");
                        }

                        if (collectionNM == "LOG_USER_ACCOUNT" || collectionNM == "LOG_SYS_SYSTEM_ROLE" || collectionNM == "LOG_SYS_SYSTEM_FUN_GROUP" || collectionNM == "LOG_SYS_SYSTEM_FUN" || collectionNM == "LOG_SYS_SYSTEM_ROLE_GROUP" || collectionNM == "LOG_SYS_TRUST_IP" || collectionNM == "LOG_SYS_SYSTEM_ROLE_CONDTION")
                        {
                            systemRecord.ContentDict.Add(EnumUserPurview.ModifyType.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "ModifyTypeNM")).returnHtml(), ((string)GetValue(entity, "ModifyType")).returnHtml()));
                        }

                        if (columns.Contains("ModifyType") && (collectionNM == "LOG_USER_FUN" || collectionNM == "LOG_SYS_SYSTEM_ROLE_FUN" || collectionNM == "LOG_USER_SYSTEM_ROLE"))
                        {
                            string modifyTypeNM = null;
                            if (_modifyTypeList != null && _modifyTypeList.Count > 0 && !string.IsNullOrWhiteSpace(entity["ModifyType"].ToString()))
                            {
                                modifyTypeNM = (string.Format(Common.GetEnumDesc(EnumTextFormat.Code), (_modifyTypeList.Find(e => e.CodeID == entity["ModifyType"].ToString())).CodeNM.HtmlValue(),
                                               entity["ModifyType"].ToString()));
                            }
                            if (!string.IsNullOrWhiteSpace(entity["ModifyType"].ToString()))
                            {
                                systemRecord.ContentDict.Add(EnumSysRoleFun.ModifyType.ToString(), modifyTypeNM.returnHtml());
                            }
                        }

                        string restrictType = null;
                        if ((string)GetValue(entity, "RestrictType") != null)
                        {
                            restrictType = (string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "RestrictTypeNM")).returnHtml(), ((string)GetValue(entity, "RestrictType")).returnHtml()));
                        }

                        if (collectionNM == "LOG_SYS_SYSTEM_ROLE" || collectionNM == "LOG_USER_FUN" || collectionNM == "LOG_SYS_SYSTEM_FUN_GROUP" || collectionNM == "LOG_SYS_SYSTEM_FUN" || collectionNM == "LOG_SYS_SYSTEM_ROLE_FUN" || collectionNM == "LOG_SYS_SYSTEM_ROLE_CONDTION" || collectionNM == "LOG_USER_SYSTEM_ROLE" || collectionNM == "LOG_SYS_SYSTEM_LINE_RECEIVER")
                        {
                            systemRecord.ContentDict.Add(EnumUserPurview.SysID.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "SysNM")).returnHtml(), ((string)GetValue(entity, "SysID")).returnHtml()));
                        }

                        if (collectionNM == "LOG_SYS_SYSTEM_LINE")
                        {
                            systemRecord.ContentDict.Add(EnumSysLine.SysID.ToString(), ((string)GetValue(entity, "SysNM")).returnHtml());
                        }

                        if (collectionNM == "LOG_SYS_SYSTEM_FUN")
                        {
                            string purview = null;
                            if ((string)GetValue(entity, "PurviewID") != null)
                            {
                                purview = (string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "PurviewNM")).returnHtml(), ((string)GetValue(entity, "PurviewID")).returnHtml()));
                            }
                            systemRecord.ContentDict.Add(EnumSysFun.PurviewID.ToString(), purview.returnHtml());
                        }

                        string roleCategory = null;
                        if (((string)GetValue(entity, "RoleCategoryID")) != null)
                        {
                            roleCategory = (string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "RoleCategoryNM")).returnHtml(), ((string)GetValue(entity, "RoleCategoryID")).returnHtml()));
                        }

                        if (collectionNM == "LOG_SYS_SYSTEM_ROLE")
                        {
                            systemRecord.ContentDict.Add(EnumSysRole.RoleID.ToString(), ((string)GetValue(entity, "RoleID")).returnHtml());
                        }

                        if (collectionNM == "LOG_SYS_SYSTEM_ROLE_FUN" || collectionNM == "LOG_USER_SYSTEM_ROLE")
                        {
                            systemRecord.ContentDict.Add(EnumSysRoleFun.RoleID.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "RoleNM")).returnHtml(), ((string)GetValue(entity, "RoleID")).returnHtml()));
                        }

                        if (collectionNM == "LOG_SYS_SYSTEM_FUN_GROUP")
                        {
                            systemRecord.ContentDict.Add(EnumSysFunGroup.FunControllerID.ToString(), ((string)GetValue(entity, "FunControllerID")).returnHtml());
                        }

                        if (collectionNM == "LOG_SYS_SYSTEM_FUN" || collectionNM == "LOG_SYS_SYSTEM_ROLE_FUN")
                        {
                            systemRecord.ContentDict.Add(EnumSysFun.FunControllerID.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "FunGroupNM")).returnHtml(), ((string)GetValue(entity, "FunControllerID")).returnHtml()));
                        }

                        if (collectionNM == "LOG_USER_FUN")
                        {
                            systemRecord.ContentDict.Add(EnumUserFun.FunControllerID.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "FunControllerNM")).returnHtml(), ((string)GetValue(entity, "FunControllerID")).returnHtml()));
                        }

                        string subSystem = null;
                        if ((string)GetValue(entity, "SubSysID") != null && (string)GetValue(entity, "SubSysID") != (string)GetValue(entity, "SysID"))
                        {
                            subSystem = (string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "SubSysNM")).returnHtml(), ((string)GetValue(entity, "SubSysID")).returnHtml()));
                        }

                        if (collectionNM == "LOG_SYS_SYSTEM_FUN")
                        {
                            systemRecord.ContentDict.Add(EnumSysFun.FunActionName.ToString(), ((string)GetValue(entity, "FunActionName")).returnHtml());
                        }

                        if (collectionNM == "LOG_USER_FUN" || collectionNM == "LOG_SYS_SYSTEM_ROLE_FUN")
                        {
                            systemRecord.ContentDict.Add(EnumUserFun.FunActionName.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "FunNM")).returnHtml(), ((string)GetValue(entity, "FunActionName")).returnHtml()));
                        }

                        if (columns.Contains("RoleList"))
                        {
                            var roles = new List<string>();
                            if (entity.ContainsKey("Roles"))
                            {
                                roles = JsonConvert.DeserializeObject<List<string>>(JsonConvert.SerializeObject(entity["Roles"]));//Dictionary => jsonstring => object
                            }
                            systemRecord.ContentDict.Add(EnumSysRoleCondition.RoleList.ToString(), roles != null && roles.Count() > 0 ? string.Join("、", roles) : string.Empty);
                        }

                        if (collectionNM == "LOG_SYS_SYSTEM_LINE_RECEIVER")
                        {
                            systemRecord.ContentDict.Add(EnumSysLineReceiver.SourceType.ToString(), ((string)GetValue(entity, "SourceType")).returnHtml());
                        }

                        if (collectionNM == "LOG_SYS_TRUST_IP")
                        {
                            systemRecord.ContentDict.Add(EnumSysTrustIP.SourceType.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "SourceTypeNM")).returnHtml(), ((string)GetValue(entity, "SourceType")).returnHtml()));
                        }

                        if (collectionNM == "LOG_USER_SYSTEM_ROLE")
                        {
                            JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                            QueryCondition queryCondition = new QueryCondition();
                            List<QueryCondition.GroupRule> roleConditionRules = new List<QueryCondition.GroupRule>();
                            if ((string)GetValue(entity, "RoleConditionRules") != null)
                            {
                                roleConditionRules = queryCondition.GetGroupRules(new List<QueryCondition.RoleConditionRules> { jsonConvert.Deserialize<QueryCondition.RoleConditionRules>((string)GetValue(entity, "RoleConditionRules")) });
                            }
                            systemRecord.ContentDict.Add(EnumUserSystemRole.Condition.ToString(), (string)GetValue(entity, "RoleConditionRules") != null ? jsonConvert.Serialize(roleConditionRules).returnHtml() : string.Empty);
                        }

                        if (collectionNM == "LOG_SYS_SYSTEM_ROLE_CONDTION")
                        {
                            systemRecord.ContentDict.Add(EnumSysRoleCondition.Condition.ToString(), GetValue(entity, "RoleConditionRules") != null ? entity["RoleConditionRules"].ToString() : string.Empty);
                        }

                        string company = null;
                        if ((string)GetValue(entity, "ComID") != null)
                        {
                            company = (string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "ComNM")).returnHtml(), ((string)GetValue(entity, "ComID")).returnHtml()));
                        }

                        systemRecord.ContentDict.Add(EnumUserLogin.UserID.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.User), ((string)GetValue(entity, "UserID")).returnHtml(), ((string)GetValue(entity, "UserNM")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserLogin.LogNo.ToString(), ((string)GetValue(entity, "LogNo")).returnHtml());
                        systemRecord.ContentDict.Add(EnumUserLogin.Location.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "LocationNM")).returnHtml(), ((string)GetValue(entity, "Location")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserLogin.LocationDesc.ToString(), ((string)GetValue(entity, "LocationDesc")).returnHtml());
                        systemRecord.ContentDict.Add(EnumUserLogin.IPAddress.ToString(), ((string)GetValue(entity, "IPAddress")).returnHtml());
                        systemRecord.ContentDict.Add(EnumUserLogin.ValidResult.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "ValidResultNM")).returnHtml(), ((string)GetValue(entity, "ValidResult")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserAccount.APINo.ToString(), ((string)GetValue(entity, "APINo")) != null ? ((string)GetValue(entity, "APINo")) : (string.Empty));
                        systemRecord.ContentDict.Add(EnumUserLogin.UpdUserID.ToString(), updUser.returnHtml());
                        systemRecord.ContentDict.Add(EnumUserLogin.ExecSysID.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "ExecSysNM")).returnHtml(), ((string)GetValue(entity, "ExecSysID")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserLogin.ExecIPAddress.ToString(), ((string)GetValue(entity, "ExecIPAddress")).returnHtml());
                        systemRecord.ContentDict.Add(EnumUserAccount.IsLeft.ToString(), ((string)GetValue(entity, "IsLeft")).returnHtml());
                        systemRecord.ContentDict.Add(EnumUserAccount.UserOrgWorkCompNM.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "UserOrgWorkComNM")).returnHtml(), ((string)GetValue(entity, "UserOrgWorkCom")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserAccount.UserOrgAreaNM.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "UserOrgAreaNM")).returnHtml(), ((string)GetValue(entity, "UserOrgArea")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserAccount.UserOrgGroupNM.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "UserOrgGroupNM")).returnHtml(), ((string)GetValue(entity, "UserOrgGroup")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserAccount.UserOrgPlaceNM.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "UserOrgPlaceNM")).returnHtml(), ((string)GetValue(entity, "UserOrgPlace")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserAccount.UserOrgDeptNM.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "UserOrgDeptNM")).returnHtml(), ((string)GetValue(entity, "UserOrgDept")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserAccount.UserOrgTeamNM.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "UserOrgTeamNM")).returnHtml(), ((string)GetValue(entity, "UserOrgTeam")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserAccount.UserOrgPTitleNM.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "UserOrgJobTitleNM")).returnHtml(), ((string)GetValue(entity, "UserOrgJobTitle")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserAccount.UserOrgPTitle2NM.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "UserOrgBizTitleNM")).returnHtml(), ((string)GetValue(entity, "UserOrgBizTitle")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserAccount.UserOrgLevelNM.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "UserOrgLevelNM")).returnHtml(), ((string)GetValue(entity, "UserOrgLevel")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserAccount.UserOrgTitleNM.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "UserOrgTitleNM")).returnHtml(), ((string)GetValue(entity, "UserOrgTitle")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserAccount.UserCompNM.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "UserComNM")).returnHtml(), ((string)GetValue(entity, "UserComID")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserAccount.UserUnitNM.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "UserUnitNM")).returnHtml(), ((string)GetValue(entity, "UserUnitID")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserAccount.UserTeamNM.ToString(), ((string)GetValue(entity, "UserTeamID")).returnHtml());
                        systemRecord.ContentDict.Add(EnumUserAccount.UserTitleNM.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "UserTitleNM")).returnHtml(), ((string)GetValue(entity, "UserTitleID")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserAccount.UserWorkNM.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "UserWorkNM")).returnHtml(), ((string)GetValue(entity, "UserWorkID")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserAccess.RestrictType.ToString(), restrictType.returnHtml());
                        systemRecord.ContentDict.Add(EnumUserAccess.IsLock.ToString(), ((string)GetValue(entity, "IsLock")).returnHtml());
                        systemRecord.ContentDict.Add(EnumUserAccess.IsDisable.ToString(), ((string)GetValue(entity, "IsDisable")).returnHtml());
                        systemRecord.ContentDict.Add(EnumUserPurview.Detail.ToString(), string.Join("|", new List<string> { ((string)GetValue(entity, "LogNo")).returnHtml(), ((string)GetValue(entity, "SysID")).returnHtml(), ((string)GetValue(entity, "PurviewID")).returnHtml() }));
                        systemRecord.ContentDict.Add(EnumSysRole.RoleCategoryID.ToString(), roleCategory.returnHtml());
                        systemRecord.ContentDict.Add(EnumSysRole.RoleNMZHTW.ToString(), ((string)GetValue(entity, "RoleNMZHTW")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysRole.RoleNMZHCN.ToString(), ((string)GetValue(entity, "RoleNMZHCN")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysRole.RoleNMENUS.ToString(), ((string)GetValue(entity, "RoleNMENUS")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysRole.RoleNMTHTH.ToString(), ((string)GetValue(entity, "RoleNMTHTH")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysRole.RoleNMJAJP.ToString(), ((string)GetValue(entity, "RoleNMJAJP")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysRole.RoleNMKOKR.ToString(), ((string)GetValue(entity, "RoleNMKOKR")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysRole.IsMaster.ToString(), ((string)GetValue(entity, "IsMaster")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysFunGroup.FunGroupZHTW.ToString(), ((string)GetValue(entity, "FunGroupZHTW")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysFunGroup.FunGroupZHCN.ToString(), ((string)GetValue(entity, "FunGroupZHCN")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysFunGroup.FunGroupENUS.ToString(), ((string)GetValue(entity, "FunGroupENUS")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysFunGroup.FunGroupTHTH.ToString(), ((string)GetValue(entity, "FunGroupTHTH")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysFunGroup.FunGroupJAJP.ToString(), ((string)GetValue(entity, "FunGroupJAJP")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysFunGroup.FunGroupKOKR.ToString(), ((string)GetValue(entity, "FunGroupKOKR")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysFunGroup.SortOrder.ToString(), ((string)GetValue(entity, "SortOrder")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysFun.SubSysID.ToString(), subSystem.returnHtml());
                        systemRecord.ContentDict.Add(EnumSysFun.FunNMZHTW.ToString(), ((string)GetValue(entity, "FunNMZHTW")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysFun.FunNMZHCN.ToString(), ((string)GetValue(entity, "FunNMZHCN")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysFun.FunNMENUS.ToString(), ((string)GetValue(entity, "FunNMENUS")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysFun.FunNMTHTH.ToString(), ((string)GetValue(entity, "FunNMTHTH")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysFun.FunNMJAJP.ToString(), ((string)GetValue(entity, "FunNMJAJP")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysFun.FunNMKOKR.ToString(), ((string)GetValue(entity, "FunNMKOKR")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysFun.FunType.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "FunTypeNM")).returnHtml(), ((string)GetValue(entity, "FunType")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumSysFun.IsOutside.ToString(), ((string)GetValue(entity, "IsOutside")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysRoleCondition.ConditionID.ToString(), ((string)GetValue(entity, "RoleConditionID")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysLine.LineID.ToString(), ((string)GetValue(entity, "LineID")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysLine.LineNMZHTW.ToString(), ((string)GetValue(entity, "LineNMZHTW")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysLine.LineNMZHCN.ToString(), ((string)GetValue(entity, "LineNMZHCN")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysLine.LineNMENUS.ToString(), ((string)GetValue(entity, "LineNMENUS")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysLine.LineNMTHTH.ToString(), ((string)GetValue(entity, "LineNMTHTH")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysLine.LineNMJAJP.ToString(), ((string)GetValue(entity, "LineNMJAJP")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysLine.LineNMKOKR.ToString(), ((string)GetValue(entity, "LineNMKOKR")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysLineReceiver.LineReceiverID.ToString(), ((string)GetValue(entity, "LineReceiverID")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysLineReceiver.LineReceiverNMZHTW.ToString(), ((string)GetValue(entity, "LineReceiverNMZHTW")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysLineReceiver.LineReceiverNMZHCN.ToString(), ((string)GetValue(entity, "LineReceiverNMZHCN")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysLineReceiver.LineReceiverNMENUS.ToString(), ((string)GetValue(entity, "LineReceiverNMENUS")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysLineReceiver.LineReceiverNMTHTH.ToString(), ((string)GetValue(entity, "LineReceiverNMTHTH")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysLineReceiver.LineReceiverNMJAJP.ToString(), ((string)GetValue(entity, "LineReceiverNMJAJP")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysLineReceiver.LineReceiverNMKOKR.ToString(), ((string)GetValue(entity, "LineReceiverNMKOKR")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysRoleGroup.RoleGroupID.ToString(), ((string)GetValue(entity, "RoleGroupID")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysRoleGroup.RoleGroupNMZHTW.ToString(), ((string)GetValue(entity, "RoleGroupNMZHTW")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysRoleGroup.RoleGroupNMZHCN.ToString(), ((string)GetValue(entity, "RoleGroupNMZHCN")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysRoleGroup.RoleGroupNMENUS.ToString(), ((string)GetValue(entity, "RoleGroupNMENUS")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysRoleGroup.RoleGroupNMTHTH.ToString(), ((string)GetValue(entity, "RoleGroupNMTHTH")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysRoleGroup.RoleGroupNMJAJP.ToString(), ((string)GetValue(entity, "RoleGroupNMJAJP")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysRoleGroup.RoleGroupNMKOKR.ToString(), ((string)GetValue(entity, "RoleGroupNMKOKR")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysRoleGroup.Remark.ToString(), ((string)GetValue(entity, "Remark")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysTrustIP.IPBegin.ToString(), ((string)GetValue(entity, "IPBegin")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysTrustIP.IPEnd.ToString(), ((string)GetValue(entity, "IPEnd")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysTrustIP.ComID.ToString(), company.returnHtml());
                        systemRecord.ContentDict.Add(EnumSysTrustIP.TrustStatus.ToString(), ((string)GetValue(entity, "TrustStatus")).returnHtml());
                        systemRecord.ContentDict.Add(EnumSysTrustIP.TrustType.ToString(), string.Format(Common.GetEnumDesc(EnumTextFormat.Code), ((string)GetValue(entity, "TrustTypeNM")).returnHtml(), ((string)GetValue(entity, "TrustType")).returnHtml()));
                        systemRecord.ContentDict.Add(EnumUserFun.ErpSign.ToString(), ((string)GetValue(entity, "ErpWFNo")) != null ? ((string)GetValue(entity, "ErpWFNo")).returnHtml() : string.Empty);

                        systemRecords.Add(systemRecord);
                    }
                    #endregion

                    if (systemRecords != null)
                    {
                        systemRecords = systemRecords.OrderByDescending(e => e.LogNo).ThenByDescending(e => e.UpdDT).ToList();
                        systemRecords = base.GetEntitysByPage(systemRecords, pageSize);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        private List<Mongo_BaseAP.LOG_SYS_USER_PURVIEW_COLLECT> _GetPurviewCollectList()
        {
            if (PurviewCollectList != null)
            {
                PurviewCollectList = HttpUtility.UrlDecode(PurviewCollectList);
                var paraData = string.IsNullOrWhiteSpace(PurviewCollectList) ? new string[] { } : PurviewCollectList.Split('|');

                if (paraData.Any())
                {
                    MongoSystemRecord.LogUserPurviewPara para = new MongoSystemRecord.LogUserPurviewPara
                    {
                        LogNo = new DBChar(paraData[0]),
                        SysID = new DBVarChar(paraData[1]),
                        PurviewID = new DBVarChar(paraData[2])
                    };

                    return new MongoSystemRecord(ConnectionStringMSERP, ProviderNameMSERP)
                        .SelectLogPurviewCollectList(para).PurviewCollectList;
                }
            }
            return new List<Mongo_BaseAP.LOG_SYS_USER_PURVIEW_COLLECT>();
        }

    }

    public static class ReturnHtml
    {
        public static string returnHtml(this string value)
        {
            return (value == String.Empty || value == null) ? "<font style='color:red'><i>NULL</i></font>" : value;
        }
    }
}