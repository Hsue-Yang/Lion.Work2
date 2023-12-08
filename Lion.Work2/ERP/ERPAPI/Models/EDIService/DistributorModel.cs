using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ERPAPI.Models.EDIService
{
    public class DistributorModel : EDIServiceModel
    {
        #region - API Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }

        public string SysID { get; set; }
        public string EventGroupID { get; set; }
        public string EventID { get; set; }
        public string UserID { get; set; }

        //[AllowHtml]
        public string EventPara { get; set; }
        #endregion

        public string ExcuteSubscription()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(EventPara) == false)
                {
                    Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                    {
                        {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                    };

                    var paraJsonStr = Common.GetJsonSerializeObject(new
                    {
                        SysID,
                        EventGroupID,
                        EventID,
                        ExecEDIEventNo = (string)null,
                        UpdUserID = "APIService.ERP"
                    });

                    var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                    string apiUrl = API.SystemEvent.ExcuteSubscription(null);
                    var response = Common.HttpWebRequestGetResponseString(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                    var responseObj = new
                    {
                        EDIEventNo = (string)null
                    };

                    responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                    if (responseObj != null)
                    {
                        return responseObj.EDIEventNo;
                    }
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return null;
        }
    }
}