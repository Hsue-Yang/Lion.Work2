using System.Linq;
using System.Web.Mvc;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.WorkFlow;

namespace LionTech.Utility.ERP
{
    public class WorkFlow
    {
        #region - Definitions -
        public enum EnumExecActionType
        {
            NewFlow,
            GetNextNode,
            NextToProcessNode,
            DecisionToProcessNode,
            NextToEndNode,
            Signature,
            NextSigStep,
            TerminateFlow,
            EditeNode,
            BackToNode,
            SetSignature,
            AddDocument,
            DeleteDocument,
            AddRemark
        }

        private enum EnumRequestKey
        {
            WorkFlowIsPopup
        }
        #endregion

        #region - Constructor -
        public WorkFlow(string userID, string workFlowNo, string connectionString, string providerName)
        {
            _userID = userID;
            _workFlowNo = workFlowNo;
            _entity = new Entity_Base(connectionString, providerName);
        }
        #endregion

        #region - Property -
        public bool IsGetWorkFlowData { get; set; }
        public bool IsHideDataBar { get; private set; }
        public bool IsHideButtonBar { get; private set; }
        public bool IsHideSignatureButton { get; private set; }
        public bool IsHideSetSignatureButton { get; private set; }
        public bool IsHideDocumentButton { get; private set; }
        public bool IsHideRemarkButton { get; private set; }
        public bool IsHideTerminateFlowButton { get; private set; }
        public bool IsHideBackToNodeButton { get; private set; }
        public bool IsHideNextToNodeButton { get; private set; }
        public bool IsHideAssginNextNodeNewUserBar { get; private set; }
        public bool IsHidePickNewUserIDButton { get; private set; }

        public Entity_Base.RunTimeWFFlow EntityRunTimeWFFlow { get; private set; }
        #endregion

        #region - Private -
        private readonly string _userID;
        private readonly string _workFlowNo;
        private readonly Entity_Base _entity;
        #endregion
        
        public bool GetWorkFlowData(EnumCultureID cultureID)
        {
            Entity_Base.RunTimeWFFlowPara para =
                new Entity_Base.RunTimeWFFlowPara(cultureID.ToString())
                {
                    WFNo = new DBChar(string.IsNullOrWhiteSpace(_workFlowNo) ? null : _workFlowNo),
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(_userID) ? null : _userID),
                    SigUserIDStr = new DBObject(string.IsNullOrWhiteSpace(_userID) ? null : _userID),
                    NewUserIDStr = new DBObject(string.IsNullOrWhiteSpace(_userID) ? null : _userID)
                };

            if (GetCheckRunTimeWFFlow(cultureID))
            {
                EntityRunTimeWFFlow = _entity.SelectRunTimeWFFlow(para);

                if (EntityRunTimeWFFlow != null)
                {
                    return true;
                }
            }
            return false;
        }

