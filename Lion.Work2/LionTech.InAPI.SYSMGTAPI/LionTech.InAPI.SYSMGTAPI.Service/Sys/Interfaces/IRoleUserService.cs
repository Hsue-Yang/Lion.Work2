using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface IRoleUserService
    {
        Task<IEnumerable<RoleUser>> GetRoleUsers(string sysID, string roleID, string cultureID);
    }
}
