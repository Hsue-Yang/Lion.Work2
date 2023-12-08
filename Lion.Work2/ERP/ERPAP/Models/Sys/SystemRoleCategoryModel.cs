using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;

namespace ERPAP.Models.Sys
{
    public class SystemRoleCategoryModel : SysModel
    {
        #region - Enum - 
        public enum Field
        {
            SysID, RoleCategoryNM
        }
        #endregion

        #region - Class -
        public class SysRoleCategory
        {
            public string SysID { get; set; }
            public string RoleCategoryID { get; set; }
            public string RoleCategoryNM { get; set; }

            public string SortOrder { get; set; }

            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }
        #endregion

        #region - Property -
        [Required]
        public string SysID { get; set; }

        public string RoleCategoryNM { get; set; }

        public List<SysRoleCategory> SystemRoleCategoryList { get; private set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            this.SysID = string.Empty;
            this.RoleCategoryNM = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemRoleCategoryList(string userID, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SysID)) return true;
                string apiUrl = API.SystemRoleCategory.QuerySystemRoleCategories(SysID, userID, RoleCategoryNM, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemRoleCategoryList = Common.GetJsonDeserializeObject<List<SysRoleCategory>>(response);

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