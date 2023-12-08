using System;
using System.Collections.Generic;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemWorkFlowNext : EntitySys
    {
        public EntitySystemWorkFlowNext(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemWFNextPara : DBCulture
        {
            public SystemWFNextPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                WF_FLOW_ID,
                WF_FLOW_VER,
                WF_NODE_ID,
                WF_NODE,
                SYS_NM,
                CODE_NM,
                FUN_GROUP,
                API_GROUP,
                FUN_NM,
                API_NM
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBVarChar WFFlowVer;
            public DBVarChar WFNodeID;
        }

        public class SystemWFNext : DBTableRow
        {
            public DBVarChar NextWFNodeID;
            public DBNVarChar NextWFNodeNM;
            public DBNVarChar NextNodeTypeNM;
            public DBVarChar NextResultValue;

            public DBNVarChar FunSysNM;
            public DBNVarChar SubSysNM;
            public DBNVarChar FunControllerNM;
            public DBNVarChar FunActionNameNM;

            public DBVarChar SortOrder;

            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDt;
        }

        public List<SystemWFNext> SelectSystemWFNextList(SystemWFNextPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT X.NEXT_WF_NODE_ID AS NextWFNodeID",
                        "     , dbo.FN_GET_NMID(X.NEXT_WF_NODE_ID, W.{WF_NODE}) AS NextWFNodeNM ",
                        "     , dbo.FN_GET_NMID(W.NODE_TYPE, C.{CODE_NM}) AS NextNodeTypeNM ",
                        "     , X.NEXT_RESULT_VALUE AS NextResultValue",
                        "     , (CASE WHEN W.FUN_SYS_ID IS NOT NULL THEN dbo.FN_GET_NMID(W.FUN_SYS_ID, M.{SYS_NM}) ELSE NULL END) AS FunSysNM ",
                        "     , (CASE WHEN U.SUB_SYS_ID = U.SYS_ID THEN NULL ",
                        "             ELSE (CASE WHEN U.SUB_SYS_ID IS NOT NULL THEN (dbo.FN_GET_NMID(U.SUB_SYS_ID, S.{SYS_NM})) ELSE NULL END) ",
                        "        END) AS SubSysNM ",
                        "     , (CASE WHEN W.FUN_CONTROLLER_ID IS NOT NULL THEN (dbo.FN_GET_NMID(W.FUN_CONTROLLER_ID, G.FUN_GROUP_NM)) ",
                        "             ELSE NULL ",
                        "        END) AS FunControllerNM ",
                        "     , (CASE WHEN W.FUN_ACTION_NAME IS NOT NULL THEN (dbo.FN_GET_NMID(W.FUN_ACTION_NAME, U.FUN_NM)) ",
                        "             ELSE NULL ",
                        "        END) AS FunActionNameNM ",
                        "     , X.SORT_ORDER AS SortOrder ",
                        "     , dbo.FN_GET_USER_NM(X.UPD_USER_ID) AS UpdUserNM ",
                        "     , X.UPD_DT AS UpdDt",
                        "  FROM SYS_SYSTEM_WF_NEXT X ",
                        "  JOIN SYS_SYSTEM_WF_NODE W ",
                        "    ON X.SYS_ID = W.SYS_ID ",
                        "   AND X.WF_FLOW_ID = W.WF_FLOW_ID ",
                        "   AND X.WF_FLOW_VER = W.WF_FLOW_VER ",
                        "   AND X.NEXT_WF_NODE_ID = W.WF_NODE_ID ",
                        "  JOIN SYS_SYSTEM_WF_FLOW F ",
                        "    ON W.SYS_ID = F.SYS_ID ",
                        "   AND W.WF_FLOW_ID = F.WF_FLOW_ID ",
                        "   AND W.WF_FLOW_VER = F.WF_FLOW_VER ",
                        "  JOIN CM_CODE C ",
                        "    ON C.CODE_KIND = '0027' ",
                        "   AND C.CODE_ID = W.NODE_TYPE ",
                        "  LEFT JOIN (SELECT 'P' AS NODE_TYPE, SYS_ID, SUB_SYS_ID ",
                        "                  , FUN_CONTROLLER_ID, FUN_ACTION_NAME ",
                        "                  , {FUN_NM} AS FUN_NM ",
                        "             FROM SYS_SYSTEM_FUN ",
                        "             UNION ",
                        "             SELECT 'D' AS NODE_TYPE, SYS_ID, NULL AS SUB_SYS_ID ",
                        "                  , API_CONTROLLER_ID AS FUN_CONTROLLER_ID, API_ACTION_NAME AS FUN_ACTION_NAME ",
                        "                  , {API_NM} AS FUN_NM ",
                        "             FROM SYS_SYSTEM_API) U ",
                        "    ON W.FUN_SYS_ID = U.SYS_ID ",
                        "   AND W.FUN_CONTROLLER_ID = U.FUN_CONTROLLER_ID ",
                        "   AND W.FUN_ACTION_NAME = U.FUN_ACTION_NAME ",
                        "   AND W.NODE_TYPE = U.NODE_TYPE ",
                        "  LEFT JOIN SYS_SYSTEM_MAIN M ON W.FUN_SYS_ID = M.SYS_ID ",
                        "  LEFT JOIN SYS_SYSTEM_SUB S ON U.SYS_ID = S.PARENT_SYS_ID AND U.SUB_SYS_ID = S.SYS_ID ",
                        "  LEFT JOIN (SELECT 'P' AS NODE_TYPE, SYS_ID ",
                        "                  , FUN_CONTROLLER_ID ",
                        "                  , {FUN_GROUP} AS FUN_GROUP_NM ",
                        "             FROM SYS_SYSTEM_FUN_GROUP ",
                        "             UNION ",
                        "             SELECT 'D' AS NODE_TYPE, SYS_ID ",
                        "                  , API_CONTROLLER_ID AS FUN_CONTROLLER_ID ",
                        "                  , {API_GROUP} AS FUN_GROUP_NM ",
                        "             FROM SYS_SYSTEM_API_GROUP) G ",
                        "    ON U.SYS_ID = G.SYS_ID ",
                        "   AND U.FUN_CONTROLLER_ID = G.FUN_CONTROLLER_ID ",
                        "   AND W.NODE_TYPE = G.NODE_TYPE ",
                        " WHERE X.SYS_ID = {SYS_ID} ",
                        "   AND X.WF_FLOW_ID = {WF_FLOW_ID} ",
                        "   AND X.WF_FLOW_VER = {WF_FLOW_VER} ",
                        "   AND X.WF_NODE_ID = {WF_NODE_ID} ",
                        " ORDER BY X.SORT_ORDER "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.WF_FLOW_ID.ToString(), Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.WF_FLOW_VER.ToString(), Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.WF_NODE_ID.ToString(), Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.WF_NODE.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFNextPara.ParaField.WF_NODE.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFNextPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFNextPara.ParaField.CODE_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFNextPara.ParaField.FUN_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.API_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFNextPara.ParaField.API_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFNextPara.ParaField.FUN_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.API_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFNextPara.ParaField.API_NM.ToString())) });

            return GetEntityList<SystemWFNext>(commandText, dbParameters);
        }
    }
}