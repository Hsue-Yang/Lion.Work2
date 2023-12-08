using LionTech.Entity;
using LionTech.Entity.ERP;
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
using System.Web.Mvc;

namespace ERPAP.Models.Sys
{
    public class SystemFunModel : SysModel
    {
        #region - Enum -
        public enum Field
        {
            QuerySysID,
            QuerySubSysID,
            QueryFunControllerID,
            QueryFunGroupNM,
            QueryFunActionName,
            QueryFunName,
            QueryFunMenuSysID,
            QueryFunMenu
        }

        public enum EnumEditSystemFunByPurviewResult
        {
            Success, Failure, NotExecuted
        }
        #endregion

        #region - Class -
        public class SystemFun
        {
            public string SysID { get; set; }

            public string SubSysID { get; set; }
            public string SubSysNM { get; set; }

            public string FunControllerID { get; set; }
            public string FunGroupNM { get; set; }

            public string FunActionName { get; set; }
            public string FunNM { get; set; }

            public string IsDisable { get; set; }
            public string IsOutside { get; set; }
            public string SortOrder { get; set; }

            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }

            public List<SysMenuFun> MenuFunList { get; set; }

            public string GetPrimaryKey(string sysID)
            {
                return $"{sysID}|{FunControllerID}|{FunActionName}";
            }

            public string GetFullURL()
            {
                return $"{Common.GetEnumDesc(Utility.GetSystemID(SubSysID))}/{FunControllerID}/{FunActionName}";
            }
        }

