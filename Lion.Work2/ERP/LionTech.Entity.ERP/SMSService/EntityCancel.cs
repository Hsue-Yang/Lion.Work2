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
    public class EntityCancel : EntitySMSService
    {
        public EntityCancel(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SMSCancelPara
        {
            public enum ParaField
            {
                SMSYear,
                SMSSeq,
                UserID
            }

            public DBChar SMSYear;
            public DBInt SMSSeq;
            public DBVarChar UserID;
        }

        public enum EnumSMSCancelResult
        {
            Success,
            Failure
        }

        public EnumSMSCancelResult EditSMSCancel(SMSCancelPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText =
                new StringBuilder(string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT CHAR(1) = 'N';",
                        "DECLARE @ERROR_LINE INT;",
                        "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",

                        "BEGIN TRANSACTION",
                        "    BEGIN TRY",

                        "       UPDATE smsm00",
                        "          SET sms00_sts = 'V'",
                        "            , sms00_vstfn = {UserID}",
                        "            , sms00_vdate = GETDATE()",
                        "        WHERE sms00_smsyear = {SMSYear}",
                        "          AND sms00_smsseq = {SMSSeq};",

                        "       SET @RESULT = 'Y';",
                        "       COMMIT;",
                        "    END TRY",
                        "    BEGIN CATCH",
                        "       SET @RESULT = 'N';",
                        "       SET @ERROR_LINE = ERROR_LINE();",
                        "       SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                        "       ROLLBACK TRANSACTION;",
                        "    END CATCH;",
                        "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;",
                        Environment.NewLine
                    }));

            dbParameters.Add(new DBParameter { Name = SMSCancelPara.ParaField.SMSYear, Value = para.SMSYear });
            dbParameters.Add(new DBParameter { Name = SMSCancelPara.ParaField.SMSSeq, Value = para.SMSSeq });
            dbParameters.Add(new DBParameter { Name = SMSCancelPara.ParaField.UserID, Value = para.UserID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumSMSCancelResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
    }
}