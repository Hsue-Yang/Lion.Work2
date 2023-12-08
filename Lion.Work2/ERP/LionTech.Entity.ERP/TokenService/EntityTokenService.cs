using System;
using System.Collections.Generic;

namespace LionTech.Entity.ERP.TokenService
{
    public class EntityTokenService : DBEntity
    {
#if !NET461
        public EntityTokenService(string connectionName)
            : base(connectionName)
        {
        }
#endif
        public EntityTokenService(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class TokenServicePara
        {
            public enum ParaField
            {
                IP_ADDRESS
            }

            public DBVarChar IPAddress;
        }

        public enum EnumValidateSystemIPResult
        {
            Success, Failure
        }

        public EnumValidateSystemIPResult ValidateERPAPSystemIP(TokenServicePara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT INT = 0; ", Environment.NewLine,
                "SELECT @RESULT=COUNT(1) ", Environment.NewLine,
                "FROM SYS_SYSTEM_IP ", Environment.NewLine,
                "WHERE SYS_ID='ERPAP' AND IP_ADDRESS={IP_ADDRESS}; ", Environment.NewLine,
                "SELECT @RESULT; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = TokenServicePara.ParaField.IP_ADDRESS.ToString(), Value = para.IPAddress });

            DBInt result = new DBInt(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() > 0) ? EnumValidateSystemIPResult.Success : EnumValidateSystemIPResult.Failure;
        }
    }
}