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
    public class SystemEventTargetModel : SysModel
    {
        #region - Property -
        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string SysID { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string EventGroupID { get; set; }

        [Required]
        [InputType(EnumInputType.TextBoxHidden)]
        public string EventID { get; set; }

        [StringLength(2000)]
        [InputType(EnumInputType.TextBoxNotChinese)]
        public string EventPara { get; set; }

        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string Remark { get; set; }

        [Required]
        public string TargetSysID { get; set; }

        [Required]
        [StringLength(600)]
        [InputType(EnumInputType.TextBox)]
        public string TargetPath { get; set; }

        public string SubSysID { get; set; }

        public List<SystemEventTarget> SystemEventTargetList { get; private set; }
        #endregion

        #region - Reset -
        public void FormReset()
        {
            TargetSysID = string.Empty;
            TargetPath = string.Empty;
        }
        #endregion

        public async Task<bool> GetSystemEventTargetList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemEvent.QuerySystemEventTargets(SysID, userID, EventGroupID, EventID, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                SystemEventTargetList = Common.GetJsonDeserializeObject<List<SystemEventTarget>>(response);

                if (SystemEventTargetList != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public async Task<bool> EditSystemEventTarget(string userID)
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
                    EventGroupID,
                    EventID,
                    TargetSysID,
                    TargetPath,
                    SubSysID = SubSysID ?? TargetSysID,
                    UpdUserID = userID
                });
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemEvent.EditSystemEventTarget(userID);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        public async Task<bool> DeleteSystemEventTarget(string userID)
        {
            try
            {
                string apiUrl = API.SystemEvent.DeleteSystemEventTarget(SysID, userID, EventGroupID, EventID, TargetSysID);
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