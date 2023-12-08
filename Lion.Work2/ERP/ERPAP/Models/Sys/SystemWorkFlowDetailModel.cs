using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
using static LionTech.Entity.ERP.Sys.EntitySystemWorkFlowDetail;

namespace ERPAP.Models.Sys
{
    public class SystemWorkFlowDetailModel : SysModel, IValidatableObject
    {
        #region - Constructor - 
        public class SystemWorkFlowDetail
        {
            public string SysID { get; set; }
            public string WFFlowGroupID { get; set; }
            public string WFFlowID { get; set; }
            public string WFFlowZHTW { get; set; }
            public string WFFlowZHCN { get; set; }
            public string WFFlowENUS { get; set; }
            public string WFFlowTHTH { get; set; }
            public string WFFlowJAJP { get; set; }
            public string WFFlowKOKR { get; set; }
            public string WFFlowVer { get; set; }
            public string FlowType { get; set; }
            public string FlowManUserID { get; set; }
            public string EnableDate { get; set; }
            public string DisableDate { get; set; }
            public string IsStartFun { get; set; }
            public string SortOrder { get; set; }
            public string MsgSysID { get; set; }
            public string MsgControllerID { get; set; }
            public string MsgActionName { get; set; }
            public string Remark { get; set; }
            public string UpdUserID { get; set; }
        }

        public class FlowRole
        {
            public string SysID;
            public string RoleID;
            public string RoleNM;
            public bool HasRole;
        }

        public class UserFlowRolePara
        {
            public string SysID;
            public string RoleID;
            public string WFFlowID;
            public string WFFlowVer;
            public string UpdUserID;
        }

        public class SystemWorkFlowDetails
        {
            public SystemWorkFlowDetail SystemWorkFlowDetail { get; set; }
            public List<UserFlowRolePara> SystemWorkFlowRoles { get; set; }
        }

        public SystemWorkFlowDetail EntitySystemWorkFlowDetail;
        #endregion

