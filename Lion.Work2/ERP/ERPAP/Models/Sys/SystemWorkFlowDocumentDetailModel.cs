using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.EDIService;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static LionTech.Entity.ERP.Sys.EntitySystemWorkFlowDocumentDetail;

namespace ERPAP.Models.Sys
{
    public class SystemWorkFlowDocumentDetailModel : SysModel
    {
        #region - Definitions -
        public class SystemWorkFlowDocumentDetail
        {
            public string SysID { get; set; }
            public string WFFlowID { get; set; }
            public string WFFlowVer { get; set; }
            public string WFNodeID { get; set; }
            public string WFDocSeq { get; set; }
            public string WFDocZHTW { get; set; }
            public string WFDocZHCN { get; set; }
            public string WFDocENUS { get; set; }
            public string WFDocTHTH { get; set; }
            public string WFDocJAJP { get; set; }
            public string WFDocKOKR { get; set; }
            public string IsReq { get; set; }
            public string Remark { get; set; }
        }
        #endregion

        #region - Constructor -
        public SystemWorkFlowDocumentDetailModel()
        {
            _entity = new EntitySystemWorkFlowDocumentDetail(ConnectionStringSERP, ProviderNameSERP);
            TabList = new List<TabStripHelper.Tab>
                      {
                          new TabStripHelper.Tab
                          {
                              ControllerName = string.Empty,
                              ActionName = string.Empty,
                              TabText = SysSystemWorkFlowDocumentDetail.TabText_SystemWorkFlowDocumentDetail,
                              ImageURL = string.Empty
                          }
                      };
        }
        #endregion

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

