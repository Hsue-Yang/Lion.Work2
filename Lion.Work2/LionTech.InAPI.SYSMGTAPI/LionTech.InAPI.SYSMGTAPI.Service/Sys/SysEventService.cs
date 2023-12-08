using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysEventService : ISysEventService
    {
        private readonly ISysEventRepository _sysEventRepository;

        public SysEventService(ISysEventRepository sysEventRepository)
        {
            _sysEventRepository = sysEventRepository;
        }

        public Task<SystemEvent> GetSystemEventFullName(string sysID, string eventGroupID, string eventID, string cultureID)
        {
            return _sysEventRepository.GetSystemEventFullName(sysID, eventGroupID, eventID, cultureID);
        }

        public Task<IEnumerable<SystemEvent>> GetSystemEventByIdList(string sysID, string eventGroupID, string cultureID)
        {
            return _sysEventRepository.GetSystemEventByIdList(sysID, eventGroupID, cultureID);
        }

        public Task<(int rowCount, IEnumerable<SystemEvent> SystemEventList)> GetSystemEventList(string sysID, string eventGroupID, string cultureID, int pageIndex, int pageSize)
        {
            return _sysEventRepository.GetSystemEventList(sysID, eventGroupID, cultureID, pageIndex, pageSize);
        }

        public Task<SystemEventMain> GetSystemEventDetail(string sysID, string eventGroupID, string eventID)
        {
            return _sysEventRepository.GetSystemEventDetail(sysID, eventGroupID, eventID);
        }

        public Task EditSystemEventDetail(SystemEventMain systemEvent)
        {
            return _sysEventRepository.EditSystemEventDetail(systemEvent);
        }

        public Task<EnumDeleteResult> DeleteSystemEventDetail(string sysID, string eventGroupID, string eventID)
        {
            return _sysEventRepository.DeleteSystemEventDetail(sysID, eventGroupID, eventID);
        }
    }
}
