using System;
using System.Collections.Generic;
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
    public class SystemEDIFlowLogSettingModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryEDIFlowID
        }

        public enum ParaEnum
        {
            W
        }

        public bool SaveType;
        [Required]
        public string QuerySysID { get; set; }

        [Required]
        public string QueryEDIFlowID { get; set; }

        [StringLength(12, MinimumLength = 12)]
        public string EDINO { get; set; }

        [InputType(EnumInputType.TextBoxDatePicker)]
        [Required]
        public string DataDate { get; set; }

        [Required]
        public string StatusID { get; set; }

        [Required]
        public string IsAutomatic { get; set; }

        [Required]
        public string IsDeleted { get; set; }

        public List<TabStripHelper.Tab> SysEDIFlowLogSettingTabList = new List<TabStripHelper.Tab>(); //TabText

        public void GetSysEDIFlowLogSettingTabList(EnumTabAction actionNM)
        {
            SysEDIFlowLogSettingTabList = new List<TabStripHelper.Tab>()
            {
                new TabStripHelper.Tab
                {
                    ControllerName=(actionNM == EnumTabAction.SysSystemEDIFlowLogSetting ? string.Empty : EnumTabController.Sys.ToString()),
                    ActionName=(actionNM == EnumTabAction.SysSystemEDIFlowLogSetting ? string.Empty : Common.GetEnumDesc(EnumTabAction.SysSystemEDIPara)),
                    TabText=SysSystemEDIFlowLogSetting.TabText_SystemEDIFlowLogSetting,
                    ImageURL=string.Empty
                }
            };
        }

        public SystemEDIFlowLogSettingModel()
        {

        }

        public void FormReset()
        {
            this.DataDate = Common.GetDateString();
        }

        public async Task<bool> GetEditEDIFlowLogSettingResult(string userID)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>
                {
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}
                };

                var paraJsonStr = Common.GetJsonSerializeObject(new
                {
                    SysID = QuerySysID,
                    EDIFlowID = QueryEDIFlowID,
                    DataDate = DataDate,
                    StatusID = ParaEnum.W.ToString(),
                    IsAutomatic = EnumYN.N.ToString(),
                    IsDeleted = EnumYN.N.ToString(),
                    UpdUserID = userID,
                });

                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemEDIFlowLogSetting.EditEDIFlowLogSetting(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

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