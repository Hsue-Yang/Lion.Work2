using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemAPIClientDetailModel : SysModel
    {
        #region - Property -
        public string APINo { get; set; }
        public string SysID { get; set; }
        public string SysNM { get; set; }
        public string APIGroupID { get; set; }
        public string APIGroupNM { get; set; }
        public string APIFunID { get; set; }
        public string APIFunNM { get; set; }

        public string ClientSysID { get; set; }
        public string ClientSysNM { get; set; }
        public string ClientUserNM { get; set; }
        public DateTime ClientDTBegin { get; set; }
        public DateTime? ClientDTEnd { get; set; }

        public string IPAddress { get; set; }
        public string REQHeaders { get; set; }
        public string REQUrl { get; set; }
        public string REQReturn { get; set; }
        #endregion

        public async Task<bool> GetSystemAPIClientDetail(string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemAPI.QuerySystemAPIClient(userID, APINo, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    APINo = (string)null,
                    SysID = (string)null,
                    SysNM = (string)null,
                    APIGroupID = (string)null,
                    APIGroupNM = (string)null,
                    APIFunID = (string)null,
                    APIFunNM = (string)null,
                    ClientSysID = (string)null,
                    ClientSysNM = (string)null,
                    ClientUserNM = (string)null,
                    ClientDTBegin = new DateTime(),
                    ClientDTEnd = new DateTime?(),
                    IPAddress = (string)null,
                    REQUrl = (string)null,
                    REQHeaders = (string)null,
                    REQReturn = (string)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    APINo = responseObj.APINo;
                    SysID = responseObj.SysID;
                    SysNM = $"{responseObj.SysNM} ({responseObj.SysID})";
                    APIGroupID = responseObj.APIGroupID;
                    APIGroupNM = $"{responseObj.APIGroupNM} ({responseObj.APIGroupID})";
                    APIFunID = responseObj.APIFunID;
                    APIFunNM = $"{responseObj.APIFunNM} ({responseObj.APIFunID})";
                    ClientUserNM = responseObj.ClientUserNM;
                    ClientDTBegin = responseObj.ClientDTBegin;
                    ClientDTEnd = (responseObj.ClientDTEnd.HasValue == false) ? null : responseObj.ClientDTEnd;
                    IPAddress = responseObj.IPAddress;
                    REQUrl = responseObj.REQUrl;
                    REQHeaders = responseObj.REQHeaders;
                    REQReturn = responseObj.REQReturn;

                    if (responseObj.ClientSysID != null)
                    {
                        ClientSysID = responseObj.ClientSysID;
                        ClientSysNM = $"{responseObj.ClientSysNM} ({responseObj.ClientSysID})";
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