using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemWorkFlowSignatureDetail : EntitySys
    {
        public EntitySystemWorkFlowSignatureDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemWFSigSeqPara : DBCulture
        {
            public SystemWFSigSeqPara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                SYS_ID,
                WF_FLOW_ID,
                WF_FLOW_VER,
                WF_NODE_ID,
                WF_NODE,
                WF_SIG
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
        }

        public class SystemWFSigSeq : DBTableRow, ISelectItem
        {
            public DBVarChar CodeID;
            public DBNVarChar CodeNM;

            public string ItemText()
            {
                return CodeNM.StringValue();
            }

            public string ItemValue()
            {
                return CodeID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SystemWFSigSeq> SelectSystemWFSigSeqList(SystemWFSigSeqPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @SYS_ID VARCHAR(12) = {SYS_ID};",
                        "DECLARE @WF_FLOW_ID VARCHAR(50) = {WF_FLOW_ID};",
                        "DECLARE @WF_FLOW_VER VARCHAR(3) = {WF_FLOW_VER};",
                        "DECLARE @WF_NODE_ID VARCHAR(50) = {WF_NODE_ID};",

                        ";WITH WF_ALL_NEXT_NODE AS",
                        "(",
                        "    SELECT CAST(@WF_NODE_ID AS VARCHAR(50)) AS WF_NODE_ID",
                        "     UNION ALL",
                        "    SELECT N.WF_NODE_ID ",
                        "      FROM WF_ALL_NEXT_NODE WANN ",
                        "      JOIN SYS_SYSTEM_WF_NEXT N ",
                        "        ON WANN.WF_NODE_ID = N.NEXT_WF_NODE_ID",
                        "     WHERE N.SYS_ID = @SYS_ID",
                        "       AND N.WF_FLOW_ID = @WF_FLOW_ID",
                        "       AND N.WF_FLOW_VER = @WF_FLOW_VER",
                        ")",
                        "SELECT N.WF_NODE_ID + '|' + S.WF_SIG_SEQ AS CodeID ",
                        "     , dbo.FN_GET_NMID(N.WF_NODE_ID, N.WF_NODE_ZH_TW) + '-' ",
                        "     + CAST(S.SIG_STEP AS VARCHAR) + '-' ",
                        "     + dbo.FN_GET_NMID(S.WF_SIG_SEQ, S.WF_SIG_ZH_TW) AS CodeNM ",
                        "  FROM SYS_SYSTEM_WF_NODE N ",
                        "  JOIN WF_ALL_NEXT_NODE WANN",
                        "    ON WANN.WF_NODE_ID = N.WF_NODE_ID",
                        "  JOIN SYS_SYSTEM_WF_SIG S",
                        "    ON S.SYS_ID = N.SYS_ID ",
                        "   AND S.WF_FLOW_ID = N.WF_FLOW_ID ",
                        "   AND S.WF_FLOW_VER = N.WF_FLOW_VER ",
                        "   AND S.WF_NODE_ID = N.WF_NODE_ID ",
                        " WHERE S.SYS_ID = @SYS_ID",
                        "   AND S.WF_FLOW_ID = @WF_FLOW_ID",
                        "   AND S.WF_FLOW_VER = @WF_FLOW_VER",
                        "   AND NODE_TYPE = 'P' ",
                        "   AND N.WF_NODE_ID <> @WF_NODE_ID",
                        " ORDER BY N.SORT_ORDER, N.WF_NODE_ID, S.SIG_STEP, S.WF_SIG_SEQ",
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFSigSeqPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigSeqPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigSeqPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFSigSeqPara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigSeqPara.ParaField.WF_NODE, Value = para.GetCultureFieldNM(new DBObject(SystemWFSigSeqPara.ParaField.WF_NODE.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemWFSigSeqPara.ParaField.WF_SIG, Value = para.GetCultureFieldNM(new DBObject(SystemWFSigSeqPara.ParaField.WF_SIG.ToString())) });
            return GetEntityList<SystemWFSigSeq>(commandText, dbParameters);
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
                SIG_STEP,
                WF_SIG_SEQ,
                WF_SIG_ZH_TW,
                WF_SIG_ZH_CN,
                WF_SIG_EN_US,
                WF_SIG_TH_TH,
                WF_SIG_JA_JP,
                SIG_TYPE,
                API_SYS_ID,
                API_CONTROLLER_ID,
                API_ACTION_NAME,
                COMPARE_WF_NODE_ID,
                COMPARE_WF_SIG_SEQ,
                CHK_API_SYS_ID,
                CHK_API_CONTROLLER_ID,
                CHK_API_ACTION_NAME,
                IS_REQ,
                REMARK,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;

            public DBInt SigStep;
            public DBChar WFSigSeq;

            public DBNVarChar WFSigZHTW;
            public DBNVarChar WFSigZHCN;
            public DBNVarChar WFSigENUS;
            public DBNVarChar WFSigTHTH;
            public DBNVarChar WFSigJAJP;

            public DBVarChar SigType;

            public DBVarChar APISysID;
            public DBVarChar APIControllerID;
            public DBVarChar APIActionName;

            public DBVarChar CompareWFNodeID;
            public DBChar CompareWFSigSeq;

            public DBVarChar ChkAPISysID;
            public DBVarChar ChkAPIControllerID;
            public DBVarChar ChkAPIActionName;

            public DBChar IsReq;
            public DBNVarChar Remark;
            public DBVarChar UpdUserID;
        }

        public class SystemWFSig : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
            public DBChar WFSigSeq;

            public DBInt SigStep;

            public DBNVarChar WFSigZHTW;
            public DBNVarChar WFSigZHCN;
            public DBNVarChar WFSigENUS;
            public DBNVarChar WFSigTHTH;
            public DBNVarChar WFSigJAJP;

            public DBVarChar SigType;

            public DBVarChar APISysID;
            public DBVarChar APIControllerID;
            public DBVarChar APIActionName;

            public DBVarChar CompareWFNodeID;
            public DBChar CompareWFSigSeq;

            public DBVarChar ChkAPISysID;
            public DBVarChar ChkAPIControllerID;
            public DBVarChar ChkAPIActionName;

            public DBChar IsReq;
            public DBNVarChar Remark;
        }

        public SystemWFSig SelectSystemWFSig(SystemWFSigPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT SYS_ID AS SysID ",
                        "     , WF_FLOW_ID AS WFFlowID ",
                        "     , WF_FLOW_VER AS WFFlowVer ",
                        "     , WF_NODE_ID AS WFNodeID ",
                        "     , WF_SIG_SEQ AS WFSigSeq ",
                        "     , SIG_STEP AS SigStep ",
                        "     , WF_SIG_ZH_TW AS WFSigZHTW ",
                        "     , WF_SIG_ZH_CN AS WFSigZHCN ",
                        "     , WF_SIG_EN_US AS WFSigENUS ",
                        "     , WF_SIG_TH_TH AS WFSigTHTH ",
                        "     , WF_SIG_JA_JP AS WFSigJAJP ",
                        "     , SIG_TYPE AS SigType ",
                        "     , API_SYS_ID AS APISysID ",
                        "     , API_CONTROLLER_ID AS APIControllerID ",
                        "     , API_ACTION_NAME AS APIActionName ",
                        "     , COMPARE_WF_NODE_ID AS CompareWFNodeID ",
                        "     , COMPARE_WF_SIG_SEQ AS CompareWFSigSeq ",
                        "     , CHK_API_SYS_ID AS ChkAPISysID ",
                        "     , CHK_API_CONTROLLER_ID AS ChkAPIControllerID ",
                        "     , CHK_API_ACTION_NAME AS ChkAPIActionName ",
                        "     , IS_REQ AS IsReq ",
                        "     , REMARK AS Remark ",
                        "  FROM SYS_SYSTEM_WF_SIG ",
                        " WHERE SYS_ID = {SYS_ID} ",
                        "   AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "   AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "   AND WF_NODE_ID = {WF_NODE_ID} ",
                        "   AND WF_SIG_SEQ = {WF_SIG_SEQ} "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_SIG_SEQ, Value = para.WFSigSeq });
            return GetEntityList<SystemWFSig>(commandText, dbParameters).SingleOrDefault();
        }

        public class SystemRoleSigPara : DBCulture
        {
            public SystemRoleSigPara(string culture)
                : base(culture)
            {
            }

            public enum ParaField
            {
                SYS_ID,
                WF_FLOW_ID,
                WF_FLOW_VER,
                WF_NODE_ID,
                WF_SIG_SEQ,
                ROLE_ID,
                ROLE_NM,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
            public DBChar WFSigSeq;

            public DBVarChar RoleID;
            public DBVarChar UpdUserID;
        }

        public class SystemRoleSig : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
            public DBChar WFSigSeq;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
            public DBChar HasRole;
        }

        public List<SystemRoleSig> SelectSystemRoleSigList(SystemRoleSigPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT R.SYS_ID AS SysID ",
                        "     , S.WF_FLOW_ID AS WFFlowID ",
                        "     , S.WF_FLOW_VER AS WFFlowVer ",
                        "     , S.WF_NODE_ID AS WFNodeID ",
                        "     , S.WF_SIG_SEQ AS WFSigSeq ",
                        "     , R.ROLE_ID AS RoleID ",
                        "     , dbo.FN_GET_NMID(R.ROLE_ID, R.{ROLE_NM}) AS RoleNM ",
                        "     , (CASE WHEN S.ROLE_ID IS NOT NULL ",
                        "             THEN 'Y' ",
                        "             ELSE 'N' ",
                        "        END) AS HasRole ",
                        "  FROM SYS_SYSTEM_ROLE R ",
                        "  LEFT JOIN (",
                        "             SELECT SYS_ID ",
                        "                  , WF_FLOW_ID ",
                        "                  , WF_FLOW_VER ",
                        "                  , WF_NODE_ID ",
                        "                  , WF_SIG_SEQ ",
                        "                  , ROLE_ID ",
                        "               FROM SYS_SYSTEM_ROLE_SIG ",
                        "              WHERE SYS_ID = {SYS_ID} ",
                        "                AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "                AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "                AND WF_NODE_ID = {WF_NODE_ID} ",
                        "  			     AND WF_SIG_SEQ = {WF_SIG_SEQ}",
                        "             ) S ",
                        "    ON R.SYS_ID = S.SYS_ID ",
                        "   AND R.ROLE_ID = S.ROLE_ID ",
                        " WHERE R.SYS_ID = {SYS_ID} "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.WF_SIG_SEQ, Value = para.WFSigSeq });
            dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemRoleSigPara.ParaField.ROLE_NM.ToString())) });
            return GetEntityList<SystemRoleSig>(commandText, dbParameters);
        }

        public enum EnumInsertSystemWFSigResult
        {
            Success,
            Failure
        }

        public EnumInsertSystemWFSigResult InsertSystemWFSig(SystemWFSigPara para, List<SystemRoleSigPara> paraList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            if (paraList != null)
            {
                foreach (SystemRoleSigPara role in paraList)
                {
                    string commandInsertSystemRole =
                        string.Join(Environment.NewLine,
                            new object[]
                        {
                            "        INSERT INTO SYS_SYSTEM_ROLE_SIG VALUES ( ",
                            "              {SYS_ID}, {ROLE_ID}, {WF_FLOW_ID}, {WF_FLOW_VER}, {WF_NODE_ID} ",
                            "            , @WF_SIG_SEQ ",
                            "            , {UPD_USER_ID}, GETDATE() ",
                            "        ); "
                        });

                    dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.SYS_ID, Value = role.SysID });
                    dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.ROLE_ID, Value = role.RoleID });
                    dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.WF_FLOW_ID, Value = role.WFFlowID });
                    dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.WF_FLOW_VER, Value = role.WFFlowVer });
                    dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.WF_NODE_ID, Value = role.WFNodeID });
                    dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.UPD_USER_ID, Value = role.UpdUserID });

                    commandTextStringBuilder.Append(GetCommandText(ProviderName, commandInsertSystemRole, dbParameters));
                    dbParameters.Clear();
                }
            }

            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT CHAR(1) = 'N';",
                        "DECLARE @ERROR_LINE INT;",
                        "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                        
                        "DECLARE @WF_SIG_SEQ CHAR(3); ", 

                        "BEGIN TRANSACTION ", 
                        "    BEGIN TRY ", 

                        "        SELECT @WF_SIG_SEQ = RIGHT('00' + CAST(ISNULL(CAST(MAX(WF_SIG_SEQ) AS INT), 0) + 1 AS VARCHAR), 3) ", 
                        "          FROM SYS_SYSTEM_WF_SIG ", 
                        "         WHERE SYS_ID = {SYS_ID} ", 
                        "           AND WF_FLOW_ID = {WF_FLOW_ID} ", 
                        "           AND WF_FLOW_VER = {WF_FLOW_VER} ", 
                        "           AND WF_NODE_ID = {WF_NODE_ID}; ", 

                        "        INSERT INTO SYS_SYSTEM_WF_SIG VALUES ( ", 
                        "              {SYS_ID}, {WF_FLOW_ID}, {WF_FLOW_VER}, {WF_NODE_ID} ", 
                        "            , {SIG_STEP}, @WF_SIG_SEQ ", 
                        "            , {WF_SIG_ZH_TW}, {WF_SIG_ZH_CN}, {WF_SIG_EN_US}, {WF_SIG_TH_TH}, {WF_SIG_JA_JP} ", 
                        "            , {SIG_TYPE} ", 
                        "            , {API_SYS_ID}, {API_CONTROLLER_ID}, {API_ACTION_NAME} ", 
                        "            , {COMPARE_WF_NODE_ID}, {COMPARE_WF_SIG_SEQ} ", 
                        "            , {CHK_API_SYS_ID}, {CHK_API_CONTROLLER_ID}, {CHK_API_ACTION_NAME} ", 
                        "            , {IS_REQ}, {REMARK} ", 
                        "            , {UPD_USER_ID}, GETDATE() ", 
                        "        );", 

                        commandTextStringBuilder.ToString(),
                        
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

            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.SIG_STEP, Value = para.SigStep });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_SIG_ZH_TW, Value = para.WFSigZHTW });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_SIG_ZH_CN, Value = para.WFSigZHCN });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_SIG_EN_US, Value = para.WFSigENUS });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_SIG_TH_TH, Value = para.WFSigTHTH });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_SIG_JA_JP, Value = para.WFSigJAJP });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.SIG_TYPE, Value = para.SigType });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.API_SYS_ID, Value = para.APISysID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.API_CONTROLLER_ID, Value = para.APIControllerID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.API_ACTION_NAME, Value = para.APIActionName });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.COMPARE_WF_NODE_ID, Value = para.CompareWFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.COMPARE_WF_SIG_SEQ, Value = para.CompareWFSigSeq });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.CHK_API_SYS_ID, Value = para.ChkAPISysID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.CHK_API_CONTROLLER_ID, Value = para.ChkAPIControllerID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.CHK_API_ACTION_NAME, Value = para.ChkAPIActionName });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.IS_REQ, Value = para.IsReq });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumInsertSystemWFSigResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }

        public enum EnumUpdateSystemWFSigResult
        {
            Success,
            Failure
        }

        public EnumUpdateSystemWFSigResult UpdateSystemWFSig(SystemWFSigPara para, List<SystemRoleSigPara> paraList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            if (paraList != null)
            {
                foreach (SystemRoleSigPara role in paraList)
                {
                    string commandInsertSystemRole =
                        string.Join(Environment.NewLine,
                            new object[]
                        {
                            "        INSERT INTO SYS_SYSTEM_ROLE_SIG VALUES ( ",
                            "              {SYS_ID}, {ROLE_ID}, {WF_FLOW_ID}, {WF_FLOW_VER}, {WF_NODE_ID} ",
                            "            , {WF_SIG_SEQ} ",
                            "            , {UPD_USER_ID}, GETDATE() ",
                            "        ); "
                        });

                    dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.SYS_ID, Value = role.SysID });
                    dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.ROLE_ID, Value = role.RoleID });
                    dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.WF_FLOW_ID, Value = role.WFFlowID });
                    dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.WF_FLOW_VER, Value = role.WFFlowVer });
                    dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.WF_NODE_ID, Value = role.WFNodeID });
                    dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.WF_SIG_SEQ, Value = role.WFSigSeq });
                    dbParameters.Add(new DBParameter { Name = SystemRoleSigPara.ParaField.UPD_USER_ID, Value = role.UpdUserID });

                    commandTextStringBuilder.Append(GetCommandText(ProviderName, commandInsertSystemRole, dbParameters));
                    dbParameters.Clear();
                }
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
                        "        DELETE FROM SYS_SYSTEM_WF_SIG ",
                        "        WHERE SYS_ID = {SYS_ID} ",
                        "          AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "          AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "          AND WF_NODE_ID = {WF_NODE_ID} ",
                        "          AND WF_SIG_SEQ = {WF_SIG_SEQ}; ",

                        "        INSERT INTO SYS_SYSTEM_WF_SIG VALUES ( ",
                        "              {SYS_ID}, {WF_FLOW_ID}, {WF_FLOW_VER}, {WF_NODE_ID} ",
                        "            , {SIG_STEP}, {WF_SIG_SEQ} ",
                        "            , {WF_SIG_ZH_TW}, {WF_SIG_ZH_CN}, {WF_SIG_EN_US}, {WF_SIG_TH_TH}, {WF_SIG_JA_JP} ",
                        "            , {SIG_TYPE} ",
                        "            , {API_SYS_ID}, {API_CONTROLLER_ID}, {API_ACTION_NAME} ",
                        "            , {COMPARE_WF_NODE_ID}, {COMPARE_WF_SIG_SEQ} ",
                        "            , {CHK_API_SYS_ID}, {CHK_API_CONTROLLER_ID}, {CHK_API_ACTION_NAME} ",
                        "            , {IS_REQ}, {REMARK} ",
                        "            , {UPD_USER_ID}, GETDATE() ",
                        "        );   ",

                        "		  DELETE FROM SYS_SYSTEM_ROLE_SIG ",
                        "		  WHERE SYS_ID = {SYS_ID} ",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "           AND WF_NODE_ID = {WF_NODE_ID} ",
                        "           AND WF_SIG_SEQ = {WF_SIG_SEQ}; ",

                        commandTextStringBuilder.ToString(),

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

            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.SIG_STEP, Value = para.SigStep });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_SIG_SEQ, Value = para.WFSigSeq });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_SIG_ZH_TW, Value = para.WFSigZHTW });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_SIG_ZH_CN, Value = para.WFSigZHCN });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_SIG_EN_US, Value = para.WFSigENUS });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_SIG_TH_TH, Value = para.WFSigTHTH });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_SIG_JA_JP, Value = para.WFSigJAJP });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.SIG_TYPE, Value = para.SigType });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.API_SYS_ID, Value = para.APISysID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.API_CONTROLLER_ID, Value = para.APIControllerID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.API_ACTION_NAME, Value = para.APIActionName });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.COMPARE_WF_NODE_ID, Value = para.CompareWFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.COMPARE_WF_SIG_SEQ, Value = para.CompareWFSigSeq });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.CHK_API_SYS_ID, Value = para.ChkAPISysID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.CHK_API_CONTROLLER_ID, Value = para.ChkAPIControllerID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.CHK_API_ACTION_NAME, Value = para.ChkAPIActionName });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.IS_REQ, Value = para.IsReq });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumUpdateSystemWFSigResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }

        public enum EnumDeleteSystemWFSigResult
        {
            Success,
            Failure
        }

        public EnumDeleteSystemWFSigResult DeleteSystemWFSig(SystemWFSigPara para)
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

                        "        DELETE FROM SYS_SYSTEM_ROLE_SIG ", 
                        "        WHERE SYS_ID = {SYS_ID} ", 
                        "          AND WF_FLOW_ID = {WF_FLOW_ID} ", 
                        "          AND WF_FLOW_VER = {WF_FLOW_VER} ", 
                        "          AND WF_NODE_ID = {WF_NODE_ID} ", 
                        "          AND WF_SIG_SEQ = {WF_SIG_SEQ}; ", 

                        "        DELETE FROM SYS_SYSTEM_WF_SIG ", 
                        "        WHERE SYS_ID = {SYS_ID} ", 
                        "          AND WF_FLOW_ID = {WF_FLOW_ID} ", 
                        "          AND WF_FLOW_VER = {WF_FLOW_VER} ", 
                        "          AND WF_NODE_ID = {WF_NODE_ID} ", 
                        "          AND WF_SIG_SEQ = {WF_SIG_SEQ}; ", 

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
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFSigPara.ParaField.WF_SIG_SEQ, Value = para.WFSigSeq });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumDeleteSystemWFSigResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
    }
}
