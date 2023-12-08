using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityUserRoleFun : EntitySys
    {
        public EntityUserRoleFun(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserRoleFunPara
        {
            public enum ParaField
            {
                USER_ID, USER_NM
            }

            public DBVarChar UserID;
            public DBObject UserNM;
        }

        public class UserRoleFun : DBTableRow
        {
            public enum DataField
            {
                USER_ID, USER_NM, UNIT_ID, UNIT_NM, IS_LEFT, IS_DISABLE, UPD_USER_NM, UPD_DT
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UnitID;
            public DBNVarChar UnitNM;
            public DBChar IsLeft;
            public DBChar IsDisable;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<UserRoleFun> SelectUserRoleFunList(UserRoleFunPara para)
        {
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.UserID.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                    {
                        commandWhere, (string.IsNullOrWhiteSpace(commandWhere) ? " WHERE " : " AND "), " U.USER_ID={USER_ID} ", Environment.NewLine
                    });
            }

            if (!string.IsNullOrWhiteSpace(para.UserNM.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                    {
                        commandWhere, (string.IsNullOrWhiteSpace(commandWhere) ? " WHERE " : " AND "), " U.USER_NM LIKE N'%{USER_NM}%' ", Environment.NewLine
                    });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT U.USER_ID, dbo.FN_GET_IDNM(U.USER_ID, U.USER_NM) AS USER_NM ", Environment.NewLine,
                "     , N.UNIT_ID, dbo.FN_GET_IDNM(N.UNIT_ID, N.UNIT_NM) AS UNIT_NM ", Environment.NewLine,
                "     , U.IS_LEFT ", Environment.NewLine,
                "     , (CASE WHEN M.IS_DISABLE IS NULL THEN 'Y' ELSE M.IS_DISABLE END) AS IS_DISABLE ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(M.UPD_USER_ID) AS UPD_USER_NM, M.UPD_DT ", Environment.NewLine,
                "FROM RAW_CM_USER U ", Environment.NewLine,
                "JOIN RAW_CM_ORG_UNIT N ON U.USER_UNIT_ID=N.UNIT_ID ", Environment.NewLine,
                "LEFT OUTER JOIN SYS_USER_MAIN M ON U.USER_ID=M.USER_ID ", Environment.NewLine,
                commandWhere, Environment.NewLine,
                "ORDER BY U.USER_ID ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserRoleFunPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            if (!string.IsNullOrWhiteSpace(para.UserNM.GetValue()))
            {
                dbParameters.Add(new DBParameter { Name = UserRoleFunPara.ParaField.USER_NM.ToString(), Value = para.UserNM });
            }

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserRoleFun> userRoleFunList = new List<UserRoleFun>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserRoleFun userRoleFun = new UserRoleFun()
                    {
                        UserID = new DBVarChar(dataRow[UserRoleFun.DataField.USER_ID.ToString()]),
                        UserNM = new DBNVarChar(dataRow[UserRoleFun.DataField.USER_NM.ToString()]),
                        UnitID = new DBVarChar(dataRow[UserRoleFun.DataField.UNIT_ID.ToString()]),
                        UnitNM = new DBNVarChar(dataRow[UserRoleFun.DataField.UNIT_NM.ToString()]),
                        IsLeft = new DBChar(dataRow[UserRoleFun.DataField.IS_LEFT.ToString()]),
                        IsDisable = new DBChar(dataRow[UserRoleFun.DataField.IS_DISABLE.ToString()]),
                        UpdUserNM = new DBNVarChar(dataRow[UserRoleFun.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[UserRoleFun.DataField.UPD_DT.ToString()]),
                    };
                    userRoleFunList.Add(userRoleFun);
                }
                return userRoleFunList;
            }
            return null;
        }
    }
}