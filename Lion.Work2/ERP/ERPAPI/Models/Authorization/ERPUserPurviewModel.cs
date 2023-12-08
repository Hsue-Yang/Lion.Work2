// 新增日期：2016-11-08
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Authorization;
using LionTech.Utility;

namespace ERPAPI.Models.Authorization
{
    public class ERPUserPurviewModel : AuthorizationModel
    {
        #region - Definitions -
        public class APIParaData
        {
            public string SysID { get; set; }
            public string UpdUserID { get; set; }
            public string UserID { get; set; }
            public List<Purview> PurviewList { get; set; }
        }

        public class Purview
        {
            public string PurviewID { get; set; }
            public string CodeType { get; set; }
            public List<Item> ItemList { get; set; }
        }

        public class Item
        {
            public string CodeID { get; set; }
            public string PurviewOP { get; set; }
        }
        #endregion

        #region - Constructor -
        public ERPUserPurviewModel()
        {
            _entity = new EntityERPUserPurview(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - API Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }

        //[AllowHtml]
        public string APIPara { get; set; }

        public APIParaData APIData { get; set; }
        #endregion

        #region - Private -
        private readonly EntityERPUserPurview _entity;
        private List<EntityERPUserPurview.CodeInfo> _codeInfoList;
        private List<EntityERPUserPurview.Purview> _excludeDuplicatesPurviewList;
        #endregion

        #region - 編輯使用者資料權限 -
        /// <summary>
        /// 編輯使用者資料權限
        /// </summary>
        /// <returns></returns>
        public bool EditUserPurview()
        {
            try
            {
                _GetExcludeDuplicatesPurviewList();

                EntityERPUserPurview.UserPurviewPara para = new EntityERPUserPurview.UserPurviewPara
                {
                    SysID = new DBVarChar(string.IsNullOrWhiteSpace(APIData.SysID) ? null : APIData.SysID),
                    UserID = new DBVarChar(APIData.UserID),
                    UpdUserID = new DBVarChar(APIData.UpdUserID),
                    PurviewList = _excludeDuplicatesPurviewList
                };

                return _entity.EditUserPurview(para) == EntityERPUserPurview.EnumUserPurviewResult.Success;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 紀錄更新使用者資料權限 -
        /// <summary>
        /// 紀錄更新使用者資料權限
        /// </summary>
        /// <param name="apiNo"></param>
        /// <param name="updUserID"></param>
        /// <param name="execSysID"></param>
        /// <param name="ipAddress"></param>
        public void RecordUserPurview(string apiNo, string updUserID, string execSysID, string ipAddress)
        {
            try
            {
                _GetPurviewCodeInfo();

                Entity_BaseAP.BasicInfoPara entityBasicInfoPara =
                    new Entity_BaseAP.BasicInfoPara(EnumCultureID.zh_TW.ToString())
                    {
                        UserID = new DBVarChar(APIData.UserID),
                        ExecSysID = new DBVarChar(APIData.SysID)
                    };
                Entity_BaseAP.BasicInfo entityBasicInfo = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectBasicInfo(entityBasicInfoPara);

                var codeTypeNameDictionary = _GetCMCodeList(Entity_BaseAP.EnumCMCodeKind.PurviewType)
                    .ToDictionary(k => k.CodeID.GetValue(), v => v.CodeNM);

                var modifyTypeNameDictionary = _GetCMCodeList(Entity_BaseAP.EnumCMCodeKind.ModifyType)
                    .ToDictionary(k => k.CodeID.GetValue(), v => v.CodeNM);

                var userPurviewNameDictionary =
                    _entity.SelectSysPurviewList(new EntityERPUserPurview.SysPurviewPara(EnumCultureID.zh_TW.ToString())
                    {
                        SysID = new DBVarChar(string.IsNullOrWhiteSpace(APIData.SysID) ? null : APIData.SysID)
                    }).ToDictionary(k => k.PurviewID.GetValue(), v => v.PurviewNM);

                var mongodata =
                    (from e in _excludeDuplicatesPurviewList
                     where Enum.IsDefined(typeof(Entity_BaseAP.EnumPurviewCodeType), e.CodeType.GetValue())
                     group e by new
                     {
                         PurviewID = e.PurviewID.GetValue()
                     }
                     into groupbyPurview
                     select new Mongo_BaseAP.RecordLogUserPurviewPara
                     {
                         PurviewID = new DBVarChar(groupbyPurview.Key.PurviewID),
                         PurviewNM = userPurviewNameDictionary[groupbyPurview.Key.PurviewID],
                         SysID = new DBVarChar(APIData.SysID),
                         SysNM = entityBasicInfo.ExecSysNM,
                         UserID = new DBVarChar(APIData.UserID),
                         UserNM = entityBasicInfo.UserNM,
                         PurviewCollectList = (from collect in groupbyPurview
                                               select new Mongo_BaseAP.LOG_SYS_USER_PURVIEW_COLLECT()
                                               {
                                                   CodeID = collect.CodeID,
                                                   CodeNM = _GetCodeNM((Entity_BaseAP.EnumPurviewCodeType)Enum.Parse(typeof(Entity_BaseAP.EnumPurviewCodeType), collect.CodeType.GetValue()), collect.CodeID.GetValue()),
                                                   CodeType = collect.CodeType,
                                                   CodeTypeNM = codeTypeNameDictionary[collect.CodeType.GetValue()],
                                                   PurviewOP = collect.PurviewOP,
                                                   PurviewOPNM = modifyTypeNameDictionary[collect.PurviewOP.GetValue()]
                                               }).ToList()
                     }).ToList();

                Mongo_BaseAP.RecordLogUserPurviewPara para = new Mongo_BaseAP.RecordLogUserPurviewPara();

                foreach (var purview in mongodata)
                {
                    para.PurviewID = purview.PurviewID;
                    para.PurviewNM = purview.PurviewNM;
                    para.SysID = purview.SysID;
                    para.SysNM = purview.SysNM;
                    para.UserID = purview.UserID;
                    para.UserNM = purview.UserNM;
                    para.PurviewCollectList = purview.PurviewCollectList;
                    RecordLog(Mongo_BaseAP.EnumLogDocName.LOG_USER_PURVIEW, Mongo_BaseAP.EnumModifyType.U, apiNo, updUserID, execSysID, ipAddress, para);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }
        #endregion

        #region - 取得代碼清單 -
        /// <summary>
        /// 取得代碼清單
        /// </summary>
        /// <param name="cmCodeKind"></param>
        /// <returns></returns>
        private List<Entity_BaseAP.CMCode> _GetCMCodeList(Entity_BaseAP.EnumCMCodeKind cmCodeKind)
        {
            Entity_BaseAP.CMCodePara codePurviewTypePara =
                new Entity_BaseAP.CMCodePara
                {
                    ItemTextType = Entity_BaseAP.EnumCMCodeItemTextType.CodeNM,
                    CodeKind = cmCodeKind,
                    CodeParent = new DBVarChar(null),
                    CultureID = new DBVarChar(EnumCultureID.zh_TW.ToString())
                };
            return new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                .SelectCMCodeList(codePurviewTypePara);
        }
        #endregion

        #region - 取得資料權限代碼資訊 -
        /// <summary>
        /// 取得資料權限代碼資訊
        /// </summary>
        private void _GetPurviewCodeInfo()
        {
            EntityERPUserPurview.CodeInfoPara para = new EntityERPUserPurview.CodeInfoPara(EnumCultureID.zh_TW.ToString())
            {
                Codes = new List<EntityERPUserPurview.Code>()
            };

            var purviewIDList =
                (from s in _excludeDuplicatesPurviewList
                 where Enum.IsDefined(typeof(Entity_BaseAP.EnumPurviewCodeType), s.CodeType.GetValue())
                 select new
                 {
                     CodeType = s.CodeType.GetValue(),
                     CodeID = s.CodeID.GetValue()
                 }).Distinct().Select(s => new EntityERPUserPurview.Code
                 {
                     CodeID = new DBVarChar(s.CodeID),
                     CodeType = (Entity_BaseAP.EnumPurviewCodeType)Enum.Parse(typeof(Entity_BaseAP.EnumPurviewCodeType), s.CodeType)
                 }).ToList();

            foreach (var row in purviewIDList)
            {
                switch (row.CodeType)
                {
                    case Entity_BaseAP.EnumPurviewCodeType.COMPANY:
                        para.Codes.Add(new EntityERPUserPurview.Code
                        {
                            CodeType = Entity_BaseAP.EnumPurviewCodeType.COMPANY,
                            CodeID = row.CodeID
                        });
                        break;
                    case Entity_BaseAP.EnumPurviewCodeType.COUNTRY:
                        para.Codes.Add(new EntityERPUserPurview.Code
                        {
                            CodeType = Entity_BaseAP.EnumPurviewCodeType.COUNTRY,
                            CodeKind = new DBVarChar(Common.GetEnumDesc(Entity_BaseAP.EnumCMCodeKind.LionCountryCode)),
                            CodeID = row.CodeID
                        });
                        break;
                    case Entity_BaseAP.EnumPurviewCodeType.UNIT:
                        para.Codes.Add(new EntityERPUserPurview.Code
                        {
                            CodeType = Entity_BaseAP.EnumPurviewCodeType.UNIT,
                            CodeID = row.CodeID
                        });
                        break;
                }
            }

            _codeInfoList = _entity.SelectCodeInfoList(para);
        }
        #endregion

        #region - 取得代碼名稱 -
        /// <summary>
        /// 取得代碼名稱
        /// </summary>
        /// <param name="enumCodetype"></param>
        /// <param name="codeID"></param>
        /// <returns></returns>
        private DBNVarChar _GetCodeNM(Entity_BaseAP.EnumPurviewCodeType enumCodetype, string codeID)
        {
            return (from s in _codeInfoList
                    where s.CodeType.GetValue() == enumCodetype.ToString() &&
                          s.CodeID.IsNull() == false &&
                          string.IsNullOrWhiteSpace(codeID) == false &&
                          s.CodeID.GetValue().Trim() == codeID.Trim()
                    select s.CodeNM).SingleOrDefault();
        }
        #endregion

        #region - 取得排除重複資料權限 -
        /// <summary>
        /// 取得排除重複資料權限
        /// </summary>
        private void _GetExcludeDuplicatesPurviewList()
        {
            _excludeDuplicatesPurviewList = APIData.PurviewList.SelectMany(purview => purview.ItemList, (purview, item) => new
            {
                item.CodeID,
                purview.CodeType,
                purview.PurviewID,
                item.PurviewOP
            }).Distinct().Select(row => new EntityERPUserPurview.Purview
            {
                CodeID = new DBVarChar(string.IsNullOrWhiteSpace(row.CodeID) ? null : row.CodeID),
                CodeType = new DBVarChar(string.IsNullOrWhiteSpace(row.CodeType) ? null : row.CodeType),
                PurviewID = new DBVarChar(string.IsNullOrWhiteSpace(row.PurviewID) ? null : row.PurviewID),
                PurviewOP = new DBChar(string.IsNullOrWhiteSpace(row.PurviewOP) ? null : row.PurviewOP)
            }).ToList();
        }
        #endregion

    }
}