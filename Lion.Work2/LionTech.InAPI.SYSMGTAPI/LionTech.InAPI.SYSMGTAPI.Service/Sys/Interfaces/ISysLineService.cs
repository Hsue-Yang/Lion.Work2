using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysLineService
    {
        Task<bool> CheckSystemLineBotIdIsExists(string sysID, string lineID);
        Task<IEnumerable<SystemLine>> GetSystemLineBotIdList(string sysID, string cultureID);
        Task<(int rowCount, IEnumerable<SystemLine> systemLineAccountList)> GetSystemLineBotAccountList(string sysID, string lineID, string cultureID, int pageIndex, int pageSize);
        Task<SystemLine> GetSystemLineBotAccountDetail(string sysID, string lineID);
        Task EditSystemLineBotAccountDetail(SystemLine systemLine);
        Task DeleteSystemLineById(string sysID, string lineID);
    }
}
