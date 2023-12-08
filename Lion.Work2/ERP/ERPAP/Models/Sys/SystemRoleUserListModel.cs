using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemRoleUserListModel : SysModel
    {
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysID { get; set; }

        [InputType(EnumInputType.TextBoxHidden)]
        public string SysNM { get; set; }

        [InputType(EnumInputType.TextBoxHidden)]
        public string RoleID { get; set; }

        [InputType(EnumInputType.TextBoxHidden)]
        public string RoleNM { get; set; }

        private string _QueryUserID;

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string QueryUserID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_QueryUserID))
                {
                    return _QueryUserID;
                }
                return _QueryUserID.ToUpper();
            }
            set
            {
                _QueryUserID = value;
            }
        }

        [InputType(EnumInputType.TextBox)]
        public string QueryUserNM { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemRoleUserList.TabText_SystemRoleUserList,
                ImageURL=string.Empty
            }
        };

        public class SystemRoleUser
        {
            public string UserID;
            public string UserNM;
            public string UpdUserNM;
            public DateTime UpdDT;
        }

        public SystemRoleUserListModel()
        {

        }

        public List<SystemRoleUser> EntitySystemRoleUserList { get; private set; }

        public async Task<bool> GetSystemRoleUserList(int pageSize)
        {
            try
            {
                string userID = this.QueryUserID ?? null;
                string userNM = this.QueryUserNM ?? null;
                string apiUrl = API.SystemRoleUsers.QuerySystemRoleUsers(SysID, RoleID, userID, userNM, PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemRoleUserList = (List<SystemRoleUser>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    EntitySystemRoleUserList = responseObj.SystemRoleUserList;
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
    }
}