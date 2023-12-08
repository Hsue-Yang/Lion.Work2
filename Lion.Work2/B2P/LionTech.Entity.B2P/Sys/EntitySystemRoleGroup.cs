using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemRoleGroup : EntitySys
    {
        public EntitySystemRoleGroup(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemRoleGroupPara : DBCulture
        {
            public SystemRoleGroupPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                ROLE_GROUP_NM
            }

            //public DBVarChar UpdUserID;
            //public DBVarChar SysID;
        }


        public class SystemRoleGroup : DBTableRow
        {
            public enum DataField
            {
                ROLE_GROUP_ID,
                ROLE_GROUP_NM,
                SORT_ORDER, REMARK,
                UPD_USER_NM, UPD_DT
            }

            public DBVarChar RoleGroupID;
            public DBNVarChar RoleGroupNM;
            public DBVarChar SortOrder;
            public DBNVarChar Remark;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDt;
        }

        public List<SystemRoleGroup> SelectSystemRoleGroupList(SystemRoleGroupPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT G.ROLE_GROUP_ID ", Environment.NewLine,
                "     , G.{ROLE_GROUP_NM} AS ROLE_GROUP_NM ", Environment.NewLine,
                "     , G.SORT_ORDER, G.REMARK ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(G.UPD_USER_ID) AS UPD_USER_NM, G.UPD_DT ", Environment.NewLine,
                "FROM SYS_SYSTEM_ROLE_GROUP G ", Environment.NewLine,
                //"WHERE E.SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY G.SORT_ORDER, G.ROLE_GROUP_ID ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemRoleGroupPara.ParaField.ROLE_GROUP_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemRoleGroupPara.ParaField.ROLE_GROUP_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemRoleGroup> SystemRoleGroupList = new List<SystemRoleGroup>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemRoleGroup SystemRoleGroup = new SystemRoleGroup()
                    {
                        RoleGroupID = new DBVarChar(dataRow[SystemRoleGroup.DataField.ROLE_GROUP_ID.ToString()]),
                        RoleGroupNM = new DBNVarChar(dataRow[SystemRoleGroup.DataField.ROLE_GROUP_NM.ToString()]),
                        SortOrder = new DBVarChar(dataRow[SystemRoleGroup.DataField.SORT_ORDER.ToString()]),
                        Remark = new DBNVarChar(dataRow[SystemRoleGroup.DataField.REMARK.ToString()]),
                        UpdUserNM = new DBNVarChar(dataRow[SystemRoleGroup.DataField.UPD_USER_NM.ToString()]),
                        UpdDt = new DBDateTime(dataRow[SystemRoleGroup.DataField.UPD_DT.ToString()])
                    };
                    SystemRoleGroupList.Add(SystemRoleGroup);
                }
                return SystemRoleGroupList;
            }
            return null;
        }
    }
}