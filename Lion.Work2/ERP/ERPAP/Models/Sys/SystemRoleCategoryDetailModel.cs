using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;

namespace ERPAP.Models.Sys
{
    public class SystemRoleCategoryDetailModel : SysModel
    {
        #region - Enum -
        public enum EnumDeleteSystemRoleCategoryDetailResult
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
        public string RoleCategoryID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleCategoryNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleCategoryNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleCategoryNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleCategoryNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleCategoryNMJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string RoleCategoryNMKOKR { get; set; }

        [StringLength(6)]
        [InputType(EnumInputType.TextBox)]
        public string SortOrder { get; set; }
        #endregion
        
        #region - Reset -
        public void FormReset()
        {
            this.RoleCategoryNMZHTW = string.Empty;
            this.RoleCategoryNMZHCN = string.Empty;
            this.RoleCategoryNMENUS = string.Empty;
            this.RoleCategoryNMTHTH = string.Empty;
            this.RoleCategoryNMJAJP = string.Empty;
            this.RoleCategoryNMKOKR = string.Empty;
            this.SortOrder = string.Empty;
        }
        #endregion
        
        public async Task<bool> GetSystemRoleCategoryDetail(string userID)
        {
            try
            {
                string apiUrl = API.SystemRoleCategory.QuerySystemRoleCategory(SysID, userID, RoleCategoryID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SysID = (string)null,
                    RoleCategoryID = (string)null,
                    RoleID = (string)null,
                    RoleCategoryNMZHTW = (string)null,
                    RoleCategoryNMZHCN = (string)null,
                    RoleCategoryNMENUS = (string)null,
                    RoleCategoryNMTHTH = (string)null,
                    RoleCategoryNMJAJP = (string)null,
                    RoleCategoryNMKOKR = (string)null,
                    SortOrder = (string)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    SysID = responseObj.SysID;
                    RoleCategoryID = responseObj.RoleCategoryID;
                    RoleCategoryNMZHTW = responseObj.RoleCategoryNMZHTW;
                    RoleCategoryNMZHCN = responseObj.RoleCategoryNMZHCN;
                    RoleCategoryNMENUS = responseObj.RoleCategoryNMENUS;
                    RoleCategoryNMTHTH = responseObj.RoleCategoryNMTHTH;
                    RoleCategoryNMJAJP = responseObj.RoleCategoryNMJAJP;
                    RoleCategoryNMKOKR = responseObj.RoleCategoryNMKOKR;
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

        public async Task<bool> EditSystemRoleCategoryDetail(string userID)
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
                    RoleCategoryID,
                    RoleCategoryNMZHTW,
                    RoleCategoryNMZHCN,
                    RoleCategoryNMENUS,
                    RoleCategoryNMTHTH,
                    RoleCategoryNMJAJP,
                    RoleCategoryNMKOKR,
                    SortOrder,
                    UpdUserID = userID
                });
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemRoleCategory.EditSystemRoleCategory(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<EnumDeleteSystemRoleCategoryDetailResult> DeleteSystemRoleCategoryDetail(string userID)
        {
            var result = EnumDeleteSystemRoleCategoryDetailResult.Failure;
            try
            {
                string apiUrl = API.SystemRoleCategory.DeleteSystemRoleCategory(SysID, userID, RoleCategoryID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                result = EnumDeleteSystemRoleCategoryDetailResult.Success;
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

                    if (msg.Message == EnumDeleteSystemRoleCategoryDetailResult.DataExist.ToString())
                    {
                        result = EnumDeleteSystemRoleCategoryDetailResult.DataExist;
                    }
                }
            }
            return result;
        }

        #region - Event -
        public string GetEventParaSysSystemRoleCategoryEdit()
        {
            try
            {
                var entityEventParaSysSystemRoleCategoryEdit = new 
                {
                    TargetSysIDList = new List<string> {SysID},
                    SysID,
                    RoleCategoryID,
                    RoleCategoryNMZHTW,
                    RoleCategoryNMZHCN,
                    RoleCategoryNMENUS,
                    RoleCategoryNMTHTH,
                    RoleCategoryNMJAJP,
                    RoleCategoryNMKOKR,
                    SortOrder
                };
                
                return Common.GetJsonSerializeObject(entityEventParaSysSystemRoleCategoryEdit);
            }

            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
        }

        public string GetEventParaSysSystemRoleCategoryDelete()
        {
            try
            {
                var entityEventParaSysSystemRoleCategoryDelete = new 
                {
                    TargetSysIDList = new List<string> {SysID},
                    SysID ,
                    RoleCategoryID,
                };

                return Common.GetJsonSerializeObject(entityEventParaSysSystemRoleCategoryDelete);
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
        }
        #endregion
    }
}