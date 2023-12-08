using System;
using System.Collections.Generic;
using System.Linq;

namespace LionTech.Entity.ERP.WorkFlowService
{
    public class EntityTerminateFlow : EntityWorkFlowService
    {
        public EntityTerminateFlow(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public enum EnumTerminateFlowResult
        {
            Success,
            Failure,
            UserUnAuthorize,
            NotProcessNode
        }

        public class TerminateFlowExecuteResult : ExecuteResult
        {
            public DBChar WFNo;
            public DBNVarChar WFFlowNM;
            public DBVarChar ApplyUser;
            public DBNVarChar Subject;
            public DBVarChar SysID;
            public DBChar NodeNo;
            public DBVarChar NodeID;
            public DBNVarChar WFNodeNM;
            public DBVarChar ResultID;
            public DBChar DTEnd;
            public DBChar IsAutoAssignNewUser;
        }

        public TerminateFlowExecuteResult TerminateFlow(WorkFlowPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "EXECUTE dbo.SP_WF_TERMINATE_FLOW {WF_NO}, {USER_ID}, {UPD_USER_ID};"
                    });
            
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<TerminateFlowExecuteResult>(commandText, dbParameters).SingleOrDefault();

            var enumResult = ((EnumTerminateFlowResult)Enum.Parse(typeof(EnumTerminateFlowResult), result.Result.GetValue()));

            if (enumResult == EnumTerminateFlowResult.Failure)
            {
                throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
            }

            return result;
        }
    }
}