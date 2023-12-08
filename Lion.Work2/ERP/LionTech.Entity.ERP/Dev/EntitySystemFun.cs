using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Dev
{
    public class EntitySystemFun : EntityDev
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
                SYS_ID, FUN_CONTROLLER_ID, FUN_MENU_SYS_ID, FUN_MENU,
                SYS_NM, FUN_GROUP, FUN_NM, EVENT_GROUP, EVENT_NM, CODE_NM
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunMenuSysID;
            public DBVarChar FunMenu;
        }

        public class SystemFun : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM, FUN_CONTROLLER_ID, FUN_GROUP, FUN_ACTION_NAME, FUN_NM,
                IS_DISABLE, FUN_TYPE, FUN_TYPE_NM,
                DEV_PHASE, DEV_PHASE_NM, DEV_OWNER, USER_NM,
                PRE_BEGIN_DATE, PRE_END_DATE, PRE_WORK_HOURS,
                ACT_BEGIN_DATE, ACT_END_DATE, ACT_WORK_HOURS
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar FunControllerID;
            public DBNVarChar FunGroup;
            public DBVarChar FunActionName;
            public DBNVarChar FunNM;

            public DBChar IsDisable;
            public DBVarChar FunType;
            public DBNVarChar FunTypeNM;

            public DBVarChar DevPhase;
            public DBNVarChar DevPhaseNM;
            public DBVarChar DevOwner;
            public DBNVarChar UserNM;
            
            public DBChar PreBeginDate;
            public DBChar PreEndDate;
            public DBNumeric PreWorkHours;

            public DBChar ActBeginDate;
            public DBChar ActEndDate;
            public DBNumeric ActWorkHours;
        }

        public List<SystemFun> SelectSystemFunList(SystemFunPara para)
        {
            #region - commandWhere -
            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.FunControllerID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND F.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.FunMenuSysID.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND D.FUN_MENU_SYS_ID={FUN_MENU_SYS_ID} ", Environment.NewLine });
            }

            if (!string.IsNullOrWhiteSpace(para.FunMenu.GetValue()))
            {
                commandWhere = string.Concat(new object[] { commandWhere, "  AND D.FUN_MENU={FUN_MENU} ", Environment.NewLine });
            }
            #endregion

            #region - commandJoin -
            string commandJoin = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.FunMenuSysID.GetValue()))
            {
                commandJoin = string.Concat(new object[] {
                    commandJoin,
                    "LEFT JOIN SYS_SYSTEM_MENU_FUN D ON F.SYS_ID=D.SYS_ID ", Environment.NewLine,
                    "                               AND F.FUN_CONTROLLER_ID=D.FUN_CONTROLLER_ID ", Environment.NewLine,
                    "                               AND F.FUN_ACTION_NAME=D.FUN_ACTION_NAME ", Environment.NewLine
                });
            }
            #endregion

            string commandText = string.Concat(new object[]
            {
                "SELECT F.SYS_ID, dbo.FN_GET_NMID(F.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , F.FUN_CONTROLLER_ID, dbo.FN_GET_NMID(F.FUN_CONTROLLER_ID, G.{FUN_GROUP}) AS FUN_GROUP ", Environment.NewLine,
                "     , F.FUN_ACTION_NAME, dbo.FN_GET_NMID(F.FUN_ACTION_NAME, F.{FUN_NM}) AS FUN_NM ", Environment.NewLine,
                "     , F.IS_DISABLE ", Environment.NewLine,
                "     , F.FUN_TYPE, dbo.FN_GET_NMID(F.FUN_TYPE, C1.{CODE_NM}) AS FUN_TYPE_NM ", Environment.NewLine,
                "     , S.DEV_PHASE, (CASE WHEN S.DEV_PHASE IS NULL THEN NULL ELSE dbo.FN_GET_NMID(S.DEV_PHASE, C2.{CODE_NM}) END) AS DEV_PHASE_NM ", Environment.NewLine,
                "     , S.DEV_OWNER, dbo.FN_GET_USER_NM(S.DEV_OWNER) AS USER_NM ", Environment.NewLine,
                "     , S.PRE_BEGIN_DATE, S.PRE_END_DATE, S.PRE_WORK_HOURS ", Environment.NewLine,
                "     , S.ACT_BEGIN_DATE, S.ACT_END_DATE, S.ACT_WORK_HOURS ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN F ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON F.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_FUN_GROUP G ON F.SYS_ID=G.SYS_ID AND F.FUN_CONTROLLER_ID=G.FUN_CONTROLLER_ID ", Environment.NewLine,
                "JOIN CM_CODE C1 ON C1.CODE_KIND='0011' AND F.FUN_TYPE=C1.CODE_ID ", Environment.NewLine,
                "LEFT JOIN DEV_FUN_SCHEDULE S ON F.SYS_ID=S.SYS_ID ", Environment.NewLine,
                "                            AND F.FUN_CONTROLLER_ID=S.FUN_CONTROLLER_ID ", Environment.NewLine,
                "                            AND F.FUN_ACTION_NAME=S.FUN_ACTION_NAME ", Environment.NewLine,
                "                            AND S.DEV_PHASE=dbo.FN_GET_DEV_PHASE(S.SYS_ID, S.FUN_CONTROLLER_ID, S.FUN_ACTION_NAME) ", Environment.NewLine,
                "LEFT JOIN CM_CODE C2 ON C2.CODE_KIND='0012' AND S.DEV_PHASE=C2.CODE_ID ", Environment.NewLine,
                commandJoin,
                "WHERE F.SYS_ID={SYS_ID} ", Environment.NewLine,
                commandWhere,
                "ORDER BY F.SORT_ORDER "
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_MENU_SYS_ID.ToString(), Value = para.FunMenuSysID });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_MENU.ToString(), Value = para.FunMenu });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.FUN_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.FUN_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.CODE_NM.ToString())) });

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
                        FunControllerID = new DBVarChar(dataRow[SystemFun.DataField.FUN_CONTROLLER_ID.ToString()]),
                        FunGroup = new DBNVarChar(dataRow[SystemFun.DataField.FUN_GROUP.ToString()]),
                        FunActionName = new DBVarChar(dataRow[SystemFun.DataField.FUN_ACTION_NAME.ToString()]),
                        FunNM = new DBNVarChar(dataRow[SystemFun.DataField.FUN_NM.ToString()]),
                        IsDisable = new DBChar(dataRow[SystemFun.DataField.IS_DISABLE.ToString()]),
                        FunType = new DBVarChar(dataRow[SystemFun.DataField.FUN_TYPE.ToString()]),
                        FunTypeNM = new DBNVarChar(dataRow[SystemFun.DataField.FUN_TYPE_NM.ToString()]),
                        DevPhase = new DBVarChar(dataRow[SystemFun.DataField.DEV_PHASE.ToString()]),
                        DevPhaseNM = new DBNVarChar(dataRow[SystemFun.DataField.DEV_PHASE_NM.ToString()]),
                        DevOwner = new DBVarChar(dataRow[SystemFun.DataField.DEV_OWNER.ToString()]),
                        UserNM = new DBNVarChar(dataRow[SystemFun.DataField.USER_NM.ToString()]),
                        PreBeginDate = new DBChar(dataRow[SystemFun.DataField.PRE_BEGIN_DATE.ToString()]),
                        PreEndDate = new DBChar(dataRow[SystemFun.DataField.PRE_END_DATE.ToString()]),
                        PreWorkHours = new DBNumeric(dataRow[SystemFun.DataField.PRE_WORK_HOURS.ToString()]),
                        ActBeginDate = new DBChar(dataRow[SystemFun.DataField.ACT_BEGIN_DATE.ToString()]),
                        ActEndDate = new DBChar(dataRow[SystemFun.DataField.ACT_END_DATE.ToString()]),
                        ActWorkHours = new DBNumeric(dataRow[SystemFun.DataField.ACT_WORK_HOURS.ToString()])
                    };
                    systemFunList.Add(systemFun);
                }
                return systemFunList;
            }
            return null;
        }

        public List<SystemFun> SelectSystemEventTargetList(SystemFunPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT F.TARGET_SYS_ID AS SYS_ID, dbo.FN_GET_NMID(F.TARGET_SYS_ID, M2.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , F.EVENT_GROUP_ID AS FUN_CONTROLLER_ID, dbo.FN_GET_NMID(F.SYS_ID, M1.{SYS_NM})+' '+dbo.FN_GET_NMID(F.EVENT_GROUP_ID, G.{EVENT_GROUP}) AS FUN_GROUP ", Environment.NewLine,
                "     , F.EVENT_ID AS FUN_ACTION_NAME, dbo.FN_GET_NMID(F.EVENT_ID, F.EVENT_NM) AS FUN_NM ", Environment.NewLine,
                "     , F.IS_DISABLE ", Environment.NewLine,
                "     , NULL AS FUN_TYPE, NULL AS FUN_TYPE_NM ", Environment.NewLine,
                "     , S.DEV_PHASE, (CASE WHEN S.DEV_PHASE IS NULL THEN NULL ELSE dbo.FN_GET_NMID(S.DEV_PHASE, C.{CODE_NM}) END) AS DEV_PHASE_NM ", Environment.NewLine,
                "     , S.DEV_OWNER, dbo.FN_GET_USER_NM(S.DEV_OWNER) AS USER_NM ", Environment.NewLine,
                "     , S.PRE_BEGIN_DATE, S.PRE_END_DATE, S.PRE_WORK_HOURS ", Environment.NewLine,
                "     , S.ACT_BEGIN_DATE, S.ACT_END_DATE, S.ACT_WORK_HOURS ", Environment.NewLine,
                "FROM (SELECT T.SYS_ID, T.EVENT_GROUP_ID, T.EVENT_ID, E.{EVENT_NM} AS EVENT_NM ", Environment.NewLine,
                "           , T.TARGET_SYS_ID, E.IS_DISABLE, E.SORT_ORDER ", Environment.NewLine,
                "      FROM SYS_SYSTEM_EVENT_TARGET T JOIN SYS_SYSTEM_EVENT E ", Environment.NewLine,
                "      ON T.SYS_ID=E.SYS_ID AND T.EVENT_GROUP_ID=E.EVENT_GROUP_ID AND T.EVENT_ID=E.EVENT_ID) F ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M1 ON F.SYS_ID=M1.SYS_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_EVENT_GROUP G ON F.SYS_ID=G.SYS_ID AND F.EVENT_GROUP_ID=G.EVENT_GROUP_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M2 ON F.TARGET_SYS_ID=M2.SYS_ID ", Environment.NewLine,
                "LEFT JOIN DEV_FUN_SCHEDULE S ON F.TARGET_SYS_ID=S.SYS_ID ", Environment.NewLine,
                "                            AND F.EVENT_GROUP_ID=S.FUN_CONTROLLER_ID ", Environment.NewLine,
                "                            AND F.EVENT_ID=S.FUN_ACTION_NAME ", Environment.NewLine,
                "                            AND S.DEV_PHASE=dbo.FN_GET_DEV_PHASE(S.SYS_ID, S.FUN_CONTROLLER_ID, S.FUN_ACTION_NAME) ", Environment.NewLine,
                "LEFT JOIN CM_CODE C ON C.CODE_KIND='0012' AND S.DEV_PHASE=C.CODE_ID ", Environment.NewLine,
                "WHERE F.TARGET_SYS_ID={SYS_ID} ", Environment.NewLine,
                "ORDER BY F.SORT_ORDER ", Environment.NewLine
            });
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.EVENT_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.EVENT_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.EVENT_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.EVENT_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.CODE_NM.ToString())) });

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
                        FunControllerID = new DBVarChar(dataRow[SystemFun.DataField.FUN_CONTROLLER_ID.ToString()]),
                        FunGroup = new DBNVarChar(dataRow[SystemFun.DataField.FUN_GROUP.ToString()]),
                        FunActionName = new DBVarChar(dataRow[SystemFun.DataField.FUN_ACTION_NAME.ToString()]),
                        FunNM = new DBNVarChar(dataRow[SystemFun.DataField.FUN_NM.ToString()]),
                        IsDisable = new DBChar(dataRow[SystemFun.DataField.IS_DISABLE.ToString()]),
                        FunType = new DBVarChar(dataRow[SystemFun.DataField.FUN_TYPE.ToString()]),
                        FunTypeNM = new DBNVarChar(dataRow[SystemFun.DataField.FUN_TYPE_NM.ToString()]),
                        DevPhase = new DBVarChar(dataRow[SystemFun.DataField.DEV_PHASE.ToString()]),
                        DevPhaseNM = new DBNVarChar(dataRow[SystemFun.DataField.DEV_PHASE_NM.ToString()]),
                        DevOwner = new DBVarChar(dataRow[SystemFun.DataField.DEV_OWNER.ToString()]),
                        UserNM = new DBNVarChar(dataRow[SystemFun.DataField.USER_NM.ToString()]),
                        PreBeginDate = new DBChar(dataRow[SystemFun.DataField.PRE_BEGIN_DATE.ToString()]),
                        PreEndDate = new DBChar(dataRow[SystemFun.DataField.PRE_END_DATE.ToString()]),
                        PreWorkHours = new DBNumeric(dataRow[SystemFun.DataField.PRE_WORK_HOURS.ToString()]),
                        ActBeginDate = new DBChar(dataRow[SystemFun.DataField.ACT_BEGIN_DATE.ToString()]),
                        ActEndDate = new DBChar(dataRow[SystemFun.DataField.ACT_END_DATE.ToString()]),
                        ActWorkHours = new DBNumeric(dataRow[SystemFun.DataField.ACT_WORK_HOURS.ToString()])
                    };
                    systemFunList.Add(systemFun);
                }
                return systemFunList;
            }
            return null;
        }
    }
}