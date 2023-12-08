using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemFunMenu : EntitySys
    {
        public EntitySystemFunMenu(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemFunMenuPara : DBCulture
        {
            public SystemFunMenuPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, FUN_MENU,
                SYS_NM, FUN_MENU_NM
            }

            public DBVarChar SysID;
            public DBVarChar FunMenu;
        }

        public class SystemFunMenu : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM,
                FUN_MENU, FUN_MENU_NM,
                DEFAULT_MENU_ID, IS_DISABLE, SORT_ORDER, UPD_USER_NM, UPD_DT
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public DBVarChar FunMenu;
            public DBNVarChar FunMenuNM;

            public DBVarChar DefaultMenuID;
            public DBChar IsDisable;
            public DBVarChar SortOrder;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<SystemFunMenu> SelectSystemFunMenuList(SystemFunMenuPara para)
        {
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.FunMenu.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND N.FUN_MENU={FUN_MENU} ", Environment.NewLine });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT N.SYS_ID, dbo.FN_GET_NMID(N.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , N.FUN_MENU, dbo.FN_GET_NMID(N.FUN_MENU, N.{FUN_MENU_NM}) AS FUN_MENU_NM ", Environment.NewLine,
                "     , N.DEFAULT_MENU_ID, N.IS_DISABLE, N.SORT_ORDER ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(N.UPD_USER_ID) AS UPD_USER_NM, N.UPD_DT ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN_MENU N ", Environment.NewLine,
                "INNER JOIN SYS_SYSTEM_MAIN M ON N.SYS_ID = M.SYS_ID ", Environment.NewLine,
                "WHERE N.SYS_ID={SYS_ID} ", Environment.NewLine,
                commandWhere, Environment.NewLine,
                "ORDER BY M.SORT_ORDER, N.SORT_ORDER ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemFunMenuPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunMenuPara.ParaField.FUN_MENU.ToString(), Value = para.FunMenu });
            dbParameters.Add(new DBParameter { Name = SystemFunMenuPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunMenuPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunMenuPara.ParaField.FUN_MENU_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunMenuPara.ParaField.FUN_MENU_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemFunMenu> systemFunMenuList = new List<SystemFunMenu>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemFunMenu systemFunMenu = new SystemFunMenu()
                    {
                        SysID = new DBVarChar(dataRow[SystemFunMenu.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemFunMenu.DataField.SYS_NM.ToString()]),

                        FunMenu = new DBVarChar(dataRow[SystemFunMenu.DataField.FUN_MENU.ToString()]),
                        FunMenuNM = new DBNVarChar(dataRow[SystemFunMenu.DataField.FUN_MENU_NM.ToString()]),

                        DefaultMenuID = new DBVarChar(dataRow[SystemFunMenu.DataField.DEFAULT_MENU_ID.ToString()]),
                        IsDisable = new DBChar(dataRow[SystemFunMenu.DataField.IS_DISABLE.ToString()]),
                        SortOrder = new DBVarChar(dataRow[SystemFunMenu.DataField.SORT_ORDER.ToString()]),

                        UpdUserNM = new DBNVarChar(dataRow[SystemFunMenu.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[SystemFunMenu.DataField.UPD_DT.ToString()]),
                    };
                    systemFunMenuList.Add(systemFunMenu);
                }
                return systemFunMenuList;
            }
            return null;
        }
    }
}