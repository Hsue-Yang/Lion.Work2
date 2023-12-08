using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySys : DBEntity
    {
        public EntitySys(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SysUserSystemSysIDPara : DBCulture
        {
            public SysUserSystemSysIDPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                USER_ID, SYS_NM
            }

            public DBVarChar UserID;
        }

        public class SysUserSystemSysID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                SYS_ID, SYS_NM
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public string ItemText()
            {
                return this.SysNM.StringValue();
            }

            public string ItemValue()
            {
                return this.SysID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysUserSystemSysID> SelectSysUserSystemSysIDList(SysUserSystemSysIDPara para, bool excludeOutsourcing)
        {
            string commandWhere = string.Empty;

            if (excludeOutsourcing)
            {
                commandWhere = string.Concat(new object[]
                {
                    " AND M.IS_OUTSOURCING='N' ", Environment.NewLine
                });
            }

            string commandText = string.Concat(new object[]
            {
                "DECLARE @ROLE_CN INT = 0; ", Environment.NewLine,
                "SELECT @ROLE_CN=COUNT(ROLE_ID) ", Environment.NewLine,
                "FROM SYS_USER_SYSTEM_ROLE ", Environment.NewLine,
                "WHERE SYS_ID='B2PAP' AND ROLE_ID='GRANTOR' ", Environment.NewLine,
                "  AND USER_ID={USER_ID}; ", Environment.NewLine,

                "SELECT DISTINCT M.SYS_ID ", Environment.NewLine,
                "     , (CASE WHEN M.IS_OUTSOURCING='N' THEN '' ELSE '*' END)+dbo.FN_GET_NMID(M.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , M.SORT_ORDER ", Environment.NewLine,
                "FROM SYS_USER_SYSTEM S ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON S.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "LEFT JOIN SYS_USER_SYSTEM_ROLE R ON S.USER_ID=R.USER_ID AND S.SYS_ID=R.SYS_ID ", Environment.NewLine,
                "WHERE S.USER_ID={USER_ID} AND ((M.IS_OUTSOURCING='Y' AND (M.SYS_MAN_USER_ID={USER_ID} OR @ROLE_CN>0)) OR (M.IS_OUTSOURCING='N' AND R.ROLE_ID<>'USER')) ", Environment.NewLine,
                commandWhere, Environment.NewLine, 
                "ORDER BY M.SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysUserSystemSysIDPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = SysUserSystemSysIDPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysUserSystemSysIDPara.ParaField.SYS_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysUserSystemSysID> sysUserSystemSysIDList = new List<SysUserSystemSysID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysUserSystemSysID userSystemSysID = new SysUserSystemSysID()
                    {
                        SysID = new DBVarChar(dataRow[SysUserSystemSysID.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SysUserSystemSysID.DataField.SYS_NM.ToString()])
                    };
                    sysUserSystemSysIDList.Add(userSystemSysID);
                }
                return sysUserSystemSysIDList;
            }
            return null;
        }

        public class SysSystemSysIDPara : DBCulture
        {
            public SysSystemSysIDPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID
            }

            public DBVarChar SysID;
        }

        public string GetFileDataPath(SysSystemSysIDPara para)
        {
            string FileDataPath = string.Empty;
            string commandText = string.Concat(new object[]
            {
                "SELECT FOLDER_PATH ", Environment.NewLine,
                "FROM SYS_SYSTEM_IP ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND IS_FILE_SERVER='Y' ", Environment.NewLine,
                "ORDER BY SYS_ID "
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            //帶入參數
            dbParameters.Add(new DBParameter { Name = SysSystemSysIDPara.ParaField.SYS_ID.ToString(), Value = para.SysID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    FileDataPath = new DBVarChar(dataRow["FOLDER_PATH"]).StringValue();
                }
                return FileDataPath;
            }
            return null;
        }

        public class SystemSysIDPara : DBCulture
        {
            public SystemSysIDPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_NM
            }
        }

        public class SystemSysID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                SYS_ID, SYS_NM
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public string ItemText()
            {
                return this.SysNM.StringValue();
            }

            public string ItemValue()
            {
                return this.SysID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SystemSysID> SelectSystemSysIDList(SystemSysIDPara para, bool excludeOutsourcing)
        {
            string commandWhere = string.Empty;

            if (excludeOutsourcing)
            {
                commandWhere = string.Concat(new object[]
                {
                    "WHERE IS_OUTSOURCING='N' ", Environment.NewLine
                });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID ", Environment.NewLine,
                "     , (CASE WHEN IS_OUTSOURCING='N' THEN '' ELSE '*' END)+dbo.FN_GET_NMID(SYS_ID, {SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_MAIN ", Environment.NewLine,
                commandWhere, Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemSysIDPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemSysIDPara.ParaField.SYS_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemSysID> systemSysIDList = new List<SystemSysID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemSysID systemSysID = new SystemSysID()
                    {
                        SysID = new DBVarChar(dataRow[SystemSysID.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemSysID.DataField.SYS_NM.ToString()])
                    };
                    systemSysIDList.Add(systemSysID);
                }
                return systemSysIDList;
            }
            return null;
        }

        public class SystemSubsysIDPara : DBCulture
        {
            public SystemSubsysIDPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_NM, PARENT_SYS_ID
            }

            public DBVarChar ParentSysID;
        }

        public class SystemSubsysID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                SYS_ID, SYS_NM
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public string ItemText()
            {
                return this.SysNM.StringValue();
            }

            public string ItemValue()
            {
                return this.SysID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SystemSubsysID> SelectSystemSubsysIDList(SystemSubsysIDPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID ", Environment.NewLine,
                "     , dbo.FN_GET_NMID(SYS_ID, {SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_SUB ", Environment.NewLine,
                "WHERE PARENT_SYS_ID={PARENT_SYS_ID} ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemSubsysIDPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemSubsysIDPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemSubsysIDPara.ParaField.PARENT_SYS_ID.ToString(), Value = para.ParentSysID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemSubsysID> systemSubsysIDList = new List<SystemSubsysID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemSubsysID systemSubsysID = new SystemSubsysID()
                    {
                        SysID = new DBVarChar(dataRow[SystemSubsysID.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemSubsysID.DataField.SYS_NM.ToString()])
                    };
                    systemSubsysIDList.Add(systemSubsysID);
                }
                return systemSubsysIDList;
            }
            return null;
        }

        public class SysSystemRoleIDPara : DBCulture
        {
            public SysSystemRoleIDPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, ROLE_NM
            }

            public DBVarChar SysID;
        }

        public class SysSystemRoleID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                ROLE_ID, ROLE_NM
            }

            public DBVarChar RoleID;
            public DBNVarChar RoleIDNM;

            public string ItemText()
            {
                return this.RoleIDNM.StringValue();
            }

            public string ItemValue()
            {
                return this.RoleID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysSystemRoleID> SelectSysSystemRoleIDList(SysSystemRoleIDPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT ROLE_ID, {ROLE_NM} AS ROLE_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_ROLE ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SYS_ID, ROLE_ID ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemRoleIDPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleIDPara.ParaField.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemRoleIDPara.ParaField.ROLE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemRoleID> sysSystemRoleIDList = new List<SysSystemRoleID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemRoleID sysSystemRoleID = new SysSystemRoleID()
                    {
                        RoleID = new DBVarChar(dataRow[SysSystemRoleID.DataField.ROLE_ID.ToString()]),
                        RoleIDNM = new DBNVarChar(dataRow[SysSystemRoleID.DataField.ROLE_NM.ToString()])
                    };
                    sysSystemRoleIDList.Add(sysSystemRoleID);
                }
                return sysSystemRoleIDList;
            }
            return null;
        }

        public class SysSystemFunControllerIDPara : DBCulture
        {
            public SysSystemFunControllerIDPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, FUN_GROUP
            }

            public DBVarChar SysID;
        }

        public class SysSystemFunControllerID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                FUN_CONTROLLER_ID, FUN_GROUP
            }

            public DBVarChar FunControllerID;
            public DBNVarChar FunGroup;

            public string ItemText()
            {
                return this.FunGroup.StringValue();
            }

            public string ItemValue()
            {
                return this.FunControllerID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysSystemFunControllerID> SelectSysSystemFunControllerIDList(SysSystemFunControllerIDPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT FUN_CONTROLLER_ID, dbo.FN_GET_NMID(FUN_CONTROLLER_ID, {FUN_GROUP}) AS FUN_GROUP ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN_GROUP ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SYS_ID, SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemFunControllerIDPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemFunControllerIDPara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemFunControllerIDPara.ParaField.FUN_GROUP.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemFunControllerID> sysSystemFunControllerIDList = new List<SysSystemFunControllerID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemFunControllerID sysSystemFunControllerID = new SysSystemFunControllerID()
                    {
                        FunControllerID = new DBVarChar(dataRow[SysSystemFunControllerID.DataField.FUN_CONTROLLER_ID.ToString()]),
                        FunGroup = new DBNVarChar(dataRow[SysSystemFunControllerID.DataField.FUN_GROUP.ToString()])
                    };
                    sysSystemFunControllerIDList.Add(sysSystemFunControllerID);
                }
                return sysSystemFunControllerIDList;
            }
            return null;
        }

        public class SysSystemFunNamePara : DBCulture
        {
            public SysSystemFunNamePara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, FUN_NM, FUN_CONTROLLER_ID
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
        }

        public class SysSystemFunName : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                FUN_ACTION_NAME, FUN_NM
            }


            public DBVarChar FunActionName;
            public DBVarChar FunName;

            public string ItemText()
            {
                return this.FunName.StringValue();
            }

            public string ItemValue()
            {
                return this.FunActionName.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysSystemFunName> SelectSysSystemFunNameList(SysSystemFunNamePara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT FUN_ACTION_NAME, dbo.FN_GET_NMID(FUN_ACTION_NAME, {FUN_NM}) AS FUN_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} ", Environment.NewLine,
                "ORDER BY SYS_ID, SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemFunNamePara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemFunNamePara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SysSystemFunNamePara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemFunNamePara.ParaField.FUN_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemFunName> sysSystemFunNameList = new List<SysSystemFunName>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemFunName sysSystemFunName = new SysSystemFunName()
                    {
                        FunActionName = new DBVarChar(dataRow[SysSystemFunName.DataField.FUN_ACTION_NAME.ToString()]),
                        FunName = new DBVarChar(dataRow[SysSystemFunName.DataField.FUN_NM.ToString()])

                    };
                    sysSystemFunNameList.Add(sysSystemFunName);
                }
                return sysSystemFunNameList;
            }
            return null;
        }

        public class SysSystemPurviewIDPara : DBCulture
        {
            public SysSystemPurviewIDPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, PURVIEW_NM
            }

            public DBVarChar SysID;
        }

        public class SysSystemPurviewID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                PURVIEW_ID, PURVIEW_NM
            }

            public DBVarChar PurviewID;
            public DBNVarChar PurviewNM;

            public string ItemText()
            {
                return this.PurviewNM.StringValue();
            }

            public string ItemValue()
            {
                return this.PurviewID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysSystemPurviewID> SelectSysSystemPurviewIDList(SysSystemPurviewIDPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT PURVIEW_ID, dbo.FN_GET_NMID(PURVIEW_ID, {PURVIEW_NM}) AS PURVIEW_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_PURVIEW ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SYS_ID, SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemPurviewIDPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemPurviewIDPara.ParaField.PURVIEW_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemPurviewIDPara.ParaField.PURVIEW_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemPurviewID> sysSystemPurviewIDList = new List<SysSystemPurviewID>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemPurviewID sysSystemPurviewID = new SysSystemPurviewID()
                    {
                        PurviewID = new DBVarChar(dataRow[SysSystemPurviewID.DataField.PURVIEW_ID.ToString()]),
                        PurviewNM = new DBNVarChar(dataRow[SysSystemPurviewID.DataField.PURVIEW_NM.ToString()])
                    };
                    sysSystemPurviewIDList.Add(sysSystemPurviewID);
                }
                return sysSystemPurviewIDList;
            }
            return null;
        }

        public class SysSystemFunMenuPara : DBCulture
        {
            public SysSystemFunMenuPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, FUN_MENU_NM
            }

            public DBVarChar SysID;
        }

        public class SysSystemFunMenu : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                FUN_MENU, FUN_MENU_NM
            }

            public DBVarChar FunMenu;
            public DBNVarChar FunMenuNM;

            public string ItemText()
            {
                return this.FunMenuNM.StringValue();
            }

            public string ItemValue()
            {
                return this.FunMenu.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysSystemFunMenu> SelectSysSystemFunMenuList(SysSystemFunMenuPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT FUN_MENU, dbo.FN_GET_NMID(FUN_MENU, {FUN_MENU_NM}) AS FUN_MENU_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN_MENU ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SYS_ID, SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemFunMenuPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemFunMenuPara.ParaField.FUN_MENU_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemFunMenuPara.ParaField.FUN_MENU_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemFunMenu> sysSystemFunMenuList = new List<SysSystemFunMenu>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemFunMenu sysSystemFunMenu = new SysSystemFunMenu()
                    {
                        FunMenu = new DBVarChar(dataRow[SysSystemFunMenu.DataField.FUN_MENU.ToString()]),
                        FunMenuNM = new DBNVarChar(dataRow[SysSystemFunMenu.DataField.FUN_MENU_NM.ToString()])
                    };
                    sysSystemFunMenuList.Add(sysSystemFunMenu);
                }
                return sysSystemFunMenuList;
            }
            return null;
        }

        public class SystemMenuFunPara : DBCulture
        {
            public SystemMenuFunPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                FUN_CONTROLLER_ID, FUN_ACTION_NAME,
                FUN_MENU_SYS_ID, FUN_MENU,

                FUN_MENU_NM
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
            public DBVarChar FunMenuSysID;
            public DBVarChar FunMenu;
        }

        public class SystemMenuFun : DBTableRow
        {
            public enum DataField
            {
                SYS_ID,
                FUN_MENU_SYS_ID,
                FUN_MENU, FUN_MENU_NM,
                FUN_CONTROLLER_ID, FUN_ACTION_NAME,
                FUN_MENU_XAXIS, FUN_MENU_YAXIS,
                FUN_MENU_IS_DISABLE
            }

            public DBVarChar SysID;

            public DBVarChar FunMenuSysID;

            public DBVarChar FunMenu;
            public DBNVarChar FunMenuNM;

            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;

            public DBVarChar FunMenuXAxis;
            public DBVarChar FunMenuYAxis;

            public DBChar FunMenuIsDisable;
        }

        public List<SystemMenuFun> SelectSystemMenuFunList(SystemMenuFunPara para)
        {
            #region - commandWhere -
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.FunControllerID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND F.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.FunActionName.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND F.FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.FunMenuSysID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND F.FUN_MENU_SYS_ID={FUN_MENU_SYS_ID} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.FunMenu.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND F.FUN_MENU={FUN_MENU} ", Environment.NewLine });
            }
            #endregion

            string commandText = string.Concat(new object[]
            {
                "SELECT F.SYS_ID ", Environment.NewLine,
                "     , F.FUN_MENU_SYS_ID ", Environment.NewLine,
                "     , F.FUN_MENU, dbo.FN_GET_NMID(F.FUN_MENU, M.{FUN_MENU_NM}) AS FUN_MENU_NM ", Environment.NewLine,
                "     , F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME ", Environment.NewLine,
                "     , F.FUN_MENU_XAXIS, F.FUN_MENU_YAXIS ", Environment.NewLine,
                "     , M.IS_DISABLE AS FUN_MENU_IS_DISABLE ", Environment.NewLine,
                "FROM SYS_SYSTEM_MENU_FUN F ", Environment.NewLine,
                "JOIN SYS_SYSTEM_FUN_MENU M ON F.FUN_MENU_SYS_ID=M.SYS_ID AND F.FUN_MENU=M.FUN_MENU ", Environment.NewLine,
                "WHERE F.SYS_ID={SYS_ID} ", Environment.NewLine,
                commandWhere,
                "ORDER BY F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemMenuFunPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemMenuFunPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemMenuFunPara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = SystemMenuFunPara.ParaField.FUN_MENU_SYS_ID.ToString(), Value = para.FunMenuSysID });
            dbParameters.Add(new DBParameter { Name = SystemMenuFunPara.ParaField.FUN_MENU.ToString(), Value = para.FunMenu });
            dbParameters.Add(new DBParameter { Name = SystemMenuFunPara.ParaField.FUN_MENU_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemMenuFunPara.ParaField.FUN_MENU_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemMenuFun> systemMenuFunList = new List<SystemMenuFun>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemMenuFun systemMenuFun = new SystemMenuFun()
                    {
                        SysID = new DBVarChar(dataRow[SystemMenuFun.DataField.SYS_ID.ToString()]),

                        FunMenuSysID = new DBVarChar(dataRow[SystemMenuFun.DataField.FUN_MENU_SYS_ID.ToString()]),

                        FunMenu = new DBVarChar(dataRow[SystemMenuFun.DataField.FUN_MENU.ToString()]),
                        FunMenuNM = new DBNVarChar(dataRow[SystemMenuFun.DataField.FUN_MENU_NM.ToString()]),

                        FunControllerID = new DBVarChar(dataRow[SystemMenuFun.DataField.FUN_CONTROLLER_ID.ToString()]),
                        FunActionName = new DBVarChar(dataRow[SystemMenuFun.DataField.FUN_ACTION_NAME.ToString()]),

                        FunMenuXAxis = new DBVarChar(dataRow[SystemMenuFun.DataField.FUN_MENU_XAXIS.ToString()]),
                        FunMenuYAxis = new DBVarChar(dataRow[SystemMenuFun.DataField.FUN_MENU_YAXIS.ToString()]),

                        FunMenuIsDisable = new DBChar(dataRow[SystemMenuFun.DataField.FUN_MENU_IS_DISABLE.ToString()])
                    };
                    systemMenuFunList.Add(systemMenuFun);
                }
                return systemMenuFunList;
            }
            return null;
        }

        public class SysSystemFunTypePara : DBCulture
        {
            public SysSystemFunTypePara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                CODE_NM
            }
        }

        public class SysSystemFunType : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                CODE_ID, CODE_NM
            }

            public DBVarChar CodeID;
            public DBNVarChar CodeNM;

            public string ItemText()
            {
                return this.CodeNM.StringValue();
            }

            public string ItemValue()
            {
                return this.CodeID.StringValue();
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SysSystemFunType> SelectSysSystemFunTypeList(SysSystemFunTypePara para)
        {
            string commandText = string.Concat(new object[]{
                "SELECT CODE_ID, dbo.FN_GET_NMID(CODE_ID, {CODE_NM}) AS CODE_NM ", Environment.NewLine,
                "FROM CM_CODE ", Environment.NewLine,
                "WHERE CODE_KIND='0011' ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemFunTypePara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemFunTypePara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemFunType> sysSystemFunTypeList = new List<SysSystemFunType>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemFunType sysSystemFunType = new SysSystemFunType()
                    {
                        CodeID = new DBVarChar(dataRow[SysSystemFunType.DataField.CODE_ID.ToString()]),
                        CodeNM = new DBNVarChar(dataRow[SysSystemFunType.DataField.CODE_NM.ToString()])
                    };
                    sysSystemFunTypeList.Add(sysSystemFunType);
                }
                return sysSystemFunTypeList;
            }
            return null;
        }

        public class SysSystemEDIFlowPara : DBCulture
        {
            public SysSystemEDIFlowPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, EDI_FLOW
            }

            public DBVarChar SysID;
        }

        public class SysSystemEDIFlow : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                EDI_FLOW_ID, EDI_FLOW_NM
            }

            public DBVarChar EDIFlowID;
            public DBNVarChar EDIFlowNM;

            public string ItemText()
            {
                return this.EDIFlowNM.StringValue();
            }

            public string ItemValue()
            {
                return this.EDIFlowID.GetValue();
            }

            public string ItemValue(string key)
            {
                throw new System.NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new System.NotImplementedException();
            }

            public string GroupBy()
            {
                throw new System.NotImplementedException();
            }
        }

        public List<SysSystemEDIFlow> SelectSystemEDIFlowList(SysSystemEDIFlowPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT EDI_FLOW_ID, dbo.FN_GET_NMID(EDI_FLOW_ID, {EDI_FLOW}) AS EDI_FLOW_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_FLOW ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemEDIFlowPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemEDIFlowPara.ParaField.EDI_FLOW.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemEDIFlowPara.ParaField.EDI_FLOW.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemEDIFlow> systemEDIFlowList = new List<SysSystemEDIFlow>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemEDIFlow systemEDIFlow = new SysSystemEDIFlow()
                    {
                        EDIFlowID = new DBVarChar(dataRow[SysSystemEDIFlow.DataField.EDI_FLOW_ID.ToString()]),
                        EDIFlowNM = new DBNVarChar(dataRow[SysSystemEDIFlow.DataField.EDI_FLOW_NM.ToString()])
                    };
                    systemEDIFlowList.Add(systemEDIFlow);
                }
                return systemEDIFlowList;
            }
            return null;
        }

        public class SysSystemEDIJobPara : DBCulture
        {
            public SysSystemEDIJobPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, EDI_FLOW_ID, EDI_JOB
            }

            public DBVarChar SysID;
            public DBVarChar EDIFlowID;
        }

        public class SysSystemEDIJob : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                EDI_JOB_ID, EDI_JOB_NM
            }

            public DBVarChar EDIJobID;
            public DBNVarChar EDIJobNM;

            public string ItemText()
            {
                return this.EDIJobNM.StringValue();
            }

            public string ItemValue()
            {
                return this.EDIJobID.GetValue();
            }

            public string ItemValue(string key)
            {
                throw new System.NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new System.NotImplementedException();
            }

            public string GroupBy()
            {
                throw new System.NotImplementedException();
            }
        }

        public List<SysSystemEDIJob> SelectSystemEDIJobList(SysSystemEDIJobPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT EDI_JOB_ID, dbo.FN_GET_NMID(EDI_JOB_ID, {EDI_JOB}) AS EDI_JOB_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_EDI_JOB ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND EDI_FLOW_ID={EDI_FLOW_ID} ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemEDIJobPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemEDIJobPara.ParaField.EDI_FLOW_ID.ToString(), Value = para.EDIFlowID });
            dbParameters.Add(new DBParameter { Name = SysSystemEDIJobPara.ParaField.EDI_JOB.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemEDIJobPara.ParaField.EDI_JOB.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemEDIJob> systemEDIJobList = new List<SysSystemEDIJob>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemEDIJob systemEDIJob = new SysSystemEDIJob()
                    {
                        EDIJobID = new DBVarChar(dataRow[SysSystemEDIJob.DataField.EDI_JOB_ID.ToString()]),
                        EDIJobNM = new DBNVarChar(dataRow[SysSystemEDIJob.DataField.EDI_JOB_NM.ToString()])
                    };
                    systemEDIJobList.Add(systemEDIJob);
                }
                return systemEDIJobList;
            }
            return null;
        }

        public class SysSystemEventGroupPara : DBCulture
        {
            public SysSystemEventGroupPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, EVENT_GROUP
            }

            public DBVarChar SysID;
        }

        public class SysSystemEventGroup : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                EVENT_GROUP_ID, EVENT_GROUP
            }

            public DBVarChar EventGroupID;
            public DBNVarChar EventGroup;

            public string ItemText()
            {
                return this.EventGroup.StringValue();
            }

            public string ItemValue()
            {
                return this.EventGroupID.GetValue();
            }

            public string ItemValue(string key)
            {
                throw new System.NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new System.NotImplementedException();
            }

            public string GroupBy()
            {
                throw new System.NotImplementedException();
            }
        }

        public List<SysSystemEventGroup> SelectSystemEventGroupList(SysSystemEventGroupPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT EVENT_GROUP_ID, dbo.FN_GET_NMID(EVENT_GROUP_ID, {EVENT_GROUP}) AS EVENT_GROUP ", Environment.NewLine,
                "FROM SYS_SYSTEM_EVENT_GROUP ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY SORT_ORDER ", Environment.NewLine
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSystemEventGroupPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemEventGroupPara.ParaField.EVENT_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSystemEventGroupPara.ParaField.EVENT_GROUP.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SysSystemEventGroup> systemEventGroupList = new List<SysSystemEventGroup>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSystemEventGroup systemEventGroup = new SysSystemEventGroup()
                    {
                        EventGroupID = new DBVarChar(dataRow[SysSystemEventGroup.DataField.EVENT_GROUP_ID.ToString()]),
                        EventGroup = new DBNVarChar(dataRow[SysSystemEventGroup.DataField.EVENT_GROUP.ToString()])
                    };
                    systemEventGroupList.Add(systemEventGroup);
                }
                return systemEventGroupList;
            }
            return null;
        }

        public class SystemEventPara : DBCulture
        {
            public SystemEventPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, EVENT_GROUP_ID, EVENT_ID,
                SYS_NM, EVENT_GROUP, EVENT_NM
            }

            public DBVarChar SysID;
            public DBVarChar EventGroupID;
            public DBVarChar EventID;
        }

        public class SystemEvent : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM,
                EVENT_GROUP_ID, EVENT_GROUP_NM,
                EVENT_ID, EVENT_NM,
                EVENT_PARA,
                REMARK
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public DBVarChar EventGroupID;
            public DBNVarChar EventGroupNM;

            public DBVarChar EventID;
            public DBNVarChar EventNM;

            public DBVarChar EventPara;
            public DBNVarChar Remark;
        }

        public SystemEvent SelectSystemEvent(SystemEventPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT E.SYS_ID, dbo.FN_GET_NMID(E.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , E.EVENT_GROUP_ID, dbo.FN_GET_NMID(E.EVENT_GROUP_ID, G.{EVENT_GROUP}) AS EVENT_GROUP_NM ", Environment.NewLine,
                "     , E.EVENT_ID, dbo.FN_GET_NMID(E.EVENT_ID, E.{EVENT_NM}) AS EVENT_NM ", Environment.NewLine,
                "     , E.EVENT_PARA ", Environment.NewLine,
                "     , E.REMARK ", Environment.NewLine,
                "FROM SYS_SYSTEM_EVENT E ", Environment.NewLine,
                "JOIN SYS_SYSTEM_EVENT_GROUP G ON E.SYS_ID=G.SYS_ID AND E.EVENT_GROUP_ID=G.EVENT_GROUP_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON E.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "WHERE E.SYS_ID={SYS_ID} AND E.EVENT_GROUP_ID={EVENT_GROUP_ID} AND E.EVENT_ID={EVENT_ID} ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemEventPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemEventPara.ParaField.EVENT_GROUP_ID.ToString(), Value = para.EventGroupID });
            dbParameters.Add(new DBParameter { Name = SystemEventPara.ParaField.EVENT_ID.ToString(), Value = para.EventID });
            dbParameters.Add(new DBParameter { Name = SystemEventPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEventPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEventPara.ParaField.EVENT_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEventPara.ParaField.EVENT_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemEventPara.ParaField.EVENT_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemEventPara.ParaField.EVENT_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                SystemEvent systemEvent = new SystemEvent()
                {
                    SysID = new DBVarChar(dataTable.Rows[0][SystemEvent.DataField.SYS_ID.ToString()]),
                    SysNM = new DBNVarChar(dataTable.Rows[0][SystemEvent.DataField.SYS_NM.ToString()]),
                    EventGroupID = new DBVarChar(dataTable.Rows[0][SystemEvent.DataField.EVENT_GROUP_ID.ToString()]),
                    EventGroupNM = new DBNVarChar(dataTable.Rows[0][SystemEvent.DataField.EVENT_GROUP_NM.ToString()]),
                    EventID = new DBVarChar(dataTable.Rows[0][SystemEvent.DataField.EVENT_ID.ToString()]),
                    EventNM = new DBNVarChar(dataTable.Rows[0][SystemEvent.DataField.EVENT_NM.ToString()]),
                    EventPara = new DBVarChar(dataTable.Rows[0][SystemEvent.DataField.EVENT_PARA.ToString()]),
                    Remark = new DBNVarChar(dataTable.Rows[0][SystemEvent.DataField.REMARK.ToString()])
                };
                return systemEvent;
            }
            return null;
        }

        public class SystemUserDetailPara
        {
            public enum ParaField
            {
                USER_ID
            }

            public DBVarChar UserID;
        }

        public class SystemUserDetail : DBTableRow
        {
            public enum DataField
            {
                USER_ID,
                IS_GRANTOR
            }

            public DBVarChar UserID;
            public DBChar IsGrantor;
        }

        public SystemUserDetail SelectSystemUserDetail(SystemUserDetailPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT USER_ID, IS_GRANTOR ", Environment.NewLine,
                "FROM SYS_USER_DETAIL ", Environment.NewLine,
                "WHERE USER_ID={USER_ID} "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemUserDetailPara.ParaField.USER_ID.ToString(), Value = para.UserID });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                SystemUserDetail systemUserDetail = new SystemUserDetail()
                {
                    UserID = new DBVarChar(dataTable.Rows[0][SystemUserDetail.DataField.USER_ID.ToString()]),
                    IsGrantor = new DBChar(dataTable.Rows[0][SystemUserDetail.DataField.IS_GRANTOR.ToString()])
                };
                return systemUserDetail;
            }
            return null;
        }
    }
}