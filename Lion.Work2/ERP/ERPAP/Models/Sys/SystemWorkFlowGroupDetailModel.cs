using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.EDIService;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemWorkFlowGroupDetailModel : SysModel, IValidatableObject
    {
        #region - Definitions -
        public class SystemWorkFlowGroupDetail
        {
            public string SysID { get; set; }
            public string WFFlowGroupID { get; set; }
            public string WFFlowGroupZHTW { get; set; }
            public string WFFlowGroupZHCN { get; set; }
            public string WFFlowGroupENUS { get; set; }
            public string WFFlowGroupTHTH { get; set; }
            public string WFFlowGroupJAJP { get; set; }
            public string WFFlowGroupKOKR { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserID { get; set; }
        }
        #endregion

        #region - Property -
        public SystemWorkFlowGroupDetail SystemWorkFlowGroupDetails { get; private set; }

        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysID { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string WFFlowGroupID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFFlowGroupZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFFlowGroupZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFFlowGroupENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFFlowGroupTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFFlowGroupJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFFlowGroupKOKR { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }
        #endregion

        #region - Validation -
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsValid == false)
            {
                yield break;
            }

            string apiUrl = API.SystemWorkFlowGroup.CheckSystemWorkFlowIsExists(SysID, WFFlowGroupID);

            string response = Common.HttpWebRequestGetResponseString(apiUrl, AppSettings.APITimeOut);

            var resopnseObj = Common.GetJsonDeserializeAnonymousType(response, new { IsExists = false });

            if (ExecAction == EnumActionType.Delete && resopnseObj.IsExists)
            {
                yield return new ValidationResult(SysSystemWorkFlowGroupDetail.DeleteSystemWorkFlowGroupDetailResult_DataExist);
            }

            apiUrl = API.SystemWorkFlowGroup.CheckSystemWorkFlowGroupIsExists(SysID, WFFlowGroupID);

            response = Common.HttpWebRequestGetResponseString(apiUrl, AppSettings.APITimeOut);

            resopnseObj = Common.GetJsonDeserializeAnonymousType(response, new { IsExists = false });

            if (ExecAction == EnumActionType.Add && resopnseObj.IsExists)
            {
                yield return new ValidationResult(SysSystemWorkFlowGroupDetail.SystemMsg_AddSystemWorkFlowGroupDetailExist);
            }
        }
        #endregion

        #region - 表單初始 -
        /// <summary>
        /// 表單初始
        /// </summary>
        public void FormReset()
        {
            WFFlowGroupZHTW = null;
            WFFlowGroupZHCN = null;
            WFFlowGroupENUS = null;
            WFFlowGroupTHTH = null;
            WFFlowGroupJAJP = null;
            WFFlowGroupKOKR = null;
            SortOrder = null;
        }
        #endregion

        #region - 取得工作流程群組明細 -
        /// <summary>
        /// 取得工作流程群組明細
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetSystemWorkFlowGroupDetail(string userID)
        {
            try
            {
                string apiUrl = API.SystemWorkFlowGroup.QuerySystemWorkFlowGroupDetail(userID, SysID, WFFlowGroupID);

                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var resopnseObj = new
                {
                    systemWorkFlowGroupDetails = (SystemWorkFlowGroupDetail)null
                };

                resopnseObj = Common.GetJsonDeserializeAnonymousType(response, resopnseObj);

                if (resopnseObj.systemWorkFlowGroupDetails != null)
                {
                    SystemWorkFlowGroupDetails = resopnseObj.systemWorkFlowGroupDetails;
                    WFFlowGroupZHTW = SystemWorkFlowGroupDetails.WFFlowGroupZHTW;
                    WFFlowGroupZHCN = SystemWorkFlowGroupDetails.WFFlowGroupZHCN;
                    WFFlowGroupENUS = SystemWorkFlowGroupDetails.WFFlowGroupENUS;
                    WFFlowGroupTHTH = SystemWorkFlowGroupDetails.WFFlowGroupTHTH;
                    WFFlowGroupJAJP = SystemWorkFlowGroupDetails.WFFlowGroupJAJP;
                    WFFlowGroupKOKR = SystemWorkFlowGroupDetails.WFFlowGroupKOKR;
                    SortOrder = SystemWorkFlowGroupDetails.SortOrder;
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

        #region - 編輯工作流程明細 -
        /// <summary>
        /// 編輯工作流程明細
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public async Task<bool> GetEditSystemWorkFlowGroupDetail(string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                SystemWorkFlowGroupDetail para = new SystemWorkFlowGroupDetail
                {
                    SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                    WFFlowGroupID = string.IsNullOrWhiteSpace(WFFlowGroupID) ? null : WFFlowGroupID,
                    WFFlowGroupZHTW = string.IsNullOrWhiteSpace(WFFlowGroupZHTW) ? null : WFFlowGroupZHTW,
                    WFFlowGroupZHCN = string.IsNullOrWhiteSpace(WFFlowGroupZHCN) ? null : WFFlowGroupZHCN,
                    WFFlowGroupENUS = string.IsNullOrWhiteSpace(WFFlowGroupENUS) ? null : WFFlowGroupENUS,
                    WFFlowGroupTHTH = string.IsNullOrWhiteSpace(WFFlowGroupTHTH) ? null : WFFlowGroupTHTH,
                    WFFlowGroupJAJP = string.IsNullOrWhiteSpace(WFFlowGroupJAJP) ? null : WFFlowGroupJAJP,
                    WFFlowGroupKOKR = string.IsNullOrWhiteSpace(WFFlowGroupKOKR) ? null : WFFlowGroupKOKR,
                    SortOrder = string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder,
                    UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID,
                };

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                var paraJsonStr = Common.GetJsonSerializeObject(para);

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.SystemWorkFlowGroup.EditSystemWorkFlowGroupDetail(userID);

                var response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new { EditStatus = false });

                if (responseObj.EditStatus)
                {
                    Mongo_BaseAP.EnumModifyType modifyType =
                    (ExecAction == EnumActionType.Add)
                        ? Mongo_BaseAP.EnumModifyType.I
                        : Mongo_BaseAP.EnumModifyType.U;

                    EntitySystemWorkFlowGroupDetail.SystemWorkFlowGroupDetailPara recordPara = _GetRecordPara(userID);

                    RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_WF_FLOW_GROUP, modifyType, userID, ipAddress, cultureID, recordPara);
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 刪除工作流程群組明細 -
        /// <summary>
        /// 刪除工作流程群組明細
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetDeleteSystemWorkFlowGroupDetail(string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                if (await GetSystemWorkFlowGroupDetail(userID))
                {
                    string apiUrl = API.SystemWorkFlowGroup.DeleteSystemWorkFlowGroupDetail(userID, SysID, WFFlowGroupID);

                    string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                    var responseObj = Common.GetJsonDeserializeAnonymousType(response, new { DeleteStatus = false });

                    if (responseObj.DeleteStatus == true)
                    {
                        EntitySystemWorkFlowGroupDetail.SystemWorkFlowGroupDetailPara recordPara = _GetRecordPara(userID);
                        RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_WF_FLOW_GROUP, Mongo_BaseAP.EnumModifyType.D, userID, ipAddress, cultureID, recordPara);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 事件-編輯 -
        internal DBEntity.DBTableRow GetEventParaSysSystemWFGroupEdit()
        {
            return
                new EntityEventPara.SysSystemWFGroupEdit
                {
                    TargetSysIDList = new List<DBVarChar> { new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID) },
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(SysID) ? null : SysID)),
                    WFFlowGroupID = new DBVarChar((string.IsNullOrWhiteSpace(WFFlowGroupID) ? null : WFFlowGroupID)),
                    WFFlowGroupzhTW = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowGroupZHTW) ? null : WFFlowGroupZHTW)),
                    WFFlowGroupzhCN = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowGroupZHCN) ? null : WFFlowGroupZHCN)),
                    WFFlowGroupenUS = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowGroupENUS) ? null : WFFlowGroupENUS)),
                    WFFlowGroupthTH = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowGroupTHTH) ? null : WFFlowGroupTHTH)),
                    WFFlowGroupjaJP = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowGroupJAJP) ? null : WFFlowGroupJAJP)),
                    WFFlowGroupkoKR = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowGroupKOKR) ? null : WFFlowGroupKOKR)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder))
                };
        }
        #endregion

        #region - 事件-刪除 -
        internal DBEntity.DBTableRow GetEventParaSysSystemWFGroupDelete()
        {
            return
                new EntityEventPara.SysSystemWFGroupDelete
                {
                    TargetSysIDList = new List<DBVarChar> { new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID) },
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(SysID) ? null : SysID)),
                    WFFlowGroupID = new DBVarChar((string.IsNullOrWhiteSpace(WFFlowGroupID) ? null : WFFlowGroupID))
                };
        }
        #endregion

        private EntitySystemWorkFlowGroupDetail.SystemWorkFlowGroupDetailPara _GetRecordPara(string userID)
        {
            return 
            new EntitySystemWorkFlowGroupDetail.SystemWorkFlowGroupDetailPara
            {
                SysID = new DBVarChar((string.IsNullOrWhiteSpace(SysID) ? null : SysID)),
                WFFlowGroupID = new DBVarChar((string.IsNullOrWhiteSpace(WFFlowGroupID) ? null : WFFlowGroupID)),
                WFFlowGroupZHTW = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowGroupZHTW) ? null : WFFlowGroupZHTW)),
                WFFlowGroupZHCN = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowGroupZHCN) ? null : WFFlowGroupZHCN)),
                WFFlowGroupENUS = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowGroupENUS) ? null : WFFlowGroupENUS)),
                WFFlowGroupTHTH = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowGroupTHTH) ? null : WFFlowGroupTHTH)),
                WFFlowGroupJAJP = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowGroupJAJP) ? null : WFFlowGroupJAJP)),
                WFFlowGroupKOKR = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowGroupKOKR) ? null : WFFlowGroupKOKR)),
                SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder)),
                UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
            };
        }
    }
}