using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityDomainGroupUser : EntitySys
    {
        public EntityDomainGroupUser(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class DomainGroupAccountPara: DBCulture
        {
            public DomainGroupAccountPara(string cultureID)
                : base(cultureID)
            {
            }
            public enum ParaField
            {
                USER_EMAIL,
                CODE_NM
            }
            
            public List<DBVarChar> UserEmail;
        }

        public class DomainGroupAccount : DBTableRow
        {
            public DBNVarChar UserNM;
            public DBNVarChar UserGroup;
            public DBNVarChar UserPlace;
            public DBNVarChar UserDept;
            public DBNVarChar UserTeam;
            public DBVarChar DomainAccount;
        }

        public List<DomainGroupAccount> SelectDomainGroupAccount(DomainGroupAccountPara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            List<string> commandWhere = new List<string>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT U.USER_NM AS UserNM",
                "     , U.USER_EMAIL AS DomainAccount",
                "     , U.USER_ID AS UserID",
                "     , C1.{CODE_NM} AS UserGroup ",
                "     , C2.{CODE_NM} AS UserPlace ",
                "     , C3.{CODE_NM} AS UserDept ",
                "     , C4.{CODE_NM} AS UserTeam ",
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
                "   AND C4.CODE_KIND = '0019'"
            }));

            if (para.UserEmail != null &&
                para.UserEmail.Any())
            {
                commandWhere.Add("U.USER_EMAIL IN ({USER_EMAIL})");
                dbParameters.Add(new DBParameter { Name = DomainGroupAccountPara.ParaField.USER_EMAIL, Value = para.UserEmail });
            }

            if (commandWhere.Any())
            {
                commandText.AppendLine(string.Format(" WHERE {0}", string.Join(" AND ", commandWhere)));
            }

            commandText.AppendLine("ORDER BY U.USER_ID");

            dbParameters.Add(new DBParameter { Name = DomainGroupAccountPara.ParaField.CODE_NM, Value = para.GetCultureFieldNM(new DBObject(DomainGroupAccountPara.ParaField.CODE_NM.ToString())) });
            return GetEntityList<DomainGroupAccount>(commandText.ToString(), dbParameters);
        }
    }
}
