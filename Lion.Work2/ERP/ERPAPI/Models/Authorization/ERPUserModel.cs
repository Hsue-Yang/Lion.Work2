using System;
using System.Collections.Generic;
using System.Linq;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Authorization;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;

namespace ERPAPI.Models.Authorization
{
    public class ERPUserModel : AuthorizationModel
    {
        #region - Definitions -
        public class APIParaData
        {
            public string UserID { get; set; }
            public string UserNM { get; set; }
            public string UserComID { get; set; }
            public string UserUnitID { get; set; }
            public string UserPWD { get; set; }
            public string PWDValidDate { get; set; }
            public string IsLeft { get; set; }

            public string UserIDNo { get; set; }
            public string UserBirthday { get; set; }
            public EnumLocation Location { get; set; }
            public string LocationDesc { get; set; }
            public string IPAddress { get; set; }
        }

        public class APIParaUserAccountData
        {
            public bool IsIncludeAuthApplyed { get; set; } = true;
            
            public string UserID { get; set; }
            public string UserNM { get; set; }
            public string IsLeft { get; set; }
            public string JoinDate { get; set; }

            public string UserOrgArea { get; set; }
            public string UserOrgBIZTitle { get; set; }
            public string UserOrgDept { get; set; }
            public string UserOrgGroup { get; set; }
            public string UserOrgJobTitle { get; set; }
            public string UserOrgLevel { get; set; }
            public string UserOrgPlace { get; set; }
            public string UserOrgTeam { get; set; }
            public string UserOrgTitle { get; set; }
            public string UserOrgWorkCom { get; set; }

            public string UserComID { get; set; }
            public string UserSalaryComID { get; set; }
            public string UserTeamID { get; set; }
            public string UserTitleID { get; set; }
            public string UserUnitID { get; set; }
            public string UserWorkID { get; set; }
        }

        public class SysUserRoleEventData
        {
            public List<string> TargetSysIDList { get; set; }
            public string UserID { get; set; }
            public string RoleGroupID { get; set; }
            public List<string> RoleIDList { get; set; }
        }
        #endregion

        #region - Constructor -
        public ERPUserModel()
        {
            _entity = new EntityERPUser(this.ConnectionStringSERP, this.ProviderNameSERP);
            _mongoEntity = new MongoERPUser(this.ConnectionStringMSERP, this.ProviderNameMSERP);
            _systemRoleApplies = new List<EntityERPUser.UserSystemRoleApply>();
        }
        #endregion

        #region - API Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }

        public string ClientIP { get; set; }

        public string APIPara { get; set; }

        public APIParaData APIData { get; set; }
        public APIParaUserAccountData ApiUserAccountData { get; set; }
        public List<SysUserRoleEventData> SysRoleEventParaList { get; set; }
        #endregion

        #region - Private -
        private readonly EntityERPUser _entity;
        private readonly MongoERPUser _mongoEntity;
        private List<EntityERPUser.SysUserRole> _sysUserRoleList;
        private List<EntityERPUser.CMCodeInfo> _codeInfoList;
        private List<EntityERPUser.UserSystemRoleApply> _systemRoleApplies;
        #endregion

