using System;
using System.Collections.Generic;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemWorkFlowGroup : EntitySys
    {
        public EntitySystemWorkFlowGroup(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemWorkFlowGroupPara : DBCulture
        {
            public SystemWorkFlowGroupPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, SYS_NM, WF_FLOW_GROUP
            }

            public DBVarChar SysID;
        }

        public class SystemWorkFlowGroup : DBTableRow
        {
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar WFFlowGroupID;
            public DBNVarChar WFFlowGroupNM;
            public DBVarChar SortOrder;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDt;
        }

        public List<SystemWorkFlowGroup> SelectSystemWorkFlowGroupList(SystemWorkFlowGroupPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT E.SYS_ID AS SysID",
                        "     , dbo.FN_GET_NMID(E.SYS_ID, M.{SYS_NM}) AS SysNM",
                        "     , E.WF_FLOW_GROUP_ID AS WFFlowGroupID",
                        "     , dbo.FN_GET_NMID(E.WF_FLOW_GROUP_ID, E.{WF_FLOW_GROUP}) AS WFFlowGroupNM",
                        "     , E.SORT_ORDER AS SortOrder",
                        "     , dbo.FN_GET_USER_NM(E.UPD_USER_ID) AS UpdUserNM",
                        "     , E.UPD_DT AS UpdDt",
                        "  FROM SYS_SYSTEM_WF_FLOW_GROUP E",
                        "  JOIN SYS_SYSTEM_MAIN M",
                        "    ON E.SYS_ID = M.SYS_ID",
                        " WHERE E.SYS_ID = {SYS_ID}",
                        " ORDER BY E.SORT_ORDER"
                    });

            List<DBParameter> dbParameters = 
                new List<DBParameter>
                {
                    new DBParameter { Name = SystemWorkFlowGroupPara.ParaField.SYS_ID.ToString(), Value = para.SysID }, 
                    new DBParameter { Name = SystemWorkFlowGroupPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWorkFlowGroupPara.ParaField.SYS_NM.ToString())) }, 
                    new DBParameter { Name = SystemWorkFlowGroupPara.ParaField.WF_FLOW_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWorkFlowGroupPara.ParaField.WF_FLOW_GROUP.ToString())) }
                };
            return GetEntityList<SystemWorkFlowGroup>(commandText, dbParameters);
        }
    }
}