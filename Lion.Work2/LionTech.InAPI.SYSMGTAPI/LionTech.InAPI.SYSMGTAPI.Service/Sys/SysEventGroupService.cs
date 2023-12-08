using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SysEventGroupService : ISysEventGroupService
    {
        private readonly ISysEventGroupRepository _sysEventGroupRepository;

        public SysEventGroupService(ISysEventGroupRepository sysEventGroupRepository)
        {
            _sysEventGroupRepository = sysEventGroupRepository;
        }

        public Task<IEnumerable<SystemEventGroup>> GetSystemEventGroupByIdList(string sysID, string cultureID)
        {
            return _sysEventGroupRepository.GetSystemEventGroupByIdList(sysID, cultureID);
        }

        public Task<(int rowCount, IEnumerable<SystemEventGroup> SystemEventGroupList)> GetSystemEventGroupList(string sysID, string eventGroupID, string cultureID, int pageIndex, int pageSize)
        {
            return _sysEventGroupRepository.GetSystemEventGroupList(sysID, eventGroupID, cultureID, pageIndex, pageSize);
        }

        public Task<SystemEventGroupMain> GetSystemEventGroupDetail(string sysID, string eventGroupID)
        {
            return _sysEventGroupRepository.GetSystemEventGroupDetail(sysID, eventGroupID);
        }

        public Task EditSystemEventGroupDetail(SystemEventGroupMain systemEventGroup)
        {
            return _sysEventGroupRepository.EditSystemEventGroupDetail(systemEventGroup);
        }

        public Task<EnumDeleteResult> DeleteSystemEventGroupDetail(string sysID, string eventGroupID)
        {
            return _sysEventGroupRepository.DeleteSystemEventGroupDetail(sysID, eventGroupID);
        }
    }
}
