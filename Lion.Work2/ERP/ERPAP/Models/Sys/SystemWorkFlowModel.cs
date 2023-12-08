using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;

namespace ERPAP.Models.Sys
{
    public class SystemWorkFlowModel : SysModel
    {
        #region - Definitions -
        public new enum EnumCookieKey
        {
            SysID,
            WFFlowGroupID
        }

        public class SystemWorkFlow
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string WFFlowGroupID { get; set; }
            public string WFFlowGroupNM { get; set; }
            public string WFFlowID { get; set; }
            public string WFFlowNM { get; set; }
            public string WFFlowVer { get; set; }
            public string FlowTypeNM { get; set; }
            public string FlowManUserNM { get; set; }
            public string EnableDate { get; set; }
            public string DisableDate { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDt { get; set; }
        }
        #endregion

        #region - Property -
        [Required]
        public string SysID { get; set; }

        public string WFFlowGroupID { get; set; }

        public List<SystemWorkFlow> EntitySystemWorkFlowList { get; private set; }
        #endregion

        #region - 表單初始 -
        /// <summary>
        /// 表單初始
        /// </summary>
        public void FormReset()
        {
            SysID = EnumSystemID.ERPAP.ToString();
            WFFlowGroupID = null;
        }
        #endregion

        #region - 取得工作流程清單 -
        /// <summary>
        /// 取得工作流程清單
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemWorkFlowList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemWorkFlow.QuerySystemWorkFlow(userID, SysID, WFFlowGroupID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    systemWorkFlowList = (List<SystemWorkFlow>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    EntitySystemWorkFlowList = responseObj.systemWorkFlowList;
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