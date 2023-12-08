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
    public class SystemEventGroupDetailModel : SysModel
    {
        #region - Enum -
        public enum EnumDeleteSystemEventGroupDetailResult
        {
            Success, Failure, DataExist
        }
        #endregion

        #region - Property -
        [Required]
        public string SysID { get; set; }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string EventGroupID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EventGroupZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EventGroupZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EventGroupENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EventGroupTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EventGroupJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EventGroupKOKR { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            EventGroupZHTW = string.Empty;
            EventGroupZHCN = string.Empty;
            EventGroupENUS = string.Empty;
            EventGroupTHTH = string.Empty;
            EventGroupJAJP = string.Empty;
            EventGroupKOKR = string.Empty;
            SortOrder = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemEventGroupDetail(string userID)
        {
            try
            {
                string apiUrl = API.SystemEventGroup.QuerySystemEventGroup(SysID, userID, EventGroupID);
                var response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var systemEventGroupDetail = new
                {
                    EventGroupZHTW = (string)null,
                    EventGroupZHCN = (string)null,
                    EventGroupENUS = (string)null,
                    EventGroupTHTH = (string)null,
                    EventGroupJAJP = (string)null,
                    EventGroupKOKR = (string)null,
                    SortOrder = (string)null
                };

                systemEventGroupDetail = Common.GetJsonDeserializeAnonymousType(response, systemEventGroupDetail);

                if (systemEventGroupDetail != null)
                {
                    EventGroupZHTW = systemEventGroupDetail.EventGroupZHTW;
                    EventGroupZHCN = systemEventGroupDetail.EventGroupZHCN;
                    EventGroupENUS = systemEventGroupDetail.EventGroupENUS;
                    EventGroupTHTH = systemEventGroupDetail.EventGroupTHTH;
                    EventGroupJAJP = systemEventGroupDetail.EventGroupJAJP;
                    EventGroupKOKR = systemEventGroupDetail.EventGroupKOKR;
                    SortOrder = systemEventGroupDetail.SortOrder;

                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> EditSystemEventGroupDetail(string userID)
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
                    EventGroupZHTW,
                    EventGroupZHCN,
                    EventGroupENUS,
                    EventGroupTHTH,
                    EventGroupJAJP,
                    EventGroupKOKR,
                    SortOrder,
                    UpdUserID = userID
                });
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemEventGroup.EditSystemEventGroup(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<EnumDeleteSystemEventGroupDetailResult> GetDeleteSystemEventGroupDetailResult(string userID)
        {
            var result = EnumDeleteSystemEventGroupDetailResult.Failure;
            try
            {
                string apiUrl = API.SystemEventGroup.DeleteSystemEventGroup(SysID, userID, EventGroupID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                result = EnumDeleteSystemEventGroupDetailResult.Success;
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

                    if (msg.Message == EnumDeleteSystemEventGroupDetailResult.DataExist.ToString())
                    {
                        result = EnumDeleteSystemEventGroupDetailResult.DataExist;
                    }
                }
            }
            return result;
        }
    }
}