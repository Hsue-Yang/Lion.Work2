using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemWorkFlowNextModel : SysModel
    {
        #region - Definitions -
        public new enum EnumCookieKey
        {
            SysID, WFFlowGroupID, WFCombineKey, WFFlowID, WFFlowVer, WFNodeID
        }

        public class SystemWorkFlowNext
        {
            public string NextWFNodeID { get; set; }
            public string NextWFNodeNM { get; set; }
            public string NextNodeTypeNM { get; set; }
            public string NextResultValue { get; set; }
            public string FunSysNM { get; set; }
            public string SubSysNM { get; set; }
            public string FunControllerNM { get; set; }
            public string FunActionNameNM { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDt { get; set; }
        }
        #endregion

        #region - Constructor -
        public SystemWorkFlowNextModel()
        {
            _entity = new EntitySystemWorkFlowNext(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        [Required]
        public string SysID { get; set; }

        [Required]
        public string WFFlowGroupID { get; set; }

        [Required]
        public string WFCombineKey { get; set; }
        
        public string WFFlowID { get; set; }

        public string WFFlowVer { get; set; }

        public string WFNodeID { get; set; }

        public List<SystemWorkFlowNext> EntitySystemWFNextList { get; private set; }
        #endregion

        #region - Private -
        private readonly EntitySystemWorkFlowNext _entity;
        #endregion

        #region - 取得工作流程次節點檔 -
        /// <summary>
        /// 取得工作流程次節點檔
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemWFNextList(EnumCultureID cultureID)
        {
            try
            {
                string sysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                string wfFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID;
                string wfFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer;
                string wfNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID;

                string apiUrl = API.SystemWorkFlowNext.QuerySystemWorkFlowNext(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID.ToString().ToUpper());
                var response =  await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                var responseObj = new
                {
                    SystemWorkFlowNextList = (List<SystemWorkFlowNext>)null
                };
                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj.SystemWorkFlowNextList != null)
                {
                    EntitySystemWFNextList = responseObj.SystemWorkFlowNextList;
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