using System;
using System.Collections.Generic;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemWorkFlowDocument : EntitySys
    {
        public EntitySystemWorkFlowDocument(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemWFDocPara : DBCulture
        {
            public SystemWFDocPara(string culture)
                : base(culture)
            {
            }

            public enum ParaField
            {
                SYS_ID, WF_FLOW_ID, WF_FLOW_VER, WF_NODE_ID, 
                WF_DOC
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
        }

        public class SystemWFDoc : DBTableRow
        {
            public DBChar WFDocSeq;
            public DBNVarChar WFDocNM;

            public DBChar IsReq;

            public DBVarChar UpdUserNM;
            public DBDateTime UpdDt;
        }

        public List<SystemWFDoc> SelectSystemWFDocList(SystemWFDocPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT WF_DOC_SEQ AS WFDocSeq ",
                        "     , dbo.FN_GET_NMID(WF_DOC_SEQ, {WF_DOC}) AS WFDocNM ",
                        "     , IS_REQ AS IsReq ",
                        "     , dbo.FN_GET_USER_NM(UPD_USER_ID) AS UpdUserNM",
                        "     , UPD_DT AS UpdDt ",
                        "  FROM SYS_SYSTEM_WF_DOC ",
                        " WHERE SYS_ID = {SYS_ID} ",
                        "   AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "   AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "   AND WF_NODE_ID = {WF_NODE_ID} ",
                        Environment.NewLine
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_FLOW_ID.ToString(), Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_FLOW_VER.ToString(), Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_NODE_ID.ToString(), Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_DOC.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFDocPara.ParaField.WF_DOC.ToString())) });
            return GetEntityList<SystemWFDoc>(commandText, dbParameters);
        }
    }
}
