using LionTech.Entity;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemEventDetailModel : SysModel
    {
        #region - Enum -
        public enum EnumDeleteResult
        {
            Success, Failure, DataExist
        }
        #endregion

        #region - Property -
        [Required]
        public string SysID { get; set; }

        [Required]
        public string EventGroupID { get; set; }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string EventID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EventNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EventNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EventNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EventNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EventNMJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EventNMKOKR { get; set; }

        [Required]
        [StringLength(2000)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string EventPara { get; set; }

        public string IsDisable { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string Remark { get; set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            EventNMZHTW = string.Empty;
            EventNMZHCN = string.Empty;
            EventNMENUS = string.Empty;
            EventNMTHTH = string.Empty;
            EventNMJAJP = string.Empty;
            EventNMKOKR = string.Empty;
            EventPara = string.Empty;
            IsDisable = string.Empty;
            SortOrder = string.Empty;
            Remark = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemEventDetail(string userID)
        {
            try
            {
                string apiUrl = API.SystemEvent.QuerySystemEvent(SysID, userID, EventGroupID, EventID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    EventNMZHTW = (string)null,
                    EventNMZHCN = (string)null,
                    EventNMENUS = (string)null,
                    EventNMTHTH = (string)null,
                    EventNMJAJP = (string)null,
                    EventNMKOKR = (string)null,
                    EventPara = (string)null,
                    IsDisable = IsDisable ?? EnumYN.N.ToString(),
                    SortOrder = (string)null,
                    Remark = (string)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    EventNMZHTW = responseObj.EventNMZHTW;
                    EventNMZHCN = responseObj.EventNMZHCN;
                    EventNMENUS = responseObj.EventNMENUS;
                    EventNMTHTH = responseObj.EventNMTHTH;
                    EventNMJAJP = responseObj.EventNMJAJP;
                    EventNMKOKR = responseObj.EventNMKOKR;
                    EventPara = responseObj.EventPara;
                    IsDisable = responseObj.IsDisable;
                    SortOrder = responseObj.SortOrder;
                    Remark = responseObj.Remark;

                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> EditSystemEventDetail(string userID)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                {
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                };

                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    SysID,
                    EventGroupID,
                    EventID,
                    EventNMZHTW,
                    EventNMZHCN,
                    EventNMENUS,
                    EventNMTHTH,
                    EventNMJAJP,
                    EventNMKOKR,
                    EventPara,
                    IsDisable = IsDisable ?? EnumYN.N.ToString(),
                    SortOrder,
                    Remark,
                    UpdUserID = userID
                });

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemEvent.EditSystemEvent(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<EnumDeleteResult> GetDeleteSystemEventDetailResult(string userID)
        {
            var result = EnumDeleteResult.Failure;
            try
            {
                string apiUrl = API.SystemEvent.DeleteSystemEvent(SysID, userID, EventGroupID, EventID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                result = EnumDeleteResult.Success;
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

                    if (msg.Message == EnumDeleteResult.DataExist.ToString())
                    {
                        result = EnumDeleteResult.DataExist;
                    }
                }
            }
            return result;
        }
    }
}