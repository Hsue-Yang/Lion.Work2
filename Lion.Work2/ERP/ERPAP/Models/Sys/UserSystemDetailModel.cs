using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class UserSystemDetailModel : SysModel
    {
        public class UserRawData
        {
            public string UserID { get; set; }
            public string UserNM { get; set; }
        }

        public class UserSystemRole
        {
            public string DeptID { get; set; }
            public string DeptNM { get; set; }
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string HasSys { get; set; }
        }

        public class UserSystemRolePara
        {
            public string UserID { get; set; }
            public string UpdUserID { get; set; }
            public string SysID { get; set; }
            public string CultureID { get; set; }
        }

        public string UserID { get; set; }

        public List<string> HasSys { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysUserSystemDetail.TabText_UserSystemDetail,
                ImageURL=string.Empty
            }
        };

        public UserSystemDetailModel()
        {

        }

        public void FormReset()
        {
            this.HasSys = new List<string>();
        }

        UserRawData _entityUserRawData;
        public UserRawData EntityUserRawData { get { return _entityUserRawData; } }

        public async Task<bool> GetUserRawData()
        {
            try
            {
                string userID = string.IsNullOrWhiteSpace(UserID) ? null : UserID;

                string apiUrl = API.UserSystem.QueryUserRawData(userID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    UserRawData = (UserRawData)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                _entityUserRawData = responseObj.UserRawData;

                if (_entityUserRawData != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<UserSystemRole> _entityUserSystemRoleList;
        public List<UserSystemRole> EntityUserSystemRoleList { get { return _entityUserSystemRoleList; } }

        public async Task<bool> GetUserSystemRoleList(EnumCultureID cultureID)
        {
            try
            {
                string userID = string.IsNullOrWhiteSpace(UserID) ? null : UserID;

                string apiUrl = API.UserSystem.QueryUserOutSourcingSystemRoles(userID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    UserSystemRoles = (List<UserSystemRole>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                _entityUserSystemRoleList = responseObj.UserSystemRoles;

                if (_entityUserSystemRoleList != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetEditUserSystemRoleResult(string userID, EnumCultureID cultureID)
        {
            try
            {
                var paraList = new List<string>();
                if (this.HasSys != null && this.HasSys.Count > 0)
                {
                    foreach (string sysString in this.HasSys)
                    {
                        paraList.Add(sysString);
                    }
                    paraList.Sort();
                }

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                var paraJsonStr = Common.GetJsonSerializeObject(
                   new
                   {
                       UserID = string.IsNullOrWhiteSpace(UserID) ? null : UserID,
                       UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID,
                       UserSystemRoleList = paraList
                   });

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.UserSystem.EditUserOutSourcingSystemRoles();
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
    }
}