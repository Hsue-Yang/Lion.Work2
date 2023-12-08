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
using System.ComponentModel;
using System.Configuration;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static ERPAP.Models.Sys.SystemFunAssignModel;

namespace ERPAP.Models.Sys
{
    public class SysModel : _BaseAPModel
    {
        #region - Enum -
        public enum EnumDomainType
        {
            [Description("LDAP://lionmail.com")]
            LionMail,
            [Description("LDAP://liontech.com.tw")]
            LionTech
        }

        //for FileData's FilePath
        public enum EnumFilePathKeyWord
        {
            FileData, LionTech, EDIService
        }

        public enum EnumEDIFlowItem
        {
            NULL
        }

        public enum EnumTextFormat
        {
            [Description("{0} {1}")]
            User,
            [Description("{0} ({1})")]
            Code
        }
        #endregion

        #region - Class -
        protected class SelectListGroupItem : SelectListItem
        {
            public string GroupID { get; set; }
        }

        protected class SystemInfo
        {
            public SelectListItem Sys { get; set; }
            public List<SelectListItem> SubSysList { get; set; }
            public List<SelectListGroupItem> FunControllerList { get; set; }
            public List<SelectListGroupItem> FunActionList { get; set; }
        }

        public class SystemSub : DBEntity.ISelectItem
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }

            public string SubSysID { get; set; }
            public string SubSysNM { get; set; }

            public string ItemText()
            {
                return $"{SysNM} ({SysID})";
            }

