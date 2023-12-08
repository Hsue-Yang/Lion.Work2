using LionTech.AspNetCore.InApi.HostedService;
using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SysAPIGroupService : ISysAPIGroupService
    {
        private readonly ISysAPIGroupRepository _sysAPIGroupRepository;
        private readonly IEventDistributorQueues _eventDistributorQueues;

        public SysAPIGroupService(ISysAPIGroupRepository sysAPIGroupRepository, IEventDistributorQueues eventDistributorQueues)
        {
            _sysAPIGroupRepository = sysAPIGroupRepository;
            _eventDistributorQueues = eventDistributorQueues;
        }

        public Task<IEnumerable<SystemAPIGroup>> GetSystemAPIGroupByIdList(string sysID, string cultureID)
        {
            return _sysAPIGroupRepository.GetSystemAPIGroupByIdList(sysID, cultureID);
        }

        public Task<(int rowCount, IEnumerable<SystemAPIGroup> SystemAPIGroupList)> GetSystemAPIGroupList(string sysID, string cultureID, int pageIndex, int pageSize)
        {
            return _sysAPIGroupRepository.GetSystemAPIGroupList(sysID, cultureID, pageIndex, pageSize);
        }

        public Task<SystemAPIGroupMain> GetSystemAPIGroupDetail(string sysID, string apiGroupID)
        {
            return _sysAPIGroupRepository.GetSystemAPIGroupDetail(sysID, apiGroupID);
        }

        public async Task EditSystemAPIGroupDetail(SystemAPIGroupMain systemAPIGroup)
        {
            await _sysAPIGroupRepository.EditSystemAPIGroupDetail(systemAPIGroup);
            var eventParaSystemAPIGroupEdit = new
            {
                systemAPIGroup.SysID,
                APIControllerID = systemAPIGroup.APIGroupID,
                APIGroupzhTW = systemAPIGroup.APIGroupZHTW,
                APIGroupzhCN = systemAPIGroup.APIGroupZHCN,
                APIGroupenUS = systemAPIGroup.APIGroupENUS,
                APIGroupthTH = systemAPIGroup.APIGroupTHTH,
                APIGroupjaJP = systemAPIGroup.APIGroupJAJP,
                APIGroupkoKR = systemAPIGroup.APIGroupKOKR,
                systemAPIGroup.SortOrder,
            };

            await _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemAPIGroup, EnumEDIServiceEventID.Edit, eventParaSystemAPIGroupEdit, systemAPIGroup.SysID);
        }

        public async Task<EnumDeleteSystemAPIGroupResult> DeleteSystemAPIGroupDetail(string sysID, string apiGroupID)
        {
            var deleteSystemAPIGroupDetailResult = await _sysAPIGroupRepository.DeleteSystemAPIGroupDetail(sysID, apiGroupID);
            var eventParaSystemAPIGroupDelete = new { SysID = sysID, APIControllerID = apiGroupID };

            if (deleteSystemAPIGroupDetailResult == EnumDeleteSystemAPIGroupResult.Success)
            {
                await _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemAPIGroup, EnumEDIServiceEventID.Delete, eventParaSystemAPIGroupDelete, sysID);
            }

            return deleteSystemAPIGroupDetailResult;
        }
    }
}