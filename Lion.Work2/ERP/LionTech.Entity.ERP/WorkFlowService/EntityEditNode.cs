using System;
using System.Collections.Generic;
using System.Linq;

namespace LionTech.Entity.ERP.WorkFlowService
{
    public class EntityEditNode : EntityWorkFlowService
    {
        public EntityEditNode(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public enum EnumEditNodeResult
        {
            Success,
            NotProcessNode,
            WFNodeNotUserRole,
            Failure
        }

        public class EditNodeExecuteResult : ExecuteResult
        {
            public DBChar NodeNo;
            public DBVarChar SysID;
            public DBVarChar NodeID;
            public DBVarChar ResultID;
        }

        public EditNodeExecuteResult EditNode(WorkFlowPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText = "EXECUTE dbo.SP_WF_EDIT_NODE {WF_NO}, {USER_ID}, {NEW_USER_ID}, {UPD_USER_ID};";

            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.NEW_USER_ID, Value = para.NewUserID });

            var result = GetEntityList<EditNodeExecuteResult>(commandText, dbParameters).SingleOrDefault();

            var enumResult = ((EnumEditNodeResult)Enum.Parse(typeof(EnumEditNodeResult), result.Result.GetValue()));

            if (enumResult == EnumEditNodeResult.Failure)
            {
                throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
            }

            return result;
        }
    }
}