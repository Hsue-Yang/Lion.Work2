using System;
using System.Collections.Generic;
using System.Linq;

namespace LionTech.Entity.ERP.WorkFlowService
{
    public class EntityNewFlow : EntityWorkFlowService
    {
        public EntityNewFlow(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 建立工作流程 -
        public enum EnumNewFlowResult
        {
            Success,
            NotUserAuthorize,
            CheckWFLifeCycle,
            Failure
        }

        public class WorkFlow : ExecuteResult
        {
            public DBChar WFNo;
            public DBChar NodeNo;

            public DBVarChar SysID;
            public DBVarChar FlowID;
            public DBChar FlowVer;

            public DBVarChar NodeID;
            public DBVarChar NodeType;
            
            public DBVarChar FunSysID;
            public DBVarChar SubSysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;

            public DBChar DTBegin;

            public DBVarChar ResultID;

            public DBVarChar NodeUrl;
        }
        
        /// <summary>
        /// 建立工作流程
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public WorkFlow NewFlow(WorkFlowPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText = "EXECUTE dbo.SP_WF_NEW_FLOW {SYS_ID}, {FLOW_ID}, {FLOW_VER}, {LOT}, {SUBJECT}, {NODE_NO}, {USER_ID}, {UPD_USER_ID};";

            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.FLOW_ID, Value = para.FlowID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.FLOW_VER, Value = para.FlowVer });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.LOT, Value = para.Lot });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.SUBJECT, Value = para.Subject });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.NODE_NO, Value = para.NodeNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<WorkFlow>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumNewFlowResult.Failure.ToString())
            {
                throw new EntityExecuteResultException(result);
            }

            return result;
        }
        #endregion

        #region - 確認是否內部IP -
        public class CheckInternalIPPara
        {
            public enum ParaField
            {
                SYS_ID,
                CLIENT_IP_ADDRESS
            }

            public List<DBVarChar> SysID;
            public DBVarChar ClientIPAddress;
        }

        public enum EnumCheckInternalIPResult
        {
            Success,
            Failure
        }

        /// <summary>
        /// 確認是否內部IP
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public EnumCheckInternalIPResult CheckInternalIP(CheckInternalIPPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT CHAR(1) = 'N';",
                        "IF EXISTS(SELECT * ",
                        "            FROM SYS_SYSTEM_IP ",
                        "           WHERE SYS_ID IN ({SYS_ID}) ",
                        "             AND IP_ADDRESS = {CLIENT_IP_ADDRESS}) ",
                        "    SET @RESULT = 'Y'; ",
                        "SELECT @RESULT "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = CheckInternalIPPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = CheckInternalIPPara.ParaField.CLIENT_IP_ADDRESS, Value = para.ClientIPAddress });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumCheckInternalIPResult.Success : EnumCheckInternalIPResult.Failure;
        }
        #endregion

    }
}