using LionTech.Entity;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemSettingDetailModel : SysModel
    {
        #region - Enum -
        public enum EnumDeleteSystemSettingDetailResult
        {
            Success, Failure, DataExist
        }
        #endregion

        #region - Property -
        [Required]
        [StringLength(12, MinimumLength = 4)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string SysID { get; set; }

        private string _sysManUserId;

        [Required]
        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string SysMANUserID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_sysManUserId))
                {
                    return _sysManUserId;
                }
                return _sysManUserId.ToUpper();
            }
            set
            {
                _sysManUserId = value;
            }
        }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMKOKR { get; set; }

        [Required]
        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string SysIndexPath { get; set; }

        public string SysIconPath { get; set; }

        public string SysKey { get; set; }

        public string ENSysID { get; set; }

        public string IsOutsourcing { get; set; }

        public string IsDisable { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            SysMANUserID = string.Empty;
            SysNMZHTW = string.Empty;
            SysNMZHCN = string.Empty;
            SysNMENUS = string.Empty;
            SysNMTHTH = string.Empty;
            SysNMJAJP = string.Empty;
            SysNMKOKR = string.Empty;
            SysIndexPath = string.Empty;
            SysIconPath = string.Empty;
            SysKey = string.Empty;
            ENSysID = string.Empty;
            IsOutsourcing = EnumYN.N.ToString();
            IsDisable = EnumYN.N.ToString();
            SortOrder = string.Empty;
        }
        #endregion

        #region - Tab -
        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName = string.Empty,
                ActionName = string.Empty,
                TabText = SysSystemSettingDetail.TabText_SystemSettingDetail,
                ImageURL = string.Empty
            }
        };
        #endregion

        public async Task<bool> IsExistSystem(string userID, string sysID)
        {
            try
            {
                string apiUrl = API.SystemSetting.QuerySystemSetting(sysID, userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                return true;
            }
            catch (WebException webException)
                when (webException.Response is HttpWebResponse &&
                      ((HttpWebResponse) webException.Response).StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        public async Task<bool> GetSystemMain(string userID, string sysID)
        {
            try
            {
                string apiUrl = API.SystemSetting.QuerySystemSetting(sysID, userID);
                var response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SysID = (string)null,
                    SysMANUserID = (string)null,
                    SysNMZHTW = (string)null,
                    SysNMZHCN = (string)null,
                    SysNMENUS = (string)null,
                    SysNMTHTH = (string)null,
                    SysNMJAJP = (string)null,
                    SysNMKOKR = (string)null,
                    SysIndexPath = (string)null,
                    SysKey = (string)null,
                    ENSysID = (string)null,
                    IsOutsourcing = (string)null,
                    IsDisable = (string)null,
                    SortOrder = (string)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    SysMANUserID = responseObj.SysMANUserID;
                    SysNMZHTW = responseObj.SysNMZHTW;
                    SysNMZHCN = responseObj.SysNMZHCN;
                    SysNMENUS = responseObj.SysNMENUS;
                    SysNMTHTH = responseObj.SysNMTHTH;
                    SysNMJAJP = responseObj.SysNMJAJP;
                    SysNMKOKR = responseObj.SysNMKOKR;
                    SysIndexPath = responseObj.SysIndexPath;
                    SysKey = responseObj.SysKey;
                    ENSysID = responseObj.ENSysID;
                    IsOutsourcing = responseObj.IsOutsourcing;
                    IsDisable = responseObj.IsDisable;
                    SortOrder = responseObj.SortOrder;
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> EditSystemSetting(string clientUserID, string clientIPAddress)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                {
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                };

                var paraJsonStr = Common.GetJsonSerializeObject(new {
                    SysID = SysID,
                    SysMANUserID = SysMANUserID,
                    SysNMZHTW = SysNMZHTW,
                    SysNMZHCN = SysNMZHCN,
                    SysNMENUS = SysNMENUS,
                    SysNMTHTH = SysNMTHTH,
                    SysNMJAJP = SysNMJAJP,
                    SysNMKOKR = SysNMKOKR,
                    SysIndexPath = SysIndexPath,
                    SysIconPath = SysIconPath,
                    SysKey = LionTech.Utility.Validator.GetEncodeString(RandomString.Generate(31)),
                    ENSysID = Token.Encrypt(string.IsNullOrWhiteSpace(SysID) ? string.Empty : SysID),
                    IsOutsourcing = string.IsNullOrWhiteSpace(IsOutsourcing) ? EnumYN.N.ToString() : IsOutsourcing,
                    IsDisable = string.IsNullOrWhiteSpace(IsDisable) ? EnumYN.N.ToString() : EnumYN.Y.ToString(),
                    SortOrder = SortOrder
                });
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemSetting.EditSystemSetting(clientUserID, clientIPAddress);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
                
                return true;
            }
            catch (WebException webException)
                when (webException.Response is HttpWebResponse &&
                      ((HttpWebResponse)webException.Response).StatusCode == HttpStatusCode.BadRequest)
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse)webException.Response;

                if (string.IsNullOrWhiteSpace(httpWebResponse.StatusDescription) == false)
                {
                    string errorMsg = Common.GetStreamToString(httpWebResponse.GetResponseStream(), Encoding.UTF8);

                    if (errorMsg == null)
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public async Task<EnumDeleteSystemSettingDetailResult> DeleteSystemSetting (string clientUserID, string sysID)
        {
            var result = EnumDeleteSystemSettingDetailResult.Failure;
            try
            {
                string apiUrl = API.SystemSetting.DeleteSystemSetting(sysID, clientUserID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                result = EnumDeleteSystemSettingDetailResult.Success;
                return result;
            }
            catch (WebException webException)
                when (webException.Response is HttpWebResponse &&
                      ((HttpWebResponse)webException.Response).StatusCode == HttpStatusCode.BadRequest)
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse)webException.Response;

                if (string.IsNullOrWhiteSpace(httpWebResponse.StatusDescription) == false)
                {
                    string errorMsg = Common.GetStreamToString(httpWebResponse.GetResponseStream(), Encoding.UTF8);
                    var msg = new { Message = (string)null };
                    msg = Common.GetJsonDeserializeAnonymousType(errorMsg, msg);
                    
                    if(msg.Message == EnumDeleteSystemSettingDetailResult.DataExist.ToString())
                    {
                        result = EnumDeleteSystemSettingDetailResult.DataExist;
                    }
                }
            }
            return result;
        }
    }
}