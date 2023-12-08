using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemFunGroup : EntitySys
    {
        public EntitySystemFunGroup(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemFunGroupPara : DBCulture
        {
            public SystemFunGroupPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, FUN_CONTROLLER_ID,
                SYS_NM, FUN_GROUP
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
        }

        public class SystemFunGroup : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM,
                FUN_CONTROLLER_ID, FUN_GROUP, SORT_ORDER, 
                UPD_USER_NM, UPD_DT
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public DBVarChar FunControllerID;
            public DBNVarChar FunGroup;
            public DBVarChar SortOrder;

            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<SystemFunGroup> SelectSystemFunGroupList(SystemFunGroupPara para)
        {
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.FunControllerID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND G.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} ", Environment.NewLine });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT G.SYS_ID, dbo.FN_GET_NMID(G.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , G.FUN_CONTROLLER_ID, dbo.FN_GET_NMID(G.FUN_CONTROLLER_ID, G.{FUN_GROUP}) AS FUN_GROUP ", Environment.NewLine,
                "     , G.SORT_ORDER ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(G.UPD_USER_ID) AS UPD_USER_NM, G.UPD_DT ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN_GROUP G ", Environment.NewLine,
                "INNER JOIN SYS_SYSTEM_MAIN M ON G.SYS_ID = M.SYS_ID ", Environment.NewLine,
                "WHERE G.SYS_ID={SYS_ID} ", Environment.NewLine,
                commandWhere, Environment.NewLine,
                "ORDER BY M.SORT_ORDER, G.SORT_ORDER ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemFunGroupPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunGroupPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunGroupPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunGroupPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunGroupPara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunGroupPara.ParaField.FUN_GROUP.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemFunGroup> systemFunGroupList = new List<SystemFunGroup>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemFunGroup systemFunGroup = new SystemFunGroup()
                    {
                        SysID = new DBVarChar(dataRow[SystemFunGroup.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemFunGroup.DataField.SYS_NM.ToString()]),

                        FunControllerID = new DBVarChar(dataRow[SystemFunGroup.DataField.FUN_CONTROLLER_ID.ToString()]),
                        FunGroup = new DBNVarChar(dataRow[SystemFunGroup.DataField.FUN_GROUP.ToString()]),
                        SortOrder = new DBVarChar(dataRow[SystemFunGroup.DataField.SORT_ORDER.ToString()]),

                        UpdUserNM = new DBNVarChar(dataRow[SystemFunGroup.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[SystemFunGroup.DataField.UPD_DT.ToString()]),
                    };
                    systemFunGroupList.Add(systemFunGroup);
                }
                return systemFunGroupList;
            }
            return null;
        }
    }
}