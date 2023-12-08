using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemAPIModel : SysModel
    {
        #region - Enum -
        public enum Field
        {
            QuerySysID,
            QueryAPIGroupID
        }
        #endregion

        #region - Class -
        public class SystemAPI
        {
            public string APIGroupID { get; set; }
            public string APIGroupNM { get; set; }

            public string APIFunID { get; set; }
            public string APIFunNM { get; set; }
            public string APIPara { get; set; }
            public string APIReturn { get; set; }
            public string APIParaDesc { get; set; }
            public string APIReturnContent { get; set; }

            public string IsOutside { get; set; }
            public string IsDisable { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }
        #endregion

        #region - Property -
        [Required]
        public string QuerySysID { get; set; }

        public string QueryAPIGroupID { get; set; }

        public List<SystemAPI> SystemAPIList { get; private set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            QuerySysID = string.Empty;
            QueryAPIGroupID = string.Empty;
        }
        #endregion

        #region - 取得API清單 -
        /// <summary>
        /// 取得API清單
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageSize"></param>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemAPIList(string userID, int pageSize, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(QuerySysID)) return true;

                string apiUrl = API.SystemAPI.QuerySystemAPIs(QuerySysID, userID, QueryAPIGroupID, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemAPIs = (List<SystemAPI>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemAPIList = responseObj.SystemAPIs;

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