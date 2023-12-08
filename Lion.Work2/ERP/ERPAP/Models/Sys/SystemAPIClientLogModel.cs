using System.Collections.Generic;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemAPIClientLogModel :SysModel
    {
        #region - Enum -
        public enum EDIFlowID
        {
            SUBS
        }
        #endregion

        public string EDIDate { get; set; }

        public string EDITime { get; set; }

        public string EDIFilePath { get; set; }

        public string EDIFile { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemAPIClientLog.TabText_SystemAPIClientLog,
                ImageURL=string.Empty
            }
        };

        public SystemAPIClientLogModel()
        { 

        }

        public string GetFileDataPath(EnumCultureID CultureID)
        {
            EntitySys.SysSystemSysIDPara Pathpara = new EntitySys.SysSystemSysIDPara(CultureID.ToString())
            {
                SysID = new DBVarChar(EnumSystemID.ERPAP.ToString()),
            };

            string fileDataPath = new EntitySys(this.ConnectionStringSERP, this.ProviderNameSERP).GetFileDataPath(Pathpara);
            return fileDataPath;
        }
    }
}