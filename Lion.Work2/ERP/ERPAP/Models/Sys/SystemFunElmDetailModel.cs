// 新增日期：2018-01-09
// 新增人員：廖先駿
// 新增內容：元素權限明細
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemFunElmDetailModel : SysModel, IValidatableObject
    {
        public enum EnumFunElmDisplayDefaultType
        {
            DISPLAY = 1,
            READ_ONLY = 2,
            MASKING = 3,
            HIDE = 4
        }

        #region - Constructor -
        public SystemFunElmDetailModel()
        {
            _entity = new EntitySystemFunElmDetail(ConnectionStringSERP, ProviderNameSERP);
            _entityMongo = new MongoSystemFunElmDetail(ConnectionStringMSERP, ProviderNameMSERP);
        }
        #endregion

        #region - Property -
        [Required]
        public string SysID { set; get; }

        [Required]
        public string FunControllerID { set; get; }

        [Required]
        public string FunActionName { set; get; }

        [Required]
        [StringLength(50)]
        public string FunElmID { get; set; }

        [Required]
        [StringLength(150)]
        public string FunElmNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        public string FunElmNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        public string FunElmNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        public string FunElmNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        public string FunElmNMJAJP { get; set; }

        [Required]
        [StringLength(150)]
        public string FunElmNMKOKR { get; set; }

        public string IsDisable { get; set; }

        public string SystemInfoJsonString { get; set; }

        [Required]
        public int FunElmDisplayDefaultType { get; set; }

        public Dictionary<string, string> SystemFunControllerDic { get; set; }
        public Dictionary<string, string> SystemFunActionDic { get; set; }

        private Dictionary<string, string> _funElmDisplayDefaultTypeDic;

        public Dictionary<string, string> FunElmDisplayDefaultTypeDic
        {
            get
            {
                if (_funElmDisplayDefaultTypeDic == null &&
                    CMCodeDictionary != null)
                {
                    _funElmDisplayDefaultTypeDic = (from s in CMCodeDictionary[Entity_BaseAP.EnumCMCodeKind.ElmDisplayType].Cast<Entity_BaseAP.CMCode>()
                                                    select new
                                                    {
                                                        SourceType = s.CodeID.GetValue(),
                                                        SourceTypeNM = s.CodeNM.GetValue()
                                                    }).ToDictionary(k => k.SourceType, v => v.SourceTypeNM);
                }

                return _funElmDisplayDefaultTypeDic;
            }
        }
        #endregion

        #region - Private -
        private readonly EntitySystemFunElmDetail _entity;
        private readonly MongoSystemFunElmDetail _entityMongo;
        #endregion

        #region - Validation -
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ExecAction == EnumActionType.Add)
            {
                #region - 驗證功能元素代碼重複 -             

                string apiUrl = API.SysFunElm.CheckSystemFunElmIdIsExists(SysID, FunElmID, FunControllerID, FunActionName);
                string response = Common.HttpWebRequestGetResponseString(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new { IsExists = false });

                if (responseObj.IsExists)
                {
                    yield return new ValidationResult(SysSystemFunElmDetail.SystemMsg_SystemFunElmIDRepeat);
                }
                #endregion
            }
        }
        #endregion

        public void FormReset()
        {
            IsDisable = string.Empty;
            FunElmDisplayDefaultType = (int)EnumFunElmDisplayDefaultType.DISPLAY;

            EntitySystemFunControllerList.Insert(0, new SystemFunController
            {
                SysID = SysID,
                FunControllerID = string.Empty,
                FunGroupNM = string.Empty
            });

            EntitySystemFunActionList.Insert(0, new SysModel.SystemFunAction
            {
                SysID = SysID,
                FunControllerID = string.IsNullOrWhiteSpace(FunControllerID) ? string.Empty : FunControllerID,
                FunAction = string.Empty,
                FunActionNM = string.Empty
            });

            SystemFunControllerDic = EntitySystemFunControllerList.Where(s => s.SysID == SysID)
                                                                  .ToDictionary(k => k.FunControllerID, v => v.FunGroupNM);

            SystemFunActionDic = EntitySystemFunActionList.Where(s => s.SysID == SysID && s.FunControllerID == FunControllerID)
                                                          .ToDictionary(k => k.FunAction, v => v.FunActionNM);
        }

        #region - 取得元素權限明細 -
        /// <summary>
        /// 取得元素權限明細
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetSystemFunElmDetail(string userID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FunElmID) || string.IsNullOrWhiteSpace(FunControllerID) || string.IsNullOrWhiteSpace(FunActionName))
                    return true;

                string apiUrl = API.SysFunElm.QuerySystemFunElmDetail(SysID, userID, FunElmID, FunControllerID, FunActionName);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                var systemFunElmDetail = Common.GetJsonDeserializeObject<SystemFunElm>(response);

                if (systemFunElmDetail != null)
                {
                    IsDisable = systemFunElmDetail.IsDisable;
                    FunElmDisplayDefaultType = systemFunElmDetail.DefaultDisplaySts;
                    FunElmNMZHTW = systemFunElmDetail.ElmNMZHTW;
                    FunElmNMZHCN = systemFunElmDetail.ElmNMZHCN;
                    FunElmNMENUS = systemFunElmDetail.ElmNMENUS;
                    FunElmNMTHTH = systemFunElmDetail.ElmNMTHTH;
                    FunElmNMJAJP = systemFunElmDetail.ElmNMJAJP;
                    FunElmNMKOKR = systemFunElmDetail.ElmNMKOKR;
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

        #region - 編輯元素權限明細 -
        /// <summary>
        /// 編輯元素權限明細
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<bool> EditSystemFunElmDetail(string userID)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                {
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                };

                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    ElmID = string.IsNullOrWhiteSpace(FunElmID) ? null : FunElmID,
                    SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                    FunControllerID = string.IsNullOrWhiteSpace(FunControllerID) ? null : FunControllerID,
                    FunActionNM = string.IsNullOrWhiteSpace(FunActionName) ? null : FunActionName,
                    IsDisable = string.IsNullOrWhiteSpace(IsDisable) ? EnumYN.N.ToString() : EnumYN.Y.ToString(),
                    DefaultDisplaySts = FunElmDisplayDefaultType,
                    ElmNMZHTW = string.IsNullOrWhiteSpace(FunElmNMZHTW) ? null : FunElmNMZHTW,
                    ElmNMZHCN = string.IsNullOrWhiteSpace(FunElmNMZHCN) ? null : FunElmNMZHCN,
                    ElmNMENUS = string.IsNullOrWhiteSpace(FunElmNMENUS) ? null : FunElmNMENUS,
                    ElmNMJAJP = string.IsNullOrWhiteSpace(FunElmNMJAJP) ? null : FunElmNMJAJP,
                    ElmNMTHTH = string.IsNullOrWhiteSpace(FunElmNMTHTH) ? null : FunElmNMTHTH,
                    ElmNMKOKR = string.IsNullOrWhiteSpace(FunElmNMKOKR) ? null : FunElmNMKOKR,
                    UpdUserID = userID
                });

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SysFunElm.EditSystemFunElmDetail(userID, SysID, FunElmID, FunControllerID, FunActionName);
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

        #region - 刪除元素權限明細 -
        /// <summary>
        /// 刪除元素權限明細
        /// </summary>
        /// <returns></returns>
        public bool DeleteSystemFunElmDetail()
        {
            try
            {
                EntitySystemFunElmDetail.SystemFunElmDetailPara para = new EntitySystemFunElmDetail.SystemFunElmDetailPara
                {
                    ElmID = new DBVarChar(string.IsNullOrWhiteSpace(FunElmID) ? null : FunElmID),
                    SysID = new DBVarChar(string.IsNullOrWhiteSpace(SysID) ? null : SysID),
                    FunControllerID = new DBVarChar(string.IsNullOrWhiteSpace(FunControllerID) ? null : FunControllerID),
                    FunActionNM = new DBVarChar(string.IsNullOrWhiteSpace(FunActionName) ? null : FunActionName)
                };

                return _entity.DeleteSystemFunElmDetail(para) == EntitySystemFunElmDetail.EnumDeleteSystemFunElmDetailResult.Success;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 紀錄元素權限 -
        /// <summary>
        /// 紀錄元素權限
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public bool RecordLogSysSystemFunElm(string userID, EnumCultureID cultureID)
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

                MongoSystemFunElmDetail.SysSystemFunElmPara para = new MongoSystemFunElmDetail.SysSystemFunElmPara
                {
                    ElmID = new DBVarChar(FunElmID),
                    SysID = new DBVarChar(SysID),
                    SysNM = entityBasicInfo.ExecSysNM,
                    ControllerName = new DBVarChar(FunControllerID),
                    ActionName = new DBVarChar(FunActionName),
                    IsDisable = new DBChar(string.IsNullOrWhiteSpace(IsDisable) ? EnumYN.N.ToString() : EnumYN.Y.ToString()),
                    DefaultDisplaySts = new DBNVarChar(FunElmDisplayDefaultTypeDic[FunElmDisplayDefaultType.ToString()]),
                    ElmNMZHTW = new DBNVarChar(FunElmNMZHTW),
                    ElmNMZHCN = new DBNVarChar(FunElmNMZHCN),
                    ElmNMENUS = new DBNVarChar(FunElmNMENUS),
                    ElmNMTHTH = new DBNVarChar(FunElmNMTHTH),
                    ElmNMJAJP = new DBNVarChar(FunElmNMJAJP),
                    ElmNMKOKR = new DBNVarChar(FunElmNMKOKR),
                    UpdUserID = new DBVarChar(userID),
                    UpdUserNM = entityBasicInfo.UpdUserNM,
                    UpdDT = new DBDateTime(DateTime.Now)
                };

                switch (ExecAction)
                {
                    case EnumActionType.Add:
                        para.ModifyType = Mongo_BaseAP.EnumModifyType.I.ToString();
                        para.ModifyTypeNM = modifyTypeDictionary[Mongo_BaseAP.EnumModifyType.I.ToString()];
                        break;
                    case EnumActionType.Update:
                        para.ModifyType = Mongo_BaseAP.EnumModifyType.U.ToString();
                        para.ModifyTypeNM = modifyTypeDictionary[Mongo_BaseAP.EnumModifyType.U.ToString()];
                        break;
                    case EnumActionType.Delete:
                        para.ModifyType = Mongo_BaseAP.EnumModifyType.D.ToString();
                        para.ModifyTypeNM = modifyTypeDictionary[Mongo_BaseAP.EnumModifyType.D.ToString()];
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return _entityMongo.RecordLogSysSystemFunElm(para) == MongoSystemFunElmDetail.EnumRecordLogSysSystemFunElmResult.Success;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 取得使用者資料 -
        /// <summary>
        /// 取得使用者資料
        /// </summary>
        /// <param name="userIDList"></param>
        /// <returns></returns>
        public List<Entity_BaseAP.RawCMUser> GetElmRawCMUserList(List<string> userIDList)
        {
            try
            {
                var userList = new Entity_BaseAP(ConnectionStringSERP, ProviderNameSERP)
                    .SelectRawCMUserList(new Entity_BaseAP.RawCMUserPara
                    {
                        UserIDList = userIDList.Select(u => new DBVarChar(u)).ToList()
                    });

                if (userList.Any())
                {
                    return userList;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return new List<Entity_BaseAP.RawCMUser>();
        }
        #endregion

        public async Task<bool> GetSystemInfoList(string userID, EnumCultureID cultureID)
        {
            try
            {
                await _GetSystemFunControllerList(userID, cultureID);
                await _GetSystemFunActionList(cultureID);

                var result = new List<SystemInfo>
                {
                    new SystemInfo
                    {
                        Sys = new SelectListItem { Value = SysID, Text = string.Empty },
                        FunControllerList = (from s in EntitySystemFunControllerList
                                             where s.SysID == SysID
                                             select new SelectListGroupItem
                                             {
                                                 GroupID = s.SysID,
                                                 Value = s.FunControllerID,
                                                 Text = s.FunGroupNM
                                             }).ToList(),
                        FunActionList = (from s in EntitySystemFunActionList
                                         where s.SysID == SysID
                                         select new SelectListGroupItem
                                         {
                                             GroupID = $"{s.SysID}|{s.FunControllerID}",
                                             Value = s.FunAction,
                                             Text = s.FunActionNM
                                         }).ToList()
                    }
                };

                SystemInfoJsonString = new JavaScriptSerializer().Serialize(result);

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