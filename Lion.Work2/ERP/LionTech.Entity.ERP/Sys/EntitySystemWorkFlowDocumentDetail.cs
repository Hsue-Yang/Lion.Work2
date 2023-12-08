using System;
using System.Collections.Generic;
using System.Linq;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemWorkFlowDocumentDetail : EntitySys
    {
        public EntitySystemWorkFlowDocumentDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemWFDocPara : DBCulture
        {
            public SystemWFDocPara(string culture)
                : base(culture)
            {
            }

            public enum ParaField
            {
                SYS_ID,
                WF_FLOW_ID,
                WF_FLOW_VER,
                WF_NODE_ID,
                WF_DOC_SEQ,
                WF_DOC_ZH_TW,
                WF_DOC_ZH_CN,
                WF_DOC_EN_US,
                WF_DOC_TH_TH,
                WF_DOC_JA_JP,
                IS_REQ,
                REMARK,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;

            public DBChar WFDocSeq;

            public DBNVarChar WFDocZHTW;
            public DBNVarChar WFDocZHCN;
            public DBNVarChar WFDocENUS;
            public DBNVarChar WFDocTHTH;
            public DBNVarChar WFDocJAJP;
            public DBNVarChar WFDocKOKR;

            public DBChar IsReq;
            public DBNVarChar Remark;

            public DBVarChar UpdUserID;
        }

        public class SystemWFDoc : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
            public DBChar WFDocSeq;

            public DBNVarChar WFDocZHTW;
            public DBNVarChar WFDocZHCN;
            public DBNVarChar WFDocENUS;
            public DBNVarChar WFDocTHTH;
            public DBNVarChar WFDocJAJP;

            public DBChar IsReq;
            public DBNVarChar Remark;
        }

        public SystemWFDoc SelectSystemWFDoc(SystemWFDocPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT SYS_ID AS SysID ",
                        "     , WF_FLOW_ID AS WFFlowID ",
                        "     , WF_FLOW_VER AS WFFlowVer ",
                        "     , WF_NODE_ID AS WFNodeID ",
                        "     , WF_DOC_SEQ AS WFDocSeq ",
                        "     , WF_DOC_ZH_TW AS WFDocZHTW ",
                        "     , WF_DOC_ZH_CN AS WFDocZHCN ",
                        "     , WF_DOC_EN_US AS WFDocENUS ",
                        "     , WF_DOC_TH_TH AS WFDocTHTH ",
                        "     , WF_DOC_JA_JP AS WFDocJAJP ",
                        "     , IS_REQ AS IsReq ",
                        "     , REMARK AS Remark ",
                        "  FROM SYS_SYSTEM_WF_DOC ",
                        " WHERE SYS_ID = {SYS_ID} ",
                        "   AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "   AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "   AND WF_NODE_ID = {WF_NODE_ID} ",
                        "   AND WF_DOC_SEQ = {WF_DOC_SEQ} ",
                        Environment.NewLine
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_DOC_SEQ, Value = para.WFDocSeq });
            return GetEntityList<SystemWFDoc>(commandText, dbParameters).SingleOrDefault();
        }

        public enum EnumInsertSystemWFDocResult
        {
            Success,
            Failure
        }

        public EnumInsertSystemWFDocResult InsertSystemWFDoc(SystemWFDocPara para)
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

                        "        DECLARE @WF_DOC_SEQ CHAR(3); ",
                        "        SELECT @WF_DOC_SEQ = RIGHT('00' + CAST(ISNULL(CAST(MAX(WF_DOC_SEQ) AS INT), 0) + 1 AS VARCHAR), 3) ",
                        "          FROM SYS_SYSTEM_WF_DOC ",
                        "         WHERE SYS_ID = {SYS_ID} ",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "           AND WF_NODE_ID = {WF_NODE_ID}; ",

                        "        INSERT INTO SYS_SYSTEM_WF_DOC VALUES ( ",
                        "              {SYS_ID}, {WF_FLOW_ID}, {WF_FLOW_VER}, {WF_NODE_ID} ",
                        "            , @WF_DOC_SEQ ",
                        "            , {WF_DOC_ZH_TW}, {WF_DOC_ZH_CN}, {WF_DOC_EN_US}, {WF_DOC_TH_TH}, {WF_DOC_JA_JP} ",
                        "            , {IS_REQ}, {REMARK}, {UPD_USER_ID}, GETDATE() ",
                        "        ); ",

                        "        SET @RESULT = 'Y'; ",
                        "        COMMIT; ",
                        "    END TRY",
                        "    BEGIN CATCH ",
                        "        SET @RESULT = 'N';",
                        "        SET @ERROR_LINE = ERROR_LINE();",
                        "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                        "        ROLLBACK TRANSACTION; ",
                        "    END CATCH;",
                        "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_DOC_ZH_TW, Value = para.WFDocZHTW });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_DOC_ZH_CN, Value = para.WFDocZHCN });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_DOC_EN_US, Value = para.WFDocENUS });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_DOC_TH_TH, Value = para.WFDocTHTH });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_DOC_JA_JP, Value = para.WFDocJAJP });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.IS_REQ, Value = para.IsReq });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumInsertSystemWFDocResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }

        public enum EnumUpdateSystemWFDocResult
        {
            Success,
            Failure
        }

        public EnumUpdateSystemWFDocResult UpdateSystemWFDoc(SystemWFDocPara para)
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

                        "        DELETE FROM SYS_SYSTEM_WF_DOC ",
                        "         WHERE SYS_ID = {SYS_ID} ",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "           AND WF_NODE_ID = {WF_NODE_ID} ",
                        "           AND WF_DOC_SEQ = {WF_DOC_SEQ}",

                        "        INSERT INTO SYS_SYSTEM_WF_DOC VALUES ( ",
                        "            {SYS_ID}, {WF_FLOW_ID}, {WF_FLOW_VER}, {WF_NODE_ID} ",
                        "          , {WF_DOC_SEQ} ",
                        "          , {WF_DOC_ZH_TW}, {WF_DOC_ZH_CN}, {WF_DOC_EN_US}, {WF_DOC_TH_TH}, {WF_DOC_JA_JP} ",
                        "          , {IS_REQ}, {REMARK}, {UPD_USER_ID}, GETDATE() ",
                        "        ); ",

                        "        SET @RESULT = 'Y'; ",
                        "        COMMIT; ",
                        "    END TRY",
                        "    BEGIN CATCH ",
                        "        SET @RESULT = 'N';",
                        "        SET @ERROR_LINE = ERROR_LINE();",
                        "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                        "        ROLLBACK TRANSACTION; ",
                        "    END CATCH;",
                        "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_DOC_SEQ, Value = para.WFDocSeq });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_DOC_ZH_TW, Value = para.WFDocZHTW });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_DOC_ZH_CN, Value = para.WFDocZHCN });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_DOC_EN_US, Value = para.WFDocENUS });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_DOC_TH_TH, Value = para.WFDocTHTH });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_DOC_JA_JP, Value = para.WFDocJAJP });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.IS_REQ, Value = para.IsReq });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumUpdateSystemWFDocResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }

        public enum EnumDeleteSystemWFDocResult
        {
            Success,
            Failure,
            RuntimeExist
        }

        public EnumDeleteSystemWFDocResult DeleteSystemWFDoc(SystemWFDocPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT VARCHAR(50) = '" + EnumDeleteSystemWFDocResult.Failure + "';",
                        "DECLARE @ERROR_LINE INT;",
                        "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",

                        "IF NOT EXISTS (SELECT * ",
                        "                 FROM WF_FLOW ",
                        "                WHERE SYS_ID = {SYS_ID} ",
                        "                  AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "                  AND WF_FLOW_VER = {WF_FLOW_VER}) ",
                        "BEGIN ",
                        "    BEGIN TRANSACTION ",
                        "        BEGIN TRY ",
                        "            DELETE FROM SYS_SYSTEM_WF_DOC ",
                        "             WHERE SYS_ID = {SYS_ID} ",
                        "               AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "               AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "               AND WF_NODE_ID = {WF_NODE_ID} ",
                        "               AND WF_DOC_SEQ = {WF_DOC_SEQ}",

                        "            SET @RESULT = '" + EnumDeleteSystemWFDocResult.Success + "'; ",
                        "            COMMIT; ",
                        "        END TRY ",
                        "        BEGIN CATCH ",
                        "            SET @RESULT = '" + EnumDeleteSystemWFDocResult.Failure + "';",
                        "            SET @ERROR_LINE = ERROR_LINE();",
                        "            SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                        "            ROLLBACK TRANSACTION; ",
                        "        END CATCH;",
                        "END; ",
                        "ELSE ",
                        "BEGIN ",
                        "    SET @RESULT = '" + EnumDeleteSystemWFDocResult.RuntimeExist + "'; ",
                        "END; ",
                        "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFDocPara.ParaField.WF_DOC_SEQ, Value = para.WFDocSeq });
            
            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumDeleteSystemWFDocResult.Failure.ToString())
            {
                throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
            }

            return (EnumDeleteSystemWFDocResult)Enum.Parse(typeof(EnumDeleteSystemWFDocResult), result.Result.GetValue());
        }
    }
}