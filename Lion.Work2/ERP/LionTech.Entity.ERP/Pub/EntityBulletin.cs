using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.ERP.Pub
{
    public class EntityBulletin : EntityPub
    {
        public EntityBulletin(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }
        
        public class UserLoginInfoPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public class UserLoginInfo : DBTableRow
        {
            public enum DataField
            {
                IS_DAILY_FIRST, LAST_LOGIN_DATE
            }

            public DBChar IsDailyFirst;
            public DBChar LastLoginDate;
        }

        public UserLoginInfo SelectUserLoginInfo(UserLoginInfoPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT IS_DAILY_FIRST, LAST_LOGIN_DATE ", Environment.NewLine,
                "FROM SYS_USER_MAIN ", Environment.NewLine,
                "WHERE USER_ID={USER_ID} ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserLoginInfoPara.ParaField.USER_ID.ToString(), Value = para.UserID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                UserLoginInfo userLoginInfo = new UserLoginInfo()
                {
                    IsDailyFirst = new DBChar(dataTable.Rows[0][UserLoginInfo.DataField.IS_DAILY_FIRST.ToString()]),
                    LastLoginDate = new DBChar(dataTable.Rows[0][UserLoginInfo.DataField.LAST_LOGIN_DATE.ToString()])
                };
                return userLoginInfo;
            }
            return null;
        }
    }
}