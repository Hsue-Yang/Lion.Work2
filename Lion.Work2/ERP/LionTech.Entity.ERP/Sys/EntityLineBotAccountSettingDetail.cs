using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityLineBotAccountSettingDetail : EntitySys
    {
        public EntityLineBotAccountSettingDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 取得LineBot好友設定明細主檔 -
        public class LineBotAccountSettingDetailPara : DBCulture
        {
            public LineBotAccountSettingDetailPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                LINE_ID,
                LINE_NM_ZH_TW,
                LINE_NM_ZH_CN,
                LINE_NM_EN_US,
                LINE_NM_TH_TH,
                LINE_NM_JA_JP,
                CHANNEL_ID,
                CHANNEL_SECRET,
                CHANNEL_ACCESS_TOKEN,
                IS_DISABLE,
                SORT_ORDER,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar LineID;
            public DBNVarChar LineNMZHTW;
            public DBNVarChar LineNMZHCN;
            public DBNVarChar LineNMENUS;
            public DBNVarChar LineNMTHTH;
            public DBNVarChar LineNMJAJP;
            public DBChar IsDisable;
            public DBVarChar SortOrder;
            public DBVarChar UpdUserID;
            public DBVarChar ChannelID;
            public DBVarChar ChannelSecret;
            public DBVarChar ChannelAccessToken;
        }

        public class LineBotAccountSettingDetail : DBTableRow
        {
            public DBNVarChar LineNMZHTW;
            public DBNVarChar LineNMZHCN;
            public DBNVarChar LineNMENUS;
            public DBNVarChar LineNMTHTH;
            public DBNVarChar LineNMJAJP;
            public DBChar IsDisable;
            public DBVarChar SortOrder;
            public DBVarChar ChannelID;
            public DBVarChar ChannelSecret;
            public DBVarChar ChannelAccessToken;
        }

        public LineBotAccountSettingDetail SelectLineBotAccountSettingDetail(LineBotAccountSettingDetailPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT SYS_ID AS SysID",
                    "     , LINE_ID AS LineID",
                    "     , LINE_NM_ZH_TW AS LineNMZHTW",
                    "     , LINE_NM_ZH_CN AS LineNMZHCN",
                    "     , LINE_NM_EN_US AS LineNMENUS",
                    "     , LINE_NM_TH_TH AS LineNMTHTH",
                    "     , LINE_NM_JA_JP AS LineNMJAJP",
                    "     , CHANNEL_ID AS ChannelID",
                    "     , CHANNEL_SECRET AS ChannelSecret",
                    "     , CHANNEL_ACCESS_TOKEN AS ChannelAccessToken",
                    "	  , IS_DISABLE AS IsDisable",
                    "	  , SORT_ORDER AS SortOrder",
                    "	  , UPD_USER_ID AS UpdUserID",
                    "	  , UPD_DT AS UpdDT",
                    "  FROM SYS_SYSTEM_LINE",
                    " WHERE SYS_ID = {SYS_ID}",
                    "   AND LINE_ID = {LINE_ID}"
                }));

            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.LINE_ID, Value = para.LineID });

            return GetEntityList<LineBotAccountSettingDetail>(commandText.ToString(), dbParameters).SingleOrDefault();
        }
        #endregion

        #region - 查詢是否有LineID -
        /// <summary>
        /// 查詢是否有LineID
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DBChar SelectHasLineID(LineBotAccountSettingDetailPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "IF EXISTS (SELECT *",
                    "             FROM SYS_SYSTEM_LINE",
                    "            WHERE SYS_ID = {SYS_ID}",
                    "              AND LINE_ID = {LINE_ID})",
                    "BEGIN",
                    "    SET @RESULT = 'Y';",
                    "END",
                    "SELECT @RESULT;"
                }));

            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.LINE_ID, Value = para.LineID });

            return new DBChar(ExecuteScalar(commandText.ToString(), dbParameters));
        }
        #endregion

        #region - 編輯LineBot好友設定明細 -
        public enum EnumEditLineBotAccountSettingDetailResult
        {
            Success,
            Failure
        }

        public EnumEditLineBotAccountSettingDetailResult EditLineBotAccountSettingDetail(LineBotAccountSettingDetailPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "DECLARE @ERROR_LINE INT;",
                    "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                    "BEGIN TRANSACTION",
                    "    BEGIN TRY",
                    "        DELETE FROM SYS_SYSTEM_LINE",
                    "         WHERE SYS_ID = {SYS_ID}",
                    "           AND LINE_ID = {LINE_ID};",

                    "        INSERT INTO SYS_SYSTEM_LINE",
                    "             ( SYS_ID",
                    "             , LINE_ID",
                    "             , LINE_NM_ZH_TW",
                    "             , LINE_NM_ZH_CN",
                    "             , LINE_NM_EN_US",
                    "             , LINE_NM_TH_TH",
                    "             , LINE_NM_JA_JP",
                    "             , CHANNEL_ID",
                    "             , CHANNEL_SECRET",
                    "             , CHANNEL_ACCESS_TOKEN",
                    "             , IS_DISABLE",
                    "             , SORT_ORDER",
                    "             , UPD_USER_ID",
                    "             , UPD_DT",
                    "             )",
                    "        SELECT {SYS_ID}",
                    "             , {LINE_ID}",
                    "             , {LINE_NM_ZH_TW}",
                    "             , {LINE_NM_ZH_CN}",
                    "             , {LINE_NM_EN_US}",
                    "             , {LINE_NM_TH_TH}",
                    "             , {LINE_NM_JA_JP}",
                    "             , {CHANNEL_ID}",
                    "             , {CHANNEL_SECRET}",
                    "             , {CHANNEL_ACCESS_TOKEN}",
                    "             , {IS_DISABLE}",
                    "             , {SORT_ORDER}",
                    "             , {UPD_USER_ID}",
                    "             , GETDATE();",
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

            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.LINE_ID, Value = para.LineID });
            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.LINE_NM_ZH_TW, Value = para.LineNMZHTW });
            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.LINE_NM_ZH_CN, Value = para.LineNMZHCN });
            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.LINE_NM_EN_US, Value = para.LineNMENUS });
            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.LINE_NM_TH_TH, Value = para.LineNMTHTH });
            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.LINE_NM_JA_JP, Value = para.LineNMJAJP });
            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.CHANNEL_ID, Value = para.ChannelID });
            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.CHANNEL_SECRET, Value = para.ChannelSecret });
            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.CHANNEL_ACCESS_TOKEN, Value = para.ChannelAccessToken });
            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.IS_DISABLE, Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = LineBotAccountSettingDetailPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditLineBotAccountSettingDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion

        #region - 刪除LineBot好友設定明細 -
        public class DeleteLineBotAccountSettingDetailPara
        {
            public enum ParaField
            {
                SYS_ID,
                LINE_ID
            }

            public DBVarChar SysID;
            public DBVarChar LineID;
        }

        public enum EnumDeleteLineBotAccountSettingResult
        {
            Success,
            Failure
        }

        public EnumDeleteLineBotAccountSettingResult DeleteLineBotAccountSettingDetail(DeleteLineBotAccountSettingDetailPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,new object[]
            {
                "DECLARE @RESULT CHAR(1) = 'N';",
                "DECLARE @ERROR_LINE INT;",
                "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                "  BEGIN TRANSACTION",
                "      BEGIN TRY",
                "          DELETE FROM SYS_SYSTEM_LINE",
                "           WHERE SYS_ID = {SYS_ID}",
                "             AND LINE_ID = {LINE_ID}",
                "             SET @RESULT = 'Y';",
                "             COMMIT;",
                "      END TRY",
                "      BEGIN CATCH",
                "          SET @RESULT = 'N';",
                "          SET @ERROR_LINE = ERROR_LINE();",
                "          SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                "          ROLLBACK TRANSACTION;",
                "      END CATCH;",
                "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
            }));

            dbParameters.Add(new DBParameter { Name = DeleteLineBotAccountSettingDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = DeleteLineBotAccountSettingDetailPara.ParaField.LINE_ID, Value = para.LineID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumDeleteLineBotAccountSettingResult.Success;
            }
            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError,result);
        }
        #endregion
    }
}