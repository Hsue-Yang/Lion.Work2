using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemAPILogModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryAPIGroupID, QueryAPIFunID,
            QueryClientSysID,
            APINo,
            BeginDate, EndDate, BeginTime, EndTime
        }

        #region - Class -
        public class SystemAPILog
        {
            public string APINo { get; set; }
            public string SysID { get; set; }
            public string APIGroupID { get; set; }
            public string APIFunID { get; set; }
            public string APIFunNM { get; set; }
            public string ClientSysID { get; set; }
            public string ClientSysNM { get; set; }
            public DateTime ClientDTBegin { get; set; }
            public DateTime? ClientDTEnd { get; set; }
            public string IPAddress { get; set; }
        }
        #endregion

        #region - Property -
        [Required]
        public string QuerySysID { get; set; }

        [Required]
        public string QueryAPIGroupID { get; set; }

        public string QueryAPIFunID { get; set; }

        public string QueryClientSysID { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string BeginDate { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string EndDate { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 4)]
        public string BeginTime { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 4)]
        public string EndTime { get; set; }

        [StringLength(16, MinimumLength = 0)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string APINo { get; set; }

        public List<SystemAPILog> SystemAPILogList { get; private set; }
        #endregion

        private const int TimeInterval = 1;

        #region - Reset -

        public void FormReset()
        {
            QuerySysID = string.Empty;
            QueryAPIGroupID = string.Empty;
            QueryAPIFunID = string.Empty;
            QueryClientSysID = string.Empty;
            APINo = string.Empty;
            BeginDate = Common.GetDateString();
            EndDate = Common.GetDateString();
            BeginTime = Common.GetDateTimeFormattedText(DateTime.Now.AddHours(-1 * TimeInterval), Common.EnumDateTimeFormatted.HoursForMinutes);
            EndTime = Common.GetDateTimeFormattedText(DateTime.Now, Common.EnumDateTimeFormatted.HoursForMinutes);
        }

        #endregion

        public async Task<bool> GetSystemAPILogList(string userID, int pageSize, EnumCultureID cultureID)
        {
            try
            {
                var dtBegin = $"{Common.FormatDateString(BeginDate)} {BeginTime}:00.000";
                var dtEnd = $"{Common.FormatDateString(EndDate)} {EndTime}:59.999";
                
                TimeSpan ts = Convert.ToDateTime(dtEnd).AddMilliseconds(-59999).Subtract(Convert.ToDateTime(dtBegin));

                if (ts.TotalHours >= 0 && ts.TotalHours <= TimeInterval)
                {
                    string apiUrl = API.SystemAPI.QuerySystemAPILogs(QuerySysID, userID, QueryAPIGroupID, QueryAPIFunID, QueryClientSysID, APINo, dtBegin, dtEnd, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                    string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                    var responseObj = new
                    {
                        RowCount = 0,
                        SystemAPILogs = (List<SystemAPILog>)null
                    };

                    responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                    if (responseObj != null)
                    {
                        RowCount = responseObj.RowCount;
                        SystemAPILogList = responseObj.SystemAPILogs;

                        SetPageCount();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
    }
}