using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemRoleFunListModel : SysModel
    {
        #region - Definition -
        public class SystemRoleFun
        {
            public string FunActionName { get; set; }
            public string FunControllerID { get; set; }
            public string IsAdd { get; set; }
        }

        public class SystemRoleFunEdit
        {
            public string SysID { get; set; }
            public string RoleID { get; set; }
            public string FunControllerID { get; set; }
            public string FunActionName { get; set; }
            public string UpdUserID { get; set; }
        }

        public class SystemRoleFunEditLists
        {
            public List<SystemRoleFunEdit> SystemRoleFunsAddList = new List<SystemRoleFunEdit>();

            public List<SystemRoleFunEdit> SystemRoleFunsDeleteList = new List<SystemRoleFunEdit>();
        }

        public class SystemRoleFunsList
        {
            public string SubSysNM { get; set; }
            public string SysID { get; set; }
            public string FunGroupNM { get; set; }
            public string FunActionNMID { get; set; }
            public string FunControllerID { get; set; }
            public string FunActionName { get; set; }
            public string UpdUserNM { get; set; }
            public string UpdUserID { get; set; }
            public DateTime UpdDT { get; set; }
        }

        #endregion

        #region - Property -
        public string SysID { get; set; }

        public string SysNM { get; set; }

        public string QueryFunControllerID { get; set; }

        public string RoleID { get; set; }
        public string RoleNM { set; get; }

        public string SystemInfoJsonString { get; set; }

        public List<SystemRoleFun> SysRoleFunList { get; set; }

        public List<SystemRoleFunsList> EntitySystemRoleFunList { get; private set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName = string.Empty,
                ActionName = string.Empty,
                TabText = SysSystemRoleFunList.TabText_SystemRoleFunList,
                ImageURL = string.Empty
            }
        };
        #endregion

        #region - 取得系統角色功能清單 -
        /// <summary>
        /// 取得系統角色功能清單
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemRoleFunList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string funControllerID = string.IsNullOrWhiteSpace(this.QueryFunControllerID) ? null : this.QueryFunControllerID;
                string apiUrl = API.SystemRoleFuns.QuerySystemRoleFunsList(SysID, userID, RoleID, funControllerID, cultureID.ToString().ToUpper(), PageIndex, PageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    rowCount = 0,
                    systemRoleFunsList = (List<SystemRoleFunsList>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.rowCount;
                    EntitySystemRoleFunList = responseObj.systemRoleFunsList;
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

        #region - 編輯系統角色功能清單 -
        /// <summary>
        /// 編輯系統角色功能清單
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<bool> EditSystemRoleFunList(string userID)
        {
            try
            {
                SystemRoleFunEditLists systemRoleFunEditLists = new SystemRoleFunEditLists();
                if (SysRoleFunList == null)
                {
                    SysRoleFunList = new List<SystemRoleFun>();
                }

                foreach (var item in SysRoleFunList)
                {
                    if (item.IsAdd == "Y")
                    {
                        systemRoleFunEditLists.SystemRoleFunsAddList.Add(new SystemRoleFunEdit
                        {
                            SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                            RoleID = string.IsNullOrWhiteSpace(RoleID) ? null : RoleID,
                            FunControllerID = string.IsNullOrWhiteSpace(item.FunControllerID) ? null : item.FunControllerID,
                            FunActionName = string.IsNullOrWhiteSpace(item.FunActionName) ? null : item.FunActionName,
                            UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID
                        });
                    }
                    else
                    {
                        systemRoleFunEditLists.SystemRoleFunsDeleteList.Add(new SystemRoleFunEdit
                        {
                            SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                            RoleID = string.IsNullOrWhiteSpace(RoleID) ? null : RoleID,
                            FunControllerID = string.IsNullOrWhiteSpace(item.FunControllerID) ? null : item.FunControllerID,
                            FunActionName = string.IsNullOrWhiteSpace(item.FunActionName) ? null : item.FunActionName,
                            UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID
                        });
                    }
                }

                if ((systemRoleFunEditLists.SystemRoleFunsAddList != null && systemRoleFunEditLists.SystemRoleFunsAddList.Any()) 
                    || (systemRoleFunEditLists.SystemRoleFunsDeleteList != null && systemRoleFunEditLists.SystemRoleFunsDeleteList.Any()))
                {
                    Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                    var paraJsonStr = Common.GetJsonSerializeObject(systemRoleFunEditLists);
                    var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                    string apiUrl = API.SystemRoleFuns.EditSystemRoleFunsList(SysID, RoleID, userID);
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

        #region - 取得系統功能JSON字串 -
        /// <summary>
        /// 取得系統功能JSON字串
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetSystemFunJsonStr( EnumCultureID cultureID)
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
    }
}