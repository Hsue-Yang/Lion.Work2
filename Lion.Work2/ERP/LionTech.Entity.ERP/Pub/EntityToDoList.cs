using System;
using System.Collections.Generic;
using System.Linq;
using LionTech.Utility;

namespace LionTech.Entity.ERP.Pub
{
    public class EntityToDoList : EntityPub
    {
        public EntityToDoList(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 查詢節點URL -
        public class WFNodeURLPara
        {
            public enum ParaField
            {
                WF_NO
            }

            public DBChar WFNo;
        }

        public class WFNodeURL : DBTableRow
        {
            public DBVarChar FunSysID;
            public DBVarChar SubSysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;

            public DBVarChar NodeUrl;
        }

        /// <summary>
        /// 查詢節點URL
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public WFNodeURL SelectWFNodeUrl(WFNodeURLPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @NODE_NO CHAR(3) = NULL; ",
                        "DECLARE @WF_NODE_ID VARCHAR(50) = NULL; ",
                        "SELECT @NODE_NO = NODE_NO ",
                        "     , @WF_NODE_ID =",
                        "           CASE WHEN RESULT_ID = '" + EnumWFResultID.F + "'",
                        "                THEN 'END'",
                        "                ELSE 'START'",
                        "           END",
                        "  FROM WF_FLOW ",
                        " WHERE WF_NO = {WF_NO}; ",
                        "",
                        "IF @NODE_NO IS NOT NULL ",
                        "BEGIN ",
                        "    SELECT N.FUN_SYS_ID AS FunSysID ",
                        "         , F.SUB_SYS_ID AS SubSysID ",
                        "         , N.FUN_CONTROLLER_ID AS FunControllerID ",
                        "         , N.FUN_ACTION_NAME AS FunActionName ",
                        "      FROM WF_FLOW M ",
                        "      JOIN WF_NODE W ",
                        "        ON M.WF_NO = W.WF_NO ",
                        "       AND M.NODE_NO = W.NODE_NO ",
                        "      JOIN SYS_SYSTEM_WF_NODE N ",
                        "        ON W.SYS_ID = N.SYS_ID ",
                        "       AND W.WF_FLOW_ID = N.WF_FLOW_ID ",
                        "       AND W.WF_FLOW_VER = N.WF_FLOW_VER ",
                        "       AND W.WF_NODE_ID = N.WF_NODE_ID ",
                        "      JOIN SYS_SYSTEM_FUN F ",
                        "        ON N.FUN_SYS_ID = F.SYS_ID ",
                        "       AND N.FUN_CONTROLLER_ID = F.FUN_CONTROLLER_ID ",
                        "       AND N.FUN_ACTION_NAME = F.FUN_ACTION_NAME ",
                        "     WHERE M.WF_NO = {WF_NO}",
                        "END; ",
                        "ELSE ",
                        "BEGIN ",
                        "    SELECT N.FUN_SYS_ID AS FunSysID ",
                        "         , F.SUB_SYS_ID AS SubSysID ",
                        "         , N.FUN_CONTROLLER_ID AS FunControllerID ",
                        "         , N.FUN_ACTION_NAME AS FunActionName",
                        "      FROM WF_FLOW M ",
                        "      JOIN SYS_SYSTEM_WF_NODE N ",
                        "        ON M.SYS_ID = N.SYS_ID ",
                        "       AND M.WF_FLOW_ID = N.WF_FLOW_ID ",
                        "       AND M.WF_FLOW_VER = N.WF_FLOW_VER ",
                        "      JOIN SYS_SYSTEM_FUN F ",
                        "        ON N.FUN_SYS_ID = F.SYS_ID ",
                        "       AND N.FUN_CONTROLLER_ID = F.FUN_CONTROLLER_ID ",
                        "       AND N.FUN_ACTION_NAME = F.FUN_ACTION_NAME ",
                        "     WHERE M.WF_NO = {WF_NO}",
                        "       AND N.WF_NODE_ID = @WF_NODE_ID ",
                        "END; ",
                        Environment.NewLine,
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = WFNodeURLPara.ParaField.WF_NO, Value = para.WFNo });
            return GetEntityList<WFNodeURL>(commandText, dbParameters).SingleOrDefault();
        }
        #endregion
        
