using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LionTech.Entity;
using LionTech.Entity.B2P;
using LionTech.Entity.B2P.Sys;

namespace B2PAP.Models.Sys
{
    public class SystemRoleModel : SysModel
    {
        public enum Field
        {
            QuerySysID, QueryRoleID
        }

        [Required]
        public string QuerySysID { get; set; }

        public string QueryRoleID { get; set; }

        public SystemRoleModel()
        {
        }

        public void FormReset()
        {
            this.QuerySysID = EnumSystemID.B2PAP.ToString();
            this.QueryRoleID = string.Empty;
        }

        List<EntitySystemRole.SystemRole> _entitySystemRoleList;
        public List<EntitySystemRole.SystemRole> EntitySystemRoleList { get { return _entitySystemRoleList; } }

        public bool GetSystemRoleList(int pageSize, EnumCultureID cultureID)
        {
            try
            {
                EntitySystemRole.SystemRolePara para = new EntitySystemRole.SystemRolePara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(this.QuerySysID) ? null : this.QuerySysID)),
                    RoleID = new DBVarChar((string.IsNullOrWhiteSpace(this.QueryRoleID) ? null : this.QueryRoleID))
                };

                _entitySystemRoleList = new EntitySystemRole(this.ConnectionStringB2P, this.ProviderNameB2P)
                    .SelectSystemRoleList(para);

                if (_entitySystemRoleList != null)
                {
                    _entitySystemRoleList = base.GetEntitysByPage(_entitySystemRoleList, pageSize);
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