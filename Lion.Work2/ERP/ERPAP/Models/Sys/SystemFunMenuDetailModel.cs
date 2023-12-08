using LionTech.Entity;
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
    public class SystemFunMenuDetailModel : SysModel
    {
        #region - Enum -
        public enum EnumDeleteSystemFunMenuDetailResult
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
        public string FunMenu { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunMenuNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunMenuNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunMenuNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunMenuNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunMenuNMJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string FunMenuNMKOKR { get; set; }

        [Required]
        [StringLength(2)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string DefaultMenuID { get; set; }

        [Required]
        public string IsDisable { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        private Dictionary<string, string> _defaultMenuIDDictionary;

        public Dictionary<string, string> DefaultMenuIDDictionary => _defaultMenuIDDictionary ?? (_defaultMenuIDDictionary = Enumerable.Range(1, 3).ToDictionary(k => k.ToString(), v => v.ToString()));
        #endregion

        #region - Reset -
        public void FormReset()
        {
            FunMenuNMZHTW = string.Empty;
            FunMenuNMZHCN = string.Empty;
            FunMenuNMENUS = string.Empty;
            FunMenuNMTHTH = string.Empty;
            FunMenuNMJAJP = string.Empty;
            FunMenuNMKOKR = string.Empty;
            DefaultMenuID = "1";
            IsDisable = EnumYN.N.ToString();
            SortOrder = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemFunMenuDetail(string userID)
        {
            try
            {
                string apiUrl = API.SystemFunMenu.QuerySystemFunMenu(SysID, userID, FunMenu);
                var response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var systemFunMenuDetail = new
                {
                    SysID = (string)null,
                    FunMenu = (string)null,
                    FunMenuNMZHTW = (string)null,
                    FunMenuNMZHCN = (string)null,
                    FunMenuNMENUS = (string)null,
                    FunMenuNMTHTH = (string)null,
                    FunMenuNMJAJP = (string)null,
                    FunMenuNMKOKR = (string)null,
                    DefaultMenuID = (string)null,
                    IsDisable = EnumYN.N.ToString(),
                    SortOrder = (string)null
                };

                systemFunMenuDetail = Common.GetJsonDeserializeAnonymousType(response, systemFunMenuDetail);

                if (systemFunMenuDetail != null)
                {
                    SysID = systemFunMenuDetail.SysID;
                    FunMenu = systemFunMenuDetail.FunMenu;
                    FunMenuNMZHTW = systemFunMenuDetail.FunMenuNMZHTW;
                    FunMenuNMZHCN = systemFunMenuDetail.FunMenuNMZHCN;
                    FunMenuNMENUS = systemFunMenuDetail.FunMenuNMENUS;
                    FunMenuNMTHTH = systemFunMenuDetail.FunMenuNMTHTH;
                    FunMenuNMJAJP = systemFunMenuDetail.FunMenuNMJAJP;
                    FunMenuNMKOKR = systemFunMenuDetail.FunMenuNMKOKR;
                    DefaultMenuID = systemFunMenuDetail.DefaultMenuID;
                    IsDisable = systemFunMenuDetail.IsDisable;
                    SortOrder = systemFunMenuDetail.SortOrder;

                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> EditSystemFunMenuDetail(string userID)
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
                    FunMenu,
                    FunMenuNMZHTW,
                    FunMenuNMZHCN,
                    FunMenuNMENUS,
                    FunMenuNMTHTH,
                    FunMenuNMJAJP,
                    FunMenuNMKOKR,
                    DefaultMenuID,
                    IsDisable = IsDisable ?? EnumYN.N.ToString(),
                    SortOrder,
                    UpdUserID = userID
                });
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemFunMenu.EditSystemFunMenu(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<EnumDeleteSystemFunMenuDetailResult> GetDeleteSystemFunMenuDetailResult(string userID)
        {
            var result = EnumDeleteSystemFunMenuDetailResult.Failure;
            try
            {
                string apiUrl = API.SystemFunMenu.DeleteSystemFunMenu(SysID, userID, FunMenu);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                result = EnumDeleteSystemFunMenuDetailResult.Success;
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

                    if (msg.Message == EnumDeleteSystemFunMenuDetailResult.DataExist.ToString())
                    {
                        result = EnumDeleteSystemFunMenuDetailResult.DataExist;
                    }
                }
            }
            return result;
        }

        #region - Event -
        public string GetEventParaSysSystemFunMenuEdit()
        {
            var entityEventParaSystemFunMenuEdit = new 
            {
                TargetSysIDList = new List<string>() {SysID},
                SysID,
                FunMenu,
                FunMenuNMZHTW,
                FunMenuNMZHCN,
                FunMenuNMENUS,
                FunMenuNMTHTH,
                FunMenuNMJAJP,
                FunMenuNMKOKR,
                DefaultMenuID,
                IsDisable = string.IsNullOrWhiteSpace(IsDisable) ? EnumYN.N.ToString() : EnumYN.Y.ToString(),
                SortOrder,
            };

            return Common.GetJsonSerializeObject(entityEventParaSystemFunMenuEdit);
        }

        public string GetEventParaSysSystemFunMenuDelete()
        {
            var entityEventParaSystemMenuDelete = new 
            {
                TargetSysIDList = new List<string>() {SysID},
                SysID,
                FunMenu
            };

            return Common.GetJsonSerializeObject(entityEventParaSystemMenuDelete);
        }
        #endregion
    }
}