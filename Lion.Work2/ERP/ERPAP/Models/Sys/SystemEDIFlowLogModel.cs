using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERPAP.Models.Sys
{
    public class SystemEDIFlowLogModel : SysModel
    {
        public class SystemEDIFlowLog
        {
            public string EDINO { get; set; }
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string EDIFlowID { get; set; }
            public string EDIFlowNM { get; set; }
            public string EDIDate { get; set; }
            public string EDITime { get; set; }
            public string DataDate { get; set; }
            public string StatusID { get; set; }
            public string ResultID { get; set; }
            public string ResultCode { get; set; }
            public string EDIFlowStatusNM { get; set; }
            public string EDIFlowResultNM { get; set; }
            public DateTime DTBegin { get; set; }
            public DateTime DTEnd { get; set; }
            public string IsAutomatic { get; set; }
            public DateTime AutoSchedule { get; set; }
            public string AutoEDINO { get; set; }
            public string AutoFlowID { get; set; }
            public string IsDeleted { get; set; }
            public string UpdUserID { get; set; }
            public DateTime UpdDt { get; set; }
        }

        public enum Field
        {
            QuerySysID,
            QueryEDIFlowID,
            DataDate,
            EDIDate,
            EDINO,
            OnlyQuery
        }

        public enum EnumResultID
        {
            F
        }

        public enum EnumStatusID
        {
            W
        }

        public enum EnumOnlyQuery
        {
            FailureResult,
            WaitStatus
        }

        [Required]
        public string QuerySysID { get; set; }

        public string QueryEDIFlowID { get; set; }

        [StringLength(14, MinimumLength = 14)]
        public string EDINO { get; set; }

        [InputType(EnumInputType.TextBoxDatePicker)]
        public string DataDate { get; set; }

        [InputType(EnumInputType.TextBoxDatePicker)]
        public string EDIDate { get; set; }

        public string OnlyQuery { get; set; }

        public Dictionary<string, string> OnlyQueryDictionary => new Dictionary<string, string>
        {
            { string.Empty, string.Empty },
            { EnumOnlyQuery.FailureResult.ToString(), SysSystemEDIFlowLog.Label_OnlyQueryFailureResult },
            { EnumOnlyQuery.WaitStatus.ToString(), SysSystemEDIFlowLog.Label_OnlyQueryWaitStatus }
        };

        public List<SystemEDIFlowLog> EntitySystemEDIFlowLogList { get; private set; }
        
        public void FormReset()
        {
            EDIDate = Common.GetDateString();
        }

        public async Task<bool> GetSystemEDIFlowLogList(string userID, int pageSize, EnumCultureID cultureID)
        {
            try
            {
                string ediNo = (string.IsNullOrWhiteSpace(EDINO) ? null : EDINO);
                string ediDate = (string.IsNullOrWhiteSpace(EDIDate) ? null : EDIDate);
                string dataDate = (string.IsNullOrWhiteSpace(DataDate) ? null : DataDate);
                string resultID = string.Empty;
                string statusID = string.Empty;

                if (OnlyQuery == EnumOnlyQuery.FailureResult.ToString())
                {
                    resultID = EnumResultID.F.ToString();
                }
                else if (OnlyQuery == EnumOnlyQuery.WaitStatus.ToString())
                {
                    statusID = EnumStatusID.W.ToString();
                }

                string apiUrl = API.SystemEDIFlowLog.QuerySystemEDIFlowsLogs(userID, QuerySysID, ediNo, QueryEDIFlowID, ediDate, dataDate, resultID, statusID, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    systemEDIFlowLogList = (List<SystemEDIFlowLog>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    EntitySystemEDIFlowLogList = responseObj.systemEDIFlowLogList;

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

        public async Task<bool> UpdateWaitStatusLog(string userID)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                {
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                };

                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    SysID = QuerySysID,
                    EDIFlowID = (string.IsNullOrWhiteSpace(QueryEDIFlowID) ? null : QueryEDIFlowID),
                    DataDate = (string.IsNullOrWhiteSpace(DataDate) ? null : DataDate),
                    UpdUserID = userID
                });

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemEDIFlowLog.EditSystemEDIFlowWaitStatusLog(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

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