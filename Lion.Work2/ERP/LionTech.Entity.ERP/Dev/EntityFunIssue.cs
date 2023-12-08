using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.ERP.Dev
{
    public class EntityFunIssue : EntityDev
    {
        public EntityFunIssue(string connectionString, string providerName)
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
                DEV_PHASE, DEV_PHASE_NM
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
                "     , {DEV_PHASE} AS DEV_PHASE, dbo.FN_GET_NMID({DEV_PHASE}, C2.{CODE_NM}) AS DEV_PHASE_NM ", Environment.NewLine,
                "FROM SYS_SYSTEM_FUN F ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON F.SYS_ID=M.SYS_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_FUN_GROUP G ON F.SYS_ID=G.SYS_ID AND F.FUN_CONTROLLER_ID=G.FUN_CONTROLLER_ID ", Environment.NewLine,
                "JOIN CM_CODE C1 ON C1.CODE_KIND='0011' AND F.FUN_TYPE=C1.CODE_ID ", Environment.NewLine,
                "JOIN CM_CODE C2 ON C2.CODE_KIND='0012' AND {DEV_PHASE}=C2.CODE_ID ", Environment.NewLine,
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
                    DevPhaseNM = new DBNVarChar(dataTable.Rows[0][SystemFun.DataField.DEV_PHASE_NM.ToString()])
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
                "     , {DEV_PHASE} AS DEV_PHASE, dbo.FN_GET_NMID({DEV_PHASE}, C.{CODE_NM}) AS DEV_PHASE_NM ", Environment.NewLine,
                "FROM (SELECT T.SYS_ID, T.EVENT_GROUP_ID, T.EVENT_ID, E.{EVENT_NM} AS EVENT_NM ", Environment.NewLine,
                "           , T.TARGET_SYS_ID, E.IS_DISABLE, E.SORT_ORDER ", Environment.NewLine,
                "      FROM SYS_SYSTEM_EVENT_TARGET T JOIN SYS_SYSTEM_EVENT E ", Environment.NewLine,
                "      ON T.SYS_ID=E.SYS_ID AND T.EVENT_GROUP_ID=E.EVENT_GROUP_ID AND T.EVENT_ID=E.EVENT_ID) F ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M1 ON F.SYS_ID=M1.SYS_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_EVENT_GROUP G ON F.SYS_ID=G.SYS_ID AND F.EVENT_GROUP_ID=G.EVENT_GROUP_ID ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M2 ON F.TARGET_SYS_ID=M2.SYS_ID ", Environment.NewLine,
                "JOIN CM_CODE C ON C.CODE_KIND='0012' AND {DEV_PHASE}=C.CODE_ID ", Environment.NewLine,
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
                    DevPhaseNM = new DBNVarChar(dataTable.Rows[0][SystemFun.DataField.DEV_PHASE_NM.ToString()])
                };

                return systemFun;
            }
            return null;
        }

        public class FunIssuePara : DBCulture
        {
            public FunIssuePara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID, FUN_CONTROLLER_ID, FUN_ACTION_NAME, DEV_PHASE,
                REMARK, UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
            public DBVarChar DevPhase;

            public DBNVarChar Remark;
            public DBVarChar UpdUserID;
        }

        public class FunIssue : DBTableRow
        {
            public enum DataField
            {
                ISSUE_NO, REMARK, 
                UPD_USER_ID, UPD_USER_NM, UPD_DT
            }

            public DBChar IssueNo;
            public DBNVarChar Remark;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<FunIssue> SelectFunIssueList(FunIssuePara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT ISSUE_NO, REMARK ", Environment.NewLine,
                "     , UPD_USER_ID, dbo.FN_GET_USER_NM(UPD_USER_ID) AS UPD_USER_NM, UPD_DT ", Environment.NewLine,
                "FROM DEV_FUN_ISSUE ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} ", Environment.NewLine,
                "  AND FUN_ACTION_NAME={FUN_ACTION_NAME} AND DEV_PHASE={DEV_PHASE} ", Environment.NewLine,
                "ORDER BY ISSUE_NO DESC "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = FunIssuePara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = FunIssuePara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = FunIssuePara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = FunIssuePara.ParaField.DEV_PHASE.ToString(), Value = para.DevPhase });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<FunIssue> funIssueList = new List<FunIssue>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    FunIssue funIssue = new FunIssue()
                    {
                        IssueNo = new DBChar(dataRow[FunIssue.DataField.ISSUE_NO.ToString()]),
                        Remark = new DBNVarChar(dataRow[FunIssue.DataField.REMARK.ToString()]),
                        UpdUserID = new DBVarChar(dataRow[FunIssue.DataField.UPD_USER_ID.ToString()]),
                        UpdUserNM = new DBNVarChar(dataRow[FunIssue.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[FunIssue.DataField.UPD_DT.ToString()])
                    };
                    funIssueList.Add(funIssue);
                }
                return funIssueList;
            }
            return null;
        }

        public void AddFunIssue(FunIssuePara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @ISSUE_NO char(6); ", Environment.NewLine,
                "SELECT @ISSUE_NO=RIGHT('00000'+CAST(ISNULL(CAST(MAX(ISSUE_NO) AS INT),0)+1 AS VARCHAR),6) ", Environment.NewLine,
                "FROM DEV_FUN_ISSUE ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID} AND FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} ", Environment.NewLine,
                "  AND FUN_ACTION_NAME={FUN_ACTION_NAME} AND DEV_PHASE={DEV_PHASE}; ", Environment.NewLine,
                "INSERT INTO DEV_FUN_ISSUE VALUES ( ", Environment.NewLine,
                "    {SYS_ID}, {FUN_CONTROLLER_ID}, {FUN_ACTION_NAME}, {DEV_PHASE} ", Environment.NewLine,
                "  , @ISSUE_NO, {REMARK}, {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "); ", Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = FunIssuePara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = FunIssuePara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = FunIssuePara.ParaField.FUN_ACTION_NAME, Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = FunIssuePara.ParaField.DEV_PHASE, Value = para.DevPhase });
            dbParameters.Add(new DBParameter { Name = FunIssuePara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = FunIssuePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            base.ExecuteNonQuery(commandText, dbParameters);
        }
    }
}