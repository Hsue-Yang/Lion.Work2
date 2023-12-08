using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using LionTech.Utility;

namespace LionTech.Entity.ERP.Authorization
{
    public class EntityERPUser : EntityAuthorization
    {
        public EntityERPUser(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 更新應用系統角色檔 -
        public class UserSystemRolePara
        {
            public enum ParaField
            {
                USER_ID,
                SYS_ID,
                ROLE_ID,
                UPD_USER_ID
            }

            public List<UserSystemRoleApply> UserSystemRoleApplys;
        }

        public class UserSystemRoleApply
        {
            public DBVarChar WFNo;
            public DBVarChar UserID;
            public DBVarChar SysID;
            public DBVarChar RoleID;
            public DBVarChar UpdUserID;
            public DBChar ModifyType;
            public DBNVarChar SysNM;
            public DBNVarChar RoleNM;
        }

        public enum EnumEditUserSystemRoleResult
        {
            Success,
            Failure
        }

        public EnumEditUserSystemRoleResult EditUserSystemRole(UserSystemRolePara para)
        {
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "DECLARE @RESULT CHAR(1) = 'N';",
                    "DECLARE @ERROR_LINE INT;",
                    "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                    "BEGIN TRANSACTION",
                    "    BEGIN TRY"
                }));

