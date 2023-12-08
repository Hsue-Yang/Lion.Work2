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
using static ERPAP.Models.Sys.UserFunctionModel;

namespace ERPAP.Models.Sys
{
    public class SystemFunAssignModel : SysModel
    {
        public EnumCultureID CurrentCultureID { get; set; }

        public class SystemFunAssign
        {
            public string UserID { get; set; }
            public string UserNM { get; set; }
        }

        public class UserRawData
        {
            public string UserID { get; set; }
            public string UserNM { get; set; }
            public string RoleGroupID { get; set; }
            public string IsDisable { get; set; }
        }

        [Required]
        public string SysID { get; set; }

        public string _SubSysID;

        public string SubSysID
        {
            get
            {
                if (_SubSysID == EnumSystemID.ERPAP.ToString())
                {
                    _SubSysID = string.Empty;
                }
                return _SubSysID;
            }
            set
            {
                _SubSysID = value;
            }
        }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string FunControllerID { get; set; }

        [Required]
        [StringLength(40)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string FunActionName { get; set; }

        public string FunType { get; set; }

        [Required]
        public string FunNM { get; set; }

        [Required]
        [StringLength(8)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string ErpWFNo { get; set; }

        [StringLength(1000)]
        public string Memo { get; set; }

        #region - Private -
        private string _sysNM { get; set; }
        private string _funGroupNM { get; set; }
        #endregion

        public void FormReset()
        {
            ErpWFNo = null;
            Memo = null;
        }

        List<SystemFunAssign> _entitySystemFunAssignList;
        public List<SystemFunAssign> EntitySystemFunAssignList { get { return _entitySystemFunAssignList; } }
        public async Task<bool> GetSystemFunAssignList(EnumCultureID cultureID)
        {
            try
            {
                string sysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                string funControllerID = string.IsNullOrWhiteSpace(FunControllerID) ? null : FunControllerID;
                string funActionName = string.IsNullOrWhiteSpace(FunActionName) ? null : FunActionName;

                string apiUrl = API.SystemFunAssign.QuerySystemFunAssigns(sysID, funControllerID, funActionName);
                var response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                var responseObj = new
                {
                    SystemFunAssigns = (List<SystemFunAssign>)null
                };
                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj.SystemFunAssigns != null)
                {
                    _entitySystemFunAssignList = responseObj.SystemFunAssigns;
                }
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetEditSystemFunAssignResult(string userID, List<EntitySystemFunAssign.SystemFunAssignValue> systemFunAssignValueList, EnumCultureID cultureID)
        {
            try
            {
                List<string> userIDList = new List<string>();

                if (systemFunAssignValueList != null && systemFunAssignValueList.Count > 0)
                {
                    foreach (var systemFunAssignValue in systemFunAssignValueList)
                    {
                        if (!string.IsNullOrEmpty(systemFunAssignValue.UserID))
                        {
                            userIDList.Add(systemFunAssignValue.UserID);
                        }
                    }
                }

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                    FunControllerID = string.IsNullOrWhiteSpace(FunControllerID) ? null : FunControllerID,
                    FunActionName = string.IsNullOrWhiteSpace(FunActionName) ? null : FunActionName,
                    UserIDList = userIDList,
                    UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID,
                    ErpWFNO = ErpWFNo,
                    Memo
                });
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.SystemFunAssign.EditSystemFunAssign(userID);

                var result = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public EntityEventPara.SysSystemFunAssignEdit GetEventParaSysSystemFunAssignEditEntity(List<EntitySystemFunAssign.SystemFunAssignValue> systemFunAssignValueList)
        {
            try
            {
                EntityEventPara.SysSystemFunAssignEdit entityEventParaSystemFunAssignEdit = new EntityEventPara.SysSystemFunAssignEdit()
                {
                    TargetSysIDList = new List<DBVarChar>() { new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)) },
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.SysID) ? null : this.SysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.FunControllerID) ? null : this.FunControllerID)),
                    FunActionName = new DBVarChar((string.IsNullOrWhiteSpace(this.FunActionName) ? null : this.FunActionName)),
                    UserIDList = new List<DBVarChar>()
                };

                if (systemFunAssignValueList != null && systemFunAssignValueList.Count > 0)
                {
                    foreach (EntitySystemFunAssign.SystemFunAssignValue systemFunAssignValue in systemFunAssignValueList)
                    {
                        if (systemFunAssignValue.GetUserID().IsNull() == false)
                        {
                            entityEventParaSystemFunAssignEdit.UserIDList.Add(new DBVarChar((string.IsNullOrWhiteSpace(systemFunAssignValue.UserID) ? null : systemFunAssignValue.UserID)));
                        }
                    }
                }

