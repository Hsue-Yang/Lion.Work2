using LionTech.Entity;
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
    public class SystemIPListModel : SysModel
    {
        #region - Class -
        public class SystemIP
        {
            public string SysID { get; set; }

            public string SubSysID { get; set; }
            public string SubSysNM { get; set; }

            public string IPAddress { get; set; }
            public string ServerNM { get; set; }

            public string IsAPServer { get; set; }
            public string IsAPIServer { get; set; }
            public string IsDBServer { get; set; }
            public string IsFileServer { get; set; }
            public string IsOutsourcing { get; set; }

            public string FolderPath { get; set; }
            public string Remark { get; set; }

            public string UpdUserID { get; set; }
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
        public string SysNM { get; set; }

        public string SubSysID { get; set; }

        [Required]
        [StringLength(15)]
        [InputType(EnumInputType.TextBox)]
        public string IPAddress { get; set; }

        [Required]
        [StringLength(20)]
        [InputType(EnumInputType.TextBox)]
        public string ServerNM { get; set; }

        public string IsAPServer { get; set; }
        public string IsAPIServer { get; set; }
        public string IsDBServer { get; set; }
        public string IsFileServer { get; set; }
        public string IsOutsourcing { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string FolderPath { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string Remark { get; set; }

        public List<SystemIP> SystemIPList { get; private set; }

        #endregion

        #region - Reset -

        public void FormReset()
        {
            IPAddress = string.Empty;
            ServerNM = string.Empty;
            IsAPServer = string.Empty;
            IsAPIServer = string.Empty;
            IsDBServer = string.Empty;
            IsFileServer = string.Empty;
            IsOutsourcing = string.Empty;
            FolderPath = string.Empty;
            Remark = string.Empty;
        }

        #endregion

        public async Task<bool> GetSystemIPList(string sysID, string userID, int pageSize, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemSetting.QuerySystemIPs(sysID, userID, cultureID.ToString().ToUpper(), PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemIPs = (List<SystemIP>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemIPList = responseObj.SystemIPs;

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

        public async Task<bool> EditSystemIP(string userID)
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
                    IPAddress,
                    ServerNM,
                    IsAPServer = IsAPServer ?? EnumYN.N.ToString(),
                    IsAPIServer = IsAPIServer ?? EnumYN.N.ToString(),
                    IsDBServer = IsDBServer ?? EnumYN.N.ToString(),
                    IsFileServer = IsFileServer ?? EnumYN.N.ToString(),
                    IsOutsourcing = IsOutsourcing ?? EnumYN.N.ToString(),
                    FolderPath,
                    Remark,
                    UpdUserID = userID
                });
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemSetting.EditSystemIP(userID);

                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }

        public async Task<bool> DeleteSystemIP(string userID)
        {
            try
            {
                string apiUrl = API.SystemSetting.DeleteSystemIP(SysID, userID,
                    SubSysID ?? SysID, IPAddress);

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