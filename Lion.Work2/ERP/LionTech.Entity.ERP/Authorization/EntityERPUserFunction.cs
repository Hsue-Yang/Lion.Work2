using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Authorization
{
    public class EntityERPUserFunction : EntityAuthorization
    {
        public EntityERPUserFunction(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserSystemPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public class UserSystem : DBTableRow
        {
            public enum DataField
            {
                SYS_ID
            }

            public DBVarChar SysID;
        }

        public List<UserSystem> SelectUserSystemList(UserSystemPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT U.SYS_ID ", Environment.NewLine,
                "FROM SYS_USER_SYSTEM U ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON U.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "WHERE U.USER_ID={USER_ID} AND M.IS_OUTSOURCING='N' ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSystemPara.ParaField.USER_ID.ToString(), Value = para.UserID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserSystem> userSystemList = new List<UserSystem>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserSystem userRole = new UserSystem()
                    {
                        SysID = new DBVarChar(dataRow[UserSystem.DataField.SYS_ID.ToString()])
                    };
                    userSystemList.Add(userRole);
                }
                return userSystemList;
            }
            return null;
        }
    }
}