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
    public class SystemCultureSettingDetailModel : SysModel
    {
        #region - Property -
        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string CultureID { get; set; }
        [Required]
        [StringLength(150)]
        public string CultureNM { get; set; }
        [StringLength(150)]
        public string DisplayNM { get; set; }
        public string IsSerpUse { get; set; }
        public string IsDisable { get; set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            CultureNM = string.Empty;
            DisplayNM = string.Empty;
            IsSerpUse = string.Empty;
            IsDisable = string.Empty;
        }
        #endregion

        /// <summary>
        /// 取得語系代碼明細列表
        /// </summary>
        /// <param name="userID">使用者代碼</param>
        /// <returns></returns>
        public async Task<bool> GetSystemCultureDetail(string userID)
        {
            try
            {
                string apiUrl = API.SystemCultureSetting.QuerySystemCultureDetail(userID, CultureID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    CultureID = (string)null,
                    CultureNM = (string)null,
                    DisplayNM = (string)null,
                    IsSerpUse = IsSerpUse ?? EnumYN.N.ToString(),
                    IsDisable = IsDisable ?? EnumYN.N.ToString()
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    CultureID = responseObj.CultureID;
                    CultureNM = responseObj.CultureNM;
                    DisplayNM = responseObj.DisplayNM;
                    IsSerpUse = responseObj.IsSerpUse;
                    IsDisable = responseObj.IsDisable;

                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        /// <summary>
        /// 編輯語系代碼明細
        /// </summary>
        /// <param name="userID">使用者代碼</param>
        /// <returns></returns>
        public async Task<bool> EditSystemCultureDetail(string userID)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                {
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                };

                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    CultureID,
                    CultureNM,
                    DisplayNM,
                    IsSerpUse = IsSerpUse ?? EnumYN.N.ToString(),
                    IsDisable = IsDisable ?? EnumYN.N.ToString(),
                    UpdUserID = userID
                });

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemCultureSetting.EditSystemCultureDetail(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        /// <summary>
        /// 刪除語系代碼明細
        /// </summary>
        /// <param name="userID">使用者代碼</param>
        /// <returns></returns>
        public async Task<bool> DeleteSystemCultureDetail(string userID)
        {
            try
            {
                string apiUrl = API.SystemCultureSetting.DeleteSystemCultureDetail(userID, CultureID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
    }
}