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
using LionTech.WorkFlow;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemWorkFlowNodeDetailModel : SysModel, IValidatableObject
    {
        #region - Constructor -
        public SystemWorkFlowNodeDetailModel()
        {
            _entity = new EntitySystemWorkFlowNodeDetail(ConnectionStringSERP, ProviderNameSERP);
            TabList = new List<TabStripHelper.Tab>
                      {
                          new TabStripHelper.Tab
                          {
                              ControllerName = string.Empty,
                              ActionName = string.Empty,
                              TabText = SysSystemWorkFlowNodeDetail.TabText_SystemWorkFlowNodeDetail,
                              ImageURL = string.Empty
                          }
                      };
        }
        #endregion

        #region -  -

        public class SystemWorkFlow
        {
            public string SysNM { get; set; }
            public string WFFlowNM { get; set; }
        }

        public class SystemWorkFlowNodeRole
        {
            public string SysID { get; set; }
            public string RoleID { get; set; }
            public string RoleNM { get; set; }
            public string HasRole { get; set; }
        }

        public class SystemWorkFlowNodeDetailExecuteResult
        {
            public string SysID { get; set; }
            public string WFFlowID { get; set; }
            public string WFFlowVer { get; set; }

            public string WFNodeID { get; set; }
            public string WFNodeZHTW { get; set; }
            public string WFNodeZHCN { get; set; }
            public string WFNodeENUS { get; set; }
            public string WFNodeTHTH { get; set; }
            public string WFNodeJAJP { get; set; }
            public string WFNodeKOKR { get; set; }

            public string NodeType { get; set; }
            public int NodeSeqX { get; set; }
            public int NodeSeqY { get; set; }

            public int NodePosBeginX { get; set; }
            public int NodePosBeginY { get; set; }
            public int NodePosEndX { get; set; }
            public int NodePosEndY { get; set; }

            public string IsFirst { get; set; }
            public string IsFinally { get; set; }
            public string BackWFNodeID { get; set; }

            public string WFSigMemoZHTW { get; set; }
            public string WFSigMemoZHCN { get; set; }
            public string WFSigMemoENUS { get; set; }
            public string WFSigMemoTHTH { get; set; }
            public string WFSigMemoJAJP { get; set; }
            public string WFSigMemoKOKR { get; set; }

            public string FunSysID { get; set; }
            public string FunControllerID { get; set; }
            public string FunActionName { get; set; }

            public string SigApiSysID { get; set; }
            public string SigApiControllerID { get; set; }
            public string SigApiActionName { get; set; }
            public string ChkApiSysID { get; set; }
            public string ChkApiControllerID { get; set; }
            public string ChkApiActionName { get; set; }

            public string AssgAPISysID { get; set; }
            public string AssgAPIControllerID { get; set; }
            public string AssgAPIActionName { get; set; }

            public string IsSigNextNode { get; set; }
            public string IsSigBackNode { get; set; }

            public string IsAssgNextNode { get; set; }

            public string SortOrder { get; set; }
            public string Remark { get; set; }
        }

        public enum EnumDeleteSystemWorkFlowNodeDetailResult
        {
            Success,
            Failure
        }

        public class SystemWorkFlowNodePara
        {
            public SystemWorkFlowNodeDetailPara SystemWorkFlowNodeDetailPara { get; set; }
            public List<SystemWorkFlowNodeRolePara> SystemWorkFlowNodeRoleParas { get; set; }
        }

        public class SystemWorkFlowNodeDetailPara
        {
            public string SysID { get; set; }
            public string WFFlowID { get; set; }
            public string WFFlowVer { get; set; }

            public string WFNodeID { get; set; }
            public string WFNodeZHTW { get; set; }
            public string WFNodeZHCN { get; set; }
            public string WFNodeENUS { get; set; }
            public string WFNodeTHTH { get; set; }
            public string WFNodeJAJP { get; set; }
            public string WFNodeKOKR { get; set; }

            public string NodeType { get; set; }
            public int NodeSeqX { get; set; }
            public int NodeSeqY { get; set; }

            public int NodePosBeginX { get; set; }
            public int NodePosBeginY { get; set; }
            public int NodePosEndX { get; set; }
            public int NodePosEndY { get; set; }

            public string IsFirst { get; set; }
            public string IsFinally { get; set; }
            public string BackWFNodeID { get; set; }

            public string FunSysID { get; set; }
            public string FunControllerID { get; set; }
            public string FunActionName { get; set; }
            public string AssgAPISysID { get; set; }
            public string AssgAPIControllerID { get; set; }
            public string AssgAPIActionName { get; set; }

            public string IsAssgNextNode { get; set; }

            public string Remark { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserID { get; set; }
        }

        public class SystemWorkFlowNodeRolePara
        {
            public string SysID { get; set; }
            public string RoleID { get; set; }
            public string WFFlowID { get; set; }
            public string WFFlowVer { get; set; }

            public string WFNodeID { get; set; }
            public string UpdUserID { get; set; }
        }


        #endregion

        #region - Property -
        [Required]
        public string SysID { get; set; }

        public string SysNM { get; private set; }

        [Required]
        public string WFFlowGroupID { get; set; }

        public string WFFlowNM { get; private set; }

        [Required]
        public string WFFlowID { get; set; }

        [Required]
        public string WFFlowVer { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string WFNodeID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFNodeZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFNodeZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFNodeENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFNodeTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFNodeJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string WFNodeKOKR { get; set; }

        [Required]
        public string NodeType { get; set; }

        [Required]
        [StringLength(2)]
        [InputType(EnumInputType.TextBoxInteger)]
        public string NodeSeqX { get; set; }

        [Required]
        [StringLength(2)]
        [InputType(EnumInputType.TextBoxInteger)]
        public string NodeSeqY { get; set; }

        public string IsFirst { get; set; }

        public string IsFinally { get; set; }

        public string BackWFNodeID { get; set; }

        public string FunSysID { get; set; }

        public string FunControllerID { get; set; }

        public string FunActionName { get; set; }

        public string APIControllerID { get; set; }

        public string APIActionName { get; set; }

        public string AssgAPISysID { get; set; }

        public string AssgAPIControllerID { get; set; }

        public string AssgAPIActionName { get; set; }

        public string IsAssgNextNode { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string Remark { get; set; }

        [StringLength(6)]
        [InputType(EnumInputType.TextBox)]
        public string SortOrder { get; set; }

        public List<string> HasRole { get; set; }

        public List<TabStripHelper.Tab> TabList { get; set; }

        public List<UserSystemSysID> EntityAssgSysUserSystemSysIDList { get; set; }
        public List<SysModel.SysSystemAPIGroup> EntityAssgSysSystemAPIGroupList { get; set; }
        public List<SysModel.SystemAPIFuntions> EntityAssgSysSystemAPIFuntionList { get; set; }
        public List<SystemWorkFlowNode> BackSystemWorkFlowNodeList { get; private set; }
        public List<SystemWorkFlowNodeRole> SystemWorkFlowNodeRoleList { get; private set; }
        #endregion

        #region - Private -
        private readonly EntitySystemWorkFlowNodeDetail _entity;
        #endregion

        #region - Validation -
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsValid == false)
            {
                yield break;
            }

            List<ValidationResult> validationResults = new List<ValidationResult>();

            if ((ExecAction == EnumActionType.Add ||
                 ExecAction == EnumActionType.Update) &&
                _IsValidSystemWFNodeDetail(validationResults) == false)
            {
                yield return validationResults.FirstOrDefault();
            }
            else if (ExecAction == EnumActionType.Delete)
            {
                string apiUrl = API.SystemWorkFlowNodeDetail.CheckWorkFlowChildsIsExist(SysID, WFFlowID, WFFlowVer, WFNodeID);
                string response = Common.HttpWebRequestGetResponseString(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    IsExist = new bool()
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                string checkRunTimeUrl = API.SystemWorkFlowNodeDetail.CheckWorkFlowHasRunTime(SysID, WFFlowID, WFFlowVer);
                string checkRunTimeResponse = Common.HttpWebRequestGetResponseString(checkRunTimeUrl, AppSettings.APITimeOut);

                var isRunTime = new
                {
                    IsRunTime = new bool()
                };

                isRunTime = Common.GetJsonDeserializeAnonymousType(checkRunTimeResponse, isRunTime);
                if (responseObj.IsExist)
                {
                    yield return new ValidationResult(SysSystemWorkFlowNodeDetail.DeleteSystemWorkFlowNodeDetailResult_ChildExist);
                }
                else if (isRunTime.IsRunTime)
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
            WFNodeID = null;
            WFNodeZHTW = null;
            WFNodeZHCN = null;
            WFNodeENUS = null;
            WFNodeTHTH = null;
            WFNodeJAJP = null;
            WFNodeKOKR = null;
            NodeType = null;
            IsFirst = null;
            IsFinally = null;
            BackWFNodeID = null;
            FunSysID = null;
            FunControllerID = null;
            FunActionName = null;
            APIControllerID = null;
            APIActionName = null;
            AssgAPISysID = null;
            AssgAPIControllerID = null;
            AssgAPIActionName = null;
            IsAssgNextNode = EnumYN.N.ToString();
            Remark = null;
            SortOrder = null;
            HasRole = null;
        }
        #endregion

        #region - 取得工作流程資訊 -
        /// <summary>
        /// 取得工作流程資訊
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemWFFlowName(EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemWorkFlowNodeDetail.QuerySystemWorkFlowName(SysID, WFFlowID, WFFlowVer, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new SystemWorkFlow());

                if (responseObj != null)
                {
                    SysNM = responseObj.SysNM;
                    WFFlowNM = responseObj.WFFlowNM;
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

        #region - 取得退回節點清單 -
        /// <summary>
        /// 取得退回節點清單
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetBackSystemWFNodeList(EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemWorkFlowNodeDetail.QueryBackSystemWorkFlowNodes(SysID, WFFlowID, WFFlowVer, WFNodeID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new List<SystemWorkFlowNode>());

                BackSystemWorkFlowNodeList = responseObj;

                if (BackSystemWorkFlowNodeList != null)
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

        #region - 取得工作流程節點角色清單 -
        /// <summary>
        /// 取得工作流程節點角色清單
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemWFNodeRoleList(EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemWorkFlowNodeDetail.QuerySystemWorkFlowNodeRoles(SysID, WFFlowID, WFFlowVer, WFNodeID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new List<SystemWorkFlowNodeRole>());

                SystemWorkFlowNodeRoleList = responseObj;

                if (SystemWorkFlowNodeRoleList != null)
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

        #region - 取得工作流程節點明細 -
        /// <summary>
        /// 取得工作流程節點明細
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetSystemWFNodeDetail()
        {
            try
            {
                string apiUrl = API.SystemWorkFlowNodeDetail.QuerySystemWorkFlowNode(SysID, WFFlowID, WFFlowVer, WFNodeID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new SystemWorkFlowNodeDetailExecuteResult());

                if (responseObj != null)
                {
                    SysID = responseObj.SysID;
                    WFFlowID = responseObj.WFFlowID;
                    WFFlowVer = responseObj.WFFlowVer;
                    WFNodeID = responseObj.WFNodeID;

                    WFNodeZHTW = responseObj.WFNodeZHTW;
                    WFNodeZHCN = responseObj.WFNodeZHCN;
                    WFNodeENUS = responseObj.WFNodeENUS;
                    WFNodeTHTH = responseObj.WFNodeTHTH;
                    WFNodeJAJP = responseObj.WFNodeJAJP;
                    WFNodeKOKR = responseObj.WFNodeKOKR;

                    NodeType = responseObj.NodeType;
                    NodeSeqX = responseObj.NodeSeqX.ToString();
                    NodeSeqY = responseObj.NodeSeqY.ToString();

                    IsFirst = responseObj.IsFirst;
                    IsFinally = responseObj.IsFinally;
                    BackWFNodeID = responseObj.BackWFNodeID;
                    FunSysID = responseObj.FunSysID;

                    FunControllerID = null;
                    FunActionName = null;
                    APIControllerID = null;
                    APIActionName = null;

                    AssgAPISysID = responseObj.AssgAPISysID;
                    AssgAPIControllerID = responseObj.AssgAPIControllerID;
                    AssgAPIActionName = responseObj.AssgAPIActionName;

                    IsAssgNextNode = (responseObj.IsAssgNextNode == EnumYN.Y.ToString()) ? EnumYN.Y.ToString() : null;

                    EnumNodeType nodeType = (EnumNodeType)Enum.Parse(typeof(EnumNodeType), NodeType);

                    if (nodeType == EnumNodeType.D)
                    {
                        FunControllerID = null;
                        FunActionName = null;
                        APIControllerID = responseObj.FunControllerID;
                        APIActionName = responseObj.FunActionName;
                    }
                    else if (new[] { EnumNodeType.S, EnumNodeType.P, EnumNodeType.E }.Contains(nodeType))
                    {
                        FunControllerID = responseObj.FunControllerID;
                        FunActionName = responseObj.FunActionName;
                        APIControllerID = null;
                        APIActionName = null;
                    }

                    Remark = responseObj.Remark;
                    SortOrder = responseObj.SortOrder;

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

        #region - 驗證工作流程節點明細 -
        /// <summary>
        /// 驗證工作流程節點明細
        /// </summary>
        /// <param name="validationResults"></param>
        /// <returns></returns>
        private bool _IsValidSystemWFNodeDetail(List<ValidationResult> validationResults)
        {
            try
            {
                string apiUrl = API.SystemWorkFlowNodeDetail.QuerySystemWorkFlowNodes(SysID, WFFlowID, WFFlowVer);
                string response = Common.HttpWebRequestGetResponseString(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new List<SystemWorkFlowNodeDetailExecuteResult>());

                if (ExecAction == EnumActionType.Add)
                {
                    if (responseObj.Any(e => e.WFNodeID == WFNodeID))
                    {
                        validationResults.Add(new ValidationResult(SysSystemWorkFlowNodeDetail.SystemMsg_EditSystemWorkFlowNodeDetailResult_NodeIDRepeat));
                    }
                    else if (responseObj.Any(e => e.NodeSeqX.ToString() == NodeSeqX && e.NodeSeqY.ToString() == NodeSeqY))
                    {
                        validationResults.Add(new ValidationResult(SysSystemWorkFlowNodeDetail.SystemMsg_EditSystemWorkFlowNodeDetailResult_NodeSeqRepeat));
                    }
                    else if (NodeType == EnumNodeType.S.ToString() &&
                             responseObj.Any(e => e.NodeType == EnumNodeType.S.ToString()))
                    {
                        validationResults.Add(new ValidationResult(SysSystemWorkFlowNodeDetail.SystemMsg_EditSystemWorkFlowNodeDetailResult_NodeTypeSRepeat));
                    }
                    else if (NodeType == EnumNodeType.E.ToString() &&
                             responseObj.Any(e => e.NodeType == EnumNodeType.E.ToString()))
                    {
                        validationResults.Add(new ValidationResult(SysSystemWorkFlowNodeDetail.SystemMsg_EditSystemWorkFlowNodeDetailResult_NodeTypeERepeat));
                    }
                }
                else if (ExecAction == EnumActionType.Update)
                {
                    if (responseObj.Any(e => e.WFNodeID != WFNodeID && e.NodeSeqX.ToString() == NodeSeqX && e.NodeSeqY.ToString() == NodeSeqY))
                    {
                        validationResults.Add(new ValidationResult(SysSystemWorkFlowNodeDetail.SystemMsg_EditSystemWorkFlowNodeDetailResult_NodeSeqRepeat));
                    }
                }
            }
            catch (Exception ex)
            {
                validationResults.Add(new ValidationResult(SysSystemWorkFlowNodeDetail.SystemMsg_EditSystemWorkFlowNodeDetailResult_Failure));
                OnException(ex);
            }
            return validationResults.Any() == false;
        }
        #endregion

        #region - 取得編輯工作流程節點明細 -
        /// <summary>
        /// 取得編輯工作流程節點明細
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetEditSystemWFNodeDetail(string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                EnumNodeType nodeType = (EnumNodeType)Enum.Parse(typeof(EnumNodeType), NodeType);

                string newBackWFNodeID = string.Empty;

                string newFunSysID = string.Empty;
                string newControllerID = string.Empty;
                string newActionName = string.Empty;

                string newIsFirst = string.Empty;
                string newIsFinally = string.Empty;

                List<string> newHasRole = null;

                string assgAPISysID = (string.IsNullOrWhiteSpace(AssgAPISysID) ? null : AssgAPISysID);
                string assgAPIControllerID = (string.IsNullOrWhiteSpace(AssgAPIControllerID) ? null : AssgAPIControllerID);
                string assgAPIActionName = (string.IsNullOrWhiteSpace(AssgAPIActionName) ? null : AssgAPIActionName);

                if (assgAPISysID == null ||
                    assgAPIControllerID == null ||
                    assgAPIActionName == null)
                {
                    assgAPISysID = null;
                    assgAPIControllerID = null;
                    assgAPIActionName = null;
                }

                if (new[] { EnumNodeType.S, EnumNodeType.P, EnumNodeType.E }.Contains(nodeType))
                {
                    newFunSysID = FunSysID;
                    newControllerID = FunControllerID;
                    newActionName = FunActionName;
                    newHasRole = nodeType == EnumNodeType.E ? null : HasRole;

                    if (nodeType == EnumNodeType.P)
                    {
                        newBackWFNodeID = BackWFNodeID;
                        newIsFirst = IsFirst;
                        newIsFinally = IsFinally;
                    }
                }
                else if (nodeType == EnumNodeType.D)
                {
                    newFunSysID = FunSysID;
                    newControllerID = APIControllerID;
                    newActionName = APIActionName;
                    newHasRole = HasRole;
                }


                List<SystemWorkFlowNodeRolePara> roleList = new List<SystemWorkFlowNodeRolePara>();

                if (newHasRole != null &&
                    newHasRole.Any())
                {
                    roleList =
                        (from roleString in newHasRole
                         select new SystemWorkFlowNodeRolePara()
                         {
                             SysID = (string.IsNullOrWhiteSpace(roleString.Split('|')[0]) ? null : roleString.Split('|')[0]),
                             RoleID = (string.IsNullOrWhiteSpace(roleString.Split('|')[1]) ? null : roleString.Split('|')[1]),
                             WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID,
                             WFFlowVer =  string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer,
                             WFNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID,
                             UpdUserID =  string.IsNullOrWhiteSpace(userID) ? null : userID
                         }).ToList();
                }


                SystemWorkFlowNodePara para =
                    new SystemWorkFlowNodePara
                    {
                        SystemWorkFlowNodeDetailPara = new SystemWorkFlowNodeDetailPara
                        {
                            SysID =  SysID,
                            WFFlowID = string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID,
                            WFFlowVer =  string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer,
                            WFNodeID = string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID,
                            WFNodeZHTW =  string.IsNullOrWhiteSpace(WFNodeZHTW) ? null : WFNodeZHTW,
                            WFNodeZHCN =  string.IsNullOrWhiteSpace(WFNodeZHCN) ? null : WFNodeZHCN,
                            WFNodeENUS = string.IsNullOrWhiteSpace(WFNodeENUS) ? null : WFNodeENUS,
                            WFNodeTHTH =  string.IsNullOrWhiteSpace(WFNodeTHTH) ? null : WFNodeTHTH,
                            WFNodeJAJP =  string.IsNullOrWhiteSpace(WFNodeJAJP) ? null : WFNodeJAJP,
                            WFNodeKOKR =  string.IsNullOrWhiteSpace(WFNodeKOKR) ? null : WFNodeKOKR,
                            NodeType =  string.IsNullOrWhiteSpace(NodeType) ? null : NodeType,
                            NodeSeqX = int.Parse(NodeSeqX),
                            NodeSeqY = int.Parse(NodeSeqY),
                            IsFirst = (string.IsNullOrWhiteSpace(newIsFirst) ? EnumYN.N.ToString() : EnumYN.Y.ToString()),
                            IsFinally = (string.IsNullOrWhiteSpace(newIsFinally) ? EnumYN.N.ToString() : EnumYN.Y.ToString()),
                            BackWFNodeID =  string.IsNullOrWhiteSpace(newBackWFNodeID) ? null : newBackWFNodeID,
                            FunSysID =  string.IsNullOrWhiteSpace(newFunSysID) ? null : newFunSysID,
                            FunControllerID =  string.IsNullOrWhiteSpace(newControllerID) ? null : newControllerID,
                            FunActionName =  string.IsNullOrWhiteSpace(newActionName) ? null : newActionName,
                            Remark = string.IsNullOrWhiteSpace(Remark) ? null : Remark,
                            AssgAPISysID = string.IsNullOrWhiteSpace(assgAPISysID) ? null : assgAPISysID,
                            AssgAPIControllerID = string.IsNullOrWhiteSpace(assgAPIControllerID) ? null : assgAPIControllerID,
                            AssgAPIActionName = string.IsNullOrWhiteSpace(assgAPIActionName) ? null : assgAPIActionName,
                            IsAssgNextNode =(string.IsNullOrWhiteSpace(IsAssgNextNode) ? EnumYN.N.ToString() : EnumYN.Y.ToString()),
                            SortOrder =  string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder,
                            UpdUserID =  string.IsNullOrWhiteSpace(userID) ? null : userID
                        },
                        SystemWorkFlowNodeRoleParas = roleList
                    };

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};


                var paraJsonStr = Common.GetJsonSerializeObject(para);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemWorkFlowNodeDetail.EditSystemWorkFlowNodeDetail(userID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new SystemWorkFlowNodeDetailExecuteResult());

                if (responseObj != null)
                {
                    Mongo_BaseAP.EnumModifyType modifyType =
                        (ExecAction == EnumActionType.Add)
                            ? Mongo_BaseAP.EnumModifyType.I
                            : Mongo_BaseAP.EnumModifyType.U;

                    RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_WF_NODE, modifyType, userID, ipAddress, cultureID, responseObj);
                    RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_ROLE_NODE, modifyType, userID, ipAddress, cultureID, roleList);
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

        #region - 取得刪除工作節點明細 -
        /// <summary>
        /// 取得刪除工作節點明細
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetDeleteSystemWFNodeDetail(string userID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemWorkFlowNodeDetail.QuerySystemWorkFlowNode(SysID, WFFlowID, WFFlowVer, WFNodeID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new SystemWorkFlowNodeDetailExecuteResult());

                string apiDelUrl = API.SystemWorkFlowNodeDetail.DeleteSystemWorkFlowNodeDetail(userID,SysID, WFFlowID, WFFlowVer, WFNodeID);
                await Common.HttpWebRequestGetResponseStringAsync(apiDelUrl, AppSettings.APITimeOut, "DELETE");

                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_WF_NODE, Mongo_BaseAP.EnumModifyType.D, userID, ipAddress, cultureID, responseObj);
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
        internal DBEntity.DBTableRow GetEventParaSysSystemWFNodeEdit()
        {
            return
                new EntityEventPara.SysSystemWFNodeEdit
                {
                    TargetSysIDList = new List<DBVarChar> { new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID) },
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(SysID) ? null : SysID)),
                    WFFlowID = new DBVarChar((string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID)),
                    WFFlowVer = new DBChar((string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer)),
                    WFNodeID = new DBVarChar((string.IsNullOrWhiteSpace(WFNodeID) ? null : WFNodeID)),
                    WFNodezhTW = new DBNVarChar((string.IsNullOrWhiteSpace(WFNodeZHTW) ? null : WFNodeZHTW)),
                    WFNodezhCN = new DBNVarChar((string.IsNullOrWhiteSpace(WFNodeZHCN) ? null : WFNodeZHCN)),
                    WFNodeenUS = new DBNVarChar((string.IsNullOrWhiteSpace(WFNodeENUS) ? null : WFNodeENUS)),
                    WFNodethTH = new DBNVarChar((string.IsNullOrWhiteSpace(WFNodeTHTH) ? null : WFNodeTHTH)),
                    WFNodejaJP = new DBNVarChar((string.IsNullOrWhiteSpace(WFNodeJAJP) ? null : WFNodeJAJP)),
                    WFNodekoKR = new DBNVarChar((string.IsNullOrWhiteSpace(WFNodeKOKR) ? null : WFNodeKOKR)),
                    NodeType = new DBVarChar((string.IsNullOrWhiteSpace(NodeType) ? null : NodeType)),
                    SortOrder = new DBVarChar((string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder))
                };
        }
        #endregion

        #region - 事件-刪除 -
        internal DBEntity.DBTableRow GetEventParaSysSystemWFNodeDelete()
        {
            return
                new EntityEventPara.SysSystemWFNodeDelete
                {
                    TargetSysIDList = new List<DBVarChar> { new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID) },
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(SysID) ? null : SysID)),
                    WFFlowID = new DBVarChar((string.IsNullOrWhiteSpace(WFFlowID) ? null : WFFlowID)),
                    WFFlowVer = new DBChar((string.IsNullOrWhiteSpace(WFFlowVer) ? null : WFFlowVer))
                };
        }
        #endregion
    }
}