            foreach (var motifyResult in para.UserSystemRoleApplys)
            {
                commandText.AppendLine(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                    " DELETE SYS_USER_SYSTEM_ROLE WHERE SYS_ID = {SYS_ID} AND USER_ID = {USER_ID} AND ROLE_ID = {ROLE_ID}",
                    " IF EXISTS(SELECT * FROM SYS_SYSTEM_ROLE WHERE SYS_ID = {SYS_ID} AND ROLE_ID = {ROLE_ID})",
                    " BEGIN",
                    "     INSERT INTO SYS_USER_SYSTEM_ROLE",
                    "          ( USER_ID",
                    "          , SYS_ID",
                    "          , ROLE_ID",
                    "          , UPD_USER_ID",
                    "          , UPD_DT",
                    "          ) ",
                    "     VALUES",
                    "          ( {USER_ID}",
                    "          , {SYS_ID}",
                    "          , {ROLE_ID}",
                    "          , {UPD_USER_ID}",
                    "          , GETDATE()",
                    "          );",
                    " END "
                    ),
                    new List<DBParameter>
                    {
                        new DBParameter { Name = UserSystemRolePara.ParaField.USER_ID, Value = motifyResult.UserID },
                        new DBParameter { Name = UserSystemRolePara.ParaField.SYS_ID, Value = motifyResult.SysID },
                        new DBParameter { Name = UserSystemRolePara.ParaField.ROLE_ID, Value = motifyResult.RoleID },
                        new DBParameter { Name = UserSystemRolePara.ParaField.UPD_USER_ID, Value = motifyResult.UpdUserID }
                    }));
            }

            commandText.AppendLine(string.Join(Environment.NewLine,
                new object[]
                {
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

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), null).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumEditUserSystemRoleResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion

        #region - 建立使用者帳號 -
        public class UserAccountPara
        {
            public enum ParaField
            {
                USER_ID,
                USER_NM,
                USER_COM_ID,
                USER_SALARY_COM_ID,
                USER_UNIT_ID,
                USER_TEAM_ID,
                USER_TITLE_ID,
                USER_WORK_ID,
                IS_LEFT,
                UPD_USER_ID,
                UPD_DT,
                UPD_EDI_EVENT_NO,

                USER_AREA,
                USER_WORK_COM,
                USER_GROUP,
                USER_PLACE,
                USER_DEPT,
                USER_TEAM,
                USER_JOB_TITLE,
                USER_BIZ_TITLE,
                USER_LEVEL,
                USER_TITLE,

                ROLE_GROUP_ID,
                RESTRICT_TYPE,
                USER_PWD,
                PWD_VALID_DATE,
                ERROR_TIMES,
                IS_LOCK,
                LOCK_DT,
                IS_DISABLE,
                LEFT_DATE,
                IS_DAILY_FIRST,
                LAST_LOGIN_DATE,

                USER_ENM,
                USER_IDNO,
                USER_BIRTHDAY,
                USER_TEL,
                USER_EXTENSION,
                USER_MOBILE,
                USER_EMAIL,
                USER_GOOGLE_ACCOUNT,
                IS_GACC_ENABLE
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UserComID;
            public DBVarChar UserSalaryComID;
            public DBVarChar UserUnitID;
            public DBVarChar UserTeamID;
            public DBVarChar UserTitleID;
            public DBVarChar UserWorkID;
            public DBChar IsLeft;
            public DBVarChar UpdUserID;
            public DBDateTime UpdDT;
            public DBChar UpdEdiEventNO;

            public DBVarChar UserArea;
            public DBVarChar UserWorkCom;
            public DBVarChar UserGroup;
            public DBVarChar UserPlace;
            public DBVarChar UserDept;
            public DBVarChar UserTeam;
            public DBVarChar UserJobTitle;
            public DBVarChar UserBizTitle;
            public DBVarChar UserLevel;
            public DBVarChar UserTitle;

            public DBVarChar RoleGroupID;
            public DBChar RestrictType;
            public DBVarChar UserPWD;
            public DBChar PWDValidDate;
            public DBInt ErrorTimes;
            public DBChar IsLock;
            public DBDateTime LockDT;
            public DBChar IsDisable;
            public DBChar LeftDate;
            public DBChar IsDailyFirst;
            public DBChar LastLoginDate;

            public DBNVarChar UserENM;
            public DBVarChar UserIDNO;
            public DBVarChar UserBirthday;
            public DBVarChar UserTel;
            public DBVarChar UserExtension;
            public DBVarChar UserMobile;
            public DBVarChar UserEmail;
            public DBVarChar UserGoogleAccount;
            public DBChar IsGaccEnable;
        }

        public class UserAccount : DBTableRow
        {
            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UserComID;
            public DBVarChar UserSalaryComID;
            public DBVarChar UserUnitID;
            public DBVarChar UserTeamID;
            public DBVarChar UserTitleID;
            public DBVarChar UserWorkID;
            public DBChar IsLeft;
            public DBVarChar UpdUserID;
            public DBDateTime UpdDT;
            public DBChar UpdEdiEventNO;

            public DBVarChar UserArea;
            public DBVarChar UserGroup;
            public DBVarChar UserPlace;
            public DBVarChar UserDept;
            public DBVarChar UserTeam;
            public DBVarChar UserJobTitle;
            public DBVarChar UserBizTitle;
            public DBVarChar UserLevel;
            public DBVarChar UserTitle;

            public DBVarChar RoleGroupID;
            public DBChar RestrictType;
            public DBVarChar UserPWD;
            public DBChar PWDValidDate;
            public DBInt ErrorTimes;
            public DBChar IsLock;
            public DBDateTime LockDT;
            public DBChar IsDisable;
            public DBChar LeftDate;
            public DBChar IsDailyFirst;
            public DBChar LastLoginDate;

            public DBNVarChar UserENM;
            public DBVarChar UserIDNO;
            public DBVarChar UserBirthday;
            public DBVarChar UserTel;
            public DBVarChar UserExtension;
            public DBVarChar UserMobile;
            public DBVarChar UserEmail;
            public DBVarChar UserGoogleAccount;
            public DBChar IsGaccEnable;
        }

        public enum EnumCreateUserAccountResult
        {
            Success, Failure
        }

        public EnumCreateUserAccountResult CreateUserAccount(UserAccountPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText =
                new StringBuilder(string.Join(Environment.NewLine,
                    new object[]
                    {
                        " DECLARE @ERROR_LINE INT;",
                        " DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                        " DECLARE @RESULT CHAR(1) = 'N';",
                        " DECLARE @USER_ID VARCHAR(20) = {USER_ID};",
                        "   BEGIN TRANSACTION",
                        "       BEGIN TRY",

                        "           DELETE FROM RAW_CM_USER ",
                        "            WHERE USER_ID = @USER_ID;",

                        "           DELETE FROM RAW_CM_USER_ORG ",
                        "            WHERE USER_ID = @USER_ID;",

                        "           DELETE FROM SYS_USER_MAIN ",
                        "            WHERE USER_ID = @USER_ID;",

                        "           DELETE FROM SYS_USER_DETAIL ",
                        "            WHERE USER_ID = @USER_ID;",

                        "           DELETE FROM SYS_USER_SYSTEM_ROLE ",
                        "            WHERE USER_ID = @USER_ID;",

                        "           DELETE FROM SYS_USER_FUN ",
                        "            WHERE USER_ID = @USER_ID;",

                        "           DELETE FROM SYS_USER_FUN_MENU ",
                        "            WHERE USER_ID = @USER_ID;",

                        "           INSERT INTO dbo.RAW_CM_USER",
                        "                ( USER_ID",
                        "                , USER_NM",
                        "                , USER_COM_ID",
                        "                , USER_SALARY_COM_ID",
                        "                , USER_UNIT_ID",
                        "                , USER_TEAM_ID",
                        "                , USER_TITLE_ID",
                        "                , USER_WORK_ID",
                        "                , IS_LEFT",
                        "                , UPD_USER_ID",
                        "                , UPD_DT",
                        "                , UPD_EDI_EVENT_NO",
                        "                )",
                        "           VALUES",
                        "                ( @USER_ID",
                        "                , {USER_NM}",
                        "                , {USER_COM_ID}",
                        "                , {USER_SALARY_COM_ID}",
                        "                , {USER_UNIT_ID}",
                        "                , {USER_TEAM_ID}",
                        "                , {USER_TITLE_ID}",
                        "                , {USER_WORK_ID}",
                        "                , {IS_LEFT}",
                        "                , {UPD_USER_ID}",
                        "                , {UPD_DT}",
                        "                , {UPD_EDI_EVENT_NO}",
                        "                );",

                        "           INSERT INTO dbo.RAW_CM_USER_ORG",
                        "                ( USER_ID",
                        "                , USER_COM_ID",
                        "                , USER_AREA",
                        "                , USER_GROUP",
                        "                , USER_PLACE",
                        "                , USER_DEPT",
                        "                , USER_TEAM",
                        "                , USER_TITLE",
                        "                , USER_JOB_TITLE",
                        "                , USER_BIZ_TITLE",
                        "                , USER_LEVEL",
                        "                , UPD_USER_ID",
                        "                , UPD_DT",
                        "                , UPD_EDI_EVENT_NO",
                        "                )",
                        "           VALUES",
                        "                ( @USER_ID",
                        "                , {USER_WORK_COM}",
                        "                , {USER_AREA}",
                        "                , {USER_GROUP}",
                        "                , {USER_PLACE}",
                        "                , {USER_DEPT}",
                        "                , {USER_TEAM}",
                        "                , {USER_TITLE}",
                        "                , {USER_JOB_TITLE}",
                        "                , {USER_BIZ_TITLE}",
                        "                , {USER_LEVEL}",
                        "                , {UPD_USER_ID}",
                        "                , {UPD_DT}",
                        "                , {UPD_EDI_EVENT_NO}",
                        "                );",

                        "           INSERT INTO dbo.SYS_USER_MAIN",
                        "                ( USER_ID",
                        "                , ROLE_GROUP_ID",
                        "                , RESTRICT_TYPE",
                        "                , USER_NM",
                        "                , USER_PWD",
                        "                , PWD_VALID_DATE",
                        "                , ERROR_TIMES",
                        "                , IS_LOCK",
                        "                , LOCK_DT",
                        "                , IS_DISABLE",
                        "                , IS_LEFT",
                        "                , LEFT_DATE",
                        "                , IS_DAILY_FIRST",
                        "                , LAST_LOGIN_DATE",
                        "                , UPD_USER_ID",
                        "                , UPD_DT",
                        "                )",
                        "           VALUES",
                        "                ( @USER_ID",
                        "                , {ROLE_GROUP_ID}",
                        "                , {RESTRICT_TYPE}",
                        "                , {USER_NM}",
                        "                , {USER_PWD}",
                        "                , {PWD_VALID_DATE}",
                        "                , {ERROR_TIMES}",
                        "                , {IS_LOCK}",
                        "                , {LOCK_DT}",
                        "                , {IS_DISABLE}",
                        "                , {IS_LEFT}",
                        "                , {LEFT_DATE}",
                        "                , {IS_DAILY_FIRST}",
                        "                , {LAST_LOGIN_DATE}",
                        "                , {UPD_USER_ID}",
                        "                , {UPD_DT}",
                        "                );",

                        "           INSERT INTO dbo.SYS_USER_DETAIL",
                        "                ( USER_ID",
                        "                , USER_ENM",
                        "                , USER_IDNO",
                        "                , USER_BIRTHDAY",
                        "                , USER_TEL",
                        "                , USER_EXTENSION",
                        "                , USER_MOBILE",
                        "                , USER_EMAIL",
                        "                , USER_GOOGLE_ACCOUNT",
                        "                , IS_GACC_ENABLE",
                        "                , UPD_USER_ID",
                        "                , UPD_DT",
                        "                )",
                        "           VALUES",
                        "                ( @USER_ID",
                        "                , {USER_ENM}",
                        "                , {USER_IDNO}",
                        "                , {USER_BIRTHDAY}",
                        "                , {USER_TEL}",
                        "                , {USER_EXTENSION}",
                        "                , {USER_MOBILE}",
                        "                , {USER_EMAIL}",
                        "                , {USER_GOOGLE_ACCOUNT}",
                        "                , {IS_GACC_ENABLE}",
                        "                , {UPD_USER_ID}",
                        "                , {UPD_DT}",
                        "                )",
                        "         SET @RESULT = 'Y';",
                        "         COMMIT;",
                        "     END TRY",
                        "     BEGIN CATCH",
                        "         SET @RESULT = 'N';",
                        "         SET @ERROR_LINE = ERROR_LINE();",
                        "         SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                        "         ROLLBACK TRANSACTION;",
                        "     END CATCH;",
                        " SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                    }));

            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_NM, Value = para.UserNM });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_COM_ID, Value = para.UserComID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_SALARY_COM_ID, Value = para.UserSalaryComID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_UNIT_ID, Value = para.UserUnitID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_TEAM_ID, Value = para.UserTeamID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_TITLE_ID, Value = para.UserTitleID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_WORK_ID, Value = para.UserWorkID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.IS_LEFT, Value = para.IsLeft });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.UPD_DT, Value = para.UpdDT });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.UPD_EDI_EVENT_NO, Value = para.UpdEdiEventNO });

            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_AREA, Value = para.UserArea });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_WORK_COM, Value = para.UserWorkCom });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_GROUP, Value = para.UserGroup });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_PLACE, Value = para.UserPlace });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_DEPT, Value = para.UserDept });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_TEAM, Value = para.UserTeam });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_TITLE, Value = para.UserTitle });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_JOB_TITLE, Value = para.UserJobTitle });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_BIZ_TITLE, Value = para.UserBizTitle });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_LEVEL, Value = para.UserLevel });

            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.ROLE_GROUP_ID, Value = para.RoleGroupID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.RESTRICT_TYPE, Value = para.RestrictType });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_PWD, Value = para.UserPWD });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.PWD_VALID_DATE, Value = para.PWDValidDate });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.ERROR_TIMES, Value = para.ErrorTimes });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.IS_LOCK, Value = para.IsLock });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.LOCK_DT, Value = para.LockDT });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.IS_DISABLE, Value = para.IsDisable });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.LEFT_DATE, Value = para.LeftDate });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.IS_DAILY_FIRST, Value = para.IsDailyFirst });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.LAST_LOGIN_DATE, Value = para.LastLoginDate });

            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_ENM, Value = para.UserENM });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_IDNO, Value = para.UserIDNO });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_BIRTHDAY, Value = para.UserBirthday });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_TEL, Value = para.UserTel });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_EXTENSION, Value = para.UserExtension });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_MOBILE, Value = para.UserMobile });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_EMAIL, Value = para.UserEmail });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_GOOGLE_ACCOUNT, Value = para.UserGoogleAccount });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.IS_GACC_ENABLE, Value = para.IsGaccEnable });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumCreateUserAccountResult.Success;
            }
            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion

        #region - 紀錄使用者系統角色 -
        public class LogUserSystemRolePara
        {
            public enum ParaField
            {
                USER_ID,
                API_NO,
                EXEC_SYS_ID,
                IP_ADDRESS,
                UPD_USER_ID,
                SYS_USER_SYSTEM_ROLE_CONDITION_JSON_STR,
                SYS_SYSTEM_ROLE_CONDTION_JSON_STR,
                USER_SYSTEM_ROLE_APPLY_JSON_STR
            }

            public DBVarChar UserID;
            public DBChar ApiNO;
            public DBVarChar ExecSysID;
            public DBVarChar IPAddress;
            public DBVarChar UpdUserID;
            public DBNVarChar SysUserSystemRoleConditionJsonStr;
            public DBNVarChar SysRoleConditionJsonStr;
            public DBNVarChar UserSysRoleApplyJsonStr;
        }

        public class LogUserSystemRole : ExecuteResult
        {
            public DBVarChar SysID;
            public DBVarChar RoleID;
        }

        public enum EnumAddLogUserSystemRoleResult
        {
            Success,
            Failure
        }

        public List<LogUserSystemRole> AddLogUserSystemRole(LogUserSystemRolePara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @RESULT CHAR(50) = 'Failure';",
                "DECLARE @ERROR_LINE INT;",
                "DECLARE @ERROR_NUMBER INT;",
                "DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                "DECLARE @LOG_NO CHAR(6) = NULL;",
                "DECLARE @RETURN_DATA TABLE (",
                "    SysID VARCHAR(12),",
                "    RoleID VARCHAR(20),",
                "    Result VARCHAR(50),",
                "    ErrorLine INT,",
                "    ErrorNumber INT,",
                "    ErrorMessage NVARCHAR(4000)",
                ");",
                "BEGIN TRANSACTION",
                "    BEGIN TRY",
                "        EXECUTE dbo.SP_LOG_USER_SYSTEM_ROLE {USER_ID} ,NULL ,NULL ,{API_NO} ,'"+ Mongo_BaseAP.EnumModifyType.U +"', {EXEC_SYS_ID} ,{IP_ADDRESS} ,{UPD_USER_ID} ,@LOG_NO OUTPUT;",
                "        INSERT INTO @RETURN_DATA (SysID, RoleID)",
                "        EXECUTE dbo.SP_LOG_USER_SYSTEM_ROLE_SET_DATA {SYS_USER_SYSTEM_ROLE_CONDITION_JSON_STR} ,{SYS_SYSTEM_ROLE_CONDTION_JSON_STR} ,{USER_SYSTEM_ROLE_APPLY_JSON_STR} ,{USER_ID} ,@LOG_NO",
                "        UPDATE @RETURN_DATA SET Result = 'Success'",
                "        SET @RESULT = 'Success';",
                "        COMMIT;",
                "    END TRY",
                "    BEGIN CATCH",
                "        SET @RESULT = 'Failure';",
                "        SET @ERROR_LINE = ERROR_LINE();",
                "        SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                "        INSERT INTO @RETURN_DATA (Result, ErrorLine, ErrorNumber, ErrorMessage)",
                "        SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_NUMBER AS ErrorNumber, @ERROR_MESSAGE AS ErrorMessage;",
                "        ROLLBACK TRANSACTION;",
                "    END CATCH;",
                "SELECT * FROM @RETURN_DATA;"
        }));

            dbParameters.Add(new DBParameter { Name = LogUserSystemRolePara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = LogUserSystemRolePara.ParaField.API_NO, Value = para.ApiNO });
            dbParameters.Add(new DBParameter { Name = LogUserSystemRolePara.ParaField.EXEC_SYS_ID, Value = para.ExecSysID });
            dbParameters.Add(new DBParameter { Name = LogUserSystemRolePara.ParaField.IP_ADDRESS, Value = para.IPAddress });
            dbParameters.Add(new DBParameter { Name = LogUserSystemRolePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = LogUserSystemRolePara.ParaField.SYS_USER_SYSTEM_ROLE_CONDITION_JSON_STR, Value = para.SysUserSystemRoleConditionJsonStr });
            dbParameters.Add(new DBParameter { Name = LogUserSystemRolePara.ParaField.SYS_SYSTEM_ROLE_CONDTION_JSON_STR, Value = para.SysRoleConditionJsonStr });
            dbParameters.Add(new DBParameter { Name = LogUserSystemRolePara.ParaField.USER_SYSTEM_ROLE_APPLY_JSON_STR, Value = para.UserSysRoleApplyJsonStr });

            var logUserSystemRoleList = GetEntityList<LogUserSystemRole>(commandText.ToString(), dbParameters);
            var result = logUserSystemRoleList.FirstOrDefault();

            var enumResult = ((EnumAddLogUserSystemRoleResult)Enum.Parse(typeof(EnumAddLogUserSystemRoleResult), result.Result.GetValue()));

            if (enumResult == EnumAddLogUserSystemRoleResult.Failure)
            {
                throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
            }

            return logUserSystemRoleList;
        }
        #endregion

        #region - 取得代碼對應名稱 -
        public enum EnumCMCodeType
        {
            CMCode,
            CMOrgCom,
            CMOrgUnit
        }

        public class CMCodeInfoPara : DBCulture
        {
            public CMCodeInfoPara(string cultureID)
                : base(cultureID)
            {
            }

            public enum ParaField
            {
                CODE_ID,
                CODE_KIND,
                CULTURE_ID,
                CODE_TYPE
            }

            public List<CMCode> CMCodes;
        }

        public class CMCode
        {
            public EnumCMCodeType CMCodeType;
            public DBVarChar CodeID;
            public DBVarChar CodeKind;
        }

        public class CMCodeInfo : DBTableRow
        {
            public DBVarChar CMCodeType;
            public DBVarChar CodeID;
            public DBNVarChar CodeNM;
            public DBVarChar CodeKind;
        }

        public List<CMCodeInfo> SelectCMCodeInfoList(CMCodeInfoPara para)
        {
            StringBuilder commandText = new StringBuilder();
            List<string> comm = new List<string>();
            List<DBParameter> dbParameters = new List<DBParameter>();

            foreach (var row in para.CMCodes)
            {
                switch (row.CMCodeType)
                {
                    case EnumCMCodeType.CMCode:
                        comm.Add(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                            "   SELECT {CODE_TYPE} AS CMCodeType",
                            "        , CODE_ID AS CodeID",
                            "        , dbo.FN_GET_CM_NM({CODE_KIND}, CODE_ID, {CULTURE_ID}) AS CodeNM",
                            "        , CODE_KIND AS CodeKind",
                            "     FROM CM_CODE ",
                            "    WHERE CODE_KIND = {CODE_KIND} ",
                            "      AND CODE_ID = {CODE_ID}"
                            ),
                            new List<DBParameter>
                            {
                                new DBParameter { Name = CMCodeInfoPara.ParaField.CODE_TYPE, Value = new DBVarChar(row.CMCodeType) },
                                new DBParameter { Name = CMCodeInfoPara.ParaField.CODE_ID, Value = row.CodeID },
                                new DBParameter { Name = CMCodeInfoPara.ParaField.CODE_KIND.ToString(), Value = row.CodeKind },
                                new DBParameter { Name = CMCodeInfoPara.ParaField.CULTURE_ID, Value = new DBVarChar(para.CultureID) }
                            }));
                        break;
                    case EnumCMCodeType.CMOrgCom:
                        comm.Add(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                            "   SELECT {CODE_TYPE} AS CMCodeType",
                            "        , COM_ID AS CodeID",
                            "        , COM_NM AS CodeNM",
                            "        , NULL AS CodeKind",
                            "     FROM RAW_CM_ORG_COM",
                            "    WHERE COM_ID = {CODE_ID}"
                            ),
                            new List<DBParameter>
                            {
                                new DBParameter { Name = CMCodeInfoPara.ParaField.CODE_TYPE, Value = new DBVarChar(row.CMCodeType) },
                                new DBParameter { Name = CMCodeInfoPara.ParaField.CODE_ID.ToString(), Value = row.CodeID }
                            }));
                        break;
                    case EnumCMCodeType.CMOrgUnit:
                        {
                            comm.Add(GetCommandText(ProviderName, string.Join(Environment.NewLine,
                                "   SELECT {CODE_TYPE} AS CMCodeType",
                                "        , UNIT_ID AS CodeID",
                                "        , UNIT_NM AS CodeNM",
                                "        , NULL AS CodeKind",
                                "     FROM RAW_CM_ORG_UNIT ",
                                "    WHERE UNIT_ID = {CODE_ID}"
                                ),
                                new List<DBParameter>
                                {
                                new DBParameter { Name = CMCodeInfoPara.ParaField.CODE_TYPE, Value = new DBVarChar(row.CMCodeType) },
                                new DBParameter { Name = CMCodeInfoPara.ParaField.CODE_ID.ToString(), Value = row.CodeID }
                                }));
                            break;
                        }
                }
            }

            if (comm.Any())
            {
                commandText.AppendLine(string.Join(" UNION ", comm));
            }
            return GetEntityList<CMCodeInfo>(commandText.ToString(), dbParameters);
        }
        #endregion

        #region - 編輯使用者權限 -
        public class UserSysRolePara
        {
            public enum ParaField
            {
                USER_ID,
                UPD_USER_ID
            }

            public DBVarChar SysID;
            public DBVarChar RoleID;
            public DBVarChar UserID;
            public DBVarChar UpdUserID;
        }

        public class SysUserRole : DBTableRow
        {
            public enum DataField
            {
                SYS_ID,
                SYS_NM,
                ROLE_ID,
                ROLE_NM,
                ROLE_CONDITION_ID,
                ROLE_CONDITION_NM,
                ROLE_CONDITION_SYNTAX
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
            public DBVarChar RoleConditionID;
            public DBNVarChar RoleConditionNM;
            public DBNVarChar RoleConditionSyntax;
        }

        public List<SysUserRole> EditSysUserRole(UserSysRolePara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "DECLARE @USER_ID VARCHAR(20) = {USER_ID};",
                "DECLARE @UPD_USER_ID VARCHAR(50) = {UPD_USER_ID};",
                "EXECUTE dbo.SP_FUN_SYS_USER_SYSTEM_ROLE_CONDTION @USER_ID ,@UPD_USER_ID;"
            }));

            dbParameters.Add(new DBParameter { Name = UserSysRolePara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserSysRolePara.ParaField.UPD_USER_ID, Value = para.UpdUserID });

            DataTable dataTable = base.GetDataTable(commandText.ToString(), dbParameters);
            List<SysUserRole> userSystemList = new List<SysUserRole>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SysUserRole userRole = new SysUserRole()
                    {
                        SysID = new DBVarChar(dataRow[SysUserRole.DataField.SYS_ID.ToString()]),
                        SysNM = new DBNVarChar(dataRow[SysUserRole.DataField.SYS_NM.ToString()]),
                        RoleID = new DBVarChar(dataRow[SysUserRole.DataField.ROLE_ID.ToString()]),
                        RoleNM = new DBNVarChar(dataRow[SysUserRole.DataField.ROLE_NM.ToString()]),
                        RoleConditionID = new DBVarChar(dataRow[SysUserRole.DataField.ROLE_CONDITION_ID.ToString()]),
                        RoleConditionNM = new DBNVarChar(dataRow[SysUserRole.DataField.ROLE_CONDITION_NM.ToString()]),
                        RoleConditionSyntax = new DBNVarChar(dataRow[SysUserRole.DataField.ROLE_CONDITION_SYNTAX.ToString()])
                    };
                    userSystemList.Add(userRole);
                }
                return userSystemList;
            }
            return userSystemList;
        }
        #endregion

        #region - 編輯使用者 -
        public enum EnumRawCMUserResult
        {
            Success, Failure
        }

        public EnumRawCMUserResult EditRawCMUser(UserAccountPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText =
                new StringBuilder(string.Join(Environment.NewLine,
                    new object[]
                    {
                        " DECLARE @RESULT CHAR(1) = 'N';",
                        " DECLARE @ERROR_LINE INT;",
                        " DECLARE @ERROR_MESSAGE NVARCHAR(4000);",
                        " DECLARE @USER_ID VARCHAR(20) = {USER_ID};",
                        "     BEGIN TRANSACTION",
                        "         BEGIN TRY",
                        "             DELETE FROM RAW_CM_USER_ORG ",
                        "              WHERE USER_ID = {USER_ID};",

                        "             IF EXISTS(SELECT * FROM RAW_CM_USER WHERE USER_ID = @USER_ID)",
                        "             BEGIN",
                        "                 UPDATE RAW_CM_USER",
                        "                    SET USER_NM = {USER_NM}",
                        "                      , USER_COM_ID = {USER_COM_ID}",
                        "                      , USER_SALARY_COM_ID = {USER_SALARY_COM_ID}",
                        "                      , USER_UNIT_ID = {USER_UNIT_ID}",
                        "                      , USER_TEAM_ID = {USER_TEAM_ID}",
                        "                      , USER_TITLE_ID = {USER_TITLE_ID}",
                        "                      , USER_WORK_ID = {USER_WORK_ID}",
                        "                      , IS_LEFT = {IS_LEFT}",
                        "                      , UPD_USER_ID = {UPD_USER_ID}",
                        "                      , UPD_DT = {UPD_DT}",
                        "                      , UPD_EDI_EVENT_NO = {UPD_EDI_EVENT_NO}",
                        "                 WHERE USER_ID = @USER_ID",
                        "             END",
                        "             ELSE",
                        "             BEGIN",
                        "                 INSERT INTO dbo.RAW_CM_USER",
                        "                      ( USER_ID",
                        "                      , USER_NM",
                        "                      , USER_COM_ID",
                        "                      , USER_SALARY_COM_ID",
                        "                      , USER_UNIT_ID",
                        "                      , USER_TEAM_ID",
                        "                      , USER_TITLE_ID",
                        "                      , USER_WORK_ID",
                        "                      , IS_LEFT",
                        "                      , UPD_USER_ID",
                        "                      , UPD_DT",
                        "                      , UPD_EDI_EVENT_NO",
                        "                      )",
                        "                 VALUES",
                        "                      ( @USER_ID",
                        "                      , {USER_NM}",
                        "                      , {USER_COM_ID}",
                        "                      , {USER_SALARY_COM_ID}",
                        "                      , {USER_UNIT_ID}",
                        "                      , {USER_TEAM_ID}",
                        "                      , {USER_TITLE_ID}",
                        "                      , {USER_WORK_ID}",
                        "                      , {IS_LEFT}",
                        "                      , {UPD_USER_ID}",
                        "                      , {UPD_DT}",
                        "                      , {UPD_EDI_EVENT_NO}",
                        "                      );",
                        "             END",

                        "             INSERT INTO dbo.RAW_CM_USER_ORG",
                        "                  ( USER_ID",
                        "                  , USER_COM_ID",
                        "                  , USER_AREA",
                        "                  , USER_GROUP",
                        "                  , USER_PLACE",
                        "                  , USER_DEPT",
                        "                  , USER_TEAM",
                        "                  , USER_TITLE",
                        "                  , USER_JOB_TITLE",
                        "                  , USER_BIZ_TITLE",
                        "                  , USER_LEVEL",
                        "                  , UPD_USER_ID",
                        "                  , UPD_DT",
                        "                  , UPD_EDI_EVENT_NO",
                        "                  )",
                        "             VALUES",
                        "                  ( @USER_ID",
                        "                  , {USER_COM_ID}",
                        "                  , {USER_AREA}",
                        "                  , {USER_GROUP}",
                        "                  , {USER_PLACE}",
                        "                  , {USER_DEPT}",
                        "                  , {USER_TEAM}",
                        "                  , {USER_TITLE}",
                        "                  , {USER_JOB_TITLE}",
                        "                  , {USER_BIZ_TITLE}",
                        "                  , {USER_LEVEL}",
                        "                  , {UPD_USER_ID}",
                        "                  , {UPD_DT}",
                        "                  , {UPD_EDI_EVENT_NO}",
                        "                  )",
                        "           SET @RESULT = 'Y';",
                        "           COMMIT;",
                        "       END TRY",
                        "       BEGIN CATCH",
                        "           SET @RESULT = 'N';",
                        "           SET @ERROR_LINE = ERROR_LINE();",
                        "           SET @ERROR_MESSAGE = ERROR_MESSAGE();",
                        "           ROLLBACK TRANSACTION;",
                        "       END CATCH;",
                        " SELECT @RESULT AS Result, @ERROR_LINE AS ErrorLine, @ERROR_MESSAGE AS ErrorMessage;"
                    }));

            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_NM, Value = para.UserNM });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_COM_ID, Value = para.UserComID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_SALARY_COM_ID, Value = para.UserSalaryComID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_UNIT_ID, Value = para.UserUnitID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_TEAM_ID, Value = para.UserTeamID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_TITLE_ID, Value = para.UserTitleID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_WORK_ID, Value = para.UserWorkID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.IS_LEFT, Value = para.IsLeft });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.UPD_USER_ID, Value = para.UpdUserID });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.UPD_DT, Value = para.UpdDT });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.UPD_EDI_EVENT_NO, Value = para.UpdEdiEventNO });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_AREA, Value = para.UserArea });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_GROUP, Value = para.UserGroup });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_PLACE, Value = para.UserPlace });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_DEPT, Value = para.UserDept });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_TEAM, Value = para.UserTeam });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_TITLE, Value = para.UserTitle });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_JOB_TITLE, Value = para.UserJobTitle });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_BIZ_TITLE, Value = para.UserBizTitle });
            dbParameters.Add(new DBParameter { Name = UserAccountPara.ParaField.USER_LEVEL, Value = para.UserLevel });

            var result = GetEntityList<ExecuteResult>(commandText.ToString(), dbParameters).SingleOrDefault();

            if (result != null &&
                result.Result.GetValue() == EnumYN.Y.ToString())
            {
                return EnumRawCMUserResult.Success;
            }

            throw new EntityExecuteResultException(EnumEntityExceptionMessage.SQLCommandError, result);
        }
        #endregion

        public class ERPUserAccountPara
        {
            public enum ParaField
            {
                USER_ID, USER_NM,
                USER_COM_ID, USER_UNIT_ID,
                USER_PWD, PWD_VALID_DATE,
                IS_LEFT,

                USER_IDNO, USER_BIRTHDAY,
                UPD_USER_ID
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar UserComID;
            public DBVarChar UserUnitID;
            public DBVarChar UserPWD;
            public DBVarChar UserIDNo;
            public DBVarChar UserBirthday;
            public DBVarChar PWDValidDate;
            public DBChar IsLeft;
        }

        public enum EnumCreateAccountResult
        {
            Success, Failure
        }

        public EnumCreateAccountResult CreateAccount(ERPUserAccountPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        DECLARE @ROLE_GROUP_ID VARCHAR(20); ", Environment.NewLine,
                "        SELECT @ROLE_GROUP_ID=ROLE_GROUP_ID FROM SYS_USER_MAIN WHERE USER_ID={USER_ID}; ", Environment.NewLine,

                "        DELETE FROM RAW_CM_USER WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                "        DELETE FROM SYS_USER_MAIN WHERE USER_ID={USER_ID}; ", Environment.NewLine,
                "        DELETE FROM SYS_USER_DETAIL WHERE USER_ID={USER_ID}; ", Environment.NewLine,

                "        INSERT INTO RAW_CM_USER VALUES ( ", Environment.NewLine,
                "            {USER_ID}, {USER_NM}, {USER_COM_ID}, {USER_UNIT_ID}, {IS_LEFT} ", Environment.NewLine,
                "          , {UPD_USER_ID}, GETDATE(), NULL ", Environment.NewLine,
                "        ); ", Environment.NewLine,

                "        INSERT INTO SYS_USER_MAIN VALUES ( ", Environment.NewLine,
                "            {USER_ID}, @ROLE_GROUP_ID, 'N' ", Environment.NewLine,
                "          , {USER_NM}, {USER_PWD}, {PWD_VALID_DATE}, 0 ", Environment.NewLine,
                "          , 'N', NULL, {IS_LEFT}, {IS_LEFT}, NULL ", Environment.NewLine,
                "          , 'N', NULL ", Environment.NewLine,
                "          , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "        ); ", Environment.NewLine,

                "        INSERT INTO SYS_USER_DETAIL VALUES ( ", Environment.NewLine,
                "            {USER_ID}, NULL, NULL, NULL ", Environment.NewLine,
                "          , NULL, NULL, NULL, NULL ", Environment.NewLine,
                "          , {UPD_USER_ID}, GETDATE() ", Environment.NewLine,
                "        ); ", Environment.NewLine,

                "        IF {IS_LEFT}='N' AND (SELECT USER_ID FROM SYS_USER_SYSTEM_ROLE WHERE USER_ID={USER_ID} AND SYS_ID='ERPAP' AND ROLE_ID='USER') IS NULL ", Environment.NewLine,
                "        BEGIN", Environment.NewLine,
                "            INSERT INTO SYS_USER_SYSTEM_ROLE VALUES ({USER_ID},'ERPAP','USER',{UPD_USER_ID},GETDATE());", Environment.NewLine,
                "        END;", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_NM.ToString(), Value = para.UserNM });
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_COM_ID.ToString(), Value = para.UserComID });
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_UNIT_ID.ToString(), Value = para.UserUnitID });
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_PWD.ToString(), Value = para.UserPWD });
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.PWD_VALID_DATE.ToString(), Value = para.PWDValidDate });
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.IS_LEFT.ToString(), Value = para.IsLeft });
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.UPD_USER_ID.ToString(), Value = base.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumCreateAccountResult.Success : EnumCreateAccountResult.Failure;
        }

        public DBVarChar SelectUserPWD(ERPUserAccountPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT USER_PWD ",
                "  FROM SYS_USER_MAIN ",
                " WHERE USER_ID = {USER_ID} ",
                "   AND IS_LOCK = 'N' ",
                "   AND IS_DISABLE = 'N' "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            return new DBVarChar(base.ExecuteScalar(commandText, dbParameters));
        }

        public class AppUserMobile : DBTableRow
        {
            public DBVarChar AppUUID;
        }

        public List<AppUserMobile> SelectAppUserMobile(ERPUserAccountPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT APP_UUID AS AppUUID ",
                "  FROM APP_USER_MOBILE ",
                " WHERE USER_ID = {USER_ID} ",
                "   AND IS_DISABLE = 'N' "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            return GetEntityList<AppUserMobile>(commandText, dbParameters);
        }

        public enum EnumResetUserPWDResult
        {
            Success, Failure
        }

        public EnumResetUserPWDResult ResetUserPWD(ERPUserAccountPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        UPDATE SYS_USER_MAIN SET ", Environment.NewLine,
                "            USER_PWD={USER_PWD}, PWD_VALID_DATE={PWD_VALID_DATE}, UPD_USER_ID={UPD_USER_ID}, UPD_DT=GETDATE() ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID}; ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_ID.ToString(), Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_PWD.ToString(), Value = para.UserPWD });
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.PWD_VALID_DATE.ToString(), Value = para.PWDValidDate });
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.UPD_USER_ID.ToString(), Value = base.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumResetUserPWDResult.Success : EnumResetUserPWDResult.Failure;
        }

        public enum EnumUnlockUserAccountResult
        {
            Success, Failure
        }

        public EnumUnlockUserAccountResult UnlockUserAccount(ERPUserAccountPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        UPDATE SYS_USER_MAIN SET ", Environment.NewLine,
                "            ERROR_TIMES=0, IS_LOCK='N', LOCK_DT=NULL ", Environment.NewLine,
                "          , UPD_USER_ID={UPD_USER_ID}, UPD_DT=GETDATE() ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID}; ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.UPD_USER_ID.ToString(), Value = base.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumUnlockUserAccountResult.Success : EnumUnlockUserAccountResult.Failure;
        }

        public enum EnumValidateAccountResult
        {
            [Description("V")]
            Valid,
            [Description("I")]
            Invalid,
            [Description("P")]
            UserInfoInvalid,
            [Description("N")]
            NotExist,
            [Description("K")]
            Lock,
            [Description("L")]
            Leave
        }

        public EnumValidateAccountResult SelectAccountStatus(ERPUserAccountPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "DECLARE @IS_LOCK CHAR(1); ", Environment.NewLine,
                "DECLARE @IS_LEFT CHAR(1); ", Environment.NewLine,

                "SET @RESULT='V'; ", Environment.NewLine,

                "IF EXISTS(SELECT USER_ID FROM SYS_USER_MAIN WHERE USER_ID={USER_ID}) ", Environment.NewLine,
                "BEGIN ", Environment.NewLine,
                "    SELECT @IS_LOCK=(CASE WHEN IS_LOCK='Y' AND GETDATE() >= DATEADD(n,30,LOCK_DT) THEN 'N' ELSE IS_LOCK END) ", Environment.NewLine,
                "         , @IS_LEFT=IS_LEFT ", Environment.NewLine,
                "    FROM SYS_USER_MAIN ", Environment.NewLine,
                "    WHERE USER_ID={USER_ID}; ", Environment.NewLine,

                "    IF @IS_LOCK='Y' ", Environment.NewLine,
                "    BEGIN ", Environment.NewLine,
                "        SET @RESULT='K'; ", Environment.NewLine,
                "    END; ", Environment.NewLine,
                "    ELSE IF @IS_LEFT='Y' ", Environment.NewLine,
                "    BEGIN ", Environment.NewLine,
                "        SET @RESULT='L'; ", Environment.NewLine,
                "    END; ", Environment.NewLine,
                "END; ", Environment.NewLine,
                "ELSE ", Environment.NewLine,
                "BEGIN ", Environment.NewLine,
                "    SET @RESULT='N'; ", Environment.NewLine,
                "END; ", Environment.NewLine,

                "SELECT @RESULT; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_ID, Value = para.UserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return Common.GetEnumField<EnumValidateAccountResult>(result.GetValue());
        }

        public EnumValidateAccountResult ValidateUserAccount(ERPUserAccountPara para)
        {
            string commandAdvance = string.Empty;

            if (!string.IsNullOrWhiteSpace(para.UserIDNo.GetValue()) ||
                !string.IsNullOrWhiteSpace(para.UserBirthday.GetValue()))
            {
                commandAdvance = string.Concat(new object[]
                {
                    "    SELECT @USER_CNT=COUNT(1) ", Environment.NewLine,
                    "    FROM SYS_USER_DETAIL ", Environment.NewLine,
                    "    WHERE USER_ID={USER_ID} ", Environment.NewLine,
                    "      AND USER_IDNO={USER_IDNO} AND USER_BIRTHDAY={USER_BIRTHDAY}; ", Environment.NewLine,

                    "    IF @USER_CNT>0 ", Environment.NewLine,
                    "    BEGIN ", Environment.NewLine,
                    "        SET @RESULT='V'; ", Environment.NewLine,
                    "    END; ", Environment.NewLine,
                    "    ELSE ", Environment.NewLine,
                    "    BEGIN ", Environment.NewLine,
                    "        SET @RESULT='P'; ", Environment.NewLine,
                    "    END; ", Environment.NewLine
                });
            }

            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                //"DECLARE @USER_ID VARCHAR(20); ", Environment.NewLine,
                "DECLARE @USER_CNT INT; ", Environment.NewLine,
                
                //"SET @RESULT='I'; ", Environment.NewLine,
                "SET @RESULT='V'; ", Environment.NewLine,

                //已先從ERP驗證密碼
                //"SELECT @USER_ID=USER_ID ", Environment.NewLine,
                //"FROM SYS_USER_MAIN ", Environment.NewLine,
                //"WHERE USER_ID={USER_ID} ", Environment.NewLine,
                //"  AND USER_PWD={USER_PWD}; ", Environment.NewLine,

                //"IF @USER_ID IS NOT NULL ", Environment.NewLine,
                //"BEGIN ", Environment.NewLine,
                //"    SET @RESULT='V'; ", Environment.NewLine,
                commandAdvance,
                //"END; ", Environment.NewLine,
                
                "SELECT @RESULT; "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_PWD.ToString(), Value = para.UserPWD });
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_IDNO.ToString(), Value = para.UserIDNo });
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_BIRTHDAY.ToString(), Value = para.UserBirthday });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return Common.GetEnumField<EnumValidateAccountResult>(result.GetValue());
        }

        public bool ValidateERPUserAccount(ERPUserAccountPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT stfn_pswd AS USER_PWD ",
                "FROM opagm20 ",
                "WHERE stfn_stfn={USER_ID} "
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_PWD.ToString(), Value = para.UserPWD });

            DBChar result = new DBChar(ExecuteScalar(commandText, dbParameters));
            return result.GetValue() == para.UserPWD.GetValue();
        }

        public enum EnumEditUserLoginErrorTimesResult
        {
            Success, Failure
        }

        public EnumEditUserLoginErrorTimesResult EditUserLoginErrorTimes(ERPUserAccountPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "DECLARE @RESULT CHAR(1); ", Environment.NewLine,
                "SET @RESULT = 'N'; ", Environment.NewLine,
                "BEGIN TRANSACTION ", Environment.NewLine,
                "    BEGIN TRY ", Environment.NewLine,

                "        UPDATE SYS_USER_MAIN SET ", Environment.NewLine,
                "            ERROR_TIMES=(ERROR_TIMES+1) ", Environment.NewLine,
                "          , IS_LOCK=(CASE WHEN (ERROR_TIMES+1)>=3 THEN 'Y' ELSE 'N' END) ", Environment.NewLine,
                "          , LOCK_DT=(CASE WHEN (ERROR_TIMES+1)>=3 THEN GETDATE() ELSE LOCK_DT END) ", Environment.NewLine,
                "          , UPD_USER_ID={UPD_USER_ID} ", Environment.NewLine,
                "          , UPD_DT=GETDATE() ", Environment.NewLine,
                "        WHERE USER_ID={USER_ID}; ", Environment.NewLine,

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
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_ID, Value = para.UserID });
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.UPD_USER_ID.ToString(), Value = base.UpdUserID });

            DBChar result = new DBChar(base.ExecuteScalar(commandText, dbParameters));
            return (result.GetValue() == EnumYN.Y.ToString()) ? EnumEditUserLoginErrorTimesResult.Success : EnumEditUserLoginErrorTimesResult.Failure;
        }

        public DBInt SelectAccountErrorTimes(ERPUserAccountPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT ERROR_TIMES ", Environment.NewLine,
                "FROM SYS_USER_MAIN ", Environment.NewLine,
                "WHERE USER_ID={USER_ID}; ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = ERPUserAccountPara.ParaField.USER_ID, Value = para.UserID });

            return new DBInt(base.ExecuteScalar(commandText, dbParameters));
        }

        public class SystemPara
        {
            public enum ParaField
            {
                SYS_ID
            }

            public DBVarChar SysID;
        }

        public class SystemUser : DBTableRow
        {
            public DBVarChar UserID;
        }

        public List<SystemUser> SelectSystemUserList(SystemPara para)
        {
            string commandText = string.Concat(new object[]
            {
                "SELECT USER_ID AS UserID", Environment.NewLine,
                "FROM SYS_USER_SYSTEM ", Environment.NewLine,
                "WHERE SYS_ID={SYS_ID}; ", Environment.NewLine,
            });

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = SystemPara.ParaField.SYS_ID, Value = para.SysID });

            return GetEntityList<SystemUser>(commandText, dbParameters);
        }
    }

    public class MongoERPUser : Mongo_BaseAP
    {
        public MongoERPUser(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 查詢應用系統預設條件 -
        public class SysSystemRoleCondtionPara
        {
            public enum ParaField
            {
                SYS_ID,
                ROLE_CONDITION_ID,
                ROLE_CONDITION_SYNTAX
            }

            public List<SysSystemRoleCondtion> SysSystemRoleCondtions;
        }

        public class SysSystemRoleCondtion : MongoDocument
        {
            public enum DataField
            {
                SYS_ID,
                ROLE_CONDITION_ID,
                ROLE_CONDITION_SYNTAX,
                ROLE_CONDITION_RULES
            }

            public DBVarChar SysID;
            public DBVarChar RoleConditionID;
            public DBNVarChar RoleConditionSynTax;
            public RecordLogSystemRoleConditionGroupRule RoleConditionRules;
        }

        /// <summary>
        /// 查詢應用系統預設條件
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<SysSystemRoleCondtion> SelectSysSystemRoleCondtionList(SysSystemRoleCondtionPara para)
        {
            List<SysSystemRoleCondtion> resultList = new List<SysSystemRoleCondtion>();
            MongoCommand command = new MongoCommand(EnumMongoDocName.SYS_SYSTEM_ROLE_CONDTION.ToString());
            DBParameters dbParameters = new DBParameters();

            foreach (var row in para.SysSystemRoleCondtions)
            {
                command.AddFields(EnumSpecifiedFieldType.Select, SysSystemRoleCondtion.DataField.SYS_ID.ToString());
                command.AddFields(EnumSpecifiedFieldType.Select, SysSystemRoleCondtion.DataField.ROLE_CONDITION_ID.ToString());
                command.AddFields(EnumSpecifiedFieldType.Select, SysSystemRoleCondtion.DataField.ROLE_CONDITION_SYNTAX.ToString());
                command.AddFields(EnumSpecifiedFieldType.Select, SysSystemRoleCondtion.DataField.ROLE_CONDITION_RULES.ToString());

                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysSystemRoleCondtionPara.ParaField.SYS_ID.ToString());
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, SysSystemRoleCondtionPara.ParaField.ROLE_CONDITION_ID.ToString());

                dbParameters.Add(new DBParameter { Name = SysSystemRoleCondtionPara.ParaField.SYS_ID.ToString(), Value = row.SysID });
                dbParameters.Add(new DBParameter { Name = SysSystemRoleCondtionPara.ParaField.ROLE_CONDITION_ID.ToString(), Value = row.RoleConditionID });

                var result = Select<SysSystemRoleCondtion>(command, dbParameters).SingleOrDefault();
                if (result != null)
                {
                    resultList.Add(result);
                }

                command.Clear();
                dbParameters.Clear();
            }

            return resultList;
        }
        #endregion

        #region - 取得使用者系統角色異動紀錄檔 -
        public class UserSystemRoleApplyPara
        {
            public enum ParaField
            {
                USER_ID,
                UPD_DT,
                BaseLine_DT
            }

            public DBVarChar UserID;
            public DBDateTime JoinDate;
            public DBDateTime BaseLineDT;
        }

        public class UserSystemRoleApply : MongoDocument
        {
            public enum DataField
            {
                LOG_NO,
                USER_ID,
                SYS_ID,
                SYS_NM,
                ROLE_ID,
                ROLE_NM,
                WFNO,
                MODIFY_LIST
            }

            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
            public DBVarChar UserID;
            public DBVarChar LogNo;
            public DBVarChar WFNo;
            public List<UserSystemRoleModify> ModifyList;
            public DBDateTime BaseLineDT;
        }

        public List<UserSystemRoleApply> SelectUserSystemRoleApplyList(UserSystemRoleApplyPara para)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.LOG_USER_SYSTEM_ROLE_APPLY.ToString());

            command.AddFields(EnumSpecifiedFieldType.Select, UserSystemRoleApply.DataField.LOG_NO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, UserSystemRoleApply.DataField.USER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, UserSystemRoleApply.DataField.SYS_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, UserSystemRoleApply.DataField.SYS_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, UserSystemRoleApply.DataField.ROLE_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, UserSystemRoleApply.DataField.ROLE_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, UserSystemRoleApply.DataField.WFNO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, UserSystemRoleApply.DataField.MODIFY_LIST.ToString());

            command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, UserSystemRoleApplyPara.ParaField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            
            if (!para.BaseLineDT.IsNull())
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.GreaterThan, UserSystemRoleApplyPara.ParaField.UPD_DT.ToString(), para.BaseLineDT);
            }
            else
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.GreaterThan, UserSystemRoleApplyPara.ParaField.UPD_DT.ToString(), para.JoinDate);
            }

            command.AddSortBy(EnumSortType.ASC, UserSystemRoleApply.DataField.LOG_NO.ToString());

            return Select<UserSystemRoleApply>(command);
        }
        #endregion

        #region - 取得使用者系統功能異動紀錄檔 -
        public class UserFunApplyPara
        {
            public enum ParaField
            {
                USER_ID,
                UPD_DT,
                BaseLine_DT
            }

            public DBVarChar UserID;
            public DBDateTime JoinDate;
            public DBDateTime BaseLineDT;
        }

        public class UserFunApply : MongoDocument
        {
            public enum DataField
            {
                LOG_NO,
                USER_ID,
                WFNO,
                MODIFY_LIST
            }

            public DBVarChar LogNo;
            public DBVarChar UserID;
            public List<UserFunModify> ModifyList;
            public DBDateTime BaseLineDT;
        }

        public List<UserFunApply> SelectUserFunApplyList(UserFunApplyPara para)
        {
            MongoCommand command = new MongoCommand(EnumMongoDocName.LOG_USER_FUN_APPLY.ToString());

            command.AddFields(EnumSpecifiedFieldType.Select, UserFunApply.DataField.LOG_NO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, UserFunApply.DataField.USER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, UserFunApply.DataField.MODIFY_LIST.ToString());

            command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, UserFunApplyPara.ParaField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            
            if (!para.BaseLineDT.IsNull())
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.GreaterThan, UserFunApplyPara.ParaField.UPD_DT.ToString(), para.BaseLineDT);
            }
            else
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.GreaterThan, UserFunApplyPara.ParaField.UPD_DT.ToString(), para.JoinDate);    
            }

            command.AddSortBy(EnumSortType.ASC, UserFunApply.DataField.LOG_NO.ToString());

            return Select<UserFunApply>(command);
        }
        #endregion

        #region 取得最後一筆勾選不包含原先權限的日期
        public class UserApplyPara
        {
            public enum ParaField
            {
                USER_ID,
                BaseLine_DT
            }
            public DBVarChar UserID;
        }

        public class UserApply : MongoDocument
        {
            public DBDateTime BaseLineDT;
        }

        public List<UserApply> SelectBaseLineDate(EnumMongoDocName MongoDocName, UserApplyPara para)
        {
            MongoCommand command = new MongoCommand(MongoDocName.ToString());

            command.AddFields(EnumSpecifiedFieldType.Select, UserApplyPara.ParaField.BaseLine_DT.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, UserApplyPara.ParaField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            command.AddSortBy(EnumSortType.DESC, UserApplyPara.ParaField.BaseLine_DT.ToString());

            return Select<UserApply>(command);
        } 
        #endregion
    }
}