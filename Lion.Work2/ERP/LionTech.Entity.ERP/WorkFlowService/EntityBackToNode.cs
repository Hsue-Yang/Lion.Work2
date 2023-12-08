using System;
using System.Collections.Generic;
using System.Linq;

namespace LionTech.Entity.ERP.WorkFlowService
{
    public class EntityBackToNode : EntityWorkFlowService
    {
        public EntityBackToNode(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public enum EnumBackToNodeResult
        {
            Success,
            NotProcessNode,
            NotUserAuthorize,
            ProcessNodeNotAnyInfo,
            Failure
        }

        public class BackToNodeExecuteResult : ExecuteResult
        {
            public DBChar WFNo;

            public DBVarChar SysID;
            public DBVarChar FlowID;
            public DBNVarChar FlowNM;
            public DBChar FlowVer;
            public DBNVarChar Subject;

            public DBVarChar NewUserID;
            public DBChar NodeNo;
            public DBVarChar NodeID;

            public DBChar BackNodeNo;
            public DBVarChar BackNodeID;
            public DBNVarChar BackNodeNM;
            public DBVarChar BackNodeType;

            public DBVarChar BackFunSysID;
            public DBVarChar BackSubSysID;
            public DBVarChar BackFunControllerID;
            public DBVarChar BackFunActionName;
            
            public DBVarChar ResultID;
            public DBChar IsAutoAssignNewUser;
        }

        public BackToNodeExecuteResult BackToNode(WorkFlowPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "EXECUTE dbo.SP_WF_BACK_TO_NODE {WF_NO}, {USER_ID}, {UPD_USER_ID};"
                    });

            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<BackToNodeExecuteResult>(commandText, dbParameters).SingleOrDefault();

            var enumResult = ((EnumBackToNodeResult)Enum.Parse(typeof(EnumBackToNodeResult), result.Result.GetValue()));

            if (enumResult == EnumBackToNodeResult.Failure)
            {
                throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
            }

            return result;
        }
    }
}