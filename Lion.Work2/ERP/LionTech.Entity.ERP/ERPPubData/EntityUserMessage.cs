using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.ERPPubData
{
    public class EntityUserMessage : EntityERPPubData
    {
        public EntityUserMessage(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserMessagePara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public class UserMessageReturn : DBTableRow
        {
            public enum DataField
            {
                msg_message
            }

            public DBNVarChar UserMessage;
        }

        public List<UserMessageReturn> SelectUserMessageList(UserMessagePara para)
        {
            string commandText = string.Concat(new object[]
            {
                " SELECT TOP 5 msg_message,msg_prod,msg_ordr,msg_sys",
                " FROM MESSAGE NOLOCK",
                " WHERE msg_stfn={USER_ID} and msg_sts='0' ",
                " AND (msg_hdate <= CONVERT(VARCHAR(8),GETDATE(),112) OR msg_hdate IS NULL )",
                " AND msg_date < GETDATE() ORDER BY msg_date DESC"
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserMessagePara.ParaField.USER_ID.ToString(), Value = para.UserID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserMessageReturn> systemFunList = new List<UserMessageReturn>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserMessageReturn systemFun = new UserMessageReturn()
                    {
                        UserMessage = new DBNVarChar(dataRow[UserMessageReturn.DataField.msg_message.ToString()])
                    };
                    systemFunList.Add(systemFun);
                }
                return systemFunList;
            }
            return null;
        }
        
        #region - 新增使用者留言 -
        public class MessagePara
        {
            public enum ParaField
            {
                MSG_STFN,
                MSG_YEAR,
                MSG_ORDR,
                MSG_SYS,
                MSG_STS,
                MSG_RESPONSE,
                MSG_TIMEOUT,
                MSG_ORDRKIND,
                MSG_PROD,
                MSG_PRODCOMP,
                MSG_HDATE,
                MSG_MESSAGE,
                MSG_URL,
                MSG_MSTFN,
                MSG_MDATE_TIMEZONE,
                MSG_MDATE
            }

            public DBVarChar MsgStfn;
            public DBChar MsgYear;
            public DBInt MsgOrdr;
            public DBBit MsgSys;
            public DBBit MsgSts;
            public DBBit MsgResponse;
            public DBBit MsgTimeOut;
            public DBBit MsgOrdrKind;
            public DBVarChar MsgProd;
            public DBChar MsgProdcomp;
            public DBChar MsgHDate;
            public DBNVarChar MsgMessage;
            public DBVarChar MsgURL;
            public DBVarChar MsgMStfn;
            public DBVarChar MsgMDateTimeZone;
            public DBDateTime MsgMDate;
        }
        
        public enum EnumAddMessageResult
        {
            Success,
            Failure
        }

        public EnumAddMessageResult AddMessage(MessagePara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                " DECLARE @RESULT CHAR(1) = 'N';",
                " DECLARE @ERROR_LINE INT;",
                " DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                " BEGIN TRANSACTION",
                "     BEGIN TRY",
                "         INSERT INTO message",
                "              ( msg_date",
                "              , msg_stfn",
                "              , msg_year",
                "              , msg_ordr",
                "              , msg_sts",
                "              , msg_sys",
                "              , msg_timeout",
                "              , msg_ordrkind",
                "              , msg_prod",
                "              , msg_prodcomp",
                "              , msg_hdate",
                "              , msg_message",
                "              , msg_url",
                "              , msg_response",
                "              , msg_mstfn",
                "              , msg_mdate_timezone",
                "              , msg_mdate",
                "              , msg_fstfn",
                "              , msg_fdate",
                "              , msg_fsysdate",
                "              )",
                "         VALUES",
                "              ( GETDATE()",
                "              , {MSG_STFN}",
                "              , {MSG_YEAR}",
                "              , {MSG_ORDR}",
                "              , {MSG_STS}",
                "              , {MSG_SYS}",
                "              , {MSG_TIMEOUT}",
                "              , {MSG_ORDRKIND}",
                "              , {MSG_PROD}",
                "              , {MSG_PRODCOMP}",
                "              , {MSG_HDATE}",
                "              , {MSG_MESSAGE}",
                "              , {MSG_URL}",
                "              , {MSG_RESPONSE}",
                "              , {MSG_MSTFN}",
                "              , {MSG_MDATE_TIMEZONE}",
                "              , {MSG_MDATE}",
                "              , NULL",
                "              , NULL",
                "              , NULL",
                "              )",
                "       SET @RESULT = 'Y';",
                "       COMMIT;",
                "     END TRY",
                "     BEGIN CATCH",
                "         SET @RESULT = 'N';",
                "         SET @ERROR_LINE = ERROR_LINE();",
                "         SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                "         ROLLBACK TRANSACTION;",
                "     END CATCH;",
                " SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
            }));

            dbParameters.Add(new DBParameter { Name = MessagePara.ParaField.MSG_STFN, Value = para.MsgStfn });
            dbParameters.Add(new DBParameter { Name = MessagePara.ParaField.MSG_YEAR, Value = para.MsgYear });
            dbParameters.Add(new DBParameter { Name = MessagePara.ParaField.MSG_ORDR, Value = para.MsgOrdr });
            dbParameters.Add(new DBParameter { Name = MessagePara.ParaField.MSG_STS, Value = para.MsgSts });
            dbParameters.Add(new DBParameter { Name = MessagePara.ParaField.MSG_SYS, Value = para.MsgSys });
            dbParameters.Add(new DBParameter { Name = MessagePara.ParaField.MSG_TIMEOUT, Value = para.MsgTimeOut });
            dbParameters.Add(new DBParameter { Name = MessagePara.ParaField.MSG_ORDRKIND, Value = para.MsgOrdrKind });
            dbParameters.Add(new DBParameter { Name = MessagePara.ParaField.MSG_PROD, Value = para.MsgProd });
            dbParameters.Add(new DBParameter { Name = MessagePara.ParaField.MSG_PRODCOMP, Value = para.MsgProdcomp });
            dbParameters.Add(new DBParameter { Name = MessagePara.ParaField.MSG_HDATE, Value = para.MsgHDate });
            dbParameters.Add(new DBParameter { Name = MessagePara.ParaField.MSG_MESSAGE, Value = para.MsgMessage });
            dbParameters.Add(new DBParameter { Name = MessagePara.ParaField.MSG_URL, Value = para.MsgURL });
            dbParameters.Add(new DBParameter { Name = MessagePara.ParaField.MSG_RESPONSE, Value = para.MsgResponse });
            dbParameters.Add(new DBParameter { Name = MessagePara.ParaField.MSG_MSTFN, Value = para.MsgMStfn });
            dbParameters.Add(new DBParameter { Name = MessagePara.ParaField.MSG_MDATE_TIMEZONE, Value = para.MsgMDateTimeZone });
            dbParameters.Add(new DBParameter { Name = MessagePara.ParaField.MSG_MDATE, Value = para.MsgMDate });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumAddMessageResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion
    }
}
