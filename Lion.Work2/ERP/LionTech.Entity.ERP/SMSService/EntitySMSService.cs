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
    public class EntitySMSService : Entity_BaseAP
    {
        public EntitySMSService(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserStatePara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }
        
        public DBChar ValidUserState(UserStatePara para)
        {
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "IF EXISTS(SELECT * FROM opagm20 WHERE stfn_stfn = {USER_ID} AND stfn_sts = 0)",
                "BEGIN",
                "    SELECT 'Y'",
                "END",
                "ELSE",
                "BEGIN",
                "    SELECT 'N'",
                "END"
            });
            
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserStatePara.ParaField.USER_ID.ToString(), Value = para.UserID });
            return new DBChar(ExecuteScalar(commandText, dbParameters));
        }

        #region - 查詢簡訊訊息 -
        public class SMSDetailPara
        {
            public enum ParaField
            {
                SMSYear, SMSSeq, PhoneNumber
            }

            public DBChar SMSYear;
            public DBInt SMSSeq;
            public DBChar PhoneNumber;
        }

        public class SMSDetail : DBTableRow
        {
            public DBChar State;
            public DBNVarChar Message;
            public DBVarChar SendUser;
            public DBVarChar ProjectName;
            public DBVarChar PhoneNumber;
            public DBChar OrderYear;
            public DBInt OrderNumber;
            public DBDateTime BookingDateTime;
        }

        /// <summary>
        /// 查詢簡訊訊息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public SMSDetail SelectSMSDetail(SMSDetailPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT sms00_sts AS State",
                    "     , sms00_desc AS Message",
                    "     , sms00_stfn AS SendUser",
                    "     , sms00_project AS ProjectName",
                    "     , sms00_number AS PhoneNumber",
                    "     , sms00_year AS OrderYear",
                    "     , sms00_ordr AS OrderNumber",
                    "     , sms00_booking_time AS BookingDateTime",
                    "  FROM smsm00",
                    " WHERE sms00_smsyear = {SMSYear}",
                    "   AND sms00_smsseq = {SMSSeq}"
                }));

            if (para.PhoneNumber != null)
            {
                commandText.AppendLine("   AND sms00_number = {PhoneNumber}");
                dbParameters.Add(new DBParameter { Name = SMSDetailPara.ParaField.PhoneNumber, Value = para.PhoneNumber });
            }

            dbParameters.Add(new DBParameter { Name = SMSDetailPara.ParaField.SMSYear, Value = para.SMSYear });
            dbParameters.Add(new DBParameter { Name = SMSDetailPara.ParaField.SMSSeq, Value = para.SMSSeq });
            
            return GetEntityList<SMSDetail>(commandText.ToString(), dbParameters).SingleOrDefault();
        }
        #endregion

    }
}