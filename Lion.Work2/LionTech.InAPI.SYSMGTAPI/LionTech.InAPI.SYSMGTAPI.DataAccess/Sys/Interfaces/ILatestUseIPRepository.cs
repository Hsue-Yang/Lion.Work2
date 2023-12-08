using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ILatestUseIPRepository
    {
        Task<(int rowCount, IEnumerable<LatestUseIPInfo>)> SelectLatestUseIPInfoList(string ipAddress, string codeNM, int pageIndex, int pageSize);
    }
}
