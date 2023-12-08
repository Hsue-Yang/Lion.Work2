// 新增日期：2018-06-20
// 新增人員：方道筌
// 新增內容：
// ---------------------------------------------------

using LionTech.APIService.SystemSetting;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Sys;
using System;
using System.Linq;

namespace ERPAPI.Models.SystemSetting
{
    public class SystemFunElmRoleModel : SystemSettingModel
    {
        #region - Constructor -
        public SystemFunElmRoleModel()
        {
            _entity = new EntitySystemFunElmRole(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        #endregion

        #region - Private -
        private readonly EntitySystemFunElmRole _entity;
        #endregion

        #region - 編輯元素權限角色 - 

        /// <summary>
        /// 編輯元素權限角色
        /// </summary>
        /// <param name="funElmRoleItem"></param>
        /// <param name="updUserID"></param>
        /// <returns></returns>
        public bool EditSystemFunElmRole(SystemFunElmRoleItem funElmRoleItem, string updUserID)
        {
            try
            {
                EntitySystemFunElmRole.SystemFunElmRolePara para = new EntitySystemFunElmRole.SystemFunElmRolePara(EnumCultureID.zh_TW.ToString())
                {
                    SysID = new DBVarChar(string.IsNullOrWhiteSpace(funElmRoleItem.SysID) ? null : funElmRoleItem.SysID),
                    FunControllerID = new DBVarChar(string.IsNullOrWhiteSpace(funElmRoleItem.FunControllerID) ? null : funElmRoleItem.FunControllerID),
                    FunActionNM = new DBVarChar(string.IsNullOrWhiteSpace(funElmRoleItem.FunActionName) ? null : funElmRoleItem.FunActionName),
                    ElmID = new DBVarChar(string.IsNullOrWhiteSpace(funElmRoleItem.ElmID) ? null : funElmRoleItem.ElmID),
                    UpdUserID = new DBVarChar(string.IsNullOrWhiteSpace(updUserID) ? null : updUserID),
                    ElmRoleInfoList =
                        funElmRoleItem
                            .FunElmRoleDisplayStatusList
                            .Select(n => from s in n.RoleIDlList
                                         select new EntitySystemFunElmRole.ElmRoleInfo
                                         {
                                             DispalySts = new DBTinyInt((int)n.FunElmDisplayType),
                                             RoleID = new DBVarChar(s)
                                         })
                            .SelectMany(sm => sm)
                            .ToList()
                };
                
                return _entity.EditSystemFunElmRole(para) == EntitySystemFunElmRole.EnumEditSystemFunElmRoleResult.Success;
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