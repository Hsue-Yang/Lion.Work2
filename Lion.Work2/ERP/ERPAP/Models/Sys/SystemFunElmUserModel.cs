// 新增日期：2018-01-12
// 新增人員：廖先駿
// 新增內容：元素權限使用者設定
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemFunElmUserModel : SysModel, IValidatableObject
    {
        #region - Definitions -
        public enum EnumDisplaySts
        {
            DISPLAY = 1,
            READ_ONLY = 2,
            MASKING = 3,
            HIDE = 4
        }
        
        public class UserInfo
        {
            public string UserID { get; set; }
            public string UserNMID { get; set; }
        }
        #endregion

        #region - Constructor -
        public SystemFunElmUserModel()
        {
            _entity = new EntitySystemFunElmUser(ConnectionStringSERP, ProviderNameSERP);
            _entityMongo = new MongoSystemFunElmUser(ConnectionStringMSERP, ProviderNameMSERP);
        }
        #endregion

        #region - Property -
        public string SysID { set; get; }
        public string FunControllerID { set; get; }
        public string FunActionName { set; get; }
        public string FunElmID { get; set; }
        public string IsSingleUser { get; set; }

        [RegularExpression(@"^([0-9a-zA-Z]{6},)*([0-9a-zA-Z]{6})$",
            ErrorMessageResourceType = typeof(SysSystemFunElmUser),
            ErrorMessageResourceName = nameof(SysSystemFunElmUser.SystemMsg_UserMemoFormate_Error))]
        [StringLength(1000)]
        public string MultiUserMemo { get; set; }

        [Required]
        [StringLength(8)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string ErpWFNo { get; set; }

        [StringLength(1000)]
        public string Memo { get; set; }

        public Dictionary<EnumDisplaySts, List<UserInfo>> FunElmUserDictionary { get; set; }
        public bool ElmUserSaveSuccess;
        
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

        private Dictionary<string, string> _elmUserNumDictionary;

        public Dictionary<string, string> ElmUserNumDictionary => _elmUserNumDictionary ?? (_elmUserNumDictionary = new Dictionary<string, string>
        {
            { EnumYN.Y.ToString(), SysSystemFunElmUser.Label_SingleUser },
            { EnumYN.N.ToString(), SysSystemFunElmUser.Label_MultiUser }
        });
        #endregion

        #region - Private -
        private readonly EntitySystemFunElmUser _entity;
        private readonly MongoSystemFunElmUser _entityMongo;
        private List<EntitySystemFunElmUser.ElmAuthUser> EntityElmAuthUserList;
        #endregion

        public string FunGroupNMID;
        public string FunActionNMID;
        public string ElmNMID;
        public string DefaultDisplay;
        
        #region - Validation -
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsValid == false)
            {
                yield break;
            }

            #region - 驗證員編是否重複 -
            if (FunElmUserDictionary != null)
            {
                var userCount =
                    from s in FunElmUserDictionary.SelectMany(sm => sm.Value)
                    where string.IsNullOrWhiteSpace(s.UserID) == false
                    group s by s.UserID
                    into g
                    select new
                    {
                        userID = g.Key,
                        count = g.Count()
                    };

                if (userCount.Any(w => w.count > 1))
                {
                    yield return new ValidationResult(SysSystemFunElmUser.SystemMsg_UserIDRepeat_Failure);
                }
            }
            #endregion
        }
        #endregion

        #region - 取得元素權限使用者明細 -

        /// <summary>
        /// 取得元素權限使用者明細
        /// </summary>
        /// <returns></returns>
        public bool GetElmAuthUserDetail()
        {
            try
            {
                EntitySystemFunElmUser.ElmAuthUserPara para = new EntitySystemFunElmUser.ElmAuthUserPara
                {
                    SysID = new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID),
                    ElmID = new DBVarChar(string.IsNullOrWhiteSpace(FunElmID) ? null : FunElmID),
                    FunControllerID = new DBVarChar(string.IsNullOrWhiteSpace(FunControllerID) ? null : FunControllerID),
                    FunActionNM = new DBVarChar(string.IsNullOrWhiteSpace(FunActionName) ? null : FunActionName)
                };

                EntityElmAuthUserList = _entity.SelectElmAuthUserList(para);
                
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }

        public void ConvertFunElmUserDictionary()
        {
            FunElmUserDictionary = new Dictionary<EnumDisplaySts, List<UserInfo>>();

            foreach (var name in Enum.GetNames(typeof(EnumDisplaySts)))
            {
                var displaySts = (EnumDisplaySts)Enum.Parse(typeof(EnumDisplaySts), name);
                if (FunElmUserDictionary.ContainsKey(displaySts) == false)
                {
                    FunElmUserDictionary.Add(displaySts, new List<UserInfo>());
                }
            }

            if (EntityElmAuthUserList.Any())
            {
                foreach (var funElmUser in
                    from funElmUser in EntityElmAuthUserList
                    group funElmUser by (EnumDisplaySts)Enum.Parse(typeof(EnumDisplaySts), funElmUser.DisplaySts.StringValue())
                    into g
                    select g)
                {
                    FunElmUserDictionary[funElmUser.Key] =
                    (from s in funElmUser
                     select new UserInfo
                     {
                         UserID = s.UserID.GetValue(),
                         UserNMID = s.UserNMID.GetValue()
                     }).ToList();
                }
            }
        }

        #endregion

        #region - 編輯元素權限使用者 -
        /// <summary>
        /// 編輯元素權限使用者
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool EditElmAuthUser(string userID)
        {
            try
            {
                EntitySystemFunElmUser.ElmAuthUserPara para = new EntitySystemFunElmUser.ElmAuthUserPara
                {
                    SysID = new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID),
                    ElmUserInfoList = _GetElmUserInfoListPara(),
                    FunControllerID = new DBVarChar(string.IsNullOrWhiteSpace(FunControllerID) ? null : FunControllerID),
                    FunActionNM = new DBVarChar(string.IsNullOrWhiteSpace(FunActionName) ? null : FunActionName),
                    ElmID = new DBVarChar(string.IsNullOrWhiteSpace(FunElmID) ? null : FunElmID),
                    UpdUserID = new DBVarChar(userID)

                };

                return _entity.EditElmAuthUser(para) == EntitySystemFunElmUser.EnumEditElmAuthUserResult.Success;
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
        public bool RecordLogSystemUserFunElm(string userID, EnumCultureID cultureID)
        {
            try
            {
                var para = new MongoSystemFunElmUser.UserFunElmPara();
                var dateTimeNow = new DBDateTime(DateTime.Now);
                var basicInfoPara = new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                {
                    UserID = new DBVarChar(null),
                    UpdUserID = new DBVarChar(userID),
                    ExecSysID = new DBVarChar(SysID)
                };

                var entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectBasicInfo(basicInfoPara);

                var modifyTypeDictionary =
                (from s in CMCodeDictionary[Entity_BaseAP.EnumCMCodeKind.ModifyType].Cast<Entity_BaseAP.CMCode>()
                 select new
                 {
                     modifyType = s.CodeID.GetValue(),
                     modifyTypeNM = s.CodeNM
                 }).ToDictionary(k => k.modifyType, v => v.modifyTypeNM);

                var originalUserFunElms =
                (from s in EntityElmAuthUserList
                 select new
                 {
                     userID = s.UserID.GetValue(),
                     displaySts = (EnumDisplaySts)Enum.Parse(typeof(EnumDisplaySts), s.DisplaySts.StringValue())
                 }).ToList();

                var newUserFunElms =
                (from s in _GetElmUserInfoListPara()
                 select new
                 {
                     userID = s.User.GetValue(),
                     displaySts = (EnumDisplaySts)Enum.Parse(typeof(EnumDisplaySts), s.DispalySts.StringValue())
                 }).ToList();

                var removeUserFunElms =
                (from s in originalUserFunElms.Except(newUserFunElms)
                 select new
                 {
                     s.userID,
                     s.displaySts,
                     modifyType = Mongo_BaseAP.EnumModifyType.D
                 });

                var addUserFunElms =
                (from s in newUserFunElms.Except(originalUserFunElms)
                 select new
                 {
                     s.userID,
                     s.displaySts,
                     modifyType = Mongo_BaseAP.EnumModifyType.I
                 });

                var allUserFunElms = removeUserFunElms.Union(addUserFunElms).ToList();

                var userInfoDic =
                    new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                        .SelectRawCMUserList(new Entity_BaseAP.RawCMUserPara
                        {
                            UserIDList = Utility.ToDBTypeList<DBVarChar>(allUserFunElms.Select(s => s.userID).Distinct())
                        }).ToDictionary(k => k.UserID.GetValue(), v => v.UserNM.GetValue());

                para.UserInfoList =
                (from s in allUserFunElms
                 select new MongoSystemFunElmUser.UserFunElmInfo
                 {
                     SysID = new DBVarChar(SysID),
                     SysNM = entityBasicInfo.ExecSysNM,
                     ControllerName = new DBVarChar(FunControllerID),
                     ActionName = new DBVarChar(FunActionName),
                     ElmID = new DBVarChar(FunElmID),
                     UserID = new DBVarChar(s.userID),
                     UserNM = new DBVarChar(userInfoDic[s.userID]),
                     DisplaySts = new DBTinyInt((int)s.displaySts),
                     DisplayNM = new DBNVarChar(FunElmDisplayTypeDic[((int)s.displaySts).ToString()]),
                     ModifyType = new DBChar(s.modifyType.ToString()),
                     ModifyTypeNM = modifyTypeDictionary[s.modifyType.ToString()],
                     WFNO = new DBVarChar(ErpWFNo),
                     Memo = new DBVarChar(Memo),
                     UpdUserID = new DBVarChar(userID),
                     UpdUserNM = entityBasicInfo.UpdUserNM,
                     UpdDT = dateTimeNow
                 }).ToList();

                if (para.UserInfoList.Any())
                {
                    return _entityMongo.RecordLogSystemUserFunElm(para) == MongoSystemFunElmUser.EnumRecordLogSystemUserFunElmResult.Success;
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

        #region - 取得元素權限使用者參數 -
        /// <summary>
        /// 取得元素權限使用者參數
        /// </summary>
        /// <returns></returns>
        private List<EntitySystemFunElmUser.ElmUserInfo> _GetElmUserInfoListPara()
        {
            if (FunElmUserDictionary != null)
            {
                return
                (from funElmUser in FunElmUserDictionary
                 select (from s in funElmUser.Value
                         where string.IsNullOrWhiteSpace(s.UserID) == false
                         select new EntitySystemFunElmUser.ElmUserInfo
                         {
                             DispalySts = new DBTinyInt((byte)funElmUser.Key),
                             User = new DBVarChar(s.UserID)
                         })).SelectMany(sm => sm).ToList();
            }

            return new List<EntitySystemFunElmUser.ElmUserInfo>();
        }
        #endregion

        public bool GetSystemFunElmInfo(EnumCultureID cultureID)
        {
            try
            {
                EntitySystemFunElmUser.SystemFunElmInfoPara para = new EntitySystemFunElmUser.SystemFunElmInfoPara(cultureID.ToString())
                {
                    SysID = new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID),
                    ElmID = new DBVarChar(string.IsNullOrWhiteSpace(FunElmID) ? null : FunElmID),
                    FunControllerID = new DBVarChar(string.IsNullOrWhiteSpace(FunControllerID) ? null : FunControllerID),
                    FunActionNM = new DBVarChar(string.IsNullOrWhiteSpace(FunActionName) ? null : FunActionName)
                };

                var result = _entity.SelectSystemFunElmInfo(para);
                if (result != null)
                {
                    DefaultDisplay = result.DefaultDisplay.GetValue();
                    FunGroupNMID = result.FnGroupNMID.GetValue();
                    FunActionNMID = result.FnNMID.GetValue();
                    ElmNMID = result.ElmNMID.GetValue();
                }

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }

    }
}