using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class UserSystemModel : SysModel
    {
        public class UserSystems
        {
            public string UserID { get; set; }
            public string UserNM { get; set; }
            public string UnitID { get; set; }
            public string UnitNM { get; set; }
            public string IsLeft { get; set; }
            public string IsDisable { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }

        public enum Field
        {
            QueryUserID, QueryUserNM
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

        public UserSystemModel()
        {
        }

        public void FormReset(string userID)
        {
            this.QueryUserID = userID;
            this.QueryUserNM = string.Empty;
        }

        List<UserSystems> _entityUserSystemList;
        public List<UserSystems> EntityUserSystemList { get { return _entityUserSystemList; } }

        public async Task<bool> GetUserSystemList(int pageSize)
        {
            try
            {
                string userID = string.IsNullOrWhiteSpace(QueryUserID) ? null : QueryUserID;
                string userNM = string.IsNullOrWhiteSpace(QueryUserNM) ? null : QueryUserNM;

                string apiUrl = API.UserSystem.QueryUserStatusList(userID, userNM, PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    UserSystems = (List<UserSystems>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if(responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    _entityUserSystemList = responseObj.UserSystems;

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