// 新增日期：2018-01-19
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Collections.Generic;
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
    public class SystemRoleElmListModel : SysModel
    {
        #region - Definitions -
        public new enum EnumCookieKey
        {
            FunControllerID,
            FunActionName
        }
        public enum EnumDisplay
        {
            Show = 1,
            ReadOnly = 2,
            Mask = 3,
            Hide = 4
        }

        public class SysRoleElm
        {
            public string ElmID { set; get; }
            public string FunControllerID { set; get; }
            public string FunActionName { set; get; }
            public string DisplaySts { set; get; }
            public string IsAdd { set; get; }
        }

        public class SystemRoleElmEdit
        {
            public string SYS_ID { get; set; }
            public string ROLE_ID { get; set; }
            public string FUN_CONTROLLER_ID { get; set; }
            public string FUN_ACTION_NAME { get; set; }
            public string ELM_ID { get; set; }
            public int DISPLAY_STS { get; set; }
            public string UPD_USER_ID { get; set; }

        }

        public class SystemRoleElmEditLists
        {
            public List<SystemRoleElmEdit> SystemRoleElmAddList = new List<SystemRoleElmEdit>();
            public List<SystemRoleElmEdit> SystemRoleElmDeleteList = new List<SystemRoleElmEdit>();
        }

        public class SystemRoleElms
        {
            public string FunControllerID { get; set; }
            public string FunActionNM { get; set; }
            public string ElmID { get; set; }
            public string ElmNM { get; set; }
            public string IsDisable { get; set; }
            public string DisplaySts { get; set; }
            public string UpdUserIDNM { get; set; }
            public DateTime UpdDT { get; set; }
        }

        public class SysElmName : ISelectItem
        {
            public string ElmNM { get; set; }
            public string ElmID { get; set; }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }

            public string ItemText()
            {
                return $"{ElmNM} ({ElmID})";
            }

            public string ItemValue()
            {
                return ElmID;
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region - Constructor -
        public SystemRoleElmListModel()
        {
        }
        #endregion

        #region - Property -
        public string SysID { set; get; }
        public string SysNM { get; set; }
        public string RoleID { set; get; }
        public string RoleNM { set; get; }
        public string FunControllerID { get; set; }
        public string FunActionName { get; set; }
        public string SystemInfoJsonString { get; set; }
        public List<SysRoleElm> SysRoleElmList { get; set; }
        public List<SysElmName> SystemElmIDList { get; set; }
        private Dictionary<string, string> _displayDictionary;

        public Dictionary<string, string> DisplayDictionary => _displayDictionary ?? (_displayDictionary = new Dictionary<string, string>
        {
            { string.Empty,string.Empty},
            { ((int)EnumDisplay.Show).ToString(), SysSystemRoleElmList.Label_Show },
            { ((int)EnumDisplay.ReadOnly).ToString(),SysSystemRoleElmList.Label_ReadOnly },
            { ((int)EnumDisplay.Mask).ToString(), SysSystemRoleElmList.Label_Mask },
            { ((int)EnumDisplay.Hide).ToString(), SysSystemRoleElmList.Label_Hide }
        });
        public Dictionary<string, string> SystemFunControllerDic { get; set; }
        public Dictionary<string, string> SystemFunActionDic { get; set; }

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

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText= SysSystemRoleElmList.TabText_SystemRoleElmList,
                ImageURL=string.Empty
            }
        };

        public List<SystemRoleElms> SystemRoleElmList { get; private set; }
        #endregion

        public void FormReset()
        {
            EntitySystemFunActionList.Insert(0, new SysModel.SystemFunAction
            {
                SysID = SysID,
                FunControllerID = string.IsNullOrWhiteSpace(FunControllerID) ? string.Empty : FunControllerID,
                FunAction = string.Empty,
                FunActionNM = string.Empty
            });

            SystemFunControllerDic = GetDictionaryFormSelectItem(EntitySysSystemFunControllerIDList.ToDictionary(p => p.ItemValue(), p => p.ItemText()), true);

            SystemFunActionDic = EntitySystemFunActionList.Where(s => s.SysID == SysID && s.FunControllerID == FunControllerID)
                                                          .ToDictionary(k => k.FunAction, v => v.FunActionNM);
        }

        #region - 取得功能元素清單 -
        /// <summary>
        /// 取得功能元素清單
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemRoleElmList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string funControllerID = string.IsNullOrWhiteSpace(FunControllerID) ? null : FunControllerID;
                string funactionNM = string.IsNullOrWhiteSpace(FunActionName) ? null : FunActionName;
                string apiUrl = API.SystemRoleElms.QuerySystemRoleElms(userID,SysID, RoleID, cultureID.ToString().ToUpper(), funControllerID, funactionNM, PageIndex, PageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    rowCount = 0,
                    systemRoleElmList = (List<SystemRoleElms>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.rowCount;
                    SystemRoleElmList = responseObj.systemRoleElmList;
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

        #region - 取得系統資訊清單 -
        /// <summary>
        /// 取得系統資訊清單
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemInfoList(string userID, EnumCultureID cultureID)
        {
            try
            {
                await _GetSystemFunActionList(cultureID);

                var result = new List<SystemInfo>
                {
                    new SystemInfo
                    {
                        Sys = new SelectListItem { Value = SysID, Text = string.Empty },
                        FunControllerList = (from s in EntitySysSystemFunControllerIDList
                                             select new SelectListGroupItem
                                             {
                                                 GroupID = SysID,
                                                 Value = s.ItemValue(),
                                                 Text = s.ItemText()
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
        #endregion

        #region - 取得元素代碼清單 -
        /// <summary>
        /// 取得元素代碼清單
        /// </summary>
        /// <param name="sysID"></param>
        /// <param name="controllerID"></param>
        /// <param name="actionName"></param>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemElmIDList(string userID, string sysID, string controllerID, string actionName, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemRoleElms.QuerySystemFunElmIds(userID, sysID, cultureID.ToString().ToUpper(), controllerID, actionName);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    systemElmIDList = (List<SysElmName>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    SystemElmIDList = responseObj.systemElmIDList;
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

        #region - 編輯功能元素清單 -
        /// <summary>
        /// 編輯功能元素清單
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<bool> EditSystemRoleElmList(string userID)
        {
            try
            {
                SystemRoleElmEditLists systemRoleElmEditLists = new SystemRoleElmEditLists();
                if (SysRoleElmList == null)
                {
                    SysRoleElmList = new List<SysRoleElm>();
                }

                foreach (var item in SysRoleElmList)
                {
                    if (item.IsAdd == "Y")
                    {
                        systemRoleElmEditLists.SystemRoleElmAddList.Add(new SystemRoleElmEdit
                        {
                            SYS_ID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                            ROLE_ID = string.IsNullOrWhiteSpace(RoleID) ? null : RoleID,
                            FUN_CONTROLLER_ID = string.IsNullOrWhiteSpace(item.FunControllerID) ? null : item.FunControllerID,
                            FUN_ACTION_NAME = string.IsNullOrWhiteSpace(item.FunActionName) ? null : item.FunActionName,
                            DISPLAY_STS = int.Parse(item.DisplaySts),
                            ELM_ID = string.IsNullOrWhiteSpace(item.ElmID) ? null : item.ElmID,
                            UPD_USER_ID = string.IsNullOrWhiteSpace(userID) ? null : userID
                        });
                    }
                    else
                    {
                        systemRoleElmEditLists.SystemRoleElmDeleteList.Add(new SystemRoleElmEdit
                        {
                            SYS_ID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                            ROLE_ID = string.IsNullOrWhiteSpace(RoleID) ? null : RoleID,
                            FUN_CONTROLLER_ID = string.IsNullOrWhiteSpace(item.FunControllerID) ? null : item.FunControllerID,
                            FUN_ACTION_NAME = string.IsNullOrWhiteSpace(item.FunActionName) ? null : item.FunActionName,
                            DISPLAY_STS = 0,
                            ELM_ID = string.IsNullOrWhiteSpace(item.ElmID) ? null : item.ElmID,
                            UPD_USER_ID = string.IsNullOrWhiteSpace(userID) ? null : userID
                        });
                    }
                }

                if ((systemRoleElmEditLists.SystemRoleElmAddList != null && systemRoleElmEditLists.SystemRoleElmAddList.Any())
                    || (systemRoleElmEditLists.SystemRoleElmDeleteList != null && systemRoleElmEditLists.SystemRoleElmDeleteList.Any()))
                {
                    Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                    var paraJsonStr = Common.GetJsonSerializeObject(systemRoleElmEditLists);
                    var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                    string apiUrl = API.SystemRoleElms.EditSystemRoleElms(userID,SysID, RoleID);
                    await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
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