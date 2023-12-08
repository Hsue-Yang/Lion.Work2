using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemEventParaModel : SysModel
    {
        #region - Property -
        public string SysID { get; set; }

        public string EventGroupID { get; set; }

        public string EventID { get; set; }

        public string EventPara { get; set; }

        public string Remark { get; set; }
        #endregion

        public async Task<bool> GetSystemEventByPara(string userID)
        {
            try
            {
                string apiUrl = API.SystemEvent.QuerySystemEvent(SysID, userID, EventGroupID, EventID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    EventPara = (string)null,
                    Remark = (string)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    EventPara = responseObj.EventPara;
                    Remark = responseObj.Remark;

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