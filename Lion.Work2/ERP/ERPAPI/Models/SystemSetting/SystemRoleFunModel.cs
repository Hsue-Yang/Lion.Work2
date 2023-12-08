using LionTech.APIService.SystemSetting;
using LionTech.Entity;
using LionTech.Entity.ERP.SystemSetting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERPAPI.Models.SystemSetting
{
    public class SystemRoleFunModel : SystemSettingModel
    {
        #region - Constructor -
        public SystemRoleFunModel()
        {
            _entity = new EntitySystemRoleFun(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - API Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public string RoleID { get; set; }
        #endregion

        #region - Private -
        private readonly EntitySystemRoleFun _entity;
        #endregion

        public List<SystemFun> GetSystemRoleFun()
        {
            EntitySystemRoleFun.SystemRoleFunPara para = new EntitySystemRoleFun.SystemRoleFunPara
            {
                SysID = new DBVarChar(ClientSysID),
                RoleID = new DBVarChar(RoleID)
            };
            return (from s in _entity.SelectSystemRoleFunList(para)
                    select new SystemFun
                    {
                        ControllerID = s.ControllerID.GetValue(),
                        ActionName = s.ActionName.GetValue(),
                        ActionNMzhTW = s.ActionNMzhTW.GetValue(),
                        ActionNMzhCN = s.ActionNMzhCN.GetValue(),
                        ActionNMenUS = s.ActionNMenUS.GetValue(),
                        ActionNMthTH = s.ActionNMthTH.GetValue(),
                        ActionNMjaJP = s.ActionNMjaJP.GetValue(),
                        ActionNMkoKR = s.ActionNMkoKR.GetValue()
                    }).ToList();
        }

        public bool EditSystemRoleFun(List<SystemRoleFun> systemRoleFuns, string updUserID)
        {
            try
            {
                EntitySystemRoleFun.SystemRoleFunPara para = new EntitySystemRoleFun.SystemRoleFunPara
                {
                    SysID = new DBVarChar(ClientSysID),
                    UpdUserID = new DBVarChar(updUserID),
                    RoleIDList = systemRoleFuns.Select(s => new DBVarChar(s.RoleID)).ToList(),
                    SystemRoleFunList =
                        systemRoleFuns
                            .SelectMany((s, sm) =>
                                (from systemFun in s.SystemFunList
                                 select new EntitySystemRoleFun.SystemRoleFun
                                 {
                                     RoleID = s.RoleID,
                                     FunControllerID = systemFun.ControllerID,
                                     FunActionNM = systemFun.ActionName
                                 })).ToList()
                };

                return _entity.EditSystemRoleFun(para) == EntitySystemRoleFun.EnumEditSystemRoleFunResult.Success;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
    }
}