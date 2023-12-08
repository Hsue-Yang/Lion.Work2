using System;
using System.Collections.Generic;

namespace LionTech.Entity.ERP.Pub
{
    public class EntityExternalSystem : EntityPub
    {
        public EntityExternalSystem(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class ExternalSystemPara : DBCulture
        {
            public ExternalSystemPara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                USER_ID, SYS_NM
            }

            public DBVarChar UserID;
        }

        public class ExternalSystem : DBTableRow
        {
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBNVarChar SysIconPath;
        }

        public List<ExternalSystem> SelectExternalSystemList(ExternalSystemPara para)
        {
            string commandText = string.Join(Environment.NewLine, new object[]
            {
                "SELECT S.SYS_ID AS SysID",
                "     , M.{SYS_NM} AS SysNM",
                "     , M.SYS_ICON_PATH AS SysIconPath",
                "  FROM SYS_USER_SYSTEM S ",
                "  JOIN SYS_SYSTEM_MAIN M ",
                "    ON S.SYS_ID=M.SYS_ID ",
                " WHERE S.USER_ID={USER_ID}",
                "   AND M.IS_DISABLE='N'",
                "   AND M.IS_OUTSOURCING='Y' ",
                " ORDER BY M.SORT_ORDER; ",
                Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = ExternalSystemPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = ExternalSystemPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(ExternalSystemPara.ParaField.SYS_NM.ToString())) });
            return GetEntityList<ExternalSystem>(commandText, dbParameters);
        }
    }
}