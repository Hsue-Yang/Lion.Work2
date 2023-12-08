using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.WorkFlowService
{
    public class EntityNextToNode : EntityWorkFlowService
    {
        public EntityNextToNode(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public enum EnumSelectNextNodeResult
        {
            Success,
            NotProcessNode,
            WFNodeNotUserRole,
            WFNodeNotPassAudit,
            WFNodeDocIsReq,
            Failure
        }

        public class NextNodeExecuteResult : ExecuteResult
        {
            public DBChar WFNo;
            public DBChar NodeNo;

            public DBVarChar SysID;
            public DBVarChar FlowID;
            public DBNVarChar FlowNM;
            public DBChar FlowVer;
            public DBNVarChar Subject;

            public DBVarChar NodeID;
            public DBVarChar NewUserID;

            public DBVarChar NextNodeID;
            public DBNVarChar NextNodeNM;
            public DBVarChar NextNodeType;

            public DBVarChar NextFunSysID;
            public DBVarChar NextSubSysID;
            public DBVarChar NextFunControllerID;
            public DBVarChar NextFunActionName;

            public DBChar IsAutoAssignNewUser;
        }

        public NextNodeExecuteResult SelectNextNode(WorkFlowPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT VARCHAR(50) = '" + EnumSelectNextNodeResult.Success + "'; ",
                        "DECLARE @ERROR_LINE INT;",
                        "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",

                        "DECLARE @ISAUTO_ASSIGN_NEWUSER CHAR(1) = 'N'; ",

                        "DECLARE @NODE_NO CHAR(3) = NULL; ",
                        "DECLARE @SYS_ID VARCHAR(12) = NULL; ",
                        "DECLARE @WF_FLOW_ID VARCHAR(50) = NULL; ",
                        "DECLARE @WF_FLOW_NM NVARCHAR(150) = NULL; ",
                        "DECLARE @WF_FLOW_VER VARCHAR(50) = NULL; ",
                        "DECLARE @WF_SUBJECT VARCHAR(150) = NULL; ",
                        "DECLARE @WF_NODE_ID VARCHAR(50) = NULL; ",
                        "DECLARE @NEW_USER_ID VARCHAR(20) = NULL; ",
                        
                        "IF NOT EXISTS (SELECT WF_NO ",
                        "                 FROM WF_FLOW ",
                        "                WHERE WF_NO = {WF_NO} ",
                        "                  AND RESULT_ID = '" + EnumWFNodeResultID.P + "') ",
                        "BEGIN ",
                        "    SET @RESULT = '" + EnumSelectNextNodeResult.NotProcessNode + "'; ",
                        "END; ",
                        "ELSE ",
                        "BEGIN ",
                        "    SELECT @NODE_NO = D.NODE_NO ",
                        "         , @SYS_ID = D.SYS_ID ",
                        "         , @WF_FLOW_ID = D.WF_FLOW_ID ",
                        "         , @WF_FLOW_NM = D.WF_FLOW_NM",
                        "         , @WF_FLOW_VER = D.WF_FLOW_VER ",
                        "         , @WF_SUBJECT = D.WF_SUBJECT",
                        "         , @WF_NODE_ID = D.WF_NODE_ID ",
                        "         , @NEW_USER_ID = D.NEW_USER_ID ",
                        "      FROM dbo.FNTB_GET_WF_NODE({WF_NO}) D ",

                        #region - 更新建立節點使用者 -
                        // 建立節點使用者 IS NULL
                        // 使用者在[候選建立節點使用者]清單內
                        "    IF @NEW_USER_ID IS NULL ",
                        "   AND EXISTS ( ",
                        "               SELECT * ",
                        "                 FROM WF_NODE_NEW_USER ",
                        "                WHERE WF_NO = {WF_NO} ",
                        "                  AND NODE_NO = @NODE_NO ",
                        "                  AND NEW_USER_ID = {USER_ID}",
                        "               ) ",
                        "    BEGIN ",
                        "        BEGIN TRANSACTION ",
                        "            BEGIN TRY ",

                        "                UPDATE WF_NODE ",
                        "                   SET NEW_USER_ID = {USER_ID} ",
                        "                 WHERE WF_NO = {WF_NO} ",
                        "                   AND NODE_NO = @NODE_NO; ",

                        "                SET @NEW_USER_ID = {USER_ID}; ",
                        
                        "                SET @ISAUTO_ASSIGN_NEWUSER = 'Y'; ",
                        "                SET @RESULT = '" + EnumSelectNextNodeResult.Success + "'; ",
                        "                COMMIT; ",
                        "            END TRY ",
                        "        BEGIN CATCH ",
                        "            SET @RESULT = '" + EnumSelectNextNodeResult.Failure + "';",
                        "            SET @ERROR_LINE = ERROR_LINE();",
                        "            SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                        "            ROLLBACK TRANSACTION; ",
                        "        END CATCH; ",
                        "    END; ",
                        #endregion
                        
                        #region - 使用者無權限 -
                        // 使用者角色不在節點系統角色裡
                        // 最新簽核者<>使用者
                        "    IF @NEW_USER_ID IS NULL OR",
                        "       ((@NEW_USER_ID <> {USER_ID} AND ",
                        "         dbo.FN_GET_WF_NODE_HAS_USER_ROLE(@SYS_ID, @WF_FLOW_ID, @WF_FLOW_VER, @WF_NODE_ID, {USER_ID}) = 'N')",
                        "         AND ",
                        "        (@NEW_USER_ID <> {USER_ID} AND ",
                        "         NOT EXISTS ( SELECT * ",
                        "                        FROM WF_SIG ",
                        "                       WHERE WF_NO = {WF_NO} ",
                        "                         AND SIG_USER_ID = {USER_ID} ",
                        "                         AND SIG_STEP = (SELECT MAX(SIG_STEP) ",
                        "                                           FROM WF_SIG ",
                        "                                          WHERE WF_NO = {WF_NO} ",
                        "                                            AND NODE_NO = @NODE_NO)",
                        "                    )",
                        "         )",
                        "       ) ",
                        "    BEGIN ",
                        "        SET @NODE_NO = NULL; ",
                        "        SET @RESULT = '" + EnumSelectNextNodeResult.WFNodeNotUserRole + "'; ",
                        "    END; ",
                        #endregion


                        "    IF @NODE_NO IS NOT NULL ",
                        "    BEGIN ",

                        #region - 現況工作流程節點，是否為簽核節點 -
                        "        IF EXISTS (SELECT * ",
                        "                     FROM WF_NODE WN ",
                        "                     JOIN SYS_SYSTEM_WF_SIG SS ",
                        "                       ON WN.SYS_ID = SS.SYS_ID ",
                        "                      AND WN.WF_FLOW_ID = SS.WF_FLOW_ID ",
                        "                      AND WN.WF_FLOW_VER = SS.WF_FLOW_VER ",
                        "                      AND WN.WF_NODE_ID = SS.WF_NODE_ID ",
                        "                    WHERE WN.WF_NO = {WF_NO} ",
                        "                      AND WN.NODE_NO = @NODE_NO) ",
                        "        BEGIN ",
                        // 簽核節點，是否完全審核通過
                        "            SELECT @RESULT = (CASE WHEN SIG_RESULT_ID = '" + EnumWFNodeSignatureResultID.A + "' ",
                        "                                   THEN '" + EnumSelectNextNodeResult.Success + "' ",
                        "                                   ELSE '" + EnumSelectNextNodeResult.WFNodeNotPassAudit + "' END) ",
                        "              FROM WF_NODE ",
                        "             WHERE WF_NO = {WF_NO} ",
                        "               AND NODE_NO = @NODE_NO; ",
                        "        END; ",
                        #endregion
                        
                        "        IF @RESULT = '" + EnumSelectNextNodeResult.Success + "' ",
                        "        BEGIN ",
                        "            IF EXISTS (SELECT * ",
                        "                         FROM SYS_SYSTEM_WF_DOC D ",
                        "                         LEFT JOIN (",
                        "                                  SELECT * ",
                        "                                    FROM WF_DOC ",
                        "                                   WHERE WF_NO = {WF_NO} ",
                        "                                     AND IS_DELETE = 'N'",
                        "                              ) W ",
                        "                           ON D.SYS_ID = W.SYS_ID ",
                        "                          AND D.WF_FLOW_ID = W.WF_FLOW_ID ",
                        "                          AND D.WF_FLOW_VER = W.WF_FLOW_VER ",
                        "                          AND D.WF_NODE_ID = W.WF_NODE_ID ",
                        "                          AND D.WF_DOC_SEQ = W.WF_DOC_SEQ ",
                        "                        WHERE D.SYS_ID = @SYS_ID ",
                        "                          AND D.WF_FLOW_ID = @WF_FLOW_ID ",
                        "                          AND D.WF_FLOW_VER = @WF_FLOW_VER ",
                        "                          AND D.WF_NODE_ID = @WF_NODE_ID ",
                        "                          AND D.IS_REQ = 'Y' ",
                        "                          AND W.DOC_NO IS NULL) ",
                        "            BEGIN ",
                        "                SET @RESULT = '" + EnumSelectNextNodeResult.WFNodeDocIsReq + "' ",
                        "            END; ",
                        "        END; ",
                        "    END; ",
                        "END; ",

                        "IF @RESULT = '" + EnumSelectNextNodeResult.Success + "' ",
                        "BEGIN ",
                        "    IF EXISTS (SELECT SYS_ID ",
                        "                    , WF_FLOW_ID ",
                        "                    , WF_FLOW_VER ",
                        "                    , WF_NODE_ID ",
                        "                 FROM SYS_SYSTEM_WF_NODE ",
                        "                WHERE SYS_ID = @SYS_ID ",
                        "                  AND WF_FLOW_ID = @WF_FLOW_ID ",
                        "                  AND WF_FLOW_VER = @WF_FLOW_VER ",
                        "                  AND WF_NODE_ID = @WF_NODE_ID ",
                        "                  AND IS_FINALLY = 'Y') ",
                        "    BEGIN ",

                        #region - 此節點為結束節點 -
                        "        SELECT {WF_NO} AS WFNo ",
                        "             , @NODE_NO AS NodeNo ",
                        "             , @SYS_ID AS SysID ",
                        "             , @WF_FLOW_ID AS FlowID ",
                        "             , @WF_FLOW_VER AS FlowVer ",
                        "             , @WF_SUBJECT AS Subject",
                        "             , NULL AS NodeID ",
                        "             , NULL AS NextNodeID ",
                        "             , '" + EnumWFNodeTypeID.E + "' AS NextNodeType ",
                        "             , NULL AS NextFunSysID ",
                        "             , NULL AS NextSubSysID ",
                        "             , NULL AS NextFunControllerID ",
                        "             , NULL AS NextFunActionName ",
                        "             , @NEW_USER_ID AS NewUserID ",
                        "             , @ISAUTO_ASSIGN_NEWUSER AS IsAutoAssignNewUser ",
                        "             , @RESULT AS Result ",
                        #endregion
                        "    END; ",
                        "    ELSE ",
                        "    BEGIN ",

                        #region - 取得下一節點明細 -
                        "        SELECT {WF_NO} AS WFNo ",
                        "             , @NODE_NO AS NodeNo ",
                        "             , @SYS_ID AS SysID ",
                        "             , @WF_FLOW_ID AS FlowID ",
                        "             , @WF_FLOW_NM AS FlowNM ",
                        "             , @WF_FLOW_VER AS FlowVer ",
                        "             , @WF_SUBJECT AS Subject",
                        "             , @WF_NODE_ID AS NodeID ",
                        "             , X.NEXT_WF_NODE_ID AS NextNodeID ",
                        "             , N.WF_NODE_ZH_TW AS NextNodeNM",
                        "             , N.NODE_TYPE AS NextNodeType ",
                        "             , N.FUN_SYS_ID AS NextFunSysID ",
                        "             , (CASE WHEN N.NODE_TYPE = '" + EnumWFNodeTypeID.P + "' ",
                        "                     THEN F.SUB_SYS_ID  ",
                        "                     ELSE NULL ",
                        "                END) AS NextSubSysID ",
                        "             , N.FUN_CONTROLLER_ID AS NextFunControllerID ",
                        "             , N.FUN_ACTION_NAME AS NextFunActionName ",
                        "             , @NEW_USER_ID AS NewUserID ",
                        "             , @ISAUTO_ASSIGN_NEWUSER AS IsAutoAssignNewUser ",
                        "             , @RESULT AS Result ",
                        "          FROM SYS_SYSTEM_WF_NEXT X ",
                        "          JOIN SYS_SYSTEM_WF_NODE N ",
                        "            ON X.SYS_ID = N.SYS_ID ",
                        "           AND X.WF_FLOW_ID = N.WF_FLOW_ID ",
                        "           AND X.WF_FLOW_VER = N.WF_FLOW_VER ",
                        "           AND X.NEXT_WF_NODE_ID = N.WF_NODE_ID ",
                        "          LEFT JOIN SYS_SYSTEM_FUN F ",
                        "            ON N.FUN_SYS_ID = F.SYS_ID ",
                        "           AND N.FUN_CONTROLLER_ID = F.FUN_CONTROLLER_ID ",
                        "           AND N.FUN_ACTION_NAME = F.FUN_ACTION_NAME ",
                        "         WHERE X.SYS_ID = @SYS_ID ",
                        "           AND X.WF_FLOW_ID = @WF_FLOW_ID ",
                        "           AND X.WF_FLOW_VER = @WF_FLOW_VER ",
                        "           AND X.WF_NODE_ID = @WF_NODE_ID; ",
                        #endregion
                        "    END; ",
                        "END ",
                        "ELSE ",
                        "BEGIN ",
                        "    SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage; ",
                        "END "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<NextNodeExecuteResult>(commandText, dbParameters).SingleOrDefault();
            
            var enumResult = ((EnumSelectNextNodeResult)Enum.Parse(typeof(EnumSelectNextNodeResult), result.Result.GetValue()));

            if (enumResult == EnumSelectNextNodeResult.Failure)
            {
                throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
            }

            return result;
        }

        public class NextToProcessNodeExecuteResult : ExecuteResult
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

            public DBVarChar ResultID;
        }

        public NextToProcessNodeExecuteResult NextToProcessNode(WorkFlowPara para, List<NodeNewUserPara> nodeNewUserParaList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            foreach (NodeNewUserPara nodeNewUserPara in nodeNewUserParaList)
            {
                dbParameters.Add(new DBParameter { Name = NodeNewUserPara.ParaField.NEW_USER_ID.ToString(), Value = nodeNewUserPara.NewUserID });
                commandTextStringBuilder.Append(
                    GetCommandText(
                        ProviderName,
                        string.Join(Environment.NewLine,
                            new object[]
                            {
                                "INSERT INTO @USER_LIST VALUES ({NEW_USER_ID});"
                            }),
                        dbParameters));

                dbParameters.Clear();
            }

            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @USER_LIST AS dbo.USER_TYPE;",
                        commandTextStringBuilder.ToString(),
                        "EXECUTE dbo.SP_WF_NEXT_TO_PROCESS_NODE {WF_NO}, {USER_ID}, {NEW_USER_ID}, {UPD_USER_ID}, @USER_LIST;"
                    });

            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.NEW_USER_ID, Value = para.NewUserID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<NextToProcessNodeExecuteResult>(commandText, dbParameters).SingleOrDefault();

            var enumResult = ((EnumYN)Enum.Parse(typeof(EnumYN), result.Result.GetValue()));

            if (enumResult == EnumYN.N)
            {
                throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
            }

            return result;
        }
        
        public enum EnumDecisionToProcessNodeResult
        {
            Success,
            NotNextNode,
            Failure
        }

        public class DecisionToProcessNodeExecuteResult : ExecuteResult
        {
            public DBChar WFNo;
            public DBChar NodeNo;

            public DBVarChar SysID;
            public DBVarChar FlowID;
            public DBChar FlowVer;

            public DBVarChar NodeID;
            public DBNVarChar NodeNM;
            public DBVarChar NodeType;

            public DBVarChar FunSysID;
            public DBVarChar SubSysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;

            public DBChar DecisionNodeNo;

            public DBVarChar ResultID;
        }

        public DecisionToProcessNodeExecuteResult DecisionToProcessNode(WorkFlowPara para, List<NodeNewUserPara> nodeNewUserParaList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            foreach (NodeNewUserPara nodeNewUserPara in nodeNewUserParaList)
            {
                dbParameters.Add(new DBParameter { Name = NodeNewUserPara.ParaField.NEW_USER_ID.ToString(), Value = nodeNewUserPara.NewUserID });
                commandTextStringBuilder.Append(
                    GetCommandText(
                        ProviderName,
                        string.Join(Environment.NewLine,
                            new object[]
                            {
                                "INSERT INTO @USER_LIST VALUES ({NEW_USER_ID});"
                            }),
                        dbParameters));

                dbParameters.Clear();
            }

            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @USER_LIST AS dbo.USER_TYPE;",
                        commandTextStringBuilder.ToString(),
                        "EXECUTE dbo.SP_WF_DECISION_TO_PROCESS_NODE {WF_NO}, {USER_ID}, {NEW_USER_ID}, {NEXT_RESULT_VALUE}, {UPD_USER_ID}, @USER_LIST;"
                    });

            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.NEW_USER_ID, Value = para.NewUserID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.NEXT_RESULT_VALUE, Value = para.NextResultValue });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<DecisionToProcessNodeExecuteResult>(commandText, dbParameters).SingleOrDefault();

            var enumResult = ((EnumDecisionToProcessNodeResult)Enum.Parse(typeof(EnumDecisionToProcessNodeResult), result.Result.GetValue()));

            if (enumResult == EnumDecisionToProcessNodeResult.Failure)
            {
                throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
            }

            return result;
        }

        public class NextToEndNodeExecuteResult : ExecuteResult
        {
            public DBChar WFNo;
            public DBChar NodeNo;

            public DBVarChar SysID;
            public DBVarChar FlowID;
            public DBChar FlowVer;
            public DBNVarChar Subject;
            public DBVarChar ApplyUser;

            public DBVarChar NodeID;
            public DBVarChar NextNodeID;
            public DBVarChar NodeType;

            public DBVarChar FunSysID;
            public DBVarChar SubSysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;

            public DBChar DTEnd;

            public DBVarChar ResultID;
        }

        public NextToEndNodeExecuteResult NextToEndNode(WorkFlowPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT CHAR(1) = 'N'; ",
                        "DECLARE @ERROR_LINE INT;",
                        "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                        
                        "DECLARE @SYS_ID VARCHAR(12) = NULL; ",
                        "DECLARE @WF_FLOW_ID VARCHAR(50) = NULL; ",
                        "DECLARE @WF_FLOW_VER VARCHAR(50) = NULL; ",
                        "DECLARE @WF_SUBJECT NVARCHAR(150) = NULL; ",
                        "DECLARE @WF_APPLY_USER VARCHAR(20) = NULL; ",
                        "DECLARE @WF_NODE_ID VARCHAR(50) = NULL; ",
                        "DECLARE @REMARK_NO CHAR(3); ",

                        "DECLARE @NOW_DATETIME CHAR(17) = dbo.FN_GET_SYSDATE(NULL) + dbo.FN_GET_SYSTIME(NULL); ",
                        
                        "SELECT @REMARK_NO = MAX(REMARK_NO) ",
                        "  FROM WF_REMARK ",
                        " WHERE WF_NO = {WF_NO};",

                        "SELECT @SYS_ID = D.SYS_ID ",
                        "     , @WF_FLOW_ID = D.WF_FLOW_ID ",
                        "     , @WF_FLOW_VER = D.WF_FLOW_VER ",
                        "     , @WF_NODE_ID = D.WF_NODE_ID ",
                        "     , @WF_SUBJECT = D.WF_SUBJECT ",
                        "     , @WF_APPLY_USER = D.USER_ID ",
                        "  FROM dbo.FNTB_GET_WF_NODE({WF_NO}) D ",

                        "BEGIN TRANSACTION ",
                        "    BEGIN TRY ",
                        "        UPDATE WF_NODE ",
                        "           SET END_USER_ID = {USER_ID} ",
                        "             , DT_END = @NOW_DATETIME ",
                        "             , RESULT_ID = '" + EnumWFNodeResultID.F + "' ",
                        "             , UPD_USER_ID = {UPD_USER_ID} ",
                        "             , UPD_DT = GETDATE() ",
                        "         WHERE WF_NO = {WF_NO} ",
                        "           AND NODE_NO = {NODE_NO}; ",

                        "        UPDATE WF_FLOW ",
                        "           SET END_USER_ID = {USER_ID}",
                        "             , DT_END = @NOW_DATETIME",
                        "             , RESULT_ID = '" + EnumWFNodeResultID.F + "'",
                        "             , NODE_NO = NULL ",
                        "             , UPD_USER_ID = {UPD_USER_ID}",
                        "             , UPD_DT = GETDATE() ",
                        "         WHERE WF_NO = {WF_NO}; ",
                        
                        #region - 新增備註 -
                        "        SET @REMARK_NO = RIGHT('00' + CAST(ISNULL(CAST(@REMARK_NO AS INT), 0) + 1 AS VARCHAR), 3) ",
                        "        INSERT INTO dbo.WF_REMARK (",
                        "               WF_NO, NODE_NO, REMARK_NO, SYS_ID, WF_FLOW_ID, WF_FLOW_VER, WF_NODE_ID, NODE_RESULT_ID, NODE_NEW_USER_ID, BACK_WF_NODE_ID",
                        "             , SIG_STEP, WF_SIG_SEQ, SIG_DATE, SIG_RESULT_ID",
                        "             , DOC_NO, WF_DOC_SEQ, DOC_DATE, DOC_IS_DELETE",
                        "             , REMARK_USER_ID, REMARK_DATE, REMARK",
                        "             , UPD_USER_ID, UPD_DT",
                        "        ) VALUES (",
                        "               {WF_NO}, {NODE_NO}, @REMARK_NO, @SYS_ID, @WF_FLOW_ID, @WF_FLOW_VER, @WF_NODE_ID, '" + EnumWFNodeResultID.F + "', NULL, NULL",
                        "             , NULL, NULL, NULL, NULL",
                        "             , NULL, NULL, NULL, NULL",
                        "             , {USER_ID}, @NOW_DATETIME, NULL",
                        "             , {UPD_USER_ID}, GETDATE()",
                        "        )",
                        #endregion

                        "        SET @RESULT = 'Y'; ",
                        "        COMMIT; ",
                        "    END TRY ",
                        "    BEGIN CATCH ",
                        "        SET @RESULT = 'N';",
                        "        SET @ERROR_LINE = ERROR_LINE();",
                        "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                        "        ROLLBACK TRANSACTION; ",
                        "    END CATCH; ",

                        "IF @RESULT='Y' ",
                        "BEGIN ",
                        "    SELECT {WF_NO} AS WFNo",
                        "         , NULL AS NodeNo",
                        "         , SYS_ID AS SysID",
                        "         , WF_FLOW_ID AS FlowID ",
                        "         , WF_FLOW_VER AS FlowVer",
                        "         , @WF_SUBJECT AS Subject",
                        "         , @WF_APPLY_USER AS ApplyUser",
                        "         , NULL AS NodeID",
                        "         , '" + EnumWFNodeTypeID.E + "' AS NodeType ",
                        "         , NULL AS FunSysID ",
                        "         , NULL AS SubSysID ",
                        "         , NULL AS FunControllerID ",
                        "         , NULL AS FunActionName ",
                        "         , @NOW_DATETIME AS DTEnd ",
                        "         , @RESULT AS Result ",
                        "      FROM WF_NODE ",
                        "     WHERE WF_NO = {WF_NO} ",
                        "       AND NODE_NO = {NODE_NO};  ",
                        "END; ",
                        "ELSE ",
                        "BEGIN ",
                        "    SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage; ",
                        "END; "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.NODE_NO, Value = para.NodeNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<NextToEndNodeExecuteResult>(commandText, dbParameters).SingleOrDefault();

            var enumResult = ((EnumYN)Enum.Parse(typeof(EnumYN), result.Result.GetValue()));

            if (enumResult == EnumYN.N)
            {
                throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
            }

            return result;
        }

        public EnumYN IsNextNodeEnd(WorkFlowPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT CHAR(1) = 'N';",
                        "IF EXISTS (SELECT * ",
                        "             FROM WF_NODE WN ",
                        "             JOIN SYS_SYSTEM_WF_NEXT N1 ",
                        "               ON WN.SYS_ID = N1.SYS_ID ",
                        "              AND WN.WF_FLOW_ID = N1.WF_FLOW_ID ",
                        "              AND WN.WF_FLOW_VER = N1.WF_FLOW_VER ",
                        "              AND WN.WF_NODE_ID = N1.WF_NODE_ID ",
                        "             JOIN SYS_SYSTEM_WF_NEXT N2 ",
                        "               ON N1.SYS_ID = N2.SYS_ID ",
                        "              AND N1.WF_FLOW_ID = N2.WF_FLOW_ID ",
                        "              AND N1.WF_FLOW_VER = N2.WF_FLOW_VER ",
                        "              AND N1.NEXT_WF_NODE_ID = N2.WF_NODE_ID ",
                        "            WHERE N2.NEXT_WF_NODE_ID = 'END' ",
                        "              AND WN.WF_NO = {WF_NO} ",
                        "              AND WN.NODE_NO = {NODE_NO} ",
                        "              AND N2.NEXT_RESULT_VALUE = {NEXT_RESULT_VALUE}) ",
                        "BEGIN ",
                        "    SET @RESULT='Y'; ",
                        "END;",
                        "SELECT @RESULT; "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.NODE_NO, Value = para.NodeNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.NEXT_RESULT_VALUE, Value = para.NextResultValue });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));

            return result.GetValue() == EnumYN.Y.ToString() ? EnumYN.Y : EnumYN.N;
        }

        public class NodeNewUserPara
        {
            public enum ParaField
            {
                NEW_USER_ID
            }
            
            public DBVarChar NewUserID;
        }

        public class AssignNextNode : DBTableRow
        {
            public DBVarChar AssgAPISysID;
            public DBVarChar AssgAPIControllerID;
            public DBVarChar AssgAPIActionName;
        }

        /// <summary>
        /// 查詢是否指定下一節點處理人
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public AssignNextNode SelectProcessNextNodeAssignUserAPI(WorkFlowPara para)
        {
            string commandText = string.Concat(new object[]
            {
                " SELECT SN.ASSG_API_SYS_ID AS AssgAPISysID ",
                "      , SN.ASSG_API_CONTROLLER_ID AS AssgAPIControllerID ",
                "      , SN.ASSG_API_ACTION_NAME AS AssgAPIActionName ",
                "   FROM WF_NODE WN ",
                "   JOIN SYS_SYSTEM_WF_NODE SN ",
                "     ON WN.SYS_ID = SN.SYS_ID ",
                "    AND WN.WF_FLOW_ID = SN.WF_FLOW_ID ",
                "    AND WN.WF_FLOW_VER = SN.WF_FLOW_VER ",
                "    AND WN.WF_NODE_ID = SN.WF_NODE_ID ",
                "  WHERE WN.WF_NO = {WF_NO} ",
                "    AND WN.NODE_NO = {NODE_NO} ",
                "    AND SN.IS_ASSG_NEXT_NODE = 'Y' "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.NODE_NO, Value = para.NodeNo });
            return GetEntityList<AssignNextNode>(commandText, dbParameters).SingleOrDefault();
        }
        
        public AssignNextNode SelectDecisionNextNodeAssignUserAPI(WorkFlowPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        " SELECT SN2.ASSG_API_SYS_ID AS AssgAPISysID ",
                        "      , SN2.ASSG_API_CONTROLLER_ID AS AssgAPIControllerID ",
                        "      , SN2.ASSG_API_ACTION_NAME AS AssgAPIActionName ",
                        "   FROM WF_NODE WN ",
                        "   JOIN SYS_SYSTEM_WF_NEXT SN1 ",
                        "     ON WN.SYS_ID = SN1.SYS_ID ",
                        "    AND WN.WF_FLOW_ID = SN1.WF_FLOW_ID ",
                        "    AND WN.WF_FLOW_VER = SN1.WF_FLOW_VER ",
                        "    AND WN.WF_NODE_ID = SN1.WF_NODE_ID ",
                        "   JOIN SYS_SYSTEM_WF_NODE SN2 ",
                        "     ON SN1.SYS_ID = SN2.SYS_ID ",
                        "    AND SN1.WF_FLOW_ID = SN2.WF_FLOW_ID ",
                        "    AND SN1.WF_FLOW_VER = SN2.WF_FLOW_VER ",
                        "    AND SN1.NEXT_WF_NODE_ID = SN2.WF_NODE_ID ",
                        "  WHERE WF_NO = {WF_NO} ",
                        "    AND NODE_NO = {NODE_NO} ",
                        "    AND IS_ASSG_NEXT_NODE = 'Y' "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.NODE_NO, Value = para.NodeNo });
            return GetEntityList<AssignNextNode>(commandText, dbParameters).SingleOrDefault();
        }

        public class AssignNextNodeNewUser : DBTableRow
        {
            public DBChar WFNo;
            public DBChar NodeNo;
            public DBVarChar SysID;
            public DBVarChar FlowID;
            public DBChar FlowVer;
            public DBVarChar NodeID;
        }

        public class DecisionToNextNodeNewUser : DBTableRow
        {
            public DBVarChar NewUserID;
        }

        public DecisionToNextNodeNewUser GetDecisionToNextNodeNewUser(WorkFlowPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        " IF NOT EXISTS (SELECT * ",
                        "                  FROM WF_NODE WN ",
                        "                  JOIN SYS_SYSTEM_WF_NEXT SN1 ",
                        "                    ON WN.SYS_ID = SN1.SYS_ID ",
                        "                   AND WN.WF_FLOW_ID = SN1.WF_FLOW_ID ",
                        "                   AND WN.WF_FLOW_VER = SN1.WF_FLOW_VER ",
                        "                   AND WN.WF_NODE_ID = SN1.WF_NODE_ID ",
                        "                  JOIN SYS_SYSTEM_WF_NODE SN2 ",
                        "                    ON SN1.SYS_ID = SN2.SYS_ID ",
                        "                   AND SN1.WF_FLOW_ID = SN2.WF_FLOW_ID ",
                        "                   AND SN1.WF_FLOW_VER = SN2.WF_FLOW_VER ",
                        "                   AND SN1.NEXT_WF_NODE_ID = SN2.WF_NODE_ID ",
                        "                  JOIN SYS_SYSTEM_WF_NEXT SN3 ",
                        "                    ON SN2.SYS_ID = SN3.SYS_ID ",
                        "                   AND SN2.WF_FLOW_ID = SN3.WF_FLOW_ID ",
                        "                   AND SN2.WF_FLOW_VER = SN3.WF_FLOW_VER ",
                        "                   AND SN2.WF_NODE_ID = SN3.WF_NODE_ID ",
                        "                  JOIN SYS_SYSTEM_ROLE_NODE R ",
                        "                    ON SN3.SYS_ID = R.SYS_ID ",
                        "                   AND SN3.WF_FLOW_ID = R.WF_FLOW_ID ",
                        "                   AND SN3.WF_FLOW_VER = R.WF_FLOW_VER ",
                        "                   AND SN3.NEXT_WF_NODE_ID = R.WF_NODE_ID ",
                        "                 WHERE WN.WF_NO = {WF_NO} ",
                        "                   AND WN.NODE_NO = {NODE_NO} ",
                        "                   AND SN3.NEXT_RESULT_VALUE = {NEXT_RESULT_VALUE}) ",
                        " BEGIN ",
                        "     SELECT NEW_USER_ID AS NewUserID",
                        "     FROM WF_FLOW ",
                        "     WHERE WF_NO = {WF_NO} ",
                        " END; "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.WF_NO, Value = para.WFNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.NODE_NO, Value = para.NodeNo });
            dbParameters.Add(new DBParameter { Name = WorkFlowPara.ParaField.NEXT_RESULT_VALUE, Value = para.NextResultValue });
            return GetEntityList<DecisionToNextNodeNewUser>(commandText, dbParameters).SingleOrDefault();
        }
    }
}