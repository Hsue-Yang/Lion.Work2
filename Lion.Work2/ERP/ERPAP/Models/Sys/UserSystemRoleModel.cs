using System;
using System.Collections.Generic;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;

namespace ERPAP.Models.Sys
{
    public class UserSystemRoleModel : SysModel
    {
        public string UserID { get; set; }

        public string UserNM { get; set; }

        public string RoleGroupID { get; set; }

        public UserSystemRoleModel()
        {
        }

        public void FormReset()
        {
            
        }

        EntityUserSystemRole.UserMain _entityUserMainInfo;
        public EntityUserSystemRole.UserMain EntityUserMainInfo { get { return _entityUserMainInfo; } }

        public bool GetUserMainInfo()
        {
            try
            {
                EntityUserSystemRole.UserMainPara para = new EntityUserSystemRole.UserMainPara()
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                _entityUserMainInfo = new EntityUserSystemRole(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectUserMainInfo(para);

                if (_entityUserMainInfo != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntityUserSystemRole.UserSystemRole> _entityUserSystemRoleList;
        public List<EntityUserSystemRole.UserSystemRole> EntityUserSystemRoleList { get { return _entityUserSystemRoleList; } }

        public bool GetUserSystemRoleList(EnumCultureID cultureID)
        {
            try
            {
                EntityUserSystemRole.UserSystemRolePara para = new EntityUserSystemRole.UserSystemRolePara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(this.UserID) ? null : this.UserID))
                };

                _entityUserSystemRoleList = new EntityUserSystemRole(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectUserSystemRoleList(para);

                if (_entityUserSystemRoleList != null)
                {
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