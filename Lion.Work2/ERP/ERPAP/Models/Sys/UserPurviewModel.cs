using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class UserPurviewModel : SysModel
    {
        public class SysUserPurview
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string PurviewID { get; set; }
            public string PurviewNM { get; set; }
            public bool HasDataPurAuth { get; set; }
            public bool HasUpdateAuth { get; set; }
        }
        public List<SysUserPurview> SysUserPurviews { get; set; }

        #region - Constructor -
        public UserPurviewModel()
        {
            _entity = new EntityUserPurview(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string SysID { get; set; }

        public string SysNM { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string UserID { get; set; }

        [InputType(EnumInputType.TextBox)]
        public string UserNM { get; set; }

        public string UpdUserID { get; set; }

        public List<EntityUserPurview.SysUserPurview> SysUserPurviewList { get; set; }
        #endregion

        #region - Private -
        private readonly EntityUserPurview _entity;
        #endregion

        #region - 取得使用者資料權限清單 -
        /// <summary>
        /// 取得使用者資料權限清單
        /// </summary>
        /// <param name="cultureId"></param>
        /// <returns></returns>
        public async Task<bool> GetSysUserPurviewList(string updUserId, EnumCultureID cultureId)
        {
            try
            {
                UserID = string.IsNullOrWhiteSpace(UserID) ? null : UserID;
                UpdUserID = string.IsNullOrWhiteSpace(updUserId) ? null : updUserId;

                string apiUrl = API.SystemUserPurview.QuerySysUserPurviews(UserID, UpdUserID, cultureId.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SysUserPurviewList = (List<SysUserPurview>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                SysUserPurviews = responseObj.SysUserPurviewList;

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