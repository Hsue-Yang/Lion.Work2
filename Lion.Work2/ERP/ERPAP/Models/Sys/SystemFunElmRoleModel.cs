// 新增日期：2018-01-12
// 新增人員：廖先駿
// 新增內容：元素權限角色設定
// ---------------------------------------------------

using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemFunElmRoleModel : SysModel, IValidatableObject
    {
        #region - Definitions -
        public enum EnumDisplaySts
        {
            DISPLAY = 1,
            READ_ONLY = 2,
            MASKING = 3,
            HIDE = 4
        }

        public class RoleInfo
        {
            public string RoleID { get; set; }
            public string RoleNMID { get; set; }
        }
        #endregion

        #region - Constructor -
        public SystemFunElmRoleModel()
        {
            _entityMongo = new MongoSystemFunElmRole(ConnectionStringMSERP, ProviderNameMSERP);
        }
        #endregion

        #region - Property -
        public string SysID { set; get; }
        public string FunControllerID { set; get; }
        public string FunActionName { set; get; }
        public string FunElmID { get; set; }
        public Dictionary<EnumDisplaySts, List<RoleInfo>> FunElmRoleDictionary { get; set; }
        public IEnumerable<SystemRole> SystemRoleIDList { get; private set; }
        public bool IsSaveSuccess;

        private Dictionary<string, string> _funElmDisplayTypeDic;

        public Dictionary<string, string> FunElmDisplayTypeDic
        {
            get
            {
                if (_funElmDisplayTypeDic == null &&
                    CMCodeDictionary != null)
                {
                    _funElmDisplayTypeDic = (from s in CMCodeDictionary[Entity_BaseAP.EnumCMCodeKind.ElmDisplayType].Cast<Entity_BaseAP.CMCode>()
                                             select new
                                             {
                                                 SourceType = s.CodeID.GetValue(),
                                                 SourceTypeNM = s.CodeNM.GetValue()
                                             }).ToDictionary(k => k.SourceType, v => v.SourceTypeNM);
                }

                return _funElmDisplayTypeDic;
            }
        }
        #endregion

        #region - Private -
        private readonly MongoSystemFunElmRole _entityMongo;
        #endregion

        public string FunGroupNMID;
        public string FunActionNMID;
        public string ElmNMID;
        public string DefaultDisplay;

        public class ElmRoleInfo
        {
            public string RoleID { get; set; }
            public int DispalySts { get; set; }
        }

        public class ElmRoleInfoValue
        {
            public string SYS_ID { get; set; }
            public string ROLE_ID { get; set; }
            public string FUN_CONTROLLER_ID { get; set; }
            public string FUN_ACTION_NAME { get; set; }
            public string ELM_ID { get; set; }
            public int DISPLAY_STS { get; set; }
            public string UPD_USER_ID { get; set; }
        }

        public class SystemFunElmRole
        {
            public string RoleID { get; set; }
            public string RoleNMID { get; set; }
            public int DisplaySts { get; set; }
        }

        #region - Validation -
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsValid == false)
            {
                yield break;
            }

            #region - 驗證系統角色是否重複 -
            if (FunElmRoleDictionary != null)
            {
                var roleCount =
                    from s in FunElmRoleDictionary.SelectMany(sm => sm.Value)
                    where string.IsNullOrWhiteSpace(s.RoleID) == false
                    group s by s.RoleID
                    into g
                    select new
                    {
                        roleID = g.Key,
                        count = g.Count()
                    };

                if (roleCount.Any(w => w.count > 1))
                {
                    yield return new ValidationResult(SysSystemFunElmRole.SystemMsg_RoleIDRepeat_Failure);
                }
            }
            #endregion
        }
        #endregion

        public async Task<bool> GetSystemFunElmInfo(string userID, EnumCultureID cultureID)
        {
            try
            {              
                string apiUrl = API.SysFunElm.QuerySystemFunElmInfo(SysID, userID, FunElmID, FunControllerID, FunActionName, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                var result = Common.GetJsonDeserializeObject<SystemFunElm>(response);

                if (result != null)
                {
                    DefaultDisplay = result.DefaultDisplay;
                    FunGroupNMID = result.FnGroupNMID;
                    FunActionNMID = result.FnNMID;
                    ElmNMID = result.ElmNMID;
                }

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }

        #region - 取得元素權限角色明細 -
        /// <summary>
        /// 取得元素權限角色明細
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemFunElmRoleList(string userID, EnumCultureID cultureID, List<SystemRole> sysRoles)
        {
            try
            {   
                string apiUrl = API.SysFunElm.QuerySystemFunElmRoleList(SysID, userID, FunElmID, FunControllerID, FunActionName, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var elmAuthRoleList = Common.GetJsonDeserializeAnonymousType(response, new List<SystemFunElmRole>());

                FunElmRoleDictionary = new Dictionary<EnumDisplaySts, List<RoleInfo>>();

                foreach (var name in Enum.GetNames(typeof(EnumDisplaySts)))
                {
                    var displaySts = (EnumDisplaySts)Enum.Parse(typeof(EnumDisplaySts), name);
                    if (FunElmRoleDictionary.ContainsKey(displaySts) == false)
                    {
                        FunElmRoleDictionary.Add(displaySts, new List<RoleInfo>());
                    }
                }

                if (elmAuthRoleList != null &&
                elmAuthRoleList.Any())
                {
                    List<string> funElmRoles = elmAuthRoleList.Select(r => r.RoleID).ToList();

                    SystemRoleIDList =
                        sysRoles
                            .Where(s => funElmRoles.Contains(s.RoleID) == false)
                            .ToList();

                    foreach (var elmAuthRole in
                        from elmAuthRole in elmAuthRoleList
                        group elmAuthRole by (EnumDisplaySts)Enum.Parse(typeof(EnumDisplaySts), elmAuthRole.DisplaySts.ToString())
                        into g
                        select g)
                    {
                        FunElmRoleDictionary[elmAuthRole.Key] =
                        (from s in elmAuthRole
                         select new RoleInfo
                         {
                             RoleID = s.RoleID,
                             RoleNMID = s.RoleNMID
                         }).ToList();
                    }
                }
                else
                {
                    SystemRoleIDList = sysRoles;
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

        #region - 編輯元素權限角色 -
        /// <summary>
        /// 編輯元素權限角色
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<bool> EditSystemFunElmRole(string userID)
        {
            try
            {               
                List<ElmRoleInfo> list = GetElmRoleInfoListPara();
                List<ElmRoleInfo> elmRoleInfoList = (list != null && list.Any()) ? list : new List<ElmRoleInfo>();
                List<ElmRoleInfoValue> elmRoleInfoValueList = new List<ElmRoleInfoValue>();

                foreach (var elmRoleValue in elmRoleInfoList)
                {
                    elmRoleInfoValueList.Add(new ElmRoleInfoValue
                    {
                        SYS_ID = SysID,
                        ROLE_ID = elmRoleValue.RoleID,
                        FUN_CONTROLLER_ID = FunControllerID,
                        FUN_ACTION_NAME = FunActionName,
                        ELM_ID = FunElmID,
                        DISPLAY_STS = elmRoleValue.DispalySts,
                        UPD_USER_ID = userID
                    });
                }

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                var paraJsonStr = Common.GetJsonSerializeObject(elmRoleInfoValueList);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.SysFunElm.EditSystemFunElmRole(userID, SysID, FunElmID, FunControllerID, FunActionName);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 紀錄元素權限角色 -
        /// <summary>
        /// 紀錄元素權限角色
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public bool RecordLogSystemRoleFunElm(string userID, EnumCultureID cultureID)
        {
            try
            {
                Entity_BaseAP.BasicInfoPara basicInfoPara = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(null),
                    UpdUserID = new DBVarChar(userID),
                    ExecSysID = new DBVarChar(SysID)
                };

                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectBasicInfo(basicInfoPara);

                var modifyTypeDictionary =
                    (from s in CMCodeDictionary[Entity_BaseAP.EnumCMCodeKind.ModifyType].Cast<Entity_BaseAP.CMCode>()
                     select new
                     {
                         modifyType = s.CodeID.GetValue(),
                         modifyTypeNM = s.CodeNM
                     }).ToDictionary(k => k.modifyType, v => v.modifyTypeNM);

                MongoSystemFunElmRole.SystemRoleFunElmPara para = new MongoSystemFunElmRole.SystemRoleFunElmPara
                {
                    SysID = new DBVarChar(SysID),
                    SysNM = entityBasicInfo.ExecSysNM,
                    ElmID = new DBVarChar(FunElmID),
                    ControllerName = new DBVarChar(FunControllerID),
                    ActionName = new DBVarChar(FunActionName),
                    ElmRoleList = _GetElmRoleLogList(),
                    ModifyType = Mongo_BaseAP.EnumModifyType.U.ToString(),
                    ModifyTypeNM = modifyTypeDictionary[Mongo_BaseAP.EnumModifyType.U.ToString()],
                    UpdUserID = new DBVarChar(userID),
                    UpdUserNM = entityBasicInfo.UpdUserNM,
                    UpdDT = new DBDateTime(DateTime.Now)
                };

                return _entityMongo.RecordLogSystemRoleFunElm(para) == MongoSystemFunElmRole.EnumRecordLogSystemRoleFunElmResult.Success;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 取得元素權限角色紀錄清單 -
        /// <summary>
        /// 取得元素權限角色紀錄清單
        /// </summary>
        /// <returns></returns>
        private List<MongoSystemFunElmRole.ElmRole> _GetElmRoleLogList()
        {
            try
            {
                var roleInfoList = GetElmRoleInfoListPara();

                var elmRoleList = (from s in roleInfoList
                                   group s by s.DispalySts
                                   into elmRole
                                   select new MongoSystemFunElmRole.ElmRole
                                   {
                                       DisplaySts = new DBTinyInt(elmRole.Key),
                                       DisplayNM = new DBNVarChar(FunElmDisplayTypeDic[elmRole.Key.ToString()]),
                                       RoleIDList = elmRole.Select(e => new DBVarChar(e.RoleID)).ToList()
                                   }).ToList();

                return elmRoleList;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return new List<MongoSystemFunElmRole.ElmRole>();
        }
        #endregion

        #region - 取得元素權限角色參數 -
        /// <summary>
        /// 取得元素權限角色參數
        /// </summary>
        /// <returns></returns>
        private List<ElmRoleInfo> GetElmRoleInfoListPara()
        {
            if (FunElmRoleDictionary != null)
            {
                return
                (from funElmRole in FunElmRoleDictionary
                 select (from s in funElmRole.Value
                         where string.IsNullOrWhiteSpace(s.RoleID) == false
                         select new ElmRoleInfo
                         {
                             DispalySts = (int)funElmRole.Key,
                             RoleID = s.RoleID
                         })).SelectMany(sm => sm).ToList();
            }

            return new List<ElmRoleInfo>();
        }
        #endregion
    }
}