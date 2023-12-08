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
    public class SystemAPIAuthorizeModel : SysModel
    {
        #region - Class -
        public class SystemAPIAuthorize
        {
            public string ClientSysID { get; set; }
            public string ClientSysNM { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }
        #endregion

        #region - Property -
        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysID { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string APIGroupID { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string APIFunID { get; set; }

        [StringLength(300)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string APIPara { get; set; }

        [Required]
        public string ClientSysID { get; set; }

        public List<SystemAPIAuthorize> SystemAPIAuthorizeList { get; set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            ClientSysID = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemAPIAuthorizeList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemAPI.QuerySystemAPIAuthorizes(SysID, userID, APIGroupID, APIFunID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemAPIAuthorizeList = Common.GetJsonDeserializeObject<List<SystemAPIAuthorize>>(response);
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> EditSystemAPIAuthorize(string userID)
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
                    APIGroupID,
                    APIFunID,
                    ClientSysID,
                    UpdUserID = userID
                });
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemAPI.EditSystemAPIAuthorize(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> DeleteSystemAPIAuthorize(string userID)
        {
            try
            {
                string apiUrl = API.SystemAPI.DeleteSystemAPIAuthorize(SysID, userID, APIGroupID, APIFunID, ClientSysID);
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