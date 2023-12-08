using LionTech.Entity;
using LionTech.Entity.ERP.Pub;
using LionTech.Utility;
using LionTech.Web.ERPHelper;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ERPAP.Models.Pub
{
    public class LogUserTraceActionModel : PubModel, IValidatableObject
    {
        #region Definitions
        public enum EnumTraceDateRange
        {
            Range = 1
        }
        public new enum EnumCookieKey
        {
            SearchType,
            UserID,
            SysID,
            ControllerName,
            ActionName,
            SessionID,
            RequestSessionID,
            StartTraceDate,
            EndTraceDate,
            StartTraceTime,
            EndTraceTime
        }
        public enum EnumSearchType
        {
            A,
            B,
            C
        }

        #endregion
        #region Constructor
        public LogUserTraceActionModel()
        {
            _mongoEntity = new MongoLogUserTraceAction(ConnectionStringMSERP, ProviderNameMSERP);
        }
        #endregion
        #region Property
        public string SearchType { get; set; }
        public string UserID { get; set; }
        public string SysID { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string SessionID { get; set; }
        public string RequestSessionID { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string StartTraceDate { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string EndTraceDate { get; set; }
        [Required]
        [StringLength(5, MinimumLength = 4)]
        public string StartTraceTime { get; set; }
        [Required]
        [StringLength(5, MinimumLength = 4)]
        public string EndTraceTime { get; set; }
        public List<MongoLogUserTraceAction.LogUserTraceAction> LogUserTraceActionList { get; private set; }

        public Dictionary<string, string> _searchTypeDictionary { get; set; }
        public Dictionary<string, string> SearchTypeDictionary => _searchTypeDictionary ?? (_searchTypeDictionary = new Dictionary<string, string>
        {
            {EnumSearchType.A.ToString(), PubLogUserTraceAction.Label_SearchTypeA},
            {EnumSearchType.B.ToString(), PubLogUserTraceAction.Label_SearchTypeB},
            {EnumSearchType.C.ToString(), PubLogUserTraceAction.Label_SearchTypeC}
        });

        #endregion
        #region Reset
        public void FormReset()
        {
            SearchType = string.Empty;
            UserID = string.Empty;
            SysID = string.Empty;
            ControllerName = string.Empty;
            ActionName = string.Empty;
            SessionID = string.Empty;
            RequestSessionID = string.Empty;
            StartTraceDate = Common.GetDateString();
            EndTraceDate = Common.GetDateString();
            StartTraceTime = Common.GetDateTimeFormattedText(DateTime.Now.AddHours(-1), Common.EnumDateTimeFormatted.HoursForMinutes);
            EndTraceTime = Common.GetDateTimeFormattedText(DateTime.Now, Common.EnumDateTimeFormatted.HoursForMinutes);
        }
        #endregion
        #region Private
        private readonly MongoLogUserTraceAction _mongoEntity;
        #endregion
        #region Validation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var dtStart = $"{Common.FormatDateString(StartTraceDate)} {StartTraceTime}:00.000";
            var dtEnd = $"{Common.FormatDateString(EndTraceDate)} {EndTraceTime}:59.999";
            if (IsValid == false)
            {
                yield break;
            }
            TimeSpan ts = Convert.ToDateTime(dtEnd).AddMilliseconds(-59999).Subtract(Convert.ToDateTime(dtStart));
            if (ExecAction == EnumActionType.Select)
            {
                #region - 搜尋日期資料驗證 -
                if (ts.TotalHours < 0)
                {
                    yield return new ValidationResult(PubLogUserTraceAction.SystemMsg_TraceDate_Error);
                }
                #endregion
                #region - 搜尋日期範圍驗證(一小時) -
                if (ts.Hours > (int)EnumTraceDateRange.Range)
                {
                    yield return new ValidationResult(PubLogUserTraceAction.SystemMsg_TraceDateRange_Error);
                }
                #endregion
            }

            if(SearchType == EnumSearchType.A.ToString())
            {
                if(string.IsNullOrEmpty(UserID) || string.IsNullOrEmpty(SessionID) || string.IsNullOrEmpty(RequestSessionID))
                {
                    yield return new ValidationResult(PubLogUserTraceAction.SystemMsg_Required_Error);
                }
            }
            if (SearchType == EnumSearchType.B.ToString())
            {
                if (string.IsNullOrEmpty(UserID) || string.IsNullOrEmpty(SysID))
                {
                    yield return new ValidationResult(PubLogUserTraceAction.SystemMsg_Required_Error);
                }
            }
            if (SearchType == EnumSearchType.C.ToString())
            {
                if (string.IsNullOrEmpty(SysID) || string.IsNullOrEmpty(ControllerName) || string.IsNullOrEmpty(ActionName))
                {
                    yield return new ValidationResult(PubLogUserTraceAction.SystemMsg_Required_Error);
                }
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// 取得使用者追蹤紀錄
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public bool GetLogUserTraceList(int pageSize)
        {
            var dtStart = $"{Common.FormatDateString(StartTraceDate)} {StartTraceTime}:00.000";
            var dtEnd = $"{Common.FormatDateString(EndTraceDate)} {EndTraceTime}:59.999";
            try
            {
                MongoLogUserTraceAction.LogUserTraceActionPara para = new MongoLogUserTraceAction.LogUserTraceActionPara
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(UserID) ? null : UserID),
                    SysID = new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID),
                    ControllerName = new DBVarChar(string.IsNullOrWhiteSpace(ControllerName) ? null : ControllerName),
                    ActionName = new DBVarChar(string.IsNullOrWhiteSpace(ActionName) ? null : ActionName),
                    SessionID = new DBChar(string.IsNullOrWhiteSpace(SessionID) ? null : SessionID),
                    RequestSessionID = new DBChar(string.IsNullOrWhiteSpace(RequestSessionID) ? null : RequestSessionID),
                    StartTraceDateTime = new DBDateTime(string.IsNullOrWhiteSpace(dtStart) ? new DateTime?() : Convert.ToDateTime(dtStart)),
                    EndTraceDateTime = new DBDateTime(string.IsNullOrWhiteSpace(dtEnd) ? new DateTime?() : Convert.ToDateTime(dtEnd))
                };
                LogUserTraceActionList = _mongoEntity.SelectLogUserTraceList(para);
                LogUserTraceActionList = GetEntitysByPage(LogUserTraceActionList, pageSize);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion
    }
}