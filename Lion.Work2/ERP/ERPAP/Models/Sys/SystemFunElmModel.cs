// 新增日期：2018-01-09
// 新增人員：廖先駿
// 新增內容：元素權限
// ---------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;

namespace ERPAP.Models.Sys
{
    public class SystemFunElmModel : SysModel
    {
        #region - Definitions -
        public new enum EnumCookieKey
        {
            SysID,
            FunControllerID,
            FunActionName,
            FunElmID,
            FunElmNM,
            IsDisable
        }
        #endregion

        #region - Constructor -
        public SystemFunElmModel()
        {
            _entity = new EntitySystemFunElm(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        [Required]
        public string SysID { set; get; }
        public string FunControllerID { set; get; }
        public string FunActionName { set; get; }
        public string FunElmID { get; set; }
        public string FunElmNM { get; set; }
        public string IsDisable { get; set; }
        public string SystemInfoJsonString { get; set; }
        public List<SystemFunElm> SystemFunElmList { get; private set; }

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
        private readonly EntitySystemFunElm _entity;
        #endregion
        
        #region - 取得元素權限清單 -
        /// <summary>
        /// 取得元素權限清單
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetSysFunElmList(string userID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SysID)) return true;

                string elmID = string.IsNullOrWhiteSpace(FunElmID) ? null : FunElmID;
                string elmName = string.IsNullOrWhiteSpace(FunElmNM) ? null : FunElmNM;
                string funControllerID = string.IsNullOrWhiteSpace(FunControllerID) ? null : FunControllerID;
                string funActionNM = string.IsNullOrWhiteSpace(FunActionName) ? null : FunActionName;
                string isDisable = string.IsNullOrWhiteSpace(IsDisable) ? EnumYN.N.ToString() : null;

                string apiUrl = API.SysFunElm.QuerySysFunElmList(SysID, userID, isDisable, elmID, elmName, funControllerID, funActionNM, cultureID.ToString().ToUpper(), PageIndex, PageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemFunElmList = (List<SystemFunElm>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemFunElmList = responseObj.SystemFunElmList;

                    SetPageCount();
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

        public async Task<bool> GetSystemInfoList(string userID, EnumCultureID cultureID)
        {
            try
            {
                await GetUserSystemSysIDList(userID, true, cultureID);
                await _GetSystemFunControllerList(userID, cultureID);
                await _GetSystemFunActionList(cultureID);

                if (EntityUserSystemSysIDList != null &&
                    EntityUserSystemSysIDList.Any())
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var result = new List<SystemInfo>();

                    EntityUserSystemSysIDList.ForEach(sys =>
                    {
                        var item = new SystemInfo
                        {
                            Sys = new SelectListItem { Value = sys.SysID, Text = sys.SysNM },
                            FunControllerList = (from s in EntitySystemFunControllerList
                                                 where s.SysID == sys.SysID
                                                 select new SelectListGroupItem
                                                 {
                                                     GroupID = s.SysID,
                                                     Value = s.FunControllerID,
                                                     Text = s.FunGroupNM
                                                 }).ToList(),
                            FunActionList = (from s in EntitySystemFunActionList
                                             where s.SysID == sys.SysID
                                             select new SelectListGroupItem
                                             {
                                                 GroupID = $"{s.SysID}|{s.FunControllerID}",
                                                 Value = s.FunAction,
                                                 Text = s.FunActionNM
                                             }).ToList()
                        };

                        result.Add(item);
                    });

                    SystemInfoJsonString = js.Serialize(result);
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