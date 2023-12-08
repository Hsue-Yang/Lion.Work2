// 新增日期：2017-07-21
// 新增人員：王汶智
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.SMSService
{
    public class EntitySend : EntitySMSService
    {
        public EntitySend(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 編輯發送簡訊 -
        private enum EnumIsnoType
        {
            SMS
        }

        public enum EnumSMSSendResult
        {
            Success,
            Failure
        }

        public class SMSSendPara
        {
            public enum ParaField
            {
                SmsDesc,
                Stfn,
                Project,
                Number,
                BookingTime,
                OrdrYear,
                OrdrOrdr
            }

            public DBNVarChar SmsDesc;
            public DBVarChar Stfn;
            public DBNVarChar Project;
            public DBVarChar Number;
            public DBDateTime BookingTime;
            public DBChar OrdrYear;
            public DBInt OrdrOrdr;
        }

        public class SMSSend : ExecuteResult
        {
            public DBChar SMSYear;
            public DBInt SMSSeq;
        }

        /// <summary>
        /// 編輯發送簡訊
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public SMSSend EditSMSSend(SMSSendPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText =
                new StringBuilder(string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT CHAR(1) = 'N';",
                        "DECLARE @ERROR_LINE INT;",
                        "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                        "DECLARE @YEAR CHAR(4) = CAST(YEAR(GETDATE()) AS CHAR(4));",
                        "DECLARE @SEQ INT;",
                        "DECLARE @UNIT_ID CHAR(2);",
                        "DECLARE @RETRY_INDEX INT = 1;",

                        "SELECT @UNIT_ID = stfn_prof FROM opagm20 WHERE stfn_stfn = {Stfn}",

                        "BEGIN TRANSACTION",
                        "    BEGIN TRY",
                        "       SELECT @SEQ = isno_seq",
                        "         FROM isnom00",
                        "        WHERE isno_type = '" + EnumIsnoType.SMS + "'",
                        "          AND isno_year = @YEAR",

                        "       IF @SEQ IS NULL",
                        "       BEGIN",
                        "           INSERT dbo.isnom00 (isno_type, isno_year, isno_seq)",
                        "           SELECT '" + EnumIsnoType.SMS + "', @YEAR, 1",
                        "           SET @SEQ = 0;",
                        "       END",

                        "       WHILE @RETRY_INDEX < 10",
                        "       BEGIN",
                        "           SET @SEQ = @SEQ + 1;",

                        "           IF NOT EXISTS(SELECT *",
                        "                           FROM smsm00",
                        "                          WHERE sms00_smsyear = @YEAR",
                        "                            AND sms00_smsseq = @SEQ)",
                        "           BEGIN",
                        "               INSERT INTO dbo.smsm00 ( ",
                        "                      sms00_smsyear ",
                        "                    , sms00_smsseq ",
                        "                    , sms00_sts ",
                        "                    , sms00_desc ",
                        "                    , sms00_stfn ",
                        "                    , sms00_prof ",
                        "                    , sms00_qty ",
                        "                    , sms00_project ",
                        "                    , sms00_number ",
                        "                    , sms00_time ",
                        "                    , sms00_year ",
                        "                    , sms00_ordr ",
                        "                    , sms00_booking_time ",
                        "               ) VALUES ( ",
                        "                      @YEAR",
                        "                    , @Seq",
                        "                    , ''",
                        "                    , {SmsDesc}",
                        "                    , {Stfn}",
                        "                    , @UNIT_ID",
                        "                    , 0",
                        "                    , {Project}",
                        "                    , {Number}",
                        "                    , GETDATE()",
                        "                    , {OrdrYear}",
                        "                    , {OrdrOrdr}",
                        "                    , {BookingTime}",
                        "               )",

                        "               UPDATE isnom00",
                        "                  SET isno_seq = @SEQ",
                        "                WHERE isno_type = '" + EnumIsnoType.SMS + "'",
                        "                  AND isno_year = @YEAR",
                        "               BREAK;",
                        "           END",

                        "           SET @RETRY_INDEX = @RETRY_INDEX + 1;",
                        "       END",

                        "       IF @RETRY_INDEX = 10",
                        "       BEGIN",
                        "           ROLLBACK TRANSACTION;",
                        "           SET @RESULT = 'N';",
                        "       END",
                        "       ELSE",
                        "       BEGIN",
                        "           COMMIT;",
                        "           SET @RESULT = 'Y';",
                        "       END",						
                        
                        "    END TRY",
                        "    BEGIN CATCH",
                        "       SET @RESULT = 'N';",
                        "       SET @ERROR_LINE = ERROR_LINE();",
                        "       SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                        "       ROLLBACK TRANSACTION;",
                        "    END CATCH;",
                        "SELECT @YEAR AS [SMSYear], @Seq AS [SMSSeq], @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;",
                        Environment.NewLine
                    }));

            dbParameters.Add(new DBParameter { Name = SMSSendPara.ParaField.SmsDesc, Value = para.SmsDesc });
            dbParameters.Add(new DBParameter { Name = SMSSendPara.ParaField.Stfn, Value = para.Stfn });
            dbParameters.Add(new DBParameter { Name = SMSSendPara.ParaField.Project, Value = para.Project });
            dbParameters.Add(new DBParameter { Name = SMSSendPara.ParaField.Number, Value = para.Number });
            dbParameters.Add(new DBParameter { Name = SMSSendPara.ParaField.BookingTime, Value = para.BookingTime });
            dbParameters.Add(new DBParameter { Name = SMSSendPara.ParaField.OrdrYear, Value = para.OrdrYear });
            dbParameters.Add(new DBParameter { Name = SMSSendPara.ParaField.OrdrOrdr, Value = para.OrdrOrdr });

            var result = GetEntityList<SMSSend>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result.Result.GetValue() == EnumYN.N.ToString())
            {
                throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
            }

            return result;
        }
        #endregion
    }
}