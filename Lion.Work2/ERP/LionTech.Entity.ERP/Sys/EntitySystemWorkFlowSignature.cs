using System;
using System.Collections.Generic;
using System.Linq;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemWorkFlowSignature : EntitySys
    {
        public EntitySystemWorkFlowSignature(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemWFSigPara : DBCulture
        {
            public SystemWFSigPara(string culture)
                : base(culture)
            {
            }

            public enum ParaField
            {
                SYS_ID,
                WF_FLOW_ID,
                WF_FLOW_VER,
                WF_NODE_ID,
                WF_SIG,
                SYS_NM,
                API_GROUP,
                API_NM,
                CODE_NM
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
        }

        public class SystemWFSig : DBTableRow
        {
            public DBInt SigStep;
            public DBChar WFSigSeq;
            public DBNVarChar WFSigNM;

            public DBNVarChar SigTypeNM;

            public DBNVarChar APISysNM;
            public DBNVarChar APIControllerNM;
            public DBNVarChar APIActionNameNM;

            public DBVarChar UpdUserNM;
            public DBDateTime UpdDt;
        }

        public List<SystemWFSig> SelectSystemWFSigList(SystemWFSigPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT S.SIG_STEP AS SigStep ",
                        "     , S.WF_SIG_SEQ AS WFSigSeq ",
                        "     , dbo.FN_GET_NMID(S.WF_SIG_SEQ, S.{WF_SIG}) AS WFSigNM  ",
                        "     , dbo.FN_GET_NMID(S.SIG_TYPE, C.{CODE_NM}) AS SigTypeNM ",
                        "     , (CASE WHEN S.API_SYS_ID IS NOT NULL THEN dbo.FN_GET_NMID(S.API_SYS_ID, M.{SYS_NM}) ELSE NULL END) AS APISysNM ",
                        "     , (CASE WHEN S.API_CONTROLLER_ID IS NOT NULL THEN (dbo.FN_GET_NMID(S.API_CONTROLLER_ID, G.{API_GROUP})) ",
                        "             ELSE NULL ",
                        "        END) AS APIControllerNM ",
                        "     , (CASE WHEN S.API_ACTION_NAME IS NOT NULL THEN (dbo.FN_GET_NMID(S.API_ACTION_NAME, U.{API_NM})) ",
                        "             ELSE NULL ",
                        "        END) AS APIActionNameNM ",
                        "     , dbo.FN_GET_USER_NM(S.UPD_USER_ID) AS UpdUserNM",
                        "     , S.UPD_DT AS UpdDt ",
                        "  FROM SYS_SYSTEM_WF_SIG S ",
                        "  LEFT JOIN SYS_SYSTEM_API U ",
                        "    ON S.API_SYS_ID = U.SYS_ID AND S.API_CONTROLLER_ID = U.API_CONTROLLER_ID AND S.API_ACTION_NAME = U.API_ACTION_NAME ",
                        "  LEFT JOIN SYS_SYSTEM_MAIN M ",
                        "    ON S.API_SYS_ID = M.SYS_ID ",
                        "  LEFT JOIN SYS_SYSTEM_API_GROUP G ",
                        "    ON U.SYS_ID = G.SYS_ID AND U.API_CONTROLLER_ID = G.API_CONTROLLER_ID ",
                        "  LEFT JOIN CM_CODE C ",
                        "    ON C.CODE_KIND = '0030' AND S.SIG_TYPE = C.CODE_ID ",
                        " WHERE S.SYS_ID = {SYS_ID} AND S.WF_FLOW_ID = {WF_FLOW_ID} AND S.WF_FLOW_VER = {WF_FLOW_VER} ",
                        "   AND S.WF_NODE_ID = {WF_NODE_ID} ",
                        " ORDER BY S.SIG_STEP, S.WF_SIG_SEQ; "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_FLOW_ID.ToString(), Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_FLOW_VER.ToString(), Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_NODE_ID.ToString(), Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_SIG.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFSigPara.ParaField.WF_SIG.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFSigPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.API_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFSigPara.ParaField.API_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.API_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFSigPara.ParaField.API_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFSigPara.ParaField.CODE_NM.ToString())) });

            return GetEntityList<SystemWFSig>(commandText, dbParameters);
        }

        public class SystemWFNodePara
        {
            public enum ParaField
            {
                SYS_ID,
                WF_FLOW_ID,
                WF_FLOW_VER,
                WF_NODE_ID,
                WF_SIG_MEMO_ZH_TW,
                WF_SIG_MEMO_ZH_CN,
                WF_SIG_MEMO_EN_US,
                WF_SIG_MEMO_TH_TH,
                WF_SIG_MEMO_JA_JP,
                SIG_API_SYS_ID,
                SIG_API_CONTROLLER_ID,
                SIG_API_ACTION_NAME,
                CHK_API_SYS_ID,
                CHK_API_CONTROLLER_ID,
                CHK_API_ACTION_NAME,
                IS_SIG_NEXT_NODE,
                IS_SIG_BACK_NODE,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;

            public DBNVarChar WFSigMemoZHTW;
            public DBNVarChar WFSigMemoZHCN;
            public DBNVarChar WFSigMemoENUS;
            public DBNVarChar WFSigMemoTHTH;
            public DBNVarChar WFSigMemoJAJP;

            public DBVarChar SigAPISysID;
            public DBVarChar SigAPIControllerID;
            public DBVarChar SigAPIActionName;

            public DBVarChar ChkAPISysID;
            public DBVarChar ChkAPIControllerID;
            public DBVarChar ChkAPIActionName;

            public DBChar IsSigNextNode;
            public DBChar IsSigBackNode;

            public DBVarChar UpdUserID;
        }

        public SystemWFNodeDetailExecuteResult UpdateSystemWFNode(SystemWFNodePara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT CHAR(1) = 'N';",
                        "DECLARE @ERROR_LINE INT;",
                        "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",

                        "BEGIN TRANSACTION ",
                        "    BEGIN TRY ",

                        "        UPDATE SYS_SYSTEM_WF_NODE ",
                        "           SET SIG_API_SYS_ID = {SIG_API_SYS_ID} ",
                        "             , SIG_API_CONTROLLER_ID = {SIG_API_CONTROLLER_ID} ",
                        "             , SIG_API_ACTION_NAME = {SIG_API_ACTION_NAME} ",
                        "             , CHK_API_SYS_ID = {CHK_API_SYS_ID} ",
                        "             , CHK_API_CONTROLLER_ID = {CHK_API_CONTROLLER_ID} ",
                        "             , CHK_API_ACTION_NAME = {CHK_API_ACTION_NAME} ",
                        "             , IS_SIG_NEXT_NODE = {IS_SIG_NEXT_NODE} ",
                        "             , IS_SIG_BACK_NODE = {IS_SIG_BACK_NODE} ",
                        "             , WF_SIG_MEMO_ZH_TW = {WF_SIG_MEMO_ZH_TW} ",
                        "             , WF_SIG_MEMO_ZH_CN = {WF_SIG_MEMO_ZH_CN} ",
                        "             , WF_SIG_MEMO_EN_US = {WF_SIG_MEMO_EN_US} ",
                        "             , WF_SIG_MEMO_TH_TH = {WF_SIG_MEMO_TH_TH} ",
                        "             , WF_SIG_MEMO_JA_JP = {WF_SIG_MEMO_JA_JP} ",
                        "         WHERE SYS_ID = {SYS_ID} ",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "           AND WF_NODE_ID = {WF_NODE_ID};",

                        "        SET @RESULT = 'Y';",
                        "        COMMIT;",
                        "    END TRY",
                        "	 BEGIN CATCH ",
                        "        SET @RESULT = 'N';",
                        "        SET @ERROR_LINE = ERROR_LINE();",
                        "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                        "        ROLLBACK TRANSACTION;",
                        "    END CATCH;",
                        "SELECT SYS_ID AS SysID",
                        "     , WF_FLOW_ID AS WFFlowID",
                        "     , WF_FLOW_VER AS WFFlowVer",
                        "     , WF_NODE_ID AS WFNodeID",
                        "     , WF_NODE_ZH_TW AS WFNodeZHTW",
                        "     , WF_NODE_ZH_CN AS WFNodeZHCN",
                        "     , WF_NODE_EN_US AS WFNodeENUS",
                        "     , WF_NODE_TH_TH AS WFNodeTHTH",
                        "     , WF_NODE_JA_JP AS WFNodeJAJP",
                        "     , NODE_TYPE AS NodeType",
                        "     , NODE_SEQ_X AS NodeSeqX",
                        "     , NODE_SEQ_Y AS NodeSeqY",
                        "     , NODE_POS_BEGIN_X AS NodePosBeginX",
                        "     , NODE_POS_BEGIN_Y AS NodePosBeginY",
                        "     , NODE_POS_END_X AS NodePosEndX",
                        "     , NODE_POS_END_Y AS NodePosEndY",
                        "     , IS_FIRST AS IsFirst",
                        "     , IS_FINALLY AS IsFinally",
                        "     , BACK_WF_NODE_ID AS BackWFNodeID",
                        "     , WF_SIG_MEMO_ZH_TW AS WFSigMemoZHTW",
                        "     , WF_SIG_MEMO_ZH_CN AS WFSigMemoZHCN",
                        "     , WF_SIG_MEMO_EN_US AS WFSigMemoENUS",
                        "     , WF_SIG_MEMO_TH_TH AS WFSigMemoTHTH",
                        "     , WF_SIG_MEMO_JA_JP AS WFSigMemoJAJP",
                        "     , FUN_SYS_ID AS FunSysID",
                        "     , FUN_CONTROLLER_ID AS FunControllerID",
                        "     , FUN_ACTION_NAME AS FunActionName",
                        "     , SIG_API_SYS_ID AS SigApiSysID",
                        "     , SIG_API_CONTROLLER_ID AS SigApiControllerID",
                        "     , SIG_API_ACTION_NAME AS SigApiActionName",
                        "     , CHK_API_SYS_ID AS ChkApiSysID",
                        "     , CHK_API_CONTROLLER_ID AS ChkApiControllerID",
                        "     , CHK_API_ACTION_NAME AS ChkApiActionName",
                        "     , ASSG_API_SYS_ID AS AssgAPISysID",
                        "     , ASSG_API_CONTROLLER_ID AS AssgAPIControllerID",
                        "     , ASSG_API_ACTION_NAME AS AssgAPIActionName",
                        "     , IS_SIG_NEXT_NODE AS IsSigNextNode",
                        "     , IS_SIG_BACK_NODE AS IsSigBackNode",
                        "     , IS_ASSG_NEXT_NODE AS IsAssgNextNode",
                        "     , SORT_ORDER AS SortOrder",
                        "     , REMARK AS Remark",
                        "     , @RESULT AS Result",
                        "     , @ERROR_LINE AS ErrorLine",
                        "     , @ERROR_MESSAGE AS ErrorMessage",
                        "  FROM SYS_SYSTEM_WF_NODE ",
                        " WHERE SYS_ID = {SYS_ID} ",
                        "   AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "   AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "   AND WF_NODE_ID = {WF_NODE_ID}; "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_SIG_MEMO_ZH_TW, Value = para.WFSigMemoZHTW });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_SIG_MEMO_ZH_CN, Value = para.WFSigMemoZHCN });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_SIG_MEMO_EN_US, Value = para.WFSigMemoENUS });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_SIG_MEMO_TH_TH, Value = para.WFSigMemoTHTH });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_SIG_MEMO_JA_JP, Value = para.WFSigMemoJAJP });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.SIG_API_SYS_ID, Value = para.SigAPISysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.SIG_API_CONTROLLER_ID, Value = para.SigAPIControllerID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.SIG_API_ACTION_NAME, Value = para.SigAPIActionName });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.CHK_API_SYS_ID, Value = para.ChkAPISysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.CHK_API_CONTROLLER_ID, Value = para.ChkAPIControllerID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.CHK_API_ACTION_NAME, Value = para.ChkAPIActionName });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.IS_SIG_NEXT_NODE, Value = para.IsSigNextNode });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.IS_SIG_BACK_NODE, Value = para.IsSigBackNode });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<SystemWFNodeDetailExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return result;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
    }
}