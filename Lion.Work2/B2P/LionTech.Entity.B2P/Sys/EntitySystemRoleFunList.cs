using System;
using System.Collections.Generic;
using System.Data;

namespace LionTech.Entity.B2P.Sys
{
    public class EntitySystemRoleFunList : EntitySys
    {
        public EntitySystemRoleFunList(string connectionString, string providerName)
            :base(connectionString, providerName)
        {}

        public class SystemRoleFunListPara : DBCulture
        {
            public SystemRoleFunListPara(string cultureID)
                : base(cultureID)
            {}
            public enum ParaField 
            {
                SYS_ID, ROLE_ID, FUN_CONTROLLER_ID, FUN_GROUP, FUN_NM, ROLE_NM, SYS_NM
            }

            public DBVarChar SysID;
            public DBVarChar RoleID;
            public DBVarChar FunControllerID;
        }

        public class SystemRoleFunList : DBTableRow
        {
            public enum DataField 
            {
                SYS_ID, SUB_SYS_NM, FUN_GROUP_NM, FUN_CONTROLLER_ID, FUN_ACTION_NMID, FUN_ACTION_NAME, UPD_USER_ID, UPD_DT
            }

            public DBVarChar SubSysNM;
            public DBVarChar SysID;
            public DBVarChar FunGroupNM;
            public DBVarChar FunActionNMID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
            public DBVarChar UpdUserID;
            public DBDateTime UpdDT;
        }

        public List<SystemRoleFunList> SelectSystemRoleFunList(SystemRoleFunListPara para){

            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.RoleID.GetValue())) 
            {
                commandWhere = string.Concat(new object[]
                {
                    commandWhere," AND R.ROLE_ID={ROLE_ID} ", Environment.NewLine
                });
            }

            if (!string.IsNullOrWhiteSpace(para.FunControllerID.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                {
                    commandWhere," AND R.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} ", Environment.NewLine
                });
            }

            string commandText = string.Concat(new object[]
            {
                "SELECT (CASE WHEN (F.FUN_ACTION_NAME IS NULL AND F.{FUN_NM} IS NULL) THEN NULL ELSE dbo.FN_GET_NMID(F.FUN_ACTION_NAME, F.{FUN_NM}) END) AS FUN_ACTION_NMID ", Environment.NewLine,
                "     , (CASE WHEN (F.SUB_SYS_ID IS NULL OR F.SUB_SYS_ID=F.SYS_ID) THEN NULL ELSE dbo.FN_GET_NMID(F.SUB_SYS_ID, S.{SYS_NM}) END) AS SUB_SYS_NM ", Environment.NewLine,
                "     , (CASE WHEN G.FUN_CONTROLLER_ID IS NULL AND G.{FUN_GROUP} IS NULL THEN NULL ELSE dbo.FN_GET_NMID(G.FUN_CONTROLLER_ID, G.{FUN_GROUP}) END) AS FUN_GROUP_NM ", Environment.NewLine,
                "     , dbo.FN_GET_USER_NM(R.UPD_USER_ID) AS UPD_USER_ID, R.UPD_DT ", Environment.NewLine,
                "     , F.FUN_ACTION_NAME, F.FUN_CONTROLLER_ID, F.SYS_ID ", Environment.NewLine,
                "FROM SYS_SYSTEM_ROLE_FUN R", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_FUN_GROUP G ON G.SYS_ID = R.SYS_ID AND G.FUN_CONTROLLER_ID = R.FUN_CONTROLLER_ID ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_FUN F ON F.SYS_ID = R.SYS_ID AND F.FUN_CONTROLLER_ID = R.FUN_CONTROLLER_ID AND F.FUN_ACTION_NAME = R.FUN_ACTION_NAME ", Environment.NewLine,
                "LEFT JOIN SYS_SYSTEM_SUB S ON F.SYS_ID = S.PARENT_SYS_ID AND F.SUB_SYS_ID = S.SYS_ID ", Environment.NewLine,
                "WHERE R.SYS_ID={SYS_ID}  ", Environment.NewLine,
                commandWhere, Environment.NewLine
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemRoleFunListPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunListPara.ParaField.ROLE_ID, Value = para.RoleID });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunListPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunListPara.ParaField.FUN_GROUP.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemRoleFunListPara.ParaField.FUN_GROUP.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunListPara.ParaField.ROLE_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemRoleFunListPara.ParaField.ROLE_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunListPara.ParaField.FUN_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemRoleFunListPara.ParaField.FUN_NM.ToString())) });
            dbParameters.Add(new DBParameter { Name = SystemRoleFunListPara.ParaField.SYS_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemRoleFunListPara.ParaField.SYS_NM.ToString())) });

            DataTable dataTable = base.GetDataTable(commandText, dbParameters);
            if (dataTable.Rows.Count > 0)
            {
                List<SystemRoleFunList> systemRoleFunListList = new List<SystemRoleFunList>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                     SystemRoleFunList systemRoleFunList = new SystemRoleFunList()
                     { 
                        SysID = new DBVarChar(dataRow[SystemRoleFunList.DataField.SYS_ID.ToString()]),
                        SubSysNM = new DBVarChar(dataRow[SystemRoleFunList.DataField.SUB_SYS_NM.ToString()]),
                        FunActionNMID = new DBVarChar(dataRow[SystemRoleFunList.DataField.FUN_ACTION_NMID.ToString()]),
                        FunActionName = new DBVarChar(dataRow[SystemRoleFunList.DataField.FUN_ACTION_NAME.ToString()]),
                        FunGroupNM = new DBVarChar(dataRow[SystemRoleFunList.DataField.FUN_GROUP_NM.ToString()]),
                        FunControllerID = new DBVarChar(dataRow[SystemRoleFunList.DataField.FUN_CONTROLLER_ID.ToString()]),
                        UpdUserID = new DBVarChar(dataRow[SystemRoleFunList.DataField.UPD_USER_ID.ToString()]),
                        UpdDT = new DBDateTime(dataRow[SystemRoleFunList.DataField.UPD_DT.ToString()]),
                    };
                    systemRoleFunListList.Add(systemRoleFunList);
                }
                return systemRoleFunListList;
            }
            return null;
        }
    }
}
