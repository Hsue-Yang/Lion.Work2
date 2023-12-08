using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;

namespace ERPAP.Models.Sys
{
    public class SystemEDIJobLogModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryEDIFlowID, QueryEDIJobID, DataDate, EDIDate, EDINO,
            EDIFlowIDSearch, EDIJobIDSearch
        }

        public enum EnumJobResultID
        {
            S, F
        }

        [Required]
        public string QuerySysID { get; set; }

        [Required]
        public string QueryEDIFlowID { get; set; }

        public string QueryEDIJobID { get; set; }

        [Required]
        public string DataDate { get; set; }

        [InputType(EnumInputType.TextBoxDatePicker)]
        public string EDIDate { get; set; }

        [StringLength(12, MinimumLength = 0)]
        public string EDINO { get; set; }

        public string EDIFlowIDSearch { get; set; }

        public string EDIJobIDSearch { get; set; }

        public SystemEDIJobLogModel()
        {

        }

        public class SystemEDIJobLog
        {
            public string EDINO { get; set; }
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string EDIFlowID { get; set; }
            public string EDIFlowNM { get; set; }
            public string EDIJobID { get; set; }
            public string EDIJobNM { get; set; }
            public string StatusID { get; set; }
            public string EDIJobStatusNM { get; set; }
            public string ResultID { get; set; }
            public string EDIJobResultNM { get; set; }
            public string ResultCode { get; set; }
            public DateTime DTBegin { get; set; }
            public DateTime DTEnd { get; set; }
            public int? CountRow { get; set; }
            public string UpdUserID { get; set; }
            public DateTime UpdDt { get; set; }
        }


        public void FormReset()
        {
            this.EDIDate = Common.GetDateString();
        }

        public List<SystemEDIJobLog> SystemEDIJobLogList { get; private set; }

        public async Task<bool> GetSystemEDIJobLogList(string userID, int pageSize, EnumCultureID cultureID)
        {
            try
            {
                string ediJobID = (string.IsNullOrWhiteSpace(QueryEDIJobID) ? null : QueryEDIJobID);
                string ediNO = (string.IsNullOrWhiteSpace(EDINO) ? null : EDINO);
                string ediDate = (string.IsNullOrWhiteSpace(EDIDate) ? null : EDIDate);
                string ediFlowIDSearch = (string.IsNullOrWhiteSpace(EDIFlowIDSearch) ? null : EDIFlowIDSearch);
                string ediJobIDSearch = (string.IsNullOrWhiteSpace(EDIJobIDSearch) ? null : EDIJobIDSearch);

                string apiUrl = API.SystemEDIJobLog.QuerySystemEDIJobLogs(userID, QuerySysID, ediNO, QueryEDIFlowID, ediJobID, ediFlowIDSearch, ediJobIDSearch, ediDate, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemEDIJobLogList = (List<SystemEDIJobLog>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemEDIJobLogList = responseObj.SystemEDIJobLogList;

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