        #region - 查詢代辦清單 -
        public class WFFlowPara : DBCulture
        {
            public WFFlowPara(string culture)
                : base(culture)
            {
            }

            public enum ParaField
            {
                WF_NO,
                FLOW_TYPE,
                SYS_ID,
                WF_FLOW_ID,
                WF_FLOW_VER,
                WF_DT_BEGIN_START,
                WF_DT_BEGIN_STOP,
                WF_DT_END_START,
                WF_DT_END_STOP,
                WF_SUBJECT,
                LOT,
                WF_NEW_USER_ID,
                WF_RESULT_ID,
                NODE_NEW_USER_ID,
                NODE_RESULT_ID,
                SIG_USER_ID,
                SIG_RESULT_ID,
                USER_ID,
                SIG_USER_ID_STR,
                NEW_USER_ID_STR,
                SORT_ORDER,
                CULTURE_ID,
                WF_FLOW,
                WF_NODE,
                WF_SIG
            }

            public enum OrderField
            {
                WF_NO,
                FLOW_TYPE,
                FLOW_SORT_ORDER
            }

            public DBChar WFNo;
            public DBVarChar FlowType;
            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;

            public DBChar WFDTBeginStart;
            public DBChar WFDTBeginStop;
            public DBChar WFDTEndStart;
            public DBChar WFDTEndStop;
            public DBObject WFSubject;
            public DBVarChar Lot;

            public DBVarChar WFNewUserID;
            public List<DBVarChar> WFResultID;

            public DBVarChar NodeNewUserID;
            public List<DBVarChar> NodeResultID;
            public bool IsNullNodeNewUserID;

            public DBObject SigUserID;
            public List<DBVarChar> SigResultID;

            public DBVarChar UserID;
            public DBObject SigUserIDStr;
            public DBObject NewUserIDStr;
            public OrderField SortOrder;
            public new DBVarChar CultureID;
        }

        public class WFFlow : DBTableRow
        {
            public DBChar WFNo;
            public DBChar FormatWFNo;
            public DBVarChar FlowType;
            public DBNVarChar FlowTypeNM;

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBNVarChar WFFlowNM;
            public DBNVarChar WFFlowDesc;
            public DBNVarChar WFSubject;
            public DBVarChar WFNewUserID;
            public DBNVarChar WFNewUserNM;
            public DBYMD WFBeginDate;
            public DBYMD WFEndDate;
            public DBVarChar WFResultID;
            public DBNVarChar WFResultNM;
            
            
            public DBChar NodeNo;
            public DBVarChar WFNodeID;
            public DBNVarChar WFNodeNM;
            public DBNVarChar WFNodeDesc;
            public DBVarChar NodeNewUserID;
            public DBNVarChar NodeNewUserNM;
            public DBNVarChar NodeNewUserNMStr;
            public DBYMD NodeBeginDate;
            public DBYMD NodeEndDate;
            public DBVarChar NodeResultID;
            public DBNVarChar NodeResultNM;

            public DBChar IsStartSig;
            public DBVarChar WFSigNM;
            public DBNVarChar SigUserNMStr;
            public DBVarChar SigResultID;
            public DBNVarChar SigResultNM;
            public DBChar IsSigCurrently;
            
        }

