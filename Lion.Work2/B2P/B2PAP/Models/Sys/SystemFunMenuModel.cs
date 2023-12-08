using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;

namespace B2PAP.Models.Sys
{
    public class SystemFunMenuModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryFunMenu
        }

        [Required]
        public string QuerySysID { get; set; }

        public string QueryFunMenu { get; set; }

        public SystemFunMenuModel()
        {
        }

        public void FormReset()
        {
            this.QuerySysID = EnumSystemID.B2PAP.ToString();
            this.QueryFunMenu = string.Empty;
        }

        List<EntitySystemFunMenu.SystemFunMenu> _entitySystemFunMenuList;
        public List<EntitySystemFunMenu.SystemFunMenu> EntitySystemFunMenuList { get { return _entitySystemFunMenuList; } }

        public bool GetSystemFunMenuList(int pageSize, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemFunMenu.SystemFunMenuPara para = new EntitySystemFunMenu.SystemFunMenuPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                    FunMenu = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryFunMenu) ? null : this.QueryFunMenu))
                };

                _entitySystemFunMenuList = new EntitySystemFunMenu(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemFunMenuList(para);

                if (_entitySystemFunMenuList != null)
                {
                    _entitySystemFunMenuList = base.GetEntitysByPage(_entitySystemFunMenuList, pageSize);
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