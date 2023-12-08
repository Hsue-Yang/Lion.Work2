using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;

namespace ERPAP.Models.Sys
{
    public class UserRoleFunModel : SysModel
    {
        #region - Class -
        public class UserRoleFun
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
        #endregion

        public List<UserRoleFun> UserRoleFunList { get; set; }

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

        public UserRoleFunModel()
        {
        }

        public void FormReset(string userID)
        {
            this.QueryUserID = userID;
            this.QueryUserNM = string.Empty;
        }

        public async Task<bool> GetUserRoleFunList(int pageSize)
        {
            try
            {
                string apiUrl = API.UserRoleFun.QueryUserRoleFunGroups(this.QueryUserID, this.QueryUserNM, PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SysUserRoleFunList = (List<UserRoleFun>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    UserRoleFunList = responseObj.SysUserRoleFunList;

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