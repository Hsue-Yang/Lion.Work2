using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysEventService
    {
        Task<SystemEvent> GetSystemEventFullName(string sysID, string eventGroupID, string eventFunID, string cultureID);

        Task<IEnumerable<SystemEvent>> GetSystemEventByIdList(string sysID, string eventGroupID, string cultureID);

        Task<(int rowCount, IEnumerable<SystemEvent> SystemEventList)> GetSystemEventList(string sysID, string eventGroupID, string cultureID, int pageIndex, int pageSize);

        Task<SystemEventMain> GetSystemEventDetail(string sysID, string eventGroupID, string eventID);

        Task EditSystemEventDetail(SystemEventMain systemEvent);

        Task<EnumDeleteResult> DeleteSystemEventDetail(string sysID, string eventGroupID, string eventID);

        Task<IEnumerable<SystemEventTarget>> GetSystemEventTargetList(string sysID, string userID, string eventGroupID, string eventID, string cultureID);

        Task EditSystemEventTarget(SystemEventTarget systemEventTarget);

        Task DeleteSystemEventTarget(string sysID, string eventGroupID, string eventID, string targetSysID);

        Task<(int rowCount, IEnumerable<SystemEDIEvent> SystemEventEDIList)> GetSystemEventEDIList(string sysID, string userID, string targetSysID, string eventGroupID, string eventID, string dtBegin, string dtEnd, string isOnlyFail, string cultureID, int pageIndex, int pageSize);

        Task<(int rowCount, IEnumerable<SystemEDIEvent> SystemEventTargetEDIList)> GetSystemEventTargetEDIList(string sysID, string userID, string targetSysID, string eventGroupID, string eventID, string dtBegin, string dtEnd, string isOnlyFail, string cultureID, int pageIndex, int pageSize);

        Task<string> ExcuteSubscription(SystemEDIEvent systemEDIEvent);

        Task<IEnumerable<SystemEventTarget>> GetSystemEventTargetIDs(string sysID, string eventGroupID, string eventID);
    }
}
