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
    public class SystemSubsysListModel : SysModel
    {
        public class SystemSubMain
        {
            public string SubSysID { get; set; }

            public string SysMANUserID { get; set; }
            public string SysMANUserNM { get; set; }

            public string SysID { get; set; }
            public string SysNM { get; set; }

            public string SysNMZHTW { get; set; }
            public string SysNMZHCN { get; set; }
            public string SysNMENUS { get; set; }
            public string SysNMTHTH { get; set; }
            public string SysNMJAJP { get; set; }
            public string SysNMKOKR { get; set; }

            public string SortOrder { get; set; }

            public string UpdUserID { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }

        #region - Property -
        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysID { get; set; }

        [InputType(EnumInputType.TextBoxHidden)]
        public string SysNM { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 4)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string SubSysID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string SysNMKOKR { get; set; }

        [StringLength(6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        private string _SysMANUserID;
        [Required]
        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxAlphanumeric)]
        public string SysMANUserID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_SysMANUserID))
                {
                    return _SysMANUserID;
                }
                return _SysMANUserID.ToUpper();
            }
            set
            {
                _SysMANUserID = value;
            }
        }

        public List<SystemSubMain> SystemSubMainList { get; set; }
        #endregion

        #region - Reset -

        public void FormReset()
        {
            SubSysID = string.Empty;
            SysNMZHTW = string.Empty;
            SysNMZHCN = string.Empty;
            SysNMENUS = string.Empty;
            SysNMTHTH = string.Empty;
            SysNMJAJP = string.Empty;
            SysNMKOKR = string.Empty;
            SortOrder = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemSubList(string sysID, string userID)
        {
            try
            {
                string apiUrl = API.SystemSetting.QuerySystemSubs(sysID, userID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemSubMainList = Common.GetJsonDeserializeObject<List<SystemSubMain>>(response);
                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> EditSystemSub(string userID)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                {
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                };

                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    SubSysID,
                    SysID,
                    SysMANUserID,
                    SysNMZHTW,
                    SysNMZHCN,
                    SysNMENUS,
                    SysNMTHTH,
                    SysNMJAJP,
                    SysNMKOKR,
                    SortOrder,
                    UpdUserID = userID 
                });
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemSetting.EditSystemSub(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }

        public async Task<bool> DeleteSystemSub(string userID)
        {
            try
            {
                string apiUrl = API.SystemSetting.DeleteSystemSub(SubSysID, userID);

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