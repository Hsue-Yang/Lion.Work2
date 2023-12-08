using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemEventModel : SysModel
    {
        #region - Enum -
        public enum Field
        {
            QuerySysID, QueryEventGroupID
        }
        #endregion

        #region - Class -
        public class SystemEvent
        {
            public string EventGroupID { get; set; }
            public string EventGroupNM { get; set; }
            public string EventID { get; set; }
            public string EventNM { get; set; }
            public string IsDisable { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }
        #endregion

        #region - Property -
        [Required]
        public string QuerySysID { get; set; }

        public string QueryEventGroupID { get; set; }

        public List<SystemEvent> SystemEventList { get; private set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            QuerySysID = string.Empty;
            QueryEventGroupID = string.Empty;
        }
        #endregion

        #region - 取得事件訂閱列表 -
        public async Task<bool> GetSystemEventList(string userID, int pageSize, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(QuerySysID)) return true;

                string apiUrl = API.SystemEvent.QuerySystemEvents(QuerySysID, userID, QueryEventGroupID, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemEvents = (List<SystemEvent>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemEventList = responseObj.SystemEvents;

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