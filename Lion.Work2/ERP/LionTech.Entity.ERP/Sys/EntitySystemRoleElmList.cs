// 新增日期：2018-01-19
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemRoleElmList : EntitySys
    {
        public EntitySystemRoleElmList(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemRoleElmListPara : DBCulture
        {
            public SystemRoleElmListPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                ROLE_ID,
                FUN_CONTROLLER_ID,
                FUN_ACTION_NAME,
                ELM_NM
            }

            public DBVarChar SysID;
            public DBVarChar RoleID;
            public DBVarChar FunControllerID;
            public DBVarChar FunActionNM;
        }

        public class SystemRoleElmList : DBTableRow
        {
            public DBVarChar FunControllerID;
            public DBVarChar FunActionNM;
            public DBVarChar ElmID;
            public DBNVarChar ElmNM;
            public DBChar IsDisable;
            public DBTinyInt DisplaySts;
            public DBNVarChar UpdUserIDNM;
            public DBDateTime UpdDT;
        }

        public List<SystemRoleElmList> SelectSystemRoleElmList(SystemRoleElmListPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandWhere = new List<string>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT R.FUN_CONTROLLER_ID AS FunControllerID",
                "     , R.FUN_ACTION_NAME AS FunActionNM",
                "     , R.ELM_ID AS ElmID",
                "     , {ELM_NM} AS ElmNM",
                "     , E.IS_DISABLE AS IsDisable",
                "     , R.DISPLAY_STS AS DisplaySts",
                "     , dbo.FN_GET_USER_NM(R.UPD_USER_ID) AS UpdUserIDNM",
                "     , R.UPD_DT AS UpdDT",
                "  FROM SYS_SYSTEM_ROLE_FUN_ELM R",
                "  JOIN SYS_SYSTEM_FUN_ELM E",
                "    ON E.ELM_ID = R.ELM_ID",
                "   AND E.SYS_ID = R.SYS_ID",
                "   AND E.FUN_CONTROLLER_ID = R.FUN_CONTROLLER_ID",
                "   AND E.FUN_ACTION_NAME = R.FUN_ACTION_NAME",
                " WHERE R.SYS_ID = {SYS_ID}",
                "   AND ROLE_ID = {ROLE_ID}"
            }));

            if (para.FunControllerID.IsNull() == false)
            {
                commandWhere.Add("R.FUN_CONTROLLER_ID = {FUN_CONTROLLER_ID}");
                dbParameters.Add(new DBParameter { Name = SystemRoleElmListPara.ParaField.FUN_CONTROLLER_ID, Value = para.FunControllerID });
            }

            if (para.FunActionNM.IsNull() == false)
            {
                commandWhere.Add("R.FUN_ACTION_NAME = {FUN_ACTION_NAME}");
                dbParameters.Add(new DBParameter { Name = SystemRoleElmListPara.ParaField.FUN_ACTION_NAME, Value = para.FunActionNM });
            }

            if (commandWhere.Any())
            {
                commandText.AppendLine($" AND {string.Join(" AND ", commandWhere)}");
            }

            dbParameters.Add(new DBParameter { Name = SystemRoleElmListPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemRoleElmListPara.ParaField.ROLE_ID, Value = para.RoleID });
            dbParameters.Add(new DBParameter { Name = SystemRoleElmListPara.ParaField.ELM_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemRoleElmListPara.ParaField.ELM_NM.ToString())) });

            return GetEntityList<SystemRoleElmList>(commandText.ToString(), dbParameters);
        }

        #region - 取得元素代碼及名稱 -
        public class SystemElmIDPara : DBCulture
        {
            public SystemElmIDPara(string cultureID)
                : base(cultureID)
            {

            }

            public enum ParaField
            {
                SYS_ID,
                FUN_CONTROLLER_ID,
                FUN_ACTION_NAME,
                ELM_NM
            }

            public DBVarChar SysID;
            public DBVarChar ElmID;
            public DBVarChar ControllerID;
            public DBVarChar ActionName;
        }

        public class SysElmName : DBTableRow,ISelectItem
        {
            public DBVarChar ElmNM;
            public DBVarChar ElmID;

            public string ItemText()
            {
                return $"{ElmNM.StringValue()}({ElmID.StringValue()})";
            }

            public string ItemValue()
            {
                return ElmID.StringValue();
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

        public List<SysElmName> SelectSystemElmIDList(SystemElmIDPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT {ELM_NM} AS ElmNM",
                "     , ELM_ID AS ElmID",
                "  FROM SYS_SYSTEM_FUN_ELM ",
                " WHERE SYS_ID = {SYS_ID}",
                "   AND FUN_CONTROLLER_ID = {FUN_CONTROLLER_ID}",
                "   AND FUN_ACTION_NAME = {FUN_ACTION_NAME}"
            }));

            dbParameters.Add(new DBParameter { Name = SystemElmIDPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemElmIDPara.ParaField.FUN_CONTROLLER_ID, Value = para.ControllerID });
            dbParameters.Add(new DBParameter { Name = SystemElmIDPara.ParaField.FUN_ACTION_NAME, Value = para.ActionName });
            dbParameters.Add(new DBParameter { Name = SystemElmIDPara.ParaField.ELM_NM.ToString(), Value = para.GetCultureFieldNM(new DBObject(SystemRoleElmListPara.ParaField.ELM_NM.ToString())) });

            return GetEntityList<SysElmName>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 編輯系統角色功能元素清單 -
        public class EditSystemRoleElmListPara
        {
            public enum ParaField
            {
                SYS_ID,
                ROLE_ID,
                CONTROLLER_ID,
                ACTION_NAME,
                ELM_ID,
                DISPLAY_STS,
                UPD_USER,
                UPD_DATE
            }

            public DBVarChar SysID;
            public DBVarChar RoleID;
            public List<SystemRoleElm> EditSysRoleElmList;
        }

        public class SystemRoleElm
        {
            public DBVarChar FUN_CONTROLLER_ID;
            public DBVarChar FUN_ACTION_NAME;
            public DBVarChar ELM_ID;
            public DBTinyInt DISPLAY_STS;
            public DBVarChar UPD_USER_ID;
            public DBBit IsAdd;
        }

        public enum EnumEditSystemRoleElmListResult
        {
            Success,
            Failure
        }

        public EnumEditSystemRoleElmListResult EditSystemRoleFunList(EditSystemRoleElmListPara para)
        {
            List<DBParameter> dbParameters = new List<DBParameter>();
            List<string> editSystemRoleElmList = new List<string>();

            foreach (SystemRoleElm elm in para.EditSysRoleElmList)
            {
                editSystemRoleElmList.Add(GetCommandText(ProviderName, string.Join(Environment.NewLine, new object[]
                {
                    elm.IsAdd.Bool()
                        ? string.Join(Environment.NewLine, new object[]
                        {
                            "INSERT INTO SYS_SYSTEM_ROLE_FUN_ELM",
                            "     ( SYS_ID",
                            "     , ROLE_ID",
                            "     , FUN_CONTROLLER_ID",
                            "     , FUN_ACTION_NAME",
                            "     , ELM_ID",
                            "     , DISPLAY_STS",
                            "     , UPD_USER_ID",
                            "     , UPD_DT",
                            "     )",
                            "VALUES",
                            "     ( @SYS_ID",
                            "     , @ROLE_ID",
                            "     , {CONTROLLER_ID}",
                            "     , {ACTION_NAME}",
                            "     , {ELM_ID}",
                            "     , {DISPLAY_STS}",
                            "     , {UPD_USER}",
                            "     , GETDATE()",
                            "     );"
                        }) : string.Join(Environment.NewLine, new object[]
                        {
                            "DELETE SYS_SYSTEM_ROLE_FUN_ELM ",
                            " WHERE SYS_ID = @SYS_ID",
                            "   AND ROLE_ID = @ROLE_ID",
                            "   AND ELM_ID = {ELM_ID}",
                            "   AND FUN_CONTROLLER_ID = {CONTROLLER_ID}",
                            "   AND FUN_ACTION_NAME = {ACTION_NAME};"
                        })
                }), new List<DBParameter>
                {
                    new DBParameter { Name = EditSystemRoleElmListPara.ParaField.ACTION_NAME, Value = elm.FUN_ACTION_NAME },
                    new DBParameter { Name = EditSystemRoleElmListPara.ParaField.CONTROLLER_ID, Value = elm.FUN_CONTROLLER_ID },
                    new DBParameter { Name = EditSystemRoleElmListPara.ParaField.ELM_ID, Value = elm.ELM_ID },
                    new DBParameter { Name = EditSystemRoleElmListPara.ParaField.DISPLAY_STS, Value = elm.DISPLAY_STS },
                    new DBParameter { Name = EditSystemRoleElmListPara.ParaField.UPD_USER, Value = elm.UPD_USER_ID }
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
                string.Join(Environment.NewLine, editSystemRoleElmList),
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

            dbParameters.Add(new DBParameter { Name = EditSystemRoleElmListPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = EditSystemRoleElmListPara.ParaField.ROLE_ID, Value = para.RoleID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditSystemRoleElmListResult.Success;
            }
            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion
    }
}