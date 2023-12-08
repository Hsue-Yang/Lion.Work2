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

namespace ERPAP.Models.Sys
{
    public class SystemWorkFlowSignatureDetailModel : SysModel, IValidatableObject
    {
        #region - Constructor -
        public SystemWorkFlowSignatureDetailModel()
        {
            SigStepList = Enumerable.Range(1, 20).ToDictionary(k => k.ToString(), v => v.ToString());
            _entity = new EntitySystemWorkFlowSignatureDetail(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        public class SystemWFSigSeq : DBEntity.ISelectItem
        {
            public string CodeID { get; set; }
            public string CodeNM { get; set; }

            public string ItemText()
            {
                return CodeNM;
            }

            public string ItemValue()
            {
                return CodeID;
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

        public class SystemWFSignatureDetail
        {
            public SystemWFSigDetail SystemWFSigDetail = new SystemWFSigDetail();

            public List<SystemRoleSignatures> SystemRoleSignatures = new List<SystemRoleSignatures>();
        }

        public class SystemWFSigDetail
        {
            public string SysID { get; set; }
            public string WFFlowID { get; set; }
            public string WFFlowVer { get; set; }
            public string WFNodeID { get; set; }
            public int SigStep { get; set; }
            public string WFSigSeq { get; set; }
            public string WFSigZHTW { get; set; }
            public string WFSigZHCN { get; set; }
            public string WFSigENUS { get; set; }
            public string WFSigTHTH { get; set; }
            public string WFSigJAJP { get; set; }
            public string WFSigKOKR { get; set; }
            public string SigType { get; set; }
            public string APISysID { get; set; }
            public string APIControllerID { get; set; }
            public string APIActionName { get; set; }
            public string CompareWFNodeID { get; set; }
            public string CompareWFSigSeq { get; set; }
            public string ChkAPISysID { get; set; }
            public string ChkAPIControllerID { get; set; }
            public string ChkAPIActionName { get; set; }
            public string IsReq { get; set; }
            public string Remark { get; set; }
            public string UpdUserID { get; set; }
        }

        public class SystemRoleSignatures
        {
            public string SysID { get; set; }
            public string RoleID { get; set; }
            public string WFFlowID { get; set; }
            public string WFFlowVer { get; set; }
            public string WFNodeID { get; set; }
            public string WFSigSeq { get; set; }
            public string UpdUserID { get; set; }
        }

        public class SystemWorkFlowSignatureDetail
        {
            public string SysID { get; set; }
            public string WFFlowID { get; set; }
            public string WFFlowVer { get; set; }
            public string WFNodeID { get; set; }
            public string WFSigSeq { get; set; }
            public int SigStep { get; set; }
            public string WFSigZHTW { get; set; }
            public string WFSigZHCN { get; set; }
            public string WFSigENUS { get; set; }
            public string WFSigTHTH { get; set; }
            public string WFSigJAJP { get; set; }
            public string WFSigKOKR { get; set; }
            public string SigType { get; set; }
            public string APISysID { get; set; }
            public string APIControllerID { get; set; }
            public string APIActionName { get; set; }
            public string CompareWFNodeID { get; set; }
            public string CompareWFSigSeq { get; set; }
            public string ChkAPISysID { get; set; }
            public string ChkAPIControllerID { get; set; }
            public string ChkAPIActionName { get; set; }
            public string IsReq { get; set; }
            public string Remark { get; set; }
        }

        public class SystemRoleSig
        {
            public string SysID { get; set; }
            public string WFFlowID { get; set; }
            public string WFFlowVer { get; set; }
            public string WFNodeID { get; set; }
            public string WFSigSeq { get; set; }
            public string RoleID { get; set; }
            public string RoleNM { get; set; }
            public string HasRole { get; set; }
        }

        public SystemWorkFlowSignatureDetail SystemWorkFlowSigDetail { get; set; }

        public List<SystemWFSigSeq> SystemWFSigSeqs { get; set; }

        public List<SystemRoleSig> SystemRoleSigs { get; set; }
        #region - Property -
        [Required]
        public string SysID { get; set; }

        [Required]
        public string WFFlowGroupID { get; set; }

        [Required]
        public string WFCombineKey { get; set; }

        [Required]
        public string WFFlowID { get; set; }

        [Required]
        public string WFFlowVer { get; set; }

        [Required]
        public string WFNodeID { get; set; }

        [Required]
        [Range(0, 999)]
        [InputType(EnumInputType.TextBoxInteger)]
        public int SigStep { get; set; }

        public string WFSigSeq { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFSigZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFSigZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFSigENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFSigTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFSigJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFSigKOKR { get; set; }

        [Required]
        public string SigType { get; set; }

        public string APISysID { get; set; }

        public string APIControllerID { get; set; }

        public string APIActionName { get; set; }

        public string CompareWFSigSeq { get; set; }

        public string ValidAPISysID { get; set; }

        public string ValidAPIControllerID { get; set; }

        public string ValidAPIActionName { get; set; }

        public string IsReq { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string Remark { get; set; }

        public List<string> HasRole { get; set; }

        public Dictionary<string, string> SigStepList { get; private set; }

        public IEnumerable<SysModel.ISelectItem> ValidSystemSysIDSelectItems { get; set; }
        public IEnumerable<SysModel.ISelectItem> ValidSystemAPIGroupSelectItems { get; set; }
        public IEnumerable<SysModel.ISelectItem> ValidSystemAPIFuntionSelectItems { get; set; }
        public IEnumerable<DBEntity.ISelectItem> SignatureUserTypeSelectItems { get; private set; }
        public IEnumerable<DBEntity.ISelectItem> SystemWFSigSeqseSelectItems { get; private set; }
        public List<EntitySystemWorkFlowSignatureDetail.SystemRoleSig> EntitySystemRoleSigList { get; private set; }
        #endregion

        #region - Private -
        private readonly EntitySystemWorkFlowSignatureDetail _entity;
        #endregion

        #region - Validation -
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsValid == false)
            {
                yield break;
            }

            if (ExecAction == EnumActionType.Delete)
            {
                string checkRunTimeUrl = API.SystemWorkFlowNodeDetail.CheckWorkFlowHasRunTime(SysID, WFFlowID, WFFlowVer);
                string checkRunTimeResponse = Common.HttpWebRequestGetResponseString(checkRunTimeUrl, AppSettings.APITimeOut);

                var isRunTime = new
                {
                    IsRunTime = new bool()
                };
                isRunTime = Common.GetJsonDeserializeAnonymousType(checkRunTimeResponse, isRunTime);

                if (isRunTime.IsRunTime)
                {
                    yield return new ValidationResult(SysSystemWorkFlowNodeDetail.DeleteSystemWorkFlowNodeDetailResult_RuntimeExist);
                }
            }
        }
        #endregion

        #region - 表單初始 -
        public void FormReset()
        {
            SigStep = 1;
        }
        #endregion

        #region - 取得簽核人員類型 -
        /// <summary>
        /// 取得簽核人員類型
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public bool GetSigTypeList(EnumCultureID cultureID)
        {
            try
            {
                SignatureUserTypeSelectItems =
                    new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                        .SelectCMCodeList(new Entity_BaseAP.CMCodePara
                        {
                            CodeKind = Entity_BaseAP.EnumCMCodeKind.SignatureUserType,
                            ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNMID,
                            CultureID = new DBVarChar(cultureID)
                        });

                if (SignatureUserTypeSelectItems != null)
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

        #region - 取得簽核點 -
        /// <summary>
        /// 取得簽核點
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemWFSigSeqList(EnumCultureID cultureID)
        {
            try
            {
                SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID;
                WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer;
                WFNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID;

                string apiUrl = API.SystemWorkFlowSignature.QuerySystemWorkFlowSignatureSeqs(SysID, WFFlowID, WFFlowVer, WFNodeID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                var responseObj = new
                {
                    SystemWFSigSeqList = (List<SystemWFSigSeq>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);
                SystemWFSigSeqs = responseObj.SystemWFSigSeqList;
                return true;
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
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemRoleSigList(EnumCultureID cultureID)
        {
            try
            {
                SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID;
                WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer;
                WFNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID;
                WFSigSeq = string.IsNullOrWhiteSpace(WFSigSeq) ? null : WFSigSeq;

                string apiUrl = API.SystemWorkFlowSignature.QuerytSystemRoleSignatures(SysID, WFFlowID, WFFlowVer, WFNodeID, WFSigSeq, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SystemRoleSigList = (List<SystemRoleSig>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);
                SystemRoleSigs = responseObj.SystemRoleSigList;

                if (responseObj != null)
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

        #region - 取得工作流程簽核明細 -

        /// <summary>
        /// 取得工作流程簽核明細
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemWFSig(EnumCultureID cultureID)
        {
            try
            {
                SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID;
                WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer;
                WFNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID;
                WFSigSeq = string.IsNullOrWhiteSpace(WFSigSeq) ? null : WFSigSeq;

                string apiUrl = API.SystemWorkFlowSignature.QuerytSystemWorkFlowSignatureDetail(SysID, WFFlowID, WFFlowVer, WFNodeID, WFSigSeq, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SystemWFSigDetail = (SystemWorkFlowSignatureDetail)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj.SystemWFSigDetail != null)
                {
                    SigStep = responseObj.SystemWFSigDetail.SigStep;
                    WFSigZHTW = responseObj.SystemWFSigDetail.WFSigZHTW;
                    WFSigZHCN = responseObj.SystemWFSigDetail.WFSigZHCN;
                    WFSigENUS = responseObj.SystemWFSigDetail.WFSigENUS;
                    WFSigTHTH = responseObj.SystemWFSigDetail.WFSigTHTH;
                    WFSigJAJP = responseObj.SystemWFSigDetail.WFSigJAJP;
                    WFSigKOKR = responseObj.SystemWFSigDetail.WFSigKOKR;
                    SigType = responseObj.SystemWFSigDetail.SigType;
                    APISysID = responseObj.SystemWFSigDetail.APISysID;
                    APIControllerID = responseObj.SystemWFSigDetail.APIControllerID;
                    APIActionName = responseObj.SystemWFSigDetail.APIActionName;
                    if (responseObj.SystemWFSigDetail.CompareWFNodeID != null &&
                        responseObj.SystemWFSigDetail.CompareWFSigSeq != null)
                    {
                        CompareWFSigSeq = string.Format("{0}|{1}", responseObj.SystemWFSigDetail.CompareWFNodeID, responseObj.SystemWFSigDetail.CompareWFSigSeq);
                    }
                    ValidAPISysID = responseObj.SystemWFSigDetail.ChkAPISysID;
                    ValidAPIControllerID = responseObj.SystemWFSigDetail.ChkAPIControllerID;
                    ValidAPIActionName = responseObj.SystemWFSigDetail.ChkAPIActionName;
                    IsReq = responseObj.SystemWFSigDetail.IsReq;
                    Remark = responseObj.SystemWFSigDetail.Remark;
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

        #region - 編輯工作流程簽核明細 -

        /// <summary>
        /// 編輯工作流程簽核明細
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> EditSystemWFSig(string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                string apiSysID = (string.IsNullOrWhiteSpace(APISysID) ? null : APISysID);
                string apiControllerID = (string.IsNullOrWhiteSpace(APIControllerID) ? null : APIControllerID);
                string apiActionName = (string.IsNullOrWhiteSpace(APIActionName) ? null : APIActionName);

                if (apiSysID == null ||
                    apiControllerID == null ||
                    apiActionName == null ||
                    EnumSignatureTypeID.A.ToString() != SigType)
                {
                    apiSysID = null;
                    apiControllerID = null;
                    apiActionName = null;
                }

                string compareWFNodeID = (string.IsNullOrWhiteSpace(CompareWFSigSeq) ? null : CompareWFSigSeq.Split('|')[0]);
                string compareWFSigSeq = (string.IsNullOrWhiteSpace(CompareWFSigSeq) ? null : CompareWFSigSeq.Split('|')[1]);

                if (compareWFNodeID == null ||
                    compareWFSigSeq == null ||
                    EnumSignatureTypeID.C.ToString() != SigType)
                {
                    compareWFNodeID = null;
                    compareWFSigSeq = null;
                }

                string chkAPISysID = (string.IsNullOrWhiteSpace(ValidAPISysID) ? null : ValidAPISysID);
                string chkAPIControllerID = (string.IsNullOrWhiteSpace(ValidAPIControllerID) ? null : ValidAPIControllerID);
                string chkAPIActionName = (string.IsNullOrWhiteSpace(ValidAPIActionName) ? null : ValidAPIActionName);

                if (chkAPISysID == null ||
                    chkAPIControllerID == null ||
                    chkAPIActionName == null)
                {
                    chkAPISysID = null;
                    chkAPIControllerID = null;
                    chkAPIActionName = null;
                }

                SystemWFSignatureDetail para = new SystemWFSignatureDetail();
                para.SystemWFSigDetail = new SystemWFSigDetail()
                {
                    SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                    WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID,
                    WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer,
                    WFNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID,
                    WFSigSeq = string.IsNullOrWhiteSpace(WFSigSeq) ? null : WFSigSeq,
                    SigStep = (SigStep == 0) ? 1 : SigStep,
                    WFSigZHTW = string.IsNullOrWhiteSpace(WFSigZHTW) ? null : WFSigZHTW,
                    WFSigZHCN = string.IsNullOrWhiteSpace(WFSigZHCN) ? null : WFSigZHCN,
                    WFSigENUS = string.IsNullOrWhiteSpace(WFSigENUS) ? null : WFSigENUS,
                    WFSigTHTH = string.IsNullOrWhiteSpace(WFSigTHTH) ? null : WFSigTHTH,
                    WFSigJAJP = string.IsNullOrWhiteSpace(WFSigJAJP) ? null : WFSigJAJP,
                    WFSigKOKR = string.IsNullOrWhiteSpace(WFSigKOKR) ? null : WFSigKOKR,
                    SigType = string.IsNullOrWhiteSpace(SigType) ? null : SigType,
                    APISysID = apiSysID,
                    APIControllerID = apiControllerID,
                    APIActionName = apiActionName,
                    CompareWFNodeID = compareWFNodeID,
                    CompareWFSigSeq = compareWFSigSeq,
                    ChkAPISysID = chkAPISysID,
                    ChkAPIControllerID = chkAPIControllerID,
                    ChkAPIActionName = chkAPIActionName,
                    IsReq = (IsReq == EnumYN.Y.ToString() ? EnumYN.Y.ToString() : EnumYN.N.ToString()),
                    Remark = string.IsNullOrWhiteSpace(Remark) ? null : Remark,
                    UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID
                };

                List<SystemRoleSignatures> paraList = null;

                if (EnumSignatureTypeID.R.ToString() == SigType &&
                    HasRole != null &&
                    HasRole.Any())
                {
                    paraList =
                        HasRole
                            .Select(roleString =>
                    new SystemRoleSignatures()
                    {
                        SysID = string.IsNullOrWhiteSpace(roleString.Split('|')[0]) ? null : roleString.Split('|')[0],
                        RoleID = string.IsNullOrWhiteSpace(roleString.Split('|')[1]) ? null : roleString.Split('|')[1],
                        WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID,
                        WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer,
                        WFNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID,
                        WFSigSeq = string.IsNullOrWhiteSpace(WFSigSeq) ? null : WFSigSeq,
                        UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID
                    }).ToList();
                }

                para.SystemRoleSignatures = paraList;


                Mongo_BaseAP.EnumModifyType modifyType =
                        (ExecAction == EnumActionType.Add)
                            ? Mongo_BaseAP.EnumModifyType.I
                            : Mongo_BaseAP.EnumModifyType.U;

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                if (ExecAction == EnumActionType.Add)
                {
                    var paraJsonStr = Common.GetJsonSerializeObject(para);
                    var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                    string apiUrl = API.SystemWorkFlowSignature.InsertSystemWorkFlowSignatureDetail(userID);
                    await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
                }

                if (ExecAction == EnumActionType.Update)
                {
                    var paraJsonStr = Common.GetJsonSerializeObject(para);
                    var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                    string apiUrl = API.SystemWorkFlowSignature.EditSystemWorkFlowSignatureDetail(userID);
                    await PublicFun.HttpPutWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes);
                }
                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_WF_SIG, modifyType, userID, ipAddress, cultureID, para);
                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_ROLE_SIG, modifyType, userID, ipAddress, cultureID, paraList);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        #endregion

        #region - 刪除工作流程簽核明細 -
        /// <summary>
        /// 刪除工作流程簽核明細
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> DeleteSystemWFSig(string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                SystemWorkFlowSignatureDetail para = new SystemWorkFlowSignatureDetail()
                {
                    SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                    WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID,
                    WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer,
                    WFNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID,
                    WFSigSeq = string.IsNullOrWhiteSpace(WFSigSeq) ? null : WFSigSeq
                };

                SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID;
                WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer;
                WFNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID;
                WFSigSeq = string.IsNullOrWhiteSpace(WFSigSeq) ? null : WFSigSeq;

                string apiUrl = API.SystemWorkFlowSignature.QuerytSystemRoleSignatures(SysID, WFFlowID, WFFlowVer, WFNodeID, WFSigSeq, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SystemRoleSigList = (List<SystemRoleSig>)null
                };

                var roleSig = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                string url = API.SystemWorkFlowSignature.DeleteSystemWorkFlowSignatureDetail(userID, SysID, WFFlowID, WFFlowVer, WFNodeID, WFSigSeq);
                await Common.HttpWebRequestGetResponseStringAsync(url, AppSettings.APITimeOut, requestMethod: "DELETE");

                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_ROLE_SIG, Mongo_BaseAP.EnumModifyType.D, userID, ipAddress, cultureID, roleSig);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 事件-編輯 -
        internal DBEntity.DBTableRow GetEventParaSysSystemWFSignatureEdit()
        {
            return
                new EntityEventPara.SysSystemWFSignatureEdit
                {
                    TargetSysIDList = new List<DBVarChar> { new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID) },
                    SysID = new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID),
                    WFFlowID = new DBVarChar(string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID),
                    WFFlowVer = new DBChar(string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer),
                    WFNodeID = new DBVarChar(string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID),
                    WFSigSeq = new DBChar((string.IsNullOrWhiteSpace(WFSigSeq) ? null : WFSigSeq)),
                    SigStep = new DBInt((SigStep == 0) ? 1 : SigStep),
                    WFSigzhTW = new DBNVarChar(string.IsNullOrWhiteSpace(WFSigZHTW) ? null : WFSigZHTW),
                    WFSigzhCN = new DBNVarChar(string.IsNullOrWhiteSpace(WFSigZHCN) ? null : WFSigZHCN),
                    WFSigenUS = new DBNVarChar(string.IsNullOrWhiteSpace(WFSigENUS) ? null : WFSigENUS),
                    WFSigthTH = new DBNVarChar(string.IsNullOrWhiteSpace(WFSigTHTH) ? null : WFSigTHTH),
                    WFSigjaJP = new DBNVarChar(string.IsNullOrWhiteSpace(WFSigJAJP) ? null : WFSigJAJP),
                    WFSigkoKR = new DBNVarChar(string.IsNullOrWhiteSpace(WFSigKOKR) ? null : WFSigKOKR),
                    IsReq = new DBChar(IsReq == EnumYN.Y.ToString() ? EnumYN.Y.ToString() : EnumYN.N.ToString())
                };
        }
        #endregion

        #region - 事件-刪除 -
        internal DBEntity.DBTableRow GetEventParaSysSystemWFSignatureDelete()
        {
            return
                new EntityEventPara.SysSystemWFSignatureDelete
                {
                    TargetSysIDList = new List<DBVarChar> { new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID) },
                    SysID = new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID),
                    WFFlowID = new DBVarChar(string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID),
                    WFFlowVer = new DBChar(string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer),
                    WFNodeID = new DBVarChar(string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID),
                    WFSigSeq = new DBChar((string.IsNullOrWhiteSpace(WFSigSeq) ? null : WFSigSeq))
                };
        }
        #endregion
    }
}