        #region - 編輯使用者資料 -
        /// <summary>
        /// 編輯使用者資料
        /// </summary>
        public bool EditRawUserData(string updUserID)
        {
            try
            {
                EntityERPUser.UserAccountPara para = new EntityERPUser.UserAccountPara
                {
                    IsLeft = new DBChar(string.IsNullOrWhiteSpace(ApiUserAccountData.IsLeft) ? null : ApiUserAccountData.IsLeft),
                    UserComID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserComID) ? null : ApiUserAccountData.UserComID),
                    UserSalaryComID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserSalaryComID) ? null : ApiUserAccountData.UserSalaryComID),
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserID) ? null : ApiUserAccountData.UserID),
                    UserNM = new DBNVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserNM) ? null : ApiUserAccountData.UserNM),
                    UserArea = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgArea) ? null : ApiUserAccountData.UserOrgArea),
                    UserDept = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgDept) ? null : ApiUserAccountData.UserOrgDept),
                    UserGroup = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgGroup) ? null : ApiUserAccountData.UserOrgGroup),
                    UserLevel = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgLevel) ? null : ApiUserAccountData.UserOrgLevel),
                    UserPlace = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgPlace) ? null : ApiUserAccountData.UserOrgPlace),
                    UserTeam = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgTeam) ? null : ApiUserAccountData.UserOrgTeam),
                    UserTitle = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgTitle) ? null : ApiUserAccountData.UserOrgTitle),
                    UserJobTitle = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgJobTitle) ? null : ApiUserAccountData.UserOrgJobTitle),
                    UserBizTitle = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgBIZTitle) ? null : ApiUserAccountData.UserOrgBIZTitle),
                    UserUnitID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserUnitID) ? null : ApiUserAccountData.UserUnitID),
                    UserTeamID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserTeamID) ? null : ApiUserAccountData.UserTeamID),
                    UserTitleID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserTitleID) ? null : ApiUserAccountData.UserTitleID),
                    UserWorkID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserWorkID) ? null : ApiUserAccountData.UserWorkID),
                    UpdUserID = new DBVarChar(updUserID),
                    UpdDT = new DBDateTime(DateTime.Now),
                    UpdEdiEventNO = new DBChar(null)
                };

                if (_entity.EditRawCMUser(para) == EntityERPUser.EnumRawCMUserResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 紀錄異動使用者帳號 -
        /// <summary>
        /// 紀錄異動使用者帳號
        /// </summary>
        /// <param name="apiNo"></param>
        /// <param name="updUserID"></param>
        /// <param name="execSysID"></param>
        /// <param name="ipAddress"></param>
        public void RecordModifyUserAccount(string apiNo, string updUserID, string execSysID, string ipAddress)
        {
            try
            {
                Mongo_BaseAP.RecordLogUserAccountPara para = new Mongo_BaseAP.RecordLogUserAccountPara
                {
                    UserID = new DBVarChar(ApiUserAccountData.UserID),
                    UserNM = new DBNVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserNM) ? null : ApiUserAccountData.UserNM),
                    IsLeft = new DBChar(string.IsNullOrWhiteSpace(ApiUserAccountData.IsLeft) ? null : ApiUserAccountData.IsLeft),
                    UserComID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserComID) ? null : ApiUserAccountData.UserComID),
                    UserSalaryComID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserSalaryComID) ? null : ApiUserAccountData.UserSalaryComID),
                    UserUnitID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserUnitID) ? null : ApiUserAccountData.UserUnitID),
                    UserTeamID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserTeamID) ? null : ApiUserAccountData.UserTeamID),
                    UserTitleID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserTitleID) ? null : ApiUserAccountData.UserTitleID),
                    UserWorkID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserWorkID) ? null : ApiUserAccountData.UserWorkID),
                    UserOrgWorkCom = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgWorkCom) ? null : ApiUserAccountData.UserOrgWorkCom),
                    UserOrgArea = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgArea) ? null : ApiUserAccountData.UserOrgArea),
                    UserOrgGroup = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgGroup) ? null : ApiUserAccountData.UserOrgGroup),
                    UserOrgPlace = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgPlace) ? null : ApiUserAccountData.UserOrgPlace),
                    UserOrgDept = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgDept) ? null : ApiUserAccountData.UserOrgDept),
                    UserOrgTeam = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgTeam) ? null : ApiUserAccountData.UserOrgTeam),
                    UserOrgJobTitle = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgJobTitle) ? null : ApiUserAccountData.UserOrgJobTitle),
                    UserOrgBizTitle = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgBIZTitle) ? null : ApiUserAccountData.UserOrgBIZTitle),
                    UsreOrgLevel = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgLevel) ? null : ApiUserAccountData.UserOrgLevel),
                    UserOrgTitle = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgTitle) ? null : ApiUserAccountData.UserOrgTitle)
                };

                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_USER_ACCOUNT, Mongo_BaseAP.EnumModifyType.U, apiNo, updUserID, execSysID, ipAddress, para);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }
        #endregion

        #region - 建立使用者帳號 -
        /// <summary>
        /// 建立使用者帳號
        /// </summary>
        /// <returns></returns>
        public bool CreateUserAccount(string updUserID)
        {
            try
            {
                EntityERPUser.UserAccountPara para = new EntityERPUser.UserAccountPara
                {
                    IsLeft = new DBChar(string.IsNullOrWhiteSpace(ApiUserAccountData.IsLeft) ? null : ApiUserAccountData.IsLeft),
                    UserComID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserComID) ? null : ApiUserAccountData.UserComID),
                    UserSalaryComID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserSalaryComID) ? null : ApiUserAccountData.UserSalaryComID),
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserID) ? null : ApiUserAccountData.UserID),
                    UserNM = new DBNVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserNM) ? null : ApiUserAccountData.UserNM),
                    UserArea = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgArea) ? null : ApiUserAccountData.UserOrgArea),
                    UserBizTitle = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgBIZTitle) ? null : ApiUserAccountData.UserOrgBIZTitle),
                    UserDept = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgDept) ? null : ApiUserAccountData.UserOrgDept),
                    UserGroup = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgGroup) ? null : ApiUserAccountData.UserOrgGroup),
                    UserWorkCom = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgWorkCom) ? null : ApiUserAccountData.UserOrgWorkCom),
                    UserJobTitle = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgJobTitle) ? null : ApiUserAccountData.UserOrgJobTitle),
                    UserLevel = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgLevel) ? null : ApiUserAccountData.UserOrgLevel),
                    UserPlace = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgPlace) ? null : ApiUserAccountData.UserOrgPlace),
                    UserTeam = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgTeam) ? null : ApiUserAccountData.UserOrgTeam),
                    UserTitle = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgTitle) ? null : ApiUserAccountData.UserOrgTitle),
                    UserTeamID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserTeamID) ? null : ApiUserAccountData.UserTeamID),
                    UserTitleID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserTitleID) ? null : ApiUserAccountData.UserTitleID),
                    UserUnitID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserUnitID) ? null : ApiUserAccountData.UserUnitID),
                    UserWorkID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserWorkID) ? null : ApiUserAccountData.UserWorkID),
                    UpdUserID = new DBVarChar(updUserID),
                    UpdDT = new DBDateTime(DateTime.Now),
                    UpdEdiEventNO = new DBChar(null),
                    RoleGroupID = new DBVarChar(null),
                    RestrictType = new DBChar(EnumYN.N.ToString()),
                    UserPWD = new DBVarChar(null),
                    PWDValidDate = new DBChar(null),
                    ErrorTimes = new DBInt(0),
                    IsLock = new DBChar(EnumYN.N.ToString()),
                    LockDT = new DBDateTime(DateTime.Now),
                    IsDisable = new DBChar(EnumYN.N.ToString()),
                    LeftDate = new DBChar(null),
                    IsDailyFirst = new DBChar(EnumYN.N.ToString()),
                    LastLoginDate = new DBChar(null),
                    UserENM = new DBNVarChar(null),
                    UserIDNO = new DBVarChar(null),
                    UserBirthday = new DBVarChar(null),
                    UserTel = new DBVarChar(null),
                    UserExtension = new DBVarChar(null),
                    UserMobile = new DBVarChar(null),
                    UserEmail = new DBVarChar(null),
                    UserGoogleAccount = new DBVarChar(null),
                    IsGaccEnable = new DBChar(EnumYN.N.ToString())
                };

                return _entity.CreateUserAccount(para) == EntityERPUser.EnumCreateUserAccountResult.Success;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 紀錄建立使用者帳號 -
        /// <summary>
        /// 紀錄建立使用者帳號
        /// </summary>
        /// <param name="apiNo"></param>
        /// <param name="updUserID"></param>
        /// <param name="execSysID"></param>
        /// <param name="ipAddress"></param>
        public void RecordCreateUserAccount(string apiNo, string updUserID, string execSysID, string ipAddress)
        {
            if (_GetCMCodeInfoList() == false)
            {
                return;
            }

            try
            {
                Mongo_BaseAP.RecordLogUserAccountPara para = new Mongo_BaseAP.RecordLogUserAccountPara
                {
                    UserID = new DBVarChar(ApiUserAccountData.UserID),
                    UserNM = new DBNVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserNM) ? null : ApiUserAccountData.UserNM),
                    IsLeft = new DBChar(string.IsNullOrWhiteSpace(ApiUserAccountData.IsLeft) ? null : ApiUserAccountData.IsLeft),
                    JoinDate = new DBChar(string.IsNullOrWhiteSpace(ApiUserAccountData.JoinDate) ? null : ApiUserAccountData.JoinDate),
                    UserComID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserComID) ? null : ApiUserAccountData.UserComID),
                    UserComNM = _GetCMCodeName(EntityERPUser.EnumCMCodeType.CMOrgCom, ApiUserAccountData.UserComID),
                    UserSalaryComID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserSalaryComID) ? null : ApiUserAccountData.UserSalaryComID),
                    UserSalaryComNM = _GetCMCodeName(EntityERPUser.EnumCMCodeType.CMOrgCom, ApiUserAccountData.UserSalaryComID),
                    UserUnitID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserUnitID) ? null : ApiUserAccountData.UserUnitID),
                    UserUnitNM = _GetCMCodeName(EntityERPUser.EnumCMCodeType.CMOrgUnit, ApiUserAccountData.UserUnitID),
                    UserTeamID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserTeamID) ? null : ApiUserAccountData.UserTeamID),
                    UserTitleID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserTitleID) ? null : ApiUserAccountData.UserTitleID),
                    UserTitleNM = _GetCMCodeName(Entity_BaseAP.EnumCMCodeKind.Title, ApiUserAccountData.UserTitleID),
                    UserWorkID = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserWorkID) ? null : ApiUserAccountData.UserWorkID),
                    UserWorkNM = _GetCMCodeName(Entity_BaseAP.EnumCMCodeKind.Work, ApiUserAccountData.UserWorkID),
                    UserOrgWorkCom = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgWorkCom) ? null : ApiUserAccountData.UserOrgWorkCom),
                    UserOrgWorkComNM = _GetCMCodeName(EntityERPUser.EnumCMCodeType.CMOrgCom, ApiUserAccountData.UserOrgWorkCom),
                    UserOrgArea = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgArea) ? null : ApiUserAccountData.UserOrgArea),
                    UserOrgAreaNM = _GetCMCodeName(Entity_BaseAP.EnumCMCodeKind.OrgArea, ApiUserAccountData.UserOrgArea),
                    UserOrgGroup = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgGroup) ? null : ApiUserAccountData.UserOrgGroup),
                    UserOrgGroupNM = _GetCMCodeName(Entity_BaseAP.EnumCMCodeKind.OrgGroup, ApiUserAccountData.UserOrgGroup),
                    UserOrgPlace = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgPlace) ? null : ApiUserAccountData.UserOrgPlace),
                    UserOrgPlaceNM = _GetCMCodeName(Entity_BaseAP.EnumCMCodeKind.OrgPlace, ApiUserAccountData.UserOrgPlace),
                    UserOrgDept = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgDept) ? null : ApiUserAccountData.UserOrgDept),
                    UserOrgDeptNM = _GetCMCodeName(Entity_BaseAP.EnumCMCodeKind.OrgDept, ApiUserAccountData.UserOrgDept),
                    UserOrgTeam = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgTeam) ? null : ApiUserAccountData.UserOrgTeam),
                    UserOrgTeamNM = _GetCMCodeName(Entity_BaseAP.EnumCMCodeKind.OrgTeam, ApiUserAccountData.UserOrgTeam),
                    UserOrgJobTitle = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgJobTitle) ? null : ApiUserAccountData.UserOrgJobTitle),
                    UserOrgJobTitleNM = _GetCMCodeName(Entity_BaseAP.EnumCMCodeKind.OrgPTitle, ApiUserAccountData.UserOrgJobTitle),
                    UserOrgBizTitle = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgBIZTitle) ? null : ApiUserAccountData.UserOrgBIZTitle),
                    UserOrgBizTitleNM = _GetCMCodeName(Entity_BaseAP.EnumCMCodeKind.OrgPTitle2, ApiUserAccountData.UserOrgBIZTitle),
                    UsreOrgLevel = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgLevel) ? null : ApiUserAccountData.UserOrgLevel),
                    UsreOrgLevelNM = _GetCMCodeName(Entity_BaseAP.EnumCMCodeKind.OrgLevel, ApiUserAccountData.UserOrgLevel),
                    UserOrgTitle = new DBVarChar(string.IsNullOrWhiteSpace(ApiUserAccountData.UserOrgTitle) ? null : ApiUserAccountData.UserOrgTitle),
                    UserOrgTitleNM = _GetCMCodeName(Entity_BaseAP.EnumCMCodeKind.OrgTitle, ApiUserAccountData.UserOrgTitle)
                };

                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_USER_ACCOUNT, Mongo_BaseAP.EnumModifyType.I, apiNo, updUserID, execSysID, ipAddress, para);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }
        #endregion

        #region - 紀錄使用者權限 -
        /// <summary>
        /// 紀錄使用者權限
        /// </summary>
        /// <param name="apiNo"></param>
        /// <param name="updUserID"></param>
        /// <param name="sysID"></param>
        /// <param name="ipAddress"></param>
        public void RecordSysUserRole(string apiNo, string updUserID, string sysID, string ipAddress)
        {
            try
            {
                MongoERPUser.SysSystemRoleCondtionPara conditionPara = new MongoERPUser.SysSystemRoleCondtionPara
                {
                    SysSystemRoleCondtions = (from s in _sysUserRoleList
                                              select new
                                              {
                                                  SysID = s.SysID.GetValue(),
                                                  RoleConditionID = s.RoleConditionID.GetValue()
                                              }).Distinct()
                                                .Select(row => new MongoERPUser.SysSystemRoleCondtion
                                                {
                                                    SysID = new DBVarChar(row.SysID),
                                                    RoleConditionID = new DBVarChar(row.RoleConditionID)
                                                }).ToList()
                };

                List<MongoERPUser.SysSystemRoleCondtion> sysSystemRoleCondtionList = _mongoEntity.SelectSysSystemRoleCondtionList(conditionPara);

                EntityERPUser.LogUserSystemRolePara para = new EntityERPUser.LogUserSystemRolePara
                {
                    UserID = new DBVarChar(ApiUserAccountData.UserID),
                    ApiNO = new DBChar(apiNo),
                    ExecSysID = new DBVarChar(sysID),
                    IPAddress = new DBVarChar(ipAddress),
                    UpdUserID = new DBVarChar(updUserID),
                    SysUserSystemRoleConditionJsonStr = new DBNVarChar(Common.GetJsonSerializeObject(_sysUserRoleList.Select(n => new
                    {
                        SysID = n.SysID.GetValue(),
                        SysNM = n.SysNM.GetValue(),
                        RoleID = n.RoleID.GetValue(),
                        RoleNM = n.RoleNM.GetValue(),
                        RoleConditionID = n.RoleConditionID.GetValue(),
                        RoleConditionNM = n.RoleConditionNM.GetValue(),
                        RoleConditionSyntax = n.RoleConditionSyntax.GetValue()
                    }))),
                    SysRoleConditionJsonStr = new DBNVarChar(Common.GetJsonSerializeObject(sysSystemRoleCondtionList.Select(n =>
                    {
                        var roleConditionRules = _SystemRoleConditionToRules(new List<Mongo_BaseAP.RecordLogSystemRoleConditionGroupRule> { n.RoleConditionRules }).SingleOrDefault();

                        return new
                        {
                            SysID = n.SysID.GetValue(),
                            RoleConditionID = n.RoleConditionID.GetValue(),
                            RoleConditionSynTax = n.RoleConditionSynTax.GetValue(),
                            RoleConditionRules = Common.GetJsonSerializeObject(roleConditionRules)
                        };
                    }))),
                    UserSysRoleApplyJsonStr = new DBNVarChar(_systemRoleApplies.Any() ?
                        Common.GetJsonSerializeObject(_systemRoleApplies.Select(n => new
                        {
                            WFNo = n.WFNo.GetValue(),
                            UserID = n.UserID.GetValue(),
                            SysID = n.SysID.GetValue(),
                            RoleID = n.RoleID.GetValue(),
                            UpdUserID = n.UpdUserID.GetValue(),
                            ModifyType = n.ModifyType.GetValue(),
                            SysNM = n.SysNM.GetValue(),
                            RoleNM = n.RoleNM.GetValue()
                        })) :
                        null)
                };

                var userSystemRoleList = _entity.AddLogUserSystemRole(para);

                if (userSystemRoleList.Any())
                {
                    SysRoleEventParaList =
                        (from s in userSystemRoleList
                         group s by s.SysID.GetValue()
                         into sysRole
                         select new SysUserRoleEventData
                         {
                             TargetSysIDList = new List<string> { sysRole.Key },
                             UserID = ApiUserAccountData.UserID,
                             RoleGroupID = string.Empty,
                             RoleIDList = sysRole.Select(w => w.RoleID.GetValue()).Distinct().ToList()
                         }).ToList();
                }
                else
                {
                    SysRoleEventParaList = new List<SysUserRoleEventData>();
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }
        #endregion

        #region - 取得使用者權限清單 -
        /// <summary>
        /// 取得使用者權限清單
        /// </summary>
        public bool EditSysUserRoleList(string updUserID)
        {
            try
            {
                EntityERPUser.UserSysRolePara para = new EntityERPUser.UserSysRolePara
                {
                    UserID = new DBVarChar(ApiUserAccountData.UserID),
                    UpdUserID = new DBVarChar(updUserID)
                };

                _sysUserRoleList = _entity.EditSysUserRole(para);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 增加使用者申請過系統角色 -
        /// <summary>
        /// 增加使用者申請過系統角色
        /// </summary>
        /// <param name="updUserID"></param>
        /// <returns></returns>
        public bool EditUserSystemRoleApply(string updUserID)
        {
            try
            {
                string baseLineDT = GetNotIncludeAuthApplyedDate(MongoERPUser.EnumMongoDocName.LOG_USER_SYSTEM_ROLE_APPLY);
                EntityERPUser.UserSystemRolePara para = new EntityERPUser.UserSystemRolePara();
                MongoERPUser.UserSystemRoleApplyPara mongoPara = new MongoERPUser.UserSystemRoleApplyPara
                {
                    UserID = new DBVarChar(ApiUserAccountData.UserID),
                    JoinDate = new DBDateTime(Common.GetFormatDate(ApiUserAccountData.JoinDate)),
                    BaseLineDT = new DBDateTime(Convert.ToDateTime(baseLineDT))
                };

                var userSystemRoleApplyList = _mongoEntity.SelectUserSystemRoleApplyList(mongoPara);

                var userSystemRole = userSystemRoleApplyList
                    .Where(w => w.ModifyList != null)
                    .SelectMany(s => s.ModifyList, (roleApply, modify) =>
                        new
                        {
                            UserID = roleApply.UserID.GetValue(),
                            LogNo = roleApply.LogNo.GetValue(),
                            SysID = modify.SysID.GetValue(),
                            SysNM = modify.SysNM.GetValue(),
                            RoleID = modify.RoleID.GetValue(),
                            RoleNM = modify.RoleNM.GetValue(),
                            ModifyType = modify.ModifyType.GetValue(),
                            WFNo = roleApply.WFNo.GetValue()
                        }).ToList();

                _systemRoleApplies =
                    (from s in userSystemRole
                     group s by new
                     {
                         s.UserID,
                         s.SysID,
                         s.SysNM,
                         s.RoleID,
                         s.RoleNM
                     }
                     into g
                     let modifyInfo = (from d in g
                                       orderby d.LogNo descending
                                       select d).FirstOrDefault()
                     select new EntityERPUser.UserSystemRoleApply
                     {
                         UserID = new DBVarChar(g.Key.UserID),
                         SysID = new DBVarChar(g.Key.SysID),
                         SysNM = new DBNVarChar(g.Key.SysNM),
                         RoleID = new DBVarChar(g.Key.RoleID),
                         RoleNM = new DBNVarChar(g.Key.RoleNM),
                         UpdUserID = new DBVarChar(updUserID),
                         ModifyType = new DBChar(modifyInfo.ModifyType),
                         WFNo = new DBVarChar(modifyInfo.WFNo)
                     }).Where(w => w.ModifyType.GetValue() == Mongo_BaseAP.EnumModifyType.I.ToString())
                       .ToList();

                para.UserSystemRoleApplys = _systemRoleApplies;

                return _entity.EditUserSystemRole(para) == EntityERPUser.EnumEditUserSystemRoleResult.Success;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 增加使用者申請過系統功能 -
        public bool EditUserFunApply(string updUserID)
        {
            try
            {
                string baseLineDT = GetNotIncludeAuthApplyedDate(MongoERPUser.EnumMongoDocName.LOG_USER_FUN_APPLY);
                var funApplies = GetFunApplies(baseLineDT);
                EntityUserFunction.UserFunctionPara para = new EntityUserFunction.UserFunctionPara(EnumCultureID.zh_TW.ToString())
                {
                    UserID = new DBVarChar(ApiUserAccountData.UserID),
                    UpdUserID = new DBVarChar(updUserID)
                };

                var userFunctionList = new EntityUserFunction(ConnectionStringSERP, ProviderNameSERP)
                    .SelectUserFunctionList(para);

                funApplies.AddRange(
                    from s in userFunctionList
                    select $"{s.SysID.GetValue()}|{s.FunControllerID.GetValue()}|{s.FunActionName.GetValue()}");

                funApplies = funApplies.Distinct().ToList();

                if (funApplies.Any())
                {
                    EntityUserFunction.UserFunctionPara userFunctionPara = new EntityUserFunction.UserFunctionPara(string.Empty)
                    {
                        UserID = new DBVarChar((string.IsNullOrWhiteSpace(ApiUserAccountData.UserID) ? null : ApiUserAccountData.UserID)),
                        IsDisable = new DBChar(EnumYN.N.ToString()),
                        FunctionList = Utility.ToDBTypeList<DBVarChar>(funApplies),
                        UpdUserID = new DBVarChar((string.IsNullOrWhiteSpace(updUserID) ? null : updUserID))
                    };

                    return new EntityUserFunction(ConnectionStringSERP, ProviderNameSERP)
                        .EditUserFunctionResult(userFunctionPara) == EntityUserFunction.EnumEditUserFunctionResult.Success;
                }
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        private List<string> GetFunApplies(string baseLineDT)
        {
            MongoERPUser.UserFunApplyPara mongoPara = new MongoERPUser.UserFunApplyPara
            {
                UserID = new DBVarChar(ApiUserAccountData.UserID),
                JoinDate = new DBDateTime(Common.GetFormatDate(ApiUserAccountData.JoinDate)),
                BaseLineDT = new DBDateTime(Convert.ToDateTime(baseLineDT))
            };

            var userFun =
                _mongoEntity
                    .SelectUserFunApplyList(mongoPara)
                    .Where(w => w.ModifyList != null)
                    .SelectMany(f => f.ModifyList,
                        (funApply, modify) =>
                            new
                            {
                                LogNo = funApply.LogNo.GetValue(),
                                UserID = funApply.UserID.GetValue(),
                                SysID = modify.SysID.GetValue(),
                                SysNM = modify.SysNM.GetValue(),
                                FunControllerID = modify.FunControllerID.GetValue(),
                                FunControllerNM = modify.FunControllerNM.GetValue(),
                                FunActionNM = modify.FunActionNM.GetValue(),
                                FunNM = modify.FunNM.GetValue(),
                                ModifyType = modify.ModifyType.GetValue(),
                                ModifyTypeNM = modify.ModifyTypeNM.GetValue()
                            }).ToList();

            var funApplies =
                (from fun in userFun
                 group fun by new
                 {
                     fun.SysID,
                     fun.FunControllerID,
                     fun.FunActionNM
                 }
                    into g
                 let modifyInfo = (from d in g
                                   orderby d.LogNo descending
                                   select d).FirstOrDefault()
                 where modifyInfo.ModifyType == Mongo_BaseAP.EnumModifyType.I.ToString()
                 select $"{g.Key.SysID}|{g.Key.FunControllerID}|{g.Key.FunActionNM}")
                .ToList();
            return funApplies;
        }

        #endregion

        #region 查詢前一次勾選[不包含原來權限紀錄]的日期
        public string GetNotIncludeAuthApplyedDate(MongoERPUser.EnumMongoDocName EnumMongoDocName)
        {
            try
            {
                MongoERPUser.UserApplyPara mongoPara = new MongoERPUser.UserApplyPara
                {
                    UserID = new DBVarChar(ApiUserAccountData.UserID)
                };

                return (from s in _mongoEntity.SelectBaseLineDate(EnumMongoDocName, mongoPara)
                        where s.BaseLineDT.IsNull() == false
                        select s.BaseLineDT.GetFormattedValue(Common.EnumDateTimeFormatted.FullDateTime)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return string.Empty;
        }
        #endregion

        #region 增加使用者應用系統角色檔-選擇不保留權限日期LOG
        public void InsertUserSystemRoleApplyBaseLineDate(string apiNo, string updUserID, string execSysID, string ipAddress)
        {
            try
            {
                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_USER_SYSTEM_ROLE_APPLY, Mongo_BaseAP.EnumModifyType.U, apiNo, updUserID, execSysID, ipAddress, new Mongo_BaseAP.RecordLogUserSystemRoleApplyPara()
                {
                    UserID = new DBVarChar(this.ApiUserAccountData.UserID),
                    UserNM = new DBNVarChar(this.ApiUserAccountData.UserNM),
                    BaseLineDT = new DBDateTime(DateTime.Now)
                });
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }
        #endregion

        #region 增加使用者功能檔-選擇不保留權限日期LOG
        public void InsertUserFunApplyBaseLineDate(string apiNo, string updUserID, string execSysID, string ipAddress)
        {
            try
            {
                RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_USER_FUN_APPLY, Mongo_BaseAP.EnumModifyType.U, apiNo, updUserID, execSysID, ipAddress, new Mongo_BaseAP.RecordLogUserFunApplyPara()
                {
                    UserID = new DBVarChar(this.ApiUserAccountData.UserID),
                    UserNM = new DBNVarChar(this.ApiUserAccountData.UserNM),
                    BaseLineDT = new DBDateTime(DateTime.Now)
                });
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        } 
        #endregion

        #region - 取得代碼名稱 -
        /// <summary>
        /// 取得代碼名稱
        /// </summary>
        /// <param name="enumCmCodeType"></param>
        /// <param name="codeID"></param>
        /// <returns></returns>
        private DBNVarChar _GetCMCodeName(EntityERPUser.EnumCMCodeType enumCmCodeType, string codeID)
        {
            return (from s in _codeInfoList
                    where s.CMCodeType.GetValue() == enumCmCodeType.ToString() &&
                          s.CodeID.IsNull() == false &&
                          string.IsNullOrWhiteSpace(codeID) == false &&
                          s.CodeID.GetValue().Trim() == codeID.Trim()
                    select s.CodeNM).SingleOrDefault();
        }

        /// <summary>
        /// 取得代碼名稱
        /// </summary>
        /// <param name="enumCodeKind"></param>
        /// <param name="codeID"></param>
        /// <returns></returns>
        private DBNVarChar _GetCMCodeName(Entity_BaseAP.EnumCMCodeKind enumCodeKind, string codeID)
        {
            return (from s in _codeInfoList
                    where s.CodeKind.GetValue() == Common.GetEnumDesc(enumCodeKind) &&
                          s.CodeID.IsNull() == false &&
                          string.IsNullOrWhiteSpace(codeID) == false &&
                          s.CodeID.GetValue().Trim() == codeID.Trim()
                    select s.CodeNM).SingleOrDefault();
        }
        #endregion

        #region - 取得代碼資訊 -
        /// <summary>
        /// 取得代碼資訊
        /// </summary>
        /// <returns></returns>
        private bool _GetCMCodeInfoList()
        {
            try
            {
                EntityERPUser.CMCodeInfoPara para = new EntityERPUser.CMCodeInfoPara(EnumCultureID.zh_TW.ToString())
                {
                    CMCodes = new List<EntityERPUser.CMCode>
                    {
                        new EntityERPUser.CMCode
                        {
                            CMCodeType = EntityERPUser.EnumCMCodeType.CMCode,
                            CodeID = new DBVarChar(ApiUserAccountData.UserOrgArea),
                            CodeKind = new DBVarChar(Common.GetEnumDesc(Entity_BaseAP.EnumCMCodeKind.OrgArea))
                        },
                        new EntityERPUser.CMCode
                        {
                            CMCodeType = EntityERPUser.EnumCMCodeType.CMCode,
                            CodeID = new DBVarChar(ApiUserAccountData.UserOrgGroup),
                            CodeKind = new DBVarChar(Common.GetEnumDesc(Entity_BaseAP.EnumCMCodeKind.OrgGroup))
                        },
                        new EntityERPUser.CMCode
                        {
                            CMCodeType = EntityERPUser.EnumCMCodeType.CMCode,
                            CodeID = new DBVarChar(ApiUserAccountData.UserOrgPlace),
                            CodeKind = new DBVarChar(Common.GetEnumDesc(Entity_BaseAP.EnumCMCodeKind.OrgPlace))
                        },
                        new EntityERPUser.CMCode
                        {
                            CMCodeType = EntityERPUser.EnumCMCodeType.CMCode,
                            CodeID = new DBVarChar(ApiUserAccountData.UserOrgDept),
                            CodeKind = new DBVarChar(Common.GetEnumDesc(Entity_BaseAP.EnumCMCodeKind.OrgDept))
                        },
                        new EntityERPUser.CMCode
                        {
                            CMCodeType = EntityERPUser.EnumCMCodeType.CMCode,
                            CodeID = new DBVarChar(ApiUserAccountData.UserOrgTeam),
                            CodeKind = new DBVarChar(Common.GetEnumDesc(Entity_BaseAP.EnumCMCodeKind.OrgTeam))
                        },
                        new EntityERPUser.CMCode
                        {
                            CMCodeType = EntityERPUser.EnumCMCodeType.CMCode,
                            CodeID = new DBVarChar(ApiUserAccountData.UserOrgJobTitle),
                            CodeKind = new DBVarChar(Common.GetEnumDesc(Entity_BaseAP.EnumCMCodeKind.OrgPTitle))
                        },
                        new EntityERPUser.CMCode
                        {
                            CMCodeType = EntityERPUser.EnumCMCodeType.CMCode,
                            CodeID = new DBVarChar(ApiUserAccountData.UserOrgBIZTitle),
                            CodeKind = new DBVarChar(Common.GetEnumDesc(Entity_BaseAP.EnumCMCodeKind.OrgPTitle2))
                        },
                        new EntityERPUser.CMCode
                        {
                            CMCodeType = EntityERPUser.EnumCMCodeType.CMCode,
                            CodeID = new DBVarChar(ApiUserAccountData.UserOrgLevel),
                            CodeKind = new DBVarChar(Common.GetEnumDesc(Entity_BaseAP.EnumCMCodeKind.OrgLevel))
                        },
                        new EntityERPUser.CMCode
                        {
                            CMCodeType = EntityERPUser.EnumCMCodeType.CMCode,
                            CodeID = new DBVarChar(ApiUserAccountData.UserOrgTitle),
                            CodeKind = new DBVarChar(Common.GetEnumDesc(Entity_BaseAP.EnumCMCodeKind.OrgTitle))
                        },
                        new EntityERPUser.CMCode
                        {
                            CMCodeType = EntityERPUser.EnumCMCodeType.CMOrgCom,
                            CodeID = new DBVarChar(ApiUserAccountData.UserOrgWorkCom)
                        },
                        new EntityERPUser.CMCode
                        {
                            CMCodeType = EntityERPUser.EnumCMCodeType.CMOrgCom,
                            CodeID = new DBVarChar(ApiUserAccountData.UserComID)
                        },
                        new EntityERPUser.CMCode
                        {
                            CMCodeType = EntityERPUser.EnumCMCodeType.CMOrgCom,
                            CodeID = new DBVarChar(ApiUserAccountData.UserSalaryComID)
                        },
                        new EntityERPUser.CMCode
                        {
                            CMCodeType = EntityERPUser.EnumCMCodeType.CMCode,
                            CodeID = new DBVarChar(ApiUserAccountData.UserTitleID),
                            CodeKind = new DBVarChar(Common.GetEnumDesc(Entity_BaseAP.EnumCMCodeKind.Title))
                        },
                        new EntityERPUser.CMCode
                        {
                            CMCodeType = EntityERPUser.EnumCMCodeType.CMOrgUnit,
                            CodeID = new DBVarChar(ApiUserAccountData.UserUnitID)
                        },
                        new EntityERPUser.CMCode
                        {
                            CMCodeType = EntityERPUser.EnumCMCodeType.CMCode,
                            CodeID = new DBVarChar(ApiUserAccountData.UserWorkID),
                            CodeKind = new DBVarChar(Common.GetEnumDesc(Entity_BaseAP.EnumCMCodeKind.Work))
                        }
                    }
                };

                _codeInfoList = _entity.SelectCMCodeInfoList(para);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        public bool CreateERPUserAccount()
        {
            try
            {
                EntityERPUser entityERPUser = new EntityERPUser(this.ConnectionStringSERP, this.ProviderNameSERP);

                EntityERPUser.ERPUserAccountPara para = new EntityERPUser.ERPUserAccountPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserID) ? null : this.APIData.UserID)),
                    UserNM = new DBNVarChar((string.IsNullOrWhiteSpace(this.APIData.UserNM) ? null : this.APIData.UserNM)),
                    UserComID = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserComID.Trim()) ? null : this.APIData.UserComID.Trim())),
                    UserUnitID = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserUnitID) ? null : this.APIData.UserUnitID)),
                    UserPWD = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserPWD) ? null : this.APIData.UserPWD)),
                    PWDValidDate = new DBVarChar(Common.GetDateString(DateTime.Now.AddMonths(3))),
                    IsLeft = new DBChar((string.IsNullOrWhiteSpace(this.APIData.IsLeft) ? EnumYN.N.ToString() : this.APIData.IsLeft))
                };

                if (entityERPUser.CreateAccount(para) == EntityERPUser.EnumCreateAccountResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }

        public bool ResetERPUserPWD()
        {
            try
            {
                EntityERPUser entityERPUser = new EntityERPUser(this.ConnectionStringSERP, this.ProviderNameSERP);

                EntityERPUser.ERPUserAccountPara para = new EntityERPUser.ERPUserAccountPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserID) ? null : this.APIData.UserID)),
                    UserPWD = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserPWD) ? null : this.APIData.UserPWD)),
                    PWDValidDate = new DBVarChar(Common.GetDateString(DateTime.Now.AddMonths(3)))
                };

                if (entityERPUser.ResetUserPWD(para) == EntityERPUser.EnumResetUserPWDResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }

        public bool UnlockERPUserAccount()
        {
            return this.GetUnlockUserAccountResult();
        }

        public AuthenticationResults ValidateERPUserInfor()
        {
            AuthenticationResults auth = new AuthenticationResults();
            auth.VerificationResult = EnumVerificationResult.None;
            auth.ErrorTimes = -1;

            try
            {
                if (!string.IsNullOrWhiteSpace(this.APIData.UserID) && !string.IsNullOrWhiteSpace(this.APIData.UserPWD) &&
                    !string.IsNullOrWhiteSpace(this.APIData.IPAddress))
                {
                    Entity_Base.EnumValidateIPAddressIsTrustResult trustResult = this.GetValidateIPAddressIsTrust(this.APIData.IPAddress);
                    if (trustResult == Entity_Base.EnumValidateIPAddressIsTrustResult.Reject)
                    {
                        auth.VerificationResult = EnumVerificationResult.RejectIP;
                        auth.VerificationMessage = Common.GetEnumDesc(auth.VerificationResult);
                        return auth;
                    }

                    if (trustResult == Entity_Base.EnumValidateIPAddressIsTrustResult.Trust)
                    {
                        if (this.APIData.Location == EnumLocation.COMP)
                        {
                            auth.VerificationResult = this.ValidateAccountStatus(this.APIData.UserID, this.APIData.UserPWD, null, null);
                        }
                        else
                        {
                            auth.VerificationResult = EnumVerificationResult.InternalIPLocationError;
                        }
                    }
                    else
                    {
                        if (this.APIData.Location == EnumLocation.COMP)
                        {
                            auth.VerificationResult = EnumVerificationResult.ExternalIPLocationError;
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(this.APIData.UserIDNo) && !string.IsNullOrWhiteSpace(this.APIData.UserBirthday))
                            {
                                auth.VerificationResult = this.ValidateAccountStatus(this.APIData.UserID, this.APIData.UserPWD, this.APIData.UserIDNo, this.APIData.UserBirthday);
                            }
                            else
                            {
                                auth.VerificationResult = EnumVerificationResult.ExternalIPUserInfoRequired;
                            }
                        }
                    }
                }
                else
                {
                    auth.VerificationResult = EnumVerificationResult.ParameterProcessingError;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            auth.VerificationMessage = Common.GetEnumDesc(auth.VerificationResult);
            auth.ErrorTimes = this.GetAccountErrorTimesResult();
            return auth;
        }

        private List<QueryCondition.RoleConditionRules> _SystemRoleConditionToRules(List<Mongo_BaseAP.RecordLogSystemRoleConditionGroupRule> systemRoleConGroupRule)
        {
            List<QueryCondition.RoleConditionRules> result = new List<QueryCondition.RoleConditionRules>();
            if (systemRoleConGroupRule != null &&
                systemRoleConGroupRule.Any())
            {
                result.AddRange((from s in systemRoleConGroupRule
                                 let groupRuleList = _SystemRoleConditionToRules(s.GroupRuleList)
                                 where ((groupRuleList != null && groupRuleList.Any()) ||
                                        (s.RuleList != null && s.RuleList.Any(w => string.IsNullOrWhiteSpace(w.Operator.GetValue()) == false &&
                                                                                   string.IsNullOrWhiteSpace(w.ID.GetValue()) == false)))
                                 select new QueryCondition.RoleConditionRules
                                 {
                                     CONDITION = s.Condition.GetValue(),
                                     RULE_LIST =
                                         (s.RuleList ?? new List<Mongo_BaseAP.RecordLogSystemRoleConditionRoleRule>())
                                         .Select(c => new QueryCondition.RoleConditionRoleRule
                                         {
                                             ID = c.ID.GetValue(),
                                             OPERATOR = c.Operator.GetValue(),
                                             VALUE = c.Value.GetValue()
                                         }).ToList(),
                                     GROUP_RULE_LIST = groupRuleList
                                 }));
            }

            return result;
        }

        private Entity_Base.EnumValidateIPAddressIsTrustResult GetValidateIPAddressIsTrust(string ip)
        {
            try
            {
                Entity_Base.TrustIPPara trustIPPara = new Entity_Base.TrustIPPara()
                {
                    IpAddress = new DBVarChar((string.IsNullOrWhiteSpace(ip) ? null : ip))
                };

                Entity_Base.EnumValidateIPAddressIsTrustResult result = new Entity_Base(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .ValidateIPAddressIsTrust(trustIPPara);

                return result;
            }
            catch (Exception ex)
            {
                this.OnException(ex);
            }
            return Entity_Base.EnumValidateIPAddressIsTrustResult.UnTrust;
        }

        private EnumVerificationResult ValidateAccountStatus(string userID, string userPWD, string userIDNo, string userBirthday)
        {
            EnumVerificationResult result = EnumVerificationResult.None;

            EntityERPUser.ERPUserAccountPara para = new EntityERPUser.ERPUserAccountPara()
            {
                UserID = new DBVarChar(userID)
            };

            EntityERPUser.EnumValidateAccountResult accountStatus = new EntityERPUser(this.ConnectionStringSERP, this.ProviderNameSERP)
                .SelectAccountStatus(para);

            switch (accountStatus)
            {
                case EntityERPUser.EnumValidateAccountResult.Valid:
                    result = this.ValidateAccountInfor(this.APIData.UserID, userPWD, userIDNo, userBirthday);
                    break;
                case EntityERPUser.EnumValidateAccountResult.NotExist:
                    result = EnumVerificationResult.AccountNotExist;
                    break;
                case EntityERPUser.EnumValidateAccountResult.Lock:
                    result = EnumVerificationResult.AccountIsLock;
                    break;
                case EntityERPUser.EnumValidateAccountResult.Leave:
                    result = EnumVerificationResult.AccountIsLeft;
                    break;
            }

            return result;
        }

        private EnumVerificationResult ValidateAccountInfor(string userID, string userPWD, string userIDNo, string userBirthday)
        {
            EnumVerificationResult result = EnumVerificationResult.None;

            EntityERPUser.ERPUserAccountPara para = new EntityERPUser.ERPUserAccountPara()
            {
                UserID = new DBVarChar(userID),
                UserPWD = new DBVarChar(userPWD),
                UserIDNo = new DBVarChar(userIDNo),
                UserBirthday = new DBVarChar(userBirthday)
            };

            EntityERPUser.EnumValidateAccountResult validateResult = EntityERPUser.EnumValidateAccountResult.Valid;

            if (ValidateERPUserAccount(userID, userPWD) == false)
            {
                validateResult = EntityERPUser.EnumValidateAccountResult.Invalid;
            }

            if (validateResult == EntityERPUser.EnumValidateAccountResult.Valid)
            {
                validateResult = new EntityERPUser(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .ValidateUserAccount(para);
            }

            switch (validateResult)
            {
                case EntityERPUser.EnumValidateAccountResult.Valid:
                    result = EnumVerificationResult.Success;
                    this.GetUnlockUserAccountResult();
                    break;
                case EntityERPUser.EnumValidateAccountResult.UserInfoInvalid:
                    result = EnumVerificationResult.ExternalIPUserInfoError;
                    this.GetEditSystemPurviewDetailResult();
                    break;
                case EntityERPUser.EnumValidateAccountResult.Invalid:
                    result = EnumVerificationResult.ValidateError;
                    this.GetEditSystemPurviewDetailResult();
                    break;
            }

            return result;
        }

        internal virtual bool ValidateERPUserAccount(string userID, string userPWD)
        {
            return new EntityERPUser(this.ConnectionStringERP, this.ProviderNameERP)
                .ValidateERPUserAccount(new EntityERPUser.ERPUserAccountPara
                {
                    UserID = new DBVarChar(userID),
                    UserPWD = new DBVarChar(Security.Decrypt(userPWD))
                });
        }

        private bool GetEditSystemPurviewDetailResult()
        {
            try
            {
                EntityERPUser.ERPUserAccountPara para = new EntityERPUser.ERPUserAccountPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserID) ? null : this.APIData.UserID))
                };

                if (new EntityERPUser(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .EditUserLoginErrorTimes(para) == EntityERPUser.EnumEditUserLoginErrorTimesResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        private bool GetUnlockUserAccountResult()
        {
            try
            {
                EntityERPUser.ERPUserAccountPara para = new EntityERPUser.ERPUserAccountPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserID) ? null : this.APIData.UserID))
                };

                if (new EntityERPUser(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .UnlockUserAccount(para) == EntityERPUser.EnumUnlockUserAccountResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }

        private int GetAccountErrorTimesResult()
        {
            try
            {
                EntityERPUser.ERPUserAccountPara para = new EntityERPUser.ERPUserAccountPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserID) ? null : this.APIData.UserID))
                };

                DBInt errorTimes = new EntityERPUser(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectAccountErrorTimes(para);

                if (!errorTimes.IsNull())
                {
                    return errorTimes.GetValue();
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return -1;
        }

        public bool GetRecordUserLoginResult(EnumVerificationResult result, string apiNo)
        {
            try
            {
                DBVarChar updUserID = new EntityAuthorization(this.ConnectionStringSERP, this.ProviderNameSERP).UpdUserID;

                string cultureID = LionTech.Entity.ERP.EnumCultureID.zh_TW.ToString();

                Entity_BaseAP.BasicInfoPara para = new Entity_BaseAP.BasicInfoPara(cultureID)
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(this.APIData.UserID) ? null : this.APIData.UserID),
                    UpdUserID = updUserID,
                    ExecSysID = new DBVarChar((string.IsNullOrWhiteSpace(this.ClientSysID) ? null : this.ClientSysID))
                };

                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectBasicInfo(para);

                Entity_BaseAP.CMCodePara codePara = new Entity_BaseAP.CMCodePara()
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNM,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.UserLocation,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID.ToString())
                };

                List<Entity_BaseAP.CMCode> userLocationList = new Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectCMCodeList(codePara);

                Mongo_BaseAP.RecordUserLoginPara recordPara = new Mongo_BaseAP.RecordUserLoginPara()
                {
                    UserID = entityBasicInfo.UserID,
                    UserNM = entityBasicInfo.UserNM,
                    UserPWD = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserPWD) ? null : this.APIData.UserPWD)),
                    Location = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.Location.ToString()) ? null : this.APIData.Location.ToString())),
                    LocationNM = new DBNVarChar(null),
                    LocationDesc = new DBNVarChar((string.IsNullOrWhiteSpace(this.APIData.LocationDesc) ? null : this.APIData.LocationDesc)),
                    UserIDNo = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserIDNo) ? null : this.APIData.UserIDNo)),
                    UserBirthday = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.UserBirthday) ? null : this.APIData.UserBirthday)),
                    IPAddress = new DBVarChar((string.IsNullOrWhiteSpace(this.APIData.IPAddress) ? null : this.APIData.IPAddress)),
                    ValidResult = new DBVarChar((string.IsNullOrWhiteSpace(result.ToString()) ? null : result.ToString())),
                    ValidResultNM = new DBNVarChar(Common.GetEnumDesc(result)),
                    APINo = new DBChar(apiNo),
                    UpdUserID = entityBasicInfo.UpdUserID,
                    UpdUserNM = entityBasicInfo.UpdUserNM,
                    UpdDT = new DBDateTime(DateTime.Now),
                    ExecSysID = entityBasicInfo.ExecSysID,
                    ExecSysNM = entityBasicInfo.ExecSysNM,
                    ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(this.ClientIP) ? null : this.ClientIP))
                };

                if (userLocationList != null && userLocationList.Count > 0 &&
                    !string.IsNullOrWhiteSpace(this.APIData.Location.ToString()))
                {
                    recordPara.LocationNM = (userLocationList.Find(e => e.CodeID.GetValue() == this.APIData.Location.ToString())).CodeNM;
                }

                if (new Mongo_BaseAP(this.ConnectionStringMSERP, this.ProviderNameMSERP)
                    .RecordUserLogin(recordPara) == Mongo_BaseAP.EnumRecordUserLoginResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public bool GetRecordUserPWDResult(EnumYN isReset, string apiNo)
        {
            try
            {
                DBVarChar updUserID = new MongoAuthorization(this.ConnectionStringMSERP, this.ProviderNameMSERP).UpdUserID;

                string cultureID = LionTech.Entity.ERP.EnumCultureID.zh_TW.ToString();

                Entity_BaseAP.BasicInfoPara para = new Entity_BaseAP.BasicInfoPara(cultureID)
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(this.APIData.UserID) ? null : this.APIData.UserID),
                    UpdUserID = new DBVarChar(updUserID),
                    ExecSysID = new DBVarChar((string.IsNullOrWhiteSpace(this.ClientSysID) ? null : this.ClientSysID))
                };

                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectBasicInfo(para);

                Mongo_BaseAP.RecordUserPWDPara recordPara = new Mongo_BaseAP.RecordUserPWDPara()
                {
                    UserID = entityBasicInfo.UserID,
                    UserNM = entityBasicInfo.UserNM,
                    UserPWD = new DBVarChar(string.IsNullOrWhiteSpace(this.APIData.UserPWD) ? null : this.APIData.UserPWD),
                    IsReset = new DBChar(isReset.ToString()),
                    ModifyDate = new DBChar(Common.GetDateString()),
                    APINo = new DBChar(apiNo),
                    UpdUserID = entityBasicInfo.UpdUserID,
                    UpdUserNM = entityBasicInfo.UpdUserNM,
                    UpdDT = new DBDateTime(DateTime.Now),
                    ExecSysID = entityBasicInfo.ExecSysID,
                    ExecSysNM = entityBasicInfo.ExecSysNM,
                    ExecIPAddress = new DBVarChar((string.IsNullOrWhiteSpace(this.ClientIP) ? null : this.ClientIP))
                };

                if (new Mongo_BaseAP(this.ConnectionStringMSERP, this.ProviderNameMSERP)
                    .RecordUserPWD(recordPara) == Mongo_BaseAP.EnumRecordUserPWDResult.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }

        public List<string> GetSystemUserList()
        {
            var para = new EntityERPUser.SystemPara { SysID = new DBVarChar(ClientSysID) };
            return _entity.SelectSystemUserList(para).Select(s => s.UserID.GetValue()).ToList();
        }
    }
}