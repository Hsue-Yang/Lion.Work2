using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemServiceListModel : SysModel
    {
        public class SystemService
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string SubSysID { get; set; }
            public string SubSysNM { get; set; }
            public string ServiceID { get; set; }
            public string ServiceNM { get; set; }
            public string Remark { get; set; }
            public string UpdUserID { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }

        #region - Property -
        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysID { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysNM { get; set; }

        public string SubSysID { get; set; }

        [Required]
        public string ServiceID { get; set; }

        public string Remark { get; set; }

        public string UpdUserID { get; set; }

        public List<SystemService> SystemServiceList { get; private set; }

        public List<Entity_BaseAP.CMCode> SystemServiceTypeList { get; private set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            ServiceID = string.Empty;
            Remark = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemServiceTypeList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemSetting.CodeManagement(userID, Common.GetEnumDesc(Entity_BaseAP.EnumCMCodeKind.SystemService), cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemServiceTypeList = Common.GetJsonDeserializeObject<List<Entity_BaseAP.CMCode>>(response);

                if (SystemServiceTypeList != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }

        public async Task<bool> GetSystmeServices(string sysID, string userID , EnumCultureID cultureID, int pageSize)
        {
            try
            {
                string apiUrl = API.SystemSetting.QuerySystemServices(sysID, userID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemServiceList = Common.GetJsonDeserializeObject<List<SystemService>>(response);
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> EditSystmeService(string userID, EnumCultureID cultureID)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                {
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                };

                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    SysID,
                    SubSysID = SubSysID ?? SysID,
                    ServiceID,
                    Remark,
                    UpdUserID = userID
                });
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemSetting.EditSystemService(userID);

                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }

        public async Task<bool> DeleteSystmeService(string userID)
        {
            try
            {
                string apiUrl = API.SystemSetting.DeleteSystemService(SysID, userID, 
                    SubSysID ?? SysID, ServiceID);

                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

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