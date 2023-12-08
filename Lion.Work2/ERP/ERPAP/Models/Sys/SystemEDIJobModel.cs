using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;

namespace ERPAP.Models.Sys
{
    public class SystemEDIJobModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryEDIFlowID,
            QueryEDIJobType
        }

        [Required]
        public string QuerySysID { get; set; }

        [Required]
        public string QueryEDIFlowID { get; set; }

        public string QueryEDIJobType { get; set; }
        
        public SystemEDIJobModel()
        {

        }

        public class SystemEDIJob
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string EDIFlowID { get; set; }
            public string EDIFlowNM { get; set; }
            public string EDIJobID { get; set; }
            public string EDIJobNM { get; set; }
            public string EDIJobType { get; set; }
            public string ObjectName { get; set; }
            public string DepEDIJobID { get; set; }
            public string IsUseRes { get; set; }
            public string IgnoreWarning { get; set; }
            public string IsDisable { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserID { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDt { get; set; }
        }

        public class SystemEDIJobValue
        {
            public string SysID { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserID { get; set; }
            public string EDIJobID { get; set; }
            public string EDIFlowID { get; set; }
        }

        public class EDIJobValue
        {
            public string SysID { get; set; }
            public string EDIJobID { get; set; }
            public string BeforeSortOrder { get; set; }
            public string AfterSortOrder { get; set; }
        }

        public void FormReset()
        {
            this.QuerySysID = string.Empty;
            this.QueryEDIFlowID = string.Empty;
            this.QueryEDIJobType = string.Empty;
        }

        List<SystemEDIJob> _systemEDIJobList;
        public List<SystemEDIJob> SystemEDIJobList { get { return _systemEDIJobList; } }

        public async Task<bool> GetSystemEDIJobList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string ediJobType = (string.IsNullOrWhiteSpace(QueryEDIJobType) ? null : QueryEDIJobType);

                string apiUrl = API.SystemEDIJob.QuerySystemEDIJobs(userID, QuerySysID, QueryEDIFlowID, ediJobType, cultureID.ToString().ToUpper());

                if (string.IsNullOrWhiteSpace(QueryEDIFlowID))
                {
                    _systemEDIJobList = new List<SystemEDIJob>();
                }
                else
                {
                    string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                    _systemEDIJobList = Common.GetJsonDeserializeObject<List<SystemEDIJob>>(response);
                }

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetEDIJobSettingResult(string userID, List<EDIJobValue> ediJobSortValuetList)
        {
            try
            {
                List<SystemEDIJobValue> ediJobValueList = new List<SystemEDIJobValue>();
                foreach (var ediJobValue in ediJobSortValuetList)
                {
                    //判斷SORT_ORDER有變才更新
                    if (ediJobValue.AfterSortOrder != ediJobValue.BeforeSortOrder)
                    {
                        ediJobValueList.Add(new SystemEDIJobValue
                        {
                            SysID = QuerySysID,
                            SortOrder = ediJobValue.AfterSortOrder,
                            UpdUserID = userID,
                            EDIJobID = ediJobValue.EDIJobID,
                            EDIFlowID = QueryEDIFlowID
                        });
                    }
                }
                if (ediJobValueList.Any())
                {
                    Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                    var paraJsonStr = Common.GetJsonSerializeObject(ediJobValueList);
                    var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                    string apiUrl = API.SystemEDIJob.EditSystemEDIJobSortOrder(userID);
                    await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
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