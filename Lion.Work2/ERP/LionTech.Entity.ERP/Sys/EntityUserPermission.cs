using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityUserPermission : EntitySys
    {
        public EntityUserPermission(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class UserPermissionPara
        {
            public enum ParaField
            {
                USER_ID, USER_NM,
                RESTRICT_TYPE,
                CULTURE_ID
            }

            public DBVarChar UserID;
            public DBObject UserNM;
            public DBChar RestrictType;

            public DBVarChar CultureID;
        }

        public class UserPermission : DBTableRow
        {
            public enum DataField
            {
                USER_ID, USER_NM,
                UNIT_ID, UNIT_NM,
                RESTRICT_TYPE, RESTRICT_TYPE_NM,
                IS_LOCK,
                UPD_USER_NM, UPD_DT
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UnitID;
            public DBNVarChar UnitNM;
            public DBVarChar RestrictType;
            public DBNVarChar RestrictTypeNM;
            public DBChar IsLock;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<UserPermission> SelectUserPermissionList(UserPermissionPara para)
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

            if (!string.IsNullOrWhiteSpace(para.RestrictType.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                    {
                        commandWhere, (string.IsNullOrWhiteSpace(commandWhere) ? " WHERE " : " AND "), " M.RESTRICT_TYPE={RESTRICT_TYPE} ", Environment.NewLine
                    });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT U.USER_ID, dbo.FN_GET_IDNM(U.USER_ID, U.USER_NM) AS USER_NM ", Environment.NewLine,
                "     , N.UNIT_ID, dbo.FN_GET_IDNM(N.UNIT_ID, N.UNIT_NM) AS UNIT_NM ", Environment.NewLine,
                "     , M.RESTRICT_TYPE, dbo.FN_GET_NMID(M.RESTRICT_TYPE, dbo.FN_GET_CM_NM('0033',M.RESTRICT_TYPE,{CULTURE_ID})) AS RESTRICT_TYPE_NM ", Environment.NewLine,
                "     , M.IS_LOCK ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(M.UPD_USER_ID) AS UPD_USER_NM, M.UPD_DT ", Environment.NewLine,
                "FROM RAW_CM_USER U ", Environment.NewLine,
                "JOIN RAW_CM_ORG_UNIT N ON U.USER_UNIT_ID=N.UNIT_ID ", Environment.NewLine,
                "LEFT OUTER JOIN SYS_USER_MAIN M ON U.USER_ID=M.USER_ID ", Environment.NewLine,
                commandWhere, Environment.NewLine,
                "ORDER BY U.USER_ID ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = UserPermissionPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            if (!string.IsNullOrWhiteSpace(para.UserNM.GetValue()))
            {
                dbParameters.Add(new DBParameter { Name = UserPermissionPara.ParaField.USER_NM.ToString(), Value = para.UserNM });
            }
            dbParameters.Add(new DBParameter { Name = UserPermissionPara.ParaField.RESTRICT_TYPE.ToString(), Value = para.RestrictType });
            dbParameters.Add(new DBParameter { Name = UserPermissionPara.ParaField.CULTURE_ID, Value = para.CultureID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<UserPermission> userPermissionList = new List<UserPermission>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    UserPermission userPermission = new UserPermission()
                    {
                        UserID = new DBVarChar(dataRow[UserPermission.DataField.USER_ID.ToString()]),
                        UserNM = new DBNVarChar(dataRow[UserPermission.DataField.USER_NM.ToString()]),
                        UnitID = new DBVarChar(dataRow[UserPermission.DataField.UNIT_ID.ToString()]),
                        UnitNM = new DBNVarChar(dataRow[UserPermission.DataField.UNIT_NM.ToString()]),
                        RestrictType = new DBVarChar(dataRow[UserPermission.DataField.RESTRICT_TYPE.ToString()]),
                        RestrictTypeNM = new DBNVarChar(dataRow[UserPermission.DataField.RESTRICT_TYPE_NM.ToString()]),
                        IsLock = new DBChar(dataRow[UserPermission.DataField.IS_LOCK.ToString()]),
                        UpdUserNM = new DBNVarChar(dataRow[UserPermission.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[UserPermission.DataField.UPD_DT.ToString()])
                    };
                    userPermissionList.Add(userPermission);
                }
                return userPermissionList;
            }
            return null;
        }
    }
}