using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemFunToolSettingModel : SysModel
    {
        public enum Field
        {
            SysID, FunControllerID, FunActionName
        }

        public enum AuthStateField
        {
            ERPAPSYS
        }

        [Required]
        public string SysID { get; set; }

        [StringLength(10)]
        public string SysCondition { get; set; }

        public string FunControllerID { get; set; }

        public string FunActionName { get; set; }

        public string IsMoved { get; set; }

        public string CopyToUserID { get; set; }

        public string FunControllerIDSearch { get; set; }

        public string FunActionNameSearch { get; set; }

        public string IsAdSearch { get; set; }

        public bool AuthStateUserRole { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemFunToolSetting.TabText_SystemFunToolSetting,
                ImageURL=string.Empty
            }
        };

        public SystemFunToolSettingModel()
        {
        }

        public void FormReset()
        {
            this.SysID = EnumSystemID.ERPAP.ToString();
            this.FunControllerID = string.Empty;
            this.FunActionName = string.Empty;
        }

        public List<FunToolSetting> SystemFunToolSettingList;
        public class FunToolSetting
        {
            public string UserID { get; set; }
            public string SysID { get; set; }
            public string SysNM { get; set; }
            public string FunControllerID { get; set; }
            public string FunGroupNM { get; set; }
            public string FunActionName { get; set; }
            public string FunNM { get; set; }
            public string ToolNo { get; set; }
            public string ToolNM { get; set; }
            public string IsCurrently { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public DateTime UpdDT { get; set; }
        }

        public List<SystemFunControllerID> SystemFunControllerIDList { get; set; }

        public class SystemFunControllerID : DBEntity.ISelectItem
        {
            public string FunControllerID { get; set; }
            public string FunGroupNM { get; set; }
            public string ItemText()
            {
                return FunGroupNM;
            }

            public string ItemValue()
            {
                return FunControllerID;
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public List<SystemFunToolFunName> SystemFunToolFunNameList { get; set; }

        public class SystemFunToolFunName : DBEntity.ISelectItem
        {
            public string FunActionName { get; set; }
            public string FunNM { get; set; }
            public string ItemText()
            {
                return FunNM;
            }

            public string ItemValue()
            {
                return FunActionName;
            }

            public string ItemValue(string key)
            {
                throw new NotImplementedException();
            }

            public string PictureUrl()
            {
                throw new NotImplementedException();
            }

            public string GroupBy()
            {
                throw new NotImplementedException();
            }
        }

        public class FunToolSettingPara
        {
            public string CopyToUserID { get; set; }
            public string UpdUserID { get; set; }
            public List<FunToolSettingValue> SystemUserToolList { get; set; }
        }

        public class FunToolSettingValue
        {
            public string PickData { get; set; }
            public string UserID { get; set; }
            public string SysID { get; set; }
            public string FunControllerID { get; set; }
            public string FunActionName { get; set; }
            public string ToolNo { get; set; }
            public string ToolNM { get; set; }
            public string IsMoved { get; set; }
            public string AfterSortOrder { get; set; }
            public string BeforeSortOrder { get; set; }
        }

        public async Task<bool> GetFunToolSettingList(string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemFunToolSetting.QuerySystemFunToolSettings(userID, SysID, FunControllerID, FunActionName, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new List<FunToolSetting>());

                SystemFunToolSettingList = responseObj;
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetSearchSystemFunToolControllerIDList(string sysID, string condition, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemFunToolSetting.QuerySystemFunToolControllerIDs(sysID, condition, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new List<SystemFunControllerID>());

                SystemFunControllerIDList = responseObj;
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetSearchSystemFunToolFunNameList(string sysID, string funControllerID, string condition, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemFunToolSetting.QuerySystemFunToolFunNames(sysID, funControllerID, condition, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new List<SystemFunToolFunName>());

                SystemFunToolFunNameList = responseObj;
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetEditFunToolSettingResult(string userID, EnumCultureID cultureID, List<FunToolSettingValue> funToolSettingValueList)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};
                funToolSettingValueList = funToolSettingValueList.Where(x => x.PickData == EnumYN.Y.ToString() || x.AfterSortOrder != x.BeforeSortOrder).ToList();
                FunToolSettingPara funToolSettingPara = new FunToolSettingPara
                {
                    UpdUserID = userID,
                    SystemUserToolList = funToolSettingValueList
                };
                var paraJsonStr = Common.GetJsonSerializeObject(funToolSettingPara);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemFunToolSetting.EditSystemFunToolSetting();
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetDeleteFunToolSettingResult(string userID, EnumCultureID cultureID, List<FunToolSettingValue> funToolSettingValueList)
        {
            try
            {
                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};
                funToolSettingValueList = funToolSettingValueList.Where(x => x.PickData == EnumYN.Y.ToString()).ToList();
                FunToolSettingPara funToolSettingPara = new FunToolSettingPara
                {
                    UpdUserID = userID,
                    SystemUserToolList = funToolSettingValueList
                };
                var paraJsonStr = Common.GetJsonSerializeObject(funToolSettingPara);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiDelUrl = API.SystemFunToolSetting.DeleteSystemFunToolSetting(userID, SysID, FunControllerID, FunActionName);
                await PublicFun.HttpPutWebRequestGetResponseStringAsync(apiDelUrl, AppSettings.APITimeOut, bodyBytes);

                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        public async Task<bool> GetCopyFunToolSettingResult(string userID, EnumCultureID cultureID, List<FunToolSettingValue> funToolSettingValueList)
        {
            try
            {
                string defaultToolNo = Common.GetEnumDesc(_BaseAPModel.EnumSysFunToolNo.DefaultNo);

                Dictionary<HttpRequestHeader, string> headers = new Dictionary<HttpRequestHeader, string>{
                    {HttpRequestHeader.ContentType, "application/json; charset=utf-8"}};
                funToolSettingValueList = funToolSettingValueList.Where(x => x.PickData == EnumYN.Y.ToString()).ToList();
                FunToolSettingPara funToolSettingPara = new FunToolSettingPara
                {
                    CopyToUserID = CopyToUserID,
                    UpdUserID = userID,
                    SystemUserToolList = funToolSettingValueList
                };
                var paraJsonStr = Common.GetJsonSerializeObject(funToolSettingPara);
                var bodyBytes = Encoding.UTF8.GetBytes(paraJsonStr);
                string apiUrl = API.SystemFunToolSetting.CopySystemUserFunTool(defaultToolNo);
                await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut, bodyBytes, headers);
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
    }
}