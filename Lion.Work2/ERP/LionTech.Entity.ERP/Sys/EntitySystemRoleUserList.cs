using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemRoleUserList : EntitySys
    {
        public EntitySystemRoleUserList(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemRoleUserListPara
        {
            public enum ParaField
            {
                SYS_ID, ROLE_ID, USER_ID, USER_NM
            }

            public DBVarChar SysID;
            public DBVarChar RoleID;
            public DBVarChar UserID;
            public DBObject UserNM;
        }

        public class SystemRoleUserList : DBTableRow
        {
            public enum DataField
            {
                USER_ID, USER_NM, UPD_USER_NM, UPD_DT
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<SystemRoleUserList> SelectSystemRoleUserList(SystemRoleUserListPara para)
        {
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.UserID.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                    {
                        commandWhere, " AND U.USER_ID={USER_ID} ", Environment.NewLine
                    });
            }

            if (!string.IsNullOrWhiteSpace(para.UserNM.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                    {
                        commandWhere, " AND U.USER_NM LIKE N'%{USER_NM}%' ", Environment.NewLine
                    });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT U.USER_ID, dbo.FN_GET_IDNM(U.USER_ID, U.USER_NM) AS USER_NM ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(S.UPD_USER_ID) AS UPD_USER_NM, S.UPD_DT ", Environment.NewLine,
                "FROM SYS_USER_SYSTEM_ROLE S JOIN RAW_CM_USER U ON S.USER_ID=U.USER_ID ", Environment.NewLine,
                "WHERE S.SYS_ID={SYS_ID} AND S.ROLE_ID={ROLE_ID} ", Environment.NewLine,
                commandWhere, Environment.NewLine,
                "ORDER BY S.USER_ID ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemRoleUserListPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemRoleUserListPara.ParaField.ROLE_ID, Value = para.RoleID });
            dbParameters.Add(new DBParameter { Name = SystemRoleUserListPara.ParaField.USER_ID, Value = para.UserID });
            if (!string.IsNullOrWhiteSpace(para.UserNM.GetValue()))
            {
                dbParameters.Add(new DBParameter { Name = SystemRoleUserListPara.ParaField.USER_NM.ToString(), Value = para.UserNM });
            }

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemRoleUserList> systemRoleUserListList = new List<SystemRoleUserList>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemRoleUserList systemRoleUserList = new SystemRoleUserList()
                    {
                        UserID = new DBVarChar(dataRow[SystemRoleUserList.DataField.USER_ID.ToString()]),
                        UserNM = new DBNVarChar(dataRow[SystemRoleUserList.DataField.USER_NM.ToString()]),
                        UpdUserNM = new DBNVarChar(dataRow[SystemRoleUserList.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[SystemRoleUserList.DataField.UPD_DT.ToString()]),
                    };
                    systemRoleUserListList.Add(systemRoleUserList);
                }
                return systemRoleUserListList;
            }
            return null;
        }
    }
}