                return entityEventParaSystemFunAssignEdit;
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
        /// <param name="systemFunAssignValueList"></param>
        public async Task RecordLogUserFunApply(EnumCultureID cultureID, string updUserID, string ipAddress, List<EntitySystemFunAssign.SystemFunAssignValue> systemFunAssignValueList)
        {
            try
            {
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

                foreach (var paras in await _GetLogUserFunApplyList(systemFunAssignValueList, modifyTypeDic))
                {
                    RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_USER_FUN_APPLY, Mongo_BaseAP.EnumModifyType.U, updUserID, ipAddress, cultureID, paras);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }
        #endregion

        #region - 取得功能異動參數 -
        /// <summary>
        /// 取得功能異動參數
        /// </summary>
        /// <param name="systemFunAssignValueList"></param>
        /// <param name="modifyTypeDic"></param>
        /// <returns></returns>
        private async Task<List<Mongo_BaseAP.RecordLogUserFunApplyPara>> _GetLogUserFunApplyList(List<EntitySystemFunAssign.SystemFunAssignValue> systemFunAssignValueList, Dictionary<string, DBNVarChar> modifyTypeDic)
        {
            var result = new List<Mongo_BaseAP.RecordLogUserFunApplyPara>();

            Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

            var UserIDList = systemFunAssignValueList.Where(s => !string.IsNullOrEmpty(s.UserID)).Select(u => new { UserID = u.UserID }).ToList();

            var paraJsonStr = Common.GetJsonSerializeObject(UserIDList);
            var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
            string apiUrl = API.SystemFunAssign.QueryUserRawDatas();
            var response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

            var responseObj = new
            {
                UserRawDataList = (List<UserRawData>)null
            };

            responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

            result.AddRange(
                (from s in EntitySystemFunAssignList
                 where systemFunAssignValueList.Exists(u => s.UserID == u.UserID) == false
                 select new Mongo_BaseAP.RecordLogUserFunApplyPara
                 {
                     UserID = s.UserID,
                     UserNM = s.UserNM,
                     WFNO = new DBVarChar(string.IsNullOrWhiteSpace(ErpWFNo) ? null : ErpWFNo),
                     Memo = new DBVarChar(string.IsNullOrWhiteSpace(Memo) ? null : Memo),
                     ModifyList = new List<Mongo_BaseAP.UserFunModify>
                     {
                         new Mongo_BaseAP.UserFunModify
                         {
                             SysID = new DBVarChar(SysID),
                             SysNM = new DBNVarChar(_sysNM),
                             FunControllerID = new DBVarChar(FunControllerID),
                             FunControllerNM = new DBNVarChar(_funGroupNM),
                             FunActionNM = new DBVarChar(FunActionName),
                             FunNM = new DBNVarChar(FunNM),
                             ModifyType = new DBChar(Mongo_BaseAP.EnumModifyType.D),
                             ModifyTypeNM = modifyTypeDic[Mongo_BaseAP.EnumModifyType.D.ToString()]
                         }
                     }
                 }).Concat(
                     (from d in systemFunAssignValueList
                      join userRawData in responseObj.UserRawDataList
                          on d.UserID equals userRawData.UserID
                      where EntitySystemFunAssignList.Exists(u => d.UserID == u.UserID) == false
                      select new Mongo_BaseAP.RecordLogUserFunApplyPara
                      {
                          UserID = new DBVarChar(d.UserID),
                          UserNM = userRawData.UserNM,
                          WFNO = new DBVarChar(string.IsNullOrWhiteSpace(ErpWFNo) ? null : ErpWFNo),
                          Memo = new DBVarChar(string.IsNullOrWhiteSpace(Memo) ? null : Memo),
                          ModifyList = new List<Mongo_BaseAP.UserFunModify>
                          {
                              new Mongo_BaseAP.UserFunModify
                              {
                                  SysID = new DBVarChar(SysID),
                                  SysNM = new DBNVarChar(_sysNM),
                                  FunControllerID = new DBVarChar(FunControllerID),
                                  FunControllerNM = new DBNVarChar(_funGroupNM),
                                  FunActionNM = new DBVarChar(FunActionName),
                                  FunNM = new DBNVarChar(FunNM),
                                  ModifyType = new DBChar(Mongo_BaseAP.EnumModifyType.I),
                                  ModifyTypeNM = modifyTypeDic[Mongo_BaseAP.EnumModifyType.I.ToString()]
                              }
                          }
                      })).ToList());

            return result;
        }
        #endregion

        #region - 取得系統功能名稱 -
        /// <summary>
        /// 取得系統功能名稱
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemFunInfo(string userID, EnumCultureID cultureID)
        {
            try
            {
                var paralist = new List<UserFunction>()
                {
                    new UserFunction
                    {
                        SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                        FunControllerID = string.IsNullOrWhiteSpace(FunControllerID) ? null : FunControllerID,
                        FunActionName = string.IsNullOrWhiteSpace(FunActionName) ? null : FunActionName
                    }
                };

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

                var result = responseObj.SysFunNMInfo.SingleOrDefault();

                if (result != null)
                {
                    _sysNM = result.SysNM;
                    _funGroupNM = result.FunControllerNM;
                    FunNM = result.FunActionNM;
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
    }
}