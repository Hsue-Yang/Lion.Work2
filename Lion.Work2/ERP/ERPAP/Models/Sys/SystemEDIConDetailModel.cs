using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;


namespace ERPAP.Models.Sys
{
    public class SystemEDIConDetailModel : SysModel
    {
        public enum EnumDeleteSystemEDIConDetailResult
        {
            Success,
            Failure,
            DataExist
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
        public class SystemEDIConDetail
        {
            public string SysID { get; set; }
            public string EDIFlowID { get; set; }
            public string EDIConID { get; set; }
            public string EDIConZHTW { get; set; }
            public string EDIConZHCN { get; set; }
            public string EDIConENUS { get; set; }
            public string EDIConTHTH { get; set; }
            public string EDIConJAJP { get; set; }
            public string EDIConKOKR { get; set; }
            public string ProviderName { get; set; }
            public string ConValue { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserID { get; set; }
        }



        [Required]
        public string SysID { get; set; }

        [Required]
        public string EDIFlowID { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBox)]
        public string EDIConID { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIConZHTW { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIConZHCN { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIConENUS { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIConTHTH { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIConJAJP { get; set; }

        [Required]
        [StringLength(150)]
        [InputType(EnumInputType.TextBox)]
        public string EDIConKOKR { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBox)]
        public string ProviderName { get; set; }

        [Required]
        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string ConValue { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemEDIConDetail.TabText_SystemEDIConDetail,
                ImageURL=string.Empty
            }
        };

        public SystemEDIConDetailModel()
        {

        }

        public void FormReset()
        {
            this.EDIConZHTW = string.Empty;
            this.EDIConZHCN = string.Empty;
            this.EDIConENUS = string.Empty;
            this.EDIConTHTH = string.Empty;
            this.EDIConJAJP = string.Empty;
            this.EDIConKOKR = string.Empty;
            this.ProviderName = string.Empty;
            this.ConValue = string.Empty;
            this.SortOrder = string.Empty;
        }

        public bool SetHasSysID()
        {
            foreach (var systemSysIDList in UserSystemByIdList)
            {
                if (this.SysID == systemSysIDList.SysID)
                {
                    this.HasSysID = true;
                    break;
                }
            }

            return this.HasSysID;
        }

        SystemEDIConDetail _entitySystemEDIConDetail;
        public SystemEDIConDetail EntitySystemEDIConDetail { get { return _entitySystemEDIConDetail; } }

        public async Task<bool> GetSystemEDIConDetail(string userID)
        {
            try
            {

                string apiUrl = API.SystemEDICon.QuerySystemEDIConDetail(userID, SysID, EDIFlowID, EDIConID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    SystemEDIFlowDetail = (SystemEDIConDetail)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    _entitySystemEDIConDetail = responseObj.SystemEDIFlowDetail;
                }

                if (_entitySystemEDIConDetail != null)
                {
                    SysID = EntitySystemEDIConDetail.SysID;
                    EDIConID = EntitySystemEDIConDetail.EDIConID;
                    EDIConZHTW = EntitySystemEDIConDetail.EDIConZHTW;
                    EDIConZHCN = EntitySystemEDIConDetail.EDIConZHCN;
                    EDIConENUS = EntitySystemEDIConDetail.EDIConENUS;
                    EDIConTHTH = EntitySystemEDIConDetail.EDIConTHTH;
                    EDIConJAJP = EntitySystemEDIConDetail.EDIConJAJP;
                    EDIConKOKR = EntitySystemEDIConDetail.EDIConKOKR;
                    ProviderName = EntitySystemEDIConDetail.ProviderName;
                    ConValue = EntitySystemEDIConDetail.ConValue;
                    SortOrder = EntitySystemEDIConDetail.SortOrder;

                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetEditSystemEDIConDetailResult(string userID)
        {
            try
            {
                SystemEDIConDetail para = new SystemEDIConDetail();

                para = new SystemEDIConDetail()
                {
                    SysID = this.SysID,
                    EDIFlowID = this.EDIFlowID,
                    EDIConID = this.EDIConID,
                    EDIConZHTW = string.IsNullOrWhiteSpace(this.EDIConZHTW) ? null : this.EDIConZHTW,
                    EDIConZHCN = string.IsNullOrWhiteSpace(this.EDIConZHCN) ? null : this.EDIConZHCN,
                    EDIConENUS = string.IsNullOrWhiteSpace(this.EDIConENUS) ? null : this.EDIConENUS,
                    EDIConTHTH = string.IsNullOrWhiteSpace(this.EDIConTHTH) ? null : this.EDIConTHTH,
                    EDIConJAJP = string.IsNullOrWhiteSpace(this.EDIConJAJP) ? null : this.EDIConJAJP,
                    EDIConKOKR = string.IsNullOrWhiteSpace(this.EDIConKOKR) ? null : this.EDIConKOKR,
                    ProviderName = string.IsNullOrWhiteSpace(this.ProviderName) ? null : this.ProviderName,
                    ConValue = string.IsNullOrWhiteSpace(this.ConValue) ? null : this.ConValue,
                    SortOrder = string.IsNullOrWhiteSpace(this.SortOrder) ? null : this.SortOrder,
                    UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID
                };

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};


                var paraJsonStr = Common.GetJsonSerializeObject(para);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.SystemEDICon.EditSystemEDIConDetails(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
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
                if (EDIFlowID != null)
                {
                    string apiUrl = API.SystemEDICon.QueryConNewSortOrder(userID, SysID, this.EDIFlowID);
                    string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);
                    newSortOrder = response.ToString().Replace("0", null);
                    if (!string.IsNullOrWhiteSpace(newSortOrder))
                    {
                        newSortOrder = Convert.ToString(Convert.ToInt32(newSortOrder) + 1);
                        newSortOrder = Common.GetEnumDesc(SortorderField.Left) + newSortOrder + Common.GetEnumDesc(SortorderField.Right);
                        newSortOrder = newSortOrder.Substring(newSortOrder.Length - 6, 6);
                    }
                }

                this.SortOrder = newSortOrder;

                if (string.IsNullOrWhiteSpace(this.SortOrder) == false)
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

        public async Task<EnumDeleteSystemEDIConDetailResult> GetDeleteSystemEDIConDetailResult(string userID)
        {
            var result = EnumDeleteSystemEDIConDetailResult.Failure;
            try
            {
                string apiUrl = API.SystemEDICon.DeleteSystemEDIConDetails(userID, SysID, EDIFlowID, EDIConID);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                if (response == EnumDeleteSystemEDIConDetailResult.Success.ToString())
                {
                    return EnumDeleteSystemEDIConDetailResult.Success;
                }
                else
                {
                    return EnumDeleteSystemEDIConDetailResult.DataExist;
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
                        if (errorMsg == EnumDeleteSystemEDIConDetailResult.Failure.ToString())
                        {
                            result = EnumDeleteSystemEDIConDetailResult.Failure;
                        }
                    }
                }
            }
            return result;
        }
    }
}