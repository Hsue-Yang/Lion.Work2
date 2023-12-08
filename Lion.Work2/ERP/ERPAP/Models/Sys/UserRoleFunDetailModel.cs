using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.EDIService;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static ERPAP.Models.Sys.UserSystemDetailModel;

namespace ERPAP.Models.Sys
{
    public class UserRoleFunDetailModel : SysModel
    {
        #region - Constructor -
        public UserRoleFunDetailModel()
        {
        }
        #endregion

        #region - Property -
        public string SysID { get; set; }

        public string UserID { get; set; }

        public string UserNM { get; set; }

        public string RoleGroupID { get; set; }

        public List<string> HasRole { get; set; }

        [Required]
        [StringLength(8)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string ErpWFNo { get; set; }

        [StringLength(1000)]
        public string Memo { get; set; }

        public class UserMain
        {
            public string UserID { get; set; }
            public string UserNM { get; set; }
            public string RoleGroupID { get; set; }
            public string IsDisable { get; set; }
        }

        public UserMain UserMainInfo { get; set; }

        public class SystemRoleGroupCollect
        {
            public string SysID { get; set; }
            public string RoleID { get; set; }
            public string Remark { get; set; }
        }

        public List<SystemRoleGroupCollect> SystemRoleGroupCollectList { get; set; }

        public class UserRoleFunDetailParaList
        {
            public UserRoleFunDetailPara userRoleFunDetailPara { get; set; }
            public List<AuthUserSystemRole> userRoleFunDetailParaList { get; set; }
        }

        public class UserRoleFunDetailPara
        {
            public string UserID { get; set; }
            public string IpAddress { get; set; }
            public string ExecSysID { get; set; }
            public string ErpWFNO { get; set; }
            public string Memo { get; set; }
            public string RoleGroupID { get; set; }
            public string ApiNO { get; set; }
            public string IsDisable { get; set; }
            public string SysID { get; set; }
            public string RoleID { get; set; }
            public string UpdUserID { get; set; }
            public DateTime UpdDT { get; set; }
        }

        public class AuthUserSystemRole
        {
            public string SysID { get; set; }
            public string RoleID { get; set; }
            public string UpdUserID { get; set; }
            public DateTime UpdDT { get; set; }
        }

        public class UserSystemRole
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string SysNMID { get; set; }
            public string RoleID { get; set; }
            public string RoleNMID { get; set; }
            public string RoleNM { get; set; }
            public string HasRole { get; set; }
            public int HasAuth { get; set; }
            public string UpdUserID { get; set; }
            public DateTime UpdDT { get; set; }
        }

        public class UserRawData
        {
            public string UserID { get; set; }
            public string UserNM { get; set; }
            public string UnitID { get; set; }
            public string UnitNM { get; set; }
            public string IsLeft { get; set; }
            public string IsDisable { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }

        public List<UserSystemRole> UserSystemRoleList { get; set; }

        #endregion

        #region - 表單初始 -
        /// <summary>
        /// 表單初始
        /// </summary>
        public void FormReset()
        {
            HasRole = new List<string>();
        }
        #endregion

