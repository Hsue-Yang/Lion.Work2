using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemTeamsModel : SysModel
    {
        public class SystemTeams
        {
            public string TeamsChannelID { get; set; }
            public string TeamsChannelNM { get; set; }

            public string IsDisable { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }

        #region - Property -
        [Required]
        public string SysID { get; set; }

        public List<SystemTeams> SystemTeamsList { get; private set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            SysID = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemTeamsList(string userID, int pageSize, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SysID)) return true;

                string apiUrl = API.SystemTeams.QuerySystemTeamss(SysID, userID, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemTeamss = (List<SystemTeams>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemTeamsList = responseObj.SystemTeamss;

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