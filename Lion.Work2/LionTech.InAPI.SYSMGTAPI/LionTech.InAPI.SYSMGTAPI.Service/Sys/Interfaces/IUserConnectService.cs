using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface IUserConnectService
    {
        Task<(int rowCount, IEnumerable<UserConnect> userConnectList)> GetUserConnectList(string connectDTBegin, string connectDTEnd, int pageIndex, int pageSize);
    }
}
