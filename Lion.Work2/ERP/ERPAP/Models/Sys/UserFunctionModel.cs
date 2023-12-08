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

namespace ERPAP.Models.Sys
{
    public class UserFunctionModel : SysModel
    {
        public class UserRawData
        {
            public string UserID { get; set; }
            public string UserNM { get; set; }
            public string IsDisable { get; set; }
        }

        public class UserFunction
        {
            public string  HasAuth { get; set; }
            public string  SysID { get; set; }
            public string  SysNM { get; set; }
            public string  SysNMID { get; set; }
            public string  FunControllerID { get; set; }
            public string  FunGroupNM { get; set; }
            public string  FunGroupNMID { get; set; }
            public string  FunActionName { get; set; }
            public string  FunNM { get; set; }
            public string  FunNMID { get; set; }
            public string  UpdUserID { get; set; }
            public string  UpdUserNM { get; set; }
            public DateTime  UpdDT { get; set; }
        }

        public enum Field
        {
            SysID,
            FunControllerID, FunActionName
        }

        public EnumCultureID CurrentCultureID { get; set; }

        public string UserID { get; set; }

        public string IsDisable { get; set; }

        [Required]
        public string SysID { get; set; }

        public string QuerySysID { get; set; }

        [Required]
        public string FunControllerID { get; set; }

        [Required]
        public string FunActionName { get; set; }

