using LionTech.AspNetCore.InApi.HostedService;
using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysRoleCategoryService : ISysRoleCategoryService
    {
        private readonly ISysRoleCategoryRepository _sysRoleCategoryRepository;
        private readonly IEventDistributorQueues _eventDistributorQueues;

        public SysRoleCategoryService(ISysRoleCategoryRepository sysRoleCategoryRepository, IEventDistributorQueues eventDistributorQueues)
        {
            _sysRoleCategoryRepository = sysRoleCategoryRepository;
            _eventDistributorQueues = eventDistributorQueues;
        }

        public Task<IEnumerable<SystemRoleCategory>> GetSystemRoleCategoryByIdList(string sysID, string cultureID)
        {
            return _sysRoleCategoryRepository.GetSystemRoleCategoryByIdList(sysID, cultureID);
        }

        public Task<IEnumerable<SystemRoleCategory>> GetSystemRoleCategoryList(string sysID, string roleCategoryNM, string cultureID)
        {
            return _sysRoleCategoryRepository.GetSystemRoleCategoryList(sysID, roleCategoryNM, cultureID);
        }

        public Task<SystemRoleCategoryMain> GetSystemRoleCategoryDetail(string sysID, string roleCategoryID)
        {
            return _sysRoleCategoryRepository.GetSystemRoleCategoryDetail(sysID, roleCategoryID);
        }

        public async Task EditSystemRoleCategoryDetail(SystemRoleCategoryMain systemRoleCategory)
        {
            await _sysRoleCategoryRepository.EditSystemRoleCategoryDetail(systemRoleCategory);
            var eventPara = new
            {
                systemRoleCategory.SysID,
                systemRoleCategory.RoleCategoryID,
                systemRoleCategory.RoleCategoryNMZHTW,
                systemRoleCategory.RoleCategoryNMZHCN,
                systemRoleCategory.RoleCategoryNMENUS,
                systemRoleCategory.RoleCategoryNMTHTH,
                systemRoleCategory.RoleCategoryNMJAJP,
                systemRoleCategory.RoleCategoryNMKOKR,
                systemRoleCategory.SortOrder
            };

            await _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemRoleCateg, EnumEDIServiceEventID.Edit, eventPara, systemRoleCategory.SysID);
        }

        public async Task<EnumDeleteSystemRoleCategoryDetailResult> DeleteSystemRoleCategoryDetail(string sysID, string roleCategoryID)
        {
            var deleteSystemRoleCategoryDetailResult = await _sysRoleCategoryRepository.DeleteSystemRoleCategoryDetail(sysID, roleCategoryID);

            if (deleteSystemRoleCategoryDetailResult == EnumDeleteSystemRoleCategoryDetailResult.Success)
            {
                var eventPara = new { SysID = sysID, RoleCategoryID = roleCategoryID };
                await _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemRoleCateg, EnumEDIServiceEventID.Delete, eventPara, sysID);
            }

            return deleteSystemRoleCategoryDetailResult;
        }
    }
}