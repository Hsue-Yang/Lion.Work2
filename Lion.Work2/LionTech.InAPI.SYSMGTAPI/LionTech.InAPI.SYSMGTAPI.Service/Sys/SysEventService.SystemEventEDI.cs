using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysEventService
    {
        public Task<(int rowCount, IEnumerable<SystemEDIEvent> SystemEventEDIList)> GetSystemEventEDIList(string sysID, string userID, string targetSysID, string eventGroupID, string eventID, string dtBegin, string dtEnd, string isOnlyFail, string cultureID, int pageIndex, int pageSize)
        {
            return _sysEventRepository.GetSystemEventEDIList(sysID, userID, targetSysID, eventGroupID, eventID, dtBegin, dtEnd, isOnlyFail, cultureID, pageIndex, pageSize);
        }

        public Task<(int rowCount, IEnumerable<SystemEDIEvent> SystemEventTargetEDIList)> GetSystemEventTargetEDIList(string sysID, string userID, string targetSysID, string eventGroupID, string eventID, string dtBegin, string dtEnd, string isOnlyFail, string cultureID, int pageIndex, int pageSize)
        {
            return _sysEventRepository.GetSystemEventTargetEDIList(sysID, userID, targetSysID, eventGroupID, eventID, dtBegin, dtEnd, isOnlyFail, cultureID, pageIndex, pageSize);
        }

        public Task<string> ExcuteSubscription(SystemEDIEvent systemEDIEvent)
        {
            return _sysEventRepository.ExcuteSubscription(systemEDIEvent);
        }
    }
}