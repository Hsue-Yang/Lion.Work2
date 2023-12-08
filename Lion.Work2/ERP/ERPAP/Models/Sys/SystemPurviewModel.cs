using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemPurviewModel : SysModel
    {
        #region - Enum -
        public enum Field
        {
            QuerySysID
        }
        #endregion

        #region - Class -
        public class SystemPurview
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }

            public string PurviewID { get; set; }
            public string PurviewNM { get; set; }
            public string SortOrder { get; set; }
            public string Remark { get; set; }

            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }
        #endregion

        #region - Property -
        [Required]
        public string QuerySysID { get; set; }

        public List<SystemPurview> SystemPurviewList { get; set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            QuerySysID = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemPurviewList(string userID, int pageSize, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(QuerySysID)) return true;

                string apiUrl = API.SystemPurview.QuerySystemPurviews(QuerySysID, userID, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemPurviews = (List<SystemPurview>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemPurviewList = responseObj.SystemPurviews;

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