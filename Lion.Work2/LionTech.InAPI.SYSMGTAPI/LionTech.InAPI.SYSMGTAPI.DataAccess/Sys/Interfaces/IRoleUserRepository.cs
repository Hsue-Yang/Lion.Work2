using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface IRoleUserRepository
    {
        Task<IEnumerable<RoleUser>> GetRoleUsers(string sysID, string roleID, string cultureID);
    }
}
