using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Dev;
using LionTech.Web.ERPHelper;
using Resources;

namespace ERPAP.Models.Dev
{
    public class SystemFunModel : DevModel
    {
        public enum Field
        {
            QuerySysID, QueryFunControllerID, QueryFunMenuSysID, QueryFunMenu, OnlyEvent
        }

        [Required]
        public string QuerySysID { get; set; }

        public string QueryFunControllerID { get; set; }

        public string QueryFunMenuSysID { get; set; }

        public string QueryFunMenu { get; set; }

        public string OnlyEvent { get; set; }

        public List<TabStripHelper.Tab> TabList = new List<TabStripHelper.Tab>() 
        {
            new TabStripHelper.Tab {
                ControllerName=string.Empty,
                ActionName=string.Empty,
                TabText=DevSystemFun.TabText_SystemFun,
                ImageURL=string.Empty
            }
        };

        public SystemFunModel()
        {

        }

        public void FormReset()
        {
            this.QuerySysID = string.Empty;
            this.QueryFunControllerID = string.Empty;
            this.QueryFunMenuSysID = string.Empty;
            this.QueryFunMenu = string.Empty;
            this.OnlyEvent = EnumYN.N.ToString();
        }

        List<EntitySystemFun.SystemFun> _entitySystemFunList;
        public List<EntitySystemFun.SystemFun> EntitySystemFunList { get { return _entitySystemFunList; } }

        public bool GetSystemFunList(int pageSize, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemFun.SystemFunPara para = new EntitySystemFun.SystemFunPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                    FunControllerID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryFunControllerID) ? null : this.QueryFunControllerID)),
                    FunMenuSysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryFunMenuSysID) ? null : this.QueryFunMenuSysID)),
                    FunMenu = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryFunMenu) ? null : this.QueryFunMenu))
                };

                if (string.IsNullOrWhiteSpace(this.OnlyEvent) == false && this.OnlyEvent == EnumYN.Y.ToString())
                {
                    _entitySystemFunList = new EntitySystemFun(this.ConnectionStringSERP, this.ProviderNameSERP)
                        .SelectSystemEventTargetList(para);
                }
                else
                {
                    _entitySystemFunList = new EntitySystemFun(this.ConnectionStringSERP, this.ProviderNameSERP)
                        .SelectSystemFunList(para);
                }

                if (_entitySystemFunList != null)
                {
                    _entitySystemFunList = base.GetEntitysByPage(_entitySystemFunList, pageSize);
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