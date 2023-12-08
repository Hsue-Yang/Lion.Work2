using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemRoleFunList : EntitySys
    {
        public EntitySystemRoleFunList(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemRoleFunListPara : DBCulture
        {
            public SystemRoleFunListPara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                SYS_ID,
                ROLE_ID,
                FUN_CONTROLLER_ID,
                FUN_GROUP,
                FUN_NM,
                ROLE_NM,
                SYS_NM
            }

            public DBVarChar SysID;
            public DBVarChar RoleID;
            public DBVarChar FunControllerID;
        }

        public class SystemRoleFunList : DBTableRow
        {
            public enum DataField
            {
                SYS_ID,
                SUB_SYS_NM,
                FUN_GROUP_NM,
                FUN_CONTROLLER_ID,
                FUN_ACTION_NMID,
                FUN_ACTION_NAME,
                UPD_USER_ID,
                UPD_DT
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

        public List<SystemRoleFunList> SelectSystemRoleFunList(SystemRoleFunListPara para)
        {

            string commandWhere = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.RoleID.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                {
                    commandWhere, " AND R.ROLE_ID={ROLE_ID} ", Environment.NewLine
                });
            }

            if (!string.IsNullOrWhiteSpace(para.FunControllerID.GetValue()))
            {
                commandWhere = string.Concat(new object[]
                {
                    commandWhere, " AND R.FUN_CONTROLLER_ID={FUN_CONTROLLER_ID} ", Environment.NewLine
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

        #region - 編輯系統角色功能清單 -
        public class EditSystemRoleFunListPara
        {
            public enum ParaField
            {
                SYS_ID,
                ROLE_ID,
                CONTROLLER_ID,
                ACTION_NAME,
                UPD_USER
            }

            public DBVarChar SysID;
            public DBVarChar RoleID;
            public List<SystemRoleFun> EditSysRoleFunList;
        }

        public class SystemRoleFun
        {
            public DBVarChar FUN_CONTROLLER_ID;
            public DBVarChar FUN_ACTION_NAME;
            public DBVarChar UPD_USER_ID;
            public DBBit IsAdd;
        }

        public enum EnumEditSystemRoleFunListResult
        {
            Success,
            Failure
        }

        public EnumEditSystemRoleFunListResult EditSystemRoleFunList(EditSystemRoleFunListPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            List<string> editSystemRoleFunList = new List<string>();

            foreach (SystemRoleFun roleFun in para.EditSysRoleFunList)
            {
                editSystemRoleFunList.Add(GetCommandText(ProviderName, string.Join(Environment.NewLine, new object[]
                {
                    roleFun.IsAdd.Bool()
                        ? string.Join(Environment.NewLine, new object[]
                        {
                            "INSERT INTO SYS_SYSTEM_ROLE_FUN",
                            "     ( SYS_ID",
                            "     , ROLE_ID",
                            "     , FUN_CONTROLLER_ID",
                            "     , FUN_ACTION_NAME",
                            "     , UPD_USER_ID",
                            "     , UPD_DT",
                            "     )",
                            "VALUES",
                            "     ( @SYS_ID",
                            "     , @ROLE_ID",
                            "     , {CONTROLLER_ID}",
                            "     , {ACTION_NAME}",
                            "     , {UPD_USER}",
                            "     , GETDATE()",
                            "     );"
                        }) : string.Join(Environment.NewLine, new object[]
                        {
                            "DELETE SYS_SYSTEM_ROLE_FUN ",
                            " WHERE SYS_ID = @SYS_ID",
                            "   AND ROLE_ID = @ROLE_ID",
                            "   AND FUN_CONTROLLER_ID = {CONTROLLER_ID}",
                            "   AND FUN_ACTION_NAME = {ACTION_NAME};"
                        })
                }), new List<DBParameter>
                {
                    new DBParameter { Name = EditSystemRoleFunListPara.ParaField.ACTION_NAME, Value = roleFun.FUN_ACTION_NAME },
                    new DBParameter { Name = EditSystemRoleFunListPara.ParaField.CONTROLLER_ID, Value = roleFun.FUN_CONTROLLER_ID },
                    new DBParameter { Name = EditSystemRoleFunListPara.ParaField.UPD_USER, Value = roleFun.UPD_USER_ID }
                }));
            }

            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(1) = 'N';",
                "DECLARE @ERROR_LINE INT;",
                "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                "DECLARE @SYS_ID VARCHAR(12) = {SYS_ID};",
                "DECLARE @ROLE_ID VARCHAR(20) = {ROLE_ID};",
                "BEGIN TRANSACTION",
                "    BEGIN TRY",
                string.Join(Environment.NewLine, editSystemRoleFunList),
                "        SET @RESULT = 'Y';",
                "        COMMIT;",
                "    END TRY",
                "    BEGIN CATCH",
                "        SET @RESULT = 'N';",
                "        SET @ERROR_LINE = ERROR_LINE();",
                "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                "        ROLLBACK TRANSACTION;",
                "    END CATCH;",
                "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
            }));

            dbParameters.Add(new DBParameter { Name = EditSystemRoleFunListPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = EditSystemRoleFunListPara.ParaField.ROLE_ID, Value = para.RoleID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditSystemRoleFunListResult.Success;
            }
            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion
    }
}