        public class SystemFunMain
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }

            public string SubSysID { get; set; }
            public string SubSysNM { get; set; }

            public string PurviewID { get; set; }
            
            public string FunControllerID { get; set; }
            public string FunGroupNM { get; set; }

            public string FunActionName { get; set; }
            public string FunNMZHTW { get; set; }
            public string FunNMZHCN { get; set; }
            public string FunNMENUS { get; set; }
            public string FunNMTHTH { get; set; }
            public string FunNMJAJP { get; set; }
            public string FunNMKOKR { get; set; }

            public string FunType { get; set; }
            public string FunTypeNM { get; set; }

            public string IsDisable { get; set; }
            public string IsOutside { get; set; }
            public string SortOrder { get; set; }
        }

        public class SysMenuFun
        {
            public string FunMenu { get; set; }
            public string FunMenuNM { get; set; }

            public string FunControllerID { get; set; }
            public string FunActionName { get; set; }

            public string FunMenuIsDisable { get; set; }
        }
        #endregion

        #region - Property -
        [Required]
        public string QuerySysID { get; set; }

        public string QuerySubSysID { get; set; }

        public string QueryFunControllerID { get; set; }

        [StringLength(10)]
        [InputType(EnumInputType.TextBox)]
        public string QueryFunGroupNM { get; set; }

        public string QueryFunActionName { get; set; }

        [StringLength(10)]
        [InputType(EnumInputType.TextBox)]
        public string QueryFunNM { get; set; }

        public string QueryFunMenuSysID { get; set; }

        public string QueryFunMenu { get; set; }

        public string PurviewID { get; set; }

        public List<string> PickList { get; set; }

        public string SystemInfoJsonString;

        public List<SystemFun> SystemFunList { get; private set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            QuerySysID = string.Empty;
            QuerySubSysID = string.Empty;
            QueryFunControllerID = string.Empty;
            QueryFunGroupNM = string.Empty;
            QueryFunActionName = string.Empty;
            QueryFunNM = string.Empty;
            QueryFunMenuSysID = string.Empty;
            QueryFunMenu = string.Empty;
            PickList = new List<string>();
        }
        #endregion

        public async Task<bool> GetSystemInfoList(string userID, EnumCultureID cultureID)
        {
            try
            {
                Task task1 = GetUserSystemByIdList(userID, true, cultureID);
                Task task2 = _GetUserSystemSubList(userID, cultureID);
                Task task3 = _GetUserSystemFunGroupList(userID, cultureID);
                Task task4 = _GetUserSystemFunList(userID, cultureID);
                await Task.WhenAll(task1, task2, task3, task4);

                if (UserSystemByIdList != null && UserSystemByIdList.Any())
                {
                    const string formatText = "{0} ({1})";
                    var result = new List<SystemInfo>();
                    UserSystemByIdList.ForEach(u =>
                    {
                        var item = new SystemInfo();
                        item.Sys = new SelectListItem
                        {
                            Value = u.SysID,
                            Text = string.Format(formatText, u.SysNM, u.SysID)
                        };
                        item.SubSysList =
                            (from s in UserSystemSubList
                             where s.SysID == u.SysID
                             select new SelectListItem
                             {
                                 Value = s.SubSysID,
                                 Text = string.Format(formatText, s.SubSysNM, s.SubSysID)
                             }).ToList();

                        item.FunControllerList =
                            (from c in UserSystemFunGroupList
                             where c.SysID == u.SysID
                             select new SelectListGroupItem
                             {
                                 GroupID = c.SysID,
                                 Value = c.FunControllerID,
                                 Text = string.Format(formatText, c.FunGroupNM, c.FunControllerID)
                             }).ToList();

                        item.FunActionList =
                            (from a in UserSystemFunList
                             where a.SysID == u.SysID
                             select new SelectListGroupItem
                             {
                                 GroupID = string.Format("{0}|{1}", a.SysID, a.FunControllerID),
                                 Value = a.FunActionName,
                                 Text = string.Format(formatText, a.FunNM, a.FunActionName)
                             }).ToList();
                        result.Add(item);
                    });
                    SystemInfoJsonString = Common.GetJsonSerializeObject(result);
                }
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetSystemFunList(string userID, int pageSize, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(QuerySysID)) return true;

                string apiUrl = API.SystemFun.QuerySystemFuns(QuerySysID, userID, QuerySubSysID, QueryFunControllerID, QueryFunActionName, QueryFunGroupNM, QueryFunNM, QueryFunMenuSysID, QueryFunMenu, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemFuns = (List<SystemFun>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemFunList = responseObj.SystemFuns;

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

        public async Task<EnumEditSystemFunByPurviewResult> GetEditSystemFunByPurviewResult(string userID)
        {
            try
            {
                if (PickList != null && PickList.Any())
                {
                    Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                    {
                        {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                    };

                    foreach (string pickData in PickList)
                    {
                        var paraJsonStr = Common.GetJsonSerializeObject(new
                        {
                            SysID = pickData.Split('|')[0],
                            PurviewID,
                            FunControllerID = pickData.Split('|')[1],
                            FunActionName = pickData.Split('|')[2],
                            UpdUserID = userID
                        });
                        var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                        string apiUrl = API.SystemFun.EditSystemFunByPurview(userID);
                        await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
                    }
                    return EnumEditSystemFunByPurviewResult.Success;
                }
                return EnumEditSystemFunByPurviewResult.NotExecuted;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return EnumEditSystemFunByPurviewResult.Failure;
        }

        #region - Event -
        public async Task<string> GetEventParaSysSystemFunEdit(string userID, string pickData, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemFun.QuerySystemFun(pickData.Split('|')[0], userID, pickData.Split('|')[1], pickData.Split('|')[2], cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var systemFunDetail = Common.GetJsonDeserializeObject<SystemFunMain>(response);

                await GetSystemFunRoleList(pickData.Split('|')[0], userID, pickData.Split('|')[1], pickData.Split('|')[2], cultureID);

                if (systemFunDetail != null)
                {
                    var eventParaSystemFunEdit = new
                    {
                        TargetSysIDList = new List<string> { pickData.Split('|')[0] },
                        systemFunDetail.SysID,
                        systemFunDetail.PurviewID,
                        systemFunDetail.FunControllerID,
                        systemFunDetail.FunActionName,
                        FunNMzhTW = systemFunDetail.FunNMZHTW,
                        FunNMzhCN = systemFunDetail.FunNMZHCN,
                        FunNMenUS = systemFunDetail.FunNMENUS,
                        FunNMthTH = systemFunDetail.FunNMTHTH,
                        FunNMjaJP = systemFunDetail.FunNMJAJP,
                        FunNMkoKR = systemFunDetail.FunNMKOKR,
                        systemFunDetail.IsOutside,
                        systemFunDetail.IsDisable,
                        systemFunDetail.SortOrder,
                        RoleIDList = new List<string>()
                    };

                    if (SystemRoleFunList != null && SystemRoleFunList.Any())
                    {
                        foreach (var role in SystemRoleFunList)
                        {
                            if (role.HasRole == EnumYN.Y.ToString())
                            {
                                eventParaSystemFunEdit.RoleIDList.Add(role.RoleID);
                            }
                        }
                    }
                    return Common.GetJsonSerializeObject(eventParaSystemFunEdit);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return null;
        }
        #endregion
    }
}