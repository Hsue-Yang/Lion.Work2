// 新增日期：2018-01-23
// 新增人員：廖先駿
// 新增內容：IP最後使用
// ---------------------------------------------------

using System;
using System.Collections.Generic;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityLatestUseIP : EntitySys
    {
        public EntityLatestUseIP(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class LatestUseIPInfoPara : DBCulture
        {
            public LatestUseIPInfoPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                IP_ADDRESS,
                CODE_NM
            }

            public DBVarChar IPAddress;
        }

        public class LatestUseIPInfo : DBTableRow
        {
            public DBVarChar IPAddress;
            public DBNVarChar UserIDNM;
            public DBNVarChar CompNM;
            public DBNVarChar UnitNM;
            public DBNVarChar JobNM;
            public DBVarChar CompTel;
            public DBVarChar CompTelExt;
        }

        public List<LatestUseIPInfo> SelectLatestUseIPInfoList(LatestUseIPInfoPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = string.Join(Environment.NewLine, new object[]
            {
                ";WITH LatestUseIP AS (",
                "	SELECT ROW_NUMBER() OVER (PARTITION BY IP_ADDRESS ORDER BY LAST_CONNECT_DT DESC) AS SEQ",
                "	     , IP_ADDRESS",
                "		 , USER_ID",
                "	  FROM SYS_USER_CONNECT",
                "	 WHERE IP_ADDRESS LIKE '%' + {IP_ADDRESS} + '%'",
                ")",
                "SELECT IP_ADDRESS AS IPAddress",
                "     , RCM.USER_ID + ' ' +USER_NM AS UserIDNM",
                "     , ORG.COM_NM AS CompNM",
                "     , UNIT.UNIT_NM AS UnitNM",
                "     , CM.{CODE_NM} AS JobNM",
                "     , UD.USER_TEL AS CompTel",
                "     , UD.USER_EXTENSION AS CompTelExt",
                "  FROM LatestUseIP SUC",
                "  JOIN RAW_CM_USER RCM",
                "    ON SUC.USER_ID = RCM.USER_ID",
                "  JOIN RAW_CM_ORG_COM ORG",
                "    ON ORG.COM_ID = USER_COM_ID",
                "  JOIN RAW_CM_ORG_UNIT UNIT",
                "    ON UNIT.UNIT_ID = USER_UNIT_ID",
                "  JOIN CM_CODEH CMH",
                "    ON CMH.CODE_KIND_NM_EN_US = 'JOB1'",
                "  LEFT JOIN CM_CODE CM",
                "    ON CM.CODE_KIND = CMH.CODE_KIND",
                "   AND CM.CODE_ID = USER_WORK_ID",
                "  JOIN SYS_USER_DETAIL UD",
                "    ON UD.USER_ID = RCM.USER_ID",
                " WHERE SUC.SEQ = 1"
            });

            dbParameters.Add(new DBParameter { Name = LatestUseIPInfoPara.ParaField.IP_ADDRESS, Value = para.IPAddress });
            dbParameters.Add(new DBParameter { Name = LatestUseIPInfoPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(LatestUseIPInfoPara.ParaField.CODE_NM.ToString())) });

            return GetEntityList<LatestUseIPInfo>(commandText, dbParameters);
        }
    }
}