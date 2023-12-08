using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemWorkFlowNodeDetail : EntitySys
    {
        public EntitySystemWorkFlowNodeDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemWFFlowPara : DBCulture
        {
            public SystemWFFlowPara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                SYS_ID,
                WF_FLOW_ID,
                WF_FLOW_VER,
                SYS_NM,
                WF_FLOW
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
        }

        public class SystemWFFlow : DBTableRow
        {
            public DBNVarChar SysNM;
            public DBNVarChar WFFlowNM;
        }

        public SystemWFFlow SelectSystemWFFlow(SystemWFFlowPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT dbo.FN_GET_NMID(F.SYS_ID, M.{SYS_NM}) AS SysNM ",
                        "     , dbo.FN_GET_NMID(F.WF_FLOW_ID, F.{WF_FLOW}) AS WFFlowNM ",
                        "  FROM SYS_SYSTEM_WF_FLOW F ",
                        "  JOIN SYS_SYSTEM_MAIN M ",
                        "    ON F.SYS_ID = M.SYS_ID ",
                        " WHERE F.SYS_ID = {SYS_ID} ",
                        "   AND F.WF_FLOW_ID = {WF_FLOW_ID} ",
                        "   AND F.WF_FLOW_VER = {WF_FLOW_VER} "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFFlowPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFFlowPara.ParaField.WF_FLOW_ID.ToString(), Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFFlowPara.ParaField.WF_FLOW_VER.ToString(), Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFFlowPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFFlowPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWFFlowPara.ParaField.WF_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFFlowPara.ParaField.WF_FLOW.ToString())) });
            return GetEntityList<SystemWFFlow>(commandText, dbParameters).SingleOrDefault();
        }

        public class SystemWFNodeTypePara : DBCulture
        {
            public SystemWFNodeTypePara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                CODE_NM
            }
        }

        public class SystemWFNodeType : DBTableRow, ISelectItem
        {
            public DBVarChar CodeID;
            public DBNVarChar CodeNM;

            public string ItemText()
            {
                return CodeNM.GetValue();
            }

            public string ItemValue()
            {
                return CodeID.GetValue();
            }

            public string ItemValue(string key)
            {
                return CodeID.GetValue();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                return CodeID.GetValue();
            }
        }

        public List<SystemWFNodeType> SelectSystemWFNodeTypeList(SystemWFNodeTypePara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT CODE_ID AS CodeID ",
                        "     , dbo.FN_GET_NMID(CODE_ID, {CODE_NM}) AS CodeNM ",
                        "  FROM CM_CODE ",
                        " WHERE CODE_KIND = '0027' ",
                        " ORDER BY SORT_ORDER; "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFNodeTypePara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFNodeTypePara.ParaField.CODE_NM.ToString())) });

            return GetEntityList<SystemWFNodeType>(commandText, dbParameters);
        }

        public class SystemWFNodePara : DBCulture
        {
            public SystemWFNodePara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                SYS_ID,
                WF_FLOW_ID,
                WF_FLOW_VER,
                WF_NODE_ID,
                WF_NODE
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
        }

        public class SystemWFNode : DBTableRow, ISelectItem
        {
            public DBVarChar WFNodeID;
            public DBNVarChar WFNodeNM;

            public string ItemText()
            {
                return WFNodeNM.GetValue();
            }

            public string ItemValue()
            {
                return WFNodeID.GetValue();
            }

            public string ItemValue(string key)
            {
                return WFNodeID.GetValue();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                return WFNodeID.GetValue();
            }
        }

        public List<SystemWFNode> SelectBackSystemWFNodeList(SystemWFNodePara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @SYS_ID VARCHAR(12) = {SYS_ID};",
                        "DECLARE @WF_FLOW_ID VARCHAR(50) = {WF_FLOW_ID};",
                        "DECLARE @WF_FLOW_VER VARCHAR(3) = {WF_FLOW_VER};",
                        "DECLARE @WF_NODE_ID VARCHAR(50) = {WF_NODE_ID};",

                        "IF @WF_NODE_ID IS NULL",
                        "BEGIN",
                        "    SELECT WF_NODE_ID AS WFNodeID ",
                        "         , dbo.FN_GET_NMID(WF_NODE_ID, {WF_NODE}) AS WFNodeNM ",
                        "      FROM SYS_SYSTEM_WF_NODE ",
                        "     WHERE SYS_ID = @SYS_ID",
                        "       AND WF_FLOW_ID = @WF_FLOW_ID",
                        "       AND WF_FLOW_VER = @WF_FLOW_VER",
                        "       AND NODE_TYPE = 'P' ",
                        "     ORDER BY SORT_ORDER ",
                        "END",
                        "ELSE",
                        "BEGIN",
                        "    ;WITH WF_ALL_NEXT_NODE AS",
                        "    (",
                        "        SELECT CAST(@WF_NODE_ID AS VARCHAR(50)) AS WF_NODE_ID",
                        "         UNION ALL",
                        "        SELECT N.WF_NODE_ID ",
                        "          FROM WF_ALL_NEXT_NODE WANN ",
                        "          JOIN SYS_SYSTEM_WF_NEXT N ",
                        "            ON WANN.WF_NODE_ID = N.NEXT_WF_NODE_ID",
                        "         WHERE N.SYS_ID = @SYS_ID",
                        "           AND N.WF_FLOW_ID = @WF_FLOW_ID",
                        "           AND N.WF_FLOW_VER = @WF_FLOW_VER",
                        "    )",
                        "    SELECT DISTINCT N.WF_NODE_ID AS WFNodeID ",
                        "         , dbo.FN_GET_NMID(N.WF_NODE_ID, {WF_NODE}) AS WFNodeNM ",
                        "      FROM SYS_SYSTEM_WF_NODE N ",
                        "	   JOIN WF_ALL_NEXT_NODE WANN",
                        "	     ON WANN.WF_NODE_ID = N.WF_NODE_ID",
                        "     WHERE SYS_ID = @SYS_ID",
                        "       AND WF_FLOW_ID = @WF_FLOW_ID",
                        "       AND WF_FLOW_VER = @WF_FLOW_VER",
                        "       AND NODE_TYPE = '" + EnumWFNodeTypeID.P + "' ",
                        "       AND N.WF_NODE_ID <> @WF_NODE_ID",
                        "END"
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_FLOW_ID.ToString(), Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_FLOW_VER.ToString(), Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_NODE_ID.ToString(), Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_NODE.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFNodePara.ParaField.WF_NODE.ToString())) });

            return GetEntityList<SystemWFNode>(commandText, dbParameters);
        }

        public class SystemWFNodeRolePara : DBCulture
        {
            public SystemWFNodeRolePara(string culture)
                : base(culture)
            {
            }

            public enum Field
            {
                SYS_ID,
                WF_FLOW_ID,
                WF_FLOW_VER,
                WF_NODE_ID,
                ROLE_ID,
                ROLE_NM,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;

            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
            public DBVarChar UpdUserID;
        }

        public class SystemWFNodeRole : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
            public DBChar HasRole;
        }

        public List<SystemWFNodeRole> SelectSystemWFNodeRoleList(SystemWFNodeRolePara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT R.SYS_ID AS SysID ",
                        "     , R.ROLE_ID AS RoleID ",
                        "     , dbo.FN_GET_NMID(R.ROLE_ID, R.{ROLE_NM}) AS RoleNM ",
                        "     , (CASE WHEN S.ROLE_ID IS NOT NULL THEN 'Y' ELSE 'N' END) AS HasRole ",
                        "  FROM SYS_SYSTEM_ROLE R ",
                        "  LEFT JOIN (SELECT SYS_ID ",
                        "                  , ROLE_ID ",
                        "                  , WF_FLOW_ID",
                        "                  , WF_FLOW_VER",
                        "                  , WF_NODE_ID ",
                        "               FROM SYS_SYSTEM_ROLE_NODE ",
                        "              WHERE SYS_ID = {SYS_ID} ",
                        "                AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "                AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "                AND WF_NODE_ID = {WF_NODE_ID} ",
                        "            ) S ",
                        "    ON R.SYS_ID = S.SYS_ID ",
                        "   AND R.ROLE_ID = S.ROLE_ID ",
                        " WHERE R.SYS_ID = {SYS_ID}; "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFNodeRolePara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeRolePara.Field.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeRolePara.Field.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeRolePara.Field.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeRolePara.Field.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFNodeRolePara.Field.ROLE_NM.ToString())) });

            return GetEntityList<SystemWFNodeRole>(commandText, dbParameters);
        }
        
        public class SystemWFNodeDetailPara
        {
            public enum Field
            {
                SYS_ID,
                WF_FLOW_ID,
                WF_FLOW_VER,
                WF_NODE_ID,
                WF_NODE_ZH_TW,
                WF_NODE_ZH_CN,
                WF_NODE_EN_US,
                WF_NODE_TH_TH,
                WF_NODE_JA_JP,
                NODE_TYPE,
                NODE_SEQ_X,
                NODE_SEQ_Y,
                NODE_POS_BEGIN_X,
                NODE_POS_BEGIN_Y,
                NODE_POS_END_X,
                NODE_POS_END_Y,
                IS_FIRST,
                IS_FINALLY,
                BACK_WF_NODE_ID,
                FUN_SYS_ID,
                FUN_CONTROLLER_ID,
                FUN_ACTION_NAME,
                ASSG_API_SYS_ID,
                ASSG_API_CONTROLLER_ID,
                ASSG_API_ACTION_NAME,
                IS_ASSG_NEXT_NODE,
                REMARK,
                SORT_ORDER,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;

            public DBVarChar WFNodeID;
            public DBNVarChar WFNodeZHTW;
            public DBNVarChar WFNodeZHCN;
            public DBNVarChar WFNodeENUS;
            public DBNVarChar WFNodeTHTH;
            public DBNVarChar WFNodeJAJP;

            public DBVarChar NodeType;
            public DBInt NodeSeqX;
            public DBInt NodeSeqY;

            public DBInt NodePosBeginX;
            public DBInt NodePosBeginY;
            public DBInt NodePosEndX;
            public DBInt NodePosEndY;

            public DBChar IsFirst;
            public DBChar IsFinally;
            public DBVarChar BackWFNodeID;

            public DBVarChar FunSysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
            public DBVarChar AssgAPISysID;
            public DBVarChar AssgAPIControllerID;
            public DBVarChar AssgAPIActionName;

            public DBChar IsAssgNextNode;

            public DBNVarChar Remark;
            public DBVarChar SortOrder;
            public DBVarChar UpdUserID;
        }

        public SystemWFNodeDetailExecuteResult SelectSystemWFNodeDetail(SystemWFNodeDetailPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
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
                        "     , WF_NODE_ZH_TW AS WFNodeZHTW",
                        "     , WF_NODE_ZH_CN AS WFNodeZHCN",
                        "     , WF_NODE_EN_US AS WFNodeENUS",
                        "     , WF_NODE_TH_TH AS WFNodeTHTH",
                        "     , WF_NODE_JA_JP AS WFNodeJAJP",
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
                        "  FROM SYS_SYSTEM_WF_NODE ",
                        " WHERE SYS_ID = {SYS_ID} ",
                        "   AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "   AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "   AND WF_NODE_ID = {WF_NODE_ID}; "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_NODE_ID, Value = para.WFNodeID });

            return GetEntityList<SystemWFNodeDetailExecuteResult>(commandText, dbParameters).SingleOrDefault();
        }

        public List<SystemWFNodeDetailExecuteResult> SelectSystemWFNodeDetailList(SystemWFNodeDetailPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT WF_NODE_ID AS WFNodeID ",
                        "     , NODE_TYPE AS NodeType ",
                        "     , NODE_SEQ_X AS NodeSeqX ",
                        "     , NODE_SEQ_Y AS NodeSeqY ",
                        "  FROM SYS_SYSTEM_WF_NODE ",
                        " WHERE SYS_ID = {SYS_ID} ",
                        "   AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "   AND WF_FLOW_VER = {WF_FLOW_VER}; "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_FLOW_VER, Value = para.WFFlowVer });

            return GetEntityList<SystemWFNodeDetailExecuteResult>(commandText, dbParameters);
        }
        
        public SystemWFNodeDetailExecuteResult EditSystemWFNodeDetail(SystemWFNodeDetailPara para, List<SystemWFNodeRolePara> paraList)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            StringBuilder commandTextStringBuilder = new StringBuilder();

            foreach (SystemWFNodeRolePara role in paraList)
            {
                string commandInsertSystemRole =
                    string.Join(Environment.NewLine,
                        new object[]
                        {
                            "        INSERT INTO SYS_SYSTEM_ROLE_NODE VALUES ( ",
                            "            {SYS_ID}, {ROLE_ID} ",
                            "          , {WF_FLOW_ID}, {WF_FLOW_VER}, {WF_NODE_ID} ",
                            "          , {UPD_USER_ID}, GETDATE() ",
                            "        ); "
                        });
                dbParameters.Add(new DBParameter { Name = SystemWFNodeRolePara.Field.SYS_ID, Value = role.SysID });
                dbParameters.Add(new DBParameter { Name = SystemWFNodeRolePara.Field.ROLE_ID, Value = role.RoleID });
                dbParameters.Add(new DBParameter { Name = SystemWFNodeRolePara.Field.WF_FLOW_ID, Value = role.WFFlowID });
                dbParameters.Add(new DBParameter { Name = SystemWFNodeRolePara.Field.WF_FLOW_VER, Value = role.WFFlowVer });
                dbParameters.Add(new DBParameter { Name = SystemWFNodeRolePara.Field.WF_NODE_ID, Value = role.WFNodeID });
                dbParameters.Add(new DBParameter { Name = SystemWFNodeRolePara.Field.UPD_USER_ID, Value = role.UpdUserID });

                commandTextStringBuilder.Append(GetCommandText(ProviderName, commandInsertSystemRole, dbParameters));
                dbParameters.Clear();
            }

            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT CHAR(1) = 'N';",
                        "DECLARE @ERROR_LINE INT;",
                        "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",

                        "BEGIN TRANSACTION ",
                        "    BEGIN TRY ",

                        "        DECLARE @SIG_API_SYS_ID VARCHAR(6) = NULL; ",
                        "        DECLARE @SIG_API_CONTROLLER_ID VARCHAR(20) = NULL; ",
                        "        DECLARE @SIG_API_ACTION_NAME VARCHAR(50) = NULL; ",
                        "        DECLARE @CHK_API_SYS_ID VARCHAR(6) = NULL; ",
                        "        DECLARE @CHK_API_CONTROLLER_ID VARCHAR(20) = NULL; ",
                        "        DECLARE @CHK_API_ACTION_NAME VARCHAR(50) = NULL; ",
                        "        DECLARE @IS_SIG_NEXT_NODE CHAR(1) = 'N'; ",
                        "        DECLARE @IS_SIG_BACK_NODE CHAR(1) = 'N'; ",
                        "        DECLARE @WF_SIG_MEMO_ZH_TW NVARCHAR(4000) = NULL; ",
                        "        DECLARE @WF_SIG_MEMO_ZH_CN NVARCHAR(4000) = NULL; ",
                        "        DECLARE @WF_SIG_MEMO_EN_US NVARCHAR(4000) = NULL; ",
                        "        DECLARE @WF_SIG_MEMO_TH_TH NVARCHAR(4000) = NULL; ",
                        "        DECLARE @WF_SIG_MEMO_JA_JP NVARCHAR(4000) = NULL; ",

                        "        SELECT @SIG_API_SYS_ID = SIG_API_SYS_ID ",
                        "             , @SIG_API_CONTROLLER_ID = SIG_API_CONTROLLER_ID ",
                        "             , @SIG_API_ACTION_NAME = SIG_API_ACTION_NAME ",
                        "             , @CHK_API_SYS_ID = CHK_API_SYS_ID ",
                        "             , @CHK_API_CONTROLLER_ID = CHK_API_CONTROLLER_ID ",
                        "             , @CHK_API_ACTION_NAME = CHK_API_ACTION_NAME ",
                        "             , @IS_SIG_NEXT_NODE = IS_SIG_NEXT_NODE ",
                        "             , @IS_SIG_BACK_NODE = IS_SIG_BACK_NODE ",
                        "             , @WF_SIG_MEMO_ZH_TW = WF_SIG_MEMO_ZH_TW ",
                        "             , @WF_SIG_MEMO_ZH_CN = WF_SIG_MEMO_ZH_CN ",
                        "             , @WF_SIG_MEMO_EN_US = WF_SIG_MEMO_EN_US ",
                        "             , @WF_SIG_MEMO_TH_TH = WF_SIG_MEMO_TH_TH ",
                        "             , @WF_SIG_MEMO_JA_JP = WF_SIG_MEMO_JA_JP ",
                        "          FROM SYS_SYSTEM_WF_NODE ",
                        "         WHERE SYS_ID = {SYS_ID} ",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "           AND WF_NODE_ID = {WF_NODE_ID}; ",

                        "        DELETE FROM SYS_SYSTEM_WF_NODE ",
                        "         WHERE SYS_ID = {SYS_ID} ",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "           AND WF_NODE_ID = {WF_NODE_ID}; ",

                        "        INSERT INTO SYS_SYSTEM_WF_NODE VALUES ( ",
                        "            {SYS_ID}, {WF_FLOW_ID}, {WF_FLOW_VER} ",
                        "          , {WF_NODE_ID}, {WF_NODE_ZH_TW}, {WF_NODE_ZH_CN}, {WF_NODE_EN_US}, {WF_NODE_TH_TH}, {WF_NODE_JA_JP} ",
                        "          , {NODE_TYPE}, {NODE_SEQ_X}, {NODE_SEQ_Y} ",
                        "          , {NODE_POS_BEGIN_X}, {NODE_POS_BEGIN_Y}, {NODE_POS_END_X}, {NODE_POS_END_Y} ",
                        "          , {IS_FIRST}, {IS_FINALLY}, {BACK_WF_NODE_ID} ",
                        "          , @WF_SIG_MEMO_ZH_TW, @WF_SIG_MEMO_ZH_CN, @WF_SIG_MEMO_EN_US, @WF_SIG_MEMO_TH_TH, @WF_SIG_MEMO_JA_JP ",
                        "          , {FUN_SYS_ID}, {FUN_CONTROLLER_ID}, {FUN_ACTION_NAME} ",
                        "          , @SIG_API_SYS_ID, @SIG_API_CONTROLLER_ID, @SIG_API_ACTION_NAME ",
                        "          , @CHK_API_SYS_ID, @CHK_API_CONTROLLER_ID, @CHK_API_ACTION_NAME ",
                        "          , {ASSG_API_SYS_ID}, {ASSG_API_CONTROLLER_ID}, {ASSG_API_ACTION_NAME} ",
                        "          , @IS_SIG_NEXT_NODE, @IS_SIG_BACK_NODE, {IS_ASSG_NEXT_NODE} ",
                        "          , {SORT_ORDER}, {REMARK}, {UPD_USER_ID}, GETDATE() ",
                        "        ); ",

                        "        DELETE FROM SYS_SYSTEM_ROLE_NODE ",
                        "         WHERE SYS_ID = {SYS_ID} ",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "           AND WF_NODE_ID = {WF_NODE_ID}; ",

                        commandTextStringBuilder.ToString(),

                        "        SET @RESULT = 'Y'; ",
                        "        COMMIT; ",

                        "    END TRY ",
                        "    BEGIN CATCH ",
                        "        SET @RESULT = 'N';",
                        "        SET @ERROR_LINE = ERROR_LINE();",
                        "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                        "        ROLLBACK TRANSACTION; ",
                        "    END CATCH; ",
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

            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_NODE_ZH_TW, Value = para.WFNodeZHTW });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_NODE_ZH_CN, Value = para.WFNodeZHCN });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_NODE_EN_US, Value = para.WFNodeENUS });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_NODE_TH_TH, Value = para.WFNodeTHTH });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_NODE_JA_JP, Value = para.WFNodeJAJP });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.NODE_TYPE, Value = para.NodeType });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.NODE_SEQ_X, Value = para.NodeSeqX });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.NODE_SEQ_Y, Value = para.NodeSeqY });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.NODE_POS_BEGIN_X, Value = para.NodePosBeginX });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.NODE_POS_BEGIN_Y, Value = para.NodePosBeginY });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.NODE_POS_END_X, Value = para.NodePosEndX });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.NODE_POS_END_Y, Value = para.NodePosEndY });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.IS_FIRST, Value = para.IsFirst });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.IS_FINALLY, Value = para.IsFinally });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.BACK_WF_NODE_ID, Value = para.BackWFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.FUN_SYS_ID, Value = para.FunSysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.FUN_ACTION_NAME, Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.ASSG_API_SYS_ID, Value = para.AssgAPISysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.ASSG_API_CONTROLLER_ID, Value = para.AssgAPIControllerID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.ASSG_API_ACTION_NAME, Value = para.AssgAPIActionName });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.IS_ASSG_NEXT_NODE, Value = para.IsAssgNextNode });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<SystemWFNodeDetailExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return result;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        
        #region - 查詢工作流程該節點尚有下一節點檔、簽核檔或文件檔存在 -
        /// <summary>
        /// 查詢工作流程該節點尚有下一節點檔、簽核檔或文件檔存在
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DBChar SelectWorkFlowChildExistList(SystemWFNodeDetailPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT CHAR(1) = 'Y';",
                        "IF     NOT EXISTS (SELECT * FROM SYS_SYSTEM_WF_NEXT WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER} AND WF_NODE_ID={WF_NODE_ID}) ",
                        "   AND NOT EXISTS (SELECT * FROM SYS_SYSTEM_WF_SIG WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER} AND WF_NODE_ID={WF_NODE_ID}) ",
                        "   AND NOT EXISTS (SELECT * FROM SYS_SYSTEM_WF_DOC WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER} AND WF_NODE_ID={WF_NODE_ID}) ",
                        "BEGIN ",
                        "    SET @RESULT = 'N'; ",
                        "END ",
                        "SELECT @RESULT;"
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_NODE_ID, Value = para.WFNodeID });
            return new DBChar(ExecuteScalar(commandText, dbParameters));
        }
        #endregion

        public enum EnumDeleteSystemWFNodeDetailResult
        {
            Success,
            Failure
        }

        public EnumDeleteSystemWFNodeDetailResult DeleteSystemWFNodeDetail(SystemWFNodeDetailPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT CHAR(1) = 'N';",
                        "DECLARE @ERROR_LINE INT;",
                        "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",

                        "BEGIN TRANSACTION ",
                        "	 BEGIN TRY ",
                        "	     DELETE FROM SYS_SYSTEM_ROLE_NODE ",
                        "	     WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER} AND WF_NODE_ID={WF_NODE_ID}; ",
                        "	     DELETE FROM SYS_SYSTEM_WF_NODE ",
                        "        WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER} AND WF_NODE_ID={WF_NODE_ID}; ",
                        "	     DELETE FROM SYS_SYSTEM_WF_NEXT ",
                        "        WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER} AND NEXT_WF_NODE_ID={WF_NODE_ID}; ",
                        "	     DELETE FROM SYS_SYSTEM_WF_ARROW ",
                        "        WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER} AND NEXT_WF_NODE_ID={WF_NODE_ID}; ",
                        "	     SET @RESULT = 'Y'; ",
                        "	     COMMIT; ",
                        "	 END TRY ",
                        "	 BEGIN CATCH ",
                        "        SET @RESULT = 'N';",
                        "        SET @ERROR_LINE = ERROR_LINE();",
                        "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                        "	     ROLLBACK TRANSACTION; ",
                        "	 END CATCH ",
                        "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFNodeDetailPara.Field.WF_NODE_ID, Value = para.WFNodeID });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumDeleteSystemWFNodeDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
    }
}