        #region - 取得使用者主檔 -
        /// <summary>
        /// 取得使用者主檔
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetUserMainInfo()
        {
            try
            {
                string apiUrl = API.UserRoleFun.QueryUserMainInfo(string.IsNullOrWhiteSpace(UserID) ? null : UserID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new UserMain());

                UserMainInfo = responseObj;

                return responseObj != null;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 取得應用系統角色群組清單 -
        /// <summary>
        /// 取得應用系統角色群組清單
        /// </summary>
        /// <param name="roleGroupID"></param>
        /// <returns></returns>
        //public async Task<bool> GetSysSystemRoleGroupCollectList(string roleGroupID)
        //{
        //    try
        //    {
        //        string apiUrl = API.UserRoleFun.QuerySystemRoleGroupCollects(string.IsNullOrWhiteSpace(roleGroupID) ? null : roleGroupID);
        //        string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

        //        var responseObj = Common.GetJsonDeserializeAnonymousType(response, new List<SystemRoleGroupCollect>());

        //        SystemRoleGroupCollectList = responseObj;

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        OnException(ex);
        //    }
        //    return false;
        //}
        #endregion

        #region - 取得使用者角色清單 -
        /// <summary>
        /// 取得使用者角色清單
        /// </summary>
        /// <param name="updUserID"></param>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetUserSystemRoleList(string updUserID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.UserRoleFun.QueryUserSystemRoles(
                    string.IsNullOrWhiteSpace(UserID) ? null : UserID,
                    string.IsNullOrWhiteSpace(updUserID) ? null : updUserID,
                    cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new List<UserSystemRole>());

                UserSystemRoleList = responseObj;
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 編輯使用者角色 -
        /// <summary>
        /// 編輯使用者角色
        /// </summary>
        /// <param name="updUserID"></param>
        /// <param name="ipAddress"></param>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetEditUserSystemRoleResult(string updUserID, string ipAddress, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.UserSystem.QueryUserRawData(string.IsNullOrWhiteSpace(UserID) ? null : UserID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new UserRawData());

                bool isLeft = (responseObj.IsLeft == EnumYN.Y.ToString() ? true : false);

                UserRoleFunDetailParaList para = new UserRoleFunDetailParaList()
                {
                    userRoleFunDetailPara =  new UserRoleFunDetailPara()
                    {
                        UserID = (string.IsNullOrWhiteSpace(UserID) ? null : UserID),
                        RoleGroupID = (string.IsNullOrWhiteSpace(RoleGroupID) ? null : RoleGroupID),
                        IsDisable = (isLeft ? EnumYN.Y.ToString() : EnumYN.N.ToString()),
                        ErpWFNO = ErpWFNo,
                        Memo = string.IsNullOrWhiteSpace(Memo) ? null : Memo,
                        ApiNO = null,
                        IpAddress = ipAddress,
                        ExecSysID = EnumSystemID.ERPAP.ToString(),
                        UpdUserID = (string.IsNullOrWhiteSpace(updUserID) ? null : updUserID),
                    },
                    userRoleFunDetailParaList = _GetAuthUserSystemRoleList(updUserID, cultureID)
                };

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                var paraJsonStr = Common.GetJsonSerializeObject(para);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                apiUrl = API.UserRoleFun.EditUserSystemRole(updUserID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                apiUrl = API.UserRoleFun.QueryUserMainInfo(UserID);
                var userMain = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var userMainObj = Common.GetJsonDeserializeAnonymousType(response, new UserMain());
                bool isDisable = (userMainObj.IsDisable == EnumYN.Y.ToString() ? true : false);

                GetRecordUserAccessResult(UserID, null,
                    null, (isDisable ? EnumYN.Y.ToString() : EnumYN.N.ToString()),
                    updUserID, ipAddress, cultureID);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 取得事件訂閱參數-使用者角色 -
        /// <summary>
        /// 取得事件訂閱參數-使用者角色
        /// </summary>
        /// <param name="sysID"></param>
        /// <returns></returns>
        public EntityEventPara.SysUserSystemRoleEdit GetEventParaSysUserSystemRoleEditEntity(string sysID)
        {
            try
            {
                EntityEventPara.SysUserSystemRoleEdit entityEventParaUserSystemRoleEdit = new EntityEventPara.SysUserSystemRoleEdit
                {
                    TargetSysIDList = new List<DBVarChar> { new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID)) },
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(UserID) ? null : UserID)),
                    RoleGroupID = new DBVarChar((string.IsNullOrWhiteSpace(RoleGroupID) ? null : RoleGroupID)),
                    RoleIDList = new List<DBVarChar>()
                };

                if (HasRole != null &&
                    HasRole.Count > 0)
                {
                    foreach (string roleString in HasRole)
                    {
                        if (roleString.Split('|')[0] == sysID)
                        {
                            entityEventParaUserSystemRoleEdit.RoleIDList.Add(new DBVarChar(roleString.Split('|')[1]));
                        }
                    }
                }

                return entityEventParaUserSystemRoleEdit;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return null;
        }
        #endregion

        #region - 生成使用者Menu -
        /// <summary>
        /// 生成使用者Menu
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public string GenerateUserMenuXML(EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UserID))
                {
                    return null;
                }