        public string WFDocSeq { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFDocZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFDocZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFDocENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFDocTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFDocJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFDocKOKR { get; set; }

        public string IsReq { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string Remark { get; set; }
        
        public readonly List<TabStripHelper.Tab> TabList;
        #endregion

        #region - Private -
        private readonly EntitySystemWorkFlowDocumentDetail _entity;
        #endregion

        #region - 表單初始 -
        /// <summary>
        /// 表單初始
        /// </summary>
        public void FormReset()
        {
            WFDocZHTW = null;
            WFDocZHCN = null;
            WFDocENUS = null;
            WFDocTHTH = null;
            WFDocJAJP = null;
            WFDocKOKR = null;
            IsReq = EnumYN.N.ToString();
            Remark = null;
        }
        #endregion

        #region - 取得工作流程文件檔 -
        /// <summary>
        /// 取得工作流程文件檔
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemWFDoc(EnumCultureID cultureID)
        {
            try
            {
                string sysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                string wfFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID;
                string wfFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer;
                string wfNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID;
                string wfDocSeq = string.IsNullOrWhiteSpace(WFDocSeq) ? null : WFDocSeq;

                string apiUrl = API.SystemWorkFlowDocument.QuerySystemWorkFlowDocumentDetail(sysID, wfFlowID, wfFlowVer, wfNodeID, wfDocSeq);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SystemWorkFlowDocumentDetail = (SystemWorkFlowDocumentDetail)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj.SystemWorkFlowDocumentDetail != null)
                {
                    WFDocSeq = responseObj.SystemWorkFlowDocumentDetail.WFDocSeq;
                    WFDocZHTW = responseObj.SystemWorkFlowDocumentDetail.WFDocZHTW;
                    WFDocZHCN = responseObj.SystemWorkFlowDocumentDetail.WFDocZHCN;
                    WFDocENUS = responseObj.SystemWorkFlowDocumentDetail.WFDocENUS;
                    WFDocTHTH = responseObj.SystemWorkFlowDocumentDetail.WFDocTHTH;
                    WFDocJAJP = responseObj.SystemWorkFlowDocumentDetail.WFDocJAJP;
                    WFDocKOKR = responseObj.SystemWorkFlowDocumentDetail.WFDocKOKR;
                    IsReq = responseObj.SystemWorkFlowDocumentDetail.IsReq;
                    Remark = responseObj.SystemWorkFlowDocumentDetail.Remark;
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

        #region - 新增工作流程文件檔 -
        /// <summary>
        /// 新增工作流程文件檔
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetInsertSystemWFDocResult(string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    SysID =  string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                    WFFlowID =  string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID,
                    WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer,
                    WFNodeID =  string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID,
                    WFDocZHTW = string.IsNullOrWhiteSpace(WFDocZHTW) ? null : WFDocZHTW,
                    WFDocZHCN = string.IsNullOrWhiteSpace(WFDocZHCN) ? null : WFDocZHCN,
                    WFDocENUS = string.IsNullOrWhiteSpace(WFDocENUS) ? null : WFDocENUS,
                    WFDocTHTH = string.IsNullOrWhiteSpace(WFDocTHTH) ? null : WFDocTHTH,
                    WFDocJAJP = string.IsNullOrWhiteSpace(WFDocJAJP) ? null : WFDocJAJP,
                    WFDocKOKR = string.IsNullOrWhiteSpace(WFDocKOKR) ? null : WFDocKOKR,
                    IsReq = string.IsNullOrWhiteSpace(IsReq) ? EnumYN.N.ToString() : EnumYN.Y.ToString(),
                    Remark = string.IsNullOrWhiteSpace(Remark) ? null : Remark,
                    UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID
                });

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.SystemWorkFlowDocument.InsertSystemWorkFlowDocument(userID);

                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                EntitySystemWorkFlowDocumentDetail.SystemWFDocPara logpara =
                    new EntitySystemWorkFlowDocumentDetail.SystemWFDocPara(cultureID.ToString())
                    {
                        SysID = new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID),
                        WFFlowID = new DBVarChar(string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID),
                        WFFlowVer = new DBChar(string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer),
                        WFNodeID = new DBVarChar(string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID),
                        WFDocZHTW = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocZHTW) ? null : WFDocZHTW),
                        WFDocZHCN = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocZHCN) ? null : WFDocZHCN),
                        WFDocENUS = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocENUS) ? null : WFDocENUS),
                        WFDocTHTH = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocTHTH) ? null : WFDocTHTH),
                        WFDocJAJP = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocJAJP) ? null : WFDocJAJP),
                        WFDocKOKR = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocKOKR) ? null : WFDocKOKR),
                        IsReq = new DBChar(string.IsNullOrWhiteSpace(IsReq) ? EnumYN.N.ToString() : EnumYN.Y.ToString()),
                        Remark = new DBNVarChar(string.IsNullOrWhiteSpace(Remark) ? null : Remark),
                        UpdUserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID)
                    };

                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_WF_DOC, Mongo_BaseAP.EnumModifyType.I, userID, ipAddress, cultureID, logpara);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 編輯工作流程文件檔 -
        /// <summary>
        /// 編輯工作流程文件檔
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetEditSystemWFDocResult(string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                    WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID,
                    WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer,
                    WFNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID,
                    WFDocSeq = string.IsNullOrWhiteSpace(WFDocSeq) ? null : WFDocSeq,
                    WFDocZHTW = string.IsNullOrWhiteSpace(WFDocZHTW) ? null : WFDocZHTW,
                    WFDocZHCN = string.IsNullOrWhiteSpace(WFDocZHCN) ? null : WFDocZHCN,
                    WFDocENUS = string.IsNullOrWhiteSpace(WFDocENUS) ? null : WFDocENUS,
                    WFDocTHTH = string.IsNullOrWhiteSpace(WFDocTHTH) ? null : WFDocTHTH,
                    WFDocJAJP = string.IsNullOrWhiteSpace(WFDocJAJP) ? null : WFDocJAJP,
                    WFDocKOKR = string.IsNullOrWhiteSpace(WFDocKOKR) ? null : WFDocKOKR,
                    IsReq = string.IsNullOrWhiteSpace(IsReq) ? EnumYN.N.ToString() : EnumYN.Y.ToString(),
                    Remark = string.IsNullOrWhiteSpace(Remark) ? null : Remark,
                    UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID
                });

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.SystemWorkFlowDocument.EditSystemWorkFlowDocument(userID);

                await PublicFun.HttpPutWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes);

                EntitySystemWorkFlowDocumentDetail.SystemWFDocPara logpara =
                    new EntitySystemWorkFlowDocumentDetail.SystemWFDocPara(cultureID.ToString())
                    {
                        SysID = new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID),
                        WFFlowID = new DBVarChar(string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID),
                        WFFlowVer = new DBChar(string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer),
                        WFNodeID = new DBVarChar(string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID),
                        WFDocSeq = new DBChar(string.IsNullOrWhiteSpace(WFDocSeq) ? null : WFDocSeq),
                        WFDocZHTW = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocZHTW) ? null : WFDocZHTW),
                        WFDocZHCN = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocZHCN) ? null : WFDocZHCN),
                        WFDocENUS = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocENUS) ? null : WFDocENUS),
                        WFDocTHTH = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocTHTH) ? null : WFDocTHTH),
                        WFDocJAJP = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocJAJP) ? null : WFDocJAJP),
                        WFDocKOKR = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocKOKR) ? null : WFDocKOKR),
                        IsReq = new DBChar(string.IsNullOrWhiteSpace(IsReq) ? EnumYN.N.ToString() : EnumYN.Y.ToString()),
                        Remark = new DBNVarChar(string.IsNullOrWhiteSpace(Remark) ? null : Remark),
                        UpdUserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID)
                    };

                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_WF_DOC, Mongo_BaseAP.EnumModifyType.U, userID, ipAddress, cultureID, logpara);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 刪除工作流程文件檔 -
        /// <summary>
        /// 刪除工作流程文件檔
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<EntitySystemWorkFlowDocumentDetail.EnumDeleteSystemWFDocResult> GetDeleteSystemWFDocResult(string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                string sysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                string wfFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID;
                string wfFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer;
                string wfNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID;
                string wfDocSeq = string.IsNullOrWhiteSpace(WFDocSeq) ? null : WFDocSeq;

                string apiUrl = API.SystemWorkFlowDocument.QuerySystemWorkFlowDocumentDetail(sysID, wfFlowID, wfFlowVer, wfNodeID, wfDocSeq);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SystemWorkFlowDocumentDetail = (SystemWFDoc)null
                };

                string success = EntitySystemWorkFlowDocumentDetail.EnumDeleteSystemWFDocResult.Success.ToString();
                string runtimeExist = EntitySystemWorkFlowDocumentDetail.EnumDeleteSystemWFDocResult.RuntimeExist.ToString();

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                string delapiUrl = API.SystemWorkFlowDocument.DeleteSystemWorkFlowDocument(userID,sysID, wfFlowID, wfFlowVer, wfNodeID, wfDocSeq);
                string delresponse = await Common.HttpWebRequestGetResponseStringAsync(delapiUrl, AppSettings.APITimeOut,"DELETE");

                if(delresponse == success)
                {
                    RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_WF_DOC, Mongo_BaseAP.EnumModifyType.D, userID, ipAddress, cultureID, responseObj.SystemWorkFlowDocumentDetail);
                    return EntitySystemWorkFlowDocumentDetail.EnumDeleteSystemWFDocResult.Success;
                }
                else if (delresponse == runtimeExist)
                {
                    return EntitySystemWorkFlowDocumentDetail.EnumDeleteSystemWFDocResult.RuntimeExist;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return EntitySystemWorkFlowDocumentDetail.EnumDeleteSystemWFDocResult.Failure;
        }
        #endregion

        #region - 事件-編輯 -
        internal DBEntity.DBTableRow GetEventParaSysSystemWFDocumentEdit()
        {
            return
                new EntityEventPara.SysSystemWFDocumentEdit
                {
                    TargetSysIDList = new List<DBVarChar> { new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID) },
                    SysID = new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID),
                    WFFlowID = new DBVarChar(string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID),
                    WFNodeID = new DBVarChar(string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID),
                    WFDocSeq = new DBChar(string.IsNullOrWhiteSpace(WFDocSeq) ? null : WFDocSeq),
                    WFDoczhTW = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocZHTW) ? null : WFDocZHTW),
                    WFDoczhCN = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocZHCN) ? null : WFDocZHCN),
                    WFDocenUS = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocENUS) ? null : WFDocENUS),
                    WFDocthTH = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocTHTH) ? null : WFDocTHTH),
                    WFDocjaJP = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocJAJP) ? null : WFDocJAJP),
                    WFDockoKR = new DBNVarChar(string.IsNullOrWhiteSpace(WFDocKOKR) ? null : WFDocKOKR),
                    WFFlowVer = new DBChar(string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer),
                    IsReq = new DBChar(string.IsNullOrWhiteSpace(IsReq) ? EnumYN.N.ToString() : EnumYN.Y.ToString())
                };
        }
        #endregion

        #region - 事件-刪除 -
        internal DBEntity.DBTableRow GetEventParaSysSystemWFDocumentDelete()
        {
            return
                new EntityEventPara.SysSystemWFDocumentDelete
                {
                    TargetSysIDList = new List<DBVarChar> { new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID) },
                    SysID = new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID),
                    WFFlowID = new DBVarChar(string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID),
                    WFNodeID = new DBVarChar(string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID),
                    WFFlowVer = new DBChar(string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer),
                    WFDocSeq = new DBChar(string.IsNullOrWhiteSpace(WFDocSeq) ? null : WFDocSeq)
                };
        }
        #endregion
    }
}