// 新增日期：2016-12-16
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.LineBotService
{
    public class EntityLeave : EntityLineBotService
    {
        public EntityLeave(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public enum EnumLineBotLeaveResult
        {
            [Description("{}")]
            Success
        }

        #region - 取得Line接收者資訊 -
        public class LineReceiverInfoPara
        {
            public enum ParaField
            {
                SYS_ID,
                LINE_ID,
                RECEIVER_ID
            }

            public DBVarChar SysID;
            public DBVarChar LineID;
            public DBVarChar ReceiverID;
        }

        public class LineReceiverInfo : DBTableRow
        {
            public DBVarChar ReceiverID;
            public DBVarChar SourceType;
        }

        public LineReceiverInfo SelectLineReceiverInfo(LineReceiverInfoPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT RECEIVER_ID AS ReceiverID",
                    "     , SOURCE_TYPE AS SourceType",
                    "  FROM SYS_SYSTEM_LINE_RECEIVER",
                    " WHERE SYS_ID = {SYS_ID}",
                    "   AND LINE_ID = {LINE_ID}",
                    "   AND RECEIVER_ID = {RECEIVER_ID}",
                    "   AND SOURCE_TYPE <> '" + Entity_BaseAP.EnumLineSourceType.USER + "' ",
                    "   AND IS_DISABLE = 'N';"
                }));

            dbParameters.Add(new DBParameter { Name = LineReceiverInfoPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LineReceiverInfoPara.ParaField.LINE_ID, Value = para.LineID });
            dbParameters.Add(new DBParameter { Name = LineReceiverInfoPara.ParaField.RECEIVER_ID, Value = para.ReceiverID });

            return GetEntityList<LineReceiverInfo>(commandText.ToString(), dbParameters).SingleOrDefault();
        }
        #endregion

        #region - 更新Line好友設定明細 -
        public class LineBotReceiverPara
        {
            public enum ParaField
            {
                SYS_ID,
                LINE_ID,
                RECEIVER_ID
            }

            public DBVarChar SysID;
            public DBVarChar LineID;
            public DBVarChar ReceiverID;
        }

        public enum EnumUpdateLineBotReceiverResult
        {
            Success,
            Failure
        }

        public EnumUpdateLineBotReceiverResult UpdateLineBotReceiverDetail(LineBotReceiverPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "DECLARE @ERROR_LINE INT;",
                    "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                    "BEGIN TRANSACTION",
                    "    BEGIN TRY",
                    "        UPDATE SYS_SYSTEM_LINE_RECEIVER",
                    "           SET IS_DISABLE = '" + EnumYN.Y +"'",
                    "             , UPD_DT = GETDATE()",
                    "         WHERE SYS_ID = {SYS_ID}",
                    "           AND LINE_ID = {LINE_ID}",
                    "           AND RECEIVER_ID = {RECEIVER_ID}",
                    "        SET @RESULT = 'Y';",
                    "        COMMIT;",
                    "    END TRY",
                    "    BEGIN CATCH",
                    "        SET @RESULT = 'N';",
                    "        SET @ERROR_LINE = ERROR_LINE();",
                    "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                    "        ROLLBACK TRANSACTION;",
                    "    END CATCH;",
                    "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                }));

            dbParameters.Add(new DBParameter { Name = LineBotReceiverPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverPara.ParaField.LINE_ID, Value = para.LineID });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverPara.ParaField.RECEIVER_ID, Value = para.ReceiverID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumUpdateLineBotReceiverResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion
    }
}