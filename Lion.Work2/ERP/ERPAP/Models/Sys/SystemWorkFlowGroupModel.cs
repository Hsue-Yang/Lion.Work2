using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;

namespace ERPAP.Models.Sys
{
    public class SystemWorkFlowGroupModel : SysModel
    {
        #region - Definitions -
        public new enum EnumCookieKey
        {
            SysID
        }

        public class SystemWorkFlowGroup
        {
            public string SysID { get; set; }
            public string SysNm { get; set; }
            public string WFFlowGroupID { get; set; }
            public string WFFlowGroupNM { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDt { get; set; }
        }

        #endregion

        #region - Property -
        [Required]
        public string SysID { get; set; }

        public List<SystemWorkFlowGroup> EntitySystemWorkFlowGroupList { get; private set; }
    
    #endregion

    #region - 表單初始 -
    /// <summary>
    /// 表單初始
    /// </summary>
    public void FormReset()
    {
        SysID = EnumSystemID.ERPAP.ToString();
    }
    #endregion

    #region - 取得工作流程群組清單 -
    /// <summary>
    /// 取得工作流程群組清單
    /// </summary>
    /// <param name="cultureID"></param>
    /// <returns></returns>
    public async Task<bool> GetSystemWorkFlowGroupList(string userID, EnumCultureID cultureID)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(SysID))
            {
                EntitySystemWorkFlowGroupList = new List<SystemWorkFlowGroup>();
                return true;
            }

            string apiUrl = API.SystemWorkFlowGroup.QuerySystemWorkFlowGroup(userID, SysID, cultureID.ToString().ToUpper());
            string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

            var responseObj = new
            {
                systemWorkFlowGroupList = (List<SystemWorkFlowGroup>)null
            };

            responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

            if (responseObj != null)
            {
                EntitySystemWorkFlowGroupList = responseObj.systemWorkFlowGroupList;
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