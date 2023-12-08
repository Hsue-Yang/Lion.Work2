using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Utility;
using LionTech.Utility.ERP;
using LionTech.Web.ERPHelper;
using Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPAP.Models.Sys
{
    public class SystemFunToolParaModel : SysModel
    {
        public string UserID { get; set; }

        public string SysID { get; set; }

        public string FunControllerID { get; set; }

        public string FunActionName { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName = string.Empty,
                ActionName = string.Empty,
                TabText = SysSystemFunToolPara.TabText_SystemFunToolPara,
                ImageURL = string.Empty,
            }
        };

        public class SystemFunTool
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
            public string ParaID { get; set; }
            public string ParaValue { get; set; }
            public string IsCurrently { get; set; }
            public string SortOrder { get; set; }
            public string UpdUserNM { get; set; }
            public string UpdDt { get; set; }
        }

        public SystemFunTool SystemFunToolParaForm { get; set; }
        public List<SystemFunTool> SystemFunToolParaList { get; set; }

        public SystemFunToolParaModel() { }

        public void FormReset() { }

        EntitySystemFunToolPara.SystemFunTool _entitySystemFunToolParaFormList;
        public EntitySystemFunToolPara.SystemFunTool EntitySystemFunToolParaForm { get { return _entitySystemFunToolParaFormList; } }

        public async Task<bool> GetSystemFunToolParaForm(EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemFunToolPara.QuerySystemFunToolParaForms(UserID, SysID, FunControllerID, FunActionName, ToolNo, cultureID.ToString().ToUpper());
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = Common.GetJsonDeserializeAnonymousType(response, new SystemFunTool());

                if (responseObj != null)
                {
                    SystemFunToolParaForm = responseObj;
                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }

        List<EntitySystemFunToolPara.SystemFunTool> _entitySystemFunToolParaList;
        public List<EntitySystemFunToolPara.SystemFunTool> EntitySystemFunToolParaList { get { return _entitySystemFunToolParaList; } }

        public async Task<bool> GetSystemFunToolParaList(int pageSize, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemFunToolPara.QuerySystemFunToolParas(UserID, SysID, FunControllerID, FunActionName, ToolNo, PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemFunToolParaList = (List<SystemFunTool>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);


                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    SystemFunToolParaList = responseObj.SystemFunToolParaList;
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }
    }
}