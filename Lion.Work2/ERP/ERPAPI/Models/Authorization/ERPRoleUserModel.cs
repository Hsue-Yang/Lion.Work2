using System;
using System.Collections.Generic;
using System.Linq;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Authorization;

namespace ERPAPI.Models.Authorization
{
    public class ERPRoleUserModel : AuthorizationModel
    {
        #region - Definitions -
        public class APIParaData
        {
            public string SysID { get; set; }
            public string RoleID { get; set; }
            public string ErpWFNo { get; set; }
            public string Memo { get; set; }
            public List<string> UserIDList { get; set; }
            public bool IsOverride { get; set; }
        }
        #endregion

        #region - Constructor -
        public ERPRoleUserModel()
        {
            _entity = new EntityERPRoleUser(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - API Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public string RoleID { get; set; }

        public string APIPara { get; set; }

        public APIParaData APIData { get; set; }

        public List<string> AllUserIDList => APIData.UserIDList.Concat(OriginalRoleUserList).Distinct().ToList();
        #endregion

        #region - Field -
        public List<string> OriginalRoleUserList;
        #endregion

        #region - Private -
        private readonly EntityERPRoleUser _entity;
        #endregion

        #region - 編輯角色使用者 -
        /// <summary>
        /// 編輯角色使用者
        /// </summary>
        /// <param name="apiNo"></param>
        /// <param name="updUserID"></param>
        /// <param name="execSysID"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool EditRoleUser(string apiNo, string updUserID, string execSysID, string ipAddress)
        {
            try
            {
                EntityERPRoleUser.RoleUserPara para = new EntityERPRoleUser.RoleUserPara
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(APIData.SysID) ? null : APIData.SysID)),
                    RoleID = new DBVarChar((string.IsNullOrWhiteSpace(APIData.RoleID) ? null : APIData.RoleID)),
                };

                OriginalRoleUserList 
                    = APIData.IsOverride ? _entity.SelectRoleUserList(para).Select(s => s.UserID.GetValue()).ToList() : new List<string>();

                para.ErpWFNO = new DBVarChar(APIData.ErpWFNo);
                para.ApiNO = new DBChar(apiNo);
                para.Memo = new DBNVarChar(string.IsNullOrWhiteSpace(APIData.Memo) ? null : APIData.Memo);
                para.IpAddress = new DBVarChar(ipAddress);
                para.ExecSysID = new DBVarChar(execSysID);
                para.IsOverride = new DBBit(APIData.IsOverride);
                para.UpdUserID = new DBVarChar(updUserID);
                para.UserIDList = Utility.ToDBTypeList<DBVarChar>(APIData.UserIDList.Distinct().ToList());
                para.LogUserIDList = Utility.ToDBTypeList<DBVarChar>(APIData.UserIDList.Concat(OriginalRoleUserList).Distinct().ToList());

                if (_entity.EditRoleUser(para) == EntityERPRoleUser.EnumEditRoleUserResult.Success)
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

        public List<string> GetRoleUserList()
        {
            EntityERPRoleUser.RoleUserPara para = new EntityERPRoleUser.RoleUserPara
            {
                SysID = new DBVarChar((string.IsNullOrWhiteSpace(ClientSysID) ? null : ClientSysID)),
                RoleID = new DBVarChar((string.IsNullOrWhiteSpace(RoleID) ? null : RoleID))
            };

            return _entity.SelectRoleUserList(para).Select(s => s.UserID.GetValue()).ToList();
        }

        #region - 紀錄角色使用者異動 -
        public void GetERPRecordUserSystemRoleApplyResult(string apiNo, string updUserID, string execSysID, string ipAddress)
        {
            try
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

                EntityERPRoleUser.SystemRoleNMPara roleNMPara = new EntityERPRoleUser.SystemRoleNMPara(EnumCultureID.zh_TW.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(APIData.SysID) ? null : APIData.SysID))
                };

                var systemRoleNMDic = _entity.SelectSystemRoleNMList(roleNMPara).ToDictionary(k => k.RoleID.GetValue(), v => v.RoleNM);

                foreach (var userID in APIData.UserIDList)
                {
                    Entity_BaseAP.BasicInfoPara entityBasicInfoPara =
                        new Entity_BaseAP.BasicInfoPara(EnumCultureID.zh_TW.ToString())
                        {
                            ExecSysID = new DBVarChar(APIData.SysID),
                            UserID = new DBVarChar(userID)
                        };

                    Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                        .SelectBasicInfo(entityBasicInfoPara);

                    Mongo_BaseAP.RecordLogUserSystemRoleApplyPara para = new Mongo_BaseAP.RecordLogUserSystemRoleApplyPara();
                    para.UserID = entityBasicInfo.UserID;
                    para.UserNM = entityBasicInfo.UserNM;
                    para.WFNO = new DBVarChar(APIData.ErpWFNo);
                    para.Memo = new DBVarChar(APIData.Memo);
                    para.ModifyList = new List<Mongo_BaseAP.UserSystemRoleModify>
                    {
                        new Mongo_BaseAP.UserSystemRoleModify
                        {
                            SysID = entityBasicInfo.ExecSysID,
                            SysNM = entityBasicInfo.ExecSysNM,
                            RoleID = new DBVarChar(APIData.RoleID),
                            RoleNM = systemRoleNMDic[APIData.RoleID],
                            ModifyType = new DBChar(Mongo_BaseAP.EnumModifyType.I.ToString()),
                            ModifyTypeNM = modifyTypeDic[Mongo_BaseAP.EnumModifyType.I.ToString()]
                        }
                    };

                    RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_USER_SYSTEM_ROLE_APPLY, Mongo_BaseAP.EnumModifyType.U, apiNo, updUserID, execSysID, ipAddress, para);
                }

                if (APIData.IsOverride)
                {
                    foreach (var userID in (from s in AllUserIDList
                                            where APIData.UserIDList.Exists(e => e == s) == false
                                            select s))
                    {
                        Entity_BaseAP.BasicInfoPara entityBasicInfoPara =
                            new Entity_BaseAP.BasicInfoPara(EnumCultureID.zh_TW.ToString())
                            {
                                ExecSysID = new DBVarChar(APIData.SysID),
                                UserID = new DBVarChar(userID)
                            };

                        Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                            .SelectBasicInfo(entityBasicInfoPara);

                        Mongo_BaseAP.RecordLogUserSystemRoleApplyPara para = new Mongo_BaseAP.RecordLogUserSystemRoleApplyPara();
                        para.UserID = entityBasicInfo.UserID;
                        para.UserNM = entityBasicInfo.UserNM;
                        para.WFNO = new DBVarChar(APIData.ErpWFNo);
                        para.Memo = new DBVarChar(APIData.Memo);
                        para.ModifyList = new List<Mongo_BaseAP.UserSystemRoleModify>
                        {
                            new Mongo_BaseAP.UserSystemRoleModify
                            {
                                SysID = entityBasicInfo.ExecSysID,
                                SysNM = entityBasicInfo.ExecSysNM,
                                RoleID = new DBVarChar(APIData.RoleID),
                                RoleNM = systemRoleNMDic[APIData.RoleID],
                                ModifyType = new DBChar(Mongo_BaseAP.EnumModifyType.D.ToString()),
                                ModifyTypeNM = modifyTypeDic[Mongo_BaseAP.EnumModifyType.D.ToString()]
                            }
                        };

                        RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_USER_SYSTEM_ROLE_APPLY, Mongo_BaseAP.EnumModifyType.U, apiNo, updUserID, execSysID, ipAddress, para);
                    }
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }
        #endregion
    }
}