using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.B2P.Pub
{
    public class EntityMenuSetting : EntityPub
    {
        public EntityMenuSetting(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class MenuSettingValue : ValueListRow
        {
            public enum ValueField
            {
                SysID, FunMenu, MenuID, SortOrder
            }

            public string SysID { get; set; }
            public string FunMenu { get; set; }
            public string MenuID { get; set; }
            public string SortOrder { get; set; }

            public DBVarChar GetSysID()
            {
                return new DBVarChar(SysID);
            }
            public DBVarChar GetFunMenu()
            {
                return new DBVarChar(FunMenu);
            }
            public DBVarChar GetMenuID()
            {
                return new DBVarChar(MenuID);
            }
            public DBVarChar GetSortOrder()
            {
                return new DBVarChar(SortOrder);
            }
        }
        
        public class MenuSettingPara : DBCulture
        {
            public MenuSettingPara(string cultureID)
                : base(cultureID)
            {
            
            }

            public enum ParaField
            {
                USER_ID, SYS_ID,
                FUN_MENU, FUN_MENU_NM, MENU_ID,
                SORT_ORDER, UPD_USER_ID
            }

            public DBVarChar UserID;
            public DBVarChar UpdUserID;
        }

        public class MenuSetting : DBTableRow
        {
            public enum DataField
            {
                SYS_ID,
                FUN_MENU, FUN_MENU_NM, MENU_ID,
                SORT_ORDER
            }

            public DBVarChar SysID;
            public DBVarChar FunMenu;
            public DBNVarChar FunMenuNM;
            public DBVarChar MenuID;
            public DBVarChar SortOrder;
        }

        public List<MenuSetting> SelectMenuSettingList(MenuSettingPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT U.SYS_ID, U.FUN_MENU, dbo.FN_GET_NMID(U.FUN_MENU, M.{FUN_MENU_NM}) AS FUN_MENU_NM ", Environment.NewLine,
                "     , U.MENU_ID ", Environment.NewLine,
                "     , U.SORT_ORDER ", Environment.NewLine,
                "FROM SYS_USER_FUN_MENU U ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_FUN_MENU M ON U.SYS_ID = M.SYS_ID AND U.FUN_MENU = M.FUN_MENU ", Environment.NewLine,
                "WHERE U.USER_ID={USER_ID} ", Environment.NewLine,
                "ORDER BY U.MENU_ID, U.SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = MenuSettingPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = MenuSettingPara.ParaField.FUN_MENU_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(MenuSettingPara.ParaField.FUN_MENU_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<MenuSetting> menuSettingList = new List<MenuSetting>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MenuSetting menuSetting = new MenuSetting()
                    {
                        SysID = new DBVarChar(dataRow[MenuSetting.DataField.SYS_ID.ToString()]),
                        FunMenu = new DBVarChar(dataRow[MenuSetting.DataField.FUN_MENU.ToString()]),
                        FunMenuNM = new DBNVarChar(dataRow[MenuSetting.DataField.FUN_MENU_NM.ToString()]),
                        MenuID = new DBVarChar(dataRow[MenuSetting.DataField.MENU_ID.ToString()]),
                        SortOrder = new DBVarChar(dataRow[MenuSetting.DataField.SORT_ORDER.ToString()])
                    };
                    menuSettingList.Add(menuSetting);
                }
                return menuSettingList;
            }
            return null;
        }

        public enum EnumEditMenuSettingResult
        {
            Success, Failure
        }

        public EnumEditMenuSettingResult EditMenuSetting(MenuSettingPara para, List<MenuSettingValue> menuSettingValueList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            string deleteCommandText = string.Concat(new object[] 
            {
                "DELETE FROM SYS_USER_FUN_MENU ", Environment.NewLine,
                "WHERE USER_ID={USER_ID}; ", Environment.NewLine
            });

            dbParameters.Add(new DBParameter { Name = MenuSettingPara.ParaField.USER_ID, Value = para.UserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, deleteCommandText, dbParameters));
            dbParameters.Clear();

            foreach (MenuSettingValue menuSettingValue in menuSettingValueList)
            {
                string insertCommand = string.Concat(new object[]
                {
                    "INSERT INTO SYS_USER_FUN_MENU VALUES ( ", Environment.NewLine,
                    "    {USER_ID}, {SYS_ID}, {FUN_MENU}, {MENU_ID}, {SORT_ORDER} ", Environment.NewLine,
                    "  , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                    "); ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = MenuSettingPara.ParaField.USER_ID, Value = para.UserID });
                dbParameters.Add(new DBParameter { Name = MenuSettingPara.ParaField.SYS_ID, Value = menuSettingValue.GetSysID() });
                dbParameters.Add(new DBParameter { Name = MenuSettingPara.ParaField.FUN_MENU, Value = menuSettingValue.GetFunMenu() });
                dbParameters.Add(new DBParameter { Name = MenuSettingPara.ParaField.MENU_ID, Value = menuSettingValue.GetMenuID() });
                dbParameters.Add(new DBParameter { Name = MenuSettingPara.ParaField.SORT_ORDER, Value = menuSettingValue.GetSortOrder() });
                dbParameters.Add(new DBParameter { Name = MenuSettingPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, insertCommand, dbParameters));
                dbParameters.Clear();
            }

            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,
                commandTextStringBuilder.ToString(), Environment.NewLine,
                "        SET @RESULT = 'Y'; ", Environment.NewLine,
                "        COMMIT; ", Environment.NewLine,
                "    END TRY ", Environment.NewLine,
                "    BEGIN CATCH ", Environment.NewLine,
                "        SET @RESULT = 'N'; ", Environment.NewLine,
                "        ROLLBACK TRANSACTION; ", Environment.NewLine,
                "    END CATCH ", Environment.NewLine,
                "; ", Environment.NewLine,
                "SELECT @RESULT; ", Environment.NewLine                
            });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditMenuSettingResult.Success : EnumEditMenuSettingResult.Failure;            
        }
    }
}