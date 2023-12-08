using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemEventGroupModel : SysModel
    {
        #region - Enum -
        public enum Field
        {
            QuerySysID, QueryEventGroupID
        }
        #endregion

        #region - Class -
        public class SystemEventGroup
        {
            public string EventGroupID { get; set; }
            public string EventGroupNM { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }
        #endregion

        #region - Property -
        [Required]
        public string QuerySysID { get; set; }

        public string QueryEventGroupID { get; set; }

        public List<SystemEventGroup> SystemEventGroupList { get; set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            QuerySysID = string.Empty;
            QueryEventGroupID = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemEventGroupList(string userID, int pageSize, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(QuerySysID)) return true;

                string apiUrl = API.SystemEventGroup.QuerySystemEventGroups(QuerySysID, userID, QueryEventGroupID, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemEventGroups = (List<SystemEventGroup>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemEventGroupList = responseObj.SystemEventGroups;

                    SetPageCount();
                }
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