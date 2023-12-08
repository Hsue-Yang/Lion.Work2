using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;

namespace ERPAP.Models.Sys
{
    public class SystemWorkFlowNodeModel : SysModel
    {
        #region - Definitions -
        public new enum EnumCookieKey
        {
            SysID, WFFlowGroupID, WFCombineKey,
            WFFlowID, WFFlowVer
        }

        public class SystemWorkFlowGroup : DBEntity.ISelectItem
        {
            public string SysID { get; set; }
            public string SysNm { get; set; }
            public string WFFlowGroupID { get; set; }
            public string WFFlowGroupNM { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDt { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return $"{WFFlowGroupNM}";
            }

            public string ItemValue()
            {
                return WFFlowGroupID;
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }
        }

        public class SysUserSystemWorkFlowID : DBEntity.ISelectItem
        {

            public string WF_FLOW_ID { get; set; }
            public string WF_FLOW_VER { get; set; }
            public string WF_FLOW_VALUE { get; set; }
            public string WF_FLOW_TEXT { get; set; }
            public string SORT_ORDER { get; set; }


            public string WFFlowID { get { return WF_FLOW_ID; } }
            public string WFFlowVer { get { return WF_FLOW_VER; } }

            public string WFFlowValue { get { return WF_FLOW_VALUE; } }
            public string WFFlowText { get { return WF_FLOW_TEXT; } }
            public string SortOrder { get { return SORT_ORDER; } }

            public string ItemText()
            {
                return $"{WFFlowText}";
            }

            public string ItemValue()
            {
                return WFFlowValue;
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region - Constructor -
        public SystemWorkFlowNodeModel()
        {
            _entity = new EntitySystemWorkFlowNode(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        [Required]
        public string SysID { get; set; }

        [Required]
        public string WFFlowGroupID { get; set; }

        [Required]
        public string WFCombineKey { get; set; }

        [InputType(EnumInputType.TextBoxHidden)]
        public string WFFlowID { get; set; }

        [InputType(EnumInputType.TextBoxHidden)]
        public string WFFlowVer { get; set; }

        public List<SystemWorkFlowNode> EntitySystemWorkFlowNodeList { get; private set; }

        public List<SystemWorkFlowGroup> EntitySystemWorkFlowGroupList { get; private set; }

        public List<SysUserSystemWorkFlowID> SysUserSystemWorkFlowIDList { get; private set; }
        #endregion

        #region - Private -
        private readonly EntitySystemWorkFlowNode _entity;
        #endregion

        #region - 表單初始 -
        /// <summary>
        /// 表單初始
        /// </summary>
        public void FormReset()
        {
            SysID = EnumSystemID.ERPAP.ToString();
            WFFlowGroupID = null;
            WFCombineKey = null;

            WFFlowID = null;
            WFFlowVer = null;
        }
        #endregion

        #region - 設定系統參數 -
        /// <summary>
        /// 設定系統參數
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cultureID"></param>
        public void SetSysParameter(string userID, EnumCultureID cultureID)
        {
            if (SysUserSystemWorkFlowIDList != null &&
                SysUserSystemWorkFlowIDList.Count > 0 &&
                (string.IsNullOrWhiteSpace(WFFlowID) || string.IsNullOrWhiteSpace(WFFlowVer)))
            {
                WFFlowID = SysUserSystemWorkFlowIDList[0].WFFlowID;
                WFFlowVer = SysUserSystemWorkFlowIDList[0].WFFlowVer;
            }

            if (SysUserSystemWorkFlowIDList != null)
            {
                WFCombineKey = (from s in SysUserSystemWorkFlowIDList
                                where s.WFFlowID == WFFlowID &&
                                      s.WFFlowVer == WFFlowVer
                                select s.ItemValue()).SingleOrDefault();
            }
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

        #region - 取得工作流程結點 -
        /// <summary>
        /// 取得工作流程結點
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemWorkFlowNodeList(string userID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cultureID.ToString()) || string.IsNullOrWhiteSpace(SysID) || string.IsNullOrWhiteSpace(WFFlowID) || string.IsNullOrWhiteSpace(WFFlowVer))
                {
                    EntitySystemWorkFlowNodeList = new List<SystemWorkFlowNode>();
                    return true;
                }

                List<SystemWorkFlowNode> systemWorkFlowNode = new List<SystemWorkFlowNode>();
                string apiUrl = API.SystemWorkFlowNode.QuerySystemWorkFlowNodes(userID, SysID, cultureID.ToString().ToUpper(), WFFlowID, WFFlowVer);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    systemWorkFlowNodeList = (List<SystemWorkFlowNode>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    EntitySystemWorkFlowNodeList = responseObj.systemWorkFlowNodeList;
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

        #region - WorkFlow -
        public async Task<bool> GetSysUserSystemWorkFlowIDs(string userID, string sysID, EnumCultureID cultureID, string wfFlowGroupID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cultureID.ToString()) || string.IsNullOrWhiteSpace(wfFlowGroupID))
				{
                    SysUserSystemWorkFlowIDList = new List<SysUserSystemWorkFlowID>();
                    return true;
                }

                if (EntitySystemWorkFlowGroupList != null && EntitySystemWorkFlowGroupList.Count > 0 && string.IsNullOrWhiteSpace(WFFlowGroupID))
                {
                    WFFlowGroupID = EntitySystemWorkFlowGroupList[0].ItemValue();
                }

                List<SysUserSystemWorkFlowID> systemWorkFlowNode = new List<SysUserSystemWorkFlowID>();
                string apiUrl = API.SystemWorkFlowNode.QuerySysUserSystemWorkFlowID(userID, sysID, cultureID.ToString().ToUpper(), wfFlowGroupID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    sysUserSystemWorkFlowIDList = (List<SysUserSystemWorkFlowID>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    SysUserSystemWorkFlowIDList = responseObj.sysUserSystemWorkFlowIDList;
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