using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface IUserConnectRepository
    {
        Task<(int rowCount, IEnumerable<UserConnect>)> GetUserConnectList(string connectDTBegin, string connectDTEnd, int pageIndex, int pageSize);
    }
}
