using System;
using System.Collections.Generic;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemWorkFlow : EntitySys
    {
        public EntitySystemWorkFlow(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemWorkFlowPara : DBCulture 
        {
            public SystemWorkFlowPara(string culture)
                : base(culture)
            {
                
            }

            public enum ParaField
            {
                SYS_ID, WF_FLOW_GROUP_ID, 
                SYS_NM, WF_FLOW_GROUP, WF_FLOW, CODE_NM
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowGroupID;
        }

        public class SystemWorkFlow : DBTableRow
        {
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar WFFlowGroupID;
            public DBNVarChar WFFlowGroupNM;
            public DBVarChar WFFlowID;
            public DBNVarChar WFFlowNM;
            public DBVarChar WFFlowVer;
            public DBNVarChar FlowTypeNM;
            public DBVarChar FlowManUserNM;
            public DBChar EnableDate;
            public DBChar DisableDate;
            public DBVarChar SortOrder;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDt;
        }

        public List<SystemWorkFlow> SelectSystemWorkFlowList(SystemWorkFlowPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT W.SYS_ID AS SysID ",
                        "     , dbo.FN_GET_NMID(W.SYS_ID, M.{SYS_NM}) AS SysNM",
                        "     , W.WF_FLOW_GROUP_ID AS WFFlowGroupID ",
                        "     , dbo.FN_GET_NMID(W.WF_FLOW_GROUP_ID, E.{WF_FLOW_GROUP}) AS WFFlowGroupNM ",
                        "     , W.WF_FLOW_ID AS WFFlowID ",
                        "     , dbo.FN_GET_NMID(W.WF_FLOW_ID, W.{WF_FLOW}) AS WFFlowNM ",
                        "     , W.WF_FLOW_VER AS WFFlowVer ",
                        "     , dbo.FN_GET_NMID(W.FLOW_TYPE, C.{CODE_NM}) AS FlowTypeNM ",
                        "     , dbo.FN_GET_USER_NM(W.FLOW_MAN_USER_ID) AS FlowManUserNM ",
                        "     , W.ENABLE_DATE AS EnableDate ",
                        "     , W.DISABLE_DATE AS DisableDate ",
                        "     , W.SORT_ORDER AS SortOrder",
                        "     , dbo.FN_GET_USER_NM(W.UPD_USER_ID) AS UpdUserNM ",
                        "     , W.UPD_DT AS UpdDt ",
                        "  FROM SYS_SYSTEM_WF_FLOW W ",
                        "  JOIN SYS_SYSTEM_MAIN M ",
                        "    ON W.SYS_ID = M.SYS_ID ",
                        "  JOIN SYS_SYSTEM_WF_FLOW_GROUP E ",
                        "    ON W.SYS_ID = E.SYS_ID ",
                        "   AND W.WF_FLOW_GROUP_ID = E.WF_FLOW_GROUP_ID ",
                        "  LEFT JOIN CM_CODE C ",
                        "    ON C.CODE_KIND='0026' ",
                        "   AND W.FLOW_TYPE = C.CODE_ID ",
                        " WHERE W.SYS_ID = {SYS_ID} ",
                        (para.WFFlowGroupID.IsNull() == false)
                            ? "  AND W.WF_FLOW_GROUP_ID = {WF_FLOW_GROUP_ID} "
                            : string.Empty,
                        "ORDER BY E.SORT_ORDER, W.SORT_ORDER ",
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowPara.ParaField.WF_FLOW_GROUP_ID.ToString(), Value = para.WFFlowGroupID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWorkFlowPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowPara.ParaField.WF_FLOW_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWorkFlowPara.ParaField.WF_FLOW_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowPara.ParaField.WF_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWorkFlowPara.ParaField.WF_FLOW.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWorkFlowPara.ParaField.CODE_NM.ToString())) });
            
            return GetEntityList<SystemWorkFlow>(commandText, dbParameters);
        }
    }
}
