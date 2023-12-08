using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemAPIDetailModel : SysModel
    {
        #region - Enum -
        public enum EnumDeleteSystemAPIDetailResult
        {
            Success, Failure, DataExist
        }
        #endregion

        #region - Class -
        public class SystemAPIRole
        {
            public string RoleID { get; set; }
            public string RoleNM { get; set; }
            public string HasRole { get; set; }
        }
        #endregion

        #region - Property -
        [Required]
        public string SysID { get; set; }

        [Required]
        public string APIGroupID { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string APIFunID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string APINMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string APINMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string APINMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string APINMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string APINMJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string APINMKOKR { get; set; }

        [Required]
        [StringLength(300)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string APIPara { get; set; }

        [Required]
        public string APIReturn { get; set; }

        [StringLength(600)]
        public string APIReturnContent { get; set; }

        [StringLength(600)]
        public string APIParaDesc { get; set; }

        public string IsOutside { get; set; }

        public string IsDisable { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        public List<string> HasRole { get; set; }

        public List<SystemAPIRole> SystemAPIRoleList { get; set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            APINMZHTW = string.Empty;
            APINMZHCN = string.Empty;
            APINMENUS = string.Empty;
            APINMTHTH = string.Empty;
            APINMJAJP = string.Empty;
            APINMKOKR = string.Empty;
            APIPara = string.Empty;
            APIParaDesc = string.Empty;
            APIReturnContent = string.Empty;
            IsOutside = string.Empty;
            IsDisable = string.Empty;
            SortOrder = string.Empty;
            HasRole = new List<string>();
        }
        #endregion

        public async Task<bool> GetSystemAPIRoleList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemAPI.QuerySystemAPIRoles(SysID, userID, APIGroupID, APIFunID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemAPIRoleList = Common.GetJsonDeserializeObject<List<SystemAPIRole>>(response);

                if (SystemAPIRoleList != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetSystemAPIDetail(string userID)
        {
            try
            {
                string apiUrl = API.SystemAPI.QuerySystemAPI(SysID, userID, APIGroupID, APIFunID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    APINMZHTW = (string)null,
                    APINMZHCN = (string)null,
                    APINMENUS = (string)null,
                    APINMTHTH = (string)null,
                    APINMJAJP = (string)null,
                    APINMKOKR = (string)null,
                    APIPara = (string)null,
                    APIReturn = (string)null,
                    APIParaDesc = (string)null,
                    APIReturnContent = (string)null,
                    IsOutside = IsOutside ?? EnumYN.N.ToString(),
                    IsDisable = IsDisable ?? EnumYN.N.ToString(),
                    SortOrder = (string)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    APINMZHTW = responseObj.APINMZHTW;
                    APINMZHCN = responseObj.APINMZHCN;
                    APINMENUS = responseObj.APINMENUS;
                    APINMTHTH = responseObj.APINMTHTH;
                    APINMJAJP = responseObj.APINMJAJP;
                    APINMKOKR = responseObj.APINMKOKR;
                    APIPara = responseObj.APIPara;
                    APIReturn = responseObj.APIReturn;
                    APIParaDesc = responseObj.APIParaDesc;
                    APIReturnContent = responseObj.APIReturnContent;
                    IsOutside = responseObj.IsOutside;
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

        public async Task<bool> EditSystemAPIDetail(string userID)
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
                    APIFunID,
                    APINMZHTW,
                    APINMZHCN,
                    APINMENUS,
                    APINMTHTH,
                    APINMJAJP,
                    APINMKOKR,
                    APIPara,
                    APIReturn,
                    APIParaDesc,
                    APIReturnContent,
                    IsOutside = IsOutside ?? EnumYN.N.ToString(),
                    IsDisable = IsDisable ?? EnumYN.N.ToString(),
                    SortOrder,
                    UpdUserID = userID,
                    RoleIDs = HasRole?
                        .Select(r => string.IsNullOrWhiteSpace(r.Split('|')[1]) ? null : r.Split('|')[1])
                });

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemAPI.EditSystemAPI(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<EnumDeleteSystemAPIDetailResult> GetDeleteSystemAPIDetailResult(string userID)
        {
            var result = EnumDeleteSystemAPIDetailResult.Failure;
            try
            {
                string apiUrl = API.SystemAPI.DeleteSystemAPI(SysID, userID, APIGroupID, APIFunID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                result = EnumDeleteSystemAPIDetailResult.Success;
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

                    if (msg.Message == EnumDeleteSystemAPIDetailResult.DataExist.ToString())
                    {
                        result = EnumDeleteSystemAPIDetailResult.DataExist;
                    }
                }
            }
            return result;
        }

        #region - Event -
        public string GetEventParaSysSystemAPIEdit()
        {
            var eventParaSystemAPIEdit = new
            {
                TargetSysIDList = new List<string> { SysID },
                SysID,
                APIControllerID = APIGroupID,
                APIActionName = APIFunID,
                APINMzhTW = APINMZHTW,
                APINMzhCN = APINMZHCN,
                APINMenUS = APINMENUS,
                APINMthTH = APINMTHTH,
                APINMjaJP = APINMJAJP,
                APINMkoKR = APINMKOKR,
                APIPara,
                APIReturn,
                APIParaDesc,
                APIReturnContent,
                IsOutside = string.IsNullOrWhiteSpace(IsOutside) ? EnumYN.N.ToString() : EnumYN.Y.ToString(),
                IsDisable = string.IsNullOrWhiteSpace(IsDisable) ? EnumYN.N.ToString() : EnumYN.Y.ToString(),
                SortOrder
            };

            return Common.GetJsonSerializeObject(eventParaSystemAPIEdit);
        }

        public string GetEventParaSysSystemAPIDelete()
        {
            var eventParaSystemAPIDelete = new
            {
                TargetSysIDList = new List<string> { SysID },
                SysID,
                APIControllerID = APIGroupID,
                APIActionName = APIFunID
            };

            return Common.GetJsonSerializeObject(eventParaSystemAPIDelete);
        }
        #endregion
    }
}