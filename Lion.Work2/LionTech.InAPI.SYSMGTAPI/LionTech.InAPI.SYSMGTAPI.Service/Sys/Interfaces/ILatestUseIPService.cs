using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ILatestUseIPService
    {
        Task<(int rowCount, IEnumerable<LatestUseIPInfo> LatestUseIPInfoLists)> GetLatestUseIPInfoList(string ipAddress, string codeNM, int pageIndex, int pageSize);
    }
}
