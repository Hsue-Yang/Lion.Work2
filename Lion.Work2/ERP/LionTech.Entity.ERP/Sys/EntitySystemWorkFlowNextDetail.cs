using System;
using System.Collections.Generic;
using System.Linq;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemWorkFlowNextDetail : EntitySys
    {
        public EntitySystemWorkFlowNextDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
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
                NODE_TYPE,
                WF_NODE
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBChar WFNodeID;

            public List<DBVarChar> NodeTypeList;
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

        public List<SystemWFNode> SelectSystemWFNodeList(SystemWFNodePara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT WF_NODE_ID AS WFNodeID ",
                        "     , dbo.FN_GET_NMID(WF_NODE_ID, {WF_NODE}) AS WFNodeNM ",
                        "  FROM SYS_SYSTEM_WF_NODE ",
                        " WHERE SYS_ID = {SYS_ID} ",
                        "   AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "   AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "   AND WF_NODE_ID <> {WF_NODE_ID} ",
                        "   AND NODE_TYPE IN ({NODE_TYPE}) ",
                        " ORDER BY SORT_ORDER "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_FLOW_ID.ToString(), Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_FLOW_VER.ToString(), Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.NODE_TYPE, Value = para.NodeTypeList });
            dbParameters.Add(new DBParameter { Name = SystemWFNodePara.ParaField.WF_NODE.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFNodePara.ParaField.WF_NODE.ToString())) });
            return GetEntityList<SystemWFNode>(commandText, dbParameters);
        }

        public class SystemWFNextPara : DBCulture
        {
            public SystemWFNextPara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                SYS_ID,
                WF_FLOW_ID,
                WF_FLOW_VER,
                WF_NODE_ID,
                NEXT_WF_NODE_ID,
                NEXT_RESULT_VALUE,
                SORT_ORDER,
                REMARK,
                UPD_USER_ID,
                WF_NODE
            }

            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
            public DBVarChar NextWFNodeID;

            public DBVarChar NextResultValue;
            public DBVarChar SortOrder;
            public DBNVarChar Remark;
            public DBVarChar UpdUserID;
        }

        public class SystemWFNext : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar WFFlowID;
            public DBChar WFFlowVer;
            public DBVarChar WFNodeID;
            public DBVarChar NextWFNodeID;

            public DBNVarChar NextWFNodeNM;
            public DBVarChar NextResultValue;
            public DBVarChar SortOrder;
            public DBNVarChar Remark;
        }

        public SystemWFNext SelectSystemWFNext(SystemWFNextPara para)
        {
            string commandText =
                string.Join(Environment.NewLine,
                    new object[]
                    {
                        "SELECT X.SYS_ID AS SysID ",
                        "     , X.WF_FLOW_ID AS WFFlowID ",
                        "     , X.WF_FLOW_VER AS WFFlowVer ",
                        "     , X.WF_NODE_ID AS WFNodeID ",
                        "     , X.NEXT_WF_NODE_ID AS NextWFNodeID ",
                        "     , dbo.FN_GET_NMID(X.NEXT_WF_NODE_ID, N.{WF_NODE}) AS NextWFNodeNM ",
                        "     , X.NEXT_RESULT_VALUE AS NextResultValue ",
                        "     , X.SORT_ORDER AS SortOrder ",
                        "     , X.REMARK AS Remark ",
                        "  FROM SYS_SYSTEM_WF_NEXT X ",
                        "  JOIN SYS_SYSTEM_WF_NODE N ",
                        "    ON X.SYS_ID = N.SYS_ID ",
                        "   AND X.WF_FLOW_ID = N.WF_FLOW_ID ",
                        "   AND X.WF_FLOW_VER = N.WF_FLOW_VER ",
                        "   AND X.NEXT_WF_NODE_ID = N.WF_NODE_ID ",
                        " WHERE X.SYS_ID = {SYS_ID} ",
                        "   AND X.WF_FLOW_ID = {WF_FLOW_ID} ",
                        "   AND X.WF_FLOW_VER = {WF_FLOW_VER} ",
                        "   AND X.WF_NODE_ID = {WF_NODE_ID} ",
                        "   AND X.NEXT_WF_NODE_ID = {NEXT_WF_NODE_ID} "
                    });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.NEXT_WF_NODE_ID, Value = para.NextWFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.WF_NODE.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemWFNextPara.ParaField.WF_NODE.ToString())) });
            return GetEntityList<SystemWFNext>(commandText, dbParameters).SingleOrDefault();
        }

        public enum EnumEditSystemWFNextResult
        {
            Success,
            Failure
        }

        public EnumEditSystemWFNextResult EditSystemWFNext(SystemWFNextPara para)
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

                        "        IF (SELECT NODE_TYPE ",
                        "            FROM SYS_SYSTEM_WF_NODE ",
                        "            WHERE SYS_ID = {SYS_ID} ",
                        "              AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "              AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "              AND WF_NODE_ID = {WF_NODE_ID}) = 'D' ",
                        "        BEGIN ",
                        "            DELETE FROM SYS_SYSTEM_WF_NEXT ",
                        "             WHERE SYS_ID = {SYS_ID} ",
                        "               AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "               AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "               AND WF_NODE_ID = {WF_NODE_ID} ",
                        "               AND NEXT_WF_NODE_ID = {NEXT_WF_NODE_ID}; ",
                        "        END; ",
                        "        ELSE ",
                        "        BEGIN ",

                        #region - P，次節點檔只可有一筆資料 -
                        "            DELETE FROM SYS_SYSTEM_WF_NEXT ",
                        "             WHERE SYS_ID = {SYS_ID} ",
                        "               AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "               AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "               AND WF_NODE_ID = {WF_NODE_ID}; ",
                        #endregion

                        "        END; ",

                        "        INSERT INTO SYS_SYSTEM_WF_NEXT VALUES ( ",
                        "            {SYS_ID} ",
                        "          , {WF_FLOW_ID} ",
                        "          , {WF_FLOW_VER} ",
                        "          , {WF_NODE_ID}, {NEXT_WF_NODE_ID}, {NEXT_RESULT_VALUE} ",
                        "          , {SORT_ORDER} ",
                        "          , {REMARK} ",
                        "          , {UPD_USER_ID}, GETDATE() ",
                        "        ); ",

                        "        UPDATE SYS_SYSTEM_WF_NODE ",
                        "           SET IS_FIRST = 'N'",
                        "             , IS_FINALLY = 'N' ",
                        "         WHERE SYS_ID = {SYS_ID} ",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER}; ",

                        "        UPDATE SYS_SYSTEM_WF_NODE ",
                        "           SET IS_FIRST = 'Y' ",
                        "         WHERE SYS_ID = {SYS_ID} ",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "           AND WF_NODE_ID IN (SELECT NEXT_WF_NODE_ID ",
                        "                                FROM SYS_SYSTEM_WF_NEXT ",
                        "                               WHERE SYS_ID = {SYS_ID} ",
                        "                                 AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "                                 AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "                                 AND WF_NODE_ID = 'START'); ",

                        "        UPDATE SYS_SYSTEM_WF_NODE ",
                        "           SET IS_FINALLY = 'Y' ",
                        "         WHERE SYS_ID = {SYS_ID} ",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "           AND WF_NODE_ID IN (SELECT WF_NODE_ID ",
                        "                                FROM SYS_SYSTEM_WF_NEXT ",
                        "                               WHERE SYS_ID = {SYS_ID} ",
                        "                                 AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "                                 AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "                                 AND NEXT_WF_NODE_ID = 'END'); ",

                        "        SET @RESULT  =  'Y'; ",
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
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.NEXT_WF_NODE_ID, Value = para.NextWFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.NEXT_RESULT_VALUE, Value = para.NextResultValue });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditSystemWFNextResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        
        public enum EnumDeleteSystemWFNextResult
        {
            Success,
            Failure
        }

        public EnumDeleteSystemWFNextResult DeleteSystemWFNext(SystemWFNextPara para)
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
                        
                        "        DELETE FROM SYS_SYSTEM_WF_NEXT ",
                        "         WHERE SYS_ID = {SYS_ID} ",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "           AND WF_NODE_ID = {WF_NODE_ID} ",
                        "           AND NEXT_WF_NODE_ID = {NEXT_WF_NODE_ID}; ",
                                 
                        "        DELETE FROM SYS_SYSTEM_WF_ARROW ",
                        "         WHERE SYS_ID = {SYS_ID} ",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "           AND WF_NODE_ID = {WF_NODE_ID} ",
                        "           AND NEXT_WF_NODE_ID = {NEXT_WF_NODE_ID}; ",
                                 
                        "        UPDATE SYS_SYSTEM_WF_NODE ",
                        "           SET IS_FIRST = 'N'",
                        "             , IS_FINALLY = 'N' ",
                        "         WHERE SYS_ID = {SYS_ID} ",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER}; ",
                                 
                        "        UPDATE SYS_SYSTEM_WF_NODE ",
                        "           SET IS_FIRST = 'Y' ",
                        "         WHERE SYS_ID = {SYS_ID} ",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "           AND WF_NODE_ID IN (SELECT NEXT_WF_NODE_ID ",
                        "                                FROM SYS_SYSTEM_WF_NEXT ",
                        "                               WHERE SYS_ID = {SYS_ID} ",
                        "                                 AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "                                 AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "                                 AND WF_NODE_ID = 'START'); ",
                                 
                        "        UPDATE SYS_SYSTEM_WF_NODE ",
                        "           SET IS_FINALLY = 'Y' ",
                        "         WHERE SYS_ID = {SYS_ID} ",
                        "           AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "           AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "           AND WF_NODE_ID IN (SELECT WF_NODE_ID ",
                        "                                FROM SYS_SYSTEM_WF_NEXT ",
                        "                               WHERE SYS_ID = {SYS_ID} ",
                        "                                 AND WF_FLOW_ID = {WF_FLOW_ID} ",
                        "                                 AND WF_FLOW_VER = {WF_FLOW_VER} ",
                        "                                 AND NEXT_WF_NODE_ID = 'END'); ",
                                 
                        "        SET @RESULT  =  'Y';",
                        "        COMMIT;",

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
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.WF_FLOW_ID, Value = para.WFFlowID });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.WF_FLOW_VER, Value = para.WFFlowVer });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.WF_NODE_ID, Value = para.WFNodeID });
            dbParameters.Add(new DBParameter { Name = SystemWFNextPara.ParaField.NEXT_WF_NODE_ID, Value = para.NextWFNodeID });

            var result = GetEntityList<ExecuteResult>(commandText, dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumDeleteSystemWFNextResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
    }
}