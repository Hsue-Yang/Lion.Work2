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
    public class SystemPurviewDetailModel : SysModel
    {
        #region - Enum -
        public enum EnumDeleteSystemPurviewDetailResult
        {
            Success, Failure, DataExist
        }
        #endregion

        #region - Property -
        [Required]
        public string SysID { get; set; }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string PurviewID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string PurviewNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string PurviewNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string PurviewNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string PurviewNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string PurviewNMJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string PurviewNMKOKR { get; set; }

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
            PurviewNMZHTW = string.Empty;
            PurviewNMZHCN = string.Empty;
            PurviewNMENUS = string.Empty;
            PurviewNMTHTH = string.Empty;
            PurviewNMJAJP = string.Empty;
            PurviewNMKOKR = string.Empty;
            SortOrder = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemPurviewDetail(string userID)
        {
            try
            {
                string apiUrl = API.SystemPurview.QuerySystemPurview(SysID, userID, PurviewID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    PurviewNMZHTW = (string)null,
                    PurviewNMZHCN = (string)null,
                    PurviewNMENUS = (string)null,
                    PurviewNMTHTH = (string)null,
                    PurviewNMJAJP = (string)null,
                    PurviewNMKOKR = (string)null,
                    Remark = (string)null,
                    SortOrder = (string)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    PurviewNMZHTW = responseObj.PurviewNMZHTW;
                    PurviewNMZHCN = responseObj.PurviewNMZHCN;
                    PurviewNMENUS = responseObj.PurviewNMENUS;
                    PurviewNMTHTH = responseObj.PurviewNMTHTH;
                    PurviewNMJAJP = responseObj.PurviewNMJAJP;
                    PurviewNMKOKR = responseObj.PurviewNMKOKR;
                    Remark = responseObj.Remark;
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

        public async Task<bool> EditSystemPurviewDetail(string userID)
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
                    PurviewID,
                    PurviewNMZHTW,
                    PurviewNMZHCN,
                    PurviewNMENUS,
                    PurviewNMTHTH,
                    PurviewNMJAJP,
                    PurviewNMKOKR,
                    SortOrder,
                    Remark,
                    UpdUserID = userID
                });

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemPurview.EditSystemPurview(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
               
               return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<EnumDeleteSystemPurviewDetailResult> DeleteSystemPurviewDetail(string userID)
        {
            var result = EnumDeleteSystemPurviewDetailResult.Failure;
            try
            {
                string apiUrl = API.SystemPurview.DeleteSystemPurview(SysID, userID, PurviewID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                result = EnumDeleteSystemPurviewDetailResult.Success;
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

                    if (msg.Message == EnumDeleteSystemPurviewDetailResult.DataExist.ToString())
                    {
                        result = EnumDeleteSystemPurviewDetailResult.DataExist;
                    }
                }
            }
            return result;
        }

        #region - Event -
        public string GetEventParaSysSystemPurviewEdit()
        {
            var eventParaSystemPurviewEdit = new
            {
                TargetSysIDList = new List<string> { SysID },
                SysID,
                PurviewID,
                PurviewNMZHTW,
                PurviewNMZHCN,
                PurviewNMENUS,
                PurviewNMTHTH,
                PurviewNMJAJP,
                PurviewNMKOKR,
                SortOrder,
                Remark
            };

            return Common.GetJsonSerializeObject(eventParaSystemPurviewEdit);
        }

        public string GetEventParaSysSystemPurviewDelete()
        {
            var eventParaSystemPurviewDelete = new
            {
                TargetSysIDList = new List<string> { SysID },
                SysID,
                PurviewID
            };

            return Common.GetJsonSerializeObject(eventParaSystemPurviewDelete);
        }
        #endregion
    }
}