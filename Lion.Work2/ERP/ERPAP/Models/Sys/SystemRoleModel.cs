using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemRoleModel : SysModel
    {
        #region - Enum -
        public enum Field
        {
            QuerySysID, QueryRoleCategoryID
        }

        public enum EnumEditSystemRoleByCategoryResult
        {
            Success, Failure, NotExecuted
        }
        #endregion

        #region - Class -
        public class SystemRoleDetail
        {
            public string RoleCategoryID { get; set; }
            public string RoleCategoryNM { get; set; }

            public string RoleID { get; set; }
            public string RoleNM { get; set; }

            public string IsMaster { get; set; }

            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }

            public string GetPrimaryKey(string sysID)
            {
                return $"{sysID}|{RoleID}";
            }
        }
        #endregion

        #region - Property -
        [Required]
        public string QuerySysID { get; set; }

        public string QueryRoleCategoryID { get; set; }

        public string QueryRoleID { get; set; }

        public string RoleCategoryID { get; set; }

        public List<string> PickList { get; set; }

        public List<SystemRoleDetail> SystemRoleList { get; private set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            this.QuerySysID = string.Empty;
            this.QueryRoleID = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemRoleList(string userID, int pageSize, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(QuerySysID)) return true;

                string apiUrl = API.SystemRole.QuerySystemRole(QuerySysID, userID, QueryRoleID, QueryRoleCategoryID, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemRoles = (List<SystemRoleDetail>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemRoleList = responseObj.SystemRoles;

                    SetPageCount();
                }
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<EnumEditSystemRoleByCategoryResult> GetEditSystemRoleByCategoryResult(string userID, EnumCultureID cultureID)
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
                            RoleCategoryID,
                            RoleID = pickData.Split('|')[1],
                            UpdUserID = userID
                        });
                        var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                        string apiUrl = API.SystemRole.EditSystemRoleByCategory(userID, cultureID.ToString().ToUpper());
                        await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
                    }
                    return EnumEditSystemRoleByCategoryResult.Success;
                }
                return EnumEditSystemRoleByCategoryResult.NotExecuted;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return EnumEditSystemRoleByCategoryResult.Failure;
        }

        #region - Event -
        public string GetEventParaSysSystemRoleEdit(string pickData, string userID)
        {
            try
            {
                string apiUrl = API.SystemRole.QuerySystemRole(pickData.Split('|')[0], userID, pickData.Split('|')[1]);
                var response = Common.HttpWebRequestGetResponseString(apiUrl, AppSettings.APITimeOut);

                var systemRoleDetail = new
                {
                    SysID = (string)null,
                    RoleCategoryID = (string)null,
                    RoleID = (string)null,
                    RoleNMZHTW = (string)null,
                    RoleNMZHCN = (string)null,
                    RoleNMENUS = (string)null,
                    RoleNMTHTH = (string)null,
                    RoleNMJAJP = (string)null,
                    RoleNMKOKR = (string)null,
                    IsMaster = EnumYN.N.ToString()
                };

                systemRoleDetail = Common.GetJsonDeserializeAnonymousType(response, systemRoleDetail);

                if (systemRoleDetail != null)
                {
                    var entityEventParaSystemRoleEdit = new
                    {
                        TargetSysIDList = new List<string> { string.IsNullOrWhiteSpace(pickData.Split('|')[0]) ? null : pickData.Split('|')[0] },
                        systemRoleDetail.SysID,
                        systemRoleDetail.RoleCategoryID,
                        systemRoleDetail.RoleID,
                        RoleNMzhTW = systemRoleDetail.RoleNMZHTW,
                        RoleNMzhCN = systemRoleDetail.RoleNMZHCN,
                        RoleNMenUS = systemRoleDetail.RoleNMENUS,
                        RoleNMthTH = systemRoleDetail.RoleNMTHTH,
                        RoleNMjaJP = systemRoleDetail.RoleNMJAJP,
                        RoleNMkoKR = systemRoleDetail.RoleNMKOKR,
                        systemRoleDetail.IsMaster
                    };
                    return Common.GetJsonSerializeObject(entityEventParaSystemRoleEdit);
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return null;
        }
        #endregion
    }
}