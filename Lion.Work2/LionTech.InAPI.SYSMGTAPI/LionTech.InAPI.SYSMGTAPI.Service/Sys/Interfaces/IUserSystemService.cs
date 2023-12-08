using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface IUserSystemService
    {
        Task<(int rowCount, IEnumerable<UserSystem> UserSystemList)> GetUserStatusList(string userID, string userNM, int pageIndex, int pageSize);
        Task<UserSystem> GetUserRawData(string userID);
        Task<IEnumerable<UserSystemRole>> GetUserOutSourcingSystemRoles(string userID, string cultureID);
        Task EditUserOutSourcingSystemRoles(UserSystemRoleParas para);
    }
}