        #region - Property -
        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysID { get; set; }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxHidden)]
        public string WFFlowGroupID { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string WFFlowID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFFlowZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFFlowZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFFlowENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFFlowTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFFlowJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFFlowKOKR { get; set; }

        [Required]
        [StringLength(3)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string WFFlowVer { get; set; }

        [Required]
        public string FlowType { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string FlowManUserID { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string EnableDate { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [InputType(EnumInputType.TextBoxChar8)]
        public string DisableDate { get; set; }

        [EnumDataType(typeof(EnumYN))]
        public string IsStartFun { get; set; }

        [StringLength(6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }
        public string MsgSysID { get; set; }
        public string MsgControllerID { get; set; }
        public string MsgActionName { get; set; }
        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string Remark { get; set; }
        public List<string> HasRole { get; set; }

        public List<FlowType> EntityFlowTypeList { get; private set; }
        public List<FlowRole> EntityFlowRoleList { get; private set; }
        #endregion

        #region - Validation -
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsValid == false)
            {
                yield break;
            }

            string apiUrl = API.SystemWorkFlow.CheckSystemWorkFlowExists(SysID, WFFlowID, WFFlowVer);

            string resopnse = Common.HttpWebRequestGetResponseString(apiUrl, AppSettings.APITimeOut);

            var responseObj = Common.GetJsonDeserializeAnonymousType(resopnse, new { IsExists = false });

            if (ExecAction == EnumActionType.Delete && responseObj.IsExists == true)
            {
                yield return new ValidationResult(SysSystemWorkFlowDetail.DeleteSystemWorkFlowDetailResult_LogExist);
            }
        }
        #endregion

        #region - 表單初始 -
        /// <summary>
        /// 表單初始
        /// </summary>
        public void FormReset()
        {
            WFFlowZHTW = null;
            WFFlowZHCN = null;
            WFFlowENUS = null;
            WFFlowJAJP = null;
            WFFlowTHTH = null;
            WFFlowKOKR = null;

            FlowType = null;
            FlowManUserID = null;
            EnableDate = null;
            DisableDate = null;

            IsStartFun = EnumYN.N.ToString();
            SortOrder = null;
            MsgSysID = null;
            MsgControllerID = null;
            MsgActionName = null;
            Remark = null;
        }

        #endregion

        #region - 流程類型 -
        /// <summary>
        /// 流程類型
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetFlowTypeList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemSetting.CodeManagement(userID, Common.GetEnumDesc(Entity_BaseAP.EnumCMCodeKind.WorkFlowType), cultureID.ToString().ToUpper());
                string reponse = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                EntityFlowTypeList = Common.GetJsonDeserializeObject<List<FlowType>>(reponse);

                if (EntityFlowTypeList != null)
                {
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

        #region - 取得工作流程明細 -
        /// <summary>
        /// 取得工作流程明細
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetSystemWorkFlowDetail(string userID)
        {
            try
            {
                string apiUrl = API.SystemWorkFlow.QuerySystemWorkFlowDetail(userID, SysID, WFFlowID, WFFlowVer);
                string resopnse = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                EntitySystemWorkFlowDetail = Common.GetJsonDeserializeObject<SystemWorkFlowDetail>(resopnse);

                if (EntitySystemWorkFlowDetail != null)
                {
                    WFFlowZHTW = EntitySystemWorkFlowDetail.WFFlowZHTW;
                    WFFlowZHCN = EntitySystemWorkFlowDetail.WFFlowZHCN;
                    WFFlowENUS = EntitySystemWorkFlowDetail.WFFlowENUS;
                    WFFlowTHTH = EntitySystemWorkFlowDetail.WFFlowTHTH;
                    WFFlowJAJP = EntitySystemWorkFlowDetail.WFFlowJAJP;
                    WFFlowKOKR = EntitySystemWorkFlowDetail.WFFlowKOKR;
                    FlowType = EntitySystemWorkFlowDetail.FlowType;
                    FlowManUserID = EntitySystemWorkFlowDetail.FlowManUserID;
                    EnableDate = EntitySystemWorkFlowDetail.EnableDate;
                    DisableDate = EntitySystemWorkFlowDetail.DisableDate;
                    IsStartFun = EntitySystemWorkFlowDetail.IsStartFun;
                    SortOrder = EntitySystemWorkFlowDetail.SortOrder;
                    MsgSysID = EntitySystemWorkFlowDetail.MsgSysID;
                    MsgControllerID = EntitySystemWorkFlowDetail.MsgControllerID;
                    MsgActionName = EntitySystemWorkFlowDetail.MsgActionName;
                    Remark = EntitySystemWorkFlowDetail.Remark;
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

        #region - 取得系統角色清單 -
        /// <summary>
        /// 取得系統角色清單
        /// </summary>
        /// <param name="cultrueID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemUserRoleList(string userID, EnumCultureID cultrueID)
        {
            try
            {
                string apiUrl = API.SystemWorkFlow.QuerySystemWorkFlowRoles(userID, SysID, WFFlowID, WFFlowVer, cultrueID.ToString().ToUpper());

                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                EntityFlowRoleList = Common.GetJsonDeserializeObject<List<FlowRole>>(response);

                if (EntityFlowRoleList != null)
                {
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

        #region - 編輯工作流程明細 -
        /// <summary>
        /// 編輯工作流程明細
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public async Task<bool> GetEditSystemWorkFlowDetail(string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                SystemWorkFlowDetails para = new SystemWorkFlowDetails();

                para.SystemWorkFlowDetail = new SystemWorkFlowDetail
                {
                    SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                    WFFlowGroupID = string.IsNullOrWhiteSpace(WFFlowGroupID) ? null : WFFlowGroupID,
                    WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID,
                    WFFlowZHTW = string.IsNullOrWhiteSpace(WFFlowZHTW) ? null : WFFlowZHTW,
                    WFFlowZHCN = string.IsNullOrWhiteSpace(WFFlowZHCN) ? null : WFFlowZHCN,
                    WFFlowENUS = string.IsNullOrWhiteSpace(WFFlowENUS) ? null : WFFlowENUS,
                    WFFlowTHTH = string.IsNullOrWhiteSpace(WFFlowTHTH) ? null : WFFlowTHTH,
                    WFFlowJAJP = string.IsNullOrWhiteSpace(WFFlowJAJP) ? null : WFFlowJAJP,
                    WFFlowKOKR = string.IsNullOrWhiteSpace(WFFlowKOKR) ? null : WFFlowKOKR,
                    WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer,
                    FlowType = string.IsNullOrWhiteSpace(FlowType) ? null : FlowType,
                    FlowManUserID = string.IsNullOrWhiteSpace(FlowManUserID) ? null : FlowManUserID,
                    EnableDate = string.IsNullOrWhiteSpace(EnableDate) ? null : EnableDate,
                    DisableDate = string.IsNullOrWhiteSpace(DisableDate) ? null : DisableDate,
                    IsStartFun = EnumYN.Y.ToString(),
                    SortOrder = string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder,
                    MsgSysID = string.IsNullOrWhiteSpace(MsgSysID) ? null : MsgSysID,
                    MsgControllerID = string.IsNullOrWhiteSpace(MsgControllerID) ? null : MsgControllerID,
                    MsgActionName = string.IsNullOrWhiteSpace(MsgActionName) ? null : MsgActionName,
                    Remark = string.IsNullOrWhiteSpace(Remark) ? null : Remark,
                    UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID
                };

                List<EntitySystemWorkFlowDetail.UserFlowRolePara> paraList = new List<EntitySystemWorkFlowDetail.UserFlowRolePara>();

                if (HasRole != null && HasRole.Count > 0)
                {
                    para.SystemWorkFlowRoles = new List<UserFlowRolePara>(HasRole.Select(roleString => new UserFlowRolePara
                    {
                        SysID = string.IsNullOrWhiteSpace(roleString.Split('|')[0]) ? null : roleString.Split('|')[0],
                        RoleID = string.IsNullOrWhiteSpace(roleString.Split('|')[1]) ? null : roleString.Split('|')[1],
                        WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID,
                        WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer,
                        UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID
                    }));
                    paraList.AddRange(HasRole.Select(roleString =>
                    new EntitySystemWorkFlowDetail.UserFlowRolePara
                    {
                        SysID = new DBVarChar((string.IsNullOrWhiteSpace(roleString.Split('|')[0]) ? null : roleString.Split('|')[0])),
                        RoleID = new DBVarChar((string.IsNullOrWhiteSpace(roleString.Split('|')[1]) ? null : roleString.Split('|')[1])),
                        WFFlowID = new DBVarChar((string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID)),
                        WFFlowVer = new DBChar((string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer)),
                        UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                    }));
                }

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                {
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                };

                var paraJsonStr = Common.GetJsonSerializeObject(para);

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.SystemWorkFlow.EditSystemWorkFlowDetail(userID);

                var response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new { EditStatus = false });

                if (responseObj.EditStatus)
                {
                    Mongo_BaseAP.EnumModifyType modifyType =
                        (ExecAction == EnumActionType.Add)
                           ? Mongo_BaseAP.EnumModifyType.I
                           : Mongo_BaseAP.EnumModifyType.U;
                    RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_WF_FLOW, modifyType, userID, ipAddress, cultureID, _GetWorkFlowRecordPara(userID));
                    RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_ROLE_FLOW, modifyType, userID, ipAddress, cultureID, paraList);
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

        #region - 刪除工作流程 -
        /// <summary>
        /// 刪除工作流程
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetDeleteSystemWorkFlowDetail(string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                if (await GetSystemWorkFlowDetail(userID))
                {
                    SystemWorkFlowDetail para = new SystemWorkFlowDetail
                    {
                        SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                        WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID,
                        WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer
                    };
                    string apiUrl = API.SystemWorkFlow.DeleteSystemWorkFlowDetail(userID, para.SysID, para.WFFlowID, para.WFFlowVer);

                    var response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                    var responseObj = Common.GetJsonDeserializeAnonymousType(response, new { DeleteStatus = false });

                    if (responseObj.DeleteStatus == true)
                    {
                        RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_WF_FLOW, Mongo_BaseAP.EnumModifyType.D, userID, ipAddress, cultureID, _GetWorkFlowRecordPara(userID));
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
        internal DBEntity.DBTableRow GetEventParaSysSystemWFEdit()
        {
            return
                new EntityEventPara.SysSystemWFEdit
                {
                    TargetSysIDList = new List<DBVarChar> { new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID) },
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(SysID) ? null : SysID)),
                    WFFlowGroupID = new DBVarChar((string.IsNullOrWhiteSpace(WFFlowGroupID) ? null : WFFlowGroupID)),
                    WFFlowID = new DBVarChar((string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID)),
                    WFFlowzhTW = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowZHTW) ? null : WFFlowZHTW)),
                    WFFlowzhCN = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowZHCN) ? null : WFFlowZHCN)),
                    WFFlowenUS = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowENUS) ? null : WFFlowENUS)),
                    WFFlowthTH = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowTHTH) ? null : WFFlowTHTH)),
                    WFFlowjaJP = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowJAJP) ? null : WFFlowJAJP)),
                    WFFlowkoKR = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowKOKR) ? null : WFFlowKOKR)),
                    WFFlowVer = new DBChar((string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer)),
                    FlowType = new DBVarChar((string.IsNullOrWhiteSpace(FlowType) ? null : FlowType)),
                    FlowManUserID = new DBVarChar((string.IsNullOrWhiteSpace(FlowManUserID) ? null : FlowManUserID)),
                    EnableDate = new DBChar((string.IsNullOrWhiteSpace(EnableDate) ? null : EnableDate)),
                    DisableDate = new DBChar((string.IsNullOrWhiteSpace(DisableDate) ? null : DisableDate)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder)),
                    MsgSysID = new DBVarChar((string.IsNullOrWhiteSpace(MsgSysID) ? null : MsgSysID)),
                    MsgControllerID = new DBVarChar((string.IsNullOrWhiteSpace(MsgControllerID) ? null : MsgControllerID)),
                    MsgActionName = new DBVarChar((string.IsNullOrWhiteSpace(MsgActionName) ? null : MsgActionName)),
                };
        }
        #endregion

        #region - 事件-刪除 -
        internal DBEntity.DBTableRow GetEventParaSysSystemWFDelete()
        {
            return
                new EntityEventPara.SysSystemWFDelete
                {
                    TargetSysIDList = new List<DBVarChar> { new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID) },
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(SysID) ? null : SysID)),
                    WFFlowGroupID = new DBVarChar((string.IsNullOrWhiteSpace(WFFlowGroupID) ? null : WFFlowGroupID)),
                    WFFlowID = new DBVarChar((string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID)),
                    WFFlowVer = new DBChar((string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer))
                };
        }
        #endregion

        private SystemWorkFlowDetailPara _GetWorkFlowRecordPara(string userID)
        {
            return new SystemWorkFlowDetailPara
            {
                SysID = new DBVarChar((string.IsNullOrWhiteSpace(SysID) ? null : SysID)),
                WFFlowGroupID = new DBVarChar((string.IsNullOrWhiteSpace(WFFlowGroupID) ? null : WFFlowGroupID)),
                WFFlowID = new DBVarChar((string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID)),
                WFFlowZHTW = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowZHTW) ? null : WFFlowZHTW)),
                WFFlowZHCN = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowZHCN) ? null : WFFlowZHCN)),
                WFFlowENUS = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowENUS) ? null : WFFlowENUS)),
                WFFlowTHTH = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowTHTH) ? null : WFFlowTHTH)),
                WFFlowJAJP = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowJAJP) ? null : WFFlowJAJP)),
                WFFlowKOKR = new DBNVarChar((string.IsNullOrWhiteSpace(WFFlowKOKR) ? null : WFFlowKOKR)),
                WFFlowVer = new DBChar((string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer)),
                FlowType = new DBVarChar((string.IsNullOrWhiteSpace(FlowType) ? null : FlowType)),
                FlowManUserID = new DBVarChar((string.IsNullOrWhiteSpace(FlowManUserID) ? null : FlowManUserID)),
                EnableDate = new DBChar((string.IsNullOrWhiteSpace(EnableDate) ? null : EnableDate)),
                DisableDate = new DBChar((string.IsNullOrWhiteSpace(DisableDate) ? null : DisableDate)),
                IsStartFun = new DBChar(EnumYN.Y.ToString()),
                SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder)),
                MsgSysID = new DBVarChar((string.IsNullOrWhiteSpace(MsgSysID) ? null : MsgSysID)),
                MsgControllerID = new DBVarChar((string.IsNullOrWhiteSpace(MsgControllerID) ? null : MsgControllerID)),
                MsgActionName = new DBVarChar((string.IsNullOrWhiteSpace(MsgActionName) ? null : MsgActionName)),
                Remark = new DBNVarChar((string.IsNullOrWhiteSpace(Remark) ? null : Remark)),
                UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
            };
        }
    }
}