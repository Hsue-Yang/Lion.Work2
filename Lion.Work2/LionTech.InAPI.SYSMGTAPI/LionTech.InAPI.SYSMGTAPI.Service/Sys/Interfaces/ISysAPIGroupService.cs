using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysAPIGroupService
    {
        Task<IEnumerable<SystemAPIGroup>> GetSystemAPIGroupByIdList(string sysID, string cultureID);

        Task<(int rowCount, IEnumerable<SystemAPIGroup> SystemAPIGroupList)> GetSystemAPIGroupList(string sysID, string cultureID, int pageIndex, int pageSize);

        Task<SystemAPIGroupMain> GetSystemAPIGroupDetail(string sysID, string apiGroupID);

        Task EditSystemAPIGroupDetail(SystemAPIGroupMain systemAPIGroup);

        Task<EnumDeleteSystemAPIGroupResult> DeleteSystemAPIGroupDetail(string sysID, string apiGroupID);
    }
}
