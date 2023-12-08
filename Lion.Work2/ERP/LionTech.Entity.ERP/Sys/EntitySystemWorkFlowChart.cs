using System;
using System.Collections.Generic;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemWorkFlowChart : EntitySys
    {
        public EntitySystemWorkFlowChart(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemWFNodePara
        {
            public enum ParaField
            {
                SYS_ID,
                WF_FLOW_ID,
                WF_FLOW_VER
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVersion;
        }

        public class SystemWFNode : DBTableRow
        {
            public DBVarChar WFNodeID;
            public DBInt NodePOSBeginX;
            public DBInt NodePOSBeginY;
            public DBInt NodePOSEndX;
            public DBInt NodePOSEndY;
        }

        public List<SystemWFNode> SelectSystemWFNodeList(SystemWFNodePara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT WF_NODE_ID AS WFNodeID ",
                        "     , NODE_POS_BEGIN_X AS NodePOSBeginX ",
                        "     , NODE_POS_BEGIN_Y AS NodePOSBeginY ",
                        "     , NODE_POS_END_X AS NodePOSEndX ",
                        "     , NODE_POS_END_Y AS NodePOSEndY ",
                        "  FROM SYS_SYSTEM_WF_NODE ",
                        " WHERE SYS_ID = {SYS_ID} ",
                        "   AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "   AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        " ORDER BY SORT_ORDER "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_FLOW_ID.ToString(), Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_FLOW_VER.ToString(), Value = para.WFFlowVersion });
            return GetEntityList<SystemWFNode>(commandText, dbParameters);
        }

        public class SystemWFArrowPara
        {
            public enum ParaField
            {
                SYS_ID,
                WF_FLOW_ID,
                WF_FLOW_VER
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVersion;
        }

        public class SystemWFArrow : DBTableRow
        {
            public DBVarChar WFNodeID;
            public DBVarChar NextWFNodeID;
            public DBInt ArrowPOSBeginX;
            public DBInt ArrowPOSBeginY;
            public DBInt ArrowPOSEndX;
            public DBInt ArrowPOSEndY;
        }

        public List<SystemWFArrow> SelectSystemWFArrowList(SystemWFArrowPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT A.WF_NODE_ID AS WFNodeID ",
                        "     , A.NEXT_WF_NODE_ID AS NextWFNodeID ",
                        "     , A.ARROW_POS_BEGIN_X AS ArrowPOSBeginX ",
                        "     , A.ARROW_POS_BEGIN_Y AS ArrowPOSBeginY ",
                        "     , A.ARROW_POS_END_X AS ArrowPOSEndX ",
                        "     , A.ARROW_POS_END_Y AS ArrowPOSEndY ",
                        "  FROM SYS_SYSTEM_WF_ARROW A ",
                        "  JOIN SYS_SYSTEM_WF_NODE N ",
                        "    ON A.SYS_ID = N.SYS_ID ",
                        "   AND A.WF_FLOW_ID = N.WF_FLOW_ID ",
                        "   AND A.WF_FLOW_VER = N.WF_FLOW_VER ",
                        "   AND A.WF_NODE_ID = N.WF_NODE_ID ",
                        " WHERE A.SYS_ID = {SYS_ID} ",
                        "   AND A.WF_FLOW_ID = {WF_FLOW_ID} ",
                        "   AND A.WF_FLOW_VER = {WF_FLOW_VER} ",
                        " ORDER BY N.SORT_ORDER "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFArrowPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFArrowPara.ParaField.WF_FLOW_ID.ToString(), Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFArrowPara.ParaField.WF_FLOW_VER.ToString(), Value = para.WFFlowVersion });
            return GetEntityList<SystemWFArrow>(commandText, dbParameters);
        }
    }
}
