using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemCultureSettingModel : SysModel
    {
        #region - Class -
        public class SystemCulture
        {
            public string CultureID { get; set; }
            public string CultureNM { get; set; }
            public string DisplayNM { get; set; }
            public string IsSerpUse { get; set; }
            public string IsDisable { get; set; }
            public string UpdUserID { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }
        #endregion

        #region - Property -
        public string QueryCultureID { get; set; }
        public List<SystemCulture> SystemCultures { get; private set; }
        #endregion

        #region - 取得語系代碼清單 -
        /// <summary>
        /// 取得語系代碼清單
        /// </summary>
        /// <param name="userID">使用者代碼</param>
        /// <param name="pageSize">筆數</param>
        /// <returns></returns>
        public async Task<bool> GetSystemCultures(string userID, int pageSize)
        {
            try
            {
                string apiUrl = API.SystemCultureSetting.QuerySystemCultures(userID, QueryCultureID, PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemCultures = (List<SystemCulture>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemCultures = responseObj.SystemCultures;
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

        public async Task<bool> GenerateCultureJsonFile(string userID)
        {
            try
            {
                string apiUrl = API.SystemCultureSetting.GenerateCultureJsonFile(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "POST");

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
    }
}