using System.Collections.Generic;

namespace LionTech.Entity.ERP.EDIService
{
    public class EntityEventPara : EntityEDIService
    {
        public EntityEventPara(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SysSystemMenuEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar FunMenu;
            public DBNVarChar FunMenuNMZHTW;
            public DBNVarChar FunMenuNMZHCN;
            public DBNVarChar FunMenuNMENUS;
            public DBNVarChar FunMenuNMTHTH;
            public DBNVarChar FunMenuNMJAJP;
            public DBVarChar DefaultMenuID;
            public DBChar IsDisable;
            public DBVarChar SortOrder;
        }

        public class SysSystemMenuDelete : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar FunMenu;
        }

        public class SysSystemFunMenuEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
            public List<DBVarChar> FunMenuList;
        }

        public class SysSystemFunGroupEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBNVarChar FunGroupZHTW;
            public DBNVarChar FunGroupZHCN;
            public DBNVarChar FunGroupENUS;
            public DBNVarChar FunGroupTHTH;
            public DBNVarChar FunGroupJAJP;
            public DBVarChar SortOrder;
        }

        public class SysSystemFunGroupDelete : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar FunControllerID;
        }

        public class SysSystemFunEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar SubSysID;
            public DBVarChar PurviewID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
            public DBNVarChar FunNMzhTW;
            public DBNVarChar FunNMzhCN;
            public DBNVarChar FunNMenUS;
            public DBNVarChar FunNMthTH;
            public DBNVarChar FunNMjaJP;
            public DBChar IsOutside;
            public DBChar IsDisable;
            public DBVarChar SortOrder;
            public List<DBVarChar> RoleIDList;
        }

        public class SysSystemFunDelete : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
        }

        public class SysSystemFunAssignEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
            public List<DBVarChar> UserIDList;
        }

        public class SysSystemAPIGroupEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar APIControllerID;
            public DBNVarChar APIGroupzhTW;
            public DBNVarChar APIGroupzhCN;
            public DBNVarChar APIGroupenUS;
            public DBNVarChar APIGroupthTH;
            public DBNVarChar APIGroupjaJP;
            public DBVarChar SortOrder;
        }

        public class SysSystemAPIGroupDelete : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar APIControllerID;
        }

        public class SysSystemAPIEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar APIControllerID;
            public DBVarChar APIActionName;
            public DBNVarChar APINMzhTW;
            public DBNVarChar APINMzhCN;
            public DBNVarChar APINMenUS;
            public DBNVarChar APINMthTH;
            public DBNVarChar APINMjaJP;
            public DBVarChar APIPara;
            public DBVarChar APIReturn;
            public DBNVarChar APIParaDesc;
            public DBNVarChar APIReturnContent;
            public DBChar IsOutside;
            public DBChar IsDisable;
            public DBVarChar SortOrder;
        }

        public class SysSystemAPIDelete : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar APIControllerID;
            public DBVarChar APIActionName;
        }

        public class SysSystemRoleCategEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar RoleCategoryID;
            public DBNVarChar RoleCategoryNMzhTW;
            public DBNVarChar RoleCategoryNMzhCN;
            public DBNVarChar RoleCategoryNMenUS;
            public DBNVarChar RoleCategoryNMthTH;
            public DBNVarChar RoleCategoryNMjaJP;
            public DBVarChar SortOrder;
        }

        public class SysSystemRoleCategDelete : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar RoleCategoryID;
        }

        public class SysSystemRoleEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar RoleCategoryID;
            public DBVarChar RoleID;
            public DBNVarChar RoleNMzhTW;
            public DBNVarChar RoleNMzhCN;
            public DBNVarChar RoleNMenUS;
            public DBNVarChar RoleNMthTH;
            public DBNVarChar RoleNMjaJP;
            public DBChar IsMaster;
        }

        public class SysSystemRoleDelete : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar RoleID;
        }

        public class SysSystemPurviewEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar PurviewID;
            public DBNVarChar PurviewNMzhTW;
            public DBNVarChar PurviewNMzhCN;
            public DBNVarChar PurviewNMenUS;
            public DBNVarChar PurviewNMthTH;
            public DBNVarChar PurviewNMjaJP;
            public DBVarChar SortOrder;
            public DBNVarChar Remark;
        }

        public class SysSystemPurviewDelete : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar SysID;
            public DBVarChar PurviewID;
        }

        public class SysRoleGroupEdit : DBTableRow
        {
            public DBVarChar RoleGroupID;
            public DBNVarChar RoleGroupNMzhTW;
            public DBNVarChar RoleGroupNMzhCN;
            public DBNVarChar RoleGroupNMenUS;
            public DBNVarChar RoleGroupNMthTH;
            public DBNVarChar RoleGroupNMjaJP;
            public DBNVarChar RoleGroupNMkoKR;
            public DBVarChar SortOrder;
            public DBNVarChar Remark;
        }

        public class SysRoleGroupDelete : DBTableRow
        {
            public DBVarChar RoleGroupID;
        }

        public class SysRoleGroupCollectEdit : DBTableRow
        {
            public DBVarChar RoleGroupID;
            public List<DBVarChar> SysRoleIDList;
        }

        public class SysUserSystemRoleEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar UserID;
            public DBVarChar RoleGroupID;
            public List<DBVarChar> RoleIDList;
        }

        public class SysUserFunctionEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar UserID;
            public List<DBVarChar> FunctionList;
        }

        public class SysUserPurviewEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;
            public DBVarChar UserID;
            public List<Purview> PurviewList;

            public class Purview : DBTableRow
            {
                public DBVarChar PurviewID;
                public DBVarChar CodeType;
                public List<Item> ItemList;

                public class Item : DBTableRow
                {
                    public DBVarChar CodeID;
                    public DBChar PurviewOP;
                }
            }
        }
        
        #region - opagm20 -
        public class Opagm20Edit : DBTableRow
        {
            public DBVarChar stfn_stfn;
            public DBNVarChar stfn_cname;
            public DBChar stfn_sts;
            public DBChar stfn_comp;
            public DBChar stfn_prof;
            public DBChar stfn_team;
            public DBChar stfn_job1;
            public DBChar stfn_job2;
            public DBVarChar stfn_email;
            public DBVarChar stfn_property;
        }
        #endregion

        #region - Sys SystemWFGroup -
        public class SysSystemWFGroupEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBVarChar SysID;
            public DBVarChar WFFlowGroupID;
            public DBNVarChar WFFlowGroupzhTW;
            public DBNVarChar WFFlowGroupzhCN;
            public DBNVarChar WFFlowGroupenUS;
            public DBNVarChar WFFlowGroupthTH;
            public DBNVarChar WFFlowGroupjaJP;
            public DBNVarChar WFFlowGroupkoKR;
            public DBVarChar SortOrder;
        }

        public class SysSystemWFGroupDelete : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBVarChar SysID;
            public DBVarChar WFFlowGroupID;
        }
        #endregion

        #region - Sys SystemWF -
        public class SysSystemWFEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBVarChar SysID;
            public DBVarChar WFFlowGroupID;
            public DBVarChar WFFlowID;

            public DBNVarChar WFFlowzhTW;
            public DBNVarChar WFFlowzhCN;
            public DBNVarChar WFFlowenUS;
            public DBNVarChar WFFlowthTH;
            public DBNVarChar WFFlowjaJP;
            public DBNVarChar WFFlowkoKR;

            public DBChar WFFlowVer;

            public DBVarChar FlowType;
            public DBVarChar FlowManUserID;
            public DBChar EnableDate;
            public DBChar DisableDate;

            public DBVarChar SortOrder;
            public DBVarChar MsgSysID;
            public DBVarChar MsgControllerID;
            public DBVarChar MsgActionName;
        }

        public class SysSystemWFDelete : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBVarChar SysID;
            public DBVarChar WFFlowGroupID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
        }
        #endregion

        #region - Sys SystemWFNode -
        public class SysSystemWFNodeEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;

            public DBVarChar WFNodeID;
            public DBNVarChar WFNodezhTW;
            public DBNVarChar WFNodezhCN;
            public DBNVarChar WFNodeenUS;
            public DBNVarChar WFNodethTH;
            public DBNVarChar WFNodejaJP;
            public DBNVarChar WFNodekoKR;

            public DBVarChar NodeType;
            public DBVarChar BackWFNodeID;

            public DBVarChar SortOrder;
        }

        public class SysSystemWFNodeDelete : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;

            public DBVarChar WFNodeID;
        }
        #endregion

        #region - Sys SystemWFSignature -
        public class SysSystemWFSignatureEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;

            public DBInt SigStep;
            public DBChar WFSigSeq;

            public DBNVarChar WFSigzhTW;
            public DBNVarChar WFSigzhCN;
            public DBNVarChar WFSigenUS;
            public DBNVarChar WFSigthTH;
            public DBNVarChar WFSigjaJP;
            public DBNVarChar WFSigkoKR;

            public DBChar IsReq;
        }

        public class SysSystemWFSignatureDelete : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
            public DBChar WFSigSeq;
        }
        #endregion

        #region - Sys SystemWFDocument -
        public class SysSystemWFDocumentEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;

            public DBChar WFDocSeq;

            public DBNVarChar WFDoczhTW;
            public DBNVarChar WFDoczhCN;
            public DBNVarChar WFDocenUS;
            public DBNVarChar WFDocthTH;
            public DBNVarChar WFDocjaJP;
            public DBNVarChar WFDockoKR;

            public DBChar IsReq;
        }

        public class SysSystemWFDocumentDelete : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
            public DBChar WFDocSeq;
        }
        #endregion

        #region - WorkFlowNode -
        public class WorkFlowAdd : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBChar WFNo;
            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBNVarChar WFSubject;
            public DBVarChar NewUserID;
            public DBChar DTBegin;
            public DBChar NodeNo;
            public DBVarChar ResultID;
        }

        public class WorkFlowNext : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBChar WFNo;
            public DBVarChar ResultID;
            public DBChar NodeNo;
            public DBVarChar UserID;
        }

        public class WorkFlowBack : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBChar WFNo;
            public DBVarChar ResultID;
            public DBChar NodeNo;
        }

        public class WorkFlowFinish : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBChar WFNo;
            public DBVarChar EndUserID;
            public DBChar DTEnd;
            public DBVarChar ResultID;
            public DBChar NodeNo;
        }

        public class WorkFlowCancel : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBChar WFNo;
            public DBVarChar EndUserID;
            public DBChar DTEnd;
            public DBVarChar ResultID;
            public DBChar NodeNo;
        }

        public class WorkFlowNodeEdit : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBChar WFNo;
            public DBChar NodeNo;
            public DBVarChar WFNodeID;
            public DBVarChar UserID;
            public DBVarChar NewUserID;
            public DBVarChar ResultID;
        }

        public class WorkFlowNodeNext : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBChar WFNo;
            public DBChar NodeNo;
            public DBVarChar WFNodeID;
            public DBVarChar UserID;
            public DBVarChar NewUserID;
            public DBVarChar ResultID;
        }

        public class WorkFlowNodeBack : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBChar WFNo;
            public DBChar NodeNo;
            public DBVarChar WFNodeID;
            public DBVarChar ResultID;
        }
        
        public class WorkFlowNodeCancel : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBChar WFNo;
            public DBChar NodeNo;
            public DBVarChar WFNodeID;
            public DBVarChar ResultID;
        }

        public class WorkFlowSetSignature : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBChar WFNo;
            public DBChar NodeNo;
            public DBVarChar UserID;
            public DBChar IsStartSig;
            public List<WorkFlowSetSignatureItem> SetSignatureList;
        }

        public class WorkFlowSetSignatureItem : DBTableRow
        {
            public DBVarChar SigUserID;
            public DBChar WFSigSeq;
            public DBInt SigStep;
            public DBChar SigDate;
            public DBChar SigResultID;
        }

        public class WorkFlowSignatureApproved : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBChar WFNo;
            public DBChar NodeNo;
            public DBVarChar UserID;
            public DBVarChar SigResultID;
            public DBNVarChar SigComment;
        }

        public class WorkFlowSignatureReturned : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBChar WFNo;
            public DBChar NodeNo;
            public DBVarChar UserID;
            public DBVarChar SigResultID;
            public DBNVarChar SigComment;
        }

        public class WorkFlowDocumentAdd : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBChar WFNo;
            public DBChar NodeNo;
            public DBChar DocNo;
            public DBChar WFDocSeq;
            public DBVarChar DocUserID;
            public DBNVarChar DocFileNM;
            public DBVarChar DocEncodeNM;
            public DBChar DocDate;
        }

        public class WorkFlowDocumentDelete : DBTableRow
        {
            public List<DBVarChar> TargetSysIDList;

            public DBChar WFNo;
            public DBChar DocNo;
        }
        #endregion
    }
}