using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemRoleConditionDetail : EntitySys
    {
        public EntitySystemRoleConditionDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 查詢系統角色預設權限主檔 -
        public class SysSystemRoleConditionDetailPara : DBCulture
        {
            public SysSystemRoleConditionDetailPara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                SYS_ID,
                ROLE_CONDITION_ID,
                ROLE_CONDITION_NM_ZH_TW,
                ROLE_CONDITION_NM_ZH_CN,
                ROLE_CONDITION_NM_EN_US,
                ROLE_CONDITION_NM_TH_TH,
                ROLE_CONDITION_NM_JA_JP,
                ROLE_CONDITION_SYNTAX,
                SORT_ORDER,
                REMARK,
                UPD_USER_ID,
                UPD_DT,
                SysRoleList,
                ROLE_ID
            }

            public DBVarChar SysID;
            public DBVarChar RoleConditionID;
            public DBNVarChar RoleConditionNMZHTW;
            public DBNVarChar RoleConditionNMZHCN;
            public DBNVarChar RoleConditionNMENUS;
            public DBNVarChar RoleConditionNMTHTH;
            public DBNVarChar RoleConditionNMJAJP;
            public DBNVarChar RoleConditionSyntax;
            public DBVarChar SortOrder;
            public DBNVarChar Remark;
            public DBVarChar UpdUserID;
            public DBDateTime UpdDT;
            public List<DBChar> SysRoleList;
            public DBVarChar RoleID;
        }

        public class SysSystemRoleConditionDetail : DBTableRow
        {
            public DBVarChar SysID;
            public DBVarChar RoleConditionID;
            public DBNVarChar RoleConditionNMZHTW;
            public DBNVarChar RoleConditionNMZHCN;
            public DBNVarChar RoleConditionNMENUS;
            public DBNVarChar RoleConditionNMTHTH;
            public DBNVarChar RoleConditionNMJAJP;
            public DBNVarChar RoleConditionSyntax;
            public DBVarChar SortOrder;
            public DBNVarChar Remark;
            public DBVarChar UpdUserID;
            public DBDateTime UpdDT;
            public DBVarChar RoleID;
            public DBVarChar SysRole;
        }

        /// <summary>
        /// 查詢系統角色預設權限主檔
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public SysSystemRoleConditionDetail SelectSysSystemRoleConditionDetail(SysSystemRoleConditionDetailPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT ROLE_CONDITION_NM_ZH_TW AS RoleConditionNMZHTW",
                    "     , ROLE_CONDITION_NM_ZH_CN AS RoleConditionNMZHCN",
                    "     , ROLE_CONDITION_NM_EN_US AS RoleConditionNMENUS",
                    "     , ROLE_CONDITION_NM_TH_TH AS RoleConditionNMTHTH",
                    "     , ROLE_CONDITION_NM_JA_JP AS RoleConditionNMJAJP",
                    "     , ROLE_CONDITION_SYNTAX AS RoleConditionSyntax",
                    "     , STUFF((",
                    "         SELECT '、' + T.ROLE_ID",
                    "           FROM SYS_SYSTEM_ROLE_CONDITION_COLLECT T",
                    "          WHERE T.SYS_ID = N.SYS_ID",
                    "            AND T.ROLE_CONDITION_ID = N.ROLE_CONDITION_ID",
                    "            FOR XML PATH('')",
                    "            ), 1, 1, '') AS SysRole",
                    "     , REMARK AS Remark",
                    "     , SORT_ORDER AS SortOrder",
                    "  FROM SYS_SYSTEM_ROLE_CONDITION N",
                    " WHERE SYS_ID = {SYS_ID}",
                    "   AND ROLE_CONDITION_ID = {ROLE_CONDITION_ID};"
                }));

            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.ROLE_CONDITION_ID, Value = para.RoleConditionID });

            return GetEntityList<SysSystemRoleConditionDetail>(commandText.ToString(), dbParameters).SingleOrDefault();
        }
        #endregion

        #region - 編輯系統角色預設權限 -
        public enum EnumEditSystemRoleConditionDetailResult
        {
            Success,
            Failure
        }

        /// <summary>
        /// 編輯系統角色預設權限
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public EnumEditSystemRoleConditionDetailResult EditSysSystemRoleConditionDetail(SysSystemRoleConditionDetailPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText =
                new StringBuilder(string.Join(Environment.NewLine,
                    new object[]
                    {
                        " DECLARE @RESULT CHAR(1) = 'N';",
                        " DECLARE @ERROR_LINE INT;",
                        " DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                        Environment.NewLine
                    }));

            commandText.AppendLine(string.Join(Environment.NewLine,
                new object[]
                {
                    "BEGIN TRANSACTION",
                    "    BEGIN TRY",
                    "        DELETE FROM SYS_SYSTEM_ROLE_CONDITION ",
                    "         WHERE SYS_ID = {SYS_ID}",
                    "           AND ROLE_CONDITION_ID = {ROLE_CONDITION_ID};",

                    "        DELETE FROM SYS_SYSTEM_ROLE_CONDITION_COLLECT ",
                    "         WHERE SYS_ID = {SYS_ID}",
                    "           AND ROLE_CONDITION_ID = {ROLE_CONDITION_ID};",

                    "        INSERT INTO SYS_SYSTEM_ROLE_CONDITION",
                    "             ( SYS_ID",
                    "             , ROLE_CONDITION_ID",
                    "             , ROLE_CONDITION_NM_ZH_TW",
                    "             , ROLE_CONDITION_NM_ZH_CN",
                    "             , ROLE_CONDITION_NM_EN_US",
                    "             , ROLE_CONDITION_NM_TH_TH",
                    "             , ROLE_CONDITION_NM_JA_JP",
                    "             , ROLE_CONDITION_SYNTAX",
                    "             , SORT_ORDER",
                    "             , REMARK",
                    "             , UPD_USER_ID",
                    "             , UPD_DT",
                    "             )",
                    "        SELECT {SYS_ID}",
                    "             , {ROLE_CONDITION_ID}",
                    "             , {ROLE_CONDITION_NM_ZH_TW}",
                    "             , {ROLE_CONDITION_NM_ZH_CN}",
                    "             , {ROLE_CONDITION_NM_EN_US}",
                    "             , {ROLE_CONDITION_NM_TH_TH}",
                    "             , {ROLE_CONDITION_NM_JA_JP}",
                    "             , {ROLE_CONDITION_SYNTAX}",
                    "             , {SORT_ORDER}",
                    "             , {REMARK}",
                    "             , {UPD_USER_ID}",
                    "             , GETDATE();"
                }));

            if (para.SysRoleList.Any())
            {
                foreach (var role in para.SysRoleList)
                {
                    commandText.AppendLine(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                        "       INSERT INTO SYS_SYSTEM_ROLE_CONDITION_COLLECT",
                        "            ( SYS_ID",
                        "            , ROLE_CONDITION_ID",
                        "            , ROLE_ID",
                        "            , UPD_USER_ID",
                        "            , UPD_DT",
                        "            )",
                        "       SELECT {SYS_ID}",
                        "            , {ROLE_CONDITION_ID}",
                        "            , {ROLE_ID}",
                        "            , {UPD_USER_ID}",
                        "            , GETDATE();"
                        ),
                        new List<DBParameter>
                        {
                            new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.SYS_ID, Value = para.SysID },
                            new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.ROLE_CONDITION_ID, Value = para.RoleConditionID },
                            new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.ROLE_ID, Value = role },
                            new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.UPD_USER_ID, Value = para.UpdUserID }
                        }));
                }
            }

            commandText.AppendLine(string.Join(Environment.NewLine,
                new object[]
                {
                    "       SET @RESULT = 'Y';",
                    "       COMMIT;",
                    "    END TRY",
                    "    BEGIN CATCH",
                    "       SET @RESULT = 'N';",
                    "       SET @ERROR_LINE = ERROR_LINE();",
                    "       SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                    "       ROLLBACK TRANSACTION;",
                    "    END CATCH;",
                    "SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                }));

            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.ROLE_CONDITION_ID, Value = para.RoleConditionID });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.ROLE_CONDITION_NM_ZH_TW, Value = para.RoleConditionNMZHTW });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.ROLE_CONDITION_NM_ZH_CN, Value = para.RoleConditionNMZHCN });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.ROLE_CONDITION_NM_EN_US, Value = para.RoleConditionNMENUS });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.ROLE_CONDITION_NM_TH_TH, Value = para.RoleConditionNMTHTH });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.ROLE_CONDITION_NM_JA_JP, Value = para.RoleConditionNMJAJP });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.ROLE_CONDITION_SYNTAX, Value = para.RoleConditionSyntax });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.SORT_ORDER, Value = para.SortOrder });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.REMARK, Value = para.Remark });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditSystemRoleConditionDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion

        #region - 刪除系統角色預設權限 -
        public enum EnumDeleteSystemRoleConditionDetailResult
        {
            Success,
            Failure
        }

        public EnumDeleteSystemRoleConditionDetailResult DeleteSystemRoleConditionDetail(SysSystemRoleConditionDetailPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText =
                new StringBuilder(string.Join(Environment.NewLine,
                    new object[]
                    {
                        "DECLARE @RESULT CHAR(1) = 'N';",
                        "DECLARE @ERROR_LINE INT;",
                        "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                        
                        "BEGIN TRANSACTION",
                        "    BEGIN TRY",
                        "        DELETE FROM SYS_SYSTEM_ROLE_CONDITION ",
                        "         WHERE SYS_ID = {SYS_ID}",
                        "           AND ROLE_CONDITION_ID = {ROLE_CONDITION_ID};",

                        "        DELETE FROM SYS_SYSTEM_ROLE_CONDITION_COLLECT ",
                        "         WHERE SYS_ID = {SYS_ID}",
                        "           AND ROLE_CONDITION_ID = {ROLE_CONDITION_ID};",

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

            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SysSystemRoleConditionDetailPara.ParaField.ROLE_CONDITION_ID, Value = para.RoleConditionID });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumDeleteSystemRoleConditionDetailResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion
    }

    public class MongoSystemRoleConditionDetail : Mongo_BaseAP
    {
        public MongoSystemRoleConditionDetail(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemRoleCondotionPara : DBEntity.DBTableRow
        {
            public enum ParaField
            {
                SYS_ID,
                ROLE_CONDITION_ID
            }

            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;

            [DBTypeProperty("SYS_NM")]
            public DBNVarChar SysNM;

            [DBTypeProperty("ROLE_CONDITION_ID")]
            public DBVarChar RoleConditionID;

            [DBTypeProperty("ROLES")]
            public List<DBVarChar> Roles;

            [DBTypeProperty("ROLE_CONDITION_NM_ZH_TW")]
            public DBNVarChar RoleConditionNMZHTW;

            [DBTypeProperty("ROLE_CONDITION_NM_ZH_CN")]
            public DBNVarChar RoleConditionNMZHCN;

            [DBTypeProperty("ROLE_CONDITION_NM_EN_US")]
            public DBNVarChar RoleConditionNMENUS;

            [DBTypeProperty("ROLE_CONDITION_NM_TH_TH")]
            public DBNVarChar RoleConditionNMTHTH;

            [DBTypeProperty("ROLE_CONDITION_NM_JA_JP")]
            public DBNVarChar RoleConditionNMJAJP;

            [DBTypeProperty("ROLE_CONDITION_SYNTAX")]
            public DBNVarChar RoleConditionSynTax;

            [DBTypeProperty("SORT_ORDER")]
            public DBVarChar SortOrder;

            [DBTypeProperty("REMARK")]
            public DBVarChar Remark;

            [DBTypeProperty("UPD_USER_ID")]
            public DBVarChar UpdUserID;

            [DBTypeProperty("UPD_USER_NM")]
            public DBNVarChar UpdUserNM;

            [DBTypeProperty("UPD_DT")]
            public DBDateTime UpdDT;

            [DBTypeProperty("ROLE_CONDITION_RULES")]
            public RecordLogSystemRoleConditionGroupRule RoleConditionRules;
        }

        public class SystemRoleCondotion : MongoDocument
        {
            public enum DataField
            {
                SYS_ID,
                ROLE_CONDITION_RULES
            }

            [DBTypeProperty("LOG_NO")]
            public DBVarChar LogNo;
            [DBTypeProperty("SYS_ID")]
            public DBVarChar SysID;
            [DBTypeProperty("ROLE_CONDITION_ID")]
            public DBVarChar RoleConditionID;
            [DBTypeProperty("ROLE_CONDITION_RULES")]
            public RecordLogSystemRoleConditionGroupRule RoleConditionRules;
        }
        
        #region - 讀取系統角色預設權限最新資料 -
        public SystemRoleCondotion SelectSysSystemRoleCondition(SystemRoleCondotionPara para)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.SYS_SYSTEM_ROLE_CONDTION.ToString());
            command.SetRowCount(1);
            
            command.AddFields(EnumSpecifiedFieldType.Select, SystemRoleCondotion.DataField.ROLE_CONDITION_RULES.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SystemRoleCondotionPara.ParaField.SYS_ID.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SystemRoleCondotionPara.ParaField.ROLE_CONDITION_ID.ToString());
            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemRoleCondotionPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemRoleCondotionPara.ParaField.ROLE_CONDITION_ID.ToString(), Value = para.RoleConditionID });

            return Select<SystemRoleCondotion>(command, dbParameters).SingleOrDefault();
        }
        #endregion
        
        #region - 刪除統角色預設權限資料 -
        public void DeleteSystemRoleCondotion(SystemRoleCondotionPara para)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.SYS_SYSTEM_ROLE_CONDTION.ToString());
            command.SetRowCount(1);
            command.AddFields(EnumSpecifiedFieldType.Select, SystemRoleCondotion.DataField.SYS_ID.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SystemRoleCondotionPara.ParaField.SYS_ID.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SystemRoleCondotionPara.ParaField.ROLE_CONDITION_ID.ToString());

            DBParameters dbParameters = new DBParameters();
            dbParameters.Add(new DBParameter { Name = SystemRoleCondotionPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = SystemRoleCondotionPara.ParaField.ROLE_CONDITION_ID.ToString(), Value = para.RoleConditionID });
            SystemRoleCondotion result = Select<SystemRoleCondotion>(command, dbParameters).SingleOrDefault();
            if (result != null)
            {
                Delete(command, result);
            }
        }
        #endregion
    }
}