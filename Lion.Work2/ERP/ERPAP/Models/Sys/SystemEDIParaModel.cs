using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemEDIParaModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryEDIFlowID, QueryEDIJobID
        }

        [Required]
        public string QuerySysID { get; set; }

        [Required]
        public string QueryEDIFlowID { get; set; }

        [Required]
        public string QueryEDIJobID { get; set; }

        [Required]
        [StringLength(50)]
        [InputType(EnumInputType.TextBox)]
        public string EDIParaID { get; set; }

        [Required]
        public string EDIParaType { get; set; }

        [StringLength(300)]
        [InputType(EnumInputType.TextBox)]
        public string EDIParaValue { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [InputType(EnumInputType.TextBoxNumber)]
        public string SortOrder { get; set; }

        public List<TabStripHelper.Tab> SysEDIParaTabList = new List<TabStripHelper.Tab>(); //TabText

        public void GetSysEDIParaTabList(EnumTabAction actionNM)
        {
            SysEDIParaTabList = new List<TabStripHelper.Tab>()
            {
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemEDIPara ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemEDIPara ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEDIPara)),
                    TabText=SysSystemEDIPara.TabText_SystemEDIPara,
                    ImageURL=string.Empty
                }
            };
        }

        public SystemEDIParaModel()
        {

        }

        public void FormReset()
        {
            this.EDIParaType = string.Empty;
            this.EDIParaValue = string.Empty;
        }

        public bool SetHasSysID()
        {
            foreach (UserSystemSysID systemSysIDList in EntityUserSystemSysIDList)
            {
                if (this.QuerySysID == systemSysIDList.SysID)
                {
                    this.HasSysID = true;
                    break;
                }
            }

            return this.HasSysID;
        }

        public class SystemEDIPara
        {
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string EDIFlowID { get; set; }
            public string EDIFlowNM { get; set; }
            public string EDIJobID { get; set; }
            public string EDIJobNM { get; set; }
            public string EDIJobParaID { get; set; }
            public string EDIJobParaType { get; set; }
            public string EDIJobParaValue { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDt { get; set; }
        }


        public class EDIParaSortValue
        {
            public string SysID { get; set; }
            public string EDIFlowID { get; set; }
            public string EDIJobID { get; set; }
            public string EDIParaID { get; set; }
            public string BeforeSortOrder { get; set; }
            public string AfterSortOrder { get; set; }
        }


        public async Task<bool> GetEditSystemEDIParaDetailResult(string userID)
        {
            try
            {
                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    SysID = QuerySysID,
                    SortOrder = string.IsNullOrWhiteSpace(SortOrder) ? null : SortOrder,
                    UpdUserID = string.IsNullOrWhiteSpace(userID) ? null : userID,
                    EDIFlowID = QueryEDIFlowID,
                    EDIJobID = QueryEDIJobID,
                    EDIJobParaID = string.IsNullOrWhiteSpace(EDIParaID) ? null : EDIParaID,
                    EDIJobParaType = string.IsNullOrWhiteSpace(EDIParaType) ? null : EDIParaType,
                    EDIJobParaValue = string.IsNullOrWhiteSpace(EDIParaValue) ? null : EDIParaValue
                });

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                     {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                string apiUrl = API.SystemEDIJob.EditSystemEDIPara(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetDeleteSystemEDIParaDetailResult(string userID)
        {
            try
            {
                string apiUrl = API.SystemEDIJob.DeleteSystemEDIPara(userID, QuerySysID, QueryEDIFlowID, QueryEDIJobID, EDIParaID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, "DELETE");

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }


        List<SystemEDIPara> _systemEDIParaList;
        public List<SystemEDIPara> SystemEDIParaList { get { return _systemEDIParaList; } }

        public async Task<bool> GetSystemEDIParaList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemEDIJob.QuerySystemEDIParas(userID, QuerySysID, QueryEDIFlowID, QueryEDIJobID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                _systemEDIParaList = Common.GetJsonDeserializeObject<List<SystemEDIPara>>(response);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetEDIParaSettingResult(string userID, List<EDIParaSortValue> ediParaSortValueList)
        {
            try
            {
                List<SystemEDIParaValue> ediParaValueList = new List<SystemEDIParaValue>();
                if (ediParaSortValueList != null && ediParaSortValueList.Any())
                {
                    foreach (var ediParaValue in ediParaSortValueList)
                    {
                        //判斷SORT_ORDER有變才更新
                        if (ediParaValue.AfterSortOrder != ediParaValue.BeforeSortOrder)
                        {
                            ediParaValueList.Add(new SystemEDIParaValue
                            {
                                SysID = QuerySysID,
                                SortOrder = ediParaValue.AfterSortOrder,
                                EDIJobParaID = ediParaValue.EDIParaID,
                                EDIFlowID = QueryEDIFlowID,
                                EDIJobID = QueryEDIJobID,
                                UpdUserID = userID,
                            });
                        }
                    }

                    if (ediParaValueList.Any())
                    {
                        Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};

                        var paraJsonStr = Common.GetJsonSerializeObject(ediParaValueList);
                        var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);

                        string apiUrl = API.SystemEDIJob.EditSystemEDIParaSortOrder(userID);
                        await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
                    }
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