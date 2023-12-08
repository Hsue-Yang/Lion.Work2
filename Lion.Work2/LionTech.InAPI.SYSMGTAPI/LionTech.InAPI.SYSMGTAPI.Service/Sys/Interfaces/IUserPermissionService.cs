using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface IUserPermissionService
    {
        Task<(int rowCount, IEnumerable<UserPermission> userPermissionsList)> GetUserPermissionList(string userID, string userNM, string restrictType, string cultureID, int pageIndex, int pageSize);
        Task<UserPermissionDetail> GetUserPermissionDetail(string userID);
    }
}
