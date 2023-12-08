using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemAPIGroupModel : SysModel
    {
        #region - Enum - 
        public enum Field
        {
            QuerySysID
        }
        #endregion

        #region - Class -
        public class SystemAPIGroup
        {
            public string APIGroupID { get; set; }
            public string APIGroupNM { get; set; }

            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }
        #endregion

        #region - Property -
        [Required]
        public string QuerySysID { get; set; }

        public List<SystemAPIGroup> SystemAPIGroupList { get; private set; }
        #endregion

        #region - Reset - 
        public void FormReset()
        {
            QuerySysID = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemAPIGroupList(string userID , int pageSize, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(QuerySysID)) return true;

                string apiUrl = API.SystemAPIGroup.QuerySystemAPIGroups(QuerySysID, userID, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemAPIGroups = (List<SystemAPIGroup>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemAPIGroupList = responseObj.SystemAPIGroups;

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
    }
}