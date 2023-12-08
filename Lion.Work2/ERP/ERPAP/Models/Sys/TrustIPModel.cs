using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class TrustIPModel : SysModel
    {
        public class TrustIP
        {
            public string IPBegin { get; set; }
            public string IPEnd { get; set; }
            public string ComID { get; set; }
            public string ComNM { get; set; }
            public string TrustStatus { get; set; }
            public string TrustType { get; set; }
            public string TrustTypeNM { get; set; }
            public string SourceType { get; set; }
            public string SourceTypeNM { get; set; }
            public string Remark { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }

        public enum Field
        {
            QueryIPBegin, QueryIPEnd, QueryComID,
            QueryTrustStatus, QueryTrustType, QuerySourceType,
            QueryRemark
        }

        [StringLength(40)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string QueryIPBegin { get; set; }

        [StringLength(40)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string QueryIPEnd { get; set; }

        public string QueryComID { get; set; }

        public string QueryTrustStatus { get; set; }

        public string QueryTrustType { get; set; }

        public string QuerySourceType { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string QueryRemark { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysTrustIP.TabText_TrustIP,
                ImageURL=string.Empty
            }
        };

        public TrustIPModel()
        {
        }

        public void FormReset()
        {
            this.QueryIPBegin = string.Empty;
            this.QueryIPEnd = string.Empty;
            this.QueryComID = string.Empty;
            this.QueryTrustStatus = string.Empty;
            this.QueryTrustType = string.Empty;
            this.QuerySourceType = string.Empty;
            this.QueryRemark = string.Empty;
        }

        List<TrustIP> _entityTrustIPList;
        public List<TrustIP> EntityTrustIPList { get { return _entityTrustIPList; } }

        public async Task<bool> GetTrustIPList(int pageSize, EnumCultureID cultureID)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                var para = new
                {
                    IPBegin = string.IsNullOrWhiteSpace(QueryIPBegin) ? null : QueryIPBegin,
                    IPEnd = string.IsNullOrWhiteSpace(QueryIPEnd) ? null : QueryIPEnd,
                    ComID = string.IsNullOrWhiteSpace(QueryComID) ? null : QueryComID,
                    TrustStatus = string.IsNullOrWhiteSpace(QueryTrustStatus) ? null : EnumYN.Y.ToString(),
                    TrustType = string.IsNullOrWhiteSpace(QueryTrustType) ? null : QueryTrustType,
                    SourceType = string.IsNullOrWhiteSpace(QuerySourceType) ? null : QuerySourceType,
                    Remark = string.IsNullOrWhiteSpace(QueryRemark) ? null : QueryRemark,
                    CultureID = cultureID.ToString().ToUpper(),
                    PageIndex = PageIndex,
                    PageSize = pageSize
                };

                var paraJsonStr = Common.GetJsonSerializeObject(para);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.TrustIP.QueryTrustIPs();
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                var responseObj = new
                {
                    RowCount = 0,
                    TrustIPList = (List<TrustIP>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj.TrustIPList != null)
                {
                    RowCount = responseObj.RowCount;
                    _entityTrustIPList = responseObj.TrustIPList;

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