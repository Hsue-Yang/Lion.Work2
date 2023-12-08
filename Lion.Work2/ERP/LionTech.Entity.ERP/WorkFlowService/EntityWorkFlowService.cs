using System;
using System.Collections.Generic;
using System.Linq;

namespace LionTech.Entity.ERP.WorkFlowService
{
    public class EntityWorkFlowService : DBEntity
    {
#if !NET461
        public EntityWorkFlowService(string connectionName)
            : base(connectionName)
        {
        }
#endif
        public EntityWorkFlowService(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class WorkFlowPara
        {
            public enum ParaField
            {
                WF_NO, NODE_NO,
                SYS_ID, FLOW_ID, FLOW_VER, 
                SUBJECT, LOT,
                NODE_ID, NEXT_RESULT_VALUE,
                USER_ID, NEW_USER_ID,
                SIG_RESULT_ID, SIG_COMMENT,
                UPD_USER_ID
            }

            public DBChar WFNo;
            public DBChar NodeNo;

            public DBVarChar SysID;
            public DBVarChar FlowID;
            public DBChar FlowVer;

            public DBNVarChar Subject;
            public DBNVarChar Lot;

            public DBVarChar NodeID;
            public DBVarChar NextResultValue;

            public DBVarChar UserID;
            public DBVarChar NewUserID;

            public DBVarChar SigResultID;
            public DBNVarChar SigComment;

            public DBVarChar UpdUserID;
            
        }

        public class WorkFlowAPIPara : DBTableRow
        {
            public DBChar WFNo;
            public DBChar NodeNo;
            public DBVarChar UserID;

            public DBVarChar SysID;
            public DBVarChar FlowID;
            public DBChar FlowVer;

            public DBVarChar NodeID;

            public DBChar SigSeq;
        }

        public class MsgFunAction : DBTableRow {
            public DBVarChar SysID;
            public DBVarChar ControllerID;
            public DBVarChar ActionName;
        }

        public MsgFunAction SelectMsgFunAction(WorkFlowPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "EXECUTE dbo.SP_WF_MSG_FUN_ACTION {WF_NO};"
                    });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.WF_NO, Value = para.WFNo });
            return GetEntityList<MsgFunAction>(commandText, dbParameters).SingleOrDefault();
        }
    }
}
