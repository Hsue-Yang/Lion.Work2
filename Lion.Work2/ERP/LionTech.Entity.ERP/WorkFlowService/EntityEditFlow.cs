using System.Collections.Generic;

namespace LionTech.Entity.ERP.WorkFlowService
{
    public class EntityEditFlow : EntityWorkFlowService
    {
        public EntityEditFlow(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public enum EnumEditFlowResult
        {
            Success,
            Failure
        }

        public EnumEditFlowResult EditFlow(WorkFlowPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText = "EXECUTE dbo.SP_WF_EDIT_FLOW {WF_NO}, {USER_ID}, {UPD_USER_ID};";

            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditFlowResult.Success : EnumEditFlowResult.Failure;
        }
    }
}