            public string ItemValue()
            {
                return SysID;
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

        public class UserSystem : DBEntity.ISelectItem
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string IsOutsourcing { get; set; }

            public string ItemText()
            {
                return $"{SysNM} ({SysID})";
            }

            public string ItemValue()
            {
                return SysID;
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

        public class SystemRole : ISelectItem
        {
            public string RoleID { get; set; }
            public string RoleNM { get; set; }

            public string ItemText()
            {
                return $"{RoleNM} ({RoleID})";
            }

            public string ItemValue()
            {
                return RoleID;
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

        public class SystemRoleCategory : DBEntity.ISelectItem
        {
            public string RoleCategoryID { get; set; }
            public string RoleCategoryNM { get; set; }

            public string ItemText()
            {
                return $"{RoleCategoryNM} ({RoleCategoryID})";
            }

            public string ItemValue()
            {
                return RoleCategoryID;
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

        public class SysPurview : DBEntity.ISelectItem
        {
            public string PurviewID { get; set; }
            public string PurviewNM { get; set; }

            public string ItemText()
            {
                return $"{PurviewNM} ({PurviewID})";
            }

            public string ItemValue()
            {
                return PurviewID;
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

        public class SysFunMenu : DBEntity.ISelectItem
        {
            public string FunMenu { get; set; }
            public string FunMenuNM { get; set; }

            public string ItemText()
            {
                return $"{FunMenuNM} ({FunMenu})";
            }

            public string ItemValue()
            {
                return FunMenu;
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

        public class SysFunGroup : DBEntity.ISelectItem
        {
            public string SysID { get; set; }
            public string FunControllerID { get; set; }
            public string FunGroupNM { get; set; }

            public string ItemText()
            {
                return $"{FunGroupNM} ({FunControllerID})";
            }

            public string ItemValue()
            {
                return FunControllerID;
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

        public class SysFun
        {
            public string SysID { get; set; }
            public string FunControllerID { get; set; }

            public string FunActionName { get; set; }
            public string FunNM { get; set; }
        }

        public class SysRoleFun
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }

            public string FunControllerID { get; set; }
            public string FunGroupNM { get; set; }

            public string FunActionName { get; set; }
            public string FunNM { get; set; }

            public string RoleID { get; set; }
            public string RoleNM { get; set; }
            public string HasRole { get; set; }
        }

        public class SystemFunName : ISelectItem
        {
            public string FunActionName { get; set; }
            public string FunNM { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return this.FunNM;
            }

            public string ItemValue()
            {
                return this.FunActionName;
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

        public class SystemFunAction
        {
            public string SysID { get; set; }
            public string FunAction { get; set; }
            public string FunActionNM { get; set; }
            public string FunControllerID { get; set; }
        }

        public class SysSystemAPIGroup : ISelectItem
        {
            public string APIGroupID { get; set; }
            public string APIGroupNM { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return $"{APIGroupNM} ({APIGroupID})";
            }

            public string ItemValue()
            {
                return APIGroupID;
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

        public class SysSystemAPIFun : DBEntity.ISelectItem
        {
            public string APIFunID { get; set; }
            public string APIFunNM { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return $"{APIFunNM} ({APIFunID})";
            }

            public string ItemValue()
            {
                return APIFunID;
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

        public class SystemAPIFullName
        {
            public string SysNM { get; set; }
            public string APIGroupNM { get; set; }
            public string APIFunNM { get; set; }
        }

        public class SysSystemEventGroup : DBEntity.ISelectItem
        {
            public string EventGroupID { get; set; }
            public string EventGroupNM { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return $"{EventGroupNM} ({EventGroupID})";
            }

            public string ItemValue()
            {
                return EventGroupID;
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

        public class SysSystemEvent : DBEntity.ISelectItem
        {
            public string EventID { get; set; }
            public string EventNM { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return $"{EventNM} ({EventID})";
            }

            public string ItemValue()
            {
                return EventID;
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

        public class SysSystemEventFullName
        {
            public string SysNM { get; set; }
            public string EventGroupNM { get; set; }
            public string EventNM { get; set; }
        }

        public class UserBasicInfo
        {
            public string UserID { get; set; }
            public string UserNM { get; set; }
            public string ComID { get; set; }
            public string ComNM { get; set; }
            public string UnitID { get; set; }
            public string UnitNM { get; set; }
            public string RestrictType { get; set; }
            public string RestrictTypeNM { get; set; }
            public int ErrorTimes { get; set; }
            public string IsLock { get; set; }
            public string IsDisable { get; set; }
            public string IsLeft { get; set; }
            public DateTime? LastConnectDT { get; set; }
        }

        public class LineBotAccountSettingDetail
        {
            public string SysID { get; set; }
            public string LineID { get; set; }
            public string LineNM { get; set; }
            public string LineNMID { get; set; }
            public string LineNMZHTW { get; set; }
            public string LineNMZHCN { get; set; }
            public string LineNMENUS { get; set; }
            public string LineNMTHTH { get; set; }
            public string LineNMJAJP { get; set; }
            public string LineNMKOKR { get; set; }
            public string ChannelID { get; set; }
            public string ChannelSecret { get; set; }
            public string ChannelAccessToken { get; set; }
            public string IsDisable { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }

        public class SystemLineBotReceiver
        {
            public string SysID { get; set; }
            public string LineID { get; set; }
            public string LineNMID { get; set; }
            public string LineReceiverID { get; set; }
            public string LineReceiverNM { get; set; }
            public string LineReceiverNMZHTW { get; set; }
            public string LineReceiverNMZHCN { get; set; }
            public string LineReceiverNMENUS { get; set; }
            public string LineReceiverNMTHTH { get; set; }
            public string LineReceiverNMJAJP { get; set; }
            public string LineReceiverNMKOKR { get; set; }
            public string SourceType { get; set; }
            public string IsDisable { get; set; }
            public string ReceiverID { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }

        public class LineBotID : ISelectItem
        {
            public string LineID { set; get; }
            public string LineNMID { set; get; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return $"{LineNMID}";
            }

            public string ItemValue()
            {
                return LineID;
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

        public class CMCode
        {
            public string CodeID { get; set; }
            public string CodeNM { get; set; }
        }

        public class SysLoginEventID
        {
            public string LoginEventID { get; set; }
            public string LoginEventNM { get; set; }
            public string LoginEventNMID { get; set; }
        }

        public class SystemFunElm
        {
            public string ElmID { get; set; }
            public string SysID { get; set; }
            public string FunControllerID { get; set; }
            public string FunActionNM { get; set; }
            public string ElmNM { get; set; }
            public string ElmNMID { get; set; }
            public string FnNMID { get; set; }
            public string FnGroupNMID { get; set; }
            public int DefaultDisplaySts { get; set; }
            public string DefaultDisplay { get; set; }
            public string IsDisable { get; set; }
            public string UpdUserIDNM { get; set; }
            public string ElmNMZHTW { get; set; }
            public string ElmNMZHCN { get; set; }
            public string ElmNMENUS { get; set; }
            public string ElmNMTHTH { get; set; }
            public string ElmNMJAJP { get; set; }
            public string ElmNMKOKR { get; set; }
            public string UpdUserID { get; set; }
            public DateTime UpdDT { get; set; }
        }

        public interface ISelectItem
        {
            string GroupBy();
            string ItemText();
            string ItemValue();
            string ItemValue(string key);
            string PictureUrl();
        }

        public class SysSystemFunControllerID : ISelectItem
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string FunControllerID { get; set; }
            public string FunGroupNM { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return $"{FunGroupNM} ({FunControllerID})";
            }

            public string ItemValue()
            {
                return FunControllerID;
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
        public class SystemEDIJobDetail
        {
            public string SysID { get; set; }
            public string EDIFlowID { get; set; }
            public string EDIJobID { get; set; }
            public string EDIJobZHTW { get; set; }
            public string EDIJobZHCN { get; set; }
            public string EDIJobENUS { get; set; }
            public string EDIJobTHTH { get; set; }
            public string EDIJobJAJP { get; set; }
            public string EDIJobKOKR { get; set; }
            public string EDIJobType { get; set; }
            public string EDIConID { get; set; }
            public string ObjectName { get; set; }
            public string DepEDIJobID { get; set; }
            public string IsUseRes { get; set; }
            public string IgnoreWarning { get; set; }
            public string IsDisable { get; set; }
            public string FileSource { get; set; }
            public string FileEncoding { get; set; }
            public string URLPath { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserID { get; set; }
        }
        public class SystemEDIParaValue
        {
            public string SysID { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserID { get; set; }
            public string EDIFlowID { get; set; }
            public string EDIJobID { get; set; }
            public string EDIJobParaID { get; set; }
            public string EDIJobParaType { get; set; }
            public string EDIJobParaValue { get; set; }
        }

        public class SystemEDIJobID : ISelectItem
        {
            public string EDIJobID { get; set; }
            public string EDIJobNM { get; set; }

            public string ItemText()
            {
                return $"{EDIJobNM} ({EDIJobID})";
            }

            public string ItemValue()
            {
                return EDIJobID;
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

        public class UserSystemSysID : ISelectItem
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }

            string ISelectItem.GroupBy()
            {
                throw new NotImplementedException();
            }

            string ISelectItem.ItemText()
            {
                return SysNM;
            }

            string ISelectItem.ItemValue()
            {
                return SysID;
            }

            string ISelectItem.ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            string ISelectItem.PictureUrl()
            {
                throw new NotImplementedException();
            }
        }

        public class SysFunRawData
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string FunControllerID { get; set; }
            public string FunControllerNMID { get; set; }
            public string FunControllerNM { get; set; }
            public string FunActionID { get; set; }
            public string FunActionNM { get; set; }
            public string FunActionNMID { get; set; }
        }

        public class SystemMain
        {
            public string SysNM { get; set; }
            public string SysNMID { get; set; }
        }

        public class SystemEventTarget
        {
            public string SysID { get; set; }

            public string TargetSysID { get; set; }
            public string TargetSysNM { get; set; }
            public string SubSysID { get; set; }
            public string SubSysNM { get; set; }

            public string TargetPath { get; set; }
            public bool HasITRole { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }

        public class SystemAPIFuntions : ISelectItem
        {
            public string APIFun { get; set; }
            public string APIFunNM { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return this.APIFunNM;
            }

            public string ItemValue()
            {
                return this.APIFun;
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

        public class SystemRoleGroup : ISelectItem
        {
            public string ROLE_GROUP_ID { get; set; }
            public string ROLE_GROUP_NM { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return ROLE_GROUP_NM;
            }

            public string ItemValue()
            {
                return ROLE_GROUP_ID;
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

        public class EDIJobTypes : ISelectItem  
        {
            public string CodeID { get; set; }
            public string CodeNM { get; set; }
            
            public string GroupBy()
            {
                throw new NotImplementedException();
            }

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
        }

        public class SystemWorkFlowGroupIDs : ISelectItem
        {
            public string WFFlowGroupID { get; set; }
            public string WFFlowGroupNM { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return this.WFFlowGroupNM;
            }

            public string ItemValue()
            {
                return this.WFFlowGroupID;
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

        public class SystemEDIJobIDs : ISelectItem
        {
            public string EdiJobID { get; set; }
            public string EdiJobNM { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return EdiJobNM;
            }

            public string ItemValue()
            {
                return EdiJobID;
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

        public class UserSystemWorkFlowIDs : ISelectItem
        {
            public string WF_FLOW_VER { get; set; }
            public string WF_FLOW_ID { get; set; }
            public string WF_FLOW_VALUE { get; set; }
            public string WF_FLOW_TEXT { get; set; }
            public string SORT_ORDER { get; set; }
            
            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return this.WF_FLOW_TEXT;
            }

            public string ItemValue()
            {
                return this.WF_FLOW_VALUE;
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

        public class SystemEDIFlows : ISelectItem
        {
            public string EDIFlowID { get; set; }
            public string EDIFlowNM { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return this.EDIFlowNM;
            }

            public string ItemValue()
            {
                return this.EDIFlowID;
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

        public class SystemWorkFlowNodeIDs : ISelectItem
        {

            public string WFNodeID { get; set; }
            public string SortOrder { get; set; }
            public string WFNodeNM { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return WFNodeNM;
            }

            public string ItemValue()
            {
                return WFNodeID;
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

        public class SystemWorkFlowNode : ISelectItem
        {
            public string SysNM { get; set; }
            public string WFFlowNM { get; set; }
            public string WFFlowVer { get; set; }
            public string WFNodeNM { get; set; }
            public string NodeType { get; set; }
            public string WFSigMemoZHTW { get; set; }
            public string WFSigMemoZHCN { get; set; }
            public string WFSigMemoENUS { get; set; }
            public string WFSigMemoTHTH { get; set; }
            public string WFSigMemoJAJP { get; set; }
            public string WFSigMemoKOKR { get; set; }
            public string SigAPISysID { get; set; }
            public string SigAPIControllerID { get; set; }
            public string SigAPIActionName { get; set; }
            public string ChkAPISysID { get; set; }
            public string ChkAPIControllerID { get; set; }
            public string ChkAPIActionName { get; set; }
            public string AssgAPISysID { get; set; }
            public string AssgAPIControllerID { get; set; }
            public string AssgAPIActionName { get; set; }
            public string IsSigNextNode { get; set; }
            public string IsSigBackNode { get; set; }
            public string IsAssgNextNode { get; set; }
            public string SysID { get; set; }
            public string WFFlowGroupID { get; set; }
            public string WFFlowID { get; set; }
            public string WFNodeID { get; set; }

            public string NodeTypeNM { get; set; }
            public string NodeSeqX { get; set; }
            public string NodeSeqY { get; set; }
            public string IsFirst { get; set; }
            public string IsFinally { get; set; }

            public string FunSysNM { get; set; }
            public string SubSysNM { get; set; }
            public string FunControllerNM { get; set; }
            public string FunActionNameNM { get; set; }

            public string BackWFNodeNM { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDt { get; set; }

            public string GroupBy()
            {
                return WFNodeID;
            }

            public string ItemText()
            {
                return WFNodeNM;
            }

            public string ItemValue()
            {
                return WFNodeID;
            }

            public string ItemValue(string key)
            {
                return WFNodeID;
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }
        }

        public class SystemRoleCategoryID : ISelectItem
        {
            public string RoleCategoryID { get; set; }
            public string RoleCategoryNM { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return string.Format("{0} ({1})", RoleCategoryNM, RoleCategoryID);
            }

            public string ItemValue()
            {
                return RoleCategoryID;
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

        public class SystemRoleID : ISelectItem
        {
            public string RoleID { get; set; }
            public string RoleNM { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return string.Format("{0} ({1})", RoleNM, RoleID);
            }

            public string ItemValue()
            {
                return RoleID;
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

        public class SystemConditionID : ISelectItem
        {
            public string RoleConditionID { get; set; }
            public string RoleConditionNM { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return string.Format("{0} ({1})", RoleConditionNM, RoleConditionID);
            }

            public string ItemValue()
            {
                return RoleConditionID;
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

        public class SystemFunController
        {
            public string SysID { get; set; }
            public string FunControllerID { get; set; }
            public string FunGroupNM { get; set; }
        }

        public class SystemCultureID : ISelectItem
        {
            public string CultureID { get; set; }
            public string CultureNM { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return $"{CultureNM} ({CultureID})";
            }

            public string ItemValue()
            {
                return CultureID;
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


        #endregion

        #region - Tab -
        public List<TabStripHelper.Tab> SysAPITabList = new List<TabStripHelper.Tab>();

        public void GetSysAPITabList(EnumTabAction actionNM)
        {
            SysAPITabList = new List<TabStripHelper.Tab>()
                {
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysSystemAPIGroup ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysSystemAPIGroup ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemAPIGroup)),
                        TabText=SysResource.TabText_SystemAPIGroup,
                        ImageURL=string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysSystemAPI ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysSystemAPI ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemAPI)),
                        TabText=SysResource.TabText_SystemAPI,
                        ImageURL=string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysSystemAPILog ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysSystemAPILog ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemAPILog)),
                        TabText=SysResource.TabText_SystemAPILog,
                        ImageURL=string.Empty
                    }
                };
        }

        public List<TabStripHelper.Tab> GoogleAccountSettingTabList = new List<TabStripHelper.Tab>();

        public void GetGoogleAccountSettingTabList(EnumTabAction actionNM)
        {
            GoogleAccountSettingTabList = new List<TabStripHelper.Tab>()
                {
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.GoogleAccountSetting ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.GoogleAccountSetting ? string.Empty : Common.GetEnumDesc(EnumTabAction.GoogleAccountSetting)),
                        TabText=SysResource.TabText_GoogleAccountSetting,
                        ImageURL=string.Empty
                    },
                };
        }

        public List<TabStripHelper.Tab> SysEDITabList = new List<TabStripHelper.Tab>();

        public void GetSysEDITabList(EnumTabAction actionNM)
        {
            SysEDITabList = new List<TabStripHelper.Tab>()
                {
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysSystemEDIFlow ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysSystemEDIFlow ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEDIFlow)),
                        TabText=SysResource.TabText_SystemEDIFlow,
                        ImageURL=string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysSystemEDICon ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysSystemEDICon ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEDICon)),
                        TabText=SysResource.TabText_SystemEDICon,
                        ImageURL=string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysSystemEDIJob ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysSystemEDIJob ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEDIJob)),
                        TabText=SysResource.TabText_SystemEDIJob,
                        ImageURL=string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysSystemEDIFlowLog ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysSystemEDIFlowLog ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEDIFlowLog)),
                        TabText=SysResource.TabText_SystemEDIFlowLog,
                        ImageURL=string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysSystemEDIJobLog ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysSystemEDIJobLog ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEDIJobLog)),
                        TabText=SysResource.TabText_SystemEDIJobLog,
                        ImageURL=string.Empty
                    }
                };
        }

        public List<TabStripHelper.Tab> SysWFTabList = new List<TabStripHelper.Tab>();

        public void GetSysWFTabList(EnumTabAction actionNM)
        {
            SysWFTabList = new List<TabStripHelper.Tab>()
                {
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SystemWorkFlowGroup ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SystemWorkFlowGroup ? string.Empty : Common.GetEnumDesc(EnumTabAction.SystemWorkFlowGroup)),
                        TabText=SysResource.TabText_SystemWorkFlowGroup,
                        ImageURL=string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SystemWorkFlow ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SystemWorkFlow ? string.Empty : Common.GetEnumDesc(EnumTabAction.SystemWorkFlow)),
                        TabText=SysResource.TabText_SystemWorkFlow,
                        ImageURL=string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SystemWorkFlowNode ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SystemWorkFlowNode ? string.Empty : Common.GetEnumDesc(EnumTabAction.SystemWorkFlowNode)),
                        TabText=SysResource.TabText_SystemWorkFlowNode,
                        ImageURL=string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SystemWorkFlowChart ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SystemWorkFlowChart ? string.Empty : Common.GetEnumDesc(EnumTabAction.SystemWorkFlowChart)),
                        TabText=SysResource.TabText_SystemWorkFlowChart,
                        ImageURL=string.Empty
                    },
                };
        }

        public List<TabStripHelper.Tab> SysWFNodeTabList = new List<TabStripHelper.Tab>();

        public void GetWFNodeTabList(EnumTabAction actionNM, EnumNodeType nodeType)
        {
            SysWFNodeTabList = new List<TabStripHelper.Tab>();
            SysWFNodeTabList.Add(new TabStripHelper.Tab
            {
                ControllerName = (actionNM == EnumTabAction.SystemWorkFlowNodeDetail ? string.Empty : EnumTabController.Sys.ToString()),
                ActionName = (actionNM == EnumTabAction.SystemWorkFlowNodeDetail ? string.Empty : Common.GetEnumDesc(EnumTabAction.SystemWorkFlowNodeDetail)),
                TabText = SysResource.TabText_SystemWorkFlowNodeDetail,
                ImageURL = string.Empty
            });

            if (nodeType == EnumNodeType.S || nodeType == EnumNodeType.P || nodeType == EnumNodeType.D)
            {
                SysWFNodeTabList.Add(new TabStripHelper.Tab
                {
                    ControllerName = (actionNM == EnumTabAction.SystemWorkFlowNext ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName = (actionNM == EnumTabAction.SystemWorkFlowNext ? string.Empty : Common.GetEnumDesc(EnumTabAction.SystemWorkFlowNext)),
                    TabText = SysResource.TabText_SystemWorkFlowNext,
                    ImageURL = string.Empty
                });
            }

            if (nodeType == EnumNodeType.P)
            {
                SysWFNodeTabList.Add(new TabStripHelper.Tab
                {
                    ControllerName = (actionNM == EnumTabAction.SystemWorkFlowSignature ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName = (actionNM == EnumTabAction.SystemWorkFlowSignature ? string.Empty : Common.GetEnumDesc(EnumTabAction.SystemWorkFlowSignature)),
                    TabText = SysResource.TabText_SystemWorkFlowSignature,
                    ImageURL = string.Empty
                });
                SysWFNodeTabList.Add(new TabStripHelper.Tab
                {
                    ControllerName = (actionNM == EnumTabAction.SystemWorkFlowDocument ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName = (actionNM == EnumTabAction.SystemWorkFlowDocument ? string.Empty : Common.GetEnumDesc(EnumTabAction.SystemWorkFlowDocument)),
                    TabText = SysResource.TabText_SystemWorkFlowDocument,
                    ImageURL = string.Empty
                });
            }
        }

        public List<TabStripHelper.Tab> SysEventTabList = new List<TabStripHelper.Tab>();

        public void GetSysEventTabList(EnumTabAction actionNM)
        {
            SysEventTabList = new List<TabStripHelper.Tab>()
                {
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysSystemEventGroup ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysSystemEventGroup ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEventGroup)),
                        TabText=SysResource.TabText_SystemEventGroup,
                        ImageURL=string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysSystemEvent ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysSystemEvent ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEvent)),
                        TabText=SysResource.TabText_SystemEvent,
                        ImageURL=string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysSystemEventEDI ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysSystemEventEDI ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEventEDI)),
                        TabText=SysResource.TabText_SystemEventEDI,
                        ImageURL=string.Empty
                    }
                };
        }

        public List<TabStripHelper.Tab> SysSystemTabList = new List<TabStripHelper.Tab>();

        public void GetSysSystemTabList(EnumTabAction actionNM)
        {
            SysSystemTabList = new List<TabStripHelper.Tab>()
                {
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysSystemSetting ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysSystemSetting ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemSetting)),
                        TabText = SysResource.TabText_SystemSetting,
                        ImageURL = string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysSystemRole ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysSystemRole ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemRole)),
                        TabText = SysResource.TabText_SystemRole,
                        ImageURL = string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysSystemRoleCondition ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysSystemRoleCondition ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemRoleCondition)),
                        TabText = SysResource.TabText_SysSystemRoleCondition,
                        ImageURL = string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysSystemPurview ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysSystemPurview ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemPurview)),
                        TabText = SysResource.TabText_SystemPurview,
                        ImageURL = string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysSystemFunMenu ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysSystemFunMenu ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemFunMenu)),
                        TabText = SysResource.TabText_SystemFunMenu,
                        ImageURL = string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysSystemFunGroup ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysSystemFunGroup ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemFunGroup)),
                        TabText = SysResource.TabText_SystemFunGroup,
                        ImageURL = string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysSystemFun ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysSystemFun ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemFun)),
                        TabText = SysResource.TabText_SystemFun,
                        ImageURL = string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysSystemFunElm ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysSystemFunElm ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemFunElm)),
                        TabText = SysResource.TabText_SystemFunElm,
                        ImageURL = string.Empty
                    }
                };
        }

        public List<TabStripHelper.Tab> SysSystemFunTabList = new List<TabStripHelper.Tab>();

        public void GetSysSystemFunTabList(EnumTabAction actionNM)
        {
            SysSystemFunTabList = new List<TabStripHelper.Tab>()
                {
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysSystemFunDetail ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysSystemFunDetail ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemFunDetail)),
                        TabText=SysResource.TabText_SystemFunDetail,
                        ImageURL=string.Empty
                    }
                };

            if (ExecAction == EnumActionType.Update)
            {
                SysSystemFunTabList.Add(new TabStripHelper.Tab
                {
                    ControllerName = (actionNM == EnumTabAction.SysSystemFunAssign ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName = (actionNM == EnumTabAction.SysSystemFunAssign ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemFunAssign)),
                    TabText = SysResource.TabText_SystemFunAssign,
                    ImageURL = string.Empty
                });
            }
        }

