using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Dev
{
    public class EntityFunSchedule : EntityDev
    {
        public EntityFunSchedule(string connectionString, string providerName)
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
                SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME, DEV_PHASE, 
                SYS_NM, FUN_GROUP, FUN_NM, EVENT_GROUP, EVENT_NM, CODE_NM
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
            public DBVarChar DevPhase;
        }

        public class SystemFun : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM, FUN_CONTROLLER_ID, FUN_GROUP, FUN_ACTION_NAME, FUN_NM,
                IS_DISABLE, FUN_TYPE, FUN_TYPE_NM,
                DEV_PHASE, DEV_OWNER,
                PRE_BEGIN_DATE, PRE_END_DATE, PRE_WORK_HOURS,
                ACT_BEGIN_DATE, ACT_END_DATE, ACT_WORK_HOURS,
                REMARK
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
            public DBVarChar DevOwner;

            public DBChar PreBeginDate;
            public DBChar PreEndDate;
            public DBNumeric PreWorkHours;

            public DBChar ActBeginDate;
            public DBChar ActEndDate;
            public DBNumeric ActWorkHours;

            public DBNVarChar Remark;
        }

        public SystemFun SelectSystemFun(SystemFunPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT F.SYS_ID, dbo.FN_GET_NMID(F.SYS_ID, M.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , F.FUN_CONTROLLER_ID, dbo.FN_GET_NMID(F.FUN_CONTROLLER_ID, G.{FUN_GROUP}) AS FUN_GROUP ", Environment.NewLine,
                "     , F.FUN_ACTION_NAME, dbo.FN_GET_NMID(F.FUN_ACTION_NAME, F.{FUN_NM}) AS FUN_NM ", Environment.NewLine,
                "     , F.IS_DISABLE ", Environment.NewLine,
                "     , F.FUN_TYPE, dbo.FN_GET_NMID(F.FUN_TYPE, C1.{CODE_NM}) AS FUN_TYPE_NM ", Environment.NewLine,
                "     , S.DEV_PHASE, S.DEV_OWNER ", Environment.NewLine,
                "     , S.PRE_BEGIN_DATE, S.PRE_END_DATE, S.PRE_WORK_HOURS ", Environment.NewLine,
                "     , S.ACT_BEGIN_DATE, S.ACT_END_DATE, S.ACT_WORK_HOURS ", Environment.NewLine,
                "     , S.REMARK ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN F ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON F.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_FUN_GROUP G ON F.SYS_ID=G.SYS_ID AND F.FUN_CONTROLLER_ID=G.FUN_CONTROLLER_ID ", Environment.NewLine,
                "JOIN CM_CODE C1 ON C1.CODE_KIND='0011' AND F.FUN_TYPE=C1.CODE_ID ", Environment.NewLine,
                "LEFT JOIN DEV_FUN_SCHEDULE S ON F.SYS_ID=S.SYS_ID ", Environment.NewLine,
                "                            AND F.FUN_CONTROLLER_ID=S.FUN_CONTROLLER_ID ", Environment.NewLine,
                "                            AND F.FUN_ACTION_NAME=S.FUN_ACTION_NAME ", Environment.NewLine,
                "                            AND S.DEV_PHASE={DEV_PHASE} ", Environment.NewLine,
                "WHERE F.SYS_ID={SYS_ID} AND F.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND F.FUN_ACTION_NAME={FUN_ACTION_NAME}; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.DEV_PHASE.ToString(), Value = para.DevPhase });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.FUN_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.FUN_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                SystemFun systemFun = new SystemFun()
                {
                    SysID = new DBVarChar(dataTable.Rows[0][SystemFun.DataField.SYS_ID.ToString()]),
                    SysNM = new DBNVarChar(dataTable.Rows[0][SystemFun.DataField.SYS_NM.ToString()]),
                    FunControllerID = new DBVarChar(dataTable.Rows[0][SystemFun.DataField.FUN_CONTROLLER_ID.ToString()]),
                    FunGroup = new DBNVarChar(dataTable.Rows[0][SystemFun.DataField.FUN_GROUP.ToString()]),
                    FunActionName = new DBVarChar(dataTable.Rows[0][SystemFun.DataField.FUN_ACTION_NAME.ToString()]),
                    FunNM = new DBNVarChar(dataTable.Rows[0][SystemFun.DataField.FUN_NM.ToString()]),
                    IsDisable = new DBChar(dataTable.Rows[0][SystemFun.DataField.IS_DISABLE.ToString()]),
                    FunType = new DBVarChar(dataTable.Rows[0][SystemFun.DataField.FUN_TYPE.ToString()]),
                    FunTypeNM = new DBNVarChar(dataTable.Rows[0][SystemFun.DataField.FUN_TYPE_NM.ToString()]),
                    DevPhase = new DBVarChar(dataTable.Rows[0][SystemFun.DataField.DEV_PHASE.ToString()]),
                    DevOwner = new DBVarChar(dataTable.Rows[0][SystemFun.DataField.DEV_OWNER.ToString()]),
                    PreBeginDate = new DBChar(dataTable.Rows[0][SystemFun.DataField.PRE_BEGIN_DATE.ToString()]),
                    PreEndDate = new DBChar(dataTable.Rows[0][SystemFun.DataField.PRE_END_DATE.ToString()]),
                    PreWorkHours = new DBNumeric(dataTable.Rows[0][SystemFun.DataField.PRE_WORK_HOURS.ToString()]),
                    ActBeginDate = new DBChar(dataTable.Rows[0][SystemFun.DataField.ACT_BEGIN_DATE.ToString()]),
                    ActEndDate = new DBChar(dataTable.Rows[0][SystemFun.DataField.ACT_END_DATE.ToString()]),
                    ActWorkHours = new DBNumeric(dataTable.Rows[0][SystemFun.DataField.ACT_WORK_HOURS.ToString()]),
                    Remark = new DBNVarChar(dataTable.Rows[0][SystemFun.DataField.REMARK.ToString()])
                };

                return systemFun;
            }
            return null;
        }

        public SystemFun SelectSystemEventTarget(SystemFunPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT F.TARGET_SYS_ID AS SYS_ID, dbo.FN_GET_NMID(F.TARGET_SYS_ID, M2.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , F.EVENT_GROUP_ID AS FUN_CONTROLLER_ID, dbo.FN_GET_NMID(F.SYS_ID, M1.{SYS_NM})+' '+dbo.FN_GET_NMID(F.EVENT_GROUP_ID, G.{EVENT_GROUP}) AS FUN_GROUP ", Environment.NewLine,
                "     , F.EVENT_ID AS FUN_ACTION_NAME, dbo.FN_GET_NMID(F.EVENT_ID, F.EVENT_NM) AS FUN_NM ", Environment.NewLine,
                "     , F.IS_DISABLE ", Environment.NewLine,
                "     , NULL AS FUN_TYPE, NULL AS FUN_TYPE_NM ", Environment.NewLine,
                "     , S.DEV_PHASE, S.DEV_OWNER ", Environment.NewLine,
                "     , S.PRE_BEGIN_DATE, S.PRE_END_DATE, S.PRE_WORK_HOURS ", Environment.NewLine,
                "     , S.ACT_BEGIN_DATE, S.ACT_END_DATE, S.ACT_WORK_HOURS ", Environment.NewLine,
                "     , S.REMARK ", Environment.NewLine,
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
                "                            AND S.DEV_PHASE={DEV_PHASE} ", Environment.NewLine,
                "WHERE F.TARGET_SYS_ID={SYS_ID} AND F.EVENT_GROUP_ID={FUN_CONTROLLER_ID} AND F.EVENT_ID={FUN_ACTION_NAME}; ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.DEV_PHASE.ToString(), Value = para.DevPhase });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.SYS_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.EVENT_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.EVENT_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.EVENT_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.EVENT_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemFunPara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemFunPara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count == 1)
            {
                SystemFun systemFun = new SystemFun()
                {
                    SysID = new DBVarChar(dataTable.Rows[0][SystemFun.DataField.SYS_ID.ToString()]),
                    SysNM = new DBNVarChar(dataTable.Rows[0][SystemFun.DataField.SYS_NM.ToString()]),
                    FunControllerID = new DBVarChar(dataTable.Rows[0][SystemFun.DataField.FUN_CONTROLLER_ID.ToString()]),
                    FunGroup = new DBNVarChar(dataTable.Rows[0][SystemFun.DataField.FUN_GROUP.ToString()]),
                    FunActionName = new DBVarChar(dataTable.Rows[0][SystemFun.DataField.FUN_ACTION_NAME.ToString()]),
                    FunNM = new DBNVarChar(dataTable.Rows[0][SystemFun.DataField.FUN_NM.ToString()]),
                    IsDisable = new DBChar(dataTable.Rows[0][SystemFun.DataField.IS_DISABLE.ToString()]),
                    FunType = new DBVarChar(dataTable.Rows[0][SystemFun.DataField.FUN_TYPE.ToString()]),
                    FunTypeNM = new DBNVarChar(dataTable.Rows[0][SystemFun.DataField.FUN_TYPE_NM.ToString()]),
                    DevPhase = new DBVarChar(dataTable.Rows[0][SystemFun.DataField.DEV_PHASE.ToString()]),
                    DevOwner = new DBVarChar(dataTable.Rows[0][SystemFun.DataField.DEV_OWNER.ToString()]),
                    PreBeginDate = new DBChar(dataTable.Rows[0][SystemFun.DataField.PRE_BEGIN_DATE.ToString()]),
                    PreEndDate = new DBChar(dataTable.Rows[0][SystemFun.DataField.PRE_END_DATE.ToString()]),
                    PreWorkHours = new DBNumeric(dataTable.Rows[0][SystemFun.DataField.PRE_WORK_HOURS.ToString()]),
                    ActBeginDate = new DBChar(dataTable.Rows[0][SystemFun.DataField.ACT_BEGIN_DATE.ToString()]),
                    ActEndDate = new DBChar(dataTable.Rows[0][SystemFun.DataField.ACT_END_DATE.ToString()]),
                    ActWorkHours = new DBNumeric(dataTable.Rows[0][SystemFun.DataField.ACT_WORK_HOURS.ToString()]),
                    Remark = new DBNVarChar(dataTable.Rows[0][SystemFun.DataField.REMARK.ToString()])
                };

                return systemFun;
            }
            return null;
        }

        public class FunSchedulePara : DBCulture
        {
            public FunSchedulePara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME,
                IS_FUN, DEV_PHASE, DEV_OWNER,
                PRE_BEGIN_DATE, PRE_END_DATE, PRE_WORK_HOURS,
                ACT_BEGIN_DATE, ACT_END_DATE, ACT_WORK_HOURS,
                REMARK, UPD_USER_ID,
                CODE_NM
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;

            public DBChar IsFun;
            public DBVarChar DevPhase;
            public DBVarChar DevOwner;

            public DBChar PreBeginDate;
            public DBChar PreEndDate;
            public DBNumeric PreWorkHours;

            public DBChar ActBeginDate;
            public DBChar ActEndDate;
            public DBNumeric ActWorkHours;

            public DBNVarChar Remark;
            public DBVarChar UpdUserID;
        }

        public class FunSchedule : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME,
                DEV_PHASE, DEV_PHASE_NM, DEV_OWNER, DEV_OWNER_NM,
                PRE_BEGIN_DATE, PRE_END_DATE, PRE_WORK_HOURS,
                ACT_BEGIN_DATE, ACT_END_DATE, ACT_WORK_HOURS,
                REMARK, UPD_USER_ID, UPD_USER_NM, UPD_DT
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;

            public DBVarChar DevPhase;
            public DBNVarChar DevPhaseNM;
            public DBVarChar DevOwner;
            public DBNVarChar DevOwnerNM;

            public DBChar PreBeginDate;
            public DBChar PreEndDate;
            public DBNumeric PreWorkHours;

            public DBChar ActBeginDate;
            public DBChar ActEndDate;
            public DBNumeric ActWorkHours;

            public DBNVarChar Remark;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<FunSchedule> SelectFunScheduleList(FunSchedulePara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT S.SYS_ID, S.FUN_CONTROLLER_ID, S.FUN_ACTION_NAME ", Environment.NewLine,
                "     , S.DEV_PHASE, dbo.FN_GET_NMID(S.DEV_PHASE, C.{CODE_NM}) AS DEV_PHASE_NM ", Environment.NewLine,
                "     , S.DEV_OWNER, dbo.FN_GET_USER_NM(S.DEV_OWNER) AS DEV_OWNER_NM ", Environment.NewLine,
                "     , S.PRE_BEGIN_DATE, S.PRE_END_DATE, S.PRE_WORK_HOURS ", Environment.NewLine,
                "     , S.ACT_BEGIN_DATE, S.ACT_END_DATE, S.ACT_WORK_HOURS ", Environment.NewLine,
                "     , S.REMARK, S.UPD_USER_ID, dbo.FN_GET_USER_NM(S.UPD_USER_ID) AS UPD_USER_NM, S.UPD_DT ", Environment.NewLine,
                "FROM DEV_FUN_SCHEDULE S ", Environment.NewLine,
                "LEFT JOIN CM_CODE C ON C.CODE_KIND='0012' AND S.DEV_PHASE=C.CODE_ID ", Environment.NewLine,
                "WHERE S.SYS_ID={SYS_ID} AND S.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND S.FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                "ORDER BY C.SORT_ORDER ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.CODE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(FunSchedulePara.ParaField.CODE_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText.ToString(), dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<FunSchedule> funScheduleList = new List<FunSchedule>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    FunSchedule funSchedule = new FunSchedule()
                    {
                        SysID = new DBVarChar(dataRow[FunSchedule.DataField.SYS_ID.ToString()]),
                        FunControllerID = new DBVarChar(dataRow[FunSchedule.DataField.FUN_CONTROLLER_ID.ToString()]),
                        FunActionName = new DBVarChar(dataRow[FunSchedule.DataField.FUN_ACTION_NAME.ToString()]),
                        DevPhase = new DBVarChar(dataRow[FunSchedule.DataField.DEV_PHASE.ToString()]),
                        DevPhaseNM = new DBNVarChar(dataRow[FunSchedule.DataField.DEV_PHASE_NM.ToString()]),
                        DevOwner = new DBVarChar(dataRow[FunSchedule.DataField.DEV_OWNER.ToString()]),
                        DevOwnerNM = new DBNVarChar(dataRow[FunSchedule.DataField.DEV_OWNER_NM.ToString()]),
                        PreBeginDate = new DBChar(dataRow[FunSchedule.DataField.PRE_BEGIN_DATE.ToString()]),
                        PreEndDate = new DBChar(dataRow[FunSchedule.DataField.PRE_END_DATE.ToString()]),
                        PreWorkHours = new DBNumeric(dataRow[FunSchedule.DataField.PRE_WORK_HOURS.ToString()]),
                        ActBeginDate = new DBChar(dataRow[FunSchedule.DataField.ACT_BEGIN_DATE.ToString()]),
                        ActEndDate = new DBChar(dataRow[FunSchedule.DataField.ACT_END_DATE.ToString()]),
                        ActWorkHours = new DBNumeric(dataRow[FunSchedule.DataField.ACT_WORK_HOURS.ToString()]),
                        Remark = new DBNVarChar(dataRow[FunSchedule.DataField.REMARK.ToString()]),
                        UpdUserID = new DBVarChar(dataRow[FunSchedule.DataField.UPD_USER_ID.ToString()]),
                        UpdUserNM = new DBNVarChar(dataRow[FunSchedule.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[FunSchedule.DataField.UPD_DT.ToString()])
                    };
                    funScheduleList.Add(funSchedule);
                }
                return funScheduleList;
            }
            return null;
        }

        public enum EnumEditFunScheduleResult
        {
            Success, Failure
        }

        public EnumEditFunScheduleResult EditFunSchedule(FunSchedulePara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DELETE FROM DEV_FUN_SCHEDULE ", Environment.NewLine,
                "        WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} AND FUN_ACTION_NAME={FUN_ACTION_NAME} ", Environment.NewLine,
                "          AND DEV_PHASE={DEV_PHASE}; ", Environment.NewLine,
                "        INSERT INTO DEV_FUN_SCHEDULE VALUES ( ", Environment.NewLine,
                "            {SYS_ID}, {FUN_CONTROLLER_ID}, {FUN_ACTION_NAME} ", Environment.NewLine,
                "          , {IS_FUN}, {DEV_PHASE}, {DEV_OWNER} ", Environment.NewLine,
                "          , {PRE_BEGIN_DATE}, {PRE_END_DATE}, {PRE_WORK_HOURS} ", Environment.NewLine,
                "          , {ACT_BEGIN_DATE}, {ACT_END_DATE}, {ACT_WORK_HOURS} ", Environment.NewLine,
                "          , {REMARK}, {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "        ); ", Environment.NewLine,

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

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.IS_FUN, Value = para.IsFun });
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.DEV_PHASE, Value = para.DevPhase });
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.DEV_OWNER, Value = para.DevOwner });
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.PRE_BEGIN_DATE, Value = para.PreBeginDate });
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.PRE_END_DATE, Value = para.PreEndDate });
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.PRE_WORK_HOURS, Value = para.PreWorkHours });
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.ACT_BEGIN_DATE, Value = para.ActBeginDate });
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.ACT_END_DATE, Value = para.ActEndDate });
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.ACT_WORK_HOURS, Value = para.ActWorkHours });
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = FunSchedulePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditFunScheduleResult.Success : EnumEditFunScheduleResult.Failure;            
        }
    }
}