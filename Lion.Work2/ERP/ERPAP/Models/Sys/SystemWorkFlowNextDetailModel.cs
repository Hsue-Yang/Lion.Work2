using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using LionTech.WorkFlow;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static LionTech.Entity.DBEntity;

namespace ERPAP.Models.Sys
{
    public class SystemWorkFlowNextDetailModel : SysModel, IValidatableObject
    {
        #region - Definitions -
        public class SystemWFNext : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
            public DBVarChar NextWFNodeID;

            public DBNVarChar NextWFNodeNM;
            public DBVarChar NextResultValue;
            public DBVarChar SortOrder;
            public DBNVarChar Remark;
        }

        #endregion
        #region - Constructor -
        public SystemWorkFlowNextDetailModel()
        {
            _entity = new EntitySystemWorkFlowNextDetail(ConnectionStringSERP, ProviderNameSERP);
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

        [Required]
        public string NextWFNodeID { get; set; }

        public string NextWFNodeNM { get; set; }

        [StringLength(50)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string NextResultValue { get; set; }

        [StringLength(6)]
        [InputType(EnumInputType.TextBox)]
        public string SortOrder { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string Remark { get; set; }

        public List<EntitySystemWorkFlowNextDetail.SystemWFNode> EntitySystemWFNodeList { get; private set; }
        #endregion

        #region - Private -
        private readonly EntitySystemWorkFlowNextDetail _entity;
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
        /// <summary>
        /// 表單初始
        /// </summary>
        public void FormReset()
        {
            NextResultValue = string.Empty;
            SortOrder = string.Empty;
            Remark = string.Empty;
        }
        #endregion
        
        #region - 取得工作流程節點清單 -
        /// <summary>
        /// 取得工作流程節點清單
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemWFNodeList(EnumCultureID cultureID)
        {
            try
            {
                string sysID = (string.IsNullOrWhiteSpace(SysID) ? null : SysID);
                string wfFlowID = (string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID);
                string wfFlowVer = (string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer);
                string wfNodeID = (string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID);
                List<string> nodeTypeList = new List<string> { EnumNodeType.P.ToString() };

                if (EntitySystemWorkFlowNode.NodeType == EnumNodeType.P.ToString())
                {
                    nodeTypeList.Add(EnumNodeType.D.ToString());
                    nodeTypeList.Add(EnumNodeType.E.ToString());
                }
                else if (EntitySystemWorkFlowNode.NodeType == EnumNodeType.D.ToString())
                {
                    nodeTypeList.Add(EnumNodeType.E.ToString());
                }

                string nodeTypeListstr = $"{string.Join("|", nodeTypeList)}";

                string apiUrl = API.SystemWorkFlowNext.QuerySystemWFNodeList(sysID, wfFlowID, wfFlowVer, wfNodeID, nodeTypeListstr, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SystemWFNodeList = (List<EntitySystemWorkFlowNextDetail.SystemWFNode>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                EntitySystemWFNodeList = responseObj.SystemWFNodeList;

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion
        
        #region - 取得工作流程次節點檔明細 -
        /// <summary>
        /// 取得工作流程次節點檔明細
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemWFNext(EnumCultureID cultureID)
        {
            try
            {
                string sysID = (string.IsNullOrWhiteSpace(SysID) ? null : SysID);
                string wfFlowID = (string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID);
                string wfFlowVer = (string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer);
                string wfNodeID = (string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID);
                string nextWFNodeID = (string.IsNullOrWhiteSpace(NextWFNodeID) ? null : NextWFNodeID);

                string apiUrl = API.SystemWorkFlowNext.QuerySystemWFNext(sysID, wfFlowID, wfFlowVer, wfNodeID, nextWFNodeID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SystemWFNext = (SystemWFNext)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                var data = responseObj.SystemWFNext;

                if (data != null)
                {
                    NextWFNodeNM = (data.NextWFNodeNM == null ? null : data.NextWFNodeNM.GetValue());
                    NextResultValue = (data.NextResultValue == null ? null : data.NextResultValue.GetValue());
                    SortOrder = (data.SortOrder == null ? null : data.SortOrder.GetValue());
                    Remark = (data.Remark == null ? null : data.Remark.GetValue());
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
        
        #region - 編輯工作流程次節點檔明細 -
        /// <summary>
        /// 編輯工作流程次節點檔明細
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetEditSystemWFNext(string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                var paraJsonStr = Common.GetJsonSerializeObject(
                    new
                    {
                        SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                        WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID,
                        WFFlowVer = string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer,
                        WFNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID,
                        NextWFNodeID = string.IsNullOrWhiteSpace(NextWFNodeID) ? null : NextWFNodeID,
                        NextResultValue = string.IsNullOrWhiteSpace(NextResultValue) ? null : NextResultValue,
                        SortOrder = string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder,
                        Remark = string.IsNullOrWhiteSpace(Remark) ? null : Remark,
                        UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID
                    });
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.SystemWorkFlowNext.EditSystemWFNext(userID);
                var result  = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                EntitySystemWorkFlowNextDetail.SystemWFNextPara logPara =
                new EntitySystemWorkFlowNextDetail.SystemWFNextPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(SysID) ? null : SysID)),
                    WFFlowID = new DBVarChar((string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID)),
                    WFFlowVer = new DBChar((string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer)),
                    WFNodeID = new DBVarChar((string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID)),
                    NextWFNodeID = new DBVarChar((string.IsNullOrWhiteSpace(NextWFNodeID) ? null : NextWFNodeID)),
                    NextResultValue = new DBVarChar((string.IsNullOrWhiteSpace(NextResultValue) ? null : NextResultValue)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder)),
                    Remark = new DBNVarChar((string.IsNullOrWhiteSpace(Remark) ? null : Remark)),
                    UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                Mongo_BaseAP.EnumModifyType modifyType =
                    (ExecAction == EnumActionType.Add)
                        ? Mongo_BaseAP.EnumModifyType.I
                        : Mongo_BaseAP.EnumModifyType.U;

                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_WF_NEXT, modifyType, userID, ipAddress, cultureID, logPara);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion
        
        #region - 刪除工作流程次節點檔明細 -
        /// <summary>
        /// 刪除工作流程次節點檔明細
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetDeleteSystemWFNext(string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                string sysID = (string.IsNullOrWhiteSpace(SysID) ? null : SysID);
                string wfFlowID = (string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID);
                string wfFlowVer = (string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer);
                string wfNodeID = (string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID);
                string nextWFNodeID = (string.IsNullOrWhiteSpace(NextWFNodeID) ? null : NextWFNodeID);

                string apiUrl = API.SystemWorkFlowNext.QuerySystemWFNext(sysID, wfFlowID, wfFlowVer, wfNodeID, nextWFNodeID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SystemWFNext = (SystemWFNext)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                var data = responseObj.SystemWFNext;

                string delApiUrl = API.SystemWorkFlowNext.DeleteSystemWFNext(userID, sysID, wfFlowID, wfFlowVer, wfNodeID, nextWFNodeID);
                await Common.HttpWebRequestGetResponseStringAsync(delApiUrl, AppSettings.APITimeOut, "DELETE");

                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_WF_NEXT, Mongo_BaseAP.EnumModifyType.D, userID, ipAddress, cultureID, data);
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