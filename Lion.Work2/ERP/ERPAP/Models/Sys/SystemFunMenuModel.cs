using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemFunMenuModel : SysModel
    {
        #region - Enum -
        public enum Field
        {
            QuerySysID
        }
        #endregion

        #region - Class -
        public class SystemFunMenu
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }

            public string FunMenu { get; set; }
            public string FunMenuNM { get; set; }

            public string DefaultMenuID { get; set; }
            public string IsDisable { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }
        #endregion

        #region - Property -
        [Required]
        public string QuerySysID { get; set; }

        public List<SystemFunMenu> SystemFunMenuList { get; private set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            QuerySysID = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemFunMenuList(string userID, int pageSize, EnumCultureID cultureID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(QuerySysID)) return true;

                string apiUrl = API.SystemFunMenu.QuerySystemFunMenus(QuerySysID, userID, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemFunMenus = (List<SystemFunMenu>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemFunMenuList = responseObj.SystemFunMenus;

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