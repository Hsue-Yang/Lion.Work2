using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemSetting : EntitySys
    {
        public EntitySystemSetting(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemSettingPara : DBCulture
        {
            public SystemSettingPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                USER_ID,
                SYS_NM
            }

            public DBVarChar UserID;
        }

        public class SystemSetting : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM,
                SYS_MAN_USER_ID, SYS_MAN_USER_NM,
                IS_AP, IS_API, IS_EDI, IS_Event,
                SYS_INDEX_PATH, SYS_ICON_PATH,
                IS_OUTSOURCING, IS_DISABLE, SORT_ORDER,
                UPD_USER_NM, UPD_DT
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public DBVarChar SysMANUserID;
            public DBNVarChar SysMANUserNM;

            public DBChar IsAP;
            public DBChar IsAPI;
            public DBChar IsEDI;
            public DBChar IsEvent;

            public DBNVarChar SysIndexPath;
            public DBNVarChar SysIconPath;
            public DBChar IsOutsourcing;
            public DBChar IsDisable;
            public DBVarChar SortOrder;

            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;

            public List<Subsystem> SubsystemList;
        }

        public List<SystemSetting> SelectSystemSettingList(SystemSettingPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @ROLE_CN INT = 0; ", Environment.NewLine,

                "SELECT @ROLE_CN=COUNT(ROLE_ID) ", Environment.NewLine,
                "FROM SYS_USER_SYSTEM_ROLE ", Environment.NewLine,
                "WHERE SYS_ID='B2PAP' AND ROLE_ID='GRANTOR' ", Environment.NewLine,
                "  AND USER_ID={USER_ID}; ", Environment.NewLine,

                "SELECT M.SYS_ID, dbo.FN_GET_NMID(M.SYS_ID, {SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , M.SYS_MAN_USER_ID, dbo.FN_GET_USER_NM(M.SYS_MAN_USER_ID) AS SYS_MAN_USER_NM ", Environment.NewLine,
                "     , (SELECT (CASE WHEN COUNT(1)>0 THEN 'Y' ELSE 'N' END) FROM SYS_SYSTEM_SERVICE WHERE SYS_ID=M.SYS_ID AND SERVICE_ID='AP') AS IS_AP ", Environment.NewLine,
                "     , (SELECT (CASE WHEN COUNT(1)>0 THEN 'Y' ELSE 'N' END) FROM SYS_SYSTEM_SERVICE WHERE SYS_ID=M.SYS_ID AND SERVICE_ID='API') AS IS_API ", Environment.NewLine,
                "     , (SELECT (CASE WHEN COUNT(1)>0 THEN 'Y' ELSE 'N' END) FROM SYS_SYSTEM_SERVICE WHERE SYS_ID=M.SYS_ID AND SERVICE_ID='EDI') AS IS_EDI ", Environment.NewLine,
                "     , (SELECT (CASE WHEN COUNT(1)>0 THEN 'Y' ELSE 'N' END) FROM SYS_SYSTEM_SERVICE WHERE SYS_ID=M.SYS_ID AND SERVICE_ID='Event') AS IS_Event ", Environment.NewLine,
                "     , M.SYS_INDEX_PATH, M.SYS_ICON_PATH ", Environment.NewLine,
                "     , M.IS_OUTSOURCING, M.IS_DISABLE, M.SORT_ORDER ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(M.UPD_USER_ID) AS UPD_USER_NM, M.UPD_DT ", Environment.NewLine,
                "FROM SYS_SYSTEM_MAIN M ", Environment.NewLine,
                "JOIN SYS_USER_SYSTEM S ON M.SYS_ID=S.SYS_ID ", Environment.NewLine,
                "LEFT JOIN SYS_USER_SYSTEM_ROLE R ON S.USER_ID=R.USER_ID AND S.SYS_ID=R.SYS_ID ", Environment.NewLine,
                "WHERE S.USER_ID={USER_ID} AND ((M.IS_OUTSOURCING='Y' AND (M.SYS_MAN_USER_ID={USER_ID} OR @ROLE_CN>0)) OR (M.IS_OUTSOURCING='N' AND R.ROLE_ID<>'USER')) ", Environment.NewLine,
                "GROUP BY M.SYS_ID, dbo.FN_GET_NMID(M.SYS_ID, M.{SYS_NM}) ", Environment.NewLine,
                "       , M.SYS_MAN_USER_ID, dbo.FN_GET_USER_NM(M.SYS_MAN_USER_ID) ", Environment.NewLine,
                "       , M.SYS_INDEX_PATH, M.SYS_ICON_PATH ", Environment.NewLine,
                "       , M.IS_OUTSOURCING, M.IS_DISABLE, M.SORT_ORDER ", Environment.NewLine,
                "       , M.UPD_USER_ID, M.UPD_DT ", Environment.NewLine,
                "ORDER BY SORT_ORDER "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemSettingPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = SystemSettingPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemSettingPara.ParaField.SYS_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemSetting> systemSettingList = new List<SystemSetting>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SystemSetting systemSetting = new SystemSetting()
                    {
                        SysID = new DBVarChar(dataRow[SystemSetting.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SystemSetting.DataField.SYS_NM.ToString()]),

                        SysMANUserID = new DBVarChar(dataRow[SystemSetting.DataField.SYS_MAN_USER_ID.ToString()]),
                        SysMANUserNM = new DBNVarChar(dataRow[SystemSetting.DataField.SYS_MAN_USER_NM.ToString()]),

                        IsAP = new DBChar(dataRow[SystemSetting.DataField.IS_AP.ToString()]),
                        IsAPI = new DBChar(dataRow[SystemSetting.DataField.IS_API.ToString()]),
                        IsEDI = new DBChar(dataRow[SystemSetting.DataField.IS_EDI.ToString()]),
                        IsEvent = new DBChar(dataRow[SystemSetting.DataField.IS_Event.ToString()]),

                        SysIndexPath = new DBNVarChar(dataRow[SystemSetting.DataField.SYS_INDEX_PATH.ToString()]),
                        SysIconPath = new DBNVarChar(dataRow[SystemSetting.DataField.SYS_ICON_PATH.ToString()]),
                        IsOutsourcing = new DBChar(dataRow[SystemSetting.DataField.IS_OUTSOURCING.ToString()]),
                        IsDisable = new DBChar(dataRow[SystemSetting.DataField.IS_DISABLE.ToString()]),
                        SortOrder = new DBVarChar(dataRow[SystemSetting.DataField.SORT_ORDER.ToString()]),

                        UpdUserNM = new DBNVarChar(dataRow[SystemSetting.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[SystemSetting.DataField.UPD_DT.ToString()])
                    };
                    systemSettingList.Add(systemSetting);
                }
                return systemSettingList;
            }
            return null;
        }

        public class Subsystem : DBTableRow
        {
            public enum DataField
            {
                SYS_ID, SYS_NM,
                PARENT_SYS_ID, SORT_ORDER,
                UPD_USER_NM, UPD_DT
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;

            public DBVarChar ParentSysID;
            public DBVarChar SortOrder;

            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<Subsystem> SelectUserSubsystemList(SystemSettingPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT S.SYS_ID, dbo.FN_GET_NMID(S.SYS_ID, S.{SYS_NM}) AS SYS_NM ", Environment.NewLine,
                "     , S.PARENT_SYS_ID, S.SORT_ORDER ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(S.UPD_USER_ID) AS UPD_USER_NM, S.UPD_DT ", Environment.NewLine,
                "FROM SYS_SYSTEM_SUB S ", Environment.NewLine,
                "JOIN SYS_SYSTEM_MAIN M ON S.PARENT_SYS_ID=M.SYS_ID ", Environment.NewLine,
                "JOIN SYS_USER_SYSTEM U ON M.SYS_ID=U.SYS_ID ", Environment.NewLine,
                "LEFT JOIN SYS_USER_SYSTEM_ROLE R ON U.USER_ID=R.USER_ID AND U.SYS_ID=R.SYS_ID ", Environment.NewLine,
                "WHERE U.USER_ID={USER_ID} AND (M.IS_OUTSOURCING='N' AND R.ROLE_ID<>'USER') ", Environment.NewLine,
                "GROUP BY S.SYS_ID, dbo.FN_GET_NMID(S.SYS_ID, S.{SYS_NM}) ", Environment.NewLine,
                "       , S.PARENT_SYS_ID, S.SORT_ORDER ", Environment.NewLine,
                "       , S.UPD_USER_ID, S.UPD_DT ", Environment.NewLine,
                "ORDER BY S.PARENT_SYS_ID, S.SORT_ORDER "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemSettingPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = SystemSettingPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemSettingPara.ParaField.SYS_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<Subsystem> subsystemList = new List<Subsystem>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    Subsystem subsystem = new Subsystem()
                    {
                        SysID = new DBVarChar(dataRow[Subsystem.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[Subsystem.DataField.SYS_NM.ToString()]),

                        ParentSysID = new DBVarChar(dataRow[Subsystem.DataField.PARENT_SYS_ID.ToString()]),
                        SortOrder = new DBVarChar(dataRow[Subsystem.DataField.SORT_ORDER.ToString()]),

                        UpdUserNM = new DBNVarChar(dataRow[Subsystem.DataField.UPD_USER_NM.ToString()]),
                        UpdDT = new DBDateTime(dataRow[Subsystem.DataField.UPD_DT.ToString()])
                    };
                    subsystemList.Add(subsystem);
                }
                return subsystemList;
            }
            return null;
        }
    }
}