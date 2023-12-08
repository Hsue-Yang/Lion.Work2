using System;
using System.Collections.Generic;
using System.Text;

namespace LionTech.Entity.ERP.LogERPUserTrace
{
    public class EntityLogERPUserTrace : Entity_BaseAP
    {
        public EntityLogERPUserTrace(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class TrustIPPara : DBCulture
        {
            public TrustIPPara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                IP_ADDRESS,
                CULTURE_ID
            }

            public DBVarChar IPAddress;
        }

        public class TrustIP : DBTableRow
        {
            public DBVarChar IPBegin;
            public DBVarChar IPEnd;

            public DBVarChar ComID;
            public DBNVarChar ComNM;

            public DBChar TrustStatus;
            public DBChar TrustType;
            public DBNVarChar TrustTypeNM;
            public DBChar SourceType;
            public DBNVarChar SourceTypeNM;

            public DBNVarChar Remark;
        }

        public List<TrustIP> SelectTrustIPList(TrustIPPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT T.IP_BEGIN AS IPBegin, T.IP_END AS IPEnd",
                    "     , T.COM_ID AS ComID, (CASE WHEN T.COM_ID IS NULL THEN NULL ELSE dbo.FN_GET_NMID(T.COM_ID, R.COM_NM) END) AS ComNM",
                    "     , T.TRUST_STATUS AS TrustStatus",
                    "     , T.TRUST_TYPE AS TrustType, dbo.FN_GET_NMID(T.TRUST_TYPE, dbo.FN_GET_CM_NM('0031',T.TRUST_TYPE,{CULTURE_ID})) AS TrustTypeNM",
                    "     , T.SOURCE_TYPE AS SourceType, dbo.FN_GET_NMID(T.SOURCE_TYPE, dbo.FN_GET_CM_NM('0032',T.SOURCE_TYPE,{CULTURE_ID})) AS SourceTypeNM",
                    "     , T.REMARK AS Remark",
                    "FROM SYS_TRUST_IP T",
                    "LEFT JOIN RAW_CM_ORG_COM R ON T.COM_ID=R.COM_ID",
                    "WHERE dbo.FN_GET_TRSUT_BYIP(IP_BEGIN, IP_END, {IP_ADDRESS})='Y';",
                    Environment.NewLine
                }));
            dbParameters.Add(new DBParameter { Name = TrustIPPara.ParaField.CULTURE_ID, Value = new DBVarChar(para.CultureID) });
            dbParameters.Add(new DBParameter { Name = TrustIPPara.ParaField.IP_ADDRESS.ToString(), Value = para.IPAddress });
            return GetEntityList<TrustIP>(commandText.ToString(), dbParameters);
        }
    }
}
