using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityLineBotReceiverDetail : EntitySys
    {
        public EntityLineBotReceiverDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 取得LineBot好友設定明細主檔 -
        public class LineBotReceiverDetailPara : DBCulture
        {
            public LineBotReceiverDetailPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                LINE_ID,
                LINE_NM,
                LINE_RECEIVER_ID,
                LINE_RECEIVER_NM_ZH_TW,
                LINE_RECEIVER_NM_ZH_CN,
                LINE_RECEIVER_NM_EN_US,
                LINE_RECEIVER_NM_TH_TH,
                LINE_RECEIVER_NM_JA_JP,
                RECEIVER_ID,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar LineID;
            public DBVarChar LineReceiverID;
            public DBNVarChar LineReceiverNMZHTW;
            public DBNVarChar LineReceiverNMZHCN;
            public DBNVarChar LineReceiverNMENUS;
            public DBNVarChar LineReceiverNMTHTH;
            public DBNVarChar LineReceiverNMJAJP;
            public DBVarChar ReceiverID;
            public DBVarChar UpdUserID;
        }

        public class LineBotReceiverDetail : DBTableRow
        {
            public DBNVarChar LineNMID;
            public DBVarChar LineReceiverID;
            public DBNVarChar LineReceiverNMZHTW;
            public DBNVarChar LineReceiverNMZHCN;
            public DBNVarChar LineReceiverNMENUS;
            public DBNVarChar LineReceiverNMTHTH;
            public DBNVarChar LineReceiverNMJAJP;
            public DBChar IsDisable;
            public DBVarChar SourceType;
        }

        public LineBotReceiverDetail SelectLineBotReceiverDetail(LineBotReceiverDetailPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT dbo.FN_GET_NMID(R.LINE_ID, S.{LINE_NM}) AS LineNMID",
                "     , R.LINE_RECEIVER_ID AS LineReceiverID",
                "     , R.LINE_RECEIVER_NM_ZH_TW AS LineReceiverNMZHTW",
                "     , R.LINE_RECEIVER_NM_ZH_CN AS LineReceiverNMZHCN",
                "     , R.LINE_RECEIVER_NM_EN_US AS LineReceiverNMENUS",
                "     , R.LINE_RECEIVER_NM_TH_TH AS LineReceiverNMTHTH",
                "     , R.LINE_RECEIVER_NM_JA_JP AS LineReceiverNMJAJP",
                "     , R.IS_DISABLE AS IsDisable",
                "     , R.SOURCE_TYPE AS SourceType",
                "  FROM SYS_SYSTEM_LINE_RECEIVER R",
                "  JOIN SYS_SYSTEM_LINE S",
                "    ON S.SYS_ID = R.SYS_ID",
                "   AND S.LINE_ID = R.LINE_ID",
                " WHERE R.SYS_ID = {SYS_ID}",
                "   AND R.LINE_ID = {LINE_ID}",
                "   AND R.RECEIVER_ID = {RECEIVER_ID}"
            }));

            dbParameters.Add(new DBParameter { Name = LineBotReceiverDetailPara.ParaField.LINE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(LineBotReceiverDetailPara.ParaField.LINE_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverDetailPara.ParaField.LINE_ID, Value = para.LineID });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverDetailPara.ParaField.RECEIVER_ID, Value = para.ReceiverID });

            return GetEntityList<LineBotReceiverDetail>(commandText.ToString(), dbParameters).SingleOrDefault();
        }
        #endregion

        #region - 編輯LineBot好友設定明細 -
        public enum EnumEditLineBotReceiverDetailResult
        {
            Success,
            Failure
        }

        public EnumEditLineBotReceiverDetailResult EditLineBotReceiverDetail(LineBotReceiverDetailPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1) = 'N';",
                "DECLARE @ERROR_LINE INT;",
                "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                "BEGIN TRANSACTION",
                "    BEGIN TRY",
                "        UPDATE SYS_SYSTEM_LINE_RECEIVER",
                "           SET LINE_RECEIVER_ID = {LINE_RECEIVER_ID}",
                "             , LINE_RECEIVER_NM_ZH_TW = {LINE_RECEIVER_NM_ZH_TW}",
                "             , LINE_RECEIVER_NM_ZH_CN = {LINE_RECEIVER_NM_ZH_CN}",
                "             , LINE_RECEIVER_NM_EN_US = {LINE_RECEIVER_NM_EN_US}",
                "             , LINE_RECEIVER_NM_TH_TH = {LINE_RECEIVER_NM_TH_TH}",
                "             , LINE_RECEIVER_NM_JA_JP = {LINE_RECEIVER_NM_JA_JP}",
                "             , UPD_USER_ID = {UPD_USER_ID}",
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

            dbParameters.Add(new DBParameter { Name = LineBotReceiverDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverDetailPara.ParaField.LINE_ID, Value = para.LineID });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverDetailPara.ParaField.RECEIVER_ID, Value = para.ReceiverID });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverDetailPara.ParaField.LINE_RECEIVER_ID, Value = para.LineReceiverID });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverDetailPara.ParaField.LINE_RECEIVER_NM_ZH_TW, Value = para.LineReceiverNMZHTW });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverDetailPara.ParaField.LINE_RECEIVER_NM_ZH_CN, Value = para.LineReceiverNMZHCN });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverDetailPara.ParaField.LINE_RECEIVER_NM_EN_US, Value = para.LineReceiverNMENUS });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverDetailPara.ParaField.LINE_RECEIVER_NM_TH_TH, Value = para.LineReceiverNMTHTH });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverDetailPara.ParaField.LINE_RECEIVER_NM_JA_JP, Value = para.LineReceiverNMJAJP });
            dbParameters.Add(new DBParameter { Name = LineBotReceiverDetailPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditLineBotReceiverDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion

        #region - 驗證Line好友代碼是否重複 -
        public class LineBotLineReceiverIDPara
        {
            public enum ParaField
            {
                SYS_ID,
                LINE_ID,
                LINE_RECEIVER_ID,
                RECEIVER_ID
            }

            public DBVarChar SysID;
            public DBVarChar LineID;
            public DBVarChar LineReceiverID;
            public DBVarChar ReceiverID;
        }

        public DBChar SelectLineBotLineReceiverID(LineBotLineReceiverIDPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "IF EXISTS (SELECT LINE_RECEIVER_ID",
                    "             FROM SYS_SYSTEM_LINE_RECEIVER",
                    "            WHERE SYS_ID = {SYS_ID}",
                    "              AND LINE_ID = {LINE_ID}",
                    "              AND LINE_RECEIVER_ID = {LINE_RECEIVER_ID}",
                    "              AND RECEIVER_ID <> {RECEIVER_ID})",
                    "BEGIN",
                    "    SET @RESULT = 'Y';",
                    "END",
                    "SELECT @RESULT;"
                }));

            dbParameters.Add(new DBParameter { Name = LineBotLineReceiverIDPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LineBotLineReceiverIDPara.ParaField.LINE_ID, Value = para.LineID });
            dbParameters.Add(new DBParameter { Name = LineBotLineReceiverIDPara.ParaField.LINE_RECEIVER_ID, Value = para.LineReceiverID });
            dbParameters.Add(new DBParameter { Name = LineBotLineReceiverIDPara.ParaField.RECEIVER_ID, Value = para.ReceiverID });

            return new DBChar(ExecuteScalar(commandText.ToString(), dbParameters));
        }
        #endregion
    }
}