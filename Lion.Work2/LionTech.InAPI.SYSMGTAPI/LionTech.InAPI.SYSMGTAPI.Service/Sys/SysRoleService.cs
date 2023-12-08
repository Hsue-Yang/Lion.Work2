using LionTech.AspNetCore.InApi.HostedService;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Service.Utility.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysRoleService : ISysRoleService
    {
        private readonly ISysRoleRepository _sysRoleRepository;
        private readonly IEventDistributorQueues _eventDistributorQueues;
        private readonly ISystemLogger _systemLogger;

        public SysRoleService(ISysRoleRepository sysRoleRepository, ISystemLogger systemLogger, IEventDistributorQueues eventDistributorQueues)
        {
            _sysRoleRepository = sysRoleRepository;
            _eventDistributorQueues = eventDistributorQueues;
            _systemLogger = systemLogger;
        }

        public Task<IEnumerable<SystemRole>> GetSystemRoleByIdList(string sysID, string roleCategoryID, string cultureID)
        {
            return _sysRoleRepository.GetSystemRoleByIdList(sysID, roleCategoryID, cultureID);
        }

        public Task<(int rowCount, IEnumerable<SystemRole> SystemRoleList)> GetSystemRoleList(string sysID, string roleID, string roleCategoryID, string cultureID, int pageIndex, int pageSize)
        {
            return _sysRoleRepository.GetSystemRoleList(sysID, roleID, roleCategoryID, cultureID, pageIndex, pageSize);
        }

        public Task<SystemRoleMain> GetSystemRole(string sysID, string roleID)
        {
            return _sysRoleRepository.GetSystemRole(sysID, roleID);
        }

        public async Task EditSystemRoleByCategory(SystemRoleMain systemRole)
        {
            await _sysRoleRepository.EditSystemRoleByCategory(systemRole);

            var sysRole = await _sysRoleRepository.GetSystemRole(systemRole.SysID, systemRole.RoleID);
            var eventPara = new
            {
                sysRole.SysID,
                sysRole.RoleCategoryID,
                sysRole.RoleID,
                sysRole.RoleNMZHTW,
                sysRole.RoleNMZHCN,
                sysRole.RoleNMENUS,
                sysRole.RoleNMTHTH,
                sysRole.RoleNMJAJP,
                sysRole.RoleNMKOKR,
                sysRole.IsMaster
            };
            await _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemRole, EnumEDIServiceEventID.Edit, eventPara, systemRole.SysID);
        }

        public async Task EditSystemRoleDetail(SystemRoleMain systemRole)
        {
            await _sysRoleRepository.EditSystemRoleDetail(systemRole);

            var eventPara = new
            {
                systemRole.SysID,
                systemRole.RoleCategoryID,
                systemRole.RoleID,
                systemRole.RoleNMZHTW,
                systemRole.RoleNMZHCN,
                systemRole.RoleNMENUS,
                systemRole.RoleNMTHTH,
                systemRole.RoleNMJAJP,
                systemRole.RoleNMKOKR,
                systemRole.IsMaster
            };

            var eventDistributorTask = _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemRole, EnumEDIServiceEventID.Edit, eventPara, systemRole.SysID);
            var logTask =  _systemLogger.RecordLogAsync(EnumMongoDocName.LOG_SYS_SYSTEM_ROLE, systemRole, model => new { model.SysID, model.RoleID }, EnumSystemLogModify.U);
            await Task.WhenAll(eventDistributorTask, logTask);
        }

        public async Task DeleteSystemRoleDetail(string sysID, string roleID)
        {
            var systemRole = await _sysRoleRepository.GetSystemRole(sysID, roleID);
            
            await _sysRoleRepository.DeleteSystemRoleDetail(sysID, roleID);
            
            var eventPara = new { SysID = sysID, RoleID = roleID };
            var eventDistributorTask = _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemRole, EnumEDIServiceEventID.Delete, eventPara, sysID);
            var logTask = _systemLogger.RecordLogAsync(EnumMongoDocName.LOG_SYS_SYSTEM_ROLE, systemRole, model => new { model.SysID, model.RoleID }, EnumSystemLogModify.D);
            await Task.WhenAll(eventDistributorTask, logTask);
        }
    }
}