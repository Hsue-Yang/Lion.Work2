using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemFunToolPara : EntitySys
    {
        public EntitySystemFunToolPara(string connectionString, string providerName)
            : base(connectionString, providerName)
        { 
        }

        public class SystemFunToolPara : DBCulture
        {
            public SystemFunToolPara(string cultrueID) 
                : base(cultrueID)
            { 
            }

            public enum ParaField 
            {
                USER_ID, SYS_ID,
                FUN_CONTROLLER_ID,
                FUN_ACTION_NAME,
                TOOL_NO, SYS_NM,
                FUN_GROUP, FUN_NM,
            }
            public DBVarChar UserID;
            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
            public DBChar ToolNo;
        }

        public class SystemFunTool
        {
            public enum DataField 
            {
                USER_ID, SYS_ID,
                SYS_NM, 
                FUN_CONTROLLER_ID,
                FUN_GROUP_NM, 
                FUN_ACTION_NAME,
                FUN_NM,TOOL_NO,
                TOOL_NM,IS_CURRENTLY, 
                PARA_ID, PARA_VALUE,
                SORT_ORDER,
                UPD_USER_NM, UPD_DT,
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
            public DBVarChar ParaID;
            public DBNVarChar ParaValue;
            public DBChar IsCurrently;
            public DBVarChar SortOrder;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDt;
        }

        public SystemFunTool SelectSystemFunToolParaFormList(SystemFunToolPara para) 
        {
            string commandText = string.Concat(new object[] 
            { 
               "SELECT T.USER_ID ", Environment.NewLine,
                "    , T.SYS_ID, dbo.FN_GET_NMID(T.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "    , T.FUN_CONTROLLER_ID, dbo.FN_GET_NMID(T.FUN_CONTROLLER_ID, G.{FUN_GROUP}) AS FUN_GROUP_NM ", Environment.NewLine,
                "    , T.FUN_ACTION_NAME, dbo.FN_GET_NMID(T.FUN_ACTION_NAME, F.{FUN_NM}) AS FUN_NM ", Environment.NewLine,
                "    , T.TOOL_NO, T.TOOL_NM ", Environment.NewLine,
                "    , T.IS_CURRENTLY ", Environment.NewLine,
                "    , T.SORT_ORDER ", Environment.NewLine,
              " FROM SYS_USER_TOOL T ", Environment.NewLine,
              " JOIN SYS_SYSTEM_MAIN M ON T.SYS_ID=M.SYS_ID ", Environment.NewLine,
              " JOIN SYS_SYSTEM_FUN_GROUP G ON T.SYS_ID=G.SYS_ID AND T.FUN_CONTROLLER_ID=G.FUN_CONTROLLER_ID ", Environment.NewLine,
              " JOIN SYS_SYSTEM_FUN F ON T.SYS_ID=F.SYS_ID AND T.FUN_CONTROLLER_ID=F.FUN_CONTROLLER_ID AND T.FUN_ACTION_NAME=F.FUN_ACTION_NAME ", Environment.NewLine,
              " WHERE T.USER_ID={USER_ID} AND T.SYS_ID={SYS_ID} AND T.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND T.FUN_ACTION_NAME={FUN_ACTION_NAME} AND T.TOOL_NO={TOOL_NO} ", Environment.NewLine,
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemFunToolPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = SystemFunToolPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunToolPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunToolPara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = SystemFunToolPara.ParaField.TOOL_NO.ToString(), Value = para.ToolNo });
            dbParameters.Add(new DBParameter { Name = SystemFunToolPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunToolPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunToolPara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunToolPara.ParaField.FUN_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunToolPara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunToolPara.ParaField.FUN_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText,dbParameters);
            if (dataTable.Rows.Count == 1) 
            {
                DataRow dataRow = dataTable.Rows[0];
                SystemFunTool systemFunTool = new SystemFunTool() 
                {
                    UserID = new DBVarChar(dataRow[SystemFunTool.DataField.USER_ID.ToString()]),
                    SysID = new DBVarChar(dataRow[SystemFunTool.DataField.SYS_ID.ToString()]),
                    SysNM = new DBNVarChar(dataRow[SystemFunTool.DataField.SYS_NM.ToString()]),
                    FunControllerID = new DBVarChar(dataRow[SystemFunTool.DataField.FUN_CONTROLLER_ID.ToString()]),
                    FunGroupNM = new DBNVarChar(dataRow[SystemFunTool.DataField.FUN_GROUP_NM.ToString()]),
                    FunActionName = new DBVarChar(dataRow[SystemFunTool.DataField.FUN_ACTION_NAME.ToString()]),
                    FunNM = new DBNVarChar(dataRow[SystemFunTool.DataField.FUN_NM.ToString()]),
                    IsCurrently = new DBChar(dataRow[SystemFunTool.DataField.IS_CURRENTLY.ToString()]),
                    SortOrder = new DBVarChar(dataRow[SystemFunTool.DataField.SORT_ORDER.ToString()]),
                    ToolNM = new DBNVarChar(dataRow[SystemFunTool.DataField.TOOL_NM.ToString()]),
                    ToolNo = new DBChar(dataRow[SystemFunTool.DataField.TOOL_NO.ToString()]),
                };
                return systemFunTool;
            }
            return null;
        }

        public List<SystemFunTool> SelectSystemFunToolParaList(SystemFunToolPara para) 
        {
            string commandText = string.Concat(new object[]
            { 
               " SELECT T.PARA_ID ", Environment.NewLine,
		       "      , T.PARA_VALUE ", Environment.NewLine,
               " FROM SYS_USER_TOOL_PARA T ", Environment.NewLine,
               " WHERE T.USER_ID={USER_ID} AND T.SYS_ID={SYS_ID} AND T.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND T.FUN_ACTION_NAME={FUN_ACTION_NAME} AND T.TOOL_NO={TOOL_NO} ", Environment.NewLine,
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemFunToolPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = SystemFunToolPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunToolPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunToolPara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = SystemFunToolPara.ParaField.TOOL_NO.ToString(), Value = para.ToolNo });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemFunTool> systemFunToolList = new List<SystemFunTool>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemFunTool systemFunTool = new SystemFunTool()
                    {
                        ParaID = new DBVarChar(dataRow[SystemFunTool.DataField.PARA_ID.ToString()]),
                        ParaValue = new DBNVarChar(dataRow[SystemFunTool.DataField.PARA_VALUE.ToString()]),
                    };
                    systemFunToolList.Add(systemFunTool);
                }
                return systemFunToolList;
            }
            return null;
        }
    }
}