                Entity_BaseAP.UserMenuFunPara para = new Entity_BaseAP.UserMenuFunPara(cultureID.ToString().ToUpper())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(UserID) ? null : UserID))
                };

                List<Entity_BaseAP.UserMenuFun> userFunMenuList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectUserMenuFunList(para);

                var userMenu = LionTech.Entity.ERP.Utility.GetUserMenu(userFunMenuList, UserID);
                var xml = LionTech.Entity.ERP.Utility.StringToXmlDocument(userMenu.SerializeToXml());
                return xml.OuterXml;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return null;
        }
        #endregion

        #region - 紀錄使用者角色異動 -
        /// <summary>
        /// 紀錄使用者角色異動
        /// </summary>
        /// <param name="cultureID"></param>
        /// <param name="updUserID"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool RecordLogUserSystemRoleApply(EnumCultureID cultureID, string updUserID, string ipAddress)
        {
            try
            {
                var modifyList = _GetUserSystemRoleModifyList(cultureID);

                if (modifyList.Any())
                {
                    Entity_BaseAP.BasicInfoPara basicInfoPara = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                    {
                        UserID = new DBVarChar((string.IsNullOrWhiteSpace(UserID) ? null : UserID)),
                        UpdUserID = new DBVarChar(null),
                        ExecSysID = new DBVarChar(null)
                    };

                    Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                        .SelectBasicInfo(basicInfoPara);

                    Mongo_BaseAP.RecordLogUserSystemRoleApplyPara para = new Mongo_BaseAP.RecordLogUserSystemRoleApplyPara();
                    para.UserID = new DBVarChar(UserID);
                    para.UserNM = entityBasicInfo.UserNM;
                    para.WFNO = new DBVarChar(string.IsNullOrWhiteSpace(ErpWFNo) ? null : ErpWFNo);
                    para.Memo = new DBVarChar(string.IsNullOrWhiteSpace(Memo) ? null : Memo);
                    para.ModifyList = modifyList;

                    RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_USER_SYSTEM_ROLE_APPLY, Mongo_BaseAP.EnumModifyType.U, updUserID, ipAddress, cultureID, para);
                }

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 取得已授權使用者角色清單 -
        /// <summary>
        /// 取得已授權使用者角色清單
        /// </summary>
        /// <param name="updUserID"></param>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        private List<AuthUserSystemRole> _GetAuthUserSystemRoleList(string updUserID, EnumCultureID cultureID)
        {
            List<AuthUserSystemRole> paraList = new List<AuthUserSystemRole>();

            if (HasRole != null &&
                HasRole.Count > 0)
            {
                paraList.AddRange(HasRole.Select(roleString => new AuthUserSystemRole()
                {
                    SysID = (string.IsNullOrWhiteSpace(roleString.Split('|')[0]) ? null : roleString.Split('|')[0]),
                    RoleID = (string.IsNullOrWhiteSpace(roleString.Split('|')[1]) ? null : roleString.Split('|')[1]),
                    UpdUserID = updUserID,
                    UpdDT = DateTime.Now
                }));
            }

            List<AuthUserSystemRole> unAuthUserSystemRoleList =
                (from n in UserSystemRoleList
                 where n.HasAuth == 0 && n.HasRole == EnumYN.Y.ToString()
                 select new AuthUserSystemRole()
                 {
                     SysID = n.SysID,
                     RoleID = n.RoleID,
                     UpdUserID = n.UpdUserID,
                     UpdDT = n.UpdDT
                 }).ToList();

            paraList =
                (from s in paraList
                 where unAuthUserSystemRoleList.Exists(e => e.SysID == s.SysID) == false
                 select s)
                    .Union(unAuthUserSystemRoleList)
                    .ToList();

            HasRole = paraList.Select(s => string.Format("{0}|{1}", s.SysID, s.RoleID)).ToList();
            return paraList;
        }
        #endregion

        #region - 取得使用者角色異動資料 -
        /// <summary>
        /// 取得使用者角色異動資料
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        private List<Mongo_BaseAP.UserSystemRoleModify> _GetUserSystemRoleModifyList(EnumCultureID cultureID)
        {
            var result = new List<Mongo_BaseAP.UserSystemRoleModify>();

            if (HasRole == null ||
                HasRole.Any() == false)
            {
                return result;
            }

            var unAuthSysIDList =
                (from n in UserSystemRoleList
                 where n.HasAuth != 1 && n.HasRole == EnumYN.Y.ToString()
                 select new
                 {
                     SysID = n.SysID
                 }).Distinct().ToList();

            var authSysRoleList =
                (from sysRole in HasRole
                 let sysID = sysRole.Split('|')[0]
                 let roleID = sysRole.Split('|')[1]
                 where unAuthSysIDList.Exists(e => e.SysID == sysID) == false
                 select new
                 {
                     SysID = sysID,
                     RoleID = roleID
                 }).ToList();

            Entity_BaseAP.CMCodePara codePara =
                new Entity_BaseAP.CMCodePara
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNM,
                    CodeKind = Entity_BaseAP.EnumCMCodeKind.ModifyType,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(cultureID)
                };

            var modifyTypeDic = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                .SelectCMCodeList(codePara)
                .ToDictionary(key => key.CodeID.GetValue(), val => val.CodeNM);

            result.AddRange(
                (from userSystemRole in UserSystemRoleList.Where(userSystemRole => userSystemRole.HasAuth == 1)
                 let modifyType =
                     (userSystemRole.HasRole == EnumYN.N.ToString() &&
                      authSysRoleList.Exists(u => userSystemRole.SysID == u.SysID && userSystemRole.RoleID == u.RoleID))
                         ? Mongo_BaseAP.EnumModifyType.I
                         : (userSystemRole.HasRole == EnumYN.Y.ToString() &&
                            authSysRoleList.Exists(u => userSystemRole.SysID == u.SysID && userSystemRole.RoleID == u.RoleID) == false)
                             ? Mongo_BaseAP.EnumModifyType.D
                             : Mongo_BaseAP.EnumModifyType.U
                 where modifyType == Mongo_BaseAP.EnumModifyType.I || modifyType == Mongo_BaseAP.EnumModifyType.D
                 select new Mongo_BaseAP.UserSystemRoleModify
                 {
                     SysID = userSystemRole.SysID,
                     SysNM = userSystemRole.SysNM,
                     RoleID = userSystemRole.RoleID,
                     RoleNM = userSystemRole.RoleNM,
                     ModifyType = new DBChar(modifyType),
                     ModifyTypeNM = modifyTypeDic[modifyType.ToString()]
                 }));

            return result;
        }
        #endregion
    }
}