        [Required]
        [StringLength(8)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string ErpWFNo { get; set; }

        [StringLength(1000)]
        public string Memo { get; set; }
        
        public void FormReset()
        {
            ErpWFNo = null;
            Memo = null;
        }

        UserRawData _entityUserRawData;
        public UserRawData EntityUserRawData { get { return _entityUserRawData; } }

        public async Task<bool> GetUserRawData()
        {
            try
            {
                string userID = string.IsNullOrWhiteSpace(UserID) ? null : UserID;

                string apiUrl = API.UserSystem.QueryUserRawData(userID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    UserRawData = (UserRawData)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                _entityUserRawData = responseObj.UserRawData;

                if (_entityUserRawData.UserID != null)
                {
                    this.IsDisable = _entityUserRawData.IsDisable;
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<UserFunction> _entityUserFunctionList;
        public List<UserFunction> EntityUserFunctionList { get { return _entityUserFunctionList; } }

        public async Task<bool> GetUserFunctionList(string updUserID, EnumCultureID cultureID)
        {
            try
            {
                string userID = string.IsNullOrWhiteSpace(UserID) ? null : UserID;

                string apiUrl = API.UserFunction.QueryUserFunctions(userID, updUserID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    UserFunctionList = (List<UserFunction>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);
                _entityUserFunctionList = responseObj.UserFunctionList;

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetEditUserFunctionResult(List<EntityUserFunction.SystemUserFunctionValue> systemUserFunctionValueList, string updUserID, EnumCultureID cultureID)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                var systemUserFunctionList = _GetAuthUserFunctionList(systemUserFunctionValueList).Select(x=>x.GetValue()).Select(x => x.Split('|')).ToList().Select(y => new
                {
                    SysID = y[0],
                    FunControllerID = y[1],
                    FunActionName = y[2]
                }).ToList(); ;

                var paraJsonStr = Common.GetJsonSerializeObject(
                    new
                    {
                        UserID = string.IsNullOrWhiteSpace(UserID) ? null : UserID,
                        IsDisable = string.IsNullOrWhiteSpace(IsDisable) ? EnumYN.N.ToString() : IsDisable,
                        FunctionList = systemUserFunctionList,
                        ErpWFNo,
                        Memo,
                        UpdUserID = updUserID
                    });

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.UserFunction.EditUserFunction(updUserID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public EntityEventPara.SysUserFunctionEdit GetEventParaSysUserFunctionEditEntity(string sysID, List<EntityUserFunction.SystemUserFunctionValue> systemUserFunctionValueList)
        {
            try
            {
                EntityEventPara.SysUserFunctionEdit entityEventParaUserFunctionEdit = new EntityEventPara.SysUserFunctionEdit()
                {
                    TargetSysIDList = new List<DBVarChar>() { new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID)) },
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID)),
                    FunctionList = new List<DBVarChar>()
                };

                if (systemUserFunctionValueList != null && systemUserFunctionValueList.Count > 0)
                {
                    foreach (EntityUserFunction.SystemUserFunctionValue systemUserFunctionValue in systemUserFunctionValueList)
                    {
                        if (systemUserFunctionValue.GetSysID().IsNull() == false &&
                            systemUserFunctionValue.GetFunControllerID().IsNull() == false &&
                            systemUserFunctionValue.GetFunActionName().IsNull() == false)
                        {
                            entityEventParaUserFunctionEdit.FunctionList.Add(new DBVarChar(string.Format("{0}|{1}|{2}",
                                                                                                         systemUserFunctionValue.SysID,
                                                                                                         systemUserFunctionValue.FunControllerID,
                                                                                                         systemUserFunctionValue.FunActionName)));
                        }
                    }
                }

                return entityEventParaUserFunctionEdit;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
        }

        public string GenerateUserMenuXML(EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.UserID))
                    return null;

                Entity_BaseAP.UserMenuFunPara para = new Entity_BaseAP.UserMenuFunPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(UserID)
                };

                List<Entity_BaseAP.UserMenuFun> userFunMenuList = new Entity_BaseAP(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectUserMenuFunList(para);

                var userMenu = LionTech.Entity.ERP.Utility.GetUserMenu(userFunMenuList, UserID);
                var xml = LionTech.Entity.ERP.Utility.StringToXmlDocument(userMenu.SerializeToXml());
                return xml.OuterXml;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
        }

        #region - 記錄使用者功能異動 -
        /// <summary>
        /// 記錄使用者功能異動
        /// </summary>
        /// <param name="cultureID"></param>
        /// <param name="updUserID"></param>
        /// <param name="ipAddress"></param>
        /// <param name="systemUserFunctionValueList"></param>
        public async Task RecordLogUserFunApply(EnumCultureID cultureID, string updUserID, string ipAddress, List<EntityUserFunction.SystemUserFunctionValue> systemUserFunctionValueList)
        {
            try
            {
                var modifyList = await _GetUserFunModifyList(updUserID, cultureID, systemUserFunctionValueList);

                if (modifyList.Any())
                {
                    Entity_BaseAP.BasicInfoPara basicInfoPara = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                    {
                        UserID = new DBVarChar((string.IsNullOrWhiteSpace(UserID) ? null : UserID)),
                        UpdUserID = new DBVarChar(null),
                        ExecSysID = new DBVarChar(null)
                    };

                    Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                        .SelectBasicInfo(basicInfoPara);

                    Mongo_BaseAP.RecordLogUserFunApplyPara para = new Mongo_BaseAP.RecordLogUserFunApplyPara();
                    para.UserID = new DBVarChar(UserID);
                    para.UserNM = entityBasicInfo.UserNM;
                    para.WFNO = new DBVarChar(ErpWFNo);
                    para.Memo = new DBVarChar(Memo);
                    para.ModifyList = modifyList;

                    RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_USER_FUN_APPLY, Mongo_BaseAP.EnumModifyType.U, updUserID, ipAddress, cultureID, para);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }
        #endregion

        #region - 取得已授權使用者功能清單 -
        /// <summary>
        /// 取得已授權使用者功能清單
        /// </summary>
        /// <param name="systemUserFunctionValueList"></param>
        /// <returns></returns>
        private List<DBVarChar> _GetAuthUserFunctionList(List<EntityUserFunction.SystemUserFunctionValue> systemUserFunctionValueList)
        {
            List<DBVarChar> functionList = new List<DBVarChar>();

            List<DBVarChar> authUserFunctionList =
                (from fun in systemUserFunctionValueList
                 join sys in EntityUserSystemSysIDList
                     on new
                     {
                         fun.SysID
                     }
                     equals new
                     {
                         SysID = sys.SysID
                     }
                 where fun.GetSysID().IsNull() == false &&
                       fun.GetFunControllerID().IsNull() == false &&
                       fun.GetFunActionName().IsNull() == false
                 select new DBVarChar(
                     (string.Format("{0}|{1}|{2}",
                         fun.SysID,
                         fun.FunControllerID,
                         fun.FunActionName)))).ToList();

            functionList.AddRange(
                authUserFunctionList
                    .Concat((from n in EntityUserFunctionList
                             where n.HasAuth == EnumYN.N.ToString()
                             select new DBVarChar(string.Format("{0}|{1}|{2}",
                                 n.SysID,
                                 n.FunControllerID,
                                 n.FunActionName)))));

            return functionList;
        }
        #endregion

        #region - 取得功能異動清單 -
        /// <summary>
        /// 取得功能異動清單
        /// </summary>
        /// <param name="cultureID"></param>
        /// <param name="systemUserFunctionValueList"></param>
        /// <returns></returns>
        private async Task<List<Mongo_BaseAP.UserFunModify>> _GetUserFunModifyList(string userID, EnumCultureID cultureID, List<EntityUserFunction.SystemUserFunctionValue> systemUserFunctionValueList)
        {
            var result = new List<Mongo_BaseAP.UserFunModify>();

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

            systemUserFunctionValueList = (from s in systemUserFunctionValueList
                                           where string.IsNullOrWhiteSpace(s.SysID) == false
                                           select s).ToList();

            if (systemUserFunctionValueList.Any() == false)
            {
                result.AddRange((from s in EntityUserFunctionList
                                 select new Mongo_BaseAP.UserFunModify
                                 {
                                     SysID = s.SysID,
                                     SysNM = s.SysNM,
                                     FunControllerID = s.FunControllerID,
                                     FunControllerNM = s.FunGroupNM,
                                     FunActionNM = s.FunActionName,
                                     FunNM = s.FunNM,
                                     ModifyType = new DBChar(Mongo_BaseAP.EnumModifyType.D),
                                     ModifyTypeNM = modifyTypeDic[Mongo_BaseAP.EnumModifyType.D.ToString()]
                                 }).ToList());
            }
            else
            {
                var paralist = systemUserFunctionValueList.Select(f => new
                {
                    SysID = f.SysID,
                    FunControllerID = f.FunControllerID,
                    FunActionName = f.FunActionName,
                }).ToList();

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                var paraJsonStr = Common.GetJsonSerializeObject(paralist);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.SystemFunAssign.QueryFunRawDatas(userID, cultureID.ToString().ToUpper());
                var response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                var responseObj = new
                {
                    SysFunNMInfo = (List<SysFunRawData>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                result.AddRange(
                    (from s in EntityUserFunctionList
                     where systemUserFunctionValueList.Exists(u => u.SysID == s.SysID &&
                                                                   u.FunControllerID == s.FunControllerID &&
                                                                   u.FunActionName == s.FunActionName) == false
                     select new Mongo_BaseAP.UserFunModify
                     {
                         SysID = s.SysID,
                         SysNM = s.SysNM,
                         FunControllerID = s.FunControllerID,
                         FunControllerNM = s.FunGroupNM,
                         FunActionNM = s.FunActionName,
                         FunNM = s.FunNM,
                         ModifyType = new DBChar(Mongo_BaseAP.EnumModifyType.D),
                         ModifyTypeNM = modifyTypeDic[Mongo_BaseAP.EnumModifyType.D.ToString()]
                     }).Concat(
                         (from f in systemUserFunctionValueList
                          join sysFunRawData in responseObj.SysFunNMInfo
                              on new
                              {
                                  f.SysID,
                                  f.FunControllerID,
                                  f.FunActionName
                              } equals new
                              {
                                  SysID = sysFunRawData.SysID,
                                  FunControllerID = sysFunRawData.FunControllerID,
                                  FunActionName = sysFunRawData.FunActionID
                              }
                          where EntityUserFunctionList.Exists(fun => fun.SysID == f.SysID &&
                                                                     fun.FunControllerID == f.FunControllerID &&
                                                                     fun.FunActionName == f.FunActionName) == false
                          select new Mongo_BaseAP.UserFunModify
                          {
                              SysID = new DBVarChar(f.SysID),
                              SysNM = sysFunRawData.SysNM,
                              FunControllerID = new DBVarChar(f.FunControllerID),
                              FunControllerNM = sysFunRawData.FunControllerNM,
                              FunActionNM = new DBVarChar(f.FunActionName),
                              FunNM = sysFunRawData.FunActionNM,
                              ModifyType = new DBChar(Mongo_BaseAP.EnumModifyType.I),
                              ModifyTypeNM = modifyTypeDic[Mongo_BaseAP.EnumModifyType.I.ToString()]
                          })
                        ).ToList());
            }

            return result;
        }
        #endregion
    }
}