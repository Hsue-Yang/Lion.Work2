using System;
using System.Collections.Generic;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemWorkFlowNode : EntitySys
    {
        public EntitySystemWorkFlowNode(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemWorkFlowNodePara : DBCulture
        {
            public SystemWorkFlowNodePara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, WF_FLOW_ID, WF_FLOW_VER, WF_FLOW_GROUP_ID,
                SYS_NM, WF_FLOW_GROUP, WF_FLOW, WF_NODE, CODE_NM, FUN_GROUP, API_GROUP, FUN_NM, API_NM
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFFlowGroupID;
        }

        public class SystemWorkFlowNode : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar WFFlowGroupID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
            public DBNVarChar WFNodeNM;

            public DBNVarChar NodeTypeNM;
            public DBInt NodeSeqX;
            public DBInt NodeSeqY;
            public DBChar IsFirst;
            public DBChar IsFinally;

            public DBNVarChar FunSysNM;
            public DBNVarChar SubSysNM;
            public DBNVarChar FunControllerNM;
            public DBNVarChar FunActionNameNM;

            public DBNVarChar BackWFNodeNM;
            public DBVarChar SortOrder;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDt;
        }

        public List<SystemWorkFlowNode> SelectSystemWorkFlowNodeList(SystemWorkFlowNodePara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT W.SYS_ID AS SysID ",
                        "     , F.WF_FLOW_GROUP_ID AS WFFlowGroupID ",
                        "     , W.WF_FLOW_ID AS WFFlowID ",
                        "     , W.WF_FLOW_VER AS WFFlowVer ",
                        "     , W.WF_NODE_ID AS WFNodeID ",
                        "     , dbo.FN_GET_NMID(W.WF_NODE_ID, W.{WF_NODE}) AS WFNodeNM ",
                        "     , dbo.FN_GET_NMID(W.NODE_TYPE, C.{CODE_NM}) AS NodeTypeNM ",
                        "     , W.NODE_SEQ_X AS NodeSeqX ",
                        "     , W.NODE_SEQ_Y AS NodeSeqY ",
                        "     , W.IS_FIRST AS IsFirst ",
                        "     , W.IS_FINALLY AS IsFinally ",
                        "     , (CASE WHEN W.FUN_SYS_ID IS NOT NULL THEN dbo.FN_GET_NMID(W.FUN_SYS_ID, N.{SYS_NM}) ELSE NULL END) AS FunSysNM ",
                        "     , (CASE WHEN U.SUB_SYS_ID=U.SYS_ID THEN NULL ",
                        "             ELSE (CASE WHEN U.SUB_SYS_ID IS NOT NULL THEN (dbo.FN_GET_NMID(U.SUB_SYS_ID, S.{SYS_NM})) ELSE NULL END) ",
                        "        END) AS SubSysNM ",
                        "     , (CASE WHEN W.FUN_CONTROLLER_ID IS NOT NULL THEN (dbo.FN_GET_NMID(W.FUN_CONTROLLER_ID, G.FUN_GROUP_NM)) ",
                        "             ELSE NULL ",
                        "        END) AS FunControllerNM ",
                        "     , (CASE WHEN W.FUN_ACTION_NAME IS NOT NULL THEN (dbo.FN_GET_NMID(W.FUN_ACTION_NAME, U.FUN_NM)) ",
                        "             ELSE NULL ",
                        "        END) AS FunActionNameNM ",
                        "     , (CASE WHEN W.BACK_WF_NODE_ID IS NULL THEN NULL ELSE dbo.FN_GET_NMID(W.BACK_WF_NODE_ID, B.{WF_NODE}) END) AS BackWFNodeNM ",
                        "     , W.SORT_ORDER AS SortOrder ",
                        "     , dbo.FN_GET_USER_NM(W.UPD_USER_ID) AS UpdUserNM ",
                        "     , W.UPD_DT AS UpdDt ",
                        "  FROM SYS_SYSTEM_WF_NODE W ",
                        "  JOIN CM_CODE C ",
                        "    ON C.CODE_KIND = '0027' ",
                        "   AND C.CODE_ID = W.NODE_TYPE ",
                        "  JOIN SYS_SYSTEM_WF_FLOW F ",
                        "    ON W.SYS_ID = F.SYS_ID ",
                        "   AND W.WF_FLOW_ID = F.WF_FLOW_ID ",
                        "   AND W.WF_FLOW_VER = F.WF_FLOW_VER ",
                        "  LEFT JOIN (SELECT 'P' AS NODE_TYPE ",
                        "                  , SYS_ID, SUB_SYS_ID ",
                        "                  , FUN_CONTROLLER_ID ",
                        "                  , FUN_ACTION_NAME ",
                        "                  , {FUN_NM} AS FUN_NM ",
                        "               FROM SYS_SYSTEM_FUN ",
                        "              UNION ",
                        "             SELECT 'D' AS NODE_TYPE ",
                        "                  , SYS_ID ",
                        "                  , NULL AS SUB_SYS_ID ",
                        "                  , API_CONTROLLER_ID AS FUN_CONTROLLER_ID ",
                        "                  , API_ACTION_NAME AS FUN_ACTION_NAME ",
                        "                  , {API_NM} AS FUN_NM ",
                        "               FROM SYS_SYSTEM_API) U ",
                        "    ON W.FUN_SYS_ID = U.SYS_ID ",
                        "   AND W.FUN_CONTROLLER_ID = U.FUN_CONTROLLER_ID ",
                        "   AND W.FUN_ACTION_NAME = U.FUN_ACTION_NAME ",
                        "   AND W.NODE_TYPE = U.NODE_TYPE ",
                        "  LEFT JOIN SYS_SYSTEM_MAIN N ",
                        "    ON W.FUN_SYS_ID = N.SYS_ID ",
                        "  LEFT JOIN SYS_SYSTEM_SUB S ",
                        "    ON U.SYS_ID = S.PARENT_SYS_ID ",
                        "   AND U.SUB_SYS_ID = S.SYS_ID ",
                        "  LEFT JOIN (SELECT 'P' AS NODE_TYPE ",
                        "                  , SYS_ID ",
                        "                  , FUN_CONTROLLER_ID ",
                        "                  , {FUN_GROUP} AS FUN_GROUP_NM ",
                        "               FROM SYS_SYSTEM_FUN_GROUP ",
                        "              UNION ",
                        "             SELECT 'D' AS NODE_TYPE ",
                        "                  , SYS_ID ",
                        "                  , API_CONTROLLER_ID AS FUN_CONTROLLER_ID ",
                        "                  , {API_GROUP} AS FUN_GROUP_NM ",
                        "               FROM SYS_SYSTEM_API_GROUP) G ",
                        "   ON U.SYS_ID = G.SYS_ID ",
                        "  AND U.FUN_CONTROLLER_ID = G.FUN_CONTROLLER_ID ",
                        "  AND W.NODE_TYPE = G.NODE_TYPE ",
                        " LEFT JOIN SYS_SYSTEM_WF_NODE B ",
                        "   ON W.SYS_ID = B.SYS_ID ",
                        "  AND W.WF_FLOW_ID = B.WF_FLOW_ID ",
                        "  AND W.WF_FLOW_VER = B.WF_FLOW_VER ",
                        "  AND W.BACK_WF_NODE_ID = B.WF_NODE_ID ",
                        "WHERE W.SYS_ID = {SYS_ID}  ",
                        "  AND W.WF_FLOW_ID = {WF_FLOW_ID} ",
                        "  AND W.WF_FLOW_VER = {WF_FLOW_VER} ",
                        "ORDER BY W.SORT_ORDER ",
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowNodePara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowNodePara.ParaField.WF_FLOW_ID.ToString(), Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowNodePara.ParaField.WF_FLOW_VER.ToString(), Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowNodePara.ParaField.WF_FLOW_GROUP_ID.ToString(), Value = para.WFFlowGroupID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowNodePara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWorkFlowNodePara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowNodePara.ParaField.WF_FLOW_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWorkFlowNodePara.ParaField.WF_FLOW_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowNodePara.ParaField.WF_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWorkFlowNodePara.ParaField.WF_FLOW.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowNodePara.ParaField.WF_NODE.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWorkFlowNodePara.ParaField.WF_NODE.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowNodePara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWorkFlowNodePara.ParaField.CODE_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowNodePara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWorkFlowNodePara.ParaField.FUN_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowNodePara.ParaField.API_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWorkFlowNodePara.ParaField.API_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowNodePara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWorkFlowNodePara.ParaField.FUN_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowNodePara.ParaField.API_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWorkFlowNodePara.ParaField.API_NM.ToString())) });

            return GetEntityList<SystemWorkFlowNode>(commandText, dbParameters);
        }
    }
}
