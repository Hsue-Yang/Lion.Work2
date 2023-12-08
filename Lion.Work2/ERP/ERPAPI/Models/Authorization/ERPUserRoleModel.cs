using System;
using System.Collections.Generic;
using System.Linq;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Authorization;
using LionTech.Entity.ERP.Sys;

namespace ERPAPI.Models.Authorization
{
    public class ERPUserRoleModel : AuthorizationModel
    {
        #region - Definitions -
        public class APIParaData
        {
            public string UserID { get; set; }
            public string SysID { get; set; }
            public string ErpWFNo { get; set; }
            public string Memo { get; set; }
            public List<string> RoleIDList { get; set; }
        }
        #endregion

        #region - Constructor -
        public ERPUserRoleModel()
        {
            _entity = new EntityERPUserRole(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - API Property -
        public string ClientSysID { get; set; }

        public string ClientUserID { get; set; }

        public string UserID { get; set; }

        public string APIPara { get; set; }

        public APIParaData APIData { get; set; }
        #endregion

        #region - Private -
        private readonly EntityERPUserRole _entity;
        private List<EntityERPUserRole.UserSystemRole> _entityUserSystemRoleList;
        #endregion

        internal List<string> GetUserRoleList()
        {
            EntityERPUserRole.UserSystemRolePara para = new EntityERPUserRole.UserSystemRolePara(EnumCultureID.zh_TW.ToString())
            {
                UserID = new DBVarChar(UserID),
                SysID = new DBVarChar(ClientSysID)
            };

            return _entity.SelectUserSystemRoleList(para).Select(s => s.RoleID.GetValue()).ToList();
        }

        #region - 編輯使用者角色 -
        /// <summary>
        /// 編輯使用者角色
        /// </summary>
        /// <param name="apiNo"></param>
        /// <param name="updUserID"></param>
        /// <param name="execSysID"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool EditUserRole(string apiNo, string updUserID, string execSysID, string ipAddress)
        {
            try
            {
                EntityUserRoleFunDetail.UserRawPara userRawPara = new EntityUserRoleFunDetail.UserRawPara
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(APIData.UserID) ? null : APIData.UserID))
                };

                bool isDisable = new EntityUserRoleFunDetail(ConnectionStringSERP, ProviderNameSERP)
                    .SelectRawUserIsDisable(userRawPara);

                EntityUserRoleFunDetail.UserSystemRolePara para = new EntityUserRoleFunDetail.UserSystemRolePara(string.Empty)
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(APIData.UserID) ? null : APIData.UserID)),
                    IpAddress = new DBVarChar(ipAddress),
                    ApiNO = new DBChar(apiNo),
                    ErpWFNO = new DBVarChar(APIData.ErpWFNo),
                    Memo = new DBNVarChar(APIData.Memo),
                    ExecSysID = new DBVarChar(execSysID),
                    IsDisable = new DBChar((isDisable ? EnumYN.Y.ToString() : EnumYN.N.ToString())),
                    UpdUserID = new DBVarChar(_entity.UpdUserID.GetValue())
                };

                List<EntityUserRoleFunDetail.UserSystemRolePara> paraList = new List<EntityUserRoleFunDetail.UserSystemRolePara>();

                EntityERPUserRole.UserSystemRolePara paraOriginal = new EntityERPUserRole.UserSystemRolePara(EnumCultureID.zh_TW.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(APIData.UserID) ? null : APIData.UserID))
                };

                _entityUserSystemRoleList = _entity.SelectUserSystemRoleList(paraOriginal);

                paraList.AddRange(from s in _entityUserSystemRoleList
                                  where s.SysID.GetValue() != APIData.SysID
                                  select new EntityUserRoleFunDetail.UserSystemRolePara(string.Empty)
                                  {
                                      UserID = new DBVarChar((string.IsNullOrWhiteSpace(APIData.UserID) ? null : APIData.UserID)),
                                      SysID = s.SysID,
                                      RoleID = s.RoleID,
                                      UpdUserID = s.UpdUserID,
                                      UpdDT = s.UpdDT
                                  });

                if (APIData.RoleIDList != null &&
                    APIData.RoleIDList.Any())
                {
                    paraList.AddRange(
                        APIData.RoleIDList
                               .Select(role => new EntityUserRoleFunDetail.UserSystemRolePara(string.Empty)
                               {
                                   UserID = new DBVarChar((string.IsNullOrWhiteSpace(APIData.UserID) ? null : APIData.UserID)),
                                   SysID = new DBVarChar((string.IsNullOrWhiteSpace(APIData.SysID) ? null : APIData.SysID)),
                                   RoleID = new DBVarChar((string.IsNullOrWhiteSpace(role) ? null : role)),
                                   UpdUserID = new DBVarChar(_entity.UpdUserID.GetValue()),
                                   UpdDT = new DBDateTime(DateTime.Now)
                               }));
                }

                if (new EntityUserRoleFunDetail(ConnectionStringSERP, ProviderNameSERP)
                    .EditUserSystemRole(para, paraList) == EntityUserRoleFunDetail.EnumEditUserSystemRoleResult.Success)
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

        #region - 紀錄使用者角色異動 -
        public void GetERPRecordUserSystemRoleApplyResult(string apiNo, string updUserID, string execSysID, string ipAddress)
        {
            try
            {
                var modifyList = _GetUserSystemRoleModifyList();

                if (modifyList.Any())
                {
                    Entity_BaseAP.BasicInfoPara entityBasicInfoPara =
                        new Entity_BaseAP.BasicInfoPara(EnumCultureID.zh_TW.ToString())
                        {
                            UserID = new DBVarChar(APIData.UserID),
                            ExecSysID = new DBVarChar(APIData.SysID)
                        };

                    Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                        .SelectBasicInfo(entityBasicInfoPara);

                    Mongo_BaseAP.RecordLogUserSystemRoleApplyPara para = new Mongo_BaseAP.RecordLogUserSystemRoleApplyPara();
                    para.UserID = new DBVarChar(APIData.UserID);
                    para.UserNM = entityBasicInfo.UserNM;
                    para.WFNO = new DBVarChar(APIData.ErpWFNo);
                    para.Memo = new DBVarChar(APIData.Memo);
                    para.ModifyList = _GetUserSystemRoleModifyList();

                    RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_USER_SYSTEM_ROLE_APPLY, Mongo_BaseAP.EnumModifyType.U, apiNo, updUserID, execSysID, ipAddress, para);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }
        #endregion

        #region - 取得系統角色異動清單 -
        private List<Mongo_BaseAP.UserSystemRoleModify> _GetUserSystemRoleModifyList()
        {
            Entity_BaseAP.CMCodePara codePara =
                new Entity_BaseAP.CMCodePara
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNM,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.ModifyType,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(EnumCultureID.zh_TW.ToString())
                };

            var modifyTypeDic = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                .SelectCMCodeList(codePara)
                .ToDictionary(key => key.CodeID.GetValue(), val => val.CodeNM);

            EntityERPUserRole.SystemRoleNMPara para = new EntityERPUserRole.SystemRoleNMPara(EnumCultureID.zh_TW.ToString())
            {
                SysID = new DBVarChar((string.IsNullOrWhiteSpace(APIData.SysID) ? null : APIData.SysID))
            };

            var systemRoleNMDic = _entity.SelectSystemRoleNMList(para).ToDictionary(k => k.RoleID.GetValue(), v => v.RoleNM);

            var sysNM = _entityUserSystemRoleList.Select(s => s.SysNM).FirstOrDefault();

            var userSystemRoles = _entityUserSystemRoleList.Where(s => s.SysID.GetValue() == APIData.SysID).ToList();

            return (from s in userSystemRoles
                    where APIData.RoleIDList.Contains(s.RoleID.GetValue()) == false
                    select new Mongo_BaseAP.UserSystemRoleModify
                    {
                        SysID = s.SysID,
                        SysNM = s.SysNM,
                        RoleID = s.RoleID,
                        RoleNM = s.RoleNM,
                        ModifyType = new DBChar(Mongo_BaseAP.EnumModifyType.D.ToString()),
                        ModifyTypeNM = modifyTypeDic[Mongo_BaseAP.EnumModifyType.D.ToString()]
                    }).Concat(
                        (from s in APIData.RoleIDList
                         where userSystemRoles.Exists(r => r.RoleID.GetValue() == s) == false
                         select new Mongo_BaseAP.UserSystemRoleModify
                         {
                             SysID = new DBVarChar(APIData.SysID),
                             SysNM = sysNM,
                             RoleID = new DBVarChar(s),
                             RoleNM = systemRoleNMDic[s],
                             ModifyType = new DBChar(Mongo_BaseAP.EnumModifyType.I.ToString()),
                             ModifyTypeNM = modifyTypeDic[Mongo_BaseAP.EnumModifyType.I.ToString()]
                         })).ToList();
        }
        #endregion
    }
}