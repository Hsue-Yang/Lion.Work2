using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;

namespace ERPAP.Models.Sys
{
    public class LineBotAccountSettingModel : SysModel
    {
        #region - Property -
        public string SysID { set; get; }
        public string LineID { set; get; }
        public List<LineBotAccountSettingDetail> LineBotAccountSettingsList { get; private set; }
        #endregion

        #region - 取得LineBot好友清單列表 -    
        public async Task<bool> GetLineBotAccountSettingsList(int pageSize, string userID, string lineID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SysID)) return true;

                string apiUrl = API.SystemLineBot.QuerySystemLineBotAccountList(userID, SysID, lineID, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemLineAccountList = (List<LineBotAccountSettingDetail>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    LineBotAccountSettingsList = responseObj.SystemLineAccountList;

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

        #endregion
    }
}