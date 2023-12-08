using LionTech.APIService.SystemSetting;
using LionTech.Entity;
using LionTech.Entity.ERP.SystemSetting;
using System;
using System.Linq;

namespace ERPAPI.Models.SystemSetting
{
    public class SystemRoleFunElmModel : SystemSettingModel
    {
        #region - Constructor -
        public SystemRoleFunElmModel()
        {
            _entity = new EntitySystemRoleFunElm(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        #endregion

        #region - Private -
        private readonly EntitySystemRoleFunElm _entity;

        internal bool EditSystemRoleFunElm(SystemRoleFunElm systemRoleFunElm, string updUserID)
        {
            try
            {
                var para = new EntitySystemRoleFunElm.EditSystemRoleElmListPara
                {
                    SysID = new DBVarChar(ClientSysID),
                    RoleID = new DBVarChar(systemRoleFunElm.RoleID),
                    UpdUserID = new DBVarChar(updUserID),
                    SystemRoleElmList = systemRoleFunElm.FunElmList.Select(n => new EntitySystemRoleFunElm.SystemRoleElm
                    {
                        FunControllerID = new DBVarChar(string.IsNullOrWhiteSpace(n.FunControllerID) ? null : n.FunControllerID),
                        FunActionName = new DBVarChar(string.IsNullOrWhiteSpace(n.FunActionName) ? null : n.FunActionName),
                        ElmID = new DBVarChar(string.IsNullOrWhiteSpace(n.ElmID) ? null : n.ElmID),
                        DisplaySts = new DBTinyInt((int)n.FunElmDisplayType)
                    }).ToList()
                };

                return _entity.EditSystemRoleFunList(para) == EntitySystemRoleFunElm.EnumEditSystemRoleElmListResult.Success;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion
    }
}