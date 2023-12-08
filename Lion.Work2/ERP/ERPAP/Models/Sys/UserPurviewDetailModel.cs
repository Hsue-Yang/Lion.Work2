using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.EDIService;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class UserPurviewDetailModel : SysModel, IValidatableObject
    {
        public class SysUserPurviewDetails
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string PurviewNM { get; set; }
            public string PurviewID { get; set; }
            public string CodeType { get; set; }
            public string CodeID { get; set; }
            public string PurviewOP { get; set; }
            public bool HasDataPur { get; set; }
        }

        public class PurviewName
        {
            public string PurviewID { get; set; }
            public string PurviewNM { get; set; }
        }

        public class UserPurviewPara
        {
            public string SysID { get; set; }
            public string UserID { get; set; }
            public string UpdUserID { get; set; }
            public List<SysUserPurviewDetails> SysUserPurviewDetailList { get; set; }
        }

        List<SysUserPurviewDetails> UserPurviewDetails { get; set; }
        List<PurviewName> PurviewNames { get; set; }

        List<SysUserPurviewDetails> UserPurviewDetailList;

        #region - Definitions -
        public enum EnumPurviewOPType
        {
            [Description("Query")]
            Q,

            [Description("Update")]
            U
        }

        public class UserPurviewInfo
        {
            public string PurviewID { get; set; }
            public string PurviewNM { get; set; }
            public List<PurviewCode> PurList { get; set; }
        }

        public class PurviewCode
        {
            public Entity_BaseAP.EnumPurviewCodeType PurviewCodeType { get; set; }
            public string CodeID { get; set; }
            public string CodeNM { get; set; }
            public string PurviewOP { get; set; }
        }
        #endregion

        #region - Constructor -
        public UserPurviewDetailModel()
        {
            _entity = new EntityUserPurviewDetail(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string SysID { get; set; }

        public string SysNM { get; set; }

        [StringLength(6, MinimumLength = 6)]
        public string UserID { get; set; }

        [InputType(EnumInputType.TextBox)]
        public string UserNM { get; set; }

        [Required]
        [StringLength(8)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string ErpWFNo { get; set; }

        [StringLength(1000)]
        public string Memo { get; set; }

        public List<UserPurviewInfo> UserPurviewInfoList { get; set; }

        public List<EntityEventPara.SysUserPurviewEdit> UserPurviewEventList { get; private set; }

        private Dictionary<string, string> _purviewOPDic;

        public Dictionary<string, string> PurviewOPDic
        {
            get
            {
                return _purviewOPDic ?? (_purviewOPDic = new Dictionary<string, string>
                {
                    { EnumPurviewOPType.Q.ToString(), SysUserPurviewDetail.Label_PurviewQuery },
                    { EnumPurviewOPType.U.ToString(), SysUserPurviewDetail.Label_PurviewUpdate }
                });
            }
        }
        #endregion

        #region - Field -
        public bool SaveSuccess;
        #endregion

        #region - Private -
        private readonly EntityUserPurviewDetail _entity;
        private List<EntityUserPurviewDetail.UserPurviewDetail> _entityUserPurviewDetailList;
        private List<EntityUserPurviewDetail.UserPurviewDetail> _entityOriginalUserPurviewDetailList;
        private List<Mongo_BaseAP.RecordLogUserPurviewPara> _recordLogUserPurviewParas;
        #endregion

        #region - Validation -
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsValid == false)
            {
                yield break;
            }

            if (_GetIsValidUserPurviewInfoList() == false)
            {
                yield return new ValidationResult(SysUserPurviewDetail.SystemMsg_IsValidPurviewCode_Failure);
            }
        }

        #region - 資料權限代碼驗證 -
        private bool _GetIsValidUserPurviewInfoList()
        {
            if (UserPurviewInfoList != null &&
                UserPurviewInfoList.Any())
            {
                var userPurviewInfoList =
                    UserPurviewInfoList
                        .Where(p => p.PurList != null)
                        .SelectMany(t => t.PurList, (t, v) => new
                        {
                            t.PurviewID,
                            v.PurviewCodeType,
                            v.CodeID,
                            v.PurviewOP
                        })
                        .Distinct()
                        .ToList();

                foreach (var row in userPurviewInfoList)
                {
                    if (row.PurviewCodeType == Entity_BaseAP.EnumPurviewCodeType.COMPANY &&
                        EntityBaseRawCMOrgComList.Exists(h => h.ComID.GetValue() == row.CodeID) == false)
                    {
                        return false;
                    }
                    if (row.PurviewCodeType == Entity_BaseAP.EnumPurviewCodeType.COUNTRY &&
                        CMCodeDictionary[Entity_BaseAP.EnumCMCodeKind.LionCountryCode].Exists(country => country.ItemValue() == row.CodeID) == false)
                    {
                        return false;
                    }
                    if (row.PurviewCodeType == Entity_BaseAP.EnumPurviewCodeType.UNIT &&
                        EntityRawCMOrgUnitList.Exists(h => h.UnitID.GetValue() == row.CodeID) == false)
                    {
                        return false;
                    }
                }

                UserPurviewDetailList =
                    userPurviewInfoList.Select(e => new SysUserPurviewDetails
                    {
                        PurviewID = (e.PurviewID),
                        CodeType = (e.PurviewCodeType.ToString()),
                        CodeID = (e.CodeID),
                        PurviewOP = (e.PurviewOP)
                    }).ToList();
            }
            UserPurviewDetailList = UserPurviewDetailList ?? new List<SysUserPurviewDetails>();
            return true;
        }
        #endregion

        #endregion

        #region - 取得使用者資料權限明細 -
        /// <summary>
        /// 取得使用者資料權限明細
        /// </summary>
        /// <param name="cultureId"></param>
        /// <returns></returns>
        public async Task<bool> GetOriginalUserPurviewDetailList(EnumCultureID cultureId)
        {
            try
            {
                SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                UserID = string.IsNullOrWhiteSpace(UserID) ? null : UserID;

                string apiUrl = API.SystemUserPurview.QuerySysUserPurviewDetails(SysID, UserID, cultureId.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SysUserPurviewDetailList = (List<SysUserPurviewDetails>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                UserPurviewDetails = responseObj.SysUserPurviewDetailList;

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 取得使用者資料資訊清單 -
        /// <summary>
        /// 取得使用者資料資訊清單
        /// </summary>
        /// <returns></returns>
        public bool GetUserPurviewInfoList()
        {
            try
            {
                UserPurviewInfoList =
                    (from s in UserPurviewDetails
                     group s by new
                     {
                         PurviewID = s.PurviewID,
                         PurviewNM = s.PurviewNM
                     }
                         into g
                     select new UserPurviewInfo
                     {
                         PurviewID = g.Key.PurviewID,
                         PurviewNM = g.Key.PurviewNM,
                         PurList = (from a in g
                                    where !string.IsNullOrEmpty(a.CodeID)
                                    select new PurviewCode
                                    {
                                        CodeID = a.CodeID,
                                        CodeNM = string.Format(Common.GetEnumDesc(EnumTextFormat.Code), _GetPurviewCodeName(a.CodeType, a.CodeID), a.CodeID),
                                        PurviewCodeType = (Entity_BaseAP.EnumPurviewCodeType)Enum.Parse(typeof(Entity_BaseAP.EnumPurviewCodeType), a.CodeType),
                                        PurviewOP = a.PurviewOP
                                    }).ToList()
                     }).ToList();

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 更新使用者資料權限 -
        /// <summary>
        /// 更新使用者資料權限
        /// </summary>
        /// <param name="updUserID"></param>
        /// <param name="cultureId"></param>
        /// <returns></returns>
        public async Task<bool> EditSysUserPurviewDetail(string updUserID, EnumCultureID cultureId)
        {
            try
            {
                UserPurviewPara para = new UserPurviewPara()
                {
                    SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                    UserID = string.IsNullOrWhiteSpace(UserID) ? null : UserID,
                    UpdUserID = updUserID,
                    SysUserPurviewDetailList = UserPurviewDetailList
                };

                var paraJsonStr = Common.GetJsonSerializeObject(para);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.SystemUserPurview.EditSysUserPurviewDetail(updUserID);
                await PublicFun.HttpPutWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 寫入使用者資料權限log紀錄 -
        /// <summary>
        /// 寫入使用者資料權限log紀錄
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RecordLogSysUserPurview(EnumCultureID cultureID, string updUserID, string ipAddress)
        {
            try
            {
                Entity_BaseAP.BasicInfoPara entityBasicInfoPara =
                    new Entity_BaseAP.BasicInfoPara(cultureID.ToString())
                    {
                        UserID = new DBVarChar(UserID),
                        ExecSysID = new DBVarChar((string.IsNullOrWhiteSpace(SysID) ? null : SysID))
                    };

                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectBasicInfo(entityBasicInfoPara);

                var purviewDictionary =
                    (from s in CMCodeDictionary[Entity_BaseAP.EnumCMCodeKind.PurviewType].Cast<Entity_BaseAP.CMCode>()
                     select new
                     {
                         purviewType = s.CodeID.GetValue(),
                         purviewTypeNM = s.CodeNM
                     }).ToDictionary(k => k.purviewType, v => v.purviewTypeNM);

                SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                string apiUrl = API.SystemUserPurview.QueryPurviewNames(SysID, cultureID.ToString().ToUpper());
                var response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                var responseObj = new
                {
                    PurviewNameList = (List<PurviewName>)null
                };
                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);
                PurviewNames = responseObj.PurviewNameList;
                var userPurviewNameDictionary = PurviewNames.ToDictionary(k => k.PurviewID, v => v.PurviewNM);

                _recordLogUserPurviewParas =
                    (from s in UserPurviewDetailList
                     where Enum.IsDefined(typeof(Entity_BaseAP.EnumPurviewCodeType), s.CodeType)
                     group s by new
                     {
                         PurviewID = s.PurviewID
                     }
                     into groupbyPurview
                     select new Mongo_BaseAP.RecordLogUserPurviewPara
                     {
                         PurviewID = new DBVarChar(groupbyPurview.Key.PurviewID),
                         PurviewNM = userPurviewNameDictionary[groupbyPurview.Key.PurviewID],
                         SysID = entityBasicInfo.ExecSysID,
                         SysNM = entityBasicInfo.ExecSysNM,
                         UserID = entityBasicInfo.UserID,
                         UserNM = entityBasicInfo.UserNM,
                         PurviewCollectList = (from collect in groupbyPurview
                                               select new Mongo_BaseAP.LOG_SYS_USER_PURVIEW_COLLECT
                                               {
                                                   CodeID = collect.CodeID,
                                                   CodeNM = new DBNVarChar(_GetPurviewCodeName(collect.CodeType, collect.CodeID)),
                                                   CodeType = collect.CodeType,
                                                   CodeTypeNM = purviewDictionary[collect.CodeType],
                                                   PurviewOP = collect.PurviewOP,
                                                   PurviewOPNM = new DBNVarChar(PurviewOPDic[collect.PurviewOP])
                                               }).ToList()

                     }).ToList();

                Mongo_BaseAP.RecordLogUserPurviewPara para = new Mongo_BaseAP.RecordLogUserPurviewPara();
                foreach (var s in _recordLogUserPurviewParas)
                {
                    para.APINo = s.APINo;
                    para.PurviewID = s.PurviewID;
                    para.PurviewNM = s.PurviewNM;
                    para.SysID = s.SysID;
                    para.SysNM = s.SysNM;
                    para.UserID = s.UserID;
                    para.UserNM = s.UserNM;
                    para.PurviewCollectList = s.PurviewCollectList;
                    RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_USER_PURVIEW, Mongo_BaseAP.EnumModifyType.U, updUserID, ipAddress, cultureID, para);
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

        #region - 記錄使用者資料權限異動 -
        /// <summary>
        /// 記錄使用者資料權限異動
        /// </summary>
        /// <param name="cultureID"></param>
        /// <param name="updUserID"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool RecordLogSysUserPurviewApply(EnumCultureID cultureID, string updUserID, string ipAddress)
        {
            try
            {
                var modifyList = _GetUserPurviewModifyList(cultureID);

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

                    Mongo_BaseAP.RecordLogUserPurviewApplyPara para = new Mongo_BaseAP.RecordLogUserPurviewApplyPara();
                    para.UserID = new DBVarChar(UserID);
                    para.UserNM = entityBasicInfo.UserNM;
                    para.WFNO = new DBVarChar(ErpWFNo);
                    para.Memo = new DBVarChar(Memo);
                    para.ModifyList = modifyList;

                    RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_USER_PURVIEW_APPLY, Mongo_BaseAP.EnumModifyType.U, updUserID, ipAddress, cultureID, para);
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

        #region - 取得使用者資料權限異動清單 -
        /// <summary>
        /// 取得使用者資料權限異動清單
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        private List<Mongo_BaseAP.UserPurviewModify> _GetUserPurviewModifyList(EnumCultureID cultureID)
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

            var purviewDictionary =
                (from s in CMCodeDictionary[Entity_BaseAP.EnumCMCodeKind.PurviewType].Cast<Entity_BaseAP.CMCode>()
                 select new
                 {
                     purviewType = s.CodeID.GetValue(),
                     purviewTypeNM = s.CodeNM
                 }).ToDictionary(k => k.purviewType, v => v.purviewTypeNM);

            var recordLogUserPurviewParas =
                _recordLogUserPurviewParas
                    .SelectMany(sm => sm.PurviewCollectList, (m, d) =>
                        new
                        {
                            m.PurviewID,
                            m.PurviewNM,
                            m.SysID,
                            m.SysNM,
                            m.UserID,
                            m.UserNM,
                            d.CodeID,
                            d.CodeNM,
                            d.CodeType,
                            d.CodeTypeNM,
                            d.PurviewOP,
                            d.PurviewOPNM
                        }).ToList();

            return
                            (from s in UserPurviewDetails
                             where recordLogUserPurviewParas.Exists(e => e.PurviewID.GetValue() == s.PurviewID &&
                                                                         e.CodeID.GetValue() == s.CodeID &&
                                                             e.CodeType.GetValue() == s.CodeType &&
                                                             e.PurviewOP.GetValue() == s.PurviewOP) == false &&
                       s.HasDataPur
                             select new Mongo_BaseAP.UserPurviewModify
                             {
                                 SysID = s.SysID,
                                 SysNM = s.SysNM,
                                 PurviewID = s.PurviewID,
                                 PurviewNM = s.PurviewNM,
                                 CodeID = s.CodeID,
                                 CodeNM = new DBNVarChar(_GetPurviewCodeName(s.CodeType, s.CodeID)),
                                 CodeType = s.CodeType,
                                 CodeTypeNM = purviewDictionary[s.CodeType],
                                 PurviewOP = s.PurviewOP,
                                 PurviewOPNM = new DBNVarChar(PurviewOPDic[s.PurviewOP]),
                                 ModifyType = new DBChar(Mongo_BaseAP.EnumModifyType.D),
                                 ModifyTypeNM = modifyTypeDic[Mongo_BaseAP.EnumModifyType.D.ToString()]
                             }).Concat(
                     (from s in recordLogUserPurviewParas
                      where UserPurviewDetails.Exists(e => e.PurviewID == s.PurviewID.GetValue() &&
                                                                             e.CodeID == s.CodeID.GetValue() &&
                                                                             e.CodeType == s.CodeType.GetValue() &&
                                                                             e.PurviewOP == s.PurviewOP.GetValue() &&
                                                                             e.HasDataPur) == false

                      select new Mongo_BaseAP.UserPurviewModify
                      {
                          SysID = s.SysID,
                          SysNM = s.SysNM,
                          PurviewID = s.PurviewID,
                          PurviewNM = s.PurviewNM,
                          CodeID = s.CodeID,
                          CodeNM = s.CodeNM,
                          CodeType = s.CodeType,
                          CodeTypeNM = s.CodeTypeNM,
                          PurviewOP = s.PurviewOP,
                          PurviewOPNM = s.PurviewOPNM,
                          ModifyType = new DBChar(Mongo_BaseAP.EnumModifyType.I),
                          ModifyTypeNM = modifyTypeDic[Mongo_BaseAP.EnumModifyType.I.ToString()]
                      })
                    ).ToList();
        }
        #endregion

        #region - 取得事件訂閱使用者資料權限參數 -
        /// <summary>
        /// 取得事件訂閱使用者資料權限參數
        /// </summary>
        public bool GetUserPurviewEventList()
        {
            try
            {
                UserPurviewEventList =
                    (from purview in UserPurviewDetailList
                     group purview by new
                     {
                         PurviewID = purview.PurviewID
                     }
                     into groupbyPurview
                     select new EntityEventPara.SysUserPurviewEdit
                     {
                         TargetSysIDList = new List<DBVarChar> { new DBVarChar(SysID) },
                         UserID = new DBVarChar(UserID),
                         PurviewList = (from p in groupbyPurview
                                        group p by new
                                        {
                                            CodeType = p.CodeType,
                                            PurviewID = p.PurviewID
                                        }
                                        into item
                                        select new EntityEventPara.SysUserPurviewEdit.Purview
                                        {
                                            PurviewID = new DBVarChar(item.Key.PurviewID),
                                            CodeType = new DBVarChar(item.Key.CodeType),
                                            ItemList = (from code in item
                                                        select new EntityEventPara.SysUserPurviewEdit.Purview.Item
                                                        {
                                                            CodeID = code.CodeID,
                                                            PurviewOP = code.PurviewOP
                                                        }).ToList()
                                        }).ToList()
                     }).ToList();

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 取得資料權限代碼名稱 -
        /// <summary>
        /// 取得資料權限代碼名稱
        /// </summary>
        /// <param name="purviewCodeType"></param>
        /// <param name="codeID"></param>
        /// <returns></returns>
        private string _GetPurviewCodeName(string purviewCodeType, string codeID)
        {
            var enumPurviewCodeType = (Entity_BaseAP.EnumPurviewCodeType)Enum.Parse(typeof(Entity_BaseAP.EnumPurviewCodeType), purviewCodeType);

            switch (enumPurviewCodeType)
            {
                case Entity_BaseAP.EnumPurviewCodeType.COMPANY:
                    return (from s in EntityBaseRawCMOrgComList
                            where s.ComID.GetValue() == codeID
                            select s.ComNM.GetValue()).SingleOrDefault();
                case Entity_BaseAP.EnumPurviewCodeType.COUNTRY:
                    return (from s in CMCodeDictionary[Entity_BaseAP.EnumCMCodeKind.LionCountryCode]
                            where s.ItemValue() == codeID
                            select s.ItemValue()).SingleOrDefault();
                case Entity_BaseAP.EnumPurviewCodeType.UNIT:
                    return (from s in EntityRawCMOrgUnitList
                            where s.UnitID.GetValue() == codeID
                            select s.UnitNM.GetValue()).SingleOrDefault();
                default:
                    throw new ArgumentOutOfRangeException("purviewCodeType", enumPurviewCodeType, null);
            }
        }
        #endregion
    }
}