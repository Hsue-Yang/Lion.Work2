using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;
using LionTech.Web.ERPHelper;
using Resources;


namespace B2PAP.Models.Sys
{
    public class SystemRoleFunListModel : SysModel
    {
        
        [Required]
        public string QuerySysID { get; set; }

        public string QueryFunControllerID { get; set; }

        public string RoleID { get; set; }

        public string RoleNM { get; set; }

        
        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab
            {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText= SysSystemRoleFunList.TabText_SystemRoleFunList,
                ImageURL=string.Empty
            }
        };

        List<EntitySystemRoleFunList.SystemRoleFunList> _entitySystemRoleFunList;
        public List<EntitySystemRoleFunList.SystemRoleFunList> entitySystemRoleFunList { get { return _entitySystemRoleFunList;} }

        public bool GetSystemRoleFunList(int pagesize, EnumCultureID cultureID)
        {
            try 
            {

                EntitySystemRoleFunList.SystemRoleFunListPara para = new EntitySystemRoleFunList.SystemRoleFunListPara(cultureID.ToString())
                {
                    SysID = new DBVarChar(string.IsNullOrWhiteSpace(this.QuerySysID) ? null:this.QuerySysID),
                    RoleID = new DBVarChar((string.IsNullOrWhiteSpace(this.RoleID) ? null : this.RoleID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryFunControllerID) ? null : this.QueryFunControllerID))
                };

                _entitySystemRoleFunList = new EntitySystemRoleFunList(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemRoleFunList(para);

                if (_entitySystemRoleFunList != null)
                    _entitySystemRoleFunList = base.GetEntitysByPage(_entitySystemRoleFunList, pagesize);
                else 
                    _entitySystemRoleFunList = new List<EntitySystemRoleFunList.SystemRoleFunList>();
                
                return true;
            }
            catch(Exception ex)
            {
                base.OnException(ex);
            }

            return false;
        }

    }
}