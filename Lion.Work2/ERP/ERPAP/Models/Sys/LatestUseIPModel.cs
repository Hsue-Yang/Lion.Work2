// 新增日期：2018-01-23
// 新增人員：廖先駿
// 新增內容：IP最後使用
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using Resources;

namespace ERPAP.Models.Sys
{
    public class LatestUseIPModel : SysModel
    {
        #region - Class -
        public class LatestUseIPInfoAPI
        {
            public string IPAddress { get; set; }
            public string UserIDNM { get; set; }
            public string CompNM { get; set; }
            public string UnitNM { get; set; }
            public string JobNM { get; set; }
            public string CompTel { get; set; }
            public string CompTelExt { get; set; }
        }
        #endregion

        #region - Definitions -
        public new enum EnumCookieKey
        {
            IPAddresss
        }
        #endregion

        #region - Property -
        [Required]
        [StringLength(15)]
        [RegularExpression(@"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])(\.25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)?(\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0))?(\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9]))*$",
            ErrorMessageResourceType = typeof(SysLatestUseIP),
            ErrorMessageResourceName = nameof(SysLatestUseIP.SystemMsg_IPAddressFormate_Error))]
        public string IPAddresss { set; get; }
        public List<LatestUseIPInfoAPI> LatestUseIPInfoLists { get; private set; }
        #endregion


        public async Task<bool> GetLatestUseIPInfoList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string response = string.Empty;
                if (!string.IsNullOrWhiteSpace(IPAddresss))
                {
                    string apiUrl = API.LatestUseIP.QueryLatestUseIPInfoList(userID, IPAddresss, cultureID.ToString().ToUpper(), PageIndex, PageSize);
                    response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                }

                var responseObj = new
                {
                    RowCount = 0,
                    LatestUseIPInfoLists = (List<LatestUseIPInfoAPI>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    LatestUseIPInfoLists = responseObj.LatestUseIPInfoLists;
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
    }
}