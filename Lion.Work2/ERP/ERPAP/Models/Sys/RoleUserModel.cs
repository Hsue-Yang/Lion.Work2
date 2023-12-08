using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;

namespace ERPAP.Models.Sys
{
    public class RoleUserModel : SysModel
    {
        #region - Definitions -
        public new enum EnumCookieKey
        {
            SysID,
            RoleCategoryID,
            RoleID
        }
        #endregion

        #region - Constructor -
        public RoleUserModel()
        {
            _entity = new EntityRoleUser(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string SysID { set; get; }
        public string RowDataInfo { get; set; }
        public string RoleCategoryID { get; set; }
        public string RoleID { get; set; }
        [Required]
        [StringLength(8)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string ErpWFNo { get; set; }
        [StringLength(1000)]
        public string Memo { get; set; }
        public List<EntityRoleUser.RoleUser> RoleUserList { get; private set; }
        public List<string> UserChangeList { get; set; }
        #endregion

        #region - Private -
        private readonly EntityRoleUser _entity;
        #endregion

        public class RoleUser
        {
            public string UserID { get; set; }
            public string UserIDNM { get; set; }
            public string ComIDNM { get; set; }
            public string UserDeptIDNM { get; set; }
            public string UnitIDNM { get; set; }
            public string UpdUserID { get; set; }
            public string UpdUserIDNM { get; set; }
            public DateTime UpdDT { get; set; }
        }
        public List<RoleUser> RoleUsers { get; set; }

        public void FormReset()
        {
            if (string.IsNullOrWhiteSpace(SysID) == false &&
                string.IsNullOrWhiteSpace(RoleID) == false)
            {
                RowDataInfo = string.Format("{0}|{1}", SysID, RoleID);
            }
        }

        #region - 取得角色使用者權限清單 -
        /// <summary>
        /// 取得角色使用者權限清單
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetRoleUserList(EnumCultureID cultureID)
        {
            try
            {
                SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID;
                RoleID = string.IsNullOrWhiteSpace(RoleID) ? null : RoleID;

                string apiUrl = API.RoleUser.QueryRoleUsers(SysID, RoleID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RoleUserList = (List<RoleUser>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                RoleUsers = responseObj.RoleUserList;

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }
        #endregion

        #region - 取得角色使用者權限設定參數 -
        /// <summary>
        /// 取得角色使用者權限設定參數
        /// </summary>
        /// <returns></returns>
        public string GetAuthRoleUserParaJsonString()
        {
            var sysID = string.Empty;
            var roleID = string.Empty;

            if (RowDataInfo != null)
            {
                var infoArray = RowDataInfo.Split('|');
                sysID = infoArray[0];
                roleID = infoArray[1];
            }

            var result = new
            {
                SysID = string.IsNullOrWhiteSpace(sysID) ? null : sysID,
                RoleID = string.IsNullOrWhiteSpace(roleID) ? null : roleID,
                ErpWFNo = string.IsNullOrWhiteSpace(ErpWFNo) ? null : ErpWFNo,
                Memo = string.IsNullOrWhiteSpace(Memo) ? null : Memo,
                IsOverride = true,
                UserIDList = UserChangeList ?? new List<string>()
            };

            return new JavaScriptSerializer().Serialize(result);
        }
        #endregion
    }
}