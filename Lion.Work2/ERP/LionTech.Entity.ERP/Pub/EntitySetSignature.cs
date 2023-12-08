using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LionTech.Utility;

namespace LionTech.Entity.ERP.Pub
{
    public class EntitySetSignature : EntityPub
    {
        public EntitySetSignature(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 查詢簽核名單 -
        public class SignatureInfoPara : DBCulture
        {
            public SignatureInfoPara(string culture)
                : base(culture)
            {
            }

            public enum ParaField
            {
                WF_NO, NODE_NO,
                SYS_ID, WF_FLOW_ID, WF_FLOW_VER, WF_NODE_ID,
                WF_SIG, CULTURE_ID
            }

            public DBChar WFNo;
            public DBChar NodeNo;

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
        }

        public class SignatureInfo : DBTableRow
        {
            public DBInt SigStep;
            public DBChar WFSigSeq;
            public DBNVarChar WFSigNM;

            public DBNVarChar SelUserID;
            public DBVarChar SigUserID;
            public DBNVarChar SigUserNM;

            public DBVarChar SigKind;
            
            public DBChar IsAddSig;

            public DBVarChar SigType;
            public DBVarChar APISysID;
            public DBVarChar APIControllerID;
            public DBVarChar APIActionName;
            public DBVarChar CompareWFNodeID;
            public DBChar CompareWFSigSeq;
            public DBVarChar ChkAPISysID;
            public DBVarChar ChkAPIControllerID;
            public DBVarChar ChkAPIActionName;
            
            public DBVarChar SigResultID;
            public DBNVarChar SigResultNM;
            public DBChar SigDate;
            public DBChar IsReq;

            public List<RawCMUser> UserIDList;
        }
        
        /// <summary>
        /// 查詢簽核名單
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<SignatureInfo> SelectSignatureInfoList(SignatureInfoPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "IF EXISTS (SELECT * ",
                        "             FROM WF_SIG ",
                        "            WHERE WF_NO = {WF_NO} ",
                        "              AND NODE_NO = {NODE_NO}) ",
                        "BEGIN ",

                        #region - 已設定簽核名單 -
                        "    SELECT W.SIG_STEP AS SigStep ",
                        "         , W.WF_SIG_SEQ AS WFSigSeq ",
                        "         , W.{WF_SIG} AS WFSigNM ",
                        "         , ISNULL(C.USER_NM,'') + ' | ' + W.SIG_USER_ID AS SelUserID ",
                        "         , W.SIG_USER_ID AS SigUserID ",
                        "         , dbo.FN_GET_IDNM(W.SIG_USER_ID, C.USER_NM) AS SigUserNM ",
                        "         , W.SIG_KIND AS SigKind ",
                        "         , (CASE WHEN S.SYS_ID IS NOT NULL THEN 'N' ELSE 'Y' END) AS IsAddSig ",
                        "         , (CASE WHEN S.SYS_ID IS NOT NULL  ",
                        "                 THEN S.SIG_TYPE  ",
                        "                 ELSE 'I' ",
                        "           END) AS SigType ",
                        "         , S.API_SYS_ID AS APISysID ",
                        "         , S.API_CONTROLLER_ID AS APIControllerID ",
                        "         , S.API_ACTION_NAME AS APIActionName ",
                        "         , S.COMPARE_WF_NODE_ID AS CompareWFNodeID ",
                        "         , S.COMPARE_WF_SIG_SEQ AS CompareWFSigSeq ",
                        "         , S.CHK_API_SYS_ID AS ChkAPISysID ",
                        "         , S.CHK_API_CONTROLLER_ID AS ChkAPIControllerID ",
                        "         , S.CHK_API_ACTION_NAME AS ChkAPIActionName ",
                        "         , W.SIG_RESULT_ID AS SigResultID ",
                        "         , dbo.FN_GET_NMID(W.SIG_RESULT_ID, dbo.FN_GET_CM_NM({SignatureResultType}, W.SIG_RESULT_ID, {CULTURE_ID})) AS SigResultNM ",
                        "         , W.SIG_DATE AS SigDate ",
                        "         , W.IS_REQ AS IsReq ",
                        "      FROM WF_SIG W ",
                        "      LEFT JOIN SYS_SYSTEM_WF_SIG S ",
                        "        ON W.SYS_ID = S.SYS_ID ",
                        "       AND W.WF_FLOW_ID = S.WF_FLOW_ID ",
                        "       AND W.WF_FLOW_VER = S.WF_FLOW_VER ",
                        "       AND W.WF_NODE_ID = S.WF_NODE_ID ",
                        "       AND W.WF_SIG_SEQ = S.WF_SIG_SEQ ",
                        "      LEFT JOIN RAW_CM_USER C ",
                        "        ON W.SIG_USER_ID = C.USER_ID ",
                        "     WHERE W.WF_NO = {WF_NO} ",
                        "       AND W.NODE_NO = {NODE_NO} ",
                        "     ORDER BY W.SIG_STEP, W.WF_SIG_SEQ ",
                        #endregion

                        "END; ",
                        "ELSE ",
                        "BEGIN ",

                        #region - 預設簽核名單 -
                        "    SELECT S.SIG_STEP AS SigStep ",
                        "         , S.WF_SIG_SEQ AS WFSigSeq ",
                        "         , S.{WF_SIG} AS WFSigNM ",
                        "         , NULL AS SelUserID ",
                        "         , NULL AS SigUserID ",
                        "         , NULL AS SigUserNM ",
                        "         , NULL AS SigKind ",
                        "         , 'N' AS IsAddSig ",
                        "         , S.SIG_TYPE AS SigType ",
                        "         , S.API_SYS_ID AS APISysID ",
                        "         , S.API_CONTROLLER_ID AS APIControllerID ",
                        "         , S.API_ACTION_NAME AS APIActionName ",
                        "         , S.COMPARE_WF_NODE_ID AS CompareWFNodeID ",
                        "         , S.COMPARE_WF_SIG_SEQ AS CompareWFSigSeq ",
                        "         , S.CHK_API_SYS_ID AS ChkAPISysID ",
                        "         , S.CHK_API_CONTROLLER_ID AS ChkAPIControllerID ",
                        "         , S.CHK_API_ACTION_NAME AS ChkAPIActionName ",
                        "         , NULL AS SigResultID ",
                        "         , NULL AS SigResultNM ",
                        "         , NULL AS SigDate ",
                        "         , S.IS_REQ AS IsReq ",
                        "      FROM SYS_SYSTEM_WF_SIG S ",
                        "     WHERE S.SYS_ID = {SYS_ID} ",
                        "       AND S.WF_FLOW_ID = {WF_FLOW_ID} ",
                        "       AND S.WF_FLOW_VER = {WF_FLOW_VER} ",
                        "       AND S.WF_NODE_ID = {WF_NODE_ID} ",
                        "     ORDER BY S.SIG_STEP, S.WF_SIG_SEQ ",
                        #endregion

                        "END; "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SignatureInfoPara.ParaField.WF_NO.ToString(), Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = SignatureInfoPara.ParaField.NODE_NO.ToString(), Value = para.NodeNo });
            dbParameters.Add(new DBParameter { Name = SignatureInfoPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SignatureInfoPara.ParaField.WF_FLOW_ID.ToString(), Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SignatureInfoPara.ParaField.WF_FLOW_VER.ToString(), Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SignatureInfoPara.ParaField.WF_NODE_ID.ToString(), Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SignatureInfoPara.ParaField.CULTURE_ID.ToString(), Value = new DBChar(para.CultureID) });
            dbParameters.Add(new DBParameter { Name = SignatureInfoPara.ParaField.WF_SIG.ToString(), Value = para.GetCultureFieldNM(new DBObject(SignatureInfoPara.ParaField.WF_SIG.ToString())) });
            dbParameters.Add(new DBParameter { Name = EnumCMCodeKind.SignatureResultType, Value = new DBVarChar(Common.GetEnumDesc(EnumCMCodeKind.SignatureResultType)) });
            return GetEntityList<SignatureInfo>(commandText, dbParameters);
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

