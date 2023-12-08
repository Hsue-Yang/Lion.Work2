using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.LineBotService
{
    public class EntityWebhooks : EntityLineBotService
    {
        public EntityWebhooks(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 查詢應用系統Line清單 -
        public class SystemLinePara: DBCulture
        {
            public SystemLinePara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                SYS_NM
            }
        }

        public class SystemLine : DBTableRow
        {
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar LineID;
            public DBNVarChar LineNM;
            public DBVarChar ChannelSecret;
            public DBVarChar ChannelAccessToken;
        }

        /// <summary>
        /// 查詢應用系統Line清單
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<SystemLine> SelectSystemLineList(SystemLinePara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT L.SYS_ID AS SysID",
                    "     , M.{SYS_NM} AS SysNM ",
                    "     , L.LINE_ID AS LineID",
                    "     , L.LINE_NM_ZH_TW AS LineNM",
                    "     , L.CHANNEL_SECRET AS ChannelSecret",
                    "     , L.CHANNEL_ACCESS_TOKEN AS ChannelAccessToken",
                    "  FROM SYS_SYSTEM_LINE L",
                    "  JOIN SYS_SYSTEM_MAIN M",
                    "    ON M.SYS_ID = L.SYS_ID",
                    " WHERE L.IS_DISABLE = 'N';"
                }));
            dbParameters.Add(new DBParameter { Name = SystemLinePara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemLinePara.ParaField.SYS_NM.ToString())) });
            return GetEntityList<SystemLine>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 編輯應用系統Line接收者 -
        public enum EnumSystemLineReceiverResult
        {
            Success,
            Failure
        }

        public class SystemLineReceiverPara
        {
            public enum ParaField
            {
                SYS_ID,
                LINE_ID,
                IS_DISABLE,
                RECEIVER_ID,
                SOURCE_TYPE,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar LineID;
            public DBChar IsDisable;
            public DBVarChar ReceiverID;
            public DBVarChar SourceType;
            public DBVarChar UpdUserID;
        }

        /// <summary>
        /// 編輯應用系統Line接收者
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public EnumSystemLineReceiverResult EditSystemLineReceiver(SystemLineReceiverPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText =
                new StringBuilder(string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT CHAR(1) = 'N';",
                        "DECLARE @ERROR_LINE INT;",
                        "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                        Environment.NewLine
                    }));

            commandText.AppendLine(string.Join(Environment.NewLine,
                new object[]
                {
                    "BEGIN TRANSACTION",
                    "    BEGIN TRY",

                    "        IF NOT EXISTS(SELECT *",
                    "                        FROM SYS_SYSTEM_LINE_RECEIVER ",
                    "                       WHERE SYS_ID = {SYS_ID}",
                    "                         AND LINE_ID = {LINE_ID}",
                    "                         AND RECEIVER_ID = {RECEIVER_ID}",
                    "                     )",
                    "        BEGIN",
                    "            INSERT INTO dbo.SYS_SYSTEM_LINE_RECEIVER (",
                    "                   SYS_ID",
                    "                 , LINE_ID",
                    "                 , IS_DISABLE",
                    "                 , RECEIVER_ID",
                    "                 , SOURCE_TYPE",
                    "                 , UPD_USER_ID",
                    "                 , UPD_DT",
                    "            ) VALUES (",
                    "                   {SYS_ID}",
                    "                 , {LINE_ID}",
                    "                 , 'N'",
                    "                 , {RECEIVER_ID}",
                    "                 , {SOURCE_TYPE}",
                    "                 , {UPD_USER_ID}",
                    "                 , GETDATE()",
                    "            )",
                    "        END",
                    "        ELSE",
                    "        BEGIN",
                    "            UPDATE dbo.SYS_SYSTEM_LINE_RECEIVER",
                    "               SET IS_DISABLE = {IS_DISABLE}",
                    "                 , UPD_USER_ID = {UPD_USER_ID}",
                    "                 , UPD_DT = GETDATE()",
                    "             WHERE SYS_ID = {SYS_ID}",
                    "               AND LINE_ID = {LINE_ID}",
                    "               AND RECEIVER_ID = {RECEIVER_ID}",
                    "        END",

                    "       SET @RESULT = 'Y';",
                    "       COMMIT;",
                    "    END TRY",
                    "    BEGIN CATCH",
                    "       SET @RESULT = 'N';",
                    "       SET @ERROR_LINE = ERROR_LINE();",
                    "       SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                    "       ROLLBACK TRANSACTION;",
                    "    END CATCH;",
                    "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                }));

            dbParameters.Add(new DBParameter { Name = SystemLineReceiverPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemLineReceiverPara.ParaField.LINE_ID, Value = para.LineID });
            dbParameters.Add(new DBParameter { Name = SystemLineReceiverPara.ParaField.IS_DISABLE, Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = SystemLineReceiverPara.ParaField.RECEIVER_ID, Value = para.ReceiverID });
            dbParameters.Add(new DBParameter { Name = SystemLineReceiverPara.ParaField.SOURCE_TYPE, Value = para.SourceType });
            dbParameters.Add(new DBParameter { Name = SystemLineReceiverPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumSystemLineReceiverResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion
    }

    public class MongoWebhooks : Mongo_BaseAP
    {
        public MongoWebhooks(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class LangMessagePara
        {
            public DBVarChar SysID;
            public DBVarChar LineID;
            public DBNVarChar Keyword;
            public DBNVarChar Value;
        }

        public class LangMessage : MongoDocument
        {
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;

            [DBTypeProperty("LINE_ID")]
            public DBVarChar LineID;

            [DBTypeProperty("KEYWORD")]
            public DBNVarChar Keyword;

            [DBTypeProperty("VALUE")]
            public DBNVarChar Value;
        }

        public void EditLangMessage(LangMessagePara para)
        {
            DBParameters dbParameters = new DBParameters();
            MongoCommand command = new MongoCommand("SYS_SYSTEM_LINE_REPLY");
            command.AddFields(EnumSpecifiedFieldType.Select, "KEYWORD");
            command.SetRowCount(1);
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, "SYS_ID");
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, "LINE_ID");
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, "KEYWORD");
            dbParameters.Add(new DBParameter { Name = "SYS_ID", Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = "LINE_ID", Value = para.LineID });
            dbParameters.Add(new DBParameter { Name = "KEYWORD", Value = para.Keyword });
            var langMessage = Select<LangMessage>(command, dbParameters).SingleOrDefault();

            if (langMessage != null)
            {
                Delete(command, langMessage);
            }

            langMessage = new LangMessage();
            langMessage.SysID = para.SysID;
            langMessage.LineID = para.LineID;
            langMessage.Keyword = para.Keyword;
            langMessage.Value = para.Value;
            Insert(command, langMessage);
        }

        public void DeleteLangMessage(LangMessagePara para)
        {
            DBParameters dbParameters = new DBParameters();
            MongoCommand command = new MongoCommand("SYS_SYSTEM_LINE_REPLY");
            command.AddFields(EnumSpecifiedFieldType.Select, "KEYWORD");
            command.SetRowCount(1);
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, "SYS_ID");
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, "LINE_ID");
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, "KEYWORD");
            dbParameters.Add(new DBParameter { Name = "SYS_ID", Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = "LINE_ID", Value = para.LineID });
            dbParameters.Add(new DBParameter { Name = "KEYWORD", Value = para.Keyword });
            var langMessage = Select<LangMessage>(command, dbParameters).SingleOrDefault();

            if (langMessage != null)
            {
                Delete(command, langMessage);
            }
        }

        public LangMessage SelectReplyMessage(LangMessagePara para)
        {
            DBParameters dbParameters = new DBParameters();
            MongoCommand command = new MongoCommand("SYS_SYSTEM_LINE_REPLY");
            command.AddFields(EnumSpecifiedFieldType.Select, "VALUE");
            command.SetRowCount(1);
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, "SYS_ID");
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, "LINE_ID");
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, "KEYWORD");
            dbParameters.Add(new DBParameter { Name = "SYS_ID", Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = "LINE_ID", Value = para.LineID });
            dbParameters.Add(new DBParameter { Name = "KEYWORD", Value = para.Keyword });
            return Select<LangMessage>(command, dbParameters).SingleOrDefault();
        }
    }
}