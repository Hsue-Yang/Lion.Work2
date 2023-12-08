using System;
using System.Collections.Generic;
using LionTech.Entity;
using LionTech.Entity.ERP;
using LionTech.Entity.ERP.Dev;
using LionTech.Entity.ERP.Sys;

namespace ERPAP.Models.Dev
{
    public class DevModel : _BaseAPModel
    {
        List<EntitySys.SysUserSystemSysID> _entitySysUserSystemSysIDList = new List<EntitySys.SysUserSystemSysID>();
        public List<EntitySys.SysUserSystemSysID> EntitySysUserSystemSysIDList { get { return _entitySysUserSystemSysIDList; } }

        public bool GetSysUserSystemSysIDList(string userID, bool excludeOutsourcing, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SysUserSystemSysIDPara para = new EntitySys.SysUserSystemSysIDPara(cultureID.ToString())
                {
                    UserID = new DBVarChar((string.IsNullOrWhiteSpace(userID) ? null : userID))
                };

                _entitySysUserSystemSysIDList = new EntitySys(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectSysUserSystemSysIDList(para, excludeOutsourcing);

                if (_entitySysUserSystemSysIDList != null)
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

        List<EntitySys.SystemSysID> _entitySysSystemSysIDList = new List<EntitySys.SystemSysID>();
        public List<EntitySys.SystemSysID> EntitySysSystemSysIDList { get { return _entitySysSystemSysIDList; } }

        public bool GetSysSystemSysIDList(bool excludeOutsourcing, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SystemSysIDPara para = new EntitySys.SystemSysIDPara(cultureID.ToString())
                {
                };

                _entitySysSystemSysIDList = new EntitySys(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectSystemSysIDList(para, excludeOutsourcing);

                if (_entitySysSystemSysIDList != null)
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

        List<EntitySys.SysSystemFunControllerID> _entitySysSystemFunControllerIDList = new List<EntitySys.SysSystemFunControllerID>();
        public List<EntitySys.SysSystemFunControllerID> EntitySysSystemFunControllerIDList { get { return _entitySysSystemFunControllerIDList; } }

        public bool GetSysSystemFunControllerIDList(string sysID, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SysSystemFunControllerIDPara para = new EntitySys.SysSystemFunControllerIDPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID))
                };

                _entitySysSystemFunControllerIDList = new EntitySys(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectSysSystemFunControllerIDList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntitySys.SysSystemFunMenu> _entitySysSystemFunMenuList = new List<EntitySys.SysSystemFunMenu>();
        public List<EntitySys.SysSystemFunMenu> EntitySysSystemFunMenuList { get { return _entitySysSystemFunMenuList; } }

        public bool GetSysSystemFunMenuList(string sysID, EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SysSystemFunMenuPara para = new EntitySys.SysSystemFunMenuPara(cultureID.ToString())
                {
                    SysID = new DBVarChar((string.IsNullOrWhiteSpace(sysID) ? null : sysID))
                };

                _entitySysSystemFunMenuList = new EntitySys(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectSysSystemFunMenuList(para);

                return true;
            }
            catch (Exception ex)
            {
                base.OnException(ex);
            }
            return false;
        }

        List<EntitySys.SysSystemFunType> _entitySysSystemFunTypeList = new List<EntitySys.SysSystemFunType>();
        public List<EntitySys.SysSystemFunType> EntitySysSystemFunTypeList { get { return _entitySysSystemFunTypeList; } }

        public bool GetSysSystemFunTypeList(EnumCultureID cultureID)
        {
            try
            {
                EntitySys.SysSystemFunTypePara para = new EntitySys.SysSystemFunTypePara(cultureID.ToString());

                _entitySysSystemFunTypeList = new EntitySys(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectSysSystemFunTypeList(para);

                if (_entitySysSystemFunTypeList != null)
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

        List<EntityDev.DevPhase> _entityDevPhaseList = new List<EntityDev.DevPhase>();
        public List<EntityDev.DevPhase> EntityDevPhaseList { get { return _entityDevPhaseList; } }

        public bool GetDevPhaseList(EnumCultureID cultureID)
        {
            try
            {
                EntityDev.DevPhasePara para =
                    new EntityDev.DevPhasePara(cultureID.ToString());

                _entityDevPhaseList = new EntityDev(this.ConnectionStringSERP, this.ProviderNameSERP)
                    .SelectDevPhaseList(para);

                if (_entityDevPhaseList != null)
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