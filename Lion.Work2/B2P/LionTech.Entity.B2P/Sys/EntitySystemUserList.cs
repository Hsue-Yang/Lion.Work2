using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemUserList : EntitySys
    {
        public EntitySystemUserList(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemUserListPara
        {
            public enum ParaField
            {
                SYS_ID, USER_ID, USER_NM
            }

            public DBVarChar SysID;
            public DBVarChar UserID;
            public DBObject UserNM;
        }

        public class SystemUserList : DBTableRow
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

        public List<SystemUserList> SelectSystemUserList(SystemUserListPara para)
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
                        commandWhere, " AND U.USER_NM LIKE '%{USER_NM}%' ", Environment.NewLine
                    });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT U.USER_ID, dbo.FN_GET_IDNM(U.USER_ID, U.USER_NM) AS USER_NM ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(S.UPD_USER_ID) AS UPD_USER_NM, S.UPD_DT ", Environment.NewLine,
                "FROM SYS_USER_SYSTEM S JOIN RAW_CM_USER U ON S.USER_ID=U.USER_ID ", Environment.NewLine,
                "WHERE S.SYS_ID={SYS_ID} ", Environment.NewLine,
                commandWhere, Environment.NewLine,
                "ORDER BY S.USER_ID ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemUserListPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemUserListPara.ParaField.USER_ID, Value = para.UserID });
            if (!string.IsNullOrWhiteSpace(para.UserNM.GetValue()))
            {
                dbParameters.Add(new DBParameter { Name = SystemUserListPara.ParaField.USER_NM.ToString(), Value = para.UserNM });
            }

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemUserList> systemUserListList = new List<SystemUserList>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemUserList systemUserList = new SystemUserList()
                    {
                        UserID = new DBVarChar(dataRow[SystemUserList.DataField.USER_ID.ToString()]),
                        UserNM = new DBNVarChar(dataRow[SystemUserList.DataField.USER_NM.ToString()]),
                        UpdUserNM = new DBNVarChar(dataRow[SystemUserList.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[SystemUserList.DataField.UPD_DT.ToString()]),
                    };
                    systemUserListList.Add(systemUserList);
                }
                return systemUserListList;
            }
            return null;
        }
    }
}