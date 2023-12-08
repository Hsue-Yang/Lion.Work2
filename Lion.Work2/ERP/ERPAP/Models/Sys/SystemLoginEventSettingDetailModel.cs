// 新增日期：2017-02-17
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemLoginEventSettingDetailModel : SysModel, IValidatableObject
    {
        #region - Property -
        [Required]
        public string SysID { get; set; }
        public string SysNMID { get; set; }
        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string LoginEventID { get; set; }
        [Required]
        [StringLength(150)]
        public string LoginEventNMZHCN { get; set; }
        [Required]
        [StringLength(150)]
        public string LoginEventNMZHTW { get; set; }
        [Required]
        [StringLength(150)]
        public string LoginEventNMENUS { get; set; }
        [Required]
        [StringLength(150)]
        public string LoginEventNMTHTH { get; set; }
        [Required]
        [StringLength(150)]
        public string LoginEventNMJAJP { get; set; }
        [Required]
        [StringLength(150)]
        public string LoginEventNMKOKR { get; set; }
        [RegularExpression(@"^(19|20)\d{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])(([1-9]{1})|([0-1][0-9])|([1-2][0-3]))([0-5][0-9])\d{5}$",
            ErrorMessageResourceType = typeof(SysSystemLoginEventSettingDetail),
            ErrorMessageResourceName = nameof(SysSystemLoginEventSettingDetail.SystemMsg_StartDateTimeFormat_Failure))]
        [Required]
        [StringLength(17, MinimumLength = 17)]
        [InputType(EnumInputType.Other)]
        public string StartDT { get; set; }
        [RegularExpression(@"^(19|20)\d{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])(([1-9]{1})|([0-1][0-9])|([1-2][0-3]))([0-5][0-9])\d{5}$",
            ErrorMessageResourceType = typeof(SysSystemLoginEventSettingDetail),
            ErrorMessageResourceName = nameof(SysSystemLoginEventSettingDetail.SystemMsg_EndDateTimeFormat_Failure))]
        [Required]
        [StringLength(17, MinimumLength = 17)]
        [InputType(EnumInputType.Other)]
        public string EndDT { get; set; }
        [Range(1, 999)]
        [StringLength(3)]
        [InputType(EnumInputType.Other)]
        public string Frequency { get; set; }
        [RegularExpression(@"^(([1-9]{1})|([0-1][0-9])|([1-2][0-3])):([0-5][0-9])$",
            ErrorMessageResourceType = typeof(SysSystemLoginEventSettingDetail),
            ErrorMessageResourceName = nameof(SysSystemLoginEventSettingDetail.SystemMsg_StartExecTimeFormat_Failure))]
        [StringLength(5, MinimumLength = 4)]
        [InputType(EnumInputType.Other)]
        public string StartExecTime { get; set; }
        [RegularExpression(@"^(([1-9]{1})|([0-1][0-9])|([1-2][0-3])):([0-5][0-9])$",
            ErrorMessageResourceType = typeof(SysSystemLoginEventSettingDetail),
            ErrorMessageResourceName = nameof(SysSystemLoginEventSettingDetail.SystemMsg_EndExecTimeFormat_Failure))]
        [StringLength(5, MinimumLength = 4)]
        [InputType(EnumInputType.Other)]
        public string EndExecTime { get; set; }
        [Required]
        [RegularExpression(@"(((ht|f)tp(s?))\://{1}\S+)",
            ErrorMessageResourceType = typeof(SysSystemLoginEventSettingDetail),
            ErrorMessageResourceName = nameof(SysSystemLoginEventSettingDetail.SystemMsg_UrlFormat_Failure))]
        [StringLength(600)]
        public string TargetPath { get; set; }
        [RegularExpression(@"(((ht|f)tp(s?))\://{1}\S+)",
            ErrorMessageResourceType = typeof(SysSystemLoginEventSettingDetail),
            ErrorMessageResourceName = nameof(SysSystemLoginEventSettingDetail.SystemMsg_UrlFormat_Failure))]
        [StringLength(600)]
        public string ValidPath { get; set; }
        public string SubSysID { get; set; }
        public string IsDisable { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }
        #endregion

        #region - class -
        public class LoginEventSettingDetail
        {
            public string SysID;
            public string LoginEventID;
            public string LoginEventNMZHCN;
            public string LoginEventNMZHTW;
            public string LoginEventNMENUS;
            public string LoginEventNMTHTH;
            public string LoginEventNMJAJP;
            public string LoginEventNMKOKR;
            public DateTime StartDT;
            public DateTime EndDT;
            public string Frequency;
            public string StartExecTime;
            public string EndExecTime;
            public string TargetPath;
            public string ValidPath;
            public string SubSysID;
            public string IsDisable;
            public string SortOrder;
            public string UpdUserID;
        }
        #endregion

        #region - Validation -
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsValid == false)
            {
                yield break;
            }

            if (ExecAction == EnumActionType.Add)
            {
                #region - 驗證LoginEventID是否重複 -
                SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                LoginEventID = string.IsNullOrWhiteSpace(LoginEventID) ? null : LoginEventID;

                string apiUrl = API.SysLoginEvent.CheckSystemLineBotIdIsExists(SysID, LoginEventID);
                string response = Common.HttpWebRequestGetResponseString(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new { IsExists = false });

                if (responseObj.IsExists)
                {
                    yield return new ValidationResult(SysSystemLoginEventSettingDetail.SystemMsg_LoginEventIDRepeat);
                }
                #endregion
            }
        }
        #endregion

        #region - 取得登入事件設定明細主檔 -
        public async Task<bool> GetLoginEventSettingDetail(string userID, EnumCultureID cultureId)
        {
            try
            {
                string logineventID = this.LoginEventID ?? string.Empty;

                var sysSystemMain = GetSysSystemMain(SysID, cultureId);

                if (sysSystemMain != null)
                {
                    SysNMID = sysSystemMain.SysNMID;
                }

                string apiUrl = API.SysLoginEvent.QueryLoginEventSettingDetail(SysID, userID, logineventID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    LoginEventSettingDetail = (LoginEventSettingDetail)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    var loginEventSettingDetail = responseObj.LoginEventSettingDetail;
                    if (loginEventSettingDetail != null)
                    {
                        LoginEventID = loginEventSettingDetail.LoginEventID;
                        LoginEventNMZHCN = loginEventSettingDetail.LoginEventNMZHCN;
                        LoginEventNMZHTW = loginEventSettingDetail.LoginEventNMZHTW;
                        LoginEventNMENUS = loginEventSettingDetail.LoginEventNMENUS;
                        LoginEventNMTHTH = loginEventSettingDetail.LoginEventNMTHTH;
                        LoginEventNMJAJP = loginEventSettingDetail.LoginEventNMJAJP;
                        LoginEventNMKOKR = loginEventSettingDetail.LoginEventNMKOKR;
                        StartDT = Common.GetDateTimeString(loginEventSettingDetail.StartDT);
                        EndDT = Common.GetDateTimeString(loginEventSettingDetail.EndDT);
                        Frequency = (int.Parse(loginEventSettingDetail.Frequency) == 0) ? string.Empty : loginEventSettingDetail.Frequency.ToString();
                        StartExecTime = loginEventSettingDetail.StartExecTime;
                        EndExecTime = loginEventSettingDetail.EndExecTime;
                        TargetPath = loginEventSettingDetail.TargetPath;
                        ValidPath = loginEventSettingDetail.ValidPath;
                        SubSysID = loginEventSettingDetail.SubSysID;
                        IsDisable = loginEventSettingDetail.IsDisable;
                        SortOrder = loginEventSettingDetail.SortOrder;
                    }
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

        #region - 編輯登入事件設定明細 -
        public async Task<bool> EditLoginEventSettingDetail(string updUserID, EnumCultureID cultureId)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                {
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                };

                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                    LoginEventID = string.IsNullOrWhiteSpace(LoginEventID) ? null : LoginEventID,
                    LoginEventNMZHCN = string.IsNullOrWhiteSpace(LoginEventNMZHCN) ? null : LoginEventNMZHCN,
                    LoginEventNMZHTW = string.IsNullOrWhiteSpace(LoginEventNMZHTW) ? null : LoginEventNMZHTW,
                    LoginEventNMENUS = string.IsNullOrWhiteSpace(LoginEventNMENUS) ? null : LoginEventNMENUS,
                    LoginEventNMTHTH = string.IsNullOrWhiteSpace(LoginEventNMTHTH) ? null : LoginEventNMTHTH,
                    LoginEventNMJAJP = string.IsNullOrWhiteSpace(LoginEventNMJAJP) ? null : LoginEventNMJAJP,
                    LoginEventNMKOKR = string.IsNullOrWhiteSpace(LoginEventNMKOKR) ? null : LoginEventNMKOKR,
                    StartDT = string.IsNullOrWhiteSpace(StartDT) ? new DateTime?() : Common.GetDateTime(Common.FormatDateTimeString(StartDT)),
                    EndDT = string.IsNullOrWhiteSpace(EndDT) ? new DateTime?() : Common.GetDateTime(Common.FormatDateTimeString(EndDT)),
                    Frequency = string.IsNullOrWhiteSpace(Frequency) ? "0" : Frequency,
                    StartExecTime = string.IsNullOrWhiteSpace(StartExecTime) ? "00:00:00.000" : StartExecTime,
                    EndExecTime = string.IsNullOrWhiteSpace(EndExecTime) ? "23:59:59.999" : EndExecTime,
                    TargetPath = string.IsNullOrWhiteSpace(TargetPath) ? null : TargetPath,
                    ValidPath = string.IsNullOrWhiteSpace(ValidPath) ? null : ValidPath,
                    SubSysID = string.IsNullOrWhiteSpace(SubSysID) ? null : SubSysID,
                    IsDisable = IsDisable ?? EnumYN.N.ToString(),
                    SortOrder = string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder,
                    UpdUserID = updUserID
                });

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SysLoginEvent.EditLoginEventSettingDetail(updUserID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 刪除登入事件設定明細 -
        public async Task<bool> DeleteLoginEventSettingDetail(string userID)
        {
            try
            {
                string apiUrl = API.SysLoginEvent.DeleteLoginEventSettingDetail(SysID, userID, LoginEventID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

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