        #region - 查詢簽核說明 -
        public class WFSigMemoPara : DBCulture
        {
            public WFSigMemoPara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                SYS_ID, WF_FLOW_ID, WF_FLOW_VER, WF_NODE_ID,
                WF_SIG_MEMO
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
        }
        
        /// <summary>
        /// 查詢簽核說明
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DBNVarChar SelectWFSigMemo(WFSigMemoPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT {WF_SIG_MEMO} AS WFSigMemo",
                    "  FROM SYS_SYSTEM_WF_NODE",
                    " WHERE SYS_ID = {SYS_ID}",
                    "   AND WF_FLOW_ID = {WF_FLOW_ID}",
                    "   AND WF_FLOW_VER = {WF_FLOW_VER}",
                    "   AND WF_NODE_ID = {WF_NODE_ID}"
                }));

            dbParameters.Add(new DBParameter { Name = WFSigMemoPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = WFSigMemoPara.ParaField.WF_FLOW_ID.ToString(), Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = WFSigMemoPara.ParaField.WF_FLOW_VER.ToString(), Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = WFSigMemoPara.ParaField.WF_NODE_ID.ToString(), Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = WFSigMemoPara.ParaField.WF_SIG_MEMO.ToString(), Value = para.GetCultureFieldNM(new DBObject(WFSigMemoPara.ParaField.WF_SIG_MEMO.ToString())) });
            return new DBNVarChar(ExecuteScalar(commandText.ToString(), dbParameters));
        }
        #endregion

        #region - 查詢工作流程節點驗證簽核人員API -
        public class WFNodeSignCheckAPIPara
        {
            public enum ParaField
            {
                SYS_ID, WF_FLOW_ID, WF_FLOW_VER, WF_NODE_ID
            }
            
            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
        }

        public class WFNodeSignCheckAPI : DBTableRow
        {
            public DBVarChar ChkAPISysID;
            public DBVarChar ChkAPIControllerID;
            public DBVarChar ChkAPIActionName;
        }

        /// <summary>
        /// 查詢工作流程節點驗證簽核人員API
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public WFNodeSignCheckAPI SelectWFNodeSignCheckAPI(WFNodeSignCheckAPIPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT CHK_API_SYS_ID AS ChkAPISysID ",
                    "     , CHK_API_CONTROLLER_ID AS ChkAPIControllerID ",
                    "     , CHK_API_ACTION_NAME AS ChkAPIActionName ",
                    "  FROM SYS_SYSTEM_WF_NODE",
                    " WHERE SYS_ID = {SYS_ID}",
                    "   AND WF_FLOW_ID = {WF_FLOW_ID}",
                    "   AND WF_FLOW_VER = {WF_FLOW_VER}",
                    "   AND WF_NODE_ID = {WF_NODE_ID}"
                }));

            dbParameters.Add(new DBParameter { Name = WFNodeSignCheckAPIPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = WFNodeSignCheckAPIPara.ParaField.WF_FLOW_ID.ToString(), Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = WFNodeSignCheckAPIPara.ParaField.WF_FLOW_VER.ToString(), Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = WFNodeSignCheckAPIPara.ParaField.WF_NODE_ID.ToString(), Value = para.WFNodeID });
            return GetEntityList<WFNodeSignCheckAPI>(commandText.ToString(), dbParameters).SingleOrDefault();
        }
        #endregion
        
        public class NodeSigStepDynamicAPIPara : DBTableRow
        {
            public DBChar WFNo;
            public DBChar NodeNo;

            public DBVarChar UserID;
            
            public DBVarChar SysID;
            public DBVarChar FlowID;
            public DBChar FlowVer;
            public DBVarChar NodeID;
        }
        
        public class NodeSigChkAPIPara : DBTableRow
        {
            public class SigUser : DBTableRow
            {
                public DBChar SigStep;
                public DBChar SigSeq;
                public DBVarChar SigUserID;
            }

            public DBChar WFNo;
            public DBChar NodeNo;

            public DBVarChar UserID;

            public DBVarChar SysID;
            public DBVarChar FlowID;
            public DBChar FlowVer;
            public DBVarChar NodeID;
            public List<SigUser> SigUserIDList;
        }

        public class NodeSigSeqAPIPara : DBTableRow
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

        public class NodeSigSeqChkAPIPara : DBTableRow
        {
            public DBChar WFNo;
            public DBChar NodeNo;

            public DBVarChar UserID;
            public DBVarChar SigUserID;

            public DBVarChar SysID;
            public DBVarChar FlowID;
            public DBChar FlowVer;
            public DBVarChar NodeID;
            public DBChar SigSeq;
        }
    }
}
