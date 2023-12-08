using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysPurviewService
    {
        Task<IEnumerable<SystemPurview>> GetSystemPurviewByIdList(string sysID, string cultureID);

        Task<(int rowCount, IEnumerable<SystemPurview> SystemPurviewList)> GetSystemPurviewList(string sysID, string cultureID, int pageIndex, int pageSize);

        Task<SystemPurviewMain> GetSystemPurviewDetail(string sysID, string purviewID);

        Task EditSystemPurviewDetail(SystemPurviewMain systemPurview);

        Task<EnumDeleteSystemPurviewResult> DeleteSystemPurviewDetail(string sysID, string purviewID);
    }
}