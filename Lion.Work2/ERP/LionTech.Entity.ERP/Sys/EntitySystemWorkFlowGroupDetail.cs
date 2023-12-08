using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemWorkFlowGroupDetail : EntitySys
    {
        public EntitySystemWorkFlowGroupDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemWorkFlowGroupDetailPara
        {
            public enum ParaField
            {
                SYS_ID, WF_FLOW_GROUP_ID,
                WF_FLOW_GROUP_ZH_TW, WF_FLOW_GROUP_ZH_CN, WF_FLOW_GROUP_EN_US, WF_FLOW_GROUP_TH_TH, WF_FLOW_GROUP_JA_JP, WF_FLOW_GROUP_KO_KR,
                SORT_ORDER, UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowGroupID;

            public DBNVarChar WFFlowGroupZHTW;
            public DBNVarChar WFFlowGroupZHCN;
            public DBNVarChar WFFlowGroupENUS;
            public DBNVarChar WFFlowGroupTHTH;
            public DBNVarChar WFFlowGroupJAJP;
            public DBNVarChar WFFlowGroupKOKR;

            public DBVarChar SortOrder;
            public DBVarChar UpdUserID;
        }

        public class SystemWorkFlowGroupDetail : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar WFFlowGroupID;
            public DBNVarChar WFFlowGroupZHTW;
            public DBNVarChar WFFlowGroupZHCN;
            public DBNVarChar WFFlowGroupENUS;
            public DBNVarChar WFFlowGroupTHTH;
            public DBNVarChar WFFlowGroupJAJP;

            public DBVarChar SortOrder;
        }

        public SystemWorkFlowGroupDetail SelectSystemWorkFlowGroupDetail(SystemWorkFlowGroupDetailPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT SYS_ID AS SysID",
                        "     , WF_FLOW_GROUP_ID AS WFFlowGroupID",
                        "     , WF_FLOW_GROUP_ZH_TW AS WFFlowGroupZHTW",
                        "     , WF_FLOW_GROUP_ZH_CN AS WFFlowGroupZHCN",
                        "     , WF_FLOW_GROUP_EN_US AS WFFlowGroupENUS",
                        "     , WF_FLOW_GROUP_TH_TH AS WFFlowGroupTHTH",
                        "     , WF_FLOW_GROUP_JA_JP AS WFFlowGroupJAJP",
                        "     , SORT_ORDER AS SortOrder",
                        "  FROM SYS_SYSTEM_WF_FLOW_GROUP",
                        " WHERE SYS_ID = {SYS_ID}",
                        "   AND WF_FLOW_GROUP_ID = {WF_FLOW_GROUP_ID}"
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.WF_FLOW_GROUP_ID, Value = para.WFFlowGroupID });
            return GetEntityList<SystemWorkFlowGroupDetail>(commandText, dbParameters).SingleOrDefault();
        }

        public enum EnumEditSystemWorkFlowGroupDetailResult
        {
            Success, Failure
        }

        public EnumEditSystemWorkFlowGroupDetailResult EditSystemWorkFlowGroupDetail(SystemWorkFlowGroupDetailPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT CHAR(1) = 'N';",
                        "DECLARE @ERROR_LINE INT;",
                        "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",

                        "BEGIN TRANSACTION",
                        "    BEGIN TRY ",

                        "        DELETE FROM SYS_SYSTEM_WF_FLOW_GROUP",
                        "         WHERE SYS_ID = {SYS_ID}",
                        "           AND WF_FLOW_GROUP_ID = {WF_FLOW_GROUP_ID};",

                        "        INSERT INTO SYS_SYSTEM_WF_FLOW_GROUP VALUES (",
                        "               {SYS_ID}",
                        "             , {WF_FLOW_GROUP_ID}",
                        "             , {WF_FLOW_GROUP_ZH_TW}",
                        "             , {WF_FLOW_GROUP_ZH_CN}",
                        "             , {WF_FLOW_GROUP_EN_US}",
                        "             , {WF_FLOW_GROUP_TH_TH}",
                        "             , {WF_FLOW_GROUP_JA_JP}",
                        "             , {SORT_ORDER}",
                        "             , {UPD_USER_ID}",
                        "             , GETDATE()",
                        "        ); ",

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
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.WF_FLOW_GROUP_ID, Value = para.WFFlowGroupID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.WF_FLOW_GROUP_ZH_TW, Value = para.WFFlowGroupZHTW });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.WF_FLOW_GROUP_ZH_CN, Value = para.WFFlowGroupZHCN });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.WF_FLOW_GROUP_EN_US, Value = para.WFFlowGroupENUS });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.WF_FLOW_GROUP_TH_TH, Value = para.WFFlowGroupTHTH });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.WF_FLOW_GROUP_JA_JP, Value = para.WFFlowGroupJAJP });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditSystemWorkFlowGroupDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }

        #region - 查詢工作流程群組是否存在 -
        /// <summary>
        /// 查詢工作流程群組是否存在
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DBChar SelectSystemWorkFlowGroupExist(SystemWorkFlowGroupDetailPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "IF EXISTS (SELECT *",
                    "             FROM SYS_SYSTEM_WF_FLOW_GROUP",
                    "            WHERE SYS_ID = {SYS_ID}",
                    "              AND WF_FLOW_GROUP_ID = {WF_FLOW_GROUP_ID})",
                    "BEGIN ",
                    "    SET @RESULT = 'Y'; ",
                    "END; ",
                    "SELECT @RESULT;"
                }));

            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.WF_FLOW_GROUP_ID, Value = para.WFFlowGroupID });
            return new DBChar(base.ExecuteScalar(commandText.ToString(), dbParameters));
        }
        #endregion

        #region - 查詢工作流程是否存在 -
        /// <summary>
        /// 查詢工作流程是否存在
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DBChar SelectSystemWorkFlowExist(SystemWorkFlowGroupDetailPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "IF EXISTS (SELECT *",
                    "             FROM SYS_SYSTEM_WF_FLOW",
                    "            WHERE SYS_ID = {SYS_ID}",
                    "              AND WF_FLOW_GROUP_ID = {WF_FLOW_GROUP_ID})",
                    "BEGIN ",
                    "    SET @RESULT = 'Y'; ",
                    "END; ",
                    "SELECT @RESULT;"
                }));

            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.WF_FLOW_GROUP_ID, Value = para.WFFlowGroupID });
            return new DBChar(base.ExecuteScalar(commandText.ToString(), dbParameters));
        }
        #endregion

        public enum EnumDeleteSystemWorkFlowGroupDetailResult
        {
            Success, Failure
        }

        public EnumDeleteSystemWorkFlowGroupDetailResult DeleteSystemWorkFlowGroupDetail(SystemWorkFlowGroupDetailPara para)
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

                        "        DELETE FROM SYS_SYSTEM_WF_FLOW_GROUP",
                        "         WHERE SYS_ID = {SYS_ID}",
                        "           AND WF_FLOW_GROUP_ID = {WF_FLOW_GROUP_ID}; ",
                        
                        "        SET @RESULT = 'Y';",
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
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWorkFlowGroupDetailPara.ParaField.WF_FLOW_GROUP_ID, Value = para.WFFlowGroupID });
            
            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumDeleteSystemWorkFlowGroupDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
    }
}
