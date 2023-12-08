// 新增日期：2016-07-11
// 新增人員：王汶智
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.WorkFlowService
{
    public class EntitySetSignature : EntityWorkFlowService
    {
        public EntitySetSignature(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 查詢目前簽核名單資訊 -
        public class SigUserInfoPara
        {
            public enum ParaField
            {
                WF_NO
            }

            public DBChar WFNo;
        }

        public class SigUserInfo : DBTableRow
        {
            public DBNVarChar WFSubject;
            public DBNVarChar WFNodeZHTW;
            public DBNVarChar WFSigZHTW;
            public DBVarChar SigUserID;

            public DBChar SysID;
            public DBChar NodeNo;
            public DBChar WFSigSeq;
            public DBInt SigStep;
            public DBChar SigDate;
            public DBChar SigResultID;
            public DBChar IsCurrentlySig;
        }

        /// <summary>
        /// 查詢目前簽核名單資訊
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<SigUserInfo> SelectSigUserInfoList(SigUserInfoPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @WF_NO CHAR(14) = {WF_NO};",
                    "DECLARE @NODE_NO CHAR(3) = NULL;",
                    "DECLARE @SYS_ID VARCHAR(12) = NULL;",
                    "DECLARE @WF_FLOW_ID VARCHAR(50) = NULL;",
                    "DECLARE @WF_FLOW_VER VARCHAR(50) = NULL;",
                    "DECLARE @WF_NODE_ID VARCHAR(50) = NULL;",
                    "DECLARE @WF_SUBJECT NVARCHAR(150) = NULL;",
                    "DECLARE @WF_NODE_ZH_TW NVARCHAR(150) = NULL;",

                    "DECLARE @SIG_STEP INT = NULL;",

                    "SELECT @NODE_NO = D.NODE_NO",
                    "     , @SYS_ID = D.SYS_ID",
                    "     , @WF_FLOW_ID = D.WF_FLOW_ID",
                    "     , @WF_FLOW_VER = D.WF_FLOW_VER",
                    "     , @WF_NODE_ID = D.WF_NODE_ID",
                    "     , @WF_SUBJECT = D.WF_SUBJECT",
                    "  FROM dbo.FNTB_GET_WF_NODE(@WF_NO) D;",

                    "SELECT @WF_NODE_ZH_TW = SWN.WF_NODE_ZH_TW",
                    "  FROM SYS_SYSTEM_WF_NODE SWN",
                    " WHERE SWN.SYS_ID = @SYS_ID",
                    "   AND SWN.WF_FLOW_ID = @WF_FLOW_VER",
                    "   AND SWN.WF_NODE_ID = @WF_NODE_ID;",

                    "SELECT @SIG_STEP = MIN(TWS.SIG_STEP)",
                    "  FROM WF_SIG TWS",
                    " WHERE TWS.WF_NO = @WF_NO",
                    "   AND TWS.NODE_NO = @NODE_NO;",
                    
                    "SELECT @WF_SUBJECT AS WFSubject",
                    "     , @WF_NODE_ZH_TW AS WFNodeZHTW",
                    "     , WS.WF_SIG_ZH_TW AS WFSigZHTW",
                    "     , WS.SIG_USER_ID AS SigUserID",
                    "     , WS.WF_SIG_SEQ AS WFSigSeq",
                    "     , WS.SIG_STEP AS SigStep",
                    "     , WS.SIG_DATE AS SigDate",
                    "     , WS.SIG_RESULT_ID AS SigResultID",
                    "     , WS.SYS_ID AS SysID",
                    "     , @NODE_NO AS NodeNo",
                    "     , CASE WHEN @SIG_STEP = WS.SIG_STEP",
                    "            THEN 'Y'",
                    "            ELSE 'N'",
                    "       END AS IsCurrentlySig",
                    "  FROM WF_SIG WS",
                    " WHERE WS.WF_NO = @WF_NO",
                    "   AND WS.NODE_NO = @NODE_NO",
                    Environment.NewLine
                }));

            dbParameters.Add(new DBParameter { Name = SigUserInfoPara.ParaField.WF_NO, Value = para.WFNo });
            return GetEntityList<SigUserInfo>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 查詢簽核預設名單 -
        public class DefaultSignatureInfoPara
        {
            public enum ParaField
            {
                WF_NO
            }

            public DBChar WFNo;
        }

        public class DefaultSignatureInfo : DBTableRow
        {
            public DBInt SigStep;
            public DBChar WFSigSeq;
            public DBChar IsReq;
        }

        /// <summary>
        /// 查詢簽核預設名單
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<DefaultSignatureInfo> SelectDefaultSignatureInfoList(DefaultSignatureInfoPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @WF_NO CHAR(14) = {WF_NO}; ",
                        "DECLARE @NODE_NO CHAR(3) = NULL; ",
                        "DECLARE @SYS_ID VARCHAR(12) = NULL; ",
                        "DECLARE @WF_FLOW_ID VARCHAR(50) = NULL; ",
                        "DECLARE @WF_FLOW_VER VARCHAR(50) = NULL; ",
                        "DECLARE @WF_NODE_ID VARCHAR(50) = NULL; ",

                        "SELECT @NODE_NO = D.NODE_NO ",
                        "     , @SYS_ID = D.SYS_ID ",
                        "     , @WF_FLOW_ID = D.WF_FLOW_ID ",
                        "     , @WF_FLOW_VER = D.WF_FLOW_VER ",
                        "     , @WF_NODE_ID = D.WF_NODE_ID ",
                        "  FROM dbo.FNTB_GET_WF_NODE(@WF_NO) D ",
                        
                        "SELECT S.SIG_STEP AS SigStep ",
                        "     , S.WF_SIG_SEQ AS WFSigSeq ",
                        "     , S.IS_REQ AS IsReq ",
                        "  FROM SYS_SYSTEM_WF_SIG S ",
                        " WHERE S.SYS_ID = @SYS_ID ",
                        "   AND S.WF_FLOW_ID = @WF_FLOW_ID ",
                        "   AND S.WF_FLOW_VER = @WF_FLOW_VER ",
                        "   AND S.WF_NODE_ID = @WF_NODE_ID ",
                        " ORDER BY S.SIG_STEP, S.WF_SIG_SEQ ",
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = DefaultSignatureInfoPara.ParaField.WF_NO.ToString(), Value = para.WFNo });
            return GetEntityList<DefaultSignatureInfo>(commandText, dbParameters);
        }
        #endregion

        #region - 查詢文件是否還沒上傳 -
        public class CheckDocRequiredPara
        {
            public enum ParaField
            {
                WF_NO
            }

            public DBChar WFNo;
        }

        /// <summary>
        /// 查詢文件是否還沒上傳
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DBChar SelectCheckDocRequired(CheckDocRequiredPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    
                     "DECLARE @NODE_NO CHAR(3) = NULL; ",
                     "DECLARE @SYS_ID VARCHAR(12) = NULL; ",
                     "DECLARE @WF_FLOW_ID VARCHAR(50) = NULL; ",
                     "DECLARE @WF_FLOW_VER VARCHAR(50) = NULL; ",
                     "DECLARE @WF_NODE_ID VARCHAR(50) = NULL; ",

                    "SELECT @NODE_NO = D.NODE_NO ",
                    "     , @SYS_ID = D.SYS_ID ",
                    "     , @WF_FLOW_ID = D.WF_FLOW_ID ",
                    "     , @WF_FLOW_VER = D.WF_FLOW_VER ",
                    "     , @WF_NODE_ID = D.WF_NODE_ID ",
                    "  FROM dbo.FNTB_GET_WF_NODE({WF_NO}) D ",

                    "IF EXISTS (SELECT * ",
                    "             FROM SYS_SYSTEM_WF_DOC D ",
                    "             LEFT JOIN (",
                    "                      SELECT * ",
                    "                        FROM WF_DOC ",
                    "                       WHERE WF_NO = {WF_NO} ",
                    "                         AND IS_DELETE = 'N'",
                    "                  ) W ",
                    "               ON D.SYS_ID = W.SYS_ID ",
                    "              AND D.WF_FLOW_ID = W.WF_FLOW_ID ",
                    "              AND D.WF_FLOW_VER = W.WF_FLOW_VER ",
                    "              AND D.WF_NODE_ID = W.WF_NODE_ID ",
                    "              AND D.WF_DOC_SEQ = W.WF_DOC_SEQ ",
                    "            WHERE D.SYS_ID = @SYS_ID ",
                    "              AND D.WF_FLOW_ID = @WF_FLOW_ID ",
                    "              AND D.WF_FLOW_VER = @WF_FLOW_VER ",
                    "              AND D.WF_NODE_ID = @WF_NODE_ID ",
                    "              AND D.IS_REQ = 'Y' ",
                    "              AND W.DOC_NO IS NULL) ",
                    "BEGIN ",
                    "   SET @RESULT = 'Y';",
                    "END; ",
                    "SELECT @RESULT"
                }));

            dbParameters.Add(new DBParameter { Name = CheckDocRequiredPara.ParaField.WF_NO, Value = para.WFNo });
            return new DBChar(ExecuteScalar(commandText.ToString(), dbParameters));
        }
        #endregion

        #region - 編輯簽核名單 -
        public enum EnumSetSignatureResult
        {
            Success,
            Failure,
            WFFlowTerminal,
            WFFlowCancel
        }

        public class SetNodePara
        {
            public enum ParaField
            {
                WF_NO,
                IS_START_SIG,
                UPD_USER_ID
            }

            public DBChar WFNo;
            public DBVarChar UpdUserID;
            public DBBit IsStartSig;
        }

        public class SetSigPara
        {
            public enum ParaField
            {
                SIG_STEP, WF_SIG_SEQ,
                SIG_USER_ID,
                UPD_USER_ID
            }

            public DBInt SigStep;
            public DBChar WFSigSeq;

            public DBVarChar SigUserID;

            public DBVarChar UpdUserID;
        }

        public class SetSigExecuteResult : ExecuteResult
        {
            public DBNVarChar WFFlowNM;
            public DBNVarChar WFNodeNM;
        }

        /// <summary>
        /// 編輯簽核名單
        /// </summary>
        /// <param name="nodePara"></param>
        /// <param name="sigParaList"></param>
        /// <returns></returns>
        public SetSigExecuteResult EditSetSigList(SetNodePara nodePara, List<SetSigPara> sigParaList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();
            
            foreach (var sigPara in sigParaList)
            {
                dbParameters.Add(new DBParameter { Name = SetSigPara.ParaField.SIG_STEP.ToString(), Value = sigPara.SigStep });
                dbParameters.Add(new DBParameter { Name = SetSigPara.ParaField.WF_SIG_SEQ.ToString(), Value = sigPara.WFSigSeq });
                dbParameters.Add(new DBParameter { Name = SetSigPara.ParaField.SIG_USER_ID.ToString(), Value = sigPara.SigUserID });
                dbParameters.Add(new DBParameter { Name = SetSigPara.ParaField.UPD_USER_ID.ToString(), Value = sigPara.UpdUserID });

                commandTextStringBuilder.Append(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                    new object[]
                    {
                        "INSERT INTO @WF_SIG_LIST (SIG_STEP, WF_SIG_SEQ, SIG_USER_ID, UPD_USER_ID)",
                        "SELECT {SIG_STEP}, {WF_SIG_SEQ}, {SIG_USER_ID}, {UPD_USER_ID};",
                        Environment.NewLine
                    }), dbParameters));
                dbParameters.Clear();
            }

            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @WF_SIG_LIST AS dbo.WF_SIG_TYPE;",
                        commandTextStringBuilder.ToString(),
                        "EXECUTE dbo.SP_WF_SET_SIGNATURE {WF_NO}, {IS_START_SIG}, {UPD_USER_ID}, @WF_SIG_LIST;"
                    });

            dbParameters.Add(new DBParameter { Name = SetNodePara.ParaField.WF_NO.ToString(), Value = nodePara.WFNo });
            dbParameters.Add(new DBParameter { Name = SetNodePara.ParaField.IS_START_SIG.ToString(), Value = nodePara.IsStartSig });
            dbParameters.Add(new DBParameter { Name = SetNodePara.ParaField.UPD_USER_ID.ToString(), Value = nodePara.UpdUserID });

            var result = GetEntityList<SetSigExecuteResult>(commandText, dbParameters).SingleOrDefault();

            var enumResult = ((EnumSetSignatureResult)Enum.Parse(typeof(EnumSetSignatureResult), result.Result.GetValue()));

            if (enumResult == EnumSetSignatureResult.Failure)
            {
                throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
            }

            return result;
        }
        #endregion
    }
}