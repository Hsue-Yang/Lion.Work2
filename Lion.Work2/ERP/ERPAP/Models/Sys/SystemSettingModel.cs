using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemSettingModel : SysModel
    {
        public class SystemSetting
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }

            public string SysMANUserID { get; set; }
            public string SysMANUserNM { get; set; }

            public string IsAP { get; set; }
            public string IsAPI { get; set; }
            public string IsEDI { get; set; }
            public string IsEvent { get; set; }

            public string SysIndexPath { get; set; }
            public string IsOutsourcing { get; set; }
            public string IsDisable { get; set; }
            public string SortOrder { get; set; }

            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }

            public List<Subsystem> SubsystemList { get; set; }
        }

        public class Subsystem
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }

            public string AP { get; set; }
            public string API { get; set; }
            public string EDI { get; set; }
            public string Event { get; set; }

            public string SysMANUserNM { get; set; }

            public string ParentSysID { get; set; }
            public string SortOrder { get; set; }

            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }

        #region - Property -
        public List<SystemSetting> SystemSettingList { get; private set; }
        #endregion

        public async Task<bool> GetUserSystemsAsync(string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemSetting.UserSystems(userID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    IsITManager = false,
                    SystemSettings = (List<SystemSetting>)null
                };
                
                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);
                
                if (responseObj != null)
                {
                    IsITManager = responseObj.IsITManager;
                    SystemSettingList = responseObj.SystemSettings;
                }

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
    }
}