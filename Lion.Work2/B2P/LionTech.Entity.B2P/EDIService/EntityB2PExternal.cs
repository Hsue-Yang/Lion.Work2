using System;
using System.Collections.Generic;
using System.Data;
using LionTech.Entity.EDI;

namespace LionTech.Entity.B2P.EDIService
{
    public class EntityB2PExternal : EntityEDIService
    {
        public EntityB2PExternal(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SysUserFunMenu : DBTableRow
        {
            public enum DataField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public List<SysUserFunMenu> SelectSysUserFunMenuList()
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT DISTINCT USER_ID FROM SYS_USER_FUN_MENU; ", Environment.NewLine
            });

            DataTable dataTable = base.GetDataTable(commandText, null);
            if (dataTable.Rows.Count > 0)
            {
                List<SysUserFunMenu> sysUserFunMenuList = new List<SysUserFunMenu>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysUserFunMenu sysUserFunMenu = new SysUserFunMenu()
                    {
                        UserID = new DBVarChar(dataRow[SysUserFunMenu.DataField.USER_ID.ToString()])
                    };
                    sysUserFunMenuList.Add(sysUserFunMenu);
                }
                return sysUserFunMenuList;
            }
            return null;
        }
    }
}