        /// <summary>
        /// 查詢代辦清單
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<WFFlow> SelectWFFlowList(WFFlowPara para)
        {
            List<string> commandWhere = new List<string>();
            string commandOrder = "ORDER BY F.WF_NO DESC ";

            if (para.WFNo.IsNull())
            {
                if (para.FlowType.IsNull() == false)
                {
                    commandWhere.Add(" M.FLOW_TYPE = {FLOW_TYPE} ");
                }
                if (para.SysID.IsNull() == false)
                {
                    commandWhere.Add(" F.SYS_ID = {SYS_ID} ");
                }
                if (para.WFFlowID.IsNull() == false)
                {
                    commandWhere.Add(" F.WF_FLOW_ID = {WF_FLOW_ID} ");
                }
                if (para.WFFlowVer.IsNull() == false)
                {
                    commandWhere.Add(" F.WF_FLOW_VER = {WF_FLOW_VER} ");
                }
                if (para.WFDTEndStart.IsNull() == false)
                {
                    commandWhere.Add(" F.DT_END >= {WF_DT_END_START} + '000000000' ");
                }
                if (para.WFDTEndStop.IsNull() == false)
                {
                    commandWhere.Add(" F.DT_END <= {WF_DT_END_STOP} + '999999999' ");
                }
                if (para.WFSubject.IsNull() == false)
                {
                    commandWhere.Add(" F.WF_SUBJECT LIKE N'%{WF_SUBJECT}%' ");
                }
                if (para.Lot.IsNull() == false)
                {
                    commandWhere.Add(" AND F.LOT = {LOT} ");
                }
                if (para.WFNewUserID.IsNull() == false)
                {
                    commandWhere.Add(" F.NEW_USER_ID = {WF_NEW_USER_ID} ");
                }
                if (para.WFResultID != null &&
                    para.WFResultID.Count > 0)
                {
                    commandWhere.Add(" F.RESULT_ID IN ({WF_RESULT_ID}) ");
                }
                if (para.NodeNewUserID.IsNull() == false)
                {
                    commandWhere.Add(" N.NEW_USER_ID = {NODE_NEW_USER_ID} ");
                }
                if (para.IsNullNodeNewUserID)
                {
                    commandWhere.Add(" N.NEW_USER_ID IS NULL ");
                }
                if (para.NodeResultID != null &&
                    para.NodeResultID.Any())
                {
                    commandWhere.Add(" N.RESULT_ID IN ({NODE_RESULT_ID}) ");
                }
                if (para.SigResultID != null &&
                    para.SigResultID.Any())
                {
                    commandWhere.Add(" N.SIG_RESULT_ID IN ({SIG_RESULT_ID}) ");
                }
                if (para.SigUserID.IsNull() == false)
                {
                    commandWhere.Add(" N.SIG_USER_ID_STR LIKE '%{SIG_USER_ID},%' ");
                }
            }

            if (para.SortOrder == WFFlowPara.OrderField.FLOW_TYPE)
            {
                commandOrder = "ORDER BY M.FLOW_TYPE, F.WF_NO DESC ";
            }

            if (para.SortOrder == WFFlowPara.OrderField.FLOW_SORT_ORDER)
            {
                commandOrder = "ORDER BY M.SORT_ORDER, F.WF_NO DESC ";
            }

            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @DT_BEGIN AS CHAR(17) = {WF_DT_BEGIN_START} + '000000000';",
                        "DECLARE @DT_END AS CHAR(17) = {WF_DT_BEGIN_STOP} + '999999999';",
                        "DECLARE @USER_ID AS VARCHAR(10) = {USER_ID};",
                        "DECLARE @CULTURE_ID AS CHAR(5) = {CULTURE_ID};",
                        ";WITH TEMP_WF_FLOW AS (",
                        // 撈取符合單據
                        "    SELECT F.*",
                        "    FROM (",
                        // 申請者、節點處理人、簽核者、節點處理人清單
                        "        SELECT F.WF_NO ",
                        "          FROM WF_FLOW F WITH (NOLOCK) ",
                        "          LEFT JOIN WF_NODE N WITH (NOLOCK) ",
                        "            ON F.WF_NO = N.WF_NO ",
                        "         WHERE F.DT_BEGIN BETWEEN @DT_BEGIN AND @DT_END ",
                        (!para.WFNo.IsNull()) ? " AND F.WF_NO = {WF_NO} " : string.Empty,
                        "           AND (F.NEW_USER_ID = @USER_ID ",
                        "            OR  N.NEW_USER_ID = @USER_ID ",
                        "            OR  (N.IS_START_SIG = 'Y' AND N.SIG_USER_ID_STR LIKE '%{SIG_USER_ID_STR},%') ",
                        "            OR  N.NEW_USER_ID_STR LIKE '%{NEW_USER_ID_STR},%') ",
                        "         UNION ",
                        // 目前節點角色人員
                        "        SELECT F.WF_NO ",
                        "          FROM WF_FLOW F WITH (NOLOCK) ",
                        "          JOIN WF_NODE N WITH (NOLOCK) ",
                        "            ON F.WF_NO = N.WF_NO ",
                        "           AND F.NODE_NO = N.NODE_NO ",
                        "          JOIN SYS_SYSTEM_ROLE_NODE Y WITH (NOLOCK) ",
                        "            ON N.SYS_ID = Y.SYS_ID ",
                        "           AND N.WF_FLOW_ID = Y.WF_FLOW_ID ",
                        "           AND N.WF_FLOW_VER = Y.WF_FLOW_VER ",
                        "           AND N.WF_NODE_ID = Y.WF_NODE_ID ",
                        "          JOIN SYS_USER_SYSTEM_ROLE R WITH (NOLOCK) ",
                        "            ON R.USER_ID = @USER_ID ",
                        "           AND Y.SYS_ID = R.SYS_ID ",
                        "           AND Y.ROLE_ID = R.ROLE_ID ",
                        "         WHERE F.DT_BEGIN BETWEEN @DT_BEGIN AND @DT_END ",
                        (!para.WFNo.IsNull()) ? " AND F.WF_NO = {WF_NO} " : string.Empty,
                        "         UNION ",
                        // 目前已簽核過人員
                        "        SELECT F.WF_NO ",
                        "          FROM WF_FLOW F WITH (NOLOCK) ",
                        "          JOIN WF_NODE N WITH (NOLOCK) ",
                        "            ON F.WF_NO = N.WF_NO ",
                        "           AND F.NODE_NO = N.NODE_NO ",
                        "          JOIN WF_SIG S WITH (NOLOCK) ",
                        "            ON F.WF_NO = S.WF_NO ",
                        "           AND F.SYS_ID = S.SYS_ID ",
                        "           AND F.WF_FLOW_ID = S.WF_FLOW_ID ",
                        "           AND F.WF_FLOW_VER = S.WF_FLOW_VER ",
                        "           AND N.WF_NODE_ID = S.WF_NODE_ID ",
                        "           AND N.NODE_NO = S.NODE_NO ",
                        "           AND S.SIG_DATE IS NOT NULL ",
                        "           AND S.SIG_USER_ID = @USER_ID ",
                        "         WHERE F.DT_BEGIN BETWEEN @DT_BEGIN AND @DT_END ",
                        (!para.WFNo.IsNull()) ? " AND F.WF_NO = {WF_NO} " : string.Empty,
                        "         ) A  ",
                        "    JOIN WF_FLOW F WITH (NOLOCK)",
                        "      ON A.WF_NO = F.WF_NO ",
                        "), TEMP_WF_SG AS (",
                        // 簽核名單
                        "    SELECT WS.WF_NO",
                        "         , WS.NODE_NO",
                        "         , WS.SIG_STEP",
                        "         , WS.WF_SIG_SEQ",
                        "         , WS.SIG_USER_ID",
                        "         , WS.SIG_DATE",
                        "         , WS.WF_SIG_ZH_TW AS WF_SIG_NM ",
                        "      FROM WF_SIG WS WITH (NOLOCK) ",
                        "      JOIN TEMP_WF_FLOW F ",
                        "        ON F.WF_NO = WS.WF_NO ",
                        "), TEMP_WF_NODE1 AS (",
                        // 撈取目前+前一節點
                        "	  SELECT *",
                        "        FROM (",
                        "           SELECT ROW_NUMBER() OVER(PARTITION BY N.WF_NO ORDER BY N.NODE_NO DESC) AS SN",
                        "                , N.WF_NO",
                        "                , N.NODE_NO",
                        "                , N.SYS_ID",
                        "                , N.WF_FLOW_ID",
                        "                , N.WF_FLOW_VER",
                        "                , N.WF_NODE_ID",
                        "                , N.NEW_USER_ID",
                        "                , N.NEW_USER_ID_STR",
                        "                , N.NEW_USER_NM_STR",
                        "                , N.END_USER_ID",
                        "                , N.DT_BEGIN",
                        "                , N.DT_END",
                        "                , N.RESULT_ID",
                        "                , N.RESULT_VALUE",
                        "                , N.IS_START_SIG",
                        "                , N.SIG_STEP",
                        "                , N.SIG_RESULT_ID",
                        "                , N.SIG_USER_ID_STR",
                        "                , N.SIG_USER_NM_STR",
                        "	          FROM WF_NODE N WITH (NOLOCK) ",
                        "             JOIN TEMP_WF_FLOW F ",
                        "               ON F.WF_NO = N.WF_NO ",
                        "        ) A",
                        "      WHERE A.SN < 3  ",
                        "), TEMP_WF_NODE2 AS (",
                        // 撈取退回節點or簽退
                        "   SELECT N2.WF_NO",
                        "        , N1.NODE_NO",
                        "        , N2.SYS_ID",
                        "        , N2.WF_FLOW_ID",
                        "        , N2.WF_FLOW_VER",
                        "        , N2.WF_NODE_ID",
                        "        , N2.NEW_USER_ID",
                        "        , N2.NEW_USER_ID_STR",
                        "        , N2.NEW_USER_NM_STR",
                        "        , N2.END_USER_ID",
                        "        , N2.DT_BEGIN",
                        "        , N2.DT_END",
                        "        , N2.RESULT_ID",
                        "        , N2.RESULT_VALUE",
                        "        , N2.IS_START_SIG",
                        "        , N2.SIG_STEP",
                        "        , N2.SIG_RESULT_ID",
                        "        , N2.SIG_USER_ID_STR",
                        "        , N2.SIG_USER_NM_STR",
                        "     FROM TEMP_WF_NODE1 N1 ",
                        "     JOIN TEMP_WF_NODE1 N2 ",
                        "       ON N1.WF_NO = N2.WF_NO ",
                        "      AND N1.SYS_ID = N2.SYS_ID ",
                        "      AND N1.WF_FLOW_ID = N2.WF_FLOW_ID ",
                        "      AND N1.WF_FLOW_VER = N2.WF_FLOW_VER ",
                        "	  AND N1.SN = N2.SN - 1",
                        "      AND (",
                        "           (N1.RESULT_ID = '" + EnumWFNodeResultID.P + "' AND (N1.IS_START_SIG = 'N' OR N1.IS_START_SIG IS NULL)", // 目前處理節點,未開始簽核
                        "      AND   ((N2.SIG_RESULT_ID = '" + EnumWFNodeSignatureResultID.R + "' AND N2.RESULT_ID = '" + EnumWFResultID.B + "') ", // 前一節點,簽退and節點退回
                        "       OR    (N2.SIG_RESULT_ID = '" + EnumWFNodeSignatureResultID.R + "' AND N2.RESULT_ID = '" + EnumWFResultID.P + "') ", // 前一節點,簽退and節點處理中
                        "       OR    (N2.SIG_RESULT_ID = '" + EnumWFNodeSignatureResultID.P + "' AND N2.RESULT_ID = '" + EnumWFResultID.B + "') ", // 前一節點,簽核處理中and節點退回
                        "       OR    (N2.SIG_RESULT_ID IS NULL AND N2.RESULT_ID = '" + EnumWFResultID.B + "') ", // 前一節點,簽核處理中and節點退回
                        "            ))",
                        "          )",
                        "   UNION",
                        // 撈取目前節點(不含退回節點、簽退)
                        "   SELECT N1.WF_NO",
                        "        , N1.NODE_NO",
                        "        , N1.SYS_ID",
                        "        , N1.WF_FLOW_ID",
                        "        , N1.WF_FLOW_VER",
                        "        , N1.WF_NODE_ID",
                        "        , N1.NEW_USER_ID",
                        "        , N1.NEW_USER_ID_STR",
                        "        , N1.NEW_USER_NM_STR",
                        "        , N1.END_USER_ID",
                        "        , N1.DT_BEGIN",
                        "        , N1.DT_END",
                        "        , N1.RESULT_ID",
                        "        , N1.RESULT_VALUE",
                        "        , N1.IS_START_SIG",
                        "        , N1.SIG_STEP",
                        "        , N1.SIG_RESULT_ID",
                        "        , N1.SIG_USER_ID_STR",
                        "        , N1.SIG_USER_NM_STR",
                        "     FROM TEMP_WF_NODE1 N1 ",
                        "     LEFT JOIN TEMP_WF_NODE1 N2 ",
                        "       ON N1.WF_NO = N2.WF_NO ",
                        "      AND N1.SYS_ID = N2.SYS_ID ",
                        "      AND N1.WF_FLOW_ID = N2.WF_FLOW_ID ",
                        "      AND N1.WF_FLOW_VER = N2.WF_FLOW_VER ",
                        "	  AND N1.SN = N2.SN - 1",
                        "	WHERE N2.WF_NO IS NULL",
                        "	   OR (N2.RESULT_ID = '" + EnumWFNodeResultID.N + "')", // 前一節點,移至下一節點
                        "      OR (N1.RESULT_ID = '" + EnumWFNodeResultID.P + "' AND N1.IS_START_SIG = 'Y')", // 目前處理節點,已開始簽核
                        "), TEMP_WF_NODE3 AS (",
                        "   SELECT N.WF_NO",
                        "        , N.NODE_NO",
                        "        , D.SYS_ID",
                        "        , D.WF_FLOW_ID",
                        "        , D.WF_FLOW_VER",
                        "        , D.WF_NODE_ID",
                        "        , D.WF_NODE_ZH_TW AS WF_NODE_NM",
                        "        , (CASE WHEN N.NODE_NO IS NULL THEN NULL ELSE N.NODE_NO + '-' + N.WF_NODE_ID END) AS WF_NODE_DESC ",
                        "        , N.NEW_USER_NM_STR AS NODE_NEW_USER_NM_STR ",
                        "        , N.RESULT_ID AS NODE_RESULT_ID",
                        "        , dbo.FN_GET_CM_NM({WorkFlowResultType}, N.RESULT_ID, @CULTURE_ID) AS NODE_RESULT_NM ",
                        "        , N.IS_START_SIG",
                        "        , N.NEW_USER_ID AS NODE_NEW_USER_ID",
                        "        , dbo.FN_GET_USER_NM(N.NEW_USER_ID) AS NODE_NEW_USER_NM",
                        "        , SUBSTRING(N.DT_BEGIN, 1, 8) AS NODE_BEGIN_DATE",
                        "        , SUBSTRING(N.DT_END, 1, 8) AS NODE_END_DATE",
                        "        , N.SIG_USER_ID_STR ",
                        "        , N.SIG_USER_NM_STR",
                        "        , N.SIG_RESULT_ID",
                        "        , dbo.FN_GET_CM_NM({SignatureResultType}, N.SIG_RESULT_ID, @CULTURE_ID) AS SIG_RESULT_NM ",
                        "        , N.SIG_STEP ",
                        "        , N.NEW_USER_ID ",
                        "        , (SELECT WF_SIG_NM + '<BR/>'",
                        "             FROM TEMP_WF_SG S ",
                        "            WHERE F.WF_NO = S.WF_NO ",
                        "              AND F.NODE_NO = S.NODE_NO ",
                        "              AND N.SIG_STEP = S.SIG_STEP ",
                        "            ORDER BY S.SIG_STEP, S.WF_SIG_SEQ ",
                        "              FOR XML PATH('')) AS WF_SIG_NM",
                        "     FROM TEMP_WF_FLOW F ",
                        "     JOIN TEMP_WF_NODE2 N ",
                        "       ON F.WF_NO = N.WF_NO ",
                        "      AND F.NODE_NO = N.NODE_NO ",
                        "     JOIN SYS_SYSTEM_WF_NODE D WITH (NOLOCK) ",
                        "       ON F.SYS_ID = D.SYS_ID ",
                        "      AND F.WF_FLOW_ID = D.WF_FLOW_ID ",
                        "      AND F.WF_FLOW_VER = D.WF_FLOW_VER ",
                        "      AND N.WF_NODE_ID = D.WF_NODE_ID ",
                        "      AND D.NODE_TYPE = '" + EnumWFNodeTypeID.P + "' ",
                        ")",
                        "SELECT F.WF_NO AS WFNo ",
                        "     , dbo.FN_GET_WF_NO(F.WF_NO) AS FormatWFNo ",
                        "     , M.FLOW_TYPE AS FlowType ",
                        "     , dbo.FN_GET_CM_NM({WorkFlowType}, M.FLOW_TYPE, @CULTURE_ID) AS FlowTypeNM ",
                        "     , F.SYS_ID AS SysID ",
                        "     , F.WF_FLOW_ID AS WFFlowID ",
                        "     , F.WF_FLOW_VER AS WFFlowVer ",
                        "     , M.WF_FLOW_ZH_TW AS WFFlowNM ",
                        "     , F.SYS_ID + '/' + F.WF_FLOW_ID + '-' + F.WF_FLOW_VER AS WFFlowDesc ",
                        "     , F.WF_SUBJECT AS WFSubject ",
                        "     , F.NEW_USER_ID AS WFNewUserID",
                        "     , dbo.FN_GET_USER_NM(F.NEW_USER_ID) AS WFNewUserNM ",
                        "     , SUBSTRING(F.DT_BEGIN,1,8) AS WFBeginDate ",
                        "     , SUBSTRING(F.DT_END,1,8) AS WFEndDate ",
                        "     , F.RESULT_ID AS WFResultID",
                        "     , dbo.FN_GET_CM_NM({WorkFlowResultType}, F.RESULT_ID, @CULTURE_ID) AS WFResultNM ",
                        "     , N.NODE_NO AS NodeNo ",
                        "     , N.WF_NODE_ID AS WFNodeID ",
                        "     , N.WF_NODE_NM AS WFNodeNM ",
                        "     , N.WF_NODE_DESC AS WFNodeDesc ",
                        "     , N.NODE_NEW_USER_ID AS NodeNewUserID ",
                        "     , N.NODE_NEW_USER_NM AS NodeNewUserNM ",
                        "     , N.NODE_NEW_USER_NM_STR AS NodeNewUserNMStr ",
                        "     , N.NODE_BEGIN_DATE AS NodeBeginDate ",
                        "     , N.NODE_END_DATE AS NodeEndDate ",
                        "     , N.NODE_RESULT_ID AS NodeResultID ",
                        "     , N.NODE_RESULT_NM AS NodeResultNM ",
                        "     , N.IS_START_SIG AS IsStartSig ",
                        "     , N.SIG_USER_NM_STR AS SigUserNMStr ",
                        "     , N.SIG_RESULT_ID AS SigResultID ",
                        "     , N.SIG_RESULT_NM AS SigResultNM ",
                        "     , N.WF_SIG_NM AS WFSigNM",
                        "     , CASE WHEN EXISTS (",
                        "                    SELECT WF_SIG_NM",
                        "                      FROM TEMP_WF_SG S ",
                        "                     WHERE F.WF_NO = S.WF_NO ",
                        "                       AND F.NODE_NO = S.NODE_NO ",
                        "                       AND N.SIG_STEP = S.SIG_STEP ",
                        "                       AND S.SIG_USER_ID = @USER_ID ",
                        "                       AND S.SIG_DATE IS NULL",
                        "                 )",
                        "            THEN 'Y'",
                        "            ELSE 'N' ",
                        "       END AS IsSigCurrently",
                        "FROM TEMP_WF_FLOW F ",
                        "JOIN SYS_SYSTEM_WF_FLOW M WITH (NOLOCK) ",
                        "  ON F.SYS_ID = M.SYS_ID ",
                        " AND F.WF_FLOW_ID = M.WF_FLOW_ID ",
                        " AND F.WF_FLOW_VER = M.WF_FLOW_VER ",
                        "LEFT JOIN TEMP_WF_NODE3 N ",
                        "  ON F.WF_NO = N.WF_NO ",
                        " AND F.NODE_NO = N.NODE_NO ",
                        commandWhere.Any()
                            ? $" WHERE {string.Join(" AND ", commandWhere)}"
                            : string.Empty,
                        commandOrder,
                        Environment.NewLine
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.WF_NO.ToString(), Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.FLOW_TYPE.ToString(), Value = para.FlowType });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.WF_FLOW_ID.ToString(), Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.WF_FLOW_VER.ToString(), Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.WF_DT_BEGIN_START.ToString(), Value = para.WFDTBeginStart });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.WF_DT_BEGIN_STOP.ToString(), Value = para.WFDTBeginStop });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.WF_DT_END_START.ToString(), Value = para.WFDTEndStart });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.WF_DT_END_STOP.ToString(), Value = para.WFDTEndStop });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.WF_SUBJECT.ToString(), Value = para.WFSubject });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.LOT.ToString(), Value = para.Lot });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.WF_NEW_USER_ID.ToString(), Value = para.WFNewUserID });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.WF_RESULT_ID.ToString(), Value = para.WFResultID });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.NODE_NEW_USER_ID.ToString(), Value = para.NodeNewUserID });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.NODE_RESULT_ID.ToString(), Value = para.NodeResultID });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.SIG_USER_ID.ToString(), Value = para.SigUserID });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.SIG_RESULT_ID.ToString(), Value = para.SigResultID });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.SIG_USER_ID_STR.ToString(), Value = para.SigUserIDStr });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.NEW_USER_ID_STR.ToString(), Value = para.NewUserIDStr });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.CULTURE_ID, Value = para.CultureID });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.WF_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(WFFlowPara.ParaField.WF_FLOW.ToString())) });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.WF_NODE.ToString(), Value = para.GetCultureFieldNM(new DBObject(WFFlowPara.ParaField.WF_NODE.ToString())) });
            dbParameters.Add(new DBParameter { Name = WFFlowPara.ParaField.WF_SIG.ToString(), Value = para.GetCultureFieldNM(new DBObject(WFFlowPara.ParaField.WF_SIG.ToString())) });
            dbParameters.Add(new DBParameter { Name = EnumCMCodeKind.WorkFlowType, Value = new DBVarChar(Common.GetEnumDesc(EnumCMCodeKind.WorkFlowType)) });
            dbParameters.Add(new DBParameter { Name = EnumCMCodeKind.WorkFlowResultType, Value = new DBVarChar(Common.GetEnumDesc(EnumCMCodeKind.WorkFlowResultType)) });
            dbParameters.Add(new DBParameter { Name = EnumCMCodeKind.SignatureResultType, Value = new DBVarChar(Common.GetEnumDesc(EnumCMCodeKind.SignatureResultType)) });
            return GetEntityList<WFFlow>(commandText, dbParameters);
        }
        #endregion

        #region - 查詢起始功能url -
        public class WFStartFunUrlPara
        {
            public enum ParaField
            {
                SYS_ID,
                WF_FLOW_ID,
                WF_FLOW_VER,
                WF_NODE_ID
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBVarChar WFFlowVer;
            public DBVarChar WFNodeID;
        }

        public class WFStartFunUrl : DBTableRow
        {
            public DBVarChar SubSysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;

            public DBVarChar StartFunUrl;
        }

        /// <summary>
        /// 查詢起始功能url
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public WFStartFunUrl SelectWFStartFunUrl(WFStartFunUrlPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT F.SUB_SYS_ID AS SubSysID ",
                        "     , S.FUN_CONTROLLER_ID AS FunControllerID ",
                        "     , S.FUN_ACTION_NAME AS FunActionName ",
                        "  FROM SYS_SYSTEM_WF_NODE S ",
                        "  JOIN SYS_SYSTEM_WF_NEXT N ",
                        "    ON S.SYS_ID = N.SYS_ID ",
                        "   AND S.WF_FLOW_ID = N.WF_FLOW_ID ",
                        "   AND S.WF_FLOW_VER = N.WF_FLOW_VER ",
                        "   AND S.WF_NODE_ID = N.NEXT_WF_NODE_ID ",
                        "  JOIN SYS_SYSTEM_FUN F ",
                        "    ON S.FUN_SYS_ID = F.SYS_ID ",
                        "   AND S.FUN_CONTROLLER_ID = F.FUN_CONTROLLER_ID ",
                        "   AND S.FUN_ACTION_NAME = F.FUN_ACTION_NAME ",
                        " WHERE N.SYS_ID = {SYS_ID} ",
                        "   AND N.WF_FLOW_ID = {WF_FLOW_ID} ",
                        "   AND N.WF_FLOW_VER = {WF_FLOW_VER} ",
                        "   AND N.WF_NODE_ID = {WF_NODE_ID} "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = WFStartFunUrlPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = WFStartFunUrlPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = WFStartFunUrlPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = WFStartFunUrlPara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            return GetEntityList<WFStartFunUrl>(commandText, dbParameters).SingleOrDefault();
        }
        #endregion
    }
}
