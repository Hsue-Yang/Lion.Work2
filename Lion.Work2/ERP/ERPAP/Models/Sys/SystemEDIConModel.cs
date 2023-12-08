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
    public class SystemEDIConModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryEDIFlowID
        }

        public class SystemEDICon
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string EDIFlowID { get; set; }
            public string EDIFlowNM { get; set; }
            public string EDIConID { get; set; }
            public string EDIConNM { get; set; }
            public string ProviderName { get; set; }
            public string ConValue { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserID { get; set; }
            public DateTime UpdDt { get; set; }
        }

        public class EDIConValue
        {
            public string SysID { get; set; }
            public string EDIFlowID { get; set; }
            public string EDIConID { get; set; }
            public string BeforeSortOrder { get; set; }
            public string AfterSortOrder { get; set; }
        }

        [Required]
        public string QuerySysID { get; set; }

        public string QueryEDIFlowID { get; set; }

        public SystemEDIConModel()
        {

        }

        public void FormReset()
        {
            this.QuerySysID = string.Empty;
            this.QueryEDIFlowID = string.Empty;
        }

        List<SystemEDICon> _entitySystemEDIConList;
        public List<SystemEDICon> EntitySystemEDIConList { get { return _entitySystemEDIConList; } }

        public async Task<bool> GetSystemEDIConList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string ediflowID = string.IsNullOrWhiteSpace(this.QueryEDIFlowID) ? null : this.QueryEDIFlowID;
                string apiUrl = API.SystemEDICon.QuerySystemEDICons(userID, QuerySysID, cultureID.ToString().ToUpper(), ediflowID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    systemEDIConList = (List<SystemEDICon>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    _entitySystemEDIConList = responseObj.systemEDIConList;
                }

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetEDIConSettingResult(string userID, EnumCultureID cultureID, List<EDIConValue> ediconValue)
        {
            try
            {
                List<SystemEDICon> ediConValueList = new List<SystemEDICon>();
                foreach (var ediCon in ediconValue)
                {
                    if (ediCon.AfterSortOrder != ediCon.BeforeSortOrder)
                    {
                        ediConValueList.Add(new SystemEDICon
                        {
                            SysID = QuerySysID,
                            SortOrder = string.IsNullOrWhiteSpace(ediCon.AfterSortOrder) ? null : ediCon.AfterSortOrder,
                            UpdUserID = userID,
                            EDIFlowID = string.IsNullOrWhiteSpace(ediCon.EDIFlowID) ? null : ediCon.EDIFlowID,
                            EDIConID = string.IsNullOrWhiteSpace(ediCon.EDIConID) ? null : ediCon.EDIConID
                        });
                    }
                }

                if (ediConValueList.Any())
                {
                    Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                    var paraJsonStr = Common.GetJsonSerializeObject(ediConValueList);
                    var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                    string apiUrl = API.SystemEDICon.EditSystemEDIConSortOrder(QuerySysID);
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