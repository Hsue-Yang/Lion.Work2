using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Sys
{
    public class EntityADSReport : EntitySys
    {
        public EntityADSReport(string connectionString, string providerName)
            : base(connectionString, providerName)
        {   
        }

        public enum EnumReportType
        {
            SyRoleToFunction,
            SyUserToFunction,
            SySingleFunctionAwarded,
            SysLastTime,
        }

        public class ADSReportPara 
        {
            public enum ParaField
            {
                SYS_ID,
            }
            public DBVarChar SysID;
        }

        public class ADSReport 
        {
            public enum DataField
            {
                SYSTEM_NM, ROLE_NM,
                FUNCTION_NM, USER_NM,
                UNIT_NM, IS_DISABLE,
                IS_LEFT, LAST_TIME,
                SYSTEM_ROLE_NM
            }
            public DBNVarChar SystemNM;
            public DBNVarChar SystemRoleNM;
            public DBNVarChar RoleNM;
            public DBNVarChar FunctionNM;
            public DBNVarChar UserNM;
            public DBNVarChar UnitNM;
            public DBChar IsDisable;
            public DBChar IsLeft;
            public DBDateTime LastTime;
        }

        public List<ADSReport> SelectSyRoleToFunction(ADSReportPara para) 
        {
            string commandWhere = string.Empty;

            if (para.SysID.GetValue() != null)
            {
                commandWhere = string.Concat(new object[] { commandWhere, " WHERE F.SYS_ID={SYS_ID} " });
            }

            string commandText = string.Concat(new object[] 
            { 
               " SELECT dbo.FN_GET_NMID(F.SYS_ID, M.SYS_NM_ZH_TW) AS SYSTEM_NM ", Environment.NewLine,
               "      , dbo.FN_GET_NMID(F.ROLE_ID, R.ROLE_NM_ZH_TW)  AS SYSTEM_ROLE_NM ", Environment.NewLine,
               "      , (SELECT FUN_NM_ZH_TW FROM SYS_SYSTEM_FUN WHERE SYS_ID=F.SYS_ID AND FUN_CONTROLLER_ID=F.FUN_CONTROLLER_ID AND FUN_ACTION_NAME=F.FUN_ACTION_NAME)+'('+F.FUN_CONTROLLER_ID+'/'+F.FUN_ACTION_NAME+')' AS FUNCTION_NM ", Environment.NewLine,
               " FROM SYS_SYSTEM_ROLE_FUN F ", Environment.NewLine,
               " LEFT JOIN SYS_SYSTEM_ROLE R ON F.SYS_ID=R.SYS_ID AND F.ROLE_ID=R.ROLE_ID ", Environment.NewLine,
               " LEFT JOIN SYS_SYSTEM_MAIN M ON F.SYS_ID=M.SYS_ID ", Environment.NewLine,
                 commandWhere , Environment.NewLine,
               " ORDER BY F.SYS_ID, F.ROLE_ID, F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME ", Environment.NewLine,
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            if (para.SysID.GetValue() != null)
            {
                dbParameters.Add(new DBParameter { Name = ADSReportPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            }

            DataTable dataTables = base.GetDataTable(commandText, dbParameters);
            if (dataTables.Rows.Count > 0)
            {
                List<ADSReport> aDSReportList = new List<ADSReport>();
                foreach (DataRow dataRow in dataTables.Rows)
                {
                    ADSReport aDSReport = new ADSReport()
                    {
                        SystemNM = new DBNVarChar(dataRow[ADSReport.DataField.SYSTEM_NM.ToString()]),
                        SystemRoleNM = new DBNVarChar(dataRow[ADSReport.DataField.SYSTEM_ROLE_NM.ToString()]),
                        FunctionNM = new DBNVarChar(dataRow[ADSReport.DataField.FUNCTION_NM.ToString()]),
                    };
                    aDSReportList.Add(aDSReport);
                }
                return aDSReportList;
            }
            return null;
        }

        public List<ADSReport> SelectSyUserToFunction(ADSReportPara para) 
        {
            string commandWhere = string.Empty;

            if (para.SysID.GetValue() != null)
            {
                commandWhere = string.Concat(new object[] { commandWhere, " WHERE R.SYS_ID={SYS_ID} " });
            }

            string commandText = string.Concat(new object[] 
            { 
                " SELECT dbo.FN_GET_NMID(R.SYS_ID, M.SYS_NM_ZH_TW) AS SYSTEM_NM ", Environment.NewLine,
                "      , dbo.FN_GET_NMID(R.USER_ID, U.USER_NM) AS USER_NM ", Environment.NewLine,
                "      , dbo.FN_GET_NMID(O.USER_UNIT_ID, C.UNIT_NM) AS UNIT_NM ", Environment.NewLine,
                "      , dbo.FN_GET_NMID(R.ROLE_ID, S.ROLE_NM_ZH_TW) AS ROLE_NM ", Environment.NewLine,
                " FROM SYS_USER_SYSTEM_ROLE R ", Environment.NewLine,
                " JOIN RAW_CM_USER O ON R.USER_ID=O.USER_ID ", Environment.NewLine,
                " LEFT JOIN SYS_SYSTEM_MAIN M ON R.SYS_ID=M.SYS_ID ", Environment.NewLine,
                " LEFT JOIN RAW_CM_USER U ON R.USER_ID=U.USER_ID ", Environment.NewLine,
                " LEFT JOIN RAW_CM_ORG_UNIT C ON C.UNIT_ID=O.USER_UNIT_ID ", Environment.NewLine,
                " LEFT JOIN SYS_SYSTEM_ROLE S ON R.SYS_ID=S.SYS_ID AND R.ROLE_ID=S.ROLE_ID ", Environment.NewLine,
                  commandWhere, Environment.NewLine,
                " ORDER BY R.SYS_ID, R.USER_ID, R.ROLE_ID ", Environment.NewLine,
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            if (para.SysID.GetValue() != null)
            {
                dbParameters.Add(new DBParameter { Name = ADSReportPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            }

            DataTable dataTables = base.GetDataTable(commandText, dbParameters);
            if (dataTables.Rows.Count > 0)
            {
                List<ADSReport> aDSReportList = new List<ADSReport>();
                foreach (DataRow dataRow in dataTables.Rows)
                {
                    ADSReport aDSReport = new ADSReport()
                    {
                        SystemNM = new DBNVarChar(dataRow[ADSReport.DataField.SYSTEM_NM.ToString()]),
                        RoleNM = new DBNVarChar(dataRow[ADSReport.DataField.ROLE_NM.ToString()]),
                        UserNM = new DBNVarChar(dataRow[ADSReport.DataField.USER_NM.ToString()]),
                        UnitNM = new DBNVarChar(dataRow[ADSReport.DataField.UNIT_NM.ToString()]),
                    };
                    aDSReportList.Add(aDSReport);
                }
                return aDSReportList;
            }
            return null;
        }

        public List<ADSReport> SelectSySingleFunctionAwarded(ADSReportPara para)
        {
            string commandWhere = string.Empty;

            if (para.SysID.GetValue() != null)
            {
                commandWhere = string.Concat(new object[] { commandWhere, " AND F.SYS_ID={SYS_ID} " });
            }

            string commandText = string.Concat(new object[] 
            { 
              " SELECT dbo.FN_GET_NMID(F.SYS_ID, M.SYS_NM_ZH_TW) AS SYSTEM_NM ", Environment.NewLine,
              "      , (SELECT FUN_NM_ZH_TW FROM SYS_SYSTEM_FUN WHERE SYS_ID=F.SYS_ID AND FUN_CONTROLLER_ID=F.FUN_CONTROLLER_ID AND FUN_ACTION_NAME=F.FUN_ACTION_NAME)+'('+F.FUN_CONTROLLER_ID+'/'+F.FUN_ACTION_NAME+')' AS FUNCTION_NM ", Environment.NewLine,
              "      , dbo.FN_GET_NMID(F.USER_ID, U.USER_NM) AS USER_NM ", Environment.NewLine,
              " FROM SYS_USER_FUN F ", Environment.NewLine,
              " LEFT JOIN SYS_SYSTEM_MAIN M ON F.SYS_ID=M.SYS_ID ", Environment.NewLine,
              " LEFT JOIN RAW_CM_USER U ON F.USER_ID=U.USER_ID ", Environment.NewLine,
              " WHERE IS_ASSIGN='Y' ", Environment.NewLine,
                commandWhere , Environment.NewLine,
              " ORDER BY F.SYS_ID, F.FUN_CONTROLLER_ID, F.FUN_ACTION_NAME, F.USER_ID ", Environment.NewLine,
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            if (para.SysID.GetValue() != null)
            {
                dbParameters.Add(new DBParameter { Name = ADSReportPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            }

            DataTable dataTables = base.GetDataTable(commandText, dbParameters);
            if (dataTables.Rows.Count > 0)
            {
                List<ADSReport> aDSReportList = new List<ADSReport>();
                foreach (DataRow dataRow in dataTables.Rows)
                {
                    ADSReport aDSReport = new ADSReport()
                    {
                        SystemNM = new DBNVarChar(dataRow[ADSReport.DataField.SYSTEM_NM.ToString()]),
                        FunctionNM = new DBNVarChar(dataRow[ADSReport.DataField.FUNCTION_NM.ToString()]),
                        UserNM = new DBNVarChar(dataRow[ADSReport.DataField.USER_NM.ToString()]),
                    };
                    aDSReportList.Add(aDSReport);
                }
                return aDSReportList;
            }
            return null;
        }

        public List<ADSReport> SelectSysLastTime(ADSReportPara para)
        {
            string commandText = string.Concat(new object[] 
            { 
             " SELECT dbo.FN_GET_NMID(M.USER_ID, U.USER_NM) AS USER_NM ", Environment.NewLine,
             "      , M.IS_DISABLE AS IS_DISABLE ", Environment.NewLine,
             "      , M.IS_LEFT AS IS_LEFT ", Environment.NewLine,
             "      , Z.LAST_CONNECT_DT AS LAST_TIME ", Environment.NewLine,
             " FROM SYS_USER_MAIN M ", Environment.NewLine,
             " LEFT JOIN RAW_CM_USER U ON M.USER_ID=U.USER_ID ", Environment.NewLine,
             " LEFT JOIN( ", Environment.NewLine,
             "     SELECT USER_ID, MAX(LAST_CONNECT_DT) AS LAST_CONNECT_DT ", Environment.NewLine,
             "     FROM SYS_USER_CONNECT ", Environment.NewLine,
             "     GROUP BY USER_ID ", Environment.NewLine,
             " ) Z ON M.USER_ID=Z.USER_ID ", Environment.NewLine,
             " ORDER BY M.USER_ID ", Environment.NewLine,
            });

            DataTable dataTables = base.GetDataTable(commandText, null);
            if (dataTables.Rows.Count > 0)
            {
                List<ADSReport> aDSReportList = new List<ADSReport>();
                foreach (DataRow dataRow in dataTables.Rows)
                {
                    ADSReport aDSReport = new ADSReport()
                    {
                        UserNM = new DBNVarChar(dataRow[ADSReport.DataField.USER_NM.ToString()]),
                        IsDisable = new DBChar(dataRow[ADSReport.DataField.IS_DISABLE.ToString()]),
                        IsLeft = new DBChar(dataRow[ADSReport.DataField.IS_LEFT.ToString()]),
                        LastTime = new DBDateTime(dataRow[ADSReport.DataField.LAST_TIME.ToString()]),
                    };
                    aDSReportList.Add(aDSReport);
                }
                return aDSReportList;
            }
            return null;
        }
    }
}
