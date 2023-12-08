using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysEventGroupRepository
    {
        Task<IEnumerable<SystemEventGroup>> GetSystemEventGroupByIdList(string sysID, string cultureID);

        Task<(int rowCount, IEnumerable<SystemEventGroup> SystemEventGroupList)> GetSystemEventGroupList(string sysID, string eventGroupID, string cultureID, int pageIndex, int pageSize);

        Task<SystemEventGroupMain> GetSystemEventGroupDetail(string sysID, string eventGroupID);

        Task EditSystemEventGroupDetail(SystemEventGroupMain systemEventGroup);

        Task<EnumDeleteResult> DeleteSystemEventGroupDetail(string sysID, string eventGroupID);
    }
}
