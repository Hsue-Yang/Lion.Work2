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
    public class SystemAPIGroupDetailModel : SysModel
    {
        #region - Enum -
        public enum EnumDeleteSystemAPIGroupDetailResult
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
        public string APIGroupID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string APIGroupZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string APIGroupZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string APIGroupENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string APIGroupTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string APIGroupJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string APIGroupKOKR { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            APIGroupZHTW = string.Empty;
            APIGroupZHCN = string.Empty;
            APIGroupENUS = string.Empty;
            APIGroupTHTH = string.Empty;
            APIGroupJAJP = string.Empty;
            APIGroupKOKR = string.Empty;
            SortOrder = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemAPIGroupDetail(string userID)
        {
            try
            {
                string apiUrl = API.SystemAPIGroup.QuerySystemAPIGroup(SysID, userID, APIGroupID);
                var response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    APIGroupZHTW = (string)null,
                    APIGroupZHCN = (string)null,
                    APIGroupENUS = (string)null,
                    APIGroupTHTH = (string)null,
                    APIGroupJAJP = (string)null,
                    APIGroupKOKR = (string)null,
                    SortOrder = (string)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    APIGroupZHTW = responseObj.APIGroupZHTW;
                    APIGroupZHCN = responseObj.APIGroupZHCN;
                    APIGroupENUS = responseObj.APIGroupENUS;
                    APIGroupTHTH = responseObj.APIGroupTHTH;
                    APIGroupJAJP = responseObj.APIGroupJAJP;
                    APIGroupKOKR = responseObj.APIGroupKOKR;
                    SortOrder = responseObj.SortOrder;
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public async Task<bool> EditSystemAPIGroupDetail(string userID)
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
                    APIGroupID,
                    APIGroupZHTW,
                    APIGroupZHCN,
                    APIGroupENUS,
                    APIGroupTHTH,
                    APIGroupJAJP,
                    APIGroupKOKR,
                    SortOrder,
                    UpdUserID = userID
                });
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemAPIGroup.EditSystemAPIGroup(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<EnumDeleteSystemAPIGroupDetailResult> GetDeleteSystemAPIGroupDetailResult(string userID)
        {
            var result = EnumDeleteSystemAPIGroupDetailResult.Failure;
            try
            {
                string apiUrl = API.SystemAPIGroup.DeleteSystemAPIGroup(SysID, userID, APIGroupID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                result = EnumDeleteSystemAPIGroupDetailResult.Success;
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

                    if (msg.Message == EnumDeleteSystemAPIGroupDetailResult.DataExist.ToString())
                    {
                        result = EnumDeleteSystemAPIGroupDetailResult.DataExist;
                    }
                }
            }
            return result;
        }

        #region Event
        public string GetEventParaSysSystemAPIGroupEdit()
        {
            var eventParaSystemAPIGroupEdit = new
            {
                TargetSysIDList = new List<string> { SysID },
                SysID,
                APIControllerID = APIGroupID,
                APIGroupzhTW = APIGroupZHTW,
                APIGroupzhCN = APIGroupZHCN,
                APIGroupenUS = APIGroupENUS,
                APIGroupthTH = APIGroupTHTH,
                APIGroupjaJP = APIGroupJAJP,
                APIGroupkoKR = APIGroupKOKR,
                SortOrder
            };

            return Common.GetJsonSerializeObject(eventParaSystemAPIGroupEdit);
        }

        public string GetEventParaSysSystemAPIGroupDelete()
        {
            var eventParaSystemAPIGroupDelete = new
            {
                TargetSysIDList = new List<string> { SysID },
                SysID,
                APIControllerID = APIGroupID
            };

            return Common.GetJsonSerializeObject(eventParaSystemAPIGroupDelete);
        }
        #endregion
    }
}