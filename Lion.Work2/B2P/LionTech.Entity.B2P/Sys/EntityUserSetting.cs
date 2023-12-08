using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntityUserSetting : EntitySys
    {
        public EntityUserSetting(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserSettingPara : DBCulture
        {
            public UserSettingPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                USER_ID, QUERY_USER_ID, QUERY_USER_NM
            }

            public DBVarChar UserID;
            public DBVarChar QueryUserID;
            public DBObject QueryUserNM;
        }

        public class UserSetting : DBTableRow
        {
            public enum DataField
            {
                USER_ID, USER_NM, IS_DISABLE,
                UPD_USER_NM, UPD_DT
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBChar IsDisable;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<UserSetting> SelectUserSettingList(UserSettingPara para)
        {
            string commandWhere = string.Empty;

            if (para.QueryUserID != null && !string.IsNullOrWhiteSpace(para.QueryUserID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND M.USER_ID={QUERY_USER_ID} ", Environment.NewLine });
            }

            if (para.QueryUserNM != null && !string.IsNullOrWhiteSpace(para.QueryUserNM.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND D.USER_NM LIKE '%{QUERY_USER_NM}%' ", Environment.NewLine });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT M.USER_ID, dbo.FN_GET_NMID(M.USER_ID, D.USER_NM) AS USER_NM ", Environment.NewLine,
                "     , M.IS_DISABLE ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(M.UPD_USER_ID) AS UPD_USER_NM, M.UPD_DT ", Environment.NewLine,
                "FROM SYS_USER_MAIN M ", Environment.NewLine,
                "JOIN SYS_USER_DETAIL D ON M.USER_ID = D.USER_ID ", Environment.NewLine,
                "WHERE (M.USER_ID={USER_ID} OR (D.GRANTOR_USER_ID={USER_ID} AND ISNULL((SELECT IS_GRANTOR FROM SYS_USER_DETAIL WHERE USER_ID={USER_ID}),'N')='Y')) ", Environment.NewLine,
                commandWhere,
                "ORDER BY M.USER_ID "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserSettingPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSettingPara.ParaField.QUERY_USER_ID.ToString(), Value = para.QueryUserID });
            if (!para.QueryUserNM.IsNull())
            {
                dbParameters.Add(new DBParameter { Name = UserSettingPara.ParaField.QUERY_USER_NM.ToString(), Value = para.QueryUserNM });
            }

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserSetting> userSettingList = new List<UserSetting>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserSetting userSetting = new UserSetting()
                    {
                        UserID = new DBVarChar(dataRow[UserSetting.DataField.USER_ID.ToString()]),
                        UserNM = new DBNVarChar(dataRow[UserSetting.DataField.USER_NM.ToString()]),
                        IsDisable = new DBChar(dataRow[UserSetting.DataField.IS_DISABLE.ToString()]),
                        UpdUserNM = new DBNVarChar(dataRow[UserSetting.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[UserSetting.DataField.UPD_DT.ToString()])
                    };
                    userSettingList.Add(userSetting);
                }
                return userSettingList;
            }
            return null;
        }
    }
}