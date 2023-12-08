using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemEDIFlowDetailModel : SysModel, IValidatableObject
    {
        #region - Definitions -
        public enum EnumMonthlyFinalDay
        {
            [Description("99")]
            NinetyNine
        }

        public enum EnumFrequency
        {
            Continuity,
			Daily,
			Weekly,
			Monthly,
			FixedTime
        }

        private enum EnumDayOfWeek
        {
            Sunday = 1,
            Monday = 2,
            Tuesday = 4,
            Wednesday = 8,
            Thursday = 16,
            Friday = 32,
            Saturday = 64
        }

        public enum EnumDeleteSystemEDIFlowDetailResult
        {
            Success, 
            Failure, 
            DataExist
        }

        public enum SortorderField
        {
            [Description("00")]
            Right,
            [Description("000")]
            Left,
            [Description("00001")]
            First
        }

        public class SystemEDIFlowDetail
        {
            public SystemEDIFlowDetails SystemEDIFlowDetails = new SystemEDIFlowDetails();

            public List<SystemEDIFixEDTime> SystemEDIFixEDTime = new List<SystemEDIFixEDTime>();
        }

        public class SystemEDIFlowDetails
        {
            public string SysID { get; set; }
            public string EDIFlowID { get; set; }

            public string EDIFlowZHTW { get; set; }

            public string EDIFlowZHCN { get; set; }

            public string EDIFlowENUS { get; set; }

            public string EDIFlowTHTH { get; set; }

            public string EDIFlowJAJP { get; set; }

            public string EDIFlowKOKR { get; set; }

            public string SCHFrequency { get; set; }

            public string SCHStartDate { get; set; }

            public string SCHStartTime { get; set; }

            public Nullable<int> SCHIntervalNum { get; set; }

            public Nullable<int> SCHIntervalTime { get; set; }

            public string SCHEndTime { get; set; }
            public Nullable<int> SCHWeeks { get; set; }
            public string SCHDaysStr { get; set; }
            public string SCHDataDelay { get; set; }
            public string SCHKeepLogDay { get; set; }
            public string PATHSCmd { get; set; }

            public string PATHSDat { get; set; }

            public string PATHSSrc { get; set; }

            public string PATHSRes { get; set; }

            public string PATHSBad { get; set; }

            public string PATHSLog { get; set; }

            public string PATHSFlowXml { get; set; }

            public string PATHSFlowCmd { get; set; }

            public string PATHSZipDat { get; set; }

            public string PATHSException { get; set; }

            public string PATHSSummary { get; set; }

            public string SortOrder { get; set; }
            public string UpdUserID { get; set; }
            public string ExeCuteTIME { get; set; }
            public List<string> ExecuteTimeList { get; set; }

        }


        public class SystemEDIFixEDTime
        {
            public string SysID { get; set; }
            public string EDIFlowID { get; set; }
            public string ExeCuteTIME { get; set; }
            public string UpdUserID { get; set; }
        }


        #endregion

        #region - Constructor -
        public SystemEDIFlowDetailModel()
        {
            _entity = new EntitySystemEDIFlowDetail(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        [Required]
        public string SysID { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBox)]
        public string EDIFlowID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIFlowZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIFlowZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIFlowENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIFlowTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIFlowJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIFlowKOKR { get; set; }

        [Required]
        public string SCHFrequency { get; set; }

        [Required]
        [StringLength(8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string SCHStartDate { get; set; }

        [Required]
        [StringLength(9, MinimumLength = 9)]
        [InputType(EnumInputType.TextBox)]
        public string SCHStartTime { get; set; }

        [StringLength(9, MinimumLength = 9)]
        [InputType(EnumInputType.TextBox)]
        public string SCHEndTime { get; set; }

        [Range(1, 999)]
        [StringLength(2, MinimumLength = 1)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SCHIntervalNum { get; set; }

        [StringLength(3)]
        [InputType(EnumInputType.TextBoxInteger)]
        public string SCHIntervalTime { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SCHDataDelay { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSCmd { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSDat { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSSrc { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSRes { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSBad { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSLog { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSFlowXml { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSFlowCmd { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSZipDat { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSException { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string PATHSSummary { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        [InputType(EnumInputType.TextBoxNumber)]
        public string KeepLogDay { get; set; }

        public List<string> SCHExecuteWeeklyList { get; set; }

        private List<string> _schExecuteDayList;

        public List<string> SCHExecuteDayList
        {
            get { return _schExecuteDayList ?? (_schExecuteDayList = new List<string>()); }
            set { _schExecuteDayList = value; }
        }

        public List<string> ExecuteTimeList { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemEDIFlowDetail.TabText_SystemEDIFlowDetail,
                ImageURL=string.Empty
            }
        };

        private Dictionary<string, string> _schWeeklyDic;

        public Dictionary<string, string> SCHWeeklyDic
        {
            get
            {
                return _schWeeklyDic ?? (_schWeeklyDic = new Dictionary<string, string>
                {
                    { ((int)EnumDayOfWeek.Sunday).ToString(), SysSystemEDIFlowDetail.Label_Sunday },
                    { ((int)EnumDayOfWeek.Monday).ToString(), SysSystemEDIFlowDetail.Label_Monday },
                    { ((int)EnumDayOfWeek.Tuesday).ToString(), SysSystemEDIFlowDetail.Label_Tuesday },
                    { ((int)EnumDayOfWeek.Wednesday).ToString(), SysSystemEDIFlowDetail.Label_Wednesday },
                    { ((int)EnumDayOfWeek.Thursday).ToString(), SysSystemEDIFlowDetail.Label_Thursday },
                    { ((int)EnumDayOfWeek.Friday).ToString(), SysSystemEDIFlowDetail.Label_Friday },
                    { ((int)EnumDayOfWeek.Saturday).ToString(), SysSystemEDIFlowDetail.Label_Saturday }
                });
            }
        }

        private Dictionary<string, string> _schMonthlyDayDic;

        public Dictionary<string, string> SCHMonthlyDayDic
        {
            get { return _schMonthlyDayDic ?? (_schMonthlyDayDic = Enumerable.Range(1, 31).ToDictionary(k => k.ToString(), v => v.ToString())); }
        }

        SystemEDIFlowDetails _entitySystemEDIFlowDetail;
        public SystemEDIFlowDetails EntitySystemEDIFlowDetail { get { return _entitySystemEDIFlowDetail; } }
        #endregion

        #region - Private -
        private readonly EntitySystemEDIFlowDetail _entity;
        #endregion

        #region - Validation -
        #endregion

        #region - 表單初始 -
        public void FormReset()
        {
            EDIFlowZHTW = string.Empty;
            EDIFlowZHCN = string.Empty;
            EDIFlowENUS = string.Empty;
            EDIFlowTHTH = string.Empty;
            EDIFlowJAJP = string.Empty;
            EDIFlowKOKR = string.Empty;
            SCHStartDate = string.Empty;
            SCHStartTime = string.Empty;
            SCHDataDelay = string.Empty;
            KeepLogDay = "3";
            PATHSCmd = string.Empty;
            PATHSDat = string.Empty;
            PATHSSrc = string.Empty;
            PATHSRes = string.Empty;
            PATHSBad = string.Empty;
            PATHSLog = string.Empty;
            PATHSFlowXml = string.Empty;
            PATHSFlowCmd = string.Empty;
            PATHSZipDat = string.Empty;
            PATHSException = string.Empty;
            PATHSSummary = string.Empty;
            SortOrder = string.Empty;
        }
        #endregion

        #region - Validation -
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var schFrequency = (EnumFrequency)Enum.Parse(typeof(EnumFrequency), SCHFrequency, false);

            #region - 排程頻率必填驗證 -
            switch (schFrequency)
            {
                case EnumFrequency.Daily:
                    if (ExecuteTimeList == null || ExecuteTimeList.Any() == false)
                    {
                        yield return new ValidationResult(SysSystemEDIFlowDetail.SystemMsg_IsValidExecuteTimeRequired_Failure);
                    }
                    if (string.IsNullOrWhiteSpace(SCHIntervalNum))
                    {
                        yield return new ValidationResult(SysSystemEDIFlowDetail.SystemMsg_IsValidSCHIntervalNumRequired_Failure);
                    }
                    break;
                case EnumFrequency.Weekly:
                    if (ExecuteTimeList == null || ExecuteTimeList.Any() == false)
                    {
                        yield return new ValidationResult(SysSystemEDIFlowDetail.SystemMsg_IsValidExecuteTimeRequired_Failure);
                    }
                    if (string.IsNullOrWhiteSpace(SCHIntervalNum))
                    {
                        yield return new ValidationResult(SysSystemEDIFlowDetail.SystemMsg_IsValidSCHIntervalNumRequired_Failure);
                    }
                    if (SCHExecuteWeeklyList.Any() == false)
                    {
                        yield return new ValidationResult(SysSystemEDIFlowDetail.SystemMsg_IsValidSCHExecuteWeeklyListRequired_Failure);
                    }
                    break;
                case EnumFrequency.Monthly:
                    if (ExecuteTimeList == null || ExecuteTimeList.Any() == false)
                    {
                        yield return new ValidationResult(SysSystemEDIFlowDetail.SystemMsg_IsValidExecuteTimeRequired_Failure);
                    }
                    if (string.IsNullOrWhiteSpace(SCHIntervalNum))
                    {
                        yield return new ValidationResult(SysSystemEDIFlowDetail.SystemMsg_IsValidSCHIntervalNumRequired_Failure);
                    }
                    if (SCHExecuteDayList.Any() == false)
                    {
                        yield return new ValidationResult(SysSystemEDIFlowDetail.SysMsg_IsValidSCHExecuteDayListRequired_Failure);
                    }
                    break;
                case EnumFrequency.FixedTime:
                    if (string.IsNullOrWhiteSpace(SCHIntervalTime))
                    {
                        yield return new ValidationResult(SysSystemEDIFlowDetail.SystemMsg_IsValidSCHIntervalTimeRequired_Failure);
                    }
                    if (string.IsNullOrWhiteSpace(SCHEndTime))
                    {
                        yield return new ValidationResult(SysSystemEDIFlowDetail.SystemMsg_IsValidSCHEndTimeRequired_Failure);
                    }
                    break;
            }
            #endregion
        }
        #endregion
        
        public bool IsHasSysAuth()
        {
            return UserSystemByIdList.Exists(row => SysID == row.SysID);
        }

        public async Task<bool> GetSystemEDIFlowDetail(string userID)
        {
            try
            {
                string apiUrl = API.SystemEDIFlow.QuerySystemEDIFlowDetail(userID, SysID, EDIFlowID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    systemEDIFlowDetails = (SystemEDIFlowDetails)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    _entitySystemEDIFlowDetail = responseObj.systemEDIFlowDetails;
                }

                if (_entitySystemEDIFlowDetail != null)
                {
                    if (ExecAction == EnumActionType.Update)
                    {
                        EDIFlowZHTW = EntitySystemEDIFlowDetail.EDIFlowZHTW;
                        EDIFlowZHCN = EntitySystemEDIFlowDetail.EDIFlowZHCN;
                        EDIFlowENUS = EntitySystemEDIFlowDetail.EDIFlowENUS;
                        EDIFlowTHTH = EntitySystemEDIFlowDetail.EDIFlowTHTH;
                        EDIFlowJAJP = EntitySystemEDIFlowDetail.EDIFlowJAJP;
                        EDIFlowKOKR = EntitySystemEDIFlowDetail.EDIFlowKOKR;
                        SCHFrequency = EntitySystemEDIFlowDetail.SCHFrequency;
                        SCHStartDate = EntitySystemEDIFlowDetail.SCHStartDate;
                        SCHStartTime = EntitySystemEDIFlowDetail.SCHStartTime;

                        if (EntitySystemEDIFlowDetail.SCHIntervalNum != 0)
                        {
                            SCHIntervalNum = EntitySystemEDIFlowDetail.SCHIntervalNum.ToString();
                        }
                        else
                        {
                            SCHIntervalNum = "1";
                        }

                        if (EntitySystemEDIFlowDetail.SCHIntervalTime.ToString() != "")
                        {
                            SCHIntervalTime = EntitySystemEDIFlowDetail.SCHIntervalTime.ToString();
                        }
                        SCHEndTime = EntitySystemEDIFlowDetail.SCHEndTime;
                        SCHExecuteWeeklyList = ConvertTo2Ary(Convert.ToInt32(EntitySystemEDIFlowDetail.SCHWeeks)).ToList();
                        SCHExecuteDayList = string.IsNullOrWhiteSpace(EntitySystemEDIFlowDetail.SCHDaysStr) ? new List<string>() : EntitySystemEDIFlowDetail.SCHDaysStr.Split(',').ToList();
                        SCHDataDelay = EntitySystemEDIFlowDetail.SCHDataDelay.ToString();
                        KeepLogDay = string.IsNullOrWhiteSpace(EntitySystemEDIFlowDetail.SCHKeepLogDay) ? null : EntitySystemEDIFlowDetail.SCHKeepLogDay.ToString();
                    }
                    else
                    {
                        EDIFlowID = string.Empty;
                    }

                    PATHSCmd = EntitySystemEDIFlowDetail.PATHSCmd;
                    PATHSDat = EntitySystemEDIFlowDetail.PATHSDat;
                    PATHSSrc = EntitySystemEDIFlowDetail.PATHSSrc;
                    PATHSRes = EntitySystemEDIFlowDetail.PATHSRes;
                    PATHSBad = EntitySystemEDIFlowDetail.PATHSBad;
                    PATHSLog = EntitySystemEDIFlowDetail.PATHSLog;
                    PATHSFlowXml = EntitySystemEDIFlowDetail.PATHSFlowXml;
                    PATHSFlowCmd = EntitySystemEDIFlowDetail.PATHSFlowCmd;
                    PATHSZipDat = EntitySystemEDIFlowDetail.PATHSZipDat;
                    PATHSException = EntitySystemEDIFlowDetail.PATHSException;
                    PATHSSummary = EntitySystemEDIFlowDetail.PATHSSummary;
                    SortOrder = EntitySystemEDIFlowDetail.SortOrder;
                    
                    if (SCHFrequency == EntitySys.SCHFrequencyField.FixedTime.ToString())
                    {
                        ExecuteTimeList = new List<string>();
                    }
                    else
                    {
                        ExecuteTimeList = (EntitySystemEDIFlowDetail.ExeCuteTIME != null) ? EntitySystemEDIFlowDetail.ExeCuteTIME.ToString().Split(',').ToList() : new List<string>();    
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetEditSystemEDIFlowDetailResult(string userID)
        {
            try
            {
                var schFrequency = (EnumFrequency)Enum.Parse(typeof(EnumFrequency), SCHFrequency);

                List<SystemEDIFixEDTime> editSystemEDIFixEDTimes = new List<SystemEDIFixEDTime>();
                SystemEDIFlowDetail para = new SystemEDIFlowDetail();
                para.SystemEDIFlowDetails = new SystemEDIFlowDetails()
                {
                    SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                    EDIFlowID = string.IsNullOrWhiteSpace(EDIFlowID) ? null : EDIFlowID,
                    EDIFlowZHTW = string.IsNullOrWhiteSpace(EDIFlowZHTW) ? null : EDIFlowZHTW,
                    EDIFlowZHCN = string.IsNullOrWhiteSpace(EDIFlowZHCN) ? null : EDIFlowZHCN,
                    EDIFlowENUS = string.IsNullOrWhiteSpace(EDIFlowENUS) ? null : EDIFlowENUS,
                    EDIFlowTHTH = string.IsNullOrWhiteSpace(EDIFlowTHTH) ? null : EDIFlowTHTH,
                    EDIFlowJAJP = string.IsNullOrWhiteSpace(EDIFlowJAJP) ? null : EDIFlowJAJP,
                    EDIFlowKOKR = string.IsNullOrWhiteSpace(EDIFlowKOKR) ? null : EDIFlowKOKR,
                    SCHFrequency = string.IsNullOrWhiteSpace(SCHFrequency) ? null : SCHFrequency,
                    SCHStartDate = string.IsNullOrWhiteSpace(SCHStartDate) ? null : SCHStartDate,
                    SCHStartTime = string.IsNullOrWhiteSpace(SCHStartTime) ? null : SCHStartTime,
                    SCHIntervalTime = string.IsNullOrWhiteSpace(SCHIntervalTime) ? 0 : Convert.ToInt32(SCHIntervalTime),
                    SCHEndTime = string.IsNullOrWhiteSpace(SCHEndTime) ? null : SCHEndTime,
                    SCHDataDelay = string.IsNullOrWhiteSpace(SCHDataDelay) ? null : SCHDataDelay,
                    PATHSCmd = string.IsNullOrWhiteSpace(PATHSCmd) ? null : PATHSCmd,
                    PATHSDat = string.IsNullOrWhiteSpace(PATHSDat) ? null : PATHSDat,
                    PATHSSrc = string.IsNullOrWhiteSpace(PATHSSrc) ? null : PATHSSrc,
                    PATHSRes = string.IsNullOrWhiteSpace(PATHSRes) ? null : PATHSRes,
                    PATHSBad = string.IsNullOrWhiteSpace(PATHSBad) ? null : PATHSBad,
                    PATHSLog = string.IsNullOrWhiteSpace(PATHSLog) ? null : PATHSLog,
                    PATHSFlowXml = string.IsNullOrWhiteSpace(PATHSFlowXml) ? null : PATHSFlowXml,
                    PATHSFlowCmd = string.IsNullOrWhiteSpace(PATHSFlowCmd) ? null : PATHSFlowCmd,
                    PATHSZipDat = string.IsNullOrWhiteSpace(PATHSZipDat) ? null : PATHSZipDat,
                    PATHSException = string.IsNullOrWhiteSpace(PATHSException) ? null : PATHSException,
                    PATHSSummary = string.IsNullOrWhiteSpace(PATHSSummary) ? null : PATHSSummary,
                    SortOrder = string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder,
                    SCHKeepLogDay = string.IsNullOrWhiteSpace(KeepLogDay) ? null : KeepLogDay,
                    UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID
                };

                switch (schFrequency)
                {
                    case EnumFrequency.FixedTime:
                        var startDateTime = Common.GetDateTime(Common.FormatDateString(Common.GetDateString()), Common.FormatTimeString(SCHStartTime));
                        var endDateTime = Common.GetDateTime(Common.FormatDateString(Common.GetDateString()), Common.FormatTimeString(SCHEndTime));
                        var schIntervalTime = Convert.ToInt32(SCHIntervalTime);
                        var count = (int)((endDateTime - startDateTime).TotalMinutes) / schIntervalTime + 1;
                        para.SystemEDIFlowDetails.ExecuteTimeList =
                            (from s in Enumerable.Range(1, count).Select(index => startDateTime.AddMinutes(index * schIntervalTime))
                             where s <= endDateTime
                             select Common.GetTimeString(s)).ToList();
                        break;
                    case EnumFrequency.Daily:
                    case EnumFrequency.Monthly:
                    case EnumFrequency.Weekly:
                        if (ExecuteTimeList != null &&
                            ExecuteTimeList.Any())
                        {
                            para.SystemEDIFlowDetails.ExecuteTimeList =
                                (from s in ExecuteTimeList
                                 where string.IsNullOrWhiteSpace(s) == false
                                 select s.PadRight(9, '0')).ToList();
                        }
                        break;
                    default:
                        para.SystemEDIFlowDetails.ExecuteTimeList = new List<string>();
                        break;
                }

                if (schFrequency == EnumFrequency.Monthly)
                {
                    para.SystemEDIFlowDetails.SCHDaysStr = SCHExecuteDayList != null && SCHExecuteDayList.Any() ? string.Join(",", SCHExecuteDayList) : null;
                }
                else
                {
                    para.SystemEDIFlowDetails.SCHDaysStr = (null);
                }
                if (schFrequency == EnumFrequency.Weekly)
                {
                    if (SCHExecuteWeeklyList != null)
                    {
                        para.SystemEDIFlowDetails.SCHWeeks = ((from s in SCHExecuteWeeklyList
                                                   select Convert.ToInt32(s)).Sum(s => s));
                    }
                    else
                    {
                        para.SystemEDIFlowDetails.SCHWeeks = (null);
                    }
                }
                
                if (schFrequency == EnumFrequency.Continuity ||
                    schFrequency == EnumFrequency.FixedTime)
                {
                    para.SystemEDIFlowDetails.SCHIntervalNum = (null);
                    if (schFrequency == EnumFrequency.Continuity)
                    {
                        para.SystemEDIFlowDetails.SCHIntervalTime = (null);
                    }
                }
                else
                {
                    para.SystemEDIFlowDetails.SCHIntervalTime = (null);
                    para.SystemEDIFlowDetails.SCHIntervalNum = string.IsNullOrWhiteSpace(SCHIntervalNum) ? 1 : Convert.ToInt32(SCHIntervalNum);
                }

                if (para.SystemEDIFlowDetails.ExecuteTimeList != null)
                {
                    foreach (var row in para.SystemEDIFlowDetails.ExecuteTimeList)
                    {
                        para.SystemEDIFixEDTime.Add(new SystemEDIFixEDTime
                        {
                            SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                            EDIFlowID = string.IsNullOrWhiteSpace(EDIFlowID) ? null : EDIFlowID,
                            UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID,
                            ExeCuteTIME = row
                        });
                    }
                }

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};


                var paraJsonStr = Common.GetJsonSerializeObject(para);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.SystemEDIFlow.EditSystemEDIFlowDetails(userID, SysID, EDIFlowID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetSortOrder(string userID)
        {
            try
            {
                string newSortOrder = Common.GetEnumDesc(SortorderField.First);
                string apiUrl = API.SystemEDIFlow.QueryFlowNewSortOrder(userID, SysID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                if (response != null)
                {
                    newSortOrder = response.ToString().Replace("0", null);
                    if (!string.IsNullOrWhiteSpace(newSortOrder))
                    {
                        newSortOrder = Convert.ToString(Convert.ToInt32(newSortOrder) + 1);
                        newSortOrder = Common.GetEnumDesc(SortorderField.Left) + newSortOrder + Common.GetEnumDesc(SortorderField.Right);
                        newSortOrder = newSortOrder.Substring(newSortOrder.Length - 6, 6);
                    }
                }
                this.SortOrder = newSortOrder;

                if (string.IsNullOrWhiteSpace(this.SortOrder) == false)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public async Task<EnumDeleteSystemEDIFlowDetailResult> GetDeleteSystemEDIFlowDetailResult(string userID)
        {
            var result = EnumDeleteSystemEDIFlowDetailResult.Failure;
            try
            {
                string apiUrl = API.SystemEDIFlow.DeleteSystemEDIFlowDetails(userID, SysID, EDIFlowID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                if (response == EnumDeleteSystemEDIFlowDetailResult.Success.ToString())
                {
                    return EnumDeleteSystemEDIFlowDetailResult.Success;
                }
                else 
                {
                    return EnumDeleteSystemEDIFlowDetailResult.DataExist;
                }
            }
            catch (WebException webException)
                when (webException.Response is HttpWebResponse &&
                      ((HttpWebResponse)webException.Response).StatusCode == HttpStatusCode.BadRequest)
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse)webException.Response;

                if (string.IsNullOrWhiteSpace(httpWebResponse.StatusDescription) == false)
                {
                    string errorMsg = Common.GetStreamToString(httpWebResponse.GetResponseStream(), Encoding.UTF8);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    {
                        if (errorMsg == EnumDeleteSystemEDIFlowDetailResult.Failure.ToString())
                        {
                            result = EnumDeleteSystemEDIFlowDetailResult.Failure;
                        }
                    }
                }
            }
            return result;
        }
        
        #region - 10轉2進制 -
        /// <summary>
        /// 10轉2進制
        /// </summary>
        /// <param name="value">10進制</param>
        /// <returns>2進制陣列</returns>
        public IEnumerable<string> ConvertTo2Ary(int value)
        {
            var result = new List<string>();
            for (var index = 0; Math.Pow(2, index) <= value; index++)
            {
                if ((value & Convert.ToInt32(Math.Pow(2, index))) == Convert.ToInt32(Math.Pow(2, index)))
                {
                    result.Add(Convert.ToString(Math.Pow(2, index)));
                }
            }
            return result;
        }
        #endregion
    }
}