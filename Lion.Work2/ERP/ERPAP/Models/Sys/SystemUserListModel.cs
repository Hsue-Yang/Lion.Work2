using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemUserListModel : SysModel
    {
        #region - Class -
        public class SystemUser
        {
            public string UserID { get; set; }
            public string UserNM { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }
        #endregion

        #region - Property -
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysID { get; set; }

        [InputType(EnumInputType.TextBoxHidden)]
        public string SysNM { get; set; }

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

        public List<SystemUser> SystemUserList { get; set; }
        #endregion

        public async Task<bool> GetSystemUserList(string userID, int pageSize)
        {
            try
            {
                string apiUrl = API.SystemSetting.QuerySystemUsers(SysID, userID, QueryUserID, QueryUserNM, PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemUsers = (List<SystemUser>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemUserList = responseObj.SystemUsers;

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