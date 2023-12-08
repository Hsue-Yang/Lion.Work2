using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemRole : EntitySys
    {
        public EntitySystemRole(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemRolePara : DBCulture
        {
            public SystemRolePara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, ROLE_ID,
                SYS_NM, ROLE_NM
            }

            public DBVarChar SysID;
            public DBVarChar RoleID;
        }

        public class SystemRole : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM,
                ROLE_ID, ROLE_NM,
                UPD_USER_NM, UPD_DT
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public DBVarChar RoleID;
            public DBNVarChar RoleNM;

            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<SystemRole> SelectSystemRoleList(SystemRolePara para)
        {
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.RoleID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND R.ROLE_ID={ROLE_ID} ", Environment.NewLine });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT R.SYS_ID, dbo.FN_GET_NMID(R.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , R.ROLE_ID, dbo.FN_GET_NMID(R.ROLE_ID, R.{ROLE_NM}) AS ROLE_NM ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(R.UPD_USER_ID) AS UPD_USER_NM, R.UPD_DT ", Environment.NewLine,
                "FROM SYS_SYSTEM_ROLE R ", Environment.NewLine,
                "INNER JOIN SYS_SYSTEM_MAIN M ON R.SYS_ID = M.SYS_ID ", Environment.NewLine,
                "WHERE R.SYS_ID={SYS_ID} ", Environment.NewLine,
                commandWhere, Environment.NewLine,
                "ORDER BY M.SORT_ORDER, R.ROLE_ID ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.ROLE_ID.ToString(), Value = para.RoleID });
            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemRolePara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemRolePara.ParaField.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemRolePara.ParaField.ROLE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemRole> systemRoleList = new List<SystemRole>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemRole systemRole = new SystemRole()
                    {
                        SysID = new DBVarChar(dataRow[SystemRole.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemRole.DataField.SYS_NM.ToString()]),

                        RoleID = new DBVarChar(dataRow[SystemRole.DataField.ROLE_ID.ToString()]),
                        RoleNM = new DBNVarChar(dataRow[SystemRole.DataField.ROLE_NM.ToString()]),

                        UpdUserNM = new DBNVarChar(dataRow[SystemRole.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[SystemRole.DataField.UPD_DT.ToString()])
                    };
                    systemRoleList.Add(systemRole);
                }
                return systemRoleList;
            }
            return null;
        }
    }
}