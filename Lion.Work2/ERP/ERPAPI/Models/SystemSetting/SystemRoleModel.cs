using LionTech.APIService.SystemSetting;
using LionTech.Entity;
using LionTech.Entity.ERP.SystemSetting;
using System.Collections.Generic;
using System.Linq;

namespace ERPAPI.Models.SystemSetting
{
    public class SystemRoleModel : SystemSettingModel
    {
        #region - Constructor -
        public SystemRoleModel()
        {
            _entity = new EntitySystemRole(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public string RoleCategoryID { get; set; }
        public string RoleID { get; set; }
        #endregion

        #region - Private -
        private readonly EntitySystemRole _entity;
        #endregion

        internal List<SystemRole> GetSystemRoleList()
        {
            EntitySystemRole.SystemRolePara para = new EntitySystemRole.SystemRolePara
            {
                SysID = new DBVarChar(ClientSysID),
                RoleCategoryID = new DBVarChar(string.IsNullOrWhiteSpace(RoleCategoryID) ? null : RoleCategoryID)
            };

            return (from s in _entity.SelectSystemRoleList(para)
                    select new SystemRole
                    {
                        RoleID = s.RoleID.GetValue(),
                        RoleNMzhTW = s.RoleNMzhTW.GetValue(),
                        RoleNMenUS = s.RoleNMenUS.GetValue(),
                        RoleNMjaJP = s.RoleNMjaJP.GetValue(),
                        RoleNMthTH = s.RoleNMthTH.GetValue(),
                        RoleNMzhCN = s.RoleNMzhCN.GetValue(),
                        RoleNMkoKR = s.RoleNMkoKR.GetValue(),
                        RoleCategoryID = s.RoleCategoryID.GetValue()
                    }).ToList();
        }

        internal bool EditSystemRole(SystemRole systemRole, string updUserID)
        {
            EntitySystemRole.SystemRolePara para = new EntitySystemRole.SystemRolePara
            {
                SysID = new DBVarChar(ClientSysID),
                UpdUserID = new DBVarChar(updUserID),
                RoleID = new DBVarChar(systemRole.RoleID),
                RoleNMzhTW = new DBNVarChar(systemRole.RoleNMzhTW),
                RoleNMenUS = new DBNVarChar(systemRole.RoleNMenUS),
                RoleNMjaJP = new DBNVarChar(systemRole.RoleNMjaJP),
                RoleNMthTH = new DBNVarChar(systemRole.RoleNMthTH),
                RoleNMzhCN = new DBNVarChar(systemRole.RoleNMzhCN),
                RoleNMkoKR = new DBNVarChar(systemRole.RoleNMkoKR),
                RoleCategoryID = new DBVarChar(systemRole.RoleCategoryID)
            };

            return _entity.EditSystemRoleDetail(para) == EntitySystemRole.EnumEditSystemRoleDetailResult.Success;
        }

        internal bool DeleteSystemRole(SystemRole systemRole)
        {
            EntitySystemRole.SystemRolePara para = new EntitySystemRole.SystemRolePara
            {
                SysID = new DBVarChar(ClientSysID),
                RoleID = new DBVarChar(systemRole.RoleID)
            };

            return _entity.DeleteSystemRoleDetail(para) == EntitySystemRole.EnumEditSystemRoleDetailResult.Success;
        }
    }
}