using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemEventTargetEDIModel : SysModel
    {
        #region - Enum -
        public enum EnumResultID
        {
            S, F
        }

        private enum EnumEDIEventParaKey
        {
            TargetSysIDList
        }
        #endregion

        #region - Class -
        public class SystemEventTargetEDI
        {
            public string EDIEventNo { get; set; }

            public string StatusID { get; set; }
            public string StatusNM { get; set; }
            public string ResultID { get; set; }
            public string ResultNM { get; set; }

            public string InsertEDINo { get; set; }
            public string InsertEDIDate { get; set; }
            public string InsertEDITime { get; set; }
            public string ExecEDIEventNo { get; set; }

            public bool HasAuth { get; set; }
            public string TargetSysID { get; set; }
            public string TargetSysNM { get; set; }

            public string TargetStatusID { get; set; }
            public string TargetStatusNM { get; set; }
            public string TargetResultID { get; set; }
            public string TargetResultNM { get; set; }
            public string TargetReturnAPINo { get; set; }

            public DateTime? TargetDTBegin { get; set; }
            public DateTime? TargetDTEnd { get; set; }
        }
        #endregion

        #region - Property - 
        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysID { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string EventGroupID { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string EventID { get; set; }

        public string QueryTargetSysID { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string DTBegin { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string DTEnd { get; set; }

        public string IsOnlyFail { get; set; }

        public string ExecEDIEventNo { get; set; }

        public string TargetSysID { get; set; }

        public List<SystemEventTargetEDI> SystemEventTargetEDIList { get; private set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            DTBegin = Common.GetDateString();
            DTEnd = Common.GetDateString();
            QueryTargetSysID = string.Empty;
            IsOnlyFail = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemEventTargetEDIList(string userID, int pageSize, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemEvent.QuerySystemEventTargetEDIs(SysID, userID, QueryTargetSysID, EventGroupID, EventID, DTBegin, DTEnd, IsOnlyFail, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemEventTargetEDIs = (List<SystemEventTargetEDI>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemEventTargetEDIList = responseObj.SystemEventTargetEDIs;

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

        public string GetEDIEventPara(string path)
        {
            try
            {
                string eventPara = Common.FileReadStream(path);

                if (string.IsNullOrWhiteSpace(eventPara) == false)
                {
                    Dictionary<string, object> para = Common.GetJsonDeserializeObject<Dictionary<string, object>>(eventPara);
                    para[EnumEDIEventParaKey.TargetSysIDList.ToString()] = new List<string> { TargetSysID };
                    return Common.GetJsonSerializeObject(para);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return null;
        }

        public bool ExcuteSubscription(string userID, ref string ediEventNo)
        {
            int execTimes = 0;

            while (execTimes < 10)
            {
                execTimes++;
                try
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
                        ExecEDIEventNo = string.IsNullOrWhiteSpace(ExecEDIEventNo) ? null : ExecEDIEventNo,
                        UpdUserID = userID
                    });

                    var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                    string apiUrl = API.SystemEvent.ExcuteSubscription(userID);
                    var response = Common.HttpWebRequestGetResponseString(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                    var responseObj = new
                    {
                        EDIEventNo = (string)null
                    };

                    responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                    if (responseObj != null)
                    {
                        ediEventNo = responseObj.EDIEventNo;
                        return true;
                    }

                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    OnException(ex);
                    continue;
                }
            }
            return false;
        }
    }
}