using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemEDIJobDetailModel : SysModel
    {
        [Required]
        public string SysID { get; set; }

        [Required]
        public string EDIFlowID { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBox)]
        public string EDIJobID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIJobZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIJobZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIJobENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIJobTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIJobJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIJobKOKR { get; set; }

        [Required]
        public string EDIJobType { get; set; }

        public string EDIConID { get; set; }

        [StringLength(100)]
        [InputType(EnumInputType.TextBox)]
        public string ObjectName { get; set; }

        public string DepEDIJobID { get; set; }

        public string IsUseRes { get; set; }

        public string IsDisable { get; set; }

        public string IgnoreWarning { get; set; }

        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string FileSource { get; set; }
        public string GetFileSource { get; set; }

        public string FileEncoding { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string URLPath { get; set; }


        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemEDIJobDetail.TabText_SystemEDIJobDetail,
                ImageURL=string.Empty
            }
        };

        public SystemEDIJobDetailModel()
        {

        }

        public class SystemEDICon
        {
            public string EDIConID { get; set; }
            public string EDIConNM { get; set; }
        }

        public enum SortorderField
        {
            [Description("00")]
            Right,
            [Description("000")]
            Left,
            [Description("000001")]
            First
        }
        public enum EnumDeleteSystemEDIJobDetailResult
        {
            Success, Failure, DataExist
        }

        public void FormReset()
        {
            this.EDIJobZHTW = string.Empty;
            this.EDIJobZHCN = string.Empty;
            this.EDIJobENUS = string.Empty;
            this.EDIJobTHTH = string.Empty;
            this.EDIJobJAJP = string.Empty;
            this.EDIJobKOKR = string.Empty;
            this.EDIConID = string.Empty;
            this.ObjectName = string.Empty;
            this.DepEDIJobID = string.Empty;
            this.IsUseRes = EnumYN.Y.ToString();
            this.IsDisable = EnumYN.N.ToString();
            this.FileSource = string.Empty;
            this.SortOrder = string.Empty;
            this.URLPath = string.Empty;
        }

        public bool SetHasSysID()
        {
            foreach (UserSystemSysID systemSysIDList in EntityUserSystemSysIDList)
            {
                if (this.SysID == systemSysIDList.SysID)
                {
                    this.HasSysID = true;
                    break;
                }
            }

            return this.HasSysID;
        }

        List<SystemEDICon> _systemEDIConList = new List<SystemEDICon>();
        public List<SystemEDICon> SystemEDIConList { get { return _systemEDIConList; } }

        public async Task<bool> GetSysSystemEDIConList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemEDIJob.QuerySystemEDIConByIds(userID, SysID, EDIFlowID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                _systemEDIConList = Common.GetJsonDeserializeObject<List<SystemEDICon>>(response);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        SystemEDIJobDetail _ediJobDetail;
        public SystemEDIJobDetail EDIJobDetail { get { return _ediJobDetail; } }

        public async Task<bool> GetSystemEDIJobDetail(string userID)
        {
            try
            {
                string apiUrl = API.SystemEDIJob.QuerySystemEDIJobDetail(userID, SysID, EDIFlowID, EDIJobID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                _ediJobDetail = Common.GetJsonDeserializeObject<SystemEDIJobDetail>(response);

                if (_ediJobDetail != null)
                    return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetSortOrder(string userID)
        {
            try
            {
                string newSortOrder = Common.GetEnumDesc(SortorderField.First);

                string apiUrl = API.SystemEDIJob.QueryJobMaxSortOrder(userID, SysID, EDIFlowID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                if (!string.IsNullOrWhiteSpace(response))
                {
                    newSortOrder = response.Replace("0", null);

                    newSortOrder = Convert.ToString(Convert.ToInt32(newSortOrder) + 1);
                    newSortOrder = Common.GetEnumDesc(SortorderField.Left) + newSortOrder + Common.GetEnumDesc(SortorderField.Right);
                    newSortOrder = newSortOrder.Substring(newSortOrder.Length - 6, 6);
                }

                SortOrder = newSortOrder;

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;

        }
        public async Task<bool> GetEditSystemEDIJobDetailResult(string userID)
        {
            try
            {
                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    SysID = string.IsNullOrWhiteSpace(SysID) ? null : SysID,
                    EDIFlowID = string.IsNullOrWhiteSpace(EDIFlowID) ? null : EDIFlowID,
                    EDIJobID = string.IsNullOrWhiteSpace(EDIJobID) ? null : EDIJobID,
                    EDIJobZHTW = string.IsNullOrWhiteSpace(EDIJobZHTW) ? null : EDIJobZHTW,
                    EDIJobZHCN = string.IsNullOrWhiteSpace(EDIJobZHCN) ? null : EDIJobZHCN,
                    EDIJobENUS = string.IsNullOrWhiteSpace(EDIJobENUS) ? null : EDIJobENUS,
                    EDIJobTHTH = string.IsNullOrWhiteSpace(EDIJobTHTH) ? null : EDIJobTHTH,
                    EDIJobJAJP = string.IsNullOrWhiteSpace(EDIJobJAJP) ? null : EDIJobJAJP,
                    EDIJobKOKR = string.IsNullOrWhiteSpace(EDIJobKOKR) ? null : EDIJobKOKR,
                    EDIJobType = string.IsNullOrWhiteSpace(EDIJobType) ? null : EDIJobType,
                    EDIConID = string.IsNullOrWhiteSpace(EDIConID) ? null : EDIConID,
                    ObjectName = string.IsNullOrWhiteSpace(ObjectName) ? null : ObjectName,
                    DepEDIJobID = string.IsNullOrWhiteSpace(DepEDIJobID) ? null : DepEDIJobID,
                    IsUseRes = string.IsNullOrWhiteSpace(IsUseRes) ? null : EnumYN.Y.ToString(),
                    IgnoreWarning = string.IsNullOrWhiteSpace(IgnoreWarning) ? EnumYN.N.ToString() : IgnoreWarning,
                    IsDisable = string.IsNullOrWhiteSpace(IsDisable) ? EnumYN.N.ToString() : EnumYN.Y.ToString(),
                    FileSource = string.IsNullOrWhiteSpace(FileSource) ? null : FileSource,
                    FileEncoding = string.IsNullOrWhiteSpace(FileEncoding) ? null : FileEncoding,
                    URLPath = string.IsNullOrWhiteSpace(URLPath) ? null : URLPath,
                    SortOrder = string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder,
                    UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID
                });

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                     {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemEDIJob.EditSystemEDIJobDetail(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<EnumDeleteSystemEDIJobDetailResult> GetDeleteSystemEDIJobDetailResult(string userID)
        {
            var result = EnumDeleteSystemEDIJobDetailResult.Failure;

            try
            {
                string apiUrl = API.SystemEDIJob.DeleteSystemEDIJobDetail(userID, SysID, EDIFlowID, EDIJobID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                if (response == EnumDeleteSystemEDIJobDetailResult.Success.ToString())
                {
                    return EnumDeleteSystemEDIJobDetailResult.Success;
                }
                else
                {
                    return EnumDeleteSystemEDIJobDetailResult.DataExist;
                }
            }
            catch (WebException webException)
                when (webException.Response is HttpWebResponse &&
                      ((HttpWebResponse)webException.Response).StatusCode == HttpStatusCode.BadRequest)
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse)webException.Response;

                if (string.IsNullOrWhiteSpace(httpWebResponse.StatusDescription) == false)
                {
                    string errorMsg = Common.GetStreamToString(httpWebResponse.GetResponseStream(), Encoding.UTF8);
                    if (string.IsNullOrWhiteSpace(errorMsg) == false)
                    {
                        if (errorMsg == EnumDeleteSystemEDIJobDetailResult.Failure.ToString())
                        {
                            result = EnumDeleteSystemEDIJobDetailResult.Failure;
                        }
                    }
                }
            }
            return result;
        }
    }
}