        public List<TabStripHelper.Tab> SysUserSystemTabList = new List<TabStripHelper.Tab>();

        public void GetSysUserSystemTabList(EnumTabAction actionNM)
        {
            SysUserSystemTabList = new List<TabStripHelper.Tab>()
                {
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysUserRoleFun ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysUserRoleFun ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysUserRoleFun)),
                        TabText = SysResource.TabText_UserRoleFun,
                        ImageURL = string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysUserSystem ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysUserSystem ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysUserSystem)),
                        TabText = SysResource.TabText_UserSystem,
                        ImageURL = string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysRoleUser ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysRoleUser ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysRoleUser)),
                        TabText = SysResource.TabText_RoleUser,
                        ImageURL = string.Empty
                    }
                };
        }

        public List<TabStripHelper.Tab> SysSRCProjectTabList = new List<TabStripHelper.Tab>();

        public void GetSysDomainTabList(EnumTabAction actionNM)
        {
            SysSRCProjectTabList = new List<TabStripHelper.Tab>()
                {
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysUserDomain ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysUserDomain ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysUserDomain)),
                        TabText = SysResource.TabText_UserDomain,
                        ImageURL = string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysDomainGroup ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysDomainGroup ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysDomainGroup)),
                        TabText = SysResource.TabText_DomainGroup,
                        ImageURL = string.Empty
                    }
                };
        }

        public List<TabStripHelper.Tab> SysUserRoleFunctionTabList = new List<TabStripHelper.Tab>();

        public void GetSysUserRoleFunctionTabList(EnumTabAction actionNM)
        {
            SysUserRoleFunctionTabList = new List<TabStripHelper.Tab>()
                {
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysUserRoleFunDeatil ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysUserRoleFunDeatil ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysUserRoleFunDeatil)),
                        TabText = SysResource.TabText_UserRoleFunDetail,
                        ImageURL = string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysUserFunction ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysUserFunction ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysUserFunction)),
                        TabText = SysResource.TabText_UserFunction,
                        ImageURL = string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysUserPurview ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysUserPurview ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysUserPurview)),
                        TabText = SysResource.TabText_UserPurview,
                        ImageURL = string.Empty
                    }
                };
        }

        public List<TabStripHelper.Tab> SysADSTabList = new List<TabStripHelper.Tab>();

        public void GetSysADSTabList(EnumTabAction actionNM)
        {
            SysADSTabList = new List<TabStripHelper.Tab>()
                {
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysSystemRecord ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysSystemRecord ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemRecord)),
                        TabText=SysResource.TabText_SystemRecord,
                        ImageURL=string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysUserBasicInfo ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysUserBasicInfo ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysUserBasicInfo)),
                        TabText=SysResource.TabText_UserBasicInfo,
                        ImageURL=string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysADSReport ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysADSReport ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysADSReport)),
                        TabText=SysResource.TabText_ADSReport,
                        ImageURL=string.Empty
                    }
                };
        }

        public List<TabStripHelper.Tab> UserADSTabList = new List<TabStripHelper.Tab>();

        public void GetUserADSTabList(EnumTabAction actionNM)
        {
            UserADSTabList = new List<TabStripHelper.Tab>()
                {
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysUserBasicInfoDetail ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysUserBasicInfoDetail ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysUserBasicInfoDetail)),
                        TabText=SysResource.TabText_UserBasicInfoDetail,
                        ImageURL=string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName=(actionNM == EnumTabAction.SysUserSystemRole ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName=(actionNM == EnumTabAction.SysUserSystemRole ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysUserSystemRole)),
                        TabText=SysResource.TabText_UserSystemRole,
                        ImageURL=string.Empty
                    }
                };
        }

        public List<TabStripHelper.Tab> ConnectLogTabList = new List<TabStripHelper.Tab>();

        public void GetConnectLogTabList(EnumTabAction actionNM)
        {
            ConnectLogTabList = new List<TabStripHelper.Tab>()
                {
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysUserConnect ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysUserConnect ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysUserConnect)),
                        TabText = SysResource.TabText_UserConnect,
                        ImageURL = string.Empty
                    },
                    new TabStripHelper.Tab
                    {
                        ControllerName = (actionNM == EnumTabAction.SysLatestUseIP ? string.Empty : EnumTabController.Sys.ToString()),
                        ActionName = (actionNM == EnumTabAction.SysLatestUseIP ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysLatestUseIP)),
                        TabText = SysResource.TabText_LatestUseIP,
                        ImageURL = string.Empty
                    }
                };
        }
        #endregion

        #region - AP -
        #region - 取得使用者EMail -
        public string GetUserEMail(string userID)
        {
            try
            {
                EntitySys.UserEMailPara para = new EntitySys.UserEMailPara
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(userID) ? null : userID)
                };

                return new EntitySys(ConnectionStringSERP, ProviderNameSERP).SelectUserEMail(para).GetValue();
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return null;
        }
        #endregion

        #region - 取得登入事件代碼清單 -
        public List<SysLoginEventID> SysLoginEventIDList { get; private set; }

        public async Task<bool> GetSysLoginEventIDList(string sysID, string userID, EnumCultureID cultureId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID))
                {
                    SysLoginEventIDList = new List<SysLoginEventID>();
                    return true;
                }

                string apiUrl = API.SysLoginEvent.QuerySysLoginEventById(sysID, userID, cultureId.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SysLoginEventIDList = (List<SysLoginEventID>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    SysLoginEventIDList = responseObj.SysLoginEventIDList;
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

        #region - 取得Line代碼 -
        public List<LineBotID> LineBotIDList { get; private set; }

        public bool GetLineBotIDList(string userID, string sysID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID))
                {
                    LineBotIDList = new List<LineBotID>();
                    return true;
                }

                string apiUrl = API.SystemLineBot.QuerySystemLineBotIDList(userID, sysID, cultureID.ToString().ToUpper());
                string response = Common.HttpWebRequestGetResponseString(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new { lineBotIDList = (List<LineBotID>)null });

                if (responseObj != null)
                {
                    LineBotIDList = responseObj.lineBotIDList;
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

        public bool IsITManager { get; protected set; }

        #region - 確認是否IT管理者 -
        /// <summary>
        /// 確認是否IT管理者
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="systemID"></param>
        /// <returns></returns>
        public bool CheckIsITManager(string userID, string systemID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(systemID))
                {
                    IsITManager = false;
                    return true;
                }

                string apiUrl = API.SystemSetting.IsITManager(systemID, userID);
                var response = Common.HttpWebRequestGetResponseString(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    IsITManager = false
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                IsITManager = responseObj.IsITManager;
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        public bool HasSysID { get; protected set; } = false;

        #region - 確認是否擁有該系統權限 -
        public bool SetHasSysID(string sysID)
        {
            HasSysID = UserSystemByIdList.Any(w => w.SysID == sysID);
            return HasSysID;
        }
        #endregion

        #region - 取得系統檔案路徑 -
        public async Task<string> GetSysSystemFilePath(string sysID, string userID)
        {
            try
            {
                string apiUrl = API.SystemSetting.QuerySystemFilePath(sysID, userID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    FilePath = (string)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);
                if (Common.IsInKubernetes())
                {
                    return responseObj.FilePath.Replace(@"\\", @"C:\");
                }
                return responseObj.FilePath;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return null;
        }
        #endregion

        #region - 取得應用系統主檔 -
        protected SystemMain GetSysSystemMain(string sysID, EnumCultureID cultureID)
        {
            var sysId = string.IsNullOrWhiteSpace(sysID) ? null : sysID;
            string apiUrl = API.SystemSetting.QuerySystemSetting(sysId, "");
            var response = Common.HttpWebRequestGetResponseString(apiUrl, AppSettings.APITimeOut);

            var responseObj = new
            {
                SysID = (string)null,
                SysMANUserID = (string)null,
                SysNMZHTW = (string)null,
                SysNMZHCN = (string)null,
                SysNMENUS = (string)null,
                SysNMTHTH = (string)null,
                SysNMJAJP = (string)null,
                SysNMKOKR = (string)null,
                SysIndexPath = (string)null,
                SysKey = (string)null,
                ENSysID = (string)null,
                IsOutsourcing = (string)null,
                IsDisable = (string)null,
                SortOrder = (string)null
            };

            responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

            var result = new SystemMain();

            switch (cultureID)
            {
                case EnumCultureID.zh_TW:
                    result.SysNM = responseObj.SysNMZHTW;
                    result.SysNMID = $"{responseObj.SysNMZHTW}({responseObj.SysID})";
                    break;
                case EnumCultureID.zh_CN:
                    result.SysNM = responseObj.SysNMZHCN;
                    result.SysNMID = $"{responseObj.SysNMZHCN}({responseObj.SysID})";
                    break;
                case EnumCultureID.en_US:
                    result.SysNM = responseObj.SysNMENUS;
                    result.SysNMID = $"{responseObj.SysNMENUS}({responseObj.SysID})";
                    break;
                case EnumCultureID.th_TH:
                    result.SysNM = responseObj.SysNMTHTH;
                    result.SysNMID = $"{responseObj.SysNMTHTH}({responseObj.SysID})";
                    break;
                case EnumCultureID.ja_JP:
                    result.SysNM = responseObj.SysNMJAJP;
                    result.SysNMID = $"{responseObj.SysNMJAJP}({responseObj.SysID})";
                    break;
                case EnumCultureID.ko_KR:
                    result.SysNM = responseObj.SysNMKOKR;
                    result.SysNMID = $"{responseObj.SysNMKOKR}({responseObj.SysID})";
                    break;
            }

            return result;
        }
        #endregion

        #region - LDAP取得網域類型 -
        public EnumDomainType GetDomainTypeByLDAPPath(string ldapPath)
        {
            if (string.IsNullOrWhiteSpace(ldapPath))
            {
                return EnumDomainType.LionMail;
            }

            return (from s in Enum.GetNames(typeof(EnumDomainType))
                    let type = (EnumDomainType)Enum.Parse(typeof(EnumDomainType), s)
                    let path = Common.GetEnumDesc(type)
                    where ldapPath.ToLower().IndexOf(path.ToLower(), StringComparison.Ordinal) > -1
                    select type).SingleOrDefault();
        }
        #endregion

        public bool GetDomainLoginResult(string domainPath, string userAccount, string pwd)
        {
            try
            {
                DirectoryEntry directoryEntry = new DirectoryEntry(domainPath);
                directoryEntry.Username = userAccount.Split('@')[0];
                directoryEntry.Password = pwd;
                DirectorySearcher searcher = new DirectorySearcher(directoryEntry);
                searcher.SearchScope = SearchScope.Subtree;
                searcher.Filter = "(&(objectCategory=person)(samaccountname=" + userAccount + "))";

                searcher.PropertiesToLoad.Add("samaccountname");
                searcher.PropertiesToLoad.Add("memberOf");

                searcher.FindOne();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<UserSystemSysID> EntityUserSystemSysIDList { get; private set; }

        public async Task<bool> GetUserSystemSysIDList(string userID, bool excludeOutsourcing, EnumCultureID cultureID)
        {
            try
            {
                var userId = string.IsNullOrWhiteSpace(userID) ? null : userID;
                string apiUrl = API.SystemSetting.QueryUserSystemSysIDs(userId, excludeOutsourcing, cultureID.ToString().ToUpper());
                var response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                var responseObj = new
                {
                    UserSystemSysIDs = (List<UserSystemSysID>)null
                };
                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                EntityUserSystemSysIDList = responseObj.UserSystemSysIDs;

                if (EntityUserSystemSysIDList != null)
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

        public List<UserSystem> UserSystemByIdList { get; private set; }

        #region - 取得使用者應用系統選單 -
        public async Task<bool> GetUserSystemByIdList(string userID, bool isExcludeOutsourcing, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemSetting.QueryUserSystemByIds(userID, isExcludeOutsourcing, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                UserSystemByIdList = Common.GetJsonDeserializeObject<List<UserSystem>>(response);

                if (UserSystemByIdList != null)
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

        List<UserSystemSysID> _entitySystemSysIDList = new List<UserSystemSysID>();
        public List<UserSystemSysID> EntitySystemSysIDList { get { return _entitySystemSysIDList; } }

        public async Task<bool> GetSystemSysIDList(bool excludeOutsourcing, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemSetting.QuerySystemSysIDs(excludeOutsourcing, cultureID.ToString().ToUpper());
                var response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                var responseObj = new
                {
                    SystemSysIDs = (List<UserSystemSysID>)null
                };
                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                _entitySystemSysIDList = responseObj.SystemSysIDs;

                if (_entitySystemSysIDList != null)
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

        public List<UserSystem> SystemByIdList { get; set; }

        #region - 取得應用系統選單 -
        public async Task<bool> GetAllSystemByIdList(string userID, bool isExcludeOutsourcing, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemSetting.QueryAllSystemByIds(userID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemByIdList = Common.GetJsonDeserializeObject<List<UserSystem>>(response);

                if (SystemByIdList != null)
                {
                    if (isExcludeOutsourcing)
                    {
                        SystemByIdList = SystemByIdList.Where(s => s.IsOutsourcing == EnumYN.N.ToString()).ToList();
                    }
                    else
                    {
                        SystemByIdList = SystemByIdList.Select(s => new UserSystem
                        {
                            SysID = s.SysID,
                            SysNM = (s.IsOutsourcing == EnumYN.N.ToString() ? null : "*") + s.SysNM
                        }).ToList();
                    }

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

        List<SystemSub> _sysSystemSubByIdList = new List<SystemSub>();
        public List<SystemSub> SysSystemSubByIdList { get { return _sysSystemSubByIdList; } }

        public List<SystemSub> SystemSubList { get; private set; }

        #region - 取得應用系統的子系統選單 -
        public async Task<bool> GetSystemSubByIds(string sysID, string userID, EnumCultureID cultureID, bool hasDefaultValue = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID)) return true;

                string apiUrl = API.SystemSetting.QuerySystemSubByIds(sysID, userID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemSubList = Common.GetJsonDeserializeObject<List<SystemSub>>(response);

                EnumSystemID systemID;
                if (Enum.TryParse(sysID, true, out systemID))
                {
                    if (hasDefaultValue)
                    {
                        _sysSystemSubByIdList.Add(new SystemSub
                        {
                            SysID = systemID.ToString(),
                            SysNM = string.Format("{0} ({1})", SysResource.Combobox_DefaultSysID, systemID.ToString())
                        });
                    }

                    if (SystemSubList != null)
                    {
                        foreach (SystemSub item in SystemSubList)
                        {
                            _sysSystemSubByIdList.Add(item);
                        }
                    }
                    else
                    {
                        if (_sysSystemSubByIdList == null)
                        {
                            _sysSystemSubByIdList = new List<SystemSub>();
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }
        #endregion

        List<SystemRoleGroup> _entitySysSystemRoleGroupList = new List<SystemRoleGroup>();
        public List<SystemRoleGroup> EntitySysSystemRoleGroupList { get { return _entitySysSystemRoleGroupList; } }

        public async Task<bool> GetSysSystemRoleGroupList(EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemSetting.QuerySystemRoleGroups(cultureID.ToString().ToUpper());
                var response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                var responseObj = new
                {
                    SystemRoleGroups = (List<SystemRoleGroup>)null
                };
                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                _entitySysSystemRoleGroupList = responseObj.SystemRoleGroups;

                if (_entitySysSystemRoleGroupList == null)
                {
                    _entitySysSystemRoleGroupList = new List<SystemRoleGroup>();
                }

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        
        List<SystemRoleCategoryID> _entitySysSystemRoleCategoryIDList = new List<SystemRoleCategoryID>();
        public List<SystemRoleCategoryID> EntitySysSystemRoleCategoryIDList { get { return _entitySysSystemRoleCategoryIDList; } }

        public async Task<bool> GetSysSystemRoleCategoryIDList(string sysID, string userID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID)) return true;

                var sysid = string.IsNullOrWhiteSpace(sysID) ? null : sysID;
                string apiUrl = API.SystemRoleCategory.QuerySystemRoleCategoryByIds(sysid, userID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                _entitySysSystemRoleCategoryIDList = Common.GetJsonDeserializeObject<List<SystemRoleCategoryID>>(response);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public List<SystemRoleCategory> SystemRoleCategoryByIdList { get; private set; }

        #region - 取得角色類別選單 -
        public async Task<bool> GetSystemRoleCategoryByIdList(string sysID, string userID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID)) return true;

                string apiUrl = API.SystemRoleCategory.QuerySystemRoleCategoryByIds(sysID, userID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemRoleCategoryByIdList = Common.GetJsonDeserializeObject<List<SystemRoleCategory>>(response);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        List<SystemRoleID> _entitySysSystemRoleIDList = new List<SystemRoleID>();
        public List<SystemRoleID> EntitySysSystemRoleIDList { get { return _entitySysSystemRoleIDList; } }

        public async Task<bool> GetSysSystemRoleIDList(string sysID, string userID, EnumCultureID cultureID)
        {
            return await GetSysSystemRoleIDList(sysID, userID, null, cultureID);
        }

        public async Task<bool> GetSysSystemRolesList(string userID, string sysID, EnumCultureID cultureID)
        {
            return await GetSystemRoleByIdList(sysID, userID, null, cultureID);
        }

        public async Task<bool> GetSysSystemRoleIDList(string sysID, string userID, string roleCategoryID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID)) return true;

                var sysid = string.IsNullOrWhiteSpace(sysID) ? null : sysID;
                var roleCategoryid = string.IsNullOrWhiteSpace(roleCategoryID) ? null : roleCategoryID;
                string apiUrl = API.SystemRole.QuerySystemRoleByIds(sysid, userID, roleCategoryid, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                _entitySysSystemRoleIDList = Common.GetJsonDeserializeObject<List<SystemRoleID>>(response);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public List<SystemRole> SystemRoleByIdList { get; private set; }

        #region - 取得角色名稱選單 -
        public async Task<bool> GetSystemRoleByIdList(string sysID, string userID, string roleCategoryID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID)) return true;

                string apiUrl = API.SystemRole.QuerySystemRoleByIds(sysID, userID, roleCategoryID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemRoleByIdList = Common.GetJsonDeserializeObject<List<SystemRole>>(response);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 取得系統角色條件代碼 -
        List<SystemConditionID> _entitySysSystemConditionIDList = new List<SystemConditionID>();
        public List<SystemConditionID> EntitySysSystemConditionIDList { get { return _entitySysSystemConditionIDList; } }

        public async Task<bool> GetSysSystemConditionIDList(string sysID, EnumCultureID cultureID)
        {
            try
            {
                var sysid = string.IsNullOrWhiteSpace(sysID) ? null : sysID;
                string apiUrl = API.SystemSetting.QuerySystemConditionIDs(sysid, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SystemConditionIDs = (List<SystemConditionID>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                _entitySysSystemConditionIDList = responseObj.SystemConditionIDs;

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        public List<SysSystemFunControllerID> EntitySysSystemFunControllerIDList { get; private set; }

        public async Task<bool> GetSysSystemFunControllerIDList(string sysID, string userID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID))
                {
                    EntitySysSystemFunControllerIDList = new List<SysSystemFunControllerID>();
                    return true;
                }

                string apiUrl = API.SystemFunGroup.QuerySystemFunGroupByIds(sysID, userID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                EntitySysSystemFunControllerIDList = Common.GetJsonDeserializeObject<List<SysSystemFunControllerID>>(response);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public List<SysFunGroup> SystemFunGroupByIdList { get; private set; }

        #region - 取得功能群組選單 -
        public async Task<bool> GetSystemFunGroupByIdList(string sysID, string userID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID)) return true;

                string apiUrl = API.SystemFunGroup.QuerySystemFunGroupByIds(sysID, userID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemFunGroupByIdList = Common.GetJsonDeserializeObject<List<SysFunGroup>>(response);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        List<SystemFunName> _entitySystemFunNameList = new List<SystemFunName>();
        public List<SystemFunName> EntitySysSystemFunNameList { get { return _entitySystemFunNameList; } }

        public async Task<bool> GetSystemFunNameList(string sysID, string funControllerID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemFun.QuerySystemFunNames(sysID, funControllerID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    systemFunNameList = (List<SystemFunName>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                _entitySystemFunNameList = responseObj.systemFunNameList;

                if (_entitySystemFunNameList != null)
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

        public List<SystemSub> UserSystemSubList { get; private set; }

        #region - 取得使用者應用系統的子系統清單 -
        protected async Task _GetUserSystemSubList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemSetting.QueryUserSystemSubs(userID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                UserSystemSubList = Common.GetJsonDeserializeObject<List<SystemSub>>(response);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }
        #endregion

        List<SystemFunController> _entitySystemFunControllerList = new List<SystemFunController>();
        public List<SystemFunController> EntitySystemFunControllerList { get { return _entitySystemFunControllerList; } }
        protected async Task _GetSystemFunControllerList(string userID, EnumCultureID cultureID)
        {
            string apiUrl = API.SystemFunGroup.QuerySystemFunGroupByIds(null, userID, cultureID.ToString().ToUpper());
            string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

            var responseObj = Common.GetJsonDeserializeObject<List<SystemFunController>>(response);

            _entitySystemFunControllerList = responseObj.Select(c => new SystemFunController()
            {
                 FunControllerID= c.FunControllerID,
                 FunGroupNM = $"{c.FunGroupNM} ({c.FunControllerID})",
                 SysID= c.SysID,
            }).ToList();
        }

        public List<SysFunGroup> UserSystemFunGroupList { get; private set; }

        #region - 取得使用者應用系統的功能群組清單 -
        protected async Task _GetUserSystemFunGroupList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemFunGroup.QueryUserSystemFunGroups(userID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                UserSystemFunGroupList = Common.GetJsonDeserializeObject<List<SysFunGroup>>(response);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }
        #endregion

        List<SystemFunAction> _entitySystemFunActionList = new List<SystemFunAction>();
        public List<SystemFunAction> EntitySystemFunActionList { get { return _entitySystemFunActionList; } }
        protected async Task _GetSystemFunActionList(EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemFun.QuerySystemFunActions(cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    systemFunActionList = (List<SystemFunAction>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                _entitySystemFunActionList = responseObj.systemFunActionList;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        public List<SysFun> UserSystemFunList { get; private set; }

        #region - 取得使用者系統的功能名稱清單 -
        protected async Task _GetUserSystemFunList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemFun.QueryUserSystemFuns(userID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                UserSystemFunList = Common.GetJsonDeserializeObject<List<SysFun>>(response);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }
        #endregion

        public List<SysPurview> SystemPurviewByIdList { get; private set; }

        #region - 取得資料權限選單 -
        public async Task<bool> GetSystemPurviewByIdList(string sysID, string userID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID)) return true;

                string apiUrl = API.SystemPurview.QuerySystemPurviewByIds(sysID, userID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemPurviewByIdList = Common.GetJsonDeserializeObject<List<SysPurview>>(response);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        public List<SysFunMenu> SystemFunMenuByIdList { get; set; }

        #region - 取得功能選單列表 -
        public async Task<bool> GetSystemFunMenuByIdList(string sysID, string userID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID)) return true;

                string apiUrl = API.SystemFunMenu.QuerySystemFunMenuByIds(sysID, userID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemFunMenuByIdList = Common.GetJsonDeserializeObject<List<SysFunMenu>>(response);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        public Dictionary<string, string> SysSystemFunMenuXAxisList = new Dictionary<string, string>()
            {
                {"1", "1"}, {"2", "2"}, {"3", "3"}, {"4", "4"}, {"5", "5"}
            };

        public Dictionary<string, string> SysSystemFunMenuYAxisList = new Dictionary<string, string>()
            {
                {"1", "1"}, {"2", "2"}, {"3", "3"}, {"4", "4"}, {"5", "5"},
                {"6", "6"}, {"7", "7"}, {"8", "8"}, {"9", "9"}, {"10", "10"},
                {"11", "11"}, {"12", "12"}, {"13", "13"}, {"14", "14"}, {"15", "15"},
                {"16", "16"}, {"17", "17"}, {"18", "18"}, {"19", "19"}, {"20", "20"}
            };

        public List<SysRoleFun> SystemRoleFunList { get; set; }

        #region - 取得系統功能角色清單 -
        public async Task<bool> GetSystemFunRoleList(string sysID, string userID, string funControllerID, string funActionName, EnumCultureID cultureID)
        {
            try
            {
                funControllerID = string.IsNullOrWhiteSpace(funControllerID) ? null : funControllerID;
                funActionName = string.IsNullOrWhiteSpace(funActionName) ? null : funActionName;

                string apiUrl = API.SystemFun.QuerySystemFunRoles(sysID, userID, funControllerID, funActionName, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemRoleFunList = Common.GetJsonDeserializeObject<List<SysRoleFun>>(response);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
        #endregion

        List<Entity_BaseAP.CMCode> _entityBaseTrustTypeList = new List<Entity_BaseAP.CMCode>();
        public List<Entity_BaseAP.CMCode> EntityBaseTrustTypeList { get { return _entityBaseTrustTypeList; } }

        public bool GetBaseTrustTypeList(EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.CMCodePara para = new Entity_BaseAP.CMCodePara()
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNMID,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.IPTrustType,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID.ToString())
                };

                _entityBaseTrustTypeList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectCMCodeList(para);

                if (_entityBaseTrustTypeList != null)
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

        List<Entity_BaseAP.CMCode> _entityBaseSourceTypeList = new List<Entity_BaseAP.CMCode>();
        public List<Entity_BaseAP.CMCode> EntityBaseSourceTypeList { get { return _entityBaseSourceTypeList; } }

        public bool GetBaseSourceTypeList(EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.CMCodePara para = new Entity_BaseAP.CMCodePara()
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNMID,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.IPSourceType,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID.ToString())
                };

                _entityBaseSourceTypeList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectCMCodeList(para);

                if (_entityBaseSourceTypeList != null)
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

        List<Entity_BaseAP.CMCode> _entityBaseRestrictTypeList = new List<Entity_BaseAP.CMCode>();
        public List<Entity_BaseAP.CMCode> EntityBaseRestrictTypeList { get { return _entityBaseRestrictTypeList; } }

        public bool GetBaseRestrictTypeList(EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.CMCodePara para = new Entity_BaseAP.CMCodePara()
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNMID,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.RestrictType,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID.ToString())
                };

                _entityBaseRestrictTypeList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectCMCodeList(para);

                if (_entityBaseRestrictTypeList != null)
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

        public List<CMCode> CMCodeLists { get; private set; }

        public async Task<bool> GetCMCodeTypeList(string userID, string typeCMC, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemSetting.CodeManagement(userID, typeCMC, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                if (response != null)
                {
                    CMCodeLists = Common.GetJsonDeserializeObject<List<CMCode>>(response);
                }

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        List<Entity_BaseAP.CMCode> _entityBaseRecordTypeList = new List<Entity_BaseAP.CMCode>();
        public List<Entity_BaseAP.CMCode> EntityBaseRecordTypeList { get { return _entityBaseRecordTypeList; } }

        public bool GetBaseRecordTypeList(EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.CMCodePara para = new Entity_BaseAP.CMCodePara()
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNMID,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.RecordType,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID.ToString())
                };

                _entityBaseRecordTypeList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectCMCodeList(para);

                if (_entityBaseRecordTypeList != null)
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

        List<Entity_BaseAP.CMCode> _entityBaseDomainNameList = new List<Entity_BaseAP.CMCode>();
        public List<Entity_BaseAP.CMCode> EntityBaseDomainNameList { get { return _entityBaseDomainNameList; } }

        public bool GetBaseDomainNameList(EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.CMCodePara para = new Entity_BaseAP.CMCodePara()
                {
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.DomainName,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID.ToString())
                };

                _entityBaseDomainNameList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectCMCodeList(para);

                if (_entityBaseDomainNameList != null)
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

        List<EntitySys.DomainGroupMenu> _entityEntitySysDomainGroupMenuList;
        public List<EntitySys.DomainGroupMenu> EntitySysDomainGroupMenuList { get { return _entityEntitySysDomainGroupMenuList; } }

        public bool GetDomainGroupMenuList(EnumCultureID cultureID, string domainName)
        {
            try
            {
                EntitySys.DomainGroupMenuPara para = new EntitySys.DomainGroupMenuPara(cultureID.ToString())
                {
                    DomainName = new DBVarChar((string.IsNullOrWhiteSpace(domainName) ? null : domainName))
                };

                _entityEntitySysDomainGroupMenuList = new EntitySys(ConnectionStringSERP, ProviderNameSERP)
                    .SelectDomainGroupMenuList(para);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        #region - 取得使用者名稱 -
        UserRawData _EntitySysUserRawData;
        public UserRawData EntitySysUserRawData { get { return _EntitySysUserRawData; } }

        public async Task<bool> GetUserRawData(string userID)
        {
            try
            {
                var UserIDList = new List<UserRawData>() { (new UserRawData { UserID = string.IsNullOrWhiteSpace(userID) ? null : userID }) };

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                var paraJsonStr = Common.GetJsonSerializeObject(UserIDList);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemFunAssign.QueryUserRawDatas();
                var response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                var responseObj = new
                {
                    UserRawDataList = (List<UserRawData>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                _EntitySysUserRawData = responseObj.UserRawDataList.FirstOrDefault();

                if (_EntitySysUserRawData != null)
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

        List<Entity_BaseAP.RawCMOrgCom> _entityBaseRawCMOrgComList = new List<Entity_BaseAP.RawCMOrgCom>();
        public List<Entity_BaseAP.RawCMOrgCom> EntityBaseRawCMOrgComList { get { return _entityBaseRawCMOrgComList; } }

        public bool GetBaseRawCMOrgComList()
        {
            try
            {
                Entity_BaseAP.RawCMOrgComPara para = new Entity_BaseAP.RawCMOrgComPara();

                _entityBaseRawCMOrgComList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectRawCMOrgComList(para);

                if (_entityBaseRawCMOrgComList != null)
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

        public List<Entity_BaseAP.RawCMOrgUnit> EntityRawCMOrgUnitList { get; private set; }

        public bool GetRawCMOrgUnitList()
        {
            try
            {
                Entity_BaseAP.RawCMOrgUnitPara para = new Entity_BaseAP.RawCMOrgUnitPara();

                EntityRawCMOrgUnitList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectRawCMOrgUnitList(para);

                if (EntityRawCMOrgUnitList != null)
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

        public List<Entity_BaseAP.RawCMBusinessUnit> EntityRawCMBusinessUnitList { get; private set; }

        public bool GetRawCMBusinessUnitList(EnumCultureID cultureId)
        {
            try
            {
                Entity_BaseAP.RawCMBusinessUnitPara para = new Entity_BaseAP.RawCMBusinessUnitPara(cultureId.ToString());

                EntityRawCMBusinessUnitList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectRawCMBusinessUnitList(para);

                if (EntityRawCMBusinessUnitList != null)
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

        public List<Entity_BaseAP.RawCMCountry> EntityRawCMCountryList { get; private set; }

        public bool GetRawCMCountryList(EnumCultureID cultureId)
        {
            try
            {
                Entity_BaseAP.RawCMCountryPara para = new Entity_BaseAP.RawCMCountryPara(cultureId.ToString());

                EntityRawCMCountryList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectRawCMCountryList(para);

                if (EntityRawCMCountryList != null)
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

        #region - 系統角色預設條件表單序列化 -
        protected enum EnumSystemRoleConditionColumnType
        {
            [Description("U.USER_COM_ID")]
            UserComID,

            [Description("U.USER_UNIT_ID")]
            UserUnitID,

            [Description("C.COM_BU")]
            UserBusinessID,

            [Description("U.USER_TEAM_ID")]
            UserTeamID,

            [Description("U.USER_TITLE_ID")]
            UserTitleID,

            [Description("U.USER_WORK_ID")]
            UserWorkID,

            [Description("O.USER_COM_ID")]
            UserOrgWorkCom,

            [Description("O.USER_AREA")]
            UserOrgArea,

            [Description("O.USER_GROUP")]
            UserOrgGroup,

            [Description("O.USER_PLACE")]
            UserOrgPlace,

            [Description("O.USER_DEPT")]
            UserOrgDept,

            [Description("O.USER_TEAM")]
            UserOrgTeam,

            [Description("O.USER_TITLE")]
            UserOrgTitle,

            [Description("O.USER_JOB_TITLE")]
            UserOrgJobTitle,

            [Description("O.USER_BIZ_TITLE")]
            UserOrgBizTitle,

            [Description("O.USER_LEVEL")]
            UserOrgLevel,

            [Description("U.USER_SALARY_COM_ID")]
            UserSalaryComID,

            [Description("C.COM_COUNTRY")]
            UserCountryID,

            [Description("S.COM_COUNTRY")]
            UserSalaryCountryID
        }

        private class SystemRoleConditionFilter<TValue>
        {
            public int sortOrdor;
            public string id { get; set; }
            public string label { get; set; }
            public string type { get; set; }
            public string input { get; set; }
            public TValue values { get; set; }
        }

        protected string GetSystemRoleConditionFilterJsonString(EnumCultureID cultureId)
        {
            JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
            List<SystemRoleConditionFilter<List<SelectListItem>>> result = new List<SystemRoleConditionFilter<List<SelectListItem>>>();
            result.AddRange(_GetSystemRoleConditionFilterListByRawCMOrgCom());
            result.AddRange(_GetSystemRoleConditionFilterListByCMCode(cultureId));
            result.Add(_GetSystemRoleConditionFilterByRawCMOrgUnit());
            result.Add(_GetSystemRoleConditionFilterRawCMBusinessUnit(cultureId));
            result.AddRange(_GetSystemRoleConditionFilterListByRawCMCountry(cultureId));
            result = result.OrderBy(o => o.sortOrdor).ToList();

            return jsonConvert.Serialize(result);
        }

        private SystemRoleConditionFilter<List<SelectListItem>> _GetSystemRoleConditionFilterByRawCMOrgUnit()
        {
            SystemRoleConditionFilter<List<SelectListItem>> result = new SystemRoleConditionFilter<List<SelectListItem>>();
            if (GetRawCMOrgUnitList())
            {
                result = new SystemRoleConditionFilter<List<SelectListItem>>
                {
                    sortOrdor = 13,
                    id = EnumSystemRoleConditionColumnType.UserUnitID.ToString(),
                    label = SysResource.Label_UserUnitNM,
                    type = QueryCondition.EnumFieldType.String.ToString(),
                    input = QueryCondition.EnumInputType.Select.ToString(),
                    values = EntityRawCMOrgUnitList.ToList().Select(e => new SelectListItem
                    {
                        Value = e.ItemValue(),
                        Text = e.ItemText()
                    }).ToList()
                };
            }
            return result;
        }

        private SystemRoleConditionFilter<List<SelectListItem>> _GetSystemRoleConditionFilterRawCMBusinessUnit(EnumCultureID cultureId)
        {
            SystemRoleConditionFilter<List<SelectListItem>> result = new SystemRoleConditionFilter<List<SelectListItem>>();

            if (GetRawCMBusinessUnitList(cultureId))
            {
                result = new SystemRoleConditionFilter<List<SelectListItem>>
                {
                    sortOrdor = 14,
                    id = EnumSystemRoleConditionColumnType.UserBusinessID.ToString(),
                    label = SysResource.Label_UserBusinessNM,
                    type = QueryCondition.EnumFieldType.String.ToString(),
                    input = QueryCondition.EnumInputType.Select.ToString(),
                    values = EntityRawCMBusinessUnitList.ToList().Select(e => new SelectListItem
                    {
                        Value = e.ItemValue(),
                        Text = e.ItemText()
                    }).ToList()
                };
            }

            return result;
        }

        private List<SystemRoleConditionFilter<List<SelectListItem>>> _GetSystemRoleConditionFilterListByRawCMOrgCom()
        {
            List<SystemRoleConditionFilter<List<SelectListItem>>> result = new List<SystemRoleConditionFilter<List<SelectListItem>>>();
            if (GetBaseRawCMOrgComList())
            {
                result.Add(new SystemRoleConditionFilter<List<SelectListItem>>
                {
                    sortOrdor = 0,
                    id = EnumSystemRoleConditionColumnType.UserOrgWorkCom.ToString(),
                    label = SysResource.Label_UserOrgWorkCompNM,
                    type = QueryCondition.EnumFieldType.String.ToString(),
                    input = QueryCondition.EnumInputType.Select.ToString(),
                    values = EntityBaseRawCMOrgComList.ToList().Select(e => new SelectListItem
                    {
                        Value = e.ItemValue(),
                        Text = e.ItemText()
                    }).ToList()
                });
                result.Add(new SystemRoleConditionFilter<List<SelectListItem>>
                {
                    sortOrdor = 12,
                    id = EnumSystemRoleConditionColumnType.UserComID.ToString(),
                    label = SysResource.Label_UserCompNM,
                    type = QueryCondition.EnumFieldType.String.ToString(),
                    input = QueryCondition.EnumInputType.Select.ToString(),
                    values = EntityBaseRawCMOrgComList.ToList().Select(e => new SelectListItem
                    {
                        Value = e.ItemValue(),
                        Text = e.ItemText()
                    }).ToList()
                });
                result.Add(new SystemRoleConditionFilter<List<SelectListItem>>
                {
                    sortOrdor = 17,
                    id = EnumSystemRoleConditionColumnType.UserSalaryComID.ToString(),
                    label = SysResource.Label_UserSalaryComID,
                    type = QueryCondition.EnumFieldType.String.ToString(),
                    input = QueryCondition.EnumInputType.Select.ToString(),
                    values = EntityBaseRawCMOrgComList.ToList().Where(e => e.IsSalaryCOM.GetValue() == EnumYN.Y.ToString()).Select(e => new SelectListItem
                    {
                        Value = e.ItemValue(),
                        Text = e.ItemText()
                    }).ToList()
                });

            }

            return result;
        }

        private List<SystemRoleConditionFilter<List<SelectListItem>>> _GetSystemRoleConditionFilterListByCMCode(EnumCultureID cultureId)
        {
            List<SystemRoleConditionFilter<List<SelectListItem>>> result = new List<SystemRoleConditionFilter<List<SelectListItem>>>();

            try
            {
                GetCMCodeDictionary(cultureId, Entity_BaseAP.EnumCMCodeItemTextType.CodeNMID, Entity_BaseAP.EnumCMCodeKind.OrgArea, Entity_BaseAP.EnumCMCodeKind.OrgGroup, Entity_BaseAP.EnumCMCodeKind.OrgPlace, Entity_BaseAP.EnumCMCodeKind.OrgDept, Entity_BaseAP.EnumCMCodeKind.OrgTeam, Entity_BaseAP.EnumCMCodeKind.OrgPTitle, Entity_BaseAP.EnumCMCodeKind.OrgPTitle2, Entity_BaseAP.EnumCMCodeKind.OrgLevel, Entity_BaseAP.EnumCMCodeKind.OrgTitle, Entity_BaseAP.EnumCMCodeKind.Title, Entity_BaseAP.EnumCMCodeKind.Work);

                result = CMCodeDictionary.Select(s =>
                {
                    int sortOrdor = 0;
                    string id = null;
                    string label = null;

                    switch (s.Key)
                    {
                        case Entity_BaseAP.EnumCMCodeKind.OrgArea:
                            sortOrdor = 1;
                            id = EnumSystemRoleConditionColumnType.UserOrgArea.ToString();
                            label = SysResource.Label_UserOrgAreaNM;
                            break;
                        case Entity_BaseAP.EnumCMCodeKind.OrgGroup:
                            sortOrdor = 2;
                            id = EnumSystemRoleConditionColumnType.UserOrgGroup.ToString();
                            label = SysResource.Label_UserOrgGroupNM;
                            break;
                        case Entity_BaseAP.EnumCMCodeKind.OrgPlace:
                            sortOrdor = 3;
                            id = EnumSystemRoleConditionColumnType.UserOrgPlace.ToString();
                            label = SysResource.Label_UserOrgPlaceNM;
                            break;
                        case Entity_BaseAP.EnumCMCodeKind.OrgDept:
                            sortOrdor = 4;
                            id = EnumSystemRoleConditionColumnType.UserOrgDept.ToString();
                            label = SysResource.Label_UserOrgDeptNM;
                            break;
                        case Entity_BaseAP.EnumCMCodeKind.OrgTeam:
                            sortOrdor = 5;
                            id = EnumSystemRoleConditionColumnType.UserOrgTeam.ToString();
                            label = SysResource.Label_UserOrgTeamNM;
                            break;
                        case Entity_BaseAP.EnumCMCodeKind.OrgTitle:
                            sortOrdor = 6;
                            id = EnumSystemRoleConditionColumnType.UserOrgTitle.ToString();
                            label = SysResource.Label_UserOrgTitleNM;
                            break;
                        case Entity_BaseAP.EnumCMCodeKind.OrgPTitle:
                            sortOrdor = 7;
                            id = EnumSystemRoleConditionColumnType.UserOrgJobTitle.ToString();
                            label = SysResource.Label_UserOrgPTitleNM;
                            break;
                        case Entity_BaseAP.EnumCMCodeKind.OrgPTitle2:
                            sortOrdor = 8;
                            id = EnumSystemRoleConditionColumnType.UserOrgBizTitle.ToString();
                            label = SysResource.Label_UserOrgPTitle2NM;
                            break;
                        case Entity_BaseAP.EnumCMCodeKind.OrgLevel:
                            sortOrdor = 9;
                            id = EnumSystemRoleConditionColumnType.UserOrgLevel.ToString();
                            label = SysResource.Label_UserOrgLevelNM;
                            break;
                        case Entity_BaseAP.EnumCMCodeKind.Title:
                            sortOrdor = 10;
                            id = EnumSystemRoleConditionColumnType.UserTitleID.ToString();
                            label = SysResource.Label_UserTitleNM;
                            break;
                        case Entity_BaseAP.EnumCMCodeKind.Work:
                            sortOrdor = 11;
                            id = EnumSystemRoleConditionColumnType.UserWorkID.ToString();
                            label = SysResource.Label_UserWorkNM;
                            break;
                    }

                    return new SystemRoleConditionFilter<List<SelectListItem>>
                    {
                        sortOrdor = sortOrdor,
                        id = id,
                        label = label,
                        type = QueryCondition.EnumFieldType.String.ToString(),
                        input = QueryCondition.EnumInputType.Select.ToString(),
                        values = s.Value.Select(e => new SelectListItem
                        {
                            Value = e.ItemValue(),
                            Text = e.ItemText()
                        }).ToList()
                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return result;
        }

        private List<SystemRoleConditionFilter<List<SelectListItem>>> _GetSystemRoleConditionFilterListByRawCMCountry(EnumCultureID cultureId)
        {
            List<SystemRoleConditionFilter<List<SelectListItem>>> result = new List<SystemRoleConditionFilter<List<SelectListItem>>>();

            if (GetRawCMCountryList(cultureId))
            {
                result.Add(new SystemRoleConditionFilter<List<SelectListItem>>
                {
                    sortOrdor = 15,
                    id = EnumSystemRoleConditionColumnType.UserCountryID.ToString(),
                    label = SysResource.Label_UserCountryID,
                    type = QueryCondition.EnumFieldType.String.ToString(),
                    input = QueryCondition.EnumInputType.Select.ToString(),
                    values = EntityRawCMCountryList.Select(e => new
                    {
                        Value = e.ItemValue(),
                        Text = e.ItemText()
                    }).Distinct().Select(e => new SelectListItem
                    {
                        Value = e.Value,
                        Text = e.Text
                    }).ToList()
                });
                result.Add(new SystemRoleConditionFilter<List<SelectListItem>>
                {
                    sortOrdor = 16,
                    id = EnumSystemRoleConditionColumnType.UserSalaryCountryID.ToString(),
                    label = SysResource.Label_UserSalaryCountryID,
                    type = QueryCondition.EnumFieldType.String.ToString(),
                    input = QueryCondition.EnumInputType.Select.ToString(),
                    values = EntityRawCMCountryList.Where(e => e.IsSalaryCOM.GetValue() == EnumYN.Y.ToString()).Select(e => new
                    {
                        Value = e.ItemValue(),
                        Text = e.ItemText()
                    }).Distinct().Select(e => new SelectListItem
                    {
                        Value = e.Value,
                        Text = e.Text
                    }).ToList()
                });
            }

            return result;
        }
        #endregion

        #endregion

        #region - API -
        public List<SysSystemAPIGroup> SystemAPIGroupByIdList { get; set; }

        #region - API群組選單 -
        public async Task<bool> GetSystemAPIGroupByIdList(string sysID, string userID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID)) return true;

                string apiUrl = API.SystemAPIGroup.QuerySystemAPIGroupByIds(sysID, userID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemAPIGroupByIdList = Common.GetJsonDeserializeObject<List<SysSystemAPIGroup>>(response);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        List<SystemAPIFuntions> _entitySystemAPIFuntionList;
        public List<SystemAPIFuntions> EntitySystemAPIFuntionList { get { return _entitySystemAPIFuntionList; } }

        public async Task<bool> GetSystemAPIFuntionList(string sysID, string apiGroup, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemAPI.QuerySystemAPIFuntions(sysID, apiGroup, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    systemAPIFuntions = (List<SystemAPIFuntions>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                _entitySystemAPIFuntionList = responseObj.systemAPIFuntions;

                if (_entitySystemAPIFuntionList != null)
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

        public List<SysSystemAPIFun> SystemAPIByIdList { get; set; }

        #region - API功能選單 -
        public async Task<bool> GetSysSystemAPIByIdList(string sysID, string userID, string apiGroup, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID) || string.IsNullOrWhiteSpace(apiGroup)) return true;

                string apiUrl = API.SystemAPI.QuerySystemAPIByIds(sysID, userID, apiGroup, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemAPIByIdList = Common.GetJsonDeserializeObject<List<SysSystemAPIFun>>(response);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        public SystemAPIFullName SysSystemAPIFullName { get; set; }

        #region - API 主鍵名稱 -
        public async Task<bool> GetSysSystemAPIFullName(string sysID, string userID, string apiGroupID, string apiFunID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemAPI.QuerySystemAPIFullName(sysID, userID, apiGroupID, apiFunID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SysSystemAPIFullName = Common.GetJsonDeserializeObject<SystemAPIFullName>(response);

                if (SysSystemAPIFullName != null)
                {
                    SysSystemAPIFullName.SysNM = $"{SysSystemAPIFullName.SysNM} ({sysID})";
                    SysSystemAPIFullName.APIGroupNM = $"{SysSystemAPIFullName.APIGroupNM} ({apiGroupID})";
                    SysSystemAPIFullName.APIFunNM = $"{SysSystemAPIFullName.APIFunNM} ({apiFunID})";
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

        #endregion

        #region - Event -
        List<SystemEventTarget> _entitySystemEventTargetList;
        public List<SystemEventTarget> EntitySystemEventTargetList { get { return _entitySystemEventTargetList; } }

        public async Task<bool> GetSystemEventTargetList(string eventGroupID, string eventID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemEvent.QuerySystemEventTargetIDs(EnumSystemID.ERPAP.ToString(), eventGroupID, eventID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SystemEventTargetIDs = (List<SystemEventTarget>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                _entitySystemEventTargetList = responseObj.SystemEventTargetIDs;

                if (_entitySystemEventTargetList != null)
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

        List<EntitySys.SysSystemEventGroup> _entitySysSystemEventGroupList = new List<EntitySys.SysSystemEventGroup>();
        public List<EntitySys.SysSystemEventGroup> EntitySysSystemEventGroupList { get { return _entitySysSystemEventGroupList; } }

        public bool GetSysSystemEventGroupList(string sysID, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SysSystemEventGroupPara para = new EntitySys.SysSystemEventGroupPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID))
                };

                _entitySysSystemEventGroupList = new EntitySys(ConnectionStringSERP, ProviderNameSERP)
                    .SelectSystemEventGroupList(para);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public List<SysSystemEventGroup> SysSystemEventGroupByIdList { get; set; }

        #region - 事件群組選單 -
        public async Task<bool> GetSysSystemEventGroupByIdList(string sysID, string userID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID)) return true;

                string apiUrl = API.SystemEventGroup.QuerySystemEventGroupByIds(sysID, userID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SysSystemEventGroupByIdList = Common.GetJsonDeserializeObject<List<SysSystemEventGroup>>(response);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        public List<SysSystemEvent> SysSystemEventByIdList { get; private set; }

        #region - 事件名稱選單 -
        public async Task<bool> GetSysSystemEventByIdList(string sysID, string userID, string eventGroupID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID) || string.IsNullOrWhiteSpace(eventGroupID)) return true;

                string apiUrl = API.SystemEvent.QuerySystemEventByIds(sysID, userID, eventGroupID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SysSystemEventByIdList = Common.GetJsonDeserializeObject<List<SysSystemEvent>>(response);
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
        #endregion

        EntitySys.SystemEvent _entitySystemEvent;
        public EntitySys.SystemEvent EntitySystemEvent { get { return _entitySystemEvent; } }

        public bool GetSystemEvent(string sysID, string eventGroupID, string eventID, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SystemEventPara para = new EntitySys.SystemEventPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID)),
                    EventGroupID = new DBVarChar((string.IsNullOrWhiteSpace(eventGroupID) ? null : eventGroupID)),
                    EventID = new DBVarChar((string.IsNullOrWhiteSpace(eventID) ? null : eventID))
                };

                _entitySystemEvent = new EntitySys(ConnectionStringSERP, ProviderNameSERP)
                    .SelectSystemEvent(para);

                if (_entitySystemEvent != null)
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

        public SysSystemEventFullName SystemEventFullName { get; private set; }

        #region - 事件主鍵名稱 -
        public async Task<bool> GetSysSystemEventFullName(string sysID, string userID, string eventGroupID, string eventID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemEvent.QuerySystemEventFullName(sysID, userID, eventGroupID, eventID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemEventFullName = Common.GetJsonDeserializeObject<SysSystemEventFullName>(response);

                if (SystemEventFullName != null)
                {
                    SystemEventFullName.SysNM = $"{SystemEventFullName.SysNM} ({sysID})";
                    SystemEventFullName.EventGroupNM = $"{SystemEventFullName.EventGroupNM} ({eventGroupID})";
                    SystemEventFullName.EventNM = $"{SystemEventFullName.EventNM} ({eventID})";
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
        #endregion

        #endregion

        #region - EDI -
        List<SystemEDIFlows> _entitySystemEDIFlowList = new List<SystemEDIFlows>();
        public List<SystemEDIFlows> EntitySysSystemEDIFlowList { get { return _entitySystemEDIFlowList; } }

        public async Task<bool> GetSystemEDIFlowIDList(string sysID, EnumCultureID cultureID, bool useNullFlow = false)
        {
            try
            {
                string apiUrl = API.SystemEDIFlow.QuerySystemEDIFlowByIds(sysID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    systemEDIFlowByIds = (List<SystemEDIFlows>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                _entitySystemEDIFlowList = responseObj.systemEDIFlowByIds;

                if (useNullFlow)
                {
                    _entitySystemEDIFlowList.Add(new SystemEDIFlows
                    {
                        EDIFlowID = EnumEDIFlowItem.NULL.ToString(),
                        EDIFlowNM = string.Format("{0} ({1})", SysSystemEDIJobLog.Label_EDINoData, EnumEDIFlowItem.NULL.ToString())
                    }
                    );
                }

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        List<SystemEDIJobIDs> _entitySysSystemEDIJobIDList = new List<SystemEDIJobIDs>();
        public List<SystemEDIJobIDs> EntitySysSystemEDIJobIDList { get { return _entitySysSystemEDIJobIDList; } }


        public async Task<bool> GetSystemEDIJobIDList(string userID, string sysID, string ediFlowID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemEDIJob.QuerySystemEDIJobByIds(userID, sysID, ediFlowID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                _entitySysSystemEDIJobIDList = Common.GetJsonDeserializeObject<List<SystemEDIJobIDs>>(response);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        List<SystemEDIJobID> _sysSystemEDIJobList = new List<SystemEDIJobID>();
        public List<SystemEDIJobID> SysSystemEDIJobList { get { return _sysSystemEDIJobList; } }

        public async Task<bool> GetSysSystemEDIJobList(string userID, string sysID, string ediFlowID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemEDIJob.QuerySystemEDIJobByIds(userID, sysID, ediFlowID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                _sysSystemEDIJobList = Common.GetJsonDeserializeObject<List<SystemEDIJobID>>(response);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        List<EntitySys.SCHFrequency> _entitySCHFrequencyList = new List<EntitySys.SCHFrequency>();
        public List<EntitySys.SCHFrequency> EntitySCHFrequencyList { get { return _entitySCHFrequencyList; } }

        public bool GetSCHFrequencyList(EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SCHFrequencyPara para = new EntitySys.SCHFrequencyPara(cultureID.ToString());

                _entitySCHFrequencyList = new EntitySystemEDIFlow(ConnectionStringSERP, ProviderNameSERP)
                    .SelectSCHFrequencyList(para);
                if (_entitySCHFrequencyList != null)
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

        List<EDIJobTypes> _entityEDIJobTypeList = new List<EDIJobTypes>();
        public List<EDIJobTypes> EntityEDIJobTypeList { get { return _entityEDIJobTypeList; } }

        public async Task<bool> GetEDIJobTypeList(string userID, string typeCMC, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemSetting.CodeManagement(userID, typeCMC, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                if (response != null)
                {
                    _entityEDIJobTypeList = Common.GetJsonDeserializeObject<List<EDIJobTypes>>(response);
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
        List<UserSystemWorkFlowIDs> _entityUserSystemWorkFlowIDList = new List<UserSystemWorkFlowIDs>();
        public List<UserSystemWorkFlowIDs> EntityUserSystemWorkFlowIDList { get { return _entityUserSystemWorkFlowIDList; } }

        public async Task<bool> GetUserSystemWorkFlowIDList(string userID, string sysID, string wfFlowGroupID, EnumCultureID cultureID)
        {
            try
            {
				if (string.IsNullOrWhiteSpace(wfFlowGroupID))
				{
					return false;
				}

				string apiUrl = API.SystemWorkFlowNode.QuerySysUserSystemWorkFlowID(userID, sysID, cultureID.ToString().ToUpper(), wfFlowGroupID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SysUserSystemWorkFlowIDList = (List<UserSystemWorkFlowIDs>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                _entityUserSystemWorkFlowIDList = responseObj.SysUserSystemWorkFlowIDList;

                if (_entityUserSystemWorkFlowIDList != null)
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

        List<SystemWorkFlowGroupIDs> _entitySystemWorkFlowGroupIDList = new List<SystemWorkFlowGroupIDs>();
        public List<SystemWorkFlowGroupIDs> EntitySystemWorkFlowGroupIDList { get { return _entitySystemWorkFlowGroupIDList; } }

        public async Task<bool> GetSystemWorkFlowGroupIDList(string sysID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemWorkFlowGroup.QuerySystemWorkFlowGroupIDs(sysID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SystemWorkFlowGroupIDs = (List<SystemWorkFlowGroupIDs>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                _entitySystemWorkFlowGroupIDList = responseObj.SystemWorkFlowGroupIDs;

                if (_entitySystemWorkFlowGroupIDList != null)
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

        List<SystemWorkFlowNodeIDs> _entitySystemWorkFlowNodeIDList;
        public List<SystemWorkFlowNodeIDs> EntitySystemWorkFlowNodeIDList { get { return _entitySystemWorkFlowNodeIDList; } }

        public async Task<bool> GetSystemWorkFlowNodeIDList(string sysID, string wfFlowID, string wfFlowVer, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sysID) || string.IsNullOrWhiteSpace(wfFlowID) || string.IsNullOrWhiteSpace(wfFlowVer))
                {
                    return false;
                }

                string apiUrl = API.SystemWorkFlowNodeDetail.QuerySystemWorkFlowNodeIDs(sysID, wfFlowID, wfFlowVer, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SystemWorkFlowNodeIDs = (List<SystemWorkFlowNodeIDs>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                _entitySystemWorkFlowNodeIDList = responseObj.SystemWorkFlowNodeIDs;

                if (_entitySystemWorkFlowNodeIDList != null)
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

        SystemWorkFlowNode _entitySystemWorkFlowNode = new SystemWorkFlowNode();
        public SystemWorkFlowNode EntitySystemWorkFlowNode { get { return _entitySystemWorkFlowNode; } }

        public async Task<bool> GetSystemWorkFlowNode(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemWorkFlowNode.QuerySystemWorkFlowNode(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    systemWorkFlowNode = (SystemWorkFlowNode)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                _entitySystemWorkFlowNode = responseObj.systemWorkFlowNode;

                if (_entitySystemWorkFlowNode != null)
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

        public bool UpdateWorkFlowChart(string userID, string sysID, string wfFlowID, string wfFlowVer)
        {
            try
            {
                EnumSystemID enumSysID = Utility.GetSystemID(sysID);

                string filePath = Path.Combine(new string[] { ConfigurationManager.AppSettings[EnumAppSettingKey.FilePath.ToString()],
                                                                  Common.GetEnumDesc(EnumFilePathFolder.WorkFlow),
                                                                  sysID });

                LionTech.Utility.ERP.WorkFlow.GenerateWorkFlowChart(userID, enumSysID, wfFlowID, wfFlowVer,
                    ConnectionStringSERP, ProviderNameSERP,
                    filePath);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - Funtion -           
        public string GetJsonFormSelectItem(IEnumerable<SysModel.ISelectItem> entityList, bool isBlank = false)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

            if (isBlank)
            {
                selectListItems.Add(new SelectListItem { Value = string.Empty, Text = string.Empty });
            }

            if (entityList != null)
            {
                selectListItems.AddRange(entityList.Select((entity => new SelectListItem { Value = entity.ItemValue(), Text = entity.ItemText() })));
            }

            return jsSerializer.Serialize(selectListItems);
        }

        public string GetJsonToSelectItem<T>(List<T> entityList, bool isBlank = false)
        {
            string returnJsonString = string.Empty;
            StringBuilder jsonString = new StringBuilder();

            if (isBlank)
            {
                jsonString.Append("{");
                jsonString.Append(string.Concat(new object[] { "\"Text\"", ":\"\", " }));
                jsonString.Append(string.Concat(new object[] { "\"Value\"", ":\"\" " }));
                jsonString.Append("}");
            }

            if (entityList != null)
            {
                entityList.ForEach(entity =>
                {
                    if (entity.GetType().GetInterface(typeof(SysModel.ISelectItem).Name) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(jsonString.ToString()))
                            jsonString.Append(",");
                        jsonString.Append("{");
                        jsonString.Append(string.Concat(new object[] { "\"Text\"", ":\"", ((SysModel.ISelectItem)entity).ItemText(), "\", " }));
                        jsonString.Append(string.Concat(new object[] { "\"Value\"", ":\"", ((SysModel.ISelectItem)entity).ItemValue(), "\" " }));
                        jsonString.Append("}");
                    }
                });
            }

            returnJsonString = string.Concat(new object[] { " [ ", jsonString.ToString(), " ] " });

            return returnJsonString;
        }

        /// <summary>
        /// 取的選單預設單一文字
        /// </summary>
        /// <param name="selectItems">集合</param>
        /// <param name="value">預設值</param>
        /// <returns>預設文字</returns>
        public string GetSelectedText(IEnumerable<SysModel.ISelectItem> selectItems, string value)
        {
            if (selectItems == null)
            {
                return null;
            }
            return selectItems.Where(a => a.ItemValue() == value).Select(s => s.ItemText()).SingleOrDefault();
        }

        public Dictionary<string, string> GetDictionaryFormSelectItem(IEnumerable<SysModel.ISelectItem> entityList, bool isBlank = false)
        {
            Dictionary<string, string> listItem = new Dictionary<string, string>();
            if (isBlank)
                listItem.Add(string.Empty, string.Empty);
            if (entityList != null)
            {
                foreach (var selectItem in entityList)
                {
                    listItem.Add(selectItem.ItemValue(), selectItem.ItemText());
                }
            }
            return listItem;
        }
        #endregion


        #region - Culture -

        public List<SystemCultureID> SystemCultureIDs { get; private set; }
        #region - 取得語系代碼選單 -
        /// <summary>
        /// 取得語系代碼選單
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetSystemCultureIDs(string userID)
        {
            try
            {
                string apiUrl = API.SystemCultureSetting.QuerySystemCultureIDs(userID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                SystemCultureIDs = Common.GetJsonDeserializeObject<List<SystemCultureID>>(response);
                if (SystemCultureIDs != null)
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

        #endregion
    }
}