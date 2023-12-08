using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Web.ERPHelper;
using Resources;
using LionTech.Entity.ERP;
using LionTech.Utility.ERP;
using LionTech.Utility;
using System.Threading.Tasks;


namespace ERPAP.Models.Sys
{
    public class UserPermissionModel : SysModel
    {
        #region - Class -

        public class UserPermission
        {
            public string UserID { get; set; }
            public string UserNM { get; set; }
            public string UnitID { get; set; }
            public string UnitNM { get; set; }
            public string RestrictType { get; set; }
            public string RestrictTypeNM { get; set; }
            public string IsLock { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }

        #endregion

        public enum Field
        {
            QueryUserID, QueryUserNM, QueryRestrictType
        }

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

        public string QueryRestrictType { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName = string.Empty,
                ActionName = string.Empty,
                TabText = SysUserPermission.TabText_UserPermission,
                ImageURL = string.Empty
            }
        };

        public UserPermissionModel()
        {
        }

        public void FormReset(string userID)
        {
            this.QueryUserID = userID;
            this.QueryUserNM = string.Empty;
        }

        public List<UserPermission> UserPermissionLists { get; private set; }

        public async Task<bool> GetUserPermissionList(int pageSize, EnumCultureID cultureID)
        {
            try
            {
                string userID = this.QueryUserID ?? null;
                string userNM = this.QueryUserNM ?? string.Empty;
                string restrictType = this.QueryRestrictType ?? string.Empty;

                string apiUrl = API.UserPermission.QueryUserPermissionList(userID, userNM, restrictType, cultureID.ToString(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    UserPermissionLists = (List<UserPermission>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    UserPermissionLists = responseObj.UserPermissionLists;
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