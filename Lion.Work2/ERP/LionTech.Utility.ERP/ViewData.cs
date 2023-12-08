using System.Web.Mvc;

namespace LionTech.Utility.ERP
{
    public enum EnumViewDataItem
    {
        UserID, UserNM,
        UserMenu,
        JsMsg, EditionNo,

        SysFunToolIsEnable, SysFunToolNo, SysFunToolNM, SysFunToolDict,

        WFDataReady, WFIsCurrentNodeFun, WFIsPopup,
        
        WFNo, WFResultID, WFNodeNo, WFCurrentNodeNo, WFCurrentNodeNewUserID, WFCurrentNodeNewUserIDStr,
        WFFormatWFNo, WFFlowTypeNM, WFFlowNM, WFNewUserID, WFNewUserNM, WFResultNM, WFSubject,
        WFCurrentNodeNM, WFCurrentNodeNewUserNM, WFCurrentNodeBackNodeID,
        WFCurrentNodeSigIsStarting, WFCurrentNodeSigUserIDStr, WFCurrentNodeSigStep, WFCurrentNodeSigSeq, 
        WFCurrentNodeAssgAPISysID, WFCurrentNodeAssgAPIControllerID, WFCurrentNodeAssgAPIActionName, 
        WFCurrentNodeIsAssignNextNode,

        WFCurrentNodeSigListCount,

        WFNextNodeHasRole,

        WFIsHideDataBar, WFIsHideButtonBar,
        WFIsHideSignatureButton, WFIsHideSetSignatureButton, WFIsHideDocumentButton, WFIsHideRemarkButton, 
        WFIsHideTerminateFlowButton, WFIsHideBackToNodeButton, WFIsHideNextToNodeButton,
        WFIsHideAssginNextNodeNewUserBar, WFIsHidePickNewUserIDButton
    }

    public static class ViewDataExtensions
    {
        public static T Get<T>(this ViewDataDictionary viewData, EnumViewDataItem viewDataItem)
        {
            return (T)viewData[viewDataItem.ToString()];
        }

        public static void Set<T>(this ViewDataDictionary viewData, EnumViewDataItem viewDataItem, object value)
        {
            viewData[viewDataItem.ToString()] = value;
        }
    }
}
