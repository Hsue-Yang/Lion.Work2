using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.WorkFlowService
{
    public class EntitySignature : EntityWorkFlowService
    {
        public EntitySignature(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public enum EnumSignatureResult
        {
            Success,
            Failure,
            NextSigStep,
            NotProcessNode,
            NotSignUser
        }

        public class SignatureExecuteResult : ExecuteResult
        {
            public DBVarChar SysID;
            public DBNVarChar Subject;
            public DBNVarChar WFFlowNM;
            public DBNVarChar WFNodeNM;
            public DBVarChar NewUserID;
        }

        public SignatureExecuteResult Signature(WorkFlowPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "EXECUTE dbo.SP_WF_SIGNATURE {WF_NO}, {NODE_NO}, {USER_ID}, {SIG_RESULT_ID}, {SIG_COMMENT}, {UPD_USER_ID}, {NEW_USER_ID};"
                    });

            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.NODE_NO, Value = para.NodeNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.SIG_RESULT_ID, Value = para.SigResultID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.SIG_COMMENT, Value = para.SigComment });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.NEW_USER_ID, Value = para.NewUserID });

            var result = GetEntityList<SignatureExecuteResult>(commandText, dbParameters).SingleOrDefault();

            var enumResult = ((EnumSignatureResult)Enum.Parse(typeof(EnumSignatureResult), result.Result.GetValue()));

            if (enumResult == EnumSignatureResult.Failure)
            {
                throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
            }

            return result;
        }

        public class WFSigPara
        {
            public enum ParaField
            {
                WF_NO, UPD_USER_ID
            }

            public DBChar WFNo;
            public DBVarChar UpdUserID;
        }

        #region - 查詢關卡人員名單 -
        public class SigStepUser : DBTableRow
        {
            public DBNVarChar WFSigNM;
            public DBVarChar SigUserID;
            public DBNVarChar WFNodeNM;
            public DBNVarChar WFSubject;
            public DBNVarChar WFNM;
        }

        /// <summary>
        /// 查詢關卡人員名單
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<SigStepUser> SelectSigStepUserList(WFSigPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @SIG_STEP INT;",
                    "DECLARE @NODE_NO CHAR(3) = NULL; ",
                    "DECLARE @SYS_ID VARCHAR(12) = NULL; ",
                    "DECLARE @WF_FLOW_ID VARCHAR(50) = NULL; ",
                    "DECLARE @WF_FLOW_NM NVARCHAR(150) = NULL; ",
                    "DECLARE @WF_FLOW_VER VARCHAR(50) = NULL; ",
                    "DECLARE @WF_NODE_ID VARCHAR(50) = NULL; ",
                    "DECLARE @NEW_USER_ID VARCHAR(20) = NULL; ",
                    "DECLARE @WF_SUBJECT NVARCHAR(150) = NULL; ",

                    "SELECT @NODE_NO = D.NODE_NO ",
                    "     , @SYS_ID = D.SYS_ID ",
                    "     , @WF_FLOW_ID = D.WF_FLOW_ID ",
                    "     , @WF_FLOW_NM = D.WF_FLOW_NM",
                    "     , @WF_FLOW_VER = D.WF_FLOW_VER ",
                    "     , @WF_NODE_ID = D.WF_NODE_ID ",
                    "     , @SIG_STEP = D.SIG_STEP ",
                    "     , @WF_SUBJECT = D.WF_SUBJECT",
                    "  FROM dbo.FNTB_GET_WF_NODE({WF_NO}) D",
                    
                    "SELECT S.WF_SIG_ZH_TW AS WFSigNM",
                    "     , S.SIG_USER_ID AS SigUserID",
                    "     , WN.WF_NODE_ZH_TW AS WFNodeNM",
                    "     , @WF_SUBJECT AS WFSubject",
                    "     , @WF_FLOW_NM AS WFNM",
                    "  FROM WF_SIG S",
                    "  JOIN SYS_SYSTEM_WF_NODE WN",
                    "    ON WN.SYS_ID = S.SYS_ID ",
                    "   AND WN.WF_FLOW_ID = S.WF_FLOW_ID ",
                    "   AND WN.WF_FLOW_VER = S.WF_FLOW_VER ",
                    "   AND WN.WF_NODE_ID = S.WF_NODE_ID ",
                    " WHERE @NODE_NO = S.NODE_NO ",
                    "   AND @SYS_ID = S.SYS_ID ",
                    "   AND @WF_FLOW_ID = S.WF_FLOW_ID ",
                    "   AND @WF_FLOW_VER = S.WF_FLOW_VER ",
                    "   AND @WF_NODE_ID = S.WF_NODE_ID ",
                    "   AND @SIG_STEP = S.SIG_STEP",
                    "   AND S.WF_NO = {WF_NO}",
                    Environment.NewLine
                }));

            dbParameters.Add(new DBParameter { Name = WFSigPara.ParaField.WF_NO, Value = para.WFNo });
            return GetEntityList<SigStepUser>(commandText.ToString(), dbParameters);
        }
        #endregion

        /// <summary>
        /// 查詢完成簽核是否自動移至下一節點
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public EnumYN SelectIsSignNextNodeResult(WorkFlowPara para)
        {
            string commandText = string.Concat(new object[]
            {
                " DECLARE @RESULT CHAR(1) = 'N'; ", 
                "  SELECT @RESULT = SN.IS_SIG_NEXT_NODE ", 
                "    FROM WF_NODE WN ",  
                "    JOIN SYS_SYSTEM_WF_NODE SN ",  
                "      ON WN.SYS_ID = SN.SYS_ID ", 
                "     AND WN.WF_FLOW_ID = SN.WF_FLOW_ID ", 
                "     AND WN.WF_FLOW_VER = SN.WF_FLOW_VER ", 
                "     AND WN.WF_NODE_ID = SN.WF_NODE_ID ", 
                "   WHERE WN.WF_NO = {WF_NO} ", 
                "     AND WN.NODE_NO = {NODE_NO} ", 
                "     AND WN.SIG_RESULT_ID = '" + EnumWFNodeSignatureResultID.A + "'; ", 
                "  SELECT @RESULT; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.NODE_NO, Value = para.NodeNo });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
            if (result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumYN.Y;
            }
            return EnumYN.N;
        }

        /// <summary>
        /// 查詢簽核退回是否自動退回節點
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public EnumYN SelectIsSignBackNodeResult(WorkFlowPara para)
        {
            string commandText = string.Concat(new object[]
            {
                " DECLARE @RESULT CHAR(1) = 'N'; ", 
                "  SELECT @RESULT = SN.IS_SIG_BACK_NODE ", 
                "    FROM WF_NODE WN ",  
                "    JOIN SYS_SYSTEM_WF_NODE SN ",  
                "      ON WN.SYS_ID = SN.SYS_ID ", 
                "     AND WN.WF_FLOW_ID = SN.WF_FLOW_ID ", 
                "     AND WN.WF_FLOW_VER = SN.WF_FLOW_VER ", 
                "     AND WN.WF_NODE_ID = SN.WF_NODE_ID ", 
                "   WHERE WN.WF_NO = {WF_NO} ", 
                "     AND WN.NODE_NO = {NODE_NO} ", 
                "     AND WN.SIG_RESULT_ID = '" + EnumWFNodeSignatureResultID.R + "'; ", 
                "  SELECT @RESULT; " 
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.NODE_NO, Value = para.NodeNo });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
            if (result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumYN.Y;
            }
            return EnumYN.N;
        }
        
        public class NexttoNodeAPI : DBTableRow
        {
            public DBChar WFNo;
            public DBVarChar NodeNo;
            public DBVarChar UserID;
            public DBVarChar NewUserID;
            public List<DBVarChar> NewUserIDList;
        }

        public class BacktoNodeAPI : DBTableRow
        {
            public DBChar WFNo;
            public DBVarChar UserID;
        }
    }
}
