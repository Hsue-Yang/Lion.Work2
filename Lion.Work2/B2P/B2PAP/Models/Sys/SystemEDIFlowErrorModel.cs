﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace B2PAP.Models.Sys
{
    public class SystemEDIFlowErrorModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryEDIFlowID
        }

        [Required]
        public string QuerySysID { get; set; }

        public string EDIFlowID { get; set; }

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
                TabText=SysSystemEDIFlowError.TabText_SystemEDIFlowError,
                ImageURL=string.Empty
            }
        };

        public SystemEDIFlowErrorModel()
        {
        }

        public void FormReset()
        {
        }

        public string GetFileDataPath(EnumCultureID CultureID)
        {
            EntitySys.SysSystemSysIDPara Pathpara = new EntitySys.SysSystemSysIDPara(CultureID.ToString())
            {
                SysID = new DBVarChar((string.IsNullOrWhiteSpace(QuerySysID) ? null : QuerySysID)),
            };

            string fileDataPath = new EntitySys(this.ConnectionStringB2P, this.ProviderNameB2P).GetFileDataPath(Pathpara);
            return fileDataPath;
        }
    }
}