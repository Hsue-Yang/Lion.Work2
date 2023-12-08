using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemFunDetail : EntitySys
    {
        public EntitySystemFunDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemFunDetailPara
        {
            public enum Field
            {
                SYS_ID, SUB_SYS_ID, PURVIEW_ID,
                FUN_CONTROLLER_ID, FUN_ACTION_NAME,
                FUN_NM_ZH_TW, FUN_NM_ZH_CN, FUN_NM_EN_US, FUN_NM_TH_TH, FUN_NM_JA_JP,
                FUN_TYPE, IS_OUTSIDE, IS_DISABLE, SORT_ORDER,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar SubSysID;
            public DBVarChar PurviewID;

            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;

            public DBNVarChar FunNMZHTW;
            public DBNVarChar FunNMZHCN;
            public DBNVarChar FunNMENUS;
            public DBNVarChar FunNMTHTH;
            public DBNVarChar FunNMJAJP;
            public DBVarChar FunType;
            public DBChar IsOutside;
            public DBChar IsDisable;
            public DBVarChar SortOrder;

            public DBVarChar UpdUserID;
        }

        public class SystemFunDetail : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SUB_SYS_ID, PURVIEW_ID,
                FUN_CONTROLLER_ID, FUN_ACTION_NAME,
                FUN_NM_ZH_TW, FUN_NM_ZH_CN, FUN_NM_EN_US, FUN_NM_TH_TH, FUN_NM_JA_JP,
                FUN_TYPE, IS_OUTSIDE, IS_DISABLE, SORT_ORDER
            }

            public DBVarChar SysID;
            public DBVarChar SubSysID;
            public DBVarChar PurviewID;

            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;

            public DBNVarChar FunNMZHTW;
            public DBNVarChar FunNMZHCN;
            public DBNVarChar FunNMENUS;
            public DBNVarChar FunNMTHTH;
            public DBNVarChar FunNMJAJP;
            public DBVarChar FunType;

            public DBChar IsOutside;
            public DBChar IsDisable;
            public DBVarChar SortOrder;
        }

        public SystemFunDetail SelectSystemFunDetail(SystemFunDetailPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT SYS_ID, SUB_SYS_ID, PURVIEW_ID ", Environment.NewLine,
                "     , FUN_CONTROLLER_ID, FUN_ACTION_NAME ", Environment.NewLine,
                "     , FUN_NM_ZH_TW, FUN_NM_ZH_CN, FUN_NM_EN_US, FUN_NM_TH_TH, FUN_NM_JA_JP ", Environment.NewLine,
                "     , FUN_TYPE, IS_OUTSIDE, IS_DISABLE, SORT_ORDER ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.FUN_ACTION_NAME, Value = para.FunActionName });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                SystemFunDetail systemFunDetail = new SystemFunDetail()
                {
                    SysID = new DBVarChar(dataRow[SystemFunDetail.DataField.SYS_ID.ToString()]),
                    SubSysID = new DBVarChar(dataRow[SystemFunDetail.DataField.SUB_SYS_ID.ToString()]),
                    PurviewID = new DBVarChar(dataRow[SystemFunDetail.DataField.PURVIEW_ID.ToString()]),
                    FunControllerID = new DBVarChar(dataRow[SystemFunDetail.DataField.FUN_CONTROLLER_ID.ToString()]),
                    FunActionName = new DBVarChar(dataRow[SystemFunDetail.DataField.FUN_ACTION_NAME.ToString()]),
                    FunNMZHTW = new DBNVarChar(dataRow[SystemFunDetail.DataField.FUN_NM_ZH_TW.ToString()]),
                    FunNMZHCN = new DBNVarChar(dataRow[SystemFunDetail.DataField.FUN_NM_ZH_CN.ToString()]),
                    FunNMENUS = new DBNVarChar(dataRow[SystemFunDetail.DataField.FUN_NM_EN_US.ToString()]),
                    FunNMTHTH = new DBNVarChar(dataRow[SystemFunDetail.DataField.FUN_NM_TH_TH.ToString()]),
                    FunNMJAJP = new DBNVarChar(dataRow[SystemFunDetail.DataField.FUN_NM_JA_JP.ToString()]),
                    FunType = new DBVarChar(dataRow[SystemFunDetail.DataField.FUN_TYPE.ToString()]),
                    IsOutside = new DBChar(dataRow[SystemFunDetail.DataField.IS_OUTSIDE.ToString()]),
                    IsDisable = new DBChar(dataRow[SystemFunDetail.DataField.IS_DISABLE.ToString()]),
                    SortOrder = new DBVarChar(dataRow[SystemFunDetail.DataField.SORT_ORDER.ToString()])
                };
                return systemFunDetail;
            }
            return null;
        }

        public class SystemFunRolePara : DBCulture
        {
            public SystemFunRolePara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME, SYS_NM, ROLE_NM
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
        }

        public class SystemFunRole : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM, ROLE_ID, ROLE_NM, HAS_ROLE
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
            public DBChar HasRole;
        }

        public List<SystemFunRole> SelectSystemFunRoleList(SystemFunRolePara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT M.SYS_ID, dbo.FN_GET_NMID(M.SYS_ID, S.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , M.ROLE_ID, dbo.FN_GET_NMID(M.ROLE_ID, M.{ROLE_NM}) AS ROLE_NM ", Environment.NewLine,
                "     , (CASE WHEN R.ROLE_ID IS NOT NULL THEN 'Y' ELSE 'N' END) AS HAS_ROLE ", Environment.NewLine,
                "FROM SYS_SYSTEM_ROLE M ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN S ON M.SYS_ID=S.SYS_ID ", Environment.NewLine,
                "LEFT OUTER JOIN (SELECT SYS_ID, ROLE_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME ", Environment.NewLine,
                "                 FROM SYS_SYSTEM_ROLE_FUN ", Environment.NewLine,
                "                 WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}) R ", Environment.NewLine,
                "ON M.SYS_ID=R.SYS_ID AND M.ROLE_ID=R.ROLE_ID ", Environment.NewLine,
                "WHERE M.SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY M.ROLE_ID ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemFunRolePara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunRolePara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunRolePara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = SystemFunRolePara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunRolePara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunRolePara.ParaField.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunRolePara.ParaField.ROLE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemFunRole> systemFunRoleList = new List<SystemFunRole>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemFunRole systemFunRole = new SystemFunRole()
                    {
                        SysID = new DBVarChar(dataRow[SystemFunRole.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemFunRole.DataField.SYS_NM.ToString()]),
                        RoleID = new DBVarChar(dataRow[SystemFunRole.DataField.ROLE_ID.ToString()]),
                        RoleNM = new DBNVarChar(dataRow[SystemFunRole.DataField.ROLE_NM.ToString()]),
                        HasRole = new DBChar(dataRow[SystemFunRole.DataField.HAS_ROLE.ToString()]),
                    };
                    systemFunRoleList.Add(systemFunRole);
                }
                return systemFunRoleList;
            }
            return null;
        }

        public class UserSystemFunRolePara
        {
            public enum ParaField
            {
                SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME, ROLE_ID, UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
            public DBVarChar RoleID;
            public DBVarChar UpdUserID;
        }

        public enum EnumEditSystemFunDetailResult
        {
            Success, Failure
        }

        public EnumEditSystemFunDetailResult EditSystemFunDetail(SystemFunDetailPara para, List<UserSystemFunRolePara> paraList, List<EntitySystemFunDetail.SystemMenuFunValue> systemMenuFunValueList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();

            string mainCommandText = string.Concat(new object[] 
            {
                "DELETE FROM SYS_SYSTEM_FUN ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,

                "DELETE FROM SYS_SYSTEM_ROLE_FUN ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,

                "DELETE FROM SYS_SYSTEM_MENU_FUN ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,

                "INSERT INTO SYS_SYSTEM_FUN VALUES ( ", Environment.NewLine,
                "    {SYS_ID}, {SUB_SYS_ID}, {PURVIEW_ID} ", Environment.NewLine,
                "  , {FUN_CONTROLLER_ID}, {FUN_ACTION_NAME} ", Environment.NewLine,
                "  , {FUN_NM_ZH_TW}, {FUN_NM_ZH_CN}, {FUN_NM_EN_US}, {FUN_NM_TH_TH}, {FUN_NM_JA_JP} ", Environment.NewLine,
                "  , {FUN_TYPE}, {IS_OUTSIDE}, {IS_DISABLE}, {SORT_ORDER} ", Environment.NewLine,
                "  , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "); ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.SUB_SYS_ID, Value = para.SubSysID });
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.PURVIEW_ID, Value = para.PurviewID });
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.FUN_ACTION_NAME, Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.FUN_NM_ZH_TW, Value = para.FunNMZHTW });
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.FUN_NM_ZH_CN, Value = para.FunNMZHCN });
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.FUN_NM_EN_US, Value = para.FunNMENUS });
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.FUN_NM_TH_TH, Value = para.FunNMTHTH });
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.FUN_NM_JA_JP, Value = para.FunNMJAJP });
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.FUN_TYPE, Value = para.FunType });
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.IS_OUTSIDE, Value = para.IsOutside });
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.IS_DISABLE, Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, mainCommandText, dbParameters));
            dbParameters.Clear();

            foreach (UserSystemFunRolePara userSystemFunRolePara in paraList)
            {
                string insertCommand = string.Concat(new object[]
                {
                    "INSERT INTO SYS_SYSTEM_ROLE_FUN VALUES ({SYS_ID}, {ROLE_ID}, {FUN_CONTROLLER_ID}, {FUN_ACTION_NAME}, {UPD_USER_ID}, GETDATE()); ", Environment.NewLine
                });

                dbParameters.Add(new DBParameter { Name = UserSystemFunRolePara.ParaField.SYS_ID, Value = userSystemFunRolePara.SysID });
                dbParameters.Add(new DBParameter { Name = UserSystemFunRolePara.ParaField.ROLE_ID, Value = userSystemFunRolePara.RoleID });
                dbParameters.Add(new DBParameter { Name = UserSystemFunRolePara.ParaField.FUN_CONTROLLER_ID, Value = userSystemFunRolePara.FunControllerID });
                dbParameters.Add(new DBParameter { Name = UserSystemFunRolePara.ParaField.FUN_ACTION_NAME, Value = userSystemFunRolePara.FunActionName });
                dbParameters.Add(new DBParameter { Name = UserSystemFunRolePara.ParaField.UPD_USER_ID, Value = userSystemFunRolePara.UpdUserID });

                commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, insertCommand, dbParameters));
                dbParameters.Clear();
            }

            if (systemMenuFunValueList != null && systemMenuFunValueList.Count > 0)
            {
                foreach (SystemMenuFunValue systemMenuFunValue in systemMenuFunValueList)
                {
                    if (systemMenuFunValue.GetFunMenu().IsNull() == false)
                    {
                        string menuCommand = string.Concat(new object[]
                        {
                            "INSERT INTO SYS_SYSTEM_MENU_FUN VALUES ( ", Environment.NewLine,
                            "    {SYS_ID} ", Environment.NewLine,
                            "  , {FUN_MENU_SYS_ID}, {FUN_MENU} ", Environment.NewLine,
                            "  , {FUN_CONTROLLER_ID}, {FUN_ACTION_NAME} ", Environment.NewLine,
                            "  , {FUN_MENU_XAXIS}, {FUN_MENU_YAXIS} ", Environment.NewLine,
                            "  , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                            "); ", Environment.NewLine
                        });

                        dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.SYS_ID, Value = para.SysID });
                        dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.FUN_CONTROLLER_ID, Value = para.FunControllerID });
                        dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.FUN_ACTION_NAME, Value = para.FunActionName });

                        dbParameters.Add(new DBParameter { Name = SystemMenuFunValue.ValueField.FUN_MENU_SYS_ID, Value = systemMenuFunValue.GetFunMenuSysID() });
                        dbParameters.Add(new DBParameter { Name = SystemMenuFunValue.ValueField.FUN_MENU, Value = systemMenuFunValue.GetFunMenu() });
                        dbParameters.Add(new DBParameter { Name = SystemMenuFunValue.ValueField.FUN_MENU_XAXIS, Value = systemMenuFunValue.GetFunMenuXAxis() });
                        dbParameters.Add(new DBParameter { Name = SystemMenuFunValue.ValueField.FUN_MENU_YAXIS, Value = systemMenuFunValue.GetFunMenuYAxis() });

                        dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.UPD_USER_ID, Value = para.UpdUserID });

                        commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, menuCommand, dbParameters));
                        dbParameters.Clear();
                    }
                }
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
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditSystemFunDetailResult.Success : EnumEditSystemFunDetailResult.Failure;
        }

        public enum EnumDeleteSystemFunDetailResult
        {
            Success, Failure, DataExist
        }

        public EnumDeleteSystemFunDetailResult DeleteSystemFunDetail(SystemFunDetailPara para)
        {
            try
            {
                string commandText = string.Concat(new object[] 
                {
                    "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                    "SET @RESULT = 'N'; ", Environment.NewLine,
                    "BEGIN TRANSACTION ", Environment.NewLine,
                    "    BEGIN TRY ", Environment.NewLine,

                    "        IF NOT EXISTS (SELECT * FROM SYS_USER_FUN WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} AND IS_ASSIGN='Y') ", Environment.NewLine,
                    "        BEGIN ", Environment.NewLine,
                    "            DELETE FROM SYS_SYSTEM_FUN ", Environment.NewLine,
                    "            WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,

                    "            DELETE FROM SYS_SYSTEM_ROLE_FUN ", Environment.NewLine,
                    "            WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,

                    "            DELETE FROM SYS_SYSTEM_MENU_FUN ", Environment.NewLine,
                    "            WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,

                    "            SET @RESULT = 'Y'; ", Environment.NewLine,
                    "        END; ", Environment.NewLine,
                    "        ELSE ", Environment.NewLine,
                    "        BEGIN ", Environment.NewLine,
                    "            SET @RESULT = 'E'; ", Environment.NewLine,
                    "        END; ", Environment.NewLine,
                    "        COMMIT; ", Environment.NewLine,
                    "    END TRY ", Environment.NewLine,
                    "    BEGIN CATCH ", Environment.NewLine,
                    "        SET @RESULT = 'N'; ", Environment.NewLine,
                    "        ROLLBACK TRANSACTION; ", Environment.NewLine,
                    "    END CATCH ", Environment.NewLine,
                    "; ", Environment.NewLine,
                    "SELECT @RESULT; ", Environment.NewLine
                });

                List<DBParameter> dbParameters = new List<DBParameter>();
                dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.SYS_ID, Value = para.SysID });
                dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.FUN_CONTROLLER_ID, Value = para.FunControllerID });
                dbParameters.Add(new DBParameter { Name = SystemFunDetailPara.Field.FUN_ACTION_NAME, Value = para.FunActionName });

                DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
                if (result.GetValue() == EnumYN.Y.ToString())
                {
                    return EnumDeleteSystemFunDetailResult.Success;
                }
                else if (result.GetValue() == EnumYN.N.ToString())
                {
                    return EnumDeleteSystemFunDetailResult.Failure;
                }
                else
                {
                    return EnumDeleteSystemFunDetailResult.DataExist;
                }
            }
            catch
            {
                return EnumDeleteSystemFunDetailResult.Failure;
            }
        }

        public class SystemMenuFunValue : ValueListRow
        {
            public enum ValueField
            {
                FUN_MENU_SYS_ID, FUN_MENU, FUN_MENU_XAXIS, FUN_MENU_YAXIS,
            }

            public string FunMenuSysID { get; set; }
            public string FunMenu { get; set; }
            public string FunMenuXAxis { get; set; }
            public string FunMenuYAxis { get; set; }

            public DBVarChar GetFunMenuSysID()
            {
                return new DBVarChar(FunMenuSysID);
            }

            public DBVarChar GetFunMenu()
            {
                return new DBVarChar(FunMenu);
            }

            public DBVarChar GetFunMenuXAxis()
            {
                return new DBVarChar(FunMenuXAxis);
            }

            public DBVarChar GetFunMenuYAxis()
            {
                return new DBVarChar(FunMenuYAxis);
            }
        }
    }
}