using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using LionTech.Utility;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemFun : EntitySys
    {
        public EntitySystemFun(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemFunPara : DBCulture
        {
            public SystemFunPara(string cultureID)
                : base(cultureID)
            {
            
            }

            public enum ParaField
            {
                SYS_ID, SUB_SYS_ID, PURVIEW_ID,
                FUN_CONTROLLER_ID, FUN_CONTROLLER_NM, FUN_ACTION_NM, FUN_ACTION_NAME,
                FUN_MENU_SYS_ID, FUN_MENU,
                UPD_USER_ID,

                SYS_NM, FUN_GROUP, FUN_MENU_NM, FUN_NM
            }

            public DBVarChar SysID;
            public DBVarChar SubSysID;
            public DBVarChar PurviewID;
            public DBVarChar FunControllerID;
            public DBObject FunControllerNM;
            public DBObject FunActionNM;
            public DBVarChar FunActionName;
            public DBVarChar FunNM;
            public DBVarChar FunMenuSysID;
            public DBVarChar FunMenu;
            public DBVarChar UpdUserID;
        }

        public class SystemFun : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM,
                SUB_SYS_ID, SUB_SYS_NM,
                FUN_CONTROLLER_ID, FUN_GROUP,
                FUN_ACTION_NAME, FUN_NM,
                FUN_IS_DISABLE,
                IS_OUTSIDE, SORT_ORDER,
                UPD_USER_NM, UPD_DT
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public DBVarChar SubSysID;
            public DBNVarChar SubSysNM;

            public List<SystemMenuFun> MenuList;

            public DBVarChar FunControllerID;
            public DBNVarChar FunGroup;

            public DBVarChar FunActionName;
            public DBNVarChar FunNM;

            public DBChar FunIsDisable;

            public DBChar IsOutSide;
            public DBVarChar SortOrder;

            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;

            public string GetPrimaryKey()
            {
                return string.Format("{0}|{1}|{2}",
                    this.SysID.GetValue(),
                    this.FunControllerID.GetValue(),
                    this.FunActionName.GetValue()
                );
            }

            public string GetFullURL()
            {
                return string.Format("{0}/{1}/{2}",
                    Common.GetEnumDesc(Utility.GetSystemID(this.SubSysID.GetValue())),
                    this.FunControllerID.GetValue(),
                    this.FunActionName.GetValue()
                );
            }
        }

        public List<SystemFun> SelectSystemFunList(SystemFunPara para)
        {
            #region - commandWhere -
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.SubSysID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND F.SUB_SYS_ID={SUB_SYS_ID} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.FunControllerID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND F.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.FunNM.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND F.FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.FunControllerNM.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND (G.FUN_GROUP_ZH_TW LIKE '%{FUN_CONTROLLER_NM}%' OR G.FUN_GROUP_ZH_CN LIKE '%{FUN_CONTROLLER_NM}%' OR G.FUN_GROUP_EN_US LIKE '%{FUN_CONTROLLER_NM}%') ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.FunActionNM.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND (F.FUN_NM_ZH_TW LIKE '%{FUN_ACTION_NM}%' OR F.FUN_NM_ZH_CN LIKE '%{FUN_ACTION_NM}%' OR F.FUN_NM_EN_US LIKE '%{FUN_ACTION_NM}%') ", Environment.NewLine });
            }
            #endregion

            #region - subquery -
            string subquery = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.FunMenuSysID.GetValue()))
            {
                subquery = string.Concat(new object[] {
                    subquery,
                    "  AND F.SYS_ID+F.FUN_CONTROLLER_ID+F.FUN_ACTION_NAME IN (SELECT SYS_ID+FUN_CONTROLLER_ID+FUN_ACTION_NAME ", Environment.NewLine,
                    "                                                         FROM SYS_SYSTEM_MENU_FUN ", Environment.NewLine,
                    "                                                         WHERE SYS_ID={SYS_ID} ", Environment.NewLine,
                    "                                                           AND FUN_MENU_SYS_ID=(CASE WHEN ISNULL({FUN_MENU_SYS_ID},'NULL')='NULL' THEN FUN_MENU_SYS_ID ELSE {FUN_MENU_SYS_ID} END) ", Environment.NewLine,
                    "                                                           AND FUN_MENU=(CASE WHEN ISNULL({FUN_MENU},'NULL')='NULL' THEN FUN_MENU ELSE {FUN_MENU} END) ", Environment.NewLine,
                    "                                                         GROUP BY SYS_ID+FUN_CONTROLLER_ID+FUN_ACTION_NAME) ", Environment.NewLine
                });
            }
            #endregion

            string commandText = string.Concat(new object[]
            {
                "SELECT F.SYS_ID, dbo.FN_GET_NMID(F.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , F.SUB_SYS_ID, (CASE WHEN F.SUB_SYS_ID=F.SYS_ID THEN NULL ELSE dbo.FN_GET_NMID(F.SUB_SYS_ID, S.{SYS_NM}) END) AS SUB_SYS_NM ", Environment.NewLine,
                "     , F.FUN_CONTROLLER_ID, dbo.FN_GET_NMID(F.FUN_CONTROLLER_ID, G.{FUN_GROUP}) AS FUN_GROUP ", Environment.NewLine,
                "     , F.FUN_ACTION_NAME, dbo.FN_GET_NMID(F.FUN_ACTION_NAME, F.{FUN_NM}) AS FUN_NM ", Environment.NewLine,
                "     , F.IS_DISABLE AS FUN_IS_DISABLE ", Environment.NewLine,
                "     , F.IS_OUTSIDE, F.SORT_ORDER ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(F.UPD_USER_ID) AS UPD_USER_NM, F.UPD_DT ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN F ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON F.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_SUB S ON F.SYS_ID=S.PARENT_SYS_ID AND F.SUB_SYS_ID=S.SYS_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_FUN_GROUP G ON F.SYS_ID=G.SYS_ID AND F.FUN_CONTROLLER_ID=G.FUN_CONTROLLER_ID ", Environment.NewLine,
                "WHERE F.SYS_ID={SYS_ID} ", Environment.NewLine,
                commandWhere,
                subquery,
                "ORDER BY F.SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.SUB_SYS_ID.ToString(), Value = para.SubSysID });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            if (!string.IsNullOrWhiteSpace(para.FunControllerNM.GetValue()))
            {
                dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_CONTROLLER_NM.ToString(), Value = para.FunControllerNM });
            }
            if (!string.IsNullOrWhiteSpace(para.FunActionNM.GetValue()))
            {
                dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_ACTION_NM.ToString(), Value = para.FunActionNM });
            }
            if (!string.IsNullOrWhiteSpace(para.FunNM.GetValue()))
            {
                dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunNM });
            }
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_MENU_SYS_ID.ToString(), Value = para.FunMenuSysID });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_MENU.ToString(), Value = para.FunMenu });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.FUN_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_MENU_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.FUN_MENU_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.FUN_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemFun> systemFunList = new List<SystemFun>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemFun systemFun = new SystemFun()
                    {
                        SysID = new DBVarChar(dataRow[SystemFun.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemFun.DataField.SYS_NM.ToString()]),

                        SubSysID = new DBVarChar(dataRow[SystemFun.DataField.SUB_SYS_ID.ToString()]),
                        SubSysNM = new DBNVarChar(dataRow[SystemFun.DataField.SUB_SYS_NM.ToString()]),

                        FunControllerID = new DBVarChar(dataRow[SystemFun.DataField.FUN_CONTROLLER_ID.ToString()]),
                        FunGroup = new DBNVarChar(dataRow[SystemFun.DataField.FUN_GROUP.ToString()]),

                        FunActionName = new DBVarChar(dataRow[SystemFun.DataField.FUN_ACTION_NAME.ToString()]),
                        FunNM = new DBNVarChar(dataRow[SystemFun.DataField.FUN_NM.ToString()]),

                        FunIsDisable = new DBChar(dataRow[SystemFun.DataField.FUN_IS_DISABLE.ToString()]),

                        IsOutSide = new DBChar(dataRow[SystemFun.DataField.IS_OUTSIDE.ToString()]),
                        SortOrder = new DBVarChar(dataRow[SystemFun.DataField.SORT_ORDER.ToString()]),

                        UpdUserNM = new DBNVarChar(dataRow[SystemFun.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[SystemFun.DataField.UPD_DT.ToString()]),
                    };
                    systemFunList.Add(systemFun);
                }
                return systemFunList;
            }
            return null;
        }

        public enum EnumEditSystemFunResult
        {
            Success, Failure, NotExecuted
        }

        public EnumEditSystemFunResult EditSystemFun(List<SystemFunPara> paraList)
        {
            if (paraList != null && paraList.Count > 0)
            {
                StringBuilder commandTextStringBuilder = new StringBuilder();
                List<DBParameter> dbParameters = new List<DBParameter>();

                string updateCommand = string.Empty;
                foreach (SystemFunPara systemFunPara in paraList)
                {
                    updateCommand = string.Concat(new object[]
                    {
                        "        UPDATE SYS_SYSTEM_FUN SET PURVIEW_ID={PURVIEW_ID}, UPD_USER_ID={UPD_USER_ID}, UPD_DT=GETDATE() ",
                        "WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine
                    });

                    dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.SYS_ID, Value = systemFunPara.SysID });
                    dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.PURVIEW_ID, Value = systemFunPara.PurviewID });
                    dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_CONTROLLER_ID, Value = systemFunPara.FunControllerID });
                    dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_ACTION_NAME, Value = systemFunPara.FunActionName });
                    dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.UPD_USER_ID, Value = systemFunPara.UpdUserID });

                    commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, updateCommand, dbParameters));
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
                return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditSystemFunResult.Success : EnumEditSystemFunResult.Failure;
            }

            return EnumEditSystemFunResult.NotExecuted;
        }
    }
}