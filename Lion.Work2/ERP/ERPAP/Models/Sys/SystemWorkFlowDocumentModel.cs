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
    public class SystemWorkFlowDocumentModel : SysModel
    {
        #region - Definitions -
        public new enum EnumCookieKey
        {
            SysID,
            WFFlowGroupID,
            WFCombineKey,
            WFFlowID,
            WFFlowVer,
            WFNodeID
        }

        public class SystemWFDocument
        {
            public string WFDocSeq { get; set; }
            public string WFDocNM { get; set; }
            public string IsReq { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDt { get; set; }
        }

        #endregion

        #region - Constructor -
        public SystemWorkFlowDocumentModel()
        {
            _entity = new EntitySystemWorkFlowDocument(ConnectionStringSERP, ProviderNameSERP);
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

        public List<SystemWFDocument> EntitySystemWFDocList { get; private set; }
        #endregion

        #region - Private -
        private readonly EntitySystemWorkFlowDocument _entity;
        #endregion

        #region - 取得工作流程文件檔 -
        /// <summary>
        /// 取得工作流程文件檔
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemWFDocList(EnumCultureID cultureID)
        {
            try
            {
                string sysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                string wfFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID;
                string wfFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer;
                string wfNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID;

                string apiUrl = API.SystemWorkFlowDocument.QuerySystemWorkFlowDocuments(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SystemWorkFlowDocuments = (List<SystemWFDocument>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    EntitySystemWFDocList = responseObj.SystemWorkFlowDocuments;
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