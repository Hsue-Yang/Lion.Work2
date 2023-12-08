using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemWorkFlowDetail : EntitySys
    {
        public EntitySystemWorkFlowDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class FlowTypePara : DBCulture
        {
            public FlowTypePara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                CODE_NM
            }
        }

        public class FlowType : DBTableRow, ISelectItem
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

        public List<FlowType> SelectFlowTypeList(FlowTypePara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT CODE_ID AS CodeID",
                        "     , dbo.FN_GET_NMID(CODE_ID, {CODE_NM}) AS CodeNM ",
                        "  FROM CM_CODE ",
                        " WHERE CODE_KIND = '0026' ",
                        " ORDER BY SORT_ORDER "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = FlowTypePara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(FlowTypePara.ParaField.CODE_NM.ToString())) });
            return GetEntityList<FlowType>(commandText, dbParameters);
        }

        public class SystemWorkFlowDetailPara
        {
            public enum ParaField
            {
                SYS_ID, WF_FLOW_GROUP_ID, WF_FLOW_ID,
                WF_FLOW_ZH_TW, WF_FLOW_ZH_CN, WF_FLOW_EN_US, WF_FLOW_TH_TH, WF_FLOW_JA_JP,
                WF_FLOW_VER,
                FLOW_TYPE, FLOW_MAN_USER_ID, ENABLE_DATE, DISABLE_DATE,
                IS_START_FUN,
                SORT_ORDER,
                MSG_SYS_ID,
                MSG_CONTROLLER_ID,
                MSG_ACTION_NAME,
                REMARK,
                UPD_USER_ID,
                ROLE_ID
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowGroupID;
            public DBVarChar WFFlowID;

            public DBNVarChar WFFlowZHTW;
            public DBNVarChar WFFlowZHCN;
            public DBNVarChar WFFlowENUS;
            public DBNVarChar WFFlowTHTH;
            public DBNVarChar WFFlowJAJP;
            public DBNVarChar WFFlowKOKR;

            public DBChar WFFlowVer;

            public DBVarChar FlowType;
            public DBVarChar FlowManUserID;
            public DBChar EnableDate;
            public DBChar DisableDate;
            public DBChar IsStartFun;

            public DBVarChar SortOrder;
            public DBVarChar MsgSysID;
            public DBVarChar MsgControllerID;
            public DBVarChar MsgActionName;
            public DBNVarChar Remark;
            public DBVarChar UpdUserID;
        }

        public class SystemWorkFlowDetail : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar WFFlowGroupID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;

            public DBNVarChar WFFlowZHTW;
            public DBNVarChar WFFlowZHCN;
            public DBNVarChar WFFlowENUS;
            public DBNVarChar WFFlowTHTH;
            public DBNVarChar WFFlowJAJP;

            public DBVarChar FlowType;
            public DBVarChar FlowManUserID;
            public DBChar EnableDate;
            public DBChar DisableDate;
            public DBChar IsStartFun;

            public DBVarChar SortOrder;
            public DBVarChar MsgSysID;
            public DBVarChar MsgControllerID;
            public DBVarChar MsgActionName;
            public DBNVarChar Remark;
        }

        public SystemWorkFlowDetail SelectSystemWorkFlowDetail(SystemWorkFlowDetailPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT SYS_ID AS SysID",
                        "     , WF_FLOW_GROUP_ID AS WFFlowGroupID",
                        "     , WF_FLOW_ID AS WFFlowID",
                        "     , WF_FLOW_VER AS WFFlowVer",
                        "     , WF_FLOW_ZH_TW AS WFFlowZHTW",
                        "     , WF_FLOW_ZH_CN AS WFFlowZHCN",
                        "     , WF_FLOW_EN_US AS WFFlowENUS",
                        "     , WF_FLOW_TH_TH AS WFFlowTHTH",
                        "     , WF_FLOW_JA_JP AS WFFlowJAJP",
                        "     , FLOW_TYPE AS FlowType",
                        "     , FLOW_MAN_USER_ID AS FlowManUserID",
                        "     , ENABLE_DATE AS EnableDate",
                        "     , DISABLE_DATE AS DisableDate",
                        "     , IS_START_FUN AS IsStartFun",
                        "     , SORT_ORDER AS SortOrder",
                        "     , MSG_SYS_ID AS MsgSysID",
                        "     , MSG_CONTROLLER_ID AS MsgControllerID",
                        "     , MSG_ACTION_NAME AS MsgActionName",
                        "     , REMARK AS Remark",
                        "  FROM SYS_SYSTEM_WF_FLOW ",
                        " WHERE SYS_ID = {SYS_ID}",
                        "   AND WF_FLOW_ID = {WF_FLOW_ID}",
                        "   AND WF_FLOW_VER = {WF_FLOW_VER} "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });

            return GetEntityList<SystemWorkFlowDetail>(commandText, dbParameters).SingleOrDefault();
        }

        public class FlowDetailRolePara : DBCulture
        {
            public FlowDetailRolePara(string culture)
                : base(culture)
            {
            }

            public enum Field
            {
                SYS_ID, WF_FLOW_ID, WF_FLOW_VER,
                ROLE_NM
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
        }

        public class FlowRole : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
            public DBChar HasRole;
        }

        public List<FlowRole> SelectFlowRoleList(FlowDetailRolePara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT R.SYS_ID AS SysID",
                        "     , R.ROLE_ID AS RoleID",
                        "     , dbo.FN_GET_NMID(R.ROLE_ID, R.{ROLE_NM}) AS RoleNM ",
                        "     , (CASE WHEN S.ROLE_ID IS NOT NULL THEN 'Y' ELSE 'N' END) AS HasRole ",
                        "  FROM SYS_SYSTEM_ROLE R ",
                        "  LEFT JOIN (SELECT SYS_ID, ROLE_ID, WF_FLOW_ID, WF_FLOW_VER ",
                        "               FROM SYS_SYSTEM_ROLE_FLOW ",
                        "              WHERE SYS_ID = {SYS_ID}",
                        "                AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "                AND WF_FLOW_VER = {WF_FLOW_VER}) S ",
                        "    ON R.SYS_ID = S.SYS_ID ",
                        "   AND R.ROLE_ID = S.ROLE_ID ",
                        " WHERE R.SYS_ID = {SYS_ID} "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = FlowDetailRolePara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = FlowDetailRolePara.Field.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = FlowDetailRolePara.Field.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = FlowDetailRolePara.Field.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(FlowDetailRolePara.Field.ROLE_NM.ToString())) });

            return GetEntityList<FlowRole>(commandText, dbParameters);
        }

        public class UserFlowRolePara
        {
            public DBVarChar SysID;
            public DBVarChar RoleID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar UpdUserID;
        }

        public enum EnumEditSystemWorkFlowDetailResult
        {
            Success, Failure
        }

        public EnumEditSystemWorkFlowDetailResult EditSystemWorkFlowDetail(SystemWorkFlowDetailPara para, List<UserFlowRolePara> paraList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            foreach (UserFlowRolePara role in paraList)
            {
                string commandInsertSystemRole = string.Concat(new object[]
                    {
                        "        INSERT INTO SYS_SYSTEM_ROLE_FLOW VALUES ( ",
                        "            {SYS_ID} ",
                        "          , {ROLE_ID} ",
                        "          , {WF_FLOW_ID} ",
                        "          , {WF_FLOW_VER} ",
                        "          , {UPD_USER_ID}, GETDATE() ",
                        "        ); ",
                    });

                dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.SYS_ID, Value = role.SysID });
                dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.ROLE_ID, Value = role.RoleID });
                dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.WF_FLOW_ID, Value = role.WFFlowID });
                dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.WF_FLOW_VER, Value = role.WFFlowVer });
                dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.UPD_USER_ID, Value = role.UpdUserID });

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

                        "        DECLARE @SHIFT_POS_X INT = 0; ",
                        "        DECLARE @SHIFT_POS_Y INT = 0; ",

                        "        DELETE FROM SYS_SYSTEM_ROLE_FLOW ",
                        "        WHERE SYS_ID = {SYS_ID} AND WF_FLOW_ID = {WF_FLOW_ID} AND WF_FLOW_VER = {WF_FLOW_VER}; ",
                        "        IF NOT EXISTS (SELECT *",
                        "                         FROM SYS_SYSTEM_WF_FLOW",
                        "                        WHERE SYS_ID = {SYS_ID}",
                        "                          AND WF_FLOW_ID = {WF_FLOW_ID}",
                        "                          AND WF_FLOW_VER = {WF_FLOW_VER}) ",
                        "        BEGIN ",
                        "            INSERT INTO SYS_SYSTEM_WF_FLOW VALUES ( ",
                        "                {SYS_ID} ",
                        "              , {WF_FLOW_GROUP_ID} ",
                        "              , {WF_FLOW_ID} ",
                        "              , {WF_FLOW_ZH_TW}, {WF_FLOW_ZH_CN}, {WF_FLOW_EN_US}, {WF_FLOW_TH_TH}, {WF_FLOW_JA_JP} ",
                        "              , {WF_FLOW_VER}, {FLOW_TYPE}, UPPER({FLOW_MAN_USER_ID}), {ENABLE_DATE}, {DISABLE_DATE} ",
                        "              , {IS_START_FUN} ",
                        "              , {SORT_ORDER} ",
                        "              , {MSG_SYS_ID} ",
                        "              , {MSG_CONTROLLER_ID} ",
                        "              , {MSG_ACTION_NAME} ",
                        "              , {REMARK} ",
                        "              , {UPD_USER_ID}, GETDATE() ",
                        "            ); ",
                        "        END",
                        "        BEGIN ",
                        "            UPDATE SYS_SYSTEM_WF_FLOW ",
                        "               SET WF_FLOW_ZH_TW = {WF_FLOW_ZH_TW} ",
                        "                 , WF_FLOW_ZH_CN = {WF_FLOW_ZH_CN} ",
                        "                 , WF_FLOW_EN_US = {WF_FLOW_EN_US} ",
                        "                 , WF_FLOW_TH_TH = {WF_FLOW_TH_TH} ",
                        "                 , WF_FLOW_JA_JP = {WF_FLOW_JA_JP} ",
                        "                 , FLOW_TYPE = {FLOW_TYPE} ",
                        "                 , FLOW_MAN_USER_ID = UPPER({FLOW_MAN_USER_ID}) ",
                        "                 , ENABLE_DATE = {ENABLE_DATE} ",
                        "                 , DISABLE_DATE = {DISABLE_DATE} ",
                        "                 , IS_START_FUN = {IS_START_FUN} ",
                        "                 , SORT_ORDER = {SORT_ORDER} ",
                        "                 , MSG_SYS_ID = {MSG_SYS_ID} ",
                        "                 , MSG_CONTROLLER_ID = {MSG_CONTROLLER_ID} ",
                        "                 , MSG_ACTION_NAME = {MSG_ACTION_NAME} ",
                        "                 , REMARK = {REMARK} ",
                        "                 , UPD_USER_ID = {UPD_USER_ID} ",
                        "                 , UPD_DT = GETDATE() ",
                        "             WHERE SYS_ID = {SYS_ID}",
                        "               AND WF_FLOW_ID = {WF_FLOW_ID}",
                        "               AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "        END;",

                        #region - 新增起始點 -
                        "        IF NOT EXISTS (SELECT *",
                        "                         FROM SYS_SYSTEM_WF_NODE",
                        "                        WHERE SYS_ID = {SYS_ID}",
                        "                          AND WF_FLOW_ID = {WF_FLOW_ID}",
                        "                          AND WF_FLOW_VER = {WF_FLOW_VER}",
                        "                          AND NODE_TYPE = 'S') ",
                        "        BEGIN ",
                        "            INSERT INTO SYS_SYSTEM_WF_NODE ",
                        "            SELECT {SYS_ID} AS SYS_ID ",
                        "                 , {WF_FLOW_ID} AS WF_FLOW_ID ",
                        "                 , {WF_FLOW_VER} AS WF_FLOW_VER ",
                        "                 , 'START' AS WF_NODE_ID ",
                        "                 , 'START' AS WF_NODE_ZH_TW ",
                        "                 , 'START' AS WF_NODE_ZH_CN ",
                        "                 , 'START' AS WF_NODE_EN_US ",
                        "                 , 'START' AS WF_NODE_TH_TH ",
                        "                 , 'START' AS WF_NODE_JA_JP ",
                        "                 , 'S' AS NODE_TYPE ",
                        "                 , (ISNULL((SELECT MIN(NODE_SEQ_X) FROM SYS_SYSTEM_WF_NODE WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER}),0))-1 AS NODE_SEQ_X ",
                        "                 , (ISNULL((SELECT MIN(NODE_SEQ_Y) FROM SYS_SYSTEM_WF_NODE WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER}),0))-1 AS NODE_SEQ_Y ",
                        "                 , 0 AS NODE_POS_BEGIN_X ",
                        "                 , 0 AS NODE_POS_BEGIN_Y ",
                        "                 , 0 AS NODE_POS_END_X ",
                        "                 , 0 AS NODE_POS_END_Y ",
                        "                 , 'N' AS IS_FIRST ",
                        "                 , 'N' AS IS_FINALLY ",
                        "                 , NULL AS BACK_WF_NODE_ID ",
                        "                 , NULL AS WF_SIG_MEMO_ZH_TW ",
                        "                 , NULL AS WF_SIG_MEMO_ZH_CN ",
                        "                 , NULL AS WF_SIG_MEMO_EN_US ",
                        "                 , NULL AS WF_SIG_MEMO_TH_TH ",
                        "                 , NULL AS WF_SIG_MEMO_JA_JP ",
                        "                 , NULL AS FUN_SYS_ID ",
                        "                 , NULL AS FUN_CONTROLLER_ID ",
                        "                 , NULL AS FUN_ACTION_NAME ",
                        "                 , NULL AS SIG_API_SYS_ID ",
                        "                 , NULL AS SIG_API_CONTROLLER_ID ",
                        "                 , NULL AS SIG_API_ACTION_NAME ",
                        "                 , NULL AS CHK_API_SYS_ID ",
                        "                 , NULL AS CHK_API_CONTROLLER_ID ",
                        "                 , NULL AS CHK_API_ACTION_NAME ",
                        "                 , NULL AS ASSG_API_SYS_ID ",
                        "                 , NULL AS ASSG_API_CONTROLLER_ID ",
                        "                 , NULL AS ASSG_API_ACTION_NAME ",
                        "                 , 'N' AS IS_SIG_NEXT_NODE ",
                        "                 , 'N' AS IS_SIG_BACK_NODE ",
                        "                 , 'N' AS IS_ASSG_NEXT_NODE ",
                        "                 , '000000' AS SORT_ORDER ",
                        "                 , NULL AS REMARK ",
                        "                 , {UPD_USER_ID} AS UPD_USER_ID ",
                        "                 , GETDATE() AS UPD_DT; ",
                        "        END; ",
                        #endregion
                        
                        #region - 新增結束點 -
                        "        IF NOT EXISTS (SELECT *",
                        "                         FROM SYS_SYSTEM_WF_NODE",
                        "                        WHERE SYS_ID = {SYS_ID}",
                        "                          AND WF_FLOW_ID = {WF_FLOW_ID}",
                        "                          AND WF_FLOW_VER = {WF_FLOW_VER}",
                        "                          AND NODE_TYPE = 'E') ",
                        "        BEGIN ",
                        "            INSERT INTO SYS_SYSTEM_WF_NODE ",
                        "            SELECT {SYS_ID} AS SYS_ID ",
                        "                 , {WF_FLOW_ID} AS WF_FLOW_ID ",
                        "                 , {WF_FLOW_VER} AS WF_FLOW_VER ",
                        "                 , 'END' AS WF_NODE_ID ",
                        "                 , 'END' AS WF_NODE_ZH_TW ",
                        "                 , 'END' AS WF_NODE_ZH_CN ",
                        "                 , 'END' AS WF_NODE_EN_US ",
                        "                 , 'END' AS WF_NODE_TH_TH ",
                        "                 , 'END' AS WF_NODE_JA_JP ",
                        "                 , 'E' AS NODE_TYPE ",
                        "                 , (ISNULL((SELECT MAX(NODE_SEQ_X) FROM SYS_SYSTEM_WF_NODE WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER}),0))+1 AS NODE_SEQ_X ",
                        "                 , (ISNULL((SELECT MAX(NODE_SEQ_Y) FROM SYS_SYSTEM_WF_NODE WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER}),0)) AS NODE_SEQ_Y ",
                        "                 , 0 AS NODE_POS_BEGIN_X ",
                        "                 , 0 AS NODE_POS_BEGIN_Y ",
                        "                 , 0 AS NODE_POS_END_X ",
                        "                 , 0 AS NODE_POS_END_Y ",
                        "                 , 'N' AS IS_FIRST ",
                        "                 , 'N' AS IS_FINALLY ",
                        "                 , NULL AS BACK_WF_NODE_ID ",
                        "                 , NULL AS WF_SIG_MEMO_ZH_TW ",
                        "                 , NULL AS WF_SIG_MEMO_ZH_CN ",
                        "                 , NULL AS WF_SIG_MEMO_EN_US ",
                        "                 , NULL AS WF_SIG_MEMO_TH_TH ",
                        "                 , NULL AS WF_SIG_MEMO_JA_JP ",
                        "                 , NULL AS FUN_SYS_ID ",
                        "                 , NULL AS FUN_CONTROLLER_ID ",
                        "                 , NULL AS FUN_ACTION_NAME ",
                        "                 , NULL AS SIG_API_SYS_ID ",
                        "                 , NULL AS SIG_API_CONTROLLER_ID ",
                        "                 , NULL AS SIG_API_ACTION_NAME ",
                        "                 , NULL AS CHK_API_SYS_ID ",
                        "                 , NULL AS CHK_API_CONTROLLER_ID ",
                        "                 , NULL AS CHK_API_ACTION_NAME ",
                        "                 , NULL AS ASSG_API_SYS_ID ",
                        "                 , NULL AS ASSG_API_CONTROLLER_ID ",
                        "                 , NULL AS ASSG_API_ACTION_NAME ",
                        "                 , 'N' AS IS_SIG_NEXT_NODE ",
                        "                 , 'N' AS IS_SIG_BACK_NODE ",
                        "                 , 'N' AS IS_ASSG_NEXT_NODE ",
                        "                 , '999999' AS SORT_ORDER ",
                        "                 , NULL AS REMARK ",
                        "                 , {UPD_USER_ID} AS UPD_USER_ID ",
                        "                 , GETDATE() AS UPD_DT; ",
                        "        END; ",
                        #endregion

                        "        SELECT @SHIFT_POS_X=ABS(MIN(CASE WHEN NODE_SEQ_X<0 THEN NODE_SEQ_X ELSE 0 END)) ",
                        "             , @SHIFT_POS_Y=ABS(MIN(CASE WHEN NODE_SEQ_Y<0 THEN NODE_SEQ_Y ELSE 0 END)) ",
                        "          FROM SYS_SYSTEM_WF_NODE ",
                        "         WHERE SYS_ID = {SYS_ID}",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID}",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER}; ",

                        "        UPDATE SYS_SYSTEM_WF_NODE ",
                        "           SET NODE_SEQ_X = NODE_SEQ_X + @SHIFT_POS_X",
                        "             , NODE_SEQ_Y=NODE_SEQ_Y + @SHIFT_POS_Y ",
                        "         WHERE SYS_ID = {SYS_ID}",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID}",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER}; ",

                        commandTextStringBuilder.ToString(),

                        "        SET @RESULT = 'Y'; ",
                        "        COMMIT; ",
                        "    END TRY ",
                        "    BEGIN CATCH ",
                        "        SET @RESULT = 'N';",
                        "        SET @ERROR_LINE = ERROR_LINE();",
                        "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                        "        ROLLBACK TRANSACTION; ",
                        "    END CATCH ",
                        "; ",
                        "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                    });

            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.WF_FLOW_GROUP_ID, Value = para.WFFlowGroupID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.WF_FLOW_ZH_TW, Value = para.WFFlowZHTW });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.WF_FLOW_ZH_CN, Value = para.WFFlowZHCN });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.WF_FLOW_EN_US, Value = para.WFFlowENUS });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.WF_FLOW_TH_TH, Value = para.WFFlowTHTH });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.WF_FLOW_JA_JP, Value = para.WFFlowJAJP });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.FLOW_TYPE, Value = para.FlowType });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.FLOW_MAN_USER_ID, Value = para.FlowManUserID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.ENABLE_DATE, Value = para.EnableDate });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.DISABLE_DATE, Value = para.DisableDate });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.IS_START_FUN, Value = para.IsStartFun });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.MSG_SYS_ID, Value = para.MsgSysID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.MSG_CONTROLLER_ID, Value = para.MsgControllerID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.MSG_ACTION_NAME, Value = para.MsgActionName });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditSystemWorkFlowDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }

        public enum EnumDeleteSystemWorkFlowDetailResult
        {
            Success, Failure
        }

        public EnumDeleteSystemWorkFlowDetailResult DeleteSystemWorkFlowDetail(SystemWorkFlowDetailPara para)
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
                        "        DELETE FROM SYS_SYSTEM_ROLE_SIG WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER}; ",
                        "        DELETE FROM SYS_SYSTEM_ROLE_NODE WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER}; ",
                        "        DELETE FROM SYS_SYSTEM_ROLE_FLOW WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER}; ",
                        "        DELETE FROM SYS_SYSTEM_WF_DOC WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER}; ",
                        "        DELETE FROM SYS_SYSTEM_WF_SIG WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER}; ",
                        "        DELETE FROM SYS_SYSTEM_WF_ARROW WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER}; ",
                        "        DELETE FROM SYS_SYSTEM_WF_NEXT WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER}; ",
                        "        DELETE FROM SYS_SYSTEM_WF_NODE WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER}; ",
                        "        DELETE FROM SYS_SYSTEM_WF_FLOW WHERE SYS_ID={SYS_ID} AND WF_FLOW_ID={WF_FLOW_ID} AND WF_FLOW_VER={WF_FLOW_VER}; ",
                        "        SET @RESULT = 'Y'; ",
                        "        COMMIT; ",
                        "    END TRY ",
                        "    BEGIN CATCH ",
                        "        SET @RESULT = 'N';",
                        "        SET @ERROR_LINE = ERROR_LINE();",
                        "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                        "        ROLLBACK TRANSACTION; ",
                        "    END CATCH ",
                        "; ",
                        "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowDetailPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumDeleteSystemWorkFlowDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
    }
}
