using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemFunToolSetting : EntitySys
    {
        public EntitySystemFunToolSetting(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class FunToolSettingValue : ValueListRow
        {
            public enum ValueField
            {
                UserID,
                SysID, FunControllerID, FunActionName,
                ToolNo, ToolNM,
            }

            public string PickData { get; set; }
            public string UserID { get; set; }
            public string SysID { get; set; }
            public string FunControllerID { get; set; }
            public string FunActionName { get; set; }
            public string ToolNo { get; set; }
            public string ToolNM { get; set; }
            public string IsMoved { get; set; }
            public string AfterSortOrder { get; set; }
            public string BeforeSortOrder { get; set; }

            public DBChar GetPickData()
            {
                return new DBChar(this.PickData);
            }
            public DBVarChar GetUserID()
            {
                return new DBVarChar(this.UserID);
            }
            public DBVarChar GetSysID()
            {
                return new DBVarChar(this.SysID);
            }
            public DBVarChar GetFunControllerID()
            {
                return new DBVarChar(this.FunControllerID);
            }
            public DBVarChar GetFunActionName()
            {
                return new DBVarChar(this.FunActionName);
            }
            public DBChar GetToolNo()
            {
                return new DBChar(this.ToolNo);
            }
            public DBNVarChar GetToolNM()
            {
                return new DBNVarChar(this.ToolNM);
            }
            public DBChar GetIsMoved()
            {
                return new DBChar(this.IsMoved); 
            }
            public DBVarChar GetAfterSortOrder()
            {
                return new DBVarChar(this.AfterSortOrder);
            }
            public DBVarChar GetBeforeSortOrder()
            {
                return new DBVarChar(this.BeforeSortOrder);
            }
        }

        public class FunToolSettingPara : DBCulture
        {
            public FunToolSettingPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                USER_ID,
                SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME,
                TOOL_NO, TOOL_NM,
                UPD_USER_ID, SORT_ORDER,

                SYS_NM, FUN_GROUP, FUN_NM,

                CONDITION, COPY_TO_USER_ID,
                COPY_TOOL_NO,
            }

            public DBVarChar UserID { get; set; }
            public DBVarChar SysID { get; set; }
            public DBVarChar FunControllerID { get; set; }
            public DBVarChar FunActionName { get; set; }
            public DBVarChar UpdUserID { get; set; }
            public DBObject Condition { get; set; }
            public DBVarChar CopyToUserID { get; set; }
            public DBChar ToolNo { get; set; }
        }

        public class FunToolSetting
        {
            public enum DataField
            {
                USER_ID,
                SYS_ID, SYS_NM,
                SUB_SYS_ID, SUB_SYS_NM,
                FUN_CONTROLLER_ID, FUN_GROUP_NM,
                FUN_ACTION_NAME, FUN_NM,
                TOOL_NO, TOOL_NM,
                IS_CURRENTLY, SORT_ORDER,
                UPD_USER_NM, UPD_DT
            }

            public DBVarChar UserID;

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public DBVarChar FunControllerID;
            public DBNVarChar FunGroupNM;

            public DBVarChar FunActionName;
            public DBNVarChar FunNM;

            public DBChar ToolNo;
            public DBNVarChar ToolNM;

            public DBChar IsCurrently;
            public DBVarChar SortOrder;

            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<FunToolSetting> SelectFunToolSettingList(FunToolSettingPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT T.USER_ID ", Environment.NewLine,
                "     , T.SYS_ID, dbo.FN_GET_NMID(T.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , T.FUN_CONTROLLER_ID, dbo.FN_GET_NMID(T.FUN_CONTROLLER_ID, G.{FUN_GROUP}) AS FUN_GROUP_NM ", Environment.NewLine,
                "     , T.FUN_ACTION_NAME, dbo.FN_GET_NMID(T.FUN_ACTION_NAME, F.{FUN_NM}) AS FUN_NM ", Environment.NewLine,
                "     , T.TOOL_NO, T.TOOL_NM ", Environment.NewLine,
                "     , T.IS_CURRENTLY ", Environment.NewLine,
                "     , T.SORT_ORDER ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(T.UPD_USER_ID) AS UPD_USER_NM, T.UPD_DT ", Environment.NewLine,
                "FROM SYS_USER_TOOL T ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON T.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_FUN_GROUP G ON T.SYS_ID=G.SYS_ID AND T.FUN_CONTROLLER_ID=G.FUN_CONTROLLER_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_FUN F ON T.SYS_ID=F.SYS_ID AND T.FUN_CONTROLLER_ID=F.FUN_CONTROLLER_ID AND T.FUN_ACTION_NAME=F.FUN_ACTION_NAME ", Environment.NewLine,
                "WHERE T.USER_ID={USER_ID} AND T.SYS_ID={SYS_ID} AND T.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND T.FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                "ORDER BY T.SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(FunToolSettingPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(FunToolSettingPara.ParaField.FUN_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(FunToolSettingPara.ParaField.FUN_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<FunToolSetting> funToolSettingList = new List<FunToolSetting>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    FunToolSetting funToolSetting = new FunToolSetting()
                    {
                        UserID = new DBVarChar(dataRow[FunToolSetting.DataField.USER_ID.ToString()]),

                        SysID = new DBVarChar(dataRow[FunToolSetting.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[FunToolSetting.DataField.SYS_NM.ToString()]),

                        FunControllerID = new DBVarChar(dataRow[FunToolSetting.DataField.FUN_CONTROLLER_ID.ToString()]),
                        FunGroupNM = new DBNVarChar(dataRow[FunToolSetting.DataField.FUN_GROUP_NM.ToString()]),

                        FunActionName = new DBVarChar(dataRow[FunToolSetting.DataField.FUN_ACTION_NAME.ToString()]),
                        FunNM = new DBNVarChar(dataRow[FunToolSetting.DataField.FUN_NM.ToString()]),

                        ToolNo = new DBChar(dataRow[FunToolSetting.DataField.TOOL_NO.ToString()]),
                        ToolNM = new DBNVarChar(dataRow[FunToolSetting.DataField.TOOL_NM.ToString()]),

                        IsCurrently = new DBChar(dataRow[FunToolSetting.DataField.IS_CURRENTLY.ToString()]),
                        SortOrder = new DBVarChar(dataRow[FunToolSetting.DataField.SORT_ORDER.ToString()]),

                        UpdUserNM = new DBNVarChar(dataRow[FunToolSetting.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[FunToolSetting.DataField.UPD_DT.ToString()])
                    };
                    funToolSettingList.Add(funToolSetting);
                }
                return funToolSettingList;
            }
            return null;
        }

        public class SysSearchSystemFunControllerIDPara : DBCulture
        {
            public SysSearchSystemFunControllerIDPara(string culture)
                : base(culture)
            {

            }

            public enum ParaField
            {
                SYS_ID, FUN_GROUP,
                CONDITION, FUN_NM
            }

            public DBVarChar SysID;
            public DBObject Condition; 
        }

        public class SysSearchSystemControllerID : DBTableRow, ISelectItem
        {
            public enum DataField
            {
                FUN_CONTROLLER_ID, FUN_GROUP_NM,
            }


            public DBVarChar FunControllerID;
            public DBVarChar FunGroupNM;
            

            public string ItemText()
            {
                return this.FunGroupNM.StringValue();
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

        public List<SysSearchSystemControllerID> SelectSearchSysSystemFunControllerIDList(SysSearchSystemFunControllerIDPara para) 
        {
            string commandWhere = string.Empty;

            if (para.Condition.GetValue() != null) 
            {
                commandWhere = string.Concat(new object[] { "  AND F.{FUN_NM} LIKE '%{CONDITION}%' " });
            }

            string commandText = string.Concat(new object[] 
            { 
               "SELECT G.FUN_CONTROLLER_ID ", Environment.NewLine,
               "     , dbo.FN_GET_NMID(G.FUN_CONTROLLER_ID, G.{FUN_GROUP}) AS FUN_GROUP_NM ", Environment.NewLine,
               "FROM SYS_SYSTEM_FUN_GROUP G ", Environment.NewLine,
               "LEFT JOIN SYS_SYSTEM_FUN F ON G.SYS_ID=F.SYS_ID AND G.FUN_CONTROLLER_ID=F.FUN_CONTROLLER_ID ", Environment.NewLine,
               "WHERE G.SYS_ID={SYS_ID} ", Environment.NewLine,
               commandWhere, Environment.NewLine,
               "GROUP BY G.FUN_CONTROLLER_ID, G.{FUN_GROUP}, G.SORT_ORDER, G.SYS_ID ", Environment.NewLine,
               "ORDER BY G.SYS_ID, G.SORT_ORDER ", Environment.NewLine,
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSearchSystemFunControllerIDPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSearchSystemFunControllerIDPara.ParaField.CONDITION, Value = para.Condition });
            dbParameters.Add(new DBParameter { Name = SysSearchSystemFunControllerIDPara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSearchSystemFunControllerIDPara.ParaField.FUN_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SysSearchSystemFunControllerIDPara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSearchSystemFunControllerIDPara.ParaField.FUN_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0) 
            {
                List<SysSearchSystemControllerID> sysSearchSystemControllerIDList = new List<SysSearchSystemControllerID>();
                foreach(DataRow dataRow in dataTable.Rows)
                {
                    SysSearchSystemControllerID sysSearchSystemControllerID = new SysSearchSystemControllerID()
                    {
                        FunControllerID = new DBVarChar(dataRow[SysSearchSystemControllerID.DataField.FUN_CONTROLLER_ID.ToString()]),
                        FunGroupNM = new DBVarChar(dataRow[SysSearchSystemControllerID.DataField.FUN_GROUP_NM.ToString()]),
                    };
                    sysSearchSystemControllerIDList.Add(sysSearchSystemControllerID);
                }
                return sysSearchSystemControllerIDList;
            }
            return null;
        }

        public class SysSearchSystemFunNamePara : DBCulture
        {
            public SysSearchSystemFunNamePara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, FUN_NM, FUN_CONTROLLER_ID,
                CONDITION
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBObject Condition;
        }

        public class SysSearchSystemFunName : DBTableRow, ISelectItem
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

        public List<SysSearchSystemFunName> SelectSelectSearchSysSystemFunControllerIDList(SysSearchSystemFunNamePara para) 
        {
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.Condition.GetValue())) 
            {
                commandWhere = string.Concat(new object[] { commandWhere,  "  AND {FUN_NM} LIKE '%{CONDITION}%' ", Environment.NewLine, });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT FUN_ACTION_NAME, dbo.FN_GET_NMID(FUN_ACTION_NAME, {FUN_NM}) AS FUN_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} ", Environment.NewLine,
                commandWhere, Environment.NewLine,
                "ORDER BY SYS_ID, SORT_ORDER ", Environment.NewLine,
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SysSearchSystemFunNamePara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSearchSystemFunNamePara.ParaField.CONDITION.ToString(), Value = para.Condition });
            dbParameters.Add(new DBParameter { Name = SysSearchSystemFunNamePara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SysSearchSystemFunNamePara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SysSearchSystemFunNamePara.ParaField.FUN_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0) 
            {
                List<SysSearchSystemFunName> sysSearchSystemFunNameList = new List<SysSearchSystemFunName>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysSearchSystemFunName sysSearchSystemFunName = new SysSearchSystemFunName()
                    {
                        FunActionName = new DBVarChar(dataRow[SysSearchSystemFunName.DataField.FUN_ACTION_NAME.ToString()]),
                        FunName = new DBVarChar(dataRow[SysSearchSystemFunName.DataField.FUN_NM.ToString()]),
                    };
                    sysSearchSystemFunNameList.Add(sysSearchSystemFunName);
                }
                return sysSearchSystemFunNameList;
            }
            return null;
        }


        public enum EnumEditFunToolSettingResult
        {
            Success, Failure
        }

        public EnumEditFunToolSettingResult EditFunToolSetting(FunToolSettingPara para, List<FunToolSettingValue> funToolSettingValueList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            string updateCommand = string.Empty;
            foreach (FunToolSettingValue funToolSettingValue in funToolSettingValueList)
            {
                if (funToolSettingValue.GetPickData().GetValue() == EnumYN.Y.ToString() || funToolSettingValue.GetAfterSortOrder().GetValue() != funToolSettingValue.GetBeforeSortOrder().GetValue())
                {
                    updateCommand = string.Concat(new object[]
                    {
                        "        UPDATE SYS_USER_TOOL SET ", Environment.NewLine,
                        "            TOOL_NM={TOOL_NM} ", Environment.NewLine,
                        "          , SORT_ORDER={SORT_ORDER} ", Environment.NewLine,
                        "          , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                        "          , UPD_DT=GETDATE() ", Environment.NewLine,
                        "        WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                        "          AND USER_ID={USER_ID} AND TOOL_NO={TOOL_NO}; ", Environment.NewLine
                    });

                    dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.USER_ID, Value = funToolSettingValue.GetUserID() });
                    dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.SYS_ID, Value = funToolSettingValue.GetSysID() });
                    dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.FUN_CONTROLLER_ID, Value = funToolSettingValue.GetFunControllerID() });
                    dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.FUN_ACTION_NAME, Value = funToolSettingValue.GetFunActionName() });
                    dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.TOOL_NO, Value = funToolSettingValue.GetToolNo() });
                    dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.TOOL_NM, Value = funToolSettingValue.GetToolNM() });
                    dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.SORT_ORDER, Value = funToolSettingValue.GetAfterSortOrder() });
                    dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                    commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, updateCommand, dbParameters));
                    dbParameters.Clear();
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
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditFunToolSettingResult.Success : EnumEditFunToolSettingResult.Failure;
        }

        public enum EnumDeleteFunToolSettingResult
        {
            Success, Failure
        }

        public EnumDeleteFunToolSettingResult DeleteFunToolSetting(FunToolSettingPara para, List<FunToolSettingValue> funToolSettingValueList)
        {
            StringBuilder commandTextStringBuilder = new StringBuilder();
            List<DBParameter> dbParameters = new List<DBParameter>();

            string deleteCommand = string.Empty;
            foreach (FunToolSettingValue funToolSettingValue in funToolSettingValueList)
            {
                if (funToolSettingValue.GetPickData().GetValue() == EnumYN.Y.ToString())
                {
                    deleteCommand = string.Concat(new object[]
                    {
                        "        DELETE FROM SYS_USER_TOOL ", Environment.NewLine,
                        "        WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                        "          AND USER_ID={USER_ID} AND TOOL_NO={TOOL_NO}; ", Environment.NewLine,

                        "        DELETE FROM SYS_USER_TOOL_PARA ", Environment.NewLine,
                        "        WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                        "          AND USER_ID={USER_ID} AND TOOL_NO={TOOL_NO}; ", Environment.NewLine
                    });

                    dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.USER_ID, Value = funToolSettingValue.GetUserID() });
                    dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.SYS_ID, Value = funToolSettingValue.GetSysID() });
                    dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.FUN_CONTROLLER_ID, Value = funToolSettingValue.GetFunControllerID() });
                    dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.FUN_ACTION_NAME, Value = funToolSettingValue.GetFunActionName() });
                    dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.TOOL_NO, Value = funToolSettingValue.GetToolNo() });

                    commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, deleteCommand, dbParameters));
                    dbParameters.Clear();
                }
            }

            string updateCommand = string.Concat(new object[]
            {
                "        DECLARE @USER_ID VARCHAR(20); ", Environment.NewLine,
                "        DECLARE @TOOL_NO CHAR(6); ", Environment.NewLine,
                
                "        SELECT @USER_ID=USER_ID ", Environment.NewLine,
                "        FROM SYS_USER_TOOL ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "          AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} AND IS_CURRENTLY='Y'; ", Environment.NewLine,
                
                "        IF (LEN(ISNULL(@USER_ID,''))=0) ", Environment.NewLine,
                "        BEGIN ", Environment.NewLine,
	            "            SELECT @TOOL_NO=MAX(TOOL_NO) ", Environment.NewLine,
                "            FROM SYS_USER_TOOL ", Environment.NewLine,
                "            WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "              AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,

                "            UPDATE SYS_USER_TOOL ", Environment.NewLine,
                "            SET IS_CURRENTLY='N' ", Environment.NewLine,
                "              , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "              , UPD_DT=GETDATE() ", Environment.NewLine,
                "            WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "              AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,
             
                "            UPDATE SYS_USER_TOOL ", Environment.NewLine,
                "            SET IS_CURRENTLY='Y' ", Environment.NewLine,
                "              , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "              , UPD_DT=GETDATE() ", Environment.NewLine,
                "            WHERE USER_ID={USER_ID} ", Environment.NewLine,
                "              AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                "              AND TOOL_NO=@TOOL_NO; ", Environment.NewLine,
                "        END; ", Environment.NewLine,
            });

            dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, updateCommand, dbParameters));
            dbParameters.Clear();

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
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumDeleteFunToolSettingResult.Success : EnumDeleteFunToolSettingResult.Failure;
        }

        public enum EnumCopyFunToolSettingResult 
        {
            Success, Failure
        }

        public EnumCopyFunToolSettingResult CopyFunToolSetting(FunToolSettingPara para, List<FunToolSettingValue> funToolSettingValueList) 
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            StringBuilder commandTextStringBuilder = new StringBuilder();
            string copyCommand = string.Empty;

            if (funToolSettingValueList != null) 
            { 
                foreach(FunToolSettingValue funToolSettingValue in funToolSettingValueList)
                {
                    if (funToolSettingValue.GetPickData().GetValue() == EnumYN.Y.ToString()) 
                    {
                        copyCommand = string.Concat(new object[] 
                        { 
                          "    SELECT @TOOL_NO=RIGHT('00000'+CAST(ISNULL(CAST(MAX(TOOL_NO) AS INT),0)+1 AS VARCHAR),6) ", Environment.NewLine,
                          "         , @SORT_ORDER=RIGHT('00000'+CAST(ISNULL(CAST(MAX(SORT_ORDER) AS INT),0)+1 AS VARCHAR),6) ", Environment.NewLine,
                          "    FROM SYS_USER_TOOL ", Environment.NewLine,
                          "    WHERE USER_ID={COPY_TO_USER_ID} ", Environment.NewLine,
                          "      AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,
                               
                          "    IF (LEN(ISNULL({TOOL_NO},''))=6 AND {TOOL_NO}=@TOOL_NO) ", Environment.NewLine,
                          "    BEGIN ", Environment.NewLine,
                          "        SET @TOOL_NO=RIGHT('00000'+CAST(ISNULL(CAST(@TOOL_NO AS INT),0)+1 AS VARCHAR),6); ", Environment.NewLine,
                          "    END; ", Environment.NewLine,
                               
                          "    UPDATE SYS_USER_TOOL ", Environment.NewLine,
                          "    SET IS_CURRENTLY='N' ", Environment.NewLine,
                          "      , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                          "      , UPD_DT=GETDATE() ", Environment.NewLine,
                          "    WHERE USER_ID={COPY_TO_USER_ID} ", Environment.NewLine,
                          "      AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine,
                               
                          "    INSERT INTO SYS_USER_TOOL ", Environment.NewLine,
                          "    SELECT {COPY_TO_USER_ID}, SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME, @TOOL_NO, (TOOL_NM + '-' + {USER_ID} + @USER_NM) AS TOOL_NM ", Environment.NewLine,
                          "         , 'Y', @SORT_ORDER, {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                          "    FROM SYS_USER_TOOL ", Environment.NewLine,
                          "    WHERE USER_ID={USER_ID} ", Environment.NewLine,
                          "      AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} AND TOOL_NO={COPY_TOOL_NO}; ", Environment.NewLine,
                               
                          "    INSERT INTO SYS_USER_TOOL_PARA ", Environment.NewLine,
                          "    SELECT {COPY_TO_USER_ID}, SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME, @TOOL_NO, PARA_ID, PARA_VALUE, {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                          "    FROM SYS_USER_TOOL_PARA ", Environment.NewLine,
                          "    WHERE USER_ID={USER_ID} ", Environment.NewLine,
                          "      AND SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} AND TOOL_NO={COPY_TOOL_NO}; ", Environment.NewLine,

                        });
                        dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.USER_ID, Value = funToolSettingValue.GetUserID() });
                        dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.TOOL_NO, Value = para.ToolNo });
                        dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.COPY_TOOL_NO, Value = funToolSettingValue.GetToolNo() });
                        dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.SYS_ID, Value = funToolSettingValue.GetSysID() });
                        dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.FUN_CONTROLLER_ID, Value = funToolSettingValue.GetFunControllerID() });
                        dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.FUN_ACTION_NAME, Value = funToolSettingValue.GetFunActionName() });
                        dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.COPY_TO_USER_ID, Value = para.CopyToUserID });
                        dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

                        commandTextStringBuilder.Append(DBEntity.GetCommandText(base.ProviderName, copyCommand, dbParameters));
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

                "    DECLARE @USER_NM NVARCHAR(150); ", Environment.NewLine,
                "    DECLARE @TOOL_NO CHAR(6); ", Environment.NewLine,
                "    DECLARE @SORT_ORDER VARCHAR(6) ", Environment.NewLine,

                "    SELECT @USER_NM=USER_NM FROM RAW_CM_USER WHERE USER_ID={USER_ID} ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = FunToolSettingPara.ParaField.USER_ID, Value = para.UserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumCopyFunToolSettingResult.Success : EnumCopyFunToolSettingResult.Failure;
        }
    }
}