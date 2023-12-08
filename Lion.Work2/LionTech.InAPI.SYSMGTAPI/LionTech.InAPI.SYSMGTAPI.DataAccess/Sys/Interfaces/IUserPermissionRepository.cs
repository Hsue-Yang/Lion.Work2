using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface IUserPermissionRepository
    {
        Task<(int rowCount, IEnumerable<UserPermission>)> GetUserPermissionList(string userID, string userNM, string restrictType, string cultureID, int pageIndex, int pageSize);

        Task<UserPermissionDetail> GetUserPermissionDetail(string userID);
    }
}
