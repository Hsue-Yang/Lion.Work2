using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemAPIParaModel : SysModel
    {
        #region - Property -
        public string SysID { get; set; }

        public string APIGroupID { get; set; }

        public string APIFunID { get; set; }

        public string APIPara { get; set; }

        public string APIReturn { get; set; }
        #endregion

        public async Task<bool> GetSystemAPIPara(string userID)
        {
            try
            {
                string apiUrl = API.SystemAPI.QuerySystemAPI(SysID, userID, APIGroupID, APIFunID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    APIPara = (string)null,
                    APIReturn = (string)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    APIPara = responseObj.APIPara;
                    APIReturn = responseObj.APIReturn;

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