        public bool GetCheckRunTimeWFFlow(EnumCultureID cultureID)
        {
            Entity_Base.RunTimeWFFlowPara para =
                new Entity_Base.RunTimeWFFlowPara(cultureID.ToString())
                {
                    WFNo = new DBChar(string.IsNullOrWhiteSpace(_workFlowNo) ? null : _workFlowNo),
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(_userID) ? null : _userID),
                    SigUserIDStr = new DBObject(string.IsNullOrWhiteSpace(_userID) ? null : _userID),
                    NewUserIDStr = new DBObject(string.IsNullOrWhiteSpace(_userID) ? null : _userID)
                };
            return (_entity.CheckRunTimeWFFlow(para) == Entity_Base.EnumCheckRunTimeWFFlowResult.Success);
        }

        #region - Button Controller -
        public void HideDataBar()
        {
            IsHideDataBar = true;
        }

        public void HideButtonBar()
        {
            IsHideButtonBar = true;
        }
        
        public void HideSignatureButton()
        {
            IsHideSignatureButton = true;
        }

        public void HideSetSignatureButton()
        {
            IsHideSetSignatureButton = true;
        }

        public void HideDocumentButton()
        {
            IsHideDocumentButton = true;
        }

        public void HideRemarkButton()
        {
            IsHideRemarkButton = true;
        }

        public void HideTerminateFlowButton()
        {
            IsHideTerminateFlowButton = true;
        }

        public void HideBackToNodeButton()
        {
            IsHideBackToNodeButton = true;
        }

        public void HideNextToNodeButton()
        {
            IsHideNextToNodeButton = true;
        }

        public void HideAssginNextNodeNewUserBar()
        {
            IsHideAssginNextNodeNewUserBar = true;
        }

        public void HidePickNewUserIDButton()
        {
            IsHidePickNewUserIDButton = true;
        }
        #endregion

        public static void GenerateWorkFlowChart(
            string userID, 
            EnumSystemID sysID, string flowID, string flowVer, 
            string connectionString, string providerName, 
            string filePath)
        {
            Flow flow = new Flow();
            flow.Load(userID, sysID.ToString(), flowID, flowVer, connectionString, providerName);

            Figure figure = new Figure(flow, filePath);
            figure.Draw();
            figure.Save();
        }

        public static void DeleteWorkFlowChart(
            string userID,
            EnumSystemID sysID, string flowID, string flowVer,
            string connectionString, string providerName,
            string filePath)
        {
            Flow flow = new Flow();
            flow.Load(userID, sysID.ToString(), flowID, flowVer, connectionString, providerName);

            Figure figure = new Figure(flow, filePath);
            figure.Delete();
        }

        public void SetViewDataInformation(EnumSystemID systemId, string userID, ViewDataDictionary viewData, ActionExecutedContext filterContext)
        {
            viewData.Set<string>(EnumViewDataItem.WFDataReady, EnumYN.Y.ToString());

            if (EntityRunTimeWFFlow.WFNodeCurrent.WFFunSysID.GetValue() == systemId.ToString() &&
                EntityRunTimeWFFlow.WFNodeCurrent.WFFunControllerID.GetValue() == filterContext.ActionDescriptor.ControllerDescriptor.ControllerName &&
                EntityRunTimeWFFlow.WFNodeCurrent.WFFunActionName.GetValue() == filterContext.ActionDescriptor.ActionName)
            {
                viewData.Set<bool>(EnumViewDataItem.WFIsCurrentNodeFun, true);
            }
            else
            {
                viewData.Set<bool>(EnumViewDataItem.WFIsCurrentNodeFun, false);
            }

            if (string.IsNullOrWhiteSpace(filterContext.HttpContext.Request[EnumRequestKey.WorkFlowIsPopup.ToString()]) == false &&
                filterContext.HttpContext.Request[EnumRequestKey.WorkFlowIsPopup.ToString()] == bool.TrueString)
            {
                viewData.Set<bool>(EnumViewDataItem.WFIsPopup, true);
            }
            else
            {
                viewData.Set<bool>(EnumViewDataItem.WFIsPopup, false);
            }

            viewData.Set<string>(EnumViewDataItem.WFNo, EntityRunTimeWFFlow.WFNo.GetValue());
            viewData.Set<string>(EnumViewDataItem.WFResultID, EntityRunTimeWFFlow.ResultID.GetValue());
            viewData.Set<string>(EnumViewDataItem.WFNodeNo, EntityRunTimeWFFlow.NodeNo.GetValue());
            viewData.Set<string>(EnumViewDataItem.WFCurrentNodeNo, EntityRunTimeWFFlow.WFNodeCurrent.NodeNo.GetValue());
            viewData.Set<string>(EnumViewDataItem.WFCurrentNodeNewUserID, EntityRunTimeWFFlow.WFNodeCurrent.NewUserID.GetValue());
            viewData.Set<string>(EnumViewDataItem.WFCurrentNodeNewUserIDStr, EntityRunTimeWFFlow.WFNodeCurrent.NewUserIDStr.GetValue());

            viewData.Set<string>(EnumViewDataItem.WFFormatWFNo, EntityRunTimeWFFlow.FormatWFNo.GetValue());
            viewData.Set<string>(EnumViewDataItem.WFFlowTypeNM, EntityRunTimeWFFlow.WFFlowTypeNM.GetValue());
            viewData.Set<string>(EnumViewDataItem.WFFlowNM, EntityRunTimeWFFlow.WFFlowNM.GetValue());
            viewData.Set<string>(EnumViewDataItem.WFNewUserID, EntityRunTimeWFFlow.NewUserID.GetValue());
            viewData.Set<string>(EnumViewDataItem.WFNewUserNM, EntityRunTimeWFFlow.NewUserNM.GetValue());
            viewData.Set<string>(EnumViewDataItem.WFResultNM, EntityRunTimeWFFlow.ResultNM.GetValue());
            viewData.Set<string>(EnumViewDataItem.WFSubject, EntityRunTimeWFFlow.WFSubject.GetValue());

            viewData.Set<string>(EnumViewDataItem.WFCurrentNodeNM, EntityRunTimeWFFlow.WFNodeCurrent.WFNodeNM.GetValue());
            viewData.Set<string>(EnumViewDataItem.WFCurrentNodeNewUserNM, EntityRunTimeWFFlow.WFNodeCurrent.NewUserNM.GetValue());

            viewData.Set<string>(EnumViewDataItem.WFCurrentNodeBackNodeID, EntityRunTimeWFFlow.WFNodeCurrent.WFBackNodeID.GetValue());

            viewData.Set<string>(EnumViewDataItem.WFCurrentNodeSigIsStarting, EntityRunTimeWFFlow.WFNodeCurrent.WFSigIsStarting.GetValue());
            viewData.Set<string>(EnumViewDataItem.WFCurrentNodeSigUserIDStr, EntityRunTimeWFFlow.WFNodeCurrent.WFSigUserIDStr.GetValue());

            var sigCurrent = EntityRunTimeWFFlow.WFNodeCurrent.WFSigCurrentList.SingleOrDefault(s => s.SigUserID.GetValue() == userID);
            if (sigCurrent != null)
            {
                viewData.Set<string>(EnumViewDataItem.WFCurrentNodeSigStep, sigCurrent.SigStep.StringValue());
                viewData.Set<string>(EnumViewDataItem.WFCurrentNodeSigSeq, sigCurrent.WFSigSeq.GetValue());
            }

            viewData.Set<string>(EnumViewDataItem.WFCurrentNodeAssgAPISysID, EntityRunTimeWFFlow.WFNodeCurrent.WFAssgAPISysID.GetValue());
            viewData.Set<string>(EnumViewDataItem.WFCurrentNodeAssgAPIControllerID, EntityRunTimeWFFlow.WFNodeCurrent.WFAssgAPIControllerID.GetValue());
            viewData.Set<string>(EnumViewDataItem.WFCurrentNodeAssgAPIActionName, EntityRunTimeWFFlow.WFNodeCurrent.WFAssgAPIActionName.GetValue());
            viewData.Set<string>(EnumViewDataItem.WFCurrentNodeIsAssignNextNode, EntityRunTimeWFFlow.WFNodeCurrent.IsAssgNextNode.GetValue());

            viewData.Set<bool>(EnumViewDataItem.WFNextNodeHasRole, EntityRunTimeWFFlow.WFNodeCurrent.NextNodeRoleList != null && EntityRunTimeWFFlow.WFNodeCurrent.NextNodeRoleList.Any());

            viewData.Set<bool>(EnumViewDataItem.WFIsHideDataBar, IsHideDataBar);
            viewData.Set<bool>(EnumViewDataItem.WFIsHideButtonBar, IsHideButtonBar);
            viewData.Set<bool>(EnumViewDataItem.WFIsHideSignatureButton, IsHideSignatureButton);
            viewData.Set<bool>(EnumViewDataItem.WFIsHideSetSignatureButton, IsHideSetSignatureButton);
            viewData.Set<bool>(EnumViewDataItem.WFIsHideDocumentButton, IsHideDocumentButton);
            viewData.Set<bool>(EnumViewDataItem.WFIsHideRemarkButton, IsHideRemarkButton);
            viewData.Set<bool>(EnumViewDataItem.WFIsHideTerminateFlowButton, IsHideTerminateFlowButton);
            viewData.Set<bool>(EnumViewDataItem.WFIsHideBackToNodeButton, IsHideBackToNodeButton);
            viewData.Set<bool>(EnumViewDataItem.WFIsHideNextToNodeButton, IsHideNextToNodeButton);
            viewData.Set<bool>(EnumViewDataItem.WFIsHideAssginNextNodeNewUserBar, IsHideAssginNextNodeNewUserBar);
            viewData.Set<bool>(EnumViewDataItem.WFIsHidePickNewUserIDButton, IsHidePickNewUserIDButton);   
        }
    }

    public class WorkFlowAction
    {
        public enum EnumExecActionType
        {
            None,
            StartWorkFlow,
            FromToDoListLink,
            FromSystemNotificationLink
        }

        public bool IsPopup { get; set; }
        public string WorkFlowNo { get; set; }
        public EnumExecActionType ExecAction { get; set; }
    }

    public class WorkFlowService
    {
        public enum WFServiceController
        {
            _BaseAP, Generic
        }

        public enum WFServiceActionName
        {
            GetWFNextNodeRoleUserList,
            GetNewFlowResult,
            GetNextToNodeResult,
            GetBackToNodeResult,
            GetTerminateFlowResult,
            GetSignatureResult,
            GetPickNewUserResult
        }
    }
}