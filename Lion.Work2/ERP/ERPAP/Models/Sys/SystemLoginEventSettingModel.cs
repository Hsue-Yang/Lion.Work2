// 新增日期：2017-02-17
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
//using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;

namespace ERPAP.Models.Sys
{
    public class SystemLoginEventSettingModel : SysModel
    {
        #region - Constructor -
        public SystemLoginEventSettingModel() { }
        #endregion

        #region - Property -
        public string SysID { get; set; }
        public string SysNMID { get; set; }
        public string LoginEventID { get; set; }
        public List<LoginEventSetting> LoginEventSettingList { get; set; }
        public List<SysLoginEventSetting> SysLoginEventSettingList { get; private set; }
        #endregion


        #region - Class -
        public class SysLoginEventSetting
        {
            public string SysID;
            public string SysNMID;
            public string SubSysNMID;
            public string LoginEventID;
            public string LoginEventNMID;
            public string SubSysID;
            public DateTime StartDT;
            public DateTime EndDT;
            public string IsDisable;
            public string SortOrder;
            public string UpdUserNM;
            public DateTime UpdDT;
        }

        public class LoginEventSetting
        {
            public string SysID { get; set; }
            public string LoginEventID { get; set; }
            public string BeforeSortOrder { get; set; }
            public string AfterSortOrder { get; set; }
        }

        public class LoginEventSettingValue
        {
            public string SYS_ID { get; set; }
            public string SORT_ORDER { get; set; }
            public string UPD_USER_ID { get; set; }
            public string LOGIN_EVENT_ID { get; set; }
        }
        #endregion

        #region - 更新應用系統登入事件列表排序 -
        /// <summary>
        /// 更新應用系統登入事件列表排序
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSysLoginEventSettingSortResult(string userID)
        {
            try
            {
                string UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID;
                List<LoginEventSetting> loginEventSettingList = (this.LoginEventSettingList != null && this.LoginEventSettingList.Any()) ? this.LoginEventSettingList : new List<LoginEventSetting>();
                List<LoginEventSettingValue> loginEventSettingValueList = new List<LoginEventSettingValue>();
                foreach (var loginEventValue in loginEventSettingList)
                {
                    if (loginEventValue.AfterSortOrder != loginEventValue.BeforeSortOrder)
                    {
                        loginEventSettingValueList.Add(new LoginEventSettingValue
                        {
                            SYS_ID = string.IsNullOrWhiteSpace(loginEventValue.SysID) ? null : loginEventValue.SysID,
                            SORT_ORDER = string.IsNullOrWhiteSpace(loginEventValue.AfterSortOrder) ? null : loginEventValue.AfterSortOrder,
                            UPD_USER_ID = UpdUserID,
                            LOGIN_EVENT_ID = string.IsNullOrWhiteSpace(loginEventValue.LoginEventID) ? null : loginEventValue.LoginEventID
                        });
                    }
                }

                if (loginEventSettingValueList.Any())
                {
                    Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                    var paraJsonStr = Common.GetJsonSerializeObject(loginEventSettingValueList);
                    var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                    string apiUrl = API.SysLoginEvent.EditSysLoginEventSettingSort(UpdUserID);
                    await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
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

        #region - 取得應用系統登入事件設定清單 -
        /// <summary>
        /// 取得應用系統登入事件設定清單
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="cultureId"></param>
        /// <returns></returns>
        public async Task<bool> GetSysLoginEventSettingList(string userID, int pageSize, EnumCultureID cultureId)
        {
            try
            {
                string apiUrl = API.SysLoginEvent.QuerySysLoginEventSettings(SysID, userID, cultureId.ToString().ToUpper(), LoginEventID, PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    rowCount = 0,
                    sysLoginEventSettingList = (List<SysLoginEventSetting>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.rowCount;
                    SysLoginEventSettingList = responseObj.sysLoginEventSettingList;
                    SetPageCount();
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
    }
}