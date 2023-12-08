using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.Sys
{
    public class EntitySystemRecord : EntitySys
    {
        public EntitySystemRecord(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class SystemRecord : DBTableRow
        {
            public DBChar LogNo;
            public DBDateTime UpdDT;
            public List<string> TitleList;
            public Dictionary<string, string> ContentDict;
        }

        public class LogUserSystemRolePara
        {
            public enum ParaField
            {
                USER_ID,
                LOG_NO,
                SUPD_DT,
                EUPD_DT,
                SYS_ID
            }

            public DBVarChar UserID;
            public DBChar LogNo;
            public DBDateTime SUpdDT;
            public DBDateTime EUpdDT;
            public DBVarChar SysID;
        }

        public class LogUserSystemRole : DBTableRow
        {
            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
            public DBVarChar ErpWFNo;
            public DBChar LogNo;
            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
            public DBNVarChar RoleConditionRules;
        }

        public List<LogUserSystemRole> SelectLogUserSystemRoleList(LogUserSystemRolePara para)
        {
            List<DBParameter> dbParameters = new DBParameters();
            List<string> commandWhere = new List<string>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine, new object[]
            {
                "SELECT USER_ID AS UserID",
                "     , USER_NM AS UserNM",
                "     , ROLE_CONDITION_RULES AS RoleConditionRules",
                "	  , LOG_NO AS LogNO",
                "	  , SYS_ID AS SysID",
                "     , SYS_NM AS SysNM",
                "     , ROLE_ID AS RoleID",
                "     , ROLE_NM AS RoleNM",
                "     , ERP_WFNO AS ErpWFNO",
                "     , API_NO AS ApiNO",
                "     , UPD_USER_ID AS UpdUserID",
                "     , UPD_USER_NM AS UpdUserNM",
                "     , UPD_DT AS UpdDT",
                "     , EXEC_SYS_ID AS ExecSysID",
                "     , EXEC_SYS_NM AS ExecSysNM",
                "     , EXEC_IP_ADDRESS AS ExecIPAddress",
                "  FROM LOG_USER_SYSTEM_ROLE"
            }));

            if (para.LogNo.IsNull() == false)
            {
                commandWhere.Add("LOG_NO = {LOG_NO}");
                dbParameters.Add(new DBParameter { Name = LogUserSystemRolePara.ParaField.LOG_NO, Value = para.LogNo });
            }
            else
            {
                if (para.SUpdDT.IsNull() == false &&
                    para.EUpdDT.IsNull() == false)
                {
                    commandWhere.Add("UPD_DT BETWEEN {SUPD_DT} AND {EUPD_DT}");
                    dbParameters.Add(new DBParameter { Name = LogUserSystemRolePara.ParaField.SUPD_DT, Value = para.SUpdDT });
                    dbParameters.Add(new DBParameter { Name = LogUserSystemRolePara.ParaField.EUPD_DT, Value = para.EUpdDT });
                }
            }

            if (para.UserID.IsNull() == false)
            {
                commandWhere.Add("USER_ID = {USER_ID}");
                dbParameters.Add(new DBParameter { Name = LogUserSystemRolePara.ParaField.USER_ID, Value = para.UserID });
            }

            if (para.SysID.IsNull() == false)
            {
                commandWhere.Add("SYS_ID = {SYS_ID}");
                dbParameters.Add(new DBParameter { Name = LogUserSystemRolePara.ParaField.SYS_ID, Value = para.SysID });
            }

            if (commandWhere.Any())
            {
                commandText.AppendLine($" WHERE {string.Join(" AND ", commandWhere)}");
            }

            commandText.AppendLine(" ORDER BY LOG_NO DESC, UPD_DT DESC");

            return GetEntityList<LogUserSystemRole>(commandText.ToString(), dbParameters);
        }
    }

    public class MongoSystemRecord : MongoSys
    {
        public MongoSystemRecord(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        public class LogPara : MongoElement
        {
            public enum ParaField
            {
                USER_ID,
                SYS_ID,
                UPD_DT
            }

            public DBVarChar UserID;
            public DBVarChar SysID;
            public List<DBDateTime> UpdDT;
        }


        public class LogUserLoginPara : LogPara
        {
            public new enum ParaField
            {

            }
        }

        public class LogUserLogin
        {
            public enum DataField
            {
                USER_ID, USER_NM,
                LOG_NO,
                USER_PWD,
                LOCATION, LOCATION_NM,
                LOCATION_DESC,
                USER_IDNO, USER_BIRTHDAY,
                IP_ADDRESS,
                VALID_RESULT, VALID_RESULT_NM,
                API_NO, UPD_USER_ID, UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID, EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBChar LogNo;
            public DBVarChar UserPWD;
            public DBVarChar Location;
            public DBNVarChar LocationNM;
            public DBNVarChar LocationDesc;
            public DBVarChar UserIDNo;
            public DBVarChar UserBirthday;
            public DBVarChar IPAddress;
            public DBVarChar ValidResult;
            public DBNVarChar ValidResultNM;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public List<LogUserLogin> SelectLogUserLoginList(LogUserLoginPara para)
        {
            MongoEntity.MongoCommand command = new MongoEntity.MongoCommand("LOG_USER_LOGIN");
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.USER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.USER_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.LOG_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.USER_PWD.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.LOCATION.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.LOCATION_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.LOCATION_DESC.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.USER_IDNO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.USER_BIRTHDAY.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.IP_ADDRESS.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.VALID_RESULT.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.VALID_RESULT_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.API_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.UPD_USER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.UPD_USER_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.UPD_DT.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserLogin.DataField.EXEC_IP_ADDRESS.ToString());

            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.In, LogUserLogin.DataField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            command.AddQueryBetween(EnumConditionType.AND, LogUserSystemRole.DataField.UPD_DT.ToString(), para.UpdDT[0], para.UpdDT[1]);

            command.AddSortBy(EnumSortType.DESC, LogUserLogin.DataField.LOG_NO.ToString());

            return base.Select<LogUserLogin>(command);
        }

        #region - 取得使用者帳號記錄檔 -
        public class LogUserAccountPara : LogPara
        {
            public enum ParaField
            {

            }
        }

        public class LogUserAccount : MongoDocument
        {
            public enum DataField
            {
                LOG_NO,
                MODIFY_TYPE,
                MODIFY_TYPE_NM,
                USER_ID,
                USER_NM,
                IS_LEFT,
                USER_COM_ID,
                USER_COM_NM,
                USER_UNIT_ID,
                USER_UNIT_NM,
                USER_TEAM_ID,
                USER_TITLE_ID,
                USER_TITLE_NM,
                USER_WORK_ID,
                USER_WORK_NM,
                USER_ORG_WORKCOM,
                USER_ORG_WORKCOM_NM,
                USER_ORG_AREA,
                USER_ORG_AREA_NM,
                USER_ORG_GROUP,
                USER_ORG_GROUP_NM,
                USER_ORG_PLACE,
                USER_ORG_PLACE_NM,
                USER_ORG_DEPT,
                USER_ORG_DEPT_NM,
                USER_ORG_TEAM,
                USER_ORG_TEAM_NM,
                USER_ORG_JOB_TITLE,
                USER_ORG_JOB_TITLE_NM,
                USER_ORG_BIZ_TITLE,
                USER_ORG_BIZ_TITLE_NM,
                USER_ORG_TITLE,
                USER_ORG_TITLE_NM,
                USER_ORG_LEVEL,
                USER_ORG_LEVEL_NM,
                API_NO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBVarChar LogNo;
            public DBChar ModifyType;
            public DBNVarChar ModifyTypeNM;
            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBChar IsLeft;
            public DBVarChar UserComID;
            public DBVarChar UserComNM;
            public DBVarChar UserUnitID;
            public DBVarChar UserUnitNM;
            public DBVarChar UserTeamID;
            public DBVarChar UserTitleID;
            public DBVarChar UserTitleNM;
            public DBVarChar UserWorkID;
            public DBVarChar UserWorkNM;
            public DBVarChar UserOrgWorkCom;
            public DBVarChar UserOrgWorkComNM;
            public DBVarChar UserOrgArea;
            public DBVarChar UserOrgAreaNM;
            public DBVarChar UserOrgGroup;
            public DBVarChar UserOrgGroupNM;
            public DBVarChar UserOrgPlace;
            public DBVarChar UserOrgPlaceNM;
            public DBVarChar UserOrgDept;
            public DBVarChar UserOrgDeptNM;
            public DBVarChar UserOrgTeam;
            public DBVarChar UserOrgTeamNM;
            public DBVarChar UserOrgJobTitle;
            public DBVarChar UserOrgJobTitleNM;
            public DBVarChar UserOrgBizTitle;
            public DBVarChar UserOrgBizTitleNM;
            public DBVarChar UserOrgTitle;
            public DBVarChar UserOrgTitleNM;
            public DBVarChar UserOrgLevel;
            public DBVarChar UserOrgLevelNM;
            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBNVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public List<LogUserAccount> SelectLogUserAccountList(LogUserAccountPara para)
        {
            MongoCommand command = new MongoCommand(Mongo_BaseAP.EnumLogDocName.LOG_USER_ACCOUNT.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.LOG_NO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.MODIFY_TYPE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.MODIFY_TYPE_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.IS_LEFT.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_COM_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_COM_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_UNIT_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_UNIT_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_TEAM_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_TITLE_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_TITLE_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_WORK_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_WORK_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_WORKCOM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_WORKCOM_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_AREA.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_AREA_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_GROUP.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_GROUP_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_PLACE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_PLACE_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_DEPT.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_DEPT_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_TEAM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_TEAM_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_JOB_TITLE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_JOB_TITLE_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_BIZ_TITLE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_BIZ_TITLE_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_TITLE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_TITLE_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_LEVEL.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.USER_ORG_LEVEL_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.API_NO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.UPD_USER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.UPD_USER_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.UPD_DT.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserAccount.DataField.EXEC_IP_ADDRESS.ToString());

            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogPara.ParaField.SYS_ID.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Between, LogPara.ParaField.UPD_DT.ToString());

            command.AddSortBy(EnumSortType.DESC, LogSysRoleCondition.DataField.LOG_NO.ToString());

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.UPD_DT.ToString(), Value = para.UpdDT });

            return Select<LogUserAccount>(command, dbParameters);
        }
        #endregion

        public class LogUserAccessPara : LogPara
        {
            public new enum ParaField
            {

            }
        }

        public class LogUserAccess
        {
            public enum DataField
            {
                USER_ID, USER_NM,
                LOG_NO,
                RESTRICT_TYPE, RESTRICT_TYPE_NM,
                IS_LOCK, IS_DISABLE,
                API_NO, UPD_USER_ID, UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID, EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBChar LogNo;
            public DBChar RestrictType;
            public DBNVarChar RestrictTypeNM;
            public DBChar IsLock;
            public DBChar IsDisable;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public List<LogUserAccess> SelectLogUserAccessList(LogUserAccessPara para)
        {
            MongoEntity.MongoCommand command = new MongoEntity.MongoCommand("LOG_USER_ACCESS");
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserAccess.DataField.USER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserAccess.DataField.USER_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserAccess.DataField.LOG_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserAccess.DataField.RESTRICT_TYPE.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserAccess.DataField.RESTRICT_TYPE_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserAccess.DataField.IS_LOCK.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserAccess.DataField.IS_DISABLE.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserAccess.DataField.API_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserAccess.DataField.UPD_USER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserAccess.DataField.UPD_USER_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserAccess.DataField.UPD_DT.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserAccess.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserAccess.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserAccess.DataField.EXEC_IP_ADDRESS.ToString());

            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.In, LogUserAccess.DataField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            command.AddQueryBetween(EnumConditionType.AND, LogUserSystemRole.DataField.UPD_DT.ToString(), para.UpdDT[0], para.UpdDT[1]);

            command.AddSortBy(EnumSortType.DESC, LogUserAccess.DataField.LOG_NO.ToString());

            return base.Select<LogUserAccess>(command);
        }


        public class LogUserPasswordPara : LogPara
        {
            public new enum ParaField
            {

            }
        }

        public class LogUserPassword
        {
            public enum DataField
            {
                USER_ID, USER_NM,
                LOG_NO,
                USER_PWD, MODIFY_DATE,
                IS_RESET,
                API_NO, UPD_USER_ID, UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID, EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBChar LogNo;
            public DBVarChar UserPWD;
            public DBChar ModifyDate;
            public DBChar IsReset;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public List<LogUserPassword> SelectLogUserPasswordList(LogUserPasswordPara para)
        {
            MongoEntity.MongoCommand command = new MongoEntity.MongoCommand("LOG_USER_PWD");
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserPassword.DataField.USER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserPassword.DataField.USER_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserPassword.DataField.LOG_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserPassword.DataField.USER_PWD.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserPassword.DataField.MODIFY_DATE.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserPassword.DataField.IS_RESET.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserPassword.DataField.API_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserPassword.DataField.UPD_USER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserPassword.DataField.UPD_USER_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserPassword.DataField.UPD_DT.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserPassword.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserPassword.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogUserPassword.DataField.EXEC_IP_ADDRESS.ToString());

            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.In, LogUserPassword.DataField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            command.AddQueryBetween(EnumConditionType.AND, LogUserSystemRole.DataField.UPD_DT.ToString(), para.UpdDT[0], para.UpdDT[1]);

            command.AddSortBy(EnumSortType.DESC, LogUserPassword.DataField.LOG_NO.ToString());

            return base.Select<LogUserPassword>(command);
        }


        public class LogUserSystemRolePara : LogPara
        {
            public new enum ParaField
            {

            }
        }

        public class LogUserSystemRole
        {
            public enum DataField
            {
                USER_ID, USER_NM,
                LOG_NO,
                ROLE_GROUP_ID, ROLE_GROUP_NM,
                SYS_ID, SYS_NM,
                ROLE_ID, ROLE_NM, ERP_WFNO,
                API_NO, UPD_USER_ID, UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID, EXEC_SYS_NM,
                EXEC_IP_ADDRESS, ROLE_CONDITION_RULES
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBChar LogNo;
            public DBVarChar RoleGroupID;
            public DBNVarChar RoleGroupNM;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;

            public DBVarChar ErpWFNo;
            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
            public Mongo_BaseAP.RecordLogSystemRoleConditionGroupRule RoleConditionRules;
        }

        public class LogUserFunPara : LogPara
        {
            public new enum ParaField
            {

            }
        }

        public class LogUserFun
        {
            public enum DataField
            {
                USER_ID, USER_NM,
                LOG_NO,
                SYS_ID, SYS_NM,
                FUN_CONTROLLER_ID, FUN_CONTROLLER_NM,
                FUN_ACTION_NAME, FUN_NM, ERP_WFNO,
                API_NO, UPD_USER_ID, UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID, EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBChar LogNo;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar FunControllerID;
            public DBNVarChar FunControllerNM;
            public DBVarChar FunActionName;
            public DBNVarChar FunNM;

            public DBVarChar ErpWFNo;
            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public List<LogUserFun> SelectLogUserFunList(LogUserFunPara para)
        {
            MongoCommand command = new MongoCommand("LOG_USER_FUN");
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.USER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.USER_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.LOG_NO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.SYS_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.SYS_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.FUN_CONTROLLER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.FUN_CONTROLLER_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.FUN_ACTION_NAME.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.FUN_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.ERP_WFNO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.API_NO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.UPD_USER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.UPD_USER_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.UPD_DT.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserFun.DataField.EXEC_IP_ADDRESS.ToString());

            command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, LogUserFun.DataField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            command.AddQueryBetween(EnumConditionType.AND, LogUserSystemRole.DataField.UPD_DT.ToString(), para.UpdDT[0], para.UpdDT[1]);

            command.AddSortBy(EnumSortType.DESC, LogUserFun.DataField.LOG_NO.ToString());

            return Select<LogUserFun>(command);
        }

        #region - 取得使用者資料權限記錄檔 -
        public class LogUserPurviewPara : LogPara
        {
            public enum ParaField
            {
                LOG_NO,
                PURVIEW_ID
            }
            public DBChar LogNo;
            public DBVarChar PurviewID;
        }

        public class LogUserPurview : MongoDocument
        {
            public enum DataField
            {
                LOG_NO,
                USER_ID,
                USER_NM,
                SYS_ID,
                SYS_NM,
                PURVIEW_ID,
                PURVIEW_NM,
                MODIFY_TYPE,
                MODIFY_TYPE_NM,
                CODE_TYPE,
                API_NO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                EXEC_IP_ADDRESS,
                PURVIEW_COLLECT_LIST
            }

            public DBVarChar LogNo;
            public DBVarChar UserID;
            public DBNVarChar UserNM;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar PurviewID;
            public DBNVarChar PurviewNM;
            public DBVarChar ModifyType;
            public DBNVarChar ModifyTypeNM;
            public DBVarChar CodeType;
            public DBVarChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBNVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
            public List<Mongo_BaseAP.LOG_SYS_USER_PURVIEW_COLLECT> PurviewCollectList;
        }

        public List<LogUserPurview> SelectLogUserPurviewList(LogUserPurviewPara para)
        {
            MongoCommand command = new MongoCommand(Mongo_BaseAP.EnumLogDocName.LOG_USER_PURVIEW.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.LOG_NO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.USER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.USER_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.SYS_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.SYS_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.MODIFY_TYPE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.MODIFY_TYPE_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.PURVIEW_COLLECT_LIST.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.PURVIEW_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.PURVIEW_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.CODE_TYPE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.API_NO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.UPD_USER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.UPD_USER_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.UPD_DT.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.EXEC_IP_ADDRESS.ToString());

            command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, LogPara.ParaField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            command.AddQueryBetween(EnumConditionType.AND, LogUserSystemRole.DataField.UPD_DT.ToString(), para.UpdDT[0], para.UpdDT[1]);

            command.AddSortBy(EnumSortType.DESC, LogSysRoleCondition.DataField.LOG_NO.ToString());

            return Select<LogUserPurview>(command);
        }
        #endregion

        public LogUserPurview SelectLogPurviewCollectList(LogUserPurviewPara para)
        {
            MongoCommand command = new MongoCommand(Mongo_BaseAP.EnumLogDocName.LOG_USER_PURVIEW.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserPurview.DataField.PURVIEW_COLLECT_LIST.ToString());

            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogUserPurviewPara.ParaField.LOG_NO.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogPara.ParaField.SYS_ID.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogUserPurviewPara.ParaField.PURVIEW_ID.ToString());

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = LogUserPurviewPara.ParaField.LOG_NO.ToString(), Value = para.LogNo });
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LogUserPurviewPara.ParaField.PURVIEW_ID.ToString(), Value = para.PurviewID });

            return Select<LogUserPurview>(command, dbParameters).SingleOrDefault();
        }

        public class LogSysRolePara : LogPara
        {
            public new enum ParaField
            {

            }
        }

        public class LogSysRole
        {
            public enum DataField
            {
                LOG_NO,
                MODIFY_TYPE, MODIFY_TYPE_NM,
                SYS_ID, SYS_NM,
                ROLE_CATEGORY_ID, ROLE_CATEGORY_NM,
                ROLE_ID,
                ROLE_NM_ZH_TW, ROLE_NM_ZH_CN, ROLE_NM_EN_US, ROLE_NM_TH_TH, ROLE_NM_JA_JP,
                IS_MASTER,
                API_NO, UPD_USER_ID, UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID, EXEC_SYS_NM,
                EXEC_IP_ADDRESS,
            }

            public DBChar LogNo;
            public DBChar ModifyType;
            public DBNVarChar ModifyTypeNM;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleCategoryID;
            public DBNVarChar RoleCategoryNM;
            public DBVarChar RoleID;
            public DBNVarChar RoleNMZHTW;
            public DBNVarChar RoleNMZHCN;
            public DBNVarChar RoleNMENUS;
            public DBNVarChar RoleNMTHTH;
            public DBNVarChar RoleNMJAJP;
            public DBChar IsMaster;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public List<LogSysRole> SelectLogSysRoleList(LogSysRolePara para)
        {
            MongoEntity.MongoCommand command = new MongoEntity.MongoCommand("LOG_SYS_SYSTEM_ROLE");
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.LOG_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.MODIFY_TYPE.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.MODIFY_TYPE_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.SYS_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.SYS_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.ROLE_CATEGORY_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.ROLE_CATEGORY_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.ROLE_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.ROLE_NM_ZH_TW.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.ROLE_NM_ZH_CN.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.ROLE_NM_EN_US.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.ROLE_NM_TH_TH.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.ROLE_NM_JA_JP.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.IS_MASTER.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.API_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.UPD_USER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.UPD_USER_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.UPD_DT.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRole.DataField.EXEC_IP_ADDRESS.ToString());

            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.Equal, LogSysRole.DataField.SYS_ID.ToString());
            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.Between, LogSysRole.DataField.UPD_DT.ToString());

            command.AddSortBy(EnumSortType.DESC, LogSysRole.DataField.LOG_NO.ToString());
            command.AddSortBy(EnumSortType.DESC, LogSysRole.DataField.UPD_DT.ToString());

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.UPD_DT.ToString(), Value = para.UpdDT });

            return base.Select<LogSysRole>(command, dbParameters);
        }

        #region - 取得使用者異動紀錄檔 -
        public class LogUserApplyPara : LogPara
        {
            public enum ParaField
            {
                USER_ID,
                WFNO
            }

            public DBVarChar WFNo;
            public DBVarChar LogDocName;
        }

        public class LogUserApply : MongoDocument
        {
            public enum DataField
            {
                LOG_NO,
                MEMO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT
            }

            public DBNVarChar Memo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
        }

        public List<LogUserApply> SelectLogUserApplyList(LogUserApplyPara para)
        {
            MongoCommand command = new MongoCommand(para.LogDocName.GetValue());

            command.AddFields(EnumSpecifiedFieldType.Select, LogUserApply.DataField.MEMO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserApply.DataField.UPD_USER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserApply.DataField.UPD_USER_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogUserApply.DataField.UPD_DT.ToString());

            command.AddQuery(EnumConditionType.AND, EnumOperatorType.In, LogUserApplyPara.ParaField.USER_ID.ToString(), Utility.GetUserIDList(para.UserID));
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogUserApplyPara.ParaField.WFNO.ToString(), para.WFNo);

            command.AddSortBy(EnumSortType.DESC, LogUserApply.DataField.LOG_NO.ToString());

            return Select<LogUserApply>(command);
        }
        #endregion

        #region - 取得系統角色預設條件紀錄檔 -
        public class LogSysRoleConditionPara : LogPara
        {
            public enum ParaField
            {
                ROLE_CONDITION_ID
            }

            public DBVarChar RoleConditionID;
        }

        public class LogSysRoleCondition : MongoDocument
        {
            public enum DataField
            {
                LOG_NO,
                MODIFY_TYPE,
                MODIFY_TYPE_NM,
                ROLE_CONDITION_ID,
                SYS_ID,
                SYS_NM,
                ROLES,
                API_NO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                EXEC_IP_ADDRESS,
                ROLE_CONDITION_RULES
            }

            public DBVarChar LogNo;
            public DBChar ModifyType;
            public DBNVarChar ModifyTypeNM;
            public DBVarChar RoleConditionID;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public List<DBVarChar> Roles;
            public DBVarChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBNVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
            public Mongo_BaseAP.RecordLogSystemRoleConditionGroupRule RoleConditionRules;
        }

        public List<LogSysRoleCondition> SelectLogSysRoleConditionList(LogSysRoleConditionPara para)
        {
            MongoCommand command = new MongoCommand(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_ROLE_CONDTION.ToString());
            List<DBParameter> dbParameters = new List<DBParameter>();
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysRoleCondition.DataField.LOG_NO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysRoleCondition.DataField.MODIFY_TYPE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysRoleCondition.DataField.MODIFY_TYPE_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysRoleCondition.DataField.ROLE_CONDITION_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysRoleCondition.DataField.SYS_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysRoleCondition.DataField.SYS_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysRoleCondition.DataField.ROLES.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysRoleCondition.DataField.API_NO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysRoleCondition.DataField.UPD_USER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysRoleCondition.DataField.UPD_USER_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysRoleCondition.DataField.UPD_DT.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysRoleCondition.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysRoleCondition.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysRoleCondition.DataField.EXEC_IP_ADDRESS.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysRoleCondition.DataField.ROLE_CONDITION_RULES.ToString());

            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogPara.ParaField.SYS_ID.ToString());
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.SYS_ID.ToString(), Value = para.SysID });

            if (para.RoleConditionID.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogSysRoleConditionPara.ParaField.ROLE_CONDITION_ID.ToString());
                dbParameters.Add(new DBParameter { Name = LogSysRoleConditionPara.ParaField.ROLE_CONDITION_ID.ToString(), Value = para.RoleConditionID });
            }

            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Between, LogPara.ParaField.UPD_DT.ToString());
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.UPD_DT.ToString(), Value = para.UpdDT });

            command.AddSortBy(EnumSortType.DESC, LogSysRoleCondition.DataField.LOG_NO.ToString());

            return Select<LogSysRoleCondition>(command, dbParameters);
        }
        #endregion

        #region - 取得LineBot帳號設定記錄檔 -
        public class LogSysLinePara : LogPara
        {
            public enum ParaField
            {
                LINE_ID
            }

            public DBVarChar LineID;
        }

        public class LogSysLine : MongoDocument
        {
            public enum DataField
            {
                LOG_NO,
                SYS_NM,
                LINE_ID,
                LINE_NM_ZH_TW,
                LINE_NM_ZH_CN,
                LINE_NM_EN_US,
                LINE_NM_TH_TH,
                LINE_NM_JA_JP,
                IS_DISABLE,
                SORT_ORDER,
                API_NO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBVarChar LogNo;
            public DBNVarChar SysNM;
            public DBVarChar LineID;
            public DBNVarChar LineNMZHTW;
            public DBNVarChar LineNMZHCN;
            public DBNVarChar LineNMENUS;
            public DBNVarChar LineNMTHTH;
            public DBNVarChar LineNMJAJP;
            public DBVarChar IsDisable;
            public DBVarChar SortOrder;
            public DBVarChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBNVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public List<LogSysLine> SelectLogSysLineList(LogSysLinePara para)
        {
            MongoCommand command = new MongoCommand(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_LINE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.LOG_NO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.SYS_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.LINE_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.LINE_NM_ZH_TW.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.LINE_NM_ZH_CN.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.LINE_NM_EN_US.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.LINE_NM_TH_TH.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.LINE_NM_JA_JP.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.IS_DISABLE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.SORT_ORDER.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.API_NO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.UPD_USER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.UPD_USER_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.UPD_DT.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLine.DataField.EXEC_IP_ADDRESS.ToString());

            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogPara.ParaField.SYS_ID.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Between, LogPara.ParaField.UPD_DT.ToString());

            List<DBParameter> dbParameters = new List<DBParameter>();

            if (para.LineID.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogSysLinePara.ParaField.LINE_ID.ToString());
                dbParameters.Add(new DBParameter { Name = LogSysLinePara.ParaField.LINE_ID.ToString(), Value = para.LineID });
            }

            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.UPD_DT.ToString(), Value = para.UpdDT });

            return Select<LogSysLine>(command, dbParameters);
        }
        #endregion

        #region - 取得LineBot好友清單設定記錄檔 -
        public class LogSysLineReceiverPara : LogPara
        {
            public enum ParaField
            {
                LINE_ID
            }

            public DBVarChar LineID;
        }

        public class LogSysLineReceiver : MongoDocument
        {
            public enum DataField
            {
                LOG_NO,
                SYS_ID,
                SYS_NM,
                LINE_ID,
                LINE_RECEIVER_ID,
                LINE_RECEIVER_NM_ZH_TW,
                LINE_RECEIVER_NM_ZH_CN,
                LINE_RECEIVER_NM_EN_US,
                LINE_RECEIVER_NM_TH_TH,
                LINE_RECEIVER_NM_JA_JP,
                IS_DISABLE,
                SOURCE_TYPE,
                API_NO,
                UPD_USER_ID,
                UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID,
                EXEC_SYS_NM,
                EXEC_IP_ADDRESS
            }

            public DBVarChar LogNo;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar LineID;
            public DBVarChar LineReceiverID;
            public DBNVarChar LineReceiverNMZHTW;
            public DBNVarChar LineReceiverNMZHCN;
            public DBNVarChar LineReceiverNMENUS;
            public DBNVarChar LineReceiverNMTHTH;
            public DBNVarChar LineReceiverNMJAJP;
            public DBVarChar IsDisable;
            public DBVarChar SourceType;
            public DBVarChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBNVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public List<LogSysLineReceiver> SelectLogSysLineReceiverList(LogSysLineReceiverPara para)
        {
            MongoCommand command = new MongoCommand(Mongo_BaseAP.EnumLogDocName.LOG_SYS_SYSTEM_LINE_RECEIVER.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.LOG_NO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.SYS_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.SYS_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.LINE_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.LINE_RECEIVER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.LINE_RECEIVER_NM_ZH_TW.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.LINE_RECEIVER_NM_ZH_CN.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.LINE_RECEIVER_NM_EN_US.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.LINE_RECEIVER_NM_TH_TH.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.LINE_RECEIVER_NM_JA_JP.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.IS_DISABLE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.SOURCE_TYPE.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.API_NO.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.UPD_USER_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.UPD_USER_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.UPD_DT.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(EnumSpecifiedFieldType.Select, LogSysLineReceiver.DataField.EXEC_IP_ADDRESS.ToString());

            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogPara.ParaField.SYS_ID.ToString());
            command.AddQuery(EnumConditionType.AND, EnumOperatorType.Between, LogPara.ParaField.UPD_DT.ToString());

            List<DBParameter> dbParameters = new List<DBParameter>();

            if (para.LineID.IsNull() == false)
            {
                command.AddQuery(EnumConditionType.AND, EnumOperatorType.Equal, LogSysLineReceiverPara.ParaField.LINE_ID.ToString());
                dbParameters.Add(new DBParameter { Name = LogSysLineReceiverPara.ParaField.LINE_ID.ToString(), Value = para.LineID });
            }

            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.UPD_DT.ToString(), Value = para.UpdDT });

            return Select<LogSysLineReceiver>(command, dbParameters);
        }
        #endregion

        public class LogSysFunGroupPara : LogPara
        {
            public new enum ParaField
            {

            }
        }

        public class LogSysFunGroup
        {
            public enum DataField
            {
                LOG_NO,
                MODIFY_TYPE, MODIFY_TYPE_NM,
                SYS_ID, SYS_NM,
                FUN_CONTROLLER_ID,
                FUN_GROUP_ZH_TW, FUN_GROUP_ZH_CN, FUN_GROUP_EN_US, FUN_GROUP_TH_TH, FUN_GROUP_JA_JP,
                SORT_ORDER,
                API_NO, UPD_USER_ID, UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID, EXEC_SYS_NM,
                EXEC_IP_ADDRESS,
            }

            public DBChar LogNo;
            public DBChar ModifyType;
            public DBNVarChar ModifyTypeNM;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar FunControllerID;
            public DBNVarChar FunGroupZHTW;
            public DBNVarChar FunGroupZHCN;
            public DBNVarChar FunGroupENUS;
            public DBNVarChar FunGroupTHTH;
            public DBNVarChar FunGroupJAJP;
            public DBVarChar SortOrder;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public List<LogSysFunGroup> SelectLogSysFunGroupList(LogSysFunGroupPara para)
        {
            MongoEntity.MongoCommand command = new MongoEntity.MongoCommand("LOG_SYS_SYSTEM_FUN_GROUP");
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.LOG_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.MODIFY_TYPE.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.MODIFY_TYPE_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.SYS_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.SYS_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.FUN_CONTROLLER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.FUN_GROUP_ZH_TW.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.FUN_GROUP_ZH_CN.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.FUN_GROUP_EN_US.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.FUN_GROUP_TH_TH.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.FUN_GROUP_JA_JP.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.SORT_ORDER.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.API_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.UPD_USER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.UPD_USER_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.UPD_DT.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFunGroup.DataField.EXEC_IP_ADDRESS.ToString());

            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.Equal, LogSysFunGroup.DataField.SYS_ID.ToString());
            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.Between, LogSysFunGroup.DataField.UPD_DT.ToString());

            command.AddSortBy(EnumSortType.DESC, LogSysFunGroup.DataField.LOG_NO.ToString());
            command.AddSortBy(EnumSortType.DESC, LogSysFunGroup.DataField.UPD_DT.ToString());

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.UPD_DT.ToString(), Value = para.UpdDT });

            return base.Select<LogSysFunGroup>(command, dbParameters);
        }


        public class LogSysFunPara : LogPara
        {
            public new enum ParaField
            {

            }
        }

        public class LogSysFun
        {
            public enum DataField
            {
                LOG_NO,
                MODIFY_TYPE, MODIFY_TYPE_NM,
                SYS_ID, SYS_NM,
                SUB_SYS_ID, SUB_SYS_NM,
                PURVIEW_ID, PURVIEW_NM,
                FUN_CONTROLLER_ID, FUN_GROUP_NM,
                FUN_ACTION_NAME,
                FUN_NM_ZH_TW, FUN_NM_ZH_CN, FUN_NM_EN_US, FUN_NM_TH_TH, FUN_NM_JA_JP,
                FUN_TYPE, FUN_TYPE_NM,
                IS_OUTSIDE, IS_DISABLE, SORT_ORDER,
                API_NO, UPD_USER_ID, UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID, EXEC_SYS_NM,
                EXEC_IP_ADDRESS,
            }

            public DBChar LogNo;
            public DBChar ModifyType;
            public DBNVarChar ModifyTypeNM;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar SubSysID;
            public DBNVarChar SubSysNM;
            public DBVarChar PurviewID;
            public DBNVarChar PurviewNM;
            public DBVarChar FunControllerID;
            public DBNVarChar FunGroupNM;
            public DBVarChar FunActionName;
            public DBNVarChar FunNMZHTW;
            public DBNVarChar FunNMZHCN;
            public DBNVarChar FunNMENUS;
            public DBNVarChar FunNMTHTH;
            public DBNVarChar FunNMJAJP;
            public DBVarChar FunType;
            public DBNVarChar FunTypeNM;
            public DBChar IsOutside;
            public DBChar IsDisable;
            public DBVarChar SortOrder;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public List<LogSysFun> SelectLogSysFunList(LogSysFunPara para)
        {
            MongoEntity.MongoCommand command = new MongoEntity.MongoCommand("LOG_SYS_SYSTEM_FUN");
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.LOG_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.MODIFY_TYPE.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.MODIFY_TYPE_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.SYS_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.SYS_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.SUB_SYS_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.SUB_SYS_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.PURVIEW_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.PURVIEW_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.FUN_CONTROLLER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.FUN_GROUP_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.FUN_ACTION_NAME.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.FUN_NM_ZH_TW.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.FUN_NM_ZH_CN.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.FUN_NM_EN_US.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.FUN_NM_TH_TH.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.FUN_NM_JA_JP.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.FUN_TYPE.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.FUN_TYPE_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.IS_OUTSIDE.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.IS_DISABLE.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.SORT_ORDER.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.API_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.UPD_USER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.UPD_USER_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.UPD_DT.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysFun.DataField.EXEC_IP_ADDRESS.ToString());

            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.Equal, LogSysFun.DataField.SYS_ID.ToString());
            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.Between, LogSysFun.DataField.UPD_DT.ToString());

            command.AddSortBy(EnumSortType.DESC, LogSysFun.DataField.LOG_NO.ToString());
            command.AddSortBy(EnumSortType.DESC, LogSysFun.DataField.UPD_DT.ToString());

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.UPD_DT.ToString(), Value = para.UpdDT });

            return base.Select<LogSysFun>(command, dbParameters);
        }


        public class LogSysRoleFunPara : LogPara
        {
            public new enum ParaField
            {
                FUN_CONTROLLER_ID, FUN_ACTION_NAME
            }

            public DBVarChar FunControllerID;
            public DBVarChar FunActionName;
        }

        public class LogSysRoleFun
        {
            public enum DataField
            {
                LOG_NO,
                SYS_ID, SYS_NM,
                ROLE_ID, ROLE_NM,
                FUN_CONTROLLER_ID, FUN_GROUP_NM,
                FUN_ACTION_NAME, FUN_NM,
                API_NO, UPD_USER_ID, UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID, EXEC_SYS_NM,
                EXEC_IP_ADDRESS,
            }

            public DBChar LogNo;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;
            public DBVarChar FunControllerID;
            public DBNVarChar FunGroupNM;
            public DBVarChar FunActionName;
            public DBNVarChar FunNM;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public List<LogSysRoleFun> SelectLogSysRoleFunList(LogSysRoleFunPara para)
        {
            MongoEntity.MongoCommand command = new MongoEntity.MongoCommand("LOG_SYS_SYSTEM_ROLE_FUN");
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleFun.DataField.LOG_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleFun.DataField.SYS_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleFun.DataField.SYS_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleFun.DataField.ROLE_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleFun.DataField.ROLE_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleFun.DataField.FUN_CONTROLLER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleFun.DataField.FUN_GROUP_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleFun.DataField.FUN_ACTION_NAME.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleFun.DataField.FUN_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleFun.DataField.API_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleFun.DataField.UPD_USER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleFun.DataField.UPD_USER_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleFun.DataField.UPD_DT.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleFun.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleFun.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleFun.DataField.EXEC_IP_ADDRESS.ToString());

            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.Equal, LogSysRoleFun.DataField.SYS_ID.ToString());
            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.Equal, LogSysRoleFun.DataField.FUN_CONTROLLER_ID.ToString());
            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.Equal, LogSysRoleFun.DataField.FUN_ACTION_NAME.ToString());
            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.Between, LogSysRoleFun.DataField.UPD_DT.ToString());

            command.AddSortBy(EnumSortType.DESC, LogSysRoleFun.DataField.LOG_NO.ToString());
            command.AddSortBy(EnumSortType.DESC, LogSysRoleFun.DataField.UPD_DT.ToString());

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.SYS_ID.ToString(), Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LogSysRoleFunPara.ParaField.FUN_CONTROLLER_ID.ToString(), Value = para.FunControllerID });
            dbParameters.Add(new DBParameter { Name = LogSysRoleFunPara.ParaField.FUN_ACTION_NAME.ToString(), Value = para.FunActionName });
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.UPD_DT.ToString(), Value = para.UpdDT });

            return base.Select<LogSysRoleFun>(command, dbParameters);
        }


        public class LogSysRoleGroupPara : LogPara
        {
            public new enum ParaField
            {

            }
        }

        public class LogSysRoleGroup
        {
            public enum DataField
            {
                LOG_NO,
                MODIFY_TYPE, MODIFY_TYPE_NM,
                ROLE_GROUP_ID,
                ROLE_GROUP_NM_ZH_TW, ROLE_GROUP_NM_ZH_CN, ROLE_GROUP_NM_EN_US, ROLE_GROUP_NM_TH_TH, ROLE_GROUP_NM_JA_JP,
                SORT_ORDER, REMARK,
                API_NO, UPD_USER_ID, UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID, EXEC_SYS_NM,
                EXEC_IP_ADDRESS,
            }

            public DBChar LogNo;
            public DBChar ModifyType;
            public DBNVarChar ModifyTypeNM;
            public DBVarChar RoleGroupID;
            public DBNVarChar RoleGroupNMZHTW;
            public DBNVarChar RoleGroupNMZHCN;
            public DBNVarChar RoleGroupNMENUS;
            public DBNVarChar RoleGroupNMTHTH;
            public DBNVarChar RoleGroupNMJAJP;
            public DBVarChar SortOrder;
            public DBNVarChar Remark;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public List<LogSysRoleGroup> SelectLogSysRoleGroupList(LogSysRoleGroupPara para)
        {
            MongoEntity.MongoCommand command = new MongoEntity.MongoCommand("LOG_SYS_SYSTEM_ROLE_GROUP");
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.LOG_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.MODIFY_TYPE.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.MODIFY_TYPE_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.ROLE_GROUP_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.ROLE_GROUP_NM_ZH_TW.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.ROLE_GROUP_NM_ZH_CN.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.ROLE_GROUP_NM_EN_US.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.ROLE_GROUP_NM_TH_TH.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.ROLE_GROUP_NM_JA_JP.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.SORT_ORDER.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.REMARK.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.API_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.UPD_USER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.UPD_USER_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.UPD_DT.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroup.DataField.EXEC_IP_ADDRESS.ToString());

            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.Between, LogSysRoleGroup.DataField.UPD_DT.ToString());

            command.AddSortBy(EnumSortType.DESC, LogSysRoleGroup.DataField.LOG_NO.ToString());
            command.AddSortBy(EnumSortType.DESC, LogSysRoleGroup.DataField.UPD_DT.ToString());

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.UPD_DT.ToString(), Value = para.UpdDT });

            return base.Select<LogSysRoleGroup>(command, dbParameters);
        }


        public class LogSysRoleGroupCollectPara : LogPara
        {
            public new enum ParaField
            {
                ROLE_GROUP_ID
            }

            public DBVarChar RoleGroupID;
        }

        public class LogSysRoleGroupCollect
        {
            public enum DataField
            {
                LOG_NO,
                MODIFY_TYPE, MODIFY_TYPE_NM,
                ROLE_GROUP_ID, ROLE_GROUP_NM,
                SYS_ID, SYS_NM,
                ROLE_ID, ROLE_NM,
                API_NO, UPD_USER_ID, UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID, EXEC_SYS_NM,
                EXEC_IP_ADDRESS,
            }

            public DBChar LogNo;
            public DBVarChar RoleGroupID;
            public DBNVarChar RoleGroupNM;
            public DBVarChar SysID;
            public DBNVarChar SysNM;
            public DBVarChar RoleID;
            public DBNVarChar RoleNM;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        //不用寫
        public List<LogSysRoleGroupCollect> SelectLogSysRoleGroupCollectList(LogSysRoleGroupCollectPara para)
        {
            MongoEntity.MongoCommand command = new MongoEntity.MongoCommand("LOG_SYS_SYSTEM_ROLE_GROUP_COLLECT");
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroupCollect.DataField.LOG_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroupCollect.DataField.ROLE_GROUP_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroupCollect.DataField.ROLE_GROUP_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroupCollect.DataField.SYS_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroupCollect.DataField.SYS_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroupCollect.DataField.ROLE_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroupCollect.DataField.ROLE_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroupCollect.DataField.API_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroupCollect.DataField.UPD_USER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroupCollect.DataField.UPD_USER_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroupCollect.DataField.UPD_DT.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroupCollect.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroupCollect.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysRoleGroupCollect.DataField.EXEC_IP_ADDRESS.ToString());

            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.Equal, LogSysRoleGroupCollect.DataField.ROLE_GROUP_ID.ToString());
            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.Between, LogSysRoleGroupCollect.DataField.UPD_DT.ToString());

            command.AddSortBy(EnumSortType.DESC, LogSysRoleGroupCollect.DataField.LOG_NO.ToString());
            command.AddSortBy(EnumSortType.DESC, LogSysRoleGroupCollect.DataField.UPD_DT.ToString());

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = LogSysRoleGroupCollectPara.ParaField.ROLE_GROUP_ID.ToString(), Value = para.RoleGroupID });
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.UPD_DT.ToString(), Value = para.UpdDT });

            return base.Select<LogSysRoleGroupCollect>(command, dbParameters);
        }


        public class LogSysTrustIPPara : LogPara
        {
            public new enum ParaField
            {

            }
        }

        public class LogSysTrustIP
        {
            public enum DataField
            {
                LOG_NO,
                MODIFY_TYPE, MODIFY_TYPE_NM,
                IP_BEGIN, IP_END,
                COM_ID, COM_NM,
                TRUST_STATUS,
                TRUST_TYPE, TRUST_TYPE_NM,
                SOURCE_TYPE, SOURCE_TYPE_NM,
                REMARK, SORT_ORDER,
                API_NO, UPD_USER_ID, UPD_USER_NM,
                UPD_DT,
                EXEC_SYS_ID, EXEC_SYS_NM,
                EXEC_IP_ADDRESS,
            }

            public DBChar LogNo;
            public DBChar ModifyType;
            public DBNVarChar ModifyTypeNM;
            public DBVarChar IPBegin;
            public DBVarChar IPEnd;
            public DBVarChar ComID;
            public DBNVarChar ComNM;
            public DBChar TrustStatus;
            public DBChar TrustType;
            public DBNVarChar TrustTypeNM;
            public DBChar SourceType;
            public DBNVarChar SourceTypeNM;
            public DBNVarChar Remark;
            public DBVarChar SortOrder;

            public DBChar APINo;
            public DBVarChar UpdUserID;
            public DBNVarChar UpdUserNM;
            public DBDateTime UpdDT;
            public DBVarChar ExecSysID;
            public DBNVarChar ExecSysNM;
            public DBVarChar ExecIPAddress;
        }

        public List<LogSysTrustIP> SelectLogSysTrustIPList(LogSysTrustIPPara para)
        {
            MongoEntity.MongoCommand command = new MongoEntity.MongoCommand("LOG_SYS_TRUST_IP");
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.LOG_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.MODIFY_TYPE.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.MODIFY_TYPE_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.IP_BEGIN.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.IP_END.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.COM_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.COM_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.TRUST_STATUS.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.TRUST_TYPE.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.TRUST_TYPE_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.SOURCE_TYPE.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.SOURCE_TYPE_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.REMARK.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.SORT_ORDER.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.API_NO.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.UPD_USER_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.UPD_USER_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.UPD_DT.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.EXEC_SYS_ID.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.EXEC_SYS_NM.ToString());
            command.AddFields(MongoEntity.EnumSpecifiedFieldType.Select, LogSysTrustIP.DataField.EXEC_IP_ADDRESS.ToString());

            command.AddQuery(MongoEntity.EnumConditionType.AND, MongoEntity.EnumOperatorType.Between, LogSysTrustIP.DataField.UPD_DT.ToString());

            command.AddSortBy(EnumSortType.DESC, LogSysTrustIP.DataField.LOG_NO.ToString());
            command.AddSortBy(EnumSortType.DESC, LogSysTrustIP.DataField.UPD_DT.ToString());

            List<DBParameter> dbParameters = new List<DBParameter>();
            dbParameters.Add(new DBParameter { Name = LogPara.ParaField.UPD_DT.ToString(), Value = para.UpdDT });

            return base.Select<LogSysTrustIP>(command, dbParameters);
        }
    }
}