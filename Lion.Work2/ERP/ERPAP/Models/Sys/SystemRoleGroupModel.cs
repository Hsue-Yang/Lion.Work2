using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Sys
{
    public class SystemRoleGroupModel : SysModel
    {
        public enum Field
        {
            //QuerySysID,
        }

        [Required]
        public string QuerySysID { get; set; }

        public string QueryEDIFlowID { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>()
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=SysSystemRoleGroup.TabText_SystemRoleGroup,
                ImageURL=string.Empty
            }
        };

        public SystemRoleGroupModel()
        {

        }

        public void FormReset()
        {
            //this.QuerySysID = EnumSystemID.ERPAP.ToString();
        }

        List<EntitySystemRoleGroup.SystemRoleGroup> _entitySystemRoleGroupList;
        public List<EntitySystemRoleGroup.SystemRoleGroup> EntitySystemRoleGroupList { get { return _entitySystemRoleGroupList; } }

        public bool GetSystemRoleGroupList(EnumCultureID cultureID)
        {
            try
            {
                EntitySystemRoleGroup.SystemRoleGroupPara para = new EntitySystemRoleGroup.SystemRoleGroupPara(cultureID.ToString())
                {
                    //SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                };

                _entitySystemRoleGroupList = new EntitySystemRoleGroup(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectSystemRoleGroupList(para);

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