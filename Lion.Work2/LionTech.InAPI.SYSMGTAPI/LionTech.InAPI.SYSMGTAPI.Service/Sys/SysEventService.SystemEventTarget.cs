using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysEventService
    {
        public Task<IEnumerable<SystemEventTarget>> GetSystemEventTargetList(string sysID, string userID, string eventGroupID, string eventID, string cultureID)
        {
            return _sysEventRepository.GetSystemEventTargetList(sysID, userID, eventGroupID, eventID, cultureID);
        }

        public Task EditSystemEventTarget(SystemEventTarget systemEventTarget)
        {
            return _sysEventRepository.EditSystemEventTarget(systemEventTarget);
        }

        public Task DeleteSystemEventTarget(string sysID, string eventGroupID, string eventID, string targetSysID)
        {
            return _sysEventRepository.DeleteSystemEventTarget(sysID, eventGroupID, eventID, targetSysID);
        }

        public Task<IEnumerable<SystemEventTarget>> GetSystemEventTargetIDs(string sysID, string eventGroupID, string eventID)
        { 
            return _sysEventRepository.GetSystemEventTargetIDs(sysID, eventGroupID, eventID);
        }
    }
}
