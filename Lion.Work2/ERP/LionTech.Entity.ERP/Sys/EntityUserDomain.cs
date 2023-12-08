using System;
using System.Collections.Generic;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityUserDomain : EntitySys
    {
        public EntityUserDomain(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserDomainPara : DBCulture
        {
            public UserDomainPara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                USER_ID,
                USER_NM,
                CODE_NM
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
        }

        public class UserDomain : DBTableRow
        {
            public enum DataField
            {
                USER_ID,
                USER_NM,
                USER_GROUP,
                USER_PLACE,
                USER_DEPT,
                USER_TEAM,
                DOMAIN_NAME,
                DOMAIN_ACCOUNT,
                DOMAIN_GROUP_ID,
                DOMAIN_GROUP_NM,
                UPD_USER_NM,
                UPD_DT
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBNVarChar UserGroup;
            public DBNVarChar UserPlace;
            public DBNVarChar UserDept;
            public DBNVarChar UserTeam;
            public DBVarChar UserEMailAccount;
        }

        public List<UserDomain> SelectUserDomainList(UserDomainPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT U.USER_ID AS UserID",
                "     , U.USER_NM AS UserNM",
                "     , C1.{CODE_NM} AS UserGroup",
                "     , C2.{CODE_NM} AS UserPlace",
                "     , C3.{CODE_NM} AS UserDept",
                "     , C4.{CODE_NM} AS UserTeam",
                "     , SUBSTRING(USER_EMAIL, 0, CHARINDEX('@', USER_EMAIL)) AS UserEMailAccount",
                "  FROM RAW_CM_USER U",
                "  LEFT JOIN RAW_CM_USER_ORG R",
                "    ON R.USER_ID = U.USER_ID",
                "  LEFT JOIN CM_CODE C1",
                "    ON C1.CODE_ID = R.USER_GROUP",
                "   AND C1.CODE_KIND = '0016'",
                "  LEFT JOIN CM_CODE C2",
                "    ON C2.CODE_ID = R.USER_PLACE",
                "   AND C2.CODE_KIND = '0017'",
                "  LEFT JOIN CM_CODE C3",
                "    ON C3.CODE_ID = R.USER_DEPT",
                "   AND C3.CODE_KIND = '0018'",
                "  LEFT JOIN CM_CODE C4",
                "    ON C4.CODE_ID = R.USER_TEAM",
                "   AND C4.CODE_KIND = '0019'",
                " WHERE U.IS_LEFT = '" + EnumYN.N + "'"
            }));

            if (para.UserID.IsNull() == false)
            {
                commandText.AppendLine("AND U.USER_ID = {USER_ID}");
                dbParameters.Add(new DBParameter { Name = UserDomainPara.ParaField.USER_ID, Value = para.UserID });
            }

            if (para.UserNM.IsNull() == false)
            {
                commandText.AppendLine("AND U.USER_NM = {USER_NM}");
                dbParameters.Add(new DBParameter { Name = UserDomainPara.ParaField.USER_NM, Value = para.UserNM });
            }

            dbParameters.Add(new DBParameter { Name = UserDomainPara.ParaField.CODE_NM, Value = para.GetCultureFieldNM(new DBObject(UserDomainPara.ParaField.CODE_NM.ToString())) });

            commandText.AppendLine("ORDER BY U.USER_ID");
            return GetEntityList<UserDomain>(commandText.ToString(), dbParameters);
        }
    }
}